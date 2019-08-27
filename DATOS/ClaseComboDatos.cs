using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;

namespace DATOS
{
    public class ClaseComboDatos
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;


        // CREAMOS LA CLASE PARA LLENAR LOS COMBOBOX
        public List<ENTComboDatos> listarCombo(ENTComboDatos dato)
        {
            List<ENTComboDatos> coleccion = new List<ENTComboDatos>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTComboDatos todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTComboDatos()
                        {
                            IID = dtConsulta.Rows[i].ItemArray[0].ToString(),
                            IDATO = dtConsulta.Rows[i].ItemArray[1].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }     
    }
}
