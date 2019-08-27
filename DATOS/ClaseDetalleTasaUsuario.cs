using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;

namespace DATOS
{
    public class ClaseDetalleTasaUsuario
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE PARA DETALLAR LAS TASAS DE USUARIO
        public List<ENTDetalleTasaUsuario> listarTasaUsuario(ENTDetalleTasaUsuario dato)
        {
            List<ENTDetalleTasaUsuario> coleccion = new List<ENTDetalleTasaUsuario>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTDetalleTasaUsuario todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTDetalleTasaUsuario()
                        {
                            INUMERO = (i + 1).ToString(),
                            IFECHA = Convert.ToDateTime(dtConsulta.Rows[i][0].ToString()).ToString("dd-MM-yyyy"),
                            IHORA = Convert.ToDateTime(dtConsulta.Rows[i][1].ToString()).ToString("HH:mm"),
                            ICANTIDAD = dtConsulta.Rows[i][2].ToString(),
                            IVALOR = dtConsulta.Rows[i][3].ToString(),
                            ITASAUSUARIO = dtConsulta.Rows[i][4].ToString(),
                            IESTADOMOVIMIENTO = dtConsulta.Rows[i][5].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
