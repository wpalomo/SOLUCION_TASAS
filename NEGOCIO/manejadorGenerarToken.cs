using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorGenerarToken
    {
        ClaseGenerarToken generarD = new ClaseGenerarToken();

        // CREAMOS LA CLASE DE BUSQUEDA PARA TOKENS GENERADOS
        public List<ENTGenerarToken> listarToken(ENTGenerarToken dato)
        {
            return generarD.listarToken(dato);
        }
    }
}
