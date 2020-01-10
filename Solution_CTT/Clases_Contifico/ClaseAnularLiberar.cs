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
    public class ClaseAnularLiberar
    {
        manejadorConexion conexionM = new manejadorConexion();

        DataTable dtConsulta;

        string sSql;
        string sJson;
        string sToken;
        string sRespuestaJson;
        string sUrlVentas;
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

        //FUNCION PARA EXTRAER EL URL DE BUSES
        private int consultarUrlVentas()
        {
            try
            {
                sSql = "";
                sSql += "select api_ventas_contifico, api_viajes_contifico, timeout," + Environment.NewLine;
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

        //FUNCION QUE DEVUELVE EL JSON DE ANULACION
        public string recuperarJsonAnulacionVenta(string sToken_P, int iIdVenta_P)
        {
            try
            {
                iCantidad = consultarUrlVentas();

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
                sUrlVentas = dtConsulta.Rows[0]["api_ventas_contifico"].ToString().Trim() + iIdVenta_P.ToString() + "/anular/";
                iTiempoRespuesta = Convert.ToInt32(dtConsulta.Rows[0]["timeout"].ToString());
                iEmision = Convert.ToInt32(dtConsulta.Rows[0]["emision"].ToString());

                if (iEmision == 0)
                    sUrlEnviar = sUrlPruebas + sUrlVentas;
                else
                    sUrlEnviar = sUrlProduccion + sUrlVentas;

                if (enviarJsonAnular(sToken_P) == false)
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

        //FUNCION PARA ENVIAR EL JSON AL SERVIDOR PARA AUTORIZACION
        private bool enviarJsonAnular(string sToken)
        {
            try
            {
                //Llamar a funcion para aceptar los certificados de la URL
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                //Declara el objeto con el que haremos la llamada al servicio
                HttpWebRequest request = WebRequest.Create(sUrlEnviar) as HttpWebRequest;
                //Configurar las propiedad del objeto de llamada
                request.Method = sMetodoPut;
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

        //FUNCION QUE DEVUELVE EL JSON DE LIBERACION
        public string recuperarJsonLiberacionVenta(string sToken_P, int iIdViaje_P, int iNumeroAsiento_P)
        {
            try
            {
                iCantidad = consultarUrlVentas();

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
                sUrlVentas = dtConsulta.Rows[0]["api_viajes_contifico"].ToString().Trim() + iIdViaje_P.ToString() + "/liberar_asiento/";
                iTiempoRespuesta = Convert.ToInt32(dtConsulta.Rows[0]["timeout"].ToString());
                iEmision = Convert.ToInt32(dtConsulta.Rows[0]["emision"].ToString());

                if (iEmision == 0)
                    sUrlEnviar = sUrlPruebas + sUrlVentas;
                else
                    sUrlEnviar = sUrlProduccion + sUrlVentas;

                sJson = "";
                sJson += "{" + Environment.NewLine;
                sJson += "\"asiento\": " + iNumeroAsiento_P + Environment.NewLine;
                sJson += "}";


                if (enviarJsonLiberar(sToken_P) == false)
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

        //FUNCION PARA ENVIAR EL JSON AL SERVIDOR PARA AUTORIZACION
        private bool enviarJsonLiberar(string sToken)
        {
            try
            {
                //Llamar a funcion para aceptar los certificados de la URL
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                //Declara el objeto con el que haremos la llamada al servicio
                HttpWebRequest request = WebRequest.Create(sUrlVentas) as HttpWebRequest;
                //Configurar las propiedad del objeto de llamada
                request.Method = sMetodoPut;
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