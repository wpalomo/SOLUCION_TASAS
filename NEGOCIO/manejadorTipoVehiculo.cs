using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorTipoVehiculo
    {
        ClaseTipoVehiculo tipoVehiculoD = new ClaseTipoVehiculo();

        // CREAMOS LA CLASE DE BUSQUEDA PARA TIPO DE VEHICULO
        public List<ENTTipoVehiculo> listarTipoVehiculo(ENTTipoVehiculo dato)
        {
            return tipoVehiculoD.listarTipoVehiculo(dato);
        }
    }
}
