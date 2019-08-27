using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATOS;
using ENTIDADES;

namespace NEGOCIO
{
    public class manejadorReporteVentaClientes
    {
        ClaseReporteVentaClientes reporteClientesD = new ClaseReporteVentaClientes();

        // CREAMOS LA CLASE DE BUSQUEDA DE BOLETOS VENDIDOS
        public List<ENTReporteVentaClientes> listarReporteVentaClientes(ENTReporteVentaClientes dato)
        {
            return reporteClientesD.listarReporteVentaClientes(dato);
        }
    }
}
