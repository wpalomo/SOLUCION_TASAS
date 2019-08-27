using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATOS;
using System.Data;
using System.Data.SqlClient;

namespace NEGOCIO
{
    public class manejadorConexion
    {
        Conexion conexionD = new Conexion();

        public void conectar()
        {
            conexionD.conectar();
        }

        public bool iniciarTransaccion()
        {
            return conexionD.iniciarTransaccion();
        }

        //FUNCION PARA TERMINAR UNA TRANSACCION
        public bool terminaTransaccion()
        {
            return conexionD.terminaTransaccion();
        }

        //FUNCION PARA ROLLBACK
        public bool reversaTransaccion()
        {
            return conexionD.reversaTransaccion();
        }

        public bool ejecutarInstruccionSQL(string sSql)
        {
            return conexionD.ejecutarInstruccionSQL(sSql);
        }

        public bool consultarRegistro(string sSql, DataTable dtConsulta)
        {
            return conexionD.consultarRegistro(sSql, dtConsulta);
        }

        public long sacarMaximo(string sTabla, string sCampo, string sCondicion, string[] sDatosMaximo)
        {
            return conexionD.sacarMaximo(sTabla, sCampo, sCondicion, sDatosMaximo);
        }

        public string esNulo()
        {
            return conexionD.esNulo();
        }

        public string bddConexion()
        {
            return conexionD.bddConexion();
        }

        public string valorDefecto(string valor, string valorDefecto)
        {
            return conexionD.valorDefecto(valor, valorDefecto);
        }

        //PARA DEVOLVER UN ADAPTADOR
        public SqlDataAdapter recuperaReportViewer(string sSql)
        {
            return conexionD.recuperaReportViewer(sSql);
        }

        public bool generaRIDE(DataTable dtConsulta, long numeroFactura)
        {
            return conexionD.generaRIDE(dtConsulta, numeroFactura);
        }

        //FUNCIONES DE LA BASE DE DATOS DE CLIENTES
        public bool consultarRegistroClientes(string sSql, DataTable dtConsulta)
        {
            return conexionD.consultarRegistroClientes(sSql, dtConsulta);
        }

        //FUNCION PARA INSERTAR UN REGISTRO CLIENTE
        public bool ejecutarInstruccionSQLCliente(string sSql)
        {
            return conexionD.ejecutarInstruccionSQLCliente(sSql);
        }
    }
}
