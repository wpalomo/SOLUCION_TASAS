using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;

namespace DATOS
{
    public class ClaseVehiculo
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE VEHICULOS
        public List<ENTVehiculo> listarVehiculos(ENTVehiculo dato)
        {
            List<ENTVehiculo> coleccion = new List<ENTVehiculo>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTVehiculo todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTVehiculo()
                        {
                            IIDVEHICULO = dtConsulta.Rows[i][0].ToString(),
                            IIDTIPOVEHICULO = dtConsulta.Rows[i][1].ToString(),
                            ITIPOVEHICULO = dtConsulta.Rows[i][2].ToString(),
                            IIDMARCAVEHICULO = dtConsulta.Rows[i][3].ToString(),
                            IMARCAVEHICULO = dtConsulta.Rows[i][4].ToString(),
                            IIDMODELOVEHICULO = dtConsulta.Rows[i][5].ToString(),
                            IMODELOVEHICULO = dtConsulta.Rows[i][6].ToString(),
                            IIDTIPOBUS = dtConsulta.Rows[i][7].ToString(),
                            ITIPOBUS = dtConsulta.Rows[i][8].ToString(),
                            IIDDISCO = dtConsulta.Rows[i][9].ToString(),
                            IDISCO = dtConsulta.Rows[i][10].ToString(),
                            IPLACA = dtConsulta.Rows[i][11].ToString(),
                            ICHASIS = dtConsulta.Rows[i][12].ToString(),
                            IMOTOR = dtConsulta.Rows[i][13].ToString(),
                            IANIOPRODUCCION = dtConsulta.Rows[i][14].ToString(),
                            IPAISORIGEN = dtConsulta.Rows[i][15].ToString(),
                            ICILINDRAJE = dtConsulta.Rows[i][16].ToString(),
                            IPESO = dtConsulta.Rows[i][17].ToString(),
                            INUMEROPASAJEROS = dtConsulta.Rows[i][18].ToString(),
                            IESTADO = dtConsulta.Rows[i][19].ToString(),
                            IIDFORMATOASIENTO = dtConsulta.Rows[i][20].ToString(),
                            IACTIVO = dtConsulta.Rows[i]["is_active"].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
