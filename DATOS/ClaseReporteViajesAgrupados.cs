using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;
namespace DATOS
{
    public class ClaseReporteViajesAgrupados
    {
        //ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;

        // CREAMOS LA CLASE DE BUSQUEDA 
        public List<ENTReporteViajesAgrupados> listarReporteViajesAgrupados(ENTReporteViajesAgrupados dato)
        {
            List<ENTReporteViajesAgrupados> coleccion = new List<ENTReporteViajesAgrupados>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTReporteViajesAgrupados todos;

                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {

                        todos = new ENTReporteViajesAgrupados()
                        {
                            INUMERO = (i + 1).ToString(),
                            ITRANSPORTE = dtConsulta.Rows[i][1].ToString(),
                            IFECHA_VIAJE = dtConsulta.Rows[i][3].ToString(),
                            IRUTA = dtConsulta.Rows[i][5].ToString(),
                            IUSUARIO = dtConsulta.Rows[i][6].ToString(),
                            IVALOR = dtConsulta.Rows[i][9].ToString(),
                            ICUENTA = dtConsulta.Rows[i][10].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
