using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ENTIDADES;
using NEGOCIO;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.IO;
using BarcodeLib;
using System.Drawing;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Globalization;

namespace Solution_CTT
{
    public partial class frmPagosTasaUsuario : System.Web.UI.Page
    {
        ENTPagoTasaUsuario pagoTasaE = new ENTPagoTasaUsuario();
        ENTDetalleTasaUsuario detalleTasaE = new ENTDetalleTasaUsuario();
        ENTComboDatos comboE = new ENTComboDatos();

        manejadorDetalleTasaUsuario detalleTasaM = new manejadorDetalleTasaUsuario();
        manejadorPagoTasaUsuario pagoTasaM = new manejadorPagoTasaUsuario();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorConexion conexionM = new manejadorConexion();

        DataTable dtConsulta;

        bool bRespuesta;

        string sSql;
        string sAccion;
        string sCampo;
        string sTabla;
        string sFecha;
        string sFechaInicio;
        string sFechaFin;
        string sEstadoPago;

        string sTasaTerminal;
        string sTasaOficina;
        string sTasaCooperativa;
        string sServidorPruebas;
        string sServidorProduccion;
        string sUrlCredenciales;
        string sUrlTasaUsuario;
        string sUrlTasaAnulacion;
        string sTipoEmision;
        string sValorTasaUsuario;
        string sToken;
        string sCuentaToken;
        string sTasaUsuario;
        string sUrlEnvio;
        string sIdTasaRespuesta;

        string[] sDatosMaximo = new string[5];

        Byte[] Logo { get; set; }

        int iNumeroMovimientoCaja;
        int iIdMovimientoCaja;

        long iMaximo;

        Decimal dbTotal;
        Decimal dbValorTasa;

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

            Session["modulo"] = "MÓDULO DE PAGOS DE TASAS DE USUARIO - DEVESOFFT";

            if (!IsPostBack)
            {
                limpiar();
                llenarComboJornadas();
                llenarGrid();
            }
        }

        #region FUNCIONES DEL USUARIO 

