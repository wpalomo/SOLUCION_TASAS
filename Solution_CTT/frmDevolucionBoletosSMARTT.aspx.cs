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
    public partial class frmDevolucionBoletosSMARTT : System.Web.UI.Page
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

        Clases_Contifico.ClaseAnularLiberar anularLiberarBoleto;
        Clase_Variables_Contifico.Boleto boletoLiberar;
        Clase_Variables_Contifico.TasaUsuarioSmartt consultarTasaAnulada;

        string sSql;
        string sEstadoViaje;
        string sFecha;
        string sTabla;
        string sCampo;
        string sCiudad;
        string sDireccion;
        string sTelefono;
        string sRespuesta_A;
        string sCorreoElectronico;
        string sIdentificacionParaTasa;
        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;
        DataTable dtDetalleTasaOriginal;
        DataTable dtTasasEmitidas;

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
        int iManejaFacturacionElectronica;
        int iNuevaCantidadTasas;
        int iIdFormaPagoFactura;
        int iVendidos_REP;
        int iIdTasaSmartt;

        int iCgTipoDocumento = 7456;
        int iCgEstadoDctoPorCobrar = 7461;

        double dbPrecioUnitario;
        double dbDescuento;
        double dbCantidad;
        double dbIva;
        double dbServicio;
        double dbTotal;

        long iMaximo;

        //VARIABLES DEL REPORTE

        decimal dbCantidad_REP;
        decimal dbPrecioUnitario_REP;
        decimal dbDescuento_REP;
        decimal dbIva_REP;
        decimal dbSumaTotal_REP;

        string sNumeroFactura_REP;
        string sAsientos_REP;

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

            Session["modulo"] = "MÓDULO DE DEVOLUCIÓN Y ANULACIÓN DE FACTURAS - SMARTT";

            if (!IsPostBack)
            {
                sFecha = DateTime.Now.ToString("dd/MM/yyyy");
                txtDate.Text = sFecha;
                llenarGrid(sFecha);
                llenarGridExtras(sFecha);
                Session["idVehiculo"] = null;
                Session["idProgramacion"] = null;
            }
        }

        #region FUNCIONES DEL USUARIO PARA LAS TASAS DE USUARIO

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
                        sSql += "where id_pedido = " + iIdPedido + Environment.NewLine;
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
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                //  1. GENERAR UN NUEVO PEDIDO CON LOS ITEMS QUE SE MANTENDRÁN
                if (iOp == 1)
                {
                    if (consultarTasasEmitidas(iIdPedido_P) == false)
                    {
                        conexionM.reversaTransaccion();
                        return;
                    }

                    if (insertarPedido() == false)
                    {
                        return;
                    }

                    if (insertarPagos() == false)
                    {
                        return;
                    }

                    if (insertarFactura() == false)
                    {
                        return;
                    }
                }

                if (iOp != 1)
                {
                    //AQUI PARA ANULAR TODAS LAS VENTAS 
                    if (anularTasasEmitidas(iIdPedido_P) == false)
                    {
                        conexionM.reversaTransaccion();
                        return;
                    }
                }

                //  SE PROCEDE A ELIMINAR EL REGISTRO ANTERIOR

                if (eliminarPedido(iIdPedido_P) == false)
                {
                    return;
                }

                if (eliminarPagos() == false)
                {
                    return;
                }

                if (eliminarFactura(iIdFactura_P) == false)
                {
                    return;
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
                    return;
                }

                conexionM.terminaTransaccion();

                if (iOp == 1)
                {
                    crearReporteImprimir();
                }

                btnPopUp_ModalPopupExtender.Hide();

                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Factura anulada éxitosamente', 'success');", true);

                llenarGridVendidos(0);

                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //OBTENER LAS TASAS DE USUARIO EMITIDAS
        private bool consultarTasasEmitidas(int iIdPedido_P)
        {
            try
            {
                //CONSULTAR EL DETALLE DE LA TASA DE USUARIO
                sSql = "";
                sSql += "select * from ctt_detalle_tasa_smartt" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtDetalleTasaOriginal = new DataTable();
                dtDetalleTasaOriginal.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtDetalleTasaOriginal);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //CONSULTAR LAS TASAS DE USUARIO EMITIDAS
                sSql = "";
                sSql += "select * from ctt_vw_tasas_smartt_anular" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P + Environment.NewLine;
                //sSql += "order by id_det_pedido";

                dtTasasEmitidas = new DataTable();
                dtTasasEmitidas.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtTasasEmitidas);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //INSTRCCIONES PARA INSERTAR EN LA TABLA CV403_DET_PEDIDOS
                foreach (GridViewRow row in dgvDetalle.Rows)
                {
                    CheckBox check = row.FindControl("chkSeleccionar") as CheckBox;

                    if (check.Checked == true)
                    {
                        int iNumeroAsiento_Liberar = Convert.ToInt32(row.Cells[9].Text);

                        anularLiberarBoleto = new Clases_Contifico.ClaseAnularLiberar();
                        string sVer = Session["idViajeDevolucionSMARTT"].ToString();

                        sRespuesta_A = anularLiberarBoleto.recuperarJsonLiberacionVenta(Session["tokenSMARTT"].ToString(),
                                       Convert.ToInt32(Session["idViajeDevolucionSMARTT"].ToString()), iNumeroAsiento_Liberar);

                        if (sRespuesta_A == "ERROR")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'No se pudo obtener registros para la tasa de usuario SMARTT', 'error')</script>");
                            return false;
                        }

                        if (sRespuesta_A == "ISNULL")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Información.!', 'No se proporcionaron credenciales de autenticación. Tasa de Usuario SMARTT', 'info')</script>");
                            return false;
                        }

                        Session["JsonLiberar"] = sRespuesta_A;

                        //sRespuesta_A = "";
                        //sRespuesta_A += "{" + Environment.NewLine;
                        //sRespuesta_A += "\"id\": 56749," + Environment.NewLine;
                        //sRespuesta_A += "\"asiento\": 44," + Environment.NewLine;
                        //sRespuesta_A += "\"asiento_nombre\": \"44\"," + Environment.NewLine;
                        //sRespuesta_A += "\"nivel\": 1," + Environment.NewLine;
                        //sRespuesta_A += "\"valor\": 8," + Environment.NewLine;
                        //sRespuesta_A += "\"localidad_embarque\": 1," + Environment.NewLine;
                        //sRespuesta_A += "\"tipo_cliente\": 1," + Environment.NewLine;
                        //sRespuesta_A += "\"parada_embarque\": null," + Environment.NewLine;
                        //sRespuesta_A += "\"parada_destino\": 846," + Environment.NewLine;
                        //sRespuesta_A += "\"pasajero\": {" + Environment.NewLine;
                        //sRespuesta_A += "\"identificacion\": \"9999999999999\"," + Environment.NewLine;
                        //sRespuesta_A += "\"id\": 5380," + Environment.NewLine;
                        //sRespuesta_A += "\"nombre\": \"CONSUMIDOR FINAL\"," + Environment.NewLine;
                        //sRespuesta_A += "\"correo\": \"contabilidad@expressatenas.com.ec\"," + Environment.NewLine;
                        //sRespuesta_A += "\"tipo_cliente\": \"N\"," + Environment.NewLine;
                        //sRespuesta_A += "\"direccion\": null," + Environment.NewLine;
                        //sRespuesta_A += "\"telefono\": null," + Environment.NewLine;
                        //sRespuesta_A += "\"tipo_identificacion\": \"RUC\"," + Environment.NewLine;
                        //sRespuesta_A += "\"extranjero\": false," + Environment.NewLine;
                        //sRespuesta_A += "\"is_active\": true," + Environment.NewLine;
                        //sRespuesta_A += "\"is_enable\": true," + Environment.NewLine;
                        //sRespuesta_A += "\"actualizacion\": \"2019-12-23T14:35:46.492711-05:00\"" + Environment.NewLine;
                        //sRespuesta_A += "}," + Environment.NewLine;
                        //sRespuesta_A += "\"tasa\": \"9385167124\"," + Environment.NewLine;
                        //sRespuesta_A += "\"estado\": 0," + Environment.NewLine;
                        //sRespuesta_A += "\"estado_nombre\": \"Anulado\"," + Environment.NewLine;
                        //sRespuesta_A += "\"is_active\": true," + Environment.NewLine;
                        //sRespuesta_A += "\"is_enable\": true," + Environment.NewLine;
                        //sRespuesta_A += "\"actualizacion\": \"2019-12-23T14:42:38.429032-05:00\"" + Environment.NewLine;
                        //sRespuesta_A += "}";

                        boletoLiberar = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Boleto>(sRespuesta_A);

                        for (int j = 0; j < dtTasasEmitidas.Rows.Count; j++)
                        {
                            //int iAsientoAux = Convert.ToInt32(dtTasasEmitidas.Rows[j]["numero_asiento"].ToString());
                            int iIdCttTasaSmartt = Convert.ToInt32(dtTasasEmitidas.Rows[j]["id_ctt_tasas_smartt"].ToString());
                            string sTasaUsuarioAnulada = boletoLiberar.tasa.Trim();
                            string sTasaUsuarioEmitida = dtTasasEmitidas.Rows[j]["tasa_usuario"].ToString().Trim();

                            if (sTasaUsuarioAnulada == sTasaUsuarioEmitida)
                            {
                                dtTasasEmitidas.Rows[j]["estado_tasa_usuario"] = boletoLiberar.estado.ToString();
                                dtTasasEmitidas.Rows[j]["estado_nombre"] = boletoLiberar.estado_nombre;

                                sSql = "";
                                sSql += "update ctt_tasas_smartt set" + Environment.NewLine;
                                sSql += "estado_tasa_usuario = " + boletoLiberar.estado.ToString() + "," + Environment.NewLine;
                                sSql += "estado_nombre = '" + boletoLiberar.estado_nombre + "'" + Environment.NewLine;
                                sSql += "where id_ctt_tasas_smartt = " + iIdCttTasaSmartt;

                                //EJECUCION DE LA INSTRUCCION SQL
                                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                                {
                                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                                    return false;
                                }

                                break;
                            }
                        }
                    }
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

        //FUNCION PARA ANULAR EL REGISTRO DE SMARTT
        private bool anularTasasEmitidas(int iIdPedido_P)
        {
            try
            {
                anularLiberarBoleto = new Clases_Contifico.ClaseAnularLiberar();

                sRespuesta_A = anularLiberarBoleto.recuperarJsonAnulacionVenta(Session["tokenSMARTT"].ToString(),
                                       Convert.ToInt32(Session["idTasaFacturaSMARTT"].ToString()));

                if (sRespuesta_A == "ERROR")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'No se pudo obtener registros para la tasa de usuario SMARTT', 'error')</script>");
                    return false;
                }

                if (sRespuesta_A == "ISNULL")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Información.!', 'No se proporcionaron credenciales de autenticación. Tasa de Usuario SMARTT', 'info')</script>");
                    return false;
                }

                Session["JsonLiberar"] = sRespuesta_A;

                consultarTasaAnulada = JsonConvert.DeserializeObject<Clase_Variables_Contifico.TasaUsuarioSmartt>(sRespuesta_A);

                sSql = "";
                sSql += "update ctt_detalle_tasa_smartt set" + Environment.NewLine;
                sSql += "estado_smartt = " + consultarTasaAnulada.estado + "," + Environment.NewLine;
                sSql += "estado_nombre = '" + consultarTasaAnulada.estado_nombre + "'" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P;

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                //CONSULTAR LAS TASAS DE USUARIO EMITIDAS
                sSql = "";
                sSql += "select * from ctt_vw_tasas_smartt_anular" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P + Environment.NewLine;

                dtTasasEmitidas = new DataTable();
                dtTasasEmitidas.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtTasasEmitidas);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                for (int j = 0; j < dtTasasEmitidas.Rows.Count; j++)
                {
                    dtTasasEmitidas.Rows[j]["estado_tasa_usuario"] = consultarTasaAnulada.estado;
                    dtTasasEmitidas.Rows[j]["estado_nombre"] = consultarTasaAnulada.estado_nombre;
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

                //INSERTANOS LA RESPUESTA DE LAS TASAS DE USUARIO SMARTT EN LA TABLA ctt_detalle_tasa_smartt
                int iId_S = Convert.ToInt32(dtDetalleTasaOriginal.Rows[0]["id"].ToString());
                string sFechaHoraVenta_S = Convert.ToDateTime(dtDetalleTasaOriginal.Rows[0]["fecha_hora_venta"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                string sNumeroDocumento_S = dtDetalleTasaOriginal.Rows[0]["numero_documento"].ToString();
                string sClaveAcceso_S = dtDetalleTasaOriginal.Rows[0]["clave_acceso"].ToString();
                string sNumeroDocumentoTasa_S = dtDetalleTasaOriginal.Rows[0]["numero_documento_tasa"].ToString();
                string sClaveAccesoTasa_S = dtDetalleTasaOriginal.Rows[0]["clave_acceso_tasa"].ToString();
                Double dbTotalTasas_S = Convert.ToDouble(dtDetalleTasaOriginal.Rows[0]["total_tasas"].ToString());
                int iViaje_S = Convert.ToInt32(dtDetalleTasaOriginal.Rows[0]["viaje"].ToString());
                int iFormaPago_S = Convert.ToInt32(dtDetalleTasaOriginal.Rows[0]["forma_de_pago"].ToString());
                string sIdentificacionFactura_S = dtDetalleTasaOriginal.Rows[0]["identificacion"].ToString();
                int iIdCliente_S = Convert.ToInt32(dtDetalleTasaOriginal.Rows[0]["id_cliente"].ToString());
                string sNombreFactura_S = dtDetalleTasaOriginal.Rows[0]["nombre"].ToString();
                string sCorreoFactura_S = dtDetalleTasaOriginal.Rows[0]["correo"].ToString();
                string sTipoClienteFactura_S = dtDetalleTasaOriginal.Rows[0]["tipo_cliente"].ToString();
                string sDireccionFactura_S = dtDetalleTasaOriginal.Rows[0]["direccion"].ToString();
                string sTelefonoFactura_S = dtDetalleTasaOriginal.Rows[0]["telefono"].ToString();
                string sTipoIdentificacion_S = dtDetalleTasaOriginal.Rows[0]["tipo_identificacion"].ToString();
                int iExtranjeroFactura_S = Convert.ToInt32(dtDetalleTasaOriginal.Rows[0]["extranjero"].ToString()); ;
                int iActivoFactura_S = Convert.ToInt32(dtDetalleTasaOriginal.Rows[0]["is_active_cliente"].ToString());
                int iHabilitadoFactura_S = Convert.ToInt32(dtDetalleTasaOriginal.Rows[0]["is_enable_cliente"].ToString());
                string sFechaActualizacionFactura_S = Convert.ToDateTime(dtDetalleTasaOriginal.Rows[0]["actualizacion_cliente"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                string sUuid = dtDetalleTasaOriginal.Rows[0]["uuid"].ToString();
                int iEstadoTasa_S = Convert.ToInt32(dtDetalleTasaOriginal.Rows[0]["estado_smartt"].ToString());
                string sEstadoNombreTasa_S = dtDetalleTasaOriginal.Rows[0]["estado_nombre"].ToString();
                int iOffline_S = Convert.ToInt32(dtDetalleTasaOriginal.Rows[0]["offline"].ToString());
                string sEmisionTasa_S = dtDetalleTasaOriginal.Rows[0]["emision"].ToString();
                int iCooperativaTasa_S = Convert.ToInt32(dtDetalleTasaOriginal.Rows[0]["cooperativa"].ToString());
                string sDestinoTasa_S = dtDetalleTasaOriginal.Rows[0]["destino"].ToString();
                int iActivoTasa_S = Convert.ToInt32(dtDetalleTasaOriginal.Rows[0]["is_active"].ToString());
                int iHabilitadoTasa_S = Convert.ToInt32(dtDetalleTasaOriginal.Rows[0]["is_enable"].ToString());
                string sFechaActualizacionTasa_S = Convert.ToDateTime(dtDetalleTasaOriginal.Rows[0]["actualizacion_boletos"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

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

                //PROCEDIMINTO PARA EXTRAER EL ID DE LA TABLA ctt_detalle_tasa_smartt
                sTabla = "ctt_detalle_tasa_smartt";
                sCampo = "id_ctt_detalle_tasa_smartt";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>No se pudo obtener el código de la tabla " + sTabla + ".";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }

                iIdTasaSmartt = Convert.ToInt32(iMaximo);

                for (int k = 0; k < dtTasasEmitidas.Rows.Count; k++)
                {
                    int iId_Filtro = Convert.ToInt32(dtTasasEmitidas.Rows[k]["id"].ToString());
                    double dbValor_Filtro = Convert.ToDouble(dtTasasEmitidas.Rows[k]["valor"].ToString());
                    int iLocalidadEmbarque_Filtro = Convert.ToInt32(dtTasasEmitidas.Rows[k]["localidad_embarque"].ToString());
                    int iTipoCliente_Filtro = Convert.ToInt32(dtTasasEmitidas.Rows[k]["tipo_cliente"].ToString());
                    string sParadaEmbarque_Filtro = dtTasasEmitidas.Rows[k]["parada_embarque"].ToString();
                    int iParadaDestino_Filtro = Convert.ToInt32(dtTasasEmitidas.Rows[k]["parada_destino"].ToString());
                    int iIdPasajero_Filtro = Convert.ToInt32(dtTasasEmitidas.Rows[k]["id_pasajero_smartt"].ToString());
                    string sTipoCiente_Filtro = dtTasasEmitidas.Rows[k]["tipo_cliente_pasajero"].ToString();
                    string sTipoIdentificacion_Filtro = dtTasasEmitidas.Rows[k]["tipo_identificacion"].ToString();

                    int iExtranjero_Filtro = Convert.ToInt32(dtTasasEmitidas.Rows[k]["extranjero_pasajero"].ToString());
                    int iActivoPasajero_Filtro = Convert.ToInt32(dtTasasEmitidas.Rows[k]["is_active_pasajero"].ToString());
                    int iHabilitadoPasajero_Filtro = Convert.ToInt32(dtTasasEmitidas.Rows[k]["is_enable_pasajero"].ToString());
                    string sFechaActualizacionPasajero_Filtro = Convert.ToDateTime(dtTasasEmitidas.Rows[k]["actualizacion_pasajero"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    string sTasaUsuario_Filtro = dtTasasEmitidas.Rows[k]["tasa_usuario"].ToString();
                    int iEstadoTasa_Filtro = Convert.ToInt32(dtTasasEmitidas.Rows[k]["estado_tasa_usuario"].ToString());
                    string sNombreEstado_Filtro = dtTasasEmitidas.Rows[k]["estado_nombre"].ToString();
                    int iActivoBoletos_Filtro = Convert.ToInt32(dtTasasEmitidas.Rows[k]["is_active_smartt"].ToString());
                    int iHabilitadoBoletos_Filtro = Convert.ToInt32(dtTasasEmitidas.Rows[k]["is_enable_smartt"].ToString());
                    string sFechaActualizacionBoletos_Filtro = Convert.ToDateTime(dtTasasEmitidas.Rows[k]["actualizacion_smartt"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                    sSql = "";
                    sSql += "insert into ctt_tasas_smartt (" + Environment.NewLine;
                    sSql += "id_ctt_detalle_tasa_smartt, id, valor, localidad_embarque, tipo_cliente, parada_embarque," + Environment.NewLine;
                    sSql += "parada_destino, id_pasajero_smartt, tipo_cliente_pasajero, tipo_identificacion," + Environment.NewLine;
                    sSql += "extranjero_pasajero, is_active_pasajero, is_enable_pasajero, actualizacion_pasajero," + Environment.NewLine;
                    sSql += "tasa_usuario, estado_tasa_usuario, estado_nombre, is_active_smartt, is_enable_smartt," + Environment.NewLine;
                    sSql += "actualizacion_smartt, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += iIdTasaSmartt + ", " + iId_Filtro + ", " + dbValor_Filtro + ", " + iLocalidadEmbarque_Filtro + "," + Environment.NewLine;
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
                        sIdentificacionParaTasa = row.Cells[10].Text;

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
                    }
                }

                columnasGridDetalles(false);

                return true;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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
                sSql += "facturaelectronica, clave_acceso, emite_tasa_usuario, ambiente_tasa_usuario, cantidad_tasa_emitida)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idEmpresa"].ToString()) + ", " + iIdPersona + ", " + Convert.ToInt32(Application["cgEmpresa"].ToString()) + ", 1," + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idLocalidad"].ToString()) + ", 19, " + Convert.ToInt32(Session["idVendedor"].ToString()) + ", " + iIdFormaPagoFactura + ", '" + sFecha + "'," + Environment.NewLine;
                sSql += "'" + sFecha + "', " + Convert.ToInt32(Application["cgMoneda"].ToString()) + ", " + dbTotal + ", 0, 0, GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A', 1, 0," + Environment.NewLine;
                sSql += "'" + sDireccion + "', '" + sTelefono + "', '" + sCiudad + "'," + Environment.NewLine;
                sSql += "'" + sCorreoElectronico + "', '', " + iManejaFacturacionElectronica + ", '" + ClaveAcceso + "', 0, 0, 0)";

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

                return true;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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

                //INSTRUCCION SQL PARA ANULAR EN ctt_detalle_tasa_smartt
                sSql = "";
                sSql += "update ctt_detalle_tasa_smartt set" + Environment.NewLine;
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

                //RECORRER LOS REGISTROS DE LAS TASAS EMITIDAS
                for (int i = 0; i < dtTasasEmitidas.Rows.Count; i++)
                {
                    int iEstadoTasaUsuario_A = Convert.ToInt32(dtTasasEmitidas.Rows[i]["estado_tasa_usuario"].ToString());
                    int iIdCttTasaSmartt_API = Convert.ToInt32(dtTasasEmitidas.Rows[i]["id_ctt_tasas_smartt"].ToString());

                    sSql = "";
                    sSql += "update ctt_tasas_smartt set" + Environment.NewLine;

                    if (iEstadoTasaUsuario_A == 1)
                    {
                        sSql += "estado = 'E'," + Environment.NewLine;
                    }

                    else
                    {
                        sSql += "estado = 'N'," + Environment.NewLine;
                    }

                    sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                    sSql += "usuario_anula = '" +sDatosMaximo[0] + "'," + Environment.NewLine;
                    sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                    sSql += "where id_ctt_tasas_smartt = " + iIdCttTasaSmartt_API;

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); return false; }
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
            dgvDatos.Columns[18].Visible = ok;
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
            dgvVendidos.Columns[10].Visible = ok;

            dgvVendidos.Columns[4].ItemStyle.Width = 150;
            dgvVendidos.Columns[5].ItemStyle.Width = 300;
            dgvVendidos.Columns[6].ItemStyle.Width = 150;
            dgvVendidos.Columns[7].ItemStyle.Width = 150;
            dgvVendidos.Columns[11].ItemStyle.Width = 100;

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
                lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + ex.Message;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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
                sSql += "id_ctt_tipo_cliente, tipo_cliente, id_venta_smartt" + Environment.NewLine;
                sSql += "from ctt_vw_factura" + Environment.NewLine;
                sSql += "where id_pedido = " + Convert.ToInt32(Session["idPedido"].ToString()) + Environment.NewLine;
                sSql += "order by numero_asiento";

                columnasGridDetalles(true);
                vendidoE.ISQL = sSql;
                dgvDetalle.DataSource = vendidoM.listarVendidos(vendidoE);
                dgvDetalle.DataBind();
                columnasGridDetalles(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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
                    Session["idViajeDevolucionSMARTT"] = dgvDatos.Rows[a].Cells[18].Text;
                    llenarGridVendidos(0);
                    pnlGrid.Visible = false;
                    pnlVendidos.Visible = true;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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

                string sIdTasaSmartt = dgvVendidos.Rows[a].Cells[10].Text;

                Session["idTasaFacturaSMARTT"] = dgvVendidos.Rows[a].Cells[10].Text;

                columnasGridVendidos(false);

                btnPopUp_ModalPopupExtender.Show();
                llenarGridDetalle();
                consultarFactura(Convert.ToInt32(Session["idPedido"].ToString()));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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

                foreach (GridViewRow row in dgvDetalle.Rows)
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

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnCancelarModal_Click(object sender, EventArgs e)
        {
            btnPopUp_ModalPopupExtender.Hide();
        }

        protected void btnCancelarGrid_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmDevolucionBoletosSMARTT.aspx");
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
                    lblDetalleBus.Text = "FECHA SALIDA: " + dgvDatosExtras.Rows[a].Cells[3].Text + " - HORA SALIDA: " + dgvDatosExtras.Rows[a].Cells[6].Text + " - VEHÍCULO: " + dgvDatosExtras.Rows[a].Cells[4].Text;

                    string[] sSeparar_Bus = dgvDatosExtras.Rows[a].Cells[4].Text.Split('-');
                    Session["disco_vehiculo_tasa"] = sSeparar_Bus[0].ToString().Trim();

                    string[] sSeparar_Ruta = dgvDatosExtras.Rows[a].Cells[5].Text.Split('-');
                    Session["pueblo_origen_tasa"] = sSeparar_Ruta[0].ToString().Trim();
                    Session["pueblo_destino_tasa"] = sSeparar_Ruta[1].ToString().Trim();
                    Session["id_pueblo_origen_tasa"] = dgvDatosExtras.Rows[a].Cells[16].Text;
                    Session["id_pueblo_destino_tasa"] = dgvDatosExtras.Rows[a].Cells[17].Text;
                    Session["idViajeDevolucionSMARTT"] = dgvDatosExtras.Rows[a].Cells[18].Text;

                    llenarGridVendidos(0);
                    pnlGrid.Visible = false;
                    pnlVendidos.Visible = true;
                }
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
                sFecha = txtDate.Text.Trim();

                llenarGridExtras(sFecha);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
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
    }
}