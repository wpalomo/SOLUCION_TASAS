using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorVehiculo
    {
        ClaseVehiculo vehiculoD = new ClaseVehiculo();

        // CREAMOS LA CLASE DE BUSQUEDA DE VEHICULOS
        public List<ENTVehiculo> listarVehiculos(ENTVehiculo dato)
        {
            return vehiculoD.listarVehiculos(dato);
        }
    }
}
