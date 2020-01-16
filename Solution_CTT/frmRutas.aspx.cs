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
    public partial class frmRutas : System.Web.UI.Page
    {
        ENTRuta rutaE = new ENTRuta();
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorRuta rutaM = new manejadorRuta();
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sAccion;
        string sDescripcion;
        string []sDatosMaximo = new string[5];

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

            Session["modulo"] = "MÓDULO DE RUTAS";

            if (!IsPostBack)
            {
                consultarCodigoMaximo();
                llenarGrid(0);
                llenarComboDatos();
            }
        }

        #region FUNCION DEL USUARIO

        //FUNCION PARA OBTENER EL CODIGO SUPERIOR
        private void consultarCodigoMaximo()
        {
            try
            {
                sSql = "";
                sSql += "select top 1 isnull(codigo, '0') codigo" + Environment.NewLine;
                sSql += "from ctt_ruta" + Environment.NewLine;
                sSql += "order by id_ctt_ruta desc";

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
            dgvDatos.Columns[3].ItemStyle.Width = 100;
            dgvDatos.Columns[4].ItemStyle.Width = 200;
            dgvDatos.Columns[5].ItemStyle.Width = 200;
            dgvDatos.Columns[6].ItemStyle.Width = 200;
            dgvDatos.Columns[7].ItemStyle.Width = 100;

            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[2].Visible = ok;
            dgvDatos.Columns[8].Visible = ok;
        }

        //FUNCION PARA LLENAR EL COMOBOX DE CLIENTES
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
                cmbTerminalOrigen.DataSource = comboM.listarCombo(comboE);
                cmbTerminalOrigen.DataValueField = "IID";
                cmbTerminalOrigen.DataTextField = "IDATO";
                cmbTerminalOrigen.DataBind();
                cmbTerminalOrigen.Items.Insert(0, new ListItem("Terminal Origen", "0"));

                comboE.ISSQL = sSql;
                cmbTerminalDestino.DataSource = comboM.listarCombo(comboE);
                cmbTerminalDestino.DataValueField = "IID";
                cmbTerminalDestino.DataTextField = "IDATO";
                cmbTerminalDestino.DataBind();
                cmbTerminalDestino.Items.Insert(0, new ListItem("Terminal Destino", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select R.id_ctt_ruta, R.id_ctt_pueblo_origen, R.id_ctt_pueblo_destino," + Environment.NewLine;
                sSql += "R.codigo, PO.descripcion terminal_origen, PD.descripcion terminal_destino," + Environment.NewLine;
                sSql += "PO.descripcion + ' - ' + PD.descripcion descripcion," + Environment.NewLine;
                sSql += "case R.estado when 'A' then 'ACTIVO' else 'INACTIVO' end estado, isnull(R.via, '') via" + Environment.NewLine;
                sSql += "from ctt_pueblos PO INNER JOIN" + Environment.NewLine;
                sSql += "ctt_ruta R ON R.id_ctt_pueblo_origen = PO.id_ctt_pueblo" + Environment.NewLine;
                sSql += "and R.estado in ('A', 'N')" + Environment.NewLine;
                sSql += "and PO.estado = 'A' LEFT OUTER JOIN" + Environment.NewLine;
                sSql += "ctt_pueblos PD ON PD.id_ctt_pueblo = R.id_ctt_pueblo_destino" + Environment.NewLine;
                sSql += "and PD.estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "where R.codigo like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or R.descripcion like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by R.id_ctt_ruta" + Environment.NewLine;

                columnasGrid(true);
                rutaE.ISSQL = sSql;
                dgvDatos.DataSource = rutaM.listarRuta(rutaE);
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
                int iContarRegistros_P = consultarRegistro();

                if (iContarRegistros_P == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ocurrió un problema al consultar el código para el registro.', 'danger');", true);
                    return;
                }

                if (iContarRegistros_P > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'Ya existe un registro con el codigo ingresado.', 'warning');", true);
                    return;
                }                

                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sDescripcion = cmbTerminalOrigen.SelectedItem.ToString().ToUpper().Trim() + " - " + cmbTerminalDestino.SelectedItem.ToString().ToUpper().Trim();

                sSql = "";
                sSql += "insert into ctt_ruta (" + Environment.NewLine;
                sSql += "id_ctt_pueblo_origen, id_ctt_pueblo_destino, codigo, descripcion, via," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(cmbTerminalOrigen.SelectedValue) + ", " + Convert.ToInt32(cmbTerminalDestino.SelectedValue) + ", ";
                sSql += "'" + txtCodigo.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'" + sDescripcion + "', '" + txtVia.Text.Trim().ToUpper() + "', 'A'," + Environment.NewLine;
                sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

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

                sDescripcion = cmbTerminalOrigen.SelectedItem.ToString().ToUpper().Trim() + " - " + cmbTerminalDestino.SelectedItem.ToString().ToUpper().Trim();

                sSql = "";
                sSql += "update ctt_ruta set" + Environment.NewLine;
                sSql += "id_ctt_pueblo_origen = " + Convert.ToInt32(cmbTerminalOrigen.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_ctt_pueblo_destino = " + Convert.ToInt32(cmbTerminalDestino.SelectedValue) + "," + Environment.NewLine;
                sSql += "codigo = '" + txtCodigo.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "descripcion = '" + sDescripcion + "'," + Environment.NewLine;
                sSql += "via = '" + txtVia.Text.Trim().ToUpper() + "'" + Environment.NewLine;
                sSql += "where id_ctt_ruta = " + Convert.ToInt32(Session["idRegistroRutas"].ToString()) + Environment.NewLine;
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

            reversa: { conexionM.reversaTransaccion(); }
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
                sSql += "update ctt_ruta set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_ctt_ruta = " + Convert.ToInt32(Session["idRegistroRutas"].ToString());

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

        //FUNCION PARA CONSULTAR SI EXISTE EL REGISTRO EN LA BASE DE DATOS
        private int consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from ctt_ruta" + Environment.NewLine;
                sSql += "where codigo = '" + txtCodigo.Text.Trim() + "'" + Environment.NewLine;
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
            txtVia.Text = "";
            cmbTerminalOrigen.SelectedIndex = 0;
            cmbTerminalDestino.SelectedIndex = 0;
            Session["idRegistroRutas"] = null;
            btnSave.Text = "Crear";
            MsjValidarCampos.Visible = false;
            txtCodigo.Focus();
            llenarGrid(0);
        }

        #endregion

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);
                Session["idRegistroRutas"] = dgvDatos.Rows[a].Cells[0].Text;

                if (sAccion == "Editar")
                {
                    cmbTerminalOrigen.SelectedValue = dgvDatos.Rows[a].Cells[1].Text;
                    cmbTerminalDestino.SelectedValue = dgvDatos.Rows[a].Cells[2].Text;
                    txtCodigo.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[3].Text);
                    txtVia.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[8].Text);
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
            if (Convert.ToInt32(Session["privilegio"].ToString()) == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No tiene permisos para realizar esta acción.', 'warning');", true);
            }

            else
            {
                sAccion = "Eliminar";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
            }
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

            else if (Convert.ToInt32(cmbTerminalOrigen.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbTerminalOrigen.Focus();
            }

            else if (Convert.ToInt32(cmbTerminalDestino.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbTerminalDestino.Focus();
            }

            else
            {
                if (Session["idRegistroRutas"] == null)
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
    }
}