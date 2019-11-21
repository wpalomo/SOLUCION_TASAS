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
    public partial class frmPreciosPasajes : System.Web.UI.Page
    {
        ENTPrecioPasajes precioE = new ENTPrecioPasajes();
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorPrecioPasajes precioM = new manejadorPrecioPasajes();
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sAccion;
        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;

        bool bRespuesta;

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

            this.Session["modulo"] = "MÓDULO DE PRECIOS DE VIAJES NORMALES";

            if (!IsPostBack)
            {
                llenarComboDatos();
                datosListas();
                llenarGrid(0);
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMOBOX DE ORIGEN Y DESTINO
        private void llenarComboDatos()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_pueblo, descripcion" + Environment.NewLine;
                sSql += "from ctt_pueblos" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and terminal = 1";

                comboE.ISSQL = sSql;

                cmbOrigen.DataSource = comboM.listarCombo(comboE);
                cmbOrigen.DataValueField = "IID";
                cmbOrigen.DataTextField = "IDATO";
                cmbOrigen.DataBind();
                cmbOrigen.Items.Insert(0, new ListItem("Seleccione Origen", "0"));

                cmbDestino.DataSource = comboM.listarCombo(comboE);
                cmbDestino.DataValueField = "IID";
                cmbDestino.DataTextField = "IDATO";
                cmbDestino.DataBind();
                cmbDestino.Items.Insert(0, new ListItem("Seleccione Destino", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
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

        //FUNCION PARA LAS COLUMNAS DEL GRID
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[2].Visible = ok;
            dgvDatos.Columns[3].Visible = ok;
            dgvDatos.Columns[4].Visible = ok;

            dgvDatos.Columns[5].ItemStyle.Width = 100;
            dgvDatos.Columns[6].ItemStyle.Width = 300;
            dgvDatos.Columns[7].ItemStyle.Width = 100;
            dgvDatos.Columns[8].ItemStyle.Width = 100;
            dgvDatos.Columns[9].ItemStyle.Width = 100;
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select P.id_producto, PU.id_ctt_pueblo, P.id_ctt_pueblo_origen," + Environment.NewLine;
                sSql += "id_ctt_pueblo_destino, P.paga_iva, P.codigo, NP.nombre," + Environment.NewLine;
                sSql += "ltrim(str(PP.valor, 10, 2)) valor" + Environment.NewLine;
                sSql += "from cv401_productos P, cv401_nombre_productos NP," + Environment.NewLine;
                sSql += "cv403_precios_productos PP, ctt_pueblos PU" + Environment.NewLine;
                sSql += "where P.id_ctt_pueblo_origen = PU.id_ctt_pueblo" + Environment.NewLine;
                sSql += "and NP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and PP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "and PP.estado = 'A'" + Environment.NewLine;
                sSql += "and PU.estado = 'A'" + Environment.NewLine;
                sSql += "and PP.id_lista_precio = " + Convert.ToInt32(Session["lista_minorista"].ToString()) + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and PU.terminal = 1" + Environment.NewLine;
                    sSql += "and P.id_ctt_pueblo_origen = 1" + Environment.NewLine;
                }

                sSql += "order by P.id_producto";

                columnasGrid(true);
                precioE.ISQL = sSql;
                dgvDatos.DataSource = precioM.listarPrecioPasajes(precioE);
                dgvDatos.DataBind();
                columnasGrid(false);

            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtCodigo.Text = "";
            txtDescripcion.Text = "NINGUNO - NINGUNO";
            txtValor.Text = "";
            Session["idRegistro"] = null;
            llenarComboDatos();
            btnSave.Text = "Crear";
            MsjValidarCampos.Visible = false;
            txtCodigo.ReadOnly = false;
            txtCodigo.Focus();
            llenarGrid(0);
        }

        //VALIDAR COMBOBOX
        private void validarCombos()
        {
            try
            {
                if ((Convert.ToInt32(cmbOrigen.SelectedValue) == 0) && (Convert.ToInt32(cmbDestino.SelectedValue) == 0))
                {
                    txtDescripcion.Text = "NINGUNO - NINGUNO";
                }

                else if ((Convert.ToInt32(cmbOrigen.SelectedValue) == 0) && (Convert.ToInt32(cmbDestino.SelectedValue) != 0))
                {
                    txtDescripcion.Text = "NINGUNO - " + cmbDestino.SelectedItem.ToString();
                }

                else if ((Convert.ToInt32(cmbOrigen.SelectedValue) != 0) && (Convert.ToInt32(cmbDestino.SelectedValue) == 0))
                {
                    txtDescripcion.Text = cmbDestino.SelectedItem.ToString() + " - NINGUNO";
                }

                else
                {
                    txtDescripcion.Text = cmbOrigen.SelectedItem.ToString() + " - " + cmbDestino.SelectedItem.ToString();
                }
            }

            catch(Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
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
                    cmbOrigen.SelectedValue = dgvDatos.Rows[a].Cells[2].Text;
                    cmbDestino.SelectedValue = dgvDatos.Rows[a].Cells[3].Text;

                    if (dgvDatos.Rows[a].Cells[4].Text == "1")
                    {
                        chkPagaIva.Checked = true;
                    }

                    else
                    {
                        chkPagaIva.Checked = false;
                    }

                    txtCodigo.Text = dgvDatos.Rows[a].Cells[5].Text;
                    txtDescripcion.Text = dgvDatos.Rows[a].Cells[6].Text;
                    txtValor.Text = dgvDatos.Rows[a].Cells[7].Text;
                }

                columnasGrid(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
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
            limpiar();
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
            if (Convert.ToInt32(cmbOrigen.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbOrigen.Focus();
            }

            else if (Convert.ToInt32(cmbDestino.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbDestino.Focus();
            }

            else if (txtCodigo.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtCodigo.Focus();
            }

            else if (txtDescripcion.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtDescripcion.Focus();
            }

            else if (txtValor.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtValor.Focus();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void cmbOrigen_SelectedIndexChanged(object sender, EventArgs e)
        {
            validarCombos();
        }

        protected void cmbDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            validarCombos();
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