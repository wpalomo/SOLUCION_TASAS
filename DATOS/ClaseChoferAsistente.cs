using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;

namespace DATOS
{
    public class ClaseChoferAsistente
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA PARA CHOFERES Y ASISTENTES
        public List<ENTChoferAsistente> listarChoferAsistente(ENTChoferAsistente dato)
        {
            List<ENTChoferAsistente> coleccion = new List<ENTChoferAsistente>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTChoferAsistente todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTChoferAsistente()
                        {
                            INUMERO = (i + 1).ToString(),
                            IIDREGISTRO = dtConsulta.Rows[i][0].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i][1].ToString(),
                            ICODIGO = dtConsulta.Rows[i][2].ToString(),
                            INOMBRE = dtConsulta.Rows[i][3].ToString(),
                            IIDENTIFICACION = dtConsulta.Rows[i]["identificacion"].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
