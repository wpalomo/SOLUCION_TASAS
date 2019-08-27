using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClasePagosPendientes
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE PAGOS PENDIENTES
        public List<ENTPagosPendientes> listarPagosPendientes(ENTPagosPendientes dato)
        {
            List<ENTPagosPendientes> coleccion = new List<ENTPagosPendientes>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTPagosPendientes todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTPagosPendientes()
                        {
                            IIDPEDIDO = dtConsulta.Rows[i][0].ToString(),
                            INUMEROVIAJE = dtConsulta.Rows[i][1].ToString(),
                            IFECHAVIAJE = Convert.ToDateTime(dtConsulta.Rows[i][2].ToString()).ToString("dd/MM/yyyy"),
                            IHORAVIAJE = Convert.ToDateTime(dtConsulta.Rows[i][3].ToString()).ToString("HH:mm"),
                            IVALOR = dtConsulta.Rows[i][4].ToString(),
                            IPRECIO = dtConsulta.Rows[i][6].ToString(),
                            IVEHICULO = dtConsulta.Rows[i][8].ToString(),
                            IPROPIETARIO = dtConsulta.Rows[i][9].ToString(),
                            IRUTA = dtConsulta.Rows[i][10].ToString(),
                            IESTADODCTO = dtConsulta.Rows[i][11].ToString(),
                            IIDPERSONA = dtConsulta.Rows[i][12].ToString(),
                            IPRECIOUNITARIO = dtConsulta.Rows[i][13].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
