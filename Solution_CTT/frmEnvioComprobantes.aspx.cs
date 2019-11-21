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

namespace Solution_CTT
{
    public partial class frmEnvioComprobantes : System.Web.UI.Page
    {
        manejadorConexion conexion = new manejadorConexion();
        manejadorConexion conexionM = new manejadorConexion();
        manejadorGenerarXML generarXMLM = new manejadorGenerarXML();

        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        ENTGenerarXML generarXMLE = new ENTGenerarXML();

        Clases_Factura_Electronica.ClaseEnviarXML enviarXML = new Clases_Factura_Electronica.ClaseEnviarXML();

        DataTable dtConsulta;

        string sSql;
        string[] sDatosMaximo = new string[5];
        string sAccion;

        bool bRespuesta = false;
        int iIdFactura = 0;

        int iCol_Correlativo;
        int iCol_Codigo;
        int iCol_Descripcion;

        string sNumeroDocumento;
        //string sCodigoDocumento;
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

            Session["modulo"] = "MÓDULO DE ENVÍO DE COMPROBANTES ELECTRÓNICOS";

            if (!IsPostBack)
            {
                //llenarInstruccionesSQL();//AQUI GENERO QUERY ARA MOSTRAR EN EL MODAL
                llenarTipoAmbiente();
                llenarTipoEmision();
                llenarTipoComprobante();
                cmbTipoComprobante_SelectedIndexChanged(sender, e);
            }
        }

        #region VARIABLES PARA EL CONSUMO DEL WEB SERVICE

        long T_Ln_I;
        string T_St_ArchivoEnviar;

        long T_Ln_id_factura;
        string T_St_secuencial;

        string T_St_codigoError;
        string T_St_DescripcionError;
        string T_Ln_Numero_de_Decimales;

        string T_St_Archivobase64;
        string T_St_TxtEncode64;

        string strSoap;
        string strSOAPAction;
        string strWsdl;
        string strUrl;
        string StrHost;

        string T_st_Estado;
        string T_st_fechaAutorizacion;

        long T_ln_codigoError;
        string T_St_parteInicialSoap;
        string T_St_parteCentralSoap;
        string T_St_parteFinalSoap;

        string T_St_Archivo_In;
        string T_St_Archivo_Out;

        //HttpWebRequest xmlResponse = (HttpWebRequest)WebRequest.Create("");

        #endregion

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA RELLENAR LAS INSTRUCCIONES SQL AQUI DB AYUDA
        //private void llenarInstruccionesSQL()
        //{
            //sAyuda = enviarXML.GSub_ActualizaPantalla(sCodigoDocumento, 2, Convert.ToInt32(Application["idEmpresa"]));            
            //iCol_Correlativo = 4;
            //iCol_Codigo = 0;
            //iCol_Descripcion = 1;

            //dB_Ayuda_Facturas.Ver(sAyuda, "", iCol_Correlativo, iCol_Codigo, iCol_Descripcion);
        //}

