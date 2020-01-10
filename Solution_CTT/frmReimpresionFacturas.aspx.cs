using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ENTIDADES;
using NEGOCIO;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmReimpresionFacturas : System.Web.UI.Page
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

        //VARIABLES DEL REPORTE
        int iVendidos_REP;
        int iCuenta_REP;

        decimal dbCantidad_REP;
        decimal dbPrecioUnitario_REP;
        decimal dbDescuento_REP;
        decimal dbIva_REP;
        decimal dbSumaTotal_REP;

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

            Session["modulo"] = "MÓDULO DE REIMPRESIÓN DE FACTURAS";

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

        #region FUNCIONES PARA IMPRIMIR

        private void crearReporteImprimir(int iIdTipoComprobante_P)
        {
            try
            {
                sSql = "";
                sSql += "select establecimiento, punto_emision, numero_factura, fecha_ingreso," + Environment.NewLine;
                sSql += "identificacion, isnull(nombres, '') + ' ' + apellidos cliente, descripcion_ruta," + Environment.NewLine;
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

                        //if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                        //{
                        //    sTasaUsuarioRecuperado_REP = dtConsulta.Rows[0][12].ToString();
                        //    Logo = barcode(sTasaUsuarioRecuperado_REP);
                        //}

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

                            if (iIdTipoComprobante_P == 1)
                                reporteLocal.ReportPath = Server.MapPath("~/Reportes/rptFactura_2.rdlc");
                            else
                                reporteLocal.ReportPath = Server.MapPath("~/Reportes/rptNotaEntrega_2.rdlc");

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
                //cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
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
            dgvDatos.Columns[15].ItemStyle.Width = 100;

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
        }

        //FUNCION PARA MAIPULACION DE COLUMNAS EN GRID DE PASAJEROS
        private void columnasGridVendidos(bool ok)
        {
            dgvVendidos.Columns[0].Visible = ok;
            dgvVendidos.Columns[1].Visible = ok;
            dgvVendidos.Columns[2].Visible = ok;
            dgvVendidos.Columns[3].Visible = ok;
            dgvVendidos.Columns[8].Visible = ok;

            dgvVendidos.Columns[4].ItemStyle.Width = 150;
            dgvVendidos.Columns[5].ItemStyle.Width = 300;
            dgvVendidos.Columns[6].ItemStyle.Width = 150;
            dgvVendidos.Columns[7].ItemStyle.Width = 150;
            dgvVendidos.Columns[9].ItemStyle.Width = 100;

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
            dgvDatosExtras.Columns[15].ItemStyle.Width = 100;

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
                //cerrarModal();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL GRID DE BOLETOS VENDIDOS
        private void llenarGridVendidos()
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
                sSql += "id_ctt_pueblo id_destino, tasa_usuario, id_tasa_emitida" + Environment.NewLine;
                sSql += "from ctt_vw_factura" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["idFacturaAnular"] = dtConsulta.Rows[0][0].ToString();
                        Session["idPersonaAnular"] = dtConsulta.Rows[0][1].ToString();
                        lblFechaFactura.Text = Convert.ToDateTime(dtConsulta.Rows[0][2].ToString()).ToString("dd-MMM-yyyy");
                        lblIdentificacion.Text = dtConsulta.Rows[0][3].ToString();
                        lblRazonSocial.Text = dtConsulta.Rows[0][4].ToString();
                        lblClaveAcceso.Text = dtConsulta.Rows[0][5].ToString();

                        Session["identificacion_cliente"] = dtConsulta.Rows[0][3].ToString();
                        Session["nombre_cliente"] = dtConsulta.Rows[0][4].ToString();

                        string[] sSeparar = dtConsulta.Rows[0][6].ToString().Split('-');
                        Session["pueblo_origen"] = sSeparar[0].Trim();
                        Session["pueblo_destino"] = dtConsulta.Rows[0][7].ToString();
                        Session["fecha_viaje"] = Convert.ToDateTime(dtConsulta.Rows[0][8].ToString()).ToString("yyyy-MM-dd");
                        Session["hora_salida"] = Convert.ToDateTime(dtConsulta.Rows[0][9].ToString()).ToString("HH:mm");
                        Session["id_pueblo_origen"] = dtConsulta.Rows[0][10].ToString();
                        Session["id_pueblo_destino"] = dtConsulta.Rows[0][11].ToString();
                        Session["tasa_usuario_emitida"] = dtConsulta.Rows[0][12].ToString();
                        Session["id_tasa_usuario_emitida"] = dtConsulta.Rows[0][13].ToString();
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

                lblDetalleBus.Text = "FECHA SALIDA: " + dgvDatos.Rows[a].Cells[3].Text + " - HORA SALIDA: " + dgvDatos.Rows[a].Cells[6].Text + " - VEHÍCULO: " + dgvDatos.Rows[a].Cells[4].Text;

                llenarGridVendidos();
                pnlGrid.Visible = false;
                pnlVendidos.Visible = true;
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
            llenarGridVendidos();
        }

        protected void dgvVendidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvVendidos.SelectedIndex;
                columnasGridVendidos(true);
                iIdFactura = Convert.ToInt32(dgvVendidos.Rows[a].Cells[2].Text);
                int iTipoComprobante_R = Convert.ToInt32(dgvVendidos.Rows[a].Cells[8].Text);
                columnasGridVendidos(false);

                crearReporteImprimir(iTipoComprobante_R);
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
            //btnPopUp_ModalPopupExtender.Hide();
        }

        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            
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
            //btnPopUp_ModalPopupExtender.Hide();
        }

        protected void btnCancelarGrid_Click(object sender, EventArgs e)
        {
            pnlVendidos.Visible = false;
            pnlGrid.Visible = true;
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

                lblDetalleBus.Text = "FECHA SALIDA: " + dgvDatos.Rows[a].Cells[3].Text + " - HORA SALIDA: " + dgvDatos.Rows[a].Cells[6].Text + " - VEHÍCULO: " + dgvDatos.Rows[a].Cells[4].Text;

                llenarGridVendidos();
                pnlGrid.Visible = false;
                pnlVendidos.Visible = true;
            }

            catch (Exception ex)
            {
                //cerrarModal();
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
    }
}