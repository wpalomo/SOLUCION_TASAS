using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;

namespace DATOS
{
    public class ClaseDisco
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;


        // CREAMOS LA CLASE DE BUSQUEDA DE DISCOS
        public List<ENTDisco> listarDisco(ENTDisco dato)
        {
            List<ENTDisco> coleccion = new List<ENTDisco>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTDisco todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTDisco()
                        {
                            IIDDISCO = dtConsulta.Rows[i][0].ToString(),
                            ICODIGO = dtConsulta.Rows[i][1].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i][2].ToString(),
                            IESTADO = dtConsulta.Rows[i][3].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }   
    }
}
