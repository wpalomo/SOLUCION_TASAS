using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorHorario
    {
        ClaseHorario horarioD = new ClaseHorario();

        // CREAMOS LA CLASE DE BUSQUEDA DE HORARIOS
        public List<ENTHorario> listarHorarios(ENTHorario dato)
        {
            return horarioD.listarHorarios(dato);
        }
    }
}
