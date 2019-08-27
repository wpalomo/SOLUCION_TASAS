using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NEGOCIO;
using System.Data;

namespace Solution_CTT.Clases
{
    public class ClaseCierreItinerario
    {
        manejadorConexion conexionM = new manejadorConexion();
        Clases.ClaseManejoCaracteres caracter = new ClaseManejoCaracteres();

        DataTable dtConsulta;
        DataTable dtTerminales;

        string sSql;
        string sTexto;
        string sNombrePasajero;
        string sTerminal;
        string sTextoAyuda;

        int iIdProgramacion;
        int iPasajerosLocal;
        int iPasajerosOtros;
        int iCuenta;

        bool bRespuesta;

        double dbPrecioUnitarioLocal;
        double dbDescuentoLocal;
        double dbIvaLocal;
        double dbTotalLocal;

        double dbPrecioUnitarioOtros;
        double dbDescuentoOtros;
        double dbIvaOtros;
        double dbTotalOtros;
        double dbTotalOtrasLocalidades;

        double dbRetencionLocal;
        double dbRetencionOtros;

        double dbValorAdministracion;
        double dbValorRetencion;
        double dbPorcentajeRetencion;

        double dbSumaCobros;
        double dbSumaRetenciones;


        //FUNCION PARA EXTRAER LOS DATOS DE LOS TERMINALES
        private bool terminalesRegistrados()
        {
            try
            {
                sSql = "";
                sSql += "select LOC.id_localidad, P.descripcion" + Environment.NewLine;
                sSql += "from tp_localidades LOC, ctt_pueblos P" + Environment.NewLine;
                sSql += "where P.id_localidad_terminal = LOC.id_localidad" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and LOC.estado = 'A'" + Environment.NewLine;
                sSql += "and P.terminal = 1" + Environment.NewLine;
                sSql += "and LOC.id_localidad <> " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());

                dtTerminales = new DataTable();
                dtTerminales.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtTerminales);

                if (bRespuesta == true)
                {
                    return true;
                }
                
                else
                {
                    return false;
                }
            }

            catch
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

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            if (dtConsulta.Rows[i][3].ToString() == "1")
                            {
                                dbPorcentajeRetencion = Convert.ToDouble(dtConsulta.Rows[i][4].ToString());
                                dbValorRetencion = Convert.ToDouble(dtConsulta.Rows[i][6].ToString());
                            }

                            else if (dtConsulta.Rows[i][10].ToString() == "1")
                            {
                                dbValorAdministracion = Convert.ToDouble(dtConsulta.Rows[i][6].ToString());
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

            catch(Exception)
            {
                return false;
            }
        }

        public string llenarReporte(int iIdProgramacion_P, string sUsuario)
        {
            try
            {
                this.iIdProgramacion = iIdProgramacion_P;

                if (consultarValoresCierre() == false)
                {
                    return "ERROR";
                }

                sTexto = "";

                sSql = "";
                sSql += "select ltrim(isnull(TP.nombres, '') + ' ' + TP.apellidos) conductor," + Environment.NewLine;
                sSql += "P.fecha_viaje, P.numero_viaje, H.hora_salida," + Environment.NewLine;
                sSql += "D.descripcion + ' - ' + V.placa disco_placa" + Environment.NewLine;
                sSql += "from ctt_programacion P, ctt_vehiculo V, ctt_vehiculo_propietario VP," + Environment.NewLine;
                sSql += "tp_personas TP, ctt_horarios H, ctt_disco D" + Environment.NewLine;
                sSql += "where P.id_ctt_vehiculo = V.id_ctt_vehiculo" + Environment.NewLine;
                sSql += "and VP.id_ctt_vehiculo = V.id_ctt_vehiculo" + Environment.NewLine;
                sSql += "and VP.id_persona = TP.id_persona" + Environment.NewLine;
                sSql += "and V.id_ctt_disco = D.id_ctt_disco" + Environment.NewLine;
                sSql += "and P.id_ctt_horario = H.id_ctt_horario" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and V.estado = 'A'" + Environment.NewLine;
                sSql += "and VP.estado = 'A'" + Environment.NewLine;
                sSql += "and TP.estado = 'A'" + Environment.NewLine;
                sSql += "and H.estado = 'A'" + Environment.NewLine;
                sSql += "and D.estado = 'A'" + Environment.NewLine;
                sSql += "and P.id_ctt_programacion = " + iIdProgramacion_P;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    return "ERROR";
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    return "ERROR";
                }

                //CREACION DEL REPORTE
                sTexto += "".PadLeft(40, '=') + Environment.NewLine;
                sTexto += "COMPAÑÍA DE TRANSPORTES".PadLeft(33, ' ') + Environment.NewLine;
                sTexto += "\"EXPRESS ATENAS S.A.\"".PadLeft(32, ' ') + Environment.NewLine;
                sTexto += "Sirviendo a la Provincia de Bolívar".PadLeft(38, ' ') + Environment.NewLine + Environment.NewLine;
                sTexto += "REPORTE DE PASAJEROS".PadLeft(31, ' ') + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "FECHA DE CONSULTA   : " + DateTime.Now.ToString("dd/MM/yyyy") + Environment.NewLine;
                sTexto += "USUARIO DE CONSULTA : " + sUsuario.Trim().ToUpper() + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "CRITERIOS DE CONSULTA" + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "No. VIAJE   : " + dtConsulta.Rows[0][2].ToString() + Environment.NewLine;
                sTexto += "CONDUCTOR   : ";

                if (dtConsulta.Rows[0][0].ToString().Trim().Length > 26)
                {
                    sTexto += dtConsulta.Rows[0][0].ToString().Trim().ToUpper().Substring(0, 26) + Environment.NewLine;
                }

                else
                {
                    sTexto += dtConsulta.Rows[0][0].ToString().Trim().ToUpper() + Environment.NewLine;
                }

                sTexto += "FECHA SALIDA: " + Convert.ToDateTime(dtConsulta.Rows[0][1].ToString()).ToString("dd/MM/yyyy") + Environment.NewLine;
                sTexto += "HORA SALIDA : " + Convert.ToDateTime(dtConsulta.Rows[0][3].ToString()).ToString("HH:mm") + Environment.NewLine;
                sTexto += "DISCO PLACA : " + dtConsulta.Rows[0][4].ToString() + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "DETALLE DE PASAJEROS" + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;

                sSql = "";
                sSql += "select * from ctt_vw_reporte_cierre_viaje" + Environment.NewLine;
                sSql += "where id_ctt_programacion = " + iIdProgramacion_P + Environment.NewLine;
                sSql += "order by numero_asiento";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    return "ERROR";
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    return "ERROR";
                }

