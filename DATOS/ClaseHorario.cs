using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseHorario
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE HORARIOS
        public List<ENTHorario> listarHorarios(ENTHorario dato)
        {
            List<ENTHorario> coleccion = new List<ENTHorario>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTHorario todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTHorario()
                        {
                            IIDHORARIO = dtConsulta.Rows[i][0].ToString(),
                            IIDJORNADA = dtConsulta.Rows[i][1].ToString(),
                            ICODIGO = dtConsulta.Rows[i][2].ToString(),
                            IHORASALIDA = Convert.ToDateTime(dtConsulta.Rows[i][3].ToString()).ToString("HH:mm"),
                            IJORNADA = dtConsulta.Rows[i][4].ToString(),
                            IESTADO = dtConsulta.Rows[i][5].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
