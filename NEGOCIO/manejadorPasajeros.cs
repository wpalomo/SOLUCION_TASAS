using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorPasajeros
    {
        ClasePasajeros pasajeroD = new ClasePasajeros();

        // CREAMOS LA CLASE DE BUSQUEDA PARA PASAJEROS
        public List<ENTPasajeros> listarPasajeros(ENTPasajeros dato)
        {
            return pasajeroD.listarPasajeros(dato);
        }
    }
}
