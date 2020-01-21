using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NEGOCIO;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace Solution_CTT.Clases
{
    public class ClaseImprimirManifiesto
    {
        manejadorConexion conexionM = new manejadorConexion();

        DataTable dtConsulta;
        DataTable dtConsulta2;

        string sSql;
        string sConductor;        
        string sNumeroViaje;
        string sDiscoPlaca;
        string sTipoViaje;
        string sIdentificacionConductor;

        int iIdProgramacion;
        int iCantidadLocal;
        int iCantidadOtros;

        decimal dbTotalLocal;
        decimal dbTotalOtros;
        decimal dbRetencionLocal;
        decimal dbRetencionOtros;
        decimal dbPagoAdministracion;
        decimal dbPorcentajeRetencion;
        decimal dbValorAdministracion;
        decimal dbValorRetencion;

        DateTime dtFechaViaje;
        DateTime dtHoraViaje;

        bool bRespuesta;

        public bool llenarReporte(int iIdProgramacion_P, string sUsuario, int iOp, Decimal dbPagosPendientes_P, Decimal dbIngresoEfectivo_P, int iBandera_Imprimir_P)
        {
            try
            {
                this.iIdProgramacion = iIdProgramacion_P;

                //INSTRUCCION PARA OBTENER LOS PAGOS PENDIENTES
                sSql = "";
                sSql += "select isnull(sum(valor), 0) suma" + Environment.NewLine;
                sSql += "from cv403_documentos_pagados" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and id_ctt_programacion = " + iIdProgramacion;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    return false;
                }

                dbPagosPendientes_P = Convert.ToDecimal(dtConsulta.Rows[0][0].ToString());

                sSql = "";
                sSql += "select CH.descripcion conductor, P.fecha_viaje, P.numero_viaje," + Environment.NewLine;
                sSql += "case when P.hora_reemplazo_extra is null then H.hora_salida else P.hora_reemplazo_extra end hora_salida," + Environment.NewLine;
                sSql += "D.descripcion + ' - ' + V.placa disco_placa, TS.descripcion tipo_viaje, TP.identificacion" + Environment.NewLine;
                sSql += "from ctt_programacion P INNER JOIN" + Environment.NewLine;
                sSql += "ctt_vehiculo V ON V.id_ctt_vehiculo = P.id_ctt_vehiculo" + Environment.NewLine;
                sSql += "and V.estado = 'A'" + Environment.NewLine;
                sSql += "and P.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_chofer CH ON CH.id_ctt_chofer = P.id_ctt_chofer" + Environment.NewLine;
                sSql += "and CH.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_itinerario I ON I.id_ctt_itinerario = P.id_ctt_itinerario" + Environment.NewLine;
                sSql += "and I.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_horarios H ON H.id_ctt_horario = I.id_ctt_horario" + Environment.NewLine;
                sSql += "and H.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_disco D ON D.id_ctt_disco = V.id_ctt_disco" + Environment.NewLine;
                sSql += "and D.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_tipo_servicio TS ON TS.id_ctt_tipo_servicio = P.id_ctt_tipo_servicio" + Environment.NewLine;
                sSql += "and TS.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "tp_personas TP ON TP.id_persona = CH.id_persona" + Environment.NewLine;
                sSql += "and TP.estado = 'A'" + Environment.NewLine;
                sSql += "where P.id_ctt_programacion = " + iIdProgramacion_P;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    return false;
                }

                sConductor = dtConsulta.Rows[0]["conductor"].ToString();
                dtFechaViaje = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_viaje"].ToString());
                sNumeroViaje = dtConsulta.Rows[0]["numero_viaje"].ToString();
                dtHoraViaje = Convert.ToDateTime(dtConsulta.Rows[0]["hora_salida"].ToString());
                sDiscoPlaca = dtConsulta.Rows[0]["disco_placa"].ToString();
                sTipoViaje = dtConsulta.Rows[0]["tipo_viaje"].ToString();
                sIdentificacionConductor = dtConsulta.Rows[0]["identificacion"].ToString();

                sSql = "";
                sSql += "select identificacion, numero_asiento, descripcion, pasajero," + Environment.NewLine;
                sSql += "ltrim(str(precio_unitario - valor_dscto + valor_iva, 10, 2)) valor, id_localidad" + Environment.NewLine;
                sSql += "from ctt_vw_reporte_cierre_viaje" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + iIdProgramacion_P + Environment.NewLine;
                sSql += "order by numero_asiento";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    return false;
                }

                iCantidadLocal = 0;
                iCantidadOtros = 0;
                dbTotalLocal= 0;
                dbTotalOtros = 0;
                dbRetencionLocal = 0;
                dbRetencionOtros = 0;
                dbPagoAdministracion = 0;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    if (Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString()) == Convert.ToInt32(dtConsulta.Rows[i]["id_localidad"].ToString()))
                    {
                        iCantidadLocal++;
                        dbTotalLocal += Convert.ToDecimal(dtConsulta.Rows[i]["valor"].ToString());
                    }

                    else
                    {
                        iCantidadOtros++;
                        dbTotalOtros += Convert.ToDecimal(dtConsulta.Rows[i]["valor"].ToString());
                    }
                }

                if (consultarValoresCierre() == false)
                {
                    return false;
                }

                dbRetencionLocal = dbTotalLocal * (dbPorcentajeRetencion / 100);
                dbRetencionOtros = dbTotalOtros * (dbPorcentajeRetencion / 100);

                //AGREGANDO COLUMNAS FALTANTES
                DataColumn usuario_consulta = new DataColumn("usuario_consulta");
                usuario_consulta.DataType = System.Type.GetType("System.String");
                dtConsulta.Columns.Add(usuario_consulta);

                DataColumn numero_viaje = new DataColumn("numero_viaje");
                numero_viaje.DataType = System.Type.GetType("System.String");
                dtConsulta.Columns.Add(numero_viaje);

                DataColumn fecha_salida = new DataColumn("fecha_salida");
                fecha_salida.DataType = System.Type.GetType("System.DateTime");
                dtConsulta.Columns.Add(fecha_salida);

                DataColumn hora_salida = new DataColumn("hora_salida");
                hora_salida.DataType = System.Type.GetType("System.DateTime");
                dtConsulta.Columns.Add(hora_salida);

                DataColumn disco_placa = new DataColumn("disco_placa");
                disco_placa.DataType = System.Type.GetType("System.String");
                dtConsulta.Columns.Add(disco_placa);

                DataColumn conductor = new DataColumn("conductor");
                conductor.DataType = System.Type.GetType("System.String");
                dtConsulta.Columns.Add(conductor);

                DataColumn cantidad_local = new DataColumn("cantidad_local");
                cantidad_local.DataType = System.Type.GetType("System.Int32");
                dtConsulta.Columns.Add(cantidad_local);

                DataColumn cantidad_otros = new DataColumn("cantidad_otros");
                cantidad_otros.DataType = System.Type.GetType("System.Int32");
                dtConsulta.Columns.Add(cantidad_otros);

                DataColumn ventas_locales = new DataColumn("ventas_locales");
                ventas_locales.DataType = System.Type.GetType("System.Decimal");
                dtConsulta.Columns.Add(ventas_locales);

                DataColumn ventas_otros = new DataColumn("ventas_otros");
                ventas_otros.DataType = System.Type.GetType("System.Decimal");
                dtConsulta.Columns.Add(ventas_otros);

                DataColumn retencion_local = new DataColumn("retencion_local");
                retencion_local.DataType = System.Type.GetType("System.Decimal");
                dtConsulta.Columns.Add(retencion_local);

                DataColumn retencion_otros = new DataColumn("retencion_otros");
                retencion_otros.DataType = System.Type.GetType("System.Decimal");
                dtConsulta.Columns.Add(retencion_otros);

                DataColumn pago_administracion = new DataColumn("pago_administracion");
                pago_administracion.DataType = System.Type.GetType("System.Decimal");
                dtConsulta.Columns.Add(pago_administracion);

                DataColumn tipo_viaje = new DataColumn("tipo_viaje");
                tipo_viaje.DataType = System.Type.GetType("System.String");
                dtConsulta.Columns.Add(tipo_viaje);

                DataColumn identificacion_conductor = new DataColumn("identificacion_conductor");
                identificacion_conductor.DataType = System.Type.GetType("System.String");
                dtConsulta.Columns.Add(identificacion_conductor);

                if (dtConsulta.Rows.Count == 0)
                {
                    dtConsulta.Rows.Add();
                }

                //LLENAR EL DATATABLE
                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    dtConsulta.Rows[i]["usuario_consulta"] = sUsuario;
                    dtConsulta.Rows[i]["numero_viaje"] = sNumeroViaje;
                    dtConsulta.Rows[i]["fecha_salida"] = dtFechaViaje;
                    dtConsulta.Rows[i]["hora_salida"] = dtHoraViaje;
                    dtConsulta.Rows[i]["disco_placa"] = sDiscoPlaca;
                    dtConsulta.Rows[i]["conductor"] = sConductor;
                    dtConsulta.Rows[i]["cantidad_local"] = iCantidadLocal;
                    dtConsulta.Rows[i]["cantidad_otros"] = iCantidadOtros;
                    dtConsulta.Rows[i]["ventas_locales"] = dbTotalLocal;
                    dtConsulta.Rows[i]["ventas_otros"] = dbTotalOtros;
                    dtConsulta.Rows[i]["retencion_local"] = dbRetencionLocal;
                    dtConsulta.Rows[i]["retencion_otros"] = dbRetencionOtros;
                    dtConsulta.Rows[i]["pago_administracion"] = dbPagoAdministracion;
                    dtConsulta.Rows[i]["tipo_viaje"] = sTipoViaje;
                    dtConsulta.Rows[i]["identificacion_conductor"] = sIdentificacionConductor;

                    iCantidadLocal = 0;
                    iCantidadOtros = 0;
                    dbTotalLocal = 0;
                    dbTotalOtros = 0;
                    dbRetencionLocal = 0;
                    dbRetencionOtros = 0;
                    dbPagoAdministracion = 0;
                }

                DSReportes ds = new DSReportes();

                DataTable dt = ds.Tables["dtManifiestoPasajeros"];
                dt.Clear();

                dt = dtConsulta;

                LocalReport reporteLocal = new LocalReport();

                if (iOp == 1)
                {
                    reporteLocal.ReportPath = HttpContext.Current.Server.MapPath("~/Reportes/rptManifiestoPasajeros.rdlc");
                }

                else
                {
                    reporteLocal.ReportPath = HttpContext.Current.Server.MapPath("~/Reportes/rptListadoPasajeros.rdlc");
                }

                ReportParameter[] parametros = new ReportParameter[3];
                parametros[0] = new ReportParameter("P_Fecha", DateTime.Now.ToString("dd-MMM-yyyy"));
                parametros[1] = new ReportParameter("P_PagosPendientes", dbPagosPendientes_P.ToString("N2"));
                parametros[2] = new ReportParameter("P_IngresoEfectivo", dbIngresoEfectivo_P.ToString("N2"));

                ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
                reporteLocal.DataSources.Add(datasource);
                reporteLocal.SetParameters(parametros);

                Clases.Impresor imp = new Clases.Impresor();

                if (iBandera_Imprimir_P == 0)
                {
                    imp.Imprime(reporteLocal);
                }

                else
                {

                    int iCantidad_P = Convert.ToInt32(HttpContext.Current.Session["cantidad_manifiesto"].ToString());

                    for (int i = 0; i < iCantidad_P; i++)
                    {
                        imp.Imprime(reporteLocal);
                    }
                }                 

                return true;
            }

            catch (Exception ex)
            {



                return false;
            }
        }

        //FUNCION PARA CONSULTAR LOS VALORES DE CIERRE DE CAJA
        private bool consultarValoresCierre()
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_pagos_frecuencia" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + iIdProgramacion + Environment.NewLine;
                sSql += "and idtipocomprobante = " + Convert.ToInt32(HttpContext.Current.Application["id_comprobante"].ToString());

                dtConsulta2 = new DataTable();
                dtConsulta2.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta2);

                if (bRespuesta == true)
                {
                    if (dtConsulta2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtConsulta2.Rows.Count; i++)
                        {
                            if (dtConsulta2.Rows[i]["aplica_retencion_ticket"].ToString() == "1")
                            {
                                dbPorcentajeRetencion = Convert.ToDecimal(dtConsulta2.Rows[i]["porcentaje_retencion_ticket"].ToString());
                                dbValorRetencion = Convert.ToDecimal(dtConsulta2.Rows[i]["valor_cobrado"].ToString());
                            }

                            else if (dtConsulta2.Rows[i]["aplica_pago_administracion"].ToString() == "1")
                            {
                                dbPagoAdministracion = Convert.ToDecimal(dtConsulta2.Rows[i]["valor_cobrado"].ToString());
                            }
                        }

                        return true;
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
            }

            catch (Exception)
            {
                return false;
            }
        }
    }
}