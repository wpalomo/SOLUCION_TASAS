using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseFacturasItinerario
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE BOLETOS VENDIDOS
        public List<ENTFacturasItinerario> listarFacturasItinerario(ENTFacturasItinerario dato)
        {
            List<ENTFacturasItinerario> coleccion = new List<ENTFacturasItinerario>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTFacturasItinerario todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTFacturasItinerario()
                        {
                            IIDPEDIDO = dtConsulta.Rows[i].ItemArray[0].ToString(),
                            IIDPROGRAMACION = dtConsulta.Rows[i].ItemArray[1].ToString(),
                            IIDFACTURA = dtConsulta.Rows[i].ItemArray[2].ToString(),
                            IIDPERSONA = dtConsulta.Rows[i].ItemArray[3].ToString(),
                            IIDENTIFICACION = dtConsulta.Rows[i].ItemArray[4].ToString(),
                            ICLIENTE = dtConsulta.Rows[i].ItemArray[5].ToString(),
                            IFACTURA = dtConsulta.Rows[i].ItemArray[6].ToString() + "-" + dtConsulta.Rows[i].ItemArray[7].ToString() + "-" + dtConsulta.Rows[i].ItemArray[8].ToString().PadLeft(9, '0'),
                            IFECHAVIAJE = Convert.ToDateTime(dtConsulta.Rows[i].ItemArray[9].ToString()).ToString("dd-MM-yyyy"),
                            IIDCTTPUEBLOORIGEN = dtConsulta.Rows[i][10].ToString(),
                            IIDCTTPUEBLODESTNO = dtConsulta.Rows[i][11].ToString(),
                            IIDTASASMARTT = dtConsulta.Rows[i]["id_tasa_smartt"].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
