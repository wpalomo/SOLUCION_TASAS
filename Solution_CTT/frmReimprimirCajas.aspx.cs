using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NEGOCIO;
using ENTIDADES;
using System.Drawing;
using System.Data;

namespace Solution_CTT
{
    public partial class frmReimprimirCajas : System.Web.UI.Page
    {
        ENTReporteViajesActivos reporteViajesE = new ENTReporteViajesActivos();
        ENTComboDatos comboE = new ENTComboDatos();

        manejadorConexion conexionM = new manejadorConexion();
        manejadorReporteViajesActivos reporteViajesM = new manejadorReporteViajesActivos();
        manejadorComboDatos comboM = new manejadorComboDatos();

        Clases.ClaseCierreBoleteria_2 reporte;

        string sSql;
        string sFechaInicial;
        string sFechaFinal;
        string sAccion;

        bool bRespuesta;

        DataTable dtConsulta;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
                return;
            }

            Session["modulo"] = "MÓDULO DE CONSULTA DE VIAJES ACTIVOS";

            if (!IsPostBack)
            {
                limpiar();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMBOBOX DE USUARIOS
        private void llenarComboUsuarios()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_oficinista, descripcion" + Environment.NewLine;
                sSql += "from ctt_oficinista" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbUsuarios.DataSource = comboM.listarCombo(comboE);
                cmbUsuarios.DataValueField = "IID";
                cmbUsuarios.DataTextField = "IDATO";
                cmbUsuarios.DataBind();
                cmbUsuarios.Items.Insert(0, new ListItem("Todos", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE USUARIOS
        private void llenarComboBoleterias()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_pueblo, descripcion" + Environment.NewLine;
                sSql += "from ctt_pueblos" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and terminal = 1" + Environment.NewLine;
                sSql += "order by descripcion";

                comboE.ISSQL = sSql;
                cmbBoleteria.DataSource = comboM.listarCombo(comboE);
                cmbBoleteria.DataValueField = "IID";
                cmbBoleteria.DataTextField = "IDATO";
                cmbBoleteria.DataBind();
                cmbBoleteria.Items.Insert(0, new ListItem("Todos", "0"));
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
            txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");

            llenarComboUsuarios();
            llenarComboBoleterias();
            dtConsulta = new DataTable();
            dtConsulta.Clear();
            dgvDatos.DataSource = dtConsulta;
            dgvDatos.DataBind();
            Session["fecha_desde"] = null;
            Session["fecha_hasta"] = null;
            Session["instruccion"] = null;
            txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid()
        {
            try
            {
                sFechaInicial = Convert.ToDateTime(txtFechaDesde.Text.Trim()).ToString("yyyy/MM/dd");
                sFechaFinal = Convert.ToDateTime(txtFechaHasta.Text.Trim()).ToString("yyyy/MM/dd");

                sSql = "";
                sSql += "select id_ctt_cierre_caja, convert(varchar(10), fecha_apertura, 103) fecha_apertura," + Environment.NewLine;
                sSql += "hora_apertura, estado_cierre_caja, oficinista, jornada" + Environment.NewLine;
                sSql += "from ctt_vw_reabrir_ultima_caja" + Environment.NewLine;
                //sSql += "where id_ctt_pueblo = " + Session["id_pueblo"].ToString().Trim() + Environment.NewLine;
                sSql += "where fecha_apertura between '" + sFechaInicial + "'" + Environment.NewLine;
                sSql += "and '" + sFechaFinal + "'" + Environment.NewLine;
                sSql += "and caja_boleteria = 1" + Environment.NewLine;
                sSql += "and caja_encomienda = 0" + Environment.NewLine;

                if (Convert.ToInt32(cmbUsuarios.SelectedValue) != 0)
                {
                    sSql += "and id_ctt_oficinista = " + cmbUsuarios.SelectedValue + Environment.NewLine;
                }

                if (Convert.ToInt32(cmbBoleteria.SelectedValue) != 0)
                {
                    sSql += "and id_ctt_pueblo = " + cmbBoleteria.SelectedValue + Environment.NewLine;
                }

                sSql += "order by fecha_apertura desc";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                dgvDatos.Columns[0].Visible = true;
                dgvDatos.DataSource = dtConsulta;
                dgvDatos.DataBind();
                dgvDatos.Columns[0].Visible = false;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;

                dgvDatos.Columns[0].Visible = true;
                int iIdCierreCaja = Convert.ToInt32(dgvDatos.Rows[a].Cells[0].Text);
                dgvDatos.Columns[0].Visible = false;

                if (sAccion == "I")
                {
                    reporte = new Clases.ClaseCierreBoleteria_2();
                    reporte.llenarReporte(0, iIdCierreCaja);
                }

                else if (sAccion == "R")
                {
                    reporte = new Clases.ClaseCierreBoleteria_2();
                    reporte.llenarReporte(2, iIdCierreCaja);
                }
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

        protected void lbtnImprimir_Click(object sender, EventArgs e)
        {
            sAccion = "I";
        }

        protected void lbtnReimprimir_Click(object sender, EventArgs e)
        {
            sAccion = "R";
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
                llenarGrid();
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

    }
}