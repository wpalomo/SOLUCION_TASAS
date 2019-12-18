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
    public partial class frmConsultaViajesContifico : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        Clases_Contifico.ClaseConsultaViajes viajesConsulta;
        Clase_Variables_Contifico.ConsultaViajes viaje;

        string sSql;
        string sAccion;
        string sFecha;
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

            Session["modulo"] = "MÓDULO DE CONSULTA DE VIAJES - SMARTT";

            if (!IsPostBack)
            {
                sFecha = DateTime.Now.ToString("dd/MM/yyyy");
                txtFecha.Text = sFecha;
                consultarViajesJson();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA EXTRAER EL JSON DE FRECUENCIA
        private void consultarViajesJson()
        {
            try
            {
                viajesConsulta = new Clases_Contifico.ClaseConsultaViajes();

                sFecha = Convert.ToDateTime(txtFecha.Text.Trim()).ToString("yyyy-MM-dd");
                sFecha = "fecha=" + sFecha;

                sRespuesta_A = viajesConsulta.recuperarJson(Session["tokenSMARTT"].ToString().Trim(), sFecha);

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

                Session["JsonViajesConsulta"] = sRespuesta_A;

                viaje = JsonConvert.DeserializeObject<Clase_Variables_Contifico.ConsultaViajes>(sRespuesta_A);

                dgvDatos.DataSource = viaje.results;
                dgvDatos.DataBind();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONSTRUIR EL GRID
        private void crearGrid()
        {
            try
            {

            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvDatos.PageIndex = e.NewPageIndex;
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

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            consultarViajesJson();
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {

        }
    }
}