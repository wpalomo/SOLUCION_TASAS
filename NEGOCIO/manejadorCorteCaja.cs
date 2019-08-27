using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorCorteCaja
    {
        ClaseCorteCaja corteCajaD = new ClaseCorteCaja();

        // CREAMOS LA CLASE DE BUSQUEDA DE FRECUENCIAS 
        public List<ENTCorteCaja> listarCorteCaja(ENTCorteCaja dato)
        {
            return corteCajaD.listarCorteCaja(dato);
        }
    }
}
