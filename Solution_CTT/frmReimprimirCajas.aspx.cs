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

        Clases.ClaseCierreBoleteria_2 reporte = new Clases.ClaseCierreBoleteria_2();

        string sSql;
        string sFechaInicial;
        string sFechaFinal;

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
                sSql += "select id_ctt_oficinista, usuario" + Environment.NewLine;
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

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");

            llenarComboUsuarios();
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
                sSql += "where id_ctt_pueblo = " + Session["id_pueblo"].ToString().Trim() + Environment.NewLine;
                sSql += "and fecha_apertura between '" + sFechaInicial + "'" + Environment.NewLine;
                sSql += "and '" + sFechaFinal + "'" + Environment.NewLine;
                sSql += "order by fecha_apertura";

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

                reporte.llenarReporte(Convert.ToDateTime(dgvDatos.Rows[a].Cells[1].Text).ToString("yyyy-MM-dd"),
                                      Convert.ToInt32(Session["idJornada"].ToString()), dgvDatos.Rows[a].Cells[5].Text.ToUpper(), dgvDatos.Rows[a].Cells[4].Text, Convert.ToInt32(Session["idUsuario"].ToString()), 0, iIdCierreCaja);

                dgvDatos.Columns[0].Visible = false;
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
            //btnSave.Text = "Editar";
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