using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseExtras
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE EXTRAS
        public List<ENTExtras> listarExtra(ENTExtras dato)
        {
            List<ENTExtras> coleccion = new List<ENTExtras>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTExtras todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTExtras()
                        {
                            INUMERO = (i+1).ToString(),
                            IIDPRODUCTO = dtConsulta.Rows[i][0].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i][1].ToString(),
                            IVALOR = dtConsulta.Rows[i][2].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
