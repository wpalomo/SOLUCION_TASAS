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
        int iIdHorarioGrid;
        int iIdHorarioConsulta;
        int iCuentaFrecuenciasCreadas;

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
            btnGenerar.Visible = true;
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Session["fecha"] = DateTime.Now.ToString("dd/MM/yyyy");
            consultarFrecuenciasCreadas(Session["fecha"].ToString());
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid(string sFecha_P)
        {
            try
            {
                sSql = "";
                sSql += "select 0 numero, id_ctt_itinerario, hora_salida, id_ctt_tipo_servicio" + Environment.NewLine;
                sSql += "from ctt_vw_horarios_masivos" + Environment.NewLine;
                sSql += "where habilitado = 1" + Environment.NewLine;
                sSql += "and id_ctt_pueblo = " + Convert.ToInt32(Session["id_pueblo"].ToString()) + Environment.NewLine;
                sSql += "order by hora_salida";

                dtGrid = new DataTable();
                dtGrid.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtGrid);

                if (bRespuesta == true)
                {
                    if (iCuentaFrecuenciasCreadas > 0)
                    {
                        //PROCESO PARA ELIMINAR LOS REGISTROS DE DATATABLE ANTES DE COLOCARLO EN EL GRIDVIEW
                        //RECORRIDO DE DATATABLE CON LOS OBJETOS DEL GRIDVIEW HACIA ATRAS
                        for (int i = dtGrid.Rows.Count - 1; i >= 0; i--)
                        {
                            iIdHorarioGrid = Convert.ToInt32(dtGrid.Rows[i][1].ToString());

                            //RECORRER EL DATATABLE CON LOS REGISTROS YA CREADOS EN FRECUENCIA
                            for (int j = 0; j < dtConsulta.Rows.Count; j++)
                            {
                                iIdHorarioConsulta = Convert.ToInt32(dtConsulta.Rows[j][0].ToString());

                                //COMPARACION DE ID
                                // SI ES VERDADERO ELIMINA LA FILA Y SALE DEL CICLO
                                if (iIdHorarioConsulta == iIdHorarioGrid)
                                {
                                    dtGrid.Rows.RemoveAt(i);
                                    break;
                                }
                            }
                        }
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

        //FUNCION PARA VALIDAR SI LOS HORARIOS YA FUERON CREADOS
        private void consultarFrecuenciasCreadas(string sFecha_P)
        {
            try
            {
                //sSql = "";
                //sSql += "select id_ctt_horario" + Environment.NewLine;
                //sSql += "from ctt_programacion" + Environment.NewLine;
                //sSql += "where fecha_viaje = '" + Convert.ToDateTime(sFecha_P).ToString("yyyy/MM/dd") + "'" + Environment.NewLine;
                //sSql += "and id_ctt_tipo_servicio = 1" + Environment.NewLine;
                //sSql += "and estado = 'A'";

                sSql = "";
                sSql += "select I.id_ctt_horario" + Environment.NewLine;
                sSql += "from ctt_itinerario I INNER JOIN" + Environment.NewLine;
                sSql += "ctt_programacion P ON I.id_ctt_itinerario = P.id_ctt_itinerario" + Environment.NewLine;
                sSql += "and I.estado = 'A'" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "where fecha_viaje = '" + Convert.ToDateTime(sFecha_P).ToString("yyyy/MM/dd") + "'" + Environment.NewLine;
                sSql += "and P.id_ctt_tipo_servicio = 1";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    iCuentaFrecuenciasCreadas = dtConsulta.Rows.Count;
                    llenarGrid(Session["fecha"].ToString());
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

            catch(Exception ex)
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
                sSql += "order by D.id_ctt_disco";

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
                sSql += "select isnull(max(numero_viaje), 0) maximo_viaje," + Environment.NewLine;
                sSql += "isnull(max(codigo), 0) maximo_codigo" + Environment.NewLine;
                sSql += "from ctt_programacion" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    iNumeroViaje = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                    iCodigo = Convert.ToInt32(dtConsulta.Rows[0][1].ToString());
                    return true;
                }

                else
                {
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

        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private void insertarRegistro()
        {
            try
            {
                if (numeroViaje() == false)
                {
                    //MENSAJE DE ERROR
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar recuperar el número de viaje para continuar el secuencial.', 'danger');", true);
                    goto fin;
                }

                if (conexionM.iniciarTransaccion() == false)
                {
                    //MENSAJE DE ERROR
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar iniciar la transacción.', 'danger');", true);
                    goto fin;
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

                    sSql = "";
                    sSql += "insert into ctt_programacion (" + Environment.NewLine;
                    sSql += "id_ctt_chofer, id_ctt_asistente, id_ctt_vehiculo, id_ctt_anden, id_ctt_tipo_servicio," + Environment.NewLine;
                    sSql += "id_ctt_itinerario, codigo, numero_viaje, fecha_viaje, estado_salida, asientos_ocupados," + Environment.NewLine;
                    sSql += "cobrar_administracion, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += Convert.ToInt32(cmbListarChoferes.SelectedValue) + ", " + Convert.ToInt32(cmbListarAsistentes.SelectedValue) + ", ";
                    sSql += Convert.ToInt32(cmbListarVehiculo.SelectedValue) + ", " + Convert.ToInt32(cmbListarAndenes.SelectedValue) + ", ";
                    sSql += Convert.ToInt32(row.Cells[3].Text) + ", " + Convert.ToInt32(row.Cells[1].Text) + "," + Environment.NewLine;
                    sSql += "'" + iCodigo.ToString() + "', " + iNumeroViaje + ", '" + sFecha + "'," + Environment.NewLine;
                    sSql += "'Abierta', 0, 1, 'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
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
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); }
        fin: { };
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
            insertarRegistro();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar();
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
                Session["fecha"] = txtFecha.Text.Trim();
                consultarFrecuenciasCreadas(Session["fecha"].ToString());
                btnGenerar.Visible = false;
            }

            else
            {
                Session["fecha"] = txtFecha.Text.Trim();
                consultarFrecuenciasCreadas(Session["fecha"].ToString());
            }
        }
    }
}