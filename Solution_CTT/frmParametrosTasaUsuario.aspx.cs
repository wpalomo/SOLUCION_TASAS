using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ENTIDADES;
using NEGOCIO;
using System.Data;

namespace Solution_CTT
{
    public partial class frmParametrosTasaUsuario : System.Web.UI.Page
    {
        ENTComboDatos comboE = new ENTComboDatos();

        manejadorConexion conexionM = new manejadorConexion();
        manejadorComboDatos comboM = new manejadorComboDatos();

        string sSql;
        string[] sDatosMaximo = new string[5];

        bool bRespuesta;

        DataTable dtConsulta = new DataTable();

        int iPermitir;
        int iNotificacionEmergente;
        int iImprimirTasaUsuario;

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

            Session["modulo"] = "MÓDULO DE PARAMETRIZACIÓN DE TASAS DE USUARIO - DEVESOFFT";

            if (!IsPostBack)
            {
                llenarComboTerminales();
                cargarParametros();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMBOBOX DE TERMINALES
        private void llenarComboTerminales()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tasa_terminal, descripcion" + Environment.NewLine;
                sSql += "from ctt_tasa_terminal" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "order by descripcion";

                comboE.ISSQL = sSql;
                cmbTerminal.DataSource = comboM.listarCombo(comboE);
                cmbTerminal.DataValueField = "IID";
                cmbTerminal.DataTextField = "IDATO";
                cmbTerminal.DataBind();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CARGAR LOS PARÁMETROS
        private void cargarParametros()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tasa_parametro, id_ctt_tasa_terminal, emision," + Environment.NewLine;
                sSql += "valor_tasa, id_oficina, id_cooperativa, servidor_pruebas," + Environment.NewLine;
                sSql += "servidor_produccion, webservice_tasa_credenciales," + Environment.NewLine;
                sSql += "webservice_tasa_usuario, webservice_tasa_anulacion, permite_anular_tasa," + Environment.NewLine;
                sSql += "webservice_verifica_token, webservice_tasa_lote, webservice_detalle_transacciones," + Environment.NewLine;
                sSql += "webservice_tasa_notificacion, notificacion_emergente, webservice_get_tokens," + Environment.NewLine;
                sSql += "adjuntar_tasa_boleto" + Environment.NewLine;
                sSql += "from ctt_tasa_parametros" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["idParametroTasa"] = dtConsulta.Rows[0]["id_ctt_tasa_parametro"].ToString();
                        cmbTerminal.SelectedValue = dtConsulta.Rows[0]["id_ctt_tasa_terminal"].ToString();
                        cmbAmbiente.SelectedValue = dtConsulta.Rows[0]["emision"].ToString();
                        txtValorTasa.Text = Convert.ToDecimal(dtConsulta.Rows[0]["valor_tasa"].ToString()).ToString("N2");
                        txtIdOficina.Text = dtConsulta.Rows[0]["id_oficina"].ToString();
                        txtIdCooperativa.Text = dtConsulta.Rows[0]["id_cooperativa"].ToString();
                        txtWsPruebas.Text = dtConsulta.Rows[0]["servidor_pruebas"].ToString();
                        txtWsProduccion.Text = dtConsulta.Rows[0]["servidor_produccion"].ToString();
                        txtUrlCredendiales.Text = dtConsulta.Rows[0]["webservice_tasa_credenciales"].ToString();
                        txtUrlEmisionTasaUsuario.Text = dtConsulta.Rows[0]["webservice_tasa_usuario"].ToString();
                        txtUrlAnulacionTasaUsuario.Text = dtConsulta.Rows[0]["webservice_tasa_anulacion"].ToString();

                        txtUrlVerificarToken.Text = dtConsulta.Rows[0]["webservice_verifica_token"].ToString();
                        txtUrlEmisionLote.Text = dtConsulta.Rows[0]["webservice_tasa_lote"].ToString();
                        txtUrlDetalleTransacciones.Text = dtConsulta.Rows[0]["webservice_detalle_transacciones"].ToString();
                        txtUrlEnviarNotificacion.Text = dtConsulta.Rows[0]["webservice_tasa_notificacion"].ToString();
                        txtUrlGetTokens.Text = dtConsulta.Rows[0]["webservice_get_tokens"].ToString();

                        if (Convert.ToInt32(dtConsulta.Rows[0]["permite_anular_tasa"].ToString()) == 0)
                        {
                            chkAnularTasa.Checked = false;
                        }

                        else
                        {
                            chkAnularTasa.Checked = true;
                        }

                        if (Convert.ToInt32(dtConsulta.Rows[0]["notificacion_emergente"].ToString()) == 0)
                        {
                            chkNotificacionEmergente.Checked = false;
                        }

                        else
                        {
                            chkNotificacionEmergente.Checked = true;
                        }

                        if (Convert.ToInt32(dtConsulta.Rows[0]["adjuntar_tasa_boleto"].ToString()) == 0)
                        {
                            chkImprimirTasaUsuario.Checked = false;
                        }

                        else
                        {
                            chkImprimirTasaUsuario.Checked = true;
                        }

                        pnlRegistro.Enabled = false;
                        btnGuardar.Text = "Editar";
                    }

