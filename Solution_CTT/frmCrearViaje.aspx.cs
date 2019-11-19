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
    public partial class frmCrearViaje : System.Web.UI.Page
    {
        ENTProgramacion programacionE = new ENTProgramacion();
        ENTComboDatos comboE = new ENTComboDatos();
        ENTExtras extraE = new ENTExtras();
        ENTChoferAsistente choferAsistenteE = new ENTChoferAsistente();
        ENTVehiculoViaje vehiculoViajeE = new ENTVehiculoViaje();
        ENTItinierario itinerarioE = new ENTItinierario();

        manejadorChoferAsistente choferAsistenteM = new manejadorChoferAsistente();
        manejadorVehiculoViaje vehiculoViajeM = new manejadorVehiculoViaje();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorProgramacion programacionM = new manejadorProgramacion();
        manejadorExtras extraM = new manejadorExtras();
        manejadorItinerario itinerarioM = new manejadorItinerario();
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sFecha;
        string sAccion;
        string sAccionFiltro;
        string sAccionFiltroItinerario;
        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;

        bool bRespuesta;

        double dbValorActual;
        double dbValorNuevo;

        int iIdProducto;
        int iCobrarAdministracion;

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

            Session["modulo"] = "MÓDULO PARA CREAR VIAJES NORMALES";

            if (!IsPostBack)
            {
                Session["idRegistro"] = null;
                sFecha = DateTime.Now.ToString("dd/MM/yyyy");
                txtDate.Text = sFecha;
                datosListas();
                llenarGrid(sFecha);
                //llenarGridItinerarios(0);
                llenarComboAndenes(1);
                consultarAsistenteDefault();
                consultarIdNormal();
            }
        }

        #region FUNCIONES DEL MODAL DE FILTRO DE VEHICULOS

        private void columnasGridVehiculo(bool ok)
        {
            dgvVehiculos.Columns[1].Visible = ok;
            dgvVehiculos.Columns[5].Visible = ok;

            dgvVehiculos.Columns[0].ItemStyle.Width = 75;
            dgvVehiculos.Columns[2].ItemStyle.Width = 150;
            dgvVehiculos.Columns[3].ItemStyle.Width = 150;
            dgvVehiculos.Columns[4].ItemStyle.Width = 150;
            dgvVehiculos.Columns[6].ItemStyle.Width = 100;


            dgvVehiculos.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvVehiculos.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvVehiculos.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvVehiculos.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        }

        //FUNCION PARA LLENAR EL GRID DE ASISTENTES
        private void llenarGridVehiculo(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select V.id_ctt_vehiculo, V.placa, D.descripcion disco," + Environment.NewLine;
                sSql += "TV.descripcion tipo_vehiculo, D.descripcion + ' - ' + V.placa registro" + Environment.NewLine;
                sSql += "from ctt_vehiculo V, ctt_disco D, ctt_tipo_vehiculo TV" + Environment.NewLine;
                sSql += "where V.id_ctt_disco = D.id_ctt_disco" + Environment.NewLine;
                sSql += "and V.id_ctt_tipo_vehiculo = TV.id_ctt_tipo_vehiculo" + Environment.NewLine;
                sSql += "and V.estado = 'A'" + Environment.NewLine;
                sSql += "and D.estado = 'A'" + Environment.NewLine;
                sSql += "and TV.estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and D.descripcion like '%" + txtFiltrarVehiculos.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by D.descripcion";

                columnasGridVehiculo(true);
                vehiculoViajeE.ISQL = sSql;
                dgvVehiculos.DataSource = vehiculoViajeM.listarVehiculoViaje(vehiculoViajeE);
                dgvVehiculos.DataBind();
                columnasGridVehiculo(false);
            }

            catch (Exception ex)
            {
                ModalPopupExtender_Vehiculo.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCIONES DEL MODAL DE FILTRO DE ASISTENTES Y CHOFERES

        private void columnasGridAsistente(bool ok)
        {
            dgvAsistentesChofer.Columns[1].Visible = ok;
            dgvAsistentesChofer.Columns[4].Visible = ok;

            dgvAsistentesChofer.Columns[0].ItemStyle.Width = 75;
            dgvAsistentesChofer.Columns[2].ItemStyle.Width = 200;
            dgvAsistentesChofer.Columns[3].ItemStyle.Width = 150;
            dgvAsistentesChofer.Columns[4].ItemStyle.Width = 300;
            dgvAsistentesChofer.Columns[5].ItemStyle.Width = 100;


            dgvAsistentesChofer.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvAsistentesChofer.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        }

        //FUNCION PARA LLENAR EL GRID DE ASISTENTES
        private void llenarGridAsistente(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select A.id_ctt_asistente, A.descripcion, A.codigo," + Environment.NewLine;
                sSql += "ltrim(isnull(TP.nombres, '') + ' ' + TP.apellidos) asistente" + Environment.NewLine;
                sSql += "from ctt_asistente A, tp_personas TP" + Environment.NewLine;
                sSql += "where A.id_persona = TP.id_persona" + Environment.NewLine;
                sSql += "and A.estado = 'A'" + Environment.NewLine;
                sSql += "and TP.estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and A.descripcion like '%" + txtFiltrarChoferAsistente.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by A.descripcion";

                columnasGridAsistente(true);
                choferAsistenteE.ISQL = sSql;
                dgvAsistentesChofer.DataSource = choferAsistenteM.listarChoferAsistente(choferAsistenteE);
                dgvAsistentesChofer.DataBind();
                columnasGridAsistente(false);
            }

            catch (Exception ex)
            {
                ModalPopupExtender_AsistentesChofer.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCIONES DEL MODAL DE FILTRO DE PRECIOS

        private void columnasGridPrecios(bool ok)
        {
            dgvExtras.Columns[1].Visible = ok;

            dgvExtras.Columns[0].ItemStyle.Width = 75;
            dgvExtras.Columns[2].ItemStyle.Width = 300;
            dgvExtras.Columns[3].ItemStyle.Width = 150;
            dgvExtras.Columns[4].ItemStyle.Width = 150;

            dgvExtras.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvExtras.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        }

        //FUNCION PARA LLENAR EL GRID DE PRECIOS
        private void llenarGridPrecios(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select P.id_producto, NP.nombre," + Environment.NewLine;
                sSql += "ltrim(str(PP.valor, 10, 2)) valor" + Environment.NewLine;
                sSql += "from cv401_productos P, cv401_productos PADRE," + Environment.NewLine;
                sSql += "cv403_precios_productos PP, cv401_nombre_productos NP" + Environment.NewLine;
                sSql += "where P.id_producto_padre = PADRE.id_producto" + Environment.NewLine;
                sSql += "and PP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and NP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and PADRE.id_ctt_pueblo_origen = " + Convert.ToInt32(Session["id_pueblo"].ToString()) + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and PADRE.estado = 'A'" + Environment.NewLine;
                sSql += "and PP.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "and id_lista_precio = " + Convert.ToInt32(Session["lista_minorista"]) + Environment.NewLine;
                sSql += "and P.aplica_extra = 1" + Environment.NewLine;
                sSql += "order by P.id_producto";

                columnasGridPrecios(true);
                extraE.ISQL = sSql;
                dgvExtras.DataSource = extraM.listarExtra(extraE);
                dgvExtras.DataBind();
                columnasGridPrecios(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCION DEL USUARIO

        //CONSULTAR ID DEL TIPO SERVICIO
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                sSql += "and estado = 'A'" + Environment.NewLine;

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA OBTENER LOS DATOS DE LA LISTA BASE Y MINORISTA
        private void datosListas()
        {
            try
            {
                sSql = "";
                sSql += "select id_lista_precio, fecha_fin_validez" + Environment.NewLine;
                sSql += "from cv403_listas_precios" + Environment.NewLine;
                sSql += "where lista_base = 1" + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "union" + Environment.NewLine;
                sSql += "select id_lista_precio, fecha_fin_validez" + Environment.NewLine;
                sSql += "from cv403_listas_precios" + Environment.NewLine;
                sSql += "where lista_minorista = 1" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["lista_base"] = dtConsulta.Rows[0]["id_lista_precio"].ToString();
                        Session["fecha_base"] = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_fin_validez"].ToString()).ToString("yyyy/MM/dd");
                        Session["lista_minorista"] = dtConsulta.Rows[1]["id_lista_precio"].ToString();
                        Session["fecha_minorista"] = Convert.ToDateTime(dtConsulta.Rows[1]["fecha_fin_validez"].ToString()).ToString("yyyy/MM/dd");
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONTROLAR EL CODIGO DE VIAJE, EN ESTE CASO SOLO SE MANEJARÁ NUMERICO
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

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        txtCodigo.Text = (Convert.ToInt32(dtConsulta.Rows[0][0].ToString()) + 1).ToString();
                    }

                    else
                    {
                        txtCodigo.Text = "0";
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LAS COLUMNAS
        private void columnasGrid(bool ok)
        {
            try
            {
                dgvDatos.Columns[1].Visible = ok;
                dgvDatos.Columns[10].Visible = ok;
                dgvDatos.Columns[11].Visible = ok;
                dgvDatos.Columns[12].Visible = ok;
                dgvDatos.Columns[13].Visible = ok;
                dgvDatos.Columns[14].Visible = ok;
                dgvDatos.Columns[15].Visible = ok;
                dgvDatos.Columns[16].Visible = ok;
                dgvDatos.Columns[17].Visible = ok;
                dgvDatos.Columns[18].Visible = ok;
                dgvDatos.Columns[19].Visible = ok;
                dgvDatos.Columns[20].Visible = ok;
                dgvDatos.Columns[21].Visible = ok;
                dgvDatos.Columns[22].Visible = ok;
                dgvDatos.Columns[23].Visible = ok;
                dgvDatos.Columns[24].Visible = ok;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LAS COLUMNAS ITINERARIOS
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE HORARIOS NORMALES
        private void llenarGridItinerarios(int iOp)
        {
            try
            {
                //sSql = "";
                //sSql += "select I.id_ctt_itinerario, I.id_ctt_ruta, I.id_ctt_horario, " + Environment.NewLine;
                //sSql += "I.id_ctt_tipo_servicio, I.codigo, R.descripcion, P.descripcion destino," + Environment.NewLine;
                //sSql += "H.hora_salida, S.descripcion tipo_viaje," + Environment.NewLine;
                //sSql += "case I.estado when 'A' then 'ACTIVO' else 'INACTIVO' end estado" + Environment.NewLine;
                //sSql += "from ctt_ruta R INNER JOIN" + Environment.NewLine;
                //sSql += "ctt_itinerario I ON I.id_ctt_ruta = R.id_ctt_ruta" + Environment.NewLine;
                //sSql += "and R.estado = 'A'" + Environment.NewLine;
                //sSql += "and I.estado = 'A' INNER JOIN" + Environment.NewLine;
                //sSql += "ctt_horarios H ON H.id_ctt_horario = I.id_ctt_horario" + Environment.NewLine;
                //sSql += "and H.estado = 'A' INNER JOIN" + Environment.NewLine;
                //sSql += "ctt_tipo_servicio S ON S.id_ctt_tipo_servicio = I.id_ctt_tipo_servicio" + Environment.NewLine;
                //sSql += "and S.estado = 'A' INNER JOIN" + Environment.NewLine;
                //sSql += "ctt_pueblos P ON P.id_ctt_pueblo = R.id_ctt_pueblo_destino" + Environment.NewLine;
                //sSql += "where S.normal = 1" + Environment.NewLine;

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
                sSql += "and R.id_ctt_pueblo_origen = " + Session["id_pueblo"].ToString() + Environment.NewLine;

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE ANDENES
        private void llenarComboAndenes(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_anden, descripcion" + Environment.NewLine;
                sSql += "from ctt_anden" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(Session["id_pueblo"].ToString()) + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and anden_principal = 1" + Environment.NewLine;
                }

                sSql += "and estado = 'A'";

                comboE.ISSQL = sSql;
                cmbAnden.DataSource = comboM.listarCombo(comboE);
                cmbAnden.DataValueField = "IID";
                cmbAnden.DataTextField = "IDATO";
                cmbAnden.DataBind();

                if (iOp == 0)
                {
                    cmbAnden.Items.Insert(0, new ListItem("Seleccione..!!", "0"));
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(string sFecha_P)
        {
            try
            {
                sFecha_P = Convert.ToDateTime(sFecha_P).ToString("yyyy/MM/dd");

                sSql = "";
                sSql += "select * from ctt_vw_crear_programacion" + Environment.NewLine;
                sSql += "where fecha_viaje = '" + sFecha_P + "'" + Environment.NewLine;
                sSql += "and id_ctt_pueblo = " + Convert.ToInt32(Session["id_pueblo"].ToString()) + Environment.NewLine;
                sSql += "order by hora_salida" + Environment.NewLine;

                columnasGrid(true);
                programacionE.ISSQL = sSql;
                dgvDatos.DataSource = programacionM.listarProgramacion(programacionE);
                dgvDatos.DataBind();
                columnasGrid(false);

            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Ya existe un registro con el código ingresado.', 'warning');", true);
                    goto fin;
                }

                else if (consultarRegistro() == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ocurrió un problema al consultar el código para el registro.', 'danger');", true);
                    goto fin;
                }

                if (conexionM.iniciarTransaccion() == false)
                {
                    //MENSAJE DE ERROR
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar iniciar la transacción.', 'danger');", true);
                    goto fin;
                }

                consultarMaximoCodigo();
                numeroViaje();

                sFecha = Convert.ToDateTime(TxtFechaViaje.Text.Trim()).ToString("yyyy/MM/dd");

                sSql = "";
                sSql += "insert into ctt_programacion (" + Environment.NewLine;
                sSql += "id_ctt_chofer, id_ctt_asistente, id_ctt_vehiculo, id_ctt_anden, id_ctt_tipo_servicio," + Environment.NewLine;
                sSql += "id_ctt_itinerario, codigo, numero_viaje, fecha_viaje, estado_salida, asientos_ocupados," + Environment.NewLine;
                sSql += "cobrar_administracion, id_ctt_pueblo_origen, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Session["id_Chofer"].ToString()) + ", " + Convert.ToInt32(Session["id_Asistente"].ToString()) + ", ";
                sSql += Convert.ToInt32(Session["id_Vehiculo"].ToString()) + ", " + Convert.ToInt32(cmbAnden.SelectedValue) + ", ";
                sSql += Convert.ToInt32(Session["id_tipo_viaje"].ToString()) + ", " + Convert.ToInt32(Session["id_Itinerario"].ToString()) + "," + Environment.NewLine;
                sSql += "'" + txtCodigo.Text.Trim().ToUpper() + "', " + Convert.ToInt32(txtNumeroViaje.Text.Trim().ToUpper()) + ", '" + sFecha + "'," + Environment.NewLine;
                sSql += "'Abierta', 0, " + iCobrarAdministracion + ", " + Session["id_pueblo"].ToString() + ", 'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUCIÓN DE INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    //MENSAJE DE ERROR
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro insertado éxitosamente.', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); }
        fin: { };
        }

        //FUNCION PARA ACTUALIZAR EN LA BASE DE DATOS
        private void actualizarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    //MENSAJE DE ERROR
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar iniciar la transacción.', 'danger');", true);
                    goto fin;
                }

                sFecha = Convert.ToDateTime(TxtFechaViaje.Text.Trim()).ToString("yyyy/MM/dd");

                sSql = "";
                sSql += "update ctt_programacion set" + Environment.NewLine;
                sSql += "id_ctt_chofer = " + Convert.ToInt32(Session["id_Chofer"].ToString()) + "," + Environment.NewLine;
                sSql += "id_ctt_asistente = " + Convert.ToInt32(Session["id_Asistente"].ToString()) + "," + Environment.NewLine;
                sSql += "id_ctt_vehiculo = " + Convert.ToInt32(Session["id_Vehiculo"].ToString()) + "," + Environment.NewLine;
                sSql += "id_ctt_vehiculo_reemplazo = " + Convert.ToInt32(Session["idReemplazo"].ToString()) + "," + Environment.NewLine;
                sSql += "id_ctt_itinerario = " + Convert.ToInt32(Session["id_Itinerario"].ToString()) + "," + Environment.NewLine;
                sSql += "id_ctt_anden = " + Convert.ToInt32(cmbAnden.SelectedValue) + "," + Environment.NewLine;
                sSql += "codigo = '" + txtCodigo.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "fecha_viaje = '" + sFecha + "'," + Environment.NewLine;
                sSql += "cobrar_administracion = " + iCobrarAdministracion + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + Convert.ToInt32(Session["idRegistro"]) + Environment.NewLine;
                sSql += "and estado = 'A'";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    //MENSAJE DE ERROR
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro actualizado éxitosamente.', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); }
        fin: { };
        }

        //FUNCION PARA ELIMINAR EN LA BASE DE DATOS
        private void eliminarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    //MENSAJE DE ERROR
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar iniciar la transacción.', 'danger');", true);
                    goto fin;
                }

                sSql = "";
                sSql += "update ctt_programacion set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + Convert.ToInt32(Session["idRegistro"]);

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    //MENSAJE DE ERROR
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro eliminado éxitosamente.', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); }
        fin: { };
        }

        //FUNCION PARA CONSULTAR SI EXISTE EL REGISTRO EN LA BASE DE DATOS
        private int consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from ctt_programacion" + Environment.NewLine;
                //sSql += "where fecha_viaje = '" + Convert.ToDateTime(TxtFechaViaje.Text.Trim()).ToString("yyyy/MM/dd") + "'" + Environment.NewLine;
                sSql += "where fecha_viaje = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and id_ctt_itinerario = " + Convert.ToInt32(Session["id_Itinerario"].ToString()) + Environment.NewLine;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return -1;
            }
        }

        //FUNCION PARA CARGAR NÚMERO DE VIAJE
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

                if (bRespuesta == true)
                {
                    txtNumeroViaje.Text = (Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString()) + 1).ToString();
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    txtNumeroViaje.Text = "ERROR";
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                txtNumeroViaje.Text = "ERROR";
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            sFecha = DateTime.Now.ToString("dd/MM/yyyy");
            txtDate.Text = sFecha;
            llenarGrid(sFecha);
            txtCodigo.Text = "";
            txtNumeroViaje.Text = "";
            txtEstadoViaje.Text = "";
            TxtFechaViaje.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtVehiculo.Text = "";
            txtChofer.Text = "";
            txtAsistente.Text = "";
            txtItinerario.Text = "";
            
            Session["idRegistro"] = null;
            Session["id_Chofer"] = null;
            Session["id_Asistente"] = null;
            Session["id_Vehiculo"] = null;
            Session["id_Itinerario"] = null;
            Session["id_tipo_viaje"] = null;

            consultarAsistenteDefault();
            consultarIdNormal();

            btnEditaPrecios.Visible = false;

            pnlGrid.Visible = true;
            pnlRegistro.Visible = false;
            TxtFechaViaje.ReadOnly = false;
        }

        //FUNCION PARA CONSULTAR EL BUS DE REEMPLAZO
        private bool consultarBusReemplazo(int iIdRegistro_P)
        {
            try
            {
                sSql = "";
                sSql += "select P.id_ctt_vehiculo_reemplazo," + Environment.NewLine;
                sSql += "D.descripcion + ' - ' + V.placa transporte" + Environment.NewLine;
                sSql += "from ctt_programacion P, ctt_vehiculo V," + Environment.NewLine;
                sSql += "ctt_disco D" + Environment.NewLine;
                sSql += "where P.id_ctt_vehiculo_reemplazo = V.id_ctt_vehiculo" + Environment.NewLine;
                sSql += "and V.id_ctt_disco = D.id_ctt_disco" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and V.estado = 'A'" + Environment.NewLine;
                sSql += "and D.estado = 'A'" + Environment.NewLine;
                sSql += "and P.id_ctt_programacion = " + iIdRegistro_P;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["idReemplazo"] = dtConsulta.Rows[0][0].ToString();
                        txtVehiculoReemplazo.Text = dtConsulta.Rows[0][1].ToString();
                    }

                    else
                    {
                        Session["idReemplazo"] = "0";
                        txtVehiculoReemplazo.Text = "";
                    }

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }
        #endregion

        #region FUNCIONES PARA ACTUALIZAR LOS PRECIOS DE EXTRAS

        //FUNCION DE ACTUALIZACION DE PRECIOS
        private void actualizarPreciosExtra()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                columnasGridPrecios(true);

                foreach (GridViewRow row in dgvExtras.Rows)
                {
                    TextBox txtValorNuevo = row.FindControl("txtValorExtra") as TextBox;

                    if (txtValorNuevo.Text.Trim() == "")
                    {
                        goto ciclo;
                    }

                    else if (Convert.ToDouble(txtValorNuevo.Text.Trim()) == 0)
                    {
                        goto ciclo;
                    }

                    iIdProducto = Convert.ToInt32(row.Cells[1].Text);
                    dbValorNuevo = Convert.ToDouble(txtValorNuevo.Text.Trim());
                    dbValorActual = Convert.ToDouble(row.Cells[3].Text);

                    if (dbValorActual == dbValorNuevo)
                    {
                        goto ciclo;
                    }

                    //INSTRUCCION SQL PARA ELIMINAR EL PRECIO DEL EXTRA
                    sSql = "";
                    sSql += "update cv403_precios_productos set" + Environment.NewLine;
                    sSql += "estado = 'E'," + Environment.NewLine;
                    sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                    sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                    sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                    sSql += "where id_producto = " + iIdProducto + Environment.NewLine;
                    sSql += "and id_lista_precio = " + Convert.ToInt32(Session["lista_minorista"].ToString()) + Environment.NewLine;
                    sSql += "and estado = 'A'";

                    //EJECUCIÓN DE INSTRUCCION SQL
                    if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                    {
                        //MENSAJE DE ERROR
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        goto reversa;
                    }

                    //INSTRUCCION SQL PARA INSERTAR EL NUEVO PRECIO DEL EXTRA
                    //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV403_PRECIOS_PRODUCTOS (PRECIO MINORISTA)
                    sSql = "";
                    sSql += "insert into cv403_precios_productos (" + Environment.NewLine;
                    sSql += "id_lista_precio, id_producto, valor_porcentaje, valor, fecha_inicio," + Environment.NewLine;
                    sSql += "fecha_final, fecha_ingreso, usuario_ingreso, terminal_ingreso, estado)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += Convert.ToInt32(Session["lista_minorista"].ToString()) + ", " + iIdProducto + ", 0, ";
                    sSql += dbValorNuevo + ", '" + DateTime.Now.ToString("yyyy/MM/dd") + "'," + Environment.NewLine;
                    sSql += "'" + Convert.ToDateTime(Session["fecha_minorista"].ToString()).ToString("yyy/MM/dd") + "', GETDATE()," + Environment.NewLine;
                    sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A')";

                    //EJECUCION DE LA INSTRUCCION SQL
                    if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        goto reversa;
                    }

                ciclo: { }
                }

                columnasGridPrecios(false);

                conexionM.terminaTransaccion();
                btnPopUp_ModalPopupExtender.Hide();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Valores actualizados éxitosamente', 'success');", true);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); }
        fin: { }
        }

        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                //sFecha = DateTime.Now.ToString("yyyy/MM/dd");

                if (txtNumeroViaje.Text.Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor comuníquese con el administrador del sistema.', 'warning');", true);
                }

                else if (txtCodigo.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese un código.', 'warning');", true);
                    txtCodigo.Focus();
                }

                else if (TxtFechaViaje.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese la fecha de salida del viaje.', 'warning');", true);
                    TxtFechaViaje.Focus();
                }

                else if (Convert.ToDateTime(TxtFechaViaje.Text.Trim()) < Convert.ToDateTime(sFecha))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'La fecha ingresada ya ha pasado. Favor ingrese una nueva.', 'warning');", true);
                    TxtFechaViaje.Text = "";
                    TxtFechaViaje.Focus();
                }

                else if (txtVehiculo.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione el vehículo para el viaje.', 'warning');", true);
                    btnAbrirModalVehiculo.Focus();
                }

                else if (txtChofer.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione el chofer para el viaje.', 'warning');", true);
                    btnAbrirModalChofer.Focus();
                }

                else if (txtAsistente.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione el asistente para el viaje.', 'warning');", true);
                    btnAbrirModalAsistente.Focus();
                }

                else if (Convert.ToInt32(cmbAnden.SelectedValue) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione el andén de salida.', 'warning');", true);
                    cmbAnden.Focus();
                }

                else if (txtItinerario.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione el itinerario para el viaje.', 'warning');", true);
                    txtItinerario.Focus();
                }

                else
                {
                    sFecha = DateTime.Now.ToString("yyyy/MM/dd");

                    //VALIDACION DE FECHAS Y HORAS
                    //if (Convert.ToDateTime(TxtFechaViaje.Text.Trim()) < Convert.ToDateTime(sFecha))
                    //{
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No se puede crear frecuencias con fechas anteriores.', 'warning');", true);
                    //    return;
                    //}

                    if (chkEjecutarCobro.Checked == true)
                    {
                        iCobrarAdministracion = 1;
                    }

                    else
                    {
                        iCobrarAdministracion = 0;
                    }

                    if (Session["idRegistro"] != null)
                    {
                        if (Session["id_Vehiculo"].ToString() == Session["idReemplazo"].ToString())
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El vehículo de reemplazo no puede ser el mismo el vehículo asignado.', 'warning');", true);
                            Session["idReemplazo"] = "0";
                            txtVehiculoReemplazo.Text = "";
                        }

                        else
                        {
                            actualizarRegistro();
                        }

                        return;
                    }

                    if (Convert.ToDateTime(TxtFechaViaje.Text.Trim()) == Convert.ToDateTime(sFecha))
                    {
                        DateTime hora_1 = Convert.ToDateTime(Session["hora_seleccionada"].ToString());
                        DateTime hora_2 = DateTime.Now;

                        if (hora_1 < hora_2)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'La hora seleccionada es superior a la del sistema. Favor seleccione un nuevo itinerario.', 'warning');", true);
                            return;
                        }

                        else
                        {
                            sFecha = Convert.ToDateTime(TxtFechaViaje.Text.Trim()).ToString("yyyy/MM/dd");
                            insertarRegistro();
                            return;
                        }
                    }

                    else if (Convert.ToDateTime(TxtFechaViaje.Text.Trim()) > Convert.ToDateTime(sFecha))
                    {
                        sFecha = Convert.ToDateTime(TxtFechaViaje.Text.Trim()).ToString("yyyy/MM/dd");
                        insertarRegistro();
                        return;
                    }

                    //if (Convert.ToDateTime(TxtFechaViaje.Text.Trim()) == Convert.ToDateTime(sFecha))
                    //{
                    //    DateTime hora_1 = Convert.ToDateTime(Session["hora_seleccionada"].ToString());
                    //    DateTime hora_2 = DateTime.Now;

                    //    if (hora_1 < hora_2)
                    //    {
                    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'La hora seleccionada es superior a la del sistema. Favor seleccione un nuevo itinerario.', 'warning');", true);
                    //        return;
                    //    }

                    //    else
                    //    {
                    //        sFecha = Convert.ToDateTime(TxtFechaViaje.Text.Trim()).ToString("yyyy/MM/dd");

                    //        if (Session["idRegistro"] == null)
                    //        {
                    //            //ENVIO A FUNCION DE INSERCION
                    //            insertarRegistro();
                    //        }

                    //        else
                    //        {
                    //            if (Session["id_Vehiculo"].ToString() == Session["idReemplazo"].ToString())
                    //            {
                    //                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El vehículo de reemplazo no puede ser el mismo el vehículo asignado.', 'warning');", true);
                    //                Session["idReemplazo"] = "0";
                    //                txtVehiculoReemplazo.Text = "";
                    //            }

                    //            else
                    //            {
                    //                actualizarRegistro();
                    //            }
                    //        }
                    //    }
                    //}

                    //else if (Convert.ToDateTime(TxtFechaViaje.Text.Trim()) > Convert.ToDateTime(sFecha))
                    //{
                    //    sFecha = Convert.ToDateTime(TxtFechaViaje.Text.Trim()).ToString("yyyy/MM/dd");

                    //    if (Session["idRegistro"] == null)
                    //    {
                    //        //ENVIO A FUNCION DE INSERCION
                    //        insertarRegistro();
                    //    }

                    //    else
                    //    {
                    //        if (Session["id_Vehiculo"].ToString() == Session["idReemplazo"].ToString())
                    //        {
                    //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El vehículo de reemplazo no puede ser el mismo el vehículo asignado.', 'warning');", true);
                    //            Session["idReemplazo"] = "0";
                    //            txtVehiculoReemplazo.Text = "";
                    //        }

                    //        else
                    //        {
                    //            actualizarRegistro();
                    //        }
                    //    }
                    //}
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnNuevo_Click(object sender, EventArgs e)
        {
            consultarMaximoCodigo();
            numeroViaje();
            cmbAnden.SelectedIndex = 0;
            txtEstadoViaje.Text = "Abierta";

            pnlGrid.Visible = false;
            pnlRegistro.Visible = true;
            pnlVehiculoReemplazo.Visible = false;

            if (Session["id_Asistente"] == null)
            {
                txtAsistente.Text = "";
            }

            else
            {
                txtAsistente.Text = Session["nombre_Asistente"].ToString().ToUpper();
            }

            chkEjecutarCobro.Checked = true;
            TxtFechaViaje.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvDatos.PageIndex = e.NewPageIndex;

                if (txtDate.Text.Trim() == "")
                {
                    llenarGrid(DateTime.Now.ToString("dd/MM/yyyy"));
                }

                else
                {
                    llenarGrid(txtDate.Text.Trim());
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (txtDate.Text.Trim() == "")
            {
                llenarGrid(DateTime.Now.ToString("dd/MM/yyyy"));
            }

            else
            {
                llenarGrid(txtDate.Text.Trim());
            }
        }
        
        protected void btnEditaPrecios_Click(object sender, EventArgs e)
        {
            btnPopUp_ModalPopupExtender.Show();
            llenarGridPrecios(0);
        }

        protected void btnCerrarModal_Click(object sender, EventArgs e)
        {
            btnPopUp_ModalPopupExtender.Hide();
        }

        protected void btnGuardarValor_Click(object sender, EventArgs e)
        {
            actualizarPreciosExtra();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCrearViaje.aspx");
        }

        protected void btnAbrirModalAsistente_Click(object sender, EventArgs e)
        {
            Session["ChoferAsistente"] = "1";
            ModalPopupExtender_AsistentesChofer.Show();
            llenarGridAsistente(0);
        }

        protected void btnAbrirModalChofer_Click(object sender, EventArgs e)
        {
            Session["ChoferAsistente"] = "2";
            ModalPopupExtender_AsistentesChofer.Show();
            llenarGridChofer(0);
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
                    if (Session["ChoferAsistente"].ToString() == "1")
                    {
                        llenarGridAsistente(0);
                    }

                    else if (Session["ChoferAsistente"].ToString() == "2")
                    {
                        llenarGridChofer(0);
                    }
                }

                else
                {
                    if (Session["ChoferAsistente"].ToString() == "1")
                    {
                        llenarGridAsistente(1);
                    }

                    else if (Session["ChoferAsistente"].ToString() == "2")
                    {
                        llenarGridChofer(1);
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnSeleccionAsistenteChofer_Click(object sender, EventArgs e)
        {
            sAccionFiltro = "Seleccion";
        }

        protected void dgvAsistentesChofer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvAsistentesChofer.SelectedIndex;
                columnasGridAsistente(true);

                if (sAccionFiltro == "Seleccion")
                {
                    if (Session["ChoferAsistente"].ToString() == "1")
                    {
                        Session["id_Asistente"] = dgvAsistentesChofer.Rows[a].Cells[1].Text.Trim();
                        txtAsistente.Text = dgvAsistentesChofer.Rows[a].Cells[2].Text.Trim();
                    }

                    else if (Session["ChoferAsistente"].ToString() == "2")
                    {
                        Session["id_Chofer"] = dgvAsistentesChofer.Rows[a].Cells[1].Text.Trim();
                        txtChofer.Text = dgvAsistentesChofer.Rows[a].Cells[2].Text.Trim();
                    }
                    txtFiltrarChoferAsistente.Text = "";
                    ModalPopupExtender_AsistentesChofer.Hide();
                }

                columnasGridAsistente(false);
            }

            catch (Exception ex)
            {
                ModalPopupExtender_AsistentesChofer.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }


        protected void dgvAsistentesChofer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvAsistentesChofer.PageIndex = e.NewPageIndex;

                if (txtFiltrarChoferAsistente.Text.Trim() == "")
                {
                    if (Session["ChoferAsistente"].ToString() == "1")
                    {
                        llenarGridAsistente(0);
                    }

                    else if (Session["ChoferAsistente"].ToString() == "2")
                    {
                        llenarGridChofer(0);
                    }
                }

                else
                {
                    if (Session["ChoferAsistente"].ToString() == "1")
                    {
                        llenarGridAsistente(1);
                    }

                    else if (Session["ChoferAsistente"].ToString() == "2")
                    {
                        llenarGridChofer(1);
                    }
                }
            }

            catch (Exception ex)
            {
                ModalPopupExtender_AsistentesChofer.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }


        //CODIGO DE FILTRO DE VEHICULOS

        protected void btnAbrirModalVehiculo_Click(object sender, EventArgs e)
        {
            Session["banderaVehiculo"] = "1";
            ModalPopupExtender_Vehiculo.Show();
            llenarGridVehiculo(0);
        }

        protected void btnAbrirModalVehiculoReemplazo_Click(object sender, EventArgs e)
        {
            Session["banderaVehiculo"] = "2";
            ModalPopupExtender_Vehiculo.Show();
            llenarGridVehiculo(0);
        }

        protected void btnCerrarModalVehiculos_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Vehiculo.Hide();
            Session["banderaVehiculo"] = null;
        }

        protected void btnFiltarChoferVehiculos_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFiltrarVehiculos.Text.Trim() == "")
                {
                    llenarGridVehiculo(0);
                }

                else
                {
                    llenarGridVehiculo(1);
                }
            }

            catch (Exception ex)
            {
                ModalPopupExtender_AsistentesChofer.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnSeleccionVehiculos_Click(object sender, EventArgs e)
        {
            sAccionFiltro = "Seleccion";
        }

        protected void dgvVehiculos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvVehiculos.SelectedIndex;
                columnasGridVehiculo(true);

                if (sAccionFiltro == "Seleccion")
                {
                    if (Session["banderaVehiculo"].ToString() == "1")
                    {
                        Session["id_Vehiculo"] = dgvVehiculos.Rows[a].Cells[1].Text.Trim();
                        txtVehiculo.Text = dgvVehiculos.Rows[a].Cells[5].Text.Trim();
                        txtFiltrarVehiculos.Text = "";
                        ModalPopupExtender_Vehiculo.Hide();
                    }

                    else if (Session["banderaVehiculo"].ToString() == "2")
                    {
                        Session["idReemplazo"] = dgvVehiculos.Rows[a].Cells[1].Text.Trim();
                        txtVehiculoReemplazo.Text = dgvVehiculos.Rows[a].Cells[5].Text.Trim();
                        txtFiltrarVehiculos.Text = "";
                        ModalPopupExtender_Vehiculo.Hide();
                    }
                }

                Session["banderaVehiculo"] = null;
                columnasGridVehiculo(false);
            }

            catch (Exception ex)
            {
                ModalPopupExtender_Vehiculo.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }
       
        protected void dgvVehiculos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvVehiculos.PageIndex = e.NewPageIndex;

                if (txtFiltrarVehiculos.Text.Trim() == "")
                {
                    llenarGridVehiculo(0);
                }

                else
                {
                    llenarGridVehiculo(1);
                }
            }

            catch (Exception ex)
            {
                ModalPopupExtender_Vehiculo.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }
        //FIN DE CODIGO DE FILTRO DE VEHICULOS


        protected void lnkEditar_Click(object sender, EventArgs e)
        {
            sAccion = "Editar";
        }

        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                sAccion = "Eliminar";
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);

                if (dgvDatos.Rows[a].Cells[9].Text.Trim() == "C")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No se puede editar o eliminar el registro, ya que se encuentra procesado.', 'warning');", true);
                }

                else
                {
                    Session["idRegistro"] = dgvDatos.Rows[a].Cells[1].Text.Trim();
                    Session["ocupados"] = dgvDatos.Rows[a].Cells[23].Text.Trim();

                    if (consultarBusReemplazo(Convert.ToInt32(Session["idRegistro"].ToString())) == false)
                    {
                        Session["idRegistro"] = null;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo consultar el proceso de vehiculo de reemplazo. Favor comuníquese con el administrador.', 'error');", true);
                        goto fin;
                    }

                    if (sAccion == "Editar")
                    {
                        txtNumeroViaje.Text = dgvDatos.Rows[a].Cells[2].Text.Trim();
                        TxtFechaViaje.Text = dgvDatos.Rows[a].Cells[3].Text.Trim();
                        txtVehiculo.Text = dgvDatos.Rows[a].Cells[4].Text.Trim();
                        Session["hora_seleccionada"] = dgvDatos.Rows[a].Cells[7].Text.Trim();
                        Session["id_Chofer"] = dgvDatos.Rows[a].Cells[10].Text.Trim();
                        Session["id_Asistente"] = dgvDatos.Rows[a].Cells[11].Text.Trim();
                        Session["id_Vehiculo"] = dgvDatos.Rows[a].Cells[12].Text.Trim();
                        Session["id_Itinerario"] = dgvDatos.Rows[a].Cells[22].Text.Trim();

                        txtItinerario.Text = "CÓDIGO: " + dgvDatos.Rows[a].Cells[21].Text.Trim() + " - RUTA: " + dgvDatos.Rows[a].Cells[5].Text.Trim() + " - HORA DE SALIDA: " + dgvDatos.Rows[a].Cells[7].Text.Trim() + " - TIPO DE VIAJE: " + dgvDatos.Rows[a].Cells[8].Text.Trim();
                        cmbAnden.SelectedValue = dgvDatos.Rows[a].Cells[14].Text.Trim();
                        txtEstadoViaje.Text = dgvDatos.Rows[a].Cells[17].Text.Trim();
                        txtCodigo.Text = dgvDatos.Rows[a].Cells[18].Text.Trim();
                        txtChofer.Text = dgvDatos.Rows[a].Cells[19].Text.Trim();
                        txtAsistente.Text = dgvDatos.Rows[a].Cells[20].Text.Trim();

                        if (Convert.ToInt32(dgvDatos.Rows[a].Cells[24].Text.Trim()) == 1)
                        {
                            chkEjecutarCobro.Checked = true;
                        }

                        else
                        {
                            chkEjecutarCobro.Checked = false;
                        }

                        pnlGrid.Visible = false;
                        pnlRegistro.Visible = true;
                        //pnlVehiculoReemplazo.Visible = true;
                        TxtFechaViaje.ReadOnly = true;
                    }

                    else if (sAccion == "Eliminar")
                    {
                        if (Convert.ToInt32(Session["ocupados"].ToString()) == 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
                        }

                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No puede eliminar el viaje, ya que existen pasajes vendidos.', 'warning');", true);
                        }
                    }
                }

                columnasGrid(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

        fin: { }
        }

        protected void btnAbrirModalItinerario_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Itinerarios.Show();
            llenarGridItinerarios(0);
        }

        protected void btnCerrarModalItinerario_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Itinerarios.Hide();
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

        protected void dgvItinerarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvItinerarios.SelectedIndex;
                columnasGridItinerario(true);
                Session["id_Itinerario"] = dgvItinerarios.Rows[a].Cells[0].Text.Trim();

                if (sAccionFiltroItinerario == "Seleccion")
                {
                    txtItinerario.Text = "CÓDIGO: " + dgvItinerarios.Rows[a].Cells[3].Text.Trim() + " - RUTA: " + dgvItinerarios.Rows[a].Cells[4].Text.Trim() + " - HORA DE SALIDA: " + dgvItinerarios.Rows[a].Cells[6].Text.Trim();
                    txtFiltrarItinerarios.Text = "";
                    //Session["id_tipo_viaje"] = dgvItinerarios.Rows[a].Cells[3].Text.Trim();
                    Session["hora_seleccionada"] = dgvItinerarios.Rows[a].Cells[6].Text.Trim();
                    ModalPopupExtender_Itinerarios.Hide();
                }

                columnasGridItinerario(false);
            }

            catch (Exception ex)
            {
                ModalPopupExtender_AsistentesChofer.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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

        protected void lbtnSeleccionItinerario_Click(object sender, EventArgs e)
        {
            sAccionFiltroItinerario = "Seleccion";
        }

        protected void btnRemoverVehiculoReemplazo_Click(object sender, EventArgs e)
        {
            Session["idReemplazo"] = null;
            txtVehiculoReemplazo.Text = "";
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            eliminarRegistro();
        }

        protected void lbtnVentas_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmTransacciones.aspx");
        }
    }
}