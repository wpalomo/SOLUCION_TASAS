using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ENTIDADES;
using NEGOCIO;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;
using Microsoft.Reporting.WebForms;

namespace Solution_CTT
{
    public class TasaUsuarioAnula
    {
        public object[] Error { get; set; }
        public long id_tasa { get; set; }
    }

    public partial class frmDevolucionBoletos : System.Web.UI.Page
    {        
        ENTViajes asignarE = new ENTViajes();
        ENTVendidos vendidoE = new ENTVendidos();
        ENTFacturasItinerario facturaE = new ENTFacturasItinerario();

        manejadorConexion conexionM = new manejadorConexion();
        manejadorVendidos vendidoM = new manejadorVendidos();
        manejadorAsignarViaje asignarM = new manejadorAsignarViaje();
        manejadorFacturasItinerario facturaM = new manejadorFacturasItinerario();

        Clases.ClaseParametros parametros = new Clases.ClaseParametros();
        Clases.ClaseReporteBoleto reporte = new Clases.ClaseReporteBoleto();
        Clases.ClaseImpresion imprimir = new Clases.ClaseImpresion();

        string sSql;
        string sEstadoViaje;
        string sFecha;
        string sTabla;
        string sCampo;
        string sCiudad;
        string sDireccion;
        string sTelefono;
        string sCorreoElectronico;
        string sImprimir;
        string sPathImpresora;
        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;

        bool bRespuesta;
        bool bEstadoColumna;

        int iCuentaRegistros;
        int iTotalRegistros;
        int iIdPersona;
        int iIdPersonaDetPedido;
        int iIdPedido;
        int iNumeroPedido;
        int iIdCabDespachos;
        int iIdDespachoPedido;
        int iIdEventoCobro;
        int iIdTipoCliente;
        int iIdAsiento;
        int iIdProducto;
        int iIdDetalleRuta;
        int iIdDocumentoCobrar;
        int iIdPago;
        int iNumeroPago;
        int iIdDocumentoPago;
        int iIdFactura;
        int iNumeroFactura;
        int iCortarPapel;
        int iAbrirCajon;
        int iNumeroMovimientoCaja;
        int iIdMovimientoCaja;
        int iTasaEmitidaBandera;
        int iIdTasaAnulada;
        int iManejaFacturacionElectronica;
        int iEmiteTasaUsuario;
        int iAmbienteTasa;
        int iNuevaCantidadTasas;
        int iNuevoNumeroCantidadToken;
        int iCantidadDisponible;
        int iIdFormaPagoFactura;
        int iBanderaSincronizarTasasAnuladas;
        int iBanderaMensajeEmite_P;
        int iBanderaMensajeAnula_P;
        int iBanderaMensajeToken_P;

        int iCgTipoDocumento = 7456;
        int iCgEstadoDctoPorCobrar = 7461;

        decimal dbValorTasa;

        double dbPrecioUnitario;
        double dbDescuento;
        double dbCantidad;
        double dbIva;
        double dbServicio;
        double dbTotal;
        
        long iMaximo;

        //TASAS DE USUARIO
        string sObjetoOficina;
        string sObjetoTasa;
        string sObjetoInfo;
        string sObjetoCliente;
        string sObjetoJson;
        string sUrlCredenciales;
        string sUrlEnvio;
        string sUrlAnula;
        string sToken;
        string sCuentaToken;
        string sTasaUsuario;
        string sCantidadBoletosToken;
        string sTipoClienteTasa;
        string sIdTasaRespuesta;
        string respuestaJson;
        string strToken_P;

        //VARIABLES DEL REPORTE
        int iVendidos_REP;
        int iCuenta_REP;
        int iPorcentajeNotificacionEntero;

        decimal dbCantidad_REP;
        decimal dbPrecioUnitario_REP;
        decimal dbDescuento_REP;
        decimal dbIva_REP;
        decimal dbSumaTotal_REP;

        string sNumeroFactura_REP;
        string sAsientos_REP;
        string sTasaUsuarioRecuperado_REP;

        Decimal dbCantidad_Notificacion;
        Decimal dbDisponible_Notificacion;
        Decimal dbPorcentaje_Notificacion;        

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

            Session["modulo"] = "MÓDULO DE DEVOLUCIÓN Y ANULACIÓN DE FACTURAS";

            if (!IsPostBack)
            {
                consultarParametrosTasa();

                sFecha = DateTime.Now.ToString("dd/MM/yyyy");
                txtDate.Text = sFecha;
                llenarGrid(sFecha);
                llenarGridExtras(sFecha);
                Session["idVehiculo"] = null;
                Session["idProgramacion"] = null;
            }

            //else
            //{
            //    if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
            //    {
            //        consultarParametrosTasa();
            //    }
            //}
        }

        #region FUNCIONES RECUPERADAS PARA INTEGRACION

