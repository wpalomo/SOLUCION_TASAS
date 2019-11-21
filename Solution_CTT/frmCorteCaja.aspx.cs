using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ENTIDADES;
using NEGOCIO;

namespace Solution_CTT
{
    public partial class frmCorteCaja : System.Web.UI.Page
    {
        ENTCorteCaja corteCajaE = new ENTCorteCaja();
        ENTPagosAtrasadosPagados atrasadosE = new ENTPagosAtrasadosPagados();
        ENTViajesActivosCierre viajesE = new ENTViajesActivosCierre();

        manejadorCorteCaja corteCajaM = new manejadorCorteCaja();
        manejadorConexion conexionM = new manejadorConexion();
        manejadorPagosAtrasadosPagados atrasadosM = new manejadorPagosAtrasadosPagados();
        manejadorViajesActivosCierre viajesM = new manejadorViajesActivosCierre();

        Clases.ClaseReporteCierreCaja reporteCierre = new Clases.ClaseReporteCierreCaja();
        Clases.ClaseCierreBoleteria reporte = new Clases.ClaseCierreBoleteria();
        Clases.ClaseImpresion imprimir = new Clases.ClaseImpresion();

        string sSql;
        string[] sDatosMaximo = new string[5];
        string sFecha;
        string sHora;
        string sFechaApertura;
        string sHoraApertura;
        string sImprimir;
        string sNombreImpresora;
        string sPathImpresora;
        string sDestinatarios;

        DataTable dtConsulta;
        DataTable dtAyuda;
        bool bRespuesta;

        int iAndenSeleccion;
        int iCortarPapel;
        int AbrirCajon;
        int iNumeroImpresiones;

        double dbSuma;

        decimal dbSumaTasas;
        decimal dbPagosAdministracion;
        decimal dbPagosAtrasados;

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

            Session["modulo"] = "MÓDULO DE CIERRE DE CAJA";

            if (!IsPostBack)
            {
                consultarCaja();
                sFecha = DateTime.Now.ToString("yyyy/MM/dd");
                llenarCajasTexto();
                llenarGrid(sFecha);
                llenarGridVigentes(sFecha);
                llenarGridPagosAdministrativos(sFecha);
                llenarGridPagosCumplidos(sFecha);
                llenarGridPagosAtrasadosPagados(sFecha);
                llenarViajesActivos(sFecha);
            }
        }

        #region FUNCIONES PARA LA IMPRESION

