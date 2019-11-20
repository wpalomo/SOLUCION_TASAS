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
    public partial class frmDetalleRuta : System.Web.UI.Page
    {
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorDetalleRuta detalleM = new manejadorDetalleRuta();
        manejadorConexion conexionM = new manejadorConexion();
        ENTDetalleRuta detalleE = new ENTDetalleRuta();

        string sSql;
        string sAccion;
        string []sDatosMaximo = new string[5];

        DataTable dtConsulta;

        bool bRespuesta;

        int iIdProducto;

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

            Session["modulo"] = "MÓDULO DE DETALLE DE RUTAS";

            if (!IsPostBack)
            {
                datosListas();
                llenarComboDatos();
                llenarGrid(0);
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMOBOX DE TERMINALES
        private void llenarComboDatos()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_terminal, descripcion" + Environment.NewLine;
                sSql += "from ctt_terminal" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbTerminal.DataSource = comboM.listarCombo(comboE);
                cmbTerminal.DataValueField = "IID";
                cmbTerminal.DataTextField = "IDATO";
                cmbTerminal.DataBind();
                cmbTerminal.Items.Insert(0, new ListItem("Seleccione Terminal", "0"));
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + ex.ToString() + "', 'danger');", true);
            }
        }

        //FUNCION PARA OBTENER LOS DATOS DE LA LISTA BASE Y MINORISTA
        private void datosListas()
        {
            try
            {
                sSql = "";
                sSql += "select id_lista_precio, fecha_fin_validez" + Environment.NewLine;
                sSql += "from cv403_listas_precios" + Environment.NewLine;
                sSql += "where lista_base = 1" + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "union" + Environment.NewLine;
                sSql += "select id_lista_precio, fecha_fin_validez" + Environment.NewLine;
                sSql += "from cv403_listas_precios" + Environment.NewLine;
                sSql += "where lista_minorista = 1" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["lista_base"] = dtConsulta.Rows[0]["id_lista_precio"].ToString();
                        Session["fecha_base"] = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_fin_validez"].ToString()).ToString("yyyy/MM/dd");
                        Session["lista_minorista"] = dtConsulta.Rows[1]["id_lista_precio"].ToString();
                        Session["fecha_minorista"] = Convert.ToDateTime(dtConsulta.Rows[1]["fecha_fin_validez"].ToString()).ToString("yyyy/MM/dd");
                    }
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                }
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + ex.ToString() + "', 'danger');", true);
            }
        }

        //FUNCION PARA LAS COLUMNAS DEL GRID
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[2].Visible = ok;
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select DR.id_ctt_detalle_ruta, DR.id_ctt_terminal, DR.id_producto," + Environment.NewLine;
                sSql += "DR.codigo, T.descripcion origen, DR.descripcion destino," + Environment.NewLine;
                sSql += "ltrim(str(PP.valor, 10, 2)) valor" + Environment.NewLine;
                sSql += "from cv401_productos P, cv401_nombre_productos NP," + Environment.NewLine;
                sSql += "cv403_precios_productos PP, ctt_detalle_ruta DR," + Environment.NewLine;
                sSql += "ctt_terminal T" + Environment.NewLine;
                sSql += "where NP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and PP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and DR.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and DR.id_ctt_terminal = T.id_ctt_terminal" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "and PP.estado = 'A'" + Environment.NewLine;
                sSql += "and DR.estado = 'A'" + Environment.NewLine;
                sSql += "and T.estado = 'A'" + Environment.NewLine;
                sSql += "and PP.id_lista_precio = " + Convert.ToInt32(Session["lista_minorista"].ToString()) + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and DR.id_ctt_terminal = " + Convert.ToInt32(cmbTerminal.SelectedValue) + Environment.NewLine;
                }

                sSql += "order by DR.id_ctt_detalle_ruta";

                columnasGrid(true);
                detalleE.ISQL = sSql;
                dgvDatos.DataSource = detalleM.listarDetalleRuta(detalleE);
                dgvDatos.DataBind();
                columnasGrid(false);
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + ex.ToString() + "', 'danger');", true);
            }
        }

        #endregion

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);
                Session["idRegistro"] = dgvDatos.Rows[a].Cells[0].Text;

                if (sAccion == "Editar")
                {
                    cmbTerminal.SelectedValue = dgvDatos.Rows[a].Cells[1].Text;
                    txtCodigo.Text = dgvDatos.Rows[a].Cells[3].Text;
                    txtDescripcion.Text = dgvDatos.Rows[a].Cells[4].Text;
                    //txtNumeracion.Text = dgvDatos.Rows[a].Cells[5].Text;
                }

                columnasGrid(false);
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + ex.ToString() + "', 'danger');", true);
            }
        }
        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            sAccion = "Editar";
            btnSave.Text = "Editar";
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            sAccion = "Eliminar";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //limpiar();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (txtFiltrar.Text.Trim() == "")
            {
                llenarGrid(0);
            }

            else
            {
                llenarGrid(1);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtCodigo.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtCodigo.Focus();
            }

            //else if (txtNumeracion.Text.Trim() == "")
            //{
            //    MsjValidarCampos.Visible = true;
            //    txtNumeracion.Focus();
            //}

            else if (txtDescripcion.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtDescripcion.Focus();
            }

            else if (Convert.ToInt32(cmbTerminal.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbTerminal.Focus();
            }

            else
            {
                if (Session["idRegistro"] == null)
                {
                    //ENVIO A FUNCION DE INSERCION
                    //insertarRegistro();
                }

                else
                {
                    //actualizarRegistro();
                }
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            //eliminarRegistro();
        }

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvDatos.PageIndex = e.NewPageIndex;

                if (txtFiltrar.Text.Trim() == "")
                {
                    llenarGrid(0);
                }

                else
                {
                    llenarGrid(1);
                }
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + ex.ToString() + "', 'danger');", true);
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