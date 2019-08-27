using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;

namespace DATOS
{
    public class ClaseCategorias
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE CATEGORIAS
        public List<ENTCategorias> listarCategorias(ENTCategorias dato)
        {
            List<ENTCategorias> coleccion = new List<ENTCategorias>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTCategorias todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTCategorias()
                        {
                            IIDPRODUCTO = dtConsulta.Rows[i][0].ToString(),
                            IIDPUEBLO = dtConsulta.Rows[i][1].ToString(),
                            ICODIGO = dtConsulta.Rows[i][2].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i][3].ToString(),
                            IPUEBLO = dtConsulta.Rows[i][4].ToString(),
                            IMODIFICABLE = dtConsulta.Rows[i][5].ToString(),
                            IPRECIOMODIFICABLE = dtConsulta.Rows[i][6].ToString(),
                            IPAGAIVA = dtConsulta.Rows[i][7].ToString(),
                            IUNIDADCONSUMO = dtConsulta.Rows[i][8].ToString(),
                            IAPLICAEXTRA = dtConsulta.Rows[i][9].ToString(),
                            ITICKET = dtConsulta.Rows[i][10].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
