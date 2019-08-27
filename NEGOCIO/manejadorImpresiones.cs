using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorImpresiones
    {
        ClaseImpresiones impresionD = new ClaseImpresiones();

        // CREAMOS LA CLASE DE BUSQUEDA PARA IMPRESORAS
        public List<ENTImpresoras> listarImpresoras(ENTImpresoras dato)
        {
            return impresionD.listarImpresoras(dato);
        }
    }
}
