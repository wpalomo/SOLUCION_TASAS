using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ENTIDADES;
using NEGOCIO;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmReabrirViaje : System.Web.UI.Page
    {
        ENTComboDatos comboE = new ENTComboDatos();
        ENTViajes asignarE = new ENTViajes();
        ENTPasajeros pasajeroE = new ENTPasajeros();
        ENTPagosPendientes pendienteE = new ENTPagosPendientes();
        ENTVendidos vendidoE = new ENTVendidos();
        ENTFacturasItinerario facturaE = new ENTFacturasItinerario();

        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorConexion conexionM = new manejadorConexion();
        manejadorPasajeros pasajeroM = new manejadorPasajeros();
        manejadorAsignarViaje asignarM = new manejadorAsignarViaje();
        manejadorPagosPendientes pendienteM = new manejadorPagosPendientes();
        manejadorFacturasItinerario facturaM = new manejadorFacturasItinerario();

        Clases.ClaseInstruccionesReabrirViaje reabrir = new Clases.ClaseInstruccionesReabrirViaje();


        string sSql;
        string sAccion;
        string sAccionPersonas;
        string sFecha;
        string sTabla;
        string sCampo;

        string[] sDatosMaximo = new string[5];

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

            Session["modulo"] = "MÓDULO PARA REABRIR VIAJES";

            if (!IsPostBack)
            {
                sFecha = DateTime.Now.ToString("dd/MM/yyyy");
                txtDate.Text = sFecha;
                llenarComboOrigenItinerario();
                llenarGrid(sFecha);
                llenarGridExtras(sFecha);
                Session["auxiliar"] = "1";
                Session["idVehiculo"] = null;
                Session["idProgramacion"] = null;
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMBOBOX DE ORIGEN Y FILTRAR EL GRID
        private void llenarComboOrigenItinerario()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_pueblo, descripcion" + Environment.NewLine;
                sSql += "from ctt_pueblos" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and terminal = 1";

                comboE.ISSQL = sSql;
                cmbFiltrarGrid.DataSource = comboM.listarCombo(comboE);
                cmbFiltrarGrid.DataValueField = "IID";
                cmbFiltrarGrid.DataTextField = "IDATO";
                cmbFiltrarGrid.DataBind();

                if (cmbFiltrarGrid.Items.Count > 0)
                {
                    cmbFiltrarGrid.SelectedValue = Session["id_pueblo"].ToString();
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA MANIPULACION DE COLUMNAS
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[0].ItemStyle.Width = 75;
            dgvDatos.Columns[2].ItemStyle.Width = 75;
            dgvDatos.Columns[3].ItemStyle.Width = 130;
            dgvDatos.Columns[4].ItemStyle.Width = 150;
            dgvDatos.Columns[5].ItemStyle.Width = 250;
            dgvDatos.Columns[6].ItemStyle.Width = 100;
            dgvDatos.Columns[7].ItemStyle.Width = 100;
            dgvDatos.Columns[8].ItemStyle.Width = 100;
            dgvDatos.Columns[9].ItemStyle.Width = 75;
            dgvDatos.Columns[15].ItemStyle.Width = 100;

            dgvDatos.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[8].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[9].ItemStyle.HorizontalAlign = HorizontalAlign.Center;


            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[10].Visible = ok;
            dgvDatos.Columns[11].Visible = ok;
            dgvDatos.Columns[12].Visible = ok;
            dgvDatos.Columns[13].Visible = ok;
            dgvDatos.Columns[14].Visible = ok;
            dgvDatos.Columns[15].Visible = ok;
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid(string sFecha_P)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_itinerarios" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(cmbFiltrarGrid.SelectedValue) + Environment.NewLine;
                sSql += "and fecha_viaje = '" + Convert.ToDateTime(sFecha_P).ToString("yyyy/MM/dd") + "'" + Environment.NewLine;
                sSql += "and normal = 1" + Environment.NewLine;
                sSql += "order by hora_salida";


                columnasGrid(true);
                asignarE.ISSQL = sSql;
                dgvDatos.DataSource = asignarM.listarViajes(asignarE);
                dgvDatos.DataBind();
                columnasGrid(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA MANIPULACION DE COLUMNAS EXTRAS
        private void columnasGridExtra(bool ok)
        {
            dgvDatosExtras.Columns[0].ItemStyle.Width = 75;
            dgvDatosExtras.Columns[2].ItemStyle.Width = 75;
            dgvDatosExtras.Columns[3].ItemStyle.Width = 130;
            dgvDatosExtras.Columns[4].ItemStyle.Width = 150;
            dgvDatosExtras.Columns[5].ItemStyle.Width = 250;
            dgvDatosExtras.Columns[6].ItemStyle.Width = 100;
            dgvDatosExtras.Columns[7].ItemStyle.Width = 100;
            dgvDatosExtras.Columns[8].ItemStyle.Width = 100;
            dgvDatosExtras.Columns[9].ItemStyle.Width = 75;
            dgvDatosExtras.Columns[15].ItemStyle.Width = 100;

            dgvDatosExtras.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosExtras.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosExtras.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosExtras.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosExtras.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosExtras.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosExtras.Columns[8].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosExtras.Columns[9].ItemStyle.HorizontalAlign = HorizontalAlign.Center;


            dgvDatosExtras.Columns[1].Visible = ok;
            dgvDatosExtras.Columns[10].Visible = ok;
            dgvDatosExtras.Columns[11].Visible = ok;
            dgvDatosExtras.Columns[12].Visible = ok;
            dgvDatosExtras.Columns[13].Visible = ok;
            dgvDatosExtras.Columns[14].Visible = ok;
            dgvDatosExtras.Columns[15].Visible = ok;
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGridExtras(string sFecha_P)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_itinerarios" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(cmbFiltrarGrid.SelectedValue) + Environment.NewLine;
                sSql += "and fecha_viaje = '" + Convert.ToDateTime(sFecha_P).ToString("yyyy/MM/dd") + "'" + Environment.NewLine;
                sSql += "and extra = 1" + Environment.NewLine;
                sSql += "order by hora_salida";


                columnasGridExtra(true);
                asignarE.ISSQL = sSql;
                dgvDatosExtras.DataSource = asignarM.listarViajes(asignarE);
                dgvDatosExtras.DataBind();
                columnasGridExtra(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        protected void cmbFiltrarGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["idPuebloOrigen"] = cmbFiltrarGrid.SelectedValue;

            if (txtDate.Text.Trim() == "")
            {
                llenarGrid(DateTime.Now.ToString("dd/MM/yyyy"));
                llenarGridExtras(DateTime.Now.ToString("dd/MM/yyyy"));
            }

            else
            {
                llenarGrid(txtDate.Text.Trim());
                llenarGridExtras(txtDate.Text.Trim());
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDate.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Seleccione una fecha para proceder con la búsqueda de registros.', 'danger');", true);
                    txtDate.Focus();
                }

                else
                {
                    sFecha = txtDate.Text.Trim();
                    llenarGrid(sFecha);
                    llenarGridExtras(sFecha);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(Session["idCierreCaja"].ToString()) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Debe registrar la apertura de caja para realizar la venta de boletos.', 'info');", true);
                    return;
                }

                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);

                Session["idRegistro"] = dgvDatos.Rows[a].Cells[1].Text;

                if (dgvDatos.Rows[a].Cells[9].Text == "A")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El viaje se encuentra vigente.', 'warning');", true);
                    Session["idRegistro"] = null;
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
                }

                columnasGrid(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvDatos.PageIndex = e.NewPageIndex;

            if (txtDate.Text.Trim() != "")
            {
                sFecha = txtDate.Text.Trim();
            }

            else
            {
                sFecha = DateTime.Now.ToString("yyyy/MM/dd");
            }

            llenarGrid(sFecha);
        }

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            //btnSave.Text = "Editar";
        }

        protected void dgvDatosExtras_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(Session["idCierreCaja"].ToString()) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Debe registrar la apertura de caja para realizar la venta de boletos.', 'info');", true);
                    return;
                }

                int a = dgvDatosExtras.SelectedIndex;
                columnasGridExtra(true);

                Session["idRegistro"] = dgvDatosExtras.Rows[a].Cells[1].Text;

                string sFecha_P = dgvDatosExtras.Rows[a].Cells[3].Text;

                if (Convert.ToDateTime(sFecha) < DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No puede reabrir un viaje con una fecha anterior a la actual.', 'warning');", true);
                    columnasGridExtra(false);
                    return;
                }
                
                if (dgvDatosExtras.Rows[a].Cells[9].Text == "A")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El viaje se encuentra vigente.', 'warning');", true);
                    Session["idRegistro"] = null;
                }

                else 
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
                }

                columnasGridExtra(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvDatosExtras_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvDatosExtras.PageIndex = e.NewPageIndex;

                if (txtDate.Text.Trim() != "")
                {
                    sFecha = txtDate.Text.Trim();
                }

                else
                {
                    sFecha = DateTime.Now.ToString("yyyy/MM/dd");
                }

                llenarGridExtras(sFecha);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                int iIdRegistro = Convert.ToInt32(Session["idRegistro"].ToString());
                
                if (reabrir.iniciarCierre(iIdRegistro, sDatosMaximo) == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Ocurrió un problema al tratar de reabrir el viaje.', 'danger');", true);
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'El viaje se ha reaperturado satisfactoriamente.', 'success');", true);
                    llenarGrid(DateTime.Now.ToString("yyyy/MM/dd"));
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
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