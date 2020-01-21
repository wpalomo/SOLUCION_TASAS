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
    public partial class frmOficinistas : System.Web.UI.Page
    {
        ENTOficinista oficinistaE = new ENTOficinista();
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorOficinista oficinistaM = new manejadorOficinista();
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
        int iPermisos;
        int iBoleteria;
        int iEncomiendas;

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
                sSql += "from ctt_oficinista" + Environment.NewLine;
                sSql += "order by id_ctt_oficinista desc";

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
            dgvDatos.Columns[2].ItemStyle.Width = 100;
            dgvDatos.Columns[3].ItemStyle.Width = 250;
            dgvDatos.Columns[4].ItemStyle.Width = 250;
            dgvDatos.Columns[5].ItemStyle.Width = 100;
            dgvDatos.Columns[6].ItemStyle.Width = 100;
            dgvDatos.Columns[14].ItemStyle.Width = 100;
            dgvDatos.Columns[15].ItemStyle.Width = 100;

            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[7].Visible = ok;
            dgvDatos.Columns[8].Visible = ok;
            dgvDatos.Columns[9].Visible = ok;
            dgvDatos.Columns[10].Visible = ok;
            dgvDatos.Columns[11].Visible = ok;
            dgvDatos.Columns[12].Visible = ok;
            dgvDatos.Columns[13].Visible = ok;
        }
        
        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select O.id_ctt_oficinista, O.id_persona, O.codigo," + Environment.NewLine;
                sSql += "ltrim(isnull(TP.nombres, '') + ' ' + TP.apellidos) propietario, O.descripcion, O.usuario," + Environment.NewLine;
                sSql += "case O.estado when 'A' then 'ACTIVO' else 'ELIMINADO' end estado, O.claveacceso," + Environment.NewLine;
                sSql += "isnull(pos_secret, '') pos_secret, isnull(usuario_smartt, '') usuario_smartt," + Environment.NewLine;
                sSql += "isnull(claveacceso_smartt, '') claveacceso_smartt, isnull(privilegio, 0) privilegio," + Environment.NewLine;
                sSql += "isnull(acceso_boleteria, 0) acceso_boleteria, isnull(acceso_encomienda, 0) acceso_encomienda" + Environment.NewLine;
                sSql += "from tp_personas TP INNER JOIN" + Environment.NewLine;
                sSql += "ctt_oficinista O ON O.id_persona = TP.id_persona" + Environment.NewLine;
                sSql += "and O.estado = 'A'" + Environment.NewLine;
                sSql += "and TP.estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "where O.codigo like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or O.descripcion like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by O.id_ctt_oficinista" + Environment.NewLine;

                columnasGrid(true);
                oficinistaE.ISSQL = sSql;
                dgvDatos.DataSource = oficinistaM.listarOficinista(oficinistaE);
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
                sSql += "insert into ctt_oficinista (" + Environment.NewLine;
                sSql += "id_persona, codigo, descripcion, usuario, claveacceso, cambiar_clave," + Environment.NewLine;
                sSql += "pos_secret, usuario_smartt, claveacceso_smartt, privilegio," + Environment.NewLine;
                sSql += "acceso_boleteria, acceso_encomienda, estado, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Session["id_PersonaOFICINISTA"].ToString()) + ", '" + txtCodigo.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'" + txtDescripcion.Text.Trim().ToUpper() + "', '" + txtUsuario.Text.Trim().ToLower() + "', '" + txtUsuario.Text.Trim().ToLower() + "'," + Environment.NewLine;
                sSql += "0, '" + txtPostSecret.Text.Trim() + "', '" + txtUsuarioSmartt.Text.Trim().ToLower() + "',";
                sSql += "'" + txtPasswordSmartt.Text.Trim() + "', " + iPermisos + ", " + iBoleteria + ", " + Environment.NewLine;
                sSql += iEncomiendas + ", 'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

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
                sSql += "update ctt_oficinista set" + Environment.NewLine;
                sSql += "id_persona = " + Convert.ToInt32(Session["id_PersonaOFICINISTA"]) + "," + Environment.NewLine;
                sSql += "pos_secret = '" + txtPostSecret.Text.Trim() + "'," + Environment.NewLine;
                sSql += "descripcion = '" + txtDescripcion.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "usuario_smartt = '" + txtUsuarioSmartt.Text.Trim().ToLower() + "'," + Environment.NewLine;
                sSql += "claveacceso_smartt = '" + txtPasswordSmartt.Text.Trim() + "'," + Environment.NewLine;
                sSql += "privilegio = " + iPermisos + "," + Environment.NewLine;
                sSql += "acceso_boleteria = " + iBoleteria + "," + Environment.NewLine;
                sSql += "acceso_encomienda = " + iEncomiendas + Environment.NewLine;
                sSql += "where id_ctt_oficinista = " + Convert.ToInt32(Session["idRegistroOFICINISTA"]) + Environment.NewLine;
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
                sSql += "update ctt_oficinista set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_ctt_oficinista = " + Convert.ToInt32(Session["idRegistroOFICINISTA"]);

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
                sSql += "from ctt_oficinista" + Environment.NewLine;
                sSql += "where codigo = '" + txtCodigo.Text.Trim() + "'" + Environment.NewLine;
                sSql += "or usuario = '" + txtUsuario.Text.Trim().ToLower() + "'" + Environment.NewLine;
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
            txtUsuario.Text = "";
            TxtPersona.Text = "";
            txtPostSecret.Text = "";
            Session["idRegistroOFICINISTA"] = null;
            Session["id_PersonaOFICINISTA"] = null;
            txtUsuario.ReadOnly = false;
            MsjValidarCampos.Visible = false;
            btnSave.Text = "Crear";
            chkPermisos.Checked = false;
            chkBoleteria.Checked = false;
            chkEncomiendas.Checked = false;
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
                Session["idRegistroOFICINISTA"] = dgvDatos.Rows[a].Cells[0].Text.Trim();                

                if (sAccion == "Editar")
                {
                    Session["id_PersonaOFICINISTA"] = dgvDatos.Rows[a].Cells[1].Text.Trim();
                    txtCodigo.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[2].Text.Trim());
                    TxtPersona.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[3].Text.Trim());
                    txtDescripcion.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[4].Text.Trim());
                    txtUsuario.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[5].Text.Trim());
                    txtPostSecret.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[8].Text.Trim());
                    txtUsuarioSmartt.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[9].Text.Trim());
                    txtPasswordSmartt.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[10].Text.Trim());

                    if (Convert.ToInt32(dgvDatos.Rows[a].Cells[11].Text) == 0)
                        chkPermisos.Checked = false;
                    else
                        chkPermisos.Checked = true;

                    if (Convert.ToInt32(dgvDatos.Rows[a].Cells[12].Text) == 0)
                        chkBoleteria.Checked = false;
                    else
                        chkBoleteria.Checked = true;

                    if (Convert.ToInt32(dgvDatos.Rows[a].Cells[13].Text) == 0)
                        chkEncomiendas.Checked = false;
                    else
                        chkEncomiendas.Checked = true;

                    txtUsuario.ReadOnly = true;
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
                    Session["id_PersonaOFICINISTA"] = dgvFiltrarPersonas.Rows[a].Cells[0].Text.Trim();
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

            else if (txtUsuario.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtUsuario.Focus();
            }

            else if (TxtPersona.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                btnAbrirModalPersonas.Focus();
            }

            else
            {
                if (chkPermisos.Checked == true)
                    iPermisos = 1;
                else
                    iPermisos = 0;

                if (chkBoleteria.Checked == true)
                    iBoleteria = 1;
                else
                    iBoleteria = 0;

                if (chkEncomiendas.Checked == true)
                    iEncomiendas = 1;
                else
                    iEncomiendas = 0;

                if (Session["idRegistroOFICINISTA"] == null)
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