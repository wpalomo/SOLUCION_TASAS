using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using NEGOCIO;

namespace Solution_CTT.Clases
{
    public class ClaseReporteCobroRetenciones
    {
        manejadorConexion connexionM = new manejadorConexion();

        string sTexto;
        string sFecha;
        string sHora;
        string sVehiculo;

        double dbCantidad;
        double dbPrecioUnitario;
        double dbValorDescuento;
        double dbValorIva;
        double dbValor;
        double dbSumaTotal;

        public string llenarBoleto(DataTable dtConsulta, string sFechaDesde_P, string sFechaHasta_P, string sUsuario_P)
        {
            try
            {
                sTexto = "";
                //CREACION DEL REPORTE
                sTexto += "".PadLeft(40, '=') + Environment.NewLine;
                sTexto += "COMPAÑÍA DE TRANSPORTES".PadLeft(33, ' ') + Environment.NewLine;
                sTexto += "\"EXPRESS ATENAS S.A.\"".PadLeft(32, ' ') + Environment.NewLine;
                sTexto += "Sirviendo a la Provincia de Bolívar".PadLeft(38, ' ') + Environment.NewLine + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "REPORTE DE COBRO DE RETENCIONES".PadLeft(35, ' ') + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "FECHA DE CONSULTA   : " + DateTime.Now.ToString("dd/MM/yyyy") + Environment.NewLine;
                sTexto += "USUARIO DE CONSULTA : " + sUsuario_P + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "CRITERIOS DE CONSULTA".PadLeft(30, ' ') + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "FECHA DESDE: " + sFechaDesde_P + Environment.NewLine;
                sTexto += "FECHA HASTA: " + sFechaHasta_P + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "DETALLE DE COBROS DE TICKETS".PadLeft(33, ' ') + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "FECHA - HORA       VEHÍCULO       VALOR" + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;

                dbSumaTotal = 0;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    sFecha = Convert.ToDateTime(dtConsulta.Rows[i][5].ToString()).ToString("dd/MM/yyyy");
                    sHora = Convert.ToDateTime(dtConsulta.Rows[i][6].ToString()).ToString("HH:mm");
                    sVehiculo = dtConsulta.Rows[i][8].ToString();
                    dbCantidad = Convert.ToDouble(dtConsulta.Rows[i][9].ToString());
                    dbPrecioUnitario = Convert.ToDouble(dtConsulta.Rows[i][10].ToString());
                    dbValorDescuento = Convert.ToDouble(dtConsulta.Rows[i][11].ToString());
                    dbValorIva = Convert.ToDouble(dtConsulta.Rows[i][12].ToString());
                    dbValor = dbCantidad * (dbPrecioUnitario - dbValorDescuento + dbValorIva);
                    dbSumaTotal = dbSumaTotal + dbValor;

                    sTexto += (sFecha.Trim() + " " + sHora.Trim()).PadRight(16, ' ') + sVehiculo.PadLeft(14, ' ') + dbValor.ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                }

                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "TOTAL COBRADO:" + dbSumaTotal.ToString("N2").PadLeft(26, ' ') + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                sTexto += "Derechos RESERVADOS - APLICSIS".PadLeft(35, ' ');



                return sTexto;
            }

            catch (Exception)
            {
                return "ERROR";
            }
        }
    }
}