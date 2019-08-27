using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NEGOCIO;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Net.Mail;

namespace Solution_CTT.Clases
{
    public class ClaseCierreBoleteria
    {
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sCorreoEnvia;
        string sClaveCorreoEnvia;
        string sCorreoSMTP;
        string sCorreoPuerto;
        string sSSL;
        string sDestinatarios;
        string SCuerpoMensaje;

        int iRespuestaParametros;

        bool bRespuesta;

        DataTable dtConsulta;

        public bool llenarReporte(string sFecha_P, int iJornada_P, string sJornada, string sUsuario, DataTable dtTasasPagadas_P, int iIdOficinista_P, int iBanderaEnviaImprime)
        {
            try
            {
                DSReportes ds = new DSReportes();

                sSql = "";
                sSql += "select fecha_viaje, hora_salida, disco, cuenta, valor," + Environment.NewLine;
                sSql += "substring(tipo_viaje, 1, 3) tipo_viaje" + Environment.NewLine;
                sSql += "from ctt_vw_cierre_boleteria" + Environment.NewLine;
                sSql += "where fecha_pedido = '" + sFecha_P  + "'" + Environment.NewLine;
                sSql += "and cobro_boletos = 1" + Environment.NewLine;
                sSql += "and cobro_retencion = 0" + Environment.NewLine;
                sSql += "and cobro_administrativo = 0" + Environment.NewLine;
                sSql += "and id_ctt_jornada = " + iJornada_P;

                DataTable dtFrecuencia = ds.Tables["dtFrecuenciasCierre"];
                dtFrecuencia.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtFrecuencia);

                if (bRespuesta == false)
                {
                    return false;
                }

                sSql = "";
                sSql += "select '1. RETENCION:' descripcion," + Environment.NewLine;
                sSql += "ltrim(str(sum(isnull(valor, 0)), 10, 2)) valor" + Environment.NewLine;
                sSql += "from ctt_vw_cierre_boleteria" + Environment.NewLine;
                sSql += "where fecha_pedido = '" + sFecha_P + "'" + Environment.NewLine;
                sSql += "and cobro_boletos = 0" + Environment.NewLine;
                sSql += "and cobro_retencion = 1" + Environment.NewLine;
                sSql += "and cobro_administrativo = 0" + Environment.NewLine;
                sSql += "and id_ctt_jornada = " + iJornada_P + Environment.NewLine;
                sSql += "union" + Environment.NewLine;
                sSql += "select '2. ADMINISTRACIÓN:' descripcion," + Environment.NewLine;
                sSql += "ltrim(str(sum(isnull(valor, 0)), 10, 2)) valor" + Environment.NewLine;
                sSql += "from ctt_vw_cierre_boleteria" + Environment.NewLine;
                sSql += "where fecha_pedido = '" + sFecha_P + "'" + Environment.NewLine;
                sSql += "and cobro_boletos = 0" + Environment.NewLine;
                sSql += "and cobro_retencion = 0" + Environment.NewLine;
                sSql += "and cobro_administrativo = 1" + Environment.NewLine;
                sSql += "and pago_cumplido = 1" + Environment.NewLine;
                sSql += "and id_ctt_jornada = " + iJornada_P;

                DataTable dtPagos = ds.Tables["dtPagosCierre"];
                dtPagos.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtPagos);

                if (bRespuesta == false)
                {
                    return false;
                }

                sSql = "";
                sSql += "select isnull(sum(cantidad), 0) cantidad_tasa," + Environment.NewLine;
                sSql += "isnull(sum(valor), 0) total_tasa" + Environment.NewLine;
                sSql += "from ctt_movimiento_caja" + Environment.NewLine;
                sSql += "where fecha = '" + sFecha_P + "'" + Environment.NewLine;
                sSql += "and cobro_tasa_usuario = 1" + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "and id_ctt_jornada = " + iJornada_P;

                DataTable dtTasas = ds.Tables["dtTasasCierre"];
                dtTasas.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtTasas);

                if (bRespuesta == false)
                {
                    return false;
                }

                DataTable dtTasasPagadas = ds.Tables["dtTasasPagadasCierre"];
                dtTasasPagadas.Clear();

