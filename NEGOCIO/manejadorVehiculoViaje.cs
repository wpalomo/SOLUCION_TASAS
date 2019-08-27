using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorVehiculoViaje
    {
        ClaseVehiculoViaje vehiculoViajeD = new ClaseVehiculoViaje();

        // CREAMOS LA CLASE DE BUSQUEDA DE VEHICULOS DEL VIAJE
        public List<ENTVehiculoViaje> listarVehiculoViaje(ENTVehiculoViaje dato)
        {
            return vehiculoViajeD.listarVehiculoViaje(dato);
        }
    }
}
