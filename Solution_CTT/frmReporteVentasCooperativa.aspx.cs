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
    public partial class frmReporteVentasCooperativa : System.Web.UI.Page
    {
        ENTComboDatos comboE = new ENTComboDatos();

        manejadorConexion conexionM = new manejadorConexion();
        manejadorComboDatos comboM = new manejadorComboDatos();

        string sSql;
        string sFechaInicial;
        string sFechaFinal;

        DataTable dtConsulta;
        DataTable dtAyuda;

        bool bRespuesta;

        Decimal dbSuma;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
                return;
            }

            Session["modulo"] = "REPORTE DE VENTAS POR COOPERATIVA";

            if (!IsPostBack)
            {
                limpiar();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION DE LIMPIAR
        private void limpiar()
        {
            txtFechaInicial.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFechaFinal.Text = DateTime.Now.ToString("dd/MM/yyyy");
            llenarComboLocalidad();

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            dgvDatos.DataSource = dtConsulta;
            dgvDatos.DataBind();

            btnImprimir.Visible = false;
            Scroll.Visible = false;
        }

        //LLENAR COMBO LOCALIDAD
        private void llenarComboLocalidad()
        {
            try
            {
                sSql = "";
                sSql += "select id_localidad, nombre_localidad" + Environment.NewLine;
                sSql += "from tp_vw_localidades" + Environment.NewLine;
                sSql += "where id_localidad=" + Application["idLocalidad"].ToString();

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        comboE.ISSQL = sSql;
                        cmbLocalidad.DataSource = comboM.listarCombo(comboE);
                        cmbLocalidad.DataValueField = "IID";
                        cmbLocalidad.DataTextField = "IDATO";
                        cmbLocalidad.DataBind();
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CREAR EL GRIDVIEW
        private void llenarGrid()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tipo_cliente, descripcion, 0 cantidad, 0.00 total" + Environment.NewLine;
                sSql += "from ctt_tipo_cliente" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    Scroll.Visible = false;
                    lblSuma.Text = "Total: 0.00 $";
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No se encuentran registros de tipo de tarifas.', 'info');", true);
                    return;
                }

                dbSuma = 0;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    int iIdTipoCliente_P = Convert.ToInt32(dtConsulta.Rows[i]["id_ctt_tipo_cliente"].ToString());

                    sSql = "";
                    sSql += "select isnull(sum(isnull(DP.cantidad, 0)), 0) cantidad," + Environment.NewLine;
                    sSql += "ltrim(str(isnull(sum(isnull(DP.cantidad * (DP.precio_unitario - DP.valor_dscto + DP.valor_iva), 0)), 0), 10, 2)) suma" + Environment.NewLine;
                    sSql += "from cv403_det_pedidos DP INNER JOIN" + Environment.NewLine;
                    sSql += "cv403_cab_pedidos CP ON CP.id_pedido = DP.id_pedido" + Environment.NewLine;
                    sSql += "and CP.estado = 'A'" + Environment.NewLine;
                    sSql += "and DP.estado = 'A'" + Environment.NewLine;
                    sSql += "where DP.id_ctt_tipo_cliente = " + iIdTipoCliente_P + Environment.NewLine;
                    sSql += "and CP.fecha_pedido between '" + Convert.ToDateTime(txtFechaInicial.Text.Trim()).ToString("yyyy-MM-dd") + "'" + Environment.NewLine;
                    sSql += "and '" + Convert.ToDateTime(txtFechaFinal.Text.Trim()).ToString("yyyy-MM-dd") + "'" + Environment.NewLine;
                    sSql += "and CP.cobro_boletos = 1" + Environment.NewLine;
                    sSql += "and CP.id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString());

                    dtAyuda = new DataTable();
                    dtAyuda.Clear();

                    bRespuesta = conexionM.consultarRegistro(sSql, dtAyuda);

                    if (bRespuesta == false)
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        return;
                    }

                    dtConsulta.Rows[i]["cantidad"] = dtAyuda.Rows[0]["cantidad"].ToString();
                    dtConsulta.Rows[i]["total"] = dtAyuda.Rows[0]["suma"].ToString();

                    dbSuma += Convert.ToDecimal(dtAyuda.Rows[0]["suma"].ToString());
                }

                dgvDatos.Columns[0].Visible = true;
                dgvDatos.DataSource = dtConsulta;
                dgvDatos.DataBind();
                dgvDatos.Columns[0].Visible = false;
                Scroll.Visible = true;

                lblSuma.Text = "Total: " + dbSuma.ToString("N2") + " $";

                consultarValoresAnulados();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONSULTAR VALORES ANULADOS
        private void consultarValoresAnulados()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from ctt_tasas_smartt" + Environment.NewLine;
                sSql += "where fecha_emision_tasa between '" + Convert.ToDateTime(txtFechaInicial.Text.Trim()).ToString("yyyy-MM-dd") + "'" + Environment.NewLine;
                sSql += "and '" + Convert.ToDateTime(txtFechaFinal.Text.Trim()).ToString("yyyy-MM-dd") + "'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    txtCantidadTasas.Text = "0";
                }

                txtCantidadTasas.Text = dtConsulta.Rows[0][0].ToString();

                sSql = "";
                sSql += "select sum(total_tasas)" + Environment.NewLine;
                sSql += "from ctt_detalle_tasa_smartt" + Environment.NewLine;
                sSql += "where fecha_emision_tasa between '" + Convert.ToDateTime(txtFechaInicial.Text.Trim()).ToString("yyyy-MM-dd") + "'" + Environment.NewLine;
                sSql += "and '" + Convert.ToDateTime(txtFechaFinal.Text.Trim()).ToString("yyyy-MM-dd") + "'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    txtSumaTotalTasas.Text = "0.00";
                }

                txtSumaTotalTasas.Text = Convert.ToDecimal(dtConsulta.Rows[0][0].ToString()).ToString("N2");

                sSql = "";
                sSql += "select isnull(sum(isnull(DP.cantidad, 0)), 0) cantidad," + Environment.NewLine;
                sSql += "ltrim(str(isnull(sum(isnull(DP.cantidad * (DP.precio_unitario - DP.valor_dscto + DP.valor_iva), 0)), 0), 10, 2)) suma" + Environment.NewLine;
                sSql += "from cv403_det_pedidos DP INNER JOIN" + Environment.NewLine;
                sSql += "cv403_cab_pedidos CP ON CP.id_pedido = DP.id_pedido" + Environment.NewLine;
                sSql += "and CP.estado in ('E', 'N')" + Environment.NewLine;
                sSql += "and DP.estado in ('E', 'N')" + Environment.NewLine;
                sSql += "where CP.fecha_pedido between '" + Convert.ToDateTime(txtFechaInicial.Text.Trim()).ToString("yyyy-MM-dd") + "'" + Environment.NewLine;
                sSql += "and '" + Convert.ToDateTime(txtFechaFinal.Text.Trim()).ToString("yyyy-MM-dd") + "'" + Environment.NewLine;
                sSql += "and CP.cobro_boletos = 1" + Environment.NewLine;
                sSql += "and CP.id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    txtCantidadAnulados.Text = "0";
                    txtTotalAnulados.Text = "0.00";
                }

                txtCantidadAnulados.Text = dtConsulta.Rows[0]["cantidad"].ToString();
                txtTotalAnulados.Text = dtConsulta.Rows[0]["suma"].ToString();

                Decimal dbSuma_P = dbSuma + Convert.ToDecimal(txtSumaTotalTasas.Text.Trim());

                txtTotalReportado.Text = dbSuma_P.ToString();     
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        protected void btnExtraerInformacion_Click(object sender, EventArgs e)
        {
            sFechaInicial = Convert.ToDateTime(txtFechaInicial.Text.Trim()).ToString("yyyy-MM-dd");
            sFechaFinal = Convert.ToDateTime(txtFechaFinal.Text.Trim()).ToString("yyyy-MM-dd");

            if (Convert.ToDateTime(sFechaInicial) > Convert.ToDateTime(sFechaFinal))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'La fecha inicial no puede ser superior a la fecha final.', 'info');", true);
                return;
            }

            llenarGrid();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmReporteVentasCooperativa.aspx");
        }
    }
}