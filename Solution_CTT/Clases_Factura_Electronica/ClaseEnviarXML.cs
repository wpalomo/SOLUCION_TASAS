using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
using System.Net;
using System.Web;
using NEGOCIO;

namespace Solution_CTT.Clases_Factura_Electronica
{
    class ClaseEnviarXML
    {
        private static string xmlEnvioRequestTemplate =
            String.Concat(
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>",
            " <SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\"",
            " xmlns:ns1=\"http://ec.gob.sri.ws.recepcion\">",
            "  <SOAP-ENV:Body>",
            "    <ns1:validarComprobante>",
            "      <xml>{0}</xml>",
            "    </ns1:validarComprobante>",
            "  </SOAP-ENV:Body>",
            "</SOAP-ENV:Envelope>");

        private static string xmlAutorizacionRequestTemplate =
      String.Concat(
      "<?xml version=\"1.0\" encoding=\"utf-8\"?>",
      " <SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\"",
      " xmlns:ns1=\"http://ec.gob.sri.ws.autorizacion\">",
      "  <SOAP-ENV:Body>",
      "    <ns1:autorizacionComprobante>",
      "      <claveAccesoComprobante>{0}</claveAccesoComprobante>",
      "    </ns1:autorizacionComprobante>",
      "  </SOAP-ENV:Body>",
      "</SOAP-ENV:Envelope>");

        manejadorConexion conexion = new manejadorConexion();

        string sAyuda;
        string sSql;
        string sTipoComprobanteVenta;

        DataTable dtConsulta;
        bool bRespuesta;

        //public string GSub_ActualizaPantalla(string P_St_CodDoc, long P_Ln_Orden, int IDEmpresa)//PASO ID_EMPRESA JONA
        //{
        //    //      P_Ln_Orden
        //    //      1 Comprobantes generados
        //    //      2 Firmados
        //    //      3 Autorizados
        //    //      4 No autorizados
                        
        //    //FACTURA
        //    if (P_St_CodDoc == "01")
        //    {
        //        sTipoComprobanteVenta = "Fac";

        //        sAyuda = "";
        //        sAyuda = sAyuda + "select" + Environment.NewLine;
        //        sAyuda = sAyuda + "NF.Numero_Factura," + Environment.NewLine;

        //        if (conexion.bddConexion() == "MYSQL")
        //        {
        //            sAyuda = sAyuda + "ltrim(concat(P.apellidos,' '," + conexion.esNulo() + "(P.nombres,''))) Cliente," + Environment.NewLine;
        //        }
        //        else
        //        {
        //            sAyuda = sAyuda + "ltrim(P.apellidos + ' ' + " + conexion.esNulo() + "(P.nombres,'')) Cliente," + Environment.NewLine;
        //        }

        //        sAyuda = sAyuda + "SubString(LOCALIDAD.valor_texto, 1, 25) Localidad,F.fecha_factura," + Environment.NewLine;
        //        sAyuda = sAyuda + "F.id_factura," + Environment.NewLine;
        //        sAyuda = sAyuda + "F.clave_acceso," + Environment.NewLine;
        //        sAyuda = sAyuda + "L.establecimiento estab,isnull(L.punto_emision,'009') ptoEmi," + Environment.NewLine;
        //        sAyuda = sAyuda + conexion.esNulo() + "(autorizacion,'') autorizacion," + Environment.NewLine;
        //        sAyuda = sAyuda + conexion.esNulo() + "(CONVERT (nvarchar(19), fecha_autorizacion, 120),'') fecha_autorizacion," + Environment.NewLine;
        //        sAyuda = sAyuda + "id_tipo_emision,id_tipo_ambiente" + Environment.NewLine;
        //        sAyuda = sAyuda + "from" + Environment.NewLine;
        //        sAyuda = sAyuda + "cv403_facturas F," + Environment.NewLine;
        //        sAyuda = sAyuda + "cv403_numeros_facturas NF," + Environment.NewLine;
        //        sAyuda = sAyuda + "tp_personas P," + Environment.NewLine;
        //        sAyuda = sAyuda + "tp_localidades L," + Environment.NewLine;
        //        sAyuda = sAyuda + "tp_codigos LOCALIDAD," + Environment.NewLine;
        //        sAyuda = sAyuda + "vta_tipocomprobante TC" + Environment.NewLine;
        //        sAyuda = sAyuda + "where" + Environment.NewLine;
        //        sAyuda = sAyuda + "F.idempresa = " + IDEmpresa + Environment.NewLine;
        //        sAyuda = sAyuda + "and F.estado = 'A'" + Environment.NewLine;
        //        sAyuda = sAyuda + "and F.estado in ('A','E')" + Environment.NewLine;
        //        sAyuda = sAyuda + "and NF.estado = 'A'" + Environment.NewLine;
        //        sAyuda = sAyuda + "and NF.id_factura = F.id_factura" + Environment.NewLine;
        //        sAyuda = sAyuda + "and F.id_persona = P.id_persona" + Environment.NewLine;
        //        sAyuda = sAyuda + "and F.id_localidad = L.id_localidad" + Environment.NewLine;

