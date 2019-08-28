using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ENTIDADES;
using NEGOCIO;
using System.Data;

namespace Solution_CTT
{
    public partial class frmTasaAcompanante : System.Web.UI.Page
    {
        Clases.ClaseCrearTasaUsuario tasa = new Clases.ClaseCrearTasaUsuario();

        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sTasaUsuario;
        string sObjetoOficina;
        string sObjetoTasa;
        string sObjetoInfo;
        string sObjetoCliente;
        string sObjetoJson;
        string sUrlCredenciales;
        string sUrlEnvio;
        string sUrlAnula;
        string sRespuestaJson;
        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;

        bool bRespuesta;

        int iIdTasaRecuperado;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
                return;
            }

            sDatosMaximo[0] = Session["usuario"].ToString();
            sDatosMaximo[1] = Environment.MachineName.ToString();
            sDatosMaximo[2] = "A";

            Session["modulo"] = "MÓDULO DE IMPRESI\x00d3N DE TASAS DE ACOMPAÑANTE";

            if (!IsPostBack)
            {
                consultarParametros();
                consultarToken();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtTerminal.Text = "";
            txtAmbiente.Text = "";
            txtTokenGenerado.Text = "";
            txtSecuencial.Text = "";
            Session["token_respuesta"] = null;
            Session["secuencia_respuesta"] = null;
            consultarParametros();
            consultarToken();
        }

        //FUNCION PARA CONSULTAR LOS PARAMETROS DEL SERVIDOR
        private void consultarParametros()
        {
            try
            {
                sSql = "select * from ctt_tasa_parametros";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    Session["id_ctt_tasa_parametro"] = dtConsulta.Rows[0][0].ToString();

                    Session["tasa_terminal"] = dtConsulta.Rows[0][1].ToString();
                    Session["tasa_oficina"] = dtConsulta.Rows[0][2].ToString();
                    Session["tasa_cooperativa"] = dtConsulta.Rows[0][3].ToString();

                    Session["servidor_pruebas"] = dtConsulta.Rows[0][4].ToString();
                    Session["servidor_produccion"] = dtConsulta.Rows[0][5].ToString();
                    Session["credenciales"] = dtConsulta.Rows[0][6].ToString();
                    Session["emision"] = dtConsulta.Rows[0][9].ToString();

                    if (dtConsulta.Rows[0][1].ToString() == "1")
                    {
                        txtTerminal.Text = "QUITUMBE";
                    }

                    else
                    {
                        txtTerminal.Text = "CARCELÉN";
                    }

                    if (dtConsulta.Rows[0][9].ToString() == "0")
                    {
                        txtAmbiente.Text = "PRUEBAS";
                    }

                    else
                    {
                        txtAmbiente.Text = "PRODUCCIÓN";
                    }
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONSULTAR LOS DATOS DELTOKEN
        private bool consultarToken()
        {
            try
            {
                sSql = "";
                sSql += "select token, cuenta, id_ctt_tasa_token" + Environment.NewLine;
                sSql += "from ctt_tasa_token" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and estado_token = 'Abierta'" + Environment.NewLine;
                sSql += "and ambiente_token = " + Convert.ToInt32(Session["emision"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    Session["sToken"] = dtConsulta.Rows[0][0].ToString();
                    Session["sCuentaToken"] = dtConsulta.Rows[0][1].ToString().Trim().PadLeft(4, '0');
                    txtTokenGenerado.Text = dtConsulta.Rows[0][0].ToString();
                    txtSecuencial.Text = dtConsulta.Rows[0][1].ToString().Trim().PadLeft(4, '0');
                }

                else
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
        

        #endregion

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            tasa.tasa("1", sDatosMaximo);
            limpiar();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }
    }
}