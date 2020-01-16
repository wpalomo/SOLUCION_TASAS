using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ENTIDADES;

namespace DATOS
{
    public class ClaseOficinista
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;
        
        // CREAMOS LA CLASE DE BUSQUEDA PARA OFICINISTAS
        public List<ENTOficinista> listarOficinista(ENTOficinista dato)
        {
            List<ENTOficinista> coleccion = new List<ENTOficinista>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTOficinista todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTOficinista()
                        {
                            IIDOFICINISTA = dtConsulta.Rows[i][0].ToString(),
                            IIDPERSONA = dtConsulta.Rows[i][1].ToString(),
                            ICODIGO = dtConsulta.Rows[i][2].ToString(),
                            INOMBRE = dtConsulta.Rows[i][3].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i][4].ToString(),
                            IUSUARIO = dtConsulta.Rows[i][5].ToString(),
                            IESTADO = dtConsulta.Rows[i][6].ToString(),
                            ICLAVE = dtConsulta.Rows[i][7].ToString(),
                            IPOSSECRET = dtConsulta.Rows[i]["pos_secret"].ToString(),
                            IUSUARIO_SMARTT = dtConsulta.Rows[i]["usuario_smartt"].ToString(),
                            IIPASSWORD_SMARTT = dtConsulta.Rows[i]["claveacceso_smartt"].ToString(),
                            IPERMISOS = dtConsulta.Rows[i]["privilegio"].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
