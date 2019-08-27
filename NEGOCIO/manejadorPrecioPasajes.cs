using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorPrecioPasajes
    {
        ClasePrecioPasajes preciosD = new ClasePrecioPasajes();

        // CREAMOS LA CLASE DE BUSQUEDA DE PRECIOS DE PASAJES
        public List<ENTPrecioPasajes> listarPrecioPasajes(ENTPrecioPasajes dato)
        {
            return preciosD.listarPrecioPasajes(dato);
        }
    }
}
