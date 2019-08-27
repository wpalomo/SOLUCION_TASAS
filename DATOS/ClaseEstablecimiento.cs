using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;

namespace DATOS
{
    public class ClaseEstablecimiento
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // LISTAR
        public List<ENTEstablecimiento> listar(ENTEstablecimiento dato)
        {
            List<ENTEstablecimiento> coleccion = new List<ENTEstablecimiento>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTEstablecimiento todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTEstablecimiento()
                        {
                            INUMERO = (i + 1).ToString(),
                            ICODIGO = dtConsulta.Rows[i][0].ToString(),
                            INOMBRES = dtConsulta.Rows[i][1].ToString(),
                            IEST = dtConsulta.Rows[i][2].ToString(),
                            IPOT_EMI = dtConsulta.Rows[i][3].ToString(),
                            IDIRECCION = dtConsulta.Rows[i][4].ToString(),
                            IESTADO = dtConsulta.Rows[i][5].ToString(),
                            IID_LOCALIDAD = dtConsulta.Rows[i][6].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
