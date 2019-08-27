using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorAsignarViaje
    {
        ClaseAsignarViaje asignarD = new ClaseAsignarViaje();

        // CREAMOS LA CLASE DE BUSQUEDA DE VIAJES
        public List<ENTViajes> listarViajes(ENTViajes dato)
        {
            return asignarD.listarViajes(dato);
        }
    }
}
