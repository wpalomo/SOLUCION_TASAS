using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseReporteVentaClientes
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;

        // CREAMOS LA CLASE DE BUSQUEDA DE BOLETOS VENDIDOS
        public List<ENTReporteVentaClientes> listarReporteVentaClientes(ENTReporteVentaClientes dato)
        {
            List<ENTReporteVentaClientes> coleccion = new List<ENTReporteVentaClientes>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTReporteVentaClientes todos;

                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {

                        todos = new ENTReporteVentaClientes()
                        {
                            INUMERO = (i + 1).ToString(),
                            IIDENTIFICACION = dtConsulta.Rows[i][0].ToString(),
                            IPASAJERO = (dtConsulta.Rows[i][1].ToString() + " " + dtConsulta.Rows[i][2].ToString()).Trim(),
                            ITIPOCLIENTE = dtConsulta.Rows[i][3].ToString(),
                            ICANTIDAD = dtConsulta.Rows[i][11].ToString(),
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
