using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorComboDatos
    {
        ClaseComboDatos comboD = new ClaseComboDatos();

        // CREAMOS LA CLASE PARA LLENAR LOS COMBOBOX
        public List<ENTComboDatos> listarCombo(ENTComboDatos dato)
        {
            return comboD.listarCombo(dato);
        }
    }
}
