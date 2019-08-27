using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using NEGOCIO;

namespace Solution_CTT.Clases
{
    public class ClaseInstruccionesCierre
    {
        manejadorConexion conexionM = new manejadorConexion();

        Clases.ClaseParametros parametros = new Clases.ClaseParametros();

        string sSql;
        string sTabla;
        string sCampo;
        string sFecha;
        string sCiudad;
        string sDireccion;
        string sTelefono;
        string sCorreoElectronico;        
        string []sDatosMaximo = new string[5];

        int iIdPersona;
        int iIdPersonaOrigen;
        int iIdPersonaReemplazo;
        int iIdProgramacion;
        int iIdPropietario;
        int iIdPedido;
        int iNumeroPedido;
        int iIdCabDespachos;
        int iIdDespachoPedido;
        int iIdEventoCobro;
        int iIdDocumentoCobrar;
        int iIdPago;
        int iNumeroPago;
        int iIdDocumentoPago;
        int iIdFactura;
        int iNumeroFactura;
        int iCgEstadoDctoPorCobrar;
        int iIdProducto;
        int iBanderaRetencion;
        int iBanderaPago;
        int iEstadoXCobrar;
        int iBanderaPagoCumplido;

        int iCobraRetencion;
        int iCobraAdministracion;

        /*  7460    Pendiente
            7461    Pagado
            7462    Pagado Parcial
         */

        int iCgTipoDocumento = 7456;

        double dbTotalCobrado;
        double dbTotal;
        double dbIva;
        double dbPagoAdministracion;
        double dbPagoRetencion;
        double dbPagoAcordado;
        double dbPagoRecaudado;

        Decimal dbPagoPendienteInfo;
        Decimal dbIngresoEfectivoInfo;

        long iMaximo;

        DataTable dtConsulta;

        bool bRespuesta;

        public bool iniciarCierre(DataTable dtConsulta_P, double dbTotalCobrado_P, double dbPagoRetencion_P, 
            double dbPagoAdministracion_P, int iIdProgramacion_P, string sFecha_P, string[] sDatosMaximo_P, 
            string[,] sIdPedido_P, int iLongitud_P, int iExtra_P, Decimal dbPagoPendienteInfo_P, Decimal dbIngresoEfectivoInfo_P)
        {
            try
            {
                this.dbPagoRetencion = dbPagoRetencion_P;
                this.dbPagoAdministracion = dbPagoAdministracion_P;
                this.dbTotalCobrado = dbTotalCobrado_P;                
                this.iIdProgramacion = iIdProgramacion_P;
                this.sFecha = sFecha_P;
                this.sDatosMaximo = sDatosMaximo_P;

                //CONSULTAR LOS DATOS DEL PROPIETARIO
                if (consultarPropietario() == false)
                {
                    return false;
                }

                //INICIA TRANSACCION SQL
                if (conexionM.iniciarTransaccion() == false)
                {                    
                    return false;
                }

                iCobraRetencion = 1;
                iCobraAdministracion = 0;
                dbPagoPendienteInfo = 0;
                dbIngresoEfectivoInfo = 0;

                //PRIMERA LLAMADA PARA INSERTAR EL REGISTRO DE RETENCION
                if (creaRegistroRetencion(dbPagoRetencion) == false)
                {
                    conexionM.reversaTransaccion();
                    return false;
                }

                iCobraRetencion = 0;
                iCobraAdministracion = 1;
                this.dbPagoPendienteInfo = dbPagoPendienteInfo_P;
                this.dbIngresoEfectivoInfo = dbIngresoEfectivoInfo_P;


                if (iExtra_P == 0)
                {
                    //SEGUNDA LLAMADA PARA INSERTAR EL PAGO DE ADMINISTRACION
                    if (creaRegistroPagos(dbPagoAdministracion, iExtra_P) == false)
                    {
                        conexionM.reversaTransaccion();
                        return false;
                    }
                }

                //INSTRUCCION SQL PARA CERRAR  EL VIAJE
                sSql = "";
                sSql += "update ctt_programacion set" + Environment.NewLine;
                sSql += "estado_salida = 'Cerrada'" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " +iIdProgramacion + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    conexionM.reversaTransaccion();
                    return false;
                }

                //INSTRUCCIONES SQL PARA ACTUALIZAR LOS DOCUMENTOS POR COBRAR
                for (int i = 0; i < iLongitud_P; i++)
                {
                    if (sIdPedido_P[i, 0] != null)
                    {
                        //AQUI VERIFICAR SEGUN ESTADO DOCUMENTO
                        //7460 - INSERTAR TODO EL PAGO
                        //7462 - RECUPERAR EL ID DOCUMENTO PAGO, LUEGO EL ID PAGO E INSERTAR EL DOCUMENTO PAGO NUEVO

                        iEstadoXCobrar = Convert.ToInt32(sIdPedido_P[i, 2]);

                        if (iEstadoXCobrar == 7460)
                        {
                            //cobrarPagoPendiente(int iIdPedido_P, double dbPago_P)
                            if (cobrarPagoPendiente(Convert.ToInt32(sIdPedido_P[i, 0]), Convert.ToDouble(sIdPedido_P[i, 1])) == false)
                            {
                                return false;
                            }
                        }

                        else if (iEstadoXCobrar == 7462)
                        {
                            if (cobrarPagoParcial(Convert.ToInt32(sIdPedido_P[i, 0]), Convert.ToDouble(sIdPedido_P[i, 1])) == false)
                            {
                                return false;
                            }
                        }                       
                    }
                }

                conexionM.terminaTransaccion();
                return true;
            }

