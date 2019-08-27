using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using NEGOCIO;
using System.Data;

namespace Solution_CTT.Clases
{
    public class ClaseGenerarToken
    {
        manejadorConexion conexionM = new manejadorConexion();

        string strsb;
        string sAyuda;
        string sSql;

        DataTable dtConsulta;

        bool bRespuesta;

        public string Respuesta3()
        {
            strsb = "";
            sAyuda = "{ \"oficina\": { \"id_oficina\": \"477\", \"id_coop\": \"4462\", \"id_terminal\": \"1\" }}";

            //Declara el objeto con el que haremos la llamada al servicio
            HttpWebRequest request = WebRequest.Create("https://sandbox.devesofft.com:8085/api/credenciales") as HttpWebRequest;
            //Configurar las propiedad del objeto de llamada
            request.Method = "POST";
            request.ContentType = "application/json";

            //Serializar el objeto a enviar. Para esto uso la libreria Newtonsoft
            //string sb = JsonConvert.SerializeObject(sAyuda);
            string sb = sAyuda; ;

            //Convertir el objeto serializado a arreglo de byte
            Byte[] bt = Encoding.UTF8.GetBytes(sb);

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
                strsb = sr.ReadToEnd();
            }

            return strsb;
        }

        //FUNCION PARA CONSULTAR LOS PARAMETROS
        private bool consultarParametros()
        {
            try
            {
                

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {

                }

                else
                {
                    return false;
                }

                return true;
            }

            catch(Exception)
            {
                return false;
            }
        }
    }
}