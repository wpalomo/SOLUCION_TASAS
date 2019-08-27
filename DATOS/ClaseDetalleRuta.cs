using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;

namespace DATOS
{
    public class ClaseDetalleRuta
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE LOS DETALLES DE RUTA
        public List<ENTDetalleRuta> listarDetalleRuta(ENTDetalleRuta dato)
        {
            List<ENTDetalleRuta> coleccion = new List<ENTDetalleRuta>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTDetalleRuta todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTDetalleRuta()
                        {
                            IIDDETALLERUTA = dtConsulta.Rows[i][0].ToString(),
                            IIDTERMINAL = dtConsulta.Rows[i][1].ToString(),
                            IIDPRODUCTO = dtConsulta.Rows[i][2].ToString(),
                            ICODIGO = dtConsulta.Rows[i][3].ToString(),
                            IORIGEN = dtConsulta.Rows[i][4].ToString(),
                            IDESTINO = dtConsulta.Rows[i][5].ToString(),
                            IVALOR = dtConsulta.Rows[i][6].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