            catch(Exception)
            {
                conexionM.reversaTransaccion();
                return false;
            }
        }

        //PRIMER PAGO A INSERTAR CONSULTAR LOS DATOS DE LA RETENCION
        private bool creaRegistroRetencion(double dbPago)
        {
            try
            {
                dbTotal = dbPago;
                dbPagoRecaudado = dbPago;
                iCgEstadoDctoPorCobrar = 7461;
                iIdPersona = iIdPersonaOrigen;
                iIdProducto = Convert.ToInt32(HttpContext.Current.Session["id_producto_retencion"].ToString());

                //CONTROL DE IVA
                if (Convert.ToInt32(HttpContext.Current.Session["paga_iva_retencion"].ToString()) == 0)
                {
                    dbIva = 0;
                }

                else
                {
                    dbIva = (dbTotal * Convert.ToDouble(HttpContext.Current.Application["iva"].ToString())) / 100;
                }

                if (insertarPedido() == false)
                {
                    conexionM.reversaTransaccion();
                    return false;
                }

                if (insertarPagos() == false)
                {
                    conexionM.reversaTransaccion();
                    return false;
                }

                if (insertarFactura() == false)
                {
                    conexionM.reversaTransaccion();
                    return false;
                }

                return true;
            }

            catch(Exception)
            {
                return false;
            }
        }

