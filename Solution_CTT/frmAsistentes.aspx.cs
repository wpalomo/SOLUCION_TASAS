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
    public partial class frmAsistentes : System.Web.UI.Page
    {
        ENTAsistente asistenteE = new ENTAsistente();
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorAsistente asistenteM = new manejadorAsistente();
        manejadorConexion conexionM = new manejadorConexion();
        ENTPasajeros personaE = new ENTPasajeros();
        manejadorPasajeros personaM = new manejadorPasajeros();

        string sSql;
        string sAccion;
        string sAccionFiltro;
        string []sDatosMaximo = new string[5];

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

            Session["modulo"] = "MÓDULO DE ASISTENTES";

            if (!IsPostBack)
            {
                consultarCodigoMaximo();
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

        //FUNCION PARA OBTENER EL CODIGO SUPERIOR
        private void consultarCodigoMaximo()
        {
            try
            {
                sSql = "";
                sSql += "select top 1 isnull(codigo, '0') codigo" + Environment.NewLine;
                sSql += "from ctt_asistente" + Environment.NewLine;
                sSql += "order by id_ctt_asistente desc";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    txtCodigo.Text = (Convert.ToInt32(dtConsulta.Rows[0]["codigo"].ToString()) + 1).ToString();
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

        //FUNCION PARA LAS COLUMNAS
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[3].Visible = ok;
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select A.id_ctt_asistente, A.id_persona, A.codigo," + Environment.NewLine;
                sSql += "ltrim(isnull(TP.nombres, '') + ' ' + TP.apellidos) propietario, A.descripcion," + Environment.NewLine;
                sSql += "case A.estado when 'a' then 'ACTIVO' else 'ELIMINADO' end estado" + Environment.NewLine;
                sSql += "from tp_personas TP INNER JOIN" + Environment.NewLine;
                sSql += "ctt_asistente A ON A.id_persona = TP.id_persona" + Environment.NewLine;
                sSql += "and A.estado = 'A'" + Environment.NewLine;
                sSql += "and TP.estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "where A.codigo like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or A.descripcion like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by A.id_ctt_asistente" + Environment.NewLine;

                columnasGrid(true);
                asistenteE.ISSQL = sSql;
                dgvDatos.DataSource = asistenteM.listarAsistente(asistenteE);
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
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Registros Duplicados.!', 'Ya existe un registro con el Código o Nombre de Asistente.', 'error');", true);
                    txtCodigo.Text = "";
                    txtCodigo.Focus();
                    goto fin;
                }

                else if (iConsultarRegistro == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ocurrió un problema al consultar el código para el registro.', 'danger');", true);
                    txtCodigo.Text = "";
                    txtCodigo.Focus();
                    goto fin;
                }

                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'error');", true);
                    goto fin;
                }

                sSql = "";
                sSql += "insert into ctt_asistente (" + Environment.NewLine;
                sSql += "id_persona, codigo, descripcion, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Session["id_PersonaAsistente"].ToString()) + ", '" + txtCodigo.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'" + txtDescripcion.Text.Trim().ToUpper() + "', 'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "')";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro ingresado correctamente', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); }

            fin: { }
        }

        //FUNCION PARA ACTUALIZAR EN LA BASE DE DATOS
        private void actualizarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sSql = "";
                sSql += "update ctt_asistente set" + Environment.NewLine;
                sSql += "id_persona = " + Convert.ToInt32(Session["id_PersonaAsistente"].ToString()) + "," + Environment.NewLine;
                sSql += "codigo = '" + txtCodigo.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "descripcion = '" + txtDescripcion.Text.Trim().ToUpper() + "'" + Environment.NewLine;
                sSql += "where id_ctt_asistente = " + Convert.ToInt32(Session["idRegistroAsistente"]) + Environment.NewLine;
                sSql += "and estado = 'A'";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro actualizado correctamente', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); }

            fin: { }
        }

        //FUNCION PARA ELIMINAR EN LA BASE DE DATOS
        private void eliminarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sSql = "";
                sSql += "update ctt_asistente set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_ctt_asistente = " + Convert.ToInt32(Session["idRegistroAsistente"]);

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro eliminado correctamente', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); }

            fin: { }
        }

        //FUNCION PARA CONSULTAR SI EXISTE EL REGISTRO EN LA BASE DE DATOS
        private int consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from ctt_asistente" + Environment.NewLine;
                sSql += "where codigo = '" + txtCodigo.Text.Trim() + "'" + Environment.NewLine;
                sSql += "or id_persona = " + Convert.ToInt32(Session["id_PersonaAsistente"]) + Environment.NewLine;
                sSql += "and estado = 'A'";

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
            consultarCodigoMaximo();
            txtDescripcion.Text = "";
            TxtPersona.Text = "";
            Session["idRegistroAsistente"] = null;
            btnSave.Text = "Crear";
            MsjValidarCampos.Visible = false;
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
                Session["idRegistroAsistente"] = dgvDatos.Rows[a].Cells[0].Text.Trim();
                Session["id_PersonaAsistente"] = dgvDatos.Rows[a].Cells[1].Text.Trim();

                if (sAccion == "E")
                {
                    txtCodigo.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[2].Text.Trim());
                    TxtPersona.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[3].Text.Trim());
                    txtDescripcion.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[4].Text.Trim());
                    txtCodigo.ReadOnly = true;
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
                    Session["id_PersonaAsistente"] = dgvFiltrarPersonas.Rows[a].Cells[0].Text.Trim();
                    TxtPersona.Text = HttpUtility.HtmlDecode(dgvFiltrarPersonas.Rows[a].Cells[2].Text.Trim());
                    txtFiltrarPersonas.Text = "";
                    btnPopUp_ModalPopupExtender.Hide();
                }

                columnasGridFiltro(false);
            }

            catch (Exception ex)
            {
                btnPopUp_ModalPopupExtender.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnSeleccion_Click(object sender, EventArgs e)
        {
            sAccionFiltro = "Seleccion";
        }

        //protected void lbtnEditarPersona_Click(object sender, EventArgs e)
        //{
        //    sAccionFiltro = "E";
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
        //}

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
            sAccion = "E";
            btnSave.Text = "Editar";
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["privilegio"].ToString()) == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No tiene permisos para realizar esta acción.', 'warning');", true);
            }

            else
            {
                sAccion = "D";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtCodigo.Text = "";
            txtDescripcion.Text = "";
            TxtPersona.Text = "";
            Session["idRegistroAsistente"] = null;
            btnSave.Text = "Crear";
            MsjValidarCampos.Visible = false;
            txtCodigo.ReadOnly = false;
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
                if (Session["idRegistroAsistente"] == null)
                {
                    //ENVIO A FUNCION DE INSERCION
                    insertarRegistro();
                }
                else
                {
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
                btnPopUp_ModalPopupExtender.Hide();
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