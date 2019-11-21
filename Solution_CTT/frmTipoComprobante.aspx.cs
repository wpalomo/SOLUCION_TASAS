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
    public partial class frmTipoComprobante : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();
        ENTTipoComprobante tipoComprobanteE = new ENTTipoComprobante();
        manejadorTipoComprobante tipoComprobanteM = new manejadorTipoComprobante();

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

            Session["modulo"] = "MÓDULO DE TIPOS DE COMPROBANTES ELECTRÓNICOS";

            if (!IsPostBack)
            {
                llenarGrid(0);
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LAS COLUMNAS DEL GRID
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[1].Visible = ok;
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select id_tipo_comprobante, codigo, nombres, case estado when 'a' then 'ACTIVO' else 'ELIMINADO' end estado" + Environment.NewLine;
                sSql += "from cel_tipo_comprobante" + Environment.NewLine;
                sSql += "where estado = 'A'";

                if (iOp == 1)
                {
                    sSql += "and codigo like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or nombres like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by codigo";

                columnasGrid(true);
                tipoComprobanteE.ISQL = sSql;
                dgvDatos.DataSource = tipoComprobanteM.listar(tipoComprobanteE);
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
                sSql += "insert into cel_tipo_comprobante (" + Environment.NewLine;
                sSql += "codigo, nombres, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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
                sSql += "update cel_tipo_comprobante set" + Environment.NewLine;
                sSql += "nombres = '" + txtDescripcion.Text.Trim().ToUpper() + "'" + Environment.NewLine;
                sSql += "where id_tipo_comprobante = " + Convert.ToInt32(Session["idRegistro"]) + Environment.NewLine;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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
                sSql += "update cel_tipo_comprobante set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_tipo_comprobante = " + Convert.ToInt32(Session["idRegistro"]);

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };

        fin: { };
        }

        //VAlIDAR
        private bool Validar()
        {
            if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtDescripcion.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_ADVERTENCIA + " ', '" + Resources.MESSAGES.TXT_ADVERTENCIA_MSJ + "', 'warning');", true);
                return false;
            }
            else
            {
                return true;
            }
        }

        //FUNCION PARA CONSULTAR SI EXISTE EL REGISTRO EN LA BASE DE DATOS
        private int consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from cel_tipo_comprobante" + Environment.NewLine;
                sSql += "where codigo = '" + txtCodigo.Text.Trim() + "'" + Environment.NewLine;
                sSql += "or nombres = '" + txtDescripcion.Text.Trim() + "'" + Environment.NewLine;
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
            txtCodigo.Text = "";
            txtDescripcion.Text = "";
            Session["idRegistro"] = null;
            btnSave.Text = "Crear";
            btnSave.Attributes.Add("Class", "form-control btn btn-block btn-primary");
            txtCodigo.ReadOnly = false;
            txtCodigo.Focus();
            llenarGrid(0);
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
                    txtCodigo.Text = dgvDatos.Rows[a].Cells[2].Text.Trim();
                    txtDescripcion.Text = dgvDatos.Rows[a].Cells[3].Text.Trim();

                    //CAMBIO ESTILO AL BOTON CREAR/EDITAR
                    btnSave.Text = "Editar";
                    btnSave.Attributes.Add("Class", "form-control btn btn-block btn-warning");
                    dgvDatos.SelectedRow.BackColor = System.Drawing.Color.FromName("#ccf0cb");//PINTO CELDA SELECCIONADA
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
        protected void dgvDatos_PageIndexChanging1(object sender, GridViewPageEventArgs e)
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