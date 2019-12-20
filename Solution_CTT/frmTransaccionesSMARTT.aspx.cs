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
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Drawing.Printing;
using Microsoft.Reporting.WebForms;

namespace Solution_CTT
{
    public partial class frmTransaccionesSMARTT : System.Web.UI.Page
    {
        ENTComboDatos comboE = new ENTComboDatos();
        ENTViajes asignarE = new ENTViajes();
        ENTPasajeros pasajeroE = new ENTPasajeros();
        ENTPagosPendientes pendienteE = new ENTPagosPendientes();
        ENTVendidos vendidoE = new ENTVendidos();
        ENTFacturasItinerario facturaE = new ENTFacturasItinerario();

        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorConexion conexionM = new manejadorConexion();
        manejadorPasajeros pasajeroM = new manejadorPasajeros();
        manejadorAsignarViaje asignarM = new manejadorAsignarViaje();
        manejadorPagosPendientes pendienteM = new manejadorPagosPendientes();
        manejadorFacturasItinerario facturaM = new manejadorFacturasItinerario();

        Clases.ClaseParametros parametros = new Clases.ClaseParametros();
        Clases.ClaseReporteBoletoResumido reporteElectronico = new Clases.ClaseReporteBoletoResumido();
        Clases.ClaseReporteBoletoNormal reporteNormal = new Clases.ClaseReporteBoletoNormal();
        Clases.ClaseCierreItinerario cerrarViajeReporte = new Clases.ClaseCierreItinerario();
        Clases.ClaseInstruccionesCierre cierreViajeInstrucciones = new Clases.ClaseInstruccionesCierre();
        Clases.ClaseImpresion imprimir = new Clases.ClaseImpresion();
        Clases.ClaseValidarRUC ruc = new Clases.ClaseValidarRUC();
        Clases.ValidarCedula cedula = new Clases.ValidarCedula();
        Clases.ImpresionReporte impresiones = new Clases.ImpresionReporte();

        Clases_Contifico.ClaseCrearVenta ventaBoleto;
        Clase_Variables_Contifico.TasaUsuarioSmartt tasaUsuario;

        Button botonSeleccionado;

        string sSql;
        string sAccion;
        string sAccionPersonas;
        string sAccionPagos;
        string sFecha;
        string sTabla;
        string sCampo;
        string sCiudad;
        string sDireccion;
        string sTelefono;
        string sCorreoElectronico;
        string sClienteFactura;
        string sIdentificacionFactura;
        string sIdentificacionPasajero;
        string sNombrePasajero;
        string sDescripcionMes;
        string sIdentificacionParaTasa;
        string sJson;
        
        string sRespuesta_A;

        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;
        DataTable dtAlmacenar;
        DataTable dtAsientos;
        DataTable dtOcupados;
        DataTable dtTipos;
        DataTable dtDatosTasaUsuario;
        DataTable dtTarifasSmartt;

        bool bRespuesta;

        double dbPrecio;
        double dbDescuento;
        double dbTotal;
        double dbPagoAdministracion;
        double dbValorRetencion;

        int indice;
        int meses;
        int iIdPersona;
        int iIdPedido;
        int iNumeroPedido;
        int iIdCabDespachos;
        int iIdDespachoPedido;
        int iIdEventoCobro;
        int iIdProducto;
        int iIdAsiento;
        int iIdPersonaAsiento;
        int iIdDetalleRuta;
        int iIdTipoCliente;
        int iIdPago;
        int iNumeroPago;
        int iCgTipoDocumento = 7456;
        int iIdDocumentoPago;
        int iIdDocumentoCobrar;
        int iIdFactura;
        int iIdFacturaPedido;
        int iNumeroFactura;
        int iLongitudCedula;
        int iBanderaTipoCliente;
        int iDiscapacidad;
        int iCerrarModal;
        int iIdListaPrecio;
        int iIndiceDestino;
        int iNumeroMovimientoCaja;
        int iIdMovimientoCaja;
        int iManejaFacturacionElectronica;
        int iIdPuebloOrigen;
        int iIdPuebloDestino;
        int iIdFormaPagoFactura;
        int iIdDetPedido;

        int iPorcentajeNotificacionEntero;

        int iCgEstadoDctoPorCobrar = 7461;

        int iBandera;

        double dbPrecioUnitario;
        double dbCantidad;
        double dbIva;
        double dbServicio;

        long iMaximo;

        //VARIABLES DEL REPORTE
        int iVendidos_REP;

        decimal dbCantidad_REP;
        decimal dbPrecioUnitario_REP;
        decimal dbDescuento_REP;
        decimal dbIva_REP;
        decimal dbSumaTotal_REP;

        string sNumeroFactura_REP;
        string sAsientos_REP;

        int iFormaPago_A = 1;
        int iExtranjero_A;

        Byte Logo { get; set; }

        //VARIABLES PARA EMITIR LA TASA DE USUARIO
        int iNumeroViaje_A;
        string sNombreParada_A;

        int iNivel_API = 1;
        int iNumeroAsiento_API;
        int iTipoTarifa_API;
        int iExtranjero_API;

        string sNumeroAsiento_API;
        string sIdentificacionPasajero_API;
        string sNombrePasajero_API;
        string sCorreoPasajero_API;

        Decimal dbValorAsiento_API;
        Decimal dbValorDescuento_API;

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

            Session["modulo"] = "MÓDULO DE VENTA DE PASAJES";

            txtIdentificacion.Attributes.Add("onKeyPress", "doClick('" + btnBuscarCliente.ClientID + "',event)");

            if (!IsPostBack)
            {
                sFecha = DateTime.Now.ToString("dd/MM/yyyy");
                txtDate.Text = sFecha;
                llenarComboOrigen();
                llenarComboOrigenItinerario();
                datosListas();
                llenarComboTipoIdentificacion();
                llenarComboTipoIdentificacionRegistro();
                llenarComboTipoCliente();
                llenarGrid(sFecha);
                llenarGridExtras(sFecha);
                Session["auxiliar"] = "1";
                Session["idVehiculo"] = null;
                Session["idProgramacion"] = null;
                //Session["iIdViajeVentaSMARTT"] = null;
            }

            else
            {
                if (Session["auxiliar"].ToString() == "1")
                {
                    mostrarBotones();
                }
            }
        }
        
        #region FUNCIONES PARA REIMPRIMIR LA FACTURA

        //FUNCION PARA MAIPULACION DE COLUMNAS EN GRID DE PASAJEROS
        private void columnasGridVendidos(bool ok)
        {
            dgvVendidos.Columns[0].Visible = ok;
            dgvVendidos.Columns[1].Visible = ok;
            dgvVendidos.Columns[2].Visible = ok;
            dgvVendidos.Columns[3].Visible = ok;

            dgvVendidos.Columns[4].ItemStyle.Width = 150;
            dgvVendidos.Columns[5].ItemStyle.Width = 300;
            dgvVendidos.Columns[6].ItemStyle.Width = 150;
            dgvVendidos.Columns[7].ItemStyle.Width = 150;
            dgvVendidos.Columns[8].ItemStyle.Width = 100;

            dgvVendidos.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvVendidos.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvVendidos.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        }
        private void llenarGridVendidos(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_facturas_itinerario" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + Convert.ToInt32(Session["idProgramacion"].ToString()) + Environment.NewLine;
                sSql += "order by numero_factura";

                columnasGridVendidos(true);
                facturaE.ISQL = sSql;
                dgvVendidos.DataSource = facturaM.listarFacturasItinerario(facturaE);
                dgvVendidos.DataBind();
                columnasGridVendidos(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //CONSULTAR LOS REGISTROS
        private void consultarFacturasEmitidas()
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_facturas_itinerario" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + Convert.ToInt32(Session["idProgramacion"].ToString()) + Environment.NewLine;
                sSql += "order by numero_factura";

                Session["instruccionSQL"] = sSql;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se han emitido facturas en el viaje.', 'error');", true);
                    }

                    else
                    {
                        ModalPopupExtender_ReimprimirFacturas.Show();
                        llenarGridVendidos(0);
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

        #endregion

        #region FUNCIONES DEL MODAL DE FILTRO DE CLIENTES

        //FUNCION PARA LAS COLUMNAS DEL GRID DE CLIENTES
        private void columnasGridFiltro(bool ok)
        {
            dgvFiltrarClientes.Columns[0].Visible = ok;
            dgvFiltrarClientes.Columns[1].ItemStyle.Width = 150;
            dgvFiltrarClientes.Columns[2].ItemStyle.Width = 300;
            dgvFiltrarClientes.Columns[3].ItemStyle.Width = 150;
            dgvFiltrarClientes.Columns[4].ItemStyle.Width = 100;
        }

        //FUNCION PARA LAS COLUMNAS DEL GRID DE CLIENTES DE FACTURA
        private void columnasGridFacturar(bool ok)
        {
            dgvGridFacturar.Columns[0].Visible = ok;
            dgvGridFacturar.Columns[1].ItemStyle.Width = 150;
            dgvGridFacturar.Columns[2].ItemStyle.Width = 300;
            dgvGridFacturar.Columns[3].ItemStyle.Width = 150;
            dgvGridFacturar.Columns[4].ItemStyle.Width = 100;
        }

        //FUNCION PARA LLENAR EL GRID DE CLIENTES
        private void llenarGridClientes(int iOp, int iConstruir)
        {
            try
            {
                sSql = "";
                sSql += "select id_persona, identificacion," + Environment.NewLine;
                sSql += "ltrim(isnull(nombres, '') + ' ' + apellidos) personas," + Environment.NewLine;
                sSql += "isnull(fecha_nacimiento, GETDATE()) fecha_nacimiento" + Environment.NewLine;
                sSql += "from tp_personas" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    if (iConstruir == 0)
                    {
                        sSql += "and identificacion like '%" + txtFiltrarCliente.Text.Trim() + "%'" + Environment.NewLine;
                        sSql += "or nombres like '%" + txtFiltrarCliente.Text.Trim() + "%'" + Environment.NewLine;
                        sSql += "or apellidos like '%" + txtFiltrarCliente.Text.Trim() + "%'" + Environment.NewLine;
                    }

                    else if (iConstruir == 1)
                    {
                        sSql += "and identificacion like '%" + txtBuscarClienteFactura.Text.Trim() + "%'" + Environment.NewLine;
                        sSql += "or nombres like '%" + txtBuscarClienteFactura.Text.Trim() + "%'" + Environment.NewLine;
                        sSql += "or apellidos like '%" + txtBuscarClienteFactura.Text.Trim() + "%'" + Environment.NewLine;
                    }
                }

                sSql += "order by id_persona";

                if (iConstruir == 0)
                {
                    columnasGridFiltro(true);
                    pasajeroE.ISQL = sSql;
                    dgvFiltrarClientes.DataSource = pasajeroM.listarPasajeros(pasajeroE);
                    dgvFiltrarClientes.DataBind();
                    columnasGridFiltro(false);
                }

                else if (iConstruir == 1)
                {
                    columnasGridFacturar(true);
                    pasajeroE.ISQL = sSql;
                    dgvGridFacturar.DataSource = pasajeroM.listarPasajeros(pasajeroE);
                    dgvGridFacturar.DataBind();
                    columnasGridFacturar(false);
                }
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCIONES PARA LA IMPRESION

        //FUNCION PARA IMPRIMIR DIRECTAMENTE EL REPORT VIEWER
        private void crearReporteImprimir()
        {
            try
            {
                sSql = "";
                sSql += "select establecimiento, punto_emision, numero_factura, fecha_ingreso," + Environment.NewLine;
                sSql += "identificacion, ltrim(isnull(nombres, '') + ' ' + apellidos) cliente, descripcion_ruta," + Environment.NewLine;
                sSql += "tipo_viaje, fecha_viaje, hora_salida, descripcion_anden, descripcion_disco," + Environment.NewLine;
                sSql += "isnull(tasa_usuario, '') tasa_usuario, cantidad, precio_unitario, valor_dscto, valor_iva, clave_acceso," + Environment.NewLine;
                sSql += "oficinista, numero_asiento, '' as valor_total, '' as vendidos, '' as asientos, '' as secuencia_factura," + Environment.NewLine;
                sSql += "destino, cantidad_tasa_emitida" + Environment.NewLine;
                sSql += "from ctt_vw_factura" + Environment.NewLine;
                sSql += "where id_factura = " + iIdFactura + Environment.NewLine;
                sSql += "order by numero_asiento";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sNumeroFactura_REP = dtConsulta.Rows[0][0].ToString() + "-" + dtConsulta.Rows[0][1].ToString() + "-" + dtConsulta.Rows[0][2].ToString().PadLeft(9, '0');
                        iVendidos_REP = Convert.ToInt32(dtConsulta.Rows[0]["cantidad_tasa_emitida"].ToString());

                        sAsientos_REP = "";
                        dbSumaTotal_REP = 0;

                        //RECORRER LOS ASIENTOS Y SUMAR TOTAL
                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            sAsientos_REP += dtConsulta.Rows[i]["numero_asiento"].ToString().Trim();

                            if (i + 1 != dtConsulta.Rows.Count)
                            {
                                sAsientos_REP += " - ";
                            }

                            dbCantidad_REP = Convert.ToDecimal(dtConsulta.Rows[i]["cantidad"].ToString());
                            dbPrecioUnitario_REP = Convert.ToDecimal(dtConsulta.Rows[i]["precio_unitario"].ToString());
                            dbDescuento_REP = Convert.ToDecimal(dtConsulta.Rows[i]["valor_dscto"].ToString());
                            dbIva_REP = Convert.ToDecimal(dtConsulta.Rows[i]["valor_iva"].ToString());

                            dbSumaTotal_REP += dbCantidad_REP * (dbPrecioUnitario_REP - dbDescuento_REP + dbIva_REP);
                        }

                        //RECORRER EL DATATABLE PARA LLENAR DE DATOS
                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            dtConsulta.Rows[i]["valor_total"] = dbSumaTotal_REP.ToString("N2");
                            dtConsulta.Rows[i]["vendidos"] = iVendidos_REP.ToString();
                            dtConsulta.Rows[i]["asientos"] = sAsientos_REP.Trim();
                            dtConsulta.Rows[i]["secuencia_factura"] = sNumeroFactura_REP;
                        }

                        DSReportes ds = new DSReportes();

                        DataTable dt = ds.Tables["dtFacturaSimple"];
                        dt.Clear();

                        dt = dtConsulta;

                        //AGREGAR EL DETALLE DE BOLETOS VENDIDOS
                        sSql = "";
                        sSql += "select tipo_cliente, count(*) cuenta" + Environment.NewLine;
                        sSql += "from ctt_vw_factura" + Environment.NewLine;
                        sSql += "where id_factura = " + iIdFactura + Environment.NewLine;
                        sSql += "group by tipo_cliente";

                        DataTable dt2 = ds.Tables["dtTarifas"];
                        dt2.Clear();

                        bRespuesta = conexionM.consultarRegistro(sSql, dt2);

                        if (bRespuesta == true)
                        {
                            LocalReport reporteLocal = new LocalReport();

                            reporteLocal.ReportPath = Server.MapPath("~/Reportes/rptFacturaGuayaquil.rdlc");

                            ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
                            ReportDataSource datasource2 = new ReportDataSource("DataSet2", dt2);
                            reporteLocal.DataSources.Add(datasource);
                            reporteLocal.DataSources.Add(datasource2);


                            Clases.Impresor imp = new Clases.Impresor();
                            imp.Imprime(reporteLocal);
                        }
                    }
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo crear el reporte. Comuníquese con el administrador.', 'danger');", true);
                }
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCIONES PARA CREAR EL ARREGLO DE BOTONES

        //FUNCION PARA LLENAR EL DATATABLE Y DISEÑAR LA INTERFAZ DEL BUS
        private bool extraerInformacion()
        {
            try
            {
                //LLENAR DATATABLE DE ASIENTOS DEL BUS
                sSql = "";
                sSql += "select * from ctt_vw_ubicacion_asientos" + Environment.NewLine;
                sSql += "where id_ctt_vehiculo = " + Convert.ToInt32(Session["idVehiculo"]) + Environment.NewLine;
                sSql += "order by numero_asiento" + Environment.NewLine;

                dtAsientos = new DataTable();
                dtAsientos.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtAsientos);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }


                //LLENAR DATATABLE DE ASIENTOS OCUPADOS
                sSql = "";
                sSql += "select * from ctt_vw_asientos_ocupados" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + Convert.ToInt32(Session["idProgramacion"]) + Environment.NewLine;
                sSql += "order by id_ctt_asiento" + Environment.NewLine;

                dtOcupados = new DataTable();
                dtOcupados.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtOcupados);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA DIBUJAR LOS BOTONES
        public void mostrarBotones()
        {
            try
            {
                if (extraerInformacion() == false)
                {
                    goto reversa;
                }


                int icuenta = dtAsientos.Rows.Count;
                int a = 1;

                pnlAsientos.Controls.Clear();
                //AQUI LLENAMOS EL PANEL
                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        //ImageButton boton = new ImageButton();
                        Button boton = new Button();


                        DataRow[] dFila = dtAsientos.Select("posicion_x = " + i + " and posicion_y = " + j);

                        if (dFila.Length != 0)
                        {
                            boton.ForeColor = Color.Black;
                            boton.Click += new EventHandler(boton_clic_asiento);
                            boton.Attributes.Add("Class", "btn bg-darken-3 btn-default btn-sm");
                            boton.Width = 30;
                            boton.Height = 30;
                            boton.ID = "btnAsiento" + a.ToString();
                            boton.Text = dFila[0][1].ToString();
                            boton.CommandArgument = dFila[0][0].ToString();

                            DataRow[] dOcupado = dtOcupados.Select("id_ctt_asiento = " + Convert.ToInt32(dFila[0][0].ToString()));

                            if (dOcupado.Length != 0)
                            {
                                if (dOcupado[0][8].ToString().Trim() != Application["idLocalidad"].ToString())
                                {
                                    boton.Attributes.Add("Class", "btn bg-black btn-default btn-sm");
                                }

                                else if (dOcupado[0][6].ToString().Trim() == "1")
                                {
                                    //boton.Attributes.Add("Class", "btn bg-lime btn-default btn-sm");
                                    boton.Attributes.Add("Class", "btn bg-lime btn-primary btn-sm");
                                }

                                else if (dOcupado[0][6].ToString().Trim() == "2")
                                {
                                    boton.Attributes.Add("Class", "btn bg-red btn-default btn-sm");
                                }

                                else if (dOcupado[0][6].ToString().Trim() == "3")
                                {
                                    boton.Attributes.Add("Class", "btn bg-fuchsia btn-default btn-sm");
                                }

                                else if (dOcupado[0][6].ToString().Trim() == "4")
                                {
                                    boton.Attributes.Add("Class", "btn bg-blue btn-default btn-sm");
                                }

                                else
                                {
                                    boton.Attributes.Add("Class", "btn bg-maroon btn-default btn-sm");
                                }


                                if (dOcupado[0][4].ToString().Trim() == "")
                                {
                                    boton.ToolTip = "PASAJERO: " + Environment.NewLine + "NOMBRE: " + dOcupado[0][3].ToString().Trim().ToUpper() +
                                                    Environment.NewLine + "C.I. / RUC: " + dOcupado[0][2].ToString().Trim().ToUpper() +
                                                    Environment.NewLine + "TIPO CLIENTE: " + dOcupado[0][7].ToString().Trim().ToUpper() +
                                                    Environment.NewLine + "OFICINA VENTA: " + dOcupado[0][9].ToString().Trim().ToUpper();
                                }

                                else
                                {
                                    boton.ToolTip = "PASAJERO: " + Environment.NewLine + "NOMBRE: " + dOcupado[0][4].ToString().Trim().ToUpper() +
                                                    Environment.NewLine + "OFICINA VENTA: " + dOcupado[0][9].ToString().Trim().ToUpper();
                                }
                            }

                            else
                            {
                                boton.ToolTip = "ASIENTO DISPONIBLE";
                            }
                        }

                        else
                        {
                            boton.ForeColor = Color.Black;
                            boton.BackColor = Color.White;
                            boton.Width = 40;
                            boton.Height = 40;
                            boton.ID = "btnAsiento" + a.ToString();
                            boton.Text = " ";
                            boton.BorderStyle = BorderStyle.None;
                        }

                        pnlAsientos.Controls.Add(boton);
                        a++;
                    }
                    pnlAsientos.Controls.Add(new LiteralControl("<br />"));
                }

                goto fin;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

        reversa:
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>Error al consultar los asientos del vehículo.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

