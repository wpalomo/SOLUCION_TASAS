using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseHorarioMasivo
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;

        // CREAMOS LA CLASE DE BUSQUEDA DE HORARIOS
        public List<ENTHorarioMasivo> listarHorarioMasivo(ENTHorarioMasivo dato)
        {
            List<ENTHorarioMasivo> coleccion = new List<ENTHorarioMasivo>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTHorarioMasivo todos;

                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {

                        todos = new ENTHorarioMasivo()
                        {
                            INUMERO = (i + 1).ToString(),
                            IIDHORARIO = dtConsulta.Rows[i][0].ToString(),
                            IHORARIO = Convert.ToDateTime(dtConsulta.Rows[i][1].ToString()).ToString("HH:mm")                            
                        };

                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
