using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseTerminal
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;


        // CREAMOS LA CLASE DE BUSQUEDA PARA TERMINALES
        public List<ENTTerminal> listarTerminales(ENTTerminal dato)
        {
            List<ENTTerminal> coleccion = new List<ENTTerminal>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTTerminal todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTTerminal()
                        {
                            IIDTERMINAL = dtConsulta.Rows[i][0].ToString(),
                            IIDLOCALIDAD = dtConsulta.Rows[i][1].ToString(),
                            INOMBRELOCALIDAD = dtConsulta.Rows[i][2].ToString(),
                            IIDPROVINCIA = dtConsulta.Rows[i][3].ToString(),
                            IPROVINCIA = dtConsulta.Rows[i][4].ToString(),
                            ICODIGO = dtConsulta.Rows[i][5].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i][6].ToString(),
                            IDIRECCION = dtConsulta.Rows[i][7].ToString(),
                            IESTADO = dtConsulta.Rows[i][8].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
