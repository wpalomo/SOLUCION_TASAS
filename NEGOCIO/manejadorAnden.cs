using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorAnden
    {
        ClaseAnden andenD = new ClaseAnden();

        // CREAMOS LA CLASE DE BUSQUEDA PARA ANDENES
        public List<ENTAnden> listarAnden(ENTAnden dato)
        {
            return andenD.listarAnden(dato);
        }
    }
}
