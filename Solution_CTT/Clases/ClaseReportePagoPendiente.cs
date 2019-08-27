using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Solution_CTT.Clases
{
    public class ClaseReportePagoPendiente
    {
        Clases.ClaseManejoCaracteres caracteres = new ClaseManejoCaracteres();

        string sTexto;

        public string llenarReporte(string sUsuario, string sFechaIngreso, string sFechaViaje, 
            string sHoraViaje, string sRuta, string sNumeroViaje, string sVehiculo, 
            string sValorCobrar, string sValorAbono, string sValorAdeudado, string sObservacion)
        {
            try
            {
                sTexto = "";
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "COMPAÑÍA DE TRANSPORTES".PadLeft(33, ' ') + Environment.NewLine;
                sTexto += "\"EXPRESS ATENAS S.A.\"".PadLeft(32, ' ') + Environment.NewLine;
                sTexto += "Sirviendo a la Provincia de Bolívar".PadLeft(38, ' ') + Environment.NewLine + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "REPORTE DE PAGO DE AMINISTRACION".PadLeft(36, ' ') + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "OFICINISTA: " + sUsuario.Trim().ToUpper() + Environment.NewLine;
                sTexto += "FECHA     : " + sFechaIngreso + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "DETALLE DEL VIAJE PAGADO".PadLeft(30, ' ') + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "FECHA: " + sFechaViaje.PadRight(16, ' ') + "HORA: " + sHoraViaje + Environment.NewLine;
                sTexto += "RUTA : " + sRuta + Environment.NewLine;
                sTexto += "VIAJE: " + sNumeroViaje.PadRight(12, ' ') + "VEHICULO: " + sVehiculo + Environment.NewLine + Environment.NewLine;
                sTexto += "VALOR A COBRAR: USD $ " + sValorCobrar + Environment.NewLine;
                sTexto += "VALOR ABONO   : USD $ " + sValorAbono + Environment.NewLine;
                sTexto += "VALOR ADEUDADO: USD $ " + sValorAdeudado + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "OBSERVACION:" + Environment.NewLine;
                sTexto += caracteres.saltoLinea(sObservacion, 0) + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "Derechos RESERVADOS - APLICSIS".PadLeft(34, ' ') + Environment.NewLine;

                return sTexto;
            }

            catch (Exception)
            {
                return "ERROR";
            }
        }
    }
}