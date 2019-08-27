using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;

namespace DATOS
{
    public class ClaseSincronizarTasaUsuario
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE TASAS DE USUARIO NO SINCRONIZADAS
        public List<ENTSincronizarTasaUsuario> listarTasaNoEnviada(ENTSincronizarTasaUsuario dato)
        {
            List<ENTSincronizarTasaUsuario> coleccion = new List<ENTSincronizarTasaUsuario>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTSincronizarTasaUsuario todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTSincronizarTasaUsuario()
                        {
                            INUMERO = (i + 1).ToString(),
                            IIDFACTURA = dtConsulta.Rows[i][0].ToString(),
                            IIDENTIFICACION = dtConsulta.Rows[i][1].ToString(),
                            ICLIENTE = dtConsulta.Rows[i][2].ToString(),
                            IFECHAFACTURA = Convert.ToDateTime(dtConsulta.Rows[i][3].ToString()).ToString("dd-MM-yyyy"),
                            IFACTURA = dtConsulta.Rows[i][4].ToString().Trim().PadLeft(3, '0') + "-" + dtConsulta.Rows[i][5].ToString().Trim().PadLeft(3, '0') + "-" + dtConsulta.Rows[i][6].ToString().Trim().PadLeft(9, '0'),
                            ITASAUSUARIO = dtConsulta.Rows[i][7].ToString(),
                            ICANTIDADTASAS = dtConsulta.Rows[i][8].ToString(),
                            IDIRECCIONFACTURA = dtConsulta.Rows[i][9].ToString(),
                            ITELEFONOFACTURA = dtConsulta.Rows[i][10].ToString(),
                            ICORREOFACTURA = dtConsulta.Rows[i][11].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
