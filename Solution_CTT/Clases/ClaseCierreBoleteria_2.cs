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
    public class ClaseCierreBoleteria_2
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
        DataTable dtAyuda;

        //public bool llenarReporte(string sFecha_P, int iJornada_P, string sJornada, string sUsuario, int iIdOficinista_P, int iBanderaEnviaImprime, int iIdCierreCaja_P)
        public bool llenarReporte(int iBanderaEnviaImprime, int iIdCierreCaja_P)
        {
            try
            {
                DSReportes ds = new DSReportes();

                sSql = "";
                sSql += "select id_ctt_programacion, hora_salida, fecha_grid, disco," + Environment.NewLine;
                sSql += "asientos_ocupados cuenta, substring(tipo_viaje, 1, 3) tipo_viaje" + Environment.NewLine;
                sSql += "from ctt_vw_cierre_caja_2" + Environment.NewLine;
                sSql += "where estado_salida = 'Cerrada'" + Environment.NewLine;
                sSql += "and id_ctt_cierre_caja = " + iIdCierreCaja_P + Environment.NewLine;
                sSql += "order by fecha_viaje, hora_salida";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    DataColumn valor = new DataColumn("valor");
                    valor.DataType = System.Type.GetType("System.Decimal");
                    dtConsulta.Columns.Add(valor);

                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        sSql = "";
                        sSql += "select ltrim(str(isnull(sum(DP.cantidad * (DP.precio_unitario - DP.valor_dscto + DP.valor_iva)), 0), 10, 2)) valor" + Environment.NewLine;
                        sSql += "from cv403_cab_pedidos CP INNER JOIN" + Environment.NewLine;
                        sSql += "cv403_det_pedidos DP ON CP.id_pedido = DP.id_pedido" + Environment.NewLine;
                        sSql += "and CP.estado = 'A'" + Environment.NewLine;
                        sSql += "and DP.estado = 'A'" + Environment.NewLine;
                        sSql += "where CP.cobro_boletos = 1" + Environment.NewLine;
                        sSql += "and cobro_retencion = 0" + Environment.NewLine;
                        sSql += "and cobro_administrativo = 0" + Environment.NewLine;
                        sSql += "and CP.id_ctt_programacion = " + Convert.ToInt32(dtConsulta.Rows[i]["id_ctt_programacion"].ToString());

                        dtAyuda = new DataTable();
                        dtAyuda.Clear();

                        bRespuesta = conexionM.consultarRegistro(sSql, dtAyuda);

                        if (bRespuesta)
                        {
                            dtConsulta.Rows[i]["valor"] = Convert.ToDecimal(dtAyuda.Rows[0][0].ToString());
                        }

                        else
                        {
                            return false;
                        }
                    }
                }

                ds.Tables["dtFrecuenciasCierre"].Clear();
                dtConsulta.Columns.Remove("id_ctt_programacion");
                DataTable dt = dtConsulta;

                sSql = "";
                sSql += "select '1. RETENCION:' descripcion," + Environment.NewLine;
                sSql += "ltrim(str(isnull(sum(isnull(valor, 0)), 0), 10, 2)) valor" + Environment.NewLine;
                sSql += "from ctt_vw_cierre_boleteria" + Environment.NewLine;
                sSql += "where cobro_boletos = 0" + Environment.NewLine;
                sSql += "and cobro_retencion = 1" + Environment.NewLine;
                sSql += "and cobro_administrativo = 0" + Environment.NewLine;
                sSql += "and id_ctt_cierre_caja = " + iIdCierreCaja_P + Environment.NewLine;
                sSql += "union" + Environment.NewLine;
                sSql += "select '2. ADMINISTRACIÓN:' descripcion," + Environment.NewLine;
                sSql += "ltrim(str(isnull(sum(isnull(valor, 0)), 0), 10, 2)) valor" + Environment.NewLine;
                sSql += "from ctt_vw_cierre_boleteria" + Environment.NewLine;
                sSql += "where cobro_boletos = 0" + Environment.NewLine;
                sSql += "and cobro_retencion = 0" + Environment.NewLine;
                sSql += "and cobro_administrativo = 1" + Environment.NewLine;
                sSql += "and pago_cumplido = 1" + Environment.NewLine;
                sSql += "and id_ctt_cierre_caja = " + iIdCierreCaja_P;

                DataTable dtPagos = ds.Tables["dtPagosCierre"];
                dtPagos.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtPagos);

                if (bRespuesta == false)
                {
                    return false;
                }

                //PAGOS CUMPLIDOS
                sSql = "";
                sSql += "select fecha_viaje, hora_salida, disco + ' - ' + placa vehiculo, valor" + Environment.NewLine;
                sSql += "from ctt_vw_pagos_pendientes_cumplidos" + Environment.NewLine;
                sSql += "where id_ctt_cierre_caja = " + iIdCierreCaja_P;

                DataTable dtCumplido = ds.Tables["dtPagosCumplidos"];
                dtCumplido.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtCumplido);

                if (bRespuesta == false)
                {
                    return false;
                }

                //PAGOS ATRASADOS
                sSql = "";
                sSql += "select fecha_viaje, hora_salida, disco + ' - ' + placa vehiculo, valor" + Environment.NewLine;
                sSql += "from ctt_vw_pagos_pendientes_atrasados" + Environment.NewLine;
                sSql += "where id_ctt_cierre_caja = " + iIdCierreCaja_P;

                DataTable dtPendiente = ds.Tables["dtPagosAtrasadosPagados"];
                dtPendiente.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtPendiente);

                if (bRespuesta == false)
                {
                    return false;
                }

                sSql = "";
                sSql += "select fecha_viaje, hora_salida, sum(cantidad) cantidad," + Environment.NewLine;
                sSql += "ltrim(str(sum(cantidad * (precio_unitario - valor_dscto + valor_iva)), 10, 2)) valor" + Environment.NewLine;
                sSql += "from ctt_vw_viajes_activos" + Environment.NewLine;
                sSql += "where id_ctt_cierre_caja = " + iIdCierreCaja_P + Environment.NewLine;
                sSql += "group by fecha_viaje, hora_salida" + Environment.NewLine;
                sSql += "order by fecha_viaje, hora_salida";

                DataTable dtActivos = ds.Tables["dtViajesActivosCaja"];
                dtActivos.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtActivos);

                if (bRespuesta == false)
                {
                    return false;
                }

                sSql = "";
                sSql += "select CC.fecha_apertura, J.descripcion jornada, O.descripcion oficinista" + Environment.NewLine;
                sSql += "from ctt_cierre_caja CC INNER JOIN" + Environment.NewLine;
                sSql += "ctt_jornada J ON J.id_ctt_jornada = CC.id_ctt_jornada" + Environment.NewLine;
                sSql += "and J.estado = 'A'" + Environment.NewLine;
                sSql += "and CC.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_oficinista O ON O.id_ctt_oficinista = CC.id_ctt_oficinista" + Environment.NewLine;
                sSql += "and O.estado = 'A'" + Environment.NewLine;
                sSql += "where CC.id_ctt_cierre_caja = " + iIdCierreCaja_P;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    return false;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    return false;
                }

                string sFecha_P = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_apertura"].ToString()).ToString("dd-MMM-yyyy");
                string sJornada_P = dtConsulta.Rows[0]["jornada"].ToString().Trim().ToUpper();
                string sOficinista_P = dtConsulta.Rows[0]["oficinista"].ToString().Trim().ToUpper();

                //CREANDO EL REPORTE
                LocalReport reporteLocal = new LocalReport();
                reporteLocal.ReportPath = HttpContext.Current.Server.MapPath("~/Reportes/rptCierreBoleteria_2.rdlc");
                //reporteLocal.ReportPath = HttpContext.Current.Server.MapPath("~/Reportes/rptCierrePrueba.rdlc");
                ReportParameter[] parametros = new ReportParameter[3];
                parametros[0] = new ReportParameter("P_Fecha", sFecha_P);
                parametros[1] = new ReportParameter("P_Jornada", sJornada_P);
                parametros[2] = new ReportParameter("P_Usuario", sOficinista_P);
                ReportDataSource datasource_1 = new ReportDataSource("dsFrecuencias", dt);
                ReportDataSource datasource_2 = new ReportDataSource("dsPagos", dtPagos);
                ReportDataSource datasource_3 = new ReportDataSource("dsPagosAtrasados", dtPendiente);
                ReportDataSource datasource_4 = new ReportDataSource("dsViajesActivos", dtActivos);
                ReportDataSource datasource_5 = new ReportDataSource("dsCumplidos", dtCumplido);

                reporteLocal.DataSources.Add(datasource_1);
                reporteLocal.DataSources.Add(datasource_2);
                reporteLocal.DataSources.Add(datasource_3);
                reporteLocal.DataSources.Add(datasource_4);
                reporteLocal.DataSources.Add(datasource_5);

                reporteLocal.SetParameters(parametros);

                if (iBanderaEnviaImprime != 2)
                {
                    Clases.Impresor imp = new Clases.Impresor();
                    imp.Imprime(reporteLocal);
                }

                if ((iBanderaEnviaImprime == 1) || (iBanderaEnviaImprime == 2))
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
                        SCuerpoMensaje += "USUARIO: " + sOficinista_P + "</br>";
                        SCuerpoMensaje += "JORNADA: " + sJornada_P + "</br>";
                        SCuerpoMensaje += "FECHA:" + sFecha_P + "</br></br>";
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