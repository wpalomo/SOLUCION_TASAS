using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseProgramacion
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;

        // CREAMOS LA CLASE DE BUSQUEDA DE PROGRAMACIONES
        public List<ENTProgramacion> listarProgramacion(ENTProgramacion dato)
        {
            List<ENTProgramacion> coleccion = new List<ENTProgramacion>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTProgramacion todos;

                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {

                        todos = new ENTProgramacion()
                        {
                            INUMERO = (i+1).ToString(),
                            IIDPROGRAMACION = dtConsulta.Rows[i][0].ToString(),
                            INUMEROVIAJE = dtConsulta.Rows[i][1].ToString(),
                            IFECHAVIAJE = Convert.ToDateTime(dtConsulta.Rows[i][2].ToString()).ToString("dd/MM/yyyy"),
                            ITRANSPORTE = dtConsulta.Rows[i][3].ToString(),
                            IRUTADESTINO = dtConsulta.Rows[i][4].ToString(),
                            IDIA = Convert.ToDateTime(dtConsulta.Rows[i][5].ToString()).ToString("dddd").ToUpper(),
                            IHORASALIDA = Convert.ToDateTime(dtConsulta.Rows[i][6].ToString()).ToString("HH:mm"),
                            INUMEROPASAJEROS = dtConsulta.Rows[i][7].ToString(),
                            IESTADOVIAJE = dtConsulta.Rows[i][8].ToString(),
                            IIDCHOFER = dtConsulta.Rows[i][9].ToString(),
                            IIDASISTENTE = dtConsulta.Rows[i][10].ToString(),
                            IIDVEHICULO = dtConsulta.Rows[i][11].ToString(),
                            IIDHORARIO = dtConsulta.Rows[i][12].ToString(),
                            IIDANDEN = dtConsulta.Rows[i][13].ToString(),
                            IIDTIPOSERVICIO = dtConsulta.Rows[i][14].ToString(),
                            IIDRUTA = dtConsulta.Rows[i][15].ToString(),
                            IESTADOSALIDA = dtConsulta.Rows[i][16].ToString(),
                            ICODIGO = dtConsulta.Rows[i][17].ToString(),
                            INOMBRECHOFER = dtConsulta.Rows[i][18].ToString(),
                            INOMBREASISTENTE = dtConsulta.Rows[i][19].ToString(),
                            INOMBRESERVICIO = dtConsulta.Rows[i][21].ToString(),
                            ICODIGOITINERARIO = dtConsulta.Rows[i][22].ToString(),
                            IIDITINERARIO = dtConsulta.Rows[i][23].ToString(),
                            IOCUPADOS = dtConsulta.Rows[i][24].ToString(),
                            ICOBROADMINISTATIVO = dtConsulta.Rows[i][25].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