        //FUNCION PARA IMPRIMIR DIRECTAMENTE EL REPORT VIEWER
        private void crearReporteImprimir(string sFecha_P, string sDescripcion_P, string sValor_P)
        {
            try
            {
                LocalReport reporteLocal = new LocalReport();
                reporteLocal.ReportPath = Server.MapPath("~/Reportes/rptReporteMovimientoCaja.rdlc");
                ReportParameter[] parametros = new ReportParameter[4];
                parametros[0] = new ReportParameter("P_Fecha_Impresion", sFecha_P);
                parametros[1] = new ReportParameter("P_Usuario_Impresion", sDatosMaximo[0]);
                parametros[2] = new ReportParameter("P_Descripcion", sDescripcion_P);
                parametros[3] = new ReportParameter("P_Valor", sValor_P);

                reporteLocal.SetParameters(parametros);
                Clases.Impresor imp = new Clases.Impresor();
                imp.Imprime(reporteLocal);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            cmbEstado.SelectedIndex = 0;
            txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        //FUNCION PARA CONSULTAR EL PAGO DE LAS TASAS DE USUARIO
        private void consultarPago()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(id_ctt_movimiento_caja_pago, 0) id_ctt_movimiento_caja_pago" + Environment.NewLine;
                sSql += "from ctt_movimiento_caja" + Environment.NewLine;
                sSql += "where fecha = '" + Convert.ToDateTime(Session["fecha_tasa"].ToString()).ToString("yyyy/MM/dd") + "'" + Environment.NewLine;
                sSql += "and cobro_tasa_usuario = 1" + Environment.NewLine;
                sSql += "and estado_movimiento = 'PAGADA'" + Environment.NewLine;
                sSql += "and ambiente_tasa_usuario = 1" + Environment.NewLine;
                sSql += "and id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString()) + Environment.NewLine;
                sSql += "and id_ctt_jornada = " + Convert.ToInt32(Session["id_jornada_tasa"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                iIdMovimientoCaja = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                if (iIdMovimientoCaja == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo encontrar el comprobante de pago.', 'danger');", true);
                    return;
                }

                sSql = "";
                sSql += "select isnull(concepto, '') concepto, ltrim(str(valor, 10, 2)) valor" + Environment.NewLine;
                sSql += "from ctt_movimiento_caja" + Environment.NewLine;
                sSql += "where id_ctt_movimiento_caja = " + iIdMovimientoCaja + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        crearReporteImprimir(DateTime.Now.ToString("yyyy/MM/dd"), dtConsulta.Rows[0]["concepto"].ToString().Trim().ToUpper(), dtConsulta.Rows[0]["valor"].ToString().Trim().ToUpper());
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo encontrar el comprobante de pago.', 'danger');", true);
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

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid()
        {
            try
            {
                sFechaInicio = Convert.ToDateTime(txtFechaDesde.Text.Trim()).ToString("yyyy/MM/dd");
                sFechaFin = Convert.ToDateTime(txtFechaHasta.Text.Trim()).ToString("yyyy/MM/dd");

                sSql = "";
                sSql += "select * from ctt_vw_pagar_tasa_usuario" + Environment.NewLine;
                sSql += "where fecha between '" + sFechaInicio + "'" + Environment.NewLine;
                sSql += "and '" + sFechaFin + "'" + Environment.NewLine;

                if (cmbEstado.SelectedIndex != 0)
                {
                    sSql += "and estado_movimiento = '" + cmbEstado.SelectedValue + "'" + Environment.NewLine;
                }

                if (Convert.ToInt32(cmbJornada.SelectedValue) != 0)
                {
                    sSql += "and id_ctt_jornada = " + Convert.ToInt32(cmbJornada.SelectedValue) + Environment.NewLine;
                }

                sSql += "order by fecha";

                dgvDatos.Columns[1].Visible = true;
                pagoTasaE.ISQL = sSql;
                dgvDatos.DataSource = pagoTasaM.listarPagoTasa(pagoTasaE);
                dgvDatos.DataBind();
                dgvDatos.Columns[1].Visible = false;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL GRID TASA DE USUARIO
        private void llenarGridTasa()
        {
            try
            {
                sSql = "";
                sSql += "select fecha, hora, cantidad, " + Environment.NewLine;
                sSql += "ltrim(str(valor, 10, 2)) valor," + Environment.NewLine;
                sSql += "tasa_usuario, estado_movimiento" + Environment.NewLine;
                sSql += "from ctt_movimiento_caja" + Environment.NewLine;
                sSql += "where fecha = '" + Convert.ToDateTime(Session["fecha_tasa"].ToString()).ToString("yyyy/MM/dd") + "'" + Environment.NewLine;
                sSql += "and cobro_tasa_usuario = 1" + Environment.NewLine;
                sSql += "and ambiente_tasa_usuario = 1" + Environment.NewLine;
                sSql += "and id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString()) + Environment.NewLine;
                sSql += "and id_ctt_jornada = " + Convert.ToInt32(Session["id_jornada_tasa"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "order by id_ctt_movimiento_caja";

                detalleTasaE.ISQL = sSql;
                dgvTasa.DataSource = detalleTasaM.listarTasaUsuario(detalleTasaE);
                dgvTasa.DataBind();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE JORNADAS
        private void llenarComboJornadas()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_jornada, descripcion" + Environment.NewLine;
                sSql += "from ctt_jornada" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbJornada.DataSource = comboM.listarCombo(comboE);
                cmbJornada.DataValueField = "IID";
                cmbJornada.DataTextField = "IDATO";
                cmbJornada.DataBind();
                cmbJornada.Items.Insert(0, new ListItem("TODAS", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA ACTUALIZAR EL ESTADO DE LOS PAGOS DE TASAS DE USUARIO
        private void actualizarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    lblMensajeError.Text = "No se pudo iniciar la transacción para guardar los registros.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                //PROCEDIMIENTO PARA EXTRAER EL NUMERO DE MOVIMIENTO
                sSql = "";
                sSql += "select numeromovimientocaja" + Environment.NewLine;
                sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    iNumeroMovimientoCaja = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                } 

                sFecha = DateTime.Now.ToString("yyyy/MM/dd");
                dbTotal = Convert.ToDecimal(Session["total_tasa"].ToString());

                //INSERTAR EN LA TABLA CTT_MOVIMIENTO_CAJA
                sSql = "";
                sSql += "insert into ctt_movimiento_caja (" + Environment.NewLine;
                sSql += "tipo_movimiento, idempresa, id_localidad, id_caja, id_ctt_jornada," + Environment.NewLine;
                sSql += "cg_moneda, fecha, hora, cantidad, valor, estado, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso, concepto, egreso_tasa_usuario, estado_movimiento)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "0, " + Convert.ToInt32(Application["idEmpresa"].ToString()) + ", " + Convert.ToInt32(Application["idLocalidad"].ToString()) + "," + Environment.NewLine;
                sSql += "30, " + Convert.ToInt32(Session["idJornada"].ToString()) + ", " + Convert.ToInt32(Application["cgMoneda"].ToString()) + "," + Environment.NewLine;
                sSql += "'" + sFecha + "', GETDATE(), 1, " + dbTotal.ToString(System.Globalization.CultureInfo.InvariantCulture) + "," + Environment.NewLine;
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'PAGO DE TASAS DE USUARIO - " + Session["fecha_tasa"].ToString() + "', 1, 'PAGADA')";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //PROCEDIMINTO PARA EXTRAER EL ID DE LA TABLA CTT_MOVIMIENTO_CAJA
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sTabla = "ctt_movimiento_caja";
                sCampo = "id_ctt_movimiento_caja";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>No se pudo obtener el código de la tabla " + sTabla + ".";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                else
                {
                    iIdMovimientoCaja = Convert.ToInt32(iMaximo);
                }

                //INSTRUCCION INSERTAR EN LA TABLA CTT_NUMERO_MOVIMIENTO_CAJA
                sSql = "";
                sSql += "insert into ctt_numero_movimiento_caja (" + Environment.NewLine;
                sSql += "id_ctt_movimiento_caja, numero_movimiento_caja, estado," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdMovimientoCaja + ", " + iNumeroMovimientoCaja + ", 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //QUERY PARA ACTUALIZAR EL NUMERO DE MOVIMIENTO EN LA TABLA TP_LOCALIDADES_IMPRESORAS
                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numeromovimientocaja = numeromovimientocaja + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString());

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //INSTRUCCION SQL PARA ACTUALIZAR LOS REGISTROS A PAGADO
                sSql = "";
                sSql += "update ctt_movimiento_caja set" + Environment.NewLine;
                sSql += "estado_movimiento = 'PAGADA'," + Environment.NewLine;
                sSql += "id_ctt_movimiento_caja_pago = " + iIdMovimientoCaja + Environment.NewLine;
                sSql += "where fecha = '" + Convert.ToDateTime(Session["fecha_tasa"].ToString()).ToString("yyyy/MM/dd") + "'" + Environment.NewLine;
                sSql += "and cobro_tasa_usuario = 1" + Environment.NewLine;
                sSql += "and estado_movimiento = 'PENDIENTE'" + Environment.NewLine;
                sSql += "and ambiente_tasa_usuario = 1" + Environment.NewLine;
                sSql += "and id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString()) + Environment.NewLine;
                sSql += "and id_ctt_jornada = " + Convert.ToInt32(Session["id_jornada_tasa"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                crearReporteImprimir(sFecha, "PAGO DE TASAS DE USUARIO - " + Session["fecha_tasa"].ToString(), dbTotal.ToString("N2"));
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'El pago ha sido generado de forma exitosa. Se imprimirá un comprobante de pago.', 'success');", true);

                llenarGrid();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); }
        }

        #endregion

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                dgvDatos.Columns[1].Visible = true;
                sEstadoPago = dgvDatos.Rows[a].Cells[6].Text.Trim().ToUpper();
                Session["id_jornada_tasa"] = dgvDatos.Rows[a].Cells[1].Text.Trim();
                Session["fecha_tasa"] = dgvDatos.Rows[a].Cells[2].Text.Trim();
                Session["total_tasa"] = dgvDatos.Rows[a].Cells[5].Text.Trim();

                if (sAccion == "Ver")
                {
                    ModalPopupExtender_Tasas.Show();
                    llenarGridTasa();
                }

                else if (sAccion == "Pagar")
                {
                    if (sEstadoPago == "PAGADA")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'Las tasas de usuario del registro ya han sido pagadas.', 'warning');", true);
                    }

                    else
                    {
                        actualizarRegistro();
                    }
                }

                else if (sAccion == "Imprimir")
                {
                    if (sEstadoPago == "PAGADA")
                    {
                        consultarPago();
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'Las tasas de usuario del registro aún no han sido pagadas para imprimir un comprobante de pago.', 'warning');", true);
                    }
                }

                dgvDatos.Columns[1].Visible = false;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvDatos.PageIndex = e.NewPageIndex;
                llenarGrid();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnVer_Click(object sender, EventArgs e)
        {
            sAccion = "Ver";
        }

        protected void lbtnPagar_Click(object sender, EventArgs e)
        {
            sAccion = "Pagar";
        }

        protected void lbtnImprimir_Click(object sender, EventArgs e)
        {
            sAccion = "Imprimir";
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            if (txtFechaDesde.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'Favor ingrese una fecha inicial para realizar la consulta.', 'warning');", true);
                txtFechaDesde.Focus();
            }

            else if (txtFechaHasta.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'Favor ingrese una fecha final para realizar la consulta.', 'warning');", true);
                txtFechaHasta.Focus();
            }

            else if (Convert.ToDateTime(txtFechaDesde.Text.Trim()) > Convert.ToDateTime(txtFechaHasta.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'La fecha inicial no puede ser superior a la fecha final.', 'warning');", true);
            }

            else
            {
                llenarGrid();
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
            llenarComboJornadas();
            llenarGrid();
        }

        protected void btnCerrarModalTasa_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Tasas.Hide();
        }

        protected void dgvTasa_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvTasa.PageIndex = e.NewPageIndex;
                llenarGridTasa();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < dgvDatos.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvDatos.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvDatos.Rows[i].BackColor = Color.White;
                }
            }
        }
    }
}