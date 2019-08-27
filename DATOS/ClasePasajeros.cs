using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClasePasajeros
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA PARA PASAJEROS
        public List<ENTPasajeros> listarPasajeros(ENTPasajeros dato)
        {
            List<ENTPasajeros> coleccion = new List<ENTPasajeros>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTPasajeros todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTPasajeros()
                        {
                            IIDCLIENTEFILTRO= dtConsulta.Rows[i][0].ToString(),
                            IIDENTIFICACIONFILTRO = dtConsulta.Rows[i][1].ToString(),
                            ICLIENTEFILTRO = dtConsulta.Rows[i][2].ToString(),
                            IFECHAFILTRO = Convert.ToDateTime(dtConsulta.Rows[i][3].ToString()).ToString("dd-MM-yyyy")
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
