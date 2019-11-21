using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Data;
using ENTIDADES;
using NEGOCIO;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace Solution_CTT
{
    public partial class frmCorreo : System.Web.UI.Page
    {        
        manejadorConexion conexionM = new manejadorConexion();
        Clases.ClaseEnviarCorreo correoC = new Clases.ClaseEnviarCorreo();

        DataTable dtConsulta;

        bool bRespuesta;

        string sSql;

        string[] sDatosMaximo = new string[5];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
                return;
            }

            sDatosMaximo[0] = Session["usuario"].ToString();
            sDatosMaximo[1] = Environment.MachineName.ToString();
            sDatosMaximo[2] = "A";

            Session["modulo"] = "MÓDULO DE ENVÍO DE CORREO ELECTRÓNICO PARA COMPROBANTES ELECTRÓNICOS";

            if (!IsPostBack)
            {
                cargarInformacion();
            }
        }

        #region FUNCIONES DEL USUARIO
        //FUNCION PARA CARGAR INFORMACION
        private void cargarInformacion()
        {
            try
            {
                if (Convert.ToInt32(Application["IDTipoAmbienteFE"]) == 1)//PRUEBAS
                {
                    sSql = "";
                    sSql = sSql + "select correo_que_envia, correo_palabra_clave, correo_smtp," + Environment.NewLine;
                    sSql = sSql + "correo_puerto, correo_con_copia, correo_consumidor_final," + Environment.NewLine;
                    sSql = sSql + "correo_ambiente_prueba, wsdl_pruebas, url_pruebas," + Environment.NewLine;
                    sSql = sSql + "wsdl_produccion, url_produccion, certificado_ruta," + Environment.NewLine;
                    sSql = sSql + "certificado_palabra_clave, Estado, id_cel_parametro" + Environment.NewLine;
                    sSql = sSql + "from cel_parametro" + Environment.NewLine;
                    sSql = sSql + "where estado ='A'";

                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                    if (bRespuesta == true)
                    {
                        if (dtConsulta.Rows.Count > 0)
                        {
                            txtCorreoEnvia.Text = dtConsulta.Rows[0].ItemArray[0].ToString();
                            txtPasswordCorreoEnvia.Text = dtConsulta.Rows[0].ItemArray[1].ToString();
                            txtSmtp.Text = dtConsulta.Rows[0].ItemArray[2].ToString();
                            txtPuerto.Text = dtConsulta.Rows[0].ItemArray[3].ToString();
                            txtCorreoRecibe.Text = dtConsulta.Rows[0].ItemArray[6].ToString();

                            txtCorreoCopiaOculta.Text = dtConsulta.Rows[0].ItemArray[4].ToString();//CORREO CON COPIA OCULTA, SACADO DE CORREO COPIA PARAMETROS
                        }

                        else
                        {

                        }
                    }
                }

                if (Convert.ToInt32(Application["IDTipoAmbienteFE"]) == 2)//PRODUCCION
                {
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }
        private string tempPath = @"~/temp";
        //ENVIAR CORREO
        private bool EnviarCorreo(string from, string password, string smtp_account, int port, string to, string subject, string message, ListBox listAtt, string tempPath)
        {
            try
            {
                correoC.SendMail(from, password, smtp_account, port, to, subject, message, listAtt, tempPath);
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_MSJ_TITULO_EXITO + "', '" + Resources.MESSAGES.TXT_MSJ_BODY_EXITO_MAIL + "', 'success');", true);
                
                return true;
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }
        
        private void Limpiar()
        {
            txtAsunto.Text = "";
            txtMensaje.Text = "";
            txtCorreoRecibe.Focus();

            //while (lstFiles.Items.Count > 0)
            //{
            //    borraEntrada(lstFiles.Items[0].Value);
            //}
        }

        //VALIDAR
        private bool Validar()
        {
            if (string.IsNullOrEmpty(txtCorreoRecibe.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_ADVERTENCIA + " ', '" + Resources.MESSAGES.TXT_ADVERTENCIA_MSJ + "', 'warning');", true);
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion
        

        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validar() != true) return;
                if (EnviarCorreo(txtCorreoEnvia.Text, txtPasswordCorreoEnvia.Text, txtSmtp.Text, Convert.ToInt32(txtPuerto.Text), txtCorreoRecibe.Text.Trim(), txtAsunto.Text, txtMensaje.Text, lstFiles, tempPath) == true)                    
                Limpiar();
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            cargarInformacion();
            Limpiar();
        }
        
        protected void cmdAddFile_Click(object sender, EventArgs e)
        {
            FileUpload f = fileAdjunto1;
            if (!f.HasFile) return;

            ListItem item = new ListItem();
            item.Value = f.FileName;
            item.Text = f.FileName + " (" + f.FileContent.Length.ToString("N0") + " bytes).";
            f.SaveAs(Server.MapPath(Path.Combine(tempPath, item.Value)));
            lstFiles.Items.Add(item);
        }

        protected void cmdDelFile_Click(object sender, EventArgs e)
        {
            ListBox lb = lstFiles;
            if (lb.SelectedItem == null) return;
            borraEntrada(lb.SelectedItem.Value);
        }
        private void borraEntrada(string fileName)
        {
            string fichero = Server.MapPath(Path.Combine(tempPath, fileName));
            File.Delete(fichero);
            ListItem l = lstFiles.Items.FindByValue(fileName);
            if (l != null)
                lstFiles.Items.Remove(l);
        }
    }
}