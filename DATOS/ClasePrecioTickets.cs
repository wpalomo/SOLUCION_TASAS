using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClasePrecioTickets
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE PRECIOS DE TICKETS
        public List<ENTPrecioTickets> listarPrecioTickets(ENTPrecioTickets dato)
        {
            List<ENTPrecioTickets> coleccion = new List<ENTPrecioTickets>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTPrecioTickets todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTPrecioTickets()
                        {
                            IIDPRODUCTO = dtConsulta.Rows[i][0].ToString(),
                            IIDPRODUCTOPADRE = dtConsulta.Rows[i][1].ToString(),
                            IPAGAIVA = dtConsulta.Rows[i][2].ToString(),
                            ICGUNIDAD = dtConsulta.Rows[i][3].ToString(),
                            IAPLICARETENCION = dtConsulta.Rows[i][4].ToString(),
                            IPORCENTAJERETENCION = dtConsulta.Rows[i][5].ToString(),
                            ICODIGO = dtConsulta.Rows[i][6].ToString(),
                            INOMBREPRODUCTO = dtConsulta.Rows[i][7].ToString(),
                            IVALOR = dtConsulta.Rows[i][8].ToString(),
                            IAPLICAPAGO = dtConsulta.Rows[i][9].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
