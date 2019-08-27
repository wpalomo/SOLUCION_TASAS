using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorReporteViajesActivos
    {
        ClaseReporteViajesActivos reporteViajesD = new ClaseReporteViajesActivos();

        // CREAMOS LA CLASE DE BUSQUEDA DE VIAJES ACTIVOS
        public List<ENTReporteViajesActivos> listarReporteViajesActivos(ENTReporteViajesActivos dato)
        {
            return reporteViajesD.listarReporteViajesActivos(dato);
        }
    }
}
