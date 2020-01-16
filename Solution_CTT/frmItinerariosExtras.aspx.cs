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
    public partial class frmItinerariosExtras : System.Web.UI.Page
    {
        ENTItinierario itinerarioE = new ENTItinierario();
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorItinerario itinerarioM = new manejadorItinerario();
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

            Session["modulo"] = "MÓDULO DE ITINERARIOS EXTRAS";

            if (!IsPostBack)
            {
                cmbLocalidad.SelectedIndexChanged -= new EventHandler(cmbLocalidad_SelectedIndexChanged);
                llenarComboLocalidad();
                cmbLocalidad.SelectedIndexChanged += new EventHandler(cmbLocalidad_SelectedIndexChanged);
                llenarComboRutas();
                llenarComboHorarios();
                llenarComboTipoViajes();
                consultarCodigoMaximo();
                llenarGrid(0);

            }
        }

        #region FUNCION DEL USUARIO

        //FUNCION PARA LAS COLUMNAS
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[4].ItemStyle.Width = 100;
            dgvDatos.Columns[5].ItemStyle.Width = 200;
            dgvDatos.Columns[6].ItemStyle.Width = 150;
            dgvDatos.Columns[7].ItemStyle.Width = 100;
            dgvDatos.Columns[8].ItemStyle.Width = 100;
            dgvDatos.Columns[9].ItemStyle.Width = 100;

            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[2].Visible = ok;
            dgvDatos.Columns[3].Visible = ok;
        }

        //FUNCION PARA LLENAR EL COMOBOX DE LOCALIDADES
        private void llenarComboLocalidad()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_pueblo, descripcion" + Environment.NewLine;
                sSql += "from ctt_pueblos" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and terminal = 1";

                comboE.ISSQL = sSql;
                cmbLocalidad.DataSource = comboM.listarCombo(comboE);
                cmbLocalidad.DataValueField = "IID";
                cmbLocalidad.DataTextField = "IDATO";
                cmbLocalidad.DataBind();
                cmbLocalidad.SelectedValue = Session["id_pueblo"].ToString();

                //llenarComboRutas();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE RUTAS
        private void llenarComboRutas()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_ruta, descripcion" + Environment.NewLine;
                sSql += "from ctt_ruta" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and id_ctt_pueblo_origen = " + Convert.ToInt32(cmbLocalidad.SelectedValue);

                comboE.ISSQL = sSql;
                cmbRutas.DataSource = comboM.listarCombo(comboE);
                cmbRutas.DataValueField = "IID";
                cmbRutas.DataTextField = "IDATO";
                cmbRutas.DataBind();
                cmbRutas.Items.Insert(0, new ListItem("Seleccione Ruta", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE HORARIOS
        private void llenarComboHorarios()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_horario, 'HORA: ' + convert(char(8), hora_salida, 108) hora_salida" + Environment.NewLine;
                sSql += "from ctt_horarios" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "order by hora_salida";

                comboE.ISSQL = sSql;
                cmbHoraSalida.DataSource = comboM.listarCombo(comboE);
                cmbHoraSalida.DataValueField = "IID";
                cmbHoraSalida.DataTextField = "IDATO";
                cmbHoraSalida.DataBind();
                cmbHoraSalida.Items.Insert(0, new ListItem("Seleccione Horario", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE TIPO DE VIAJES
        private void llenarComboTipoViajes()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tipo_servicio, descripcion" + Environment.NewLine;
                sSql += "from ctt_tipo_servicio" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and extra = 1";

                comboE.ISSQL = sSql;
                cmbTipoViaje.DataSource = comboM.listarCombo(comboE);
                cmbTipoViaje.DataValueField = "IID";
                cmbTipoViaje.DataTextField = "IDATO";
                cmbTipoViaje.DataBind();
                //cmbTipoViaje.Items.Insert(0, new ListItem("Seleccione Tipo de Viaje", "0"));
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
                sSql += "select I.id_ctt_itinerario, I.id_ctt_ruta, I.id_ctt_horario, " + Environment.NewLine;
                sSql += "I.id_ctt_tipo_servicio, I.codigo, R.descripcion, P.descripcion destino," + Environment.NewLine;
                sSql += "H.hora_salida, S.descripcion tipo_viaje," + Environment.NewLine;
                sSql += "case I.estado when 'A' then 'ACTIVO' else 'INACTIVO' end estado" + Environment.NewLine;
                sSql += "from ctt_ruta R INNER JOIN" + Environment.NewLine;
                sSql += "ctt_itinerario I ON I.id_ctt_ruta = R.id_ctt_ruta" + Environment.NewLine;
                sSql += "and R.estado = 'A'" + Environment.NewLine;
                sSql += "and I.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_horarios H ON H.id_ctt_horario = I.id_ctt_horario" + Environment.NewLine;
                sSql += "and H.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_tipo_servicio S ON S.id_ctt_tipo_servicio = I.id_ctt_tipo_servicio" + Environment.NewLine;
                sSql += "and S.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_pueblos P ON P.id_ctt_pueblo = R.id_ctt_pueblo_destino" + Environment.NewLine;
                sSql += "where R.id_ctt_pueblo_origen = " + Convert.ToInt32(cmbLocalidad.SelectedValue) + Environment.NewLine;
                sSql += "and S.extra = 1" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and I.codigo = '" + txtCodigo.Text.Trim().ToUpper() + "'" + Environment.NewLine;
                }

                sSql += "order by I.codigo";

                columnasGrid(true);
                itinerarioE.ISQL = sSql;
                dgvDatos.DataSource = itinerarioM.listarItinerario(itinerarioE);
                dgvDatos.DataBind();
                columnasGrid(false);

            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA OBTENER EL CODIGO SUPERIOR
        private void consultarCodigoMaximo()
        {
            try
            {
                sSql = "";
                sSql += "select top 1 isnull(codigo, '0') codigo" + Environment.NewLine;
                sSql += "from ctt_itinerario" + Environment.NewLine;
                sSql += "order by id_ctt_itinerario desc";

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

        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private void insertarRegistro()
        {
            try
            {
                if (consultarRegistro() > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'Ya existe un registro con el codigo ingresado.', 'warning');", true);
                    return;
                }

                else if (consultarRegistro() == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ocurrió un problema al consultar el código para el registro.', 'danger');", true);
                    return;
                }

                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sSql = "";
                sSql += "insert into ctt_itinerario (" + Environment.NewLine;
                sSql += "id_ctt_ruta, id_ctt_horario, id_ctt_tipo_servicio, codigo," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(cmbRutas.SelectedValue) + ", " + Convert.ToInt32(cmbHoraSalida.SelectedValue) + ", ";
                sSql += Convert.ToInt32(cmbTipoViaje.SelectedValue) + ", '" + txtCodigo.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

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
                sSql += "update ctt_itinerario set" + Environment.NewLine;
                sSql += "id_ctt_ruta = " + Convert.ToInt32(cmbRutas.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_ctt_horario = " + Convert.ToInt32(cmbHoraSalida.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_ctt_tipo_servicio = " + Convert.ToInt32(cmbTipoViaje.SelectedValue) + Environment.NewLine;
                sSql += "where id_ctt_itinerario = " + Convert.ToInt32(Session["idRegistroIEXTRA"]) + Environment.NewLine;
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
                    goto fin;
                }

                sSql = "";
                sSql += "update ctt_itinerario set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_ctt_itinerario = " + Convert.ToInt32(Session["idRegistroIEXTRA"]);

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
                sSql += "from ctt_itinerario" + Environment.NewLine;
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
            cmbRutas.SelectedIndex = 0;
            cmbHoraSalida.SelectedIndex = 0;
            cmbTipoViaje.SelectedIndex = 0;
            Session["idRegistroIEXTRA"] = null;
            btnGuardar.Text = "Crear";
            MsjValidarCampos.Visible = false;
            llenarGrid(0);
        }

        #endregion

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                sFecha = DateTime.Now.ToString("yyyy/MM/dd");

                if (txtCodigo.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese un código.', 'warning');", true);
                    txtCodigo.Focus();
                }

                else if (Convert.ToInt32(cmbRutas.SelectedValue) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione la ruta.', 'warning');", true);
                    cmbRutas.Focus();
                }

                else if (Convert.ToInt32(cmbHoraSalida.SelectedValue) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione el horario de salida.', 'warning');", true);
                    cmbHoraSalida.Focus();
                }

                else if (Convert.ToInt32(cmbTipoViaje.SelectedValue) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione el tipo de viaje.', 'warning');", true);
                    cmbTipoViaje.Focus();
                }

                else
                {
                    if (Session["idRegistroIEXTRA"] == null)
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

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {

        }

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            sAccion = "Editar";
            btnGuardar.Text = "Editar";
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

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            eliminarRegistro();
        }

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);

                Session["idRegistroIEXTRA"] = dgvDatos.Rows[a].Cells[0].Text.Trim();

                if (sAccion == "Editar")
                {
                    cmbRutas.SelectedValue = dgvDatos.Rows[a].Cells[1].Text;
                    cmbHoraSalida.SelectedValue = dgvDatos.Rows[a].Cells[2].Text;
                    cmbTipoViaje.SelectedValue = dgvDatos.Rows[a].Cells[3].Text;
                    txtCodigo.Text = dgvDatos.Rows[a].Cells[4].Text;
                }

                columnasGrid(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

        fin: { }
        }

        protected void cmbLocalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarComboRutas();
            llenarGrid(0);
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