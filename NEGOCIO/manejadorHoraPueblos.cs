using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorHoraPueblos
    {
        ClaseHoraPueblos horaPuebloD = new ClaseHoraPueblos();

        // CREAMOS LA CLASE DE HORARIOS
        public List<ENTHoraPueblos> listarHoraPueblos(ENTHoraPueblos dato)
        {
            return horaPuebloD.listarHoraPueblos(dato);
        }
    }
}
