using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorOficinista
    {
        ClaseOficinista oficinistaD = new ClaseOficinista();

        // CREAMOS LA CLASE DE BUSQUEDA PARA OFICINISTAS
        public List<ENTOficinista> listarOficinista(ENTOficinista dato)
        {
            return oficinistaD.listarOficinista(dato);
        }
    }
}
