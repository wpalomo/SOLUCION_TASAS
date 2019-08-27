using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseHoraPueblos
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE HORARIOS
        public List<ENTHoraPueblos> listarHoraPueblos(ENTHoraPueblos dato)
        {
            List<ENTHoraPueblos> coleccion = new List<ENTHoraPueblos>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTHoraPueblos todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTHoraPueblos()
                        {
                            IIDHORARIO = dtConsulta.Rows[i][0].ToString(),
                            IHORA = Convert.ToDateTime(dtConsulta.Rows[i][1].ToString()).ToString("HH:mm"),
                            IJORNADA = dtConsulta.Rows[i][2].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
