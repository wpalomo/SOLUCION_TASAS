using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorReporteCobroRetenciones
    {
        ClaseReporteCobroRetenciones retencionesD = new ClaseReporteCobroRetenciones();

        // CREAMOS LA CLASE DE REPORTE DE COBRO DE RETENCIONES
        public List<ENTReporteCobroRetenciones> listarRetenciones(ENTReporteCobroRetenciones dato)
        {
            return retencionesD.listarRetenciones(dato);
        }
    }
}
