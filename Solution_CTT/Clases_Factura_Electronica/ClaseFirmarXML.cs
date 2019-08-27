using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;

using NEGOCIO;

namespace Solution_CTT.Clases_Factura_Electronica
{
    class ClaseFirmarXML
    {
        manejadorConexion conexion = new manejadorConexion();

        string sAyuda;
        string sSql;
        string sTipoComprobanteVenta;

        DataTable dtConsulta;
        bool bRespuesta;

        public string GSub_ActualizaPantalla(string P_St_CodDoc, long P_Ln_Orden)
        {
            //      P_Ln_Orden
            //      1 Comprobantes generados
            //      2 Firmados
            //      3 Autorizados
            //      4 No autorizados


            //FACTURA
            if (P_St_CodDoc == "01")
            {
                sTipoComprobanteVenta = "Fac";

                sAyuda = "";
                sAyuda = sAyuda + "select" + Environment.NewLine;
                sAyuda = sAyuda + "NF.Numero_Factura," + Environment.NewLine;

                if (conexion.bddConexion() == "MYSQL")
                {
                    sAyuda = sAyuda + "ltrim(concat(P.apellidos,' '," + conexion.esNulo() + "(P.nombres,''))) Cliente," + Environment.NewLine;
                }
                else
                {
                    sAyuda = sAyuda + "ltrim(P.apellidos + ' ' + " + conexion.esNulo() + "(P.nombres,'')) Cliente," + Environment.NewLine;
                }

                sAyuda = sAyuda + "SubString(LOCALIDAD.valor_texto, 1, 25) Localidad,F.fecha_factura," + Environment.NewLine;
                sAyuda = sAyuda + "F.id_factura," + Environment.NewLine;
                sAyuda = sAyuda + "F.clave_acceso," + Environment.NewLine;
                sAyuda = sAyuda + "L.establecimiento estab,isnull(L.punto_emision,'009') ptoEmi," + Environment.NewLine;
                sAyuda = sAyuda + conexion.esNulo() + "(autorizacion,'') autorizacion," + Environment.NewLine;
                sAyuda = sAyuda + conexion.esNulo() + "(CONVERT (nvarchar(19), fecha_autorizacion, 120),'') fecha_autorizacion," + Environment.NewLine;
                sAyuda = sAyuda + "id_tipo_emision,id_tipo_ambiente" + Environment.NewLine;
                sAyuda = sAyuda + "from" + Environment.NewLine;
                sAyuda = sAyuda + "cv403_facturas F," + Environment.NewLine;
                sAyuda = sAyuda + "cv403_numeros_facturas NF," + Environment.NewLine;
                sAyuda = sAyuda + "tp_personas P," + Environment.NewLine;
                sAyuda = sAyuda + "tp_localidades L," + Environment.NewLine;
                sAyuda = sAyuda + "tp_codigos LOCALIDAD," + Environment.NewLine;
                sAyuda = sAyuda + "vta_tipocomprobante TC" + Environment.NewLine;
                sAyuda = sAyuda + "where" + Environment.NewLine;
                sAyuda = sAyuda + "F.idempresa = " + HttpContext.Current.Application["idEmpresa"].ToString() + Environment.NewLine;
                sAyuda = sAyuda + "and F.estado = 'A'" + Environment.NewLine;
                sAyuda = sAyuda + "and F.estado in ('A','E')" + Environment.NewLine;
                sAyuda = sAyuda + "and NF.estado = 'A'" + Environment.NewLine;
                sAyuda = sAyuda + "and NF.id_factura = F.id_factura" + Environment.NewLine;
                sAyuda = sAyuda + "and F.id_persona = P.id_persona" + Environment.NewLine;
                sAyuda = sAyuda + "and F.id_localidad = L.id_localidad" + Environment.NewLine;

                //  Generadas
                if (P_Ln_Orden == 1)
                {
                    sAyuda = sAyuda + "and F.clave_acceso is not null" + Environment.NewLine;
                }

                //  Firmadas
                else if (P_Ln_Orden == 2)
                {
                    sAyuda = sAyuda + "and F.clave_acceso is not null" + Environment.NewLine;
                }

                //  Autorizadas
                else if (P_Ln_Orden == 3)
                {
                    sAyuda = sAyuda + "and F.autorizacion is not null" + Environment.NewLine;
                }

                sAyuda = sAyuda + "and L.cg_localidad = LOCALIDAD.correlativo" + Environment.NewLine;
                sAyuda = sAyuda + "and TC.idtipocomprobante=F.idtipocomprobante" + Environment.NewLine;
                sAyuda = sAyuda + "and TC.codigo='" + sTipoComprobanteVenta + "'" + Environment.NewLine;
                sAyuda = sAyuda + "order by F.id_factura desc";
            }


            //RETENCION
            if (P_St_CodDoc == "07")
            {
                sAyuda = "";
                sAyuda = sAyuda + "SELECT DISTINCT" + Environment.NewLine;

                if (conexion.bddConexion() == "MYSQL")
                {
                    sAyuda = sAyuda + "convert(CABCR.NUMERO_PREIMPRESO,decimal) numero_secuencial," + Environment.NewLine;
                    sAyuda = sAyuda + "concat(PER.apellidos , ' ' , " + conexion.esNulo() + "(PER.nombres,'')) Razon_Social," + Environment.NewLine;
                }

                else
                {
                    sAyuda = sAyuda + "convert(numeric,CABCR.NUMERO_PREIMPRESO) numero_secuencial," + Environment.NewLine;
                    sAyuda = sAyuda + "PER.apellidos + ' ' + " + conexion.esNulo() + "(PER.nombres,'') Razon_Social," + Environment.NewLine;
                }

                sAyuda = sAyuda + "CABM.numero_movimiento,CABM.FECHA_MOVIMIENTO," + Environment.NewLine;
                sAyuda = sAyuda + "CABCR.ID_CAB_COMPROBANTE_RETENCION,CABCR.clave_acceso," + Environment.NewLine;
                sAyuda = sAyuda + "EstabRetencion1, ptoEmiRetencion1," + Environment.NewLine;
                sAyuda = sAyuda + conexion.esNulo() + "(autorizacion,'') autorizacion," + Environment.NewLine;
                sAyuda = sAyuda + conexion.esNulo() + "(CONVERT (nvarchar(19), fecha_autorizacion, 120),'') fecha_autorizacion," + Environment.NewLine;
                sAyuda = sAyuda + "id_tipo_emision,id_tipo_ambiente" + Environment.NewLine;
                sAyuda = sAyuda + "from" + Environment.NewLine;
                sAyuda = sAyuda + "cv405_comprobantes_retencion CR," + Environment.NewLine;
                sAyuda = sAyuda + "cv405_c_movimientos CABM," + Environment.NewLine;
                sAyuda = sAyuda + "cv404_auxiliares_contables AUX," + Environment.NewLine;
                sAyuda = sAyuda + "tp_personas PER," + Environment.NewLine;
                sAyuda = sAyuda + "cv405_cab_comprobantes_retencion CABCR" + Environment.NewLine;
                sAyuda = sAyuda + "where" + Environment.NewLine;
                sAyuda = sAyuda + "CABM.id_c_movimiento = CR.id_c_movimiento" + Environment.NewLine;
                sAyuda = sAyuda + "and AUX.id_auxiliar = CABM.id_beneficiario" + Environment.NewLine;
                sAyuda = sAyuda + "and PER.id_persona = CABM.id_persona" + Environment.NewLine;
                sAyuda = sAyuda + "and CR.ID_CAB_COMPROBANTE_RETENCION = CABCR.ID_CAB_COMPROBANTE_RETENCION" + Environment.NewLine;

                //  Generadas
                if (P_Ln_Orden == 1)
                {
                    sAyuda = sAyuda + "and CABCR.clave_acceso is not null" + Environment.NewLine;
                }

                //  Firmadas
                else if (P_Ln_Orden == 2)
                {
                    sAyuda = sAyuda + "and CABCR.clave_acceso is not null" + Environment.NewLine;
                }

                //  Autorizadas
                else if (P_Ln_Orden == 3)
                {
                    sAyuda = sAyuda + "and CABCR.autorizacion is not null" + Environment.NewLine;
                }

                sAyuda = sAyuda + "AND CABM.ESTADO = 'A'" + Environment.NewLine;
                sAyuda = sAyuda + "AND CR.ESTADO = 'A'" + Environment.NewLine;
                sAyuda = sAyuda + "AND CABCR.ESTADO = 'A'" + Environment.NewLine;

                if (conexion.bddConexion() == "MYSQL")
                {
                    sAyuda = sAyuda + "Order by convert(CABCR.NUMERO_PREIMPRESO, decimal) desc, CABM.FECHA_MOVIMIENTO desc";
                }
                else
                {
                    sAyuda = sAyuda + "Order by convert(numeric,CABCR.NUMERO_PREIMPRESO) desc, CABM.FECHA_MOVIMIENTO desc";
                }
            }

            //NOTA DE CREDITO
            if (P_St_CodDoc == "04")
            {
                sAyuda = "";
                sAyuda = sAyuda + "select" + Environment.NewLine;
                sAyuda = sAyuda + "NNC.Numero_Nota," + Environment.NewLine;

                if (conexion.bddConexion() == "MYSQL")
                {
                    sAyuda = sAyuda + "concat(P.apellidos,' '," + conexion.esNulo() + "(P.nombres,'')) Cliente," + Environment.NewLine;
                }

                else
                {
                    sAyuda = sAyuda + "P.apellidos + ' ' + " + conexion.esNulo() + "(P.nombres,'') Cliente," + Environment.NewLine;
                }

                sAyuda = sAyuda + "SubString(LOCALIDAD.valor_texto, 1, 25) Localidad," + Environment.NewLine;
                sAyuda = sAyuda + "N.fecha_vcto," + Environment.NewLine;
                sAyuda = sAyuda + "N.Id_Nota_Credito,N.clave_acceso," + Environment.NewLine;
                sAyuda = sAyuda + "L.establecimiento estab,isnull(L.punto_emision,'009') ptoEmi," + Environment.NewLine;
                sAyuda = sAyuda + conexion.esNulo() + "(autorizacion,'') autorizacion," + Environment.NewLine;
                sAyuda = sAyuda + conexion.esNulo() + "(CONVERT (nvarchar(19), fecha_autorizacion, 120),'') fecha_autorizacion," + Environment.NewLine;
                sAyuda = sAyuda + "id_tipo_emision,id_tipo_ambiente" + Environment.NewLine;
                sAyuda = sAyuda + "from" + Environment.NewLine;
                sAyuda = sAyuda + "cv403_notas_credito N, tp_localidades L," + Environment.NewLine;
                sAyuda = sAyuda + "tp_codigos LOCALIDAD," + Environment.NewLine;
                sAyuda = sAyuda + "tp_personas P," + Environment.NewLine;
                sAyuda = sAyuda + "cv403_numeros_notas_creditos NNC" + Environment.NewLine;
                sAyuda = sAyuda + "where" + Environment.NewLine;
                sAyuda = sAyuda + "N.estado = 'A'" + Environment.NewLine;
                sAyuda = sAyuda + "and N.id_persona = P.id_persona" + Environment.NewLine;
                sAyuda = sAyuda + "and NNC.Id_Nota_Credito = N.Id_Nota_Credito" + Environment.NewLine;

                //If G_Ln_Id_Servidor > 1 Then
                //   T_St_Sql = T_St_Sql & "and l.id_servidor = " & G_Ln_Id_Servidor & " "
                //End If

                sAyuda = sAyuda + "and N.id_localidad = L.id_localidad" + Environment.NewLine;
                sAyuda = sAyuda + "and L.cg_localidad = LOCALIDAD.correlativo" + Environment.NewLine;

                //  Generadas
                if (P_Ln_Orden == 1)
                {
                    sAyuda = sAyuda + "and N.clave_acceso is not null" + Environment.NewLine;
                }

                //  Firmadas
                else if (P_Ln_Orden == 2)
                {
                    sAyuda = sAyuda + "and N.clave_acceso is not null" + Environment.NewLine;
                }

                //  Autorizadas
                else if (P_Ln_Orden == 3)
                {
                    sAyuda = sAyuda + "and N.autorizacion is not null" + Environment.NewLine;
                }

                sAyuda = sAyuda + "and NNC.estado = 'A'" + Environment.NewLine;
                sAyuda = sAyuda + "Order by  N.Id_nota_credito desc";
            }

            //GUIA DE REMISION
            if (P_St_CodDoc == "06")
            {
                sAyuda = "";
                sAyuda = sAyuda + "select" + Environment.NewLine;
                sAyuda = sAyuda + "NGR.Numero_Guia_Remision," + Environment.NewLine;

                if (conexion.bddConexion() == "MYSQL")
                {
                    sAyuda = sAyuda + "concat(P.apellidos,' '," + conexion.esNulo() + "(P.nombres,'')) Cliente," + Environment.NewLine;
                }

                else
                {
                    sAyuda = sAyuda + "P.apellidos + ' ' + " + conexion.esNulo() + "(P.nombres,'') Cliente," + Environment.NewLine;
                }

                sAyuda = sAyuda + "SubString(LOCALIDAD.valor_texto, 1, 25) Localidad," + Environment.NewLine;
                sAyuda = sAyuda + "G.fecha_emision," + Environment.NewLine;
                sAyuda = sAyuda + "G.Id_Guia_Remision,G.clave_acceso," + Environment.NewLine;
                sAyuda = sAyuda + "L.establecimiento estab,";
                sAyuda = sAyuda + conexion.esNulo() + "(L.punto_emision,'009') ptoEmi," + Environment.NewLine;
                sAyuda = sAyuda + conexion.esNulo() + "(G.autorizacion,'') autorizacion," + Environment.NewLine;
                sAyuda = sAyuda + conexion.esNulo() + "(CONVERT (nvarchar(19), G.fecha_autorizacion, 120),'') fecha_autorizacion," + Environment.NewLine;
                sAyuda = sAyuda + "G.id_tipo_emision,G.id_tipo_ambiente" + Environment.NewLine;
                sAyuda = sAyuda + "from" + Environment.NewLine;
                sAyuda = sAyuda + "cv403_guias_remision G, tp_localidades L," + Environment.NewLine;
                sAyuda = sAyuda + "tp_codigos LOCALIDAD," + Environment.NewLine;
                sAyuda = sAyuda + "tp_personas P," + Environment.NewLine;
                sAyuda = sAyuda + "cv403_numeros_guias_remision NGR" + Environment.NewLine;
                sAyuda = sAyuda + "where" + Environment.NewLine;
                sAyuda = sAyuda + "G.estado = 'A'" + Environment.NewLine;
                sAyuda = sAyuda + "and G.id_destinatario = P.id_persona" + Environment.NewLine;
                sAyuda = sAyuda + "and NGR.Id_Guia_Remision = G.Id_Guia_Remision" + Environment.NewLine;

                //If G_Ln_Id_Servidor > 1 Then
                //    T_St_Sql = T_St_Sql & "and L.id_servidor = " & G_Ln_Id_Servidor & " "
                //End If

                sAyuda = sAyuda + "and G.id_localidad = L.id_localidad" + Environment.NewLine;
                sAyuda = sAyuda + "and L.cg_localidad = LOCALIDAD.correlativo" + Environment.NewLine;

                //  Generadas
                if (P_Ln_Orden == 1)
                {
                    sAyuda = sAyuda + "and G.clave_acceso is not null" + Environment.NewLine;
                }

                //  Firmadas
                else if (P_Ln_Orden == 2)
                {
                    sAyuda = sAyuda + "and G.clave_acceso is not null" + Environment.NewLine;
                }

                //  Autorizadas
                else if (P_Ln_Orden == 3)
                {
                    sAyuda = sAyuda + "and G.autorizacion is not null" + Environment.NewLine;
                }

                sAyuda = sAyuda + "and NGR.estado = 'A'" + Environment.NewLine;
                sAyuda = sAyuda + "Order by  G.Id_Guia_Remision desc";
            }

            return sAyuda;
        }

