using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NEGOCIO;
using System.Data;

namespace Solution_CTT
{
    public partial class frmCambiarPassword : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;

        DataTable dtConsulta;

        bool bRespuesta;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarInformacion();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA CARGAR LOS DATOS DEL USUARIO
        private void cargarInformacion()
        {
            try
            {
                sSql = "";
                sSql += "select O.usuario, O.claveacceso," + Environment.NewLine;
                sSql += "ltrim(TP.apellidos + ' ' + isnull(TP.nombres, '')) oficinista" + Environment.NewLine;
                sSql += "from ctt_oficinista O LEFT OUTER JOIN" + Environment.NewLine;
                sSql += "tp_personas TP ON TP.id_persona = O.id_persona" + Environment.NewLine;
                sSql += "and TP.estado = 'A'" + Environment.NewLine;
                sSql += "and O.estado = 'A'" + Environment.NewLine;
                sSql += "where id_ctt_oficinista = " + Convert.ToInt32(Session["idUsuario"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    txtUsuario.Text = dtConsulta.Rows[0][0].ToString();
                    Session["claveRecuperada"] = dtConsulta.Rows[0][1].ToString();
                    txtPersona.Text = dtConsulta.Rows[0][2].ToString();
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

        //FUNCION PARA ACTUALIZAR LA CONTRASEÑA
        private void actualizarPassword()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    lblMensajeError.Text = "No se pudo iniciar la transacción para guardar los registros.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal1').modal('show');</script>", false);
                    return;
                }

                sSql = "";
                sSql += "update ctt_oficinista set" + Environment.NewLine;
                sSql += "claveacceso = '" + txtPasswordConfirmar.Text + "'" + Environment.NewLine;
                sSql += "where id_ctt_oficinista = " + Convert.ToInt32(Session["idUsuario"].ToString());


                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    conexionM.reversaTransaccion();
                    return;
                }

                conexionM.terminaTransaccion();

                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Contraseña modificada éxitosamente.', 'success');", true);
                cargarInformacion();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        protected void btnCambiar_Click(object sender, EventArgs e)
        {
            if (txtPasswordActual.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Info.!', 'Favor ingrese la contraseña actual.', 'success');", true);
                txtPasswordActual.Focus();
            }

            else if (txtPasswordNueva.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Info.!', 'Favor ingrese la nueva contraseña.', 'success');", true);
                txtPasswordNueva.Focus();
            }

            else if (txtPasswordConfirmar.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Info.!', 'Favor ingrese la contraseña para confirmar.', 'success');", true);
                txtPasswordConfirmar.Focus();
            }

            else if (txtPasswordActual.Text != Session["claveRecuperada"].ToString())
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Info.!', 'La contraseña actual ingresada no coincide con el registro en la base de datos.', 'success');", true);
                txtPasswordActual.Text = "";
                txtPasswordActual.Focus();
            }

            else if (txtPasswordNueva.Text != txtPasswordConfirmar.Text)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Info.!', 'La contraseña nueva no coincide con la contraseña de confirmación.', 'success');", true);
                txtPasswordNueva.Text = "";
                txtPasswordConfirmar.Text = "";
                txtPasswordNueva.Focus();
            }

            else
            {
                actualizarPassword();
            }
        }
    }
}