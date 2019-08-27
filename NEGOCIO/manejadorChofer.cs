using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorChofer
    {
        ClaseChofer choferD = new ClaseChofer();

        // CREAMOS LA CLASE DE BUSQUEDA PARA CHOFERES
        public List<ENTChofer> listarChofer(ENTChofer dato)
        {
            return choferD.listarChofer(dato);
        }
    }
}
