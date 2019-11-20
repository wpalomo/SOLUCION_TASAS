using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ENTIDADES;
using NEGOCIO;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmLocalidadesImpresoras : System.Web.UI.Page
    {
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string[] sDatosMaximo = new string[5];
        string sAccion;
        string sAccionFiltro;
        string sEstado;

        int iComprobanteElectronico;

        DataTable dtConsulta;
        bool bRespuesta;

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

            Session["modulo"] = "MÓDULO DE OFICINAS";

            if (!IsPostBack)
            {
                limpiar();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMOBOX DE LOCALIDADES
        private void llenarComboLocalidades()
        {
            try
            {
                sSql = "";
                sSql += "select id_localidad, nombre_localidad" + Environment.NewLine;
                sSql += "from tp_vw_localidades";

                comboE.ISSQL = sSql;
                cmbLocalidadTerminal.DataSource = comboM.listarCombo(comboE);
                cmbLocalidadTerminal.DataValueField = "IID";
                cmbLocalidadTerminal.DataTextField = "IDATO";
                cmbLocalidadTerminal.DataBind();
                cmbLocalidadTerminal.Items.Insert(0, new ListItem("Seleccione Localidad", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LAS COLUMNAS
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[1].Visible = ok;
            //dgvDatos.Columns[2].Visible = ok;
            dgvDatos.Columns[3].Visible = ok;
            //dgvDatos.Columns[4].Visible = ok;
            dgvDatos.Columns[5].Visible = ok;
            dgvDatos.Columns[6].Visible = ok;
            dgvDatos.Columns[7].Visible = ok;
            dgvDatos.Columns[8].Visible = ok;
            dgvDatos.Columns[9].Visible = ok;
            dgvDatos.Columns[10].Visible = ok;
            dgvDatos.Columns[11].Visible = ok;
            dgvDatos.Columns[12].Visible = ok;
            dgvDatos.Columns[13].Visible = ok;
            dgvDatos.Columns[14].Visible = ok;
            dgvDatos.Columns[15].Visible = ok;
            dgvDatos.Columns[16].Visible = ok;
            dgvDatos.Columns[17].Visible = ok;
            dgvDatos.Columns[18].Visible = ok;
            dgvDatos.Columns[19].Visible = ok;
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid()
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_localidades_impresoras" + Environment.NewLine;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                columnasGrid(true);

                dgvDatos.DataSource = dtConsulta;
                dgvDatos.DataBind();

                columnasGrid(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONSULTAR SI EXISTE EL REGISTRO A INSERTAR
        private int contarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and id_localidad = " + cmbLocalidadTerminal.SelectedValue;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return -1;
                }

                return Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return -1;
            }
        }

        //FUNCION PARA INSERTAR EN LA BASE DE DATOSf
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
                sSql += "insert into tp_localidades_impresoras (" + Environment.NewLine;
                sSql += "id_localidad, id_localidad_secuencia, nombre_impresora, puerto_impresora, numero_cotizacion," + Environment.NewLine;
                sSql += "numero_pedido, numero_factura, numero_nota_credito, numero_nota_debito," + Environment.NewLine;
                sSql += "numero_guia_remision, numero_pago, numeroanticipocliente, numeropagoserieb," + Environment.NewLine;
                sSql += "numeronotaventa, numeronotaentrega, numeromovimientocaja, numeroencomienda," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingeso, terminal_ingreso, numero_replica_trigger," + Environment.NewLine;
                sSql += "numero_control_replica )" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += cmbLocalidadTerminal.SelectedValue + ", 0, '" + txtNombreImpresora.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtPuertoImpresora.Text.Trim() + "', " + txtCotizacion.Text.Trim() + ", " + txtPedido.Text.Trim() + ", ";
                sSql += txtFactura.Text.Trim() + ", " + txtNotacredito.Text.Trim() + ", " + txtNotaDebito.Text.Trim() + "," + Environment.NewLine;
                sSql += txtGuiaRemision.Text.Trim() + ", " + txtPago.Text.Trim() + ", " + txtAnticipoCliente.Text.Trim() + ", ";
                sSql += txtPagoSerieB.Text.Trim() + ", " + txtNotaVenta.Text.Trim() + ", " + txtNotaEntrega.Text.Trim() + "," + Environment.NewLine;
                sSql += txtMovimientoCaja.Text.Trim() + ", " + txtEncomienda.Text.Trim() + ", ";
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 0, 0)";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro ingresado correctamente', 'success');", true);
                limpiar();
                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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

                //INSTRUCCION SQL PARA ACTUALIZAR
                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "nombre_impresora = '" + txtNombreImpresora.Text.Trim() + "'," + Environment.NewLine;
                sSql += "puerto_impresora = '" + txtPuertoImpresora.Text.Trim() + "'," + Environment.NewLine;
                sSql += "numero_cotizacion = " + txtCotizacion.Text.Trim() + "," + Environment.NewLine;
                sSql += "numero_pedido = " + txtCotizacion.Text.Trim() + "," + Environment.NewLine;
                sSql += "numero_factura = " + txtCotizacion.Text.Trim() + "," + Environment.NewLine;
                sSql += "numero_nota_credito = " + txtCotizacion.Text.Trim() + "," + Environment.NewLine;
                sSql += "numero_nota_debito = " + txtCotizacion.Text.Trim() + "," + Environment.NewLine;
                sSql += "numero_guia_remision = " + txtCotizacion.Text.Trim() + "," + Environment.NewLine;
                sSql += "numero_pago = " + txtCotizacion.Text.Trim() + "," + Environment.NewLine;
                sSql += "numeroanticipocliente = " + txtCotizacion.Text.Trim() + "," + Environment.NewLine;
                sSql += "numeropagoserieb = " + txtCotizacion.Text.Trim() + "," + Environment.NewLine;
                sSql += "numeronotaventa = " + txtCotizacion.Text.Trim() + "," + Environment.NewLine;
                sSql += "numeronotaentrega = " + txtCotizacion.Text.Trim() + "," + Environment.NewLine;
                sSql += "numeromovimientocaja = " + txtCotizacion.Text.Trim() + "," + Environment.NewLine;
                sSql += "numeroencomienda = " + txtCotizacion.Text.Trim() + "," + Environment.NewLine;
                sSql += "estado = '" + sEstado + "'" + Environment.NewLine;
                sSql += "where id_localidad_impresora = " + Session["idRegistroLIM"].ToString();

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro actualizado correctamente', 'success');", true);
                limpiar();
                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };
        }

        //FUNCION PARA ELIMINAR EN LA BASE DE DATOS
        private void eliminarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_localidad_impresora = " + Session["idRegistroLIM"].ToString();

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro eliminado correctamente', 'success');", true);
                limpiar();
                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            llenarComboLocalidades();
            cmbEstado.SelectedIndex = 0;
            cmbLocalidadTerminal.Enabled = true;

            txtNombreImpresora.Text = "";
            txtPuertoImpresora.Text = "";
            txtCotizacion.Text = "";
            txtPedido.Text = "";
            txtFactura.Text = "";
            txtNotacredito.Text = "";
            txtNotaDebito.Text = "";
            txtGuiaRemision.Text = "";
            txtPago.Text = "";
            txtAnticipoCliente.Text = "";
            txtPagoSerieB.Text = "";
            txtNotaVenta.Text = "";
            txtNotaEntrega.Text = "";
            txtMovimientoCaja.Text = "";
            txtEncomienda.Text = "";

            btnSave.Text = "Crear";

            Session["idRegistroLIM"] = null;

            llenarGrid();
        }

        #endregion

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);
                Session["idRegistroLIM"] = dgvDatos.Rows[a].Cells[0].Text;

                if (sAccion == "Editar")
                {
                    cmbLocalidadTerminal.SelectedValue = dgvDatos.Rows[a].Cells[1].Text;
                    txtNombreImpresora.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[3].Text);
                    txtPuertoImpresora.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[5].Text);
                    txtCotizacion.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[6].Text);
                    txtPedido.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[7].Text);
                    txtFactura.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[8].Text);
                    txtNotacredito.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[9].Text);
                    txtNotaDebito.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[10].Text);
                    txtGuiaRemision.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[11].Text);
                    txtPago.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[12].Text);
                    txtAnticipoCliente.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[13].Text);
                    txtPagoSerieB.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[14].Text);
                    txtNotaVenta.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[15].Text);
                    txtNotaEntrega.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[16].Text);
                    txtMovimientoCaja.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[17].Text);
                    txtEncomienda.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[18].Text);
                    cmbEstado.SelectedValue = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[19].Text);

                    cmbLocalidadTerminal.Enabled = false;
                }

                columnasGrid(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvDatos.PageIndex = e.NewPageIndex;
                llenarGrid();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            sAccion = "Editar";
            btnSave.Text = "Editar";
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            sAccion = "Eliminar";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbLocalidadTerminal.SelectedValue) == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione la localidad.', 'info');", true);
                cmbLocalidadTerminal.Focus();
                return;
            }

            if (txtNombreImpresora.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el nombre de la impresora.', 'info');", true);
                txtNombreImpresora.Focus();
                return;
            }

            if (txtPuertoImpresora.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el puerto de la impresora.', 'info');", true);
                txtPuertoImpresora.Focus();
                return;
            }

            if (txtCotizacion.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número inicial de cotización.', 'info');", true);
                txtCotizacion.Focus();
                return;
            }

            if (txtPedido.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número inicial de pedido.', 'info');", true);
                txtPedido.Focus();
                return;
            }

            if (txtFactura.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número inicial de factura.', 'info');", true);
                txtFactura.Focus();
                return;
            }

            if (txtNotacredito.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número inicial de nota de crédito.', 'info');", true);
                txtNotacredito.Focus();
                return;
            }

            if (txtNotaDebito.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número inicial de nota de débito.', 'info');", true);
                txtNotaDebito.Focus();
                return;
            }

            if (txtGuiaRemision.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número inicial de guía de remisión.', 'info');", true);
                txtGuiaRemision.Focus();
                return;
            }

            if (txtPago.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número inicial de pago.', 'info');", true);
                txtPago.Focus();
                return;
            }

            if (txtAnticipoCliente.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número inicial de anticipo de cliente.', 'info');", true);
                txtAnticipoCliente.Focus();
                return;
            }

            if (txtPagoSerieB.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número inicial de pago serie B.', 'info');", true);
                txtPagoSerieB.Focus();
                return;
            }

            if (txtNotaVenta.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número inicial de nota de venta.', 'info');", true);
                txtNotaVenta.Focus();
                return;
            }

            if (txtNotaEntrega.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número inicial de nota de entrega.', 'info');", true);
                txtNotaEntrega.Focus();
                return;
            }

            if (txtMovimientoCaja.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número inicial de movimiento de caja.', 'info');", true);
                txtMovimientoCaja.Focus();
                return;
            }

            if (txtEncomienda.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número inicial de encomienda.', 'info');", true);
                txtEncomienda.Focus();
                return;
            }

            else
            {
                if (Session["idRegistroLIM"] == null)
                {
                    int iCuenta = contarRegistro();

                    if (iCuenta == -1)
                    {
                        return;
                    }

                    if (iCuenta > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'La localidad seleccionada ya cuenta con un registro ingresado.', 'info');", true);
                        return;
                    }

                    //ENVIO A FUNCION DE INSERCION
                    insertarRegistro();
                }

                else
                {
                    sEstado = cmbEstado.SelectedValue;
                    actualizarRegistro();
                }
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            eliminarRegistro();
        }

        protected void dgvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < dgvDatos.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvDatos.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvDatos.Rows[i].BackColor = Color.White;
                }
            }
        }
    }
}