using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ENTIDADES;
using NEGOCIO;

namespace Solution_CTT
{
    public partial class frmDefinicionLocalidades : System.Web.UI.Page
    {
        ENTPueblos puebloE = new ENTPueblos();
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorPueblos puebloM = new manejadorPueblos();
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string[] sDatosMaximo = new string[5];
        string sAccion;
        string sAccionFiltro;

        int iComprobanteElectronico;

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
                limpiar();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMOBOX DE EMPRESAS
        private void llenarComboEmpresa()
        {
            try
            {
                sSql = "";
                sSql += "select idempresa, razonsocial" + Environment.NewLine;
                sSql += "from sis_empresa" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and idempresa = " + Convert.ToInt32(Application["idEmpresa"].ToString());

                comboE.ISSQL = sSql;
                cmbEmpresa.DataSource = comboM.listarCombo(comboE);
                cmbEmpresa.DataValueField = "IID";
                cmbEmpresa.DataTextField = "IDATO";
                cmbEmpresa.DataBind();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE LOCALIDADES
        private void llenarComboLocalidades()
        {
            try
            {
                sSql = "";
                sSql += "select correlativo, valor_texto" + Environment.NewLine;
                sSql += "from tp_codigos" + Environment.NewLine;
                sSql += "where tabla = 'SYS$00019'" + Environment.NewLine;
                sSql += "and estado ='A'";

                comboE.ISSQL = sSql;
                cmbLocalidadTerminal.DataSource = comboM.listarCombo(comboE);
                cmbLocalidadTerminal.DataValueField = "IID";
                cmbLocalidadTerminal.DataTextField = "IDATO";
                cmbLocalidadTerminal.DataBind();
                cmbLocalidadTerminal.Items.Insert(0, new ListItem("Seleccione Localidad", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE BODEGAS
        private void llenarComboBodegas()
        {
            try
            {
                sSql = "";
                sSql += "select id_bodega, descripcion" + Environment.NewLine;
                sSql += "from cv402_bodegas" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbBodegas.DataSource = comboM.listarCombo(comboE);
                cmbBodegas.DataValueField = "IID";
                cmbBodegas.DataTextField = "IDATO";
                cmbBodegas.DataBind();
                cmbBodegas.Items.Insert(0, new ListItem("Seleccione Bodega", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE LISTAS DE PRECIOS
        private void llenarComboListaPrecios()
        {
            try
            {
                sSql = "";
                sSql += "select id_lista_precio, descripcion" + Environment.NewLine;
                sSql += "from cv403_listas_precios" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbListasPrecios.DataSource = comboM.listarCombo(comboE);
                cmbListasPrecios.DataValueField = "IID";
                cmbListasPrecios.DataTextField = "IDATO";
                cmbListasPrecios.DataBind();
                cmbListasPrecios.Items.Insert(0, new ListItem("Seleccione Lista de Precio", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE SERVIDORES
        private void llenarComboServidores()
        {
            try
            {
                sSql = "";
                sSql += "select id_servidor, nombre_servidor" + Environment.NewLine;
                sSql += "from cv480_servidores" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbServidores.DataSource = comboM.listarCombo(comboE);
                cmbServidores.DataValueField = "IID";
                cmbServidores.DataTextField = "IDATO";
                cmbServidores.DataBind();
                cmbServidores.Items.Insert(0, new ListItem("Seleccione Servidor", "0"));
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
            //dgvDatos.Columns[2].Visible = ok;
            dgvDatos.Columns[3].Visible = ok;
            dgvDatos.Columns[4].Visible = ok;
            dgvDatos.Columns[5].Visible = ok;
            dgvDatos.Columns[6].Visible = ok;
            dgvDatos.Columns[7].Visible = ok;
            //dgvDatos.Columns[8].Visible = ok;
            dgvDatos.Columns[9].Visible = ok;
            dgvDatos.Columns[10].Visible = ok;
            dgvDatos.Columns[11].Visible = ok;
            dgvDatos.Columns[12].Visible = ok;
            dgvDatos.Columns[13].Visible = ok;
            dgvDatos.Columns[14].Visible = ok;
            dgvDatos.Columns[15].Visible = ok;
            dgvDatos.Columns[16].Visible = ok;
            dgvDatos.Columns[17].Visible = ok;
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_definicion_localidades" + Environment.NewLine;

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

        //FUNCION PARA INSERTAR EN LA BASE DE DATOSf
        private void insertarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sSql = "";
                sSql += "insert into tp_localidades (" + Environment.NewLine;
                sSql += "idempresa, cg_empresa, cg_localidad, id_responsable, id_bodega," + Environment.NewLine;
                sSql += "id_lista_defecto, id_servidor, establecimiento, punto_emision," + Environment.NewLine;
                sSql += "emite_comprobante_electronico, direccion, telefono1, telefono2," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idEmpresa"].ToString()) + ", " + Convert.ToInt32(Application["cgEmpresa"].ToString()) + ", ";
                sSql += cmbLocalidadTerminal.SelectedValue + ", " + Session["idPersonaResponsableDL"].ToString() + ", " + cmbBodegas.SelectedValue + ", ";
                sSql += cmbListasPrecios.SelectedValue + ", " + cmbServidores.SelectedValue + ", '" + txtEstablecimiento.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtPuntoEmision.Text.Trim() + "', " + iComprobanteElectronico + ", '" + txtDireccion.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'" + txtTelefono_1.Text.Trim() + "', '" + txtTelefono_2.Text.Trim() + "', 'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

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

                //INSTRUCCION SQL PARA ACTUALIZAR
                sSql = "";
                sSql += "update tp_localidades set" + Environment.NewLine;
                sSql += "id_responsable = " + Session["idPersonaResponsableDL"].ToString() + "," + Environment.NewLine;
                sSql += "id_bodega = " + cmbBodegas.SelectedValue + "," + Environment.NewLine;
                sSql += "id_lista_defecto = " + cmbListasPrecios.SelectedValue + "," + Environment.NewLine;
                sSql += "id_servidor = " + cmbServidores.SelectedValue + "," + Environment.NewLine;
                sSql += "establecimiento = '" + txtEstablecimiento.Text.Trim() + "'," + Environment.NewLine;
                sSql += "punto_emision = '" + txtPuntoEmision.Text.Trim() + "'," + Environment.NewLine;
                sSql += "emite_comprobante_electronico = " + iComprobanteElectronico + "," + Environment.NewLine;
                sSql += "direccion = '" + txtDireccion.Text.Trim() + "'," + Environment.NewLine;
                sSql += "telefono1 = '" + txtTelefono_1.Text.Trim() + "'," + Environment.NewLine;
                sSql += "telefono2 = '" + txtTelefono_2.Text.Trim() + "'" + Environment.NewLine;
                sSql += "where id_localidad = " + Session["idRegistroDF"].ToString();

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
                sSql += "update tp_localidades set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_localidad = " + Session["idRegistroDF"].ToString();

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

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            llenarComboEmpresa();
            llenarComboLocalidades();
            llenarComboBodegas();
            llenarComboListaPrecios();
            llenarComboServidores();

            txtEstablecimiento.Text = "";
            txtPuntoEmision.Text = "";
            txtIdentificacion.Text = "";
            txtNombreResponsable.Text = "";
            txtDireccion.Text = "";

            btnSave.Text = "Crear";

            chkEmiteElectronico.Checked = false;

            Session["idPersonaResponsableDL"] = null;
            Session["idRegistroDF"] = null;

            llenarGrid(0);
        }

        //FUNCION PARA LLENAR EL GRID DE PERSONAS RESPONSABLES
        private void llenarGridResposables(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select id_persona, identificacion, ltrim(isnull(nombres, '') + ' ' + apellidos) responsable, " + Environment.NewLine;
                sSql += "isnull(correo_electronico, '') correo_electronico" + Environment.NewLine;
                sSql += "from tp_personas" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and (nombres like '%" + txtFiltrarResponsables.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or apellidos like '%" + txtFiltrarResponsables.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or identificacion like '%" + txtFiltrarResponsables.Text.Trim() + "%')";
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

                dgvResponsables.Columns[0].Visible = true;

                dgvResponsables.DataSource = dtConsulta;
                dgvResponsables.DataBind();

                dgvResponsables.Columns[0].Visible = false;
            }

            catch (Exception ex)
            {
                ModalPopupExtender_Personas.Hide();
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
                columnasGrid(true);
                Session["idRegistroDF"] = dgvDatos.Rows[a].Cells[0].Text;

                if (sAccion == "Editar")
                {
                    cmbEmpresa.SelectedValue = dgvDatos.Rows[a].Cells[16].Text;
                    cmbLocalidadTerminal.SelectedValue = dgvDatos.Rows[a].Cells[17].Text;
                    txtEstablecimiento.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[4].Text);
                    txtPuntoEmision.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[5].Text);

                    if (Convert.ToInt32(dgvDatos.Rows[a].Cells[6].Text) == 1)
                    {
                        chkEmiteElectronico.Checked = true;
                    }

                    else
                    {
                        chkEmiteElectronico.Checked = false;
                    }

                    Session["idPersonaResponsableDL"] = dgvDatos.Rows[a].Cells[9].Text;
                    txtIdentificacion.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[10].Text);
                    txtNombreResponsable.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[3].Text);

                    cmbBodegas.SelectedValue = dgvDatos.Rows[a].Cells[13].Text;
                    cmbListasPrecios.Text = dgvDatos.Rows[a].Cells[14].Text;
                    cmbServidores.SelectedValue = dgvDatos.Rows[a].Cells[15].Text;

                    txtDireccion.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[7].Text);
                    txtTelefono_1.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[11].Text);
                    txtTelefono_2.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[12].Text);                    
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
            if (Convert.ToInt32(cmbLocalidadTerminal.SelectedValue) == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione la localidad.', 'info');", true);
                cmbLocalidadTerminal.Focus();
                return;
            }

            if (txtEstablecimiento.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el establecimiento.', 'info');", true);
                txtEstablecimiento.Focus();
                return;
            }

            if (txtPuntoEmision.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el punto de emisión.', 'info');", true);
                txtPuntoEmision.Focus();
                return;
            }

            if (Session["idPersonaResponsableDL"] == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione el responsable.', 'info');", true);
                txtEstablecimiento.Focus();
                return;
            }

            if (Convert.ToInt32(cmbBodegas.SelectedValue) == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione la bodega por defecto.', 'info');", true);
                cmbBodegas.Focus();
                return;
            }

            if (Convert.ToInt32(cmbListasPrecios.SelectedValue) == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione la lista de precio por defecto.', 'info');", true);
                cmbListasPrecios.Focus();
                return;
            }

            if (Convert.ToInt32(cmbServidores.SelectedValue) == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione el servidor por defecto.', 'info');", true);
                cmbServidores.Focus();
                return;
            }

            if (txtDireccion.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese la dirección de la localidad.', 'info');", true);
                txtDireccion.Focus();
                return;
            }

            if (txtTelefono_1.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese un número de teléfono para la localidad.', 'info');", true);
                txtPuntoEmision.Focus();
                return;
            }


            else
            {
                if (chkEmiteElectronico.Checked == true)
                {
                    iComprobanteElectronico = 1;
                }

                else
                {
                    iComprobanteElectronico = 0;
                }

                if (Session["idRegistroDF"] == null)
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

        protected void btnCerrarModalPersonas_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Personas.Hide();
        }

        protected void btnFiltarResponsable_Click(object sender, EventArgs e)
        {
            if (txtFiltrarResponsables.Text.Trim() == "")
            {
                llenarGridResposables(0);
            }

            else
            {
                llenarGridResposables(1);
            }
        }

        protected void lbtnSeleccion_Click(object sender, EventArgs e)
        {
            sAccionFiltro = "Seleccion";
        }

        protected void btnVerModal_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Personas.Show();
            llenarGridResposables(0);
        }

        protected void dgvResponsables_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvResponsables.SelectedIndex;
                dgvResponsables.Columns[0].Visible = true;

                if (sAccionFiltro == "Seleccion")
                {
                    Session["idPersonaResponsableDL"] = dgvResponsables.Rows[a].Cells[0].Text.Trim();
                    txtIdentificacion.Text = HttpUtility.HtmlDecode(dgvResponsables.Rows[a].Cells[1].Text.Trim());
                    txtNombreResponsable.Text = HttpUtility.HtmlDecode(dgvResponsables.Rows[a].Cells[2].Text.Trim());
                    txtFiltrarResponsables.Text = "";
                    ModalPopupExtender_Personas.Hide();
                }

                dgvResponsables.Columns[0].Visible = false;
            }

            catch (Exception ex)
            {
                ModalPopupExtender_Personas.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvResponsables_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvResponsables.PageIndex = e.NewPageIndex;

                if (txtFiltrarResponsables.Text.Trim() == "")
                {
                    llenarGridResposables(0);
                }

                else
                {
                    llenarGridResposables(1);
                }
            }

            catch (Exception ex)
            {
                ModalPopupExtender_Personas.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }
    }
}