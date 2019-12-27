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
    public class ClaseConsultarVentas
    {
        manejadorConexion conexionM = new manejadorConexion();

        DataTable dtConsulta;

        string sSql;
        string sJson;
        string sToken;
        string sRespuestaJson;
        string sUrlVentas;
        string sMetodo = "GET";

        bool bRespuesta;

        int iCantidad;
        int iTiempoRespuesta;

        //FUNCION QUE DEVUELVE EL JSON POR RANGO DE FECHAS
        public string recuperarJsonPorFechas(string sToken_P, string sFechaInicio_P, string sFechaFinal_P, int iOp, int iPagina_P)
        {
            try
            {
                iCantidad = consultarUrlVentas();

                if (iCantidad == -1)
                {
                    return "ERROR";
                }

                if (iCantidad == 0)
                {
                    return "ISNULL";
                }

                if (iOp == 0)
                {
                    sUrlVentas = dtConsulta.Rows[0]["api_ventas_contifico"].ToString().Trim() + "?fecha_inicio=" + sFechaInicio_P + "&fecha_fin=" + sFechaFinal_P;
                }

                else
                {
                    sUrlVentas = dtConsulta.Rows[0]["api_ventas_contifico"].ToString().Trim() + "?fecha_inicio=" + sFechaInicio_P + "&fecha_fin=" + sFechaFinal_P + "&page=" + iPagina_P.ToString();
                }

                iTiempoRespuesta = Convert.ToInt32(dtConsulta.Rows[0]["timeout"].ToString());

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

        //FUNCION QUE DEVUELVE EL JSON POR NUMERO DE DOCUMENTO
        public string recuperarJsonPorNumeroDocumento(string sToken_P, string sNumeroDocumento_P, int iOp, int iPagina_P)
        {
            try
            {
                iCantidad = consultarUrlVentas();

                if (iCantidad == -1)
                {
                    return "ERROR";
                }

                if (iCantidad == 0)
                {
                    return "ISNULL";
                }

                if (iOp == 0)
                {
                    sUrlVentas = dtConsulta.Rows[0]["api_ventas_contifico"].ToString().Trim() + "?numero_documento_tasa=" + sNumeroDocumento_P;
                }

                else
                {
                    sUrlVentas = dtConsulta.Rows[0]["api_ventas_contifico"].ToString().Trim() + "?numero_documento_tasa=" + sNumeroDocumento_P + "&page=" + iPagina_P.ToString();
                }

                iTiempoRespuesta = Convert.ToInt32(dtConsulta.Rows[0]["timeout"].ToString());

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

        //FUNCION PARA EXTRAER EL URL DE VENTAS
        private int consultarUrlVentas()
        {
            try
            {
                sSql = "";
                sSql += "select api_ventas_contifico, timeout" + Environment.NewLine;
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
                HttpWebRequest request = WebRequest.Create(sUrlVentas) as HttpWebRequest;
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

            catch (Exception)
            {
                return false;
            }
        }
    }
}