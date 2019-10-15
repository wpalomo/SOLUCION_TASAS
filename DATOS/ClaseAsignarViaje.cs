using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;


namespace DATOS
{
    public class ClaseAsignarViaje
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE VIAJES
        public List<ENTViajes> listarViajes(ENTViajes dato)
        {
            List<ENTViajes> coleccion = new List<ENTViajes>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTViajes todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTViajes()
                        {
                            INUMERO = (i + 1).ToString(),
                            IIDPROGRAMACION = dtConsulta.Rows[i][0].ToString(),
                            INUMEROVIAJE = dtConsulta.Rows[i][1].ToString(),
                            IFECHAVIAJE = Convert.ToDateTime(dtConsulta.Rows[i][2].ToString()).ToString("dd/MM/yyyy"),
                            IVEHICULO = dtConsulta.Rows[i][3].ToString(),
                            IRUTA = dtConsulta.Rows[i][4].ToString(),
                            IHORASALIDA = Convert.ToDateTime(dtConsulta.Rows[i][5].ToString()).ToString("HH:mm"),
                            IASIENTOSOCUPADOS = dtConsulta.Rows[i][6].ToString(),
                            IDIA = Convert.ToDateTime(dtConsulta.Rows[i][2].ToString()).ToString("dddd").ToUpper(),
                            ITIPOVIAJE = dtConsulta.Rows[i][7].ToString(),
                            IESTADOVIAJE = dtConsulta.Rows[i][8].ToString(),
                            ICHOFER = dtConsulta.Rows[i][9].ToString(),
                            IASISTENTE = dtConsulta.Rows[i][10].ToString(),
                            IANDEN = dtConsulta.Rows[i][11].ToString(),
                            IIDVEHICULO = dtConsulta.Rows[i][12].ToString(),
                            IIDPUEBLO = dtConsulta.Rows[i][13].ToString(),
                            IIDREEMPLAZO = dtConsulta.Rows[i][14].ToString(),
                            IIDPUEBLOORIGEN = dtConsulta.Rows[i][17].ToString(),
                            IIDPUEBLODESTINO = dtConsulta.Rows[i][18].ToString(),
                            ICOBROADMINISTRATIVO = dtConsulta.Rows[i][19].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }

    }
}