        //        //  Generadas
        //        if (P_Ln_Orden == 1)
        //        {
        //            sAyuda = sAyuda + "and F.clave_acceso is not null" + Environment.NewLine;
        //        }

        //        //  Firmadas
        //        else if (P_Ln_Orden == 2)
        //        {
        //            sAyuda = sAyuda + "and F.clave_acceso is not null" + Environment.NewLine;
        //        }

        //        //  Autorizadas
        //        else if (P_Ln_Orden == 3)
        //        {
        //            sAyuda = sAyuda + "and F.autorizacion is not null" + Environment.NewLine;
        //        }

        //        sAyuda = sAyuda + "and L.cg_localidad = LOCALIDAD.correlativo" + Environment.NewLine;
        //        sAyuda = sAyuda + "and TC.idtipocomprobante=F.idtipocomprobante" + Environment.NewLine;
        //        sAyuda = sAyuda + "and TC.codigo='" + sTipoComprobanteVenta + "'" + Environment.NewLine;
        //        sAyuda = sAyuda + "order by F.id_factura desc";
        //    }


        //    //RETENCION
        //    if (P_St_CodDoc == "07")
        //    {
        //        sAyuda = "";
        //        sAyuda = sAyuda + "SELECT DISTINCT" + Environment.NewLine;

        //        if (conexion.bddConexion() == "MYSQL")
        //        {
        //            sAyuda = sAyuda + "convert(CABCR.NUMERO_PREIMPRESO,decimal) numero_secuencial," + Environment.NewLine;
        //            sAyuda = sAyuda + "concat(PER.apellidos , ' ' , " + conexion.esNulo() + "(PER.nombres,'')) Razon_Social," + Environment.NewLine;
        //        }

        //        else
        //        {
        //            sAyuda = sAyuda + "convert(numeric,CABCR.NUMERO_PREIMPRESO) numero_secuencial," + Environment.NewLine;
        //            sAyuda = sAyuda + "PER.apellidos + ' ' + " + conexion.esNulo() + "(PER.nombres,'') Razon_Social," + Environment.NewLine;
        //        }

        //        sAyuda = sAyuda + "CABM.numero_movimiento,CABM.FECHA_MOVIMIENTO," + Environment.NewLine;
        //        sAyuda = sAyuda + "CABCR.ID_CAB_COMPROBANTE_RETENCION,CABCR.clave_acceso," + Environment.NewLine;
        //        sAyuda = sAyuda + "EstabRetencion1, ptoEmiRetencion1," + Environment.NewLine;
        //        sAyuda = sAyuda + conexion.esNulo() + "(autorizacion,'') autorizacion," + Environment.NewLine;
        //        sAyuda = sAyuda + conexion.esNulo() + "(CONVERT (nvarchar(19), fecha_autorizacion, 120),'') fecha_autorizacion," + Environment.NewLine;
        //        sAyuda = sAyuda + "id_tipo_emision,id_tipo_ambiente" + Environment.NewLine;
        //        sAyuda = sAyuda + "from" + Environment.NewLine;
        //        sAyuda = sAyuda + "cv405_comprobantes_retencion CR," + Environment.NewLine;
        //        sAyuda = sAyuda + "cv405_c_movimientos CABM," + Environment.NewLine;
        //        sAyuda = sAyuda + "cv404_auxiliares_contables AUX," + Environment.NewLine;
        //        sAyuda = sAyuda + "tp_personas PER," + Environment.NewLine;
        //        sAyuda = sAyuda + "cv405_cab_comprobantes_retencion CABCR" + Environment.NewLine;
        //        sAyuda = sAyuda + "where" + Environment.NewLine;
        //        sAyuda = sAyuda + "CABM.id_c_movimiento = CR.id_c_movimiento" + Environment.NewLine;
        //        sAyuda = sAyuda + "and AUX.id_auxiliar = CABM.id_beneficiario" + Environment.NewLine;
        //        sAyuda = sAyuda + "and PER.id_persona = CABM.id_persona" + Environment.NewLine;
        //        sAyuda = sAyuda + "and CR.ID_CAB_COMPROBANTE_RETENCION = CABCR.ID_CAB_COMPROBANTE_RETENCION" + Environment.NewLine;

