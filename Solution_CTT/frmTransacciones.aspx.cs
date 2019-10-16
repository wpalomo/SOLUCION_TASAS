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
    class TasaUsuario
    {
        public string id_tasa { get; set; }
    }

    class TasaVerificaToken
    {
        public string Error { get; set; }

        public bool Usar { get; set; }

        public string Msj { get; set; }

        public string Cantidad { get; set; }
    }

    public partial class frmTransacciones : System.Web.UI.Page
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
        Clases.ClaseGenerarTasaAcompanante tasaAcompanante = new Clases.ClaseGenerarTasaAcompanante();
        Clases.ClaseReporteBoletoResumido reporteElectronico = new Clases.ClaseReporteBoletoResumido();
        Clases.ClaseReporteBoletoNormal reporteNormal = new Clases.ClaseReporteBoletoNormal();
        Clases.ClaseCierreItinerario cerrarViajeReporte = new Clases.ClaseCierreItinerario();
        Clases.ClaseInstruccionesCierre cierreViajeInstrucciones = new Clases.ClaseInstruccionesCierre();
        Clases.ClaseImpresion imprimir = new Clases.ClaseImpresion();
        Clases.ClaseValidarRUC ruc = new Clases.ClaseValidarRUC();
        Clases.ValidarCedula cedula = new Clases.ValidarCedula();
        Clases.ImpresionReporte impresiones = new Clases.ImpresionReporte();

        Clases_Tasa_Usuario.ClaseValidarToken validarToken = new Clases_Tasa_Usuario.ClaseValidarToken();

        Button botonSeleccionado;

        string sSql;
        string sAccion;
        string sAccionPersonas;
        string sFecha;
        string sTabla;
        string sCampo;
        string sCiudad;
        string sDireccion;
        string sTelefono;
        string sCorreoElectronico;
        string sClienteFactura;
        string sIdentificacionFactura;
        string sImprimir;
        string sNombreImpresora;
        string sPathImpresora;
        string sEstadoCierre;
        string sIdentificacionPasajero;
        string sNombrePasajero;
        string sDescripcionMes;
        string sToken;
        string sCuentaToken;
        string sTasaUsuario;
        string sCantidadBoletosToken;
        string sTipoClienteTasa;
        string sIdTasaRespuesta;

        string sObjetoOficina;
        string sObjetoTasa;
        string sObjetoInfo;
        string sObjetoCliente;
        string sObjetoNotificacion;
        string sObjetoJson;
        string sUrlCredenciales;
        string sUrlEnvio;
        string sUrlAnula;

        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;
        DataTable dtAlmacenar;
        DataTable dtAsientos;
        DataTable dtOcupados;
        DataTable dtTipos;
        DataTable dtTasasDisponibles;

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
        int iTasaEmitidaBandera;
        int iManejaFacturacionElectronica;
        int iEmiteTasaUsuario;
        int iIdPuebloOrigen;
        int iIdPuebloDestino;
        int iAmbienteTasa;
        int iCantidadTasasEmitir;
        int iCantidadDisponible;
        int iIdFormaPagoFactura;

        int iPorcentajeNotificacionEntero;

        int iCgEstadoDctoPorCobrar = 7461;

        int iBandera;
        int iCortarPapel;
        int AbrirCajon;
        int iNumeroImpresiones;

        double dbPrecioUnitario;
        double dbCantidad;
        double dbIva;
        double dbServicio;

        double dbPagoAdministracionCierre;
        double dbTotalCobradoCierre;
        double dbValorRetencionCierre;
        double dbDescuentoCierre;
        double dbTotalCierre;

        double dbPagoAdministracionGuardar;
        double dbDescuentoGuardar;
        double dbValorRetenidoGuardar;
        double dbTotalCobrado;

        double dbAbonoGrid;
        double dbValorRealGrid;

        decimal dbValorTasa;

        long iMaximo;

        //VARIABLES DEL REPORTE
        int iVendidos_REP;
        int iCuenta_REP;

        decimal dbCantidad_REP;
        decimal dbPrecioUnitario_REP;
        decimal dbDescuento_REP;
        decimal dbIva_REP;
        decimal dbSumaTotal_REP;

        Decimal dbCantidad_Notificacion;
        Decimal dbDisponible_Notificacion;
        Decimal dbPorcentaje_Notificacion;

        string sNumeroFactura_REP;
        string sAsientos_REP;
        string sTasaUsuarioRecuperado_REP;

        Byte[] Logo { get; set; }

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

                //if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                //{
                //    consultarParametrosTasa();
                //}
            }

            else
            {
                if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                {
                    btnReimprimirFactura.Visible = false;
                    consultarParametrosTasa();
                }

                if (Session["auxiliar"].ToString() == "1")
                {
                    //if (Convert.ToInt32(Session["idVehiculo"].ToString()) != 0)
                    //{
                        mostrarBotones();
                    //}
                }
            }
        }

        #region FUNCIONES DE INTEGRACION

        //FUNCION PARA CONSULTAR LAS TASAS PENDIENTES POR SINCRONIZAR
        private int consultarTasasPendientesSincronizar()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from ctt_vw_tasa_usuario_no_enviada" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString()) + Environment.NewLine;
                sSql += "and ambiente_tasa_usuario = " + Convert.ToInt32(Session["emision"].ToString()) + Environment.NewLine;
                sSql += "and emite_tasa_usuario = 1";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    return Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                }

                cerrarModal();
                lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + this.sSql.Replace("\n", "<br/>");
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return 0;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return 0;
            }
        }

        //FUNCION PARA CONTAR LAS TASAS DISPONIBLES EN EL TOKEN
        private void contarTasasToken()
        {
            try
            {
                sSql = "";
                sSql += "select token, isnull(maximo_secuencial, 0) - isnull(emitidos, 0) total_tasas_disponibles" + Environment.NewLine;
                sSql += "from ctt_tasa_token" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and estado_token = 'Abierta'" + Environment.NewLine;
                sSql += "and ambiente_token = " + Convert.ToInt32(Session["emision"].ToString()) + Environment.NewLine;
                sSql += "and validado = 1" + Environment.NewLine;
                sSql += "and id_ctt_oficinista = " + Convert.ToInt32(Session["idUsuario"].ToString());

                dtTasasDisponibles = new DataTable();
                dtTasasDisponibles.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtTasasDisponibles);

                if (bRespuesta)
                {
                    Session["dtTasasDisponibles"] = dtTasasDisponibles;
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }
            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCIONES PARA CREAR UNA TASA ACOMPAÑANTE



        #endregion

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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

            catch(Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                //sSql += "where id_pedido = " + iIdPedido + Environment.NewLine;
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

                        if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                        {
                            sTasaUsuarioRecuperado_REP = dtConsulta.Rows[0][12].ToString();
                            Logo = barcode(sTasaUsuarioRecuperado_REP);
                        }                        

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

                        DataColumn imagen = new DataColumn("tasa_generada");
                        imagen.DataType = System.Type.GetType("System.Byte[]");
                        dtConsulta.Columns.Add(imagen);

                        //RECORRER EL DATATABLE PARA LLENAR DE DATOS
                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            dtConsulta.Rows[i]["tasa_generada"] = Logo;
                            dtConsulta.Rows[i]["valor_total"] = dbSumaTotal_REP.ToString("N2");
                            dtConsulta.Rows[i]["vendidos"] = iVendidos_REP.ToString();
                            dtConsulta.Rows[i]["asientos"] = sAsientos_REP.Trim();
                            dtConsulta.Rows[i]["secuencia_factura"] = sNumeroFactura_REP;
                        }

                        DSReportes ds = new DSReportes();

                            DataTable dt = ds.Tables["dtFactura"];
                            dt.Clear();

                        dt = dtConsulta;

                        //AGREGAR EL DETALLE DE BOLETOS VENDIDOS
                        sSql = "";
                        sSql += "select tipo_cliente, count(*) cuenta" + Environment.NewLine;
                        sSql += "from ctt_vw_factura" + Environment.NewLine;
                        //sSql += "where id_pedido = " + iIdPedido + Environment.NewLine;
                        sSql += "where id_factura = " + iIdFactura + Environment.NewLine;
                        sSql += "group by tipo_cliente";

                        DataTable dt2 = ds.Tables["dtTarifas"];
                        dt2.Clear();

                        bRespuesta = conexionM.consultarRegistro(sSql, dt2);

                        if (bRespuesta == true)
                        {
                            LocalReport reporteLocal = new LocalReport();

                            if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                            {
                                if (Convert.ToInt32(Session["adjuntar_tasa_boleto"].ToString()) == 1)
                                {
                                    reporteLocal.ReportPath = Server.MapPath("~/Reportes/rptFactura.rdlc");
                                }

                                else
                                {
                                    reporteLocal.ReportPath = Server.MapPath("~/Reportes/rptFactura_2.rdlc");
                                }
                            }

                            else
                            {
                                reporteLocal.ReportPath = Server.MapPath("~/Reportes/rptFactura_2.rdlc");
                            }

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION DE PRUEBA
        private byte[] barcode(string sTasa)
        {
            BarcodeLib.Barcode codigo = new BarcodeLib.Barcode();
            codigo.IncludeLabel = true;

            var ms = new MemoryStream();

            Bitmap imgOK = new Bitmap(codigo.Encode(BarcodeLib.TYPE.CODE128, sTasa.ToString(), Color.Black, Color.White, 500, 150));

            imgOK.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            return ms.ToArray();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                            boton.Click += new EventHandler (boton_clic_asiento);
                            //boton.Attributes.Add("Class", "btn bg-olive btn-default btn-sm");
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                    if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                    {

                        if (Convert.ToInt32(txtCantidadTasasDisponibles.Text.Trim()) == Convert.ToInt32(txtTasaUsuario.Text.Trim()))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No dispone de suficientes tasas de usuario para emitir más asientos en la factura. Favor ingrese un nuevo token.', 'warning');", true);
                            return;
                        }
                    }

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

            catch(Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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

                if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                {
                    dtTasasDisponibles = new DataTable();
                    dtTasasDisponibles.Clear();
                    dtTasasDisponibles = Session["dtTasasDisponibles"] as DataTable;

                    int num = Convert.ToInt32(txtTasaUsuario.Text.Trim()) + 1;
                    int num2 = 0;

                    for (int i = 0; i < this.dtTasasDisponibles.Rows.Count; i++)
                    {
                        if (num <= Convert.ToInt32(dtTasasDisponibles.Rows[i][1].ToString()))
                        {
                            num2 = 1;
                            break;
                        }
                    }

                    if (num2 == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'La cantidad de pasajeros a vender supera a las tasas de usuario disponibles en los tokens del sistema. Se recomienda vender en facturas separadas o  verifique en el reporte de tokens.', 'warning');", true);
                        return;
                    }
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
                    //if (Convert.ToInt32(Session["idPersonaTipoCliente"].ToString()) == Convert.ToInt32(Application["idPersonaMenorEdad"].ToString()))
                    //{
                    //    iIdPersona = Convert.ToInt32(Application["idPersonaMenorEdad"].ToString());
                    //}

                    //else
                    //{
                    //    iIdPersona = 0;
                    //}

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
                        sIdentificacionPasajero =Application["numero_id_sin_datos"].ToString();
                    }
                }

                //else
                //{
                //    iIdPersona = 0;
                //    Session["idPasajero"] = iIdPersona.ToString();
                //}
                                
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

                dtAlmacenar.Rows.Add(row);

                if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                {
                    txtTasaUsuario.Text = dtAlmacenar.Rows.Count.ToString();
                }

                Session["dtClientes"] = dtAlmacenar;

                botonProcesar.Attributes.Add("class", "btn bg-orange btn-default btn-sm");
                botonProcesar.ToolTip = "ASIENTO EN PROCESO";

                txtIdentificacion.Attributes.Add("onKeyPress", "doClick('" + btnBuscarCliente.ClientID + "',event)");
                txtIdentificacion.Focus();

                goto fin;
            }

            catch(Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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

                for (int i = dtAlmacenar.Rows.Count -1; i >=0; i--)
                {
                    if (Convert.ToInt32(dtAlmacenar.Rows[i][1].ToString()) == Convert.ToInt32(botonProcesar.CommandArgument))
                    {
                        dtAlmacenar.Rows.RemoveAt(i);
                    }
                }

                if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                {
                    txtTasaUsuario.Text = dtAlmacenar.Rows.Count.ToString();
                }

                Session["dtClientes"] = dtAlmacenar;

                //botonSeleccionado.Attributes.Add("Class", "btn bg-olive btn-default btn-sm");
                botonSeleccionado.Attributes.Add("Class", "btn bg-darken-3 btn-default btn-sm");
                botonSeleccionado.ToolTip = "ASIENTO DISPONIBLE";
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
            ModalPopupExtender_ReporteTokenInfo.Hide();
            ModalPopupExtender_ValidaToken.Hide();
            ModalPopupExtender_InfoToken.Hide();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }
        
        //FUNCION PARA LLENAR COMBOBX DE EXTRAS
        private bool llenarComboDestinosExtras(int iIdListaPrecio_P)
        {
            try
            {
                //sSql = "";
                //sSql += "select P.id_producto, NP.nombre + ' - $' + ltrim(str(PP.valor, 10, 2)) valor" + Environment.NewLine;
                //sSql += "from cv401_productos P, cv401_productos PADRE," + Environment.NewLine;
                //sSql += "cv403_precios_productos PP, cv401_nombre_productos NP" + Environment.NewLine;
                //sSql += "where P.id_producto_padre = PADRE.id_producto" + Environment.NewLine;
                //sSql += "and PP.id_producto = P.id_producto" + Environment.NewLine;
                //sSql += "and NP.id_producto = P.id_producto" + Environment.NewLine;
                //sSql += "and PADRE.id_ctt_pueblo_origen = " + Convert.ToInt32(Session["id_pueblo"].ToString()) + Environment.NewLine;
                //sSql += "and P.estado = 'A'" + Environment.NewLine;
                //sSql += "and PADRE.estado = 'A'" + Environment.NewLine;
                //sSql += "and PP.estado = 'A'" + Environment.NewLine;
                //sSql += "and NP.estado = 'A'" + Environment.NewLine;
                //sSql += "and id_lista_precio = " + iIdListaPrecio_P + Environment.NewLine;
                //sSql += "and P.aplica_extra = 1" + Environment.NewLine;
                //sSql += "order by P.id_producto";

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                //sSql += Convert.ToInt32(Session["cgTipoPersona"].ToString()) + ", ";
                //sSql += Convert.ToInt32(cmbIdentificacion.SelectedValue) + "," + Environment.NewLine;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                    goto fin;
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
                //sSql += "cg_tipo_persona = " + Convert.ToInt32(Session["cgTipoPersona"].ToString()) + "," + Environment.NewLine;
                //sSql += "cg_tipo_identificacion = " + Convert.ToInt32(cmbIdentificacion.SelectedValue) + "," + Environment.NewLine;
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

                goto fin;
            }

            catch(Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

            reversa: { conexionM.reversaTransaccion(); }
            fin: { }
        }

        //FUNCION PARA CERRAR EL MODAL DE REGISTRO DE PASAJERO
        private void cerrarModalRegistro()
        {
            cmbIdentificacion.SelectedIndex = 0;
            txtIdentificacionRegistro.Text = "";
            txtRazonSocial.Text = "";
            txtNombreRegistro.Text = "";
            txtFechaNacimiento.Text = "";
            //chkDiscapacidad.Checked = false;
            lblAlerta.Text = "";
            ModalPopupExtenderCrearEditar.Hide();
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            llenarComboTipoCliente();

            cmbTipoIdentificacion.SelectedIndex = 0;
            //cmbDestino.SelectedIndex = 0;
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
            txtTasaUsuario.Text = "0";
            lblEdad.Text = "SIN ASIGNAR";
            lblEdad.ForeColor = Color.Black;
            lblEdad.BackColor = Color.White;

            asientosOcupados();
            extraerTotalCobrado();
            crearDataTable();

            //cmbDestino.SelectedIndex = iIndiceDestino;
            
            consultarPrecio();

            if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
            {
                sumaTotalTasasDisponibles();
            }

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

            //txtIdentificacion.Text = Application["numero_consumidor_final"].ToString();
            //buscarCliente(txtIdentificacion.Text.Trim());

            txtIdentificacion.Attributes.Add("onKeyPress", "doClick('" + btnBuscarCliente.ClientID + "',event)");

            //cmbDestino.Focus();
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
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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

                if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                {
                    if (Convert.ToInt32(Session["notificacion_emergente"].ToString()) == 1)
                    {
                        consultarDatosToken();
                    }

                    contarTasasToken();
                }

                Session["idPersonaFactura"] = null;
                txtBuscarClienteFactura.Text = "";
                lblRazonSocial.Text = "";
                lblMensajeFactura.Text = "";
                ModalPopupExtender_Factura.Hide();

                if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                {
                    if (sIdTasaRespuesta == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Boletos generados éxitosamente. Tasa pendiente por sincronizar.', 'success');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Boletos generados éxitosamente', 'success');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "Popup", "swal('Éxito.!', 'Boletos generados éxitosamente', 'success');", true);
                }

                goto fin;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };

        fin: { }
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
                sSql += "bandera_boleteria, id_ctt_jornada)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idEmpresa"].ToString()) + ", " + Convert.ToInt32(Application["cgEmpresa"].ToString()) + ", ";
                sSql += Convert.ToInt32(Application["idLocalidad"].ToString()) + ", '" + sFecha + "', " + iIdPersona + "," + Environment.NewLine;
                sSql += parametros.CgTipoCliente + ", " + Convert.ToInt32(Application["cgMoneda"].ToString()) + ", 0, " + Convert.ToInt32(Session["idVendedor"].ToString()) + ",";
                sSql += parametros.CgEstadoPedido + ", 0, 7469, " + Convert.ToInt32(Session["idProgramacion"].ToString()) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(Session["idUsuario"].ToString()) + ", '" + sNombrePasajero + "', ";
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 0, 0, 1, 0, 0, " + iIdPuebloOrigen + ", " + iIdPuebloDestino + ", 1," + Environment.NewLine;
                sSql += Convert.ToInt32(Session["idJornada"].ToString()) + ")";

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
                dtConsulta = new DataTable();
                dtConsulta.Clear();

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
                    //dbPrecioUnitario = Convert.ToDouble(dtAlmacenar.Rows[i][2].ToString());
                    //dbDescuento = Convert.ToDouble(dtAlmacenar.Rows[i][3].ToString());
                    dbPrecioUnitario = Convert.ToDouble(dtAlmacenar.Rows[i][2].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    dbDescuento = Convert.ToDouble(dtAlmacenar.Rows[i][3].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    iIdProducto = Convert.ToInt32(dtAlmacenar.Rows[i][4].ToString());
                    iIdDetalleRuta = Convert.ToInt32(dtAlmacenar.Rows[i][5].ToString());
                    iIdTipoCliente = Convert.ToInt32(dtAlmacenar.Rows[i][6].ToString());
                    iIdPersonaAsiento = Convert.ToInt32(dtAlmacenar.Rows[i][7].ToString());

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
                    //sSql += iIdPedido + ", " + iIdProducto + ", 546, " + dbPrecioUnitario + ", " + Environment.NewLine;
                    //sSql += dbCantidad + ", " + dbDescuento + ", 0, " + dbIva + ", " + dbServicio + ", " + Environment.NewLine;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                sSql += "estado, numero_replica_trigger,numero_control_replica," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                //sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dbTotal + ", 'A', 1, 0, " + Environment.NewLine;
                sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dbTotal.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", 'A', 1, 0, " + Environment.NewLine;
                sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //INSERTAR FASE 3 - CREAR FACTURA
        private bool insertarFactura()
        {
            try
            {
                //INSTRUCCION SQL PARA EXTRAER DATOS DEL CLIENTE A FACTURAR
                sSql = "";
                sSql += "select isnull(TD.direccion, '') direccion, isnull(TD.calle_principal, '') calle_principal," + Environment.NewLine;
                sSql += "isnull(TD.numero_vivienda, '') numero_vivienda, isnull(TD.calle_interseccion, '') calle_interseccion," + Environment.NewLine;
                sSql += "isnull(TD.referencia, '') referencia, isnull(TP.codigo_alterno, '') codigo_alterno," + Environment.NewLine;
                sSql += "isnull(isnull(TT.domicilio, TT.celular), '') telefono, isnull(TP.correo_electronico, '') correo_electronico," + Environment.NewLine;
                sSql += "ltrim(isnull(TP.nombres, '') + ' ' + TP.apellidos) cliente_factura, TP.identificacion" + Environment.NewLine;
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

                if (bRespuesta == true)
                {                    
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

                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

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
                sTasaUsuario = "";

                //CONSULTAR EN CASO DE ENVIAR LA TASA DE USUARIO
                if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                {
                    iEmiteTasaUsuario = 1;

                    if (consultarToken() == false)
                    {
                        return false;
                    }
                }

                else
                {
                    iEmiteTasaUsuario = 0;
                }

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

                if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                {
                    if (Convert.ToInt32(Session["emision"].ToString()) == 0)
                    {
                        iAmbienteTasa = 0;
                    }

                    else
                    {
                        iAmbienteTasa = 1;
                    }
                }

                else
                {
                    iAmbienteTasa = 0;
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
                sSql += "facturaelectronica, clave_acceso, emite_tasa_usuario, ambiente_tasa_usuario, cantidad_tasa_emitida)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idEmpresa"].ToString()) + ", " + iIdPersona + ", " + Convert.ToInt32(Application["cgEmpresa"].ToString()) + ", 1," + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idLocalidad"].ToString()) + ", 19, " + Convert.ToInt32(Session["idVendedor"].ToString()) + ", " + iIdFormaPagoFactura + ", '" + sFecha + "'," + Environment.NewLine;
                //sSql += "'" + sFecha + "', " + Convert.ToInt32(Application["cgMoneda"].ToString()) + ", " + dbTotal + ", 0, 0, GETDATE()," + Environment.NewLine;
                sSql += "'" + sFecha + "', " + Convert.ToInt32(Application["cgMoneda"].ToString()) + ", " + dbTotal.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", 0, 0, GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A', 1, 0," + Environment.NewLine;
                sSql += "'" + sDireccion + "', '" + sTelefono + "', '" + sCiudad + "'," + Environment.NewLine;
                sSql += "'" + sCorreoElectronico + "', '" + sTasaUsuario + "', " + iManejaFacturacionElectronica + ", '" + ClaveAcceso + "', " + iEmiteTasaUsuario + ", " + iAmbienteTasa + ", ";
                sSql += Convert.ToInt32(txtTasaUsuario.Text.Trim()) + ")";

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

                //CONSULTAR EN CASO DE ENVIAR LA TASA DE USUARIO
                if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                {
                    sIdTasaRespuesta = "";

                    if (conexionInternet() == true)
                    {
                        if (crearJson() == false)
                        {
                            return false;
                        }

                        iTasaEmitidaBandera = 1;
                    }

                    else
                    {
                        iTasaEmitidaBandera = 1;
                    }

                    //ACTUALIZAR LA TABLA CV403_FACTURAS
                    sSql = "";
                    sSql += "update cv403_facturas set" + Environment.NewLine;
                    sSql += "id_tasa_emitida = '" + sIdTasaRespuesta + "'," + Environment.NewLine;
                    sSql += "tasa_emitida = " + iTasaEmitidaBandera + Environment.NewLine;
                    sSql += "where id_factura = " + iIdFactura + Environment.NewLine;
                    sSql += "and estado = 'A'";

                    //EJECUCION DE INSTRUCCION SQL
                    if (!conexionM.ejecutarInstruccionSQL(sSql))
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        return false;
                    }

                    //ACTUALIZAR LOS CONTADORES DE LA TABLA CTT_TASA_TOKEN
                    sSql = "";
                    sSql += "update ctt_tasa_token set" + Environment.NewLine;
                    sSql += "cuenta = cuenta + 1," + Environment.NewLine;
                    sSql += "emitidos = emitidos + " + iCantidadTasasEmitir + Environment.NewLine;
                    sSql += "where token = '" + sToken + "'" + Environment.NewLine;
                    sSql += "and estado = 'A'" + Environment.NewLine;
                    sSql += "and validado = 1" + Environment.NewLine;
                    sSql += "and ambiente_token = " + Convert.ToInt32(Session["emision"].ToString()) + Environment.NewLine;
                    sSql += "and estado_token = 'Abierta'";

                    //EJECUCION DE INSTRUCCION SQL
                    if (!conexionM.ejecutarInstruccionSQL(sSql))
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        return false;
                    }

                    //VERIFICAR SI YA ESTÁ AL LIMITE PERMITIDO LA TASA DE USUARIO Y CERRAR EL TOKEN
                    sSql = "";
                    sSql += "select maximo_secuencial, emitidos" + Environment.NewLine;
                    sSql += "from ctt_tasa_token" + Environment.NewLine;
                    sSql += "where token = '" + sToken + "'" + Environment.NewLine;
                    sSql += "and estado = 'A'" + Environment.NewLine;
                    sSql += "and validado = 1" + Environment.NewLine;
                    sSql += "and ambiente_token = " + Convert.ToInt32(Session["emision"].ToString()) + Environment.NewLine;
                    sSql += "and estado_token = 'Abierta'";

                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                    if (bRespuesta == true)
                    {
                        if (dtConsulta.Rows.Count > 0)
                        {
                            int iMaximo_P = Convert.ToInt32(dtConsulta.Rows[0]["maximo_secuencial"].ToString());
                            int iEmitidos_P = Convert.ToInt32(dtConsulta.Rows[0]["emitidos"].ToString());

                            if (iMaximo_P == iEmitidos_P)
                            {
                                //ACTUALIZAR LOS CONTADORES DE LA TABLA CTT_TASA_TOKEN
                                sSql = "";
                                sSql += "update ctt_tasa_token set" + Environment.NewLine;
                                sSql += "and estado_token = 'Cerrada'" + Environment.NewLine;
                                sSql += "where token = '" + sToken + "'" + Environment.NewLine;
                                sSql += "and estado = 'A'" + Environment.NewLine;
                                sSql += "and validado = 1" + Environment.NewLine;
                                sSql += "and ambiente_token = " + Convert.ToInt32(Session["emision"].ToString()) + Environment.NewLine;
                                sSql += "and estado_token = 'Abierta'";

                                //EJECUCION DE INSTRUCCION SQL
                                if (!conexionM.ejecutarInstruccionSQL(sSql))
                                {
                                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                                    return false;
                                }
                            }
                        }
                    }

                    else
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        return false;
                    }

                    //PROCEDIMIENTO PARA EXTRAER EL NUMERO DE MOVIMIENTO
                    sSql = "";
                    sSql += "select numeromovimientocaja" + Environment.NewLine;
                    sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                    sSql += "where estado = 'A'" + Environment.NewLine;
                    sSql += "and id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString());

                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                    if (bRespuesta == true)
                    {
                        iNumeroMovimientoCaja = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                    }

                    else
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        return false;
                    }                    

                    //dbValorTasa = Convert.ToDecimal(txtTasaUsuario.Text.Trim()) * Convert.ToDecimal(Session["valor_tasa_usuario"].ToString());
                    dbValorTasa = Convert.ToDecimal(txtTasaUsuario.Text.Trim(), System.Globalization.CultureInfo.InvariantCulture) * Convert.ToDecimal(Session["valor_tasa_usuario"].ToString(), System.Globalization.CultureInfo.InvariantCulture);

                    int iCobrarTasa_P;

                    if (iAmbienteTasa == 1)
                    {
                        iCobrarTasa_P = 1;
                    }

                    else
                    {
                        iCobrarTasa_P = 0;
                    }

                    //INSERTAR EN LA TABLA CTT_MOVIMIENTO_CAJA
                    sSql = "";
                    sSql += "insert into ctt_movimiento_caja (" + Environment.NewLine;
                    sSql += "tipo_movimiento, idempresa, id_localidad, id_factura, id_caja, id_ctt_jornada," + Environment.NewLine;
                    sSql += "cg_moneda, fecha, hora, cantidad, valor, tasa_usuario, estado, fecha_ingreso," + Environment.NewLine;
                    sSql += "usuario_ingreso, terminal_ingreso, id_tasa_usuario, ambiente_tasa_usuario, cobro_tasa_usuario)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += "1, " + Convert.ToInt32(Application["idEmpresa"].ToString()) + ", " + Convert.ToInt32(Application["idLocalidad"].ToString()) + "," + Environment.NewLine;
                    sSql += iIdFactura + ", 30, " + Convert.ToInt32(Session["idJornada"].ToString()) + ", " + Convert.ToInt32(Application["cgMoneda"].ToString()) + ", '" + sFecha + "', GETDATE()," + Environment.NewLine;
                    sSql += Convert.ToInt32(txtTasaUsuario.Text.Trim()) + ", " + dbValorTasa.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", '" + sTasaUsuario + "'," + Environment.NewLine;
                    sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', '" + sIdTasaRespuesta + "', " + iAmbienteTasa + ", " + iCobrarTasa_P + ")";

                    //EJECUCION DE INSTRUCCION SQL
                    if (!conexionM.ejecutarInstruccionSQL(sSql))
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        return false;
                    }

                    //PROCEDIMINTO PARA EXTRAER EL ID DE LA TABLA CTT_MOVIMIENTO_CAJA
                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    sTabla = "ctt_movimiento_caja";
                    sCampo = "id_ctt_movimiento_caja";

                    iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                    if (iMaximo == -1)
                    {
                        lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>No se pudo obtener el código de la tabla " + sTabla + ".";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        return false;
                    }

                    else
                    {
                        iIdMovimientoCaja = Convert.ToInt32(iMaximo);
                    }

                    //INSTRUCCION INSERTAR EN LA TABLA CTT_NUMERO_MOVIMIENTO_CAJA
                    sSql = "";
                    sSql += "insert into ctt_numero_movimiento_caja (" + Environment.NewLine;
                    sSql += "id_ctt_movimiento_caja, numero_movimiento_caja, estado," + Environment.NewLine;
                    sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += iIdMovimientoCaja + ", " + iNumeroMovimientoCaja + ", 'A', GETDATE()," + Environment.NewLine;
                    sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                    //EJECUCION DE INSTRUCCION SQL
                    if (!conexionM.ejecutarInstruccionSQL(sSql))
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        return false;
                    }

                    //QUERY PARA ACTUALIZAR EL NUMERO DE MOVIMIENTO EN LA TABLA TP_LOCALIDADES_IMPRESORAS
                    sSql = "";
                    sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                    sSql += "numeromovimientocaja = numeromovimientocaja + 1" + Environment.NewLine;
                    sSql += "where id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString());

                    //EJECUCION DE INSTRUCCION SQL
                    if (!conexionM.ejecutarInstruccionSQL(sSql))
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        return false;
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = ex.ToString();
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

        //FUNCION PARA OBTENER EL TOTAL DE TASAS DISPONIBLES
        private void sumaTotalTasasDisponibles()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(sum(isnull(maximo_secuencial, 0)) - sum(isnull(emitidos, 0)), 0) disponibles," + Environment.NewLine;
                sSql += "isnull(sum(isnull(maximo_secuencial, 0) - isnull(anulados, 0)), 0) total" + Environment.NewLine;
                sSql += "from ctt_tasa_token" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and estado_token = 'Abierta'" + Environment.NewLine;
                sSql += "and ambiente_token = " + Convert.ToInt32(Session["emision"].ToString()) + Environment.NewLine;
                sSql += "and validado = 1" + Environment.NewLine;
                sSql += "and id_ctt_oficinista = " + Convert.ToInt32(Session["idUsuario"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    txtCantidadTasasDisponibles.Text = dtConsulta.Rows[0][0].ToString();

                    int iDisponible_T = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                    int iTotal_T = Convert.ToInt32(dtConsulta.Rows[0][1].ToString());

                    if (iTotal_T == 0)
                    {
                        txtPorcentajeDisponibles.Text = "0 %";
                        txtCantidadTasasDisponibles.BackColor = ColorTranslator.FromHtml("#FF0000");
                        txtPorcentajeDisponibles.BackColor = ColorTranslator.FromHtml("#FF0000");
                    }

                    else
                    {
                        int iPorcentajeDisponible_P = (iDisponible_T * 100) / iTotal_T;
                        txtPorcentajeDisponibles.Text = iPorcentajeDisponible_P.ToString() + "%";

                        if (iPorcentajeDisponible_P > 50)
                        {
                            txtCantidadTasasDisponibles.BackColor = ColorTranslator.FromHtml("#00369C");
                            txtPorcentajeDisponibles.BackColor = ColorTranslator.FromHtml("#00369C");
                            txtCantidadTasasDisponibles.ForeColor = Color.White;
                            txtPorcentajeDisponibles.ForeColor = Color.White;
                        }

                        else if (iPorcentajeDisponible_P > 25)
                        {
                            txtCantidadTasasDisponibles.BackColor = ColorTranslator.FromHtml("#F3E212");
                            txtPorcentajeDisponibles.BackColor = ColorTranslator.FromHtml("#F3E212");
                            txtCantidadTasasDisponibles.ForeColor = Color.Black;
                            txtPorcentajeDisponibles.ForeColor = Color.Black;
                        }

                        else if (iPorcentajeDisponible_P > 10)
                        {
                            txtCantidadTasasDisponibles.BackColor = ColorTranslator.FromHtml("#F16A10");
                            txtPorcentajeDisponibles.BackColor = ColorTranslator.FromHtml("#F16A10");
                        }

                        else if (iPorcentajeDisponible_P > 5)
                        {
                            txtCantidadTasasDisponibles.BackColor = ColorTranslator.FromHtml("#A31F11");
                            txtPorcentajeDisponibles.BackColor = ColorTranslator.FromHtml("#A31F11");
                            txtCantidadTasasDisponibles.ForeColor = Color.White;
                            txtPorcentajeDisponibles.ForeColor = Color.White;
                        }

                        if (iPorcentajeDisponible_P >= 0)
                        {
                            txtCantidadTasasDisponibles.BackColor = ColorTranslator.FromHtml("#FF0000");
                            txtPorcentajeDisponibles.BackColor = ColorTranslator.FromHtml("#FF0000");
                            txtCantidadTasasDisponibles.ForeColor = Color.White;
                            txtPorcentajeDisponibles.ForeColor = Color.White;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONSULTAR LOS PARAMETROS DEL SERVIDOR
        private void consultarParametrosTasa()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tasa_terminal, id_oficina, id_cooperativa, servidor_pruebas," + Environment.NewLine;
                sSql += "servidor_produccion, webservice_tasa_credenciales, webservice_tasa_usuario," + Environment.NewLine;
                sSql += "webservice_tasa_anulacion, webservice_verifica_token, webservice_tasa_lote," + Environment.NewLine;
                sSql += "webservice_detalle_transacciones, webservice_tasa_notificacion, emision, valor_tasa," + Environment.NewLine;
                sSql += "notificacion_emergente, adjuntar_tasa_boleto" + Environment.NewLine;
                sSql += "from ctt_tasa_parametros" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {

                    Session["id_tasa_terminal"] = dtConsulta.Rows[0]["id_ctt_tasa_terminal"].ToString();
                    Session["id_tasa_oficina"] = dtConsulta.Rows[0]["id_oficina"].ToString();
                    Session["id_tasa_cooperativa"] = dtConsulta.Rows[0]["id_cooperativa"].ToString();

                    Session["servidor_pruebas"] = dtConsulta.Rows[0]["servidor_pruebas"].ToString();
                    Session["servidor_produccion"] = dtConsulta.Rows[0]["servidor_produccion"].ToString();
                    Session["credenciales"] = dtConsulta.Rows[0]["webservice_tasa_credenciales"].ToString();
                    Session["tasa_usuario"] = dtConsulta.Rows[0]["webservice_tasa_usuario"].ToString();
                    Session["tasa_anulacion"] = dtConsulta.Rows[0]["webservice_tasa_anulacion"].ToString();

                    Session["webservice_verifica_token"] = dtConsulta.Rows[0]["webservice_verifica_token"].ToString();
                    Session["webservice_tasa_lote"] = dtConsulta.Rows[0]["webservice_tasa_lote"].ToString();
                    Session["webservice_detalle_transacciones"] = dtConsulta.Rows[0]["webservice_detalle_transacciones"].ToString();
                    Session["webservice_tasa_notificacion"] = dtConsulta.Rows[0]["webservice_tasa_notificacion"].ToString();

                    Session["emision"] = dtConsulta.Rows[0]["emision"].ToString();
                    Session["valor_tasa_usuario"] = dtConsulta.Rows[0]["valor_tasa"].ToString();
                    Session["notificacion_emergente"] = dtConsulta.Rows[0]["notificacion_emergente"].ToString();
                    Session["adjuntar_tasa_boleto"] = dtConsulta.Rows[0]["adjuntar_tasa_boleto"].ToString();
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

        //FUNCION PARA GENERAR LA TASA DE USUARIO
        private bool generarTasaUsuario()
        {
            try
            {
                sCantidadBoletosToken = txtTasaUsuario.Text.Trim().PadLeft(2, '0');                                
                sTasaUsuario += sCantidadBoletosToken + Session["id_tasa_cooperativa"].ToString().Trim();
                sTasaUsuario += Session["id_tasa_oficina"].ToString().Trim() + sToken.Trim() + sCuentaToken.Trim();
                sTasaUsuario += Session["id_tasa_terminal"].ToString().Trim() + Session["id_tasa_terminal"].ToString();

                Session["tasa_usuario_generado"] = sTasaUsuario;

                return true;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal1').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA CONSULTAR LOS VALORES DE CUENTAS DEL TOKEN
        private bool consultarToken()
        {
            try
            {
                iCantidadTasasEmitir = Convert.ToInt32(txtTasaUsuario.Text.Trim());

                sSql = "";
                sSql += "select token, maximo_secuencial - emitidos disponibles, id_ctt_tasa_token, cuenta" + Environment.NewLine;
                sSql += "from ctt_tasa_token" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and estado_token = 'Abierta'" + Environment.NewLine;
                sSql += "and ambiente_token = " + Convert.ToInt32(Session["emision"].ToString()) + Environment.NewLine;
                sSql += "and validado = 1" + Environment.NewLine;
                sSql += "and id_ctt_oficinista = " + Convert.ToInt32(Session["idUsuario"].ToString()) + Environment.NewLine;
                sSql += "order by id_ctt_tasa_token";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        iCantidadDisponible = Convert.ToInt32(dtConsulta.Rows[i]["disponibles"].ToString());

                        if (iCantidadTasasEmitir <= iCantidadDisponible)
                        {
                            sToken = dtConsulta.Rows[i]["token"].ToString();
                            sCuentaToken = dtConsulta.Rows[i]["cuenta"].ToString().Trim().PadLeft(4, '0');
                            break;
                        }
                    }                    
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //LLAMAR A FUNCION PARA CREAR LA TASA DE USUARIO
                if (generarTasaUsuario() == false)
                {
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA CREAR EL JSON PARA ENVIAR
        private bool crearJson()
        {
            try
            {
                //if (txtIdentificacion.Text.Trim() == "9999999999999")
                //{
                //    sTipoClienteTasa = "07";
                //}

                //else if (txtIdentificacion.Text.Trim().Length == 10)
                //{
                //    sTipoClienteTasa = "05";
                //}

                //else if (txtIdentificacion.Text.Trim().Length == 13)
                //{
                //    sTipoClienteTasa = "04";
                //}

                //else
                //{
                //    sTipoClienteTasa = "06";
                //}

                if (sIdentificacionFactura.Trim() == "9999999999999")
                {
                    sTipoClienteTasa = "07";
                }

                else if (sIdentificacionFactura.Trim().Length == 10)
                {
                    sTipoClienteTasa = "05";
                }

                else if (sIdentificacionFactura.Trim().Length == 13)
                {
                    sTipoClienteTasa = "04";
                }

                else
                {
                    sTipoClienteTasa = "06";
                }


                sObjetoJson = "";
                sObjetoJson += "{" + Environment.NewLine;

                sObjetoOficina = "";
                sObjetoOficina += "\"oficina\": {" + Environment.NewLine;
                sObjetoOficina += "\"id_oficina\": \"" + Session["id_tasa_oficina"].ToString() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_coop\": \"" + Session["id_tasa_cooperativa"].ToString() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_terminal\": \"" + Session["id_tasa_terminal"].ToString() + "\"" + Environment.NewLine;
                sObjetoOficina += "},";

                sObjetoJson += sObjetoOficina + Environment.NewLine;

                sObjetoTasa = "";
                sObjetoTasa += "\"tasa\": {" + Environment.NewLine;
                //sObjetoTasa += "\"cantidad\": \"" + sCantidadBoletosToken.Trim().PadLeft(2, '0') + "\"," + Environment.NewLine;
                //sObjetoTasa += "\"cantidad\": \"" + dtAlmacenar.Rows.Count.ToString().PadLeft(2, '0') + "\"," + Environment.NewLine;
                sObjetoTasa += "\"cantidad\": \"" + txtTasaUsuario.Text.ToString().PadLeft(2, '0') + "\"," + Environment.NewLine;
                sObjetoTasa += "\"secuencial\": \"" + sCuentaToken.Trim() + "\"," + Environment.NewLine;
                sObjetoTasa += "\"token\": \"" + sToken.Trim() + "\"," + Environment.NewLine;
                sObjetoTasa += "\"tipo\": \"1\"," + Environment.NewLine;
                sObjetoTasa += "\"codigo\": \"" + sTasaUsuario + "\"" + Environment.NewLine;
                sObjetoTasa += "},";

                sObjetoJson += sObjetoTasa + Environment.NewLine;

                sObjetoInfo = "";
                sObjetoInfo += "\"info\": {" + Environment.NewLine;
                sObjetoInfo += "\"id_inicio\": \"" + Session["id_pueblo_origen_tasa"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"id_destino\": \"" + Session["id_pueblo_destino_tasa"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_inicio\": \"" + Session["pueblo_destino_tasa"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_destino\": \"" + Session["pueblo_destino_tasa"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_horaSalida\": \"" + txtHoraViaje.Text + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_fechaSalida\": \"" + Convert.ToDateTime(txtFechaViaje.Text.Trim()).ToString("yyyy-MM-dd") + "\"," + Environment.NewLine;
                sObjetoInfo += "\"pasajeros\": \"" + dtAlmacenar.Rows.Count.ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"list_pasajeros\": [" + Environment.NewLine;

                dtAlmacenar = new DataTable();
                dtAlmacenar = Session["dtClientes"] as DataTable;
                
                for (int i = 0; i < dtAlmacenar.Rows.Count; i++)
                {
                    sObjetoInfo += "{" + Environment.NewLine;
                    sObjetoInfo += "\"nombre\": \"" + dtAlmacenar.Rows[i]["NOMBREPASAJERO"].ToString() + "\"," + Environment.NewLine;
                    sObjetoInfo += "\"id\": \"" + dtAlmacenar.Rows[i]["IDENTIFICACION"].ToString() + "\"" + Environment.NewLine;

                    if (i + 1 == dtAlmacenar.Rows.Count)
                    {
                        sObjetoInfo += "}" + Environment.NewLine;
                    }

                    else
                    {
                        sObjetoInfo += "}," + Environment.NewLine;
                    }
                }

                sObjetoInfo += "]," + Environment.NewLine;
                sObjetoInfo += "\"n_bus\": \"" + Session["disco_vehiculo_tasa"].ToString() + "\"" + Environment.NewLine;
                sObjetoInfo += "},";

                //string sIdentificacionJson_P;
                //string sNombrePasajeroJson_P;

                //if (txtIdentificacion.Text.Trim() == "")
                //{
                //    sIdentificacionJson_P = "9999999999999";
                //    sNombrePasajeroJson_P = "CONSUMIDOR FINAL";
                //}

                //else
                //{
                //    sIdentificacionJson_P = txtIdentificacion.Text.Trim();
                //    sNombrePasajeroJson_P = txtNombrePasajero.Text.Trim().ToUpper();
                //}

                if (sCiudad.Trim() == "")
                {
                    sCiudad = Application["ciudad_default"].ToString().ToUpper();
                }

                if (sCorreoElectronico.Trim() == "")
                {
                    sCorreoElectronico = Application["correo_default"].ToString().ToLower();
                }

                if (sTelefono.Trim() == "")
                {
                    sTelefono = Application["telefono_default"].ToString();
                }

                sObjetoJson += sObjetoInfo + Environment.NewLine;

                sObjetoCliente = "";
                sObjetoCliente += "\"cliente\": {" + Environment.NewLine;
                sObjetoCliente += "\"ruc\": \"" + sIdentificacionFactura + "\"," + Environment.NewLine;
                sObjetoCliente += "\"nombre\": \"" + sClienteFactura + "\"," + Environment.NewLine;
                sObjetoCliente += "\"direccion\": \"" + sCiudad.ToUpper() + "\"," + Environment.NewLine;
                sObjetoCliente += "\"correo\": \"" + sCorreoElectronico.ToLower() + "\"," + Environment.NewLine;
                sObjetoCliente += "\"telefono\": \"" + sTelefono + "\"," + Environment.NewLine;
                sObjetoCliente += "\"tipo\": \"" + sTipoClienteTasa + "\"" + Environment.NewLine;
                sObjetoCliente += "}";

                sObjetoJson += sObjetoCliente + Environment.NewLine + "}";
                Session["Json"] = sObjetoJson;

                if (enviarJson() == "ERROR")
                {
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA ENVIAR EL JSON AL SERVIDOR PARA AUTORIZACION
        private string enviarJson()
        {
            try
            {
                string respuestaJson = "";

                if (Session["emision"].ToString() == "0")
                {
                    sUrlEnvio = Session["servidor_pruebas"].ToString() + Session["tasa_usuario"].ToString();
                }

                else
                {
                    sUrlEnvio = Session["servidor_produccion"].ToString() + Session["tasa_usuario"].ToString();            
                }

                //Llamar a funcion para aceptar los certificados de la URL
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                //Declara el objeto con el que haremos la llamada al servicio            
                HttpWebRequest request = WebRequest.Create(sUrlEnvio) as HttpWebRequest;
                //Configurar las propiedad del objeto de llamada
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 5000;

                //Serializar el objeto a enviar. Para esto uso la libreria Newtonsoft
                //string sb = JsonConvert.SerializeObject(sAyuda);
                string sb = Session["Json"].ToString();

                //Convertir el objeto serializado a arreglo de byte
                Byte[] bt = Encoding.UTF8.GetBytes(sb);

                try
                {
                    //Agregar el objeto Byte[] al request
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();

                    //Hacer la llamada
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        //Leer el resultado de la llamada
                        Stream stream1 = response.GetResponseStream();
                        StreamReader sr = new StreamReader(stream1);
                        respuestaJson = sr.ReadToEnd();
                    }

                    TasaUsuario tasa = JsonConvert.DeserializeObject<TasaUsuario>(respuestaJson);

                    sIdTasaRespuesta = tasa.id_tasa;
                    iTasaEmitidaBandera = 1;
                }

                catch (Exception)
                {
                    //sIdTasaRespuesta = "";
                    iTasaEmitidaBandera = 0;
                }

                return "OK";
            }

            catch (Exception)
            {
                return "ERROR";
            }
        }

        //Funcion para aceptar los certificados de la URL
        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
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
                        txtNombrePasajero.Text = dtConsulta.Rows[0][2].ToString();
                        Session["idPasajero"] = dtConsulta.Rows[0][0].ToString();
                        Session["identificacion"] = dtConsulta.Rows[0][1].ToString();
                        cmbTipoIdentificacion.SelectedValue = dtConsulta.Rows[0][6].ToString();
                        iDiscapacidad = Convert.ToInt32(dtConsulta.Rows[0][7].ToString());

                        Session["fechaNacimiento"] = dtConsulta.Rows[0][5].ToString();
                        Session["discapacidad"] = dtConsulta.Rows[0][7].ToString();
                        Session["cgTipoIdentificacion"] = dtConsulta.Rows[0][6].ToString();
                        Session["cgTipoPersona"] = dtConsulta.Rows[0][8].ToString();
                        Session["nombrePersona"] = dtConsulta.Rows[0][9].ToString();
                        Session["apellidoPersona"] = dtConsulta.Rows[0][10].ToString();

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

                        //if (Convert.ToInt32(cmbDestino.SelectedValue) == 0)
                        //{
                        //    txtPrecio.Text = "0.00";
                        //    txtDescuento.Text = "0.00";
                        //    txtPrecioFinal.Text = "0.00";
                        //}

                        //else
                        //{
                        //    consultarPrecio();
                        //}
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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

            catch(Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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

                        //if (dtConsulta.Rows[0][7].ToString() == "1")
                        //{
                        //    chkDiscapacidad.Checked = true;
                        //}

                        //else
                        //{
                        //    chkDiscapacidad.Checked = false;
                        //}
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                        string[,] sIdPedido = new string[dgvDetalle.Rows.Count, 3];
                        int i = 0;

                        foreach(GridViewRow row in dgvDetalle.Rows)
                        {
                            sEstadoPendiente = row.Cells[8].Text.Trim().ToUpper();
                            CheckBox check = row.FindControl("chkSeleccionar") as CheckBox;

                            if (check.Checked == true)
                            {
                                if (sEstadoPendiente == "PAGO PENDIENTE")
                                {
                                    sIdPedido[i, 0] = row.Cells[1].Text;
                                    sIdPedido[i, 1] = row.Cells[6].Text;
                                    sIdPedido[i, 2] = row.Cells[7].Text;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

            fin: { }
        }

        //FUNCION PARA REIMPRIMIR EL INFORME
        private void reimprimirInforme()
        {
            try
            {
                sImprimir = "";

                imprimir.iniciarImpresion();
                imprimir.escritoEspaciadoCorto(sImprimir);
                imprimir.cortarPapel(1);
                imprimir.imprimirReporte("ASCII");
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
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

            catch(Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        ////FUNCION PARA VALIDAR EL COBRO
        //private void validarCobro()
        //{
        //    try
        //    {
        //        dbTotalCobradoCierre = Convert.ToDouble(txtTotalCobrado.Text.Trim());
        //        dbPagoAdministracionCierre = Convert.ToDouble(txtPagoAdministracion.Text.Trim());
        //        dbDescuentoCierre = Convert.ToDouble(txtDescuentoCierre.Text.Trim());

        //        dbTotalCierre = dbTotalCobradoCierre - dbDescuentoCierre;

        //        if (dbTotalCierre <= 0)
        //        {
        //            //lblMensaje.Text = "Valor igual o menor a cero cobrado menos descuento: $" + dbTotalCierre.ToString("N2");
        //            //ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#dialogCancelNotification').modal('show');</script>", false);
        //            chkRealizarCobro.Checked = false;
        //            chkRealizarCobro.Enabled = false;

        //            Session["realizarCierre"] = "0";

        //            goto fin;
        //        }

        //        dbValorRetencionCierre = (Convert.ToDouble(Application["porcentaje_retencion"].ToString()) * dbTotalCierre) / 100;              

        //        dbTotalCierre = dbTotalCierre - dbValorRetencionCierre;

        //        if (dbTotalCierre <= 0)
        //        {
        //            //lblMensaje.Text = "Valor igual o menor a cero total menos retencion: $" + dbTotalCierre.ToString("N2");
        //            //ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#dialogCancelNotification').modal('show');</script>", false);
        //            chkRealizarCobro.Checked = false;
        //            chkRealizarCobro.Enabled = false;

        //            Session["realizarCierre"] = "0";

        //            goto fin;
        //        }

        //        dbTotalCierre = dbTotalCierre - dbPagoAdministracionCierre;

        //        if (dbTotalCierre <= 0)
        //        {
        //            //lblMensaje.Text = "Valor igual o menor a cero total menos administracion: $" + dbTotalCierre.ToString("N2");
        //            //ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#dialogCancelNotification').modal('show');</script>", false);
        //            chkRealizarCobro.Checked = false;
        //            chkRealizarCobro.Enabled = false;

        //            Session["realizarCierre"] = "0";

        //            goto fin;
        //        }
                
        //        //lblMensaje.Text = "Entregar: $" + dbTotalCierre.ToString("N2");
        //        //ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#dialogCancelNotification').modal('show');</script>", false);
        //        chkRealizarCobro.Checked = true;
        //        chkRealizarCobro.Enabled = true;

        //        Session["realizarCierre"] = "1";

        //        goto fin;

        //    }

        //    catch(Exception ex)
        //    {
        //        lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
        //    }

        //    fin: { }
        //}

        //FUNCION DE RECALCULO DE VALORES
        private void recalcularValoresXX()
        {
            try
            {
                extraerTotalCobrado();
                double dbTotalCobrado_P = Convert.ToDouble(txtTotalCobradoModal.Text.Trim());
                double dbPorcentajeRetencion_P = Convert.ToDouble(Session["porcentaje_retencion"].ToString()) / 100;
                txtPagoRetencionModal.Text = (dbTotalCobrado_P * dbPorcentajeRetencion_P).ToString("N2");
                double dbPagoRetencion_P = Convert.ToDouble(txtPagoRetencionModal.Text.Trim());
                double num4 = dbTotalCobrado_P - dbPagoRetencion_P;
                txtPrimerTotalModal.Text = num4.ToString("N2");
                txtPagoModal.Text = "0.00";
                txtSegundoTotalModal.Text = num4.ToString("N2");
                txtTotalNetoModal.Text = num4.ToString("N2");

                //double dbTotalCobrado_P = Convert.ToDouble(txtTotalCobradoModal.Text.Trim());
                //double dbIngresoEfectivo_P = Convert.ToDouble(txtEfectivoModal.Text.Trim());
                //double dbPorcentajeRetencion_P = Convert.ToDouble(Session["porcentaje_retencion"].ToString()) / 100;
                //double dbPagoRetencion_P = dbTotalCobrado_P * dbPorcentajeRetencion_P;

                //txtPagoRetencionModal.Text = dbPagoRetencion_P.ToString("N2");
                //dbPagoRetencion_P = Convert.ToDouble(txtPagoRetencionModal.Text.Trim());

                //double dbPrimerTotal_P = dbTotalCobrado_P + dbIngresoEfectivo_P - dbPagoRetencion_P;
                //double dbPagoAdministracion_P = Convert.ToDouble(txtPagoModal.Text.Trim());

                ////BREVE VALIDACION PARA PAGAR A ADMINISTRACION
                //if (dbPrimerTotal_P < dbPagoAdministracion_P)
                //{
                //    txtPagoModal.Text = "0.00";
                //    txtPagoModal.BorderStyle = BorderStyle.Groove;
                //    txtPagoModal.BackColor = Color.Red;
                //    txtPagoModal.ForeColor = Color.White;
                //    dbPagoAdministracion_P = 0;                    
                //}

                //else
                //{
                //    txtPagoModal.BackColor = Color.White;
                //    txtPagoModal.ForeColor = Color.Black;
                //}

                //double dbSegundoTotal_P = dbPrimerTotal_P - dbPagoAdministracion_P;
                //double dbPagosPendientes_P = 0;

                //if (dgvDetalle.Rows.Count == 0)
                //{
                //    dbPagosPendientes_P = 0;
                //}

                //else
                //{
                //    columnasGridPendiente(true);

                //    //RECORRER EL GRIDVIEW
                //    foreach (GridViewRow row in dgvDetalle.Rows)
                //    {
                //        CheckBox check = row.FindControl("chkSeleccionar") as CheckBox;

                //        if (check.Checked == true)
                //        {
                //            dbAbonoGrid = Convert.ToDouble(row.Cells[5].Text);
                //            dbValorRealGrid = Convert.ToDouble(row.Cells[6].Text);

                //            dbPagosPendientes_P = dbPagosPendientes_P + (dbValorRealGrid - dbAbonoGrid);
                //        }

                //    }

                //    columnasGridPendiente(false);
                //}

                //double dbTotalNeto_P = 0;

                //if (dbSegundoTotal_P < dbPagosPendientes_P)
                //{
                //    txtPagosPendientesModal.Text = "0.00";
                //    dbPagosPendientes_P = 0;

                //    if (Session["idVehiculoReemplazo"].ToString() != "0")
                //    {
                //        llenarGridPendientes();
                //    }
                //}

                //else
                //{
                //    txtPagosPendientesModal.Text = dbPagosPendientes_P.ToString("N2");
                    
                //}

                //double dbPendiente_P;

                //dbTotalNeto_P = dbSegundoTotal_P - dbPagosPendientes_P;
                //txtPrimerTotalModal.Text = dbPrimerTotal_P.ToString("N2");
                //txtSegundoTotalModal.Text = dbSegundoTotal_P.ToString("N2");
                //txtTotalNetoModal.Text = dbTotalNeto_P.ToString("N2");

                //if (Convert.ToDouble(Session["pago_administracion"].ToString()) < dbPrimerTotal_P)
                //{
                //    dbPendiente_P = 0;
                //}

                //else
                //{
                //    dbPendiente_P = Convert.ToDouble(Session["pago_administracion"].ToString()) - dbPrimerTotal_P;
                //}

                //txtFaltanteModal.Text = dbPendiente_P.ToString("N2");
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        private void recalcularValoresExtrasXX()
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
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }


        //FUNCION DE RECALCULO DE VALORES VIAJE NORMAL
        private void recalcularValoresNormalesXX()
        {
            try
            {
                extraerTotalCobrado();

                double dbTotalCobrado_P;
                double dbIngresoEfectivo_P;
                double dbPorcentajeRetencion_P;
                double dbPagoAdministracion_P;
                double dbValorRetencion_P;
                double dbPrimerSubtotal_P;
                double dbSegundoSubtotal_P;
                double dbValorAdminTextbox_P;
                double dbPagosPendientes_P;
                double dbValorValorPendiente_P;

                //VER PAGOS PENDIENTES
                if (dgvDetalle.Rows.Count == 0)
                {
                    dbPagosPendientes_P = 0;
                }

                else
                {
                    columnasGridPendiente(true);
                    dbPagosPendientes_P = 0;

                    //RECORRER EL GRIDVIEW
                    foreach (GridViewRow row in dgvDetalle.Rows)
                    {
                        CheckBox check = row.FindControl("chkSeleccionar") as CheckBox;

                        if (check.Checked == true)
                        {
                            //dbAbonoGrid = Convert.ToDouble(row.Cells[5].Text);
                            dbValorValorPendiente_P = Convert.ToDouble(row.Cells[6].Text);

                            dbPagosPendientes_P += dbValorValorPendiente_P;
                        }

                    }

                    columnasGridPendiente(false);
                }

                txtPagosPendientesModal.Text = dbPagosPendientes_P.ToString("N2");

                //FIN PAGOS PENDIENTES

                dbTotalCobrado_P = Convert.ToDouble(txtTotalCobradoModal.Text.Trim());
                dbIngresoEfectivo_P = Convert.ToDouble(txtEfectivoModal.Text.Trim());
                dbPorcentajeRetencion_P = Convert.ToDouble(Session["porcentaje_retencion"].ToString()) / 100;
                dbPagoAdministracion_P = Convert.ToDouble(Session["pago_administracion"].ToString());                
                dbValorRetencion_P = dbTotalCobrado_P * dbPorcentajeRetencion_P;
                txtPagoRetencionModal.Text = dbValorRetencion_P.ToString("N2");
                dbValorRetencion_P = Convert.ToDouble(txtPagoRetencionModal.Text.Trim());
                dbPrimerSubtotal_P = dbTotalCobrado_P - dbValorRetencion_P;

                //txtPagoRetencionModal.Text = dbValorRetencion_P.ToString("N2");
                txtPrimerTotalModal.Text = dbPrimerSubtotal_P.ToString("N2");
                dbPrimerSubtotal_P = Convert.ToDouble(txtPrimerTotalModal.Text.Trim());

                if (dbPrimerSubtotal_P + dbIngresoEfectivo_P - dbPagosPendientes_P >= dbPagoAdministracion_P)
                {
                    txtPagoModal.Text = dbPagoAdministracion_P.ToString("N2");
                    txtFaltanteModal.Text = "0.00";
                    txtPagoModal.ForeColor = Color.Black;
                    //rdbPagado.Checked = true;
                    //rdbPendiente.Checked = false;
                    //rdbPagoParcial.Checked = false;
                }

                else
                {
                    txtPagoModal.Text = "0.00";
                    txtFaltanteModal.Text = (dbPagoAdministracion_P - (dbPrimerSubtotal_P + dbIngresoEfectivo_P - dbPagosPendientes_P)).ToString("N2");
                    txtPagoModal.ForeColor = Color.Red;
                    //rdbPagado.Checked = false;
                    //rdbPendiente.Checked = true;
                    //rdbPagoParcial.Checked = false;
                }

                dbValorAdminTextbox_P = Convert.ToDouble(txtPagoModal.Text.Trim());
                dbSegundoSubtotal_P = dbPrimerSubtotal_P + dbIngresoEfectivo_P - dbValorAdminTextbox_P;
                txtSegundoTotalModal.Text = dbSegundoSubtotal_P.ToString("N2");
                dbPagosPendientes_P = Convert.ToDouble(txtPagosPendientesModal.Text.Trim());
                
                if (dbSegundoSubtotal_P < dbPagosPendientes_P)
                {
                    txtTotalNetoModal.Text = (dbSegundoSubtotal_P).ToString("N2");
                }

                else
                {
                    txtTotalNetoModal.Text = (dbSegundoSubtotal_P - dbPagosPendientes_P).ToString("N2");
                }
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        ////FUNCION DE RECALCULO DE VALORES VIAJE EXTRA
        //private void recalcularValoresExtras()
        //{
        //    try
        //    {
        //        extraerTotalCobrado();

        //        double dbTotalCobrado_P;
        //        double dbPorcentajeRetencion_P;
        //        double dbValorRetencion_P;
        //        double dbPrimerSubtotal_P;

        //        dbTotalCobrado_P = Convert.ToDouble(txtTotalCobradoModal.Text.Trim());
        //        dbPorcentajeRetencion_P = Convert.ToDouble(Session["porcentaje_retencion"].ToString()) / 100;
        //        dbValorRetencion_P = dbTotalCobrado_P * dbPorcentajeRetencion_P;
        //        txtPagoRetencionModal.Text = dbValorRetencion_P.ToString("N2");
        //        dbValorRetencion_P = Convert.ToDouble(txtPagoRetencionModal.Text.Trim());
        //        dbPrimerSubtotal_P = dbTotalCobrado_P - dbValorRetencion_P;
        //        txtPrimerTotalModal.Text = dbPrimerSubtotal_P.ToString("N2");
        //        txtPagoModal.Text = "0.00";
        //        txtSegundoTotalModal.Text = dbPrimerSubtotal_P.ToString("N2");
        //        txtTotalNetoModal.Text = dbPrimerSubtotal_P.ToString("N2");
        //    }

        //    catch (Exception ex)
        //    {
        //        cerrarModal();
        //        lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
        //    }
        //}

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
            //dgvDetalle.Columns[0].ItemStyle.Width = 70;
            //dgvDetalle.Columns[2].ItemStyle.Width = 75;
            //dgvDetalle.Columns[3].ItemStyle.Width = 120;
            //dgvDetalle.Columns[4].ItemStyle.Width = 90;
            //dgvDetalle.Columns[5].ItemStyle.Width = 100;
            //dgvDetalle.Columns[6].ItemStyle.Width = 100;

            dgvDetalle.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDetalle.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDetalle.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDetalle.Columns[5].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDetalle.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Center;

            dgvDetalle.Columns[7].Visible = ok;
            //dgvDetalle.Columns[8].Visible = ok;
        }

        //FUNCION PARA LLENAR EL GRID DE PAGOS PENDIENTES
        private void llenarGridPendientes()
        {
            try
            {
                //sSql = "";
                //sSql += "select * from ctt_vw_pagos_pendientes_itinerario" + Environment.NewLine;
                //sSql += "where id_ctt_vehiculo = " + Convert.ToInt32(Session["idVehiculo"].ToString());

                //columnasGridPendiente(true);
                //pendienteE.ISQL = sSql;
                //dgvDetalle.DataSource = pendienteM.listarPagosPendientes(pendienteE);
                //dgvDetalle.DataBind();
                //columnasGridPendiente(false);

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
                        dbSumaValores += Convert.ToDecimal(dgvDetalle.Rows[i].Cells[6].Text);
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

            catch(Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
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

                    if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                    {
                        sumaTotalTasasDisponibles();
                        contarTasasToken();
                        pnlVerTasas.Visible = true;
                    }

                    else
                    {
                        pnlVerTasas.Visible = false;
                    }
                }

                columnasGrid(false);
                goto fin;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                    int iTercerDigito = Convert.ToInt32(txtIdentificacionRegistro.Text.Trim().Substring(0,2));
                    
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


            //if (txtIdentificacion.Text.Trim() == "")
            //{
            //    Session["idPasajero"] = null;
            //    llenarGridClientes(0, 0);
            //    btnPopUp_ModalPopupExtender.Show();
                
            //}

            //else
            //{
            //    buscarCliente(txtIdentificacion.Text.Trim());
            //}            
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
                //Session["cgTipoPersona"].ToString();
                
                //if (Convert.ToInt32(Session["discapacidad"].ToString()) == 1)
                //{
                //    chkDiscapacidad.Checked = true;
                //}

                //else
                //{
                //    chkDiscapacidad.Checked = false;
                //}

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
                //Session["factorDescuento"] = "0";
            }

            else
            {
                consultarPrecio();
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

            catch(Exception)
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
                    //if (chkDiscapacidad.Checked == true)
                    //{
                    //    iDiscapacidad = 1;
                    //}

                    //else
                    //{
                    //    iDiscapacidad = 0;
                    //}

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
                lblMensajeError.Text = ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                if (consultarRealizarCobros() == true)
                {
                    pnlBus.Visible = false;
                    pnlAsientos.Visible = false;
                    pnlCierreViaje.Visible = true;

                    //if (Convert.ToInt32(Session["extra"].ToString()) == 0)
                    //{
                    //    txtPagoModal.Text = Convert.ToDouble(Session["pago_administracion"].ToString()).ToString("N2");
                    //}

                    //else
                    //{
                    //    txtPagoModal.Text = Convert.ToDouble(Application["precio_producto_extra"].ToString()).ToString("N2");
                    //}

                    //INSTRUCCIONES DEL VEHICULO DE REEMPLAZO
                    //------------------------------------------------------------------------------------------------
                    //if (Convert.ToInt32(Session["idVehiculoReemplazo"].ToString()) == 0)
                    //{
                    //    //dgvDetalle.Visible = true;
                    //    llenarGridPendientes();
                    //}
                    //else
                    //{
                    //    //dgvDetalle.Visible = false;
                    //}
                    //------------------------------------------------------------------------------------------------

                    llenarGridPendientes();
                    recalcularValoresNormales();


                    //if (Convert.ToInt32(Session["extra"].ToString()) == 0)
                    //{
                    //    recalcularValoresNormales();
                    //    //btnAbonarAdministracion.Enabled = true;
                    //}

                    //else
                    //{
                    //    recalcularValoresExtras();
                    //    //btnAbonarAdministracion.Enabled = false;
                    //}

                    //rdbPendiente.Checked = true;
                    lblEtiquetaCierre.Text = Session["etiqueta_viaje"].ToString();
                }
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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


            //if (Convert.ToInt32(cmbDestino.SelectedValue) == 0)
            //{
            //    txtPrecio.Text = "0.00";
            //    txtDescuento.Text = "0.00";
            //    txtPrecioFinal.Text = "0.00";
            //}

            //else
            //{
            //    manejarTipoClienteManual(Convert.ToInt32(cmbTipoCliente.SelectedValue));               

            //}
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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

            //iPorcentajeNotificacionEntero = 67; 
            //lblMensajeNotificacion.Text = "Has consumido el " + (100 - iPorcentajeNotificacionEntero).ToString() + "% de la cantidad de tasas de usuario";
            //lblDatosMensajeNotificacion.Text = "Hoy " + DateTime.Now.ToString("dd-MM-yyyy") + " a las " + DateTime.Now.ToString("HH:mm") + ".</br>Se le notifica que solo dispone de:";
            //lblCantidadMensajeNotificacion.Text = "45"; //Convert.ToInt32(dbDisponible_Notificacion).ToString();

            ////lblMensajeNotificacion.ForeColor = Color.White;

           

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalNotify').modal('show');</script>", false);
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

            lblSumaCobrar.Text = "Total a Cobrar: " + (dbSumaPendientes_P + dbPagoAdministracionRecuperado_P).ToString("N2") + " $";

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
            //if (Convert.ToInt32(Session["extra"].ToString()) == 0)
            //{
            //    recalcularValoresNormales();
            //}

            //else
            //{
            //    recalcularValoresExtras();
            //}

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvVendidos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvVendidos.PageIndex = e.NewPageIndex;
            llenarGridVendidos(e.NewPageIndex + 1);
        }

        protected void btnGenerarTasaAcompanante_Click(object sender, EventArgs e)
        {
            if (txtCantidadTasasAcompanate.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese la cantidad pasajeros que usarán la tasa de acompañante.', 'info');", true);
                txtCantidadTasasAcompanate.Text = "1";
            }

            else if (Convert.ToInt32(txtCantidadTasasAcompanate.Text.Trim()) == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese un número diferente de cero para geenrar la tasa de acompañante.', 'info');", true);
                txtCantidadTasasAcompanate.Text = "1";
            }

            else
            {
                int iCantidadTasas = Convert.ToInt32(txtCantidadTasasAcompanate.Text.Trim());
                tasaAcompanante.tasa(iCantidadTasas.ToString(), cmbOrigen.SelectedValue, cmbOrigen.SelectedItem.ToString(),
                                     cmbOrigen.SelectedValue, cmbOrigen.SelectedItem.ToString(), txtFechaViaje.Text.Trim(),
                                     txtHoraViaje.Text.Trim(), sDatosMaximo);

                txtCantidadTasasAcompanate.Text = "1";
            }
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

                    if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                    {
                        sumaTotalTasasDisponibles();
                        contarTasasToken();
                        pnlVerTasas.Visible = true;
                    }

                    else
                    {
                        pnlVerTasas.Visible = false;
                    }
                }

                columnasGridExtra(false);
                goto fin;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

        fin: { }
        }

        protected void btnIngresarFaltante_Click(object sender, EventArgs e)
        {
            //double dbTotal_1_P;
            //double dbAdmin_P;
            //double dbPendientes_P;
            //double dbAgregar_P;

            //if (Convert.ToInt32(Session["extra"].ToString()) == 0)
            //{
            //    if (Convert.ToDouble(txtFaltanteModal.Text.Trim()) == 0)
            //    {
            //        txtEfectivoModal.Text = "0.00";
            //    }

            //    else
            //    {
            //        dbTotal_1_P = Convert.ToDouble(txtPrimerTotalModal.Text.Trim());
            //        dbAdmin_P = Convert.ToDouble(Session["pago_administracion"].ToString());
            //        dbPendientes_P = Convert.ToDouble(txtPagosPendientesModal.Text.Trim());
            //        dbAgregar_P = dbAdmin_P - dbTotal_1_P + dbPendientes_P;

            //        txtEfectivoModal.Text = dbAgregar_P.ToString("N2");
            //    }

            //    recalcularValoresNormales();
            //}

            //else
            //{
            //    txtEfectivoModal.Text = "0.00";
            //    recalcularValoresExtras();
            //}

            if (Convert.ToDecimal(txtFaltanteModal.Text.Trim()) == 0)
            {
                return;
            }

            Decimal dbPrimerTotal_P;
            Decimal dbPagoAdministracion_P;
            Decimal dbIngresoEfectivo_P;
            Decimal dbSegundoTotal_P;
            Decimal dbFaltante_P;
            Decimal dbPagosPendientes_P;
            Decimal dbTotalNeto_P;            

            txtEfectivoModal.Text = txtFaltanteModal.Text.Trim();
            txtFaltanteModal.Text = "0.00";

            dbPrimerTotal_P = Convert.ToDecimal(txtPrimerTotalModal.Text.Trim());
            dbPagoAdministracion_P = Convert.ToDecimal(txtPagoModal.Text.Trim());
            dbIngresoEfectivo_P = Convert.ToDecimal(txtEfectivoModal.Text.Trim());
            dbSegundoTotal_P = dbPrimerTotal_P - dbPagoAdministracion_P + dbIngresoEfectivo_P;
            txtSegundoTotalModal.Text = dbSegundoTotal_P.ToString("N2");
            dbSegundoTotal_P = Convert.ToDecimal(txtSegundoTotalModal.Text.Trim());
            dbFaltante_P = Convert.ToDecimal(txtFaltanteModal.Text.Trim());
            dbPagosPendientes_P = Convert.ToDecimal(txtPagosPendientesModal.Text.Trim());
            dbTotalNeto_P = dbSegundoTotal_P - dbPagosPendientes_P;
            txtTotalNetoModal.Text = dbTotalNeto_P.ToString("N2");
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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

        //OPCIONES NUEVAS DE TASAS DE USUARIO
        //INICIO DE PROCESO: 2019-07-11

        #region FUNCIONES DEL USUARIO PARA NUEVAS TASAS DE USUARIO

        //FUNCION PARA CREAR EL JSON PARA ENVIAR
        private bool crearJsonValidarToken()
        {
            try
            {                
                sObjetoJson = "";
                sObjetoJson += "{" + Environment.NewLine;

                sObjetoOficina = "";
                sObjetoOficina += "\"oficina\": {" + Environment.NewLine;
                sObjetoOficina += "\"id_oficina\": \"" + Session["id_tasa_oficina"].ToString() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_coop\": \"" + Session["id_tasa_cooperativa"].ToString() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_terminal\": \"" + Session["id_tasa_terminal"].ToString() + "\"" + Environment.NewLine;
                sObjetoOficina += "}," + Environment.NewLine;
                sObjetoOficina += "\"token\": \"" + txtNumeroTokenModal.Text.Trim() + "\"";
                sObjetoOficina += "}";

                sObjetoJson += sObjetoOficina;

                Session["Json"] = sObjetoJson;

                if (enviarJsonValidarToken() == "ERROR")
                {
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                ModalPopupExtender_ValidaToken.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA ENVIAR EL JSON AL SERVIDOR PARA VALIDAR EL TOKEN
        private string enviarJsonValidarToken()
        {
            try
            {
                string respuestaJson = "";

                if (Session["emision"].ToString() == "0")
                {
                    sUrlEnvio = Session["servidor_pruebas"].ToString() + Session["webservice_verifica_token"].ToString();
                }

                else
                {
                    sUrlEnvio = Session["servidor_produccion"].ToString() + Session["webservice_verifica_token"].ToString();
                }

                //Llamar a funcion para aceptar los certificados de la URL
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                //Declara el objeto con el que haremos la llamada al servicio            
                HttpWebRequest request = WebRequest.Create(sUrlEnvio) as HttpWebRequest;
                //Configurar las propiedad del objeto de llamada
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 5000;

                //Serializar el objeto a enviar. Para esto uso la libreria Newtonsoft
                //string sb = JsonConvert.SerializeObject(sAyuda);
                string sb = Session["Json"].ToString();

                //Convertir el objeto serializado a arreglo de byte
                Byte[] bt = Encoding.UTF8.GetBytes(sb);

                try
                {
                    //Agregar el objeto Byte[] al request
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();

                    //Hacer la llamada
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        //Leer el resultado de la llamada
                        Stream stream1 = response.GetResponseStream();
                        StreamReader sr = new StreamReader(stream1);
                        respuestaJson = sr.ReadToEnd();
                    }

                    TasaVerificaToken verifica = JsonConvert.DeserializeObject<TasaVerificaToken>(respuestaJson);

                    if (verifica.Usar == true)
                    {
                        lblTituloValidacionModal.ForeColor = Color.Lime;
                        lblTituloValidacionModal.Text = "CÓDIGO VÁLIDO";
                        lblMensajeValidacionModal.Text = "se han acreditado " + verifica.Cantidad.ToString() + " tasas de usuario a su sistema</br>de venta de boletería.";
                        sumaTotalTasasDisponibles();

                        if (validarToken.insertarToken(txtNumeroTokenModal.Text.Trim(), Convert.ToInt32(verifica.Cantidad), Convert.ToInt32(Session["emision"].ToString()), sDatosMaximo, Convert.ToInt32(Session["idUsuario"].ToString())) == false)
                        {
                            ModalPopupExtender_ValidaToken.Hide();                            
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo registrar el token en la base de datos local. Comuníquese con el administrador del sistema', 'error');", true);
                        }                        
                    }

                    else
                    {
                        if (verifica.Error.ToString() == "El token ya fue activado con anterioridad.")
                        {
                            lblTituloValidacionModal.ForeColor = Color.Red;
                            lblTituloValidacionModal.Text = "INFORMACIÓN";
                            lblMensajeValidacionModal.Text = verifica.Error.ToString();
                        }

                        else
                        {
                            lblTituloValidacionModal.ForeColor = Color.Red;
                            lblTituloValidacionModal.Text = "CÓDIGO INCORRECTO";
                            lblMensajeValidacionModal.Text = "Por favor verifique e intente nuevamente,</br>en última instancia notifique al EPMMOP.";
                        }
                    }
                }

                catch (Exception)
                {
                    lblTituloValidacionModal.ForeColor = Color.Red;
                    lblTituloValidacionModal.Text = "INFORMACION.";
                    lblMensajeValidacionModal.Text = "Error con el servidor remoto.</br>Favor intente nuevamente.";
                    sIdTasaRespuesta = "";
                    iTasaEmitidaBandera = 0;
                    return "ERROR";
                }

                return "OK";
            }

            catch (Exception)
            {
                return "ERROR";
            }
        }

        //FUNCION PARA CONSULTAR LOS TOKEN GENERADO
        private void llenarGridTokenReporte()
        {
            try
            {
                sSql = "";
                sSql += "select top 10 token, convert(varchar, fecha_generacion, 103) fecha_compra," + Environment.NewLine;
                sSql += "substring(convert(varchar, fecha_generacion, 108), 1, 5) hora, maximo_secuencial, " + Environment.NewLine;
                sSql += "emitidos utilizados, maximo_secuencial - emitidos disponibles" + Environment.NewLine;
                sSql += "from ctt_tasa_token" + Environment.NewLine;
                sSql += "where estado_token = 'Abierta'" + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "and validado = 1" + Environment.NewLine;
                sSql += "order by fecha_generacion desc";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    dgvReporteTokenInfo.DataSource = dtConsulta;
                    dgvReporteTokenInfo.DataBind();

                    dgvReporteTokenInfo.Columns[0].ItemStyle.Width = 100;
                    dgvReporteTokenInfo.Columns[1].ItemStyle.Width = 190;
                    dgvReporteTokenInfo.Columns[2].ItemStyle.Width = 100;
                    dgvReporteTokenInfo.Columns[3].ItemStyle.Width = 100;
                    dgvReporteTokenInfo.Columns[4].ItemStyle.Width = 100;
                    dgvReporteTokenInfo.Columns[5].ItemStyle.Width = 100;

                    Decimal dbCantidad_T;
                    Decimal dbDisponible_T;
                    Decimal dbPorcentaje_T;

                    //AQUI VALIDACION DE COLORES
                    foreach(GridViewRow row in dgvReporteTokenInfo.Rows)
                    {
                        dbCantidad_T = Convert.ToDecimal(row.Cells[3].Text);
                        Label lblDisponible = row.FindControl("lblDisponibleTasasModal") as Label;
                        dbDisponible_T = Convert.ToDecimal(lblDisponible.Text.Trim());
                        dbPorcentaje_T = (dbDisponible_T * 100) / dbCantidad_T;
                        lblDisponible.ToolTip = dbDisponible_T.ToString("N0") + " Tasas de Usuario - " + dbPorcentaje_T.ToString("N0") + " % disponible";
                        lblDisponible.Attributes.Add("class", "badge btn-block");

                        if (dbPorcentaje_T > 50)
                        {
                            lblDisponible.BackColor = ColorTranslator.FromHtml("#00369C");
                        }

                        else if (dbPorcentaje_T >= 25)
                        {

                            lblDisponible.BackColor = ColorTranslator.FromHtml("#F3E212");
                            lblDisponible.ForeColor = Color.Black;
                        }

                        else if (dbPorcentaje_T >= 10)
                        {
                            lblDisponible.BackColor = ColorTranslator.FromHtml("#F16A10");
                        }

                        else if (dbPorcentaje_T >= 5)
                        {
                            lblDisponible.BackColor = ColorTranslator.FromHtml("#A31F11");
                        }

                        else if (dbPorcentaje_T >= 0)
                        {
                            lblDisponible.BackColor = ColorTranslator.FromHtml("#FF0000");
                        }
                    }
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCIONES DEL USUARIO PARA NOTIFICACIONES AUTOMATICAS

        //FUNCION PARA CONSULTAR LOS VALORES DE CADA TOKEN
        private void consultarDatosToken()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(sum(isnull(maximo_secuencial, 0)), 0) suma_total," + Environment.NewLine;
                sSql += "isnull(sum(isnull(maximo_secuencial, 0)) - sum(isnull(emitidos, 0)), 0) disponibles" + Environment.NewLine;
                sSql += "from ctt_tasa_token" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and estado_token = 'Abierta'" + Environment.NewLine;
                sSql += "and ambiente_token = " + Convert.ToInt32(Session["emision"].ToString()) + Environment.NewLine;
                sSql += "and validado = 1" + Environment.NewLine;
                sSql += "and id_ctt_oficinista = " + Convert.ToInt32(Session["idUsuario"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    dbCantidad_Notificacion = Convert.ToDecimal(dtConsulta.Rows[0]["suma_total"].ToString());
                    dbDisponible_Notificacion = Convert.ToDecimal(dtConsulta.Rows[0]["disponibles"].ToString());

                    if (dbCantidad == 0)
                    {
                        dbPorcentaje_Notificacion = 0;
                    }

                    else
                    {
                        dbPorcentaje_Notificacion = (dbDisponible_Notificacion * 100) / dbCantidad_Notificacion;
                    }

                    iPorcentajeNotificacionEntero = Convert.ToInt32(dbPorcentaje_Notificacion);

                    if ((iPorcentajeNotificacionEntero == 0) || ((iPorcentajeNotificacionEntero >= 5) && (iPorcentajeNotificacionEntero <= 6)) || ((iPorcentajeNotificacionEntero >= 9) && (iPorcentajeNotificacionEntero <= 11)) || ((iPorcentajeNotificacionEntero >= 24) && (iPorcentajeNotificacionEntero <= 26)) || ((iPorcentajeNotificacionEntero >= 49) && (iPorcentajeNotificacionEntero <= 51)))
                    {
                        Session["dbCantidad_Notificacion"] = dbCantidad_Notificacion.ToString();
                        Session["dbDisponible_Notificacion"] = dbDisponible_Notificacion.ToString();
                        Session["iPorcentajeNotificacionEntero"] = iPorcentajeNotificacionEntero.ToString();

                        string sFecha_Notificacion = DateTime.Now.ToString();
                        Session["ver_notificacion"] = "1";

                        Session["lblMensajeNotificacion"] = "Hoy " + Convert.ToDateTime(sFecha_Notificacion).ToString("dd-MM-yyyy") + " a las " + Convert.ToDateTime(sFecha_Notificacion).ToString("HH:mm") + ".</br>Se le notifica que solo dispone de:";
                        ((Master)Master).mostrarNotificacionEmergente();
                    }
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CREAR EL JSON PARA LAS NOTIFICACIONES
        private bool crearJsonNotificaciones()
        {
            try
            {
                sObjetoJson = "";
                sObjetoJson += "{" + Environment.NewLine;

                sObjetoOficina = "";
                sObjetoOficina += "\"oficina\": {" + Environment.NewLine;
                sObjetoOficina += "\"id_oficina\": \"" + Session["id_tasa_oficina"].ToString() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_coop\": \"" + Session["id_tasa_cooperativa"].ToString() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_terminal\": \"" + Session["id_tasa_terminal"].ToString() + "\"" + Environment.NewLine;
                sObjetoOficina += "}," + Environment.NewLine;

                sObjetoJson += sObjetoOficina;

                sObjetoNotificacion = "";
                sObjetoNotificacion += "\"notificacion\": {" + Environment.NewLine;
                sObjetoNotificacion += "\"cant_restante\": \"" + dbDisponible_Notificacion.ToString("N0") + "\"," + Environment.NewLine;
                sObjetoNotificacion += "\"mensaje\": \"" + "Por favor recargue, tiene " + iPorcentajeNotificacionEntero + "% de tasas restantes." + "\"" + Environment.NewLine;
                sObjetoNotificacion += "}" + Environment.NewLine;

                sObjetoJson += sObjetoNotificacion + "}";

                Session["Json"] = sObjetoJson;

                if (Convert.ToInt32(Session["notificacion_emergente"].ToString()) == 0)
                {
                    if (enviarJsonNotificaciones() == "ERROR")
                    {
                        return false;
                    }
                }

                else
                {
                    string str = DateTime.Now.ToString();
                    Session["ver_notificacion"] = "1";
                    Session["lblMensajeNotificacion"] = "Hoy " + Convert.ToDateTime(str).ToString("dd-MM-yyyy") + " a las " + Convert.ToDateTime(str).ToString("HH:mm") + ".</br>Se le notifica que solo dispone de:";
                    ((Master)Master).mostrarNotificacionEmergente();
                }

                return true;
            }

            catch (Exception ex)
            {
                ModalPopupExtender_ValidaToken.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA ENVIAR EL JSON AL SERVIDOR PARA REGISTRAR UNA NOTIFICACION
        private string enviarJsonNotificaciones()
        {
            try
            {
                string respuestaJson = "";

                if (Session["emision"].ToString() == "0")
                {
                    sUrlEnvio = Session["servidor_pruebas"].ToString() + Session["webservice_tasa_notificacion"].ToString();
                }

                else
                {
                    sUrlEnvio = Session["servidor_produccion"].ToString() + Session["webservice_tasa_notificacion"].ToString();
                }

                //Llamar a funcion para aceptar los certificados de la URL
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                //Declara el objeto con el que haremos la llamada al servicio            
                HttpWebRequest request = WebRequest.Create(sUrlEnvio) as HttpWebRequest;
                //Configurar las propiedad del objeto de llamada
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 5000;

                //Serializar el objeto a enviar. Para esto uso la libreria Newtonsoft
                //string sb = JsonConvert.SerializeObject(sAyuda);
                string sb = Session["Json"].ToString();

                //Convertir el objeto serializado a arreglo de byte
                Byte[] bt = Encoding.UTF8.GetBytes(sb);

                try
                {
                    //Agregar el objeto Byte[] al request
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();

                    //Hacer la llamada
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        //Leer el resultado de la llamada
                        Stream stream1 = response.GetResponseStream();
                        StreamReader sr = new StreamReader(stream1);
                        respuestaJson = sr.ReadToEnd();
                    }

                    Clases_Tasa_Usuario.TasaNotificacion notificacion = JsonConvert.DeserializeObject<Clases_Tasa_Usuario.TasaNotificacion>(respuestaJson);

                    insertarNotificacion(notificacion.Error, Convert.ToInt32(notificacion.IdNotificacion));
                }

                catch {}

                return "OK";
            }

            catch (Exception)
            {
                return "ERROR";
            }
        }

        //FUNCION PARA INSERTAR LA NOTIFICACION EN LA BASE DE DATOS
        private void insertarNotificacion(string sError_P, int iIdNotificacion_P)
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    return;
                }

                sSql = "";
                sSql += "insert into ctt_tasa_notificaciones (" + Environment.NewLine;
                sSql += "id_ctt_jornada, id_ctt_oficinista, id_localidad, cantidad, disponible," + Environment.NewLine;
                sSql += "porcentaje, id_notificacion, error, fecha_notificacion, hora_notificacion," + Environment.NewLine;
                sSql += "ambiente_notificacion, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Session["idJornada"].ToString()) + ", " + Convert.ToInt32(Session["idUsuario"].ToString())  + ", ";
                sSql += Convert.ToInt32(Application["idLocalidad"].ToString()) + ", " + Convert.ToInt32(Session["dbCantidad_Notificacion"].ToString()) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(Session["dbDisponible_Notificacion"].ToString()) + ", " + Convert.ToInt32(Session["iPorcentajeNotificacionEntero"].ToString()) + ", ";
                sSql += iIdNotificacion_P + ", '" + sError_P.Trim() + "', '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + Environment.NewLine;
                sSql += "GETDATE(), " + Convert.ToInt32(Session["emision"].ToString()) + ", 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    conexionM.reversaTransaccion();
                    return;
                }

                conexionM.terminaTransaccion();
                Session["Json"] = null;
                Session["dbCantidad_Notificacion"] = null;
                Session["dbDisponible_Notificacion"] = null;
                Session["iPorcentajeNotificacionEntero"] = null;
                Session["ver_notificacion"] = "0";
                return;
            }

            catch (Exception ex)
            {
                conexionM.reversaTransaccion();
                return;
            }
        }

        #endregion

        protected void btnCerrarModalValidarToken_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_ValidaToken.Hide();
        }

        protected void btnValidacionToken_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_ValidaToken.Show();
            lblTituloValidacionModal.Text = "";
            lblMensajeValidacionModal.Text = "";
            txtNumeroTokenModal.Text = "";
            txtNumeroTokenModal.Focus();
        }

        protected void btnValidarTokenModal_Click(object sender, EventArgs e)
        {
            if (conexionInternet() == false)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No hay conexión a Internet para verificar el TOKEN.', 'error');", true);
            }

            else
            {
                consultarParametrosTasa();
                crearJsonValidarToken();
                sumaTotalTasasDisponibles();
                contarTasasToken();
            }
        }

        protected void btnAbrirModalInfoToken_Click(object sender, EventArgs e)
        {
            consultarParametrosTasa();
            txtCodigoOficina.Text = Session["id_tasa_oficina"].ToString();
            ModalPopupExtender_InfoToken.Show();
        }

        protected void btnContinuarToken_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_ValidaToken.Hide();
        }

        protected void btnCerrarModalInfoToken_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_InfoToken.Hide();
        }

        protected void btnCerrarModalReporteTokenInfo_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_ReporteTokenInfo.Hide();
        }

        protected void btnReporteToken_Click(object sender, EventArgs e)
        {
            if (conexionInternet() == false)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No hay conexión a Internet para realizar la consulta.', 'error');", true);
            }

            else
            {
                llenarGridTokenReporte();
                ModalPopupExtender_ReporteTokenInfo.Show();
            }
        }

        protected void dgvReporteTokenInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvReporteTokenInfo.PageIndex = e.NewPageIndex;            
        }

        protected void btnCerrarModalNotificacionAutomatica_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_NotificacionAutomatica.Hide();
        }

        protected void btnOkNotificacionAutomatica_Click(object sender, EventArgs e)
        {
            if (chkConfirmacionVisualizacion.Checked == false)
            {
                chkConfirmacionVisualizacion.ForeColor = Color.Red;
                chkConfirmacionVisualizacion.Focus();
            }

            else
            {
                enviarJsonNotificaciones();
                chkConfirmacionVisualizacion.Checked = false;
                chkConfirmacionVisualizacion.ForeColor = Color.Black;
                ModalPopupExtender_NotificacionAutomatica.Hide();
            }
        }

        protected void btnNotificacionToken_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_NotificacionAutomatica.Show();
        }
    }
}