        //GFun_St_Ruta_Archivo
        public string GFun_St_Ruta_Archivo(string P_St_Codigo_Documento, long P_Ln_Orden)
        {
            try
            {
                sSql = "";
                sSql = sSql + "select D.nombres" + Environment.NewLine;
                sSql = sSql + "from  cel_directorio D, cel_tipo_comprobante C" + Environment.NewLine;
                sSql = sSql + "where D.id_tipo_comprobante = C.id_tipo_comprobante and" + Environment.NewLine;
                sSql = sSql + "C.codigo = '" + P_St_Codigo_Documento + "' and" + Environment.NewLine;
                sSql = sSql + "D.orden = " + P_Ln_Orden + Environment.NewLine;
                sSql = sSql + "and D.estado='A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sSql = conexion.valorDefecto(dtConsulta.Rows[0].ItemArray[0].ToString(), "");
                    }
                }

                else
                {
                    //catchMensaje.LblMensaje.Text = sSql;
                    //catchMensaje.ShowInTaskbar = false;
                    //catchMensaje.ShowDialog();
                }

                return sSql;
            }

            catch (Exception ex)
            {
                //catchMensaje.LblMensaje.Text = ex.ToString();
                //catchMensaje.ShowInTaskbar = false;
                //catchMensaje.ShowDialog();
                return "";
            }
        }

        //Sub Gsub_trae_parametros_certificado
        //public void Gsub_trae_parametros_certificado(string P_St_certificado_ruta, string P_St_certificado_palabra_clave)
        public void Gsub_trae_parametros_certificado(string[] P_St_certificado_digital)
        {
            try
            {
                //P_St_certificado_ruta = "";
                //P_St_certificado_palabra_clave = "";

                sSql = "";
                sSql = sSql + "select certificado_ruta,certificado_palabra_clave" + Environment.NewLine;
                sSql = sSql + "from cel_parametro" + Environment.NewLine;
                sSql = sSql + "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        P_St_certificado_digital[0] = conexion.valorDefecto(dtConsulta.Rows[0].ItemArray[0].ToString(), "");
                        P_St_certificado_digital[1] = conexion.valorDefecto(dtConsulta.Rows[0].ItemArray[1].ToString(), "");
                    }
                }

                else
                {
                    //catchMensaje.LblMensaje.Text = "Error: Recuperando los datos de la tabla contingencia.";
                    //catchMensaje.ShowInTaskbar = false;
                    //catchMensaje.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                //catchMensaje.LblMensaje.Text = ex.ToString();
                //catchMensaje.ShowInTaskbar = false;
                //catchMensaje.ShowDialog();
            }
        }

        //GSub_FirmarXML
        public string GSub_FirmarXML(string P_St_jar, string P_St_certificado, string P_St_passCertificado,
                                   string P_St_xmlIn, string P_St_xmlPathOut, string P_St_fileOut,
                                   string P_St_codigoError, string P_St_DescripcionError)
        {
            try
            {
                string T_St_Linea_comando_Shell = "";
                int iLongi;
                string sNombreArchivo;

                P_St_codigoError = "00";
                P_St_DescripcionError = "OK";

                //T_St_Linea_comando_Shell = @"C:\batchfirma.bat ";
                //T_St_Linea_comando_Shell = P_St_jar + " ";
                T_St_Linea_comando_Shell = T_St_Linea_comando_Shell + P_St_certificado + " ";
                T_St_Linea_comando_Shell = T_St_Linea_comando_Shell + P_St_passCertificado + " " + P_St_xmlIn + " " + P_St_xmlPathOut + " " + P_St_fileOut;

                //string exe = Application.ExecutablePath;//COMENTADO JONA
                string exe = "";
                exe = P_St_jar;

                Process proceso = new Process();
                proceso.StartInfo.FileName = exe;
                proceso.StartInfo.Arguments = T_St_Linea_comando_Shell;
                proceso.Start();

                //ENVIAR A CONVERTIR EN BASE 64
                iLongi = (P_St_xmlPathOut + P_St_fileOut).Length;
                sNombreArchivo = (P_St_xmlPathOut + P_St_fileOut).Substring(0, iLongi - 4) + ".txt";

                //if (GFun_Lo_Base64(P_St_xmlPathOut + P_St_fileOut, sNombreArchivo) == false)
                //{
                //    P_St_codigoError = "01";
                //}

                return P_St_codigoError;
            }

            catch (Exception ex)
            {
                //catchMensaje.LblMensaje.Text = ex.ToString();
                //catchMensaje.ShowInTaskbar = false;
                //catchMensaje.ShowDialog();
                return "01";
            }
        }

        //PROCESO PARA CONVERTIR EN BASE 64 EL ARCHIVO FIRMADO
        public bool GFun_Lo_Base64(string P_St_Ruta_Archivo, string P_St_Ruta_Destino)
        {
            try
            {
                byte[] arrayDeBytes = File.ReadAllBytes(P_St_Ruta_Archivo);
                string sCodificado = Convert.ToBase64String(arrayDeBytes);

                StreamWriter escribir = File.CreateText(P_St_Ruta_Destino);
                //File.AppendText(@"C:\FacturasFirmadas\prueba.txt");
                escribir.Write(sCodificado);
                escribir.Flush();
                escribir.Close();
                return true;
            }

            catch (Exception ex)
            {
                //catchMensaje.LblMensaje.Text = ex.ToString();
                //catchMensaje.ShowInTaskbar = false;
                //catchMensaje.ShowDialog();
                return false;
            }
        }

        //GFun_Lo_ExisteToken
        public bool GFun_Lo_ExisteToken(long P_Ln_Tipo_Certificado_Digital, string P_St_Token)
        {
            object T_Ob_Fso;
            object T_Ob_ColecObject;
            object T_Ob_I;


            if (P_Ln_Tipo_Certificado_Digital == 1)     //TIPO TOKEN
            {
                T_Ob_Fso = new object();
                //T_Ob_Fso = Server.CreateObject("Scripting.FileSystemObject");
                //T_Ob_ColecObject = T_Ob_Fso.Drives;



                T_Ob_Fso = null;
                T_Ob_ColecObject = null;
                T_Ob_I = null;
                return true;
            }

            else
            {
                return true;
            }
        }
    }
}
