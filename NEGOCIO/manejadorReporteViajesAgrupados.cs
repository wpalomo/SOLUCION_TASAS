using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATOS;
using ENTIDADES;

namespace NEGOCIO
{
    public class manejadorReporteViajesAgrupados
    {
        ClaseReporteViajesAgrupados CAD = new ClaseReporteViajesAgrupados();

        // CREAMOS LA CLASE DE BUSQUEDA 
        public List<ENTReporteViajesAgrupados> listarReporteViajesAgrupados(ENTReporteViajesAgrupados dato)
        {
            return CAD.listarReporteViajesAgrupados(dato);
        }
    }
}
