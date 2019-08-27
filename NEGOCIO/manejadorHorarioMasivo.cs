using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorHorarioMasivo
    {
        ClaseHorarioMasivo horarioMasivoD = new ClaseHorarioMasivo();

        // CREAMOS LA CLASE DE BUSQUEDA DE HORARIOS
        public List<ENTHorarioMasivo> listarHorarioMasivo(ENTHorarioMasivo dato)
        {
            return horarioMasivoD.listarHorarioMasivo(dato);
        }
    }
}