        //SEGUNDO PAGO A INSERTAR - CONSULTAR LOS DATOS DEL PAGO DE ADMINISTRACION
        private bool creaRegistroPagos(double dbPagoRecibido, int iExtraParametro)
        {
            try
            {
                dbPagoRecaudado = dbPagoRecibido;

                if (iExtraParametro == 0)
                {
                    dbTotal = Convert.ToDouble(HttpContext.Current.Session["pago_administracion"].ToString());
                    iIdProducto = Convert.ToInt32(HttpContext.Current.Session["id_producto_pagos"].ToString());
                }

                else
                {
                    dbTotal = Convert.ToDouble(HttpContext.Current.Application["precio_producto_extra"].ToString());
                    iIdProducto = Convert.ToInt32(HttpContext.Current.Application["id_producto_extra"].ToString());
                }

                if (dbPagoRecibido == dbTotal)
                {
                    iCgEstadoDctoPorCobrar = 7461;
                    iIdPersona = iIdPersonaOrigen;
                    dbPagoRecaudado = dbTotal;
                    iBanderaPagoCumplido = 1;
                }

                else if (dbPagoRecibido == 0)
                {
                    iCgEstadoDctoPorCobrar = 7460;                    
                    dbPagoRecaudado = 0;
                    iBanderaPagoCumplido = 0;

                    if (iIdPersonaReemplazo == 0)
                    {
                        iIdPersona = iIdPersonaOrigen;
                    }

                    else
                    {
                        iIdPersona = iIdPersonaReemplazo;
                    }
                }

                else if (dbPagoRecibido < dbTotal)
                {
                    iCgEstadoDctoPorCobrar = 7462;
                    dbPagoRecaudado = dbTotal - dbPagoRecibido;
                    iBanderaPagoCumplido = 0;
                }

                //CONTROL DE IVA
                if (Convert.ToInt32(HttpContext.Current.Session["paga_iva_pagos"].ToString()) == 0)
                {
                    dbIva = 0;
                }

                else
                {
                    dbIva = (dbTotal * Convert.ToDouble(HttpContext.Current.Application["iva"].ToString())) / 100;
                }

                if (insertarPedido() == false)
                {
                    conexionM.reversaTransaccion();
                    return false;
                }

                if (dbPagoRecibido > 0)
                {
                    if (insertarPagos() == false)
                    {
                        conexionM.reversaTransaccion();
                        return false;
                    }
                }

                if (insertarFactura() == false)
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

        //CONSULTAR LOS DATOS DEL PROPIETARIO DEL VEHICULO
        private bool consultarPropietario()
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_propietario_vehiculo_cierre" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + iIdProgramacion;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iIdPersonaOrigen = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                        if (dtConsulta.Rows[0][11].ToString() == "0")
                        {                            
                            sCiudad = dtConsulta.Rows[0][2].ToString();
                            sDireccion = dtConsulta.Rows[0][3].ToString();

                            if (dtConsulta.Rows[0][4].ToString() != "")
                            {
                                sDireccion += " " + dtConsulta.Rows[0][4].ToString();
                            }

                            if (dtConsulta.Rows[0][5].ToString() != "")
                            {
                                sDireccion += " Y " + dtConsulta.Rows[0][5].ToString();
                            }

                            if (sDireccion.Trim().Length > 100)
                            {
                                sDireccion = sDireccion.Trim().Substring(0, 100);
                            }

                            if (dtConsulta.Rows[0][8].ToString() != "")
                            {
                                sTelefono = dtConsulta.Rows[0][8].ToString();
                            }

                            else
                            {
                                sTelefono = dtConsulta.Rows[0][7].ToString();
                            }

                            sCorreoElectronico = dtConsulta.Rows[0][9].ToString();
                        }

                        else
                        {
                            if (consultarPropietarioReemplazo(iIdPersonaOrigen) == false)
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
                        return false;
                    }
                }

                else
                {
                    return false;
                }

                return true;
            }

