using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorDirectorio
    {
        ClaseDirectorio directorioD = new ClaseDirectorio();

        // LISTAR
        public List<ENTDirectorio> listar(ENTDirectorio dato)
        {
            return directorioD.listar(dato);
        }
    }
}
