using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ENTIDADES;
using NEGOCIO;
using System.Data;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmPagosPendientes : System.Web.UI.Page
    {
        ENTComboDatos comboE = new ENTComboDatos();
        ENTPagosPendientes pendienteE = new ENTPagosPendientes();

        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorConexion conexionM = new manejadorConexion();
        manejadorPagosPendientes pendienteM = new manejadorPagosPendientes();

        Clases.ClaseImpresion imprimir = new Clases.ClaseImpresion();
        Clases.ClaseReportePagoPendiente reportePendiente = new Clases.ClaseReportePagoPendiente();
        Clases.ClaseCobrarPagoPendiente cobrar = new Clases.ClaseCobrarPagoPendiente();

        string sSql;
        string sFecha;
        string sFechaViaje;
        string sHoraViaje;
        string sRuta;
        string sNumeroViaje;
        string sUsuario;
        string sVehiculo;
        string sObservacion;
        string sNombreImpresora;
        string sPathImpresora;
        string sImprimir;
        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;

        bool bRespuesta;

        int iIdPedido;
        int iCortarPapel;
        int iNumeroImpresiones;

        double dbValor;
        double dbValorAdeudado;
        double dbValorAbono;

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

            Session["modulo"] = "MÓDULO DE COBROS DE PAGOS PENDIENTES";

            if (!IsPostBack)
            {
                verificarPermiso();
                sFecha = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaDesde.Text = sFecha;
                txtFechaHasta.Text = sFecha;
                llenarComboVehiculo();
                llenarComboPropietario();
            }
        }


        #region FUNCIONES PARA LA IMPRESION

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

                        imprimir.iniciarImpresion();
                        imprimir.escritoEspaciadoCorto(sImprimir);
                        imprimir.cortarPapel(iCortarPapel);
                        imprimir.imprimirReporte(sPathImpresora);

                        //IMPRIMIR 2 VECES
                        imprimir.imprimirReporte(sPathImpresora);
                    }

                    else
                    {
                        //MENSAJE DE QUE NO HAY IMPRESORA INSTALADA O CONFIGURADA
                        lblMensajeConfirmacion.Text = "No se encuentra una impresora configurada para la localidad";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    }
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCIONES DEL USUARIO

        //CONSULTAR PERMISOS
        private void verificarPermiso()
        {
            try
            {
                if ((Session["ejecuta_cobro_administrativo"] == null) || (Session["ejecuta_cobro_administrativo"].ToString() == "0"))
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
        
        //FUNCION PARA LLENAR EL COMBOBOX DE PROPIETARIOS
        private void llenarComboPropietario()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_vehiculo_propietario, descripcion" + Environment.NewLine;
                sSql += "from ctt_vehiculo_propietario" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbPropietario.DataSource = comboM.listarCombo(comboE);
                cmbPropietario.DataValueField = "IID";
                cmbPropietario.DataTextField = "IDATO";
                cmbPropietario.DataBind();
                cmbPropietario.Items.Insert(0, new ListItem("Todos...!!!", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE VEHICULOS
        private void llenarComboVehiculo()
        {
            try
            {
                sSql = "";
                sSql += "select V.id_ctt_vehiculo, D.descripcion + '-' + V.placa vehiculo" + Environment.NewLine;
                sSql += "from ctt_vehiculo V, ctt_disco D" + Environment.NewLine;
                sSql += "where V.id_ctt_disco = D.id_ctt_disco" + Environment.NewLine;
                sSql += "and V.estado = 'A'" + Environment.NewLine;
                sSql += "and D.estado = 'A'" + Environment.NewLine;
                sSql += "order by D.descripcion";

                comboE.ISSQL = sSql;
                cmbVehiculos.DataSource = comboM.listarCombo(comboE);
                cmbVehiculos.DataValueField = "IID";
                cmbVehiculos.DataTextField = "IDATO";
                cmbVehiculos.DataBind();
                cmbVehiculos.Items.Insert(0, new ListItem("Todos...!!!", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        private void columnasGridPendiente(bool ok)
        {
            dgvDetalle.Columns[0].Visible = ok;
            dgvDetalle.Columns[5].Visible = ok;
            dgvDetalle.Columns[9].Visible = ok;
            dgvDetalle.Columns[10].Visible = ok;
            dgvDetalle.Columns[1].ItemStyle.Width = 0x4b;
            dgvDetalle.Columns[2].ItemStyle.Width = 100;
            dgvDetalle.Columns[3].ItemStyle.Width = 100;
            dgvDetalle.Columns[4].ItemStyle.Width = 150;
            dgvDetalle.Columns[5].ItemStyle.Width = 150;
            dgvDetalle.Columns[6].ItemStyle.Width = 200;
            dgvDetalle.Columns[7].ItemStyle.Width = 100;
            dgvDetalle.Columns[8].ItemStyle.Width = 100;
            dgvDetalle.Columns[11].ItemStyle.Width = 200;
            dgvDetalle.Columns[12].ItemStyle.Width = 100;
        }

        //FUNCION PARA LLENAR EL GRID DE PAGOS PENDIENTES
        private void llenarGridPendientes(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_pagos_pendientes_itinerario" + Environment.NewLine;
                sSql += "where fecha_viaje between '" + Convert.ToDateTime(txtFechaDesde.Text.Trim()).ToString("yyyy-MM-dd") + "'" + Environment.NewLine;
                sSql += "and '" + Convert.ToDateTime(txtFechaHasta.Text.Trim()).ToString("yyyy-MM-dd") + "'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and id_ctt_vehiculo = " + Convert.ToInt32(cmbVehiculos.SelectedValue);
                }

                else if (iOp == 2)
                {
                    sSql += "and id_ctt_vehiculo_propietario = " + Convert.ToInt32(cmbPropietario.SelectedValue);
                }

                else if (iOp == 3)
                {
                    sSql += "and id_ctt_vehiculo = " + Convert.ToInt32(cmbVehiculos.SelectedValue) + Environment.NewLine;
                    sSql += "and id_ctt_vehiculo_propietario = " + Convert.ToInt32(cmbPropietario.SelectedValue);
                }

                sSql += "order by fecha_viaje";

                columnasGridPendiente(true);
                pendienteE.ISQL = sSql;
                dgvDetalle.DataSource = pendienteM.listarPagosPendientes(pendienteE);
                dgvDetalle.DataBind();
                columnasGridPendiente(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA VALIDAR EL GRID
        private void validarGrid()
        {
            try
            {
                if ((Convert.ToInt32(cmbVehiculos.SelectedValue) == 0) && (Convert.ToInt32(cmbPropietario.SelectedValue) == 0))
                {
                    llenarGridPendientes(0);
                }

                else if ((Convert.ToInt32(cmbVehiculos.SelectedValue) != 0) && (Convert.ToInt32(cmbPropietario.SelectedValue) == 0))
                {
                    llenarGridPendientes(1);
                }

                else if ((Convert.ToInt32(cmbVehiculos.SelectedValue) == 0) && (Convert.ToInt32(cmbPropietario.SelectedValue) != 0))
                {
                    llenarGridPendientes(2);
                }

                else if ((Convert.ToInt32(cmbVehiculos.SelectedValue) != 0) && (Convert.ToInt32(cmbPropietario.SelectedValue) != 0))
                {
                    llenarGridPendientes(3);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }        

        #endregion

        #region FUNCIONES PARA ACTUALIZAR LOS PAGOS PENDIENTES

        //FUNCION PARA ACTUALIZAR LOS REGISTROS EN LA BASE DE DATOS
        private void actualizarPagosPendientes()
        {
            try
            {
                if (cobrar.insertarPagoPendiente(Convert.ToInt32(Session["idPedido"].ToString()), Convert.ToInt32(Session["idPersona"].ToString()), Convert.ToDecimal(Session["valorCobrar"].ToString()), Convert.ToDecimal(Session["valorReal"].ToString()), Session["observacion"].ToString(), sDatosMaximo))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Cobros realizados éxitosamente.', 'success');", true);
                    Session["idPedido"] = null;
                    Session["numeroViaje"] = null;
                    Session["fechaViaje"] = null;
                    Session["horaViaje"] = null;
                    Session["vehiculo"] = null;
                    Session["ruta"] = null;
                    Session["valorCobrar"] = null;
                    Session["valorAbono"] = null;
                    Session["valorCobrar"] = null;
                    Session["idPersona"] = null;
                    Session["observacion"] = null;
                    validarGrid();
                }
                else
                {
                    ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "Popup", "swal('Error.!', 'No se pudo realizar el cobro pendiente.', 'error');", true);
                }
            }
            catch (Exception exception)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + exception.ToString();
                ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

        }        


        #endregion


        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmPagosPendientes.aspx");
        }

        protected void dgvDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvDetalle.PageIndex = e.NewPageIndex;

                if ((Convert.ToInt32(cmbVehiculos.SelectedValue) == 0) && (Convert.ToInt32(cmbPropietario.SelectedValue) == 0))
                {
                    llenarGridPendientes(0);
                }

                else if ((Convert.ToInt32(cmbVehiculos.SelectedValue) != 0) && (Convert.ToInt32(cmbPropietario.SelectedValue) == 0))
                {
                    llenarGridPendientes(1);
                }

                else if ((Convert.ToInt32(cmbVehiculos.SelectedValue) == 0) && (Convert.ToInt32(cmbPropietario.SelectedValue) != 0))
                {
                    llenarGridPendientes(2);
                }

                else if ((Convert.ToInt32(cmbVehiculos.SelectedValue) != 0) && (Convert.ToInt32(cmbPropietario.SelectedValue) != 0))
                {
                    llenarGridPendientes(3);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            if (txtFechaDesde.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese una fecha inicial para realizar la búsqueda.', 'warning');", true);
            }

            else if (txtFechaHasta.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese una fecha final para realizar la búsqueda.', 'warning');", true);
            }

            else if (Convert.ToDateTime(txtFechaDesde.Text.Trim()) > Convert.ToDateTime(txtFechaHasta.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'La fecha inicial no puede ser superior a la fecha final.', 'warning');", true);
            }

            else
            {
                validarGrid();
            }
        }

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            
        }

        protected void dgvDetalle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDetalle.SelectedIndex;

                columnasGridPendiente(true);
                Session["idPedido"] = dgvDetalle.Rows[a].Cells[0].Text;
                Session["numeroViaje"] = dgvDetalle.Rows[a].Cells[1].Text;                
                Session["fechaViaje"] = dgvDetalle.Rows[a].Cells[2].Text;
                Session["horaViaje"] = dgvDetalle.Rows[a].Cells[3].Text;                
                Session["vehiculo"] = dgvDetalle.Rows[a].Cells[4].Text;
                Session["ruta"] = dgvDetalle.Rows[a].Cells[6].Text;
                Session["valorAbono"] = dgvDetalle.Rows[a].Cells[7].Text;
                Session["valorCobrar"] = dgvDetalle.Rows[a].Cells[8].Text;
                Session["idPersona"] = this.dgvDetalle.Rows[a].Cells[9].Text;
                Session["ValorReal"] = this.dgvDetalle.Rows[a].Cells[10].Text;

                TextBox txtObservacion = dgvDetalle.Rows[a].FindControl("txtObservacion") as TextBox;
                Session["observacion"] = txtObservacion.Text.Trim().ToUpper();

                columnasGridPendiente(false);

                lblMensajeConfirmacion.Text = "¿Desea ingresar el pago pendiente en el sistema?";
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalConfirmacion').modal('show');</script>", false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            actualizarPagosPendientes();
        }

        protected void dgvDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < dgvDetalle.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvDetalle.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvDetalle.Rows[i].BackColor = Color.White;
                }
            }
        }
    }
}