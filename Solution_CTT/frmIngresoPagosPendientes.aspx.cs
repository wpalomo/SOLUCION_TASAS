using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ENTIDADES;
using NEGOCIO;
using System.Data;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmIngresoPagosPendientes : System.Web.UI.Page
    {
        ENTComboDatos comboE = new ENTComboDatos();
        ENTChoferAsistente choferAsistenteE = new ENTChoferAsistente();
        ENTItinierario itinerarioE = new ENTItinierario();
        ENTPagosPendientes pendienteE = new ENTPagosPendientes();

        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorConexion conexionM = new manejadorConexion();        
        manejadorChoferAsistente choferAsistenteM = new manejadorChoferAsistente();
        manejadorItinerario itinerarioM = new manejadorItinerario();
        manejadorPagosPendientes pendienteM = new manejadorPagosPendientes();

        Clases.ClaseIngresarDeudaPendiente ingresoDeuda = new Clases.ClaseIngresarDeudaPendiente();

        string sSql;
        string[] sDatosMaximo = new string[5];
        string sAccion;
        string sTabla;
        string sCampo;
        string sFecha;
        string sAccionFiltro;
        string sAccionFiltroItinerario;

        DataTable dtConsulta;
        bool bRespuesta;

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

            Session["modulo"] = "MÓDULO DE INGRESO DE PAGOS PENDIENTES";

            if (!IsPostBack)
            {
                verificarPermiso();
                llenarComboVehiculo();
                llenarGridPendientes();
                consultarAsistenteDefault();
                consultarAnden();
                consultarIdNormal();
                txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        #region FUNCIONES DEL USUARIO

        //CONSULTAR PERMISOS
        private void verificarPermiso()
        {
            try
            {
                if ((Session["ejecuta_cobro_administrativo"] == null) || (Session["ejecuta_cobro_administrativo"].ToString() == "0"))
                {
                    Response.Redirect("frmMensajePermisos.aspx");
                }

                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        private void columnasGridAsistente(bool ok)
        {
            dgvAsistentesChofer.Columns[1].Visible = ok;
            dgvAsistentesChofer.Columns[4].Visible = ok;
            dgvAsistentesChofer.Columns[0].ItemStyle.Width = 0x4b;
            dgvAsistentesChofer.Columns[2].ItemStyle.Width = 200;
            dgvAsistentesChofer.Columns[3].ItemStyle.Width = 150;
            dgvAsistentesChofer.Columns[4].ItemStyle.Width = 300;
            dgvAsistentesChofer.Columns[5].ItemStyle.Width = 100;
            dgvAsistentesChofer.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvAsistentesChofer.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        }

        private void columnasGridItinerario(bool ok)
        {
            try
            {
                dgvItinerarios.Columns[4].ItemStyle.Width = 200;
                dgvItinerarios.Columns[5].ItemStyle.Width = 150;
                dgvItinerarios.Columns[6].ItemStyle.Width = 100;
                dgvItinerarios.Columns[7].ItemStyle.Width = 100;
                dgvItinerarios.Columns[0].Visible = ok;
                dgvItinerarios.Columns[1].Visible = ok;
                dgvItinerarios.Columns[2].Visible = ok;
                dgvItinerarios.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvItinerarios.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvItinerarios.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        private void columnasGridPendiente(bool ok)
        {
            dgvDetalle.Columns[0].Visible = ok;
            dgvDetalle.Columns[5].Visible = ok;
            dgvDetalle.Columns[9].Visible = ok;
            dgvDetalle.Columns[10].Visible = ok;
            dgvDetalle.Columns[1].ItemStyle.Width = 0x4b;
            dgvDetalle.Columns[2].ItemStyle.Width = 100;
            dgvDetalle.Columns[3].ItemStyle.Width = 100;
            dgvDetalle.Columns[4].ItemStyle.Width = 150;
            dgvDetalle.Columns[5].ItemStyle.Width = 150;
            dgvDetalle.Columns[6].ItemStyle.Width = 200;
            dgvDetalle.Columns[7].ItemStyle.Width = 100;
            dgvDetalle.Columns[8].ItemStyle.Width = 100;
        }

        //FUNCION PARA CONSULTAR EÑ ANDEN
        private void consultarAnden()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_anden, descripcion" + Environment.NewLine;
                sSql += "from ctt_anden" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(Session["id_pueblo"].ToString()) + Environment.NewLine;
                sSql += "and anden_principal = 1" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["idAnden"] = dtConsulta.Rows[0]["id_ctt_anden"].ToString();
                    }
                    else
                    {
                        Session["idAnden"] = "1";
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

        //FUNCION PARA CONSULTAR EL ASISTENTE DEFAULT
        private void consultarAsistenteDefault()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_asistente, descripcion" + Environment.NewLine;
                sSql += "from ctt_asistente" + Environment.NewLine;
                sSql += "where seleccion_default = 1" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["id_Asistente"] = dtConsulta.Rows[0][0].ToString();
                        Session["nombre_Asistente"] = dtConsulta.Rows[0][1].ToString();
                    }

                    else
                    {
                        Session["id_Asistente"] = null;
                        Session["nombre_Asistente"] = null;
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

        //FUNCION PARA CONSULTAR EL ID DEL TIPO DE SEVICIO NORMAL
        private void consultarIdNormal()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tipo_servicio" + Environment.NewLine;
                sSql += "from ctt_tipo_servicio" + Environment.NewLine;
                sSql += "where normal = 1" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    Session["id_tipo_viaje"] = dtConsulta.Rows[0][0].ToString();
                }
                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONSULTAR EL CODIGO MAXIMO
        private void consultarMaximoCodigo()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(max(codigo), 0) codigo" + Environment.NewLine;
                sSql += "from ctt_programacion";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["codigo_max"] = (Convert.ToInt32(dtConsulta.Rows[0][0].ToString()) + 1).ToString();
                    }

                    else
                    {
                        Session["codigo_max"] = "0";
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

        //FUNCION PARA CREAR EL VIAJE
        private bool insertarCrearViaje()
        {
            try
            {
                sFecha = Convert.ToDateTime(txtFecha.Text.Trim()).ToString("yyyy/MM/dd");

                consultarMaximoCodigo();
                numeroViaje();

                if (!conexionM.iniciarTransaccion())
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar iniciar la transacción.', 'danger');", true);
                    return false;
                }

                sSql = "";
                sSql += "insert into ctt_programacion (" + Environment.NewLine;
                sSql += "id_ctt_chofer, id_ctt_asistente, id_ctt_vehiculo, id_ctt_anden, id_ctt_tipo_servicio," + Environment.NewLine;
                sSql += "id_ctt_itinerario, codigo, numero_viaje, fecha_viaje, estado_salida, asientos_ocupados," + Environment.NewLine;
                sSql += "cobrar_administracion, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso, visualizar)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Session["id_Chofer"].ToString()) + ", " + Convert.ToInt32(Session["id_Asistente"].ToString()) +  ", ";
                sSql += Convert.ToInt32(cmbVehiculos.SelectedValue) + ", " + Convert.ToInt32(Session["idAnden"].ToString()) + ", ";
                sSql += Convert.ToInt32(Session["id_tipo_viaje"].ToString()) + ", " + Convert.ToInt32(Session["id_Itinerario"].ToString()) + "," + Environment.NewLine;
                sSql += "'" + Session["codigo_max"].ToString() + "', " + Convert.ToInt32(Session["numero_viaje"].ToString()) + ", '" + sFecha + "'," + Environment.NewLine;
                sSql += "'Cerrada', 0, 1, 'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 0)";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    conexionM.reversaTransaccion();
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                sTabla = "ctt_programacion";
                sCampo = "id_ctt_programacion";
                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1L)
                {
                    conexionM.reversaTransaccion();
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>No se pudo obtener el código de la tabla " + sTabla + ".";
                    ScriptManager.RegisterStartupScript(this, base.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                Session["id_Programacion"] = Convert.ToInt32(iMaximo);
                conexionM.terminaTransaccion();
                return true;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                conexionM.reversaTransaccion();
                return false;
            }
        }

        //FUNCION PARA INSERTAR LA DEUDA PENDIENTE
        private bool insertarRegistro()
        {
            try
            {
                if (this.insertarCrearViaje())
                {
                    if (!ingresoDeuda.crearPagoPendiente(Convert.ToInt32(Session["id_programacion"].ToString()), Convert.ToInt32(cmbVehiculos.SelectedValue), Convert.ToDateTime(txtFecha.Text.Trim()).ToString("yyyy/MM/dd"), sDatosMaximo, Convert.ToDecimal(txtValor.Text.Trim()), txtObservaciones.Text.Trim().ToUpper()))
                    {
                        conexionM.reversaTransaccion();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ocurrió un problema al ingresar el pago pendiente.', 'danger');", true);
                    }
                    else
                    {
                        conexionM.terminaTransaccion();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Se ha ingreso el pago éxitosamente.', 'success');", true);
                        limpiar();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ocurrió un problema al ingresar el pago pendiente.', 'danger');", true);
                }

                return true;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA LLENAR EL COMBO DE VEHICULOS
        private void llenarComboVehiculo()
        {
            try
            {
                sSql = "";
                sSql += "select V.id_ctt_vehiculo, D.descripcion + '-' + V.placa vehiculo" + Environment.NewLine;
                sSql += "from ctt_vehiculo V, ctt_disco D" + Environment.NewLine;
                sSql += "where V.id_ctt_disco = D.id_ctt_disco" + Environment.NewLine;
                sSql += "and V.estado = 'A'" + Environment.NewLine;
                sSql += "and D.estado = 'A'" + Environment.NewLine;
                sSql += "and V.is_active = 1" + Environment.NewLine;
                sSql += "order by D.descripcion";

                comboE.ISSQL = sSql;
                cmbVehiculos.DataSource = comboM.listarCombo(comboE);
                cmbVehiculos.DataValueField = "IID";
                cmbVehiculos.DataTextField = "IDATO";
                cmbVehiculos.DataBind();
                cmbVehiculos.Items.Insert(0, new ListItem("Todos...!!!", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL GRID DE CHOFERES
        private void llenarGridChofer(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select C.id_ctt_chofer, C.descripcion, C.codigo," + Environment.NewLine;
                sSql += "ltrim(isnull(TP.nombres, '') + ' ' + TP.apellidos) chofer" + Environment.NewLine;
                sSql += "from ctt_chofer C, tp_personas TP" + Environment.NewLine;
                sSql += "where C.id_persona = TP.id_persona" + Environment.NewLine;
                sSql += "and C.estado = 'A'" + Environment.NewLine;
                sSql += "and TP.estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and C.descripcion like '%" + txtFiltrarChoferAsistente.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by C.descripcion";

                columnasGridAsistente(true);
                choferAsistenteE.ISQL = sSql;
                dgvAsistentesChofer.DataSource = choferAsistenteM.listarChoferAsistente(choferAsistenteE);
                dgvAsistentesChofer.DataBind();
                columnasGridAsistente(false);
            }

            catch (Exception ex)
            {
                ModalPopupExtender_AsistentesChofer.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL GRID CON LOS ITINERARIOS CONFIGURADOS
        private void llenarGridItinerarios(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select I.id_ctt_itinerario, I.id_ctt_ruta, I.id_ctt_horario," + Environment.NewLine;
                sSql += "I.codigo, R.descripcion, P.descripcion destino," + Environment.NewLine;
                sSql += "H.hora_salida, case I.estado when 'A' then 'ACTIVO' else 'INACTIVO' end estado" + Environment.NewLine;
                sSql += "from ctt_ruta R INNER JOIN" + Environment.NewLine;
                sSql += "ctt_itinerario I ON I.id_ctt_ruta = R.id_ctt_ruta" + Environment.NewLine;
                sSql += "and R.estado = 'A'" + Environment.NewLine;
                sSql += "and I.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_horarios H ON H.id_ctt_horario = I.id_ctt_horario" + Environment.NewLine;
                sSql += "and H.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_pueblos P ON P.id_ctt_pueblo = R.id_ctt_pueblo_destino" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "where I.codigo = '" + txtFiltrarItinerarios.Text.Trim().ToUpper() + "'" + Environment.NewLine;
                }

                sSql += "order by H.hora_salida";

                columnasGridItinerario(true);
                itinerarioE.ISQL = sSql;
                dgvItinerarios.DataSource = itinerarioM.listarItinerario(itinerarioE);
                dgvItinerarios.DataBind();
                columnasGridItinerario(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL GRID DE PAGOS PENDIENTES
        private void llenarGridPendientes()
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_pagos_pendientes_itinerario";

                columnasGridPendiente(true);
                pendienteE.ISQL = sSql;
                dgvDetalle.DataSource = pendienteM.listarPagosPendientes(pendienteE);
                dgvDetalle.DataBind();
                columnasGridPendiente(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA EXTRAER EL NUMERO DE VIAJES
        private void numeroViaje()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(max(numero_viaje), 0) maximo" + Environment.NewLine;
                sSql += "from ctt_programacion" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta)
                {
                    Session["numero_viaje"] = (Convert.ToInt32(dtConsulta.Rows[0][0].ToString()) + 1).ToString();
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




 


        private void limpiar()
        {
            llenarComboVehiculo();
            llenarGridPendientes();
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtValor.Text = "0.00";
            txtObservaciones.Text = "";
            txtItinerario.Text = "";
            Session["id_programacion"] = null;
            Session["id_Itinerario"] = null;
        }



 



        #endregion

        protected void btnAbrirModalChofer_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_AsistentesChofer.Show();
            llenarGridChofer(0);
        }

        protected void btnAbrirModalItinerario_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Itinerarios.Show();
            llenarGridItinerarios(0);
        }

        protected void btnAceptarCerrar_Click(object sender, EventArgs e)
        {
            insertarRegistro();
        }

        protected void btnCerrarModalItinerario_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Itinerarios.Hide();
        }

        protected void btnCerrarModalPersonas_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_AsistentesChofer.Hide();
        }

        protected void btnFiltarChoferAsistente_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFiltrarChoferAsistente.Text.Trim() == "")
                {
                    llenarGridChofer(0);
                }
                else
                {
                    llenarGridChofer(1);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnFiltarItinearios_Click(object sender, EventArgs e)
        {
            if (txtFiltrarItinerarios.Text.Trim() == "")
            {
                llenarGridItinerarios(0);
            }
            else
            {
                llenarGridItinerarios(1);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbVehiculos.SelectedValue) == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione el vehículo.', 'info');", true);
            }

            else if (txtFecha.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione la fecha para ingresar el registro.', 'info');", true);
            }

            else if (Session["id_Itinerario"] == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione el itinerario para el registro.', 'info');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#QuestionModalConfirmar').modal('show');</script>", false);
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void dgvAsistentesChofer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvAsistentesChofer.PageIndex = e.NewPageIndex;

                if (txtFiltrarChoferAsistente.Text.Trim() == "")
                {
                    llenarGridChofer(0);
                }

                else
                {
                    llenarGridChofer(1);
                }
            }

            catch (Exception ex)
            {
                ModalPopupExtender_AsistentesChofer.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvAsistentesChofer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = dgvAsistentesChofer.SelectedIndex;

                columnasGridAsistente(true);

                if (sAccionFiltro == "Seleccion")
                {
                    Session["id_Chofer"] = dgvAsistentesChofer.Rows[selectedIndex].Cells[1].Text.Trim();
                    txtChofer.Text = dgvAsistentesChofer.Rows[selectedIndex].Cells[2].Text.Trim();
                    txtFiltrarChoferAsistente.Text = "";
                    ModalPopupExtender_AsistentesChofer.Hide();
                }

                columnasGridAsistente(false);
            }

            catch (Exception ex)
            {
                ModalPopupExtender_AsistentesChofer.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvDetalle.PageIndex = e.NewPageIndex;
                llenarGridPendientes();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvItinerarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvItinerarios.PageIndex = e.NewPageIndex;

            if (txtFiltrarItinerarios.Text.Trim() == "")
            {
                llenarGridItinerarios(0);
            }

            else
            {
                llenarGridItinerarios(1);
            }
        }

        protected void dgvItinerarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = dgvItinerarios.SelectedIndex;
                columnasGridItinerario(true);
                Session["id_Itinerario"] = dgvItinerarios.Rows[selectedIndex].Cells[0].Text.Trim();

                if (sAccionFiltroItinerario == "Seleccion")
                {
                    txtItinerario.Text = "C\x00d3DIGO: " + dgvItinerarios.Rows[selectedIndex].Cells[3].Text.Trim() + " - RUTA: " + dgvItinerarios.Rows[selectedIndex].Cells[4].Text.Trim() + " - HORA DE SALIDA: " + dgvItinerarios.Rows[selectedIndex].Cells[6].Text.Trim();
                    txtFiltrarItinerarios.Text = "";
                    ModalPopupExtender_Itinerarios.Hide();
                }

                columnasGridItinerario(false);
            }

            catch (Exception ex)
            {
                this.ModalPopupExtender_Itinerarios.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnSeleccionAsistenteChofer_Click(object sender, EventArgs e)
        {
            sAccionFiltro = "Seleccion";
        }

        protected void lbtnSeleccionItinerario_Click(object sender, EventArgs e)
        {
            sAccionFiltroItinerario = "Seleccion";
        }

        protected void dgvDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < dgvDetalle.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvDetalle.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvDetalle.Rows[i].BackColor = Color.White;
                }
            }
        }

        protected void dgvItinerarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < dgvItinerarios.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvItinerarios.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvItinerarios.Rows[i].BackColor = Color.White;
                }
            }
        }

        protected void dgvAsistentesChofer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < dgvAsistentesChofer.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvAsistentesChofer.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvAsistentesChofer.Rows[i].BackColor = Color.White;
                }
            }
        }
    }
}