                if (dtTasasPagadas_P.Rows.Count == 0)
                {
                    DataRow row = dtTasasPagadas_P.NewRow();
                    row["concepto"] = "NINGUNA";
                    row["valor"] = "0.00";
                    row["estado_movimiento"] = "NINGUNA";
                    dtTasasPagadas_P.Rows.Add(row);
                }

                dtTasasPagadas = dtTasasPagadas_P;

                sSql = "";
                sSql += "select fecha_viaje, hora_salida, disco + ' - ' + placa vehiculo, valor" + Environment.NewLine;
                sSql += "from ctt_vw_pagos_pendientes_atrasados" + Environment.NewLine;
                sSql += "where fecha_pago = '" + sFecha_P + "'" + Environment.NewLine;
                sSql += "and id_ctt_jornada = " + iJornada_P;

                DataTable dtPagosAtrasados = ds.Tables["dtPagosAtrasadosPagados"];
                dtPagosAtrasados.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtPagosAtrasados);

                if (bRespuesta == false)
                {
                    return false;
                }

                sSql = "";
                sSql += "select fecha_viaje, hora_salida, sum(cantidad) cantidad," + Environment.NewLine;
                sSql += "ltrim(str(sum(cantidad * (precio_unitario - valor_dscto + valor_iva)), 10, 2)) valor" + Environment.NewLine;
                sSql += "from ctt_vw_viajes_activos" + Environment.NewLine;
                sSql += "where fecha_viaje >= '" + sFecha_P + "'" + Environment.NewLine;
                sSql += "and id_ctt_oficinista = " + iIdOficinista_P + Environment.NewLine;
                sSql += "group by fecha_viaje, hora_salida" + Environment.NewLine;
                sSql += "order by fecha_viaje, hora_salida";

                DataTable dtViajesActivos = ds.Tables["dtViajesActivosCaja"];
                dtViajesActivos.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtViajesActivos);

                if (bRespuesta == false)
                {
                    return false;
                }

                //CREANDO EL REPORTE
                LocalReport reporteLocal = new LocalReport();
                reporteLocal.ReportPath = HttpContext.Current.Server.MapPath("~/Reportes/rptCierreBoleteria.rdlc");
                //reporteLocal.ReportPath = HttpContext.Current.Server.MapPath("~/Reportes/rptCierrePrueba.rdlc");
                ReportParameter[] parametros = new ReportParameter[3];
                parametros[0] = new ReportParameter("P_Fecha", Convert.ToDateTime(sFecha_P).ToString("dd-MMM-yyyy"));
                parametros[1] = new ReportParameter("P_Jornada", sJornada);
                parametros[2] = new ReportParameter("P_Usuario", sUsuario);
                ReportDataSource datasource_1 = new ReportDataSource("dsFrecuencias", dtFrecuencia);
                ReportDataSource datasource_2 = new ReportDataSource("dsPagos", dtPagos);
                ReportDataSource datasource_3 = new ReportDataSource("dsTasasCierre", dtTasas);
                ReportDataSource datasource_4 = new ReportDataSource("dsTasasPagadas", dtTasasPagadas);
                ReportDataSource datasource_5 = new ReportDataSource("dsPagosAtrasados", dtPagosAtrasados);
                ReportDataSource datasource_6 = new ReportDataSource("dsViajesActivos", dtViajesActivos);
                
                reporteLocal.DataSources.Add(datasource_1);
                reporteLocal.DataSources.Add(datasource_2);
                reporteLocal.DataSources.Add(datasource_3);
                reporteLocal.DataSources.Add(datasource_4);
                reporteLocal.DataSources.Add(datasource_5);
                reporteLocal.DataSources.Add(datasource_6);
                reporteLocal.SetParameters(parametros);

                Clases.Impresor imp = new Clases.Impresor();
                imp.Imprime(reporteLocal);

                if (iBanderaEnviaImprime == 1)
                {
                    iRespuestaParametros = cargarParametrosMail();

                    if (iRespuestaParametros == 1)
                    {
                        //Warning[] warnings;
                        //string[] streamids;
                        //string mimeType;
                        //string encoding;
                        //string extension;

                        //byte[] bytes = reporteLocal.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                        byte[] bytes = reporteLocal.Render("PDF");
                        //MemoryStream memoryStream = new MemoryStream(bytes);
                        //memoryStream.Seek(0, SeekOrigin.Begin);

                        SCuerpoMensaje = "";
                        SCuerpoMensaje += "CIERRE DE CAJA:</br></br>";
                        SCuerpoMensaje += "USUARIO: " + sUsuario + "</br>";
                        SCuerpoMensaje += "JORNADA: " + sJornada + "</br>";
                        SCuerpoMensaje += "FECHA  :" + sFecha_P + "</br></br>";
                        SCuerpoMensaje += "Saludos cordiales.";


                        MailMessage mail = new MailMessage();
                        mail.To.Add(sDestinatarios);
                        mail.From = new MailAddress(sCorreoEnvia);
                        mail.Subject = "ENVIO DE CIERRE DE CAJA - TTQ";
                        mail.Body = SCuerpoMensaje;
                        mail.Priority = MailPriority.High;
                        mail.IsBodyHtml = true;

                        mail.Attachments.Add(new Attachment(new MemoryStream(bytes), "" + "CIERRE_CAJA_" + sFecha_P + ".pdf"));

                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = sCorreoSMTP;
                        smtp.Credentials = new System.Net.NetworkCredential(sCorreoEnvia, sClaveCorreoEnvia);

                        if (sSSL == "1")
                        {
                            smtp.EnableSsl = true;
                        }
                        else
                        {
                            smtp.EnableSsl = false;
                        }

                        smtp.Send(mail);
                    }
                }                

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        #region ENVIO DE REPORTE

        //CONSULTAR PARAMETROS DE MAIL  
        private int cargarParametrosMail()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(correo_envia, '') correo_envia, isnull(clave_mail_envia, '') clave_mail_envia," + Environment.NewLine;
                sSql += "isnull(smtp_correo, '') smtp_correo, isnull(puerto_correo, 0) puerto_correo, isnull(maneja_ssl, 0) maneja_ssl," + Environment.NewLine;
                sSql += "isnull(correo_receptor_1, '') correo_receptor_1, isnull(correo_receptor_2, '') correo_receptor_2," + Environment.NewLine;
                sSql += "isnull(correo_receptor_3, '') correo_receptor_3" + Environment.NewLine;
                sSql += "from ctt_parametro" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        if (dtConsulta.Rows[0][0].ToString().Trim() == "")
                        {
                            return 0;
                        }

                        if (dtConsulta.Rows[0][1].ToString().Trim() == "")
                        {
                            return 0;
                        }

                        if (dtConsulta.Rows[0][2].ToString().Trim() == "")
                        {
                            return 0;
                        }

                        if (dtConsulta.Rows[0][3].ToString().Trim() == "")
                        {
                            return 0;
                        }

                        if (dtConsulta.Rows[0][4].ToString().Trim() == "")
                        {
                            return 0;
                        }

                        if (dtConsulta.Rows[0][5].ToString().Trim() == "")
                        {
                            return 0;
                        }

                        sCorreoEnvia = dtConsulta.Rows[0][0].ToString();
                        sClaveCorreoEnvia = dtConsulta.Rows[0][1].ToString();
                        sCorreoSMTP = dtConsulta.Rows[0][2].ToString();
                        sCorreoPuerto = dtConsulta.Rows[0][3].ToString();
                        sSSL = dtConsulta.Rows[0][4].ToString();
                        sDestinatarios = dtConsulta.Rows[0][5].ToString();

                        if (dtConsulta.Rows[0][6].ToString().Trim() != "")
                        {
                            sDestinatarios += ", " + dtConsulta.Rows[0][6].ToString().Trim();
                        }

                        if (dtConsulta.Rows[0][7].ToString().Trim() != "")
                        {
                            sDestinatarios += ", " + dtConsulta.Rows[0][7].ToString().Trim();
                        }

                        return 1;
                    }

                    else
                    {
                        return 0;
                    }
                }

                else
                {
                    return -1;
                }
            }

            catch (Exception ex)
            {
                return -1;
            }
        }


        #endregion
    }
}