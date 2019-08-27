using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClasePueblos
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE PUEBLOS
        public List<ENTPueblos> listarPueblos(ENTPueblos dato)
        {
            List<ENTPueblos> coleccion = new List<ENTPueblos>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTPueblos todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTPueblos()
                        {
                            INUMERO = (i+1).ToString(),
                            IIDPUEBLO = dtConsulta.Rows[i][0].ToString(),
                            IIDLOCALIDADTERMINAL = dtConsulta.Rows[i][1].ToString(),
                            IIDLOCALIDADENCOMIENDA = dtConsulta.Rows[i][2].ToString(),
                            IIDPROVINCIA = dtConsulta.Rows[i][3].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i][4].ToString(),
                            IPROVINCIA = dtConsulta.Rows[i][5].ToString(),
                            ITERMINAL = dtConsulta.Rows[i][6].ToString(),
                            IENCOMIENDA = dtConsulta.Rows[i][7].ToString(),
                            ICOBROSADMINISTRACION = dtConsulta.Rows[i][8].ToString(),
                            ICOBROSOTROS = dtConsulta.Rows[i][9].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
