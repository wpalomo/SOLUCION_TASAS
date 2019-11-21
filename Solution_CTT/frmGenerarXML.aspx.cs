using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ENTIDADES;
using NEGOCIO;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Drawing;


namespace Solution_CTT
{
    public partial class frmGenerarXML : System.Web.UI.Page
    {
        ENTGenerarXML generarXMLE = new ENTGenerarXML();

        manejadorConexion conexionM = new manejadorConexion();
        manejadorGenerarXML generarXMLM = new manejadorGenerarXML();

        Clases_Factura_Electronica.ClaseGenerarFacturaXml generar = new Clases_Factura_Electronica.ClaseGenerarFacturaXml();
        Clases_Factura_Electronica.ClaseGenerarRIDE ride = new Clases_Factura_Electronica.ClaseGenerarRIDE();

        List<DetalleGenerarXML> misDetalles = new List<DetalleGenerarXML>();

        string sSql;
        string[] sDatosMaximo = new string[5];
        string sAccion;

        DataTable dtConsulta;
        bool bRespuesta;

        int iConsultarRegistro;

        int iIdPersona;
        int iIdFactura;

        double dbVUnidad;
        double dbPocenDescuento;
        double dbValorDescuento;
        double dbCantidad;
        double dbValorTotal;
        double dbTotalGrid;
        double dbServicio;
        double dbSubtotalBruto;
        double dbSumaDescuento;
        double dbSumaServicio;
        double dbSubtotalNeto;
        double dbSumaIva;
        double dbTotal;
        double dbIva;
        
        string sAyuda;
        string sCodigo;
        string sNombre;
        string sUnidad;
        //string sDirectorio;


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

            Session["modulo"] = "MÓDULO PARA GENERAR EL XML DE LA FACTURA ELECTR\x00d3NICA";

            if (!IsPostBack)
            {
                Limpiar();
            }
        }


