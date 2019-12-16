using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NEGOCIO;
using ENTIDADES;

namespace Solution_CTT
{
    public partial class frmIngresarTokenPendiente : System.Web.UI.Page
    {
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorConexion conexionM = new manejadorConexion();

        Clases_Tasa_Usuario.ClaseValidarToken validarToken = new Clases_Tasa_Usuario.ClaseValidarToken();

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

            Session["modulo"] = "MÓDULO PARA INGRESAR UN TOKEN SIN SINCRONIZAR - DEVESOFFT";

            if (!IsPostBack)
            {
                consultarParametros();
            }
        }

        #region FUNCIONES DEL USUARIO

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
                    Session["id_ctt_tasa_parametro_IT"] = dtConsulta.Rows[0]["id_ctt_tasa_parametro"].ToString();
                    Session["tasa_terminal_IT"] = dtConsulta.Rows[0]["id_ctt_tasa_terminal"].ToString();
                    Session["tasa_oficina_IT"] = dtConsulta.Rows[0]["id_oficina"].ToString();
                    Session["tasa_cooperativa_IT"] = dtConsulta.Rows[0]["id_cooperativa"].ToString();
                    Session["servidor_pruebas_IT"] = dtConsulta.Rows[0]["servidor_pruebas"].ToString();
                    Session["servidor_produccion_IT"] = dtConsulta.Rows[0]["servidor_produccion"].ToString();
                    Session["credenciales_IT"] = dtConsulta.Rows[0]["webservice_tasa_credenciales"].ToString();
                    Session["emision_IT"] = dtConsulta.Rows[0]["emision"].ToString();

                    if (dtConsulta.Rows[0]["id_ctt_tasa_terminal"].ToString() == "1")
                    {
                        txtTerminal.Text = "QUITUMBE";
                    }

                    else
                    {
                        txtTerminal.Text = "CARCELÉN";
                    }

                    if (dtConsulta.Rows[0]["emision"].ToString() == "0")
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA INSERTAR UN TOKEN
        private void insertarRegistro()
        {
            try
            {
                if (validarToken.insertarToken(txtTokenGenerado.Text.Trim(), Convert.ToInt32(txtCantidad.Text.Trim()), Convert.ToInt32(Session["emision_IT"].ToString()), sDatosMaximo, Convert.ToInt32(Session["idUsuario"].ToString()), 0) == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo ingresar el token al sistema. Comuníquese con el admisnitrador.', 'danger');", true);
                    return;
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'El token se ha registrado éxitosamente. La sincronización se realizará de forma automática.', 'info');", true);
                limpiar();
                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            consultarParametros();
            txtCantidad.Text = "";
            txtTokenGenerado.Text = "";
        }

        //FUNCION PARA CONTAR O BUSCAR EL TOKEN 
        private int contarToken()
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_tasa_token" + Environment.NewLine;
                sSql += "where token = '" + txtTokenGenerado.Text.Trim() + "'" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    return dtConsulta.Rows.Count;
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return -1;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return -1;
            }
        }

        #endregion

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtTokenGenerado.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el número del token. Verifique que haya ingresado 5 dígitos.', 'info');", true);
                return;
            }

            if (txtCantidad.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese la cantidad de tasas de usuario que contiene el token.', 'info');", true);
                return;
            }

            int iContarDigitos = txtTokenGenerado.Text.Trim().Length;

            if (iContarDigitos != 5)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El número del token debe constar de 5 dígitos.', 'info');", true);
                txtTokenGenerado.Text = "";
                return;
            }

            int iCantidad_P = contarToken();

            if (iCantidad_P == -1)
            {
                return;
            }

            if (iCantidad_P > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'El token ya se encuentra registrado en el sistema. Favor verifique.', 'info');", true);
                return;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModalConfirmacion').modal('show');</script>", false);
        }

        protected void btnAceptarCerrar_Click(object sender, EventArgs e)
        {
            insertarRegistro();
        }
    }
}