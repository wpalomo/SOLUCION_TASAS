using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorViajesActivosCierre
    {
        ClaseViajesActivosCierre viajesD = new ClaseViajesActivosCierre();

        // CREAMOS LA CLASE DE BUSQUEDA DE VIAJES ACTIVOS GENERADOS
        public List<ENTViajesActivosCierre> listarViajesActivosCierre(ENTViajesActivosCierre dato)
        {
            return viajesD.listarViajesActivosCierre(dato);
        }
    }
}
