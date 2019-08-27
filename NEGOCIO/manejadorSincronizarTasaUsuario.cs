using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorSincronizarTasaUsuario
    {
        ClaseSincronizarTasaUsuario tasasD = new ClaseSincronizarTasaUsuario();

        // CREAMOS LA CLASE DE BUSQUEDA DE TASAS DE USUARIO NO SINCRONIZADAS
        public List<ENTSincronizarTasaUsuario> listarTasaNoEnviada(ENTSincronizarTasaUsuario dato)
        {
            return tasasD.listarTasaNoEnviada(dato);
        }
    }
}
