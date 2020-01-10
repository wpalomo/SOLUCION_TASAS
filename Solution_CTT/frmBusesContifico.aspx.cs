using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ENTIDADES;
using NEGOCIO;
using System.Drawing;
using Newtonsoft.Json;

namespace Solution_CTT
{
    public partial class frmBusesContifico : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        Clases_Contifico.ClaseBuses buses;
        Clase_Variables_Contifico.Buses bus;
        Clase_Variables_Contifico.ErrorRespuesta errorRespuesta;

        string sSql;
        string sAccion;
        string sRespuesta_A;
        string[] sDatosMaximo = new string[5];

        bool bRespuesta;

        DataTable dtConsulta;

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

            Session["modulo"] = "MÓDULO DE BUSES - SMARTT";

            if (!IsPostBack)
            {
                Session["pagina_buses"] = "1";
                llenarGrid(0);
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL GRID CON LA INFORMACIÓN
        private void llenarGrid(int iOp)
        {
            try
            {
                buses = new Clases_Contifico.ClaseBuses();

                sRespuesta_A = buses.recuperarJson(Session["tokenSMARTT"].ToString().Trim(), iOp, Convert.ToInt32(Session["pagina_buses"].ToString()));

                if (sRespuesta_A == "ERROR")
                {
                    if (buses.iTipoError == 1)
                    {
                        errorRespuesta = JsonConvert.DeserializeObject<Clase_Variables_Contifico.ErrorRespuesta>(buses.sError);
                        lblMensajeError.Text = "<b>SMARTT - Información:</b><br/><br/>" + errorRespuesta.detail.Trim(); ;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    }

                    else if (buses.iTipoError == 2)
                    {
                        lblMensajeError.Text = "<b>SMARTT - Información:</b><br/><br/>" + buses.sError;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    }

                    return;
                }

                if (sRespuesta_A == "ISNULL")
                {
                    ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "Popup", "swal('Información', 'No se proporcionaron credenciales de autenticación. Tasa de Usuario SMARTT', 'info');", true);
                    return;
                }

                Session["JsonBuses"] = sRespuesta_A;
                bus = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Buses>(sRespuesta_A);

                dgvDatos.DataSource = bus.results;
                dgvDatos.DataBind();

                if (bus.next == null)
                {
                    btnSiguiente.Visible = false;
                }

                else
                {
                    btnSiguiente.Visible = true;
                }

                if (bus.previous == null)
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

        //FUNCION PARA EXTRAER EL JSON DE LOCALIDADES
        private void consultarBusesJson()
        {
            try
            {
                buses = new Clases_Contifico.ClaseBuses();

                //sRespuesta_A = buses.recuperarJson(Session["tokenSMARTT"].ToString().Trim());

                if (sRespuesta_A == "ERROR")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'No se pudo obtener registros para la tasa de usuario SMARTT', 'error')</script>");
                    return;
                }

                if (sRespuesta_A == "ISNULL")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Información.!', 'No se proporcionaron credenciales de autenticación. Tasa de Usuario SMARTT', 'info')</script>");
                    return;
                }

                Session["JsonBuses"] = sRespuesta_A;
                bus = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Buses>(sRespuesta_A);

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                dtConsulta.Columns.Add("id");
                dtConsulta.Columns.Add("descripcion");

                for (int i = 0; i < bus.results.Length; i++)
                {
                    DataRow row = dtConsulta.NewRow();
                    row["id"] = (i + 1).ToString();
                    row["descripcion"] = bus.results[i].disco + " - " + bus.results[i].placa;
                    dtConsulta.Rows.Add(row);
                }

                cmbBuses.DataSource = dtConsulta;
                cmbBuses.DataValueField = "id";
                cmbBuses.DataTextField = "descripcion";
                cmbBuses.DataBind();

                postbackBuses();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION POSTBACK DEL COMBOBOX DE BUSES
        private void postbackBuses()
        {
            try
            {
                sRespuesta_A = Session["JsonBuses"].ToString();
                bus = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Buses>(sRespuesta_A);

                int iPosicion = Convert.ToInt32(cmbBuses.SelectedValue) - 1;

                for (int i = 0; i < bus.results.Length; i++)
                {
                    if (i == iPosicion)
                    {
                        txtDisco.Text = bus.results[i].disco.ToString();
                        txtPlaca.Text = bus.results[i].placa.Trim().ToUpper();

                        if (bus.results[i].conductor == null)
                        {
                            txtConductor.Text = "";
                        }

                        else
                        {
                            txtConductor.Text = bus.results[i].conductor.ToString();
                        }

                        txtCapacidad.Text = bus.results[i].capacidad.ToString();
                        txtAnioFabricacion.Text = bus.results[i].anio_fabricacion.ToString();
                        txtMarca.Text = bus.results[i].marca_nombre.Trim().ToUpper();

                        if (bus.results[i].fecha_emision_matricula == null)
                        {
                            txtFechaEmision.Text = "";
                        }

                        else
                        {
                            txtFechaEmision.Text = bus.results[i].fecha_emision_matricula.ToString();
                        }

                        if (bus.results[i].fecha_vencimiento_matricula == null)
                        {
                            txtFechaCaducidad.Text = "";
                        }

                        else
                        {
                            txtFechaCaducidad.Text = bus.results[i].fecha_vencimiento_matricula.ToString();
                        }
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

        #endregion

        protected void btnAccept_Click(object sender, EventArgs e)
        {

        }

        protected void cmbBuses_SelectedIndexChanged(object sender, EventArgs e)
        {
            postbackBuses();
        }

        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            int iPagina_A = Convert.ToInt32(Session["pagina_buses"].ToString()) - 1;
            
            if (iPagina_A == 1)
            {
                Session["pagina_buses"] = "1";
                llenarGrid(0);
            }

            else
            {
                Session["pagina_buses"] = iPagina_A.ToString();
                llenarGrid(1);
            }
        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            int iPagina_A = Convert.ToInt32(Session["pagina_buses"].ToString()) + 1;
            Session["pagina_buses"] = iPagina_A.ToString();
            llenarGrid(1);
        }
    }
}