        //        //  Generadas
        //        if (P_Ln_Orden == 1)
        //        {
        //            sAyuda = sAyuda + "and CABCR.clave_acceso is not null" + Environment.NewLine;
        //        }

        //        //  Firmadas
        //        else if (P_Ln_Orden == 2)
        //        {
        //            sAyuda = sAyuda + "and CABCR.clave_acceso is not null" + Environment.NewLine;
        //        }

        //        //  Autorizadas
        //        else if (P_Ln_Orden == 3)
        //        {
        //            sAyuda = sAyuda + "and CABCR.autorizacion is not null" + Environment.NewLine;
        //        }

        //        sAyuda = sAyuda + "AND CABM.ESTADO = 'A'" + Environment.NewLine;
        //        sAyuda = sAyuda + "AND CR.ESTADO = 'A'" + Environment.NewLine;
        //        sAyuda = sAyuda + "AND CABCR.ESTADO = 'A'" + Environment.NewLine;

        //        if (conexion.bddConexion() == "MYSQL")
        //        {
        //            sAyuda = sAyuda + "Order by convert(CABCR.NUMERO_PREIMPRESO, decimal) desc, CABM.FECHA_MOVIMIENTO desc";
        //        }
        //        else
        //        {
        //            sAyuda = sAyuda + "Order by convert(numeric,CABCR.NUMERO_PREIMPRESO) desc, CABM.FECHA_MOVIMIENTO desc";
        //        }
        //    }

        //    //NOTA DE CREDITO
        //    if (P_St_CodDoc == "04")
        //    {
        //        sAyuda = "";
        //        sAyuda = sAyuda + "select" + Environment.NewLine;
        //        sAyuda = sAyuda + "NNC.Numero_Nota," + Environment.NewLine;

        //        if (conexion.bddConexion() == "MYSQL")
        //        {
        //            sAyuda = sAyuda + "concat(P.apellidos,' '," + conexion.esNulo() + "(P.nombres,'')) Cliente," + Environment.NewLine;
        //        }

        //        else
        //        {
        //            sAyuda = sAyuda + "P.apellidos + ' ' + " + conexion.esNulo() + "(P.nombres,'') Cliente," + Environment.NewLine;
        //        }

        //        sAyuda = sAyuda + "SubString(LOCALIDAD.valor_texto, 1, 25) Localidad," + Environment.NewLine;
        //        sAyuda = sAyuda + "N.fecha_vcto," + Environment.NewLine;
        //        sAyuda = sAyuda + "N.Id_Nota_Credito,N.clave_acceso," + Environment.NewLine;
        //        sAyuda = sAyuda + "L.establecimiento estab,isnull(L.punto_emision,'009') ptoEmi," + Environment.NewLine;
        //        sAyuda = sAyuda + conexion.esNulo() + "(autorizacion,'') autorizacion," + Environment.NewLine;
        //        sAyuda = sAyuda + conexion.esNulo() + "(CONVERT (nvarchar(19), fecha_autorizacion, 120),'') fecha_autorizacion," + Environment.NewLine;
        //        sAyuda = sAyuda + "id_tipo_emision,id_tipo_ambiente" + Environment.NewLine;
        //        sAyuda = sAyuda + "from" + Environment.NewLine;
        //        sAyuda = sAyuda + "cv403_notas_credito N, tp_localidades L," + Environment.NewLine;
        //        sAyuda = sAyuda + "tp_codigos LOCALIDAD," + Environment.NewLine;
        //        sAyuda = sAyuda + "tp_personas P," + Environment.NewLine;
        //        sAyuda = sAyuda + "cv403_numeros_notas_creditos NNC" + Environment.NewLine;
        //        sAyuda = sAyuda + "where" + Environment.NewLine;
        //        sAyuda = sAyuda + "N.estado = 'A'" + Environment.NewLine;
        //        sAyuda = sAyuda + "and N.id_persona = P.id_persona" + Environment.NewLine;
        //        sAyuda = sAyuda + "and NNC.Id_Nota_Credito = N.Id_Nota_Credito" + Environment.NewLine;

