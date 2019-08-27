using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;

namespace DATOS
{
    public class ClaseDirectorio
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // LISTAR
        public List<ENTDirectorio> listar(ENTDirectorio dato)
        {
            List<ENTDirectorio> coleccion = new List<ENTDirectorio>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTDirectorio todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTDirectorio()
                        {
                            INUMERO = (i + 1).ToString(),
                            IID_DIRECTORIO = dtConsulta.Rows[i][0].ToString(),
                            IID_TIPO_COMPROBANTE = dtConsulta.Rows[i][1].ToString(),
                            INOMBRE_TIPO_COMPROBANTE = dtConsulta.Rows[i][2].ToString(),
                            IORDEN = dtConsulta.Rows[i][3].ToString(),
                            ICODIGO = dtConsulta.Rows[i][4].ToString(),
                            INOMBRES = dtConsulta.Rows[i][5].ToString(),
                            IESTADO = dtConsulta.Rows[i][6].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
