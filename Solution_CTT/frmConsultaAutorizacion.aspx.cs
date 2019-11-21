using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Data;
using ENTIDADES;
using NEGOCIO;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmConsultaAutorizacion : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();
        manejadorGenerarXML generarXMLM = new manejadorGenerarXML();

        ENTComboDatos comboE = new ENTComboDatos();
        ENTGenerarXML generarXMLE = new ENTGenerarXML();

        manejadorComboDatos comboM = new manejadorComboDatos();

        Clases_Factura_Electronica.ClaseConsultarXML consultarXML = new Clases_Factura_Electronica.ClaseConsultarXML();
        Clases_Factura_Electronica.ClaseXMLAyuda xmlH = new Clases_Factura_Electronica.ClaseXMLAyuda();

        DataTable dtConsulta;
        string sSql;
        string[] sDatosMaximo = new string[5];
        string sAccion;

        bool bRespuesta = false;

        string sAyuda;
        //string sCodigoDocumento;
        string sNumeroDocumento;
        string sRutaArchivo;
        string sRutaXmlFirmado;
        //string miXMl;
        string sFecha;

        string sVersion = "1.0";
        string sUTF = "utf-8";
        string sStandAlone = "yes";

        XDocument xml;
        XElement autorizacion;
        XElement estado;
        XElement numeroAutorizacion;
        XElement fechaAutorizacion;
        XElement ambiente;
        XElement comprobante;

        int iCol_Correlativo;
        int iCol_Codigo;
        int iCol_Descripcion;
        int iIdFactura;

        private XmlDocument xmlDoc;

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

            Session["modulo"] = "MÓDULO DE CONSULTA DE AUTORIZACIÓN DE COMPROBANTES ELECTRÓNICOS";

            if (!IsPostBack)
            {
                //llenarInstruccionesSQL();//AQUI GENERO QUERY ARA MOSTRAR EN EL MODAL
                llenarTipoAmbiente();
                llenarTipoEmision();
                llenarTipoComprobante();
                cmbTipoComprobante_SelectedIndexChanged(sender, e);
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA RELLENAR LAS INSTRUCCIONES SQL
        //private void llenarInstruccionesSQL()
        //{
        //    sAyuda = consultarXML.GSub_ActualizaPantalla(sCodigoDocumento, 2);
        //    iCol_Correlativo = 4;
        //    iCol_Codigo = 0;
        //    iCol_Descripcion = 1;
        //    dB_Ayuda_Facturas.Ver(sAyuda, "", iCol_Correlativo, iCol_Codigo, iCol_Descripcion);
        //}

        //FUNCION CARGAR MODAL
        public bool CargarDatosModal(int iOp)
        {
            try
            {
                //sSql = "";
                //sAyuda = consultarXML.GSub_ActualizaPantalla(Session["sCodigoDocumento"].ToString(), 2);//JONA - SOLO 2 PARAMETROS XQ ESTA EN CLASE YA EL IDEMPRESA
                //sSql = sAyuda;

                //dtConsulta = new DataTable();
                //dtConsulta.Clear();

                //bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

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
            iIdFactura = 0;
            txtClaveAcceso.Text="";
            txtEstadoEnvio.Text = "";
            txtAutorizacionEstado.Text = "";
            txtNumeroAutorizacion.Text = "";
            txtFechaAutorizacion.Text = "";
            txtDetalles_1.Text = "";
            txtDetalles_2.Text = "";
            txtRutaAutorizados.Text = "";
            txtDocumentoAutorizado.Text = "";

            //dB_Ayuda_Facturas.limpiar();

            llenarTipoComprobante();
            llenarTipoAmbiente();
            llenarTipoEmision();

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

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

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

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

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

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

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

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["sCodigoDocumento"] = dtConsulta.Rows[0].ItemArray[6].ToString();
                        sNumeroDocumento = "";

                        if (Session["sCodigoDocumento"] == "01")
                        {
                            sNumeroDocumento = sNumeroDocumento + "F";
                        }

                        else if (Session["sCodigoDocumento"] == "04")
                        {
                            sNumeroDocumento = sNumeroDocumento + "NC";
                        }

                        else if (Session["sCodigoDocumento"] == "07")
                        {
                            sNumeroDocumento = sNumeroDocumento + "R";
                        }

                        else if (Session["sCodigoDocumento"] == "06")
                        {
                            sNumeroDocumento = sNumeroDocumento + "G";
                        }

                        else if (Session["sCodigoDocumento"] == "05")
                        {
                            sNumeroDocumento = sNumeroDocumento + "ND";
                        }

                        sNumeroDocumento = sNumeroDocumento + dtConsulta.Rows[0].ItemArray[7].ToString() +
                                           dtConsulta.Rows[0].ItemArray[8].ToString() +
                                           dtConsulta.Rows[0].ItemArray[9].ToString().PadLeft(9, '0');

                        txtNumeroDocumento.Text = sNumeroDocumento;

                        //CONSULTAR SI EXISTE EL ARCHIVO EN LA RUTA CONFIGURADA
                        sRutaXmlFirmado = consultarXML.GFun_St_Ruta_Archivo(Session["sCodigoDocumento"].ToString(), 2) + @"\" + sNumeroDocumento + ".xml";
                        sRutaArchivo = consultarXML.GFun_St_Ruta_Archivo(Session["sCodigoDocumento"].ToString(), 3) + @"\" + sNumeroDocumento + ".xml";

                        txtDocumentoAutorizado.Text = sNumeroDocumento + ".xml";
                        txtRutaAutorizados.Text = consultarXML.GFun_St_Ruta_Archivo(Session["sCodigoDocumento"].ToString(), 3);

                        //if (File.Exists(sRutaArchivo))
                        //{
                        //    txtArchivoAutorizado.Text = sNumeroDocumento + ".xml";
                        //    txtRutaAutorizados.Text = consultarXML.GFun_St_Ruta_Archivo(sCodigoDocumento, 3);
                        //}

                        //else
                        //{
                        //    ok.LblMensaje.Text = "No existe el archivo en la ruta:" + Environment.NewLine + sRutaArchivo;
                        //    ok.ShowDialog();
                        //}
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
                    sSql = sSql + "select " + conexionM.esNulo() + "(clave_acceso, 'NINGUNA') clave_acceso " + Environment.NewLine;
                    sSql = sSql + "from cv403_facturas " + Environment.NewLine;
                    sSql = sSql + "where id_factura = " + Numerofactura;

                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

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

        //VALIDAR XML
        private bool ValidarDocumento()
        {
            //bool result = false;

            //if (!string.IsNullOrWhiteSpace(txtClaveAcceso.Text))
            //{
                //wsH.URL_Autorizacion = LblAutorizacion.Text;
                //wsH.ClaveAcceso = TxtClave.Text;

                //Cursor.Current = Cursors.WaitCursor;

                XmlDocument xmlAut;

                try
                {
                    RespuestaSRI respuesta = null;

                    if (Application["IDTipoAmbienteFE"].ToString() == "1")
                    {
                        respuesta = consultarXML.AutorizacionComprobante(out xmlAut, txtClaveAcceso.Text, Application["WSConsultaPruebas"].ToString());
                    }

                    else
                    {
                        respuesta = consultarXML.AutorizacionComprobante(out xmlAut, txtClaveAcceso.Text, Application["WSConsultaProduccion"].ToString());
                    }


                    
                    //string respuestaStr;

                    txtEstadoEnvio.Text = respuesta.Estado;
                    txtNumeroAutorizacion.Text = respuesta.NumeroAutorizacion;
                    txtFechaAutorizacion.Text = respuesta.FechaAutorizacion;
                    txtDetalles_1.Text = respuesta.ErrorIdentificador;
                    txtDetalles_2.Text = respuesta.ErrorMensaje + Environment.NewLine + respuesta.ErrorTipo;

                    if ((txtEstadoEnvio.Text == "AUTORIZADO") || (txtEstadoEnvio.Text == "NO AUTORIZADO"))
                    {
                        //Genera y guarda el XML autorizado
                        string filename = Path.GetFileNameWithoutExtension(txtDocumentoAutorizado.Text.Trim()) + ".xml";
                        string path = txtRutaAutorizados.Text;

                        filename = Path.Combine(path, filename);

                        xmlAutorizado(respuesta, filename);

                        actualizarDatos();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                    //catchMensaje.LblMensaje.Text = ex.Message;
                    //catchMensaje.ShowInTaskbar = false;
                    //catchMensaje.ShowDialog();
                    //ClientScript.RegisterClientScriptBlock(GetType(), "mensaje", "alert('Error al usar los web services del SRI 2');", true);
                }
            //}

            //return result;
        }

        //NUEVO PROCESO PARA CONVERTIR EN BASE 64 EL ARCHIVO
        public string ConvertirString()
        {
            byte[] xmlLeido = File.ReadAllBytes(sRutaXmlFirmado);
            return Convert.ToString(xmlLeido);
        }

        //CONSTRUIR XML AUTORIZADO
        private void xmlAutorizado(RespuestaSRI sri, string filename)
        {
            try
            {
                //miXMl = File.ReadAllText(sRutaXmlFirmado);
                //Declaramos el documento y su definición
                xml = new XDocument(
                    new XDeclaration(sVersion, sUTF, sStandAlone));

                autorizacion = new XElement("autorizacion");
                autorizacion.Add(new XElement("estado", sri.Estado));
                autorizacion.Add(new XElement("numeroAutorizacion", sri.NumeroAutorizacion));
                autorizacion.Add(new XElement("fechaAutorizacion", sri.FechaAutorizacion));
                autorizacion.Add(new XElement("ambiente", sri.Ambiente));
                autorizacion.Add(new XElement("comprobante", new XCData(sri.Comprobante)));
                autorizacion.Add(new XElement("mensajes", sri.ErrorMensaje));
                xml.Add(autorizacion);

                //PROBAR COMO GUARDA
                xml.Save(filename);
            }

            catch (Exception ex)
            {
                //catchMensaje.LblMensaje.Text = ex.Message;
                //catchMensaje.ShowInTaskbar = false;
                //catchMensaje.ShowDialog();
            }
        }

        //ACTUALIZAR EN LA BASE DE DATOS
        private void actualizarDatos()
        {
            try
            {
                sFecha = txtFechaAutorizacion.Text.Substring(0, 10) + " " + txtFechaAutorizacion.Text.Substring(11, 8);

                if (txtNumeroAutorizacion.Text.Trim() != "")
                {
                    //INICIO DE INSERCION
                    if (conexionM.iniciarTransaccion() == false)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                        goto fin;
                    }

                    //UPDATE PARA FACTURAS
                    if (Session["sCodigoDocumento"].ToString() == "01")
                    {
                        sSql = "";
                        sSql = sSql + "update cv403_facturas set" + Environment.NewLine;
                        sSql = sSql + "autorizacion = '" + txtNumeroAutorizacion.Text.Trim() + "'," + Environment.NewLine;
                        sSql = sSql + "fecha_autorizacion = '" + sFecha + "'" + Environment.NewLine;
                        sSql = sSql + "where id_factura = " + Convert.ToInt32(Session["Id_Factura_Modal"]);//JONA

                        //EJECUCION DE LA INSTRUCCION SQL
                        if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                            goto reversa;
                        }
                    }

                    //UPDATE PARA COMPROBANTES DE RETENCION
                    else if (Session["sCodigoDocumento"].ToString() == "07")
                    {
                        sSql = "";
                        sSql = sSql + "update cv405_cab_comprobantes_retencion set" + Environment.NewLine;
                        sSql = sSql + "autorizacion = '" + txtNumeroAutorizacion.Text.Trim() + "'," + Environment.NewLine;
                        sSql = sSql + "fecha_autorizacion = '" + sFecha + "'" + Environment.NewLine;
                        sSql = sSql + "where id_cab_comprobante_retencion = " + Convert.ToInt32(Session["Id_Factura_Modal"]);//JONA

                        //EJECUCION DE LA INSTRUCCION SQL
                        if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                            goto reversa;
                        }
                    }

                    //UPDATE PARA NOTAS DE CREDITO
                    else if (Session["sCodigoDocumento"].ToString() == "04")
                    {
                        sSql = "";
                        sSql = sSql + "update cv403_notas_credito set" + Environment.NewLine;
                        sSql = sSql + "autorizacion = '" + txtNumeroAutorizacion.Text.Trim() + "'," + Environment.NewLine;
                        sSql = sSql + "fecha_autorizacion = '" + sFecha + "'" + Environment.NewLine;
                        sSql = sSql + "where id_nota_credito = " + Convert.ToInt32(Session["Id_Factura_Modal"]);//JONA

                        //EJECUCION DE LA INSTRUCCION SQL
                        if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                            goto reversa;
                        }
                    }

                    //UPDATE PARA GUIAS DE REMISION
                    else if (Session["sCodigoDocumento"].ToString() == "06")
                    {
                        sSql = "";
                        sSql = sSql + "update cv403_guias_remision set" + Environment.NewLine;
                        sSql = sSql + "autorizacion = '" + txtNumeroAutorizacion.Text.Trim() + "'," + Environment.NewLine;
                        sSql = sSql + "fecha_autorizacion = '" + sFecha + "'" + Environment.NewLine;
                        sSql = sSql + "where id_guia_remision = " + Convert.ToInt32(Session["Id_Factura_Modal"]);//JONA

                        //EJECUCION DE LA INSTRUCCION SQL
                        if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                            goto reversa;
                        }
                    }

                    //FINALIZACION DE INSERCION
                    conexionM.terminaTransaccion();
                    //conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);//JONA
                    //ok.LblMensaje.Text = "Registro actualizado éxitosamente.";
                    //ok.ShowInTaskbar = false;
                    //ok.ShowDialog();
                    //limpiar();
                    goto fin;
                }

                else
                {
                    //ok.LblMensaje.Text = "No ha ingresado el nombre del archivo autorizado";
                    //ok.ShowDialog();
                    goto fin;
                }
            }

            catch (Exception ex)
            {
                //catchMensaje.LblMensaje.Text = ex.Message;
                //catchMensaje.ShowInTaskbar = false;
                //catchMensaje.ShowDialog();
            }

        reversa:
            {
                conexionM.reversaTransaccion();
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

        protected void lbtnEnviarXML_Click(object sender, EventArgs e)
        {
            if (Session["Id_Factura_Modal"] == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_ADVERTENCIA + " ', 'Debe elegir un Documento para Enviar', 'warning');", true);
            }
            else
            {
                ValidarDocumento();
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