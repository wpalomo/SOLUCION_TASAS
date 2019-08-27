using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseAnden
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA PARA ANDENES
        public List<ENTAnden> listarAnden(ENTAnden dato)
        {
            List<ENTAnden> coleccion = new List<ENTAnden>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTAnden todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTAnden()
                        {
                            IIDANDEN = dtConsulta.Rows[i].ItemArray[0].ToString(),
                            IIDTERMINAL = dtConsulta.Rows[i].ItemArray[1].ToString(),
                            ITERMINAL = dtConsulta.Rows[i].ItemArray[2].ToString(),
                            ICODIGO = dtConsulta.Rows[i].ItemArray[3].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i].ItemArray[4].ToString(),
                            INUMERACION = dtConsulta.Rows[i].ItemArray[5].ToString(),
                            IESTADO = dtConsulta.Rows[i].ItemArray[6].ToString(),
                            IPRINCIPAL = dtConsulta.Rows[i].ItemArray[7].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }

    }
}
