using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClasePrecioPasajes
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE PRECIOS DE PASAJES
        public List<ENTPrecioPasajes> listarPrecioPasajes(ENTPrecioPasajes dato)
        {
            List<ENTPrecioPasajes> coleccion = new List<ENTPrecioPasajes>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTPrecioPasajes todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTPrecioPasajes()
                        {
                            IIDPRODUCTO = dtConsulta.Rows[i][0].ToString(),
                            IIDPRODUCTOPADRE = dtConsulta.Rows[i][1].ToString(),
                            IIDDESTINO = dtConsulta.Rows[i][2].ToString(),
                            IPAGAIVA = dtConsulta.Rows[i][3].ToString(),
                            ICGUNIDAD = dtConsulta.Rows[i][4].ToString(),
                            ICODIGO = dtConsulta.Rows[i][5].ToString(),
                            INOMBREPRODUCTO = dtConsulta.Rows[i][6].ToString(),
                            IVALOR = dtConsulta.Rows[i][7].ToString(),
                            IVALOROTRO = dtConsulta.Rows[i][8].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
