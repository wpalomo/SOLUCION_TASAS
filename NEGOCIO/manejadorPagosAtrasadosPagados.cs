using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using DATOS;

namespace NEGOCIO
{
    public class manejadorPagosAtrasadosPagados
    {
        ClasePagosAtrasadosPagados atrasadosD = new ClasePagosAtrasadosPagados();

        // CREAMOS LA CLASE DE BUSQUEDA DE PAGOS ATRASADOS PAGADOS
        public List<ENTPagosAtrasadosPagados> listarPagosAtrasadosPagados(ENTPagosAtrasadosPagados dato)
        {
            return atrasadosD.listarPagosAtrasadosPagados(dato);
        }
    }
}
