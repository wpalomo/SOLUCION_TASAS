using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorProgramacion
    {
        ClaseProgramacion programacionD = new ClaseProgramacion();

        // CREAMOS LA CLASE DE BUSQUEDA DE PROGRAMACIONES
        public List<ENTProgramacion> listarProgramacion(ENTProgramacion dato)
        {
            return programacionD.listarProgramacion(dato);
        }
    }
}
