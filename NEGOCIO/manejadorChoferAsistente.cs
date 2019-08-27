using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorChoferAsistente
    {
        ClaseChoferAsistente choferAsistenteD = new ClaseChoferAsistente();

        // CREAMOS LA CLASE DE BUSQUEDA PARA CHOFERES Y ASISTENTES
        public List<ENTChoferAsistente> listarChoferAsistente(ENTChoferAsistente dato)
        {
            return choferAsistenteD.listarChoferAsistente(dato);
        }
    }
}
