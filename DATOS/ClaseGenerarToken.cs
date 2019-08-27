using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;

namespace DATOS
{
    public class ClaseGenerarToken
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA PARA TOKENS GENERADOS
        public List<ENTGenerarToken> listarToken(ENTGenerarToken dato)
        {
            List<ENTGenerarToken> coleccion = new List<ENTGenerarToken>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTGenerarToken todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTGenerarToken()
                        {
                            IID = dtConsulta.Rows[i][0].ToString(),
                            ITOKEN = dtConsulta.Rows[i][1].ToString(),
                            IFECHA = Convert.ToDateTime(dtConsulta.Rows[i][2].ToString()).ToString("dd-MM-yyyy"),
                            IMAXIMO = dtConsulta.Rows[i][3].ToString(),
                            ICUENTA = dtConsulta.Rows[i][4].ToString(),
                            IEMITIDOS = dtConsulta.Rows[i][5].ToString(),
                            IANULADOS = dtConsulta.Rows[i][6].ToString(),
                            IESTADO = dtConsulta.Rows[i][7].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