        //        //If G_Ln_Id_Servidor > 1 Then
        //        //   T_St_Sql = T_St_Sql & "and l.id_servidor = " & G_Ln_Id_Servidor & " "
        //        //End If

        //        sAyuda = sAyuda + "and N.id_localidad = L.id_localidad" + Environment.NewLine;
        //        sAyuda = sAyuda + "and L.cg_localidad = LOCALIDAD.correlativo" + Environment.NewLine;

        //        //  Generadas
        //        if (P_Ln_Orden == 1)
        //        {
        //            sAyuda = sAyuda + "and N.clave_acceso is not null" + Environment.NewLine;
        //        }

        //        //  Firmadas
        //        else if (P_Ln_Orden == 2)
        //        {
        //            sAyuda = sAyuda + "and N.clave_acceso is not null" + Environment.NewLine;
        //        }

        //        //  Autorizadas
        //        else if (P_Ln_Orden == 3)
        //        {
        //            sAyuda = sAyuda + "and N.autorizacion is not null" + Environment.NewLine;
        //        }

        //        sAyuda = sAyuda + "and NNC.estado = 'A'" + Environment.NewLine;
        //        sAyuda = sAyuda + "Order by  N.Id_nota_credito desc";
        //    }

        //    //GUIA DE REMISION
        //    if (P_St_CodDoc == "06")
        //    {
        //        sAyuda = "";
        //        sAyuda = sAyuda + "select" + Environment.NewLine;
        //        sAyuda = sAyuda + "NGR.Numero_Guia_Remision," + Environment.NewLine;

        //        if (conexion.bddConexion() == "MYSQL")
        //        {
        //            sAyuda = sAyuda + "concat(P.apellidos,' '," + conexion.esNulo() + "(P.nombres,'')) Cliente," + Environment.NewLine;
        //        }

        //        else
        //        {
        //            sAyuda = sAyuda + "P.apellidos + ' ' + " + conexion.esNulo() + "(P.nombres,'') Cliente," + Environment.NewLine;
        //        }

        //        sAyuda = sAyuda + "SubString(LOCALIDAD.valor_texto, 1, 25) Localidad," + Environment.NewLine;
        //        sAyuda = sAyuda + "G.fecha_emision," + Environment.NewLine;
        //        sAyuda = sAyuda + "G.Id_Guia_Remision,G.clave_acceso," + Environment.NewLine;
        //        sAyuda = sAyuda + "L.establecimiento estab,";
        //        sAyuda = sAyuda + conexion.esNulo() + "(L.punto_emision,'009') ptoEmi," + Environment.NewLine;
        //        sAyuda = sAyuda + conexion.esNulo() + "(G.autorizacion,'') autorizacion," + Environment.NewLine;
        //        sAyuda = sAyuda + conexion.esNulo() + "(CONVERT (nvarchar(19), G.fecha_autorizacion, 120),'') fecha_autorizacion," + Environment.NewLine;
        //        sAyuda = sAyuda + "G.id_tipo_emision,G.id_tipo_ambiente" + Environment.NewLine;
        //        sAyuda = sAyuda + "from" + Environment.NewLine;
        //        sAyuda = sAyuda + "cv403_guias_remision G, tp_localidades L," + Environment.NewLine;
        //        sAyuda = sAyuda + "tp_codigos LOCALIDAD," + Environment.NewLine;
        //        sAyuda = sAyuda + "tp_personas P," + Environment.NewLine;
        //        sAyuda = sAyuda + "cv403_numeros_guias_remision NGR" + Environment.NewLine;
        //        sAyuda = sAyuda + "where" + Environment.NewLine;
        //        sAyuda = sAyuda + "G.estado = 'A'" + Environment.NewLine;
        //        sAyuda = sAyuda + "and G.id_destinatario = P.id_persona" + Environment.NewLine;
        //        sAyuda = sAyuda + "and NGR.Id_Guia_Remision = G.Id_Guia_Remision" + Environment.NewLine;

        //        //If G_Ln_Id_Servidor > 1 Then
        //        //    T_St_Sql = T_St_Sql & "and L.id_servidor = " & G_Ln_Id_Servidor & " "
        //        //End If

