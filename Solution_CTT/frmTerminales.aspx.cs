using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using ENTIDADES;
using NEGOCIO;
using System.Data;

namespace Solution_CTT
{
    public partial class frmTerminales : System.Web.UI.Page
    {
        ENTTerminal terminalE = new ENTTerminal();
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorTerminal terminalM = new manejadorTerminal();
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sAccion;
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

            Session["modulo"] = "MÓDULO DE TERMINALES";

            if (!IsPostBack)
            {
                llenarGrid(0);
                llenarComboLocalidad();
                llenarComboProvincia();
            }
        }

        #region FUNCION DEL USUARIO

        //FUNCION PARA LAS COLUMNAS
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[6].Visible = ok;
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE LOCALIDAD
        private void llenarComboLocalidad()
        {
            try
            {
                sSql = "";
                sSql += "select id_localidad, nombre_localidad" + Environment.NewLine;
                sSql += "from tp_vw_localidades";

                comboE.ISSQL = sSql;
                cmbLocalidad.DataSource = comboM.listarCombo(comboE);
                cmbLocalidad.DataValueField = "IID";
                cmbLocalidad.DataTextField = "IDATO";
                cmbLocalidad.DataBind();
                cmbLocalidad.Items.Insert(0, new ListItem("Seleccione Localidad", "0"));
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', '" + ex.ToString() + "', 'danger');", true);
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE PROVINCIA
        private void llenarComboProvincia()
        {
            try
            {
                sSql = "";
                sSql += "select idsisprovincia, nombres" + Environment.NewLine;
                sSql += "from sisprovincia" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbProvincia.DataSource = comboM.listarCombo(comboE);
                cmbProvincia.DataValueField = "IID";
                cmbProvincia.DataTextField = "IDATO";
                cmbProvincia.DataBind();
                cmbProvincia.Items.Insert(0, new ListItem("Seleccione Provincia", "0"));
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', '" + ex.ToString() + "', 'danger');", true);
            }
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select T.id_ctt_terminal, T.id_localidad, VL.nombre_localidad," + Environment.NewLine;
                sSql += "T.idsisprovincia, P.nombres, T.codigo, T.descripcion, T.direccion," + Environment.NewLine;
                sSql += "case T.estado when 'A' then 'ACTIVO' else 'INACTIVO' end estado" + Environment.NewLine;
                sSql += "from ctt_terminal T INNER JOIN" + Environment.NewLine;
                sSql += "tp_vw_localidades VL ON VL.id_localidad = T.id_localidad" + Environment.NewLine;
                sSql += "and T.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "sisprovincia P ON P.idsisprovincia = T.idsisprovincia" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "where T.codigo like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or T.descripcion like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by T.id_ctt_terminal" + Environment.NewLine;

                columnasGrid(true);
                terminalE.ISSQL = sSql;
                dgvDatos.DataSource = terminalM.listarTerminales(terminalE);
                dgvDatos.DataBind();
                columnasGrid(false);
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', '" + ex.ToString() + "', 'danger');", true);
            }
        }

        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private void insertarRegistro()
        {
            try
            {
                if (consultarRegistro() > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'Ya existe un registro con el codigo ingresado.', 'warning');", true);
                    goto fin;
                }

                else if (consultarRegistro() == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ocurrió un problema al consultar el código para el registro.', 'danger');", true);
                    goto fin;
                }

                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sSql = "";
                sSql += "insert into ctt_terminal (" + Environment.NewLine;
                sSql += "id_localidad, idsisprovincia, codigo, descripcion, direccion," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(cmbLocalidad.SelectedValue) + ", " + Convert.ToInt32(cmbProvincia.SelectedValue) + ", '" + txtCodigo.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'" + txtDescripcion.Text.Trim().ToUpper() + "', '" + txtDireccion.Text.Trim() + "', 'A'," + Environment.NewLine;
                sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro ingresado correctamente', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', '" + ex.ToString() + "', 'danger');", true);
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
                sSql += "update ctt_terminal set" + Environment.NewLine;
                sSql += "id_localidad = " + Convert.ToInt32(cmbLocalidad.SelectedValue) + "," + Environment.NewLine;
                sSql += "idsisprovincia = " + Convert.ToInt32(cmbProvincia.SelectedValue) + "," + Environment.NewLine;
                sSql += "codigo = '" + txtCodigo.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "descripcion = '" + txtDescripcion.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "direccion = '" + txtDireccion.Text.Trim() + "'" + Environment.NewLine;
                sSql += "where id_ctt_terminal = " + Convert.ToInt32(Session["idRegistro"]) + Environment.NewLine;
                sSql += "and estado = 'A'";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro actualizado correctamente', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', '" + ex.ToString() + "', 'danger');", true);
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
                sSql += "update ctt_terminal set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_ctt_terminal = " + Convert.ToInt32(Session["idRegistro"]);

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro eliminado correctamente', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', '" + ex.ToString() + "', 'danger');", true);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); };

            fin: { };
        }

        //FUNCION PARA CONSULTAR SI EXISTE EL REGISTRO EN LA BASE DE DATOS
        private int consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from ctt_terminal" + Environment.NewLine;
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
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                    return -1;
                }
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', '" + ex.ToString() + "', 'danger');", true);
                return -1;
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtCodigo.Text = "";
            txtDescripcion.Text = "";
            txtDireccion.Text = "";
            Session["idRegistro"] = null;
            llenarComboLocalidad();
            llenarComboProvincia();
            btnSave.Text = "Crear";
            MsjValidarCampos.Visible = false;
            txtCodigo.ReadOnly = false;
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
                Session["idRegistro"] = dgvDatos.Rows[a].Cells[0].Text;

                if (sAccion == "Editar")
                {
                    txtCodigo.Text = dgvDatos.Rows[a].Cells[2].Text;
                    txtDescripcion.Text = dgvDatos.Rows[a].Cells[3].Text;
                    txtDireccion.Text = dgvDatos.Rows[a].Cells[4].Text;
                    cmbLocalidad.SelectedValue = dgvDatos.Rows[a].Cells[1].Text;
                    cmbProvincia.SelectedValue = dgvDatos.Rows[a].Cells[6].Text;
                }

                columnasGrid(false);
            }

            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', '" + ex.ToString() + "', 'danger');", true);
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

            else if (txtDireccion.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtDescripcion.Focus();
            }

            else if (Convert.ToInt32(cmbLocalidad.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbLocalidad.Focus();
            }

            else if (Convert.ToInt32(cmbProvincia.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbProvincia.Focus();
            }

            else
            {
                if (Session["idRegistro"] == null)
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
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + ex.ToString() + "', 'danger');", true);
            }
        }
    }
}