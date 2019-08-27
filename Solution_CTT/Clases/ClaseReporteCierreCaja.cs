using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using NEGOCIO;

namespace Solution_CTT.Clases
{
    public class ClaseReporteCierreCaja
    {
        manejadorConexion conexionM = new manejadorConexion();

        string sTexto;
        string sTextoAyuda;
        string sSql;

        //DataTable dtFrecuencia;
        DataTable dtConsulta;

        bool bRespuesta;

        int iCuenta;
        int iCuentaPasajes;
        double dbSumaFrecuencia;
        double dbSumaTotal;

        public string reporteCierreCaja(int iIdOficinista, string sUsuario, string sFecha, string sFechaApertura, string sFechaCierre, DataTable dtFrecuencias)
        {
            try
            {
                sTexto = "";
                sTextoAyuda = "";
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "INFORME DE CAJA".PadLeft(28, ' ') + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "Oficinista: " + sUsuario.ToUpper().Trim() + Environment.NewLine;
                sTexto += "Fecha-Hora Apertura: " + sFechaApertura + Environment.NewLine;
                //sTexto += "Fecha-Hora Cierre:".PadRight(21, ' ') + sFechaApertura + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;

                sTextoAyuda += "".PadLeft(40, '=') + Environment.NewLine;
                dbSumaTotal = 0;
                iCuentaPasajes = 0;

                for (int i = 0; i < dtFrecuencias.Rows.Count; i++)
                {
                    sSql = "";
                    sSql += "select ruta, fecha_viaje, hora_salida," + Environment.NewLine;
                    sSql += "ltrim(str(sum(cantidad * (precio_unitario - valor_dscto + valor_iva)), 10,2)) valor," + Environment.NewLine;
                    sSql += "descripcion, tipo_cliente, count(*) cuenta" + Environment.NewLine;
                    sSql += "from ctt_vw_cierre_caja" + Environment.NewLine;
                    sSql += "where fecha_viaje = '" + sFecha + "'" + Environment.NewLine;
                    sSql += "and id_ctt_oficinista = " + iIdOficinista + Environment.NewLine;
                    sSql += "and id_ctt_programacion = " + Convert.ToInt32(dtFrecuencias.Rows[i][0].ToString()) + Environment.NewLine;
                    sSql += "group by ruta, fecha_viaje, hora_salida, descripcion, tipo_cliente";

                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                    if (bRespuesta == true)
                    {
                        if (dtConsulta.Rows.Count > 0)
                        {
                            sTextoAyuda += Convert.ToDateTime(dtConsulta.Rows[0][2].ToString()).ToString("HH:mm").PadRight(10, ' ') + dtConsulta.Rows[0][0].ToString() + Environment.NewLine;
                            sTextoAyuda += "".PadLeft(40, '-') + Environment.NewLine;

                            iCuenta = 0;
                            dbSumaFrecuencia = 0;

                            for (int j = 0; j < dtConsulta.Rows.Count; j++)
                            {
                                sTextoAyuda += dtConsulta.Rows[j][5].ToString().Trim().PadRight(20, ' ') + dtConsulta.Rows[j][6].ToString().Trim().PadLeft(4, ' ') + dtConsulta.Rows[j][3].ToString().Trim().PadLeft(16, ' ') + Environment.NewLine;
                                iCuenta = iCuenta + Convert.ToInt32(dtConsulta.Rows[j][6].ToString());
                                dbSumaFrecuencia = dbSumaFrecuencia + Convert.ToDouble(dtConsulta.Rows[j][3].ToString());
                            }

                            sTextoAyuda += "".PadRight(17, ' ') + "".PadRight(23, '-') + Environment.NewLine;
                            sTextoAyuda += "TOTALES: ".PadRight(20, ' ') + iCuenta.ToString().PadLeft(4, ' ') + dbSumaFrecuencia.ToString("N2").PadLeft(16, ' ') + Environment.NewLine;
                            iCuentaPasajes = iCuentaPasajes + iCuenta;
                            dbSumaTotal = dbSumaTotal + dbSumaFrecuencia;

                            sTextoAyuda += "".PadLeft(40, '=') + Environment.NewLine;
                        }
                    }

                    else
                    {
                        sTextoAyuda = "ERROR AL DETALLAR LAS VENTAS";
                        sTextoAyuda += "".PadLeft(40, '=') + Environment.NewLine;
                    }
                }

                sTexto += "TOTALES" + Environment.NewLine;
                sTexto += "Cantidad Boletos Vendidos: " + iCuentaPasajes.ToString() + Environment.NewLine;
                sTexto += "FRECUENCIAS" + Environment.NewLine;
                sTexto += sTextoAyuda;
                sTexto += "TOTAL COBRADO BOLETOS: ".PadRight(30, ' ') + dbSumaTotal.ToString("N2").PadLeft(10, ' ') + Environment.NewLine;

                //SECCION PARA COMPLETAR CON LOS COBROS DE ADMINISTRACION
                if (completarCobrosTickets(iIdOficinista, sFecha) == false)
                {
                    return "ERROR EN LA IMPRESIÓN";
                }

                else
                {
                    sTexto += sTextoAyuda;
                }

                //FIN DE SECCION 
                sTexto += "TOTAL COBRADO EN LA JORNADA:".PadRight(30, ' ') + dbSumaTotal.ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                sTexto += "".PadRight(10, ' ') + "".PadRight(20, '-') + Environment.NewLine;
                sTexto += "FIRMA OFICINISTA".PadLeft(28, ' ') + Environment.NewLine;                

                return sTexto;
            }

            catch(Exception)
            {
                return "ERROR EN LA IMPRESIÓN";
            }
        }

        //FUNCION  PARA LLENAR LOS COBROS DE TICKETS
        private bool completarCobrosTickets(int iIdOficinista_P,  string sFecha_P)
        {
            try
            {
                sSql = "";
                sSql += "select P.id_producto, NP.nombre," + Environment.NewLine;
                sSql += "ltrim(str(sum(DP.cantidad * (DP.precio_unitario - DP.valor_dscto + DP.valor_iva)), 10, 2)) valor" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP, cv403_det_pedidos DP," + Environment.NewLine;
                sSql += "cv401_productos P, cv401_nombre_productos NP" + Environment.NewLine;
                sSql += "where DP.id_pedido = CP.id_pedido" + Environment.NewLine;
                sSql += "and DP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and NP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and DP.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.estado = 'A'" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.ctt_fecha_pago_pendiente = '" + sFecha_P + "'" + Environment.NewLine;
                sSql += "and CP.id_ctt_oficinista = " + iIdOficinista_P + Environment.NewLine;
                sSql += "group by P.id_producto, NP.nombre";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sTextoAyuda = "";
                        
                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            sTextoAyuda += (dtConsulta.Rows[i][1].ToString().Trim().ToUpper() + ":").PadRight(30, ' ') + dtConsulta.Rows[i][2].ToString().PadLeft(10, ' ') + Environment.NewLine;
                            dbSumaTotal = dbSumaTotal + Convert.ToDouble(dtConsulta.Rows[i][2].ToString());
                        }
                    }

                    return true;
                }

                else
                {
                    return false;
                }
            }

            catch(Exception ex)
            {
                return false;
            }
        }

    }
}