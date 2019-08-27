using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NEGOCIO;

namespace Solution_CTT.Clases_Tasa_Usuario
{
    public class ClaseValidarToken
    {
        manejadorConexion conexionM = new manejadorConexion();

        string[] sDatosMaximo = new string[5];

        string sSql;
        
        public bool insertarToken(string sToken_P, Int32 iCantidad_P, int iAmbiente_P, string[] sDatosMaximo_P, int iUsuario_P)
        {
            try
            {
                this.sDatosMaximo = sDatosMaximo_P;

                if (conexionM.iniciarTransaccion() == false)
                {
                    return false;
                }

                sSql = "";
                sSql += "insert into ctt_tasa_token (" + Environment.NewLine;
                sSql += "token, maximo_secuencial, cuenta, emitidos, anulados, estado_token," + Environment.NewLine;
                sSql += "fecha_generacion, ambiente_token, creado, validado, id_ctt_oficinista, estado," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "'" + sToken_P + "', " + iCantidad_P + ", 1, 0, 0, 'Abierta', GETDATE()," + Environment.NewLine;
                sSql += iAmbiente_P + ", 0, 1, " + iUsuario_P + ", 'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    conexionM.reversaTransaccion();
                    return false;
                }

                conexionM.terminaTransaccion();

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }
    }
}