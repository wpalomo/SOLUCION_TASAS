using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseReporteCobroAdministracion
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE REPORTE DE COBROS ADMINISTRATIVOS
        public List<ENTReporteCobroAdministracion> listarCobros(ENTReporteCobroAdministracion dato)
        {
            List<ENTReporteCobroAdministracion> coleccion = new List<ENTReporteCobroAdministracion>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTReporteCobroAdministracion todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTReporteCobroAdministracion()
                        {
                            INUMERO = (i + 1).ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i]["nombre"].ToString(),
                            IVEHICULO = dtConsulta.Rows[i]["vehiculo"].ToString(),
                            IFECHA = Convert.ToDateTime(dtConsulta.Rows[i]["fecha_viaje"].ToString()).ToString("dd/MM/yyyy"),
                            IHORA = Convert.ToDateTime(dtConsulta.Rows[i]["hora_salida"].ToString()).ToString("HH:mm"),
                            IPROPIETARIO = dtConsulta.Rows[i]["propietario"].ToString(),
                            IVALOR = dtConsulta.Rows[i]["valor_pago"].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
