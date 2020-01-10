using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ENTIDADES;
using NEGOCIO;
using System.Data;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;

namespace Solution_CTT.Clases_Contifico
{
    public class ClaseCrearViaje
    {
        manejadorConexion conexionM = new manejadorConexion();

        DataTable dtConsulta;

        string sSql;
        string sJson;
        string sToken;
        string sRespuestaJson;
        string sUrlViajes;
        string sMetodo = "POST";
        string sMetodoPut = "PUT";
        string sUrlPruebas;
        string sUrlProduccion;
        string sUrlEnviar;

        public string sError;

        bool bRespuesta;

        int iCantidad;
        int iTiempoRespuesta;
        int iEmision;

        public int iTipoError;

        //FUNCION QUE DEVUELVE EL JSON
        public string recuperarJsonCrear(string sToken_P, string sFecha_P, string sFrecuenciaHora_P, string sDestinoNombre_P,
                                    string sVia_P, string sConductor_1_P, string sConductor_2_P, string sDisco_P, string sLocalidad_P)
        {
            try
            {
                iCantidad = consultarUrlViajes();

                if (iCantidad == -1)
                {
                    iTipoError = 2;
                    sError = "No se pudo obtener los parámetros de la Tasa de Usuario SMARTT.";
                    return "ERROR";
                }

                if (iCantidad == 0)
                {
                    return "ISNULL";
                }

                sUrlPruebas = dtConsulta.Rows[0]["servidor_pruebas"].ToString().Trim();
                sUrlProduccion = dtConsulta.Rows[0]["servidor_produccion"].ToString().Trim();
                sUrlViajes = dtConsulta.Rows[0]["api_viajes_contifico"].ToString().Trim();
                iTiempoRespuesta = Convert.ToInt32(dtConsulta.Rows[0]["timeout"].ToString());
                iEmision = Convert.ToInt32(dtConsulta.Rows[0]["emision"].ToString());

                if (iEmision == 0)
                    sUrlEnviar = sUrlPruebas + sUrlViajes;
                else
                    sUrlEnviar = sUrlProduccion + sUrlViajes;

                sJson = "";
                sJson += "{" + Environment.NewLine;
                sJson += "\"fecha\": \"" + sFecha_P + "\"," + Environment.NewLine;
                sJson += "\"frecuencia_hora\": \"" + sFrecuenciaHora_P + "\"," + Environment.NewLine;
                sJson += "\"destino_nombre\": \"" + sDestinoNombre_P + "\"," + Environment.NewLine;
                sJson += "\"via\": \"" + sVia_P + "\"," + Environment.NewLine;
                sJson += "\"conductor_identificacion\": \"" + sConductor_1_P + "\"," + Environment.NewLine;
                sJson += "\"conductor2_identificacion\": \"" + sConductor_2_P + "\"," + Environment.NewLine;
                sJson += "\"bus_disco\": " + Convert.ToInt32(sDisco_P) + "," + Environment.NewLine;
                sJson += "\"localidad\": " + sLocalidad_P + Environment.NewLine;
                sJson += "}";

                if (enviarJson(sToken_P) == false)
                {
                    return "ERROR";
                }

                return sRespuestaJson;
            }

            catch
            {
                return "ERROR";
            }
        }

        //FUNCION PARA EXTRAER EL URL DE BUSES
        private int consultarUrlViajes()
        {
            try
            {
                sSql = "";
                sSql += "select api_viajes_contifico, timeout," + Environment.NewLine;
                sSql += "servidor_pruebas, servidor_produccion, emision" + Environment.NewLine;
                sSql += "from ctt_vw_parametros_contifico" + Environment.NewLine;
                sSql += "where codigo = '02'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    return -1;
                }

                return dtConsulta.Rows.Count;
            }

            catch
            {
                return -1;
            }
        }

        //Funcion para aceptar los certificados de la URL
        private bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        
        //FUNCION PARA ENVIAR EL JSON AL SERVIDOR PARA CREAR EL VIAJE
        private bool enviarJson(string sToken)
        {
            try
            {
                //Llamar a funcion para aceptar los certificados de la URL
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                //Declara el objeto con el que haremos la llamada al servicio
                HttpWebRequest request = WebRequest.Create(sUrlEnviar) as HttpWebRequest;
                //Configurar las propiedad del objeto de llamada
                request.Method = sMetodo;
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", sToken);
                request.Timeout = iTiempoRespuesta;

                //Convertir el objeto serializado a arreglo de byte
                Byte[] bt = Encoding.UTF8.GetBytes(sJson);

                try
                {
                    //Agregar el objeto Byte[] al request
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();

                    //Hacer la llamada
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        //Leer el resultado de la llamada
                        Stream stream1 = response.GetResponseStream();
                        StreamReader sr = new StreamReader(stream1);
                        sRespuestaJson = sr.ReadToEnd();
                    }

                    sError = "";
                    iTipoError = 1;
                    return true;
                }

                catch (HttpException ex)
                {
                    return false;
                }
            }

            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    using (var stream = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        iTipoError = 1;
                        sError = stream.ReadToEnd();
                    }
                }

                else if (ex.Status == WebExceptionStatus.Timeout)
                {
                    iTipoError = 2;
                    sError = "Excedió el tiempo de respuesta del servidor.";
                }

                else
                {
                    iTipoError = 2;
                    sError = ex.Message;
                }

                return false;
            }
        }

        //DEVOLVER JSON CON CAMBIO DE BUS
        public string recuperarJsonCambiarBus(string sToken_P, string sIdViajeContifico_P, string sNuevoDisco_P)
        {
            try
            {
                iCantidad = consultarUrlViajes();

                if (iCantidad == -1)
                {
                    return "ERROR";
                }

                if (iCantidad == 0)
                {
                    return "ISNULL";
                }

                sUrlPruebas = dtConsulta.Rows[0]["servidor_pruebas"].ToString().Trim();
                sUrlProduccion = dtConsulta.Rows[0]["servidor_produccion"].ToString().Trim();
                sUrlViajes = dtConsulta.Rows[0]["api_viajes_contifico"].ToString().Trim() + sIdViajeContifico_P + "/cambio_bus/";
                iTiempoRespuesta = Convert.ToInt32(dtConsulta.Rows[0]["timeout"].ToString());
                iEmision = Convert.ToInt32(dtConsulta.Rows[0]["emision"].ToString());

                if (iEmision == 0)
                    sUrlEnviar = sUrlPruebas + sUrlViajes;
                else
                    sUrlEnviar = sUrlProduccion + sUrlViajes;

                sJson = "";
                sJson += "{" + Environment.NewLine;
                sJson += "\"bus_disco\": \"" + sNuevoDisco_P + "\"" + Environment.NewLine;
                sJson += "}";

                if (enviarJson(sToken_P) == false)
                {
                    return "ERROR";
                }

                return sRespuestaJson;
            }

            catch
            {
                return "ERROR";
            }
        }
    }
}