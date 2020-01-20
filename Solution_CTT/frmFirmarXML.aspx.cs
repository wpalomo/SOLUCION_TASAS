using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ENTIDADES;
using NEGOCIO;
using System.IO;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmFirmarXML : System.Web.UI.Page
    {
        manejadorGenerarXML generarXMLM = new manejadorGenerarXML();
        manejadorConexion conexion = new manejadorConexion();

        ENTComboDatos comboE = new ENTComboDatos();
        ENTGenerarXML generarXMLE = new ENTGenerarXML();
        manejadorComboDatos comboM = new manejadorComboDatos();

        Clases_Factura_Electronica.ClaseFirmarXML firmarXML = new Clases_Factura_Electronica.ClaseFirmarXML();

        DataTable dtConsulta;

        string sSql;
        string[] sDatosMaximo = new string[5];
        string sAccion;

        bool bRespuesta;
        bool bActualizar;

        int iIdFactura = 0;

        int iCol_Correlativo;
        int iCol_Codigo;
        int iCol_Descripcion;

        string sNumeroDocumento;
        string sCodigoDocumento;
        string sRutaArchivo;

        string sTipoComprobanteVenta;
        string sAyuda;

        string sCertificado_Ruta;
        string sCertificado_Palabra_Clave;

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

            Session["modulo"] = "IMÓDULO PARA FIRMAR LOS COMPROBANTES ELECTRÓNICOS";

            if (!IsPostBack)
            {
                llenarTipoAmbiente();
                llenarCertificadoDigital();
                llenarTipoEmision();
                llenarTipoComprobante();
            }
        }

        #region FUNCIONES DEL USUARIO
        //LLENAR TIPO COMPROBANTE
        private void llenarTipoComprobante()
        {
            try
            {
                sSql = "";
                sSql = sSql + "select id_tipo_comprobante, nombres" + Environment.NewLine;
                sSql = sSql + "from cel_tipo_comprobante" + Environment.NewLine;
                sSql = sSql + "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        comboE.ISSQL = sSql;
                        cmbTipoComprobante.DataSource = comboM.listarCombo(comboE);
                        cmbTipoComprobante.DataValueField = "IID";
                        cmbTipoComprobante.DataTextField = "IDATO";
                        cmbTipoComprobante.DataBind();
                        cmbTipoComprobante.Items.Insert(0, new ListItem("Seleccione..!!", "0"));

                        if (cmbTipoComprobante.Items.Count > 0)
                        {
                            cmbTipoComprobante.SelectedIndex = 1;
                            //cmbTipoComprobante.Enabled = false;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }
        //LLENAR TIPO DE EMISION
        private void llenarTipoAmbiente()
        {
            try
            {
                sSql = "";
                sSql = sSql + "Select id_tipo_ambiente, nombres" + Environment.NewLine;
                sSql = sSql + "From cel_tipo_ambiente" + Environment.NewLine;
                sSql = sSql + "Where estado = 'A'" + Environment.NewLine;
                sSql = sSql + "Order By id_tipo_ambiente";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        comboE.ISSQL = sSql;
                        cmbTipoAmbiente.DataSource = comboM.listarCombo(comboE);
                        cmbTipoAmbiente.DataValueField = "IID";
                        cmbTipoAmbiente.DataTextField = "IDATO";
                        cmbTipoAmbiente.DataBind();
                        cmbTipoAmbiente.Items.Insert(0, new ListItem("Seleccione..!!", "0"));

                        if (cmbTipoAmbiente.Items.Count > 0)
                        {
                            cmbTipoAmbiente.SelectedValue = Application["IDTipoAmbienteFE"].ToString();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CARGAR LOS TIPOS DE EMISION
        private void llenarTipoEmision()
        {
            try
            {
                sSql = "";
                sSql = sSql + "Select id_tipo_emision, nombres" + Environment.NewLine;
                sSql = sSql + "From cel_tipo_emision" + Environment.NewLine;
                sSql = sSql + "Where estado = 'A'" + Environment.NewLine;
                sSql = sSql + "Order By id_tipo_emision";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        comboE.ISSQL = sSql;
                        cmbTipoEmision.DataSource = comboM.listarCombo(comboE);
                        cmbTipoEmision.DataValueField = "IID";
                        cmbTipoEmision.DataTextField = "IDATO";
                        cmbTipoEmision.DataBind();
                        cmbTipoEmision.Items.Insert(0, new ListItem("Seleccione..!!", "0"));

                        if (cmbTipoEmision.Items.Count > 0)
                        {
                            cmbTipoEmision.SelectedIndex = 1;
                            //cmbTipoEmision.Enabled = false;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CARGAR LOS TIPOS DE CERTIFICADOS DIGITALES
        private void llenarCertificadoDigital()
        {
            try
            {
                sSql = "";
                sSql = sSql + "Select id_tipo_certificado_digital, nombres" + Environment.NewLine;
                sSql = sSql + "From cel_tipo_certificado_digital" + Environment.NewLine;
                sSql = sSql + "Where estado = 'A'" + Environment.NewLine;
                sSql = sSql + "Order By id_tipo_certificado_digital";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        comboE.ISSQL = sSql;
                        cmbTipoCertificadoDigital.DataSource = comboM.listarCombo(comboE);
                        cmbTipoCertificadoDigital.DataValueField = "IID";
                        cmbTipoCertificadoDigital.DataTextField = "IDATO";
                        cmbTipoCertificadoDigital.DataBind();
                        cmbTipoCertificadoDigital.Items.Insert(0, new ListItem("Seleccione..!!", "0"));

                        if (cmbTipoCertificadoDigital.Items.Count > 0)
                        {
                            cmbTipoCertificadoDigital.SelectedIndex = 1;
                            //cmbTipoCertificadoDigital.Enabled = false;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA COMPLETAR LA INFORMACION DEL FORMULARIO 
        private void llenarInformacionFactura(int iId)
        {
            try
            {
                sSql = "";
                sSql = sSql + "select * from cel_vw_infofactura where id_factura = " + iId;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sCodigoDocumento = dtConsulta.Rows[0].ItemArray[6].ToString();
                        sNumeroDocumento = "";

                        if (sCodigoDocumento == "01")
                        {
                            sNumeroDocumento = sNumeroDocumento + "F";
                        }

                        else if (sCodigoDocumento == "04")
                        {
                            sNumeroDocumento = sNumeroDocumento + "NC";
                        }

                        else if (sCodigoDocumento == "07")
                        {
                            sNumeroDocumento = sNumeroDocumento + "R";
                        }

                        else if (sCodigoDocumento == "06")
                        {
                            sNumeroDocumento = sNumeroDocumento + "G";
                        }

                        else if (sCodigoDocumento == "05")
                        {
                            sNumeroDocumento = sNumeroDocumento + "ND";
                        }

                        sNumeroDocumento = sNumeroDocumento + dtConsulta.Rows[0].ItemArray[7].ToString() +
                                           dtConsulta.Rows[0].ItemArray[8].ToString() +
                                           dtConsulta.Rows[0].ItemArray[9].ToString().PadLeft(9, '0');

                        txtNumeroDocumento.Text = sNumeroDocumento;

                        //CONSULTAR SI EXISTE EL ARCHIVO EN LA RUTA CONFIGURADA
                        sRutaArchivo = firmarXML.GFun_St_Ruta_Archivo(sCodigoDocumento, 1) + @"\" + sNumeroDocumento + ".xml";

                        if (File.Exists(sRutaArchivo))
                        {
                            txtDocumentoFirmar.Text = sNumeroDocumento + ".xml";
                            txtRutaGenerados.Text = firmarXML.GFun_St_Ruta_Archivo(sCodigoDocumento, 1);
                            txtRutaFirmados.Text = firmarXML.GFun_St_Ruta_Archivo(sCodigoDocumento, 2);
                        }

                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No existe ruta del archivo', 'danger');", true);
                            //ok.LblMensaje.Text = "No existe el archivo en la ruta:" + Environment.NewLine + sRutaArchivo;
                            //ok.ShowDialog();
                        }
                    }
                }

                else
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + sSql.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //INSTRUCCIONES PARA FIRMAR EL DOCUMENTO XML
        private void firmarArchivoXML()
        {
            try
            {
                string sArchivoFirmar;

                string sJar;
                string sCertificado;
                string sPassCertificado;
                string sXmlIn;
                string sXmlPathOut;
                string sFileOut;
                string sCodigoError = "";
                string sDescripcionError = "";
                string[] sCertificado_digital = new string[5];

                firmarXML.Gsub_trae_parametros_certificado(sCertificado_digital);
                sCertificado = sCertificado_digital[0];
                sPassCertificado = sCertificado_digital[1];


                sArchivoFirmar = txtRutaGenerados.Text + @"\" + txtDocumentoFirmar.Text;
                
                sJar = @"c:\SRI.jar";
                sXmlIn = txtRutaGenerados.Text + @"\" + txtDocumentoFirmar.Text;
                sXmlPathOut = txtRutaFirmados.Text + @"\";
                sFileOut = txtDocumentoFirmar.Text;

                sCodigoError = firmarXML.GSub_FirmarXML(sJar, sCertificado, sPassCertificado, sXmlIn, sXmlPathOut, sFileOut, sCodigoError, sDescripcionError);
                

                if (sCodigoError == "00")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_MSJ_TITULO_EXITO + "', '" + Resources.MESSAGES.TXT_MSJ_BODY_EXITO_SIGN_XML + "', 'success');", true);
                    Limpiar();
                }

                else
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + sCodigoError.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

        fin: { }
        }

        //CARGAR MODAL BUSCAR FACTURAS QUE TENGAN CLAVE DE ACCESO
        public bool CargarDatosModal(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_facturas_paso_a_paso" + Environment.NewLine;
                sSql += "where clave_acceso <> ''" + Environment.NewLine;
                //sSql += "and id_localidad = " + Application["idLocalidad"].ToString() + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and numero_factura = " + Convert.ToInt32(txtFiltrarModalBuscarXML.Text.Trim());
                }

                columnaGrid(true);
                generarXMLE.ISQL = sSql;
                dgvFiltrarModalBuscarXML.DataSource = generarXMLM.listarFacturasEmitidas(generarXMLE);
                dgvFiltrarModalBuscarXML.DataBind();
                columnaGrid(false);
                
                //dtConsulta = new DataTable();
                //dtConsulta.Clear();

                //bRespuesta = conexion.consultarRegistro(sSql, dtConsulta);

                //if (bRespuesta == true)
                //{
                //    if (dtConsulta.Rows.Count > 0)
                //    {
                //        ColumnasCargarDatosModal(true);
                //        dgvFiltrarModalBuscarXML.DataSource = dtConsulta;
                //        dgvFiltrarModalBuscarXML.DataBind();
                //        ColumnasCargarDatosModal(false);
                //    }
                //}
                return true;
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA OCULTAR LAS COLUMNAS
        private void columnaGrid(bool ok)
        {
            dgvFiltrarModalBuscarXML.Columns[1].Visible = ok;
            //dgvFiltrarModalBuscarFacturas.Columns[5].Visible = ok;
        }

        //COLUMNAS CargarDatosModal
        public void ColumnasCargarDatosModal(bool ok)
        {
            dgvFiltrarModalBuscarXML.Columns[0].Visible = ok;
            dgvFiltrarModalBuscarXML.Columns[7].Visible = ok;
            dgvFiltrarModalBuscarXML.Columns[8].Visible = ok;
            dgvFiltrarModalBuscarXML.Columns[9].Visible = ok;
            dgvFiltrarModalBuscarXML.Columns[10].Visible = ok;
            dgvFiltrarModalBuscarXML.Columns[11].Visible = ok;
        }

        //LIMPIAR
        public void Limpiar()
        {
            txtClaveAcceso.Text = "";
            txtNumeroDocumento.Text = "";
            txtRutaGenerados.Text = "";
            txtRutaFirmados.Text = "";
            txtDocumentoFirmar.Text = "";
            txtClaveAcceso.Focus();

            Session["Id_Factura_Modal"] = null;
        }
        #endregion

        protected void lbtnBuscarXML_Click(object sender, EventArgs e)
        {
            if (CargarDatosModal(0) != true) return;
            ModalBuscarXML.Show();
            txtFiltrarModalBuscarXML.Text = "";
            txtFiltrarModalBuscarXML.Focus();
        }
        //FUNCION PARA TRAER DATOS DE XML ESTE ES EL OK DE WINFORMS
        public bool TraerXml(int Numerofactura)
        {
            try
            {
                //iIdFactura = dB_Ayuda_Facturas.iId;//ID DE FACTURA
                //iIdFactura = 14208;

                if (Numerofactura == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información!', 'Por favor, eliga un XML para firmar', 'info');", true);                    
                }

                else
                {
                    sSql = "";
                    sSql = sSql + "select " + conexion.esNulo() + "(clave_acceso, 'NINGUNA') clave_acceso " + Environment.NewLine;
                    sSql = sSql + "from cv403_facturas " + Environment.NewLine;
                    sSql = sSql + "where id_factura = " + Numerofactura;

                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    bRespuesta = conexion.consultarRegistro(sSql, dtConsulta);

                    if (bRespuesta == true)
                    {
                        if (dtConsulta.Rows.Count > 0)
                        {
                            if (dtConsulta.Rows[0].ItemArray[0].ToString() == "NINGUNA")
                            {
                                //ok.LblMensaje.Text = "No ha generado el archivo xml. Favor vuelva a generar.";
                                //ok.ShowDialog();
                            }

                            else
                            {
                                txtClaveAcceso.Text = dtConsulta.Rows[0].ItemArray[0].ToString();
                                llenarInformacionFactura(Numerofactura);
                            }
                        }

                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'NO existe un registro de factura', 'danger');", true);
                            //ok.LblMensaje.Text = "No existe un registro de factura. Comuníquese con el administrador.";
                            //ok.ShowDialog();
                        }
                    }

                    else
                    {
                        lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + sSql.ToString();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    }

                }
                return true;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        protected void cmbTipoComprobante_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbTipoComprobante.SelectedValue == "0")
                {
                    //ok.LblMensaje.Text = "No ha seleccionado el tipo de comprobante para recuperar los directorios.";
                    //ok.ShowDialog();
                }

                else
                {
                    if (Convert.ToInt32(cmbTipoComprobante.SelectedValue) == 1)
                    {
                        sCodigoDocumento = "01";
                    }

                    else if (Convert.ToInt32(cmbTipoComprobante.SelectedValue) == 2)
                    {
                        sCodigoDocumento = "04";
                    }

                    else if (Convert.ToInt32(cmbTipoComprobante.SelectedValue) == 3)
                    {
                        sCodigoDocumento = "05";
                    }

                    else if (Convert.ToInt32(cmbTipoComprobante.SelectedValue) == 4)
                    {
                        sCodigoDocumento = "06";
                    }

                    else if (Convert.ToInt32(cmbTipoComprobante.SelectedValue) == 5)
                    {
                        sCodigoDocumento = "07";
                    }

                    //COMENTADO
                    //lblTipoDocumento.Text = cmbTipoComprobante.Text;
                    //llenarInstruccionesSQL();
                }
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnFirmarXML_Click(object sender, EventArgs e)
        {
            if (Session["Id_Factura_Modal"] == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_ADVERTENCIA + " ', 'Debe elegir un Documento para Firmar', 'warning');", true);                
            }

            else
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "<script>Firmar();</script>", false);
                //txtRutaFirmados.Text = "EXITOOOOO FIRMADOOOO :)";
                firmarArchivoXML();
            }
        }

        protected void lbtnSeleccion_Click(object sender, EventArgs e)
        {
            sAccion = "S";
        }

        protected void dgvFiltrarModalBuscarXML_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sAccion == "S")
            {
                try
                {
                    int a = dgvFiltrarModalBuscarXML.SelectedIndex;
                    columnaGrid(true);
                    Session["Id_Factura_Modal"] = dgvFiltrarModalBuscarXML.Rows[a].Cells[1].Text.Trim();//ESTO ES PARA VALIDAR
                    //TraerXml(Convert.ToInt32(Session["Id_Factura_Modal"]));
                    txtClaveAcceso.Text = dgvFiltrarModalBuscarXML.Rows[a].Cells[5].Text.Trim();
                    llenarInformacionFactura(Convert.ToInt32(Session["Id_Factura_Modal"].ToString()));
                    ModalBuscarXML.Hide();
                    columnaGrid(true);
                }
                catch (Exception ex)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }
        }

        protected void lbtnCerrarModalBuscarXML_Click(object sender, EventArgs e)
        {
            ModalBuscarXML.Hide();
            Limpiar();
        }

        protected void lbtnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        protected void dgvFiltrarModalBuscarXML_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvFiltrarModalBuscarXML.PageIndex = e.NewPageIndex;

                if (txtFiltrarModalBuscarXML.Text.Trim() == "")
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

        protected void btnFiltarModalBuscarXML_Click(object sender, EventArgs e)
        {
            if (txtFiltrarModalBuscarXML.Text.Trim() == "")
            {
                CargarDatosModal(0);
            }

            else
            {
                CargarDatosModal(1);
            }
        }

        protected void dgvFiltrarModalBuscarXML_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < dgvFiltrarModalBuscarXML.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvFiltrarModalBuscarXML.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvFiltrarModalBuscarXML.Rows[i].BackColor = Color.White;
                }
            }
        }
    }
}