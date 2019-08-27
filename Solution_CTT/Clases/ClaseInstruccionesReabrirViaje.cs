using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using NEGOCIO;

namespace Solution_CTT.Clases
{
    public class ClaseInstruccionesReabrirViaje
    {

        manejadorConexion conexionM = new manejadorConexion();

        string sSql;                
        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;

        bool bRespuesta;

        int iIdProgramacion;
        int iIdPedido;
        int iIdPago;
        int iIdFactura;
        int iIdDocumentoCobrar;
        int iIdDespacho;
        int iIdEventoCobro;

        public bool iniciarCierre(int iIdProgramacion_P, string[] sDatosMaximo_P)
        {
            try
            {
                this.iIdProgramacion = iIdProgramacion_P;
                this.sDatosMaximo = sDatosMaximo_P;

                //INICIA TRANSACCION SQL
                if (conexionM.iniciarTransaccion() == false)
                {
                    return false;
                }

                //INSTRUCCION SQL PARA EXTRAER EL ID DEL PEDIDO DE LA RETENCION
                sSql = "";
                sSql += "select id_pedido" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + iIdProgramacion + Environment.NewLine;
                sSql += "and cobro_retencion = 1" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    conexionM.reversaTransaccion();
                    return false;
                }

                // AQUI SE PROCEDE A ENVIAR LOS PARAMETROS PARA ELIMINAR LA RETENCION
                if (dtConsulta.Rows.Count > 0)
                {
                    iIdPedido = Convert.ToInt32(dtConsulta.Rows[0]["id_pedido"].ToString());

                    if (obtenerIdentificadores(iIdPedido) == false)
                    {
                        conexionM.reversaTransaccion();
                        return false;
                    }

                    if (eliminarFactura() == false)
                    {
                        conexionM.reversaTransaccion();
                        return false;
                    }

                    if (eliminarPagos() == false)
                    {
                        conexionM.reversaTransaccion();
                        return false;
                    }

                    if (eliminarPedido() == false)
                    {
                        conexionM.reversaTransaccion();
                        return false;
                    }
                }

                //INSTRUCCION SQL PARA EXTRAER EL ID DEL PEDIDO DE LA ADMINISTRACION
                iIdPedido = 0;
                iIdPago = 0;
                iIdFactura = 0;
                iIdDocumentoCobrar = 0;
                iIdDespacho = 0;
                iIdEventoCobro = 0;

                sSql = "";
                sSql += "select id_pedido" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + iIdProgramacion + Environment.NewLine;
                sSql += "and cobro_administrativo = 1" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    conexionM.reversaTransaccion();
                    return false;
                }

                // AQUI SE PROCEDE A ENVIAR LOS PARAMETROS PARA ELIMINAR LOS PAGOS DE ADMINISTRACION
                if (dtConsulta.Rows.Count > 0)
                {
                    iIdPedido = Convert.ToInt32(dtConsulta.Rows[0]["id_pedido"].ToString());

                    if (obtenerIdentificadores(iIdPedido) == false)
                    {
                        conexionM.reversaTransaccion();
                        return false;
                    }

                    if (eliminarFactura() == false)
                    {
                        conexionM.reversaTransaccion();
                        return false;
                    }

                    if (eliminarPagos() == false)
                    {
                        conexionM.reversaTransaccion();
                        return false;
                    }

                    if (eliminarPedido() == false)
                    {
                        conexionM.reversaTransaccion();
                        return false;
                    }
                }

                sSql = "";
                sSql += "update ctt_programacion set" + Environment.NewLine;
                sSql += "estado_salida = 'Abierta'" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + iIdProgramacion + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
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

        //FASE OBTENER LOS IDENTIFICADORES PARA SOLO REALIZAR LAS ELIMINACIONES
        private bool obtenerIdentificadores(int iIdPedido_P)
        {
            try
            {
                //OBTENER ID DEL DESPACHO
                sSql = "";
                sSql += "select id_despacho" + Environment.NewLine;
                sSql += "from cv403_despachos_pedidos" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    return false;
                }

                iIdDespacho = Convert.ToInt32(dtConsulta.Rows[0]["id_despacho"].ToString());

                //OBTENER ID DE LA FACTURA
                sSql = "";
                sSql += "select id_factura" + Environment.NewLine;
                sSql += "from cv403_facturas_pedidos" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    return false;
                }