        //FUNCION CARGAR MODAL
        public bool CargarDatosModal(int iOp)
        {
            try
            {
                //sSql = "";
                //sAyuda = enviarXML.GSub_ActualizaPantalla(Session["sCodigoDocumento"].ToString(), 2, Convert.ToInt32(Application["idEmpresa"]));
                //sSql = sAyuda;

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

                sSql = "";
                sSql += "select * from ctt_vw_facturas_paso_a_paso" + Environment.NewLine;
                sSql += "where clave_acceso <> ''" + Environment.NewLine;
                sSql += "and id_localidad = " + Application["idLocalidad"].ToString() + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and numero_factura = " + Convert.ToInt32(txtFiltrarModalBuscarXML.Text.Trim());
                }

                columnaGrid(true);
                generarXMLE.ISQL = sSql;
                dgvFiltrarModalBuscarXML.DataSource = generarXMLM.listarFacturasEmitidas(generarXMLE);
                dgvFiltrarModalBuscarXML.DataBind();
                columnaGrid(false);

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
        //FUNCION PARA LIMPIAR EL FORMULARIO
        private void limpiar()
        {
            txtClaveAcceso.Text = "";
            txtNumeroDocumento.Text = "";
            //txtRutaGenerados.Text = "";
            txtRutaFirmados.Text = "";
            txtDocumentoEnviar.Text = "";
            txtClaveAcceso.Focus();

            //dB_Ayuda_Facturas.limpiar();

            llenarTipoComprobante();
            llenarTipoAmbiente();
            llenarTipoEmision();

            txtMensaje.Text = "";
            txtCodigoEnvio.Text = "";
            txtTipoEnvio.Text = "";
            txtEstadoEnvio.Text = "";
            txtMensajeErrorEnvio.Text = "";
            txtInformacionAdicionalEnvio.Text = "";
            txtDetallesEnvio.Text = "";

            Session["Id_Factura_Modal"] = null;
        }

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

        //LLENAR EL COMBOBOX DE TIPOS DE AMBIENTES
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

        //LLENAR EL COMBOBOX DE TIPOS DE EMISION
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
                        Session["sCodigoDocumento"] = dtConsulta.Rows[0].ItemArray[6].ToString();
                        sNumeroDocumento = "";

                        if (Session["sCodigoDocumento"].ToString() == "01")
                        {
                            sNumeroDocumento = sNumeroDocumento + "F";
                        }

                        else if (Session["sCodigoDocumento"].ToString() == "04")
                        {
                            sNumeroDocumento = sNumeroDocumento + "NC";
                        }

                        else if (Session["sCodigoDocumento"].ToString() == "07")
                        {
                            sNumeroDocumento = sNumeroDocumento + "R";
                        }

                        else if (Session["sCodigoDocumento"].ToString() == "06")
                        {
                            sNumeroDocumento = sNumeroDocumento + "G";
                        }

                        else if (Session["sCodigoDocumento"].ToString() == "05")
                        {
                            sNumeroDocumento = sNumeroDocumento + "ND";
                        }

                        sNumeroDocumento = sNumeroDocumento + dtConsulta.Rows[0].ItemArray[7].ToString() +
                                           dtConsulta.Rows[0].ItemArray[8].ToString() +
                                           dtConsulta.Rows[0].ItemArray[9].ToString().PadLeft(9, '0');

                        txtNumeroDocumento.Text = sNumeroDocumento;

                        //CONSULTAR SI EXISTE EL ARCHIVO EN LA RUTA CONFIGURADA
                        sRutaArchivo = enviarXML.GFun_St_Ruta_Archivo(Session["sCodigoDocumento"].ToString(), 2) + @"\" + sNumeroDocumento + ".xml";

                        if (File.Exists(sRutaArchivo))
                        {
                            txtDocumentoEnviar.Text = sNumeroDocumento + ".xml";
                            txtRutaFirmados.Text = enviarXML.GFun_St_Ruta_Archivo(Session["sCodigoDocumento"].ToString(), 2);
                        }

                        else
                        {
                            //ok.LblMensaje.Text = "No existe el archivo en la ruta:" + Environment.NewLine + sRutaArchivo;
                            //ok.ShowDialog();
                        }
                    }
                }

                else
                {
                    //catchMensaje.LblMensaje.Text = sSql;
                }
            }

            catch (Exception ex)
            {
                //catchMensaje.LblMensaje.Text = ex.Message;
                //catchMensaje.ShowDialog();
            }
        }

        //ESTE ES LA FUNCION DEL CLICK DEL OK DE WINFORM
        public bool TraerXml(int Numerofactura)
        {
            try
            {
                //iIdFactura = dB_Ayuda_Facturas.iId;//ID DE FACTURA
                //iIdFactura = 14208;

                if (Numerofactura == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información!', 'Por favor, eliga un XML para enviar', 'info');", true);
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

        //ENVIAR DOCUMENTO AL SRI
        private bool EnviarDocumento()
        {
            try
            {
                //enviarComprobanteSRI();
                bool result = false;

                //T_St_ArchivoEnviar = txtRutaFirmados.Text + @"\" + txtDocumentoEnviar.Text;

                //HACER CONFIRMACION AQUI
                //SiNo.LblMensaje.Text = "¿Está seguro de enviar el archivo " + T_St_ArchivoEnviar + "?";
                //SiNo.ShowDialog();

                //if (SiNo.DialogResult != DialogResult.OK)
                //{
                //    goto fin;
                //}

                if ((txtRutaFirmados.Text.Trim() != "") && (txtNumeroDocumento.Text.Trim() != ""))
                {
                    T_St_Archivo_In = txtRutaFirmados.Text + @"\" + txtNumeroDocumento.Text + ".xml";
                    //T_St_Archivo_Out = txtRutaFirmados.Text + @"\" + txtNumeroDocumento.Text + ".txt";
                }

                //T_St_Archivobase64 = txtRutaFirmados.Text + @"\" + txtNumeroDocumento.Text + ".txt";

                RespuestaSRI respuesta = null;

                if (Application["IDTipoAmbienteFE"].ToString() == "1")
                {
                    respuesta = enviarXML.EnvioComprobante(T_St_Archivo_In, Application["WSEnvioPruebas"].ToString());
                }

                else
                {
                    respuesta = enviarXML.EnvioComprobante(T_St_Archivo_In, Application["WSEnvioProduccion"].ToString());
                }


                if (respuesta.Estado == "RECIBIDA")
                {
                    result = true;
                }

                txtEstadoEnvio.Text = respuesta.Estado;
                txtMensajeErrorEnvio.Text = respuesta.ErrorMensaje;
                txtCodigoEnvio.Text = respuesta.ErrorIdentificador;
                txtInformacionAdicionalEnvio.Text = respuesta.ErrorInfoAdicional;
                txtTipoEnvio.Text = respuesta.ErrorTipo;

                return true;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }

        fin: { }
        }

        #endregion
        
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
                    columnaGrid(false);
                    ModalBuscarXML.Hide();
                }
                catch (Exception ex)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }
        }

        protected void lbtnEnviarXML_Click(object sender, EventArgs e)
        {
            if (Session["Id_Factura_Modal"] == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_ADVERTENCIA + " ', 'Debe elegir un Documento para Enviar', 'warning');", true);
            }
            else
            {
                EnviarDocumento();
            }
        }

        protected void lbtnCancelar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void lbtnBuscarXML_Click(object sender, EventArgs e)
        {
            if (CargarDatosModal(0) != true) return;
            ModalBuscarXML.Show();
            txtFiltrarModalBuscarXML.Text = "";
            txtFiltrarModalBuscarXML.Focus();
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
                        Session["sCodigoDocumento"] = "01";
                    }

                    else if (Convert.ToInt32(cmbTipoComprobante.SelectedValue) == 2)
                    {
                        Session["sCodigoDocumento"] = "04";
                    }

                    else if (Convert.ToInt32(cmbTipoComprobante.SelectedValue) == 3)
                    {
                        Session["sCodigoDocumento"] = "05";
                    }

                    else if (Convert.ToInt32(cmbTipoComprobante.SelectedValue) == 4)
                    {
                        Session["sCodigoDocumento"] = "06";
                    }

                    else if (Convert.ToInt32(cmbTipoComprobante.SelectedValue) == 5)
                    {
                        Session["sCodigoDocumento"] = "07";
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

        protected void lbtnCerrarModalBuscarXML_Click(object sender, EventArgs e)
        {
            ModalBuscarXML.Hide();
        }
    }
}