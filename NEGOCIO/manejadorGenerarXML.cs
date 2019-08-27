using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorGenerarXML
    {
        ClaseGenerarXML generarXMLD = new ClaseGenerarXML();

        // CREAMOS LA CLASE DE BUSQUEDA DE FACTURAS ELECTRONICAS
        public List<ENTGenerarXML> listarFacturasEmitidas(ENTGenerarXML dato)
        {
            return generarXMLD.listarFacturasEmitidas(dato);
        }
    }
}
