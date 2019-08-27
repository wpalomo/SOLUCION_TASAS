using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;


namespace DATOS
{
    public class ClaseChofer
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;


        // CREAMOS LA CLASE DE BUSQUEDA PARA CHOFERES
        public List<ENTChofer> listarChofer(ENTChofer dato)
        {
            List<ENTChofer> coleccion = new List<ENTChofer>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTChofer todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTChofer()
                        {
                            IIDCHOFER = dtConsulta.Rows[i][0].ToString(),
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
