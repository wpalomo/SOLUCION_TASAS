using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseReporteCobroRetenciones
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;

        // CREAMOS LA CLASE DE REPORTE DE COBRO DE RETENCIONES
        public List<ENTReporteCobroRetenciones> listarRetenciones(ENTReporteCobroRetenciones dato)
        {
            List<ENTReporteCobroRetenciones> coleccion = new List<ENTReporteCobroRetenciones>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTReporteCobroRetenciones todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTReporteCobroRetenciones()
                        {
                            INUMERO = (i+1).ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i][4].ToString(),
                            IVEHICULO = dtConsulta.Rows[i][8].ToString(),
                            IFECHA = Convert.ToDateTime(dtConsulta.Rows[i][5].ToString()).ToString("dd/MM/yyyy"),
                            IHORA = Convert.ToDateTime(dtConsulta.Rows[i][6].ToString()).ToString("HH:mm"),
                            IJORNADA = dtConsulta.Rows[i][7].ToString(),
                            IVALOR = (Convert.ToDouble(dtConsulta.Rows[i][9].ToString()) * (Convert.ToDouble(dtConsulta.Rows[i][10].ToString()) - Convert.ToDouble(dtConsulta.Rows[i][11].ToString()) + Convert.ToDouble(dtConsulta.Rows[i][12].ToString()))).ToString("N2"),
                            IUSUARIO = dtConsulta.Rows[i]["oficinista"].ToString().ToUpper()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
