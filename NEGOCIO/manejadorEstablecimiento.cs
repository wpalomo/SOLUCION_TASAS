using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorEstablecimiento
    {
        ClaseEstablecimiento establecimientosD = new ClaseEstablecimiento();

        // LISTAR
        public List<ENTEstablecimiento> listar(ENTEstablecimiento dato)
        {
            return establecimientosD.listar(dato);
        }
    }
}
