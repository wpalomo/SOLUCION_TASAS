using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorSincronizarTasasAnuladas
    {
        ClaseSincronizarTasasAnuladas anuladasD = new ClaseSincronizarTasasAnuladas();

        public List<ENTSincronizarTasasAnuladas> listarCorteCaja(ENTSincronizarTasasAnuladas dato)
        {
            return anuladasD.listarTasasAnuladas(dato);
        }

    }
}
