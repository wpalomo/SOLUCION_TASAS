using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorRuta
    {
        ClaseRuta rutaD = new ClaseRuta();

        // CREAMOS LA CLASE DE BUSQUEDA PARA LOS ORIGENES
        public List<ENTRuta> listarRuta(ENTRuta dato)
        {
            return rutaD.listarRuta(dato);
        }
    }
}
