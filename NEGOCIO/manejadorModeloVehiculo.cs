using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorModeloVehiculo
    {
        ClaseModeloVehiculo modeloVehiculoD = new ClaseModeloVehiculo();

        public List<ENTModeloVehiculo> listarModeloVehiculo(ENTModeloVehiculo dato)
        {
            return modeloVehiculoD.listarModeloVehiculo(dato);
        }
    }
}
