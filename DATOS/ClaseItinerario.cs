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
                            IIDITINERARIO = dtConsulta.Rows[i]["id_ctt_itinerario"].ToString(),
                            IIDRUTA = dtConsulta.Rows[i]["id_ctt_ruta"].ToString(),
                            IIDHORARIO = dtConsulta.Rows[i]["id_ctt_horario"].ToString(),
                            ICODIGO = dtConsulta.Rows[i]["codigo"].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i]["descripcion"].ToString(),
                            IDESTINO = dtConsulta.Rows[i]["destino"].ToString(),
                            IHORASALIDA = Convert.ToDateTime(dtConsulta.Rows[i]["hora_salida"].ToString()).ToString("HH:mm"),
                            IESTADO = dtConsulta.Rows[i]["estado"].ToString(),
                            IVIA = dtConsulta.Rows[i]["via"].ToString(),
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
