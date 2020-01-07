using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NEGOCIO;
using ENTIDADES;
using System.Drawing;
using Microsoft.Reporting.WebForms;

namespace Solution_CTT
{
    public partial class frmReporteListaBoletos : System.Web.UI.Page
    {
        ENTComboDatos comboE = new ENTComboDatos();

        manejadorConexion conexionM = new manejadorConexion();
        manejadorComboDatos comboM = new manejadorComboDatos();

        string sSql;
        string sFechaInicial;
        string sFechaFinal;

        DataTable dtConsulta;

        bool bRespuesta;

        int iNumeroRegistros;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
                return;
            }

            Session["modulo"] = "LISTADO DE BOLETOS POR VIAJE";

            if (!IsPostBack)
            {
                limpiar();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMBOBOX DE VEHICULOS
        private void llenarComboVehiculos()
        {
            try
            {
                sSql = "";
                sSql += "select V.id_ctt_vehiculo, D.descripcion + '-' + V.placa vehiculo" + Environment.NewLine;
                sSql += "from ctt_disco D, ctt_vehiculo V" + Environment.NewLine;
                sSql += "where V.id_ctt_disco = D.id_ctt_disco" + Environment.NewLine;
                sSql += "and V.estado = 'A'" + Environment.NewLine;
                sSql += "and D.estado = 'A'" + Environment.NewLine;
                sSql += "order by D.descripcion";

                comboE.ISSQL = sSql;
                cmbVehiculos.DataSource = comboM.listarCombo(comboE);
                cmbVehiculos.DataValueField = "IID";
                cmbVehiculos.DataTextField = "IDATO";
                cmbVehiculos.DataBind();
                cmbVehiculos.Items.Insert(0, new ListItem("Todos", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //LLENAR COMBO LOCALIDAD
        private void llenarComboLocalidad()
        {
            try
            {
                sSql = "";
                sSql += "select id_localidad, nombre_localidad" + Environment.NewLine;
                sSql += "from tp_vw_localidades" + Environment.NewLine;
                sSql += "where id_localidad=" + Application["idLocalidad"].ToString();

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        comboE.ISSQL = sSql;
                        cmbLocalidad.DataSource = comboM.listarCombo(comboE);
                        cmbLocalidad.DataValueField = "IID";
                        cmbLocalidad.DataTextField = "IDATO";
                        cmbLocalidad.DataBind();
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION DE LIMPIAR
        private void limpiar()
        {
            txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblCantidadRegistros.Text = "Registros encontrados: 0";
            llenarComboVehiculos();
            llenarComboLocalidad();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            dgvDatos.DataSource = dtConsulta;
            dgvDatos.DataBind();

            btnImprimir.Visible = false;

            Session["fecha_desdeLB"] = null;
            Session["fecha_hastaLB"] = null;
            Session["instruccionLB"] = null;
            Session["dtConsultaLB"] = null;
            Session["fecha_desde_LB"] = null;
            Session["fecha_hasta_LB"] = null;
        }

        //FUNCION PARA CONSTRUIR LA INSTRUCCION
        private void crearInstruccionSQL()
        {
            try
            {
                sSql = "";
                sSql += "select disco, numero_viaje, fecha, hora, via, destino, cantidad" + Environment.NewLine;
                sSql += "from ctt_vw_listado_boletos_viajes" + Environment.NewLine;
                sSql += "where visualizar = 1" + Environment.NewLine;
                sSql += "and cobro_boletos = 1" + Environment.NewLine;
                sSql += "and id_localidad = " + Convert.ToInt32(cmbLocalidad.SelectedValue) + Environment.NewLine;
                sSql += "and fecha_viaje between '" + sFechaInicial + "'" + Environment.NewLine;
                sSql += "and '" + sFechaFinal + "'" + Environment.NewLine;

                if (Convert.ToInt32(cmbVehiculos.SelectedValue) != 0)
                {
                    sSql += "and id_ctt_disco = " + Convert.ToInt32(cmbVehiculos.SelectedValue);
                }

                sSql += "order by fecha_viaje, hora_salida";

                Session["instruccionLB"] = sSql;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA MOSTRAR LA TABLA DE LA CONSULTA
        private void consultarTabla()
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                dtConsulta = Session["dtConsultaLB"] as DataTable;

                dgvDatos.DataSource = dtConsulta;
                dgvDatos.DataBind();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid()
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(Session["instruccionLB"].ToString(), dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    lblCantidadRegistros.Text = "Registros encontrados: 0";
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No se encuentran registros con los parámetros seleccionados.', 'info');", true);
                    return;
                }

                lblCantidadRegistros.Text = "Registros encontrados: " + dtConsulta.Rows.Count.ToString();
                Session["dtConsultaLB"] = dtConsulta;
                btnImprimir.Visible = true;

                consultarTabla();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA MOSTRAR EL MODAL CON EL REPORTE
        private void visualizarReporte()
        {
            try
            {
                DSReportes ds = new DSReportes();

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                dtConsulta = Session["dtConsultaLB"] as DataTable;

                DataTable dtListado = ds.Tables["dtListaBoletosViaje"];
                dtListado.Clear();

                dtListado = dtConsulta;

                ModalPopupExtender_Reporte.Show();

                rptReporte.ProcessingMode = ProcessingMode.Local;
                rptReporte.LocalReport.ReportPath = Server.MapPath("~/Reportes/rptListadoBoletos.rdlc");
                ReportDataSource datasource = new ReportDataSource("DataSet1", dtListado);

                ReportParameter[] parametros = new ReportParameter[4];
                parametros[0] = new ReportParameter("P_Fecha_Desde", "Fecha desde: " + Convert.ToDateTime(Session["fecha_desde_LB"].ToString()).ToString("dd-MM-yyyy"));
                parametros[1] = new ReportParameter("P_Fecha_Hasta", "Fecha hasta:" + Convert.ToDateTime(Session["fecha_hasta_LB"].ToString()).ToString("dd-MM-yyyy"));
                parametros[2] = new ReportParameter("P_Localidad", "Localidad: " + cmbLocalidad.SelectedItem.ToString());
                parametros[3] = new ReportParameter("P_Usuario_Consulta", "Usuario de consulta: " +  Session["usuario"].ToString().ToLower());

                rptReporte.LocalReport.DataSources.Clear();
                rptReporte.LocalReport.DataSources.Add(datasource);
                rptReporte.LocalReport.SetParameters(parametros);
                rptReporte.LocalReport.Refresh();
            }

            catch (Exception ex)
            {
                ModalPopupExtender_Reporte.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            visualizarReporte();
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            sFechaInicial = Convert.ToDateTime(txtFechaDesde.Text.Trim()).ToString("yyyy-MM-dd");
            sFechaFinal = Convert.ToDateTime(txtFechaHasta.Text.Trim()).ToString("yyyy-MM-dd");

            if (Convert.ToDateTime(sFechaInicial) > Convert.ToDateTime(sFechaFinal))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'La fecha inicial no puede ser superior a la fecha final.', 'info');", true);
                return;
            }

            Session["fecha_desde_LB"] = sFechaInicial;
            Session["fecha_hasta_LB"] = sFechaInicial;

            crearInstruccionSQL();
            llenarGrid();
        }

        protected void btnCerrarModal_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Reporte.Hide();
        }

        protected void btnCerrarModalReporte_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Reporte.Hide();
        }

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvDatos.PageIndex = e.NewPageIndex;
                consultarTabla();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }        
    }
}