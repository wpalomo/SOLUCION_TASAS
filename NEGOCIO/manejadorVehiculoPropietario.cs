using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorVehiculoPropietario
    {
        ClaseVehiculoPropietario propietarioD = new ClaseVehiculoPropietario();

        // CREAMOS LA CLASE DE BUSQUEDA PARA CARGOS
        public List<ENTVehiculoPropietario> listarVehiculoPropietario(ENTVehiculoPropietario dato)
        {
            return propietarioD.listarVehiculoPropietario(dato);
        }
    }
}
