using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorItinerario
    {
        ClaseItinerario itinerarioD = new ClaseItinerario();
        // CREAMOS LA CLASE DE BUSQUEDA PARA LOS ITINERARIOS
        public List<ENTItinierario> listarItinerario(ENTItinierario dato)
        {
            return itinerarioD.listarItinerario(dato);
        }
    }
}
