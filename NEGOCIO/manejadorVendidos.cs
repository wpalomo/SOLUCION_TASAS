using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorVendidos
    {
        ClaseVendidos vendidoD = new ClaseVendidos();

        // CREAMOS LA CLASE DE BUSQUEDA DE BOLETOS VENDIDOS
        public List<ENTVendidos> listarVendidos(ENTVendidos dato)
        {
            return vendidoD.listarVendidos(dato);
        }
    }
}
