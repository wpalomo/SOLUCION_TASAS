using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorTipoAsiento
    {
        ClaseTipoAsiento tipoAsientoD = new ClaseTipoAsiento();

        // CREAMOS LA CLASE DE BUSQUEDA DE TIPO DE ASIENTO
        public List<ENTTipoAsiento> listarTipoAsiento(ENTTipoAsiento dato)
        {
            return tipoAsientoD.listarTipoAsiento(dato);
        }
    }
}