                iPasajerosLocal = 0;
                iPasajerosOtros = 0;
                dbTotalLocal = 0;
                dbTotalOtros = 0;


                //FUNCION PARA RECORRER EL DATATABLE VERIFICANDO LAS VENTAS DESDE OTRAS LOCALIDAD
                if (terminalesRegistrados() == false)
                {
                    return "ERROR";
                }

                else
                {
                    sTextoAyuda = "";
                    dbTotalOtrasLocalidades = 0;
                    iPasajerosOtros = 0;

                    for (int i = 0; i < dtTerminales.Rows.Count; i++)
                    {
                        sTerminal = dtTerminales.Rows[i][1].ToString().Trim();
                        dbTotalOtros = 0;
                        iCuenta = 0;

                        for (int j = 0; j < dtConsulta.Rows.Count; j++)
                        {
                            if (Convert.ToInt32(dtConsulta.Rows[j][9].ToString()) == Convert.ToInt32(dtTerminales.Rows[i][0].ToString()))
                            {
                                dbPrecioUnitarioOtros = Convert.ToDouble(dtConsulta.Rows[j][4].ToString());
                                dbDescuentoOtros = Convert.ToDouble(dtConsulta.Rows[j][5].ToString());
                                dbIvaOtros = Convert.ToDouble(dtConsulta.Rows[j][10].ToString());
                                dbTotalOtros = dbTotalOtros + (dbPrecioUnitarioOtros - dbDescuentoOtros);
                                iCuenta++;
                                iPasajerosOtros++;
                            }
                        }

                        if (iCuenta > 0)
                        {
                            dbTotalOtrasLocalidades = dbTotalOtrasLocalidades + dbTotalOtros;
                            sTextoAyuda += "  " + sTerminal.PadRight(20, ' ') + ":" + dbTotalOtros.ToString("N2").PadLeft(12, ' ') + Environment.NewLine;
                        }
                    }
                }


                //FUN DE LA FUNCION


                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    sTexto += "__ ASIENTO: " + dtConsulta.Rows[i][3].ToString().PadRight(7, ' ') + "CI/RUC: " + dtConsulta.Rows[i][0].ToString() + Environment.NewLine;

                    if (dtConsulta.Rows[i][1].ToString().Trim().Length > 40)
                    {
                        sTexto += dtConsulta.Rows[i][1].ToString().Trim().ToUpper().Substring(0, 40) + Environment.NewLine + Environment.NewLine;
                    }

                    else
                    {
                        sTexto += dtConsulta.Rows[i][1].ToString().Trim().ToUpper() + Environment.NewLine + Environment.NewLine;
                    }

