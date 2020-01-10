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
    public class ClaseCrearVenta
    {
        manejadorConexion conexionM = new manejadorConexion();

        DataTable dtConsulta;

        string sSql;
        string sJson;
        string sToken;
        string sRespuestaJson;
        string sUrlVentas;
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

        //VARIABLES PARA RECORRER EL DATATABLE

        int iNivel;
        int iAsiento;
        int iTipoTarifa;
        int iExtranjeroPasajero;
        
        string sAsiento;
        string sIdentificacionPasajero;
        string sNombrePasajero;
        string sCorreoPasajero;
        
        Decimal dbValorAsiento;
        Decimal dbValorDescuento;

        //FIN VARIABLES

        //FUNCION QUE DEVUELVE EL JSON
        public string recuperarJsonCrearVenta(string sToken_P, string sFechaHora_P, int iTipoViaje_P, int iFormaPago_P,
                                              string sLocalidad_P, string sParadaNombre_P, string sIdentificacionFactura_P,
                                              string sClienteFactura_P, string sCorreoFactura, int iExtranjero_P, DataTable dtDatos_P)
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
                sUrlVentas = dtConsulta.Rows[0]["api_ventas_contifico"].ToString().Trim();
                iTiempoRespuesta = Convert.ToInt32(dtConsulta.Rows[0]["timeout"].ToString());
                iEmision = Convert.ToInt32(dtConsulta.Rows[0]["emision"].ToString());

                if (iEmision == 0)
                    sUrlEnviar = sUrlPruebas + sUrlVentas;
                else
                    sUrlEnviar = sUrlProduccion + sUrlVentas;

                sJson = "";
                sJson += "{" + Environment.NewLine;
                sJson += "\"fecha_hora_venta\": \"" + sFechaHora_P + "\"," + Environment.NewLine;
                sJson += "\"viaje\": " + iTipoViaje_P + "," + Environment.NewLine;
                sJson += "\"forma_de_pago\": " + iFormaPago_P + "," + Environment.NewLine;
                sJson += "\"localidad\": " + sLocalidad_P + "," + Environment.NewLine;
                sJson += "\"parada_nombre\": \"" + sParadaNombre_P + "\"," + Environment.NewLine;
                sJson += "\"cliente\": {" + Environment.NewLine;
                sJson += "\"identificacion\": \"" + sIdentificacionFactura_P + "\"," + Environment.NewLine;
                sJson += "\"nombre\": \"" + sClienteFactura_P + "\"," + Environment.NewLine;
                sJson += "\"correo\": \"" + sCorreoFactura + "\"," + Environment.NewLine;
                sJson += "\"extranjero\": " + iExtranjero_P + Environment.NewLine;
                sJson += "}," + Environment.NewLine;
                sJson += "\"boletos\": [" + Environment.NewLine;

                for (int i = 0; i < dtDatos_P.Rows.Count; i++)
                {
                    iNivel = Convert.ToInt32(dtDatos_P.Rows[i]["nivel"].ToString());
                    iAsiento = Convert.ToInt32(dtDatos_P.Rows[i]["asiento"].ToString());
                    iTipoTarifa = Convert.ToInt32(dtDatos_P.Rows[i]["tipo_tarifa"].ToString());
                    iExtranjeroPasajero = Convert.ToInt32(dtDatos_P.Rows[i]["extranjero_pasajero"].ToString());

                    sAsiento = dtDatos_P.Rows[i]["asiento_nombre"].ToString().Trim();
                    sIdentificacionPasajero = dtDatos_P.Rows[i]["identificacion_pasajero"].ToString().Trim();
                    sNombrePasajero = dtDatos_P.Rows[i]["nombre_pasajero"].ToString().Trim();
                    sCorreoPasajero = dtDatos_P.Rows[i]["correo_pasajero"].ToString().Trim();

                    dbValorAsiento = Convert.ToDecimal(dtDatos_P.Rows[i]["valor_asiento"].ToString());
                    dbValorDescuento = Convert.ToDecimal(dtDatos_P.Rows[i]["valor_descuento"].ToString());

                    sJson += "{" + Environment.NewLine;
                    sJson += "\"nivel\": " + iNivel + "," + Environment.NewLine;
                    sJson += "\"asiento\": " + iAsiento + "," + Environment.NewLine;
                    sJson += "\"asiento_nombre\": \"" + iAsiento.ToString() + "\"," + Environment.NewLine;
                    sJson += "\"valor\": " + dbValorAsiento + "," + Environment.NewLine;
                    sJson += "\"tipo\": " + iTipoTarifa + "," + Environment.NewLine;
                    sJson += "\"descuento\": " + dbValorDescuento + "," + Environment.NewLine;
                    sJson += "\"pasajero\": {" + Environment.NewLine;
                    sJson += "\"identificacion\": \"" + sIdentificacionPasajero + "\"," + Environment.NewLine;
                    sJson += "\"nombre\": \"" + sNombrePasajero + "\"," + Environment.NewLine;
                    sJson += "\"correo\": \"" + sCorreoPasajero + "\"," + Environment.NewLine;
                    sJson += "\"extranjero\": " + iExtranjeroPasajero + Environment.NewLine;
                    sJson += "}" + Environment.NewLine;

                    if (i + 1 == dtDatos_P.Rows.Count)
                    {
                        sJson += "}" + Environment.NewLine;
                    }

                    else
                    {
                        sJson += "}," + Environment.NewLine;
                    }
                }

                sJson += "]" + Environment.NewLine;
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
        private int consultarUrlVentas()
        {
            try
            {
                sSql = "";
                sSql += "select api_ventas_contifico, timeout," + Environment.NewLine;
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