        //        sAyuda = sAyuda + "and G.id_localidad = L.id_localidad" + Environment.NewLine;
        //        sAyuda = sAyuda + "and L.cg_localidad = LOCALIDAD.correlativo" + Environment.NewLine;

        //        //  Generadas
        //        if (P_Ln_Orden == 1)
        //        {
        //            sAyuda = sAyuda + "and G.clave_acceso is not null" + Environment.NewLine;
        //        }

        //        //  Firmadas
        //        else if (P_Ln_Orden == 2)
        //        {
        //            sAyuda = sAyuda + "and G.clave_acceso is not null" + Environment.NewLine;
        //        }

        //        //  Autorizadas
        //        else if (P_Ln_Orden == 3)
        //        {
        //            sAyuda = sAyuda + "and G.autorizacion is not null" + Environment.NewLine;
        //        }

        //        sAyuda = sAyuda + "and NGR.estado = 'A'" + Environment.NewLine;
        //        sAyuda = sAyuda + "Order by  G.Id_Guia_Remision desc";
        //    }

        //    return sAyuda;
        //}

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
                //catchMensaje.Show();
                return "";
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

        //NUEVO PROCESO PARA CONVERTIR EN BASE 64 EL ARCHIVO
        public string ConvertirBase64(string sRutaArchivo)
        {
            byte[] sBase64 = File.ReadAllBytes(sRutaArchivo);
            return Convert.ToBase64String(sBase64, 0, sBase64.Length);
        }

        //ENVIO DE COMPROBANTE AL WEB SERVICE DEL SRI
        /// <summary>
        /// Envía el xml firmado a los webs services del SRI para su recepción.
        /// </summary>
        public RespuestaSRI EnvioComprobante(string sRutaXML, string WSEnvio)
        {
            RespuestaSRI result = null;

            //string ws_url = "https://celcer.sri.gob.ec/comprobantes-electronicos-ws/RecepcionComprobantes?wsdl";

            //Codifica el archivo a base 64
            string bytesEncodeBase64 = ConvertirBase64(sRutaXML);

            //Crea el request del web service
            HttpWebRequest request = CreateWebRequest(WSEnvio, "POST");

            //Arma la cadena xml ara el envío al web service
            string stringRequest = string.Format(xmlEnvioRequestTemplate, bytesEncodeBase64);

            //Convierte la cadena en un documento xml
            //XmlDocument xmlRequest = ConvertStringToDocument(stringRequest);
            XmlDocument xmlRequest = new XmlDocument();
            xmlRequest.LoadXml(stringRequest);

            //Crea un flujo de datos (stream) y escribe el xml en la solicitud de respuesta del web service
            using (Stream stream = request.GetRequestStream())
            {
                xmlRequest.Save(stream);
            }

            //Obtiene la respuesta del web service
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    //Lee el flujo de datos (stream) de respuesta del web service
                    string soapResultStr = rd.ReadToEnd();

                    //Convierte la respuesta de string a xml para extraer el detalle de la respuesta del web service
                    //XmlDocument soapResultXML = XMLHelper.ConvertStringToDocument(soapResultStr);
                    //XmlDocument soapResultXML = new XmlDocument();
                    //soapResultXML.LoadXml(soapResultStr);
                    XmlDocument soapResultXML = ConvertStringToDocument(soapResultStr);

                    //Obtiene la respuesta detallada
                    result = ClaseXMLAyuda.GetRespuestaRecepcion(soapResultXML);
                    //result = ClaseEnviarXML.GetRespuestaRecepcion(soapResultXML);
                }
            }

            return result;
        }

        /// Crea y devuelve una instancia de objeto para la solicitud de respuesta desde una URI.
        /// </summary>
        /// <param name="uri">URI del recurso de internet</param>
        /// <param name="method">Método de solicitud</param>
        private static HttpWebRequest CreateWebRequest(string uri, string method)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Headers.Add("SOAP:Action");
            webRequest.ContentType = "application/soap+xml;charset=utf-8";
            webRequest.Accept = "text/xml";
            webRequest.Method = method;

            return webRequest;
        }

        public static XmlDocument ConvertStringToDocument(string xml_string)
        {
            XmlDocument result = new XmlDocument();
            result.LoadXml(xml_string);

            return result;
        }
    }
}
