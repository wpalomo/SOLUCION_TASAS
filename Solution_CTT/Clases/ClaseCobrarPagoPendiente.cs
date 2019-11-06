using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NEGOCIO;
using System.Data;

namespace Solution_CTT.Clases
{
    public class ClaseCobrarPagoPendiente
    {
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sFecha;
        string sTabla;
        string sCampo;

        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;

        bool bRespuesta;

        long iMaximo;

        int iIdDocumentoCobrar;
        int iIdPago;
        int iNumeroPago;
        int iCgTipoDocumento = 7456;
        int iCgEstadoDctoPorCobrar = 7461;

        public bool insertarPagoPendiente(int iIdPedido_P, int iIdPersona_P, decimal dbValor_P, decimal dbValorReal_P, string sObservacion_P, string[] sDatosMaximo_P)
        {
            try
            {
                this.sDatosMaximo = sDatosMaximo_P;

                sFecha = DateTime.Now.ToString("yyyy-MM-dd");

                if (conexionM.iniciarTransaccion() == false)
                {
                    return false;
                }

                sSql = "";
                sSql += "select id_documento_cobrar" + Environment.NewLine;
                sSql += "from cv403_dctos_por_cobrar" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count <= 0)
                    {
                        return false;
                    }

                    iIdDocumentoCobrar = Convert.ToInt32(dtConsulta.Rows[0]["id_documento_cobrar"].ToString());
                }
                
                sSql = "";
                sSql += "insert into cv403_pagos (" + Environment.NewLine;
                sSql += "idempresa, id_persona, fecha_pago, cg_moneda, valor," + Environment.NewLine;
                sSql += "propina, cg_empresa, id_localidad, cg_cajero, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso, estado, cambio)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Application["idEmpresa"].ToString()) + ", " + Environment.NewLine;
                sSql += iIdPersona_P + ", '" + sFecha + "', " + Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + "," + Environment.NewLine;
                sSql += dbValor_P + Environment.NewLine + ", 0, " + Convert.ToInt32(HttpContext.Current.Application["cgEmpresa"].ToString()) + "," + Environment.NewLine;
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
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numero_pago = numero_pago + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());

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
                sSql += Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + ", 1, " + dbValor_P + ", 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', " + dbValor_P + ")";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                sSql = "";
                sSql += "insert into cv403_documentos_pagados (" + Environment.NewLine;
                sSql += "id_documento_cobrar, id_pago, valor, estado," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, fecha_pago," + Environment.NewLine;
                sSql += "id_ctt_jornada, id_ctt_cierre_caja)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dbValor_P + ", 'A'," + Environment.NewLine;
                sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', '" + DateTime.Now.ToString("yyyy/MM/dd") + "'," + Environment.NewLine;
                sSql += Convert.ToInt32(HttpContext.Current.Session["idJornada"].ToString()) + ", " + HttpContext.Current.Session["idCierreCaja"].ToString() + ")";

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                sSql = "";
                sSql += "update cv403_dctos_por_cobrar set" + Environment.NewLine;
                sSql += "valor = " + dbValorReal_P + "," + Environment.NewLine;
                sSql += "cg_estado_dcto = " + iCgEstadoDctoPorCobrar + Environment.NewLine;
                sSql += "where id_documento_cobrar = " + iIdDocumentoCobrar + Environment.NewLine;

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                sSql = "";
                sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                sSql += "ctt_fecha_pago_pendiente = '" + sFecha + "'," + Environment.NewLine;
                sSql += "comentarios = '" + sObservacion_P + "'," + Environment.NewLine;
                sSql += "pago_cumplido = 0" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P;

                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                conexionM.terminaTransaccion();

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }


    }
}