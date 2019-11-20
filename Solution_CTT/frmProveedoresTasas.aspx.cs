using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ENTIDADES;
using NEGOCIO;
using System.Data;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmProveedoresTasas : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();
        ENTPasajeros personaE = new ENTPasajeros();
        manejadorPasajeros personaM = new manejadorPasajeros();

        string sSql;
        string sAccion;
        string sAccionFiltro;
        string sEstado;
        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;
        bool bRespuesta;

        int iConsultarRegistro;

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

            Session["modulo"] = "MÓDULO DE OFICINISTAS";

            if (!IsPostBack)
            {
                llenarGrid(0);
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

        #region FUNCION DEL USUARIO

        //FUNCION PARA LAS COLUMNAS
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[2].ItemStyle.Width = 100;
            dgvDatos.Columns[4].ItemStyle.Width = 250;
            dgvDatos.Columns[5].ItemStyle.Width = 100;

            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[3].Visible = ok;
            dgvDatos.Columns[6].Visible = ok;
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_proveedores_tasas" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "where codigo like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or descripcion like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by codigo" + Environment.NewLine;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                columnasGrid(true);

                dgvDatos.DataSource = dtConsulta;
                dgvDatos.DataBind();

                columnasGrid(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private void insertarRegistro()
        {
            try
            {
                iConsultarRegistro = consultarRegistro();

                if (iConsultarRegistro > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ya existe un registro con el codigo o usuario ingresado.', 'error');", true);
                    txtCodigo.Text = "";
                    txtCodigo.Focus();
                    return;
                }

                else if (iConsultarRegistro == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ocurrió un problema al consultar el código para el registro.', 'danger');", true);
                    txtCodigo.Text = "";
                    txtCodigo.Focus();
                    return;
                }

                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sSql = "";
                sSql += "insert into ctt_proveedores_tasas (" + Environment.NewLine;
                sSql += "id_persona, codigo, descripcion, estado, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Session["id_PersonaPTASA"].ToString()) + ", '" + txtCodigo.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'" + txtDescripcion.Text.Trim().ToUpper() + "', 'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro ingresado correctamente', 'success');", true);
                limpiar();
                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };

        }

        //FUNCION PARA ACTUALIZAR EN LA BASE DE DATOS
        private void actualizarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sSql = "";
                sSql += "update ctt_proveedores_tasas set" + Environment.NewLine;
                sSql += "id_persona = " + Convert.ToInt32(Session["id_PersonaPTASA"]) + "," + Environment.NewLine;
                sSql += "descripcion = '" + txtDescripcion.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "estado = '" + sEstado + "'" + Environment.NewLine;
                sSql += "where id_ctt_proveedor_tasa = " + Convert.ToInt32(Session["idRegistroPTASA"]);

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro actualizado correctamente', 'success');", true);
                limpiar();
                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };
        }

        //FUNCION PARA ELIMINAR EN LA BASE DE DATOS
        private void eliminarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sSql = "";
                sSql += "update ctt_proveedores_tasas set" + Environment.NewLine;
                sSql += "codigo = '" + Session["codigoPTASA"].ToString() + "." + Session["idRegistroPTASA"].ToString() + "'," + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_ctt_proveedor_tasa = " + Convert.ToInt32(Session["idRegistroPTASA"].ToString());

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro eliminado correctamente', 'success');", true);
                limpiar();
                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };
        }

        //FUNCION PARA CONSULTAR SI EXISTE EL REGISTRO EN LA BASE DE DATOS
        private int consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from ctt_proveedores_tasas" + Environment.NewLine;
                sSql += "where codigo = '" + txtCodigo.Text.Trim() + "'" + Environment.NewLine;
                sSql += "and estado in ('A', 'N')";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    return Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return -1;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return -1;
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtCodigo.Text = "";
            txtDescripcion.Text = "";
            TxtPersona.Text = "";
            Session["idRegistroPTASA"] = null;
            Session["id_PersonaPTASA"] = null;
            Session["codigoPTASA"] = null;
            txtCodigo.ReadOnly = false;
            btnSave.Text = "Crear";
            cmbEstado.SelectedValue = "A";
            llenarGrid(0);
        }

        #endregion

        //PARA EL GRID DE INFORMACION PRINCIPAL
        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);
                Session["idRegistroPTASA"] = dgvDatos.Rows[a].Cells[0].Text.Trim();

                if (sAccion == "Editar")
                {
                    Session["id_PersonaPTASA"] = dgvDatos.Rows[a].Cells[1].Text.Trim();
                    txtCodigo.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[2].Text.Trim());
                    TxtPersona.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[3].Text.Trim());
                    txtDescripcion.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[4].Text.Trim());
                    cmbEstado.SelectedValue = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[6].Text.Trim());
                    txtCodigo.ReadOnly = true;
                }

                else if (sAccion == "Eliminar")
                {
                    Session["codigoPTASA"] = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[2].Text.Trim());
                }

                columnasGrid(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //PARA FILTRAR LOS DATOS DEL CLIENTE
        protected void dgvFiltrarPersonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvFiltrarPersonas.SelectedIndex;
                columnasGridFiltro(true);

                if (sAccionFiltro == "Seleccion")
                {
                    Session["id_PersonaPTASA"] = dgvFiltrarPersonas.Rows[a].Cells[0].Text.Trim();
                    TxtPersona.Text = HttpUtility.HtmlDecode(dgvFiltrarPersonas.Rows[a].Cells[2].Text.Trim());
                    txtFiltrarPersonas.Text = "";
                    btnPopUp_ModalPopupExtender.Hide();
                }

                columnasGridFiltro(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnSeleccion_Click(object sender, EventArgs e)
        {
            sAccionFiltro = "Seleccion";
        }

        protected void btnCerrarModal_Click(object sender, EventArgs e)
        {
            btnPopUp_ModalPopupExtender.Hide();
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
                btnPopUp_ModalPopupExtender.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnAbrirModalPersonas_Click(object sender, EventArgs e)
        {
            btnPopUp_ModalPopupExtender.Show();
            llenarGridPersonas(0);
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
            if (txtCodigo.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtCodigo.Focus();
            }

            else if (txtDescripcion.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtDescripcion.Focus();
            }

            else if (TxtPersona.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                btnAbrirModalPersonas.Focus();
            }

            else
            {
                if (Session["idRegistroPTASA"] == null)
                {
                    //ENVIO A FUNCION DE INSERCION
                    insertarRegistro();
                }

                else
                {
                    sEstado = cmbEstado.SelectedValue;

                    actualizarRegistro();
                }
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            eliminarRegistro();
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