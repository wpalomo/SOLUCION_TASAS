using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseRuta
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA PARA LOS ORIGENES
        public List<ENTRuta> listarRuta(ENTRuta dato)
        {
            List<ENTRuta> coleccion = new List<ENTRuta>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTRuta todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTRuta()
                        {
                            IIDRUTA = dtConsulta.Rows[i][0].ToString(),
                            IIDTERMINALORIGEN = dtConsulta.Rows[i][1].ToString(),
                            IIDTERMINALDESTINO = dtConsulta.Rows[i][2].ToString(),
                            ICODIGO = dtConsulta.Rows[i][3].ToString(),
                            ITERMINALORIGEN = dtConsulta.Rows[i][4].ToString(),
                            ITERMINALDESTINO = dtConsulta.Rows[i][5].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i][6].ToString(),
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
