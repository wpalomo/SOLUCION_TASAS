using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Data;
using ENTIDADES;
using NEGOCIO;
using System.Drawing;

namespace Solution_Encomiendas
{
    public partial class frmDirectorio : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();
        ENTDirectorio directorioE = new ENTDirectorio();
        manejadorDirectorio directorioM = new manejadorDirectorio();

        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();

        string sSql;
        string[] sDatosMaximo = new string[5];
        string sAccion;

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

            Session["ModuloSistema"] = "Inicio / Facturación Electrónica / Configuración / Directorio";

            if (!IsPostBack)
            {
                llenarTipoComprobante();
                llenarGrid(0);
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LAS COLUMNAS DEL GRID
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[2].Visible = ok;
        }

        //LLENAR COMBO TIPO COMPROBANTE
        private void llenarTipoComprobante()
        {
            try
            {
                sSql = "";
                sSql += "select id_tipo_comprobante, nombres" + Environment.NewLine;
                sSql += "from cel_tipo_comprobante" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;

                cmbTipoCombrobante.DataSource = comboM.listarCombo(comboE);
                cmbTipoCombrobante.DataValueField = "IID";
                cmbTipoCombrobante.DataTextField = "IDATO";
                cmbTipoCombrobante.DataBind();
                cmbTipoCombrobante.Items.Insert(0, new ListItem(Resources.MESSAGES.TXT_CMB_TIPO_COMPROBANTE, "0"));

                if (cmbTipoCombrobante.Items.Count > 24)
                {
                    cmbTipoCombrobante.SelectedIndex = 24;
                }

                else
                {
                    cmbTipoCombrobante.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select D.id_directorio, D.id_tipo_comprobante, TC.nombres as nombre_tipo_comprobante, D.orden, D.codigo, D.nombres," + Environment.NewLine;
                sSql += "case D.estado when 'a' then 'ACTIVO' else 'ELIMINADO' end estado" + Environment.NewLine;
                sSql += "from cel_directorio D" + Environment.NewLine;
                sSql += "INNER JOIN cel_tipo_comprobante TC ON D.id_tipo_comprobante = TC.id_tipo_comprobante" + Environment.NewLine;
                sSql += "where D.estado = 'A' and TC.estado = 'A'";

                if (iOp == 1)
                {
                    sSql += "and D.codigo like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or D.orden like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by nombre_tipo_comprobante";

                columnasGrid(true);
                directorioE.ISQL = sSql;
                dgvDatos.DataSource = directorioM.listar(directorioE);
                dgvDatos.DataBind();
                columnasGrid(false);

            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONSULTAR SI EXISTE EL REGISTRO EN LA BASE DE DATOS
        private int consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from cel_directorio" + Environment.NewLine;
                sSql += "where id_tipo_comprobante = '" + cmbTipoCombrobante.SelectedValue.ToString() + "'" + Environment.NewLine;
                sSql += "or orden = '" + txtOrden.Text.Trim() + "'" + Environment.NewLine;
                sSql += "or codigo = '" + txtCodigo.Text.Trim() + "'" + Environment.NewLine;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return -1;
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
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sSql = "";
                sSql += "insert into cel_directorio (" + Environment.NewLine;
                sSql += "id_tipo_comprobante, orden, codigo, nombres, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "'" + cmbTipoCombrobante.SelectedValue.ToString() + "'," + Environment.NewLine;
                sSql += "'" + txtOrden.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'" + txtCodigo.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'" + txtDescripcion.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_MSJ_TITULO_EXITO + "', '" + Resources.MESSAGES.TXT_MSJ_BODY_EXITO_INSERT + "', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };

        fin: { };
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
                sSql += "update cel_directorio set" + Environment.NewLine;
                sSql += "orden = '" + txtOrden.Text.Trim().ToUpper() + "'" + Environment.NewLine;
                sSql += "codigo = '" + txtCodigo.Text.Trim().ToUpper() + "'" + Environment.NewLine;
                sSql += "nombres = '" + txtDescripcion.Text.Trim().ToUpper() + "'" + Environment.NewLine;
                sSql += "where id_directorio = " + Convert.ToInt32(Session["idRegistro"]) + Environment.NewLine;
                sSql += "and estado = 'A'";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_MSJ_TITULO_EXITO + "', '" + Resources.MESSAGES.TXT_MSJ_BODY_EXITO_UPDATE + "', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };

        fin: { };
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
                sSql += "update cel_directorio set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_directorio = " + Convert.ToInt32(Session["idRegistro"]);

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_MSJ_TITULO_EXITO + "', '" + Resources.MESSAGES.TXT_MSJ_BODY_EXITO_DELETE + "', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };

        fin: { };
        }

        //VAlIDAR
        private bool Validar()
        {
            if ((Convert.ToInt32(cmbTipoCombrobante.SelectedIndex) != -1) || string.IsNullOrEmpty(txtOrden.Text) || string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtDescripcion.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_ADVERTENCIA + " ', '" + Resources.MESSAGES.TXT_ADVERTENCIA_MSJ + "', 'warning');", true);
                return false;
            }
            else
            {
                return true;
            }
        }        

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtOrden.Text = "";
            txtCodigo.Text = "";
            txtDescripcion.Text = "";
            Session["idRegistro"] = null;
            btnSave.Text = "Crear";
            btnSave.Attributes.Add("Class", "form-control btn btn-block btn-primary");
            cmbTipoCombrobante.Enabled = true;
            cmbTipoCombrobante.Focus();
            llenarGrid(0);
            llenarTipoComprobante();
        }

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Validar() != true) return;
            if (btnSave.Text == "Crear")
            {
                insertarRegistro();
            }
            if (btnSave.Text == "Editar")
            {
                actualizarRegistro();
            }
            limpiar();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            sAccion = "E";//EDIT
            btnSave.Text = "Editar";
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            sAccion = "D";//DELETE
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
        }
        //ELIMINAR EL REGISTRO
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            eliminarRegistro();
        }
        protected void btnNo_Click(object sender, EventArgs e)
        {
            limpiar();
        }
        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);
                Session["idRegistro"] = dgvDatos.Rows[a].Cells[1].Text.Trim();

                if (sAccion == "E")
                {
                    cmbTipoCombrobante.SelectedValue = dgvDatos.Rows[a].Cells[2].Text;
                    txtOrden.Text = dgvDatos.Rows[a].Cells[4].Text.Trim();
                    txtCodigo.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[5].Text.Trim());
                    txtDescripcion.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[6].Text.Trim());

                    //CAMBIO ESTILO AL BOTON CREAR/EDITAR
                    btnSave.Text = "Editar";
                    btnSave.Attributes.Add("Class", "form-control btn btn-block btn-warning");
                    dgvDatos.SelectedRow.BackColor = System.Drawing.Color.FromName("#ccf0cb");//PINTO CELDA SELECCIONADA
                    cmbTipoCombrobante.Enabled = false;
                }

                columnasGrid(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }
        //FILTAR
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