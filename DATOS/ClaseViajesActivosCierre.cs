using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseViajesActivosCierre
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE VIAJES ACTIVOS GENERADOS
        public List<ENTViajesActivosCierre> listarViajesActivosCierre(ENTViajesActivosCierre dato)
        {
            List<ENTViajesActivosCierre> coleccion = new List<ENTViajesActivosCierre>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTViajesActivosCierre todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTViajesActivosCierre()
                        {
                            INUMERO = (i + 1).ToString(),
                            IFECHAVIAJE = Convert.ToDateTime(dtConsulta.Rows[i][0].ToString()).ToString("dd/MM/yyyy"),
                            IHORAVIAJE = Convert.ToDateTime(dtConsulta.Rows[i][1].ToString()).ToString("HH:mm   "),
                            IRUTA = dtConsulta.Rows[i][2].ToString(),
                            ICANTIDAD = dtConsulta.Rows[i][3].ToString(),
                            IVALOR = dtConsulta.Rows[i][4].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