        //FUNCION PARA LLENAR EL GRID cuando no se haya seleccionado ningun cliente 
        private void llenarInformacion(string NumeroFacturaBuscar)
        {
            try
            {
                sSql = "";
                sSql += "Select F.id_persona, F.Direccion_Factura, F.Telefono_Factura, F.Ciudad_Factura, F.Fabricante,F.Referencia," + Environment.NewLine;
                sSql += "F.Comentarios, CO.valor_texto as Moneda, FORPAGO.descripcion as Foma_Pago, F.fecha_vcto, F.Comentarios," + Environment.NewLine;
                sSql += "F.Peso_Neto, F.Peso_Bruto, F.numero_exportacion, F.partida_arancelaria, F.idformulariossri, TIPOCO.descripcion as Formato," + Environment.NewLine;
                sSql += "isnull(F.autorizacion,'') autorizacion, VENDE.codigo as Vendedor, CODI.valor_texto as Tipo_cliente, CP.Porcentaje_Dscto," + Environment.NewLine;
                sSql += "CP.Porcentaje_IVA, PR.codigo,NP.nombre, UNIDAD.codigo Unidad, isnull(DP.comentario,'') Comentario, DP.precio_unitario," + Environment.NewLine;
                sSql += "DP.Cantidad,Case when DP.precio_unitario=0 then 0 else round(100*DP.valor_Dscto/DP.precio_unitario,2) end Pct_Dscto," + Environment.NewLine;
                sSql += "DP.valor_Dscto, DP.valor_ICE, DP.valor_IVA, DP.Comentario, DP.Id_Det_Pedido, F.fecha_factura, Case when PR.Expira = 1 Then 1 Else 0 End Expira," + Environment.NewLine;
                sSql += "F.id_persona, TP.identificacion, rtrim(TP.apellidos + ' ' + TP.nombres) cliente, F.id_localidad, DP.valor_otro, F.id_factura, TP.correo_electronico" + Environment.NewLine;
                sSql += "From cv403_facturas F, cv403_facturas_pedidos FP, cv403_cab_pedidos CP, cv403_det_pedidos DP, cv401_productos PR, cv401_nombre_productos NP," + Environment.NewLine;
                sSql += "tp_codigos UNIDAD, tp_codigos CO, tp_codigos CODI, cv403_vendedores VENDE, cv403_formas_pagos FORPAGO, vta_tipocomprobante TIPOCO," + Environment.NewLine;
                sSql += "tp_personas TP" + Environment.NewLine;
                sSql += "Where F.id_factura = " + NumeroFacturaBuscar + Environment.NewLine;//AQUI FILTRO
                sSql += "And F.id_persona = TP.id_persona" + Environment.NewLine;
                sSql += "And F.id_factura = FP.id_factura" + Environment.NewLine;
                sSql += "And F.cg_moneda = CO.correlativo" + Environment.NewLine;
                sSql += "And CP.cg_tipo_cliente = CODI.correlativo" + Environment.NewLine;
                sSql += "And F.idtipocomprobante = TIPOCO.idtipocomprobante" + Environment.NewLine;
                sSql += "And F.id_forma_pago = FORPAGO.id_forma_pago" + Environment.NewLine;
                sSql += "And F.id_vendedor = VENDE.id_vendedor" + Environment.NewLine;
                sSql += "And FP.estado = 'A'" + Environment.NewLine;
                sSql += "And FP.Id_Pedido = CP.Id_Pedido" + Environment.NewLine;
                sSql += "And CP.Id_Pedido = DP.Id_Pedido" + Environment.NewLine;
                sSql += "And DP.estado = 'A'" + Environment.NewLine;
                sSql += "And DP.Cg_Unidad_Medida = UNIDAD.correlativo" + Environment.NewLine;
                sSql += "And DP.id_producto = PR.id_producto" + Environment.NewLine;
                sSql += "And PR.id_producto = NP.id_producto" + Environment.NewLine;
                sSql += "And NP.nombre_Interno = 1" + Environment.NewLine;//PUESTO MANUALMENTE
                sSql += "And NP.estado = 'A'" + Environment.NewLine;
                sSql += "And F.facturaelectronica = 1" + Environment.NewLine;
                sSql += "And CP.bandera_boleteria = 1" + Environment.NewLine;
                sSql += "And CP.bandera_encomienda = 0" + Environment.NewLine;
                sSql += "And F.id_localidad = " + Application["idLocalidad"].ToString() + Environment.NewLine; 
                sSql += "order by DP.Id_Det_Pedido";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        txtDireccion.Text = dtConsulta.Rows[0][1].ToString();
                        txtTelefono.Text = dtConsulta.Rows[0][2].ToString();
                        txtCiudad.Text = dtConsulta.Rows[0][3].ToString();
                        txtFabricante.Text = dtConsulta.Rows[0][4].ToString();
                        txtRefOt.Text = dtConsulta.Rows[0][5].ToString();
                        txtObser.Text = dtConsulta.Rows[0][6].ToString();
                        txtMoneda.Text = dtConsulta.Rows[0][7].ToString();
                        txtTipoPago.Text = dtConsulta.Rows[0][8].ToString();
                        txtFechaVcto.Text = dtConsulta.Rows[0][9].ToString();
                        txtPesoNeto.Text = dtConsulta.Rows[0][11].ToString();
                        txtPesoBruto.Text = dtConsulta.Rows[0][12].ToString();
                        txtNExportacion.Text = dtConsulta.Rows[0][13].ToString();
                        txtPartidaArancelaria.Text = dtConsulta.Rows[0][14].ToString();
                        cmbAutSri.Text = dtConsulta.Rows[0][15].ToString();
                        txtFormato.Text = dtConsulta.Rows[0][16].ToString();
                        txtVendedor.Text = dtConsulta.Rows[0][18].ToString();
                        txtTipoCliente.Text = dtConsulta.Rows[0][19].ToString();
                        txtPorcentajeDescuento.Text = dtConsulta.Rows[0][20].ToString();
                        txtFecha.Text = dtConsulta.Rows[0][34].ToString();
                        //dbAyudaCliente.iId = Convert.ToInt32(dtConsulta.Rows[0][36].ToString());
                        txtDocumentoCliente.Text = dtConsulta.Rows[0][37].ToString();
                        txtNombreCliente.Text = dtConsulta.Rows[0][38].ToString();
                        txtLocalidad.Text = dtConsulta.Rows[0][39].ToString();
                        txtMail.Text = dtConsulta.Rows[0][42].ToString();

                        iIdPersona = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                        iIdFactura = Convert.ToInt32(dtConsulta.Rows[0][41].ToString());

                        dbSumaDescuento = 0;
                        dbSumaServicio = 0;
                        dbSubtotalBruto = 0;
                        dbSumaIva = 0;
                        dbTotal = 0;
                        dbSubtotalNeto = 0;

                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            sCodigo = dtConsulta.Rows[i][22].ToString();

                            //if (dtConsulta.Rows[i][25].ToString() == "")
                            //{
                            //    sNombre = dtConsulta.Rows[i][23].ToString();
                            //}

                            //else
                            //{
                            //    sNombre = dtConsulta.Rows[i][25].ToString();
                            //}

                            sNombre = dtConsulta.Rows[i][23].ToString();

                            sUnidad = dtConsulta.Rows[i][24].ToString();
                            dbVUnidad = Convert.ToDouble(dtConsulta.Rows[i][26].ToString());
                            dbPocenDescuento = Convert.ToDouble(dtConsulta.Rows[i][28].ToString());
                            dbValorDescuento = Convert.ToDouble(dtConsulta.Rows[i][29].ToString());
                            dbCantidad = Convert.ToDouble(dtConsulta.Rows[i][27].ToString());
                            dbIva = Convert.ToDouble(dtConsulta.Rows[i][31].ToString());//NULOS
                            if (dtConsulta.Rows[i][40] != null)//VALIDAR PORQUE ESTA ALREVES
                            {
                                dbServicio = 0;                                
                            }
                            else
                            {
                                dbServicio = Convert.ToDouble(dtConsulta.Rows[i][40].ToString());
                            }

                            if (dbPocenDescuento == 100)
                            {
                                dbValorTotal = 0;
                            }

                            else
                            {
                                dbValorTotal = dbVUnidad * dbCantidad * (dbPocenDescuento / 100);
                            }

                            dbSubtotalBruto = dbSubtotalBruto + (dbCantidad * dbVUnidad);
                            dbSumaDescuento = dbSumaDescuento + (dbCantidad * dbValorDescuento);
                            dbSumaServicio = dbSumaServicio + (dbCantidad * dbServicio);
                            dbSumaIva = dbSumaIva + (dbCantidad * Convert.ToDouble(dtConsulta.Rows[i][31].ToString()));
                            dbTotalGrid = dbCantidad * (dbVUnidad- dbValorDescuento + dbIva);

                            //ANADIR AL GRID
                            DetalleGenerarXML miDetalle = new DetalleGenerarXML();
                            miDetalle.codigo = sCodigo;
                            miDetalle.nombre = sNombre;
                            miDetalle.Unidad = sUnidad;
                            miDetalle.Cantidad = dbCantidad.ToString("N2");
                            miDetalle.precio_unitario = dbVUnidad.ToString("N2");
                            miDetalle.Pct_Dscto = dbPocenDescuento.ToString("N2");
                            miDetalle.valor_Dscto = dbValorDescuento.ToString("N2");
                            miDetalle.valor_otro = dbServicio.ToString("N2");
                            miDetalle.ValorTotal = dbTotalGrid.ToString("N2");

                            misDetalles.Add(miDetalle);
                            dgvDatos.DataSource = misDetalles;
                            dgvDatos.DataBind();

                            //dgvDatos.Rows.Add(sCodigo, sNombre, sUnidad, dbCantidad.ToString("N2"), dbVUnidad.ToString("N2"),
                                                           //dbPocenDescuento.ToString("N0"), dbValorDescuento.ToString("N2"),
                                                           //dbServicio.ToString("N2"), dbValorTotal.ToString("N2"));
                        }

                        txtValorBruto.Text = dbSubtotalBruto.ToString("N2");
                        txtDescuento.Text = dbSumaDescuento.ToString("N2");
                        txtSubTotal.Text = (dbSubtotalBruto - dbSumaDescuento).ToString("N2");
                        txtIva.Text = dbSumaIva.ToString("N2");
                        txtServicio.Text = dbSumaServicio.ToString("N2");
                        txtTotalPagar.Text = (dbSubtotalBruto + dbSumaIva + dbSumaServicio - dbSumaDescuento).ToString("N2");

                        sSql = "";
                        sSql += "select idformulariossri, estabRetencion1, ptoEmiRetencion1, autRetencion1" + Environment.NewLine;
                        sSql += "from vta_formulariossri" + Environment.NewLine;
                        sSql += "where idformulariossri = 18";

                        dtConsulta = new DataTable();
                        dtConsulta.Clear();

                        bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                        if (bRespuesta == true)
                        {
                            if (dtConsulta.Rows.Count > 0)
                            {
                                txtNSerie1.Text = dtConsulta.Rows[0][1].ToString();
                                txtNSerie2.Text = dtConsulta.Rows[0][2].ToString();
                                txtNAut.Text = dtConsulta.Rows[0][3].ToString();
                            }
                        }

                        else
                        {
                            //ok.LblMensaje.Text = "No se pudo cargar la información tributaria.";
                            //ok.ShowDialog();
                        }
                    }
                    else
                    {
                        Limpiar();
                    }
                    //dgvReimpresionFactura.ClearSelection();
                }
                else
                {
                    Limpiar();
                    //ok.LblMensaje.Text = "Ocurrió un problema al extraer información de la factura seleccionada.";                    
                }
            }

            catch (Exception ex)
            {
                //
            }

        }

        //LIMPIAR
        private void Limpiar()
        {
            txtDireccion.Text = "";
            txtTelefono.Text = "";
            txtCiudad.Text = "";
            txtFabricante.Text = "";
            txtRefOt.Text = "";
            txtObser.Text = "";
            txtMoneda.Text = "";
            txtTipoPago.Text = "";
            txtFechaVcto.Text = "";
            txtPesoNeto.Text = "";
            txtPesoBruto.Text = "";
            txtNExportacion.Text = "";
            txtPartidaArancelaria.Text = "";
            cmbAutSri.Text = "";
            txtFormato.Text = "";
            txtVendedor.Text = "";
            txtTipoCliente.Text = "";
            txtPorcentajeDescuento.Text = "";
            txtFecha.Text = "";
            //dbAyudaCliente.iId = Convert.ToInt32(dtConsulta.Rows[0][36].ToString());
            txtDocumentoCliente.Text = "";
            txtNombreCliente.Text = "";
            txtLocalidad.Text = "";
            txtMail.Text = "";

            txtValorBruto.Text = "";
            txtDescuento.Text = "";
            txtSubTotal.Text = "";
            txtIva.Text = "";
            txtServicio.Text = "";
            txtTotalPagar.Text = "";

            txtNSerie1.Text = "";
            txtNSerie2.Text = "";
            txtNAut.Text = "";
            misDetalles.Clear();
            dgvDatos.DataSource = misDetalles;
            dgvDatos.DataBind();

            txtNumeroFacturaBuscar.Focus();
        }

        //CARGAR EL DIRECTORIO DONDE SE GUARDARAN LOS XML GENERADOS
        private bool buscarDirectorio()
        {
            try
            {
                sSql = "";
                sSql += "select codigo, nombres" + Environment.NewLine;
                sSql += "from cel_directorio" + Environment.NewLine;
                sSql += "where id_directorio = 1" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["sDirectorio"] = dtConsulta.Rows[0][1].ToString();
                        return true;
                    }
                    else
                    {
                        //ok.LblMensaje.Text = "No existe una configuracion de directorio para guardar los xml genereados.";
                        //ok.ShowDialog();
                        return false;
                    }
                }

                else
                {
                    //catchMensaje.LblMensaje.Text = sSql;
                    //catchMensaje.ShowDialog();
                    return false;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }
        //CARGAR INFO MODAL
        public void CargarDatosModal(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_facturas_paso_a_paso" + Environment.NewLine;
                sSql += "where id_localidad = " + Application["idLocalidad"].ToString() + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and numero_factura = " + Convert.ToInt32(txtFiltrarModalBuscarFacturas.Text.Trim());
                }

                columnaGrid(true);
                generarXMLE.ISQL = sSql;
                dgvFiltrarModalBuscarFacturas.DataSource = generarXMLM.listarFacturasEmitidas(generarXMLE);
                dgvFiltrarModalBuscarFacturas.DataBind();
                columnaGrid(false);
                
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA OCULTAR LAS COLUMNAS
        private void columnaGrid(bool ok)
        {
            dgvFiltrarModalBuscarFacturas.Columns[1].Visible = ok;
            dgvFiltrarModalBuscarFacturas.Columns[5].Visible = ok;
            //dgvFiltrarModalBuscarFacturas.Columns[6].Visible = ok;
        }
        
        protected void lbtnSeleccion_Click(object sender, EventArgs e)
        {
            sAccion = "S";
        }

        protected void dgvFiltrarModalBuscarFacturas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sAccion  == "S")
            {
                try
                {
                    int a = dgvFiltrarModalBuscarFacturas.SelectedIndex;
                    columnaGrid(true);
                    txtNumeroFacturaBuscar.Text = dgvFiltrarModalBuscarFacturas.Rows[a].Cells[1].Text.Trim();
                    llenarInformacion(txtNumeroFacturaBuscar.Text);
                    ModalBuscarFacturas.Hide();
                    columnaGrid(false);
                }
                catch (Exception ex)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }
        }

        protected void lbtnCerrarModalBuscarFacturas_Click(object sender, EventArgs e)
        {
            ModalBuscarFacturas.Hide();
        }

        //CREAR RIDE
        //private void crearRide(string filename, long iIdFactura_P, string sNumeroDocumento)
        //{
        //    try
        //    {
        //        dtConsulta = new DataTable();
        //        dtConsulta.Clear();

        //        bRespuesta = conexion.GFun_Lo_Genera_Ride(dtConsulta, iIdFactura_P);

        //        if (bRespuesta == true)
        //        {
        //            bRespuesta = ride.generarRide(dtConsulta, filename, iIdFactura_P);

        //            if (bRespuesta == false)
        //            {
        //                //ok.LblMensaje.Text = "Error al crear el reporte RIDE de la factura " + sNumeroDocumento;
        //                //ok.ShowDialog();
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
        //    }
        //}

        public bool CreatePDF(ReportViewer viewer, string Path, string Filename, string Extension)
        {            
            try
            {
                byte[] pdfContent = viewer.LocalReport.Render("PDF");
                string pdfPath = Path + Filename + Extension;
                //string pdfPath = "C:\\facturasautorizadas\\report.pdf";
                System.IO.FileStream pdfFile = new System.IO.FileStream(pdfPath, System.IO.FileMode.Create);
                pdfFile.Write(pdfContent, 0, pdfContent.Length);
                pdfFile.Close();
                return true;
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }
        
        protected void btnGenerarPDF_Click(object sender, EventArgs e)
        {
            //CREAR PDF Y SALVAR
            string Path = Application["RutaFacturasAutorizadas"].ToString();
            string Filename = "FACT-00";
            string Extension = ".pdf";
            //if (CreatePDF(rptGenerarRIDE, Path, Filename, Extension) != true) return;
            
            byte[] pdfContent = rptGenerarRIDE.LocalReport.Render("PDF");
            string pdfPath = Path + Filename + Extension;
            System.IO.FileStream pdfFile = new System.IO.FileStream(pdfPath, System.IO.FileMode.Create);
            pdfFile.Write(pdfContent, 0, pdfContent.Length);
            pdfFile.Close();



            
        }

        protected void btnCerrarModal_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Reporte.Hide();
        }

        protected void btnCerrarModalReporte_Click(object sender, EventArgs e)
        {
            ModalPopupExtender_Reporte.Hide();
        }

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNumeroFacturaBuscar.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_ADVERTENCIA + " ', 'Favor debe ingresar un numero de factura válido', 'warning');", true);
            }
            else
            {
                llenarInformacion(txtNumeroFacturaBuscar.Text);
            }
        }

        protected void lbtnBuscarAbrirModal_Click(object sender, EventArgs e)
        {
            ModalBuscarFacturas.Show();
            txtFiltrarModalBuscarFacturas.Text = "";
            CargarDatosModal(0);
            txtFiltrarModalBuscarFacturas.Focus();
        }

        protected void lbtnGenerarXML_Click(object sender, EventArgs e)
        {
            if (dgvDatos.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_ADVERTENCIA + " ', 'Debe existir un detalle, para generar el XML', 'warning');", true);
            }
            else
            {
                try
                {
                    if (buscarDirectorio() != true) return;

                    long NumFactura = Convert.ToInt64(txtNumeroFacturaBuscar.Text);
                    int iNumeroDecimales = 2;

                    //generar.GSub_GenerarFacturaXML(NumFactura, 0, "1", "1", Session["sDirectorio"].ToString(), "FACTURA", iNumeroDecimales, "elvis.geovanni@hotmail.com", "elvis.geovanni@hotmail.com");
                    generar.GSub_GenerarFacturaXML(NumFactura, 0, "1", Application["IDTipoAmbienteFE"].ToString(), Session["sDirectorio"].ToString(), "FACTURA", iNumeroDecimales, "elvis.geovanni@hotmail.com", "elvis.geovanni@hotmail.com");
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_MSJ_TITULO_EXITO + "', '" + Resources.MESSAGES.TXT_MSJ_BODY_EXITO_XML + "', 'success');", true);
                    Limpiar();
                }
                catch (Exception ex)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }
        }

        protected void lbtnGenerarRIDE_Click(object sender, EventArgs e)
        {
            try
            {
                long NumFactura = Convert.ToInt64(txtNumeroFacturaBuscar.Text);
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.generaRIDE(dtConsulta, NumFactura);

                if (bRespuesta == true)
                {
                    string sNumeroAux = dtConsulta.Rows[0]["estab"].ToString() + "-" + dtConsulta.Rows[0]["ptoemi"].ToString() + "-" + dtConsulta.Rows[0]["NUMERO_FACTURA"].ToString().PadLeft(9, '0');

                    DataColumn numero_factura_final = new DataColumn("numero_factura_final");
                    numero_factura_final.DataType = System.Type.GetType("System.String");
                    dtConsulta.Columns.Add(numero_factura_final);

                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        dtConsulta.Rows[i]["numero_factura_final"] = sNumeroAux;
                    }

                    ModalPopupExtender_Reporte.Show();//MUESTRO EL REPORTVIEWER

                    rptGenerarRIDE.ProcessingMode = ProcessingMode.Local;
                    rptGenerarRIDE.LocalReport.ReportPath = Server.MapPath("~/Reportes/rptGenerarRIDE.rdlc");
                    ReportDataSource datasource = new ReportDataSource("DSReportes", dtConsulta);//(NombreOriginesDatosRDLC,DataTable lleno)

                    rptGenerarRIDE.LocalReport.DataSources.Clear();
                    rptGenerarRIDE.LocalReport.DataSources.Add(datasource);
                    rptGenerarRIDE.LocalReport.Refresh();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_ADVERTENCIA + " ', 'Error al Generar el Reporte', 'warning');", true);
                }
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnCancelar2_Click(object sender, EventArgs e)
        {
            //long NumFactura = Convert.ToInt64(txtNumeroFacturaBuscar.Text);
            //dtConsulta = new DataTable();
            //dtConsulta.Clear();
            //bRespuesta = conexion.GFun_Lo_Genera_Ride(dtConsulta, NumFactura);
                        
            ////String pathReporte = Server.MapPath("~/Reportes/rptGenerarRIDE.rdlc");
            //RenderReport(dtConsulta);
        }

        //CREAR PDF SIN VISOR DE REPORTVIEWER
        private void RenderReport(DataTable datos) 
        {
          try
          {
             LocalReport localReport = new LocalReport();
             localReport.ReportPath = Server.MapPath("~/Reportes/rptGenerarRIDE.rdlc");
             ReportDataSource rdsCabecera = new ReportDataSource("DSReportes", datos);
             localReport.DataSources.Add(rdsCabecera);
             string reportType = "PDF";
             string mimeType;
             string encoding;
             string fileNameExtension;
             Warning[] warnings;
             string[] streams;
             byte[] renderedBytes;

             //Render 
             renderedBytes = localReport.Render(
             reportType,
             //deviceInfo,
             null,
             out mimeType,
             out encoding,
             out fileNameExtension,
             out streams,
             out warnings);


             string Path = Application["RutaFacturasAutorizadas"].ToString();
             string Filename = @"\F0000000";
             string Extension = ".pdf";

             string filePath = Path + Filename + Extension;
             System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
             fs.Write(renderedBytes, 0, renderedBytes.Length);
             fs.Close();

             //lblReporte.Text = "<object id=\"objPdf\" type=\"application/pdf\"  data=\"" 
             //         + nombre + "\" style=\"width: 980px; height: 100%;\" >  ERROR (no 
             //         puede mostrarse el objeto)</object>";
                            }
             catch (Exception ex)
             {
                 lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                 ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
             }
        }

        protected void dgvFiltrarModalBuscarFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvFiltrarModalBuscarFacturas.PageIndex = e.NewPageIndex;

                if (txtFiltrarModalBuscarFacturas.Text.Trim() == "")
                {
                    CargarDatosModal(0);
                }

                else
                {
                    CargarDatosModal(1);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnFiltarModalBuscarFacturas_Click(object sender, EventArgs e)
        {
            if (txtFiltrarModalBuscarFacturas.Text.Trim() == "")
            {
                CargarDatosModal(0);
            }

            else
            {
                CargarDatosModal(1);
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

        protected void dgvFiltrarModalBuscarFacturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < dgvFiltrarModalBuscarFacturas.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvFiltrarModalBuscarFacturas.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvFiltrarModalBuscarFacturas.Rows[i].BackColor = Color.White;
                }
            }
        }
    }
}