using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorReporteCobroAdministracion
    {
        ClaseReporteCobroAdministracion pagosD = new ClaseReporteCobroAdministracion();

        // CREAMOS LA CLASE DE REPORTE DE COBRO ADMINISTRATIVOS
        public List<ENTReporteCobroAdministracion> listarRetenciones(ENTReporteCobroAdministracion dato)
        {
            return pagosD.listarCobros(dato);
        }
    }
}
