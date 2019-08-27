using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorPagoTasaUsuario
    {
        ClasePagoTasaUsuario pagoTasaD = new ClasePagoTasaUsuario();

        // CREAMOS LA CLASE PARA VER LOS PAGOS DE LAS TASAS DE USUARIO
        public List<ENTPagoTasaUsuario> listarPagoTasa(ENTPagoTasaUsuario dato)
        {
            return pagoTasaD.listarPagoTasa(dato);
        }
    }
}
