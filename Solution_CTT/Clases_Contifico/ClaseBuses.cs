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

namespace Solution_CTT.Clases_Contifico
{
    public class ClaseBuses
    {
        manejadorConexion conexionM = new manejadorConexion();

        DataTable dtConsulta;

        string sSql;
        string sJson;
        string sToken;
        string sRespuestaJson;
        string sUrlBuses;
        string sMetodo = "GET";
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
        public string recuperarJson(string sToken_P, int iOp, int iPagina_P)
        {
            try
            {
                iCantidad = consultarUrlBuses();

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

                if (iOp == 0)
                {
                    sUrlBuses = dtConsulta.Rows[0]["api_buses_contifico"].ToString().Trim();
                }

                else
                {
                    sUrlBuses = dtConsulta.Rows[0]["api_buses_contifico"].ToString().Trim() + "?page=" + iPagina_P.ToString();
                }

                sUrlPruebas = dtConsulta.Rows[0]["servidor_pruebas"].ToString().Trim();
                sUrlProduccion = dtConsulta.Rows[0]["servidor_produccion"].ToString().Trim();
                iTiempoRespuesta = Convert.ToInt32(dtConsulta.Rows[0]["timeout"].ToString());
                iEmision = Convert.ToInt32(dtConsulta.Rows[0]["emision"].ToString());

                if (iEmision == 0)
                    sUrlEnviar = sUrlPruebas + sUrlBuses;
                else
                    sUrlEnviar = sUrlProduccion + sUrlBuses;

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
        private int consultarUrlBuses()
        {
            try
            {
                sSql = "";
                sSql += "select api_buses_contifico, timeout," + Environment.NewLine;
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

        //FUNCION PARA ENVIAR EL JSON AL SERVIDOR PARA AUTORIZACION
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

                try
                {
                    //Hacer la llamada
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        //Leer el resultado de la llamada
                        Stream stream1 = response.GetResponseStream();
                        StreamReader sr = new StreamReader(stream1);
                        sRespuestaJson = sr.ReadToEnd();
                    }

                    return true;
                }

                catch (Exception)
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
    }
}