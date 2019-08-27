using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;

namespace DATOS
{
    public class ClaseAsistente
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;


        // CREAMOS LA CLASE DE BUSQUEDA PARA ASISTENTES
        public List<ENTAsistente> listarAsistente(ENTAsistente dato)
        {
            List<ENTAsistente> coleccion = new List<ENTAsistente>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTAsistente todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTAsistente()
                        {
                            IIDASISTENTE = dtConsulta.Rows[i][0].ToString(),
                            IIDPERSONA = dtConsulta.Rows[i][1].ToString(),
                            ICODIGO = dtConsulta.Rows[i][2].ToString(),
                            INOMBRE = dtConsulta.Rows[i][3].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i][4].ToString(),
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
