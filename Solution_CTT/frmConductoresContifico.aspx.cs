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
    public partial class frmConductoresContifico : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        Clases_Contifico.ClaseConductores conductores;
        Clase_Variables_Contifico.Conductores conductor;

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

            Session["modulo"] = "MÓDULO DE CONDUCTORES - SMARTT";

            if (!IsPostBack)
            {
                consultarConductoresJson();
                //llenarGrid();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA EXTRAER EL JSON DE FRECUENCIA
        private void consultarConductoresJson()
        {
            try
            {
                conductores = new Clases_Contifico.ClaseConductores();

                sRespuesta_A = conductores.recuperarJson(Session["tokenSMARTT"].ToString().Trim());

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

                Session["JsonConductores"] = sRespuesta_A;

                conductor = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Conductores>(sRespuesta_A);

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                dtConsulta.Columns.Add("id");
                dtConsulta.Columns.Add("descripcion");

                for (int i = 0; i < conductor.results.Length; i++)
                {
                    DataRow row = dtConsulta.NewRow();
                    row["id"] = conductor.results[i].id;
                    row["descripcion"] = conductor.results[i].nombre.Trim().ToUpper();
                    dtConsulta.Rows.Add(row);
                }

                cmbConductores.DataSource = dtConsulta;
                cmbConductores.DataValueField = "id";
                cmbConductores.DataTextField = "descripcion";
                cmbConductores.DataBind();

                postbackConductores();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION POSTBACK DEL COMBOBOX DE BUSES
        private void postbackConductores()
        {
            try
            {
                sRespuesta_A = Session["JsonConductores"].ToString();
                conductor = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Conductores>(sRespuesta_A);

                int iId_P = Convert.ToInt32(cmbConductores.SelectedValue);

                for (int i = 0; i < conductor.results.Length; i++)
                {
                    if (Convert.ToInt32(conductor.results[i].id) == iId_P)
                    {
                        txtTipo.Text = conductor.results[i].tipo.ToString();
                        txtTipoNombre.Text = conductor.results[i].tipo_nombre.Trim().ToUpper();
                        txtIdentificacion.Text = conductor.results[i].identificacion.Trim();
                        txtNombreConductor.Text = conductor.results[i].nombre.Trim().ToUpper();
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

        protected void cmbConductores_SelectedIndexChanged(object sender, EventArgs e)
        {
            postbackConductores();
        }
    }
}