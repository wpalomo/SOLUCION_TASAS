using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorAsistente
    {
        ClaseAsistente asistenteD = new ClaseAsistente();

        // CREAMOS LA CLASE DE BUSQUEDA PARA ASISTENTES
        public List<ENTAsistente> listarAsistente(ENTAsistente dato)
        {
            return asistenteD.listarAsistente(dato);
        }
    }
}
