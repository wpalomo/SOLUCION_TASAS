using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;

namespace DATOS
{
    public class ClaseVehiculoPropietario
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;


        // CREAMOS LA CLASE DE BUSQUEDA PARA CARGOS
        public List<ENTVehiculoPropietario> listarVehiculoPropietario(ENTVehiculoPropietario dato)
        {
            List<ENTVehiculoPropietario> coleccion = new List<ENTVehiculoPropietario>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTVehiculoPropietario todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTVehiculoPropietario()
                        {
                            IIDVEHICULOPROPIETARIO = dtConsulta.Rows[i][0].ToString(),
                            IIDPERSONA = dtConsulta.Rows[i][1].ToString(),
                            IIDVEHICULO = dtConsulta.Rows[i][2].ToString(),
                            ICODIGO = dtConsulta.Rows[i][3].ToString(),
                            INOMBRE = dtConsulta.Rows[i][4].ToString(),
                            IVEHICULO = dtConsulta.Rows[i][5].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i][6].ToString(),
                            IESTADO = dtConsulta.Rows[i][7].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
