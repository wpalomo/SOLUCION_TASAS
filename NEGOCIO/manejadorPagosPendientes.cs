using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorPagosPendientes
    {
        ClasePagosPendientes pendientesD = new ClasePagosPendientes();

        // CREAMOS LA CLASE DE BUSQUEDA DE PAGOS PENDIENTES
        public List<ENTPagosPendientes> listarPagosPendientes(ENTPagosPendientes dato)
        {
            return pendientesD.listarPagosPendientes(dato);
        }
    }
}