        //FUNCION PARA CONSULTAR LA IMPRESORA DEL TERMINAL
        private void consultarImpresora()
        {
            try
            {
                sSql = "";
                sSql += "select descripcion, path_url, cortar_papel," + Environment.NewLine;
                sSql += "abrir_cajon, numero_impresion" + Environment.NewLine;
                sSql += "from ctt_impresora" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sNombreImpresora = dtConsulta.Rows[0][0].ToString();
                        sPathImpresora = dtConsulta.Rows[0][1].ToString();
                        iCortarPapel = Convert.ToInt32(dtConsulta.Rows[0][2].ToString());
                        iNumeroImpresiones = Convert.ToInt32(dtConsulta.Rows[0][4].ToString());

                        imprimir.iniciarImpresion();
                        imprimir.escritoEspaciadoCorto(sImprimir);
                        imprimir.cortarPapel(iCortarPapel);
                        imprimir.imprimirReporte(sPathImpresora);
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No se encuentran los parámetros para la impresión.', 'warning');", true);
                    }
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCIONES DEL USUARIO

        //VALIDAR APERTURA DE CADA
        private void consultarCaja()
        {
            try
            {
                Session["id_jornada_tasa"] = Session["idJornada"].ToString();

                //if (Session["id_jornada_tasa"] != null)
                //{
                //    Session["id_jornada_tasa"] = Session["idJornada"].ToString();
                //}

                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No se encuentran los parámetros para cerrar la caja.', 'warning');", true);
                //}
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CARGAR LOS PAGOS ADMINISTRATIVOS COBRADOS
        private void llenarGridPagosAdministrativos(string sFecha_P)
        {
            try
            {
                sSql = "";
                sSql += "select '1. RETENCION:' descripcion," + Environment.NewLine;
                sSql += "ltrim(str(isnull(sum(isnull(valor, 0)), 0), 10, 2)) valor" + Environment.NewLine;
                sSql += "from ctt_vw_cierre_boleteria" + Environment.NewLine;
                sSql += "where fecha_pedido = '" + sFecha_P + "'" + Environment.NewLine;
                sSql += "and cobro_boletos = 0" + Environment.NewLine;
                sSql += "and cobro_retencion = 1" + Environment.NewLine;
                sSql += "and cobro_administrativo = 0" + Environment.NewLine;
                sSql += "and id_ctt_jornada = " + Convert.ToInt32(Session["idJornada"].ToString()) + Environment.NewLine;
                sSql += "union" + Environment.NewLine;
                sSql += "select '2. ADMINISTRACIÓN:' descripcion," + Environment.NewLine;
                sSql += "ltrim(str(isnull(sum(isnull(valor, 0)), 0), 10, 2)) valor" + Environment.NewLine;
                sSql += "from ctt_vw_cierre_boleteria" + Environment.NewLine;
                sSql += "where fecha_pedido = '" + sFecha_P + "'" + Environment.NewLine;
                sSql += "and cobro_boletos = 0" + Environment.NewLine;
                sSql += "and cobro_retencion = 0" + Environment.NewLine;
                sSql += "and cobro_administrativo = 1" + Environment.NewLine;
                sSql += "and pago_cumplido = 1" + Environment.NewLine;
                sSql += "and id_ctt_jornada = " + Convert.ToInt32(Session["idJornada"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    dgvPagosAdministrativos.DataSource = dtConsulta;
                    dgvPagosAdministrativos.DataBind();

                    dgvPagosAdministrativos.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;                    
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CARGAR LOS PAGOS ATRASADOS PAGADOS
        private void llenarGridPagosAtrasadosPagados(string sFecha_P)
        {
            try
            {
                sSql = "";
                sSql += "select fecha_viaje, hora_salida, disco + ' - ' + placa vehiculo, valor" + Environment.NewLine;
                sSql += "from ctt_vw_pagos_pendientes_atrasados" + Environment.NewLine;
                sSql += "where fecha_pago = '" + sFecha_P + "'" + Environment.NewLine;
                sSql += "and id_ctt_jornada = " + Convert.ToInt32(Session["idJornada"].ToString());

                atrasadosE.ISQL = sSql;
                dgvPagosAtrasados.DataSource = atrasadosM.listarPagosAtrasadosPagados(atrasadosE);
                dgvPagosAtrasados.DataBind();

                dgvPagosAtrasados.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvPagosAtrasados.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvPagosAtrasados.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvPagosAtrasados.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvPagosAtrasados.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Right;

                dbPagosAdministracion = 0;

                for (int i = 0; i < dgvPagosAdministrativos.Rows.Count; i++)
                {
                    dbPagosAdministracion += Convert.ToDecimal(dgvPagosAdministrativos.Rows[i].Cells[1].Text);
                }

                for (int i = 0; i < dgvPagosAtrasados.Rows.Count; i++)
                {
                    dbPagosAdministracion += Convert.ToDecimal(dgvPagosAtrasados.Rows[i].Cells[4].Text);
                }

                lblPagosAdministrativos.Text = "Total Pagos Administrativos: " + dbPagosAdministracion.ToString("N2") + " $";
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CARGAR LOS PAGOS CUMPLIDOS
        private void llenarGridPagosCumplidos(string sFecha_P)
        {
            try
            {
                sSql = "";
                sSql += "select fecha_viaje, hora_salida, disco + ' - ' + placa vehiculo, valor" + Environment.NewLine;
                sSql += "from ctt_vw_pagos_pendientes_cumplidos" + Environment.NewLine;
                sSql += "where fecha_pago = '" + sFecha_P + "'" + Environment.NewLine;
                sSql += "and id_ctt_jornada = " + Convert.ToInt32(Session["idJornada"].ToString());

                atrasadosE.ISQL = sSql;
                dgvPagosCumplidos.DataSource = atrasadosM.listarPagosAtrasadosPagados(atrasadosE);
                dgvPagosCumplidos.DataBind();

                dgvPagosCumplidos.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvPagosCumplidos.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvPagosCumplidos.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvPagosCumplidos.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvPagosCumplidos.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Right;

                dbPagosAdministracion = 0;

                for (int i = 0; i < dgvPagosCumplidos.Rows.Count; i++)
                {
                    dbPagosAdministracion += Convert.ToDecimal(dgvPagosCumplidos.Rows[i].Cells[4].Text);
                }

                lblPagosAdministrativosCumplidos.Text = "Total Pagos Administrativos Cumplidos: " + dbPagosAdministracion.ToString("N2") + " $";
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA MOSTRAR LOS VIAJES ACTIVOS QUE QUEDAN EN EL SISTEMA
        private void llenarViajesActivos(string sFecha_P)
        {
            try
            {
                sSql = "";
                sSql += "select fecha_viaje, hora_salida, ruta, sum(cantidad) cantidad," + Environment.NewLine;
                sSql += "ltrim(str(sum(cantidad * (precio_unitario - valor_dscto + valor_iva)), 10, 2)) valor" + Environment.NewLine;
                sSql += "from ctt_vw_viajes_activos" + Environment.NewLine;
                sSql += "where fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and id_ctt_jornada = " + Convert.ToInt32(Session["idJornada"].ToString()) + Environment.NewLine;
                sSql += "group by fecha_viaje, hora_salida, ruta" + Environment.NewLine;
                sSql += "order by fecha_viaje, hora_salida";

                viajesE.ISQL = sSql;
                dgvViajesActivos.DataSource = viajesM.listarViajesActivosCierre(viajesE);
                dgvViajesActivos.DataBind();

                dgvViajesActivos.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvViajesActivos.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvViajesActivos.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvViajesActivos.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvViajesActivos.Columns[5].ItemStyle.HorizontalAlign = HorizontalAlign.Right;

                dbSumaTasas = 0;

                for (int i = 0; i < dgvViajesActivos.Rows.Count; i++)
                {
                    dbSumaTasas += Convert.ToDecimal(dgvViajesActivos.Rows[i].Cells[5].Text);
                }

                lblTotalViajesActivos.Text = "Total Viajes Activos: " + dbSumaTasas.ToString("N2") + " $";
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CARGAR LAS TASAS DE USUARIO COBRADAS
        private void llenarGridVigentes(string sFecha_P)
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_programacion, hora_salida, fecha_grid, vehiculo, ruta, asientos_ocupados, tipo_viaje, valor" + Environment.NewLine;
                sSql += "from ctt_vw_cierre_caja_2" + Environment.NewLine;
                sSql += "where fecha_viaje = '" + sFecha_P + "'" + Environment.NewLine;
                sSql += "and estado_salida = 'Abierta'" + Environment.NewLine;
                sSql += "order by hora_salida";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        sSql = "";
                        sSql += "select ltrim(str(isnull(sum(DP.cantidad * (DP.precio_unitario - DP.valor_dscto + DP.valor_iva)), 0), 10, 2)) valor" + Environment.NewLine;
                        sSql += "from cv403_cab_pedidos CP INNER JOIN" + Environment.NewLine;
                        sSql += "cv403_det_pedidos DP ON CP.id_pedido = DP.id_pedido" + Environment.NewLine;
                        sSql += "and CP.estado = 'A'" + Environment.NewLine;
                        sSql += "and DP.estado = 'A'" + Environment.NewLine;
                        sSql += "where CP.cobro_boletos = 1" + Environment.NewLine;
                        sSql += "and cobro_retencion = 0" + Environment.NewLine;
                        sSql += "and cobro_administrativo = 0" + Environment.NewLine;
                        sSql += "and CP.id_ctt_programacion = " + Convert.ToInt32(dtConsulta.Rows[i]["id_ctt_programacion"].ToString());
                        
                        dtAyuda = new DataTable();
                        dtAyuda.Clear();
                       
                        bRespuesta = conexionM.consultarRegistro(sSql, dtAyuda);
                        
                        if (bRespuesta)
                        {
                            dtConsulta.Rows[i]["id_ctt_programacion"] = (i + 1).ToString();
                            dtConsulta.Rows[i]["valor"] = dtAyuda.Rows[0][0].ToString();
                        }

                        else
                        {
                            return;
                        }
                    }

                }

                dgvDatosVigentes.DataSource = dtConsulta;
                dgvDatosVigentes.DataBind();
                dgvDatosVigentes.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvDatosVigentes.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvDatosVigentes.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvDatosVigentes.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvDatosVigentes.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvDatosVigentes.Columns[5].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvDatosVigentes.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvDatosVigentes.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Right;

                if (dgvDatosVigentes.Rows.Count == 0)
                {
                    lblSumaVigentes.Text = "Total Cobrado: 0.00 $";
                }

                else
                {
                    dbSuma = 0.0;
                    for (int j = 0; j < dgvDatosVigentes.Rows.Count; j++)
                    {
                        dbSuma += Convert.ToDouble(dgvDatosVigentes.Rows[j].Cells[7].Text);
                    }
                    lblSumaVigentes.Text = "Total Cobrado: " + dbSuma.ToString("N2") + " $";
                }

            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        

        //FUNCION PARA LLENAR LAS CAJAS DE TEXTO
        private void llenarCajasTexto()
        {
            try
            {
                if (Convert.ToInt32(Session["idCierreCaja"].ToString()) == 0)
                {
                    //txtFechaApertura.Text = "";
                    //txtHoraApertura.Text = "";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Debe registrar la apertura de caja.', 'info');", true);
                    Response.Redirect("frmMensajeCaja.aspx");
                }

                else
                {
                    txtFechaApertura.Text = Convert.ToDateTime(Session["fechaApertura"].ToString()).ToString("dd/MM/yyyy");
                    txtHoraApertura.Text = Convert.ToDateTime(Session["horaApertura"].ToString()).ToString("HH:mm");                    
                }

                txtOficinista.Text = Session["usuario"].ToString().ToUpper();                
            }

            catch(Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid(string sFecha_P)
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_programacion, hora_salida, fecha_grid, vehiculo, ruta, asientos_ocupados, tipo_viaje, valor" + Environment.NewLine;
                sSql += "from ctt_vw_cierre_caja_2" + Environment.NewLine;
                sSql += "where fecha_viaje = '" + sFecha_P + "'" + Environment.NewLine;
                sSql += "and estado_salida = 'Cerrada'" + Environment.NewLine;
                sSql += "and id_ctt_jornada = " + Convert.ToInt32(Session["idJornada"].ToString()) + Environment.NewLine;
                sSql += "order by hora_salida";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        sSql = "";
                        sSql += "select ltrim(str(isnull(sum(DP.cantidad * (DP.precio_unitario - DP.valor_dscto + DP.valor_iva)), 0), 10, 2)) valor" + Environment.NewLine;
                        sSql += "from cv403_cab_pedidos CP INNER JOIN" + Environment.NewLine;
                        sSql += "cv403_det_pedidos DP ON CP.id_pedido = DP.id_pedido" + Environment.NewLine;
                        sSql += "and CP.estado = 'A'" + Environment.NewLine;
                        sSql += "and DP.estado = 'A'" + Environment.NewLine;
                        sSql += "where CP.cobro_boletos = 1" + Environment.NewLine;
                        sSql += "and cobro_retencion = 0" + Environment.NewLine;
                        sSql += "and cobro_administrativo = 0" + Environment.NewLine;
                        sSql += "and CP.id_ctt_programacion = " + Convert.ToInt32(dtConsulta.Rows[i]["id_ctt_programacion"].ToString());

                        dtAyuda = new DataTable();
                        dtAyuda.Clear();

                        bRespuesta = conexionM.consultarRegistro(sSql, dtAyuda);

                        if (bRespuesta == true)
                        {
                            dtConsulta.Rows[i]["id_ctt_programacion"] = (i + 1).ToString();
                            dtConsulta.Rows[i]["valor"] = dtAyuda.Rows[0][0].ToString();
                        }

                        else
                        {
                            return;
                        }
                    }
                }

                dgvDatos.DataSource = dtConsulta;
                dgvDatos.DataBind();
                dgvDatos.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvDatos.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvDatos.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvDatos.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvDatos.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvDatos.Columns[5].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvDatos.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dgvDatos.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Right;

                if (dgvDatos.Rows.Count == 0)
                {
                    lblSuma.Text = "Total Cobrado: 0.00 $";
                }
                else
                {
                    dbSuma = 0.0;
                    for (int j = 0; j < dgvDatos.Rows.Count; j++)
                    {
                        dbSuma += Convert.ToDouble(dgvDatos.Rows[j].Cells[7].Text);
                    }
                    lblSuma.Text = "Total Cobrado: " + dbSuma.ToString("N2") + " $";
                }
            }

            catch(Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA ACTUALIZAR Y CERRAR LA CAJA
        private void actualizarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sFecha = DateTime.Now.ToString("yyyy/MM/dd");
                sHora = DateTime.Now.ToString("HH:mm");

                sSql = "";
                sSql += "update ctt_cierre_caja set" + Environment.NewLine;
                sSql += "fecha_cierre = '" + sFecha + "'," + Environment.NewLine;
                sSql += "hora_cierre = '" + sHora + "',"  + Environment.NewLine;
                sSql += "estado_cierre_caja = 'Cerrada'," + Environment.NewLine;
                sSql += "saldo_final = " + Convert.ToDouble(txtSaldoFinal.Text.Trim()) + "," + Environment.NewLine;
                sSql += "id_ctt_oficinista_cierre = " + Convert.ToInt32(Session["idUsuario"].ToString()) + Environment.NewLine;
                sSql += "where id_ctt_cierre_caja = " + Convert.ToInt32(Session["idCierreCaja"].ToString());

                //EJECUCION DE INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                dtConsulta = Session["dtTasasPagadas"] as DataTable;

                if (reporte.llenarReporte(DateTime.Now.ToString("yyyy-MM-dd"), Convert.ToInt32(Session["idJornada"].ToString()), Session["nombreJornada"].ToString(), Session["usuario"].ToString(), dtConsulta, Convert.ToInt32(Session["idUsuario"].ToString()), 1) == true)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Cierre de caja procesado éxitosamente.', 'success');", true);
                    Response.Redirect("frmCerrarSesion.aspx");
                    goto fin;
                }
            }

            catch(Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); }
            fin: { }
        }

        //FUNCION PARA CREAR EL REPORTE 
        private bool crearReporte()
        {
            try
            {
                sFecha = DateTime.Now.ToString("yyyy/MM/dd");
                //sFecha = "2019/01/18";
                sFechaApertura = Convert.ToDateTime(Session["fechaApertura"].ToString()).ToString("dd/MM/yyyy");
                sHoraApertura = Convert.ToDateTime(Session["horaApertura"].ToString()).ToString("HH:mm");

                sSql = "";
                sSql += "select id_ctt_programacion" + Environment.NewLine;
                sSql += "from ctt_programacion" + Environment.NewLine;
                sSql += "where fecha_viaje = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sImprimir = "";
                        sImprimir = reporteCierre.reporteCierreCaja(Convert.ToInt32(Session["idUsuario"].ToString()), Session["usuario"].ToString(),
                                                                    sFecha, sFechaApertura + " " + sHoraApertura, "", dtConsulta);

                        consultarImpresora();

                        return true;
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No se han generado viajes para la fecha" + sFecha + ".', 'warning');", true);
                        return false;
                    }
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se ha podido consultar los viajes para la fecha" + sFecha + ".', 'warning');", true);
                    return false;
                }
            }

            catch(Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        #endregion

        #region ENVIO DE REPORTE

        //CONSULTAR PARAMETROS DE MAIL  
        private int cargarParametrosMail()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(correo_envia, '') correo_envia, isnull(clave_mail_envia, '') clave_mail_envia," + Environment.NewLine;
                sSql += "isnull(smtp_correo, '') smtp_correo, isnull(puerto_correo, 0) puerto_correo, isnull(maneja_ssl, 0) maneja_ssl," + Environment.NewLine;
                sSql += "isnull(correo_receptor_1, '') correo_receptor_1, isnull(correo_receptor_2, '') correo_receptor_2," + Environment.NewLine;
                sSql += "isnull(correo_receptor_3, '') correo_receptor_3" + Environment.NewLine;
                sSql += "from ctt_parametro" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["correo_envia"] = dtConsulta.Rows[0][0].ToString();
                        Session["clave_correo_envia"] = dtConsulta.Rows[0][1].ToString();
                        Session["correo_smtp"] = dtConsulta.Rows[0][2].ToString();
                        Session["correo_puerto"] = dtConsulta.Rows[0][3].ToString();
                        Session["correo_ssl"] = dtConsulta.Rows[0][4].ToString();
                        sDestinatarios = dtConsulta.Rows[0][5].ToString();

                        if (dtConsulta.Rows[0][6].ToString().Trim() != "")
                        {
                            sDestinatarios += ", " + dtConsulta.Rows[0][6].ToString().Trim();
                        }

                        if (dtConsulta.Rows[0][7].ToString().Trim() != "")
                        {
                            sDestinatarios += ", " + dtConsulta.Rows[0][7].ToString().Trim();
                        }

                        return 1;
                    }

                    else
                    {
                        return 0;
                    }
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return -1;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return -1;
            }
        }


        #endregion

        protected void btnCerrarCaja_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#QuestionModalCierre').modal('show');</script>", false);
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(Session["idCierreCaja"].ToString()) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Debe registrar la apertura de caja para imprimir el reporte de cierre.', 'info');", true);
                    return;
                }
                
                dtConsulta = new DataTable();
                dtConsulta.Clear();
                dtConsulta = Session["dtTasasPagadas"] as DataTable;
                reporte.llenarReporte(DateTime.Now.ToString("yyyy-MM-dd"), Convert.ToInt32(Session["idJornada"].ToString()), Session["nombreJornada"].ToString(), Session["usuario"].ToString(), dtConsulta, Convert.ToInt32(Session["idUsuario"].ToString()), 0);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnAceptarCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(Session["idCierreCaja"].ToString()) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Debe registrar la apertura de caja para realizar el cierre.', 'info');", true);
                    return;
                }

                actualizarRegistro();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {
            int iRespuestaParametros = cargarParametrosMail();

            if (iRespuestaParametros == -1)
            {
                return;
            }

            else if (iRespuestaParametros == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No se encuentran los parámetros para el envío del cierre de caja.', 'warning');", true);
            }

            else
            {
                reporte.llenarReporte(DateTime.Now.ToString("yyyy-MM-dd"), Convert.ToInt32(Session["idJornada"].ToString()), Session["nombreJornada"].ToString(), Session["usuario"].ToString(), dtConsulta, Convert.ToInt32(Session["idUsuario"].ToString()), 1);
            }
        }
    }
}