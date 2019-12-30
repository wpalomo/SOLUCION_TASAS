using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ENTIDADES;
using NEGOCIO;
using System.Data;

namespace Solution_CTT
{
    public partial class frmViajesMasivos : System.Web.UI.Page
    {
        ENTComboDatos comboE = new ENTComboDatos();
        ENTHorarioMasivo horarioMasivoE = new ENTHorarioMasivo();

        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorConexion conexionM = new manejadorConexion();
        manejadorHorarioMasivo horarioMasivoM = new manejadorHorarioMasivo();

        string sSql;
        string sFecha;
        string []sDatosMaximo = new string[5];

        DataTable dtConsulta;
        DataTable dtGrid;

        bool bRespuesta;

        int iCodigo;
        int iNumeroViaje;
        int iIdItinerario;
        int iIditinerarioConsulta;
        int iCuentaFrecuenciasCreadas;
        int iBanderaCobraAdministracion;

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

            Session["modulo"] = "MÓDULO DE VIAJES MASIVOS";

            if (!IsPostBack)
            {
                limpiar();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LIMPIAR EL FORMULARIO
        private void limpiar()
        {
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Session["fecha"] = DateTime.Now.ToString("dd/MM/yyyy");
            controlGridViajes();            
        }

        //FUNCION PARA CONTROLAR LA CREACION DEL GRIDVIEW
        private void controlGridViajes()
        {
            try
            {
                int iIdTipoServicio = extraerIdTipoViaje();

                if (iIdTipoServicio == -1)
                {
                    return;
                }

                if (iIdTipoServicio == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No se encuentran configurados los tipos de viajes.', 'info');", true);
                    return;
                }

                if (consultarFrecuenciasCreadas(Session["fecha"].ToString(), iIdTipoServicio) == false)
                {
                    return;
                }

                sSql = "";
                sSql += "select 0 numero, * from ctt_vw_horarios_masivos_2" + Environment.NewLine;
                sSql += "where id_ctt_pueblo_origen = " + Convert.ToInt32(Session["id_pueblo"].ToString()) + Environment.NewLine;
                sSql += "and id_ctt_tipo_servicio = " + iIdTipoServicio + Environment.NewLine;
                sSql += "order by hora_salida";

                dtGrid = new DataTable();
                dtGrid.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtGrid);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                if (iCuentaFrecuenciasCreadas > 0)
                {
                    btnGenerar.Visible = true;

                    //PROCESO PARA ELIMINAR LOS REGISTROS DE DATATABLE ANTES DE COLOCARLO EN EL GRIDVIEW
                    //RECORRIDO DE DATATABLE CON LOS OBJETOS DEL GRIDVIEW HACIA ATRAS
                    for (int i = dtGrid.Rows.Count - 1; i >= 0; i--)
                    {
                        iIdItinerario = Convert.ToInt32(dtGrid.Rows[i]["id_ctt_itinerario"].ToString());

                        //RECORRER EL DATATABLE CON LOS REGISTROS YA CREADOS EN FRECUENCIA
                        for (int j = 0; j < dtConsulta.Rows.Count; j++)
                        {
                            iIditinerarioConsulta = Convert.ToInt32(dtConsulta.Rows[j]["id_ctt_itinerario"].ToString());

                            //COMPARACION DE ID
                            // SI ES VERDADERO ELIMINA LA FILA Y SALE DEL CICLO
                            if (iIditinerarioConsulta == iIdItinerario)
                            {
                                dtGrid.Rows.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }

                else
                {
                    btnGenerar.Visible = false;
                }

                dgvDatos.Columns[1].Visible = true;
                dgvDatos.Columns[3].Visible = true;
                dgvDatos.DataSource = dtGrid;
                dgvDatos.DataBind();
                dgvDatos.Columns[1].Visible = false;
                dgvDatos.Columns[3].Visible = false;

                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    dgvDatos.Rows[i].Cells[0].Text = (i + 1).ToString();
                } 
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA EXTRAER EL ID DE TIPO SERVICIO
        private int extraerIdTipoViaje()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tipo_servicio" + Environment.NewLine;
                sSql += "from ctt_tipo_servicio" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and normal = 1";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return -1;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    return 0;
                }

                return Convert.ToInt32(dtConsulta.Rows[0]["id_ctt_tipo_servicio"].ToString());
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return -1;
            }
        }

        //FUNCION PARA VALIDAR SI LOS HORARIOS YA FUERON CREADOS
        private bool consultarFrecuenciasCreadas(string sFecha_P, int iIdTipoServicio_P)
        {
            try
            {
                sSql = "";
                sSql += "select P.id_ctt_itinerario" + Environment.NewLine;
                sSql += "from ctt_itinerario I INNER JOIN" + Environment.NewLine;
                sSql += "ctt_programacion P ON I.id_ctt_itinerario = P.id_ctt_itinerario" + Environment.NewLine;
                sSql += "and I.estado = 'A'" + Environment.NewLine;
                sSql += "and P.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_ruta R ON R.id_ctt_ruta = I.id_ctt_ruta" + Environment.NewLine;
                sSql += "and R.estado = 'A'" + Environment.NewLine;
                sSql += "where P.fecha_viaje = '" + Convert.ToDateTime(sFecha_P).ToString("yyyy/MM/dd") + "'" + Environment.NewLine;
                sSql += "and P.id_ctt_tipo_servicio = " + iIdTipoServicio_P + Environment.NewLine;
                sSql += "and R.id_ctt_pueblo_origen = " + Convert.ToInt32(Session["id_pueblo"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    iCuentaFrecuenciasCreadas = dtConsulta.Rows.Count;
                    return true;
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE ANDENES
        private void llenarComboAnden(DropDownList cmbListarAndenes)
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_anden, descripcion" + Environment.NewLine;
                sSql += "from ctt_anden" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(Session["id_pueblo"].ToString()) + Environment.NewLine;
                sSql += "and anden_principal = 1" + Environment.NewLine;
                sSql += "and estado = 'A'";

                comboE.ISSQL = sSql;
                cmbListarAndenes.DataSource = comboM.listarCombo(comboE);
                cmbListarAndenes.DataValueField = "IID";
                cmbListarAndenes.DataTextField = "IDATO";
                cmbListarAndenes.DataBind();
                //cmbListarAndenes.Items.Insert(0, new ListItem("Seleccione..!!", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE CHOFERES
        private void llenarComboChoferes(DropDownList cmbChofer)
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_chofer, descripcion" + Environment.NewLine;
                sSql += "from ctt_chofer" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbChofer.DataSource = comboM.listarCombo(comboE);
                cmbChofer.DataValueField = "IID";
                cmbChofer.DataTextField = "IDATO";
                cmbChofer.DataBind();
                cmbChofer.Items.Insert(0, new ListItem("Seleccione..!!", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE ASISTENTES
        private void llenarComboAsistentes(DropDownList cmbAsistentes)
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_asistente, descripcion" + Environment.NewLine;
                sSql += "from ctt_asistente" + Environment.NewLine;                
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and seleccion_default = 1";

                comboE.ISSQL = sSql;
                cmbAsistentes.DataSource = comboM.listarCombo(comboE);
                cmbAsistentes.DataValueField = "IID";
                cmbAsistentes.DataTextField = "IDATO";
                cmbAsistentes.DataBind();
                //cmbAsistentes.Items.Insert(0, new ListItem("Seleccione..!!", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE VEHICULOS
        private void llenarComboVehiculos(DropDownList cmbVehiculo)
        {
            try
            {
                sSql = "";
                sSql += "select V.id_ctt_vehiculo," + Environment.NewLine;
                sSql += "D.descripcion + '-' + V.placa vehiculo" + Environment.NewLine;
                sSql += "from ctt_disco D, ctt_vehiculo V" + Environment.NewLine;
                sSql += "where V.id_ctt_disco = D.id_ctt_disco" + Environment.NewLine;
                sSql += "and V.estado = 'A'" + Environment.NewLine;
                sSql += "and D.estado = 'A'" + Environment.NewLine;
                sSql += "order by D.descripcion";

                comboE.ISSQL = sSql;
                cmbVehiculo.DataSource = comboM.listarCombo(comboE);
                cmbVehiculo.DataValueField = "IID";
                cmbVehiculo.DataTextField = "IDATO";
                cmbVehiculo.DataBind();
                cmbVehiculo.Items.Insert(0, new ListItem("Seleccione..!!", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }
        
        //FUNCION PARA CARGAR NÚMERO DE VIAJE
        private bool numeroViaje()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(max(numero_viaje), 0) numero_viaje" + Environment.NewLine;
                //sSql += "isnull(max(codigo), 0) maximo_codigo" + Environment.NewLine;
                sSql += "from ctt_programacion" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    return false;
                }

                iNumeroViaje = Convert.ToInt32(dtConsulta.Rows[0]["numero_viaje"].ToString());

                sSql = "";
                sSql += "select top 1 isnull(codigo, 0) codigo" + Environment.NewLine;
                sSql += "from ctt_programacion" + Environment.NewLine;
                sSql += "order by id_ctt_programacion desc";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    return false;
                }

                iCodigo = Convert.ToInt32(dtConsulta.Rows[0]["codigo"].ToString());

                return true;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private void insertarRegistro()
        {
            try
            {
                if (numeroViaje() == false)
                {
                    //MENSAJE DE ERROR
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar recuperar el número de viaje para continuar el secuencial.', 'danger');", true);
                    return;
                }

                if (conexionM.iniciarTransaccion() == false)
                {
                    //MENSAJE DE ERROR
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar iniciar la transacción.', 'danger');", true);
                    return;
                }

                sFecha = Convert.ToDateTime(Session["fecha"].ToString()).ToString("yyyy/MM/dd");
                dgvDatos.Columns[1].Visible = true;
                dgvDatos.Columns[3].Visible = true;

                foreach(GridViewRow row in dgvDatos.Rows)
                {
                    DropDownList cmbListarChoferes = row.FindControl("cmbListarChoferes") as DropDownList;
                    DropDownList cmbListarAsistentes = row.FindControl("cmbListarAsistentes") as DropDownList;
                    DropDownList cmbListarVehiculo = row.FindControl("cmbListarVehiculo") as DropDownList;
                    DropDownList cmbListarAndenes = row.FindControl("cmbListarAndenes") as DropDownList;

                    if (Convert.ToInt32(cmbListarVehiculo.SelectedValue) == 0)
                    {
                        goto continuar;
                    }

                    else if (Convert.ToInt32(cmbListarChoferes.SelectedValue) == 0)
                    {
                        goto continuar;
                    }

                    if (Convert.ToInt32(Session["ejecuta_cobro_administrativo"].ToString()) == 1)
                    {
                        iBanderaCobraAdministracion = 1;
                    }

                    else
                    {
                        iBanderaCobraAdministracion = 0;
                    }

                    sSql = "";
                    sSql += "insert into ctt_programacion (" + Environment.NewLine;
                    sSql += "id_ctt_chofer, id_ctt_asistente, id_ctt_vehiculo, id_ctt_anden, id_ctt_tipo_servicio," + Environment.NewLine;
                    sSql += "id_ctt_itinerario, codigo, numero_viaje, fecha_viaje, estado_salida, asientos_ocupados," + Environment.NewLine;
                    sSql += "cobrar_administracion, id_ctt_pueblo_origen, estado_envio_encomienda, visualizar," + Environment.NewLine;
                    sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += Convert.ToInt32(cmbListarChoferes.SelectedValue) + ", " + Convert.ToInt32(cmbListarAsistentes.SelectedValue) + ", ";
                    sSql += Convert.ToInt32(cmbListarVehiculo.SelectedValue) + ", " + Convert.ToInt32(cmbListarAndenes.SelectedValue) + ", ";
                    sSql += Convert.ToInt32(row.Cells[3].Text) + ", " + Convert.ToInt32(row.Cells[1].Text) + "," + Environment.NewLine;
                    sSql += "'" + iCodigo.ToString() + "', " + iNumeroViaje + ", '" + sFecha + "'," + Environment.NewLine;
                    sSql += "'Abierta', 0, " + iBanderaCobraAdministracion + ", " + Session["id_pueblo"].ToString() + ", 'Abierta'," + Environment.NewLine;
                    sSql += "1, 'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                    sSql += "'" + sDatosMaximo[1] + "')";

                    //EJECUCIÓN DE INSTRUCCION SQL
                    if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                    {
                        //MENSAJE DE ERROR
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        goto reversa;
                    }

                    iCodigo++;
                    iNumeroViaje++;

                    continuar:{}
                }

                dgvDatos.Columns[1].Visible = false;
                dgvDatos.Columns[3].Visible = false;

                //FUNALIZA LAS INSTRUCCIONES SQL
                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registros insertados éxitosamente.', 'success');", true);
                limpiar();
                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); return; }
        }

        #endregion

        protected void dgvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList cmbListarChoferes = (e.Row.FindControl("cmbListarChoferes") as DropDownList);
                    llenarComboChoferes(cmbListarChoferes);

                    DropDownList cmbListarAsistentes = (e.Row.FindControl("cmbListarAsistentes") as DropDownList);
                    llenarComboAsistentes(cmbListarAsistentes);

                    DropDownList cmbListarVehiculo = (e.Row.FindControl("cmbListarVehiculo") as DropDownList);
                    llenarComboVehiculos(cmbListarVehiculo);

                    DropDownList cmbListarAndenes = (e.Row.FindControl("cmbListarAndenes") as DropDownList);
                    llenarComboAnden(cmbListarAndenes);
                }
            }

            catch(Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#QuestionModalConfirmar').modal('show');</script>", false);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmPrincipal.aspx");
        }
        
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            btnGenerar.Visible = true;

            if (txtFecha.Text.Trim() == "")
            {
                limpiar();
            }

            else if (Convert.ToDateTime(txtFecha.Text.Trim()) < DateTime.Now)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Aviso.!', 'No puede crear frecuencias de fechas pasadas.', 'warning');", true);
                limpiar();
            }

            else
            {
                Session["fecha"] = txtFecha.Text.Trim();
                controlGridViajes();
            }
        }

        protected void btnAceptarCrear_Click(object sender, EventArgs e)
        {
            insertarRegistro();
        }
    }
}