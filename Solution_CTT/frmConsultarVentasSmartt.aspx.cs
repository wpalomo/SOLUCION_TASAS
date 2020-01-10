using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution_CTT
{
    public partial class frmConsultarVentasSmartt : System.Web.UI.Page
    {
        string sRespuesta_A;

        DataTable dtConsulta;

        DateTime dtInicio;
        DateTime dtFinal;

        Clases_Contifico.ClaseConsultarVentas ventas;
        Clase_Variables_Contifico.ConsultarVentas venta;
        Clase_Variables_Contifico.ErrorRespuesta errorRespuesta;

        string[] sDatosMaximo = new string[5];

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

            Session["modulo"] = "MÓDULO DE CONSULTA DE VENTAS - SMARTT";

            if (!IsPostBack)
            {
                Session["pagina_ventas"] = "1";
                txtFechaFinal.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaInicial.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        #region

        //FUNCION PARA CONSTRUIR EL GRID
        private void construirGrid()
        {
            dtConsulta = new DataTable();
            dtConsulta.Columns.Add("id");
            dtConsulta.Columns.Add("asiento");
            dtConsulta.Columns.Add("identificacion");
            dtConsulta.Columns.Add("nombre");
            dtConsulta.Columns.Add("valor");
            dtConsulta.Columns.Add("tasa");
            dtConsulta.Columns.Add("estado_nombre");
        }

        //EJECUTAR CONSULTA POR RANGO DE FECHAS
        private void consultaPorFechas(int iOp)
        {
            try
            {
                ventas = new Clases_Contifico.ClaseConsultarVentas();

                sRespuesta_A = ventas.recuperarJsonPorFechas(Session["tokenSMARTT"].ToString().Trim(), Convert.ToDateTime(Session["fecha_inicial_consulta"].ToString()).ToString("yyyy-MM-dd"), Convert.ToDateTime(Session["fecha_final_consulta"].ToString()).ToString("yyyy-MM-dd"), iOp, Convert.ToInt32(Session["pagina_ventas"].ToString()));

                if (sRespuesta_A == "ERROR")
                {
                    if (ventas.iTipoError == 1)
                    {
                        errorRespuesta = JsonConvert.DeserializeObject<Clase_Variables_Contifico.ErrorRespuesta>(ventas.sError);
                        lblMensajeError.Text = "<b>SMARTT - Información:</b><br/><br/>" + errorRespuesta.detail.Trim(); ;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    }

                    else if (ventas.iTipoError == 2)
                    {
                        lblMensajeError.Text = "<b>SMARTT - Información:</b><br/><br/>" + ventas.sError;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    }

                    return;
                }

                if (sRespuesta_A == "ISNULL")
                {
                    ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "Popup", "swal('Información', 'No se proporcionaron credenciales de autenticación. Tasa de Usuario SMARTT', 'info');", true);
                    return;
                }

                Session["JsonConsultarVenta"] = sRespuesta_A;

                venta = new Clase_Variables_Contifico.ConsultarVentas();
                venta = JsonConvert.DeserializeObject<Clase_Variables_Contifico.ConsultarVentas>(sRespuesta_A);

                dgvDatos.DataSource = venta.results;
                dgvDatos.DataBind();

                if (venta.next == null)
                {
                    btnSiguiente.Visible = false;
                }

                else
                {
                    btnSiguiente.Visible = true;
                }

                if (venta.previous == null)
                {
                    btnAnterior.Visible = false;
                }

                else
                {
                    btnAnterior.Visible = true;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //EJECUTAR CONSULTA POR RANGO DE FECHAS
        private void consultaPorNumeroDocumento(int iOp)
        {
            try
            {
                ventas = new Clases_Contifico.ClaseConsultarVentas();

                sRespuesta_A = ventas.recuperarJsonPorNumeroDocumento(Session["tokenSMARTT"].ToString().Trim(), Session["numero_documento_consulta"].ToString().Trim(), iOp, Convert.ToInt32(Session["pagina_ventas"].ToString()));

                if (sRespuesta_A == "ERROR")
                {
                    if (ventas.iTipoError == 1)
                    {
                        errorRespuesta = JsonConvert.DeserializeObject<Clase_Variables_Contifico.ErrorRespuesta>(ventas.sError);
                        lblMensajeError.Text = "<b>SMARTT - Información:</b><br/><br/>" + errorRespuesta.detail.Trim(); ;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    }

                    else if (ventas.iTipoError == 2)
                    {
                        lblMensajeError.Text = "<b>SMARTT - Información:</b><br/><br/>" + ventas.sError;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    }

                    return;
                }

                if (sRespuesta_A == "ISNULL")
                {
                    ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "Popup", "swal('Información', 'No se proporcionaron credenciales de autenticación. Tasa de Usuario SMARTT', 'info');", true);
                    return;
                }

                Session["JsonConsultarVenta"] = sRespuesta_A;

                venta = new Clase_Variables_Contifico.ConsultarVentas();
                venta = JsonConvert.DeserializeObject<Clase_Variables_Contifico.ConsultarVentas>(sRespuesta_A);

                dgvDatos.DataSource = venta.results;
                dgvDatos.DataBind();

                if (venta.next == null)
                {
                    btnSiguiente.Visible = false;
                }

                else
                {
                    btnSiguiente.Visible = true;
                }

                if (venta.previous == null)
                {
                    btnAnterior.Visible = false;
                }

                else
                {
                    btnAnterior.Visible = true;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            int iPagina_A = Convert.ToInt32(Session["pagina_ventas"].ToString()) - 1;

            if (iPagina_A == 1)
            {
                Session["pagina_ventas"] = "1";

                if (rdbFechas.Checked == true)
                {                    
                    consultaPorFechas(0);
                }

                else
                {
                    consultaPorNumeroDocumento(0);
                }
            }

            else
            {
                Session["pagina_buses"] = iPagina_A.ToString();

                if (rdbFechas.Checked == true)
                {
                    consultaPorFechas(1);
                }

                else
                {
                    consultaPorNumeroDocumento(1);
                }
            }
        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            int iPagina_A = Convert.ToInt32(Session["pagina_ventas"].ToString()) + 1;
            Session["pagina_ventas"] = iPagina_A.ToString();

            if (rdbFechas.Checked == true)
            {
                consultaPorFechas(1);
            }

            else
            {
                consultaPorNumeroDocumento(1);
            }
        }

        protected void rdbNumeroDocumento_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbNumeroDocumento.Checked == true)
            {
                txtFechaInicial.ReadOnly = true;
                txtFechaFinal.ReadOnly = true;
                txtNumeroDocumento.ReadOnly = false;
                txtNumeroDocumento.Text = "";
                txtNumeroDocumento.Focus();
            }
        }

        protected void rdbFechas_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFechas.Checked == true)
            {
                txtFechaInicial.ReadOnly = false;
                txtFechaFinal.ReadOnly = false;
                txtNumeroDocumento.ReadOnly = true;
                txtNumeroDocumento.Text = "";
                txtFechaFinal.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaInicial.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            Session["pagina_ventas"] = "1";

            if (rdbFechas.Checked == true)
            {
                dtInicio = Convert.ToDateTime(txtFechaInicial.Text.Trim());
                dtFinal = Convert.ToDateTime(txtFechaFinal.Text.Trim());

                if (dtInicio > dtFinal)
                {
                    ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "Popup", "swal('Información.!', 'La fecha final no puede ser superior a la fecha final.', 'info');", true);
                    return;
                }

                Session["fecha_inicial_consulta"] = dtInicio.ToString("yyyy-MM-dd");
                Session["fecha_final_consulta"] = dtFinal.ToString("yyyy-MM-dd");

                rdbFechas.Enabled = false;
                rdbNumeroDocumento.Enabled = false;

                txtFechaInicial.ReadOnly = true;
                txtFechaFinal.ReadOnly = true;
                txtNumeroDocumento.ReadOnly = true;

                //AQUI ENVIAR LA SOLICITUD DE JSON
                consultaPorFechas(0);
            }

            else if (rdbNumeroDocumento.Checked == true)
            {
                if (txtNumeroDocumento.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número de documento de la Tasa de Usuario.', 'info');", true);
                    return;
                }

                //AQUI ENVIAR LA SOLICITUD DE JSON
                Session["numero_documento_consulta"] = txtNumeroDocumento.Text.Trim();

                rdbFechas.Enabled = false;
                rdbNumeroDocumento.Enabled = false;

                txtFechaInicial.ReadOnly = true;
                txtFechaFinal.ReadOnly = true;
                txtNumeroDocumento.ReadOnly = true;

                consultaPorNumeroDocumento(0);
            }
        }

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;

                int iIdGrid_A = Convert.ToInt32(dgvDatos.Rows[a].Cells[0].Text);
                sRespuesta_A = Session["JsonConsultarVenta"].ToString();

                venta = new Clase_Variables_Contifico.ConsultarVentas();
                venta = JsonConvert.DeserializeObject<Clase_Variables_Contifico.ConsultarVentas>(sRespuesta_A);

                for (int i = 0; i < venta.results.Count; i++)
                {
                    int iIdVenta = venta.results[i].id;

                    if (iIdGrid_A == iIdVenta)
                    {
                        construirGrid();

                        for (int j = 0; j < venta.results[i].boletos.Count; j++)
                        {
                            dtConsulta.Rows.Add(venta.results[i].boletos[j].id,
                                                venta.results[i].boletos[j].asiento,
                                                venta.results[i].boletos[j].pasajero.identificacion,
                                                venta.results[i].boletos[j].pasajero.nombre,
                                                venta.results[i].boletos[j].valor,
                                                venta.results[i].boletos[j].tasa,
                                                venta.results[i].boletos[j].estado_nombre                                
                                );
                        }

                        ModalPopupExtender_Vendidos.Show();
                        dgvVendidos.DataSource = dtConsulta;
                        dgvVendidos.DataBind();

                        break;
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvVendidos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvVendidos.PageIndex = e.NewPageIndex;
        }

        protected void btnCerrarModalVendidos_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Vendidos.Hide();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmConsultarVentasSmartt.aspx");
        }
    }
}