using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseGenerarXML
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE BUSQUEDA DE FACTURAS ELECTRONICAS
        public List<ENTGenerarXML> listarFacturasEmitidas(ENTGenerarXML dato)
        {
            List<ENTGenerarXML> coleccion = new List<ENTGenerarXML>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTGenerarXML todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTGenerarXML()
                        {
                            INUMERO = (i + 1).ToString(),
                            IIDFACTURA = dtConsulta.Rows[i][0].ToString(),
                            IFECHAFACTURA = Convert.ToDateTime(dtConsulta.Rows[i][1].ToString()).ToString("dd-MM-yyyy"),
                            ICLIENTE = dtConsulta.Rows[i][2].ToString(),
                            IFACTURAEMITIDA = dtConsulta.Rows[i][3].ToString().Trim() + "-" + dtConsulta.Rows[i][4].ToString() + "-" + dtConsulta.Rows[i][5].ToString().Trim().PadLeft(9, '0'),
                            ICLAVEACCESO = dtConsulta.Rows[i][6].ToString(),
                            IIDLOCALIDAD = dtConsulta.Rows[i][7].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
