using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorDetalleTasaUsuario
    {
        ClaseDetalleTasaUsuario detalleTasaD = new ClaseDetalleTasaUsuario();

        // CREAMOS LA CLASE PARA DETALLAR LAS TASAS DE USUARIO
        public List<ENTDetalleTasaUsuario> listarTasaUsuario(ENTDetalleTasaUsuario dato)
        {
            return detalleTasaD.listarTasaUsuario(dato);
        }
    }
}
