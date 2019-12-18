using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NEGOCIO;
using ENTIDADES;

namespace Solution_CTT
{
    public partial class frmParametrosContifico : System.Web.UI.Page
    {
        ENTComboDatos comboE = new ENTComboDatos();

        manejadorConexion conexionM = new manejadorConexion();
        manejadorComboDatos comboM = new manejadorComboDatos();

        string sSql;
        string[] sDatosMaximo = new string[5];

        bool bRespuesta;

        DataTable dtConsulta;

        int iTiempoRespuesta;

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

            Session["modulo"] = "MÓDULO DE PARAMETRIZACIÓN DE TASAS DE USUARIO - CONTÍFICO";

            if (!IsPostBack)
            {
                llenarComboProveedor();
                consultarRegistro();
            }
        }

        #region FUNCIONES DEL USUARIO

        //CONSULTAR PERMISOS
        private void verificarPermiso()
        {
            try
            {
                if ((Session["tasaDevesofft"] == null) || (Session["tasaDevesofft"].ToString() == "0"))
                {
                    Response.Redirect("frmMensajePermisos.aspx");
                }

                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE PROVEEDORES
        private void llenarComboProveedor()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_proveedor_tasa, descripcion" + Environment.NewLine;
                sSql += "from ctt_proveedores_tasas" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and codigo = '02'";

                comboE.ISSQL = sSql;
                cmbProveedor.DataSource = comboM.listarCombo(comboE);
                cmbProveedor.DataValueField = "IID";
                cmbProveedor.DataTextField = "IDATO";
                cmbProveedor.DataBind();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONSULTAR EL REGISTRO
        private void consultarRegistro()
        {
            try
            {
                MsjValidarCampos.Visible = false;

                sSql = "";
                sSql += "select * from ctt_vw_parametros_contifico" + Environment.NewLine;
                sSql += "where codigo = '02'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    Session["idRegistro_CONTIFICO"] = null;
                    pnlRegistro.Enabled = true;
                    btnGuardar.Text = "Guardar";

                    txtUrlAutenticacion.Text = "";
                    txtUrlLocalidades.Text = "";
                    txtUrlConductores.Text = "";
                    txtUrlFrecuencias.Text = "";
                    txtUrlBuses.Text = "";
                    txtUrlRutas.Text = "";
                    txtUrlVentas.Text = "";
                    txtUrlViajes.Text = "";
                    txtUrlCambiarBus.Text = "";
                    txtUrlAnularAsiento.Text = "";
                    txtTiempoRespuesta.Text = "";
                    txtUrlAutenticacion.Focus();
                }

                else
                {
                    Session["idRegistro_CONTIFICO"] = dtConsulta.Rows[0]["id_ctt_tasa_parametro"].ToString();
                    txtUrlAutenticacion.Text = dtConsulta.Rows[0]["api_autenticacion_contifico"].ToString();
                    txtUrlLocalidades.Text = dtConsulta.Rows[0]["api_localidades_contifico"].ToString();
                    txtUrlConductores.Text = dtConsulta.Rows[0]["api_conductores_contifico"].ToString();
                    txtUrlFrecuencias.Text = dtConsulta.Rows[0]["api_frecuencias_contifico"].ToString();
                    txtUrlBuses.Text = dtConsulta.Rows[0]["api_buses_contifico"].ToString();
                    txtUrlRutas.Text = dtConsulta.Rows[0]["api_rutas_contifico"].ToString();
                    txtUrlVentas.Text = dtConsulta.Rows[0]["api_ventas_contifico"].ToString();
                    txtUrlViajes.Text = dtConsulta.Rows[0]["api_viajes_contifico"].ToString();
                    txtUrlCambiarBus.Text = dtConsulta.Rows[0]["api_cambiar_bus_contifico"].ToString();
                    txtUrlAnularAsiento.Text = dtConsulta.Rows[0]["api_anular_asiento_contifico"].ToString();
                    txtTiempoRespuesta.Text = (Convert.ToInt32(dtConsulta.Rows[0]["timeout"].ToString()) / 1000).ToString();
                    cmbProveedor.SelectedValue = dtConsulta.Rows[0]["id_ctt_proveedor_tasa"].ToString();

                    pnlRegistro.Enabled = false;
                    btnGuardar.Text = "Editar";
                    txtUrlAutenticacion.Focus();
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            Session["idRegistro_CONTIFICO"] = null;
            txtUrlAutenticacion.Text = "";
            txtUrlLocalidades.Text = "";
            txtUrlConductores.Text = "";
            txtUrlFrecuencias.Text = "";
            txtUrlBuses.Text = "";
            txtUrlRutas.Text = "";
            txtUrlVentas.Text = "";
            txtUrlViajes.Text = "";
            txtUrlCambiarBus.Text = "";
            txtUrlAnularAsiento.Text = "";
            txtTiempoRespuesta.Text = "";

            MsjValidarCampos.Visible = false;
            pnlRegistro.Enabled = false;
            btnGuardar.Text = "Guardar";

            txtUrlAutenticacion.Focus();
        }

        //FUNCION PARA GUARDAR EL REGISTRO
        private void insertarRegistro()
        {
            try
            {
                if (!conexionM.iniciarTransaccion())
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                iTiempoRespuesta = Convert.ToInt32(txtTiempoRespuesta.Text.Trim()) * 1000;

                sSql = "";
                sSql += "insert into ctt_tasa_parametros (" + Environment.NewLine;
                sSql += "api_autenticacion_contifico, api_localidades_contifico, api_conductores_contifico," + Environment.NewLine;
                sSql += "api_frecuencias_contifico, api_buses_contifico, api_rutas_contifico," + Environment.NewLine;
                sSql += "api_ventas_contifico, api_viajes_contifico, api_cambiar_bus_contifico," + Environment.NewLine;
                sSql += "api_anular_asiento_contifico, id_ctt_proveedor_tasa, timeout, estado," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "'" + txtUrlAutenticacion.Text.Trim() + "', '" + txtUrlLocalidades.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtUrlConductores.Text.Trim() + "', '" + txtUrlFrecuencias.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtUrlBuses.Text.Trim() + "', '" + txtUrlRutas.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtUrlVentas.Text.Trim() + "', '" + txtUrlViajes.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtUrlCambiarBus.Text.Trim() + "', '" + txtUrlAnularAsiento.Text.Trim() + "'," + Environment.NewLine;
                sSql += cmbProveedor.SelectedValue + ", " + iTiempoRespuesta + ", 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    conexionM.reversaTransaccion();
                    return;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro ingresado correctamente', 'success');", true);
                consultarRegistro();
                return;
            }

            catch (Exception ex)
            {
                conexionM.reversaTransaccion();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA ACTUALIZAR EL REGISTRO
        private void actualizarRegistro()
        {
            try
            {
                if (!conexionM.iniciarTransaccion())
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                iTiempoRespuesta = Convert.ToInt32(txtTiempoRespuesta.Text.Trim()) * 1000;

                sSql = "";
                sSql += "update ctt_tasa_parametros set" + Environment.NewLine;
                sSql += "api_autenticacion_contifico = '" + txtUrlAutenticacion.Text.Trim() + "'," + Environment.NewLine;
                sSql += "api_localidades_contifico = '" + txtUrlLocalidades.Text.Trim() + "'," + Environment.NewLine;
                sSql += "api_conductores_contifico = '" + txtUrlConductores.Text.Trim() + "'," + Environment.NewLine;
                sSql += "api_frecuencias_contifico = '" + txtUrlFrecuencias.Text.Trim() + "'," + Environment.NewLine;
                sSql += "api_buses_contifico = '" + txtUrlBuses.Text.Trim() + "'," + Environment.NewLine;
                sSql += "api_rutas_contifico = '" + txtUrlRutas.Text.Trim() + "'," + Environment.NewLine;
                sSql += "api_ventas_contifico = '" + txtUrlVentas.Text.Trim() + "'," + Environment.NewLine;
                sSql += "api_viajes_contifico = '" + txtUrlViajes.Text.Trim() + "'," + Environment.NewLine;
                sSql += "api_cambiar_bus_contifico = '" + txtUrlCambiarBus.Text.Trim() + "'," + Environment.NewLine;
                sSql += "api_anular_asiento_contifico = '" +  txtUrlAnularAsiento.Text.Trim() + "'," + Environment.NewLine;
                sSql += "timeout = " + iTiempoRespuesta + Environment.NewLine;
                sSql += "where id_ctt_tasa_parametro = " + Session["idRegistro_CONTIFICO"].ToString();

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    conexionM.reversaTransaccion();
                    return;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro actualizado correctamente', 'success');", true);
                consultarRegistro();
                return;
            }

            catch (Exception ex)
            {
                conexionM.reversaTransaccion();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnGuardar.Text == "Editar")
                {
                    pnlRegistro.Enabled = true;
                    txtUrlAutenticacion.Focus();
                    btnGuardar.Text = "Guardar";
                    return;
                }

                if (txtUrlAutenticacion.Text.Trim() == "")
                {
                    MsjValidarCampos.Visible = true;
                    txtUrlAutenticacion.Focus();
                }

                else if (txtUrlLocalidades.Text.Trim() == "")
                {
                    MsjValidarCampos.Visible = true;
                    txtUrlLocalidades.Focus();
                }

                else if (txtUrlConductores.Text.Trim() == "")
                {
                    MsjValidarCampos.Visible = true;
                    txtUrlConductores.Focus();
                }

                else if (txtUrlFrecuencias.Text.Trim() == "")
                {
                    MsjValidarCampos.Visible = true;
                    txtUrlFrecuencias.Focus();
                }

                else if (txtUrlBuses.Text.Trim() == "")
                {
                    MsjValidarCampos.Visible = true;
                    txtUrlBuses.Focus();
                }

                else if (txtUrlRutas.Text.Trim() == "")
                {
                    MsjValidarCampos.Visible = true;
                    txtUrlRutas.Focus();
                }

                else if (txtUrlVentas.Text.Trim() == "")
                {
                    MsjValidarCampos.Visible = true;
                    txtUrlVentas.Focus();
                }

                else if (txtUrlViajes.Text.Trim() == "")
                {
                    MsjValidarCampos.Visible = true;
                    txtUrlViajes.Focus();
                }

                else if (txtUrlCambiarBus.Text.Trim() == "")
                {
                    MsjValidarCampos.Visible = true;
                    txtUrlCambiarBus.Focus();
                }

                else if (txtUrlAnularAsiento.Text.Trim() == "")
                {
                    MsjValidarCampos.Visible = true;
                    txtUrlAnularAsiento.Focus();
                }

                else if (txtTiempoRespuesta.Text.Trim() == "")
                {
                    MsjValidarCampos.Visible = true;
                    txtTiempoRespuesta.Focus();
                }

                else
                {
                    if (Session["idRegistro_CONTIFICO"] == null)
                    {
                        insertarRegistro();
                    }

                    else
                    {
                        actualizarRegistro();
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
            consultarRegistro();
        }
    }
}