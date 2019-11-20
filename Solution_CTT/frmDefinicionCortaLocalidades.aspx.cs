using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NEGOCIO;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmDefinicionCortaLocalidades : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sAccion;
        string sFecha;
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

            Session["modulo"] = "MÓDULO DE OFICINAS";

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
                sSql += "select correlativo, codigo, valor_texto," + Environment.NewLine;
                sSql += "case estado when 'A' then 'ACTIVO' else 'ELIMINADO' end estado" + Environment.NewLine;
                sSql += "from tp_codigos" + Environment.NewLine;
                sSql += "where tabla = 'SYS$00019'" + Environment.NewLine;

                if (rdbActivos.Checked == true)
                {
                    sSql += "and estado = 'A'";
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
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

        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private void insertarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sFecha = DateTime.Now.ToString("yyyy/MM/dd");

                sSql = "";
                sSql += "insert into tp_codigos (" + Environment.NewLine;
                sSql += "tabla, codigo, valor_texto, valor_fecha, valor_numero, estado," + Environment.NewLine;
                sSql += "login, fecha, fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                sSql += "numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "'SYS$00019', '" + txtCodigo.Text.Trim() + "', '" + txtDescripcion.Text.Trim() + "'," + Environment.NewLine;
                sSql += "null, 0, 'A', '', '" + sFecha + "', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 0, 0)";

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

            reversa: { conexionM.reversaTransaccion(); }
        }

        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private void actualizarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sFecha = DateTime.Now.ToString("yyyy/MM/dd");

                sSql = "";
                sSql += "update tp_codigos set" + Environment.NewLine;
                sSql += "codigo = '" + txtCodigo.Text.Trim()  + "'," + Environment.NewLine;
                sSql += "valor_texto = '" + txtDescripcion.Text.Trim() + "'," + Environment.NewLine;
                sSql += "estado = '" + cmbEstado.SelectedValue + "'" + Environment.NewLine;
                sSql += "where correlativo = " + Session["idRegistroDCL"].ToString();

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

            reversa: { conexionM.reversaTransaccion(); }
        }

        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private void eliminarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sFecha = DateTime.Now.ToString("yyyy/MM/dd");

                sSql = "";
                sSql += "update tp_codigos set" + Environment.NewLine;
                sSql += "estado = 'E'" + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where correlativo = " + Session["idRegistroDCL"].ToString();

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

        reversa: { conexionM.reversaTransaccion(); }
        }

        //FUNCION PARA LIMPIAR 
        private void limpiar()
        {           

            Session["idRegistroDCL"] = null;
            txtCodigo.Text = "";
            txtDescripcion.Text = "";
            cmbEstado.SelectedValue = "A";
            MsjValidarCampos.Visible = false;
            btnSave.Text = "Crear";
            txtCodigo.ReadOnly = false;
            llenarGrid();
        }

        #endregion

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                dgvDatos.Columns[0].Visible = true;
                Session["idRegistroDCL"] = dgvDatos.Rows[a].Cells[0].Text;

                if (sAccion == "Editar")
                {
                    txtCodigo.Text = dgvDatos.Rows[a].Cells[1].Text.Trim();
                    txtDescripcion.Text = dgvDatos.Rows[a].Cells[2].Text.Trim();

                    if (dgvDatos.Rows[a].Cells[3].Text.Trim().ToUpper() == "ACTIVO")
                    {
                        cmbEstado.SelectedValue = "A";
                    }

                    else
                    {
                        cmbEstado.SelectedValue = "E";
                    }
                }

                dgvDatos.Columns[0].Visible = false;

                txtCodigo.ReadOnly = true;
                txtCodigo.Focus();
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
                llenarGrid();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtCodigo.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el código.', 'info');", true);
                txtCodigo.Focus();
                return;
            }

            if (txtDescripcion.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese la descripción', 'info');", true);
                txtDescripcion.Focus();
                return;
            }

            if (Session["idRegistroDCL"] == null)
            {
                insertarRegistro();
            }

            else
            {
                actualizarRegistro();
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            eliminarRegistro();
        }

        protected void rdbActivos_CheckedChanged(object sender, EventArgs e)
        {
            llenarGrid();
        }

        protected void rdbTodos_CheckedChanged(object sender, EventArgs e)
        {
            llenarGrid();
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
