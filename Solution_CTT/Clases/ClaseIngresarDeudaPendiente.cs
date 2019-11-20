using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using NEGOCIO;

namespace Solution_CTT.Clases
{
    public class ClaseIngresarDeudaPendiente
    {
        manejadorConexion conexionM = new manejadorConexion();
        ClaseParametros parametros = new ClaseParametros();

        string sSql;
        string sCiudad;
        string sDireccion;
        string sTelefono;
        string sCorreoElectronico;
        string sFecha;
        string sComentario;
        string sTabla;
        string sCampo;

        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;

        int iIdVehiculo;
        int iIdPersona;
        int iIdProducto;
        int iCgEstadoDctoPorCobrar;
        int iIdProgramacion;
        int iNumeroNotaVenta;
        int iIdFactura;
        int iIdPedido;
        int iIdPago;
        int iNumeroPago;
        int iCgTipoDocumento = 7456;
        int iIdDocumentoPago;
        int iIdDocumentoCobrar;
        int iIdEventoCobro;
        int iNumeroPedido;
        int iIdCabDespachos;
        int iIdDespachoPedido;

        bool bRespuesta;

        Decimal dbTotal;
        Decimal dbIngreso;
        Decimal dbIva;

        long iMaximo;

        public bool crearPagoPendiente(int iIdProgramacion_P, int iIdVehiculo_P, string sFecha_P, string[] sDatosMaximo_P, decimal dbIngreso_P, string sComentario_P)
        {
            try
            {
                iIdProgramacion = iIdProgramacion_P;
                sFecha = sFecha_P;
                sDatosMaximo = sDatosMaximo_P;
                dbIngreso = dbIngreso_P;
                iIdVehiculo = iIdVehiculo_P;
                sComentario = sComentario_P;

                if (!consultarPropietario())
                {
                    return false;
                }

                if (!conexionM.iniciarTransaccion())
                {
                    return false;
                }

                if (!creaRegistroPagos())
                {
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

        private bool consultarPropietario()
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_propietario_vehiculo_ingreso_pago_manual" + Environment.NewLine;
                sSql += "where id_ctt_vehiculo = " + iIdVehiculo;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (!bRespuesta == true)
                {
                    return false;
                }

                if (dtConsulta.Rows.Count <= 0)
                {
                    return false;
                }

                sCiudad = dtConsulta.Rows[0]["direccion"].ToString();
                sDireccion = dtConsulta.Rows[0]["calle_principal"].ToString();

                if (dtConsulta.Rows[0]["numero_vivienda"].ToString() != "")
                {
                    sDireccion = sDireccion + " " + dtConsulta.Rows[0]["numero_vivienda"].ToString();
                }

                if (dtConsulta.Rows[0]["calle_interseccion"].ToString() != "")
                {
                    sDireccion = sDireccion + " Y " + dtConsulta.Rows[0]["calle_interseccion"].ToString();
                }

                if (sDireccion.Trim().Length > 100)
                {
                    sDireccion = sDireccion.Trim().Substring(0, 100);
                }

                if (dtConsulta.Rows[0]["codigo_alterno"].ToString() != "")
                {
                    sTelefono = dtConsulta.Rows[0]["codigo_alterno"].ToString();
                }

                else
                {
                    sTelefono = dtConsulta.Rows[0]["referencia"].ToString();
                }

                sCorreoElectronico = dtConsulta.Rows[0]["correo_electronico"].ToString();
                iIdPersona = Convert.ToInt32(dtConsulta.Rows[0]["id_persona"].ToString());

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        private bool creaRegistroPagos()
        {
            try
            {
                dbTotal = Convert.ToDecimal(HttpContext.Current.Session["pago_administracion"].ToString());
                iIdProducto = Convert.ToInt32(HttpContext.Current.Session["id_producto_pagos"].ToString());

                if (dbIngreso == 0)
                {
                    iCgEstadoDctoPorCobrar = 7460;
                }

                else
                {
                    iCgEstadoDctoPorCobrar = 7462;
                }

                if (Convert.ToInt32(HttpContext.Current.Session["paga_iva_pagos"].ToString()) == 0)
                {
                    dbIva = 0;
                }
                else
                {
                    dbIva = (dbTotal * Convert.ToDecimal(HttpContext.Current.Application["iva"].ToString())) / 100M;
                }

                if (!insertarPedido())
                {
                    conexionM.reversaTransaccion();
                    return false;
                }

                if ((dbIngreso > 0M) && !insertarPagos())
                {
                    conexionM.reversaTransaccion();
                    return false;
                }

                if (!insertarFactura())
                {
                    conexionM.reversaTransaccion();
                    return false;
                }

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        private bool insertarFactura()
        {
            try
            {
                sSql = "";
                sSql += "insert into cv403_facturas (idempresa, id_persona, cg_empresa, idtipocomprobante," + Environment.NewLine;
                sSql += "id_localidad, idformulariossri, id_vendedor, id_forma_pago, fecha_factura, fecha_vcto," + Environment.NewLine;
                sSql += "cg_moneda, valor, cg_estado_factura, editable, fecha_ingreso, usuario_ingreso, " + Environment.NewLine;
                sSql += "terminal_ingreso, estado," + Environment.NewLine;
                sSql += "Direccion_Factura,Telefono_Factura,Ciudad_Factura, correo_electronico)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idEmpresa"].ToString()) + ", " + iIdPersona + ", " + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["cgEmpresa"].ToString()) + ", " + Convert.ToInt32(HttpContext.Current.Application["id_comprobante"].ToString()) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString()) + ", 19, " + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Session["idVendedor"].ToString()) + ", 14, '" + sFecha + "'," + Environment.NewLine;
                sSql += "'" + sFecha + "', " + Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + ", " + dbTotal + ", 0, 0, GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A'," + Environment.NewLine;
                sSql += "'" + sDireccion + "', '" + sTelefono + "', '" + sCiudad + "'," + Environment.NewLine;
                sSql += "'" + sCorreoElectronico + "')";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sTabla = "cv403_facturas";
                sCampo = "id_factura";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    return false;
                }

                iIdFactura = Convert.ToInt32(iMaximo);

                sSql = "";
                sSql += "select numeronotaventa" + Environment.NewLine;
                sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and id_localidad = " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    iNumeroNotaVenta = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());                    
                }

