using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClasePagosAtrasadosPagados
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE PAGOS ATRASADOS PAGADOS
        public List<ENTPagosAtrasadosPagados> listarPagosAtrasadosPagados(ENTPagosAtrasadosPagados dato)
        {
            List<ENTPagosAtrasadosPagados> coleccion = new List<ENTPagosAtrasadosPagados>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTPagosAtrasadosPagados todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTPagosAtrasadosPagados()
                        {
                            INUMERO = (i + 1).ToString(),
                            IFECHA = Convert.ToDateTime(dtConsulta.Rows[i][0].ToString()).ToString("dd-MM-yyyy"),
                            IHORA = Convert.ToDateTime(dtConsulta.Rows[i][1].ToString()).ToString("HH:mm"),
                            IVEHICULO = dtConsulta.Rows[i][2].ToString(),
                            IVALOR = dtConsulta.Rows[i][3].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
