using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseTipoVehiculo
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA PARA TIPO DE VEHICULO
        public List<ENTTipoVehiculo> listarTipoVehiculo(ENTTipoVehiculo dato)
        {
            List<ENTTipoVehiculo> coleccion = new List<ENTTipoVehiculo>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTTipoVehiculo todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTTipoVehiculo()
                        {
                            IIDTIPOVEHICULO = dtConsulta.Rows[i][0].ToString(),
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
