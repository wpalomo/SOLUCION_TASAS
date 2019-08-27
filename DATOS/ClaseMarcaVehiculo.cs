using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;
using System.Data.SqlClient;

namespace DATOS
{
    public class ClaseMarcaVehiculo
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;
        

        // CREAMOS LA CLASE DE BUSQUEDA PARA CARGOS
        public List<ENTMarcaVehiculo> listarMarcaVehiculo(ENTMarcaVehiculo dato)
        {
            List<ENTMarcaVehiculo> coleccion = new List<ENTMarcaVehiculo>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.SSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTMarcaVehiculo todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTMarcaVehiculo()
                        {
                            IIDMARCAVEHICULO = dtConsulta.Rows[i][0].ToString(),
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