        //FUNCION PARA ACTUALIZAR LAS TASAS PENDIENTES
        private bool actualizarTasaPendiente()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return false;
                }

                sSql = "";
                sSql += "update cv403_facturas set" + Environment.NewLine;
                sSql += "tasa_emitida = 1," + Environment.NewLine;
                sSql += "id_tasa_emitida = " + sIdTasaRespuesta + Environment.NewLine;
                sSql += "from cv403_facturas" + Environment.NewLine;
                sSql += "where id_factura = " + Convert.ToInt32(Session["idFacturaAnular"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    conexionM.reversaTransaccion();
                    return false;
                }

                conexionM.terminaTransaccion();
                return true;
            }

            catch (Exception)
            {
                conexionM.reversaTransaccion();
                return false;
            }
        }

        //FUNCION PARA CONSULTAR LOS DATOS DEL TOKEN
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

                    if (dbCantidad_Notificacion == 0)
                    {
                        dbPorcentaje_Notificacion = 0;
                    }

                    else
                    {
                        dbPorcentaje_Notificacion = (dbDisponible_Notificacion * 100) / dbCantidad_Notificacion;
                    }

                    iPorcentajeNotificacionEntero = Convert.ToInt32(dbPorcentaje_Notificacion);

                    if ((((iPorcentajeNotificacionEntero == 0) || ((iPorcentajeNotificacionEntero >= 5) && (iPorcentajeNotificacionEntero <= 6))) || ((iPorcentajeNotificacionEntero >= 9) && (iPorcentajeNotificacionEntero <= 11))) || (((iPorcentajeNotificacionEntero >= 24) && (iPorcentajeNotificacionEntero <= 26)) || ((iPorcentajeNotificacionEntero >= 48) && (iPorcentajeNotificacionEntero <= 52))))
                    {
                        Session["dbCantidad_Notificacion"] = dbCantidad_Notificacion.ToString();
                        Session["dbDisponible_Notificacion"] = dbDisponible_Notificacion.ToString();
                        Session["iPorcentajeNotificacionEntero"] = iPorcentajeNotificacionEntero.ToString();
                        string str = DateTime.Now.ToString();
                        Session["ver_notificacion"] = "1";
                        Session["lblMensajeNotificacion"] = "Hoy " + Convert.ToDateTime(str).ToString("dd-MM-yyyy") + " a las " + Convert.ToDateTime(str).ToString("HH:mm") + ".</br>Se le notifica que solo dispone de:";
                        ((Master)Master).mostrarNotificacionEmergente();
                    }
                }

                else
                {
                    cerrarModal();
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

        //FUNCION PARA CREAR EL JSON TASAS PENDIENTES
        private bool crearJsonTasaPendiente()
        {
            try
            {
                if (Session["identificacion_cliente"].ToString().Trim() == "9999999999999")
                {
                    sTipoClienteTasa = "07";
                }

                else if (Session["identificacion_cliente"].ToString().Trim().Length == 10)
                {
                    sTipoClienteTasa = "05";
                }

                else if (Session["identificacion_cliente"].ToString().Trim().Length == 13)
                {
                    sTipoClienteTasa = "04";
                }

                else
                {
                    sTipoClienteTasa = "06";
                }

                string sTasaUsuarioEmitida_P = Session["tasa_usuario_emitida"].ToString();
                string sCantidad_P = sTasaUsuarioEmitida_P.Trim().Substring(0, 2);
                string sSecuencialUnico_P = sTasaUsuarioEmitida_P.Trim().Substring(14, 4);
                string sTokenUnico_P = sTasaUsuarioEmitida_P.Trim().Substring(9, 5);

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
                sObjetoTasa += "\"cantidad\": \"" + sCantidad_P + "\"," + Environment.NewLine;
                sObjetoTasa += "\"secuencial\": \"" + sSecuencialUnico_P.Trim() + "\"," + Environment.NewLine;
                sObjetoTasa += "\"token\": \"" + sTokenUnico_P.Trim() + "\"," + Environment.NewLine;
                sObjetoTasa += "\"tipo\": \"1\"," + Environment.NewLine;
                sObjetoTasa += "\"codigo\": \"" + sTasaUsuarioEmitida_P + "\"" + Environment.NewLine;
                sObjetoTasa += "},";

                sObjetoJson += sObjetoTasa + Environment.NewLine;

                sObjetoInfo = "";
                sObjetoInfo += "\"info\": {" + Environment.NewLine;
                sObjetoInfo += "\"id_inicio\": \"" + Session["id_pueblo_origen_tasa"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"id_destino\": \"" + Session["id_pueblo_destino_tasa"] + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_inicio\": \"" + Session["pueblo_origen_tasa"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_destino\": \"" + Session["pueblo_destino_tasa"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_horaSalida\": \"" + Session["fecha_viaje"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_fechaSalida\": \"" + Session["hora_salida"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"pasajeros\": \"" + sCantidad_P.Trim() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"list_pasajeros\": [" + Environment.NewLine;


                int iSuma_P = 0;
                int num2 = Convert.ToInt32(sCantidad_P);

                foreach (GridViewRow row in this.dgvDetalle.Rows)
                {
                    sObjetoInfo += "{" + Environment.NewLine;
                    sObjetoInfo += "\"nombre\": \"" + row.Cells[11].Text + "\"," + Environment.NewLine;
                    sObjetoInfo += "\"id\": \"" + row.Cells[10].Text + "\"" + Environment.NewLine;
                    iSuma_P++;

                    if (num2 == iSuma_P)
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

                sObjetoJson += sObjetoInfo + Environment.NewLine;

                sCiudad = Application["ciudad_default"].ToString().ToUpper();
                sCorreoElectronico = Application["correo_default"].ToString().ToLower();
                sTelefono = Application["telefono_default"].ToString();

                sObjetoCliente = "";
                sObjetoCliente += "\"cliente\": {" + Environment.NewLine;
                sObjetoCliente += "\"ruc\": \"" + Session["identificacion_cliente"].ToString() + "\"," + Environment.NewLine;
                sObjetoCliente += "\"nombre\": \"" + Session["nombre_cliente"].ToString() + "\"," + Environment.NewLine;
                sObjetoCliente += "\"direccion\": \"" + sCiudad.ToUpper() + "\"," + Environment.NewLine;
                sObjetoCliente += "\"correo\": \"" + sCorreoElectronico.ToLower() + "\"," + Environment.NewLine;
                sObjetoCliente += "\"telefono\": \"" + sTelefono + "\"," + Environment.NewLine;
                sObjetoCliente += "\"tipo\": \"" + sTipoClienteTasa + "\"" + Environment.NewLine;
                sObjetoCliente += "}";

                sObjetoJson += sObjetoCliente + Environment.NewLine + "}";
                Session["Json"] = sObjetoJson;

                enviarJsonTasaPendiente();

                if (this.sIdTasaRespuesta != "0")
                {
                    this.actualizarTasaPendiente();
                }

                return true;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA ENVIAR EL JSON DE TASAS PENDIENTES
        private string enviarJsonTasaPendiente()
        {
            try
            {
                if (Session["ambiente_tasa_usuario_actual"].ToString() == "0")
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

                    TasaUsuario usuario = JsonConvert.DeserializeObject<TasaUsuario>(respuestaJson);
                    sIdTasaRespuesta = usuario.id_tasa;
                    iBanderaMensajeEmite_P = 1;
                }

                catch (Exception)
                {
                    iBanderaMensajeEmite_P = 2;
                    sIdTasaRespuesta = "0";
                }

                Session["id_tasa_usuario_emitida"] = sIdTasaRespuesta;
                return "OK";
            }

            catch (Exception)
            {
                iBanderaMensajeEmite_P = 0;
                return "ERROR";
            }
        }

        //FUNCION PARA REVERSAR LA CUENTA DEL TOKEN
        private bool reversarCuentaToken()
        {
            try
            {
                strToken_P = Session["tasa_usuario_emitida"].ToString().Substring(9, 5);

                int num = Convert.ToInt32(Session["tasa_usuario_emitida"].ToString().Substring(0, 2));

                if (!conexionM.iniciarTransaccion())
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar iniciar la transacci\x00f3n.', 'danger');", true);
                    return false;
                }

                sSql = "";
                sSql += "update ctt_tasa_token set" + Environment.NewLine;
                sSql += "emitidos = emitidos - "  + num + Environment.NewLine;
                sSql += "where token = '" + strToken_P + "'" + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "and validado = 1" + Environment.NewLine;
                sSql += "and ambiente_token = " + Convert.ToInt32(Session["emision"].ToString());

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + this.sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    conexionM.reversaTransaccion();
                    return false;
                }

                conexionM.terminaTransaccion();
                return true;
            }

            catch (Exception)
            {
                conexionM.reversaTransaccion();
                return false;
            }
        }

        #endregion

        #region FUNCIONES PARA IMPRIMIR

        //FUNCION PARA IMPRIMIR DIRECTAMENTE EL REPORT VIEWER
        private void crearReporteImprimir()
        {
            try
            {
                sSql = "";
                sSql += "select establecimiento, punto_emision, numero_factura, fecha_ingreso," + Environment.NewLine;
                sSql += "identificacion, isnull(nombres, '') + ' ' + apellidos cliente, descripcion_ruta," + Environment.NewLine;
                sSql += "tipo_viaje, fecha_viaje, hora_salida, descripcion_anden, descripcion_disco," + Environment.NewLine;
                sSql += "isnull(tasa_usuario, '') tasa_usuario, cantidad, precio_unitario, valor_dscto, valor_iva, clave_acceso," + Environment.NewLine;
                sSql += "oficinista, numero_asiento, '' as valor_total, '' as vendidos, '' as asientos, '' as secuencia_factura," + Environment.NewLine;
                sSql += "destino" + Environment.NewLine;
                sSql += "from ctt_vw_factura" + Environment.NewLine;
                sSql += "where id_pedido = " + Session["id_pedido_nuevo"].ToString();

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sNumeroFactura_REP = dtConsulta.Rows[0][0].ToString() + "-" + dtConsulta.Rows[0][1].ToString() + "-" + dtConsulta.Rows[0][2].ToString().PadLeft(9, '0');
                        iVendidos_REP = dtConsulta.Rows.Count;

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
                        sSql += "where id_pedido = " + iIdPedido + Environment.NewLine;
                        sSql += "group by tipo_cliente";

                        DataTable dt2 = ds.Tables["dtTarifas"];
                        dt2.Clear();

                        bRespuesta = conexionM.consultarRegistro(sSql, dt2);

                        if (bRespuesta == true)
                        {
                            LocalReport reporteLocal = new LocalReport();

                            if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                            {
                                if (Convert.ToInt32(Session["adjuntar_tasa_dev"].ToString()) == 1)
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

        #region FUNCIONES DEL USUARIO PARA LAS TASAS DE USUARIO

        //FUNCION PARA CONSULTAR LOS PARAMETROS DEL SERVIDOR
        private void consultarParametrosTasa()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tasa_parametro, id_ctt_tasa_terminal, id_oficina, id_cooperativa," + Environment.NewLine;
                sSql += "servidor_pruebas, servidor_produccion, webservice_tasa_anulacion, emision," + Environment.NewLine;
                sSql += "valor_tasa, permite_anular_tasa, webservice_tasa_usuario, webservice_verifica_token," + Environment.NewLine;
                sSql += "webservice_tasa_usuario, notificacion_emergente, adjuntar_tasa_boleto" + Environment.NewLine;
                sSql += "from ctt_tasa_parametros" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    Session["id_ctt_tasa_parametro"] = dtConsulta.Rows[0]["id_ctt_tasa_parametro"].ToString();
                    Session["id_tasa_terminal"] = dtConsulta.Rows[0]["id_ctt_tasa_terminal"].ToString();
                    Session["id_tasa_oficina"] = dtConsulta.Rows[0]["id_oficina"].ToString();
                    Session["id_tasa_cooperativa"] = dtConsulta.Rows[0]["id_cooperativa"].ToString();
                    Session["servidor_pruebas"] = dtConsulta.Rows[0]["servidor_pruebas"].ToString();
                    Session["servidor_produccion"] = dtConsulta.Rows[0]["servidor_produccion"].ToString();
                    Session["tasa_anulacion"] = dtConsulta.Rows[0]["webservice_tasa_anulacion"].ToString();
                    Session["emision"] = dtConsulta.Rows[0]["emision"].ToString();
                    Session["valor_tasa"] = dtConsulta.Rows[0]["valor_tasa"].ToString();
                    Session["permite_anular_tasa"] = dtConsulta.Rows[0]["permite_anular_tasa"].ToString();
                    Session["webservice_tasa_usuario"] = dtConsulta.Rows[0]["webservice_tasa_usuario"].ToString();
                    Session["webservice_verifica_token"] = dtConsulta.Rows[0]["webservice_verifica_token"].ToString();
                    Session["tasa_usuario"] = dtConsulta.Rows[0]["webservice_tasa_usuario"].ToString();
                    Session["notificacion_emergente_dev"] = dtConsulta.Rows[0]["notificacion_emergente"].ToString();
                    Session["adjuntar_tasa_dev"] = dtConsulta.Rows[0]["adjuntar_tasa_boleto"].ToString();
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

        //FUNCION PARA CONSULTAR LOS VALORES DE CUENTAS DEL TOKEN
        private bool consultarToken()
        {
            try
            {
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

                        if (iNuevaCantidadTasas <= iCantidadDisponible)
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

        //FUNCION PARA GENERAR LA TASA DE USUARIO
        private bool generarTasaUsuario()
        {
            try
            {
                sCantidadBoletosToken = iNuevaCantidadTasas.ToString().Trim().PadLeft(2, '0');
                sTasaUsuario += sCantidadBoletosToken + Session["id_tasa_cooperativa"].ToString().Trim();
                sTasaUsuario += Session["id_tasa_oficina"].ToString().Trim() + sToken.Trim() + sCuentaToken.Trim();
                sTasaUsuario += Session["id_tasa_terminal"].ToString().Trim() + "1";

                Session["tasa_usuario_generado"] = sTasaUsuario;

                return true;
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeModal.Text = ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal1').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA CREAR EL JSON PARA ENVIAR
        private bool crearJson()
        {
            try
            {
                if (Session["identificacion_cliente"].ToString().Trim() == "9999999999999")
                {
                    sTipoClienteTasa = "07";
                }

                else if (Session["identificacion_cliente"].ToString().Trim().Length == 10)
                {
                    sTipoClienteTasa = "05";
                }

                else if (Session["identificacion_cliente"].ToString().Trim().Length == 13)
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
                sObjetoTasa += "\"cantidad\": \"" + iNuevaCantidadTasas.ToString().Trim().PadLeft(2, '0') + "\"," + Environment.NewLine;
                sObjetoTasa += "\"secuencial\": \"" + sCuentaToken.Trim() + "\"," + Environment.NewLine;
                sObjetoTasa += "\"token\": \"" + sToken.Trim() + "\"," + Environment.NewLine;
                sObjetoTasa += "\"tipo\": \"1\"," + Environment.NewLine;
                sObjetoTasa += "\"codigo\": \"" + sTasaUsuario + "\"" + Environment.NewLine;
                sObjetoTasa += "},";

                sObjetoJson += sObjetoTasa + Environment.NewLine;

                sObjetoInfo = "";
                sObjetoInfo += "\"info\": {" + Environment.NewLine;
                sObjetoInfo += "\"id_inicio\": \"" + Session["id_pueblo_origen_tasa"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"id_destino\": \"" + Session["id_pueblo_destino_tasa"] + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_inicio\": \"" + Session["pueblo_origen_tasa"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_destino\": \"" + Session["pueblo_destino_tasa"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_horaSalida\": \"" + Session["fecha_viaje"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_fechaSalida\": \"" + Session["hora_salida"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"pasajeros\": \"" + sCantidadBoletosToken.Trim() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"list_pasajeros\": [" + Environment.NewLine;

                int iSuma_P = 0;

                //INSTRCCIONES PARA INSERTAR EN LA TABLA CV403_DET_PEDIDOS
                foreach (GridViewRow row in dgvDetalle.Rows)
                {
                    CheckBox check = row.FindControl("chkSeleccionar") as CheckBox;

                    if (check.Checked == false)
                    {
                        sObjetoInfo += "{" + Environment.NewLine;
                        sObjetoInfo += "\"nombre\": \"" + row.Cells[11].Text + "\"," + Environment.NewLine;
                        sObjetoInfo += "\"id\": \"" + row.Cells[10].Text + "\"" + Environment.NewLine;
                        iSuma_P++;

                        if (iNuevaCantidadTasas == iSuma_P)
                        {
                            sObjetoInfo += "}" + Environment.NewLine;
                        }

                        else
                        {
                            sObjetoInfo += "}," + Environment.NewLine;
                        }
                    }
                }

                sObjetoInfo += "]," + Environment.NewLine;
                sObjetoInfo += "\"n_bus\": \"" + Session["disco_vehiculo_tasa"].ToString() + "\"" + Environment.NewLine;
                sObjetoInfo += "},";

                sObjetoJson += sObjetoInfo + Environment.NewLine;

                sCiudad = Application["ciudad_default"].ToString().ToUpper();
                sCorreoElectronico = Application["correo_default"].ToString().ToLower();
                sTelefono = Application["telefono_default"].ToString();

                //if (sCiudad.Trim() == "")
                //{
                //    sCiudad = Application["ciudad_default"].ToString().ToUpper();
                //}

                //if (sCorreoElectronico.Trim() == "")
                //{
                //    sCorreoElectronico = Application["correo_default"].ToString().ToLower();
                //}

                //if (sTelefono.Trim() == "")
                //{
                //    sTelefono = Application["telefono_default"].ToString();
                //}

                sObjetoCliente = "";
                sObjetoCliente += "\"cliente\": {" + Environment.NewLine;
                sObjetoCliente += "\"ruc\": \"" + Session["identificacion_cliente"].ToString() + "\"," + Environment.NewLine;
                sObjetoCliente += "\"nombre\": \"" + Session["nombre_cliente"].ToString() + "\"," + Environment.NewLine;
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
                    iBanderaMensajeEmite_P = 1;
                }

                catch(Exception)
                {
                    iBanderaMensajeEmite_P = 0;
                    sIdTasaRespuesta = "0";
                }

                return "OK";
            }

            catch (Exception)
            {
                iBanderaMensajeEmite_P = 0;
                return "ERROR";
            }
        }

        //Funcion para aceptar los certificados de la URL
        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        //FUNCION PARA CREAR EL JSON PARA ENVIAR
        private bool crearJsonEliminar()
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
                sObjetoOficina += "},";

                sObjetoJson += sObjetoOficina + Environment.NewLine;

                sObjetoTasa = "";
                sObjetoTasa += "\"tasa\": {" + Environment.NewLine;
                sObjetoTasa += "\"id_tasa\": \"" + Session["id_tasa_usuario_emitida"].ToString() + "\"," + Environment.NewLine;
                sObjetoTasa += "\"n_tasa\": \"" + Session["tasa_usuario_emitida"].ToString() + "\"" + Environment.NewLine;
                sObjetoTasa += "}";

                sObjetoJson += sObjetoTasa + Environment.NewLine;
                sObjetoJson += "}";
                
                Session["JsonElimina"] = sObjetoJson;

                if (enviarJsonElimina() == "ERROR")
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
        private string enviarJsonElimina()
        {
            try
            {
                if (Session["emision"].ToString() == "0")
                {
                    sUrlEnvio = Session["servidor_pruebas"].ToString() + Session["tasa_anulacion"].ToString();
                }

                else
                {
                    sUrlEnvio = Session["servidor_produccion"].ToString() + Session["tasa_anulacion"].ToString();
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
                string sb = Session["JsonElimina"].ToString();

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

                    TasaUsuarioAnula resultado = JsonConvert.DeserializeObject<TasaUsuarioAnula>(respuestaJson);

                    Session["id_tasa_anulada"] = resultado.id_tasa.ToString();
                    iBanderaMensajeAnula_P = 1;
                }

                catch(Exception)
                {
                    iBanderaMensajeAnula_P = 0;
                    Session["id_tasa_anulada"] = "0";
                }

                return "OK";
            }

            catch (Exception)
            {
                iBanderaMensajeAnula_P = 0;
                return "ERROR";
            }
        }

        //FUNCION PARA CERRAR LOS MODAL
        private void cerrarModal()
        {
            btnPopUp_ModalPopupExtender.Hide();
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

        #endregion

        #region FUNCIONES DEL USUARIO PARA ANULAR LA FACTURA O DETALLE DE LA FACTURA

        //  FUNCION PARA PROCESAR LA ELIMINACION
        //  1. ELIMINA LA FACTURA Y GENERA UNA NUEVA CON LOS ITEMS VIGENTES
        //  2. ELIMINA TODA LA FACTURA ELIMINANDO TODO EL PEDIDO        

        private void procesarFactura(int iOp, int iIdFactura_P, int iIdPedido_P, int iRestaAsientos)
        {
            try
            {
                //PRIMERO VERIFICAR LA TASA DE USUARIO
                if (anularTasaUsuario() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ocurrió un problema al intentar anular una tasa de usuario.', 'danger');", true);
                    return;
                }


                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                //  1. GENERAR UN NUEVO PEDIDO CON LOS ITEMS QUE SE MANTENDRÁN

                if (iOp == 1)
                {
                    if (insertarPedido() == false)
                    {
                        goto fin;
                    }

                    if (insertarPagos() == false)
                    {
                        goto fin;
                    }

                    if (insertarFactura() == false)
                    {
                        goto fin;
                    }
                }

                //  SE PROCEDE A ELIMINAR EL REGISTRO ANTERIOR

                if (eliminarPedido(iIdPedido_P) == false)
                {
                    goto fin;
                }

                if (eliminarPagos() == false)
                {
                    goto fin;
                }

                if (eliminarFactura(iIdFactura_P) == false)
                {
                    goto fin;
                }

                //INSTRUCCION SQL PARA ACTUALIZAR EL NUMERO DE ASIENTOS
                sSql = "";
                sSql += "update ctt_programacion set" + Environment.NewLine;
                sSql += "asientos_ocupados = asientos_ocupados - " + iRestaAsientos + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + Convert.ToInt32(Session["idProgramacion"]) + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    conexionM.reversaTransaccion();
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto fin;
                }

                conexionM.terminaTransaccion();

                if (iOp == 1)
                {
                    crearReporteImprimir();
                }

                btnPopUp_ModalPopupExtender.Hide();

                if (Convert.ToInt32(this.Session["genera_tasa_usuario"].ToString()) == 1)
                {
                    if (iOp == 1)
                    {
                        if ((this.iIdTasaAnulada != 0) || (this.iIdTasaAnulada != -1))
                        {
                            if (((iBanderaMensajeToken_P == 1) && (iBanderaMensajeAnula_P == 1)) && (iBanderaMensajeEmite_P == 1))
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Nueva factura emitida éxitosamente', 'success');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Nueva factura emitida éxitosamente. Sincronización en segundo plano. Devolución de tasa usuario no permitida.', 'success');", true);
                            }
                        }

                        else if (((iBanderaMensajeToken_P == 1) && (iBanderaMensajeAnula_P == 1)) && (iBanderaMensajeEmite_P == 1))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Nueva factura emitida éxitosamente. La tasa de usuario excedió el límite de tiempo para anular o ya fue utilizada.', 'info');", true);
                        }

                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Nueva factura emitida éxitosamente. Sincronización en segundo plano. Devolución de tasa usuario no permitida.', 'success');", true);
                        }
                    }

                    else if ((iIdTasaAnulada != 0) || (iIdTasaAnulada != -1))
                    {
                        if ((iBanderaMensajeToken_P == 2) && (iBanderaMensajeAnula_P == 2))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Factura anulada éxitosamente. Sincronización en segundo plano. Devolución de tasa usuario no permitida.', 'success');", true);
                        }

                        else if ((iBanderaMensajeToken_P == 1) && (iBanderaMensajeAnula_P == 1))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Factura anulada éxitosamente. Tasa de usuario anulada éxitosamente.', 'success');", true);
                        }

                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Factura anulada éxitosamente. Devolución de tasa usuario no permitida.', 'success');", true);
                        }
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Factura anulada éxitosamente. La tasa de usuario se encuentra pendiente a sincronizar.', 'info');", true);
                    }

                    if (Convert.ToInt32(Session["notificacion_emergente_dev"].ToString()) == 1)
                    {
                        consultarDatosToken();
                    }
                }

                else if (iOp == 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('\x00c9xito.!', 'Nueva factura emitida \x00e9xitosamente', 'success');", true);
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('\x00c9xito.!', 'Factura anulada \x00e9xitosamente', 'success');", true);
                }

                llenarGridVendidos(0);

                goto fin;
            }

            catch(Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

            fin: { };
        }


        //INSERTAR FASE 1 -  CREAR PEDIDO
        private bool insertarPedido()
        {
            try
            {
                sFecha = DateTime.Now.ToString("yyyy/MM/dd");
                iIdPersona = Convert.ToInt32(Session["idPersonaAnular"].ToString());
                dbTotal = 0;

                for (int i = 0; i < dgvDetalle.Rows.Count; i++)
                {
                    dbTotal = dbTotal + Convert.ToDouble(dgvDetalle.Rows[i].Cells[15].Text);
                }

                //INSTRUCCION PARA INSERTAR EN LA TABLA CV403_CAB_PEDIDOS
                sSql = "";
                sSql += "insert into cv403_cab_pedidos (" + Environment.NewLine;
                sSql += "idempresa, cg_empresa, id_localidad, fecha_pedido, id_persona, cg_tipo_cliente," + Environment.NewLine;
                sSql += "cg_moneda, porcentaje_iva, id_vendedor, cg_estado_pedido, porcentaje_dscto," + Environment.NewLine;
                sSql += "cg_facturado, id_ctt_programacion, id_ctt_oficinista," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                sSql += "cobro_boletos, cobro_retencion, cobro_administrativo, bandera_boleteria," + Environment.NewLine;
                sSql += "id_ctt_jornada, id_ctt_pueblo_origen, id_ctt_pueblo_destino, id_ctt_cierre_caja)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idEmpresa"].ToString()) + ", " + Convert.ToInt32(Application["cgEmpresa"].ToString()) + ", ";
                sSql += Convert.ToInt32(Application["idLocalidad"].ToString()) + ", '" + sFecha + "', " + iIdPersona + "," + Environment.NewLine;
                sSql += parametros.CgTipoCliente + ", " + Convert.ToInt32(Application["cgMoneda"].ToString()) + ", 0, " + Convert.ToInt32(Session["idVendedor"].ToString()) + ",";
                sSql += parametros.CgEstadoPedido + ", 0, 7469, " + Convert.ToInt32(Session["idProgramacion"]) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(Session["idUsuario"]) + ", ";
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 1, 0, 0, 1, " + Convert.ToInt32(Session["idJornada"].ToString()) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(Session["idPuebloOrigen_P"].ToString()) + ", " + Convert.ToInt32(Session["idPuebloDestino_P"].ToString()) + ", ";
                sSql += Session["idCierreCaja"].ToString() + ")";

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
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                else
                {
                    iIdPedido = Convert.ToInt32(iMaximo);
                }

                Session["id_pedido_nuevo"] = iIdPedido.ToString();

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
                    iNumeroPedido = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());
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
                sTabla = "cv403_cab_despachos";
                sCampo = "id_despacho";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "No se pudo obtener el código de la tabla " + sTabla + ".";
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
                sTabla = "cv403_despachos_pedidos";
                sCampo = "id_despacho_pedido";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "No se pudo obtener el código de la tabla " + sTabla + ".";                    
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
                    lblMensajeError.Text = "No se pudo obtener el código de la tabla " + sTabla + ".";
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
                sSql += "'" + sFecha + "', " + Convert.ToInt32(Application["cgMoneda"].ToString()) + ", " + dbTotal + "," + Environment.NewLine;
                sSql += parametros.CgEstadoDcto + ", 'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 1, 0)";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                iNuevaCantidadTasas = 0;
                columnasGridDetalles(true);

                //INSTRCCIONES PARA INSERTAR EN LA TABLA CV403_DET_PEDIDOS
                foreach (GridViewRow row in dgvDetalle.Rows)
                {
                    CheckBox check = row.FindControl("chkSeleccionar") as CheckBox;

                    if (check.Checked == false)
                    {
                        iIdPersonaDetPedido = Convert.ToInt32(row.Cells[3].Text);
                        iIdAsiento = Convert.ToInt32(row.Cells[4].Text);
                        iIdDetalleRuta = Convert.ToInt32(row.Cells[5].Text);
                        iIdProducto = Convert.ToInt32(row.Cells[7].Text);
                        iIdTipoCliente = Convert.ToInt32(row.Cells[8].Text);

                        dbPrecioUnitario = Convert.ToDouble(row.Cells[14].Text);
                        dbDescuento = Convert.ToDouble(row.Cells[15].Text);
                        dbCantidad = 1;
                        dbIva = 0;

                        //INSTRUCCION SQL PARA GUARDAR EN LA BASE DE DATOS
                        sSql = "";
                        sSql += "Insert Into cv403_det_pedidos(" + Environment.NewLine;
                        sSql += "Id_Pedido, id_producto, Cg_Unidad_Medida, precio_unitario," + Environment.NewLine;
                        sSql += "Cantidad, Valor_Dscto, Valor_Ice, Valor_Iva ,Valor_otro," + Environment.NewLine;
                        sSql += "id_ctt_asiento, id_ctt_tipo_cliente, id_ctt_pueblo, estado, fecha_ingreso, usuario_ingreso, " + Environment.NewLine;
                        sSql += "terminal_ingreso, numero_replica_trigger, numero_control_replica, id_persona)" + Environment.NewLine;
                        sSql += "values(" + Environment.NewLine;
                        sSql += iIdPedido + ", " + iIdProducto + ", 546, " + dbPrecioUnitario + ", " + Environment.NewLine;
                        sSql += dbCantidad + ", " + dbDescuento + ", 0, " + dbIva + ", " + dbServicio + ", " + Environment.NewLine;
                        sSql += iIdAsiento + ", " + iIdTipoCliente + ", " + iIdDetalleRuta + ", 'A',  GETDATE()," + Environment.NewLine;
                        sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 0, 0, " + iIdPersonaDetPedido + ")";

                        //EJECUCION DE INSTRUCCION SQL
                        if (!conexionM.ejecutarInstruccionSQL(sSql))
                        {
                            columnasGridDetalles(false);
                            lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                            return false;
                        }

                        iNuevaCantidadTasas++;
                    }
                }

                columnasGridDetalles(false);                

                return true;
            }

            catch(Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); return false; }
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
                    iIdDocumentoCobrar = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());
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
                sSql += dbTotal + ", 0, " + Convert.ToInt32(Application["cgEmpresa"].ToString()) + "," + Environment.NewLine;
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
                sTabla = "cv403_pagos";
                sCampo = "id_pago";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "No se pudo obtener el código de la tabla " + sTabla + ".";
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
                    iNumeroPago = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());
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
                sSql += Convert.ToInt32(Application["cgMoneda"].ToString()) + ", 1, " + dbTotal + ", 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 1, 0, null)";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //OBTENEMOS EL MAX ID DE LA TABLA CV403_DOCUMENTOS_PAGOS
                sTabla = "cv403_documentos_pagos";
                sCampo = "id_documento_pago";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "No se pudo obtener el código de la tabla " + sTabla + ".";
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
                sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dbTotal + ", 'A', 1, 0, " + Environment.NewLine;
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
                sSql += "isnull(isnull(TT.domicilio, TT.celular), '') telefono, isnull(TP.correo_electronico, '') correo_electronico" + Environment.NewLine;
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
                sSql += "'" + sFecha + "', " + Convert.ToInt32(Application["cgMoneda"].ToString()) + ", " + dbTotal + ", 0, 0, GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A', 1, 0," + Environment.NewLine;
                sSql += "'" + sDireccion + "', '" + sTelefono + "', '" + sCiudad + "'," + Environment.NewLine;
                sSql += "'" + sCorreoElectronico + "', '" + sTasaUsuario + "', " + iManejaFacturacionElectronica + ", '" + ClaveAcceso + "', " + iEmiteTasaUsuario + ", " + iAmbienteTasa + ", ";
                sSql += iNuevaCantidadTasas + ")";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //EXTRAER ID DEL REGISTRO CV403_FACTURAS
                sTabla = "cv403_facturas";
                sCampo = "id_factura";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "No se pudo obtener el código de la tabla " + sTabla + ".";
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
                sSql += "id_factura, id_pedido, fecha_ingreso, usuario_ingreso, terminal_ingreso, estado) " + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdFactura + ", " + iIdPedido + ", GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A')";

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
                        iTasaEmitidaBandera = 0;
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
                    sSql += "emitidos = emitidos + " + iNuevaCantidadTasas + Environment.NewLine;
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
                                sSql += "estado_token = 'Cerrada'" + Environment.NewLine;
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
                    dbValorTasa = Convert.ToDecimal(iNuevaCantidadTasas.ToString(), System.Globalization.CultureInfo.InvariantCulture) * Convert.ToDecimal(Session["valor_tasa"].ToString(), System.Globalization.CultureInfo.InvariantCulture);

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
                    sSql += iNuevaCantidadTasas + ", " + dbValorTasa.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", '" + sTasaUsuario + "'," + Environment.NewLine;
                    sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', '" + sIdTasaRespuesta + "', " + iAmbienteTasa + ", " + iCobrarTasa_P + ")";

                    //EJECUCION DE INSTRUCCION SQL
                    if (!conexionM.ejecutarInstruccionSQL(sSql))
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        return false;
                    }

                    //PROCEDIMINTO PARA EXTRAER EL ID DE LA TABLA CTT_MOVIMIENTO_CAJA
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
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


        //ELIMINAR LOS DATOS DE LA FASE 1
        private bool eliminarPedido(int iIdPedidoAnterior)
        {
            try
            {
                //INSTRUCCION SQL PARA ANULAR EN CV403_CAB_PEDIDOS
                sSql = "";
                sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedidoAnterior;

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //INSTRUCCION SQL PARA ANULAR EN CV403_NUMERO_CAB_PEDIDO
                sSql = "";
                sSql += "update cv403_numero_cab_pedido set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedidoAnterior;

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //INSTRUCCION PARA EXTRAER EL ID_DESPACHO DE CV403_DESPACHOS_PEDIDOS
                sSql = "";
                sSql += "select id_despacho" + Environment.NewLine;
                sSql += "from cv403_despachos_pedidos" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedidoAnterior;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iIdCabDespachos = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                    }

                    else
                    {
                        iIdCabDespachos = 0;
                    }
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //iINSTRUCCION SQL PARA ANULAR EN CV403_CAB_DESPACHOS
                if (iIdCabDespachos != 0)
                {
                    sSql = "";
                    sSql += "update cv403_cab_despachos set" + Environment.NewLine;
                    sSql += "estado = 'E'," + Environment.NewLine;
                    sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                    sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                    sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                    sSql += "where id_despacho = " + iIdCabDespachos;

                    //EJECUCION DE INSTRUCCION SQL
                    if (!conexionM.ejecutarInstruccionSQL(sSql))
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        return false;
                    }
                }

                //INSTRUCCION PARA EXTRAER EL ID_EVENTO_COBRO E ID_DOCUMENTO_COBRAR DE CV403_DCTOS_POR_COBRAR
                sSql = "";
                sSql += "select id_documento_cobrar, id_evento_cobro" + Environment.NewLine;
                sSql += "from cv403_dctos_por_cobrar" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedidoAnterior;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iIdDocumentoCobrar = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                        iIdEventoCobro = Convert.ToInt32(dtConsulta.Rows[0][1].ToString());
                    }

                    else
                    {
                        iIdCabDespachos = 0;
                        iIdEventoCobro = 0;
                    }
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //iINSTRUCCION SQL PARA ANULAR EN CV403_EVENTOS_COBROS
                if (iIdEventoCobro != 0)
                {
                    sSql = "";
                    sSql += "update cv403_eventos_cobros set" + Environment.NewLine;
                    sSql += "estado = 'E'," + Environment.NewLine;
                    sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                    sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                    sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                    sSql += "where id_evento_cobro = " + iIdEventoCobro;

                    //EJECUCION DE INSTRUCCION SQL
                    if (!conexionM.ejecutarInstruccionSQL(sSql))
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        return false;
                    }
                }

                //iINSTRUCCION SQL PARA ANULAR EN CV403_DCTOS_POR_COBRAR
                sSql = "";
                sSql += "update cv403_dctos_por_cobrar set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_documento_cobrar = " + iIdDocumentoCobrar;

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //iINSTRUCCION SQL PARA ANULAR EN CV403_DET_PEDIDOS
                sSql = "";
                sSql += "update cv403_det_pedidos set" + Environment.NewLine;
                sSql += "estado = 'E'" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedidoAnterior;

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); return false; }
        }

        //ELIMINAR LOS DATOS DE LA FASE 2
        private bool eliminarPagos()
        {
            try
            {
                //INSTRUCCION PARA EXTRAER EL ID_PAGO DE CV403_DOCUMENTOS_PAGADOS
                sSql = "";
                sSql += "select id_pago" + Environment.NewLine;
                sSql += "from cv403_documentos_pagados" + Environment.NewLine;
                sSql += "where id_documento_cobrar = " + iIdDocumentoCobrar;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iIdPago = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                    }

                    else
                    {
                        iIdPago = 0;
                    }
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }
                
                //iINSTRUCCION SQL PARA ANULAR EN CV403_NUMEROS_PAGOS
                sSql = "";
                sSql += "update cv403_numeros_pagos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pago = " + iIdPago;

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }


                //iINSTRUCCION SQL PARA ANULAR EN CV403_DOCUMENTOS_PAGOS
                sSql = "";
                sSql += "update cv403_documentos_pagos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pago = " + iIdPago;

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }


                //iINSTRUCCION SQL PARA ANULAR EN CV403_DOCUMENTOS_PAGADOS
                sSql = "";
                sSql += "update cv403_documentos_pagados set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pago = " + iIdPago;

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }


                //iINSTRUCCION SQL PARA ANULAR EN CV403_PAGOS
                sSql = "";
                sSql += "update cv403_pagos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pago = " + iIdPago;

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); return false; }
        }


        //ELIMINAR LOS DATOS DE LA FASE 3
        private bool eliminarFactura(int iIdFacturaAnterior)
        {
            try
            {
                //iINSTRUCCION SQL PARA ANULAR EN CV403_FACTURAS
                sSql = "";
                sSql += "update cv403_facturas set" + Environment.NewLine;

                if ((iIdTasaAnulada != -1) || (iIdTasaAnulada != 0))
                {
                    sSql += "id_tasa_anulada = " + iIdTasaAnulada + "," + Environment.NewLine;
                }

                sSql += "sincronizar_tasa_anulada = " +iBanderaSincronizarTasasAnuladas + "," + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_factura = " + iIdFacturaAnterior;

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //iINSTRUCCION SQL PARA ANULAR EN CV403_NUMEROS_FACTURAS
                sSql = "";
                sSql += "update cv403_numeros_facturas set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_factura = " + iIdFacturaAnterior;

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //iINSTRUCCION SQL PARA ANULAR EN CV403_FACTURAS_PEDIDOS
                sSql = "";
                sSql += "update cv403_facturas_pedidos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_factura = " + iIdFacturaAnterior;

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); return false; }
        }

        //ELIMINACION DE LA TASA DE USUARIO EN CASO DE NO SER USADA
        private bool anularTasaUsuario()
        {
            try
            {
                iIdTasaAnulada = 0;
                iBanderaSincronizarTasasAnuladas = 0;

                if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                {
                    if (Convert.ToInt32(Session["permite_anular_tasa"].ToString()) == 1)
                    {
                        if (Session["tasa_usuario_emitida"].ToString().Trim() != "")
                        {
                            if (conexionInternet() == false)
                            {
                                iBanderaMensajeToken_P = 2;
                                iBanderaMensajeAnula_P = 2;
                                return true;
                            }

                            crearJsonTasaPendiente();

                            if (sIdTasaRespuesta != "0")
                            {
                                if (crearJsonEliminar() == false)
                                {
                                    Session["id_tasa_anulada"] = "0";
                                    //return false;
                                }

                                if (Convert.ToInt32(Session["id_tasa_anulada"].ToString()) == 0)
                                {
                                    iIdTasaAnulada = 0;
                                }

                                else if (Convert.ToInt32(Session["id_tasa_anulada"].ToString()) == -1)
                                {
                                    iIdTasaAnulada = -1;
                                }

                                else
                                {
                                    iIdTasaAnulada = Convert.ToInt32(Session["id_tasa_anulada"].ToString());
                                    sincronizarToken();
                                    iBanderaSincronizarTasasAnuladas = 1;
                                }

                                return true;
                            }
                        } //

                        if (!this.conexionInternet())
                        {
                            iBanderaMensajeToken_P = 2;
                            iBanderaMensajeAnula_P = 2;
                            return true;
                        }
                        if (!crearJsonEliminar())
                        {
                            Session["id_tasa_anulada"] = "0";
                        }
                        if (Convert.ToInt32(Session["id_tasa_anulada"].ToString()) == 0)
                        {
                            iIdTasaAnulada = 0;
                        }
                        else if (Convert.ToInt32(Session["id_tasa_anulada"].ToString()) == -1)
                        {
                            iIdTasaAnulada = -1;
                        }
                        else
                        {
                            iIdTasaAnulada = Convert.ToInt32(Session["id_tasa_anulada"].ToString());
                            sincronizarToken();
                            iBanderaSincronizarTasasAnuladas = 1;
                        }
                    }
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

        //FUNCION PARA AUMENTAR LA TASA DE USUARIO
        private bool sincronizarToken()
        {
            try
            {
                string sObtenerToken_P;
                int iObtenerCantidadPasadas_P;

                sObtenerToken_P = Session["tasa_usuario_emitida"].ToString().Substring(9, 5);
                iObtenerCantidadPasadas_P = Convert.ToInt32(Session["tasa_usuario_emitida"].ToString().Substring(0, 2));

                if (crearJsonValidarToken(sObtenerToken_P) == false)
                {
                    return false;
                }

                if (actualizarNumeroToken(sObtenerToken_P, iObtenerCantidadPasadas_P) == false)
                {
                    return false;
                }

                return true;
            }


            catch (Exception ex)
            {

                return false;
            }
        }

        //FUNCION PARA CREAR EL JSON PARA ENVIAR
        private bool crearJsonValidarToken(string sObtenerToken_P)
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
                sObjetoOficina += "\"token\": \"" + sObtenerToken_P + "\"";
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

                    if (verifica.Usar == false)
                    {
                        iNuevoNumeroCantidadToken = Convert.ToInt32(verifica.Cantidad.ToString());
                    }

                    iBanderaMensajeToken_P = 1;
                }

                catch (Exception)
                {
                    iBanderaMensajeToken_P = 0;
                    sIdTasaRespuesta = "";
                    iTasaEmitidaBandera = 0;
                }

                return "OK";
            }

            catch (Exception)
            {
                iBanderaMensajeToken_P = 0;
                return "ERROR";
            }
        }

        //FUNCION PARA ACTUALIZAR EL NUEVO NUMERO MAXIMO PARA EL TOKEN
        private bool actualizarNumeroToken(string sObtenerToken_P, int iObtenerCantidadPasadas_P)
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar iniciar la transacción.', 'danger');", true);
                    return false;
                }

                sSql = "";
                sSql += "update ctt_tasa_token set" + Environment.NewLine;
                sSql += "maximo_secuencial = " + iNuevoNumeroCantidadToken + "," + Environment.NewLine;
                sSql += "anulados = anulados + " + iObtenerCantidadPasadas_P + Environment.NewLine;
                sSql += "where token = '" + sObtenerToken_P + "'" + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "and validado = 1" + Environment.NewLine;
                sSql += "and ambiente_token = " + Convert.ToInt32(Session["emision"].ToString());

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //VERIFICAR EN CASO DE QUE EL TOKEN YA SE CERRÓ
                sSql = "";
                sSql += "select maximo_secuencial, emitidos, estado_token" + Environment.NewLine;
                sSql += "from ctt_tasa_token" + Environment.NewLine;
                sSql += "where token = '" + sObtenerToken_P + "'" + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "and validado = 1" + Environment.NewLine;
                sSql += "and ambiente_token = " + Convert.ToInt32(Session["emision"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        string sEstadoToken_P = dtConsulta.Rows[0]["estado_token"].ToString();

                        if (sEstadoToken_P == "Cerrada")
                        {
                            int iMaximoSecuencial_P = Convert.ToInt32(dtConsulta.Rows[0]["maximo_secuencial"].ToString());
                            int iEmitidos = Convert.ToInt32(dtConsulta.Rows[0]["emitidos"].ToString());

                            if (iMaximoSecuencial_P > iEmitidos)
                            {
                                sSql = "";
                                sSql += "update ctt_tasa_token set" + Environment.NewLine;
                                sSql += "estado_token = 'Abierta'," + Environment.NewLine;
                                sSql += "where token = '" + sObtenerToken_P + "'" + Environment.NewLine;
                                sSql += "and estado = 'A'" + Environment.NewLine;
                                sSql += "and validado = 1" + Environment.NewLine;
                                sSql += "and ambiente_token = " + Convert.ToInt32(Session["emision"].ToString());

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
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                conexionM.terminaTransaccion();

                return true;
            }

            catch (Exception ex)
            {

                return false;
            }
        }

        #endregion

        #region FUNCIONES DEL USUARIO

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
        }

        //FUNCION PARA MAIPULACION DE COLUMNAS EN GRID DE PASAJEROS
        private void columnasGridVendidos(bool ok)
        {
            dgvVendidos.Columns[0].Visible = ok;
            dgvVendidos.Columns[1].Visible = ok;
            dgvVendidos.Columns[2].Visible = ok;
            dgvVendidos.Columns[3].Visible = ok;
            dgvVendidos.Columns[8].Visible = ok;
            dgvVendidos.Columns[9].Visible = ok;

            dgvVendidos.Columns[4].ItemStyle.Width = 150;
            dgvVendidos.Columns[5].ItemStyle.Width = 300;
            dgvVendidos.Columns[6].ItemStyle.Width = 150;
            dgvVendidos.Columns[7].ItemStyle.Width = 150;
            dgvVendidos.Columns[10].ItemStyle.Width = 100;

            dgvVendidos.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvVendidos.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvVendidos.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        }

        //FUNCION PARA LLENAR EL GRID DE VIAJES
        private void llenarGrid(string sFecha)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_itinerarios" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(Session["id_pueblo"].ToString()) + Environment.NewLine;
                sSql += "and fecha_viaje = '" + Convert.ToDateTime(sFecha).ToString("yyyy/MM/dd") + "'" + Environment.NewLine;
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
                lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + ex.ToString();
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
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGridExtras(string sFecha_P)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_itinerarios" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(Session["id_pueblo"].ToString()) + Environment.NewLine;
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

        //FUNCION PARA LLENAR EL GRID DE BOLETOS VENDIDOS
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

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            Session["id_Programacion"] = null;
            pnlGrid.Visible = true;
            pnlVendidos.Visible = false;
            sFecha = DateTime.Now.ToString("dd/MM/yyyy");
            txtDate.Text = sFecha;
            sFecha = DateTime.Now.ToString("yyyy/MM/dd");
            llenarGrid(sFecha);
            llenarGridExtras(sFecha);
        }

        #endregion

        #region FUNCIONES DEL USUARIO DE DETALLE

        //FUNCION PARA MAIPULACION DE COLUMNAS EN GRID DE PASAJEROS
        private void columnasGridDetalles(bool ok)
        {            
            dgvDetalle.Columns[1].Visible = ok;
            dgvDetalle.Columns[2].Visible = ok;
            dgvDetalle.Columns[3].Visible = ok;
            dgvDetalle.Columns[4].Visible = ok;
            dgvDetalle.Columns[5].Visible = ok;
            dgvDetalle.Columns[6].Visible = ok;
            dgvDetalle.Columns[7].Visible = ok;
            dgvDetalle.Columns[8].Visible = ok;
            //dgvDetalle.Columns[12].Visible = ok;
            //dgvDetalle.Columns[13].Visible = ok;
            dgvDetalle.Columns[14].Visible = ok;
            dgvDetalle.Columns[15].Visible = ok;
            dgvDetalle.Columns[16].Visible = ok;

            dgvDetalle.Columns[0].ItemStyle.Width = 50;
            dgvDetalle.Columns[9].ItemStyle.Width = 100;
            dgvDetalle.Columns[10].ItemStyle.Width = 150;
            dgvDetalle.Columns[11].ItemStyle.Width = 300;
            dgvDetalle.Columns[12].ItemStyle.Width = 200;
            dgvDetalle.Columns[17].ItemStyle.Width = 100;

            dgvDetalle.Columns[9].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDetalle.Columns[10].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDetalle.Columns[17].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        }

        //FUNCION  PARA LLENAR EL GRID DETALLADO DE LA FACTURA
        private void llenarGridDetalle()
        {
            try
            {
                sSql = "";
                sSql += "select id_pedido, id_det_pedido, id_persona_det_pedido, id_ctt_asiento," + Environment.NewLine;
                sSql += "id_ctt_pueblo, id_ctt_programacion, id_producto, numero_asiento, " + Environment.NewLine;
                sSql += "identificacion_pasajero, nombre_pasajero, apellido_pasajero, Nombre," + Environment.NewLine;
                sSql += "precio_unitario, valor_dscto, valor_iva," + Environment.NewLine;
                sSql += "ltrim(str(precio_unitario - valor_dscto + valor_iva, 10, 2)) valor," + Environment.NewLine;
                sSql += "id_ctt_tipo_cliente, tipo_cliente" + Environment.NewLine;
                sSql += "from ctt_vw_factura" + Environment.NewLine;
                sSql += "where id_pedido = " + Convert.ToInt32(Session["idPedido"].ToString()) + Environment.NewLine;
                sSql += "order by numero_asiento";

                columnasGridDetalles(true);
                vendidoE.ISQL = sSql;
                dgvDetalle.DataSource = vendidoM.listarVendidos(vendidoE);
                dgvDetalle.DataBind();
                columnasGridDetalles(false);
            }

            catch(Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONSULTAR LOS DATOS DE LA FACTURA
        private void consultarFactura(int iIdPedido_P)
        {
            try
            {
                sSql = "";
                sSql += "select id_factura, id_persona, fecha_factura, identificacion," + Environment.NewLine;
                sSql += "ltrim(isnull(nombres,'') + ' ' + apellidos) cliente, clave_acceso," + Environment.NewLine;
                sSql += "descripcion_ruta, destino, fecha_viaje, hora_salida, id_origen," + Environment.NewLine;
                sSql += "id_ctt_pueblo id_destino, tasa_usuario, id_tasa_emitida," + Environment.NewLine;
                sSql += "ambiente_tasa_usuario" + Environment.NewLine;
                sSql += "from ctt_vw_factura" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["idFacturaAnular"] = dtConsulta.Rows[0]["id_factura"].ToString();
                        Session["idPersonaAnular"] = dtConsulta.Rows[0]["id_persona"].ToString();
                        lblFechaFactura.Text = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_factura"].ToString()).ToString("dd-MMM-yyyy");
                        lblIdentificacion.Text = dtConsulta.Rows[0]["identificacion"].ToString();
                        lblRazonSocial.Text = dtConsulta.Rows[0]["cliente"].ToString();
                        lblClaveAcceso.Text = dtConsulta.Rows[0]["clave_acceso"].ToString();

                        Session["identificacion_cliente"] = dtConsulta.Rows[0]["identificacion"].ToString();
                        Session["nombre_cliente"] = dtConsulta.Rows[0]["cliente"].ToString();

                        string[] sSeparar = dtConsulta.Rows[0]["descripcion_ruta"].ToString().Split('-');
                        Session["pueblo_origen"] = sSeparar[0].Trim();
                        Session["pueblo_destino"] = dtConsulta.Rows[0]["destino"].ToString();
                        Session["fecha_viaje"] = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_viaje"].ToString()).ToString("yyyy-MM-dd");
                        Session["hora_salida"] = Convert.ToDateTime(dtConsulta.Rows[0]["hora_salida"].ToString()).ToString("HH:mm");
                        Session["id_pueblo_origen"] = dtConsulta.Rows[0]["id_origen"].ToString();
                        Session["id_pueblo_destino"] = dtConsulta.Rows[0]["id_destino"].ToString();
                        Session["tasa_usuario_emitida"] = dtConsulta.Rows[0]["tasa_usuario"].ToString();
                        Session["id_tasa_usuario_emitida"] = dtConsulta.Rows[0]["id_tasa_emitida"].ToString();
                        Session["ambiente_tasa_usuario_actual"] = dtConsulta.Rows[0]["ambiente_tasa_usuario"].ToString();
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " alert('No existen datos en la factura. Comuníquese con el administrador.')", true);
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvDatos.PageIndex = e.NewPageIndex;
                sFecha = txtDate.Text.Trim();

                llenarGrid(sFecha);
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
                Session["idProgramacion"] = dgvDatos.Rows[a].Cells[1].Text;
                sEstadoViaje = dgvDatos.Rows[a].Cells[9].Text;
                columnasGrid(false);

                if (sEstadoViaje == "C")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El viaje ya se ha cerrado. No puede realizar devoluciones', 'info');", true);
                    Session["idProgramacion"] = null;
                }

                else
                {
                    lblDetalleBus.Text = "FECHA SALIDA: " + dgvDatos.Rows[a].Cells[3].Text + " - HORA SALIDA: " + dgvDatos.Rows[a].Cells[6].Text + " - VEHÍCULO: " + dgvDatos.Rows[a].Cells[4].Text;
                    
                    string[] sSeparar_Bus = dgvDatos.Rows[a].Cells[4].Text.Split('-');
                    Session["disco_vehiculo_tasa"] = sSeparar_Bus[0].ToString().Trim();

                    string[] sSeparar_Ruta = dgvDatos.Rows[a].Cells[5].Text.Split('-');
                    Session["pueblo_origen_tasa"] = sSeparar_Ruta[0].ToString().Trim();
                    Session["pueblo_destino_tasa"] = sSeparar_Ruta[1].ToString().Trim();                    
                    Session["id_pueblo_origen_tasa"] = dgvDatos.Rows[a].Cells[16].Text;
                    Session["id_pueblo_destino_tasa"] = dgvDatos.Rows[a].Cells[17].Text;
                    llenarGridVendidos(0);
                    pnlGrid.Visible = false;
                    pnlVendidos.Visible = true;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar();
        }
        
        protected void dgvVendidos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvVendidos.PageIndex = e.NewPageIndex;
            llenarGridVendidos(e.NewPageIndex + 1);
        }

        protected void dgvVendidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvVendidos.SelectedIndex;
                columnasGridVendidos(true);
                Session["idPedido"] = dgvVendidos.Rows[a].Cells[0].Text;
                Session["idPuebloOrigen_P"] = dgvVendidos.Rows[a].Cells[8].Text;
                Session["idPuebloDestino_P"] = dgvVendidos.Rows[a].Cells[9].Text;
                columnasGridVendidos(false);

                btnPopUp_ModalPopupExtender.Show();
                llenarGridDetalle();
                consultarFactura(Convert.ToInt32(Session["idPedido"].ToString()));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvDatos.PageIndex = e.NewPageIndex;

                llenarGridDetalle();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnCerrarModal_Click(object sender, EventArgs e)
        {
            btnPopUp_ModalPopupExtender.Hide();
        }

        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                lblAdvertencia.Text = "";
                iTotalRegistros = dgvDetalle.Rows.Count;
                iCuentaRegistros = 0;                
                                
                foreach(GridViewRow row in dgvDetalle.Rows)
                {
                    CheckBox check = row.FindControl("chkSeleccionar") as CheckBox;

                    if (check.Checked == true)
                    {
                        iCuentaRegistros++;
                    }
                }

                if (iCuentaRegistros == 0)
                {
                    lblAdvertencia.Text = "No ha seleccionado ningún registro.";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No ha seleccionado ningún registro.', 'info');", true);
                }

                else if (iCuentaRegistros == iTotalRegistros)
                {
                    //ENVIAR A FUNCION SOLO PARA ELIMINAR LA FACTURA
                    procesarFactura(2, Convert.ToInt32(Session["idFacturaAnular"].ToString()), Convert.ToInt32(Session["idPedido"].ToString()), iTotalRegistros);
                }

                else if (iCuentaRegistros < iTotalRegistros)
                {
                    //ENVIAR A FUNCION PARA ANULAR LA FACTURA Y CREAR UNA NUEVA CON LOS NUEVOS REGISTROS
                    procesarFactura(1, Convert.ToInt32(Session["idFacturaAnular"].ToString()), Convert.ToInt32(Session["idPedido"].ToString()), iCuentaRegistros);
                }
            }

            catch(Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
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
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnCancelarModal_Click(object sender, EventArgs e)
        {
            btnPopUp_ModalPopupExtender.Hide();
        }

        protected void btnCancelarGrid_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmDevolucionBoletos.aspx");
        }

        protected void dgvDatosExtras_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatosExtras.SelectedIndex;
                columnasGridExtra(true);
                Session["idProgramacion"] = dgvDatosExtras.Rows[a].Cells[1].Text;
                sEstadoViaje = dgvDatosExtras.Rows[a].Cells[9].Text;
                columnasGridExtra(false);

                if (sEstadoViaje == "C")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El viaje ya se ha cerrado. No puede realizar devoluciones', 'info');", true);
                    Session["idProgramacion"] = null;
                }

                else
                {
                    lblDetalleBus.Text = "FECHA SALIDA: " + dgvDatos.Rows[a].Cells[3].Text + " - HORA SALIDA: " + dgvDatos.Rows[a].Cells[6].Text + " - VEHÍCULO: " + dgvDatos.Rows[a].Cells[4].Text;

                    string[] sSeparar_Bus = dgvDatos.Rows[a].Cells[4].Text.Split('-');
                    Session["disco_vehiculo_tasa"] = sSeparar_Bus[0].ToString().Trim();

                    string[] sSeparar_Ruta = dgvDatos.Rows[a].Cells[5].Text.Split('-');
                    Session["pueblo_origen_tasa"] = sSeparar_Ruta[0].ToString().Trim();
                    Session["pueblo_destino_tasa"] = sSeparar_Ruta[1].ToString().Trim();
                    Session["id_pueblo_origen_tasa"] = dgvDatos.Rows[a].Cells[16].Text;
                    Session["id_pueblo_destino_tasa"] = dgvDatos.Rows[a].Cells[17].Text;

                    llenarGridVendidos(0);
                    pnlGrid.Visible = false;
                    pnlVendidos.Visible = true;
                }
            }

            catch (Exception ex)
            {
                cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

        }

        protected void dgvDatosExtras_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvDatosExtras.PageIndex = e.NewPageIndex;
                sFecha = txtDate.Text.Trim();

                llenarGridExtras(sFecha);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }
    }
}