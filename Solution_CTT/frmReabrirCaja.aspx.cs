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
    public partial class frmReabrirCaja : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sFecha;
        string sHora;
        string sTabla;
        string sCampo;
        string sEstadoCaja;
        string sFechaApertura;
        string sHoraApertura;
        string sUsuarioApertura;
        string sJornadaApertura;
        string sSaldoInicial;
        string sObervaciones;
        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;

        bool bRespuesta;

        int iIdCierreCaja;
        int iJornada;
        int iIdCierreCajaApertura;
        int iIdJornadaApertura;

        long iMaximo;

        DateTime fechaSistema;
        DateTime fechaCaja;

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

            Session["modulo"] = "MÓDULO DE REAPERTURA DE CAJA";

            if (!IsPostBack)
            {
                //cargarParametros();
                recuperarCaja();
            }
        }

        #region FUNCIONES PARA RECUPERAR INFORMACION DE LA CAJA

        //FUNCION PARA RECUPERAR EL ESTADO DE LA ULTIMA CAJA
        private void recuperarCaja()
        {
            try
            {
                sSql = "";
                sSql += "select top 1 * from ctt_vw_reabrir_ultima_caja" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Session["id_pueblo"].ToString().Trim() + Environment.NewLine;
                sSql += "order by id_ctt_cierre_caja desc";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                    return;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Info.!', 'No existen registros de cajas en el sistema.', 'success');", true);
                    return;
                }

                txtFechaApertura.Text = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_apertura"].ToString().Trim()).ToString("dd/MM/yyyy");
                txtHoraApertura.Text = dtConsulta.Rows[0]["hora_apertura"].ToString().Trim();
                txtOficinistaApertura.Text = dtConsulta.Rows[0]["oficinista"].ToString().Trim().ToUpper();               

                sEstadoCaja = dtConsulta.Rows[0]["estado_cierre_caja"].ToString().Trim().ToUpper();
                txtEstado.Text = dtConsulta.Rows[0]["estado_cierre_caja"].ToString().Trim().ToUpper();
                txtJornada.Text = dtConsulta.Rows[0]["jornada"].ToString().Trim().ToUpper();
                txtOficina.Text = dtConsulta.Rows[0]["pueblo"].ToString().Trim().ToUpper();
                txtObservaciones.Text = dtConsulta.Rows[0]["observaciones"].ToString().Trim().ToUpper();

                Session["idCierreCajero_REA"] = dtConsulta.Rows[0]["id_ctt_cierre_caja"].ToString().Trim();

                if (sEstadoCaja == "CERRADA")
                {
                    txtFechaCierre.Text = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_cierre"].ToString().Trim()).ToString("dd/MM/yyyy");
                    txtHoraCierre.Text = dtConsulta.Rows[0]["hora_cierre"].ToString().Trim();
                    txtOficinistaCierre.Text = dtConsulta.Rows[0]["oficinista_cierre"].ToString().Trim().ToUpper();

                    BtnReabrir.Visible = true;
                    pnlMotivo.Visible = true;
                }

                else
                {
                    txtFechaCierre.Text = "";
                    txtHoraCierre.Text = "";
                    txtOficinistaCierre.Text = "";

                    BtnReabrir.Visible = false;
                    pnlMotivo.Visible = false;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA ACTUALIZAR EL PAGO
        private void actualizarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sSql = "";
                sSql += "update ctt_cierre_caja set" + Environment.NewLine;
                sSql += "fecha_cierre = null," + Environment.NewLine;
                sSql += "hora_cierre = null," + Environment.NewLine;
                sSql += "estado_cierre_caja = 'Abierta'," + Environment.NewLine;
                sSql += "saldo_final = 0," + Environment.NewLine;
                sSql += "id_ctt_oficinista_cierre = null" + Environment.NewLine;
                sSql += "where id_ctt_cierre_caja = " + Convert.ToInt32(Session["idCierreCajero_REA"].ToString());

                //EJECUCION DE INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                sSql = "";
                sSql += "insert into ctt_log_apertura_caja (" + Environment.NewLine;
                sSql += "id_ctt_cierre_caja, motivo, fecha_modifica, usuario_modifica, terminal_modifica," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Session["idCierreCajero_REA"].ToString() + ", '" + txtMotivo.Text.Trim() + "'," + Environment.NewLine;
                sSql += "convert(varchar(10), getdate(), 120), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "'," + Environment.NewLine;
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUCION DE INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                Response.Redirect("frmMensajeReabrir.aspx");
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); }
        }

        #endregion

        protected void BtnReabrir_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#QuestionModalAbrir').modal('show');</script>", false);
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmPrincipal.aspx");
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            actualizarRegistro();
        }
    }
}