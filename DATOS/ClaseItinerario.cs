using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseItinerario
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA PARA LOS ITINERARIOS
        public List<ENTItinierario> listarItinerario(ENTItinierario dato)
        {
            List<ENTItinierario> coleccion = new List<ENTItinierario>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTItinierario todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTItinierario()
                        {
                            IIDITINERARIO = dtConsulta.Rows[i][0].ToString(),
                            IIDRUTA = dtConsulta.Rows[i][1].ToString(),
                            IIDHORARIO = dtConsulta.Rows[i][2].ToString(),
                            ICODIGO = dtConsulta.Rows[i][3].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i][4].ToString(),
                            IDESTINO = dtConsulta.Rows[i][5].ToString(),
                            IHORASALIDA = Convert.ToDateTime(dtConsulta.Rows[i][6].ToString()).ToString("HH:mm"),
                            IESTADO = dtConsulta.Rows[i][7].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
