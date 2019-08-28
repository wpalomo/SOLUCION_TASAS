using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ENTIDADES;
using NEGOCIO;
using System.IO;

namespace Solution_CTT
{
    public partial class frmParametrosFacturacionElectronica : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

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

            Session["modulo"] = "MÓDULO DE PARAMETRIZACIÓN DE COMPROBANTES ELECTRÓNICOS";

            if (!IsPostBack)
            {
                cargarInformacion();
                //txtPasswordCuenta.Attributes.Add("value", txtPasswordCuenta.Text);//OCULTAR CONTRASEÑA
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA CARGAR INFORMACION
        private void cargarInformacion()
        {
            try
            {
                sSql = "";
                sSql = sSql + "select correo_que_envia, correo_palabra_clave, correo_smtp," + Environment.NewLine;
                sSql = sSql + "correo_puerto, correo_con_copia, correo_consumidor_final," + Environment.NewLine;
                sSql = sSql + "correo_ambiente_prueba, wsdl_pruebas, url_pruebas," + Environment.NewLine;
                sSql = sSql + "wsdl_produccion, url_produccion, certificado_ruta," + Environment.NewLine;
                sSql = sSql + "certificado_palabra_clave, Estado, id_cel_parametro, maneja_SSL" + Environment.NewLine;
                sSql = sSql + "from cel_parametro" + Environment.NewLine;
                sSql = sSql + "where estado ='A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        txtCuenta.Text = dtConsulta.Rows[0].ItemArray[0].ToString();
                        txtPasswordCuenta.Text = dtConsulta.Rows[0].ItemArray[1].ToString();
                        txtSmtp.Text = dtConsulta.Rows[0].ItemArray[2].ToString();
                        txtPuerto.Text = dtConsulta.Rows[0].ItemArray[3].ToString();
                        txtCorreoCopia.Text = dtConsulta.Rows[0].ItemArray[4].ToString();
                        txtCorreoConsumidorFinal.Text = dtConsulta.Rows[0].ItemArray[5].ToString();
                        txtCorreoAmbientePruebas.Text = dtConsulta.Rows[0].ItemArray[6].ToString();
                        txtEnvioPruebas.Text = dtConsulta.Rows[0].ItemArray[7].ToString();
                        txtConsultaPruebas.Text = dtConsulta.Rows[0].ItemArray[8].ToString();
                        txtEnvioProduccion.Text = dtConsulta.Rows[0].ItemArray[9].ToString();
                        txtConsultaProduccion.Text = dtConsulta.Rows[0].ItemArray[10].ToString();
                        txtRuta.Text = dtConsulta.Rows[0].ItemArray[11].ToString();
                        txtPasswordCertificado.Text = dtConsulta.Rows[0].ItemArray[12].ToString();

                        chkSSL.Checked = Convert.ToBoolean(dtConsulta.Rows[0].ItemArray[15]);

                        //if (dtConsulta.Rows[0].ItemArray[13].ToString() == "A")
                        //    cmbEstado.SelectedIndex = 0;
                        //else
                        //    cmbEstado.SelectedIndex = 1;
                        //txtEstado.Text = dtConsulta.Rows[0].ItemArray[13].ToString();

                        Session["iIdParametro"] = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[14].ToString());


                        //chkMostrarPasswordCuenta.Checked = false;
                        //chkPasswordCertificado.Checked = false;

                        txtCuenta.Focus();
                        //txtCuenta.SelectionStart = txtCuenta.Text.Trim().Length;
                    }

                    else
                    {
                        limpiar();
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtCuenta.Text = "";
            txtPasswordCuenta.Text = "";
            txtSmtp.Text = "";
            txtPuerto.Text = "";
            txtCorreoCopia.Text = "";
            txtCorreoConsumidorFinal.Text = "";
            txtCorreoAmbientePruebas.Text = "";
            txtEnvioPruebas.Text = "";
            txtConsultaPruebas.Text = "";
            txtEnvioProduccion.Text = "";
            txtConsultaProduccion.Text = "";
            txtRuta.Text = "";
            txtPasswordCertificado.Text = "";
            cargarInformacion();
            txtCuenta.Focus();
        }

        //FUNCION PARA INSERTAR UN REGISTRO NUEVO
        private void insertarRegistro()
        {
            try
            {
                //INICIAMOS UNA NUEVA TRANSACCION
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sSql = "";
                sSql = sSql + "insert into cel_parametro(" + Environment.NewLine;
                sSql = sSql + "correo_que_envia, correo_palabra_clave, correo_smtp," + Environment.NewLine;
                sSql = sSql + "correo_puerto, correo_con_copia, correo_consumidor_final," + Environment.NewLine;
                sSql = sSql + "correo_ambiente_prueba, wsdl_pruebas, url_pruebas," + Environment.NewLine;
                sSql = sSql + "wsdl_produccion, url_produccion, certificado_ruta," + Environment.NewLine;
                sSql = sSql + "certificado_palabra_clave, estado, fecha_ingreso," + Environment.NewLine;
                sSql = sSql + "usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql = sSql + "values(" + Environment.NewLine;
                sSql = sSql + "'" + txtCuenta.Text.Trim() + "', '" + txtPasswordCuenta.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "'" + txtSmtp.Text.Trim() + "', " + Convert.ToInt32(txtPuerto.Text.Trim()) + "," + Environment.NewLine;
                sSql = sSql + "'" + txtCorreoCopia.Text.Trim() + "', '" + txtCorreoConsumidorFinal.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "'" + txtCorreoAmbientePruebas.Text.Trim() + "', '" + txtEnvioPruebas.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "'" + txtConsultaPruebas.Text.Trim() + "', '" + txtEnvioProduccion.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "'" + txtConsultaProduccion.Text.Trim() + "', '" + txtRuta.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "'" + txtPasswordCertificado.Text.Trim() + "', 'A', GETDATE()," + Environment.NewLine;
                sSql = sSql + "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUTA EL QUERY DE INSERCIÓN
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_MSJ_TITULO_EXITO + "', '" + Resources.MESSAGES.TXT_MSJ_BODY_EXITO_INSERT + "', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };

        fin: { };
        }

        //FUNCION PARA ACTUALIZAR EL REGISTRO
        private void actualizarRegistro()
        {
            try
            {
                //INICIAMOS UNA NUEVA TRANSACCION
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sSql = "";
                sSql = sSql + "update cel_parametro set" + Environment.NewLine;
                sSql = sSql + "correo_que_envia = '" + txtCuenta.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "correo_palabra_clave = '" + txtPasswordCuenta.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "correo_smtp = '" + txtSmtp.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "correo_puerto = " + Convert.ToInt32(txtPuerto.Text.Trim()) + "," + Environment.NewLine;
                sSql = sSql + "correo_con_copia = '" + txtCorreoCopia.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "correo_consumidor_final = '" + txtCorreoConsumidorFinal.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "correo_ambiente_prueba = '" + txtCorreoAmbientePruebas.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "wsdl_pruebas = '" + txtEnvioPruebas.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "url_pruebas = '" + txtConsultaPruebas.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "wsdl_produccion = '" + txtEnvioProduccion.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "url_produccion = '" + txtConsultaProduccion.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "certificado_ruta = '" + txtRuta.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "certificado_palabra_clave = '" + txtPasswordCertificado.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "usuario_ingreso = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql = sSql + "terminal_ingreso = '" + sDatosMaximo[1] + "'," + Environment.NewLine;
                sSql = sSql + "fecha_ingreso = GETDATE()" + Environment.NewLine;
                sSql = sSql + "where id_cel_parametro = " + Convert.ToInt32(Session["iIdParametro"]);

                //EJECUTA EL QUERY DE ACTUALIZACION
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_MSJ_TITULO_EXITO + "', '" + Resources.MESSAGES.TXT_MSJ_BODY_EXITO_UPDATE + "', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };

        fin: { };
        }

        private bool Validar()
        {
            if (string.IsNullOrEmpty(txtCuenta.Text) || string.IsNullOrEmpty(txtPasswordCuenta.Text) || string.IsNullOrEmpty(txtSmtp.Text) || string.IsNullOrEmpty(txtPuerto.Text) || string.IsNullOrEmpty(txtCorreoCopia.Text) || string.IsNullOrEmpty(txtCorreoConsumidorFinal.Text) || string.IsNullOrEmpty(txtCorreoAmbientePruebas.Text) || string.IsNullOrEmpty(txtEnvioPruebas.Text) || string.IsNullOrEmpty(txtConsultaPruebas.Text) || string.IsNullOrEmpty(txtEnvioProduccion.Text) || string.IsNullOrEmpty(txtConsultaProduccion.Text) || string.IsNullOrEmpty(txtRuta.Text) || string.IsNullOrEmpty(txtPasswordCertificado.Text))
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

        protected void lbtnGuardar_Click(object sender, EventArgs e)
        {

            if (Validar() != true) return;
            if (Convert.ToInt32(Session["iIdParametro"]) != 0)
            {
                //ENVIAR A FUNCION PARA ACTUALIZAR EL REGISTRO
                actualizarRegistro();
            }
            else
            {
                //ENVIAR A FUNCION PARA INSERTAR EL REGISTRO
                insertarRegistro();
            }
        }

        protected void lbtnCancelar_Click(object sender, EventArgs e)
        {
            limpiar();
        }
    }
}