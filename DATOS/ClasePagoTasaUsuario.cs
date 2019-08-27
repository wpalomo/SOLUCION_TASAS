using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClasePagoTasaUsuario
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE PARA VER LOS PAGOS DE LAS TASAS DE USUARIO
        public List<ENTPagoTasaUsuario> listarPagoTasa(ENTPagoTasaUsuario dato)
        {
            List<ENTPagoTasaUsuario> coleccion = new List<ENTPagoTasaUsuario>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTPagoTasaUsuario todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTPagoTasaUsuario()
                        {
                            INUMERO = (i + 1).ToString(),
                            IIDJORNADA = dtConsulta.Rows[i][0].ToString(),
                            IFECHA = Convert.ToDateTime(dtConsulta.Rows[i][1].ToString()).ToString("dd-MM-yyyy"),
                            IJORNADA = dtConsulta.Rows[i][2].ToString(),
                            ICANTIDAD = dtConsulta.Rows[i][3].ToString(),
                            IVALOR = dtConsulta.Rows[i][4].ToString(),
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