                iIdFactura = Convert.ToInt32(dtConsulta.Rows[0]["id_factura"].ToString());

                //OBTENER EL ID DEL DOCUMENTO POR COBRAR Y EL EVENTO COBRO
                sSql = "";
                sSql += "select id_documento_cobrar, id_evento_cobro" + Environment.NewLine;
                sSql += "from cv403_dctos_por_cobrar" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    return false;
                }

                iIdDocumentoCobrar = Convert.ToInt32(dtConsulta.Rows[0]["id_documento_cobrar"].ToString());
                iIdEventoCobro = Convert.ToInt32(dtConsulta.Rows[0]["id_evento_cobro"].ToString());

                //OBTENER EL ID DEL PAGO
                sSql = "";
                sSql += "select id_pago" + Environment.NewLine;
                sSql += "from cv403_documentos_pagados" + Environment.NewLine;
                sSql += "where id_documento_cobrar = " + iIdDocumentoCobrar + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    return false;
                }

                iIdPago = Convert.ToInt32(dtConsulta.Rows[0]["id_pago"].ToString());

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        //PRIMERA FASE - ELIMINAR LOS REGISTROS DE LA FACTURA
        private bool eliminarFactura()
        {
            try
            {
                //ELIMINAR EN LA TABLA CV403_NUMEROS_FACTURAS
                sSql = "";
                sSql += "update cv403_numeros_facturas set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_factura = " + iIdFactura + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    return false;
                }

                //ELIMINAR EN LA TABLA CV403_FACTURAS
                sSql = "";
                sSql += "update cv403_facturas set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_factura = " + iIdFactura + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    return false;
                }

                //ELIMINAR EN LA TABLA CV403_FACTURAS_FACTURAS
                sSql = "";
                sSql += "update cv403_facturas_pedidos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_factura = " + iIdFactura + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
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

        //SEGUNDA FASE - ELIMINAR LOS REGISTROS DE PAGOS
        private bool eliminarPagos()
        {
            try
            {
                //ELIMINAR EN LA TABLA CV403_NUMEROS_PAGOS
                sSql = "";
                sSql += "update cv403_numeros_pagos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pago = " + iIdPago + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    return false;
                }

                //ELIMINAR EN LA TABLA CV403_DOCUMENTOS_PAGOS
                sSql = "";
                sSql += "update cv403_documentos_pagos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pago = " + iIdPago + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    return false;
                }

                //ELIMINAR EN LA TABLA CV403_DOCUMENTOS_PAGADOS
                sSql = "";
                sSql += "update cv403_documentos_pagados set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pago = " + iIdPago + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    return false;
                }

                //ELIMINAR EN LA TABLA CV403_PAGOS
                sSql = "";
                sSql += "update cv403_pagos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pago = " + iIdPago + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
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


        //TERCERA FASE - ELIMINAR LOS REGISTROS DEL PEDIDO
        private bool eliminarPedido()
        {
            try
            {
                //ELIMINAR EN LA TABLA CV403_DET_PEDIDOS
                sSql = "";
                sSql += "update cv403_det_pedidos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    return false;
                }

                //ELIMINAR EN LA TABLA CV403_DCTOS_POR_COBRAR
                sSql = "";
                sSql += "update cv403_dctos_por_cobrar set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    return false;
                }

                //ELIMINAR EN LA TABLA CV403_EVENTOS_COBROS
                sSql = "";
                sSql += "update cv403_eventos_cobros set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_evento_cobro = " + iIdEventoCobro + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    return false;
                }

                //ELIMINAR EN LA TABLA CV403_DESPACHOS_PEDIDOS
                sSql = "";
                sSql += "update cv403_despachos_pedidos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    return false;
                }

                //ELIMINAR EN LA TABLA CV403_CAB_DESPACHOS
                sSql = "";
                sSql += "update cv403_cab_despachos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_despacho = " + iIdDespacho + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    return false;
                }

                //ELIMINAR EN LA TABLA CV403_NUMERO_CAB_PEDIDO
                sSql = "";
                sSql += "update cv403_numero_cab_pedido set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    return false;
                }

                //ELIMINAR EN LA TABLA CV403_CAB_PEDIDOS
                sSql = "";
                sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
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