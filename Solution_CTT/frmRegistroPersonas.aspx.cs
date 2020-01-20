using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NEGOCIO;
using System.Data;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmRegistroPersonas : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        Clases.ClaseValidarRUC ruc = new Clases.ClaseValidarRUC();
        Clases.ValidarCedula cedula = new Clases.ValidarCedula();

        string sSql;
        string sTabla;
        string sCampo;
        string sAccion;
        string sAccionFiltro;
        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;

        bool bRespuesta;

        int iCgTipoPersona;
        int iCgTipoIdentificacion;
        int iConsultarRegistro;
        int iIdPersona;
        int iLongitudCedula;

        long iMaximo;

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

            Session["modulo"] = "MÓDULO DE REGISTRO DE PERSONAS";

            if (!IsPostBack)
            {
                limpiar();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION COLUMNAS GRID
        private void columnasGrid(bool ok)
        {
            try
            {
                dgvDatos.Columns[0].Visible = ok;
                dgvDatos.Columns[1].Visible = ok;
                dgvDatos.Columns[2].Visible = ok;
                dgvDatos.Columns[5].Visible = ok;
                dgvDatos.Columns[6].Visible = ok;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid()
        {
            try
            {
                sSql = "";
                sSql += "select id_persona, cg_tipo_persona, cg_tipo_identificacion," + Environment.NewLine;
                sSql += "identificacion, ltrim(isnull(nombres, '') + ' ' + apellidos) persona," + Environment.NewLine;
                sSql += "apellidos, isnull(nombres, '') nombres" + Environment.NewLine;
                sSql += "from tp_personas" + Environment.NewLine;
                sSql += "where estado = 'A'";

                if (txtFiltrar.Text.Trim() != "")
                {
                    sSql += Environment.NewLine;
                    sSql += "and (apellidos like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or nombres like '%" + txtFiltrar.Text.Trim() + "%')";
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

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtIdentificacion.Text = "";
            txtApellidos.Text = "";
            txtNombres.Text = "";
            txtFiltrar.Text = "";
            Session["idRegistroPERSONAS"] = null;
            Session["id_ctt_chofer"] = null;
            chkPasaporte.Checked = false;
            MsjValidarCampos.Visible = false;
            txtIdentificacion.ReadOnly = false;
            pnlDatosRegistro.Visible = false;
            pnlDatosRegistro_2.Visible = false;
            btnSave.Text = "Crear";
            llenarGrid();
        }

        //INSERTAR REGISTRO
        private void insertarRegistro()
        {
            try
            {
                iConsultarRegistro = consultarRegistro();

                if (iConsultarRegistro > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Ya existe un registro con el codigo o usuario ingresado.', 'info');", true);
                    txtIdentificacion.Text = "";
                    txtIdentificacion.Focus();
                    return;
                }

                else if (iConsultarRegistro == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ocurrió un problema al consultar el código para el registro.', 'danger');", true);
                    txtIdentificacion.Text = "";
                    txtIdentificacion.Focus();
                    return;
                }

                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sSql = "";
                sSql += "Insert Into tp_personas (" + Environment.NewLine;
                sSql += "idempresa, Cg_Tipo_Persona, Cg_Tipo_Identificacion, Identificacion, Nombres, Apellidos," + Environment.NewLine;
                sSql += "Estado, fecha_ingreso, Usuario_Ingreso, terminal_ingreso," + Environment.NewLine;
                sSql += "numero_replica_trigger, numero_control_replica ) " + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Application["idEmpresa"].ToString() + ", " + iCgTipoPersona + ", " + iCgTipoIdentificacion + ", "; 
                sSql += "'" + txtIdentificacion.Text.Trim() + "', '" + txtNombres.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'" + txtApellidos.Text.Trim().ToUpper() + "', 'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 0, 0)";

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

                sSql = "";
                sSql += "update tp_personas set" + Environment.NewLine;
                sSql += "apellidos = '" + txtApellidos.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "nombres = '" + txtNombres.Text.Trim().ToUpper() + "'" + Environment.NewLine;
                sSql += "where id_persona = " + Convert.ToInt32(Session["idRegistroPERSONAS"].ToString());

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
                sSql += "update tp_personas set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_persona = " + Convert.ToInt32(Session["idRegistroPERSONAS"].ToString());

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

        //FUNCION PARA CONSULTAR EL REGISTRO
        private int consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from tp_personas" + Environment.NewLine;
                sSql += "where identificacion = '" + txtIdentificacion.Text.Trim() + "'";

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

        //VALIDAR IDENTIFICACION
        private void validarIdentificacion()
        {
            try
            {
                if (chkPasaporte.Checked == true)
                {
                    iCgTipoPersona = 2447;
                    iCgTipoIdentificacion = 180;
                    insertarRegistro();
                    return;
                }

                iLongitudCedula = txtIdentificacion.Text.Trim().Length;

                if ((iLongitudCedula == 12) || (iLongitudCedula == 11))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                    txtIdentificacion.Text = "";
                    return;
                }

                if (iLongitudCedula > 13)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                    txtIdentificacion.Text = "";
                    return;
                }

                if (iLongitudCedula < 10)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                    txtIdentificacion.Text = "";
                    return;
                }

                if (iLongitudCedula == 10)
                {
                    if (cedula.validarCedulaConsulta(txtIdentificacion.Text.Trim()) == "NO")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                        txtIdentificacion.Text = "";
                        return;
                    }

                    else
                    {
                        iCgTipoPersona = 2447;
                        iCgTipoIdentificacion = 178;
                        insertarRegistro();
                    }
                }

                else if (iLongitudCedula == 13)
                {
                    int iTercerDigito = Convert.ToInt32(txtIdentificacion.Text.Trim().Substring(0, 2));

                    if (iTercerDigito == 6)
                    {
                        if (ruc.validarRucPublico(txtIdentificacion.Text.Trim()) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                            txtIdentificacion.Text = "";
                            return;
                        }

                        else
                        {
                            iCgTipoPersona = 2448;
                            iCgTipoIdentificacion = 179;
                            insertarRegistro();
                        }
                    }

                    else if (iTercerDigito == 9)
                    {
                        if (ruc.validarRucPrivado(txtIdentificacion.Text.Trim()) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                            txtIdentificacion.Text = "";
                            return;
                        }

                        else
                        {
                            iCgTipoPersona = 2448;
                            iCgTipoIdentificacion = 179;
                            insertarRegistro();
                        }
                    }

                    else if ((iTercerDigito >= 0) && (iTercerDigito < 6))
                    {
                        if (ruc.validarRucNatural(txtIdentificacion.Text.Trim()) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                            txtIdentificacion.Text = "";
                            return;
                        }

                        else
                        {
                            iCgTipoPersona = 2448;
                            iCgTipoIdentificacion = 179;
                            insertarRegistro();
                        }
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                        txtIdentificacion.Text = "";
                        return;
                    }
                }

                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return;
            }
        }

        //VALIDAR IDENTIFICACION 2
        private void validarConsulta()
        {
            try
            {
                if (chkPasaporte.Checked == true)
                {
                    consultarRegistroCedula();
                    return;
                }

                iLongitudCedula = txtIdentificacion.Text.Trim().Length;

                if ((iLongitudCedula == 12) || (iLongitudCedula == 11))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                    txtIdentificacion.Text = "";
                    return;
                }

                if (iLongitudCedula > 13)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                    txtIdentificacion.Text = "";
                    return;
                }

                if (iLongitudCedula < 10)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                    txtIdentificacion.Text = "";
                    return;
                }

                if (iLongitudCedula == 10)
                {
                    if (cedula.validarCedulaConsulta(txtIdentificacion.Text.Trim()) == "NO")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                        txtIdentificacion.Text = "";
                        return;
                    }

                    else
                    {
                        consultarRegistroCedula();
                    }
                }

                else if (iLongitudCedula == 13)
                {
                    int iTercerDigito = Convert.ToInt32(txtIdentificacion.Text.Trim().Substring(0, 2));

                    if (iTercerDigito == 6)
                    {
                        if (ruc.validarRucPublico(txtIdentificacion.Text.Trim()) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                            txtIdentificacion.Text = "";
                            return;
                        }

                        else
                        {
                            consultarRegistroCedula();
                        }
                    }

                    else if (iTercerDigito == 9)
                    {
                        if (ruc.validarRucPrivado(txtIdentificacion.Text.Trim()) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                            txtIdentificacion.Text = "";
                            return;
                        }

                        else
                        {
                            consultarRegistroCedula();
                        }
                    }

                    else if ((iTercerDigito >= 0) && (iTercerDigito < 6))
                    {
                        if (ruc.validarRucNatural(txtIdentificacion.Text.Trim()) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                            txtIdentificacion.Text = "";
                            return;
                        }

                        else
                        {
                            consultarRegistroCedula();
                        }
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                        txtIdentificacion.Text = "";
                        return;
                    }
                }

                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return;
            }
        }

        //FUNCION SELECT    
        private void consultarRegistroCedula()
        {
            try
            {
                sSql = "";
                sSql += "select cg_tipo_identificacion, id_persona, apellidos, isnull(nombres, '') nombres" + Environment.NewLine;
                sSql += "from tp_personas" + Environment.NewLine;
                sSql += "where identificacion = '" + txtIdentificacion.Text.Trim() + "'" + Environment.NewLine;
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

                //txtIdentificacion.Text = "";
                txtApellidos.Text = "";
                txtNombres.Text = "";
                pnlDatosRegistro.Visible = true;
                pnlDatosRegistro_2.Visible = true;
                Session["idRegistroPERSONAS"] = null;

                if (dtConsulta.Rows.Count == 0)
                {
                    txtApellidos.Focus();
                    btnSave.Text = "Crear";
                }

                else
                {
                    btnSave.Text = "Editar";
                    Session["idRegistroPERSONAS"] = dtConsulta.Rows[0]["id_persona"].ToString().Trim();
                    txtIdentificacion.ReadOnly = true;
                    //txtIdentificacion.Text = dtConsulta.Rows[0]["identificacion"].ToString().Trim();
                    txtApellidos.Text = dtConsulta.Rows[0]["apellidos"].ToString().Trim();
                    txtNombres.Text = dtConsulta.Rows[0]["nombres"].ToString().Trim();

                    if (Convert.ToInt32(dtConsulta.Rows[0]["cg_tipo_identificacion"].ToString()) == 180)
                    {
                        chkPasaporte.Checked = true;
                    }

                    else
                    {
                        chkPasaporte.Checked = false;
                    }

                    txtApellidos.Focus();
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return;
            }
        }

        #endregion

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            llenarGrid();
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
                sAccion = "D";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtIdentificacion.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtIdentificacion.Focus();
            }

            else
            {
                if (Session["idRegistroPERSONAS"] == null)
                {
                    //ENVIO A FUNCION DE INSERCION
                    //insertarRegistro();
                    validarIdentificacion();
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

        //PARA EL GRID DE INFORMACION PRINCIPAL
        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);
                Session["idRegistroPERSONAS"] = dgvDatos.Rows[a].Cells[0].Text.Trim();

                if (sAccion == "Editar")
                {
                    txtIdentificacion.ReadOnly = true;
                    txtIdentificacion.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[3].Text.Trim());
                    txtApellidos.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[5].Text.Trim());
                    txtNombres.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[6].Text.Trim());
                }

                columnasGrid(false);
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
                    llenarGrid();
                }

                else
                {
                    llenarGrid();
                }
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

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            if (txtIdentificacion.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese un número de identificación.', 'info');", true);
                return;
            }

            validarConsulta();
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