                else
                {
                    return false;
                }

                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numeronotaventa = numeronotaventa + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());
                
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                sSql = "";
                sSql += "insert into cv403_numeros_facturas (id_factura, idtipocomprobante, numero_factura, " + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, estado) " + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdFactura + ", " + Convert.ToInt32(HttpContext.Current.Application["id_comprobante"].ToString()) + ", " + iNumeroNotaVenta + ", GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A')";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                sSql = "";
                sSql += "update cv403_dctos_por_cobrar set" + Environment.NewLine;
                sSql += "id_factura = " + iIdFactura + "," + Environment.NewLine;
                sSql += "numero_documento = " + iNumeroNotaVenta + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido;

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                sSql = "";
                sSql += "insert into cv403_facturas_pedidos (" + Environment.NewLine;
                sSql += "id_factura, id_pedido, fecha_ingreso, usuario_ingreso, " + Environment.NewLine;
                sSql += "terminal_ingreso, estado) " + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdFactura + ", " + iIdPedido + ", GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A')";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                return true;
                
            }

            catch (Exception)
            {
                return false;
            }
        }

        private bool insertarPagos()
        {
            try
            {
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
                    return false;
                }

                sSql = "";
                sSql += "insert into cv403_pagos (" + Environment.NewLine;
                sSql += "idempresa, id_persona, fecha_pago, cg_moneda, valor," + Environment.NewLine;
                sSql += "propina, cg_empresa, id_localidad, cg_cajero, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso, estado, cambio)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idEmpresa"].ToString()) + ", " + Environment.NewLine;
                sSql += iIdPersona + ", '" + sFecha + "', " + Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + "," + Environment.NewLine;
                sSql += dbIngreso + ", 0, " + Convert.ToInt32(HttpContext.Current.Application["cgEmpresa"].ToString()) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString()) + ", 7799, GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 'A', 0)";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                sTabla = "cv403_pagos";
                sCampo = "id_pago";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    return false;
                }

                iIdPago = Convert.ToInt32(iMaximo);

                sSql = "";
                sSql += "select numero_pago" + Environment.NewLine;
                sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    iNumeroPago = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());                    
                }

                else
                {
                    return false;
                }

                sSql = "";
                sSql += "insert into cv403_numeros_pagos (" + Environment.NewLine;
                sSql += "id_pago, serie, numero_pago, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                sSql += "terminal_ingreso, estado)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += iIdPago + ", 'A', " + iNumeroPago + ", GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 'A')";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                sSql = "";
                sSql += "insert into cv403_documentos_pagos (" + Environment.NewLine;
                sSql += "id_pago, cg_tipo_documento, numero_documento, fecha_vcto, " + Environment.NewLine;
                sSql += "cg_moneda, cotizacion, valor, estado," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, valor_recibido)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += iIdPago + ", " + iCgTipoDocumento + ", 9999, '" + sFecha + "', " + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + ", 1, " + dbIngreso + ", 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', " + dbIngreso + ")";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                sTabla = "cv403_documentos_pagos";
                sCampo = "id_documento_pago";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1L)
                {
                    return false;
                }

                iIdDocumentoPago = Convert.ToInt32(iMaximo);

                sSql = "";
                sSql += "insert into cv403_documentos_pagados (" + Environment.NewLine;
                sSql += "id_documento_cobrar, id_pago, valor, estado," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, fecha_pago, id_ctt_jornada)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dbIngreso + ", 'A'," + Environment.NewLine;
                sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', '" + DateTime.Now.ToString("yyyy/MM/dd") + "'," + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Session["idJornada"].ToString()) + ")";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numero_pago = numero_pago + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        private bool insertarPedido()
        {
            try
            {
                sSql = "";
                sSql += "insert into cv403_cab_pedidos (" + Environment.NewLine;
                sSql += "idempresa, cg_empresa, id_localidad, fecha_pedido, id_persona, cg_tipo_cliente," + Environment.NewLine;
                sSql += "cg_moneda, porcentaje_iva, id_vendedor, cg_estado_pedido, porcentaje_dscto," + Environment.NewLine;
                sSql += "cg_facturado, id_ctt_programacion, id_ctt_oficinista," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso, ctt_fecha_pago_pendiente," + Environment.NewLine;
                sSql += "cobro_boletos, cobro_retencion, cobro_administrativo, pago_cumplido, id_ctt_jornada," + Environment.NewLine;
                sSql += "pago_pendiente_info, ingreso_efectivo_info, comentarios)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idEmpresa"].ToString()) + ", " + Convert.ToInt32(HttpContext.Current.Application["cgEmpresa"].ToString()) + ", ";
                sSql += Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString()) + ", '" + sFecha + "', " + iIdPersona + "," + Environment.NewLine;
                sSql += parametros.CgTipoCliente + ", " + Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + ", 0, " + Convert.ToInt32(HttpContext.Current.Session["idVendedor"].ToString()) + ", ";
                sSql += parametros.CgEstadoPedido + ", 0, 7469, " + iIdProgramacion + "," + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Session["idUsuario"]) + ", ";
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', null, 0, 0, 1, 0, ";
                sSql += Convert.ToInt32(HttpContext.Current.Session["idJornada"].ToString()) + ", 0, 0, '" + sComentario + "')";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                sSql = "";
                sSql += "insert into cv403_cab_despachos (" + Environment.NewLine;
                sSql += "idempresa, id_persona, cg_empresa, id_localidad, fecha_despacho," + Environment.NewLine;
                sSql += "cg_motivo_despacho, id_destinatario, punto_partida, cg_ciudad_entrega," + Environment.NewLine;
                sSql += "direccion_entrega, id_transportador, fecha_inicio_transporte," + Environment.NewLine;
                sSql += "fecha_fin_transporte, cg_estado_despacho, punto_venta, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso, estado)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idEmpresa"].ToString()) + ", " + iIdPersona + ", " + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["cgEmpresa"].ToString()) + ", " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString()) + ", ";
                sSql += "'" + sFecha + "', 6972, " + iIdPersona + "," + Environment.NewLine;
                sSql += "'" + parametros.sPuntoPartida + "', " + parametros.iCgCiudadEntrega + ", '" + parametros.sDireccionEntrega + "'," + Environment.NewLine;
                sSql += "'" + iIdPersona + "', GETDATE(), GETDATE(), " + parametros.iCgEstadoDespacho + "," + Environment.NewLine;
                sSql += "1, GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A')";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                sTabla = "cv403_cab_pedidos";
                sCampo = "id_pedido";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    return false;
                }

                iIdPedido = Convert.ToInt32(iMaximo);

                sSql = "";
                sSql += "select numero_pedido" + Environment.NewLine;
                sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and id_localidad = " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    iNumeroPedido = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                }

                else
                {
                    return false;
                }

                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numero_pedido = numero_pedido + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                sSql = "";
                sSql += "insert into cv403_numero_cab_pedido (" + Environment.NewLine;
                sSql += "idtipocomprobante,id_pedido, numero_pedido," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, estado)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql +=  Convert.ToInt32(HttpContext.Current.Application["id_comprobante"].ToString()) + ", " + iIdPedido + ", " + iNumeroPedido + ", GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A')";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                sTabla = "cv403_cab_despachos";
                sCampo = "id_despacho";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    return false;
                }

                iIdCabDespachos = Convert.ToInt32(iMaximo);

                sSql = "";
                sSql += "insert into cv403_despachos_pedidos (" + Environment.NewLine;
                sSql += "id_despacho, id_pedido, estado, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                sSql += "terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdCabDespachos + ", " + iIdPedido + ", 'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "')";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sTabla = "cv403_despachos_pedidos";
                sCampo = "id_despacho_pedido";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    return false;
                }

                iIdDespachoPedido = Convert.ToInt32(iMaximo);

                sSql = "";
                sSql += "insert into cv403_eventos_cobros (" + Environment.NewLine;
                sSql += "idempresa, cg_empresa, id_persona, id_localidad, cg_evento_cobro," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, estado)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idEmpresa"].ToString()) + ", " + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["cgEmpresa"].ToString()) + ", " + iIdPersona + ", " + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString()) + "," + Environment.NewLine;
                sSql += "7466, GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A')";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sTabla = "cv403_eventos_cobros";
                sCampo = "id_evento_cobro";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1L)
                {
                    return false;
                }

                iIdEventoCobro = Convert.ToInt32(iMaximo);

                sSql = "";
                sSql += "insert into cv403_dctos_por_cobrar (" + Environment.NewLine;
                sSql += "id_evento_cobro, id_pedido, cg_tipo_documento, fecha_vcto, cg_moneda," + Environment.NewLine;
                sSql += "valor, cg_estado_dcto, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdEventoCobro + ", " + iIdPedido + ", " + parametros.CgTipoDocumento + "," + Environment.NewLine;
                sSql += "'" + sFecha + "', " + Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + ", " + dbIngreso + "," + Environment.NewLine;
                sSql += iCgEstadoDctoPorCobrar + ", 'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "')";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                sSql = "";
                sSql += "Insert Into cv403_det_pedidos(" + Environment.NewLine;
                sSql += "Id_Pedido, id_producto, Cg_Unidad_Medida, precio_unitario," + Environment.NewLine;
                sSql += "Cantidad, Valor_Dscto, Valor_Ice, Valor_Iva," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                sSql += "terminal_ingreso)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += iIdPedido + ", " + iIdProducto + ", 546, " + dbTotal + ", " + Environment.NewLine;
                sSql += "1 , 0, 0, " + dbIva + ", " + Environment.NewLine;
                sSql += "'A',  GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }


  



    }
}