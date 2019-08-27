using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorCategorias
    {
        ClaseCategorias categoriaD = new ClaseCategorias();

        // CREAMOS LA CLASE DE CATEGORIAS
        public List<ENTCategorias> listarCategorias(ENTCategorias dato)
        {
            return categoriaD.listarCategorias(dato);
        }
    }
}
