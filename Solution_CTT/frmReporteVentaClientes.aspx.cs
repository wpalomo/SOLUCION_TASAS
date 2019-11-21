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
    public partial class frmReporteVentaClientes : System.Web.UI.Page
    {
        ENTReporteVentaClientes reporteClientesE = new ENTReporteVentaClientes();
        ENTPasajeros personaE = new ENTPasajeros();

        manejadorPasajeros personaM = new manejadorPasajeros();
        manejadorConexion conexionM = new manejadorConexion();
        manejadorReporteVentaClientes reporteClienteM = new manejadorReporteVentaClientes();

        string sSql;
        string sFechaInicial;
        string sFechaFinal;
        string sAccionFiltro;

        bool bRespuesta;

        DataTable dtConsulta;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
                return;
            }

            Session["modulo"] = "MÓDULO DE REPORTE DE VENTA POR CLIENTES";

            if (!IsPostBack)
            {
                limpiar();
            }
        }

        #region FUNCIONES DEL MODAL DE FILTRO DE PERSONAS

        private void columnasGridFiltro(bool ok)
        {
            dgvFiltrarPersonas.Columns[0].Visible = ok;
            dgvFiltrarPersonas.Columns[1].ItemStyle.Width = 150;
            dgvFiltrarPersonas.Columns[2].ItemStyle.Width = 300;
            dgvFiltrarPersonas.Columns[3].ItemStyle.Width = 150;
            dgvFiltrarPersonas.Columns[4].ItemStyle.Width = 100;
        }

        //FUNCION PARA LLENAR EL GRID DE CLIENTES
        private void llenarGridPersonas(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select id_persona, identificacion," + Environment.NewLine;
                sSql += "ltrim(isnull(nombres, '') + ' ' + apellidos) personas," + Environment.NewLine;
                sSql += "isnull(fecha_nacimiento, GETDATE()) fecha_nacimiento" + Environment.NewLine;
                sSql += "from tp_personas" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and identificacion like '%" + txtFiltrarPersonas.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or nombres like '%" + txtFiltrarPersonas.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or apellidos like '%" + txtFiltrarPersonas.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by id_persona";

                columnasGridFiltro(true);
                personaE.ISQL = sSql;
                dgvFiltrarPersonas.DataSource = personaM.listarPasajeros(personaE);
                dgvFiltrarPersonas.DataBind();
                columnasGridFiltro(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid()
        {
            try
            {
                sFechaInicial = Convert.ToDateTime(txtFechaDesde.Text.Trim()).ToString("yyyy/MM/dd");
                sFechaFinal = Convert.ToDateTime(txtFechaHasta.Text.Trim()).ToString("yyyy/MM/dd");

                Session["fecha_desde"] = sFechaInicial;
                Session["fecha_hasta"] = sFechaFinal;

                sSql = "";
                sSql += "select identificacion, nombre, apellidos, tipo_cliente," + Environment.NewLine;
                sSql += "ltrim(str(sum(precio_unitario + valor_iva - valor_dscto), 10, 2)) valor," + Environment.NewLine;
                sSql += "ruta, destino, fecha_viaje,  fecha_pedido, nombre_pasajero, hora_salida, count(*) cuenta" + Environment.NewLine;
                sSql += "from ctt_vw_reporte_por_pasajeros" + Environment.NewLine;
                sSql += "where identificacion = '" + txtIdentificacion.Text.Trim() + "'" + Environment.NewLine;
                sSql += "and fecha_pedido between '" + sFechaInicial + "'" + Environment.NewLine;
                sSql += "and '" + sFechaFinal + "'" + Environment.NewLine;
                sSql += "group by identificacion, nombre, apellidos, tipo_cliente," + Environment.NewLine;
                sSql += "ruta, destino, fecha_viaje, fecha_pedido, nombre_pasajero," + Environment.NewLine;
                sSql += "cantidad, precio_unitario, valor_dscto, valor_iva, hora_salida";

                Session["instruccion"] = sSql;

                reporteClientesE.ISQL = sSql;
                dgvDatos.DataSource = reporteClienteM.listarReporteVentaClientes(reporteClientesE);
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

            catch(Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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

            btnImprimir.Visible = false;

            Session["fecha_desde"] = null;
            Session["fecha_hasta"] = null;
            Session["instruccion"] = null;
            Session["id_Persona"] = null;
            txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtIdentificacion.Text = "";
            txtNombres.Text = "";
        }

        #endregion

        #region FUNCIONES PARA EL REPORTE

        //FUNCION PARA MOSTRAR EL REPORTE
        public void Reporte()
        {
            try
            {
                DSReportes ds = new DSReportes();
                DataTable dt = ds.Tables["RptVentaPasajeros"];
                dt.Clear();

                bRespuesta = conexionM.consultarRegistro(Session["instruccion"].ToString(), dt);

                if (bRespuesta == true)
                {
                    if (dt.Rows.Count > 0)
                    {
                        ModalPopupExtender_Reporte.Show();

                        rptVentasClientes.ProcessingMode = ProcessingMode.Local;
                        ReportParameter[] parametros = new ReportParameter[2];
                        parametros[0] = new ReportParameter("P_Fecha_Desde", Session["fecha_desde"].ToString());
                        parametros[1] = new ReportParameter("P_Fecha_Hasta", Session["fecha_hasta"].ToString());
                        rptVentasClientes.LocalReport.ReportPath = Server.MapPath("~/Reportes/rptVentasCliente.rdlc");
                        rptVentasClientes.LocalReport.DataSources.Clear();
                        ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
                        rptVentasClientes.LocalReport.SetParameters(parametros);
                        rptVentasClientes.LocalReport.DataSources.Add(datasource);
                        rptVentasClientes.LocalReport.Refresh();
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

            else if (txtIdentificacion.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número de identificación del cliente.', 'warning');", true);
            }

            else
            {
                llenarGrid();
            }
        }

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvDatos.PageIndex = e.NewPageIndex;
            llenarGrid();
        }

        protected void btnAbrirModalClientes_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Personas.Show();
            llenarGridPersonas(0);
        }


        //EVENTOS DE LA VENTANA MODAL

        //PARA FILTRAR LOS DATOS DEL CLIENTE
        protected void dgvFiltrarPersonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvFiltrarPersonas.SelectedIndex;
                columnasGridFiltro(true);

                if (sAccionFiltro == "Seleccion")
                {
                    Session["id_Persona"] = dgvFiltrarPersonas.Rows[a].Cells[0].Text.Trim();
                    txtIdentificacion.Text = dgvFiltrarPersonas.Rows[a].Cells[1].Text.Trim();
                    txtNombres.Text = HttpUtility.HtmlDecode(dgvFiltrarPersonas.Rows[a].Cells[2].Text.Trim());
                    txtFiltrarPersonas.Text = "";
                    ModalPopupExtender_Personas.Hide();
                }

                columnasGridFiltro(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvFiltrarPersonas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvFiltrarPersonas.PageIndex = e.NewPageIndex;

                if (txtFiltrarPersonas.Text.Trim() == "")
                {
                    llenarGridPersonas(0);
                }

                else
                {
                    llenarGridPersonas(1);
                }
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnFiltrarPersonas_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFiltrarPersonas.Text.Trim() == "")
                {
                    llenarGridPersonas(0);
                }

                else
                {
                    llenarGridPersonas(1);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnCerrarModalPersonas_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Personas.Hide();
        }

        protected void lbtnSeleccion_Click(object sender, EventArgs e)
        {
            sAccionFiltro = "Seleccion";
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

        protected void dgvFiltrarPersonas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < dgvFiltrarPersonas.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvFiltrarPersonas.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvFiltrarPersonas.Rows[i].BackColor = Color.White;
                }
            }
        }
    }
}

