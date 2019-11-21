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
    public partial class frmReporteViajesActivos : System.Web.UI.Page
    {
        ENTReporteViajesActivos reporteViajesE = new ENTReporteViajesActivos();
        ENTComboDatos comboE = new ENTComboDatos();

        manejadorConexion conexionM = new manejadorConexion();
        manejadorReporteViajesActivos reporteViajesM = new manejadorReporteViajesActivos();
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

            catch(Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        ////FUNCION PARA LLENAR EL COMBOBOX DE JORNADAS
        //private void llenarComboJornadas()
        //{
        //    try
        //    {
        //        sSql = "";
        //        sSql += "select id_ctt_jornada, descripcion" + Environment.NewLine;
        //        sSql += "from ctt_jornada" + Environment.NewLine;
        //        sSql += "where estado = 'A'";

        //        comboE.ISSQL = sSql;
        //        cmbJornada.DataSource = comboM.listarCombo(comboE);
        //        cmbJornada.DataValueField = "IID";
        //        cmbJornada.DataTextField = "IDATO";
        //        cmbJornada.DataBind();
        //        cmbJornada.Items.Insert(0, new ListItem("Todos", "0"));
        //    }

        //    catch (Exception ex)
        //    {
        //        lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
        //    }
        //}

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
                //Session["jornada"] = cmbJornada.SelectedItem.ToString();

                sSql = "";
                sSql += "select numero_viaje, descripcion + '-' + placa vehiculo, fecha_viaje," + Environment.NewLine;

                if (iOp == 0)
                {
                    sSql += "hora_salida, tipo_viaje, 'TODOS' usuario_ingreso, sum(cantidad) cantidad," + Environment.NewLine;
                }

                else
                {
                    sSql += "hora_salida, tipo_viaje, usuario_ingreso, sum(cantidad) cantidad," + Environment.NewLine;
                }

                sSql += "ltrim(str(sum(cantidad * (precio_unitario - valor_dscto + valor_iva)), 10, 2)) valor" + Environment.NewLine;
                sSql += "from ctt_vw_viajes_activos" + Environment.NewLine;
                sSql += "where fecha_viaje between '" + sFechaInicial + "'" + Environment.NewLine;
                sSql += "and '" + sFechaFinal + "'" + Environment.NewLine;
                sSql += "and id_ctt_pueblo_origen = " + Session["id_pueblo"].ToString() + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and id_ctt_vehiculo = " + Convert.ToInt32(cmbVehiculos.SelectedValue) + Environment.NewLine;
                }

                else if (iOp == 2)
                {
                    sSql += "and id_ctt_oficinista = " + Convert.ToInt32(cmbUsuarios.SelectedValue) + Environment.NewLine;
                }

                else if (iOp == 3)
                {
                    sSql += "and id_ctt_vehiculo = " + Convert.ToInt32(cmbVehiculos.SelectedValue) + Environment.NewLine;
                    sSql += "and id_ctt_oficinista = " + Convert.ToInt32(cmbUsuarios.SelectedValue) + Environment.NewLine;
                }

                //if (Convert.ToInt32(cmbJornada.SelectedValue) != 0)
                //{
                //    sSql += "and id_ctt_jornada = " + Convert.ToInt32(cmbJornada.SelectedValue) + Environment.NewLine;
                //}

                sSql += "group by numero_viaje, descripcion, placa, fecha_viaje," + Environment.NewLine;

                if (iOp == 0)
                {
                    sSql += "hora_salida, tipo_viaje" + Environment.NewLine;
                }

                else
                {
                    sSql += "hora_salida, tipo_viaje, usuario_ingreso" + Environment.NewLine;
                }
                
                sSql += "order by fecha_viaje, hora_salida";

                Session["instruccion"] = sSql;

                reporteViajesE.ISQL = sSql;
                dgvDatos.DataSource = reporteViajesM.listarReporteViajesActivos(reporteViajesE);
                dgvDatos.DataBind();

                if (dgvDatos.Rows.Count > 0)
                {
                    btnImprimir.Visible = true;
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No existen registros con los datos ingresados para la búsqueda.', 'warning');", true);
                    btnImprimir.Visible = false;
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
            txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");

            llenarComboUsuarios();
            llenarComboVehiculos();
            //llenarComboJornadas();
            dtConsulta = new DataTable();
            dtConsulta.Clear();

            dgvDatos.DataSource = dtConsulta;
            dgvDatos.DataBind();

            btnImprimir.Visible = false;

            Session["fecha_desde"] = null;
            Session["fecha_hasta"] = null;
            Session["instruccion"] = null;
            txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        #endregion

        #region FUNCIONES PARA EL REPORTE

        //FUNCION PARA MOSTRAR EL REPORTE
        public void Reporte()
        {
            try
            {
                DSReportes ds = new DSReportes();

                DataTable dt = ds.Tables["RptViajesActivos"];
                dt.Clear();

                bRespuesta = conexionM.consultarRegistro(Session["instruccion"].ToString(), dt);

                if (bRespuesta == true)
                {
                    if (dt.Rows.Count > 0)
                    {
                        ModalPopupExtender_Reporte.Show();

                        rptViajesActivos.ProcessingMode = ProcessingMode.Local;
                        ReportParameter[] parametros = new ReportParameter[5];
                        parametros[0] = new ReportParameter("P_Fecha_Desde", Session["fecha_desde"].ToString());
                        parametros[1] = new ReportParameter("P_Fecha_Hasta", Session["fecha_hasta"].ToString());
                        parametros[2] = new ReportParameter("P_Usuario", Session["usuario"].ToString());
                        parametros[3] = new ReportParameter("P_Vehiculo", Session["vehiculo"].ToString());
                        parametros[4] = new ReportParameter("P_Jornada", "TODAS");
                        rptViajesActivos.LocalReport.ReportPath = Server.MapPath("~/Reportes/rptViajesActivos.rdlc");
                        rptViajesActivos.LocalReport.DataSources.Clear();
                        ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
                        rptViajesActivos.LocalReport.SetParameters(parametros);
                        rptViajesActivos.LocalReport.DataSources.Add(datasource);
                        rptViajesActivos.LocalReport.Refresh();
                    }
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo crear el reporte. Comuníquese con el administrador.', 'danger');", true);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Reporte();
            //mostrarReporte();
        }

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
                if ((Convert.ToInt32(cmbVehiculos.SelectedValue) == 0) && (Convert.ToInt32(cmbUsuarios.SelectedValue) == 0))
                {
                    llenarGrid(0);
                }

                else if ((Convert.ToInt32(cmbVehiculos.SelectedValue) != 0) && (Convert.ToInt32(cmbUsuarios.SelectedValue) == 0))
                {
                    llenarGrid(1);
                }

                else if ((Convert.ToInt32(cmbVehiculos.SelectedValue) == 0) && (Convert.ToInt32(cmbUsuarios.SelectedValue) != 0))
                {
                    llenarGrid(2);
                }

                else if ((Convert.ToInt32(cmbVehiculos.SelectedValue) != 0) && (Convert.ToInt32(cmbUsuarios.SelectedValue) != 0))
                {
                    llenarGrid(3);
                }
            }
        }

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvDatos.PageIndex = e.NewPageIndex;

            if (Convert.ToInt32(cmbVehiculos.SelectedValue) == 0)
            {
                llenarGrid(0);
            }

            else
            {
                llenarGrid(1);
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