                    if (Convert.ToInt32(dtConsulta.Rows[i][9].ToString()) == Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString()))
                    {
                        dbPrecioUnitarioLocal = Convert.ToDouble(dtConsulta.Rows[i][4].ToString());
                        dbDescuentoLocal = Convert.ToDouble(dtConsulta.Rows[i][5].ToString());
                        dbIvaLocal = Convert.ToDouble(dtConsulta.Rows[i][10].ToString());
                        dbTotalLocal = dbTotalLocal + (dbPrecioUnitarioLocal - dbDescuentoLocal);

                        iPasajerosLocal++;
                    }

                    //else
                    //{
                    //    dbPrecioUnitarioOtros = Convert.ToDouble(dtConsulta.Rows[i][4].ToString());
                    //    dbDescuentoOtros = Convert.ToDouble(dtConsulta.Rows[i][5].ToString());
                    //    dbIvaOtros = Convert.ToDouble(dtConsulta.Rows[i][10].ToString());
                    //    dbTotalOtros = dbTotalOtros + (dbPrecioUnitarioOtros - dbDescuentoOtros);

                    //    iPasajerosOtros++;
                    //}
                }

                dbRetencionLocal = dbTotalLocal * (dbPorcentajeRetencion / 100);
                dbRetencionOtros = dbTotalOtros * (dbPorcentajeRetencion / 100);
                dbSumaCobros = dbTotalLocal + dbTotalOtrasLocalidades;

                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "TOTAL DE PASAJEROS VENTA LOCAL: " + iPasajerosLocal.ToString() + Environment.NewLine;
                sTexto += "TOTAL DE PASAJEROS VENTA OTROS: " + iPasajerosOtros.ToString() + Environment.NewLine;
                sTexto += "TOTAL DE PASAJEROS A VIAJAR   : " + (iPasajerosLocal + iPasajerosOtros).ToString() + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine + Environment.NewLine;

                sTexto += "  TOTAL COBRADO LOCAL :" + dbTotalLocal.ToString("N2").PadLeft(12, ' ') + Environment.NewLine;
                sTexto += "  OTRAS LOCALIDADES___" + Environment.NewLine;

                if (sTextoAyuda.Trim().Length == 0)
                {
                    sTexto += "SIN REGISTROS" + Environment.NewLine;
                }

                else
                {
                    sTexto += sTextoAyuda;
                }

                sTexto += "  TOTAL COBRADO OTROS :" + dbTotalOtrasLocalidades.ToString("N2").PadLeft(12, ' ') + Environment.NewLine;
                //sTexto += "- DESCUENTOS    :" + dbDescuento.ToString("N2").PadLeft(12, ' ') + Environment.NewLine;
                sTexto += "  SUMA TOTAL          :" + dbSumaCobros.ToString("N2").PadLeft(12, ' ') + Environment.NewLine + Environment.NewLine;
                sTexto += "  " + dbPorcentajeRetencion.ToString("N0") + "% RETENCIÓN LOCAL  :" + dbRetencionLocal.ToString("N2").PadLeft(12, ' ') + Environment.NewLine;
                sTexto += "  " + dbPorcentajeRetencion.ToString("N0") + "% RETENCIÓN OTROS  :" + dbRetencionOtros.ToString("N2").PadLeft(12, ' ') + Environment.NewLine;
                dbSumaRetenciones = dbRetencionLocal + dbRetencionOtros;
                sTexto += "  SUMA RETENCIONES    :" + dbSumaRetenciones.ToString("N2").PadLeft(12, ' ') + Environment.NewLine + Environment.NewLine;

                sTexto += "  SUMA TOTAL          :" + dbSumaCobros.ToString("N2").PadLeft(12, ' ') + Environment.NewLine;
                sTexto += "- SUMA RETENCIONES    :" + dbSumaRetenciones.ToString("N2").PadLeft(12, ' ') + Environment.NewLine;
                dbSumaCobros = dbSumaCobros - dbSumaRetenciones;
                sTexto += "  TOTAL               :" + dbSumaCobros.ToString("N2").PadLeft(12, ' ') + Environment.NewLine + Environment.NewLine;
                sTexto += "- ADMINISTRACIÓN      :" + dbValorAdministracion.ToString("N2").PadLeft(12, ' ') + Environment.NewLine + Environment.NewLine;
                dbSumaCobros = dbSumaCobros - dbValorAdministracion;
                sTexto += "  TOTAL ENTREGAR      :" + dbSumaCobros.ToString("N2").PadLeft(12, ' ') + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                sTexto += " _________________    __________________" + Environment.NewLine;
                sTexto += "     OFICINISTA           CONDUCTOR     " + Environment.NewLine + Environment.NewLine;
                sTexto += "Derechos Reservados - APLICSIS".PadLeft(35, ' ') + Environment.NewLine + Environment.NewLine;
                sTexto += ".";

                return sTexto;
            }

            catch (Exception)
            {
                return "ERROR";
            }
        }
    }
}