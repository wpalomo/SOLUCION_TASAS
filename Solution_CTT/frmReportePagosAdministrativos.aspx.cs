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

namespace Solution_CTT
{
    public partial class frmReportePagosAdministrativos : System.Web.UI.Page
    {
        ENTReporteCobroAdministracion pagosE = new ENTReporteCobroAdministracion();
        ENTComboDatos comboE = new ENTComboDatos();

        manejadorReporteCobroAdministracion pagosM = new manejadorReporteCobroAdministracion();
        manejadorConexion conexionM = new manejadorConexion();
        manejadorComboDatos comboM = new manejadorComboDatos();

        Clases.ClaseReporteCobroRetenciones reporte = new Clases.ClaseReporteCobroRetenciones();
        Clases.ClaseImpresion imprimir = new Clases.ClaseImpresion();

        string sSql;
        string sFechaInicial;
        string sFechaFinal;
        string sImprimir;
        string sNombreImpresora;
        string sPathImpresora;

        DataTable dtConsulta;

        bool bRespuesta;

        int iCortarPapel;
        int iNumeroImpresiones;

        double dbSuma;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
                return;
            }

            Session["modulo"] = "MÓDULO DE REPORTE DE PAGOS ADMINISTRATIVOS COBRADOS";

            if (!IsPostBack)
            {
                limpiar();
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
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No existe una configuración de impresora para el terminal. Comuníquese con el administrador.', 'warning');", true);
                    }
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo cargar los parámetros de impresión. Comuníquese con el administrador.', 'danger');", true);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

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

        //FUNCION PARA LLENAR EL COMBOBOX DE JORNADAS
        private void llenarComboOficinistas()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_oficinista, descripcion" + Environment.NewLine;
                sSql += "from ctt_oficinista" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "order by descripcion";

                comboE.ISSQL = sSql;
                cmbOficinista.DataSource = comboM.listarCombo(comboE);
                cmbOficinista.DataValueField = "IID";
                cmbOficinista.DataTextField = "IDATO";
                cmbOficinista.DataBind();
                cmbOficinista.Items.Insert(0, new ListItem("Todos", "0"));
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
                sFechaInicial = Convert.ToDateTime(txtFechaDesde.Text.Trim()).ToString("yyyy/MM/dd");
                sFechaFinal = Convert.ToDateTime(txtFechaHasta.Text.Trim()).ToString("yyyy/MM/dd");

                Session["fecha_desde"] = sFechaInicial;
                Session["fecha_hasta"] = sFechaFinal;

                sSql = "";
                sSql += "select * from ctt_vw_reporte_cobro_administracion" + Environment.NewLine;
                sSql += "where fecha_viaje between '" + sFechaInicial + "'" + Environment.NewLine;
                sSql += "and '" + sFechaFinal + "'" + Environment.NewLine;

                if (Convert.ToInt32(cmbVehiculos.SelectedValue) != 0)
                {
                    sSql += "and id_ctt_vehiculo = " + Convert.ToInt32(cmbVehiculos.SelectedValue) + Environment.NewLine;
                }

                if (Convert.ToInt32(cmbOficinista.SelectedValue) != 0)
                {
                    sSql += "and id_ctt_oficinista = " + Convert.ToInt32(cmbOficinista.SelectedValue) + Environment.NewLine;
                }
                
                sSql += "order by fecha_viaje, hora_salida";

                Session["instruccion"] = sSql;

                pagosE.ISQL = sSql;
                dgvDatos.DataSource = pagosM.listarRetenciones(pagosE);
                dgvDatos.DataBind();

                if (dgvDatos.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No existen registros con los datos ingresados para la búsqueda.', 'warning');", true);
                    lblSuma.Text = "Total Cobrado: 0.00 $";
                    //btnImprimir.Visible = false;
                }

                else
                {
                    dbSuma = 0;

                    for (int i = 0; i < dgvDatos.Rows.Count; i++)
                    {
                        dbSuma = dbSuma + Convert.ToDouble(dgvDatos.Rows[i].Cells[7].Text);
                    }

                    lblSuma.Text = "Total Cobrado: " + dbSuma.ToString("N2") + " $";

                    //btnImprimir.Visible = true;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }


        //FUNCION PARA CREAR LA CONSULTA DE IMPRESION
        private void imprimirReporte()
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(Session["instruccion"].ToString(), dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sImprimir = reporte.llenarBoleto(dtConsulta, Session["fecha_desde"].ToString(), Session["fecha_hasta"].ToString(), Session["usuario"].ToString());

                        if (sImprimir == "ERROR")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Ocurrió un problema al crear el reporte de impresión. Comuníquese con el administrador.', 'warning');", true);
                        }

                        else
                        {
                            consultarImpresora();
                        }
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No hay registros en la consulta para continuar con la impresión. Comuníquese con el administrador.', 'warning');", true);
                    }
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo crear el reporte de impresión. Comuníquese con el administrador.', 'danger');", true);
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
            lblSuma.Text = "Total Cobrado: 0.00 $";
            llenarComboVehiculos();
            llenarComboOficinistas();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            dgvDatos.DataSource = dtConsulta;
            dgvDatos.DataBind();

            //btnImprimir.Visible = false;

            Session["fecha_desde"] = null;
            Session["fecha_hasta"] = null;
            Session["instruccion"] = null;
        }

        #endregion

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            imprimirReporte();
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
    }
}