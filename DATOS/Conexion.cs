using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DATOS
{
    public class Conexion
    {
        string path = "C:\\palatium\\config.ini";
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        ClaseVariables variables = new ClaseVariables();

        bool bRespuesta;

        long iMaximo;

        SqlDataAdapter da;

        public void conectar()
        {
            if (File.Exists(path))
            {
                if (conexion.lecturaConfiguracion(path) == true)
                {
                    //variables.iIdEmpresa = Convert.ToInt32(conexion.id_Empresa);
                    //variables.iCgEmpresa = Convert.ToInt32(conexion.Cg_Empresa);
                    //variables.iIdLocalidad = Convert.ToInt32(conexion.id_Localidad);
                    //variables.iCgMotivoDespacho = Convert.ToInt32(conexion.Motivo_Despacho);
                }
            }
        }

        //FUNCION PARA INICIAR UNA TRANSACCION
        public bool iniciarTransaccion()
        {
            try
            {
                if (conexion.GFun_Lo_Maneja_Transaccion(variables.G_INICIA_TRANSACCION) == false)
                {
                    return false;
                }

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        //FUNCION PARA TERMINAR UNA TRANSACCION
        public bool terminaTransaccion()
        {
            try
            {
                conexion.GFun_Lo_Maneja_Transaccion(variables.G_TERMINA_TRANSACCION);

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        //FUNCION PARA ROLLBACK
        public bool reversaTransaccion()
        {
            try
            {
                if (conexion.GFun_Lo_Maneja_Transaccion(variables.G_REVERSA_TRANSACCION) == false)
                {
                    return false;
                }

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        //FUNCION PARA INSERTAR UN REGISTRO
        public bool ejecutarInstruccionSQL(string sSql)
        {
            try
            {
                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    goto reversa;
                }

                return true;
            }

            catch(Exception)
            {
                return false;
            }

        reversa:
            {
                conexion.GFun_Lo_Maneja_Transaccion(variables.G_REVERSA_TRANSACCION);
                return false;
            }
        }

        public bool consultarRegistro(string sSql, DataTable dtConsulta)
        {
            try
            {
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {                    
                    return true;
                }

                else
                {
                    return false;
                }
            }

            catch(Exception )
            {
                return false;
            }
        }

        public long sacarMaximo(string sTabla, string sCampo, string sCondicion, string[]sDatosMaximo)
        {
            try
            {
                iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, sCondicion, sDatosMaximo);

                return iMaximo;
            }

            catch(Exception)
            {
                return -1;
            }
        }

        public string esNulo()
        {
            try
            {
                string sValor = conexion.GFun_St_esnulo();
                return sValor;
            }

            catch (Exception ex)
            {
                return "";
            }
        }

        public string bddConexion()
        {
            try
            {
                string sValor = conexion.GFun_St_Conexion();
                return sValor;
            }

            catch (Exception ex)
            {
                return "";
            }
        }

        public string valorDefecto(string valor, string valorDefecto)
        {
            try
            {
                string sValor = conexion.GFun_Va_Valor_Defecto(valor, valorDefecto);
                return sValor;
            }

            catch (Exception ex)
            {
                return "";
            }
        }

        //PARA DEVOLVER UN ADAPTADOR
        public SqlDataAdapter recuperaReportViewer(string sSql)
        {
            try
            {
                da = new SqlDataAdapter();
                return da = conexion.GFun_Lo_Recupera_Registros_Reporte(sSql);
            }

            catch (Exception ex)
            {
                return da;
            }
        }

        public bool generaRIDE(DataTable dtConsulta, long numeroFactura)
        {
            try
            {
                bRespuesta = conexion.GFun_Lo_Genera_Ride(dtConsulta, numeroFactura);

                if (bRespuesta == true)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                return false;
            }
        }

        //FUNCIONES DE LA BASE DE DATOS DE CLIENTES
        public bool consultarRegistroClientes(string sSql, DataTable dtConsulta)
        {
            try
            {
                bRespuesta = conexion.GFun_Lo_Busca_Registro_Clientes(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                return false;
            }
        }

        //FUNCION PARA INSERTAR UN REGISTRO CLIENTE
        public bool ejecutarInstruccionSQLCliente(string sSql)
        {
            try
            {
                if (!conexion.GFun_Lo_Ejecuta_SQL_Clientes(sSql))
                {
                    goto reversa;
                }

                return true;
            }

            catch (Exception)
            {
                return false;
            }

        reversa:
            {
                conexion.GFun_Lo_Maneja_Transaccion(variables.G_REVERSA_TRANSACCION);
                return false;
            }
        }
    }
}
