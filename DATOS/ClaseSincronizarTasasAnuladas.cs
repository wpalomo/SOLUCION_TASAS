using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;

namespace DATOS
{
    public class ClaseSincronizarTasasAnuladas
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        public List<ENTSincronizarTasasAnuladas> listarTasasAnuladas(ENTSincronizarTasasAnuladas dato)
        {
            List<ENTSincronizarTasasAnuladas> coleccion = new List<ENTSincronizarTasasAnuladas>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTSincronizarTasasAnuladas todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTSincronizarTasasAnuladas()
                        {
                            INUMERO = (i + 1).ToString(),
                            IFECHAFACTURA = Convert.ToDateTime(dtConsulta.Rows[i][0].ToString()).ToString("dd-MM-yyyy"),
                            INUMEROFACTURA = dtConsulta.Rows[i][1].ToString().Trim() + "-" + dtConsulta.Rows[i][2].ToString().Trim() + "-" + dtConsulta.Rows[i][3].ToString().Trim().PadLeft(9, '0'),
                            ITASAUSUARIO = dtConsulta.Rows[i][4].ToString(),
                            IIDTASA = dtConsulta.Rows[i][5].ToString(),
                            IIDFACTURA = dtConsulta.Rows[i][6].ToString()

                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
