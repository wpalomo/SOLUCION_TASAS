using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;

namespace DATOS
{
    public class ClaseCorteCaja
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE FRECUENCIAS 
        public List<ENTCorteCaja> listarCorteCaja(ENTCorteCaja dato)
        {
            List<ENTCorteCaja> coleccion = new List<ENTCorteCaja>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTCorteCaja todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTCorteCaja()
                        {
                            INUMERO = (i + 1).ToString(),
                            IHORA = Convert.ToDateTime(dtConsulta.Rows[i][0].ToString()).ToString("HH:mm"),
                            IFECHA = Convert.ToDateTime(dtConsulta.Rows[i][1].ToString()).ToString("dd/MM/yyyy"),
                            IVEHICULO = dtConsulta.Rows[i][2].ToString(),
                            IRUTA = dtConsulta.Rows[i][3].ToString(),
                            ICUENTA = dtConsulta.Rows[i][4].ToString(),
                            IVIAJE = dtConsulta.Rows[i][5].ToString(),
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
