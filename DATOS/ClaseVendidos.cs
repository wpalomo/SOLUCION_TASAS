using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseVendidos
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE BOLETOS VENDIDOS
        public List<ENTVendidos> listarVendidos(ENTVendidos dato)
        {
            List<ENTVendidos> coleccion = new List<ENTVendidos>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTVendidos todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTVendidos()
                        {
                            IIDPEDIDO= dtConsulta.Rows[i][0].ToString(),
                            IIDDETPEDIDO = dtConsulta.Rows[i][1].ToString(),                            
                            IIDPASAJERO = dtConsulta.Rows[i][2].ToString(),
                            IIDASIENTO = dtConsulta.Rows[i][3].ToString(),
                            IIDPUEBLO = dtConsulta.Rows[i][4].ToString(),
                            IIDPROGRAMACION = dtConsulta.Rows[i][5].ToString(),
                            IIDPRODUCTO = dtConsulta.Rows[i][6].ToString(),
                            INUMEROASIENTO = dtConsulta.Rows[i][7].ToString(),
                            IIDENTIFICACIONPASAJERO = dtConsulta.Rows[i][8].ToString(),
                            INOMBREPASAJERO = (dtConsulta.Rows[i][9].ToString() + " " + dtConsulta.Rows[i][10].ToString()).Trim().ToUpper(),
                            INOMBREPRODUCTO = dtConsulta.Rows[i][11].ToString(),
                            IPRECIOUNITARIO = dtConsulta.Rows[i][12].ToString(),
                            IDESCUENTO = dtConsulta.Rows[i][13].ToString(),
                            IIVA = dtConsulta.Rows[i][14].ToString(),
                            ITOTAL = dtConsulta.Rows[i][15].ToString(),
                            IIDTIPOCLIENTE = dtConsulta.Rows[i][16].ToString(),
                            ITIPOCLIENTE = dtConsulta.Rows[i][17].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
