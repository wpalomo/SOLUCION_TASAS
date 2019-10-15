using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NEGOCIO;
using ENTIDADES;
using Microsoft.Reporting.WebForms;

namespace Solution_CTT
{
    public partial class frmReporteViajesAgrupados : System.Web.UI.Page
    {
        ENTReporteViajesAgrupados reporteViajesAgrupadosE = new ENTReporteViajesAgrupados();

        manejadorConexion conexionM = new manejadorConexion();
        manejadorReporteViajesAgrupados reporteViajesAgrupadosM = new manejadorReporteViajesAgrupados();

        ENTComboDatos comboE = new ENTComboDatos();
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

            Session["modulo"] = "MÓDULO DE CONSULTA DE VIAJES AGRUPADOS";

            if (!IsPostBack)
            {
                limpiar();
            }
        }

        #region FUNCIONES DEL USUARIO
        //FUNCION PARA LLENAR EL COMOBOX DE ORIGEN Y DESTINO
        private void llenarComboTransporte()
        {
            try
            {
                sSql = "";
                sSql += "select V.id_ctt_vehiculo, D.descripcion + '-' + V.placa as transporte" + Environment.NewLine;
                sSql += "from ctt_vehiculo V, ctt_disco D" + Environment.NewLine;
                sSql += "where V.id_ctt_disco = D.id_ctt_disco" + Environment.NewLine;
                sSql += "and V.estado = 'A'" + Environment.NewLine;
                sSql += "and D.estado = 'A'" + Environment.NewLine;
                sSql += "order by D.descripcion";

                comboE.ISSQL = sSql;

                cmbTransporte.DataSource = comboM.listarCombo(comboE);
                cmbTransporte.DataValueField = "IID";
                cmbTransporte.DataTextField = "IDATO";
                cmbTransporte.DataBind();
                cmbTransporte.Items.Insert(0, new ListItem("Todos", "0"));

                //if (cmbTransporte.Items.Count > 24)
                //{
                //    cmbTransporte.SelectedIndex = 24;
                //}

                //else
                //{
                //    cmbTransporte.SelectedIndex = 0;
                //}
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION LLENAR OFICINISTA
        private void llenarComboUsuario()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_oficinista, descripcion" + Environment.NewLine;
                sSql += "from ctt_oficinista" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;

                cmbUsuario.DataSource = comboM.listarCombo(comboE);
                cmbUsuario.DataValueField = "IID";
                cmbUsuario.DataTextField = "IDATO";
                cmbUsuario.DataBind();
                cmbUsuario.Items.Insert(0, new ListItem("Todos", "0"));

                //if (cmbUsuario.Items.Count > 24)
                //{
                //    cmbUsuario.SelectedIndex = 24;
                //}

                //else
                //{
                //    cmbUsuario.SelectedIndex = 0;
                //}
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }
        //FUNCION LLENAR RUTAS
        private void llenarComboRutas()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_ruta, descripcion from ctt_ruta" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;

                cmbRuta.DataSource = comboM.listarCombo(comboE);
                cmbRuta.DataValueField = "IID";
                cmbRuta.DataTextField = "IDATO";
                cmbRuta.DataBind();
                cmbRuta.Items.Insert(0, new ListItem("Todos", "0"));

                //if (cmbRuta.Items.Count > 24)
                //{
                //    cmbRuta.SelectedIndex = 24;
                //}

                //else
                //{
                //    cmbRuta.SelectedIndex = 0;
                //}
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid(int iOP, string FechaInicial, string FechaFinal, string Transporte, string Ruta, string Usuario)
        {
            try
            {
                sFechaInicial = Convert.ToDateTime(txtFechaDesde.Text.Trim()).ToString("yyyy/MM/dd");
                sFechaFinal = Convert.ToDateTime(txtFechaHasta.Text.Trim()).ToString("yyyy/MM/dd");                

                Session["fecha_desde"] = sFechaInicial;
                Session["fecha_hasta"] = sFechaFinal;
                Session["vehiculo"] = cmbTransporte.SelectedItem.ToString();
                Session["ruta"] = cmbRuta.SelectedItem.ToString();
                Session["usuario_P"] = cmbUsuario.SelectedItem.ToString();

                sSql = "";
                sSql += "select id_ctt_programacion, transporte, numero_viaje, fecha_viaje, " + Environment.NewLine;
                sSql += "hora_salida, ruta, usuario, id_ctt_oficinista, id_ctt_vehiculo," + Environment.NewLine;
                sSql += "ltrim(str(sum(cantidad * (precio_unitario - valor_dscto + valor_iva)), 10, 2)) valor," + Environment.NewLine;
                sSql += "count (*) cuenta" + Environment.NewLine;
                sSql += "from ctt_vw_viajes_agrupados WHERE" + Environment.NewLine;
                sSql += " fecha_viaje between '" + sFechaInicial + "' and '" + sFechaFinal + "'" + Environment.NewLine;
                //FILTOS
                if (iOP == 1)//TRANSPORTE
                {
                    sSql += "and transporte ='" + Transporte + "'" + Environment.NewLine;
                }
                if (iOP == 2)//RUTA
                {
                    sSql += "and ruta = '" + Ruta + "'" + Environment.NewLine;
                }
                if (iOP == 3)//USUARIO
                {
                    sSql += "and usuario = '" + Usuario + "'" + Environment.NewLine;
                }
                if (iOP == 4)//TRANSPORTE, RUTA
                {
                    sSql += "and transporte ='" + Transporte + "'" + Environment.NewLine;
                    sSql += "and ruta = '" + Ruta + "'" + Environment.NewLine;
                }
                if (iOP == 5)//TRANSPORTE, USUARIO
                {
                    sSql += "and transporte ='" + Transporte + "'" + Environment.NewLine;
                    sSql += "and usuario = '" + Usuario + "'" + Environment.NewLine;
                }
                if (iOP == 6)// RUTA, USUARIO
                {
                    sSql += "and ruta = '" + Ruta + "'" + Environment.NewLine;
                    sSql += "and usuario = '" + Usuario + "'" + Environment.NewLine;
                }
                if (iOP == 7)//TRANSPORTE, RUTA, USUARIO
                {
                    sSql += "and transporte ='" + Transporte + "'" + Environment.NewLine;
                    sSql += "and ruta = '" + Ruta + "'" + Environment.NewLine;
                    sSql += "and usuario = '" + Usuario + "'" + Environment.NewLine;
                }
                //FIN

                sSql += "group by id_ctt_programacion, cantidad, transporte, numero_viaje," + Environment.NewLine;
                sSql += "fecha_viaje, hora_salida, ruta, usuario, id_ctt_oficinista, id_ctt_vehiculo" + Environment.NewLine;
                sSql += "order by fecha_viaje, hora_salida";

                Session["instruccion"] = sSql;

                reporteViajesAgrupadosE.ISQL = sSql;
                dgvDatos.DataSource = reporteViajesAgrupadosM.listarReporteViajesAgrupados(reporteViajesAgrupadosE);
                dgvDatos.DataBind();

                if (dgvDatos.Rows.Count > 0)
                {
                    btnImprimir.Visible = true;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No exiten registros con los datos ingresados para la búsqueda.', 'warning');", true);
                    btnImprimir.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            dtConsulta = new DataTable();
            dtConsulta.Clear();

            dgvDatos.DataSource = dtConsulta;
            dgvDatos.DataBind();

            //btnImprimir.Visible = false;

            //Session["fecha_desde"] = null;
            //Session["fecha_hasta"] = null;
            //Session["instruccion"] = null;
            txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
            llenarComboTransporte();
            llenarComboUsuario();
            llenarComboRutas();

            btnImprimir.Visible = false;
        }

        #endregion

        #region FUNCIONES PARA EL REPORTE

        //FUNCION PARA MOSTRAR EL REPORTE
        public void Reporte()
        {
            try
            {
                DSReportes ds = new DSReportes();

                DataTable dt = ds.Tables["RptViajesAgrupados"];
                dt.Clear();

                bRespuesta = conexionM.consultarRegistro(Session["instruccion"].ToString(), dt);

                if (bRespuesta == true)
                {
                    if (dt.Rows.Count > 0)
                    {
                        ModalPopupExtender_Reporte.Show();

                        rptViajeAgrupados.ProcessingMode = ProcessingMode.Local;
                        ReportParameter[] parametros = new ReportParameter[5];
                        parametros[0] = new ReportParameter("P_Fecha_Desde", Session["fecha_desde"].ToString());
                        parametros[1] = new ReportParameter("P_Fecha_Hasta", Session["fecha_hasta"].ToString());
                        parametros[2] = new ReportParameter("P_Usuario", Session["usuario_P"].ToString());
                        parametros[3] = new ReportParameter("P_Vehiculo", Session["vehiculo"].ToString());
                        parametros[4] = new ReportParameter("P_Ruta", Session["ruta"].ToString());
                        rptViajeAgrupados.LocalReport.ReportPath = Server.MapPath("~/Reportes/rptViajesAgrupados.rdlc");
                        rptViajeAgrupados.LocalReport.DataSources.Clear();
                        ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
                        rptViajeAgrupados.LocalReport.SetParameters(parametros);
                        rptViajeAgrupados.LocalReport.DataSources.Add(datasource);
                        rptViajeAgrupados.LocalReport.Refresh();
                    }
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo crear el reporte. Comuníquese con el administrador.', 'danger');", true);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

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
                if ((Convert.ToInt32(cmbTransporte.SelectedValue) == 0) && (Convert.ToInt32(cmbRuta.SelectedValue) == 0) && (Convert.ToInt32(cmbUsuario.SelectedValue) == 0))
                {
                    //LLENAR FECHAS
                    llenarGrid(0, txtFechaDesde.Text.Trim(), txtFechaHasta.Text.Trim(), "", "", "");
                }
                else
                {
                    if ((Convert.ToInt32(cmbTransporte.SelectedValue) != 0) && (Convert.ToInt32(cmbRuta.SelectedValue) == 0) && (Convert.ToInt32(cmbUsuario.SelectedValue) == 0))
                    {
                        //LLENAR FECHAS, TRANSPORTE
                        llenarGrid(1, txtFechaDesde.Text.Trim(), txtFechaHasta.Text.Trim(), cmbTransporte.SelectedItem.ToString(), "", "");
                    }
                    else
                    {
                        if ((Convert.ToInt32(cmbTransporte.SelectedValue) == 0) && (Convert.ToInt32(cmbRuta.SelectedValue) != 0) && (Convert.ToInt32(cmbUsuario.SelectedValue) == 0))
                        {
                            //LLENAR FECHAS, "", RUTA, ""
                            llenarGrid(2, txtFechaDesde.Text.Trim(), txtFechaHasta.Text.Trim(), "", cmbRuta.SelectedItem.ToString(), "");
                        }
                        else
                        {
                            if ((Convert.ToInt32(cmbTransporte.SelectedValue) == 0) && (Convert.ToInt32(cmbRuta.SelectedValue) == 0) && (Convert.ToInt32(cmbUsuario.SelectedValue) != 0))
                            {
                                //LLENAR FECHAS, "", "", USUARIO
                                llenarGrid(3, txtFechaDesde.Text.Trim(), txtFechaHasta.Text.Trim(), "", "", cmbUsuario.SelectedItem.ToString());
                            }
                            else
                            {
                                if ((Convert.ToInt32(cmbTransporte.SelectedValue) == 0) && (Convert.ToInt32(cmbRuta.SelectedValue) != 0) && (Convert.ToInt32(cmbUsuario.SelectedValue) != 0))
                                {
                                    //LLENAR FECHAS, "", RUTA, USUARIO
                                    llenarGrid(6, txtFechaDesde.Text.Trim(), txtFechaHasta.Text.Trim(), "", cmbRuta.SelectedItem.ToString(), cmbUsuario.SelectedItem.ToString());
                                }
                                else
                                {
                                    if ((Convert.ToInt32(cmbTransporte.SelectedValue) != 0) && (Convert.ToInt32(cmbRuta.SelectedValue) == 0) && (Convert.ToInt32(cmbUsuario.SelectedValue) == 0))
                                    {
                                        if ((Convert.ToInt32(cmbTransporte.SelectedValue) != 0) && (Convert.ToInt32(cmbRuta.SelectedValue) != 0) && (Convert.ToInt32(cmbUsuario.SelectedValue) == 0))
                                        {
                                            //LLENAR FECHAS, TRANSPORTE, RUTA, ""
                                            llenarGrid(4, txtFechaDesde.Text.Trim(), txtFechaHasta.Text.Trim(), cmbTransporte.SelectedItem.ToString(), cmbRuta.SelectedItem.ToString(), "");
                                        }
                                        else
                                        {
                                            if ((Convert.ToInt32(cmbTransporte.SelectedValue) != 0) && (Convert.ToInt32(cmbRuta.SelectedValue) == 0) && (Convert.ToInt32(cmbUsuario.SelectedValue) != 0))
                                            {
                                                //LLENAR FECHAS, TRANSPORTE, "", USUARIO
                                                llenarGrid(5, txtFechaDesde.Text.Trim(), txtFechaHasta.Text.Trim(), cmbTransporte.SelectedItem.ToString(), "", cmbUsuario.SelectedItem.ToString());
                                            }
                                            else
                                            {
                                                if ((Convert.ToInt32(cmbTransporte.SelectedValue) != 0) && (Convert.ToInt32(cmbRuta.SelectedValue) != 0) && (Convert.ToInt32(cmbUsuario.SelectedValue) != 0))
                                                {
                                                    //LLENAR TODOS
                                                    llenarGrid(7, txtFechaDesde.Text.Trim(), txtFechaHasta.Text.Trim(), cmbTransporte.SelectedItem.ToString(), cmbRuta.SelectedItem.ToString(), cmbUsuario.SelectedItem.ToString());
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if ((Convert.ToInt32(cmbTransporte.SelectedValue) != 0) && (Convert.ToInt32(cmbRuta.SelectedValue) == 0) && (Convert.ToInt32(cmbUsuario.SelectedValue) != 0))
                                        {
                                            //LLENAR FECHAS, TRANSPORTE, "", USUARIO
                                            llenarGrid(5, txtFechaDesde.Text.Trim(), txtFechaHasta.Text.Trim(), cmbTransporte.SelectedItem.ToString(), "", cmbUsuario.SelectedItem.ToString());
                                        }
                                        else
                                        {
                                            if ((Convert.ToInt32(cmbTransporte.SelectedValue) != 0) && (Convert.ToInt32(cmbRuta.SelectedValue) != 0) && (Convert.ToInt32(cmbUsuario.SelectedValue) != 0))
                                            {
                                                //LLENAR TODOS
                                                llenarGrid(7, txtFechaDesde.Text.Trim(), txtFechaHasta.Text.Trim(), cmbTransporte.SelectedItem.ToString(), cmbRuta.SelectedItem.ToString(), cmbUsuario.SelectedItem.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvDatos.PageIndex = e.NewPageIndex;
            //llenarGrid();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }
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
    }
}