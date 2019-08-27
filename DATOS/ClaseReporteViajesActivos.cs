using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseReporteViajesActivos
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;

        // CREAMOS LA CLASE DE BUSQUEDA DE VIAJES ACTIVOS
        public List<ENTReporteViajesActivos> listarReporteViajesActivos(ENTReporteViajesActivos dato)
        {
            List<ENTReporteViajesActivos> coleccion = new List<ENTReporteViajesActivos>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTReporteViajesActivos todos;

                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {

                        todos = new ENTReporteViajesActivos()
                        {
                            INUMERO = (i + 1).ToString(),
                            INUMEROVIAJE = dtConsulta.Rows[i][0].ToString(),
                            IVEHICULO = dtConsulta.Rows[i][1].ToString(),
                            IFECHAVIAJE = Convert.ToDateTime(dtConsulta.Rows[i][2].ToString()).ToString("dd/MM/yyyy"),
                            IHORASALIDA = Convert.ToDateTime(dtConsulta.Rows[i][3].ToString()).ToString("HH:mm"),
                            ITIPOVIAJE = dtConsulta.Rows[i][4].ToString(),
                            IUSUARIO = dtConsulta.Rows[i][5].ToString(),
                            ICANTIDAD = dtConsulta.Rows[i][6].ToString(),
                            IVALOR = dtConsulta.Rows[i][7].ToString(),
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
