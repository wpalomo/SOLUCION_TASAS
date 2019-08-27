using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATOS;
using ENTIDADES;

namespace NEGOCIO
{
    public class manejadorDisco
    {
        ClaseDisco vehiculoM = new ClaseDisco();

        // CREAMOS LA CLASE DE BUSQUEDA DE DISCOS
        public List<ENTDisco> listarDisco(ENTDisco dato)
        {
            return vehiculoM.listarDisco(dato);
        }
    }
}
