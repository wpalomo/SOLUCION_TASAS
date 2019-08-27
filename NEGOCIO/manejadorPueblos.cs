using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorPueblos
    {
        ClasePueblos puebloD = new ClasePueblos();

        // CREAMOS LA CLASE DE BUSQUEDA DE PUEBLOS
        public List<ENTPueblos> listarPueblos(ENTPueblos dato)
        {
            return puebloD.listarPueblos(dato);
        }
    }
}