                    else
                    {
                        Session["idParametroTasa"] = null;
                        cmbTerminal.SelectedIndex = 0;
                        cmbAmbiente.SelectedIndex = 0;
                        txtValorTasa.Text = "0.00";
                        txtIdOficina.Text = "";
                        txtIdCooperativa.Text = "";
                        txtWsPruebas.Text = "";
                        txtWsProduccion.Text = "";
                        txtUrlCredendiales.Text = "";
                        txtUrlEmisionTasaUsuario.Text = "";
                        txtUrlAnulacionTasaUsuario.Text = "";
                        txtUrlVerificarToken.Text = "";
                        txtUrlEmisionLote.Text = "";
                        txtUrlDetalleTransacciones.Text = "";
                        txtUrlEnviarNotificacion.Text = "";
                        chkAnularTasa.Checked = false;
                        chkNotificacionEmergente.Checked = false;
                        chkImprimirTasaUsuario.Checked = false;
                        btnGuardar.Text = "Guardar";
                        pnlRegistro.Enabled = true;
                        cmbTerminal.Focus();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private void insertarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sSql = "";
                sSql += "insert into ctt_tasa_parametros (" + Environment.NewLine;
                sSql += "id_ctt_tasa_terminal, id_oficina, id_cooperativa, servidor_pruebas," + Environment.NewLine;
                sSql += "servidor_produccion, webservice_tasa_credenciales, webservice_tasa_usuario," + Environment.NewLine;
                sSql += "webservice_tasa_anulacion, emision, valor_tasa, permite_anular_tasa," + Environment.NewLine;
                sSql += "webservice_verifica_token, webservice_tasa_lote, webservice_detalle_transacciones," + Environment.NewLine;
                sSql += "webservice_tasa_notificacion, notificacion_emergente, webservice_get_tokens," + Environment.NewLine;
                sSql += "adjuntar_tasa_boleto, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(cmbTerminal.SelectedValue) + ", " + Convert.ToInt32(txtIdOficina.Text.Trim()) + ", ";
                sSql += Convert.ToInt32(txtIdCooperativa.Text.Trim()) + ", '" + txtWsPruebas.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtWsProduccion.Text.Trim() + "', '" + txtUrlCredendiales.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtUrlEmisionTasaUsuario.Text.Trim() + "', '" + txtUrlAnulacionTasaUsuario.Text.Trim() + "'," + Environment.NewLine;
                sSql += Convert.ToInt32(cmbAmbiente.SelectedValue) + ", " + Convert.ToDecimal(txtValorTasa.Text.Trim()) + ", " + iPermitir + "," + Environment.NewLine;
                sSql += "'" + txtUrlVerificarToken.Text.Trim() + "', '" + txtUrlEmisionLote.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtUrlDetalleTransacciones.Text.Trim() + "', '" + txtUrlEnviarNotificacion.Text.Trim() + "'," + Environment.NewLine;
                sSql += iNotificacionEmergente + ", '" + txtUrlGetTokens.Text.Trim() + "', " + iImprimirTasaUsuario + "," + Environment.NewLine;
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro ingresado correctamente', 'success');", true);
                cargarParametros();
                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); };
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
                sSql += "update ctt_tasa_parametros set" + Environment.NewLine;
                sSql += "id_ctt_tasa_terminal = " + Convert.ToInt32(cmbTerminal.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_oficina = " + Convert.ToInt32(txtIdOficina.Text.Trim()) + "," + Environment.NewLine;
                sSql += "id_cooperativa = " + Convert.ToInt32(txtIdCooperativa.Text.Trim()) + "," + Environment.NewLine;
                sSql += "servidor_pruebas = '" + txtWsPruebas.Text.Trim() + "'," + Environment.NewLine;
                sSql += "servidor_produccion = '" + txtWsProduccion.Text.Trim() + "'," + Environment.NewLine;
                sSql += "webservice_tasa_credenciales = '" + txtUrlCredendiales.Text.Trim() + "'," + Environment.NewLine;
                sSql += "webservice_tasa_usuario = '" + txtUrlEmisionTasaUsuario.Text.Trim() + "'," + Environment.NewLine;
                sSql += "webservice_tasa_anulacion = '" + txtUrlAnulacionTasaUsuario.Text.Trim() + "'," + Environment.NewLine;
                sSql += "webservice_verifica_token = '" + txtUrlVerificarToken.Text.Trim() + "'," + Environment.NewLine;
                sSql += "webservice_tasa_lote = '" + txtUrlEmisionLote.Text.Trim() + "'," + Environment.NewLine;
                sSql += "webservice_detalle_transacciones = '" + txtUrlDetalleTransacciones.Text.Trim() + "'," + Environment.NewLine;
                sSql += "webservice_tasa_notificacion = '" + txtUrlEnviarNotificacion.Text.Trim() + "'," + Environment.NewLine;
                sSql += "emision = " + Convert.ToInt32(cmbAmbiente.SelectedValue) + "," + Environment.NewLine;
                sSql += "valor_tasa = " + Convert.ToDecimal(txtValorTasa.Text.Trim()) + "," + Environment.NewLine;
                sSql += "permite_anular_tasa = " + iPermitir + "," + Environment.NewLine;
                sSql += "notificacion_emergente = " + iNotificacionEmergente + "," + Environment.NewLine;
                sSql += "webservice_get_tokens = '" + txtUrlGetTokens.Text.Trim() + "'," + Environment.NewLine;
                sSql += "adjuntar_tasa_boleto = " + iImprimirTasaUsuario + Environment.NewLine;
                sSql += "where id_ctt_tasa_parametro = " + Convert.ToInt32(Session["idParametroTasa"]) + Environment.NewLine;
                sSql += "and estado = 'A'";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro actualizado correctamente', 'success');", true);
                cargarParametros();
                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); };
        }


        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnGuardar.Text == "Editar")
                {
                    pnlRegistro.Enabled = true;
                    cmbTerminal.Focus();
                    btnGuardar.Text = "Guardar";
                }

                else
                {
                    if (txtValorTasa.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtValorTasa.Focus();
                    }

                    else if (txtIdOficina.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtIdOficina.Focus();
                    }

                    else if (txtIdCooperativa.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtIdCooperativa.Focus();
                    }

                    else if (txtWsPruebas.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtWsPruebas.Focus();
                    }

                    else if (txtWsProduccion.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtWsProduccion.Focus();
                    }

                    //else if (txtUrlCredendiales.Text.Trim() == "")
                    //{
                    //    MsjValidarCampos.Visible = true;
                    //    txtUrlCredendiales.Focus();
                    //}

                    else if (txtUrlEmisionTasaUsuario.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtUrlEmisionTasaUsuario.Focus();
                    }

                    else if (txtUrlAnulacionTasaUsuario.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtUrlAnulacionTasaUsuario.Focus();
                    }

                    else if (txtUrlVerificarToken.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtUrlVerificarToken.Focus();
                    }

                    else if (txtUrlEmisionLote.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtUrlEmisionLote.Focus();
                    }

                    else if (txtUrlDetalleTransacciones.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtUrlDetalleTransacciones.Focus();
                    }

                    else if (txtUrlEnviarNotificacion.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtUrlEnviarNotificacion.Focus();
                    }

                    else
                    {
                        if (chkAnularTasa.Checked == true)
                        {
                            iPermitir = 1;
                        }

                        else
                        {
                            iPermitir = 0;
                        }

                        if (chkNotificacionEmergente.Checked == true)
                        {
                            iNotificacionEmergente = 1;
                        }

                        else
                        {
                            iNotificacionEmergente = 0;
                        }

                        if (chkImprimirTasaUsuario.Checked == true)
                        {
                            iImprimirTasaUsuario = 1;
                        }

                        else
                        {
                            iImprimirTasaUsuario = 0;
                        }

                        if (Session["idParametroTasa"] == null)
                        {
                            //ENVIO A FUNCION DE INSERCION
                            insertarRegistro();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            cargarParametros();
        }
    }
}