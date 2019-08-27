using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;

namespace DATOS
{
    public class ClaseImpresiones
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA PARA IMPRESORAS
        public List<ENTImpresoras> listarImpresoras(ENTImpresoras dato)
        {
            List<ENTImpresoras> coleccion = new List<ENTImpresoras>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTImpresoras todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTImpresoras()
                        {
                            IIDIMPRESORA = dtConsulta.Rows[i][0].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i][1].ToString(),
                            IPATH = dtConsulta.Rows[i][2].ToString(),
                            ICORTARPAPEL = dtConsulta.Rows[i][3].ToString(),
                            IABRIRCAJON = dtConsulta.Rows[i][4].ToString(),
                            INUMEROIMPRESION = dtConsulta.Rows[i][5].ToString(),
                            IIDLOCALIDAD = dtConsulta.Rows[i][6].ToString(),
                            IIDPUEBLO = dtConsulta.Rows[i][7].ToString(),
                            IPUEBLO = dtConsulta.Rows[i][8].ToString(),
                            IIPLOCAL = dtConsulta.Rows[i][9].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
