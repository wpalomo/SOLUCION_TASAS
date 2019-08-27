using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using System.Net;
using System.Web;
using NEGOCIO;

namespace Solution_CTT.Clases_Factura_Electronica
{
    class ClaseConsultarXML
    {
        public static string URL_Envio;
        public static string URL_Autorizacion;
        public static string RutaXML;
        public static string ClaveAcceso;


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
      " <SOAP-ENV:Header/>",
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

        public static RespuestaSRI GetRespuestaRecepcion(XmlDocument xml_doc)
        {
            RespuestaSRI result = new RespuestaSRI();
            result.Estado = GetNodeValue("RespuestaRecepcionComprobante", "estado", xml_doc);

            if (result.Estado != "RECIBIDA")
            {
                result.ClaveAcceso = GetNodeValue("comprobante", "claveAcceso", xml_doc);
                result.ErrorIdentificador = GetNodeValue("mensaje", "identificador", xml_doc);
                result.ErrorMensaje = GetNodeValue("mensaje", "mensaje", xml_doc);
                result.ErrorInfoAdicional = GetNodeValue("mensaje", "informacionAdicional", xml_doc);
                result.ErrorTipo = GetNodeValue("mensaje", "tipo", xml_doc);
            }

            return result;
        }

        /// <summary>
        /// Devuelve la respuesta de la solicitud de recepción del comprobante en una estructura detallada.
        /// </summary>
        /// <param name="xml_doc">Documento XML de respuesta</param>
        public static RespuestaSRI GetRespuestaAutorizacion(XmlDocument xml_doc)
        {
            RespuestaSRI result = new RespuestaSRI();
            string pathLevelAutorizacion = "RespuestaAutorizacionComprobante/autorizaciones/autorizacion[last()]";
            string pathLevelMensajes = "RespuestaAutorizacionComprobante/autorizaciones/autorizacion/mensajes[last()]/mensaje";

            result.Estado = GetNodeValue(pathLevelAutorizacion, "estado", xml_doc);

            if (result.Estado == "AUTORIZADO")
            {
                result.Estado = GetNodeValue(pathLevelAutorizacion, "estado", xml_doc);
                result.NumeroAutorizacion = GetNodeValue(pathLevelAutorizacion, "numeroAutorizacion", xml_doc);
                result.FechaAutorizacion = GetNodeValue(pathLevelAutorizacion, "fechaAutorizacion", xml_doc);
                result.Ambiente = GetNodeValue(pathLevelAutorizacion, "ambiente", xml_doc);
                result.Comprobante = GetNodeValue(pathLevelAutorizacion, "comprobante", xml_doc);
            }
            else if (result.Estado == "NO AUTORIZADO")
            {
                result.Estado = GetNodeValue(pathLevelAutorizacion, "estado", xml_doc);
                result.FechaAutorizacion = GetNodeValue(pathLevelAutorizacion, "fechaAutorizacion", xml_doc);
                result.Ambiente = GetNodeValue(pathLevelAutorizacion, "ambiente", xml_doc);
                result.ErrorIdentificador = GetNodeValue(pathLevelMensajes, "identificador", xml_doc);
                result.ErrorMensaje = GetNodeValue(pathLevelMensajes, "mensaje", xml_doc);
                result.ErrorTipo = GetNodeValue(pathLevelMensajes, "tipo", xml_doc);
            }

            return result;
        }

        private static string GetNodeValue(string rootNodo, string infoNodo, XmlDocument doc)
        {
            string result = null;
            string node_path = "//" + rootNodo + "//" + infoNodo;

            XmlNode node = doc.SelectSingleNode(node_path);

            if (node != null)
            {
                result = node.InnerText;
            }

            return result;
        }

        public static XmlDocument ConvertStringToDocument(string xml_string)
        {
            XmlDocument result = new XmlDocument();
            result.LoadXml(xml_string);

            return result;
        }

        public static XmlDocument ConvertFileToDocument(string file_path)
        {
            XmlDocument result = new XmlDocument();
            result.Load(file_path);

            return result;
        }


        /// <summary>
        /// Convierte el documento en string Base64
        /// </summary>
        /// <param name="file_path">Ruta del archivo a aconvertir</param>
        public static string ConvertToBase64String(string file_path)
        {
            byte[] binarydata = File.ReadAllBytes(file_path);
            return Convert.ToBase64String(binarydata, 0, binarydata.Length);
        }

        /// <summary>
        /// Envía la clave de acceso a los webs services del SRI para consultar ele estado de autorización.
        /// </summary>
        public RespuestaSRI AutorizacionComprobante(out XmlDocument xml_doc, string sClaveAcceso, string WSConsulta)
        {
            RespuestaSRI result = null;
            //string ws_url = "https://celcer.sri.gob.ec/comprobantes-electronicos-ws/AutorizacionComprobantes?wsdl";

            //Crea el request del web service
            HttpWebRequest request = CreateWebRequest(WSConsulta, "POST");

            //Arma la cadena xml para el envío al web service
            string stringRequest = string.Format(xmlAutorizacionRequestTemplate, sClaveAcceso);

            //Convierte la cadena en un documeto xml
            XmlDocument xmlRequest = ConvertStringToDocument(stringRequest);
            xml_doc = xmlRequest;

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
                    XmlDocument soapResultXML = ConvertStringToDocument(soapResultStr);


                    ////PROBAR COMO GUARDA
                    //soapResultXML.Save(@"D:\\FACT.XML");

                    //Obtiene la respuesta detallada
                    result = GetRespuestaAutorizacion(soapResultXML);
                }
            }

            return result;
        }

        /// <summary>
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

        public string GetInfoTributaria(string info, XmlDocument xml_doc)
        {
            return GetNodeValue("infoTributaria", info, xml_doc);
        }

        public string GetInfoFactura(string info, XmlDocument xml_doc)
        {
            return GetNodeValue("infoFactura", info, xml_doc);
        }
    }
}