            catch(Exception)
            {
                return false;
            }
        }


        //CONSULTAR LOS DATOS DEL PROPIETARIO DEL VEHICULO DE REEMPLAZO
        private bool consultarPropietarioReemplazo(int iIdPersona_P)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_propietario_vehiculo_reemplazo" + Environment.NewLine;
                sSql += "where id_persona = " + iIdPersona_P;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iIdPersonaReemplazo = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                        sCiudad = dtConsulta.Rows[0][2].ToString();
                        sDireccion = dtConsulta.Rows[0][3].ToString();

                        if (dtConsulta.Rows[0][4].ToString() != "")
                        {
                            sDireccion += " " + dtConsulta.Rows[0][4].ToString();
                        }

                        if (dtConsulta.Rows[0][5].ToString() != "")
                        {
                            sDireccion += " Y " + dtConsulta.Rows[0][5].ToString();
                        }

                        if (sDireccion.Trim().Length > 100)
                        {
                            sDireccion = sDireccion.Trim().Substring(0, 100);
                        }

                        if (dtConsulta.Rows[0][8].ToString() != "")
                        {
                            sTelefono = dtConsulta.Rows[0][8].ToString();
                        }

                        else
                        {
                            sTelefono = dtConsulta.Rows[0][7].ToString();
                        }

                        sCorreoElectronico = dtConsulta.Rows[0][9].ToString();
                    }

                    else
                    {
                        return false;
                    }
                }

                else
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


        //INSERTAR FASE 1 -  CREAR PEDIDO
        private bool insertarPedido()
        {
            try
            {                  
                //INSTRUCCION PARA INSERTAR EN LA TABLA CV403_CAB_PEDIDOS
                sSql = "";
                sSql += "insert into cv403_cab_pedidos (" + Environment.NewLine;
                sSql += "idempresa, cg_empresa, id_localidad, fecha_pedido, id_persona, cg_tipo_cliente," + Environment.NewLine;
                sSql += "cg_moneda, porcentaje_iva, id_vendedor, cg_estado_pedido, porcentaje_dscto," + Environment.NewLine;
                sSql += "cg_facturado, id_ctt_programacion, id_ctt_oficinista," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso, ctt_fecha_pago_pendiente," + Environment.NewLine;
                sSql += "cobro_boletos, cobro_retencion, cobro_administrativo, pago_cumplido, id_ctt_jornada," + Environment.NewLine;
                sSql += "pago_pendiente_info, ingreso_efectivo_info)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idEmpresa"].ToString()) + ", " + Convert.ToInt32(HttpContext.Current.Application["cgEmpresa"].ToString()) + ", ";
                sSql += Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString()) + ", '" + sFecha + "', " + iIdPersona + "," + Environment.NewLine;
                sSql += parametros.CgTipoCliente + ", " + Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + ", 0, " + Convert.ToInt32(HttpContext.Current.Session["idVendedor"].ToString()) + ",";
                sSql += parametros.CgEstadoPedido + ", 0, 7469, " + iIdProgramacion + "," + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Session["idUsuario"]) + ", ";
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', ";

                if (iCgEstadoDctoPorCobrar == 7461)
                {
                    sSql += "'" + sFecha + "', ";
                }

                else
                {
                    sSql += "null, ";
                }

                sSql += "0, " + iCobraRetencion + ", " + iCobraAdministracion + ", " + iBanderaPagoCumplido + ", ";
                sSql += Convert.ToInt32(HttpContext.Current.Session["idJornada"].ToString()) + ", ";
                sSql += dbPagoPendienteInfo + ", " + dbIngresoEfectivoInfo + ")";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {                    
                    return false;
                }

                //QUERY PARA INSERTAR EN CV403_CAB_DESPACHOS
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

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    return false;
                }

                //OBTENER EL ID DE LA TABLA CV403_CAB_PEDIDOS
                sTabla = "cv403_cab_pedidos";
                sCampo = "id_pedido";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
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

                //QUERY PARA ACTUALIZAR EL NUMERO DE PEDIDO EN LA TABLA TP_LOCALIDADES_IMPRESORAS
                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numero_pedido = numero_pedido + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //QUERY PARA PODER INSERTAR REGISTRO EN LA TABLA CV403_NUMERO_CAB_PEDIDO
                sSql = "";
                sSql += "insert into cv403_numero_cab_pedido (" + Environment.NewLine;
                sSql += "idtipocomprobante,id_pedido, numero_pedido," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, estado)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "1, " + iIdPedido + ", " + iNumeroPedido + ", GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A')";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
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
                sSql += "terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdCabDespachos + "," + iIdPedido + ", 'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "')";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
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
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, estado)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idEmpresa"].ToString()) + ", " + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["cgEmpresa"].ToString()) + ", " + iIdPersona + ", " + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString()) + "," + Environment.NewLine;
                sSql += "7466, GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A')";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
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
                sSql += "valor, cg_estado_dcto, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdEventoCobro + ", " + iIdPedido + ", " + parametros.CgTipoDocumento + "," + Environment.NewLine;
                sSql += "'" + sFecha + "', " + Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + ", " + dbPagoAdministracion + "," + Environment.NewLine;
                sSql += iCgEstadoDctoPorCobrar + ", 'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "')";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //QUERY PARA INSERTAR EN LA TABLA CV403_DET_PEDIDOS
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

                //EJECUCION DE INSTRUCCION SQL
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
                    return false;
                }

                //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV403_PAGOS
                sSql = "";
                sSql += "insert into cv403_pagos (" + Environment.NewLine;
                sSql += "idempresa, id_persona, fecha_pago, cg_moneda, valor," + Environment.NewLine;
                sSql += "propina, cg_empresa, id_localidad, cg_cajero, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso, estado, cambio)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idEmpresa"].ToString()) + ", " + Environment.NewLine;
                sSql += iIdPersona + ", '" + sFecha + "', " + Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + "," + Environment.NewLine;
                sSql += dbPagoAdministracion + ", 0, " + Convert.ToInt32(HttpContext.Current.Application["cgEmpresa"].ToString()) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString()) + ", 7799, GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 'A', 0)";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
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

                //INSERTAMOS EN LA TABLA CV403_NUMEROS_PAGOS
                sSql = "";
                sSql += "insert into cv403_numeros_pagos (" + Environment.NewLine;
                sSql += "id_pago, serie, numero_pago, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                sSql += "terminal_ingreso, estado)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += iIdPago + ", 'A', " + iNumeroPago + ", GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 'A')";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //INSERTAMOS EN LA TABLA CV403_DOCUMENTOS_PAGOS 
                sSql = "";
                sSql += "insert into cv403_documentos_pagos (" + Environment.NewLine;
                sSql += "id_pago, cg_tipo_documento, numero_documento, fecha_vcto, " + Environment.NewLine;
                sSql += "cg_moneda, cotizacion, valor, estado," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, valor_recibido)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += iIdPago + ", " + iCgTipoDocumento + ", 9999, '" + sFecha + "', " + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + ", 1, " + dbPagoAdministracion + ", 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', " + dbPagoAdministracion + ")";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
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
                    return false;
                }

                else
                {
                    iIdDocumentoPago = Convert.ToInt32(iMaximo);
                }

                //INSERTAMOS EL ÚNICO DOCUMENTO PAGADO
                sSql = "";
                sSql += "insert into cv403_documentos_pagados (" + Environment.NewLine;
                sSql += "id_documento_cobrar, id_pago, valor, estado," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, fecha_pago, id_ctt_jornada)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dbPagoAdministracion + ", 'A'," + Environment.NewLine;
                sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', '" + DateTime.Now.ToString("yyyy/MM/dd") + "'," + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Session["idJornada"].ToString()) + ")";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //ACTUALIZAR EL NUMERO DE PAGOS EN LA TABLA TP_LOCALIDADES_IMPRESORAS
                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numero_pago = numero_pago + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());

                //EJECUCION DE INSTRUCCION SQL
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
                    return false;
                }

                //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV403_FACTURAS
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

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
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
                    return false;
                }

                else
                {
                    iIdFactura = Convert.ToInt32(iMaximo);
                }


                //INSTRUCCION SQL PARA EXTRAER EL NUMERO DE FACTURA
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
                    iNumeroFactura = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                }

                else
                {
                    return false;
                }

                //QUERY PARA ACTUALIZAR EL NUMERO DE PEDIDO EN LA TABLA TP_LOCALIDADES_IMPRESORAS
                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numeronotaventa = numeronotaventa + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //INSERTAR EN LA TABLA CV403_NUMEROS_FACTURAS
                sSql = "";
                sSql += "insert into cv403_numeros_facturas (id_factura, idtipocomprobante, numero_factura, " + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, estado) " + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdFactura + ", 2, " + iNumeroFactura + ", GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A')";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //ACTUALIZAMOS LA TABLA CV403_DCTOS_POR_COBRAR
                sSql = "";
                sSql += "update cv403_dctos_por_cobrar set" + Environment.NewLine;
                sSql += "id_factura = " + iIdFactura + "," + Environment.NewLine;
                //sSql += "cg_estado_dcto = " + iCgEstadoDctoPorCobrar + "," + Environment.NewLine;
                sSql += "numero_documento = " + iNumeroFactura + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido;

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV403_FACTURAS_PEDIDOS
                sSql = "";
                sSql += "insert into cv403_facturas_pedidos (" + Environment.NewLine;
                sSql += "id_factura, id_pedido, fecha_ingreso, usuario_ingreso, " + Environment.NewLine;
                sSql += "terminal_ingreso, estado) " + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdFactura + ", " + iIdPedido + ", GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A')";

                //EJECUCION DE INSTRUCCION SQL
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

        //======================================================================================================================
        //COBRO TOTAL PAGOS PENDIENTES

        //INSERTAR PAGO PENDIENTE
        private bool cobrarPagoPendiente(int iIdPedido_P, double dbPago_P)
        {
            try
            {
                //EXTRAER EL ID DE LA TABLA CV403_DCTOS_POR_COBRAR
                sSql = "";
                sSql += "select id_documento_cobrar" + Environment.NewLine;
                sSql += "from cv403_dctos_por_cobrar" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P + Environment.NewLine;
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

                //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV403_PAGOS
                sSql = "";
                sSql += "insert into cv403_pagos (" + Environment.NewLine;
                sSql += "idempresa, id_persona, fecha_pago, cg_moneda, valor," + Environment.NewLine;
                sSql += "propina, cg_empresa, id_localidad, cg_cajero, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso, estado, cambio)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idEmpresa"].ToString()) + ", " + Environment.NewLine;
                sSql += iIdPersona + ", '" + sFecha + "', " + Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + "," + Environment.NewLine;
                sSql += dbPago_P + ", 0, " + Convert.ToInt32(HttpContext.Current.Application["cgEmpresa"].ToString()) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString()) + ", 7799, GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 'A', 0)";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
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

                //INSERTAMOS EN LA TABLA CV403_NUMEROS_PAGOS
                sSql = "";
                sSql += "insert into cv403_numeros_pagos (" + Environment.NewLine;
                sSql += "id_pago, serie, numero_pago, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                sSql += "terminal_ingreso, estado)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += iIdPago + ", 'A', " + iNumeroPago + ", GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 'A')";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //INSERTAMOS EN LA TABLA CV403_DOCUMENTOS_PAGOS 
                sSql = "";
                sSql += "insert into cv403_documentos_pagos (" + Environment.NewLine;
                sSql += "id_pago, cg_tipo_documento, numero_documento, fecha_vcto, " + Environment.NewLine;
                sSql += "cg_moneda, cotizacion, valor, estado," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, valor_recibido)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += iIdPago + ", " + iCgTipoDocumento + ", 9999, '" + sFecha + "', " + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + ", 1, " + dbPago_P + ", 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', " + dbPago_P + ")";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
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
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso, fecha_pago, id_ctt_jornada)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dbPago_P + ", 'A'," + Environment.NewLine;
                sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', '" + DateTime.Now.ToString("yyyy/MM/dd") + "'," + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Session["idJornada"].ToString()) + ")";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //ACTUALIZAR EL NUMERO DE PAGOS EN LA TABLA TP_LOCALIDADES_IMPRESORAS
                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numero_pago = numero_pago + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //INSTRUCCION PARA ACTUALIZAR EL ESTADO DEL DOCUMENTO POR COBRAR
                sSql = "";
                sSql += "update cv403_dctos_por_cobrar set" + Environment.NewLine;
                sSql += "cg_estado_dcto = 7461," + Environment.NewLine;
                sSql += "valor = " + dbPago_P + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    conexionM.reversaTransaccion();
                    return false;
                }

                //INSTRUCCION PARA ACTUALIZAR LA FECHA DE PAGO PENDIENTE 
                sSql = "";
                sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                sSql += "ctt_fecha_pago_pendiente = '" + sFecha + "'" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
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

        private bool cobrarPagoParcial(int iIdPedido_P, double dbPago_P)
        {
            try
            {
                //EXTRAER EL ID DE LA TABLA CV403_DCTOS_POR_COBRAR
                sSql = "";
                sSql += "select id_documento_cobrar" + Environment.NewLine;
                sSql += "from cv403_dctos_por_cobrar" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P + Environment.NewLine;
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

                //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV403_PAGOS
                sSql = "";
                sSql += "insert into cv403_pagos (" + Environment.NewLine;
                sSql += "idempresa, id_persona, fecha_pago, cg_moneda, valor," + Environment.NewLine;
                sSql += "propina, cg_empresa, id_localidad, cg_cajero, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso, estado, cambio)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idEmpresa"].ToString()) + ", " + Environment.NewLine;
                sSql += iIdPersona + ", '" + sFecha + "', " + Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + "," + Environment.NewLine;
                sSql += dbPago_P + ", 0, " + Convert.ToInt32(HttpContext.Current.Application["cgEmpresa"].ToString()) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString()) + ", 7799, GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 'A', 0)";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
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

                //INSERTAMOS EN LA TABLA CV403_NUMEROS_PAGOS
                sSql = "";
                sSql += "insert into cv403_numeros_pagos (" + Environment.NewLine;
                sSql += "id_pago, serie, numero_pago, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                sSql += "terminal_ingreso, estado)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += iIdPago + ", 'A', " + iNumeroPago + ", GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "', 'A')";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //INSERTAMOS EN LA TABLA CV403_DOCUMENTOS_PAGOS 
                sSql = "";
                sSql += "insert into cv403_documentos_pagos (" + Environment.NewLine;
                sSql += "id_pago, cg_tipo_documento, numero_documento, fecha_vcto, " + Environment.NewLine;
                sSql += "cg_moneda, cotizacion, valor, estado," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, valor_recibido)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += iIdPago + ", " + iCgTipoDocumento + ", 9999, '" + sFecha + "', " + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + ", 1, " + dbPago_P + ", 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', " + dbPago_P + ")";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
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
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso, fecha_pago, id_ctt_jornada)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dbPago_P + ", 'A'," + Environment.NewLine;
                sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', '" + DateTime.Now.ToString("yyyy/MM/dd") + "'," + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Session["idJornada"].ToString()) + ")";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //ACTUALIZAR EL NUMERO DE PAGOS EN LA TABLA TP_LOCALIDADES_IMPRESORAS
                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numero_pago = numero_pago + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //HASTA AQUI ES EL NUEVO CAMBIO
                //====================================================================================================================


                ////EXTRAER EL ID_PAGO DE LA TABLA CV403_DCTOS_POR_COBRAR
                //sSql = "";
                //sSql += "select id_pago" + Environment.NewLine;
                //sSql += "from cv403_documentos_pagados" + Environment.NewLine;
                //sSql += "where id_documento_cobrar = " + iIdDocumentoCobrar + Environment.NewLine;
                //sSql += "and estado = 'A'";

                //dtConsulta = new DataTable();
                //dtConsulta.Clear();

                //bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                //if (bRespuesta == true)
                //{
                //    iIdPago = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                //}

                //else
                //{
                //    return false;
                //}

                ////INSERTAMOS EN LA TABLA CV403_DOCUMENTOS_PAGOS 
                //sSql = "";
                //sSql += "insert into cv403_documentos_pagos (" + Environment.NewLine;
                //sSql += "id_pago, cg_tipo_documento, numero_documento, fecha_vcto, " + Environment.NewLine;
                //sSql += "cg_moneda, cotizacion, valor, estado," + Environment.NewLine;
                //sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, valor_recibido)" + Environment.NewLine;
                //sSql += "values(" + Environment.NewLine;
                //sSql += iIdPago + ", " + iCgTipoDocumento + ", 9999, '" + sFecha + "', " + Environment.NewLine;
                //sSql += Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + ", 1, " + dbPago_P + ", 'A', GETDATE()," + Environment.NewLine;
                //sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', " + dbPago_P + ")";

                ////EJECUCION DE INSTRUCCION SQL
                //if (!conexionM.ejecutarInstruccionSQL(sSql))
                //{
                //    return false;
                //}

                ////OBTENEMOS EL MAX ID DE LA TABLA CV403_DOCUMENTOS_PAGOS
                //dtConsulta = new DataTable();
                //dtConsulta.Clear();

                //sTabla = "cv403_documentos_pagos";
                //sCampo = "id_documento_pago";

                //iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                //if (iMaximo == -1)
                //{
                //    return false;
                //}

                //else
                //{
                //    iIdDocumentoPago = Convert.ToInt32(iMaximo);
                //}

                ////INSERTAMOS EL ÚNICO DOCUMENTO PAGADO
                //sSql = "";
                //sSql += "insert into cv403_documentos_pagados (" + Environment.NewLine;
                //sSql += "id_documento_cobrar, id_pago, valor," + Environment.NewLine;
                //sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso, fecha_pago, id_ctt_jornada)" + Environment.NewLine;
                //sSql += "values (" + Environment.NewLine;
                //sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dbPago_P + ", 'A'," + Environment.NewLine;
                //sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', '" + DateTime.Now.ToString("yyyy/MM/dd") + "'," + Environment.NewLine;
                //sSql += Convert.ToInt32(HttpContext.Current.Session["idJornada"].ToString()) + ")";

                ////EJECUCION DE INSTRUCCION SQL
                //if (!conexionM.ejecutarInstruccionSQL(sSql))
                //{
                //    return false;
                //}

                //INSTRUCCION PARA ACTUALIZAR EL ESTADO DEL DOCUMENTO POR COBRAR
                sSql = "";
                sSql += "update cv403_dctos_por_cobrar set" + Environment.NewLine;
                sSql += "cg_estado_dcto = 7461," + Environment.NewLine;
                sSql += "valor = " + dbPagoAdministracion + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    conexionM.reversaTransaccion();
                    return false;
                }

                //INSTRUCCION PARA ACTUALIZAR LA FECHA DE PAGO PENDIENTE 
                sSql = "";
                sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                sSql += "ctt_fecha_pago_pendiente = '" + sFecha + "'" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
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
    }
}