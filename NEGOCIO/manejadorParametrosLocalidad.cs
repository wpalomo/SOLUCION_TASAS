using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorParametrosLocalidad
    {
        ClaseParametrosLocalidad parametrosD = new ClaseParametrosLocalidad();

        // CREAMOS LA CLASE DE LOS PARÁMETROS POR LOCALIDAD
        public List<ENTParametrosLocalidad> listarParametrosLocalidad(ENTParametrosLocalidad dato)
        {
            return parametrosD.listarParametrosLocalidad(dato);
        }
    }
}