        fin: { }
        }

        //EVENTO CLIC DEL BOTON DE ASIENTO
        void boton_clic_asiento(object sender, EventArgs e)
        {
            try
            {
                botonSeleccionado = sender as Button;

                if (botonSeleccionado.ToolTip == "ASIENTO DISPONIBLE")
                {
                    listarAsientos(botonSeleccionado);
                }

                else if (botonSeleccionado.ToolTip == "ASIENTO EN PROCESO")
                {
                    removerAsiento(botonSeleccionado);
                }

                else if (botonSeleccionado.ToolTip != "ASIENTO DISPONIBLE")
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " alert('Asiento " + botonSeleccionado.Text + " ocupado.')", true);
                    lblMensajeError.Text = "<b>Asiento " + botonSeleccionado.Text + " ocupado.</b><br/><br/>";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA EMPEZAR A CREAR UN DATATABLE CON LOS ASIENTOS SELECCIONADOS
        private void listarAsientos(Button botonProcesar)
        {
            try
            {
                if (Convert.ToInt32(cmbDestino.SelectedValue) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No ha seleccionado el destino.', 'warning');", true);
                    goto fin;
                }

                else if (Convert.ToInt32(cmbTipoCliente.SelectedValue) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No ha seleccionado el tipo de cliente.', 'warning');", true);
                    goto fin;
                }

                if (Session["dtClientes"] != null)
                {
                    dtAlmacenar = new DataTable();
                    dtAlmacenar = Session["dtClientes"] as DataTable;
                }

                else
                {
                    crearDataTable();
                }

                sIdentificacionPasajero = txtIdentificacion.Text.Trim();

                if (Session["idPasajero"] == null)
                {
                    if (txtNombrePasajero.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese la identificación o un nombre para la lista de pasajeros.', 'success');", true);
                        return;
                    }

                    if (cmbTipoCliente.SelectedItem.ToString().Trim().ToUpper() == "MENOR DE EDAD")
                    {
                        iIdPersona = Convert.ToInt32(Application["idPersonaMenorEdad"].ToString());
                        Session["idPasajero"] = iIdPersona.ToString();
                        sIdentificacionPasajero = Application["numero_id_menor_edad"].ToString();
                    }

                    else
                    {
                        iIdPersona = Convert.ToInt32(Application["idPersonaSinDatos"].ToString());
                        Session["idPasajero"] = iIdPersona.ToString();
                        sIdentificacionPasajero = Application["numero_id_sin_datos"].ToString();
                    }
                }

                DataRow row = dtAlmacenar.NewRow();
                row["IDASIENTO"] = botonProcesar.CommandArgument;
                row["NUMEROASIENTO"] = botonProcesar.Text;
                row["PRECIO"] = txtPrecio.Text.Trim();
                row["DESCUENTO"] = txtDescuento.Text.Trim();
                row["IDPRODUCTO"] = Session["idProducto"].ToString();
                row["IDDETALLERUTA"] = Session["idPueblo"].ToString();
                row["IDTIPOCLIENTE"] = cmbTipoCliente.SelectedValue;
                row["IDPERSONAASIENTO"] = Session["idPasajero"].ToString();
                row["NOMBREPASAJERO"] = txtNombrePasajero.Text.Trim().ToUpper();
                row["IDENTIFICACION"] = sIdentificacionPasajero;

                if (Convert.ToInt32(Session["cgTipoIdentificacion"].ToString()) == 180)
                {
                    row["EXTRANJERO"] = "1";
                }

                else
                {
                    row["EXTRANJERO"] = "0";
                }

                row["DESTINO"] = Session["str_Producto"].ToString().Trim().ToUpper();

                dtAlmacenar.Rows.Add(row);

                Session["dtClientes"] = dtAlmacenar;

                botonProcesar.Attributes.Add("class", "btn bg-orange btn-default btn-sm");
                botonProcesar.ToolTip = "ASIENTO EN PROCESO";

                txtIdentificacion.Attributes.Add("onKeyPress", "doClick('" + btnBuscarCliente.ClientID + "',event)");
                txtIdentificacion.Focus();

                return;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

        fin: { }
        }

        //FUNCION PARA ELIMINAR UN ASIENTO
        private void removerAsiento(Button botonProcesar)
        {
            try
            {
                dtAlmacenar = new DataTable();
                dtAlmacenar = Session["dtClientes"] as DataTable;

                for (int i = dtAlmacenar.Rows.Count - 1; i >= 0; i--)
                {
                    if (Convert.ToInt32(dtAlmacenar.Rows[i][1].ToString()) == Convert.ToInt32(botonProcesar.CommandArgument))
                    {
                        dtAlmacenar.Rows.RemoveAt(i);
                    }
                }

                Session["dtClientes"] = dtAlmacenar;
                botonSeleccionado.Attributes.Add("Class", "btn bg-darken-3 btn-default btn-sm");
                botonSeleccionado.ToolTip = "ASIENTO DISPONIBLE";
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA VERIFICAR EL ASIENTO
        private int verificarAsiento(int iIdAsiento_P)
        {
            try
            {
                int iValor = 0;

                sSql = "";
                sSql += "select count(DP.id_ctt_asiento) cuenta" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP INNER JOIN" + Environment.NewLine;
                sSql += "ctt_programacion P ON CP.id_ctt_programacion = P.id_ctt_programacion" + Environment.NewLine;
                sSql += "and CP.estado = 'A'" + Environment.NewLine;
                sSql += "and P.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "cv403_det_pedidos DP ON DP.id_pedido = CP.id_pedido" + Environment.NewLine;
                sSql += "and DP.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_asientos ASI ON DP.id_ctt_asiento = ASI.id_ctt_asiento" + Environment.NewLine;
                sSql += "and ASI.estado = 'A'" + Environment.NewLine;
                sSql += "where DP.id_ctt_asiento = " + iIdAsiento_P + Environment.NewLine;
                sSql += "and P.id_ctt_programacion = " + Convert.ToInt32(Session["idProgramacion"]);

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return -1;
                }

                else
                {
                    iValor = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                }

                return iValor;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return -1;
            }
        }

        #endregion

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA EXTRAER LOS ASIENTOS OCUPADOS
        private void asientosOcupados()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(asientos_ocupados, 0) asientos_ocupados" + Environment.NewLine;
                sSql += "from ctt_programacion" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + Convert.ToInt32(Session["idProgramacion"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    lblCantidadVendida.Text = dtConsulta.Rows[0][0].ToString();
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA SABER SI HAY CONEXION A INTERNET
        private bool conexionInternet()
        {
            try
            {
                IPHostEntry host = Dns.GetHostEntry("www.google.com");
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        //FUNCION PARA CERRAR LOS MODAL
        private void cerrarModal()
        {
            ModalPopupExtender_ListaPasajeros.Hide();
            ModalPopupExtender_Factura.Hide();
            ModalPopupExtenderCrearEditar.Hide();
            btnPopUp_ModalPopupExtender.Hide();
            ModalPopupExtender_NuevaHora.Hide();
            ModalPopUpSeleccionViajes.Hide();
        }

        //FUNCION PARA OBTENER LOS DATOS DE LA LISTA BASE Y MINORISTA
        private void datosListas()
        {
            try
            {
                sSql = "";
                sSql += "select id_lista_precio" + Environment.NewLine;
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
                        Session["lista_minorista"] = dtConsulta.Rows[0]["id_lista_precio"].ToString();
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
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONSULTAR EL TIPO DE SERVICIO
        private int consultarTipoServicio(int idProgramacion_P)
        {
            try
            {
                sSql = "";
                sSql += "select TS.edita_precio" + Environment.NewLine;
                sSql += "from ctt_programacion P, ctt_tipo_servicio TS" + Environment.NewLine;
                sSql += "where P.id_ctt_tipo_servicio = TS.id_ctt_tipo_servicio" + Environment.NewLine;
                sSql += "and P.id_ctt_programacion = " + idProgramacion_P + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and TS.estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        return Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                    }

                    else
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        return -1;
                    }
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
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return -1;
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE ORIGEN
        private void llenarComboOrigen()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_pueblo, descripcion" + Environment.NewLine;
                sSql += "from ctt_pueblos" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and terminal = 1";

                comboE.ISSQL = sSql;
                cmbOrigen.DataSource = comboM.listarCombo(comboE);
                cmbOrigen.DataValueField = "IID";
                cmbOrigen.DataTextField = "IDATO";
                cmbOrigen.DataBind();
                //cmbOrigen.SelectedValue = Session["id_Pueblo"].ToString();
                cmbOrigen.SelectedValue = Session["id_pueblo"].ToString();

            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE ORIGEN Y FILTRAR EL GRID
        private void llenarComboOrigenItinerario()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_pueblo, descripcion" + Environment.NewLine;
                sSql += "from ctt_pueblos" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and terminal = 1";

                comboE.ISSQL = sSql;
                cmbFiltrarGrid.DataSource = comboM.listarCombo(comboE);
                cmbFiltrarGrid.DataValueField = "IID";
                cmbFiltrarGrid.DataTextField = "IDATO";
                cmbFiltrarGrid.DataBind();
                //cmbTipoIdentificacion.Items.Insert(0, new ListItem("Seleccione ..!!", "0"));

                if (cmbFiltrarGrid.Items.Count > 0)
                {
                    cmbFiltrarGrid.SelectedValue = Session["id_pueblo"].ToString();
                }
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE TIPO DE IDENTIFICACION
        private void llenarComboTipoIdentificacion()
        {
            try
            {
                sSql = "";
                sSql += "select correlativo, valor_texto" + Environment.NewLine;
                sSql += "from tp_vw_tipoidentificacion";

                comboE.ISSQL = sSql;
                cmbTipoIdentificacion.DataSource = comboM.listarCombo(comboE);
                cmbTipoIdentificacion.DataValueField = "IID";
                cmbTipoIdentificacion.DataTextField = "IDATO";
                cmbTipoIdentificacion.DataBind();
                cmbTipoIdentificacion.Items.Insert(0, new ListItem("Seleccione ..!!", "0"));
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE TIPO DE IDENTIFICACION
        private void llenarComboTipoCliente()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tipo_cliente, descripcion" + Environment.NewLine;
                sSql += "from ctt_tipo_cliente" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "order by id_ctt_tipo_cliente";

                comboE.ISSQL = sSql;
                cmbTipoCliente.DataSource = comboM.listarCombo(comboE);
                cmbTipoCliente.DataValueField = "IID";
                cmbTipoCliente.DataTextField = "IDATO";
                cmbTipoCliente.DataBind();
                //cmbTipoCliente.Items.Insert(0, new ListItem("Seleccione ..!!", "0"));
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE TIPO DE IDENTIFICACION EN REGISTRO
        private void llenarComboTipoIdentificacionRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select correlativo, valor_texto" + Environment.NewLine;
                sSql += "from tp_vw_tipoidentificacion";

                comboE.ISSQL = sSql;
                cmbIdentificacion.DataSource = comboM.listarCombo(comboE);
                cmbIdentificacion.DataValueField = "IID";
                cmbIdentificacion.DataTextField = "IDATO";
                cmbIdentificacion.DataBind();
                //cmbIdentificacion.Items.Insert(0, new ListItem("Seleccione..!!", "0"));
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE DESTINOS
        private bool llenarComboDestinos(int iIdListaPrecio_P)
        {
            try
            {
                sSql = "";
                sSql += "select P.id_producto, PU.descripcion + ' - $' + ltrim(str(PP.valor, 10, 2)) destino" + Environment.NewLine;
                sSql += "from ctt_pueblos PU, cv401_productos P," + Environment.NewLine;
                sSql += "cv403_precios_productos PP" + Environment.NewLine;
                sSql += "where P.id_ctt_pueblo_destino = PU.id_ctt_pueblo" + Environment.NewLine;
                sSql += "and PP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and id_producto_padre in" + Environment.NewLine;
                sSql += "(select id_producto from cv401_productos" + Environment.NewLine;
                sSql += "where id_ctt_pueblo_origen = " + Convert.ToInt32(Session["idPuebloOrigen"].ToString()) + ")" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and PU.estado = 'A'" + Environment.NewLine;
                sSql += "and PP.estado = 'A'" + Environment.NewLine;
                sSql += "and PP.id_lista_precio = " + iIdListaPrecio_P + Environment.NewLine;
                sSql += "and P.aplica_extra = 0" + Environment.NewLine;
                sSql += "order by PU.descripcion";

                comboE.ISSQL = sSql;
                cmbDestino.DataSource = comboM.listarCombo(comboE);
                cmbDestino.DataValueField = "IID";
                cmbDestino.DataTextField = "IDATO";
                cmbDestino.DataBind();
                cmbDestino.Items.Insert(0, new ListItem("Seleccione..!!", "0"));
                return true;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA LLENAR COMBOBX DE EXTRAS
        private bool llenarComboDestinosExtras(int iIdListaPrecio_P)
        {
            try
            {
                sSql = "";
                sSql += "select P.id_producto, PU.descripcion + ' - $' + ltrim(str(PP.valor, 10, 2)) destino" + Environment.NewLine;
                sSql += "from ctt_pueblos PU, cv401_productos P," + Environment.NewLine;
                sSql += "cv403_precios_productos PP" + Environment.NewLine;
                sSql += "where P.id_ctt_pueblo_destino = PU.id_ctt_pueblo" + Environment.NewLine;
                sSql += "and PP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and id_producto_padre in" + Environment.NewLine;
                sSql += "(select id_producto from cv401_productos" + Environment.NewLine;
                sSql += "where id_ctt_pueblo_origen = " + Convert.ToInt32(Session["idPuebloOrigen"].ToString()) + ")" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and PU.estado = 'A'" + Environment.NewLine;
                sSql += "and PP.estado = 'A'" + Environment.NewLine;
                sSql += "and PP.id_lista_precio = " + iIdListaPrecio_P + Environment.NewLine;
                sSql += "and P.aplica_extra = 1" + Environment.NewLine;
                sSql += "order by PU.descripcion";

                comboE.ISSQL = sSql;
                cmbDestino.DataSource = comboM.listarCombo(comboE);
                cmbDestino.DataValueField = "IID";
                cmbDestino.DataTextField = "IDATO";
                cmbDestino.DataBind();
                cmbDestino.Items.Insert(0, new ListItem("Seleccione..!!", "0"));
                return true;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA BUSCAR EL ID DE LA LISTA DE PRECIO SEGUN EL TIPO DE CLIENTE
        private bool consultarListaPrecioTipoCliente(int iIdTipoCliente_P)
        {
            try
            {
                sSql = "";
                sSql += "select id_lista_precio" + Environment.NewLine;
                sSql += "from ctt_tipo_cliente" + Environment.NewLine;
                sSql += "where id_ctt_tipo_cliente = " + iIdTipoCliente_P + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count == 0)
                    {
                        iIdListaPrecio = Convert.ToInt32(Session["lista_minorista"].ToString());
                    }

                    else
                    {
                        iIdListaPrecio = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                    }

                    if (Convert.ToInt32(Session["extra"].ToString()) == 0)
                    {

                        if (llenarComboDestinos(iIdListaPrecio) == false)
                        {
                            return false;
                        }

                        else
                        {
                            return true;
                        }
                    }

                    else //if (Convert.ToInt32(Session["extra"].ToString()) == 1)
                    {

                        if (llenarComboDestinosExtras(iIdListaPrecio) == false)
                        {
                            return false;
                        }

                        else
                        {
                            return true;
                        }
                    }
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
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA MANIPULACION DE COLUMNAS
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[0].ItemStyle.Width = 75;
            dgvDatos.Columns[2].ItemStyle.Width = 75;
            dgvDatos.Columns[3].ItemStyle.Width = 130;
            dgvDatos.Columns[4].ItemStyle.Width = 150;
            dgvDatos.Columns[5].ItemStyle.Width = 250;
            dgvDatos.Columns[6].ItemStyle.Width = 100;
            dgvDatos.Columns[7].ItemStyle.Width = 100;
            dgvDatos.Columns[8].ItemStyle.Width = 100;
            dgvDatos.Columns[9].ItemStyle.Width = 75;
            dgvDatos.Columns[18].ItemStyle.Width = 100;

            dgvDatos.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[8].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[9].ItemStyle.HorizontalAlign = HorizontalAlign.Center;

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
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid(string sFecha_P)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_itinerarios" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(cmbFiltrarGrid.SelectedValue) + Environment.NewLine;
                sSql += "and fecha_viaje = '" + Convert.ToDateTime(sFecha_P).ToString("yyyy/MM/dd") + "'" + Environment.NewLine;
                sSql += "and normal = 1" + Environment.NewLine;
                sSql += "order by hora_salida";

                columnasGrid(true);
                asignarE.ISSQL = sSql;
                dgvDatos.DataSource = asignarM.listarViajes(asignarE);
                dgvDatos.DataBind();
                columnasGrid(false);
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA MANIPULACION DE COLUMNAS EXTRAS
        private void columnasGridExtra(bool ok)
        {
            dgvDatosExtras.Columns[0].ItemStyle.Width = 75;
            dgvDatosExtras.Columns[2].ItemStyle.Width = 75;
            dgvDatosExtras.Columns[3].ItemStyle.Width = 130;
            dgvDatosExtras.Columns[4].ItemStyle.Width = 150;
            dgvDatosExtras.Columns[5].ItemStyle.Width = 250;
            dgvDatosExtras.Columns[6].ItemStyle.Width = 100;
            dgvDatosExtras.Columns[7].ItemStyle.Width = 100;
            dgvDatosExtras.Columns[8].ItemStyle.Width = 100;
            dgvDatosExtras.Columns[9].ItemStyle.Width = 75;
            dgvDatosExtras.Columns[18].ItemStyle.Width = 100;

            dgvDatosExtras.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosExtras.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosExtras.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosExtras.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosExtras.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosExtras.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosExtras.Columns[8].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosExtras.Columns[9].ItemStyle.HorizontalAlign = HorizontalAlign.Center;


            dgvDatosExtras.Columns[1].Visible = ok;
            dgvDatosExtras.Columns[10].Visible = ok;
            dgvDatosExtras.Columns[11].Visible = ok;
            dgvDatosExtras.Columns[12].Visible = ok;
            dgvDatosExtras.Columns[13].Visible = ok;
            dgvDatosExtras.Columns[14].Visible = ok;
            dgvDatosExtras.Columns[15].Visible = ok;
            dgvDatosExtras.Columns[16].Visible = ok;
            dgvDatosExtras.Columns[17].Visible = ok;
            dgvDatosExtras.Columns[18].Visible = ok;
            dgvDatosExtras.Columns[19].Visible = ok;
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGridExtras(string sFecha_P)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_itinerarios" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(cmbFiltrarGrid.SelectedValue) + Environment.NewLine;
                sSql += "and fecha_viaje = '" + Convert.ToDateTime(sFecha_P).ToString("yyyy/MM/dd") + "'" + Environment.NewLine;
                sSql += "and extra = 1" + Environment.NewLine;
                sSql += "order by hora_salida";


                columnasGridExtra(true);
                asignarE.ISSQL = sSql;
                dgvDatosExtras.DataSource = asignarM.listarViajes(asignarE);
                dgvDatosExtras.DataBind();
                columnasGridExtra(false);
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //INSERTAR UN CLIENTE
        private void insertarPasajero()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    lblMensajeError.Text = "No se pudo iniciar la transacción para guardar los registros.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal1').modal('show');</script>", false);
                    goto fin;
                }

                sFecha = Convert.ToDateTime(txtFechaNacimiento.Text.Trim()).ToString("yyyy/MM/dd");

                int iIdTipoPersona_P;
                int iIdTipoIdentificacion_P;

                int iTercerDigito = Convert.ToInt32(txtIdentificacionRegistro.Text.Trim().Substring(2, 1));
                int iLongitud_P = txtIdentificacionRegistro.Text.Trim().Length;

                if (iLongitud_P == 13)
                {
                    if (iTercerDigito == 9)
                    {
                        iIdTipoPersona_P = 2448;
                        iIdTipoIdentificacion_P = 179;
                    }

                    else if (iTercerDigito == 6)
                    {
                        iIdTipoPersona_P = 2448;
                        iIdTipoIdentificacion_P = 179;
                    }

                    else
                    {
                        iIdTipoPersona_P = 2447;
                        iIdTipoIdentificacion_P = 179;
                    }
                }

                else if (iLongitud_P == 10)
                {
                    iIdTipoPersona_P = 2447;
                    iIdTipoIdentificacion_P = 178;
                }

                else
                {
                    iIdTipoPersona_P = 2447;
                    iIdTipoIdentificacion_P = 180;
                }

                sSql = "";
                sSql += "insert into tp_personas (" + Environment.NewLine;
                sSql += "identificacion, apellidos, nombres, fecha_nacimiento, discapacidad," + Environment.NewLine;
                sSql += "idempresa, cg_tipo_persona, cg_tipo_identificacion," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "'" + txtIdentificacionRegistro.Text.Trim().ToUpper() + "', '" + txtRazonSocial.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'" + txtNombreRegistro.Text.Trim().ToUpper() + "', '" + sFecha + "', " + iDiscapacidad + "," + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idEmpresa"].ToString()) + ", ";
                sSql += iIdTipoPersona_P + ", " + iIdTipoIdentificacion_P + "," + Environment.NewLine;
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //OBTENER EL ID DE LA TABLA TP_PERSONAS
                sTabla = "tp_personas";
                sCampo = "id_persona";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>No se pudo obtener el código de la tabla " + sTabla + ".";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }

                else
                {
                    Session["idPasajero"] = Convert.ToInt32(iMaximo);
                }

                conexionM.terminaTransaccion();

                Session["identificacion"] = txtIdentificacionRegistro.Text;
                txtIdentificacion.Text = txtIdentificacionRegistro.Text;
                txtNombrePasajero.Text = (txtRazonSocial.Text.Trim().ToUpper() + " " + txtNombreRegistro.Text.Trim().ToUpper()).Trim();

                cerrarModalRegistro();

                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro ingresado éxitosamente.', 'success');", true);

                buscarCliente(txtIdentificacion.Text.Trim());

                goto fin;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); }
        fin: { }
        }

        //ACTUALIZAR EL REGISTRO DE PERSONAS
        private void actualizarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    lblMensajeError.Text = "No se pudo iniciar la transacción para guardar los registros.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal1').modal('show');</script>", false);
                    return;
                }

                int iIdTipoPersona_P;
                int iIdTipoIdentificacion_P;

                int iTercerDigito = Convert.ToInt32(txtIdentificacionRegistro.Text.Trim().Substring(2, 1));
                int iLongitud_P = txtIdentificacionRegistro.Text.Trim().Length;

                if (iLongitud_P == 13)
                {
                    if (iTercerDigito == 9)
                    {
                        iIdTipoPersona_P = 2448;
                        iIdTipoIdentificacion_P = 179;
                    }

                    else if (iTercerDigito == 6)
                    {
                        iIdTipoPersona_P = 2448;
                        iIdTipoIdentificacion_P = 179;
                    }

                    else
                    {
                        iIdTipoPersona_P = 2447;
                        iIdTipoIdentificacion_P = 179;
                    }
                }

                else if (iLongitud_P == 10)
                {
                    iIdTipoPersona_P = 2447;
                    iIdTipoIdentificacion_P = 178;
                }

                else
                {
                    iIdTipoPersona_P = 2447;
                    iIdTipoIdentificacion_P = 180;
                }

                sSql = "";
                sSql += "update tp_personas set" + Environment.NewLine;
                sSql += "cg_tipo_persona = " + iIdTipoPersona_P + "," + Environment.NewLine;
                sSql += "cg_tipo_identificacion = " + iIdTipoIdentificacion_P + "," + Environment.NewLine;
                sSql += "identificacion = '" + txtIdentificacionRegistro.Text.Trim() + "'," + Environment.NewLine;
                sSql += "nombres = '" + txtNombreRegistro.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "apellidos = '" + txtRazonSocial.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "fecha_nacimiento = '" + Convert.ToDateTime(txtFechaNacimiento.Text.Trim()).ToString("yyyy/MM/dd") + "'," + Environment.NewLine;
                sSql += "discapacidad = " + iDiscapacidad + Environment.NewLine;
                sSql += "where id_persona = " + Convert.ToInt32(Session["idPersonaConsulta"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();

                Session["identificacion"] = txtIdentificacionRegistro.Text;
                txtIdentificacion.Text = txtIdentificacionRegistro.Text;
                txtNombrePasajero.Text = (txtRazonSocial.Text.Trim().ToUpper() + " " + txtNombreRegistro.Text.Trim().ToUpper()).Trim();

                cerrarModalRegistro();

                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro actualizado éxitosamente.', 'success');", true);

                buscarCliente(txtIdentificacion.Text.Trim());

                return;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); }
        }

        //FUNCION PARA CERRAR EL MODAL DE REGISTRO DE PASAJERO
        private void cerrarModalRegistro()
        {
            cmbIdentificacion.SelectedIndex = 0;
            txtIdentificacionRegistro.Text = "";
            txtRazonSocial.Text = "";
            txtNombreRegistro.Text = "";
            txtFechaNacimiento.Text = "";
            lblAlerta.Text = "";
            ModalPopupExtenderCrearEditar.Hide();
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            llenarComboTipoCliente();

            cmbTipoIdentificacion.SelectedIndex = 0;
            txtIdentificacion.Text = "";
            txtNombrePasajero.Text = "";
            Session["dtClientes"] = null;
            Session["idPasajero"] = null;
            Session["idAsiento"] = null;
            Session["numeroAsiento"] = null;
            Session["identificacion"] = null;
            Session["idProducto"] = null;
            Session["idPueblo"] = null;
            Session["tasa_usuario"] = null;
            Session["Json"] = null;
            lblEdad.Text = "SIN ASIGNAR";
            lblEdad.ForeColor = Color.Black;
            lblEdad.BackColor = Color.White;

            asientosOcupados();
            extraerTotalCobrado();
            crearDataTable();

            consultarPrecio();

            //ADICIONAL 
            Session["idPersonaTipoCliente"] = null;
            indice = cmbDestino.SelectedIndex;

            if (consultarListaPrecioTipoCliente(Convert.ToInt32(cmbTipoCliente.SelectedValue)) == true)
            {
                if (indice == 0)
                {
                    txtPrecio.Text = "0.00";
                    txtDescuento.Text = "0.00";
                    txtPrecioFinal.Text = "0.00";
                }

                else
                {
                    cmbDestino.SelectedIndex = indice;
                    manejarTipoClienteManual(Convert.ToInt32(cmbTipoCliente.SelectedValue));
                    consultarPrecio();
                }
            }

            txtIdentificacion.Attributes.Add("onKeyPress", "doClick('" + btnBuscarCliente.ClientID + "',event)");
        }

        //FUNCION PARA CREAR EL DATATABLE
        private void crearDataTable()
        {
            try
            {
                dtAlmacenar = new DataTable();
                dtAlmacenar.Columns.Add("IDASIENTO");
                dtAlmacenar.Columns.Add("NUMEROASIENTO");
                dtAlmacenar.Columns.Add("PRECIO");
                dtAlmacenar.Columns.Add("DESCUENTO");
                dtAlmacenar.Columns.Add("IDPRODUCTO");
                dtAlmacenar.Columns.Add("IDDETALLERUTA");
                dtAlmacenar.Columns.Add("IDTIPOCLIENTE");
                dtAlmacenar.Columns.Add("IDPERSONAASIENTO");
                dtAlmacenar.Columns.Add("NOMBREPASAJERO");
                dtAlmacenar.Columns.Add("IDENTIFICACION");
                dtAlmacenar.Columns.Add("EXTRANJERO");
                dtAlmacenar.Columns.Add("DESTINO");
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA VALIDAR LA INSERCION
        private void validarInsertar(int iOp)
        {
            try
            {
                iBandera = 0;

                if (Session["dtClientes"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No hay pasajes a vender.', 'warning');", true);
                    return;
                }

                dtAlmacenar = new DataTable();
                dtAlmacenar = Session["dtClientes"] as DataTable;

                if (dtAlmacenar.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No hay pasajes a vender.', 'warning');", true);
                    goto fin;
                }

                if (iOp == 0)
                {
                    //INSERTAR CONSUMIDOR FINAL
                    Session["idPersonaFactura"] = Application["consumidor_final"].ToString();
                    insertarRegistro();
                }

                else if (iOp == 1)
                {
                    //INSERTAR CON DATOS
                    llenarGridClientes(0, 1);
                    lblAlerta.Text = "";
                    ModalPopupExtender_Factura.Show();
                }

                else if (iOp == 2)
                {
                    //INSERTAR CON FACTURA RÁPIDA
                    if ((Session["idPasajero"] == null) && (txtNombrePasajero.Text.Trim() == ""))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese datos del pasajero', 'warning');", true);
                        txtIdentificacion.Focus();
                        goto fin;
                    }

                    else if ((Session["idPasajero"] == null) && (txtNombrePasajero.Text.Trim() != ""))
                    {
                        Session["idPersonaFactura"] = Application["idPersonaSinDatos"].ToString();
                        sNombrePasajero = txtNombrePasajero.Text.Trim().ToUpper();
                        iBandera = 1;
                    }

                    else
                    {
                        //Session["idPersonaFactura"] = Session["idPasajero"].ToString();
                        //Session["idPersonaFactura"] = Application["consumidor_final"].ToString();

                        DataTable dtExtraer = new DataTable();
                        dtExtraer = Session["dtClientes"] as DataTable;

                        int iIdPersonaFactura_P = Convert.ToInt32(dtExtraer.Rows[0][7].ToString());
                        int iIdPersonaMenorEdad = Convert.ToInt32(Application["idPersonaMenorEdad"].ToString());
                        int iIdPersonaSinDatos = Convert.ToInt32(Application["idPersonaSinDatos"].ToString());

                        if (iIdPersonaFactura_P == iIdPersonaMenorEdad)
                        {
                            Session["idPersonaFactura"] = Application["consumidor_final"].ToString();
                        }

                        else if (iIdPersonaFactura_P == iIdPersonaSinDatos)
                        {
                            Session["idPersonaFactura"] = Application["consumidor_final"].ToString();
                        }

                        else
                        {
                            Session["idPersonaFactura"] = iIdPersonaFactura_P.ToString();
                        }
                    }

                    insertarRegistro();
                    goto fin;
                }
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto fin;
            }

        fin: { }
        }

        //PROCESO DE INSERCION
        private void insertarRegistro()
        {
            try
            {
                //AQUI VAMOS A GENERAR LAS TASAS DE USUARIO
                ventaBoleto = new Clases_Contifico.ClaseCrearVenta();

                sFecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                iNumeroViaje_A = Convert.ToInt32(Session["iIdViajeVentaSMARTT"].ToString());

                //SECCION PARA BUSCAR LSO DATOS DE LA PERSONA PARA LA FACTURA
                //---------------------------------------------------------------------------------
                iIdPersona = Convert.ToInt32(Session["idPersonaFactura"].ToString());

                if (consultarClienteFacturar() == false)
                {
                    return;
                }
                //---------------------------------------------------------------------------------

                //SECCION PARA CONSTRUIR LA TABLA PARA LA EMISION DE LA TASA DE USUARIO
                //---------------------------------------------------------------------------------
                if (obtenerIdentificadoresSmart() == false)
                {
                    return;
                }

                ////SOLO PARA PROBAR
                //dtAlmacenar = new DataTable();
                //dtAlmacenar = Session["dtClientes"] as DataTable;

                //dtDatosTasaUsuario = new DataTable();
                //dtDatosTasaUsuario.Columns.Add("nivel");
                //dtDatosTasaUsuario.Columns.Add("asiento");
                //dtDatosTasaUsuario.Columns.Add("asiento_nombre");
                //dtDatosTasaUsuario.Columns.Add("valor_asiento");
                //dtDatosTasaUsuario.Columns.Add("tipo_tarifa");
                //dtDatosTasaUsuario.Columns.Add("valor_descuento");
                //dtDatosTasaUsuario.Columns.Add("identificacion_pasajero");
                //dtDatosTasaUsuario.Columns.Add("nombre_pasajero");
                //dtDatosTasaUsuario.Columns.Add("correo_pasajero");
                //dtDatosTasaUsuario.Columns.Add("extranjero_pasajero");

                //for (int i = 0; i < dtAlmacenar.Rows.Count; i++)
                //{
                //    iNumeroAsiento_API = Convert.ToInt32(dtAlmacenar.Rows[i]["NUMEROASIENTO"].ToString());
                //    sNumeroAsiento_API = dtAlmacenar.Rows[i]["NUMEROASIENTO"].ToString().Trim();
                //    dbValorAsiento_API = Convert.ToDecimal(dtAlmacenar.Rows[i]["PRECIO"].ToString());

                //    DataRow[] dFila = dtTarifasSmartt.Select("id_ctt_tipo_cliente = " + dtAlmacenar.Rows[i]["IDTIPOCLIENTE"].ToString());

                //    if (dFila.Length != 0)
                //    {
                //        iTipoTarifa_API = Convert.ToInt32(dFila[0][1].ToString());
                //    }

                //    dbValorDescuento_API = Convert.ToDecimal(dtAlmacenar.Rows[i]["DESCUENTO"].ToString());
                //    sIdentificacionPasajero_API = dtAlmacenar.Rows[i]["IDENTIFICACION"].ToString().Trim();
                //    sNombrePasajero_API = dtAlmacenar.Rows[i]["NOMBREPASAJERO"].ToString().Trim();
                //    sCorreoPasajero_API = Application["correo_default"].ToString().ToLower();
                //    iExtranjero_API = Convert.ToInt32(dtAlmacenar.Rows[i]["EXTRANJERO"].ToString());

                //    dtDatosTasaUsuario.Rows.Add(iNivel_API.ToString(), iNumeroAsiento_API.ToString(), sNumeroAsiento_API,
                //                                dbValorAsiento_API.ToString("N2"), iTipoTarifa_API.ToString(),
                //                                dbValorDescuento_API.ToString("N2"), sIdentificacionPasajero_API,
                //                                sNombrePasajero_API, sCorreoPasajero_API, iExtranjero_API.ToString());
                //}

                //sNombreParada_A = dtAlmacenar.Rows[0]["DESTINO"].ToString().Trim();

                ////---------------------------------------------------------------------------------


                //sRespuesta_A = ventaBoleto.recuperarJsonCrearVenta(Session["tokenSMARTT"].ToString().Trim(), sFecha, iNumeroViaje_A,
                //                                                       iFormaPago_A, Session["idLocalidadSMARTT"].ToString(), sNombreParada_A,
                //                                                       sIdentificacionFactura, sClienteFactura, sCorreoElectronico,
                //                                                       iExtranjero_A, dtDatosTasaUsuario);

                //if (sRespuesta_A == "ERROR")
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'No se pudo obtener registros para la tasa de usuario SMARTT', 'error')</script>");
                //    return;
                //}

                //if (sRespuesta_A == "ISNULL")
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Información.!', 'No se proporcionaron credenciales de autenticación. Tasa de Usuario SMARTT', 'info')</script>");
                //    return;
                //}

                //Session["JsonCrearVenta"] = sRespuesta_A;

                #region JSON PRUEBAS

                string sJson;

                sJson = "";
                sJson += "{" + Environment.NewLine;
                sJson += "\"id\": 31232," + Environment.NewLine;
                sJson += "\"fecha_hora_venta\": \"2019-12-19T13:10:00-05:00\"," + Environment.NewLine;
                sJson += "\"numero_documento\": \"\"," + Environment.NewLine;
                sJson += "\"clave_acceso\": null," + Environment.NewLine;
                sJson += "\"numero_documento_tasa\": \"001-001-000000007\"," + Environment.NewLine;
                sJson += "\"clave_acceso_tasa\": \"1912201901099223657400110010010000000071404488312\"," + Environment.NewLine;
                sJson += "\"total_tasas\": 0.5," + Environment.NewLine;
                sJson += "\"viaje\": 11002615," + Environment.NewLine;
                sJson += "\"forma_de_pago\": 1," + Environment.NewLine;
                sJson += "\"cliente\": {" + Environment.NewLine;
                sJson += "\"identificacion\": \"1717644551\"," + Environment.NewLine;
                sJson += "\"id\": 10641153," + Environment.NewLine;
                sJson += "\"nombre\": \"ELVIS GEOVANNI GUAIGUA AGUALSACA\"," + Environment.NewLine;
                sJson += "\"correo\": \"\"," + Environment.NewLine;
                sJson += "\"tipo_cliente\": \"N\"," + Environment.NewLine;
                sJson += "\"direccion\": null," + Environment.NewLine;
                sJson += "\"telefono\": null," + Environment.NewLine;
                sJson += "\"tipo_identificacion\": \"CED\"," + Environment.NewLine;
                sJson += "\"extranjero\": false," + Environment.NewLine;
                sJson += "\"is_active\": true," + Environment.NewLine;
                sJson += "\"is_enable\": true," + Environment.NewLine;
                sJson += "\"actualizacion\": \"2019-12-19T13:11:04.737187-05:00\"" + Environment.NewLine;
                sJson += "}," + Environment.NewLine;
                sJson += "\"boletos\": [" + Environment.NewLine;
                sJson += "{" + Environment.NewLine;
                sJson += "\"id\": 34931," + Environment.NewLine;
                sJson += "\"asiento\": 6," + Environment.NewLine;
                sJson += "\"asiento_nombre\": \"6\"," + Environment.NewLine;
                sJson += "\"nivel\": 1," + Environment.NewLine;
                sJson += "\"valor\": 8," + Environment.NewLine;
                sJson += "\"localidad_embarque\": 1," + Environment.NewLine;
                sJson += "\"tipo_cliente\": 1," + Environment.NewLine;
                sJson += "\"parada_embarque\": null," + Environment.NewLine;
                sJson += "\"parada_destino\": 846," + Environment.NewLine;
                sJson += "\"pasajero\": {" + Environment.NewLine;
                sJson += "\"identificacion\": \"1755971478\"," + Environment.NewLine;
                sJson += "\"id\": 13630902," + Environment.NewLine;
                sJson += "\"nombre\": \"KATHERIN NICOLE ENRIQUEZ JIMENEZ\"," + Environment.NewLine;
                sJson += "\"correo\": \"contabilidad@expressatenas.com.ec\"," + Environment.NewLine;
                sJson += "\"tipo_cliente\": \"N\"," + Environment.NewLine;
                sJson += "\"direccion\": null," + Environment.NewLine;
                sJson += "\"telefono\": null," + Environment.NewLine;
                sJson += "\"tipo_identificacion\": \"CED\"," + Environment.NewLine;
                sJson += "\"extranjero\": false," + Environment.NewLine;
                sJson += "\"is_active\": true," + Environment.NewLine;
                sJson += "\"is_enable\": true," + Environment.NewLine;
                sJson += "\"actualizacion\": \"2019-12-19T13:11:04.809424-05:00\"" + Environment.NewLine;
                sJson += "}," + Environment.NewLine;
                sJson += "\"tasa\": \"6413802695\"," + Environment.NewLine;
                sJson += "\"estado\": 1," + Environment.NewLine;
                sJson += "\"estado_nombre\": \"Vendido\"," + Environment.NewLine;
                sJson += "\"is_active\": true," + Environment.NewLine;
                sJson += "\"is_enable\": true," + Environment.NewLine;
                sJson += "\"actualizacion\": \"2019-12-19T13:11:04.822454-05:00\"" + Environment.NewLine;
                sJson += "}," + Environment.NewLine;

                sJson += "{" + Environment.NewLine;
                sJson += "\"id\": 34930," + Environment.NewLine;
                sJson += "\"asiento\": 5," + Environment.NewLine;
                sJson += "\"asiento_nombre\": \"5\"," + Environment.NewLine;
                sJson += "\"nivel\": 1," + Environment.NewLine;
                sJson += "\"valor\": 8," + Environment.NewLine;
                sJson += "\"localidad_embarque\": 1," + Environment.NewLine;
                sJson += "\"tipo_cliente\": 1," + Environment.NewLine;
                sJson += "\"parada_embarque\": null," + Environment.NewLine;
                sJson += "\"parada_destino\": 846," + Environment.NewLine;
                sJson += "\"pasajero\": {" + Environment.NewLine;
                sJson += "\"identificacion\": \"1717644551\"," + Environment.NewLine;
                sJson += "\"id\": 10641153," + Environment.NewLine;
                sJson += "\"nombre\": \"ELVIS GEOVANNI GUAIGUA AGUALSACA\"," + Environment.NewLine;
                sJson += "\"correo\": \"contabilidad@expressatenas.com.ec\"," + Environment.NewLine;
                sJson += "\"tipo_cliente\": \"N\"," + Environment.NewLine;
                sJson += "\"direccion\": null," + Environment.NewLine;
                sJson += "\"telefono\": null," + Environment.NewLine;
                sJson += "\"tipo_identificacion\": \"CED\"," + Environment.NewLine;
                sJson += "\"extranjero\": false," + Environment.NewLine;
                sJson += "\"is_active\": true," + Environment.NewLine;
                sJson += "\"is_enable\": true," + Environment.NewLine;
                sJson += "\"actualizacion\": \"2019-12-19T13:11:04.772417-05:00\"" + Environment.NewLine;
                sJson += "}," + Environment.NewLine;
                sJson += "\"tasa\": \"0497329342\"," + Environment.NewLine;
                sJson += "\"estado\": 1," + Environment.NewLine;
                sJson += "\"estado_nombre\": \"Vendido\"," + Environment.NewLine;
                sJson += "\"is_active\": true," + Environment.NewLine;
                sJson += "\"is_enable\": true," + Environment.NewLine;
                sJson += "\"actualizacion\": \"2019-12-19T13:11:04.788847-05:00\"" + Environment.NewLine;
                sJson += "}" + Environment.NewLine;

                sJson += "]," + Environment.NewLine;
                sJson += "\"uuid\": \"fbd6f704-cf59-4ed1-b74f-5a5864309147\"," + Environment.NewLine;
                sJson += "\"estado\": 1," + Environment.NewLine;
                sJson += "\"estado_nombre\": \"Vendido\"," + Environment.NewLine;
                sJson += "\"offline\": false," + Environment.NewLine;
                sJson += "\"emision\": null," + Environment.NewLine;
                sJson += "\"cooperativa\": 101," + Environment.NewLine;
                sJson += "\"destino\": \"QUITO\"," + Environment.NewLine;
                sJson += "\"is_active\": true," + Environment.NewLine;
                sJson += "\"is_enable\": true," + Environment.NewLine;
                sJson += "\"actualizacion\": \"2019-12-19T13:11:04.743249-05:00\"" + Environment.NewLine;
                sJson += "}";

                Session["JsonCrearVenta"] = sJson;

                #endregion

                //INICIA LA INSERCION DE DATOS EN LA BASE
                //--------------------------------------------------------------------------------------

                if (insertarPedido() == false)
                {
                    goto reversa;
                }

                if (insertarPagos() == false)
                {
                    goto reversa;
                }

                if (insertarFactura() == false)
                {
                    goto reversa;
                }

                conexionM.terminaTransaccion();

                Session["id_pedido_reporte"] = iIdPedido.ToString();

                //abreVentana("frmReporteFactura.aspx");

                crearReporteImprimir();

                Session["auxiliar"] = "1";
                mostrarBotones();

                iIndiceDestino = cmbDestino.SelectedIndex;

                limpiar();

                Session["idPersonaFactura"] = null;
                txtBuscarClienteFactura.Text = "";
                lblRazonSocial.Text = "";
                lblMensajeFactura.Text = "";
                ModalPopupExtender_Factura.Hide();

                ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "Popup", "swal('Éxito.!', 'Boletos generados éxitosamente', 'success');", true);

                return;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };
        }

        //INSERTAR FASE 1 -  CREAR PEDIDO
        private bool insertarPedido()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar iniciar la transacción.', 'danger');", true);
                    return false;
                }

                sFecha = DateTime.Now.ToString("yyyy/MM/dd");

                iIdPersona = Convert.ToInt32(Session["idPersonaFactura"].ToString());

                if (iBandera == 0)
                {
                    sNombrePasajero = "";
                }

                iIdPuebloOrigen = Convert.ToInt32(cmbOrigen.SelectedValue);
                iIdPuebloDestino = Convert.ToInt32(Session["idPueblo"].ToString());

                dbTotal = 0;

                dtAlmacenar = new DataTable();
                dtAlmacenar = Session["dtClientes"] as DataTable;

                for (int i = 0; i < dtAlmacenar.Rows.Count; i++)
                {
                    //dbTotal = dbTotal + Convert.ToDouble(dtAlmacenar.Rows[i][2].ToString()) - Convert.ToDouble(dtAlmacenar.Rows[i][3].ToString());
                    dbTotal = dbTotal + Convert.ToDouble(dtAlmacenar.Rows[i][2].ToString(), System.Globalization.CultureInfo.InvariantCulture) - Convert.ToDouble(dtAlmacenar.Rows[i][3].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                }

                //INSTRUCCION PARA INSERTAR EN LA TABLA CV403_CAB_PEDIDOS
                sSql = "";
                sSql += "insert into cv403_cab_pedidos (" + Environment.NewLine;
                sSql += "idempresa, cg_empresa, id_localidad, fecha_pedido, id_persona, cg_tipo_cliente," + Environment.NewLine;
                sSql += "cg_moneda, porcentaje_iva, id_vendedor, cg_estado_pedido, porcentaje_dscto," + Environment.NewLine;
                sSql += "cg_facturado, id_ctt_programacion, id_ctt_oficinista, nombre_pasajero," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                sSql += "numero_replica_trigger, numero_control_replica, cobro_boletos, cobro_retencion," + Environment.NewLine;
                sSql += "cobro_administrativo, id_ctt_pueblo_origen, id_ctt_pueblo_destino," + Environment.NewLine;
                sSql += "bandera_boleteria, id_ctt_jornada, id_ctt_cierre_caja)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idEmpresa"].ToString()) + ", " + Convert.ToInt32(Application["cgEmpresa"].ToString()) + ", ";
                sSql += Convert.ToInt32(Application["idLocalidad"].ToString()) + ", '" + sFecha + "', " + iIdPersona + "," + Environment.NewLine;
                sSql += parametros.CgTipoCliente + ", " + Convert.ToInt32(Application["cgMoneda"].ToString()) + ", 0, " + Convert.ToInt32(Session["idVendedor"].ToString()) + ",";
                sSql += parametros.CgEstadoPedido + ", 0, 7469, " + Convert.ToInt32(Session["idProgramacion"].ToString()) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(Session["idUsuario"].ToString()) + ", '" + sNombrePasajero + "', ";
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 0, 0, 1, 0, 0, " + iIdPuebloOrigen + ", " + iIdPuebloDestino + ", 1," + Environment.NewLine;
                sSql += Convert.ToInt32(Session["idJornada"].ToString()) + ", " + Session["idCierreCaja"].ToString() + ")";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //QUERY PARA INSERTAR EN CV403_CAB_DESPACHOS
                sSql = "";
                sSql += "insert into cv403_cab_despachos (" + Environment.NewLine;
                sSql += "idempresa, id_persona, cg_empresa, id_localidad, fecha_despacho," + Environment.NewLine;
                sSql += "cg_motivo_despacho, id_destinatario, punto_partida, cg_ciudad_entrega," + Environment.NewLine;
                sSql += "direccion_entrega, id_transportador, fecha_inicio_transporte," + Environment.NewLine;
                sSql += "fecha_fin_transporte, cg_estado_despacho, punto_venta, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso, estado, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idEmpresa"].ToString()) + ", " + iIdPersona + ", " + Convert.ToInt32(Application["cgEmpresa"].ToString()) + ", " + Convert.ToInt32(Application["idLocalidad"].ToString()) + ", ";
                sSql += "'" + sFecha + "', 6972, " + iIdPersona + "," + Environment.NewLine;
                sSql += "'" + parametros.sPuntoPartida + "', " + parametros.iCgCiudadEntrega + ", '" + parametros.sDireccionEntrega + "'," + Environment.NewLine;
                sSql += "'" + iIdPersona + "', GETDATE(), GETDATE(), " + parametros.iCgEstadoDespacho + "," + Environment.NewLine;
                sSql += "1, GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A', 1, 0)";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //OBTENER EL ID DE LA TABLA CV403_CAB_PEDIDOS
                sTabla = "cv403_cab_pedidos";
                sCampo = "id_pedido";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>No se pudo obtener el código de la tabla " + sTabla + ".";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                else
                {
                    iIdPedido = Convert.ToInt32(iMaximo);
                }

                //INSERTANOS LA RESPUESTA DE LAS TASAS DE USUARIO SMARTT EN LA TABLA ctt_detalle_tasa_smartt
                //DESERIALIZAMOS EN VARIABLES LOS CAMPOS PARA LA TABLA
                sRespuesta_A = Session["JsonCrearVenta"].ToString();

                tasaUsuario = JsonConvert.DeserializeObject<Clase_Variables_Contifico.TasaUsuarioSmartt>(sRespuesta_A);

                int iId_S = tasaUsuario.id;
                string sFechaHoraVenta_S = tasaUsuario.fecha_hora_venta.ToString();
                string sNumeroDocumento_S = tasaUsuario.numero_documento;
                string sClaveAcceso_S;
                
                if (string.IsNullOrEmpty(tasaUsuario.clave_acceso))
                {
                    sClaveAcceso_S = "";
                }

                else
                {
                    sClaveAcceso_S = tasaUsuario.clave_acceso;
                }

                string sNumeroDocumentoTasa_S = tasaUsuario.numero_documento_tasa;
                string sClaveAccesoTasa_S = tasaUsuario.clave_acceso_tasa;
                Double dbTotalTasas_S = tasaUsuario.total_tasas;
                int iViaje_S = tasaUsuario.viaje;
                int iFormaPago_S = tasaUsuario.forma_de_pago;
                string sIdentificacionFactura_S = tasaUsuario.cliente.identificacion;
                int iIdCliente_S = tasaUsuario.cliente.id;
                string sNombreFactura_S = tasaUsuario.cliente.nombre;
                string sCorreoFactura_S = tasaUsuario.cliente.correo;
                string sTipoClienteFactura_S = tasaUsuario.cliente.tipo_cliente;
                string sDireccionFactura_S;
                
                if (string.IsNullOrEmpty(tasaUsuario.cliente.direccion))
                    sDireccionFactura_S = "";
                else
                    sDireccionFactura_S = tasaUsuario.cliente.direccion;             
                
                string sTelefonoFactura_S;

                if (string.IsNullOrEmpty(tasaUsuario.cliente.telefono))
                    sTelefonoFactura_S = "";
                else
                    sTelefonoFactura_S = tasaUsuario.cliente.telefono;

                string sTipoIdentificacion_S = tasaUsuario.cliente.tipo_identificacion;

                int iExtranjeroFactura_S, iActivoFactura_S, iHabilitadoFactura_S, iOffline_S, iActivoTasa_S, iHabilitadoTasa_S;

                if (tasaUsuario.cliente.extranjero == true)
                    iExtranjeroFactura_S = 1;
                else
                    iExtranjeroFactura_S = 0;

                if (tasaUsuario.cliente.is_active == true)
                    iActivoFactura_S = 1;
                else
                    iActivoFactura_S = 0;

                if (tasaUsuario.cliente.is_enable == true)
                    iHabilitadoFactura_S = 1;
                else
                    iHabilitadoFactura_S = 0;

                string sFechaActualizacionFactura_S = tasaUsuario.cliente.actualizacion.ToString();
                string sUuid = tasaUsuario.uuid;
                int iEstadoTasa_S = tasaUsuario.estado;
                string sEstadoNombreTasa_S = tasaUsuario.estado_nombre;

                if (tasaUsuario.offline == true)
                    iOffline_S = 1;
                else
                    iOffline_S = 0;

                string sEmisionTasa_S;
                
                if (string.IsNullOrEmpty(tasaUsuario.emision))
                    sEmisionTasa_S = "";
                else
                    sEmisionTasa_S = tasaUsuario.emision;

                int iCooperativaTasa_S = tasaUsuario.cooperativa;
                string sDestinoTasa_S = tasaUsuario.destino;

                if (tasaUsuario.is_active == true)
                    iActivoTasa_S = 1;
                else
                    iActivoTasa_S = 0;

                if (tasaUsuario.is_enable == true)
                    iHabilitadoTasa_S = 1;
                else
                    iHabilitadoTasa_S = 0;

                string sFechaActualizacionTasa_S = tasaUsuario.actualizacion.ToString();

                sSql = "";
                sSql += "insert into ctt_detalle_tasa_smartt (" + Environment.NewLine;
                sSql += "id_pedido, id, fecha_hora_venta, numero_documento, clave_acceso, numero_documento_tasa," + Environment.NewLine;
                sSql += "clave_acceso_tasa, total_tasas, viaje, forma_de_pago, identificacion, id_cliente," + Environment.NewLine;
                sSql += "nombre, correo, tipo_cliente, direccion, telefono, tipo_identificacion, extranjero," + Environment.NewLine;
                sSql += "is_active_cliente, is_enable_cliente, actualizacion_cliente, uuid, estado_smartt," + Environment.NewLine;
                sSql += "estado_nombre, offline, emision, cooperativa, destino, is_active, is_enable," + Environment.NewLine;
                sSql += "actualizacion_boletos, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdPedido + ", " + iId_S + ", '" + sFechaHoraVenta_S + "', '" + sNumeroDocumento_S + "'," + Environment.NewLine;
                sSql += "'" + sClaveAcceso_S + "', '" + sNumeroDocumentoTasa_S + "', '" + sClaveAccesoTasa_S + "'," + Environment.NewLine;
                sSql += dbTotalTasas_S + ", " + iViaje_S + ", " + iFormaPago_S + ", '" + sIdentificacionFactura_S + "'," + Environment.NewLine;
                sSql += iIdCliente_S + ", '" + sNombreFactura_S + "', '" + sCorreoFactura_S + "', '" + sTipoClienteFactura_S + "'," + Environment.NewLine;
                sSql += "'" + sDireccionFactura_S + "', '" + sTelefonoFactura_S + "', '" + sTipoIdentificacion_S + "'," + Environment.NewLine;
                sSql += iExtranjeroFactura_S + ", " + iActivoFactura_S + ", " + iHabilitadoFactura_S + ", '" + sFechaActualizacionFactura_S + "'," + Environment.NewLine;
                sSql += "'" + sUuid + "', " + iEstadoTasa_S + ", '" + sEstadoNombreTasa_S + "', " + iOffline_S + "," + Environment.NewLine;
                sSql += "'" + sEmisionTasa_S + "', " + iCooperativaTasa_S + ", '" + sDestinoTasa_S + "', " + iActivoTasa_S + "," + Environment.NewLine;
                sSql += iHabilitadoTasa_S + ", '" + sFechaActualizacionTasa_S + "', 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }
                
                //PROCEDIMIENTO PARA EXTRAER EL NUMERO DE PEDIDO
                sSql = "";
                sSql += "select numero_pedido" + Environment.NewLine;
                sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    iNumeroPedido = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //QUERY PARA ACTUALIZAR EL NUMERO DE PEDIDO EN LA TABLA TP_LOCALIDADES_IMPRESORAS
                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numero_pedido = numero_pedido + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString());

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //QUERY PARA PODER INSERTAR REGISTRO EN LA TABLA CV403_NUMERO_CAB_PEDIDO
                sSql = "";
                sSql += "insert into cv403_numero_cab_pedido (" + Environment.NewLine;
                sSql += "idtipocomprobante,id_pedido, numero_pedido," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                sSql += "estado, numero_control_replica, numero_replica_trigger)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "1, " + iIdPedido + ", " + iNumeroPedido + ", GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A', 0, 0)";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //PROCEDIMINTO PARA EXTRAER EL ID DE LA TABLA CV403_CAB_DESPACHOS
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sTabla = "cv403_cab_despachos";
                sCampo = "id_despacho";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>No se pudo obtener el código de la tabla " + sTabla + ".";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                else
                {
                    iIdCabDespachos = Convert.ToInt32(iMaximo);
                }

                //QUERY PARA INSERTAR EN LA TABLA CV403_DESPACHOS_PEDIDOS
                sSql = "";
                sSql += "insert into cv403_despachos_pedidos (" + Environment.NewLine;
                sSql += "id_despacho, id_pedido, estado, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                sSql += "terminal_ingreso, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdCabDespachos + "," + iIdPedido + ", 'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 1, 0)";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //PROCEDIMINTO PARA EXTRAER EL ID DE LA TABLA CV403_CAB_DESPACHOS_PEDIDOS
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sTabla = "cv403_despachos_pedidos";
                sCampo = "id_despacho_pedido";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>No se pudo obtener el código de la tabla " + sTabla + ".";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                else
                {
                    iIdDespachoPedido = Convert.ToInt32(iMaximo);
                }

                //QUERY PARA INSERTAR UN NUEVO REGISTRO EN LA TABLA CV403_EVENTOS_COBROS
                sSql = "";
                sSql += "insert into cv403_eventos_cobros (" + Environment.NewLine;
                sSql += "idempresa, cg_empresa, id_persona, id_localidad, cg_evento_cobro," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, estado," + Environment.NewLine;
                sSql += "numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idEmpresa"].ToString()) + ", " + Convert.ToInt32(Application["cgEmpresa"].ToString()) + ", " + iIdPersona + "," + Convert.ToInt32(Application["idLocalidad"].ToString()) + "," + Environment.NewLine;
                sSql += "7466, GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A', 1, 0)";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //PROCEDIMINTO PARA EXTRAER EL ID DE LA TABLA CV403_EVENTOS_COBROS
                sTabla = "cv403_eventos_cobros";
                sCampo = "id_evento_cobro";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>No se pudo obtener el código de la tabla " + sTabla + ".";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                else
                {
                    iIdEventoCobro = Convert.ToInt32(iMaximo);
                }

                //QUERY PARA INSERTAR EN LA TABLA CV403_DCTOS_POR_COBRAR
                sSql = "";
                sSql += "insert into cv403_dctos_por_cobrar (" + Environment.NewLine;
                sSql += "id_evento_cobro, id_pedido, cg_tipo_documento, fecha_vcto, cg_moneda," + Environment.NewLine;
                sSql += "valor, cg_estado_dcto, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                sSql += "numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdEventoCobro + ", " + iIdPedido + ", " + parametros.CgTipoDocumento + "," + Environment.NewLine;
                //sSql += "'" + sFecha + "', " + Convert.ToInt32(Application["cgMoneda"].ToString()) + ", " + dbTotal + "," + Environment.NewLine;
                sSql += "'" + sFecha + "', " + Convert.ToInt32(Application["cgMoneda"].ToString()) + ", " + dbTotal.ToString(System.Globalization.CultureInfo.InvariantCulture) + "," + Environment.NewLine;
                sSql += parametros.CgEstadoDcto + ", 'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 1, 0)";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                int iIdPersonaMenorEdad = Convert.ToInt32(Application["idPersonaMenorEdad"].ToString());
                int iIdPersonaSinDatos = Convert.ToInt32(Application["idPersonaSinDatos"].ToString());

                dtAlmacenar = new DataTable();
                dtAlmacenar = Session["dtClientes"] as DataTable;
                string sNombreComentarioPasajero;

                for (int i = 0; i < dtAlmacenar.Rows.Count; i++)
                {
                    iIdAsiento = Convert.ToInt32(dtAlmacenar.Rows[i][0].ToString());
                    dbPrecioUnitario = Convert.ToDouble(dtAlmacenar.Rows[i][2].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    dbDescuento = Convert.ToDouble(dtAlmacenar.Rows[i][3].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    iIdProducto = Convert.ToInt32(dtAlmacenar.Rows[i][4].ToString());
                    iIdDetalleRuta = Convert.ToInt32(dtAlmacenar.Rows[i][5].ToString());
                    iIdTipoCliente = Convert.ToInt32(dtAlmacenar.Rows[i][6].ToString());
                    iIdPersonaAsiento = Convert.ToInt32(dtAlmacenar.Rows[i][7].ToString());
                    sIdentificacionParaTasa = dtAlmacenar.Rows[i]["IDENTIFICACION"].ToString();

                    if (iIdPersonaAsiento == iIdPersonaMenorEdad)
                    {
                        sNombreComentarioPasajero = dtAlmacenar.Rows[i][8].ToString().ToUpper();
                    }

                    else if (iIdPersonaAsiento == iIdPersonaSinDatos)
                    {
                        sNombreComentarioPasajero = dtAlmacenar.Rows[i][8].ToString().ToUpper();
                    }

                    else
                    {
                        sNombreComentarioPasajero = "";
                    }

                    dbCantidad = 1;
                    dbIva = 0;
                    dbServicio = 0;

                    //INSTRUCCION SQL PARA GUARDAR EN LA BASE DE DATOS
                    sSql = "";
                    sSql += "Insert Into cv403_det_pedidos(" + Environment.NewLine;
                    sSql += "Id_Pedido, id_producto, Cg_Unidad_Medida, precio_unitario," + Environment.NewLine;
                    sSql += "Cantidad, Valor_Dscto, Valor_Ice, Valor_Iva ,Valor_otro," + Environment.NewLine;
                    sSql += "id_ctt_asiento, id_ctt_pueblo, id_ctt_tipo_cliente," + Environment.NewLine;
                    sSql += "estado, fecha_ingreso, usuario_ingreso, " + Environment.NewLine;
                    sSql += "terminal_ingreso, id_persona, comentario)" + Environment.NewLine;
                    sSql += "values(" + Environment.NewLine;
                    sSql += iIdPedido + ", " + iIdProducto + ", 546, " + dbPrecioUnitario.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", " + Environment.NewLine;
                    sSql += dbCantidad + ", " + dbDescuento.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", 0, " + dbIva.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", " + dbServicio.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", " + Environment.NewLine;
                    sSql += iIdAsiento + ", " + iIdDetalleRuta + "," + Environment.NewLine;
                    sSql += iIdTipoCliente + ", 'A',  GETDATE()," + Environment.NewLine;
                    sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', " + iIdPersonaAsiento + ", ";

                    if (sNombreComentarioPasajero == "")
                    {
                        sSql += "null)";
                    }

                    else
                    {
                        sSql += "'" + sNombreComentarioPasajero + "')";
                    }

                    //EJECUCION DE INSTRUCCION SQL
                    if (!conexionM.ejecutarInstruccionSQL(sSql))
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        return false;
                    }

                    //PROCEDIMINTO PARA EXTRAER EL ID DE LA TABLA CV403_EVENTOS_COBROS
                    sTabla = "cv403_det_pedidos";
                    sCampo = "id_det_pedido";

                    iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                    if (iMaximo == -1)
                    {
                        lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>No se pudo obtener el código de la tabla " + sTabla + ".";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        return false;
                    }

                    iIdDetPedido = Convert.ToInt32(iMaximo);

                    //INSERTAMOS EN LA TABLA ctt_tasas_smartt
                    for (int j = 0; j < tasaUsuario.boletos.Length; j++)
                    {
                        string sIdentificacion_Filtro = tasaUsuario.boletos[j].pasajero.identificacion.Trim();

                        if (sIdentificacion_Filtro == sIdentificacionParaTasa)
                        {
                            int iId_Filtro = tasaUsuario.boletos[j].id;
                            double dbValor_Filtro = tasaUsuario.boletos[j].valor;
                            int iLocalidadEmbarque_Filtro = tasaUsuario.boletos[j].localidad_embarque;
                            int iTipoCliente_Filtro = tasaUsuario.boletos[j].tipo_cliente;
                            string sParadaEmbarque_Filtro;

                            if (string.IsNullOrEmpty(tasaUsuario.boletos[j].parada_embarque))
                                sParadaEmbarque_Filtro = "";
                            else
                                sParadaEmbarque_Filtro = tasaUsuario.boletos[j].parada_embarque;
                            
                            int iParadaDestino_Filtro = tasaUsuario.boletos[j].parada_destino;
                            int iIdPasajero_Filtro = tasaUsuario.boletos[j].pasajero.id;
                            string sTipoCiente_Filtro = tasaUsuario.boletos[j].pasajero.tipo_cliente;
                            string sTipoIdentificacion_Filtro = tasaUsuario.boletos[j].pasajero.tipo_identificacion;

                            int iExtranjero_Filtro, iActivoPasajero_Filtro, iHabilitadoPasajero_Filtro, iActivoBoletos_Filtro, iHabilitadoBoletos_Filtro;

                            if (tasaUsuario.boletos[j].pasajero.extranjero == true)
                                iExtranjero_Filtro = 1;
                            else
                                iExtranjero_Filtro = 0;

                            if (tasaUsuario.boletos[j].pasajero.is_active == true)
                                iActivoPasajero_Filtro = 1;
                            else
                                iActivoPasajero_Filtro = 0;

                            if (tasaUsuario.boletos[j].pasajero.is_enable == true)
                                iHabilitadoPasajero_Filtro = 1;
                            else
                                iHabilitadoPasajero_Filtro = 0;

                            string sFechaActualizacionPasajero_Filtro = tasaUsuario.boletos[j].pasajero.actualizacion.ToString();
                            string sTasaUsuario_Filtro = tasaUsuario.boletos[j].tasa;
                            int iEstadoTasa_Filtro = tasaUsuario.boletos[j].estado;
                            string sNombreEstado_Filtro = tasaUsuario.boletos[j].estado_nombre;

                            if (tasaUsuario.boletos[j].is_active == true)
                                iActivoBoletos_Filtro = 1;
                            else
                                iActivoBoletos_Filtro = 0;

                            if (tasaUsuario.boletos[j].is_enable == true)
                                iHabilitadoBoletos_Filtro = 1;
                            else
                                iHabilitadoBoletos_Filtro = 0;

                            string sFechaActualizacionBoletos_Filtro = tasaUsuario.boletos[j].actualizacion.ToString();

                            sSql = "";
                            sSql += "insert into ctt_tasas_smartt (" + Environment.NewLine;
                            sSql += "id_det_pedido, id, valor, localidad_embarque, tipo_cliente, parada_embarque," + Environment.NewLine;
                            sSql += "parada_destino, id_pasajero_smartt, tipo_cliente_pasajero, tipo_identificacion," + Environment.NewLine;
                            sSql += "extranjero_pasajero, is_active_pasajero, is_enable_pasajero, actualizacion_pasajero," + Environment.NewLine;
                            sSql += "tasa_usuario, estado_tasa_usuario, estado_nombre, is_active_smartt, is_enable_smartt," + Environment.NewLine;
                            sSql += "actualizacion_smartt, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                            sSql += "values (" + Environment.NewLine;
                            sSql += iIdDetPedido + ", " + iId_Filtro + ", " + dbValor_Filtro + ", " + iLocalidadEmbarque_Filtro + "," + Environment.NewLine;
                            sSql += iTipoCliente_Filtro + ", '" + sParadaEmbarque_Filtro + "', " + iParadaDestino_Filtro + "," + Environment.NewLine;
                            sSql += iIdPasajero_Filtro + ", '" + sTipoCiente_Filtro + "', '" + sTipoIdentificacion_Filtro + "'," + Environment.NewLine;
                            sSql += iExtranjero_Filtro + ", " + iActivoPasajero_Filtro + ", " + iHabilitadoPasajero_Filtro + ", ";
                            sSql += "'" + sFechaActualizacionPasajero_Filtro + "', '" + sTasaUsuario_Filtro + "'," + Environment.NewLine;
                            sSql += iEstadoTasa_Filtro + ", '" + sNombreEstado_Filtro + "', " + iActivoBoletos_Filtro + ", ";
                            sSql += iHabilitadoBoletos_Filtro + ", '" + sFechaActualizacionBoletos_Filtro + "', 'A', GETDATE()," + Environment.NewLine;
                            sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                            //EJECUCION DE INSTRUCCION SQL
                            if (!conexionM.ejecutarInstruccionSQL(sSql))
                            {
                                lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                                return false;
                            }

                            break;
                        }
                    }
                }


                //INSTRUCCION SQL PARA ACTUALIZAR EL NUMERO DE ASIENTOS
                sSql = "";
                sSql += "update ctt_programacion set" + Environment.NewLine;
                sSql += "asientos_ocupados = asientos_ocupados + " + dtAlmacenar.Rows.Count + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + Convert.ToInt32(Session["idProgramacion"]) + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //INSERTAR FASE 2 - CREAR PAGOS
        private bool insertarPagos()
        {
            try
            {
                //EXTRAER EL ID DE LA TABLA CV403_DCTOS_POR_COBRAR
                sSql = "";
                sSql += "select id_documento_cobrar" + Environment.NewLine;
                sSql += "from cv403_dctos_por_cobrar" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    iIdDocumentoCobrar = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV403_PAGOS
                sSql = "";
                sSql += "insert into cv403_pagos (" + Environment.NewLine;
                sSql += "idempresa, id_persona, fecha_pago, cg_moneda, valor," + Environment.NewLine;
                sSql += "propina, cg_empresa, id_localidad, cg_cajero, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso, estado, " + Environment.NewLine;
                sSql += "numero_replica_trigger, numero_control_replica, cambio) " + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idEmpresa"].ToString()) + ", " + iIdPersona + ", '" + sFecha + "', " + Convert.ToInt32(Application["cgMoneda"].ToString()) + "," + Environment.NewLine;
                //sSql += dbTotal + ", 0, " + Convert.ToInt32(Application["cgEmpresa"].ToString()) + "," + Environment.NewLine;
                sSql += dbTotal.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", 0, " + Convert.ToInt32(Application["cgEmpresa"].ToString()) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idLocalidad"].ToString()) + ", 7799, GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 'A' , 1, 0, 0)";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //EXTRAER ID DEL REGISTRO CV403_PAGOS
                //=========================================================================================================
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sTabla = "cv403_pagos";
                sCampo = "id_pago";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>No se pudo obtener el código de la tabla " + sTabla + ".";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                else
                {
                    iIdPago = Convert.ToInt32(iMaximo);
                }

                //EXTRAEMOS EL NUMERO_PAGO DE LA TABLA_TP_LOCALIDADES_IMPRESORAS
                sSql = "";
                sSql += "select numero_pago" + Environment.NewLine;
                sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    iNumeroPago = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                }
                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //INSERTAMOS EN LA TABLA CV403_NUMEROS_PAGOS
                sSql = "";
                sSql += "insert into cv403_numeros_pagos (" + Environment.NewLine;
                sSql += "id_pago, serie, numero_pago, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                sSql += "terminal_ingreso, estado, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += iIdPago + ", 'A', " + iNumeroPago + ", GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 'A', 1, 0)";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //INSERTAMOS EN LA TABLA CV403_DOCUMENTOS_PAGOS 
                sSql = "";
                sSql += "insert into cv403_documentos_pagos (" + Environment.NewLine;
                sSql += "id_pago, cg_tipo_documento, numero_documento, fecha_vcto, " + Environment.NewLine;
                sSql += "cg_moneda, cotizacion, valor, estado," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                sSql += "numero_replica_trigger, numero_control_replica, valor_recibido) " + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += iIdPago + ", " + iCgTipoDocumento + ", 9999, '" + sFecha + "', " + Environment.NewLine;
                //sSql += Convert.ToInt32(Application["cgMoneda"].ToString()) + ", 1, " + dbTotal + ", 'A', GETDATE()," + Environment.NewLine;
                sSql += Convert.ToInt32(Application["cgMoneda"].ToString()) + ", 1, " + dbTotal.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 1, 0, null)";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //OBTENEMOS EL MAX ID DE LA TABLA CV403_DOCUMENTOS_PAGOS
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sTabla = "cv403_documentos_pagos";
                sCampo = "id_documento_pago";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>No se pudo obtener el código de la tabla " + sTabla + ".";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                else
                {
                    iIdDocumentoPago = Convert.ToInt32(iMaximo);
                }

                //INSERTAMOS EL ÚNICO DOCUMENTO PAGADO
                sSql = "";
                sSql += "insert into cv403_documentos_pagados (" + Environment.NewLine;
                sSql += "id_documento_cobrar, id_pago, valor," + Environment.NewLine;
                sSql += "estado, numero_replica_trigger, numero_control_replica," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, fecha_pago)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                //sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dbTotal + ", 'A', 1, 0, " + Environment.NewLine;
                sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dbTotal.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", 'A', 1, 0, " + Environment.NewLine;
                sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', '" + sFecha + "')";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //ACTUALIZAR EL NUMERO DE PAGOS EN LA TABLA TP_LOCALIDADES_IMPRESORAS
                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numero_pago = numero_pago + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString());

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //INSERTAR FASE 3 - CREAR FACTURA
        private bool insertarFactura()
        {
            try
            {
                //INSTRUCCION SQL PARA EXTRAER EL NUMERO DE FACTURA
                sSql = "";
                sSql += "select numero_factura" + Environment.NewLine;
                sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    iNumeroFactura = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //QUERY PARA ACTUALIZAR EL NUMERO DE PEDIDO EN LA TABLA TP_LOCALIDADES_IMPRESORAS
                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numero_factura = numero_factura + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString());

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                string ClaveAcceso = "";

                if (Convert.ToInt32(Application["facturacion_electronica"].ToString()) == 1)
                {
                    iManejaFacturacionElectronica = 1;

                    //GENERAR CLAVE DE ACCESO
                    string Fecha = Convert.ToDateTime(sFecha).ToString("dd/MM/yyyy");
                    string FechaEmisionFormato = Fecha.Replace("/", "");
                    string TipoComprobante = "01";
                    string NumeroRuc = Application["NumeroRUC"].ToString();
                    string TipoAmbiente = Application["IDTipoAmbienteFE"].ToString();
                    string Serie = Application["Establecimiento"].ToString() + Application["PuntoEmision"].ToString();
                    string NumeroComprobante = Convert.ToString(iNumeroFactura);
                    NumeroComprobante = NumeroComprobante.PadLeft(9, '0');
                    string DigitoVerificador = "";

                    string CodigoNumerico = "12345678";
                    string TipoEmision = Application["IDTipoEmisionFE"].ToString();

                    if (TipoEmision == "1")
                    {
                        ClaveAcceso = ClaveAcceso + FechaEmisionFormato + TipoComprobante + NumeroRuc + TipoAmbiente;
                        ClaveAcceso = ClaveAcceso + Serie + NumeroComprobante + CodigoNumerico + TipoEmision;
                    }
                    DigitoVerificador = sDigitoVerificarModulo11(ClaveAcceso);
                    ClaveAcceso = ClaveAcceso + DigitoVerificador;
                    //FIN CALVE ACCESO
                }

                else
                {
                    iManejaFacturacionElectronica = 0;
                }

                //INSTRUCCION PARA EXTRAER LA FORMA DE PAGO DE LA TABLA CV403_FORMAS_PAGOS
                sSql = "";
                sSql += "select id_forma_pago" + Environment.NewLine;
                sSql += "from cv403_formas_pagos" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "and codigo = 'EF'";

                dtConsulta = new DataTable();
                dtConsulta.Clear(); ;

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    iIdFormaPagoFactura = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV403_FACTURAS
                sSql = "";
                sSql += "insert into cv403_facturas (idempresa, id_persona, cg_empresa, idtipocomprobante," + Environment.NewLine;
                sSql += "id_localidad, idformulariossri, id_vendedor, id_forma_pago, fecha_factura, fecha_vcto," + Environment.NewLine;
                sSql += "cg_moneda, valor, cg_estado_factura, editable, fecha_ingreso, usuario_ingreso, " + Environment.NewLine;
                sSql += "terminal_ingreso, estado, numero_replica_trigger, numero_control_replica, " + Environment.NewLine;
                sSql += "Direccion_Factura,Telefono_Factura,Ciudad_Factura, correo_electronico, tasa_usuario," + Environment.NewLine;
                sSql += "facturaelectronica, clave_acceso, emite_tasa_usuario, ambiente_tasa_usuario," + Environment.NewLine;
                sSql += "cantidad_tasa_emitida, id_tipo_ambiente, id_tipo_emision)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idEmpresa"].ToString()) + ", " + iIdPersona + ", " + Convert.ToInt32(Application["cgEmpresa"].ToString()) + ", 1," + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idLocalidad"].ToString()) + ", 19, " + Convert.ToInt32(Session["idVendedor"].ToString()) + ", " + iIdFormaPagoFactura + ", '" + sFecha + "'," + Environment.NewLine;
                //sSql += "'" + sFecha + "', " + Convert.ToInt32(Application["cgMoneda"].ToString()) + ", " + dbTotal + ", 0, 0, GETDATE()," + Environment.NewLine;
                sSql += "'" + sFecha + "', " + Convert.ToInt32(Application["cgMoneda"].ToString()) + ", " + dbTotal.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", 0, 0, GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A', 1, 0," + Environment.NewLine;
                sSql += "'" + sDireccion + "', '" + sTelefono + "', '" + sCiudad + "'," + Environment.NewLine;
                sSql += "'" + sCorreoElectronico + "', '', " + iManejaFacturacionElectronica + ", '" + ClaveAcceso + "', 0, 0, ";
                sSql += "0, " + Convert.ToInt32(Application["IDTipoAmbienteFE"].ToString()) + ", " + Environment.NewLine;
                sSql += Convert.ToInt32(Application["IDTipoEmisionFE"].ToString()) + ")";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //EXTRAER ID DEL REGISTRO CV403_FACTURAS
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sTabla = "cv403_facturas";
                sCampo = "id_factura";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>No se pudo obtener el código de la tabla " + sTabla + ".";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                else
                {
                    iIdFactura = Convert.ToInt32(iMaximo);
                }

                //INSERTAR EN LA TABLA CV403_NUMEROS_FACTURAS
                sSql = "";
                sSql += "insert into cv403_numeros_facturas (id_factura, idtipocomprobante, numero_factura, " + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, estado, numero_replica_trigger, " + Environment.NewLine;
                sSql += "numero_control_replica) " + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdFactura + ", 1, " + iNumeroFactura + ", GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A', 1, 0 )";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //ACTUALIZAMOS LA TABLA CV403_DCTOS_POR_COBRAR
                sSql = "";
                sSql += "update cv403_dctos_por_cobrar set" + Environment.NewLine;
                sSql += "id_factura = " + iIdFactura + "," + Environment.NewLine;
                sSql += "cg_estado_dcto = " + iCgEstadoDctoPorCobrar + "," + Environment.NewLine;
                sSql += "numero_documento = " + iNumeroFactura + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido;

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV403_FACTURAS_PEDIDOS
                sSql = "";
                sSql += "insert into cv403_facturas_pedidos (" + Environment.NewLine;
                sSql += "id_factura, id_pedido, fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                sSql += "estado, numero_replica_trigger, numero_control_replica) " + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdFactura + ", " + iIdPedido + ", GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A', 1, 0 )";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal1').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA CONSULTAR LOS DATOS DEL CLIENTE PARA LA FACTURA
        private bool consultarClienteFacturar()
        {
            try
            {
                //INSTRUCCION SQL PARA EXTRAER DATOS DEL CLIENTE A FACTURAR
                sSql = "";
                sSql += "select isnull(TD.direccion, '') direccion, isnull(TD.calle_principal, '') calle_principal," + Environment.NewLine;
                sSql += "isnull(TD.numero_vivienda, '') numero_vivienda, isnull(TD.calle_interseccion, '') calle_interseccion," + Environment.NewLine;
                sSql += "isnull(TD.referencia, '') referencia, isnull(TP.codigo_alterno, '') codigo_alterno," + Environment.NewLine;
                sSql += "isnull(isnull(TT.domicilio, TT.celular), '') telefono, isnull(TP.correo_electronico, '') correo_electronico," + Environment.NewLine;
                sSql += "ltrim(isnull(TP.nombres, '') + ' ' + TP.apellidos) cliente_factura, TP.identificacion, TP.cg_tipo_identificacion" + Environment.NewLine;
                sSql += "from tp_personas TP LEFT OUTER JOIN" + Environment.NewLine;
                sSql += "tp_direcciones TD ON TD.id_persona = TP.id_persona" + Environment.NewLine;
                sSql += "and TP.estado = 'A'" + Environment.NewLine;
                sSql += "and TD.estado = 'A' LEFT OUTER JOIN" + Environment.NewLine;
                sSql += "tp_telefonos TT ON TT.id_persona = TP.id_persona" + Environment.NewLine;
                sSql += "and TT.estado = 'A'" + Environment.NewLine;
                sSql += "where TP.id_persona = " + iIdPersona;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                sCiudad = dtConsulta.Rows[0][0].ToString();
                sDireccion = dtConsulta.Rows[0][1].ToString();

                if (dtConsulta.Rows[0][2].ToString() != "")
                {
                    sDireccion += " " + dtConsulta.Rows[0][2].ToString();
                }

                if (dtConsulta.Rows[0][3].ToString() != "")
                {
                    sDireccion += " Y " + dtConsulta.Rows[0][3].ToString();
                }

                if (sDireccion.Trim().Length > 100)
                {
                    sDireccion = sDireccion.Trim().Substring(0, 100);
                }

                if (dtConsulta.Rows[0][6].ToString() != "")
                {
                    sTelefono = dtConsulta.Rows[0][6].ToString();
                }

                else
                {
                    sTelefono = dtConsulta.Rows[0][5].ToString();
                }

                sCorreoElectronico = dtConsulta.Rows[0][7].ToString();
                sClienteFactura = dtConsulta.Rows[0][8].ToString();
                sIdentificacionFactura = dtConsulta.Rows[0][9].ToString();

                if (Convert.ToInt32(dtConsulta.Rows[0]["cg_tipo_identificacion"].ToString()) == 180)
                {
                    iExtranjero_A = 1;
                }

                else
                {
                    iExtranjero_A = 0;
                }

                return true;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal1').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA OBTENER LOS IDENTIFICADORES DE TARIFA
        private bool obtenerIdentificadoresSmart()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tipo_cliente, identificador_smartt" + Environment.NewLine;
                sSql += "from ctt_tipo_cliente" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtTarifasSmartt = new DataTable();
                dtTarifasSmartt.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtTarifasSmartt);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal1').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA EL DIGITO VERIFICADOR MODULO 11
        private string sDigitoVerificarModulo11(string sClaveAcceso)
        {
            Int32 suma = 0;
            int inicio = 7;

            for (int i = 0; i < sClaveAcceso.Length; i++)
            {
                suma = suma + Convert.ToInt32(sClaveAcceso.Substring(i, 1)) * inicio;
                inicio--;
                if (inicio == 1)
                    inicio = 7;
            }

            Decimal modulo = suma % 11;
            suma = 11 - Convert.ToInt32(modulo);

            if (suma == 11)
            {
                suma = 0;
            }
            else if (suma == 10)
            {
                suma = 1;
            }
            //sClaveAcceso = sClaveAcceso + Convert.ToString(suma);

            return suma.ToString();
        }

        //FUNCION PARA BUSCAR EL CLIENTE CON NUMERO DE IDENTIFICACION
        private void buscarCliente(string sIdentificacion)
        {
            try
            {
                sSql = "";
                sSql += "select id_persona, identificacion, ltrim(isnull(nombres, '') + ' ' + apellidos) pasajero," + Environment.NewLine;
                sSql += "codigo_alterno, correo_electronico, isnull(fecha_nacimiento, GETDATE()) fecha_nacimiento," + Environment.NewLine;
                sSql += "cg_tipo_identificacion, isnull(discapacidad, 0) discapacidad, cg_tipo_persona," + Environment.NewLine;
                sSql += "nombres, apellidos" + Environment.NewLine;
                sSql += "from tp_personas" + Environment.NewLine;
                sSql += "where identificacion = '" + sIdentificacion + "'" + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        txtIdentificacion.Text = sIdentificacion;
                        txtNombrePasajero.Text = dtConsulta.Rows[0]["pasajero"].ToString();
                        Session["idPasajero"] = dtConsulta.Rows[0]["id_persona"].ToString();
                        Session["identificacion"] = dtConsulta.Rows[0]["identificacion"].ToString();
                        cmbTipoIdentificacion.SelectedValue = dtConsulta.Rows[0]["cg_tipo_identificacion"].ToString();
                        iDiscapacidad = Convert.ToInt32(dtConsulta.Rows[0]["discapacidad"].ToString());

                        Session["fechaNacimiento"] = dtConsulta.Rows[0]["fecha_nacimiento"].ToString();
                        Session["discapacidad"] = dtConsulta.Rows[0]["discapacidad"].ToString();
                        Session["cgTipoIdentificacion"] = dtConsulta.Rows[0]["cg_tipo_identificacion"].ToString();
                        Session["cgTipoPersona"] = dtConsulta.Rows[0]["cg_tipo_persona"].ToString();
                        Session["nombrePersona"] = dtConsulta.Rows[0]["nombres"].ToString();
                        Session["apellidoPersona"] = dtConsulta.Rows[0]["apellidos"].ToString();

                        string sFechaAyuda = Convert.ToDateTime(dtConsulta.Rows[0][5].ToString()).ToString("yyyy/MM/dd");
                        DateTime nacimiento = Convert.ToDateTime(sFechaAyuda);

                        //DateTime nacimiento = Convert.ToDateTime(dtConsulta.Rows[0][5].ToString()).ToString("yyyy/MM/dd");
                        int edad = calcularEdad(nacimiento, DateTime.Now);

                        if (sDescripcionMes == "")
                        {
                            lblEdad.Text = edad.ToString() + " AÑOS";
                        }

                        else
                        {
                            lblEdad.Text = edad.ToString() + " AÑOS, " + meses.ToString() + " MESES";
                        }

                        if (iDiscapacidad == 0)
                        {
                            tipoCliente(edad);
                        }

                        else
                        {
                            tipoClienteDiscapacidad();
                        }
                    }

                    else
                    {
                        int iRespuestaBase = 0;

                        //CONSULTAR BASE DE DATOS CLIENTES
                        if (Convert.ToInt32(Application["base_clientes"].ToString()) == 1)
                        {
                            iRespuestaBase = consultarClienteBase(sIdentificacion);
                        }

                        if (iRespuestaBase == -1)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo obtener información de la base de datos del cliente.', 'danger');", true);
                            return;
                        }

                        if (iRespuestaBase == 0)
                        {
                            //CONSULTAR BASE DE DATOS REGISTRO CIVIL
                            if (Convert.ToInt32(Application["registro_civil"].ToString()) == 1)
                            {

                            }

                            else
                            {
                                //AQUI ABRIR MODAL PARA CREAR NUEVO PASAJERO                        
                                ModalPopupExtenderCrearEditar.Show();
                                lblAlerta.Text = "";
                                txtIdentificacionRegistro.Text = txtIdentificacion.Text.Trim();
                            }
                        }
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
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CALCULAR LA EDAD
        private int calcularEdad(DateTime nacimiento, DateTime ahora)
        {
            try
            {
                int edad = ahora.Year - nacimiento.Year;
                meses = ahora.Month - nacimiento.Month;

                if (ahora.Month < nacimiento.Month || (ahora.Month == nacimiento.Month && ahora.Day < nacimiento.Day))
                {
                    edad--;
                }

                if (meses < 0)
                {
                    meses = meses * (-1);
                }

                //VERIFICAR EL MES PARA GUARDAR EN UNA VARIABLE 

                if (meses == 1)
                {
                    sDescripcionMes = "Enero";
                }

                else if (meses == 2)
                {
                    sDescripcionMes = "Febrero";
                }

                else if (meses == 3)
                {
                    sDescripcionMes = "Marzo";
                }

                else if (meses == 4)
                {
                    sDescripcionMes = "Abril";
                }

                else if (meses == 5)
                {
                    sDescripcionMes = "Mayo";
                }

                else if (meses == 6)
                {
                    sDescripcionMes = "Junio";
                }

                else if (meses == 7)
                {
                    sDescripcionMes = "Julio";
                }

                else if (meses == 8)
                {
                    sDescripcionMes = "Agosto";
                }

                else if (meses == 9)
                {
                    sDescripcionMes = "Septiembre";
                }

                else if (meses == 10)
                {
                    sDescripcionMes = "Octubre";
                }

                else if (meses == 11)
                {
                    sDescripcionMes = "Noviembre";
                }

                else if (meses == 12)
                {
                    sDescripcionMes = "Diciembre";
                }

                else if (meses == 0)
                {
                    sDescripcionMes = "";
                }

                //CAMBIAR FONDO DE LABEL
                if (edad > 65)
                {
                    //lblEdad.Attributes.Add("Class", "badge btn-block bg-fuchsia");
                    lblEdad.ForeColor = Color.White;
                    lblEdad.BackColor = Color.Fuchsia;
                }

                else if (edad > 16)
                {
                    //lblEdad.Attributes.Add("Class", "badge btn-block bg-lime");
                    lblEdad.ForeColor = Color.Black;
                    lblEdad.BackColor = Color.Lime;
                }

                else
                {
                    //lblEdad.Attributes.Add("Class", "badge btn-block bg-red");
                    lblEdad.ForeColor = Color.White;
                    lblEdad.BackColor = Color.Red;
                }


                //FIN MESES

                return edad;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return -1;
            }
        }

        //FUNCION PARA BUSCAR EL TIPO DE CLIENTE DISCAPACIDAD
        private void tipoClienteDiscapacidad()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tipo_cliente, factor" + Environment.NewLine;
                sSql += "from ctt_tipo_cliente" + Environment.NewLine;
                sSql += "where discapacidad = 1" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        cmbTipoCliente.SelectedValue = dtConsulta.Rows[0][0].ToString();
                        Session["factorDescuento"] = dtConsulta.Rows[0][1].ToString();
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
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA BUSCAR EL TIPO DE CLIENTE
        private void tipoCliente(int iEdad_P)
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tipo_cliente, factor" + Environment.NewLine;
                sSql += "from ctt_tipo_cliente" + Environment.NewLine;
                sSql += "where (edad_minima <= " + iEdad_P + Environment.NewLine;
                sSql += "and edad_maxima >= " + iEdad_P + ")" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        cmbTipoCliente.SelectedValue = dtConsulta.Rows[0][0].ToString();
                        Session["idPersonaTipoCliente"] = dtConsulta.Rows[0][0].ToString();
                        Session["factorDescuento"] = dtConsulta.Rows[0][1].ToString();

                        indice = cmbDestino.SelectedIndex;

                        if (consultarListaPrecioTipoCliente(Convert.ToInt32(cmbTipoCliente.SelectedValue)) == true)
                        {
                            if (indice == 0)
                            {
                                txtPrecio.Text = "0.00";
                                txtDescuento.Text = "0.00";
                                txtPrecioFinal.Text = "0.00";
                            }

                            else
                            {
                                cmbDestino.SelectedIndex = indice;
                                consultarPrecio();
                            }
                        }

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
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONSULTAR SI EXISTE EL REGISTRO
        private void contarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select id_persona, cg_tipo_persona, cg_tipo_identificacion," + Environment.NewLine;
                sSql += "identificacion, apellidos, isnull(nombres, '') nombres," + Environment.NewLine;
                sSql += "isnull(fecha_nacimiento, getdate()) fecha_nacimiento," + Environment.NewLine;
                sSql += "isnull(discapacidad, 0) discapacidad" + Environment.NewLine;
                sSql += "from tp_personas" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and identificacion = '" + txtIdentificacionRegistro.Text.Trim() + "'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["idPersonaConsulta"] = dtConsulta.Rows[0][0].ToString();
                        Session["cgTipoPersona"] = dtConsulta.Rows[0][1].ToString();
                        cmbIdentificacion.SelectedValue = dtConsulta.Rows[0][2].ToString();
                        txtRazonSocial.Text = dtConsulta.Rows[0][4].ToString().ToUpper();
                        txtNombreRegistro.Text = dtConsulta.Rows[0][5].ToString();
                        txtFechaNacimiento.Text = Convert.ToDateTime(dtConsulta.Rows[0][6].ToString()).ToString("dd/MM/yyyy");
                    }

                    else
                    {
                        Session["idPersonaConsulta"] = null;

                        int iTercerDigito = Convert.ToInt32(txtIdentificacionRegistro.Text.Trim().Substring(0, 2));

                        if ((iTercerDigito == 9) || (iTercerDigito == 6))
                        {
                            Session["cgTipoPersona"] = "2448";
                        }

                        else
                        {
                            Session["cgTipoPersona"] = "2447";
                        }

                        txtRazonSocial.Focus();
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
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA EXTRAER EL PRECIO DEL PASAJE
        private void consultarPrecio()
        {
            try
            {
                if (Convert.ToInt32(cmbDestino.SelectedValue) == 0)
                {
                    txtPrecio.Text = "0.00";
                    txtDescuento.Text = "0.00";
                    txtPrecioFinal.Text = "0.00";
                    return;
                }

                else
                {
                    string[] sSeparar = cmbDestino.SelectedItem.ToString().Split('$');
                    //dbPrecio = Convert.ToDouble(sSeparar[1].Trim());
                    dbPrecio = Convert.ToDouble(sSeparar[1].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                    Session["idProducto"] = cmbDestino.SelectedValue;
                    string[] sSeparar_1 = sSeparar[0].Trim().Split('-');
                    Session["str_Producto"] = sSeparar_1[0].Trim();

                    dbDescuento = 0;

                    txtPrecio.Text = dbPrecio.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    txtDescuento.Text = dbDescuento.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    txtPrecioFinal.Text = (dbPrecio - dbDescuento).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }

                sSql = "";
                sSql += "select id_ctt_pueblo_destino" + Environment.NewLine;
                sSql += "from cv401_productos" + Environment.NewLine;
                sSql += "where id_producto = " + Convert.ToInt32(cmbDestino.SelectedValue) + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["idPueblo"] = dtConsulta.Rows[0][0].ToString();
                    }

                    else
                    {
                        lblMensajeError.Text = "No ha registros en el sistema. Favor comuníquese con el administrador del sistema.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal1').modal('show');</script>", false);
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
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA MANIPULACION DEL TIPO DE CLIENTE
        private void manejarTipoClienteManual(int iId_P)
        {
            try
            {
                sSql = "";
                sSql += "select factor, isnull(id_persona, 0) id_persona" + Environment.NewLine;
                sSql += "from ctt_tipo_cliente" + Environment.NewLine;
                sSql += "where id_ctt_tipo_cliente = " + iId_P + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["factorDescuento"] = dtConsulta.Rows[0][0].ToString();
                        Session["idPersonaTipoCliente"] = dtConsulta.Rows[0][1].ToString();
                        consultarPrecio();
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
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCIONES PARA CONSULTAR EN LA BASE DE DATOS DE CLIENTES

        //FUNCION PARA CONSULTAR DATOS DE CLIENTES EN OTRA BASE
        private int consultarClienteBase(string sCedula_P)
        {
            try
            {
                string sApellidos_P;
                string sNombres_P;
                string sFechaNacimiento_P;

                sSql = "";
                sSql += "select txNombres, fcNacimiento" + Environment.NewLine;
                sSql += "from cliente" + Environment.NewLine;
                sSql += "where txIdentificacion = '" + sCedula_P + "'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistroClientes(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        string[] sSeparar = dtConsulta.Rows[0]["txNombres"].ToString().Trim().Split(' ');

                        if (sSeparar.Length >= 4)
                        {
                            sApellidos_P = sSeparar[0].Trim().ToUpper() + " " + sSeparar[1].Trim().ToUpper();
                            sNombres_P = "";

                            for (int i = 2; i < sSeparar.Length; i++)
                            {
                                sNombres_P += sSeparar[i].Trim().ToUpper() + " ";
                            }
                        }

                        else if (sSeparar.Length == 3)
                        {
                            sApellidos_P = sSeparar[0].Trim().ToUpper() + " " + sSeparar[1].Trim().ToUpper();
                            sNombres_P = sSeparar[2].Trim().ToUpper();
                        }

                        else if (sSeparar.Length == 2)
                        {
                            sApellidos_P = sSeparar[0].Trim().ToUpper();
                            sNombres_P = sSeparar[1].Trim().ToUpper();
                        }

                        else
                        {
                            sApellidos_P = sSeparar[0].Trim().ToUpper();
                            sNombres_P = "";
                        }

                        sFechaNacimiento_P = Convert.ToDateTime(dtConsulta.Rows[0]["fcNacimiento"].ToString()).ToString("yyyy/MM/dd");

                        if (insertarClienteBase(sCedula_P, sNombres_P, sApellidos_P, sFechaNacimiento_P) == true)
                        {
                            return 1;
                        }

                        else
                        {
                            return -1;
                        }
                    }

                    else
                    {
                        return 0;
                    }
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
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return -1;
            }
        }

        //FUNCION PARA INSERTAR EN LAS BASES DE DATOS
        private bool insertarClienteBase(string sCedula_P, string sNombres_P, string sApellidos_P, string sFechaNacimiento_P)
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    lblMensajeError.Text = "No se pudo iniciar la transacción para guardar los registros.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal1').modal('show');</script>", false);
                    return false;
                }

                int iTipoPersona_P = 2447;
                int iTipoIdentificacion_P = 178;

                sSql = "";
                sSql += "insert into tp_personas (" + Environment.NewLine;
                sSql += "identificacion, apellidos, nombres, fecha_nacimiento, " + Environment.NewLine;
                sSql += "idempresa, cg_tipo_persona, cg_tipo_identificacion," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "'" + sCedula_P + "', '" + sApellidos_P.Trim().ToUpper() + "', ' " + sNombres_P.Trim().ToUpper() + "', '" + sFechaNacimiento_P + "'," + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idEmpresa"].ToString()) + ", " + iTipoPersona_P + ", " + iTipoIdentificacion_P + "," + Environment.NewLine;
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    conexionM.reversaTransaccion();
                    return false;
                }

                //OBTENER EL ID DE LA TABLA TP_PERSONAS
                sTabla = "tp_personas";
                sCampo = "id_persona";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>No se pudo obtener el código de la tabla " + sTabla + ".";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                else
                {
                    Session["idPasajero"] = Convert.ToInt32(iMaximo);
                }

                conexionM.terminaTransaccion();

                Session["identificacion"] = sCedula_P;
                txtNombrePasajero.Text = sApellidos_P + " " + sNombres_P;
                buscarCliente(sCedula_P);
                return true;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        #endregion

        #region FUNCIONES PARA CERRAR EL VIAJE

        //FUNCION PARA ACTUALIZAR EL ESTADO DEL VIAJE
        private void cerrarViaje()
        {
            try
            {
                sSql = "";
                sSql += "select P.id_producto, P.aplica_retencion_ticket," + Environment.NewLine;
                sSql += "P.porcentaje_retencion_ticket, P.paga_iva, NP.nombre," + Environment.NewLine;
                sSql += "ltrim(str(PP.valor, 10, 2)) valor," + Environment.NewLine;
                sSql += "P.aplica_pago_administracion" + Environment.NewLine;
                sSql += "from cv401_productos P, cv401_productos PADRE," + Environment.NewLine;
                sSql += "cv401_nombre_productos NP, cv403_precios_productos PP" + Environment.NewLine;
                sSql += "where P.id_producto_padre = PADRE.id_producto" + Environment.NewLine;
                sSql += "and NP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and PP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and PADRE.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "and PP.estado = 'A'" + Environment.NewLine;
                //sSql += "and PADRE.id_ctt_pueblo_origen = " + Convert.ToInt32(Application["id_pueblo"].ToString()) + Environment.NewLine;
                sSql += "and PADRE.cobros_tickets = 1" + Environment.NewLine;
                sSql += "and PP.id_lista_precio = " + Convert.ToInt32(Session["lista_minorista"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {

                        columnasGridPendiente(true);
                        string sEstadoPendiente;
                        Decimal dbValorSaldo_P;
                        Decimal dbValorAbono_P;
                        int iCgEstadoDcto;
                        string[,] sIdPedido = new string[dgvDetalle.Rows.Count, 3];
                        int i = 0;

                        foreach (GridViewRow row in dgvDetalle.Rows)
                        {
                            sEstadoPendiente = row.Cells[2].Text.Trim().ToUpper();
                            CheckBox check = row.FindControl("chkSeleccionar") as CheckBox;

                            if (check.Checked == true)
                            {
                                if (sEstadoPendiente == "PAGO PENDIENTE")
                                {
                                    Label lblAbono_G = row.FindControl("lblAbonoGrid") as Label;
                                    Label lblSaldo_G = row.FindControl("lblSaldoGrid") as Label;

                                    dbValorSaldo_P = Convert.ToDecimal(lblSaldo_G.Text.Trim());
                                    dbValorAbono_P = Convert.ToDecimal(lblAbono_G.Text.Trim());

                                    if (dbValorSaldo_P == 0)
                                    {
                                        iCgEstadoDcto = 7461;
                                    }

                                    else
                                    {
                                        iCgEstadoDcto = 7462;
                                    }

                                    sIdPedido[i, 0] = row.Cells[1].Text;
                                    sIdPedido[i, 1] = dbValorAbono_P.ToString("N2");
                                    sIdPedido[i, 2] = iCgEstadoDcto.ToString();
                                    i++;
                                }
                            }
                        }

                        columnasGridPendiente(false);

                        if (txtPagosPendientesModal.Text.Trim() == "")
                        {
                            txtPagosPendientesModal.Text = "0.00";
                        }

                        if (txtEfectivoModal.Text.Trim() == "")
                        {
                            txtEfectivoModal.Text = "0.00";
                        }

                        bRespuesta = cierreViajeInstrucciones.iniciarCierre(dtConsulta, Convert.ToDouble(txtTotalCobradoModal.Text.Trim()), Convert.ToDouble(txtPagoRetencionModal.Text.Trim()),
                                     Convert.ToDouble(txtPagoModal.Text.Trim()), Convert.ToInt32(Session["idProgramacion"].ToString()),
                                     DateTime.Now.ToString("yyyy/MM/dd"), sDatosMaximo, sIdPedido, i, Convert.ToInt32(Session["extra"].ToString()),
                                     Convert.ToDecimal(txtPagosPendientesModal.Text.Trim()), Convert.ToDecimal(txtEfectivoModal.Text.Trim()), txtObservacionProgramacion.Text.Trim(),
                                     Convert.ToInt32(Session["cobrar_administracion_boletos"].ToString()));

                        if (bRespuesta == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo realizar el cierre del viaje.', 'error');", true);
                            goto fin;
                        }
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No existen productos parametrizados para cerrar el viaje.', 'warning');", true);
                        goto fin;
                    }
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto fin;
                }

                //sImprimir = "";
                //sImprimir = cerrarViajeReporte.llenarReporte(Convert.ToInt32(Session["idProgramacion"]), Session["usuario"].ToString());

                //consultarImpresora();

                Clases.ClaseImprimirManifiesto manifiesto = new Clases.ClaseImprimirManifiesto();
                manifiesto.llenarReporte(Convert.ToInt32(Session["idProgramacion"]), Session["usuario"].ToString(), 1, Convert.ToDecimal(txtPagosPendientesModal.Text.Trim()), Convert.ToDecimal(txtEfectivoModal.Text.Trim()), 1);

                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El proceso de cierre del viaje se ha completado con éxito.', 'success');", true);

                Session["idProgramacion"] = null;
                Session["dtClientes"] = null;
                Session["idVehiculo"] = null;
                Session["idPasajero"] = null;
                Session["idAsiento"] = null;

                sFecha = DateTime.Now.ToString("yyyy/MM/dd");
                llenarGrid(sFecha);
                llenarGridExtras(sFecha);

                pnlGrid.Visible = true;
                pnlBus.Visible = false;
                pnlCierreViaje.Visible = false;

                goto fin;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

        fin: { }
        }

        //FUNCION PARA EXTRAER EL VALOR TOTAL COBRADO EN LA RUTA
        private void extraerTotalCobrado()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(ltrim(str(sum(cantidad * (precio_unitario - valor_dscto + valor_iva)), 10, 2)), '0.00') suma" + Environment.NewLine;
                sSql += "from cv403_det_pedidos DP, cv403_cab_pedidos CP" + Environment.NewLine;
                sSql += "where DP.id_pedido = CP.id_pedido" + Environment.NewLine;
                sSql += "and DP.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.id_ctt_programacion = " + Convert.ToInt32(Session["idProgramacion"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        txtTotalCobradoModal.Text = dtConsulta.Rows[0][0].ToString();
                        lblTotalCobradoBus.Text = "COBRADO: $ " + dtConsulta.Rows[0][0].ToString();
                    }

                    else
                    {
                        lblMensajeError.Text = "No se pudo obtener el valor total cobrado.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal1').modal('show');</script>", false);
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
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //EXTRAER HORA EN CASO DE DARSE EL CASO
        private void consultarHoraCambioNormal(string sRuta_P)
        {
            try
            {
                sSql = "";
                sSql += "select isnull(hora_reemplazo_extra, 'SN') hora_reemplazo" + Environment.NewLine;
                sSql += "from ctt_programacion" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + Convert.ToInt32(Session["idProgramacion"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    cerrarModal();
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                Session["hora_modificar"] = dtConsulta.Rows[0][0].ToString().Trim();

                if (dtConsulta.Rows[0][0].ToString() != "SN")
                {
                    lblDetalleBus1.Text = sRuta_P + " - " + Convert.ToDateTime(dtConsulta.Rows[0][0].ToString()).ToString("HH:mm"); ;
                }

            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION DE RECALCULO DE VALORES VIAJE NORMAL
        private void recalcularValoresNormales()
        {
            try
            {
                extraerTotalCobrado();

                double dbTotalCobrado_P;
                double dbPorcentajeRetencion_P;
                double dbValorRetencion_P;
                double dbPrimerSubtotal_P;

                dbTotalCobrado_P = Convert.ToDouble(txtTotalCobradoModal.Text.Trim());
                dbPorcentajeRetencion_P = Convert.ToDouble(Session["porcentaje_retencion"].ToString()) / 100;
                dbValorRetencion_P = dbTotalCobrado_P * dbPorcentajeRetencion_P;
                txtPagoRetencionModal.Text = dbValorRetencion_P.ToString("N2");
                dbValorRetencion_P = Convert.ToDouble(txtPagoRetencionModal.Text.Trim());
                dbPrimerSubtotal_P = dbTotalCobrado_P - dbValorRetencion_P;
                txtPrimerTotalModal.Text = dbPrimerSubtotal_P.ToString("N2");
                txtPagoModal.Text = "0.00";
                txtSegundoTotalModal.Text = dbPrimerSubtotal_P.ToString("N2");
                txtTotalNetoModal.Text = dbPrimerSubtotal_P.ToString("N2");

                //rdbPendiente.Checked = true;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }        

        //PINTAR EL GRID DE VENDIDOS
        private void pintarGridPendiente()
        {
            for (int i = 0; i < dgvVendidos.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvVendidos.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvVendidos.Rows[i].BackColor = Color.White;
                }
            }
        }

        //FUNCION PARA CONSULTAR SI EL TERIMNAL PUEDE REALIZAR COBROS
        private bool consultarRealizarCobros()
        {
            try
            {
                sSql = "";
                sSql += "select cobros_administracion, cobros_otros" + Environment.NewLine;
                sSql += "from ctt_pueblos" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(Session["id_pueblo"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count == 1)
                    {
                        Session["cobros_administracion"] = dtConsulta.Rows[0][0].ToString();
                        Session["cobros_otros"] = dtConsulta.Rows[0][1].ToString();
                        return true;
                    }

                    else
                    {
                        cerrarModal();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No existen parámetros para la consulta de permisos de cobros. COmuníquese con el administrador', 'danger');", true);
                        return false;
                    }
                }

                else
                {
                    cerrarModal();
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        #endregion

        #region FUNCIONES PARA EXTRAER PAGOS PENDIENTES

        //FUNCION PARA COLUMNAS DEL GRID DE LOS PAGOS PENDIENTES
        private void columnasGridPendiente(bool ok)
        {
            dgvDetalle.Columns[1].Visible = ok;
            dgvDetalle.Columns[8].Visible = ok;
            dgvDetalle.Columns[14].Visible = ok;
            dgvDetalle.Columns[9].ItemStyle.Width = 100;
        }

        //FUNCION PARA LLENAR EL GRID DE PAGOS PENDIENTES
        private void llenarGridPendientes()
        {
            try
            {
                sSql = "";
                sSql += "select id_pedido, numero_viaje," + Environment.NewLine;
                sSql += "convert(varchar(10), fecha_viaje, 103) fecha_viaje," + Environment.NewLine;
                sSql += "convert(varchar(5),  hora_salida, 108) hora_salida," + Environment.NewLine;
                sSql += "abono, precio, cg_estado_dcto, 'PAGO PENDIENTE' estado_pago" + Environment.NewLine;
                sSql += "from ctt_vw_pagos_pendientes_itinerario" + Environment.NewLine;
                sSql += "where id_ctt_vehiculo = " + Convert.ToInt32(Session["idVehiculo"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (Convert.ToInt32(Session["extra"].ToString()) == 0)
                    {
                        if (Convert.ToInt32(Session["cobrar_administracion_boletos"]) == 1)
                        {
                            dtConsulta.Rows.Add("0", Session["numero_viaje_cierre"].ToString(), Session["fecha_viaje_cierre"].ToString(),
                                                Session["hora_viaje_cierre"].ToString(), "0.00", Session["pago_administracion"].ToString(), "7460", "PAGO ACTUAL");
                        }
                    }

                    columnasGridPendiente(true);
                    dgvDetalle.DataSource = dtConsulta;
                    dgvDetalle.DataBind();

                    Decimal dbSumaValores = 0;

                    for (int i = 0; i < dgvDetalle.Rows.Count; i++)
                    {
                        dbSumaValores += Convert.ToDecimal(dgvDetalle.Rows[i].Cells[7].Text);
                    }

                    lblSumaRecuperado.Text = "Total Recuperado: " + dbSumaValores.ToString("N2") + " $";

                    columnasGridPendiente(false);
                }

                else
                {
                    cerrarModal();
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCIONES PARA CAMBIAR LA HORA

        private void actualizarHora()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    lblMensajeError.Text = "No se pudo iniciar la transacción para guardar los registros.";
                    cerrarModal();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal1').modal('show');</script>", false);
                    return;
                }

                sSql = "";
                sSql += "update ctt_programacion set" + Environment.NewLine;
                sSql += "hora_reemplazo_extra = '" + txtNuevaHoraSalida.Text.Trim() + "'" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + Convert.ToInt32(Session["idProgramacion"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    conexionM.reversaTransaccion();
                    cerrarModal();
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                conexionM.terminaTransaccion();
                consultarHoraCambioNormal(Convert.ToDateTime(Session["fecha_viaje_cierre"].ToString()).ToString("yyyy/MM/dd"));
                cerrarModal();

                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Hora actualizada éxitosamente.', 'success');", true);

                lblDetalleBus1.Text = Session["destino_viaje_etiqueta"].ToString() + " - " + txtNuevaHoraSalida.Text.Trim();

                return;
            }

            catch (Exception ex)
            {
                conexionM.reversaTransaccion();
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        private void anularHora()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    lblMensajeError.Text = "No se pudo iniciar la transacción para guardar los registros.";
                    cerrarModal();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal1').modal('show');</script>", false);
                    return;
                }

                sSql = "";
                sSql += "update ctt_programacion set" + Environment.NewLine;
                sSql += "hora_reemplazo_extra = null" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + Convert.ToInt32(Session["idProgramacion"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    conexionM.reversaTransaccion();
                    cerrarModal();
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                conexionM.terminaTransaccion();
                consultarHoraCambioNormal(Convert.ToDateTime(Session["fecha_viaje_cierre"].ToString()).ToString("yyyy/MM/dd"));
                cerrarModal();

                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Hora removida éxitosamente.', 'success');", true);

                lblDetalleBus1.Text = Session["destino_viaje_etiqueta"].ToString() + " - " + Session["hora_viaje_etiqueta"].ToString();
                return;
            }

            catch (Exception ex)
            {
                conexionM.reversaTransaccion();
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCIONES PARA EL MODAL DE VIAJES ACTIVOS

        //FUNCION PARA LLENAR EL GRID CON LOS VIAJES ACTIVOS
        private void llenarGridViajesActivos()
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_itinerarios" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(cmbFiltrarGrid.SelectedValue) + Environment.NewLine;
                sSql += "and fecha_viaje = '" + Convert.ToDateTime(txtFechaViajesActivos.Text.Trim()).ToString("yyyy/MM/dd") + "'" + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "order by hora_salida";

                columnasGridViajesActivos(true);
                asignarE.ISSQL = sSql;
                dgvViajesActivos.DataSource = asignarM.listarViajes(asignarE);
                dgvViajesActivos.DataBind();
                columnasGridViajesActivos(false);

                ModalPopUpSeleccionViajes.Show();
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LAS COLUMNAS
        //FUNCION PARA MANIPULACION DE COLUMNAS
        private void columnasGridViajesActivos(bool ok)
        {
            dgvViajesActivos.Columns[0].ItemStyle.Width = 75;
            dgvViajesActivos.Columns[2].ItemStyle.Width = 75;
            dgvViajesActivos.Columns[3].ItemStyle.Width = 130;
            dgvViajesActivos.Columns[4].ItemStyle.Width = 150;
            dgvViajesActivos.Columns[5].ItemStyle.Width = 250;
            dgvViajesActivos.Columns[6].ItemStyle.Width = 100;
            dgvViajesActivos.Columns[7].ItemStyle.Width = 100;
            dgvViajesActivos.Columns[8].ItemStyle.Width = 100;
            dgvViajesActivos.Columns[9].ItemStyle.Width = 75;
            dgvViajesActivos.Columns[18].ItemStyle.Width = 100;

            dgvViajesActivos.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvViajesActivos.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvViajesActivos.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvViajesActivos.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvViajesActivos.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvViajesActivos.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvViajesActivos.Columns[8].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvViajesActivos.Columns[9].ItemStyle.HorizontalAlign = HorizontalAlign.Center;

            dgvViajesActivos.Columns[1].Visible = ok;
            dgvViajesActivos.Columns[10].Visible = ok;
            dgvViajesActivos.Columns[11].Visible = ok;
            dgvViajesActivos.Columns[12].Visible = ok;
            dgvViajesActivos.Columns[13].Visible = ok;
            dgvViajesActivos.Columns[14].Visible = ok;
            dgvViajesActivos.Columns[15].Visible = ok;
            dgvViajesActivos.Columns[16].Visible = ok;
            dgvViajesActivos.Columns[17].Visible = ok;
            dgvViajesActivos.Columns[18].Visible = ok;
            dgvViajesActivos.Columns[19].Visible = ok;
        }

        #endregion

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(Session["idCierreCaja"].ToString()) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Debe registrar la apertura de caja para realizar la venta de boletos.', 'info');", true);
                    return;
                }

                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);

                string sFechaSistema = DateTime.Now.ToString("dd/MM/yyyy");

                if (dgvDatos.Rows[a].Cells[9].Text == "C")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El viaje ya ha sido cerrado. No puede editar el registro.', 'warning');", true);
                }

                else if (Convert.ToDateTime(dgvDatos.Rows[a].Cells[3].Text) < Convert.ToDateTime(sFechaSistema))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'La fecha de viaje seleccionado ya se encuentra caducado.', 'warning');", true);
                }

                else
                {
                    Session["banderaFiltroViajesActivos"] = "1";

                    string[] sSeparar_Bus = dgvDatos.Rows[a].Cells[4].Text.Split('-');
                    Session["disco_vehiculo_tasa"] = sSeparar_Bus[0].ToString().Trim();

                    string[] sSeparar_Ruta = dgvDatos.Rows[a].Cells[5].Text.Split('-');
                    Session["pueblo_origen_tasa"] = sSeparar_Ruta[0].ToString().Trim();
                    Session["pueblo_destino_tasa"] = sSeparar_Ruta[1].ToString().Trim();

                    Session["idVehiculo"] = dgvDatos.Rows[a].Cells[13].Text;
                    Session["idVehiculoReemplazo"] = dgvDatos.Rows[a].Cells[15].Text;
                    Session["idProgramacion"] = dgvDatos.Rows[a].Cells[1].Text;
                    Session["idCttPueblo"] = dgvDatos.Rows[a].Cells[14].Text;
                    Session["factorDescuento"] = "0";
                    Session["idPuebloOrigen"] = cmbFiltrarGrid.SelectedValue;
                    Session["id_pueblo_origen_tasa"] = dgvDatos.Rows[a].Cells[16].Text;
                    Session["id_pueblo_destino_tasa"] = dgvDatos.Rows[a].Cells[17].Text;
                    Session["cobrar_administracion_boletos"] = dgvDatos.Rows[a].Cells[18].Text;

                    Session["iIdViajeVentaSMARTT"] = dgvDatos.Rows[a].Cells[19].Text;

                    Session["auxiliar"] = "1";
                    Session["dtClientes"] = null;
                    mostrarBotones();

                    if (consultarTipoServicio(Convert.ToInt32(Session["idProgramacion"].ToString())) == -1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo cargar los parámetros del tipo de viaje.', 'danger');", true);
                        goto fin;
                    }

                    else if (consultarTipoServicio(Convert.ToInt32(Session["idProgramacion"].ToString())) == 0)
                    {
                        Session["extra"] = "0";
                        indice = 0;

                        if (consultarListaPrecioTipoCliente(Convert.ToInt32(cmbTipoCliente.SelectedValue)) == true)
                        {
                            cmbDestino.SelectedIndex = 0;
                            txtPrecio.Text = "0.00";
                            txtDescuento.Text = "0.00";
                            txtPrecioFinal.Text = "0.00";
                        }
                    }

                    if (consultarTipoServicio(Convert.ToInt32(Session["idProgramacion"].ToString())) == 1)
                    {
                        Session["extra"] = "1";
                        indice = 0;

                        if (consultarListaPrecioTipoCliente(Convert.ToInt32(cmbTipoCliente.SelectedValue)) == true)
                        {
                            cmbDestino.SelectedIndex = 0;
                            txtPrecio.Text = "0.00";
                            txtDescuento.Text = "0.00";
                            txtPrecioFinal.Text = "0.00";
                        }
                    }

                    extraerTotalCobrado();

                    lblDetalleBus1.Text = dgvDatos.Rows[a].Cells[5].Text + " - " + dgvDatos.Rows[a].Cells[6].Text;
                    lblDetalleBus2.Text = "FECHA SALIDA: " + dgvDatos.Rows[a].Cells[3].Text + " - VEHÍCULO: " + dgvDatos.Rows[a].Cells[4].Text + " - TIPO DE VIAJE: " + dgvDatos.Rows[a].Cells[8].Text;
                    txtNumeroViaje.Text = dgvDatos.Rows[a].Cells[2].Text;
                    txtViajeDia.Text = dgvDatos.Rows[a].Cells[0].Text;
                    txtFechaViaje.Text = dgvDatos.Rows[a].Cells[3].Text;
                    txtHoraViaje.Text = dgvDatos.Rows[a].Cells[6].Text;
                    txtTransporteViaje.Text = dgvDatos.Rows[a].Cells[4].Text;
                    txtRutaViaje.Text = dgvDatos.Rows[a].Cells[5].Text;

                    Session["etiqueta_viaje"] = "FECHA SALIDA: " + dgvDatos.Rows[a].Cells[3].Text + " - VEHÍCULO: " + dgvDatos.Rows[a].Cells[4].Text + " - RUTA: " + dgvDatos.Rows[a].Cells[5].Text + " - HORA: " + dgvDatos.Rows[a].Cells[6].Text;
                    Session["destino_viaje_etiqueta"] = dgvDatos.Rows[a].Cells[5].Text;
                    Session["hora_viaje_etiqueta"] = dgvDatos.Rows[a].Cells[6].Text;

                    Session["numero_viaje_cierre"] = dgvDatos.Rows[a].Cells[2].Text;
                    Session["fecha_viaje_cierre"] = dgvDatos.Rows[a].Cells[3].Text;
                    Session["hora_viaje_cierre"] = dgvDatos.Rows[a].Cells[6].Text;

                    pnlGrid.Visible = false;
                    pnlBus.Visible = true;
                    pnlAsientos.Visible = true;

                    if (Convert.ToInt32(Session["idPuebloOrigen"].ToString()) == Convert.ToInt32(Session["id_pueblo"].ToString()))
                    {
                        btnCerrarViaje.Visible = true;
                    }

                    else
                    {
                        btnCerrarViaje.Visible = false;
                    }

                    //txtIdentificacion.Text = Application["numero_consumidor_final"].ToString();
                    //buscarCliente(txtIdentificacion.Text.Trim());
                    cmbDestino.SelectedIndex = 0;
                    asientosOcupados();
                    extraerTotalCobrado();
                    consultarHoraCambioNormal(dgvDatos.Rows[a].Cells[5].Text);
                }

                columnasGrid(false);
                goto fin;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

        fin: { }
        }

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            //btnSave.Text = "Editar";
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void txtIdentificacionRegistro_TextChanged(object sender, EventArgs e)
        {
            iLongitudCedula = txtIdentificacionRegistro.Text.Trim().Length;

            if ((iLongitudCedula == 12) || (iLongitudCedula == 11))
            {
                lblAlerta.Text = "El número de identificación es incorrecto.";
                txtIdentificacionRegistro.Text = "";
                txtIdentificacion.Focus();
            }

            else if (iLongitudCedula > 13)
            {
                lblAlerta.Text = "El número de identificación es incorrecto.";
                txtIdentificacionRegistro.Text = "";
                txtIdentificacion.Focus();
            }

            else if (iLongitudCedula < 10)
            {
                lblAlerta.Text = "El número de identificación es incorrecto.";
                txtIdentificacionRegistro.Text = "";
                txtIdentificacion.Focus();
            }

            else
            {
                if (iLongitudCedula == 10)
                {
                    if (cedula.validarCedulaConsulta(txtIdentificacionRegistro.Text.Trim()) == "NO")
                    {
                        lblAlerta.Text = "El número de identificación es incorrecto.";
                        txtIdentificacionRegistro.Text = "";
                        txtIdentificacion.Focus();
                    }

                    else
                    {
                        contarRegistro();
                    }
                }

                else if (iLongitudCedula == 13)
                {
                    int iTercerDigito = Convert.ToInt32(txtIdentificacionRegistro.Text.Trim().Substring(0, 2));

                    if (iTercerDigito == 6)
                    {
                        if (ruc.validarRucPublico(txtIdentificacionRegistro.Text.Trim()) == false)
                        {
                            lblAlerta.Text = "El número de identificación es incorrecto.";
                            txtIdentificacionRegistro.Text = "";
                            txtIdentificacion.Focus();
                        }

                        else
                        {
                            contarRegistro();
                        }
                    }

                    else if (iTercerDigito == 9)
                    {
                        if (ruc.validarRucPrivado(txtIdentificacionRegistro.Text.Trim()) == false)
                        {
                            lblAlerta.Text = "El número de identificación es incorrecto.";
                            txtIdentificacionRegistro.Text = "";
                            txtIdentificacion.Focus();
                        }

                        else
                        {
                            contarRegistro();
                        }
                    }

                    else if ((iTercerDigito >= 0) && (iTercerDigito < 6))
                    {
                        if (ruc.validarRucNatural(txtIdentificacionRegistro.Text.Trim()) == false)
                        {
                            lblAlerta.Text = "El número de identificación es incorrecto.";
                            txtIdentificacionRegistro.Text = "";
                            txtIdentificacion.Focus();
                        }

                        else
                        {
                            contarRegistro();
                        }
                    }

                    else
                    {
                        lblAlerta.Text = "El número de identificación es incorrecto.";
                        txtIdentificacionRegistro.Text = "";
                        txtIdentificacion.Focus();
                    }
                }
            }
        }

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            Session["idPasajero"] = null;

            if (txtIdentificacion.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese un número de identificación.', 'info');", true);
                txtIdentificacion.Focus();
                return;
            }

            iLongitudCedula = txtIdentificacion.Text.Trim().Length;

            if ((iLongitudCedula == 12) || (iLongitudCedula == 11))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                //txtIdentificacion.Text = "";
                txtIdentificacion.Focus();
                return;
            }

            else if (iLongitudCedula > 13)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                //txtIdentificacion.Text = "";
                txtIdentificacion.Focus();
                return;
            }

            else if (iLongitudCedula < 10)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                //txtIdentificacion.Text = "";
                txtIdentificacion.Focus();
                return;
            }

            else
            {
                if (iLongitudCedula == 10)
                {
                    if (cedula.validarCedulaConsulta(txtIdentificacion.Text.Trim()) == "NO")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                        //txtIdentificacion.Text = "";
                        txtIdentificacion.Focus();
                        return;
                    }

                    else
                    {
                        buscarCliente(txtIdentificacion.Text.Trim());
                    }
                }

                else if (iLongitudCedula == 13)
                {
                    int iTercerDigito = Convert.ToInt32(txtIdentificacion.Text.Trim().Substring(2, 1));

                    if (iTercerDigito == 6)
                    {
                        int iCuentaValidar = 0;

                        if (ruc.validarRucNatural(txtIdentificacion.Text.Trim()) == false)
                        {
                            iCuentaValidar++;
                        }

                        if (iCuentaValidar == 1)
                        {
                            if (ruc.validarRucPublico(txtIdentificacion.Text.Trim()) == false)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                                //txtIdentificacion.Text = "";
                                txtIdentificacion.Focus();
                                return;
                            }

                            else
                            {
                                buscarCliente(txtIdentificacion.Text.Trim());
                            }
                        }

                        else
                        {
                            buscarCliente(txtIdentificacion.Text.Trim());
                        }
                    }

                    else if (iTercerDigito == 9)
                    {
                        if (ruc.validarRucPrivado(txtIdentificacion.Text.Trim()) == false)
                        {
                            lblAlerta.Text = "El número de identificación es incorrecto.";
                            //txtIdentificacion.Text = "";
                            txtIdentificacion.Focus();
                        }

                        else
                        {
                            buscarCliente(txtIdentificacion.Text.Trim());
                        }
                    }

                    else if ((iTercerDigito >= 0) && (iTercerDigito < 6))
                    {
                        if (ruc.validarRucNatural(txtIdentificacion.Text.Trim()) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                            //txtIdentificacion.Text = "";
                            txtIdentificacion.Focus();
                            return;
                        }

                        else
                        {
                            buscarCliente(txtIdentificacion.Text.Trim());
                        }
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número de identificación es incorrecto.', 'info');", true);
                        //txtIdentificacion.Text = "";
                        txtIdentificacion.Focus();
                        return;
                    }
                }
            }
        }

        protected void btnEditarPasajero_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal2').modal('show');</script>", false);
            ModalPopupExtenderCrearEditar.Show();

            if ((txtIdentificacion.Text.Trim() == "") || (Session["idPasajero"] == null))
            {

            }

            else
            {
                Session["idPersonaConsulta"] = Session["idPasajero"].ToString();
                txtIdentificacionRegistro.Text = txtIdentificacion.Text.Trim();
                txtNombreRegistro.Text = Session["nombrePersona"].ToString();
                txtRazonSocial.Text = Session["apellidoPersona"].ToString();

                txtFechaNacimiento.Text = Convert.ToDateTime(Session["fechaNacimiento"].ToString()).ToString("dd/MM/yyyy");
                cmbIdentificacion.SelectedValue = Session["cgTipoIdentificacion"].ToString();
                lblAlerta.Text = "";
            }
        }

        protected void btnCerrarModalCrearEditar_Click(object sender, EventArgs e)
        {
            cerrarModalRegistro();
        }

        protected void btnCerrarPasajero_Click(object sender, EventArgs e)
        {
            cerrarModalRegistro();
        }

        protected void cmbDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDestino.SelectedValue == "0")
            {
                txtPrecio.Text = "0.00";
                txtDescuento.Text = "0.00";
                txtPrecioFinal.Text = "0.00";
            }

            else
            {
                consultarPrecio();

                if (chkCortesia.Checked == true)
                {
                    txtDescuento.Text = txtPrecio.Text.Trim();
                }

                else
                {
                    txtDescuento.Text = "0.00";
                }
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDate.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Seleccione una fecha para proceder con la búsqueda de registros.', 'danger');", true);
                    txtDate.Focus();
                }

                else
                {
                    sFecha = txtDate.Text.Trim();
                    llenarGrid(sFecha);
                    llenarGridExtras(sFecha);
                }
            }

            catch (Exception)
            {
                cerrarModal();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ocurrió un problema con el formato de fecha.', 'danger');", true);
            }
        }

        protected void btnGuardarPasajero_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbIdentificacion.SelectedValue) == 0)
            {
                lblAlerta.Text = "Favor seleccione el tipo de identificación.";
                cmbIdentificacion.Focus();
            }

            else if (txtIdentificacionRegistro.Text.Trim() == "")
            {
                lblAlerta.Text = "Favor ingrese el número de identificación.";
                txtIdentificacion.Focus();
            }

            else if (txtRazonSocial.Text.Trim() == "")
            {
                lblAlerta.Text = "Favor ingrese la razón social o apellido del pasajero.";
                txtRazonSocial.Focus();
            }

            else if (txtFechaNacimiento.Text.Trim() == "")
            {
                lblAlerta.Text = "Favor ingrese la fecha de nacimiento.";
                txtFechaNacimiento.Focus();
            }

            else
            {
                string sFechaActual = DateTime.Now.ToString("dd/MM/yyyy");

                if (Convert.ToDateTime(txtFechaNacimiento.Text.Trim()) >= Convert.ToDateTime(sFechaActual))
                {
                    lblAlerta.Text = "La fecha ingresada es superior a la actual.";
                    txtFechaNacimiento.Text = "";
                    txtFechaNacimiento.Focus();
                }

                else
                {
                    iDiscapacidad = 0;

                    if (Session["idPersonaConsulta"] == null)
                    {
                        int iTercerDigito = Convert.ToInt32(txtIdentificacionRegistro.Text.Trim().Substring(0, 2));

                        if ((iTercerDigito == 9) || (iTercerDigito == 6))
                        {
                            Session["cgTipoPersona"] = "2448";
                        }

                        else
                        {
                            Session["cgTipoPersona"] = "2447";
                        }

                        insertarPasajero();
                    }

                    else
                    {
                        actualizarRegistro();
                    }
                }
            }
        }

        protected void btnFiltrarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFiltrarCliente.Text.Trim() == "")
                {
                    llenarGridClientes(0, 0);
                }

                else
                {
                    llenarGridClientes(1, 0);
                }
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal1').modal('show');</script>", false);
            }
        }

        protected void btnCerrarModal_Click(object sender, EventArgs e)
        {
            btnPopUp_ModalPopupExtender.Hide();
            ModalPopupExtender_ListaPasajeros.Hide();//CERRAMOS MODAL LISTA PASAJEROS
        }

        protected void lbtnSeleccion_Click(object sender, EventArgs e)
        {
            sAccionPersonas = "Seleccion";
            iCerrarModal = 0;
        }

        protected void dgvFiltrarClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvFiltrarClientes.SelectedIndex;
                if (sAccionPersonas == "Seleccion")
                {
                    buscarCliente(dgvFiltrarClientes.Rows[a].Cells[1].Text);
                }

                btnPopUp_ModalPopupExtender.Hide();
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvDatos.PageIndex = e.NewPageIndex;

            if (txtDate.Text.Trim() != "")
            {
                sFecha = txtDate.Text.Trim();
            }

            else
            {
                sFecha = DateTime.Now.ToString("yyyy/MM/dd");
            }

            llenarGrid(sFecha);
        }

        protected void dgvFiltrarClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvFiltrarClientes.PageIndex = e.NewPageIndex;

            if (txtFiltrarCliente.Text.Trim() == "")
            {
                llenarGridClientes(0, 0);
            }

            else
            {
                llenarGridClientes(1, 0);
            }
        }

        protected void btnCerrarViaje_Click(object sender, EventArgs e)
        {
            try
            {
                Session["filaGrid_V"] = "-1";
                pnlBus.Visible = false;
                pnlAsientos.Visible = false;
                pnlCierreViaje.Visible = true;
                lblEtiquetaCierre.Text = Session["etiqueta_viaje"].ToString();
                txtEfectivoModal.Text = "0.00";
                txtPagosPendientesModal.Text = "0.00";
                txtFaltanteModal.Text = "0.00";

                if ((Session["ejecuta_cobro_administrativo"] == null) || (Session["ejecuta_cobro_administrativo"].ToString() == "0"))
                {
                    //btnRecalcular.Visible = false;
                    pnlMostrarPagosPendientes.Visible = false;
                    //pnlAgregarPagos.Visible = false;
                    agregarFaltante.Visible = false;
                    recalcularValoresNormales();
                }

                else
                {
                    if (consultarRealizarCobros() == true)
                    {
                        llenarGridPendientes();
                        recalcularValoresNormales();
                        //recalcularValores_V2();
                        pintarGridPendiente();
                    }

                    pnlMostrarPagosPendientes.Visible = true;
                    //pnlAgregarPagos.Visible = true;
                    agregarFaltante.Visible = true;
                    //btnRecalcular.Visible = true;
                }
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmTransacciones.aspx");
        }

        protected void cmbTipoCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["idPersonaTipoCliente"] = null;
            indice = cmbDestino.SelectedIndex;

            if (consultarListaPrecioTipoCliente(Convert.ToInt32(cmbTipoCliente.SelectedValue)) == true)
            {
                if (indice == 0)
                {
                    txtPrecio.Text = "0.00";
                    txtDescuento.Text = "0.00";
                    txtPrecioFinal.Text = "0.00";
                }

                else
                {
                    cmbDestino.SelectedIndex = indice;
                    manejarTipoClienteManual(Convert.ToInt32(cmbTipoCliente.SelectedValue));
                    consultarPrecio();
                }
            }
        }

        protected void lbtnEliminarFila_Click(object sender, EventArgs e)
        {
            sAccion = "Eliminar";
        }

        protected void cmbFiltrarGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["idPuebloOrigen"] = cmbFiltrarGrid.SelectedValue;

            if (txtDate.Text.Trim() == "")
            {
                llenarGrid(DateTime.Now.ToString("dd/MM/yyyy"));
                llenarGridExtras(DateTime.Now.ToString("dd/MM/yyyy"));
            }

            else
            {
                llenarGrid(txtDate.Text.Trim());
                llenarGridExtras(txtDate.Text.Trim());
            }
        }

        protected void btnConsumimdorFinal_Click(object sender, EventArgs e)
        {
            txtIdentificacion.Text = Application["numero_consumidor_final"].ToString();
            buscarCliente(txtIdentificacion.Text.Trim());
        }

        protected void btnFacturaDatos_Click(object sender, EventArgs e)
        {
            validarInsertar(1);
        }

        protected void btnFacturaConsumidorFinal_Click(object sender, EventArgs e)
        {
            validarInsertar(0);
        }

        protected void btnFacturaRapida_Click(object sender, EventArgs e)
        {
            validarInsertar(2);
        }

        //FUNCIONES PARA FACTURAR CON DATOS
        protected void btnFiltrarFactura_Click(object sender, EventArgs e)
        {
            try
            {
                lblMensajeFactura.Text = "";

                if (txtBuscarClienteFactura.Text.Trim() == "")
                {
                    llenarGridClientes(0, 1);
                }

                else
                {
                    llenarGridClientes(1, 1);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvGridFacturar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvGridFacturar.PageIndex = e.NewPageIndex;

            if (txtBuscarClienteFactura.Text.Trim() == "")
            {
                llenarGridClientes(0, 1);
            }

            else
            {
                llenarGridClientes(1, 1);
            }
        }

        protected void dgvGridFacturar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvGridFacturar.SelectedIndex;
                columnasGridFacturar(true);
                Session["idPersonaFactura"] = dgvGridFacturar.Rows[a].Cells[0].Text;
                lblRazonSocial.Text = dgvGridFacturar.Rows[a].Cells[2].Text;
                columnasGridFacturar(false);
                lblMensajeFactura.Text = "";
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnProcesarFactura_Click(object sender, EventArgs e)
        {
            if (Session["idPersonaFactura"] == null)
            {
                lblMensajeFactura.Text = "Favor seleccione los datos para la factura.";
            }

            else
            {
                lblMensajeFactura.Text = "";
                insertarRegistro();
            }
        }

        protected void btnCancelarModalFactura_Click(object sender, EventArgs e)
        {
            Session["idPersonaFactura"] = null;
            txtBuscarClienteFactura.Text = "";
            lblRazonSocial.Text = "";
            lblMensajeFactura.Text = "";
            ModalPopupExtender_Factura.Hide();
        }

        protected void lbtnSeleccionFactura_Click(object sender, EventArgs e)
        {
            sAccion = "Seleccionar";
        }

        protected void dgvDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvDetalle.PageIndex = e.NewPageIndex;
            llenarGridPendientes();
        }

        protected void btnLimpiarAsignacion_Click(object sender, EventArgs e)
        {
            Session["auxiliar"] = "1";
            mostrarBotones();

            limpiar();

            //extraerTotalCobrado();
            //validarCobro();

            Session["idPersonaFactura"] = null;
            txtBuscarClienteFactura.Text = "";
            lblRazonSocial.Text = "";
            lblMensajeFactura.Text = "";
            txtPrecio.Text = "0.00";
            txtDescuento.Text = "0.00";
            txtPrecioFinal.Text = "0.00";
            cmbDestino.SelectedIndex = 0;
        }

        //FUNCIONES DEL MODAL DE CIERRE
        protected void btnCancelarModal_Click(object sender, EventArgs e)
        {
            pnlBus.Visible = true;
            pnlAsientos.Visible = true;
            pnlCierreViaje.Visible = false;
        }

        protected void btnRecalcular_Click(object sender, EventArgs e)
        {
            string sEstadoPago_P;
            Decimal dbSumaPendientes_P = 0;
            Decimal dbPrimerTotal_P;
            Decimal dbIngresoEfectivo_P;
            Decimal dbPagoAdministracionRecuperado_P = 0;
            Decimal dbSegundoTotal_P;
            Decimal dbPagosPendientes_P;
            Decimal dbSubtotalNeto_P;

            txtEfectivoModal.Text = "0.00";

            dbPrimerTotal_P = Convert.ToDecimal(txtPrimerTotalModal.Text.Trim());
            dbIngresoEfectivo_P = Convert.ToDecimal(txtEfectivoModal.Text.Trim());

            columnasGridPendiente(true);

            //BUSCAR EL PAGO
            foreach (GridViewRow row in dgvDetalle.Rows)
            {
                sEstadoPago_P = row.Cells[8].Text.Trim().ToUpper();
                CheckBox check = row.FindControl("chkSeleccionar") as CheckBox;

                if (sEstadoPago_P == "PAGO ACTUAL")
                {
                    if (check.Checked == true)
                    {
                        dbPagoAdministracionRecuperado_P = Convert.ToDecimal(row.Cells[6].Text.Trim());
                        txtPagoModal.Text = row.Cells[6].Text.Trim();
                        //rdbPagado.Checked = true;
                    }

                    else
                    {
                        txtPagoModal.Text = "0.00";
                        //rdbPendiente.Checked = true;
                    }
                }

                else
                {
                    if (check.Checked == true)
                    {
                        dbSumaPendientes_P += Convert.ToDecimal(row.Cells[6].Text.Trim());
                    }
                }
            }
                        
            txtPagosPendientesModal.Text = dbSumaPendientes_P.ToString("N2");

            columnasGridPendiente(false);

            dbPagoAdministracionRecuperado_P = Convert.ToDecimal(txtPagoModal.Text.Trim());
            dbSegundoTotal_P = dbPrimerTotal_P - dbPagoAdministracionRecuperado_P + dbIngresoEfectivo_P;
            txtSegundoTotalModal.Text = dbSegundoTotal_P.ToString("N2");
            dbPagosPendientes_P = Convert.ToDecimal(txtPagosPendientesModal.Text.Trim());

            if (dbPagoAdministracionRecuperado_P + dbPagosPendientes_P > dbPrimerTotal_P + dbIngresoEfectivo_P)
            {
                txtFaltanteModal.Text = (dbPagoAdministracionRecuperado_P + dbPagosPendientes_P - dbPrimerTotal_P - dbIngresoEfectivo_P).ToString("N2");
            }

            else
            {
                txtFaltanteModal.Text = "0.00";
            }

            dbSubtotalNeto_P = dbSegundoTotal_P - dbPagosPendientes_P;
            txtTotalNetoModal.Text = dbSubtotalNeto_P.ToString("N2");
        }

        protected void btnCierreViajeModal_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(txtTotalNetoModal.Text.Trim()) < 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Los valores a entregar son inferiores a cero. No puede cerrar el viaje.', 'warning');", true);
                return;
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModalCierre').modal('show');</script>", false);
            }
        }

        protected void lbtnListaPasajeros_Click(object sender, EventArgs e)
        {
            if (ListarPasajerosVendidos() != true) return;
            ModalPopupExtender_ListaPasajeros.Show();
        }

        //FIN FUNCIONES DEL MODAL DE CIERRE

        //FUNCION PARA LISTAR EN MODAL PASAJEROS VENDIDOS
        public bool ListarPasajerosVendidos()
        {
            try
            {
                sSql = "";
                sSql += "select numero_asiento, identificacion, pasajero," + Environment.NewLine;
                sSql += "tipo_cliente, descripcion, valor" + Environment.NewLine;
                sSql += "from ctt_vw_reporte_pasajeros" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + Convert.ToInt32(Session["idProgramacion"]) + Environment.NewLine;
                sSql += "order by numero_asiento" + Environment.NewLine;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    dgvListaPasajeros.DataSource = dtConsulta;
                    dgvListaPasajeros.DataBind();
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
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        protected void btnReimprimirFactura_Click(object sender, EventArgs e)
        {
            consultarFacturasEmitidas();
        }

        protected void btnCerrarModalFacturasEmitidas_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_ReimprimirFacturas.Hide();
        }

        protected void dgvVendidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvVendidos.SelectedIndex;
                columnasGridVendidos(true);
                iIdFactura = Convert.ToInt32(dgvVendidos.Rows[a].Cells[2].Text);
                columnasGridVendidos(false);

                crearReporteImprimir();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvVendidos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvVendidos.PageIndex = e.NewPageIndex;
            llenarGridVendidos(e.NewPageIndex + 1);
        }

        protected void dgvDatosExtras_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(Session["idCierreCaja"].ToString()) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Debe registrar la apertura de caja para realizar la venta de boletos.', 'info');", true);
                    return;
                }

                int a = dgvDatosExtras.SelectedIndex;
                columnasGridExtra(true);

                string sFechaSistema = DateTime.Now.ToString("dd/MM/yyyy");

                if (dgvDatosExtras.Rows[a].Cells[9].Text == "C")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El viaje ya ha sido cerrado. No puede editar el registro.', 'warning');", true);
                }

                else if (Convert.ToDateTime(dgvDatosExtras.Rows[a].Cells[3].Text) < Convert.ToDateTime(sFechaSistema))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'La fecha de viaje seleccionado ya se encuentra caducado.', 'warning');", true);
                }

                else
                {
                    Session["banderaFiltroViajesActivos"] = "0";

                    string[] sSeparar_Bus = dgvDatosExtras.Rows[a].Cells[4].Text.Split('-');
                    Session["disco_vehiculo_tasa"] = sSeparar_Bus[0].ToString().Trim();

                    string[] sSeparar_Ruta = dgvDatosExtras.Rows[a].Cells[5].Text.Split('-');
                    Session["pueblo_origen_tasa"] = sSeparar_Ruta[0].ToString().Trim();
                    Session["pueblo_destino_tasa"] = sSeparar_Ruta[1].ToString().Trim();

                    Session["idVehiculo"] = dgvDatosExtras.Rows[a].Cells[13].Text;
                    Session["idVehiculoReemplazo"] = dgvDatosExtras.Rows[a].Cells[15].Text;
                    Session["idProgramacion"] = dgvDatosExtras.Rows[a].Cells[1].Text;
                    Session["idCttPueblo"] = dgvDatosExtras.Rows[a].Cells[14].Text;
                    Session["factorDescuento"] = "0";
                    Session["idPuebloOrigen"] = cmbFiltrarGrid.SelectedValue;
                    Session["id_pueblo_origen_tasa"] = dgvDatosExtras.Rows[a].Cells[16].Text;
                    Session["id_pueblo_destino_tasa"] = dgvDatosExtras.Rows[a].Cells[17].Text;
                    Session["cobrar_administracion_boletos"] = dgvDatosExtras.Rows[a].Cells[18].Text;

                    Session["iIdViajeVentaSMARTT"] = dgvDatosExtras.Rows[a].Cells[19].Text;

                    Session["auxiliar"] = "1";
                    Session["dtClientes"] = null;
                    mostrarBotones();

                    if (consultarTipoServicio(Convert.ToInt32(Session["idProgramacion"].ToString())) == -1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo cargar los parámetros del tipo de viaje.', 'danger');", true);
                        goto fin;
                    }

                    else if (consultarTipoServicio(Convert.ToInt32(Session["idProgramacion"].ToString())) == 0)
                    {
                        Session["extra"] = "0";
                        indice = 0;

                        if (consultarListaPrecioTipoCliente(Convert.ToInt32(cmbTipoCliente.SelectedValue)) == true)
                        {
                            cmbDestino.SelectedIndex = 0;
                            txtPrecio.Text = "0.00";
                            txtDescuento.Text = "0.00";
                            txtPrecioFinal.Text = "0.00";
                        }
                    }

                    if (consultarTipoServicio(Convert.ToInt32(Session["idProgramacion"].ToString())) == 1)
                    {
                        Session["extra"] = "1";
                        indice = 0;

                        if (consultarListaPrecioTipoCliente(Convert.ToInt32(cmbTipoCliente.SelectedValue)) == true)
                        {
                            cmbDestino.SelectedIndex = 0;
                            txtPrecio.Text = "0.00";
                            txtDescuento.Text = "0.00";
                            txtPrecioFinal.Text = "0.00";
                        }

                    }

                    extraerTotalCobrado();

                    lblDetalleBus1.Text = dgvDatosExtras.Rows[a].Cells[5].Text + " - " + dgvDatosExtras.Rows[a].Cells[6].Text;
                    lblDetalleBus2.Text = "FECHA SALIDA: " + dgvDatosExtras.Rows[a].Cells[3].Text + " - VEHÍCULO: " + dgvDatosExtras.Rows[a].Cells[4].Text + " - TIPO DE VIAJE: " + dgvDatosExtras.Rows[a].Cells[8].Text;
                    txtNumeroViaje.Text = dgvDatosExtras.Rows[a].Cells[2].Text;
                    txtViajeDia.Text = dgvDatosExtras.Rows[a].Cells[0].Text;
                    txtFechaViaje.Text = dgvDatosExtras.Rows[a].Cells[3].Text;
                    txtHoraViaje.Text = dgvDatosExtras.Rows[a].Cells[6].Text;
                    txtTransporteViaje.Text = dgvDatosExtras.Rows[a].Cells[4].Text;
                    txtRutaViaje.Text = dgvDatosExtras.Rows[a].Cells[5].Text;

                    Session["etiqueta_viaje"] = "FECHA SALIDA: " + dgvDatosExtras.Rows[a].Cells[3].Text + " - VEHÍCULO: " + dgvDatosExtras.Rows[a].Cells[4].Text + " - RUTA: " + dgvDatosExtras.Rows[a].Cells[5].Text + " - HORA: " + dgvDatosExtras.Rows[a].Cells[6].Text;

                    pnlGrid.Visible = false;
                    pnlBus.Visible = true;

                    if (Convert.ToInt32(Session["idPuebloOrigen"].ToString()) == Convert.ToInt32(Session["id_pueblo"].ToString()))
                    {
                        btnCerrarViaje.Visible = true;
                    }

                    else
                    {
                        btnCerrarViaje.Visible = false;
                    }

                    asientosOcupados();
                    extraerTotalCobrado();
                }

                columnasGridExtra(false);
                goto fin;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

        fin: { }
        }

        protected void btnIngresarFaltante_Click(object sender, EventArgs e)
        {
            try
            {
                int a = Convert.ToInt32(Session["filaGrid_V"].ToString());

                if (a < 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No ha ingresado valores a un registro de pagos. Favor abone un registro.', 'warning');", true);
                    return;
                }

                txtEfectivoModal.Text = txtFaltanteModal.Text.Trim();
                Label lblSaldo_G = dgvDetalle.Rows[a].Cells[13].FindControl("lblSaldoGrid") as Label;
                Label lblAbono_G = dgvDetalle.Rows[a].Cells[12].FindControl("lblAbonoGrid") as Label;
                Label lblIngresoEfectivo_G = dgvDetalle.Rows[a].Cells[12].FindControl("lblIngresoEfectivoFaltante") as Label;

                Decimal dbValorAbonado = Convert.ToDecimal(lblAbono_G.Text.Trim());
                Decimal dbValorFaltante = Convert.ToDecimal(lblSaldo_G.Text.Trim());
                Decimal dbValorDebido = Convert.ToDecimal(dgvDetalle.Rows[a].Cells[7].Text);

                if (dbValorFaltante == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El saldo del último registro abonado ya se encuentra en cero.', 'warning');", true);
                    return;
                }

                Decimal dbValoresPendientes = Convert.ToDecimal(txtPagosPendientesModal.Text.Trim());
                Decimal dbValorIngresado = Convert.ToDecimal(txtEfectivoModal.Text.Trim());
                lblIngresoEfectivo_G.Text = dbValorIngresado.ToString("N2");
                Decimal dbSumaValor_R = dbValorIngresado + dbValorAbonado;
                Decimal dbSaldo_R = dbValorDebido - dbSumaValor_R;
                dbValoresPendientes += dbValorIngresado;

                lblAbono_G.Text = dbSumaValor_R.ToString("N2");
                lblSaldo_G.Text = dbSaldo_R.ToString("N2");
                txtPagosPendientesModal.Text = dbValoresPendientes.ToString("N2");
                txtFaltanteModal.Text = "0.00";
                recalcularValores_V2();
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvDatosExtras_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvDatosExtras.PageIndex = e.NewPageIndex;

                if (txtDate.Text.Trim() != "")
                {
                    sFecha = txtDate.Text.Trim();
                }

                else
                {
                    sFecha = DateTime.Now.ToString("yyyy/MM/dd");
                }

                llenarGridExtras(sFecha);
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnAbonarAdministracion_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(txtSegundoTotalModal.Text.Trim()) == 0)
            {
                return;
            }

            if (Convert.ToDecimal(txtTotalNetoModal.Text.Trim()) < 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El valor a abonar como pago administrativo actual es inferior a cero. No puede cerrar el viaje.', 'warning');", true);
                return;
            }

            Decimal dbPrimerPago_P;
            Decimal dbSegundoPago_P;
            Decimal dbPagosPendientes_P;
            Decimal dbPagoAdministracion_P;
            Decimal dbIngresoEfectivo_P;
            Decimal dbTotalNeto_P;

            string sEstadoPago_P;

            dbPrimerPago_P = Convert.ToDecimal(txtPrimerTotalModal.Text.Trim());
            dbPagosPendientes_P = Convert.ToDecimal(txtPagosPendientesModal.Text.Trim());
            dbPagoAdministracion_P = Convert.ToDecimal(Session["pago_administracion"].ToString());

            if (dbPrimerPago_P - dbPagosPendientes_P > dbPagoAdministracion_P)
            {
                txtPagoModal.Text = dbPagoAdministracion_P.ToString("N2");

                //BUSCAR EL PAGO
                columnasGridPendiente(true);

                foreach (GridViewRow row in dgvDetalle.Rows)
                {
                    sEstadoPago_P = row.Cells[8].Text.Trim().ToUpper();

                    if (sEstadoPago_P == "PAGO ACTUAL")
                    {
                        CheckBox check = row.FindControl("chkSeleccionar") as CheckBox;
                        check.Checked = true;
                    }
                }

                columnasGridPendiente(false);

                //rdbPagado.Checked = true;
            }

            else
            {
                txtPagoModal.Text = (dbPrimerPago_P - dbPagosPendientes_P).ToString("N2");

                //BUSCAR EL PAGO
                columnasGridPendiente(true);

                foreach (GridViewRow row in dgvDetalle.Rows)
                {
                    sEstadoPago_P = row.Cells[8].Text.Trim().ToUpper();

                    if (sEstadoPago_P == "PAGO ACTUAL")
                    {
                        CheckBox check = row.FindControl("chkSeleccionar") as CheckBox;
                        check.Checked = false;
                    }
                }

                columnasGridPendiente(false);

                //rdbPagoParcial.Checked = true;
            }

            dbPagoAdministracion_P = Convert.ToDecimal(txtPagoModal.Text.Trim());
            dbIngresoEfectivo_P = Convert.ToDecimal(txtEfectivoModal.Text.Trim());
            dbSegundoPago_P = dbPrimerPago_P - dbPagoAdministracion_P + dbIngresoEfectivo_P;
            txtSegundoTotalModal.Text = dbSegundoPago_P.ToString("N2");
            dbSegundoPago_P = Convert.ToDecimal(txtSegundoTotalModal.Text.Trim());
            dbTotalNeto_P = dbSegundoPago_P - dbPagosPendientes_P;
            txtTotalNetoModal.Text = dbTotalNeto_P.ToString("N2");
        }

        protected void dgvListaPasajeros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvListaPasajeros.PageIndex = e.NewPageIndex;
            ListarPasajerosVendidos();
        }


        protected void btnAceptarCerrar_Click(object sender, EventArgs e)
        {
            cerrarViaje();
        }

        protected void lbtnNuevaHora_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(Session["extra"].ToString()) == 0)
                {
                    ModalPopupExtender_NuevaHora.Show();

                    if (Session["hora_modificar"].ToString() == "SN")
                    {
                        txtHoraActual.Text = Session["hora_viaje_cierre"].ToString();
                        txtNuevaHoraSalida.Text = "";
                    }

                    else
                    {
                        txtHoraActual.Text = Session["hora_viaje_cierre"].ToString();
                        txtNuevaHoraSalida.Text = Convert.ToDateTime(Session["hora_modificar"].ToString()).ToString("HH:mm");
                    }
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No se permite cambiar la hora en un viaje extra', 'error');", true);
                }
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnAsignarNuevaHora_Click(object sender, EventArgs e)
        {
            actualizarHora();
        }

        protected void btnCancelarNuevaHora_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_NuevaHora.Hide();
        }

        protected void btnCerrarModalNuevaHora_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_NuevaHora.Hide();
        }

        protected void btnRemoverNuevaHora_Click(object sender, EventArgs e)
        {
            anularHora();
        }

        protected void chkCortesia_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCortesia.Checked == true)
            {
                txtDescuento.Text = txtPrecio.Text.Trim();
            }

            else
            {
                txtDescuento.Text = "0.00";
            }
        }

        protected void lbtnAbonarPago_Click(object sender, EventArgs e)
        {
            sAccionPagos = "Abonar";
        }

        protected void lbtnRemoverPago_Click(object sender, EventArgs e)
        {
            sAccionPagos = "Remover";
        }

        protected void dgvDetalle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sTextoEncabezado;
                int a = dgvDetalle.SelectedIndex;
                Session["filaGrid_V"] = a;
                columnasGridPendiente(true);

                if (sAccionPagos == "Abonar")
                {
                    CheckBox chkSeleccionarFila = dgvDetalle.Rows[a].Cells[9].FindControl("chkSeleccionar") as CheckBox;

                    if (chkSeleccionarFila.Checked == false)
                    {
                        recalcularValores_V2();

                        Decimal dbPagoDisponible = Convert.ToDecimal(txtTotalNetoModal.Text.Trim());
                        Decimal dbIngresoEfectivo = Convert.ToDecimal(txtEfectivoModal.Text.Trim());
                        Decimal dbValorPagosPendientes = Convert.ToDecimal(txtPagosPendientesModal.Text.Trim());

                        if (dbPagoDisponible == 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El saldo ya se encuentra en cero. No puede realizar más cobros.', 'info');", true);
                        }

                        else
                        {
                            sTextoEncabezado = dgvDetalle.Rows[a].Cells[2].Text.Trim().ToUpper();

                            Decimal dbValorAbono_P = Convert.ToDecimal(dgvDetalle.Rows[a].Cells[6].Text);
                            Decimal dbValorDebido_P = Convert.ToDecimal(dgvDetalle.Rows[a].Cells[7].Text);
                            Decimal dbIngresoValor_R;

                            TextBox txtValorAbono_R = dgvDetalle.Rows[a].Cells[9].FindControl("txtValorAbonoGrid") as TextBox;
                            Label lblAbonoGrid = dgvDetalle.Rows[a].Cells[9].FindControl("lblAbonoGrid") as Label;
                            Label lblSaldoGrid = dgvDetalle.Rows[a].Cells[9].FindControl("lblSaldoGrid") as Label;

                            if ((txtValorAbono_R.Text.Trim() == "") || (Convert.ToDecimal(txtValorAbono_R.Text.Trim()) == 0))
                            {
                                //txtValorAbono_R.Text = dbValorDebido_P.ToString("N2");

                                if (dbValorDebido_P < dbPagoDisponible)
                                {
                                    txtValorAbono_R.Text = dbValorDebido_P.ToString("N2");
                                }

                                else
                                {
                                    txtValorAbono_R.Text = dbPagoDisponible.ToString("N2");
                                }
                            }

                            dbIngresoValor_R = Convert.ToDecimal(txtValorAbono_R.Text.Trim());

                            if (dbIngresoValor_R > dbPagoDisponible)
                            {
                                dbIngresoValor_R = dbPagoDisponible;
                            }

                            if (dbIngresoValor_R > dbValorDebido_P)
                            {
                                txtValorAbono_R.Text = dbValorDebido_P.ToString("N2");
                                dbIngresoValor_R = dbValorDebido_P;
                            }

                            dbPagoDisponible -= dbIngresoValor_R;

                            if (dbIngresoValor_R < 0)
                            {
                                txtValorAbono_R.Text = "";
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El efectivo ingresado dará valores negativos. El sistema cancelará la operación.', 'warning');", true);
                                return;
                            }

                            lblAbonoGrid.Text = dbIngresoValor_R.ToString("N2");
                            lblSaldoGrid.Text = (dbValorDebido_P - dbIngresoValor_R).ToString("N2");
                            txtValorAbono_R.Text = "";

                            if (sTextoEncabezado == "PAGO ACTUAL")
                            {
                                txtPagoModal.Text = dbIngresoValor_R.ToString("N2");
                                txtSegundoTotalModal.Text = dbPagoDisponible.ToString("N2");
                            }

                            else
                            {
                                dbValorPagosPendientes += dbIngresoValor_R;
                                txtPagosPendientesModal.Text = dbValorPagosPendientes.ToString("N2");
                            }

                            chkSeleccionarFila.Checked = true;
                            recalcularValores_V2();

                            Decimal dbSumarFaltantes = 0;

                            foreach (GridViewRow row in dgvDetalle.Rows)
                            {
                                Label lblSaldo_A = row.FindControl("lblSaldoGrid") as Label;

                                dbSumarFaltantes += Convert.ToDecimal(lblSaldo_A.Text.Trim());
                            }

                            txtFaltanteModal.Text = dbSumarFaltantes.ToString("N2");
                        }
                    }
                }

                else if (sAccionPagos == "Remover")
                {
                    CheckBox chkSeleccionarFila = dgvDetalle.Rows[a].Cells[9].FindControl("chkSeleccionar") as CheckBox;

                    if (chkSeleccionarFila.Checked == true)
                    {
                        sTextoEncabezado = dgvDetalle.Rows[a].Cells[2].Text.Trim().ToUpper();

                        TextBox txtValorAbono_R = dgvDetalle.Rows[a].Cells[9].FindControl("txtValorAbonoGrid") as TextBox;
                        Label lblAbonoGrid = dgvDetalle.Rows[a].Cells[9].FindControl("lblAbonoGrid") as Label;
                        Label lblSaldoGrid = dgvDetalle.Rows[a].Cells[9].FindControl("lblSaldoGrid") as Label;
                        Label lblIngresoEfectivoFaltanteGrid = dgvDetalle.Rows[a].Cells[9].FindControl("lblIngresoEfectivoFaltante") as Label;

                        Decimal dbAbonoAnular_G = Convert.ToDecimal(lblAbonoGrid.Text.Trim());
                        Decimal dbValorPagosAnular_G = Convert.ToDecimal(txtPagosPendientesModal.Text.Trim());

                        txtValorAbono_R.Text = "";
                        lblAbonoGrid.Text = "0.00";
                        lblSaldoGrid.Text = "0.00";
                        lblIngresoEfectivoFaltanteGrid.Text = "0.00";

                        if (sTextoEncabezado == "PAGO ACTUAL")
                        {
                            txtPagoModal.Text = "0.00";
                        }

                        else
                        {
                            dbValorPagosAnular_G -= dbAbonoAnular_G;
                            txtPagosPendientesModal.Text = dbValorPagosAnular_G.ToString("N2");
                        }

                        chkSeleccionarFila.Checked = false;
                        recalcularValores_V2();
                    }
                }

                columnasGridPendiente(false);

                pintarGridPendiente();
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //RECALCULAR LOS VALORES
        private void recalcularValores_V2()
        {
            if (txtEfectivoModal.Text.Trim() == "")
            {
                txtEfectivoModal.Text = "0.00";
            }

            //AQUI SE VA A RECALCULAR LAS CAJAS DE TEXTO
            Decimal dbPrimerTotal_G = Convert.ToDecimal(txtPrimerTotalModal.Text.Trim());
            Decimal dbPagoAdministracion_G = Convert.ToDecimal(txtPagoModal.Text.Trim());
            Decimal dbIngresoEfectivo_G = Convert.ToDecimal(txtEfectivoModal.Text.Trim());
            Decimal dbSegundoTotal_G = dbPrimerTotal_G + dbIngresoEfectivo_G - dbPagoAdministracion_G;
            Decimal dbPagosAtrasados_G = Convert.ToDecimal(txtPagosPendientesModal.Text.Trim());
            Decimal dbTotalNeto_G = dbPrimerTotal_G + dbIngresoEfectivo_G - dbPagoAdministracion_G - dbPagosAtrasados_G;

            //Session["valorIngreso_V"] = dbIngresoEfectivo_G.ToString();
            txtSegundoTotalModal.Text = dbSegundoTotal_G.ToString("N2");
            txtTotalNetoModal.Text = dbTotalNeto_G.ToString("N2");
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

        protected void dgvDatosExtras_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < dgvDatosExtras.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvDatosExtras.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvDatosExtras.Rows[i].BackColor = Color.White;
                }
            }
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

        protected void dgvFiltrarClientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < dgvFiltrarClientes.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvFiltrarClientes.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvFiltrarClientes.Rows[i].BackColor = Color.White;
                }
            }
        }

        protected void dgvGridFacturar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < dgvGridFacturar.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvGridFacturar.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvGridFacturar.Rows[i].BackColor = Color.White;
                }
            }
        }

        protected void dgvListaPasajeros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < dgvListaPasajeros.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvListaPasajeros.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvListaPasajeros.Rows[i].BackColor = Color.White;
                }
            }
        }

        protected void dgvVendidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            pintarGridPendiente();
        }

        //AQUI SE VERA EL MODAL CON LOS VIAJES ACTIVOS
        protected void btnAbrirModalViajes_Click(object sender, EventArgs e)
        {
            txtFechaViajesActivos.Text = DateTime.Now.ToString("dd/MM/yyyy");
            llenarGridViajesActivos();
        }

        protected void btnBuscarViajesActivos_Click(object sender, EventArgs e)
        {
            string sFecha_1 = Convert.ToDateTime(txtFechaViajesActivos.Text.Trim()).ToString("yyyy-MM-dd");
            string sFecha_2 = DateTime.Now.ToString("yyyy-MM-dd");

            if (Convert.ToDateTime(sFecha_1) < Convert.ToDateTime(sFecha_2))
            {
                ModalPopUpSeleccionViajes.Hide();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No puede realizar búsquedas con fechas inferiores a la actual.', 'warning');", true);
                return;
            }

            llenarGridViajesActivos();
        }

        protected void btnCerrarModalViajesActivos_Click(object sender, EventArgs e)
        {
            ModalPopUpSeleccionViajes.Hide();
        }

        protected void dgvViajesActivos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvViajesActivos.PageIndex = e.NewPageIndex;
        }

        protected void dgvViajesActivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < dgvViajesActivos.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvViajesActivos.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvViajesActivos.Rows[i].BackColor = Color.White;
                }
            }
        }

        protected void dgvViajesActivos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(Session["idCierreCaja"].ToString()) == 0)
                {
                    cerrarModal();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Debe registrar la apertura de caja para realizar la venta de boletos.', 'info');", true);
                    return;
                }

                int a = dgvViajesActivos.SelectedIndex;
                columnasGridViajesActivos(true);

                string[] sSeparar_Bus = dgvViajesActivos.Rows[a].Cells[4].Text.Split('-');
                Session["disco_vehiculo_tasa"] = sSeparar_Bus[0].ToString().Trim();

                string[] sSeparar_Ruta = dgvViajesActivos.Rows[a].Cells[5].Text.Split('-');
                Session["pueblo_origen_tasa"] = sSeparar_Ruta[0].ToString().Trim();
                Session["pueblo_destino_tasa"] = sSeparar_Ruta[1].ToString().Trim();

                Session["idVehiculo"] = dgvViajesActivos.Rows[a].Cells[13].Text;
                Session["idVehiculoReemplazo"] = dgvViajesActivos.Rows[a].Cells[15].Text;
                Session["idProgramacion"] = dgvViajesActivos.Rows[a].Cells[1].Text;
                Session["idCttPueblo"] = dgvViajesActivos.Rows[a].Cells[14].Text;
                Session["factorDescuento"] = "0";
                Session["idPuebloOrigen"] = cmbFiltrarGrid.SelectedValue;
                Session["id_pueblo_origen_tasa"] = dgvViajesActivos.Rows[a].Cells[16].Text;
                Session["id_pueblo_destino_tasa"] = dgvViajesActivos.Rows[a].Cells[17].Text;
                Session["cobrar_administracion_boletos"] = dgvViajesActivos.Rows[a].Cells[18].Text;
                Session["iIdViajeVentaSMARTT"] = dgvViajesActivos.Rows[a].Cells[19].Text;

                Session["auxiliar"] = "1";
                Session["dtClientes"] = null;
                mostrarBotones();

                if (consultarTipoServicio(Convert.ToInt32(Session["idProgramacion"].ToString())) == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo cargar los parámetros del tipo de viaje.', 'danger');", true);
                    return;
                }

                else if (consultarTipoServicio(Convert.ToInt32(Session["idProgramacion"].ToString())) == 0)
                {
                    Session["extra"] = "0";
                    indice = 0;

                    if (consultarListaPrecioTipoCliente(Convert.ToInt32(cmbTipoCliente.SelectedValue)) == true)
                    {
                        cmbDestino.SelectedIndex = 0;
                        txtPrecio.Text = "0.00";
                        txtDescuento.Text = "0.00";
                        txtPrecioFinal.Text = "0.00";
                    }
                }

                if (consultarTipoServicio(Convert.ToInt32(Session["idProgramacion"].ToString())) == 1)
                {
                    Session["extra"] = "1";
                    indice = 0;

                    if (consultarListaPrecioTipoCliente(Convert.ToInt32(cmbTipoCliente.SelectedValue)) == true)
                    {
                        cmbDestino.SelectedIndex = 0;
                        txtPrecio.Text = "0.00";
                        txtDescuento.Text = "0.00";
                        txtPrecioFinal.Text = "0.00";
                    }
                }

                extraerTotalCobrado();

                lblDetalleBus1.Text = dgvViajesActivos.Rows[a].Cells[5].Text + " - " + dgvViajesActivos.Rows[a].Cells[6].Text;
                lblDetalleBus2.Text = "FECHA SALIDA: " + dgvViajesActivos.Rows[a].Cells[3].Text + " - VEHÍCULO: " + dgvViajesActivos.Rows[a].Cells[4].Text + " - TIPO DE VIAJE: " + dgvViajesActivos.Rows[a].Cells[8].Text;
                txtNumeroViaje.Text = dgvViajesActivos.Rows[a].Cells[2].Text;
                txtViajeDia.Text = dgvViajesActivos.Rows[a].Cells[0].Text;
                txtFechaViaje.Text = dgvViajesActivos.Rows[a].Cells[3].Text;
                txtHoraViaje.Text = dgvViajesActivos.Rows[a].Cells[6].Text;
                txtTransporteViaje.Text = dgvViajesActivos.Rows[a].Cells[4].Text;
                txtRutaViaje.Text = dgvViajesActivos.Rows[a].Cells[5].Text;

                Session["etiqueta_viaje"] = "FECHA SALIDA: " + dgvViajesActivos.Rows[a].Cells[3].Text + " - VEHÍCULO: " + dgvViajesActivos.Rows[a].Cells[4].Text + " - RUTA: " + dgvViajesActivos.Rows[a].Cells[5].Text + " - HORA: " + dgvViajesActivos.Rows[a].Cells[6].Text;
                Session["destino_viaje_etiqueta"] = dgvViajesActivos.Rows[a].Cells[5].Text;
                Session["hora_viaje_etiqueta"] = dgvViajesActivos.Rows[a].Cells[6].Text;

                Session["numero_viaje_cierre"] = dgvViajesActivos.Rows[a].Cells[2].Text;
                Session["fecha_viaje_cierre"] = dgvViajesActivos.Rows[a].Cells[3].Text;
                Session["hora_viaje_cierre"] = dgvViajesActivos.Rows[a].Cells[6].Text;

                pnlGrid.Visible = false;
                pnlBus.Visible = true;
                pnlAsientos.Visible = true;

                if (Convert.ToInt32(Session["idPuebloOrigen"].ToString()) == Convert.ToInt32(Session["id_pueblo"].ToString()))
                {
                    btnCerrarViaje.Visible = true;
                }

                else
                {
                    btnCerrarViaje.Visible = false;
                }
                cmbDestino.SelectedIndex = 0;
                asientosOcupados();
                extraerTotalCobrado();
                consultarHoraCambioNormal(dgvViajesActivos.Rows[a].Cells[5].Text);

                columnasGridViajesActivos(false);
                ModalPopUpSeleccionViajes.Hide();
                return;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void txtEfectivoModal_TextChanged(object sender, EventArgs e)
        {
            if (txtEfectivoModal.Text.Trim() == "")
            {
                txtEfectivoModal.Text = "0.00";
            }

            recalcularValores_V2();
        }
    }
}