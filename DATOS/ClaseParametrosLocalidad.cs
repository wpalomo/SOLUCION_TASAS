using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseParametrosLocalidad
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;

        // CREAMOS LA CLASE DE LOS PARÁMETROS POR LOCALIDAD
        public List<ENTParametrosLocalidad> listarParametrosLocalidad(ENTParametrosLocalidad dato)
        {
            List<ENTParametrosLocalidad> coleccion = new List<ENTParametrosLocalidad>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTParametrosLocalidad todos;

                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTParametrosLocalidad()
                        {
                            INUMERO = (i + 1).ToString(),
                            IIDPARAMETROLOCALIDAD = dtConsulta.Rows[i][0].ToString(),
                            IIDPUEBLO = dtConsulta.Rows[i][1].ToString(),
                            IIDCIUDAD = dtConsulta.Rows[i][2].ToString(),
                            IIDVENDEDOR = dtConsulta.Rows[i][3].ToString(),
                            IPUEBLO = dtConsulta.Rows[i][4].ToString(),
                            IPAGOADMINISTRACION = dtConsulta.Rows[i][5].ToString(),
                            IPORCENTAJERETENCION = dtConsulta.Rows[i][6].ToString(),
                            IIDPRODUCTORETENCION = dtConsulta.Rows[i][7].ToString(),
                            IIDPRODUCTOPAGO = dtConsulta.Rows[i][8].ToString(),
                            INOMBRERETENCION = dtConsulta.Rows[i][9].ToString(),
                            INOMBREPAGO = dtConsulta.Rows[i][10].ToString(),
                            ITASAUSUARIO = dtConsulta.Rows[i][14].ToString(),
                            ICANTIDADMANIFIESTO = dtConsulta.Rows[i][15].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
