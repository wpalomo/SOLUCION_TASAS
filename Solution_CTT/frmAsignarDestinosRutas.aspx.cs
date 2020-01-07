using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NEGOCIO;
using System.Data;

namespace Solution_CTT
{
    public partial class frmAsignarDestinosRutas : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;

        bool bRespuesta;

        int iIdRuta;
        int iIdDestino;
        int iIdProducto;
        int iIdProductoRecuperado;
        int iHabilitado;

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

            Session["modulo"] = "MÓDULO DE ASIGNACIÓN DE DESTINOS POR RUTA";

            if (!IsPostBack)
            {
                limpiar();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL GRID
        private void llenarGridRutas()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_ruta, descripcion, via" + Environment.NewLine;
                sSql += "from ctt_ruta" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and id_ctt_pueblo_origen = " + Session["id_pueblo"].ToString();

                if (txtFiltrar.Text.Trim() != "")
                {
                    sSql += Environment.NewLine + "and descripcion like '%" + txtFiltrar.Text.Trim() + "%'";
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {

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

        //LLENAR GRID DESTINOS
        private void llenarDestinos()
        {
            try
            {
                sSql = "";
                sSql += "select id_producto, nombre" + Environment.NewLine;
                sSql += "from ctt_vw_asignar_destinos_rutas" + Environment.NewLine;
                sSql += "where id_ctt_pueblo_origen = " + Session["id_pueblo"].ToString();

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {

                    return;
                }

                dgvDestinos.Columns[0].Visible = true;
                dgvDestinos.DataSource = dtConsulta;
                dgvDestinos.DataBind();
                dgvDestinos.Columns[0].Visible = false;
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
            llenarGridRutas();
            llenarDestinos();
            lblRuta.Text = "Seleccione Ruta";
            Session["idRutaAsignacion"] = null;
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

                iIdRuta = Convert.ToInt32(Session["idRutaAsignacion"].ToString());

                sSql = "";
                sSql += "update ctt_ruta_destinos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_ctt_ruta = " + iIdRuta;

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    conexionM.reversaTransaccion();
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                dgvDestinos.Columns[0].Visible = true;

                foreach (GridViewRow row in dgvDestinos.Rows)
                {
                    iIdDestino = Convert.ToInt32(row.Cells[0].Text);

                    CheckBox chkSeleccion = (CheckBox)row.FindControl("chkSeleccionar");

                    if (chkSeleccion.Checked == true)
                    {
                        sSql = "";
                        sSql += "insert into ctt_ruta_destinos (" + Environment.NewLine;
                        sSql += "id_ctt_ruta, id_producto, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                        sSql += "values (" + Environment.NewLine;
                        sSql += iIdRuta + ", " + iIdDestino + ", 'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                        //EJECUCION DE LA INSTRUCCION SQL
                        if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                        {
                            dgvDestinos.Columns[0].Visible = false;
                            conexionM.reversaTransaccion();
                            lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                            return;
                        }
                    }
                }

                dgvDestinos.Columns[0].Visible = false;

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registros procesado éxitosamente.', 'success');", true);
                limpiar();
                return;
            }

            catch (Exception ex)
            {
                conexionM.reversaTransaccion();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONSULTAR LOS REGISTROS
        private void consultarRegistrosRutas(int iIdRuta_P)
        {
            try
            {
                sSql = "";
                sSql += "select id_producto" + Environment.NewLine;
                sSql += "from ctt_ruta_destinos" + Environment.NewLine;
                sSql += "where id_ctt_ruta = " + iIdRuta_P + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    foreach (GridViewRow row in dgvDestinos.Rows)
                    {
                        CheckBox chkSeleccion = (CheckBox)row.FindControl("chkSeleccionar");
                        chkSeleccion.Checked = false;
                    }

                    return;
                }

                dgvDestinos.Columns[0].Visible = true;

                foreach (GridViewRow row in dgvDestinos.Rows)
                {
                    iIdProducto = Convert.ToInt32(row.Cells[0].Text);
                    CheckBox chkSeleccion = (CheckBox)row.FindControl("chkSeleccionar");

                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        iIdProductoRecuperado = Convert.ToInt32(dtConsulta.Rows[i]["id_producto"].ToString());

                        if (iIdProducto == iIdProductoRecuperado)
                        {
                            chkSeleccion.Checked = true;
                            break;
                        }
                    }
                }

                dgvDestinos.Columns[0].Visible = false;
            }

            catch (Exception ex)
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
                dgvDatos.Columns[0].Visible = true;
                Session["idRutaAsignacion"] = dgvDatos.Rows[a].Cells[0].Text;
                lblRuta.Text = dgvDatos.Rows[a].Cells[1].Text.Trim().ToUpper();
                dgvDatos.Columns[0].Visible = false;

                consultarRegistrosRutas(Convert.ToInt32(Session["idRutaAsignacion"].ToString()));
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
                llenarGridRutas();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            llenarGridRutas();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["idRutaAsignacion"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione la ruta para proceder a generar los destinos.', 'warning');", true);
                    return;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModalConfirmar').modal('show');</script>", false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnAceptarCerrar_Click(object sender, EventArgs e)
        {
            insertarRegistro();   
        }
    }
}