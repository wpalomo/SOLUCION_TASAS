using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorMarcaVehiculo
    {
        ClaseMarcaVehiculo marcaVehiculoD = new ClaseMarcaVehiculo();

        public List<ENTMarcaVehiculo> listarMarcaVehiculo(ENTMarcaVehiculo dato)
        {
            return marcaVehiculoD.listarMarcaVehiculo(dato);
        }
    }
}
