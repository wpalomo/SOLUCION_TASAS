using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorTerminal
    {
        ClaseTerminal terminalD = new ClaseTerminal();

        // CREAMOS LA CLASE DE BUSQUEDA PARA TERMINALES
        public List<ENTTerminal> listarTerminales(ENTTerminal dato)
        {
            return terminalD.listarTerminales(dato);
        }
    }
}
