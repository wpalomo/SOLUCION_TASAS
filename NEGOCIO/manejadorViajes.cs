using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorViajes
    {
        ClaseViajes viajesD = new ClaseViajes();

        public List<ENTModoViaje> listarModoViaje(ENTModoViaje dato)
        {
            return viajesD.listarModoViaje(dato);
        }
    }
}
