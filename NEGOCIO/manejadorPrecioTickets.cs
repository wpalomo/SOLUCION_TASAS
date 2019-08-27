using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorPrecioTickets
    {
        ClasePrecioTickets ticketsD = new ClasePrecioTickets();

        // CREAMOS LA CLASE DE BUSQUEDA DE PRECIOS DE TICKETS
        public List<ENTPrecioTickets> listarPrecioTickets(ENTPrecioTickets dato)
        {
            return ticketsD.listarPrecioTickets(dato);
        }
    }
}
