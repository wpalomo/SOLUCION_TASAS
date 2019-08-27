using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClasePueblosHorarios
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE PARA LISTAR LAS OFICINAS TERMINALES 
        public List<ENTPueblosHorarios> listarPueblosHorarios(ENTPueblosHorarios dato)
        {
            List<ENTPueblosHorarios> coleccion = new List<ENTPueblosHorarios>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTPueblosHorarios todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTPueblosHorarios()
                        {
                            INUMERO = (i+1).ToString(),
                            IIDPUEBLO = dtConsulta.Rows[i][0].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i][1].ToString(),
                            IPROVINCIA = dtConsulta.Rows[i][2].ToString(),
                            IESTADO = dtConsulta.Rows[i][3].ToString(),
                            ICUENTA = dtConsulta.Rows[i][4].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
