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
    public partial class frmAperturaCaja : System.Web.UI.Page
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

            Session["modulo"] = "MÓDULO DE APERTURA DE CAJA";

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
                sSql += "select top 1 CC.id_ctt_jornada, CC.id_ctt_oficinista, CC.estado_cierre_caja, " + Environment.NewLine;
                sSql += "J.secuencia, CC.fecha_apertura, CC.id_ctt_cierre_caja, CC.hora_apertura," + Environment.NewLine;
                sSql += "O.descripcion oficinista, ltrim(str(isnull(CC.saldo_inicial, 0), 10, 2))saldo_inicial," + Environment.NewLine;
                sSql += "isnull(CC.observaciones, '') observaciones, J.descripcion jornada" + Environment.NewLine;
                sSql += "from ctt_cierre_caja CC INNER JOIN" + Environment.NewLine;
                sSql += "ctt_jornada J ON  J.id_ctt_jornada = CC.id_ctt_jornada" + Environment.NewLine;
                sSql += "and J.estado = 'A'" + Environment.NewLine;
                sSql += "and CC.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_oficinista O ON O.id_ctt_oficinista = CC.id_ctt_oficinista" + Environment.NewLine;
                sSql += "and O.estado = 'A'" + Environment.NewLine;
                sSql += "where CC.id_ctt_pueblo = " + Session["id_pueblo"].ToString().Trim() + Environment.NewLine;
                sSql += "and CC.caja_boleteria = 1" + Environment.NewLine;
                sSql += "and CC.caja_encomienda = 0" + Environment.NewLine;
                sSql += "order by CC.id_ctt_cierre_caja desc";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta== false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                    return;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    int iConsultaJornada;

                    iConsultaJornada = recuperarJornada(1);

                    txtJornada.Text = Session["descripcionJornada"].ToString().Trim().ToUpper();
                    txtFechaApertura.Text = Session["fechaApertura"].ToString();
                    txtHoraApertura.Text = Session["horaApertura"].ToString();
                    txtUsuario.Text = Session["usuarioApertura"].ToString();
                    txtLocalidad.Text = Session["oficina"].ToString();
                    txtSaldoInicial.Text = Convert.ToDecimal(Session["saldoInicialApertura"].ToString()).ToString("N2");
                    txtObservaciones.Text = Session["observacionesApertura"].ToString();

                    Session["idCierreCaja"] = "0";
                    Session["idJornadaApertura"] = iConsultaJornada.ToString();
                    Session["idJornada"] = iConsultaJornada.ToString();

                    btnGuardar.Visible = true;
                    txtSaldoInicial.ReadOnly = false;
                    txtObservaciones.ReadOnly = false;
                    return;
                }

                sEstadoCaja = dtConsulta.Rows[0]["estado_cierre_caja"].ToString().Trim().ToUpper();
                sFechaApertura = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_apertura"].ToString().Trim()).ToString("dd/MM/yyyy");
                sHoraApertura = dtConsulta.Rows[0]["hora_apertura"].ToString().Trim();
                sUsuarioApertura = dtConsulta.Rows[0]["oficinista"].ToString().Trim().ToUpper();
                sSaldoInicial = dtConsulta.Rows[0]["saldo_inicial"].ToString().Trim();
                sObervaciones = dtConsulta.Rows[0]["observaciones"].ToString().Trim().ToUpper();
                sJornadaApertura = dtConsulta.Rows[0]["jornada"].ToString().Trim().ToUpper();
                iIdCierreCajaApertura = Convert.ToInt32(dtConsulta.Rows[0]["id_ctt_cierre_caja"].ToString().Trim());
                iIdJornadaApertura = Convert.ToInt32(dtConsulta.Rows[0]["id_ctt_jornada"].ToString().Trim());

                fechaCaja = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_apertura"].ToString());
                fechaSistema = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

                Session["idJornadaConsulta"] = dtConsulta.Rows[0]["id_ctt_jornada"].ToString().Trim();
                Session["idCierreCajeroConsulta"] = dtConsulta.Rows[0]["id_ctt_cierre_caja"].ToString().Trim();

                if (sEstadoCaja == "CERRADA")
                {
                    int iConsultaJornada;

                    if (fechaCaja == fechaSistema)
                    {
                        iConsultaJornada = recuperarJornada(Convert.ToInt32(dtConsulta.Rows[0]["secuencia"].ToString()) + 1);
                    }

                    else
                    {
                        iConsultaJornada = recuperarJornada(1);
                    }

                    txtJornada.Text = Session["descripcionJornada"].ToString().Trim().ToUpper();
                    txtFechaApertura.Text = Session["fechaApertura"].ToString();
                    txtHoraApertura.Text = Session["horaApertura"].ToString();
                    txtUsuario.Text = Session["usuarioApertura"].ToString();
                    txtLocalidad.Text = Session["oficina"].ToString();
                    txtSaldoInicial.Text = Convert.ToDecimal(Session["saldoInicialApertura"].ToString()).ToString("N2");
                    txtObservaciones.Text = Session["observacionesApertura"].ToString();

                    Session["idCierreCaja"] = "0";
                    Session["idJornadaApertura"] = iConsultaJornada.ToString();
                    Session["idJornada"] = iConsultaJornada.ToString();

                    btnGuardar.Visible = true;
                    txtSaldoInicial.ReadOnly = false;
                    txtObservaciones.ReadOnly = false;
                }

                else
                {
                    Session["idCierreCaja"] = iIdCierreCajaApertura;
                    Session["idJornadaApertura"] = iIdJornadaApertura.ToString();
                    Session["idJornada"] = iIdJornadaApertura.ToString();
                    txtFechaApertura.Text = sFechaApertura;
                    txtHoraApertura.Text = sHoraApertura;
                    txtUsuario.Text = sUsuarioApertura;
                    txtJornada.Text = sJornadaApertura;
                    txtLocalidad.Text = Session["oficina"].ToString();
                    txtSaldoInicial.Text = sSaldoInicial;
                    txtObservaciones.Text = sObervaciones;

                    btnGuardar.Visible = false;
                    txtSaldoInicial.ReadOnly = true;
                    txtObservaciones.ReadOnly = true;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA RECUPERAR LA PRIMERA JORNADA
        private int recuperarJornada(int iOrden_P)
        {
            try
            {
                Session["descripcionJornada"] = "";

                sSql = "";
                sSql += "select id_ctt_jornada, descripcion" + Environment.NewLine;
                sSql += "from ctt_jornada" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and secuencia = " + iOrden_P;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                    return -1;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'No se encuentran configuradas las jornadas. Favor comuníquese con el administrador.', 'warning');", true);
                    return 0;
                }

                else
                {
                    Session["descripcionJornada"] = dtConsulta.Rows[0]["descripcion"].ToString().Trim().ToUpper();
                    return Convert.ToInt32(dtConsulta.Rows[0]["id_ctt_jornada"].ToString());
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

        #region FUNCIONES DEL USUARIO

        //CARGAR PARAMETROS
        private void cargarParametros()
        {
            try
            {
                txtFechaApertura.Text =  Session["fechaApertura"].ToString();
                txtHoraApertura.Text =  Session["horaApertura"].ToString();
                txtUsuario.Text = Session["usuarioApertura"].ToString();
                txtJornada.Text = Session["JornadaApertura"].ToString();
                txtLocalidad.Text = Session["oficina"].ToString();
                txtSaldoInicial.Text = Convert.ToDecimal(Session["saldoInicialApertura"].ToString()).ToString("N2");
                txtObservaciones.Text = Session["observacionesApertura"].ToString();

                if (Convert.ToInt32(Session["idCierreCaja"].ToString()) == 0)
                {
                    btnGuardar.Visible = true;
                    txtSaldoInicial.ReadOnly = false;
                    txtObservaciones.ReadOnly = false;
                }

                else
                {
                    btnGuardar.Visible = false;
                    txtSaldoInicial.ReadOnly = true;
                    txtObservaciones.ReadOnly = true;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }
        
        //FUNCION PARA INSERTAR EL REGISTRO
        private void insertarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar iniciar la transacción.', 'danger');", true);
                    return;
                }

                sFecha = DateTime.Now.ToString("yyyy/MM/dd");
                sHora = DateTime.Now.ToString("HH:mm");

                Session["fechaApertura"] = sFecha;
                Session["horaApertura"] = sHora;

                iJornada = Convert.ToInt32(Session["idJornadaApertura"].ToString());

                sSql = "";
                sSql += "insert into ctt_cierre_caja (" + Environment.NewLine;
                sSql += "id_ctt_pueblo, id_ctt_oficinista, fecha_apertura, hora_apertura," + Environment.NewLine;
                sSql += "estado_cierre_caja, porcentaje_iva, estado, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso, id_ctt_jornada, saldo_inicial," + Environment.NewLine;
                sSql += "observaciones, caja_boleteria, caja_encomienda)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Session["id_pueblo"].ToString()) + ", " + Convert.ToInt32(Session["idUsuario"].ToString()) + "," + Environment.NewLine;
                sSql += "'" + sFecha + "', '" + sHora + "', 'Abierta', " + Convert.ToDouble(Application["iva"].ToString()) + ", 'A'," + Environment.NewLine;
                sSql += "GETDATE(), '" + Session["usuario"].ToString() + "', '" + Environment.MachineName.ToString() + "', " + iJornada + "," + Environment.NewLine;
                sSql += Convert.ToDecimal(txtSaldoInicial.Text.Trim()) + ", '" + txtObservaciones.Text.Trim() + "', 1, 0)";

                //EJECUCIÓN DE INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    //MENSAJE DE ERROR
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                    goto reversa;
                }

                //OBTENER EL MAXIMO INGRESADO
                sTabla = "ctt_cierre_caja";
                sCampo = "id_ctt_cierre_caja";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo guardar los parámetros de apertura de caja.', 'danger');", true);
                    goto reversa;
                }

                else
                {
                    iIdCierreCaja = Convert.ToInt32(iMaximo);
                    Session["idCierreCaja"] = iMaximo.ToString();
                }

                sSql = "";
                sSql += "select CC.id_ctt_jornada, CC.id_ctt_oficinista, CC.estado_cierre_caja, " + Environment.NewLine;
                sSql += "J.secuencia, CC.fecha_apertura, CC.id_ctt_cierre_caja, CC.hora_apertura," + Environment.NewLine;
                sSql += "O.descripcion usuario, ltrim(str(isnull(CC.saldo_inicial, 0), 10, 2)) saldo_inicial," + Environment.NewLine;
                sSql += "isnull(CC.observaciones, '') observaciones, J.descripcion jornada" + Environment.NewLine;
                sSql += "from ctt_cierre_caja CC INNER JOIN" + Environment.NewLine;
                sSql += "ctt_jornada J ON  J.id_ctt_jornada = CC.id_ctt_jornada" + Environment.NewLine;
                sSql += "and J.estado = 'A'" + Environment.NewLine;
                sSql += "and CC.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_oficinista O ON O.id_ctt_oficinista = CC.id_ctt_oficinista" + Environment.NewLine;
                sSql += "and O.estado = 'A'" + Environment.NewLine;
                sSql += "where CC.id_ctt_pueblo = " + Session["id_pueblo"].ToString().Trim() + Environment.NewLine;
                sSql += "and CC.id_ctt_cierre_caja = " + iIdCierreCaja ; 

                //sSql = "";
                //sSql += "select id_ctt_cierre_caja, fecha_apertura, hora_apertura, O.descripcion usuario," + Environment.NewLine;
                //sSql += "CC.id_ctt_jornada, J.descripcion jornada, isnull(saldo_inicial, 0) saldo_inicial, " + Environment.NewLine;
                //sSql += "isnull(observaciones, '') observaciones" + Environment.NewLine;
                //sSql += "from ctt_cierre_caja CC INNER JOIN" + Environment.NewLine;
                //sSql += "ctt_oficinista O ON O.id_ctt_oficinista = CC.id_ctt_oficinista" + Environment.NewLine;
                //sSql += "and O.estado = 'A'" + Environment.NewLine;
                //sSql += "and CC.estado = 'A' INNER JOIN" + Environment.NewLine;
                //sSql += "ctt_jornada J ON J.id_ctt_jornada = CC.id_ctt_jornada" + Environment.NewLine;
                //sSql += "and J.estado = 'A'" + Environment.NewLine;
                //sSql += "where id_ctt_cierre_caja = " + iIdCierreCaja;                

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    goto reversa;
                }

                else
                {
                    Session["idCierreCaja"] = dtConsulta.Rows[0]["id_ctt_cierre_caja"].ToString();
                    Session["fechaApertura"] = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_apertura"].ToString()).ToString("dd/MM/yyyy");
                    Session["horaApertura"] = Convert.ToDateTime(dtConsulta.Rows[0]["hora_apertura"].ToString()).ToString("HH:mm");
                    Session["usuarioApertura"] = dtConsulta.Rows[0]["usuario"].ToString();
                    Session["idJornadaApertura"] = dtConsulta.Rows[0]["id_ctt_jornada"].ToString();
                    Session["JornadaApertura"] = dtConsulta.Rows[0]["jornada"].ToString();
                    Session["saldoInicialApertura"] = dtConsulta.Rows[0]["saldo_inicial"].ToString();
                    Session["observacionesApertura"] = dtConsulta.Rows[0]["observaciones"].ToString();

                    Session["idJornada"] = Session["idJornadaApertura"].ToString();

                    txtFechaApertura.Text = Session["fechaApertura"].ToString();
                    txtHoraApertura.Text = Session["horaApertura"].ToString();
                    txtUsuario.Text = Session["usuarioApertura"].ToString();
                    txtJornada.Text = Session["JornadaApertura"].ToString();
                    txtLocalidad.Text = Session["oficina"].ToString();
                    txtSaldoInicial.Text = Convert.ToDecimal(Session["saldoInicialApertura"].ToString()).ToString("N2");
                    txtObservaciones.Text = Session["observacionesApertura"].ToString();

                    txtSaldoInicial.ReadOnly = true;
                    txtObservaciones.ReadOnly = true;
                    btnGuardar.Visible = false;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Se ha registrado la apertura de caja éxitosamente.', 'success');", true);
                return;
            }

            catch (Exception)
            {
                goto reversa;
            }

        reversa:
            {
                conexionM.reversaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo guardar los parámetros de apertura de caja.', 'danger');", true);
                return;
            }
        }

        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            insertarRegistro();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCerrarSesion.aspx");
        }
    }
}