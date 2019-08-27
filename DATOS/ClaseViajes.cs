﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;
using System.Data;

namespace DATOS
{
    public class ClaseViajes
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool bRespuesta;
        DataTable dtConsulta;
        int i;


        // CREAMOS LA CLASE DE BUSQUEDA PARA CARGOS
        public List<ENTModoViaje> listarModoViaje(ENTModoViaje dato)
        {
            List<ENTModoViaje> coleccion = new List<ENTModoViaje>();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, dato.ISSQL);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    ENTModoViaje todos;

                    for (i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        todos = new ENTModoViaje()
                        {
                            IIDMODOVIAJE = dtConsulta.Rows[i][0].ToString(),
                            ICODIGO = dtConsulta.Rows[i][1].ToString(),
                            IDESCRIPCION = dtConsulta.Rows[i][2].ToString(),
                            IEDITARPRECIO = dtConsulta.Rows[i][3].ToString(),
                            IESTADO = dtConsulta.Rows[i][4].ToString()
                        };
                        coleccion.Add(todos);
                    }
                }
            }

            return coleccion;
        }
    }
}
