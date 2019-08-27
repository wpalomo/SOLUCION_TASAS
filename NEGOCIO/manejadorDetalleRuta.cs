using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorDetalleRuta
    {
        ClaseDetalleRuta detalleD = new ClaseDetalleRuta();

        // CREAMOS LA CLASE DE LOS DETALLES DE RUTA
        public List<ENTDetalleRuta> listarDetalleRuta(ENTDetalleRuta dato)
        {
            return detalleD.listarDetalleRuta(dato);
        }
    }
}
