using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using NEGOCIO;
using ENTIDADES;
using System.Data;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Threading;
using Microsoft.Reporting.WebForms;
using System.Net.Mail;
using System.Net;
using NEGOCIO;
using System.IO;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmAutorizarDocumentosPorLote : System.Web.UI.Page
    {
        List<DetalleEnvioAutorizacion> misDetalles = new List<DetalleEnvioAutorizacion>();

        manejadorConexion conexionM = new manejadorConexion();
        //ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        manejadorComboDatos comboM = new manejadorComboDatos();
        ENTComboDatos comboE = new ENTComboDatos();

        Clases_Factura_Electronica.ClaseGenerarFacturaXml generar = new Clases_Factura_Electronica.ClaseGenerarFacturaXml();
        Clases_Factura_Electronica.ClaseGenerarRIDE ride = new Clases_Factura_Electronica.ClaseGenerarRIDE();
        Clases_Factura_Electronica.ClaseFirmarXML firmarXML = new Clases_Factura_Electronica.ClaseFirmarXML();
        Clases_Factura_Electronica.ClaseEnviarXML enviarXML = new Clases_Factura_Electronica.ClaseEnviarXML();
        Clases_Factura_Electronica.ClaseConsultarXML consultarXML = new Clases_Factura_Electronica.ClaseConsultarXML();

        Clases.ClaseEnviarCorreo EnviarCorreo = new Clases.ClaseEnviarCorreo();

        string sSql;
        string sDireccion;
        string sAccion;
        string sAccionModalBuscarPersonas;
        string sImprimir;
        string sNombreImpresora;
        string sPathImpresora;
        string sFecha;
        string sTabla;
        string sCampo;
        string sCiudad;
        string sTelefono;
        string sCorreoElectronico;
        string sObservacion;
        string sNombrePersonaRecibe;
        string sFechaInicial;
        string sFechaFinal;

        string sSector;
        string sCallePrincipal;
        string sCalleSecundaria;
        string sNumeracion;
        string sReferencia;
        string sCodigoPostal;
        string sTelefonoDomicilio;
        string sTelefonoCelular;
        string sTelefonoOficina;
        string sMail;
        string sTelefonoSinDatos;

        string[] sDatosMaximo = new string[5];

        bool bRespuesta;

        DataTable dtConsulta;
        DataTable dtAlmacenar;

        int iIdProgramacion;
        int iConsulta;
        int iIdProducto;
        int iIdDestino;
        int iFragil;
        int iRespuestaConsulta;
        int iTercerDigito;
        int iLongitud;
        int iCortarPapel;
        int iNumeroImpresiones;
        int iIdPersona;
        int iIdPersonaEnvia;
        int iIdPersonaRecibe;
        int iIdPedido;
        int iNumeroPedido;
        int iIdCabDespachos;
        int iIdDespachoPedido;
        int iIdEventoCobro;
        int iIdPago;
        int iNumeroPago;
        int iCgTipoDocumento = 7456;
        int iIdDocumentoPago;
        int iIdDocumentoCobrar;
        int iIdFactura;
        int iIdFacturaPedido;
        int iNumeroFactura;
        int iEstadoEncomienda;
        int iPagaDestino;
        int iCgEstadoDctoPorCobrar;
        int iLongitudCedula;
        int iDiscapacidad;

        long iMaximo;

        double dbSumaSubtotalIva;
        double dbSumaSubtotalSinIva;
        double dbSumaSubtotal;
        double dbSumaIva;
        double dbSuma;
        double dbTotal;
        double dbPrecioUnitario;
        double dbDescuento;
        double dbCantidad;
        double dbIva;
        double dbPeso;

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

        #endregion

        //VARIABLES CONSULTAR XML
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
                llenarComboLocalidad();
                txtFechaFinal.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaInicial.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        #region FUNCIONES DEL USUARIO

        //LLENAR COMBO LOCALIDAD
        private void llenarComboLocalidad()
        {
            try
            {
                sSql = "";
                //sSql += "select id_localidad_impresora, nombre_impresora" + Environment.NewLine;
                //sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                //sSql += "where id_localidad=" + Application["idLocalidad"].ToString();

                sSql  = "";
                sSql += "select id_localidad, nombre_localidad" + Environment.NewLine;
                sSql += "from tp_vw_localidades" + Environment.NewLine;
                sSql += "where emite_comprobante_electronico = 1";

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
                        cmbLocalidad.Items.Insert(0, new ListItem("Seleccione..!!", "0"));

                        if (cmbLocalidad.Items.Count > 0)
                        {
                            cmbLocalidad.SelectedValue = Application["idLocalidad"].ToString();
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

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid(int Aux, string[] Mensaje)
        {
            try
            {
                sFechaInicial = Convert.ToDateTime(txtFechaInicial.Text.Trim()).ToString("yyyy/MM/dd");
                sFechaFinal = Convert.ToDateTime(txtFechaFinal.Text.Trim()).ToString("yyyy/MM/dd");

                sSql = "";
                sSql += "select * from ctt_vw_enviar_facturas_boleteria" + Environment.NewLine;
                sSql += "where fecha_factura between '" + sFechaInicial + "'" + Environment.NewLine;
                sSql += "and '" + sFechaFinal + "'" + Environment.NewLine;
                //sSql += "and id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString()) + Environment.NewLine;
                sSql += "and id_localidad = " + cmbLocalidad.SelectedValue + Environment.NewLine;
                sSql += "order by id_factura";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                lbRegistrosEncontrados.Text = "(" + dtConsulta.Rows.Count.ToString() + ")";

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {                        
                        Scroll.Visible = true;
                        DetalleEnvioAutorizacion miDetalle;

                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            miDetalle = new DetalleEnvioAutorizacion();
                            miDetalle.IDFactura = dtConsulta.Rows[i][0].ToString();
                            miDetalle.FechaFactura = Convert.ToDateTime(dtConsulta.Rows[i][1].ToString()).ToString("dd-MM-yyyy");
                            miDetalle.ClaveAcceso = dtConsulta.Rows[i][2].ToString();
                            miDetalle.NumeroFactura = dtConsulta.Rows[i][3].ToString() + "-" + dtConsulta.Rows[i][4].ToString() + "-" + dtConsulta.Rows[i][5].ToString().Trim().PadLeft(9, '0');
                            miDetalle.Cliente = dtConsulta.Rows[i][6].ToString();
                            miDetalle.CorreoCliente = dtConsulta.Rows[i][7].ToString();

                            if (Aux == 0)
                            {
                                miDetalle.Mensaje = "En espera...";
                            }

                            if (Aux == 1)
                            {
                                miDetalle.Mensaje = Mensaje[i];
                            }

                            misDetalles.Add(miDetalle);
                        }

                        columnasGrid(true);
                        dgvDatos.DataSource = misDetalles;
                        dgvDatos.DataBind();
                        columnasGrid(false);
                    }
                    else
                    {
                        Scroll.Visible = false;
                        dgvDatos.DataSource = misDetalles;
                        dgvDatos.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION DE LAS COLUMAS DEL GRID
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[3].Visible = ok;
            dgvDatos.Columns[6].Visible = ok;
        }

        //VALIDAR
        public bool Validar()
        {
            if (string.IsNullOrEmpty(txtFechaInicial.Text) || string.IsNullOrEmpty(txtFechaFinal.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + "Advertencia...!!!" + " ', 'Debe elegir fechas válidas', 'warning');", true);
                return false;
            }
            else
            {
                return true;
            }
        }

        //CREACION DE DOCUMENTOS ELECTRONICOS

        //GENERAR XML
        private bool CrearXML(int IDFactura, string NombreClienteFactura)
        {
            if (string.IsNullOrEmpty(Application["RutaFacturasGeneradas"].ToString()))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se encontro Directorio de Facturas Generadas', 'danger');", true);
                return false;
            }

            int iNumeroDecimales = Convert.ToInt32(Application["configuracion_decimales"].ToString());

            if (NombreClienteFactura != "CONSUMIDOR FINAL")//PREGUNTO SI ES CONSUMIDOR FINAL O CLIENTE, PARA OBTENER CORREO CLIENTE
            {
                sSql = "";
                sSql += "select F.id_persona, isnull(P.correo_electronico, '') correo_electronico from" + Environment.NewLine;
                sSql += "cv403_facturas F" + Environment.NewLine;
                sSql += "inner join tp_personas P on F.id_persona = P.id_persona" + Environment.NewLine;
                sSql += "and P.estado='A' and F.estado='A'" + Environment.NewLine;
                sSql += "where F.id_factura = " + IDFactura;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        try
                        {
                            if (dtConsulta.Rows[0][1].ToString().Trim() != "")
                            {
                                generar.GSub_GenerarFacturaXML(IDFactura, 0, Application["IDTipoEmisionFE"].ToString(), Application["IDTipoAmbienteFE"].ToString(), Application["RutaFacturasGeneradas"].ToString(), "FACTURA", iNumeroDecimales, Application["correo_default"].ToString(), dtConsulta.Rows[0][1].ToString());
                            }
                            else
                            {
                                generar.GSub_GenerarFacturaXML(IDFactura, 0, Application["IDTipoEmisionFE"].ToString(), Application["IDTipoAmbienteFE"].ToString(), Application["RutaFacturasGeneradas"].ToString(), "FACTURA", iNumeroDecimales, Application["correo_default"].ToString(), Application["correo_default"].ToString());
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
                    return true;
                }
                else
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + "No se a podido obtener datos del cliente";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }
            }
            else
            {
                try
                {
                    generar.GSub_GenerarFacturaXML(IDFactura, 0, Application["IDTipoEmisionFE"].ToString(), Application["IDTipoAmbienteFE"].ToString(), Application["RutaFacturasGeneradas"].ToString(), "FACTURA", iNumeroDecimales, Application["correo_default"].ToString(), Application["correo_default"].ToString());
                    return true;
                }
                catch (Exception ex)
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
                }
            }
        }

        //FIRMAR XMLX
        private bool FirmarXML()
        {
            if (string.IsNullOrEmpty(Application["RutaFacturasFirmadas"].ToString()))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se encontro Directorio de Facturas Firmadas', 'danger');", true);
                return false;
            }

            try
            {
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

                sJar = @"c:\SRI.jar";

                string RutaCompetaGeneradoXML = generar.FileNameXML;//FILENAMEXML
                string[] cadena;
                cadena = RutaCompetaGeneradoXML.Split('\\');
                string nombre = cadena[2].ToString();

                sXmlIn = RutaCompetaGeneradoXML;
                sXmlPathOut = Application["RutaFacturasFirmadas"].ToString() + @"\";
                sFileOut = nombre;

                sCodigoError = firmarXML.GSub_FirmarXML(sJar, sCertificado, sPassCertificado, sXmlIn, sXmlPathOut, sFileOut, sCodigoError, sDescripcionError);

                if (sCodigoError == "00")
                {
                    //EXITO :)
                    return true;
                }

                else
                {
                    lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + sCodigoError.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
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

        //ENVIO XML
        private string[] EnviarXML()
        {
            string[] sRespuesta = new string[2];

            try
            {
                string RutaCompetaGeneradoXML = generar.FileNameXML;//FILENAMEXML
                string[] cadena;
                cadena = RutaCompetaGeneradoXML.Split('\\');
                string nombre = cadena[2].ToString();

                RespuestaSRI respuesta = null;

                if ((Application["RutaFacturasFirmadas"].ToString().Trim() != "") && (nombre.Trim() != ""))
                {
                    T_St_Archivo_In = Application["RutaFacturasFirmadas"].ToString() + @"\" + nombre;
                }

                if (Application["IDTipoAmbienteFE"].ToString() == "1")
                {
                    respuesta = enviarXML.EnvioComprobante(T_St_Archivo_In, Application["WSEnvioPruebas"].ToString());
                }

                if (Application["IDTipoAmbienteFE"].ToString() == "2")
                {
                    respuesta = enviarXML.EnvioComprobante(T_St_Archivo_In, Application["WSEnvioProduccion"].ToString());
                }

                sRespuesta[0] = respuesta.Estado.Trim().ToUpper();
                sRespuesta[1] = respuesta.ErrorIdentificador.Trim().ToUpper();

                return sRespuesta;

                //if (respuesta.Estado == "RECIBIDA")
                //{
                //    return "RECIBIDA";
                //}
                //else
                //{
                //    return respuesta.ErrorIdentificador + " " + respuesta.ErrorInfoAdicional + " " + respuesta.ErrorMensaje + " " + respuesta.ErrorTipo;
                //}
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);

                sRespuesta[0] = "ERROR";
                sRespuesta[1] = "0";

                return sRespuesta;
            }
        }

        //CONSULTA XML
        private string ConsultarXML(int IDFactura, string ClaveAccesso)
        {
            XmlDocument xmlAut;

            string RutaCompetaGeneradoXML = generar.FileNameXML;//FILENAMEXML
            string[] cadena;
            cadena = RutaCompetaGeneradoXML.Split('\\');
            string nombre = cadena[2].ToString();

            RespuestaSRI respuesta = null;
            //string ClaveAccesso = generar.ClaveAccesoXML;//CLAVE DE ACCESO

            try
            {
                if (Application["IDTipoAmbienteFE"].ToString() == "1")
                {
                    respuesta = consultarXML.AutorizacionComprobante(out xmlAut, ClaveAccesso, Application["WSConsultaPruebas"].ToString());
                }

                if (Application["IDTipoAmbienteFE"].ToString() == "2")
                {
                    respuesta = consultarXML.AutorizacionComprobante(out xmlAut, ClaveAccesso, Application["WSConsultaProduccion"].ToString());
                }

                string Estado = respuesta.Estado;
                string NumeroAutorizacion = respuesta.NumeroAutorizacion;
                string FechaAutorizacion = respuesta.FechaAutorizacion;
                string ErrorIdentificador = respuesta.ErrorIdentificador;
                string ErrorMensajeTipo = respuesta.ErrorMensaje + Environment.NewLine + respuesta.ErrorTipo;

                if (Estado == "AUTORIZADO")
                {
                    //Genera y guarda el XML autorizado
                    string filename = nombre;
                    string path = Application["RutaFacturasAutorizadas"].ToString();
                    filename = path + @"\" + filename;

                    xmlAutorizado(respuesta, filename);//AQUI AGUARDA EL XML

                    ActualizarAutorizacionFactura(IDFactura, FechaAutorizacion, NumeroAutorizacion, ClaveAccesso);

                    //return Estado;
                }
                if (Estado == "NO AUTORIZADO")
                {
                    //Genera y guarda el XML no autorizado
                    string filename = nombre;
                    string path = Application["RutaFacturasNoAutorizadas"].ToString();
                    filename = path + @"\" + filename;

                    xmlAutorizado(respuesta, filename);//AQUI AGUARDA EL XML

                    //return Estado;
                }

                //return Estado + " " + ErrorIdentificador + " " + ErrorMensajeTipo;
                return Estado;
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return "ERROR";
            }
        }

        //GUARDAR XML, AUTORIZADO O NO AUTORIZADA
        private bool xmlAutorizado(RespuestaSRI sri, string filename)
        {
            try
            {
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

                xml.Save(filename);
                return true;
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //ACTUALIZAR EN LA BASE DE DATOS
        private void ActualizarAutorizacionFactura(int IDFactura, string FechaAutorizacion, string NumeroAutorizacion, string ClaveAccesso)
        {
            try
            {
                sFecha = FechaAutorizacion.Substring(0, 10) + " " + FechaAutorizacion.Substring(11, 8);

                if (NumeroAutorizacion.Trim() != "")
                {
                    //INICIO DE INSERCION
                    if (conexionM.iniciarTransaccion() == false)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                        goto fin;
                    }

                    string CodigoTipoDocumento = ClaveAccesso.Substring(8, 2);//SACO EL TIPO DOCUMENTO DE LA CLAVE DE ACCESO

                    //UPDATE PARA FACTURAS
                    if (CodigoTipoDocumento == "01")
                    {
                        sSql = "";
                        sSql = sSql + "update cv403_facturas set" + Environment.NewLine;
                        sSql = sSql + "autorizacion = '" + NumeroAutorizacion.Trim() + "'," + Environment.NewLine;
                        sSql = sSql + "fecha_autorizacion = '" + sFecha + "'" + Environment.NewLine;
                        sSql = sSql + "where id_factura = " + IDFactura;//JONA

                        //EJECUCION DE LA INSTRUCCION SQL
                        if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Error al ejecutar instruccion sql', 'danger');", true);
                            goto reversa;
                        }
                    }

                    //UPDATE PARA COMPROBANTES DE RETENCION
                    else if (CodigoTipoDocumento == "07")
                    {
                        sSql = "";
                        sSql = sSql + "update cv405_cab_comprobantes_retencion set" + Environment.NewLine;
                        sSql = sSql + "autorizacion = '" + NumeroAutorizacion + "'," + Environment.NewLine;
                        sSql = sSql + "fecha_autorizacion = '" + sFecha + "'" + Environment.NewLine;
                        sSql = sSql + "where id_cab_comprobante_retencion = " + IDFactura;//JONA

                        //EJECUCION DE LA INSTRUCCION SQL
                        if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Error al ejecutar instruccion sql', 'danger');", true);
                            goto reversa;
                        }
                    }

                    //UPDATE PARA NOTAS DE CREDITO
                    else if (CodigoTipoDocumento == "04")
                    {
                        sSql = "";
                        sSql = sSql + "update cv403_notas_credito set" + Environment.NewLine;
                        sSql = sSql + "autorizacion = '" + NumeroAutorizacion + "'," + Environment.NewLine;
                        sSql = sSql + "fecha_autorizacion = '" + sFecha + "'" + Environment.NewLine;
                        sSql = sSql + "where id_nota_credito = " + IDFactura;//JONA

                        //EJECUCION DE LA INSTRUCCION SQL
                        if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Error al ejecutar instruccion sql', 'danger');", true);
                            goto reversa;
                        }
                    }

                    //UPDATE PARA GUIAS DE REMISION
                    else if (CodigoTipoDocumento == "06")
                    {
                        sSql = "";
                        sSql = sSql + "update cv403_guias_remision set" + Environment.NewLine;
                        sSql = sSql + "autorizacion = '" + NumeroAutorizacion + "'," + Environment.NewLine;
                        sSql = sSql + "fecha_autorizacion = '" + sFecha + "'" + Environment.NewLine;
                        sSql = sSql + "where id_guia_remision = " + IDFactura;//JONA

                        //EJECUCION DE LA INSTRUCCION SQL
                        if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Error al ejecutar instruccion sql', 'danger');", true);
                            goto reversa;
                        }
                    }
                    //FINALIZACION DE INSERCION
                    conexionM.terminaTransaccion();
                    goto fin;
                }

                else
                {
                    goto fin;
                }
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

        reversa:
            {
                conexionM.reversaTransaccion();
            }

        fin: { }
        }
        
        //CREAR PDF SIN VISOR DE REPORTVIEWER
        private bool RenderReport(int IDFactura)
        {
            try
            {
                long NumFactura = Convert.ToInt64(IDFactura);
                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.generaRIDE(dtConsulta, NumFactura);

                LocalReport localReport = new LocalReport();
                localReport.ReportPath = Server.MapPath("~/Reportes/rptGenerarRIDE.rdlc");
                ReportDataSource rdsCabecera = new ReportDataSource("DSReportes", dtConsulta);
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

                string RutaCompetaGeneradoXML = generar.FileNameXML;//FILENAMEXML
                string[] cadena;
                cadena = RutaCompetaGeneradoXML.Split('\\');
                string nombre = cadena[2].ToString();
                string SoloNombre = nombre.Substring(0, 16);

                string Path = Application["RutaFacturasAutorizadas"].ToString();
                string filePath = Path + @"\" + SoloNombre + ".pdf";

                //string filePath = @"C:\facturasautorizadas\F001001000000407.pdf";//ESTO ES PARA HACER DIRECTO EL LA RENDERIZACION

                System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
                fs.Write(renderedBytes, 0, renderedBytes.Length);
                fs.Close();

                return true;

                //lblReporte.Text = "<object id=\"objPdf\" type=\"application/pdf\"  data=\"" 
                //         + nombre + "\" style=\"width: 980px; height: 100%;\" >  ERROR (no 
                //         puede mostrarse el objeto)</object>";
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }
        public string destinatarioCorreo;
        public string ccCorreo="";
        public string clienteCorreo;
        
        //LLENAR DATOS DEL EMISOR 
        private bool EnviarCorreoCliente(int IDFactura)
        {
            try
            {
                    sSql = "";
                    sSql = sSql + "select correo_que_envia, correo_palabra_clave, correo_smtp," + Environment.NewLine;
                    sSql = sSql + "correo_puerto, correo_con_copia, correo_consumidor_final," + Environment.NewLine;
                    sSql = sSql + "correo_ambiente_prueba, wsdl_pruebas, url_pruebas," + Environment.NewLine;
                    sSql = sSql + "wsdl_produccion, url_produccion, certificado_ruta," + Environment.NewLine;
                    sSql = sSql + "certificado_palabra_clave, Estado, id_cel_parametro, maneja_SSL" + Environment.NewLine;
                    sSql = sSql + "from cel_parametro" + Environment.NewLine;
                    sSql = sSql + "where estado ='A'";

                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                    if (bRespuesta == true)
                    {
                        if (dtConsulta.Rows.Count > 0)
                        {
                            string host = dtConsulta.Rows[0][2].ToString();
                            int puerto = Convert.ToInt32(dtConsulta.Rows[0][3].ToString());
                            string remitente = dtConsulta.Rows[0][0].ToString();
                            string contraseña = dtConsulta.Rows[0][1].ToString();
                            string nombre = "EXPRESS ATENAS SA";
                            string asunto = "FACTURACION ELECTRONICA";
                            string bcc = dtConsulta.Rows[0][4].ToString();//COPIA OCULTA
                            //string destinatarios;
                            //string cliente;
                            //string cc;

                            if (Convert.ToInt32(Application["IDTipoAmbienteFE"]) == 1)//PRUEBAS
                            {
                                destinatarioCorreo = dtConsulta.Rows[0][6].ToString();//CLIENTE DE PRUEBA
                                clienteCorreo = "Cliente de Prueba";
                            }

                            if (Convert.ToInt32(Application["IDTipoAmbienteFE"]) == 2)//PRODUCCION
                            {
                                sSql = "";
                                sSql = sSql + "select id_persona from cv403_facturas where id_factura=" + IDFactura;

                                dtConsulta = new DataTable();
                                dtConsulta.Clear();
                                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                                if (bRespuesta == true)
                                {
                                    if (dtConsulta.Rows.Count > 0)
                                    {
                                        string IDPersonas = dtConsulta.Rows[0][0].ToString();

                                        sSql = "";
                                        sSql += "select apellidos + ' ' + nombres as nombres_cliente, correo_electronico, isnull(correo_electronico2, '') correo_electronico2 from tp_personas" + Environment.NewLine;
                                        sSector += "where id_persona = '" + IDPersonas + "'";
                                        dtConsulta = new DataTable();
                                        dtConsulta.Clear();
                                        bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                                        if (bRespuesta == true)
                                        {
                                            if (dtConsulta.Rows.Count > 0)
                                            {
                                                clienteCorreo = dtConsulta.Rows[0][0].ToString();
                                                destinatarioCorreo = dtConsulta.Rows[0][1].ToString();
                                                ccCorreo = dtConsulta.Rows[0][1].ToString();//TP_PERSONAS CAMPO CORREO_ELECTRONICO2, SI HAY SE ENVIA, SINO EN BLANCO
                                            }
                                        }
                                    }
                                }
                            }

                            string RutaCompetaGeneradoXML = generar.FileNameXML;//FILENAMEXML
                            string[] cadena;
                            cadena = RutaCompetaGeneradoXML.Split('\\');
                            string nombrecompleto = cadena[2].ToString();
                            string SoloNombre = nombrecompleto.Substring(0, 16);
                            string XML = Application["RutaFacturasAutorizadas"].ToString() + @"\" + nombrecompleto;
                            string RIDE = Application["RutaFacturasAutorizadas"].ToString() + @"\" + SoloNombre + ".pdf";

                            //string XML = Application["RutaFacturasAutorizadas"].ToString() + @"\F001001000000433.xml";//PROBAR ENVIO DE CORREO
                            //string RIDE = Application["RutaFacturasAutorizadas"].ToString() + @"\F001001000000433.pdf";//PROBAR ENVIO DE CORREO

                            string adjuntos = XML + "|" + RIDE;
                            string cuerpo = "Saludos estimad@ " + clienteCorreo + Environment.NewLine;
                            cuerpo += "hemos emitido un comprobante electronico a su correo." + Environment.NewLine;
                            cuerpo += "Adjuntamos a continuacion, saludos";
                            int iEnableSSL = Convert.ToInt32(dtConsulta.Rows[0][15].ToString());

                            EnviarCorreo.enviarCorreo(host, puerto, remitente, contraseña, nombre, destinatarioCorreo, ccCorreo, bcc, asunto, adjuntos, cuerpo, iEnableSSL);
                        }
                        else
                        {
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

        //FUNCION PARA ELIMINAR LOS FICHEROS DE LOS DIRECTORIOS
        private void eliminarFicheros()
        {
            try
            {
                string[] xmlGenerados = Directory.GetFiles(Application["RutaFacturasGeneradas"].ToString());
                string[] xmlFirmados = Directory.GetFiles(Application["RutaFacturasFirmadas"].ToString());
                string[] xmlAutorizados = Directory.GetFiles(Application["RutaFacturasAutorizadas"].ToString());
                string[] xmlNoAutorizados = Directory.GetFiles(Application["RutaFacturasNoAutorizadas"].ToString());

                foreach (string archivo in xmlGenerados)
                {
                    File.Delete(archivo);
                }

                foreach (string archivo in xmlFirmados)
                {
                    File.Delete(archivo);
                }

                foreach (string archivo in xmlAutorizados)
                {
                    File.Delete(archivo);
                }

                foreach (string archivo in xmlNoAutorizados)
                {
                    File.Delete(archivo);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        protected void lbtnExtraerDocumentosXML_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;
            string[] vacio = new string[1];
            llenarGrid(0, vacio);
        }

        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow head = dgvDatos.HeaderRow;
            CheckBox chkTodos = head.FindControl("chkTodos") as CheckBox;
            if (chkTodos.Checked == true)
            {
                foreach (GridViewRow row in dgvDatos.Rows)
                {
                    CheckBox check = row.FindControl("chkSeleccionar") as CheckBox;
                    check.Checked = true;
                }
            }
            else
            {
                foreach (GridViewRow row in dgvDatos.Rows)
                {
                    CheckBox check = row.FindControl("chkSeleccionar") as CheckBox;
                    check.Checked = false;
                }
            }
        }

        protected void lbtnAutorizarXML_Click(object sender, EventArgs e)
        {
            if (dgvDatos.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No hay facturas cargadas para proceder con la autorización con el SRI.', 'info');", true);
                return;
            }

            int a = 0;

            foreach (GridViewRow row in dgvDatos.Rows)
            {
                CheckBox check = row.FindControl("chkSeleccionar") as CheckBox;
                if (check.Checked == true) a = a + 1;
            }

            if (a == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor seleccione los comprobantes a enviar al SRI para su autorización.', 'info');", true);
                return;
            }

            int iIdFacturaSincronizar;
            string sClaveAccesoSincronizar;
            string[] resultado;
            string NombreClienteSincronizar;

            //ELIMINAMOS LOS FICHEROS EXISTENTES
            eliminarFicheros();

            foreach (GridViewRow row in dgvDatos.Rows)
            {
                CheckBox check = row.FindControl("chkSeleccionar") as CheckBox;
                columnasGrid(true);
                iIdFacturaSincronizar = Convert.ToInt32(row.Cells[1].Text);
                NombreClienteSincronizar = row.Cells[4].Text;
                sClaveAccesoSincronizar = row.Cells[3].Text;
                columnasGrid(false);

                if (check.Checked == true)
                {
                    //GENERAR UN XML
                    if (!CrearXML(iIdFacturaSincronizar, NombreClienteSincronizar))
                    {
                        row.Cells[7].Text = "XML No Generado";
                        return;
                    }

                    else
                    {
                        row.Cells[7].Text = "XML Generado";
                        CheckBox chkGenerado = row.FindControl("chkGenerado") as CheckBox;
                        chkGenerado.Checked = true;
                    }

                    //FIRMAR UN XML
                    if (!FirmarXML())
                    {
                        row.Cells[7].Text = "XML No Firmado";
                        return;
                    }

                    else
                    {
                        row.Cells[7].Text = "XML Firmado";
                        CheckBox chkFirmado = row.FindControl("chkFirmado") as CheckBox;
                        chkFirmado.Checked = true;
                    }

                    //ENVIAR UN XML
                    Thread.Sleep(5000);
                    resultado = EnviarXML();

                    if (resultado[0] == "RECIBIDA")
                    {
                        row.Cells[7].Text = "XML Enviado SRI";
                        CheckBox chkEnviado = row.FindControl("chkEnviado") as CheckBox;
                        chkEnviado.Checked = true;

                        Thread.Sleep(2000);

                        resultado[0] = ConsultarXML(iIdFacturaSincronizar, sClaveAccesoSincronizar);//ENVIO CLAVE DE ACCESO

                        if (resultado[0] == "AUTORIZADO")
                        {
                            row.Cells[7].Text = "Factura Autorizada";
                            CheckBox chkAutorizado = row.FindControl("chkAutorizado") as CheckBox;
                            chkAutorizado.Checked = true;
                        }

                        else
                        {
                            row.Cells[7].Text = "Factura No Autorizada";
                            CheckBox chkAutorizado = row.FindControl("chkAutorizado") as CheckBox;
                            chkAutorizado.Checked = false;
                        }

                        Thread.Sleep(2000);
                    }

                    else
                    {
                        if (resultado[1] == "43")
                        {
                            row.Cells[7].Text = "XML ya registrado SRI";
                            CheckBox chkEnviado = row.FindControl("chkEnviado") as CheckBox;
                            chkEnviado.Checked = true;

                            if (resultado[0] == "AUTORIZADO")
                            {
                                row.Cells[7].Text = "Factura Autorizada";
                                CheckBox chkAutorizado = row.FindControl("chkAutorizado") as CheckBox;
                                chkAutorizado.Checked = true;
                            }

                            else
                            {
                                row.Cells[7].Text = "Factura No Autorizada";
                                CheckBox chkAutorizado = row.FindControl("chkAutorizado") as CheckBox;
                                chkAutorizado.Checked = false;
                            }
                        }
                        else
                        {
                            row.Cells[7].Text = "XML no recibido SRI";
                            CheckBox chkEnviado = row.FindControl("chkEnviado") as CheckBox;
                            chkEnviado.Checked = false;
                        }
                    }
                }
            }
        }

        protected void lbtnCancelar_Click(object sender, EventArgs e)
        {
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