using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NEGOCIO;

namespace Solution_CTT
{
    public partial class frmParametroCorreo : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string[] sDatosMaximo = new string[5];

        bool bRespuesta;

        int iManejaSSL;
        Int32 iPuerto;

        DataTable dtConsulta;

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

            Session["modulo"] = "MÓDULO DE PARAMETRIZACIÓN DE CORREO ELECTRÓNICO";

            if (!IsPostBack)
            {
                cargarParametros();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA CONSULTAR EL REGISTRO
        private void cargarParametros()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_parametro, isnull(correo_envia, '') correo_envia, isnull(clave_mail_envia, '') clave_mail_envia," + Environment.NewLine;
                sSql += "isnull(smtp_correo, '') smtp_correo, isnull(puerto_correo, 0) puerto_correo, isnull(maneja_ssl, 0) maneja_ssl," + Environment.NewLine;
                sSql += "isnull(correo_receptor_1, '') correo_receptor_1, isnull(correo_receptor_2, '') correo_receptor_2," + Environment.NewLine;
                sSql += "isnull(correo_receptor_3, '') correo_receptor_3" + Environment.NewLine;
                sSql += "from ctt_parametro" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["idParametro"] = dtConsulta.Rows[0][0].ToString();
                        txtCorreoEmisor.Text = dtConsulta.Rows[0][1].ToString();
                        Session["passwordMail"] = dtConsulta.Rows[0][2].ToString();
                        Session["banderaPass"] = "1";
                        txtPassword.Text = dtConsulta.Rows[0][2].ToString();
                        txtSmtp.Text = dtConsulta.Rows[0][3].ToString();
                        txtPuerto.Text = dtConsulta.Rows[0][4].ToString();

                        //CONSULTA FACTURACION ELECTRONICA
                        if (dtConsulta.Rows[0][5].ToString() == "1")
                        {
                            chkSSL.Checked = true;
                        }

                        else
                        {
                            chkSSL.Checked = false;
                        }                      

                        txtCorreoDestino_1.Text = dtConsulta.Rows[0][6].ToString();
                        txtCorreoDestino_2.Text = dtConsulta.Rows[0][7].ToString();
                        txtCorreoDestino_3.Text = dtConsulta.Rows[0][8].ToString();

                        pnlRegistro_1.Enabled = false;
                        pnlRegistro_2.Enabled = false;
                        MsjValidarCampos.Visible = false;
                        btnGuardar.Text = "Editar";
                    }

                    else
                    {
                        Session["idParametro"] = null;
                        Session["banderaPass"] = "0";
                        txtCorreoEmisor.Text = "";
                        txtSmtp.Text = "";
                        txtPuerto.Text = "";
                        txtCorreoDestino_1.Text = "";
                        txtCorreoDestino_2.Text = "";
                        txtCorreoDestino_3.Text = "";
                        chkSSL.Checked = false;
                        pnlRegistro_1.Enabled = true;
                        pnlRegistro_2.Enabled = true;
                        MsjValidarCampos.Visible = false;
                        btnGuardar.Text = "Guardar";
                    }
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA ACTUALIZAR EN LA BASE DE DATOS
        private void actualizarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sSql = "";
                sSql += "update ctt_parametro set" + Environment.NewLine;
                sSql += "correo_envia = '" + txtCorreoEmisor.Text.Trim().ToLower() + "'," + Environment.NewLine;
                sSql += "clave_mail_envia = '" + txtPassword.Text + "'," + Environment.NewLine;
                sSql += "smtp_correo = '" + txtSmtp.Text.Trim().ToLower() + "'," + Environment.NewLine;
                sSql += "puerto_correo = " + iPuerto + "," + Environment.NewLine;
                sSql += "maneja_ssl = " + iManejaSSL + "," + Environment.NewLine;
                sSql += "correo_receptor_1 = '" + txtCorreoDestino_1.Text.Trim().ToLower() + "'," + Environment.NewLine;
                sSql += "correo_receptor_2 = '" + txtCorreoDestino_2.Text.Trim().ToLower() + "'," + Environment.NewLine;
                sSql += "correo_receptor_3 = '" + txtCorreoDestino_3.Text.Trim().ToLower() + "'" + Environment.NewLine;
                sSql += "where id_ctt_parametro = " + Convert.ToInt32(Session["idParametro"]) + Environment.NewLine;
                sSql += "and estado = 'A'";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    conexionM.reversaTransaccion();
                    return;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro actualizado correctamente', 'success');", true);
                cargarParametros();
                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                conexionM.reversaTransaccion();
                return;
            }
        }

        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnGuardar.Text == "Editar")
                {
                    pnlRegistro_1.Enabled = true;
                    pnlRegistro_2.Enabled = true;
                    txtCorreoEmisor.Focus();
                    btnGuardar.Text = "Guardar";
                }

                else
                {
                    if (txtCorreoEmisor.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtCorreoEmisor.Focus();
                    }

                    else if (txtSmtp.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtSmtp.Focus();
                    }

                    else if (txtPuerto.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtPuerto.Focus();
                    }

                    else if (txtPassword.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtPassword.Focus();
                    }

                    else if (txtCorreoDestino_1.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtCorreoDestino_1.Focus();
                    }

                    else
                    {
                        //VISTA PREVIA
                        if (chkSSL.Checked == true)
                        {
                            iManejaSSL = 1;
                        }

                        else
                        {
                            iManejaSSL = 0;
                        }

                        iPuerto = Convert.ToInt32(txtPuerto.Text.Trim());

                        if (Session["idParametro"] == null)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor configure los parámetros del sistema para guardar los datos del correo emisor.', 'warning');", true);
                        }

                        else
                        {
                            actualizarRegistro();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            cargarParametros();
        }

        protected void btnVerPassword_Click(object sender, EventArgs e)
        {
            //if (Session["banderaPass"].ToString() == "1")
            //{
            //    txtPassword.Attributes.Add("value", txtPassword.Text);
            //}

            //else
            //{
            //    txtPassword.TextMode = TextBoxMode.Password;
                
            //}
        }
    }
}