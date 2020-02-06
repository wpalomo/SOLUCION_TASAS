using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using NEGOCIO;
using ENTIDADES;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmVentasOtrasLocalidades : System.Web.UI.Page
    {
        ENTReporteViajesActivos reporteViajesE = new ENTReporteViajesActivos();
        ENTComboDatos comboE = new ENTComboDatos();
        ENTViajes asignarE = new ENTViajes();

        manejadorConexion conexionM = new manejadorConexion();
        manejadorReporteViajesActivos reporteViajesM = new manejadorReporteViajesActivos();
        manejadorAsignarViaje asignarM = new manejadorAsignarViaje();
        manejadorComboDatos comboM = new manejadorComboDatos();

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

            Session["modulo"] = "MÓDULO DE VENTAS DE OTRAS LOCALIDADES";

            if (!IsPostBack)
            {
                limpiar();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMBOBOX
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

        //FUNCION PARA LLENAR EL COMBOBOX DE TERMINALES
        private void llenarComboTerminales()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_pueblo, descripcion" + Environment.NewLine;
                sSql += "from ctt_pueblos" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and terminal = 1" + Environment.NewLine;
                sSql += "order by id_ctt_pueblo";

                comboE.ISSQL = sSql;
                cmbTerminal.DataSource = comboM.listarCombo(comboE);
                cmbTerminal.DataValueField = "IID";
                cmbTerminal.DataTextField = "IDATO";
                cmbTerminal.DataBind();
                cmbTerminal.Items.Insert(0, new ListItem("Todos", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid(int iOp)
        {
            try
            {
                sFechaInicial = Convert.ToDateTime(txtFechaDesde.Text.Trim()).ToString("yyyy/MM/dd");
                sFechaFinal = Convert.ToDateTime(txtFechaHasta.Text.Trim()).ToString("yyyy/MM/dd");

                Session["fecha_desde"] = sFechaInicial;
                Session["fecha_hasta"] = sFechaFinal;
                Session["vehiculo"] = cmbVehiculos.SelectedItem.ToString();
                Session["terminal"] = cmbTerminal.SelectedItem.ToString();


                sSql = "";
                sSql += "select * from ctt_vw_itinerarios" + Environment.NewLine;
                sSql += "where (fecha_viaje between '" + sFechaInicial + "'" + Environment.NewLine;
                sSql += "and '" + sFechaFinal + "')" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and id_ctt_vehiculo = " + Convert.ToInt32(cmbVehiculos.SelectedValue) + Environment.NewLine;
                }

                else if (iOp == 2)
                {
                    sSql += "and id_ctt_pueblo = " + Convert.ToInt32(cmbTerminal.SelectedValue) + Environment.NewLine;
                }

                else if (iOp == 3)
                {
                    sSql += "and id_ctt_vehiculo = " + Convert.ToInt32(cmbVehiculos.SelectedValue) + Environment.NewLine;
                    sSql += "and id_ctt_pueblo = " + Convert.ToInt32(cmbTerminal.SelectedValue) + Environment.NewLine;
                }

                sSql += "order by fecha_viaje, hora_salida";

                columnasGrid(true);
                asignarE.ISSQL = sSql;
                dgvDatos.DataSource = asignarM.listarViajes(asignarE);
                dgvDatos.DataBind();
                columnasGrid(false);

                if (dgvDatos.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No existen registros con los datos ingresados para la búsqueda.', 'warning');", true);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }


        //FUNCION PARA MOSTRAR EL MODAL CON EL REPORTE
        private void visualizarReporte(int iIdProgramacion_P)
        {
            try
            {
                DSReportes ds = new DSReportes();

                DataTable dtPasajeros = ds.Tables["RptPasajeros"];
                dtPasajeros.Clear();

                sSql = "";
                sSql += "select * from ctt_vw_reporte_pasajeros" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + iIdProgramacion_P + Environment.NewLine;
                sSql += "order by numero_asiento";

                bRespuesta = conexionM.consultarRegistro(sSql, dtPasajeros);

                if (bRespuesta == true)
                {
                    if (dtPasajeros.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'No existen pasajeros en el viaje.', 'warning');", true);
                    }

                    else
                    {
                        ModalPopupExtender_Reporte.Show();

                        rptReportePasajerosBus.ProcessingMode = ProcessingMode.Local;
                        rptReportePasajerosBus.LocalReport.ReportPath = Server.MapPath("~/Reportes/rptPasajerosLocalidades.rdlc");
                        ReportDataSource datasource = new ReportDataSource("DataSet1", dtPasajeros);

                        rptReportePasajerosBus.LocalReport.DataSources.Clear();
                        rptReportePasajerosBus.LocalReport.DataSources.Add(datasource);
                        rptReportePasajerosBus.LocalReport.Refresh();
                    }
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo cargar el informe.', 'danger');", true);
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
            llenarComboVehiculos();
            llenarComboTerminales();
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

        #endregion

        protected void btnCerrarModal_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Reporte.Hide();
        }

        protected void btnCerrarModalReporte_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Reporte.Hide();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
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
                if ((Convert.ToInt32(cmbVehiculos.SelectedValue) == 0) && (Convert.ToInt32(cmbTerminal.SelectedValue) == 0))
                {
                    llenarGrid(0);
                }

                else if ((Convert.ToInt32(cmbVehiculos.SelectedValue) != 0) && (Convert.ToInt32(cmbTerminal.SelectedValue) == 0))
                {
                    llenarGrid(1);
                }

                else if ((Convert.ToInt32(cmbVehiculos.SelectedValue) == 0) && (Convert.ToInt32(cmbTerminal.SelectedValue) != 0))
                {
                    llenarGrid(2);
                }

                else if ((Convert.ToInt32(cmbVehiculos.SelectedValue) != 0) && (Convert.ToInt32(cmbTerminal.SelectedValue) != 0))
                {
                    llenarGrid(3);
                }
            }
        }

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);
                Session["idProgramacion"] = dgvDatos.Rows[a].Cells[1].Text;
                columnasGrid(false);

                visualizarReporte(Convert.ToInt32(Session["idProgramacion"].ToString()));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnImprimirReporte_Click(object sender, EventArgs e)
        {
            try
            {
                Clases.ClaseImprimirManifiesto manifiesto = new Clases.ClaseImprimirManifiesto();

                sSql = "";
                sSql += "select pago_pendiente_info, ingreso_efectivo_info" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + Convert.ToInt32(Session["idProgramacion"].ToString()) + Environment.NewLine;
                sSql += "and cobro_administrativo = 1" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        manifiesto.llenarReporte(Convert.ToInt32(Session["idProgramacion"].ToString()), Session["usuario"].ToString(), 1, Convert.ToDecimal(dtConsulta.Rows[0]["pago_pendiente_info"].ToString()), Convert.ToDecimal(dtConsulta.Rows[0]["ingreso_efectivo_info"].ToString()), 0);
                    }

                    else
                    {
                        manifiesto.llenarReporte(Convert.ToInt32(Session["idProgramacion"].ToString()), Session["usuario"].ToString(), 1, 0, 0, 0);
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

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvDatos.PageIndex = e.NewPageIndex;

                if ((Convert.ToInt32(cmbVehiculos.SelectedValue) == 0) && (Convert.ToInt32(cmbTerminal.SelectedValue) == 0))
                {
                    llenarGrid(0);
                }

                else if ((Convert.ToInt32(cmbVehiculos.SelectedValue) != 0) && (Convert.ToInt32(cmbTerminal.SelectedValue) == 0))
                {
                    llenarGrid(1);
                }

                else if ((Convert.ToInt32(cmbVehiculos.SelectedValue) == 0) && (Convert.ToInt32(cmbTerminal.SelectedValue) != 0))
                {
                    llenarGrid(2);
                }

                else if ((Convert.ToInt32(cmbVehiculos.SelectedValue) != 0) && (Convert.ToInt32(cmbTerminal.SelectedValue) != 0))
                {
                    llenarGrid(3);
                }
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
    }
}