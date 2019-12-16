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

namespace Solution_CTT
{
    public partial class frmAsignarDatosLocalidadesContifico : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
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

            Session["modulo"] = "MóDULO DE ASIGNACIÓN DE VALORES A LAS LOCALIDADES";

            if (!IsPostBack)
            {
                llenarGrid();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid()
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_asignar_parametros_contifico";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }

                dgvDatos.Columns[0].Visible = true;
                dgvDatos.DataSource = dtConsulta;
                dgvDatos.DataBind();
                dgvDatos.Columns[0].Visible = false;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {

        }

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            
        }
    }
}