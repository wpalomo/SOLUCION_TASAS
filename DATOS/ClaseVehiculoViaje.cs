using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseVehiculoViaje
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE VEHICULOS DEL VIAJE
        public List<ENTVehiculoViaje> listarVehiculoViaje(ENTVehiculoViaje dato)
        {
            List<ENTVehiculoViaje> coleccion = new List<ENTVehiculoViaje>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTVehiculoViaje todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTVehiculoViaje()
                        {
                            INUMERO = (i+1).ToString(),
                            IIDREGISTRO = dtConsulta.Rows[i][0].ToString(),
                            IPLACA = dtConsulta.Rows[i][1].ToString(),
                            IDISCO = dtConsulta.Rows[i][2].ToString(),
                            ITIPOVEHICULO = dtConsulta.Rows[i][3].ToString(),
                            IREGISTRO = dtConsulta.Rows[i][4].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
