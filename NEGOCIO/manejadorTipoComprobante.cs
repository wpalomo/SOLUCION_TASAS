using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorTipoComprobante
    {
        ClaseTipoComprobante tipoComprobanteD = new ClaseTipoComprobante();

        // LISTAR
        public List<ENTTipoComprobante> listar(ENTTipoComprobante dato)
        {
            return tipoComprobanteD.listar(dato);
        }
    }
}
