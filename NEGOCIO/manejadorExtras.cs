using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorExtras
    {
        ClaseExtras extraE = new ClaseExtras();

        // CREAMOS LA CLASE DE BUSQUEDA DE EXTRAS
        public List<ENTExtras> listarExtra(ENTExtras dato)
        {
            return extraE.listarExtra(dato);
        }
    }
}
