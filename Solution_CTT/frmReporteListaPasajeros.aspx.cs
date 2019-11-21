using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ENTIDADES;
using NEGOCIO;
using Microsoft.Reporting.WebForms;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmReporteListaPasajeros : System.Web.UI.Page
    {
        ENTViajes asignarE = new ENTViajes();

        manejadorConexion conexionM = new manejadorConexion();
        manejadorAsignarViaje asignarM = new manejadorAsignarViaje();

        string sSql;
        string sFecha;
        string sEstadoViaje;
        string sEstadoRetencion;
        string sEstadoPago;
        string sNombrePagoAdministracion;

        DataTable dtConsulta;

        bool bRespuesta;

        double dbSuma;
        double dbRetencion;
        double dbPago;
        double dbTotal_1;
        double dbTotal_2;
        double dbTotal_3;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
                return;
            }

            Session["modulo"] = "MÓDULO DE IMPRESIÓN DE LISTA DE PASAJEROS";

            if (!IsPostBack)
            {
                sFecha = DateTime.Now.ToString("dd/MM/yyyy");
                txtDate.Text = sFecha;
                llenarGrid(sFecha);
                Session["idVehiculo"] = null;
                Session["idProgramacion"] = null;
            }
        }

        #region FUNCIONES DEL USUARIO

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

        //FUNCION PARA LLENAR EL GRID DE VIAJES
        private void llenarGrid(string sFecha)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_itinerarios" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(Session["id_pueblo"].ToString()) + Environment.NewLine;
                sSql += "and fecha_viaje = '" + Convert.ToDateTime(sFecha).ToString("yyyy/MM/dd") + "'" + Environment.NewLine;
                sSql += "order by hora_salida";

                columnasGrid(true);
                asignarE.ISSQL = sSql;
                dgvDatos.DataSource = asignarM.listarViajes(asignarE);
                dgvDatos.DataBind();
                columnasGrid(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }


        //FUNCION PARA MOSTRAR EL MODAL CON EL REPORTE
        private void visualizarReporte(int iIdProgramacion)
        {
            try
            {
                DSReportes ds = new DSReportes();

                DataTable dtPasajeros = ds.Tables["RptPasajeros"];
                dtPasajeros.Clear();

                sSql = "";
                sSql += "select * from ctt_vw_reporte_pasajeros" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + iIdProgramacion + Environment.NewLine;
                sSql += "order by numero_asiento";

                bRespuesta = conexionM.consultarRegistro(sSql, dtPasajeros);

                if (bRespuesta == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo cargar el informe.', 'danger');", true);
                    goto fin;
                }

                if (bRespuesta == true)
                {
                    if (dtPasajeros.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'No existen pasajeros en el viaje. Favor comuníquese con administración.', 'warning');", true);
                        goto fin;
                    }

                    else
                    {
                        dbSuma = 0;

                        for (int i = 0; i < dtPasajeros.Rows.Count; i++)
                        {
                            dbSuma = dbSuma + Convert.ToDouble(dtPasajeros.Rows[i]["valor"].ToString());
                        }
                    }
                }

                //DataTable dtPagos = ds.Tables["dtPagos"];
                //dtPagos.Clear();
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sSql = "";
                sSql += "select * from ctt_vw_pagos_frecuencia" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + iIdProgramacion;

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo cargar el informe.', 'danger');", true);
                    goto fin;
                }

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'Ocurrió un problema al cargar los campos. Favor comuníquese con administración.', 'warning');", true);
                        goto fin;
                    }
                }

                //CARGAR EL REPORTE
                sEstadoRetencion = dtConsulta.Rows[0][8].ToString();               

                dbTotal_1 = dbSuma;

                dbRetencion = Convert.ToDouble(dtConsulta.Rows[0][5].ToString());
                dbPago = 0;
                sNombrePagoAdministracion = "PAGO ADMINISTRACION";

                if (dtConsulta.Rows.Count > 1)
                {
                    sNombrePagoAdministracion = dtConsulta.Rows[1][9].ToString();
                    sEstadoPago = dtConsulta.Rows[1][8].ToString();

                    if (sEstadoPago != "Pagado")
                    {
                        dbPago = 0;
                    }

                    else
                    {
                        dbPago = Convert.ToDouble(dtConsulta.Rows[1][6].ToString());
                    }
                }

                else
                {
                    sEstadoPago = "Pagado";
                }

                dbTotal_2 = dbTotal_1 - dbRetencion;
                dbTotal_3 = dbTotal_2 - dbPago;
                

                ModalPopupExtender_Reporte.Show();

                ReportParameter[] parametros = new ReportParameter[11];
                parametros[0] = new ReportParameter("P_Subtotal", dbSuma.ToString("N2"));
                parametros[1] = new ReportParameter("P_Descuento", "0.00");
                parametros[2] = new ReportParameter("P_Total_1", dbTotal_1.ToString("N2"));
                parametros[3] = new ReportParameter("P_Retencion", dbRetencion.ToString("N2"));
                parametros[4] = new ReportParameter("P_Total_2", dbTotal_2.ToString("N2"));
                parametros[5] = new ReportParameter("P_PagoAdministracion", dbPago.ToString("N2"));
                parametros[6] = new ReportParameter("P_Total_3", dbTotal_3.ToString("N2"));
                parametros[7] = new ReportParameter("P_NombreRetencion", "- " + dtConsulta.Rows[0][4].ToString() + "% " + dtConsulta.Rows[0][9].ToString());
                parametros[8] = new ReportParameter("P_NombrePago", "- " + sNombrePagoAdministracion);
                parametros[9] = new ReportParameter("P_Estado_Retencion", sEstadoRetencion);
                parametros[10] = new ReportParameter("P_Estado_Pago", sEstadoPago);

                rptListaPasajeros.ProcessingMode = ProcessingMode.Local;
                rptListaPasajeros.LocalReport.ReportPath = Server.MapPath("~/Reportes/RVPruebas.rdlc");
                ReportDataSource datasource = new ReportDataSource("DataSet1", dtPasajeros);

                rptListaPasajeros.LocalReport.DataSources.Clear();
                rptListaPasajeros.LocalReport.SetParameters(parametros);
                rptListaPasajeros.LocalReport.DataSources.Add(datasource);
                rptListaPasajeros.LocalReport.Refresh();

                goto fin;
            }

            catch(Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

            fin: {}
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

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);
                Session["idProgramacion"] = dgvDatos.Rows[a].Cells[1].Text;
                sEstadoViaje = dgvDatos.Rows[a].Cells[9].Text;
                columnasGrid(false);

                if (sEstadoViaje == "A")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El viaje se encuentra vigente.', 'info');", true);
                    Session["idProgramacion"] = null;
                }

                else
                {
                    visualizarReporte(Convert.ToInt32(Session["idProgramacion"].ToString()));
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
                sFecha = txtDate.Text.Trim();

                llenarGrid(Session["fecha"].ToString());
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
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
                    Session["fecha"] = sFecha;
                    llenarGrid(sFecha);
                }
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