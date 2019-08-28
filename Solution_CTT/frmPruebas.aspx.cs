using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NEGOCIO;
using System.Data;
using Newtonsoft.Json;
using System.Net;

namespace Solution_CTT
{
    public partial class frmPruebas : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        Clases.ClaseImpresion imprimir = new Clases.ClaseImpresion();

        string sSql;
        string sNombreImpresora;
        string sPathImpresora;
        string sImprimir;
        
        int iCortarPapel;
        int iNumeroImpresiones;

        DataTable dtConsulta;

        bool bRespuesta;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region
        //FUNCION DE IMPRIMIR
        //FUNCION PARA CONSULTAR LA IMPRESORA DEL TERMINAL
        private void consultarImpresora()
        {
            try
            {
                sSql = "";
                sSql += "select descripcion, path_url, cortar_papel," + Environment.NewLine;
                sSql += "abrir_cajon, numero_impresion" + Environment.NewLine;
                sSql += "from ctt_impresora" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sNombreImpresora = dtConsulta.Rows[0][0].ToString();
                        sPathImpresora = dtConsulta.Rows[0][1].ToString();
                        iCortarPapel = Convert.ToInt32(dtConsulta.Rows[0][2].ToString());
                        iNumeroImpresiones = Convert.ToInt32(dtConsulta.Rows[0][4].ToString());

                        //imprimir.iniciarImpresion();
                        //imprimir.escritoEspaciadoCorto(sImprimir);
                        //imprimir.cortarPapel(iCortarPapel);
                        //imprimir.imprimirReporte(sPathImpresora);
                    }

                    else
                    {
                        //MENSAJE DE QUE NO HAY IMPRESORA INSTALADA O CONFIGURADA
                        
                    }
                }

                else
                {
                    
                }
            }

            catch (Exception ex)
            {
                
            }
        }

        #endregion

        protected void btnFondo_Click(object sender, EventArgs e)
        {

        }

        protected void btnDeserializar_Click(object sender, EventArgs e)
        {
            Clases_Tasa_Usuario.TasaUsuarioLote lote = JsonConvert.DeserializeObject<Clases_Tasa_Usuario.TasaUsuarioLote>(txtJsonLote.Text.Trim());
        }

        //Funcion para aceptar los certificados de la URL
        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        protected void btnWebService_Click(object sender, EventArgs e)
        {
            try
            {
                //Llamar a funcion para aceptar los certificados de la URL
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                var myRequest = (HttpWebRequest)WebRequest.Create(txtWebService.Text.Trim());

                var response = (HttpWebResponse)myRequest.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    txtRespuestaWeb.Text = string.Format("{0} Disponible", txtWebService.Text.Trim());
                }
                else
                {
                    txtRespuestaWeb.Text = string.Format("{0} Status: {1}", txtWebService.Text.Trim(), response.StatusDescription);
                }
            }

            catch (Exception ex)
            {
                txtRespuestaWeb.Text =string.Format("{0} No Disponible: {1}", txtWebService.Text.Trim(), ex.Message);
            }
        }

    }
}