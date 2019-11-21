using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ENTIDADES;
using NEGOCIO;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmEmisor : System.Web.UI.Page
    {
        manejadorConexion conexion = new manejadorConexion();
        manejadorConexion conexionM = new manejadorConexion();

        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();

        //Clases.ClaseValidarRUC ruc = new Clases.ClaseValidarRUC();
        Clases.ValidarRUC ruc = new Clases.ValidarRUC();

        bool bRespuesta;
        bool bActualizar;

        DataTable dtConsulta;

        string sSql;
        string[] sDatosMaximo = new string[5];
        string sAccion;
                        
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

            Session["modulo"] = "MÓDULO DE CONFIGURACIÓN DEL EMISOR PARA COMPROBANTES ELECTRÓNICOS";

            if (!IsPostBack)
            {
                llenarTipoAmbiente();
                llenarTipoEmision();
                llenarCertificadoDigital();
                cargarInformacion();
            }
        }
        #region FUNCIONES DEL USUARIO

        //FUNCION PARA VALIDAR EL RUC
        private void validarRuc()
        {
            if (txtRUC.Text.Trim().Length == 13)
            {
                if (txtRUC.Text.Substring(2, 1) == "9")
                {
                    if (ruc.validarRucJuridico(txtRUC.Text.Trim()) == "SI")
                    {
                        txtCodigoEmpresa.Focus();
                    }

                    else
                    {
                        //ok.LblMensaje.Text = "El RUC ingresado es inválido.";
                        //ok.ShowDialog();
                        //txtRUC.Clear();
                        txtRUC.Focus();
                    }
                }

                else if (txtRUC.Text.Substring(2, 1) == "6")
                {
                    if (ruc.validarRucPublico(txtRUC.Text.Trim()) == "SI")
                    {
                        txtCodigoEmpresa.Focus();
                    }

                    else
                    {
                        //ok.LblMensaje.Text = "El RUC ingresado es inválido.";
                        //ok.ShowDialog();
                        //txtRUC.Clear();
                        txtRUC.Focus();
                    }
                }

                else
                {
                    if (ruc.validarRucNatural(txtRUC.Text.Trim()) == "SI")
                    {
                        txtCodigoEmpresa.Focus();
                    }

                    else
                    {
                        //ok.LblMensaje.Text = "El RUC ingresado es inválido.";
                        //ok.ShowDialog();
                        //txtRUC.Clear();
                        txtRUC.Focus();
                    }
                }
            }

            else
            {
                //ok.LblMensaje.Text = "El RUC ingresado es inválido.";
                //ok.ShowDialog();
                //txtRUC.Clear();
                txtRUC.Focus();
            }
        }

        //FUNCION PARA CARGAR LOS TIPOS DE AMBIENTE
        private void llenarTipoAmbiente()
        {
            try
            {
                sSql = "";
                sSql += "Select id_tipo_ambiente, nombres" + Environment.NewLine;
                sSql += "From cel_tipo_ambiente" + Environment.NewLine;
                sSql += "Where estado = 'A'" + Environment.NewLine;
                sSql += "Order By id_tipo_ambiente";

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
                sSql += "Select id_tipo_emision, nombres" + Environment.NewLine;
                sSql += "From cel_tipo_emision" + Environment.NewLine;
                sSql += "Where estado = 'A'" + Environment.NewLine;
                sSql += "Order By id_tipo_emision";

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
                sSql += "Select id_tipo_certificado_digital, nombres" + Environment.NewLine;
                sSql += "From cel_tipo_certificado_digital" + Environment.NewLine;
                sSql += "Where estado = 'A'" + Environment.NewLine;
                sSql += "Order By id_tipo_certificado_digital";

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
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //CARGAR INFORMACION DE LA BASE DE DATOS
        private void cargarInformacion()
        {
            try
            {
                sSql = "";
                sSql += "Select Codigo, razonSocial, numeroRuc, numeroPatronal," + Environment.NewLine;
                sSql += "gerenteGeneral, contadorGeneral, rucContador," + Environment.NewLine;
                sSql += "matriculaContador, sectorMunicipal, actividadEconomica," + Environment.NewLine;
                sSql += "direccionMatriz, Telefono, Fax, Ciudad, Pais, nombrecomercial," + Environment.NewLine;
                sSql += "numeroresolucioncontribuyenteespecial, obligadollevarcontabilidad," + Environment.NewLine;
                sSql += "archivologo, id_tipo_emision, tiempodeespera, id_tipo_ambiente," + Environment.NewLine;
                sSql += "id_tipo_certificado_digital, Estado, IdEmpresa" + Environment.NewLine;
                sSql += "From sis_empresa" + Environment.NewLine;
                sSql += "Where IdEmpresa=" + Convert.ToInt32(Application["idEmpresa"]) +Environment.NewLine;//JONA AGREGO
                sSql += "and Estado ='A'" + Environment.NewLine;
                sSql += "order by razonSocial";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        txtCodigoEmpresa.Text = dtConsulta.Rows[0][0].ToString();
                        txtRazonSocial.Text = dtConsulta.Rows[0][1].ToString();
                        txtRUC.Text = dtConsulta.Rows[0][2].ToString();
                        txtNumeroPatronal.Text = dtConsulta.Rows[0][3].ToString();
                        txtGerenteGeneral.Text = dtConsulta.Rows[0][4].ToString();
                        txtContadorGeneral.Text = dtConsulta.Rows[0][5].ToString();
                        txtRUCContador.Text = dtConsulta.Rows[0][6].ToString();
                        txtMatriculaContador.Text = dtConsulta.Rows[0][7].ToString();
                        txtSectorMunicipal.Text = dtConsulta.Rows[0][8].ToString();
                        txtActividadEconomica.Text = dtConsulta.Rows[0][9].ToString();
                        txtDireccionMatriz.Text = dtConsulta.Rows[0][10].ToString();
                        txtTelefono.Text = dtConsulta.Rows[0][11].ToString();
                        txtFax.Text = dtConsulta.Rows[0][12].ToString();
                        txtCiudad.Text = dtConsulta.Rows[0][13].ToString();
                        txtPais.Text = dtConsulta.Rows[0][14].ToString();
                        txtNombreComercial.Text = dtConsulta.Rows[0][15].ToString();
                        txtContribuyenteEspecial.Text = dtConsulta.Rows[0][16].ToString();

                        if (dtConsulta.Rows[0][17].ToString() == "0")
                        {
                            chkContabilidad.Checked = false;
                        }

                        else
                        {
                            chkContabilidad.Checked = true;
                        }

                        //txtLogo.Text = dtConsulta.Rows[0][18].ToString(); IMAGEN
                        //byte[] img = (byte[])dtConsulta.Rows[0][18];
                        //if (cargarImagen(img) == true)
                        //{
                        //    //ImgLogo.ImageUrl = Convert.ToBase64String(img);                            
                        //    ImgLogo.ImageUrl = "assets/images/icons/User_160x160.png";
                        //}
                        //else
                        //{
                        //    ImgLogo.ImageUrl = "assets/images/icons/User_160x160.png";
                        //}

                        cmbTipoEmision.SelectedValue = dtConsulta.Rows[0][19].ToString();
                        txtTiempoMaxEspera.Text = dtConsulta.Rows[0][20].ToString();
                        cmbTipoAmbiente.SelectedValue = dtConsulta.Rows[0][21].ToString();
                        cmbTipoCertificadoDigital.SelectedValue = dtConsulta.Rows[0][22].ToString();

                        txtEstado.Text = dtConsulta.Rows[0][23].ToString();

                        //if (dtConsulta.Rows[0][23].ToString() == "A")
                        //    cmbEstado.SelectedIndex = 0;
                        //else
                        //    cmbEstado.SelectedIndex = 1;

                        Session["iIdEmpresa"] = Convert.ToInt32(dtConsulta.Rows[0][24].ToString());
                        //bActualizar = true;
                        txtRUC.Focus();

                        //txtRUC.SelectionStart = txtRUC.Text.Trim().Length;
                    }

                    else
                    {
                        //bActualizar = false;
                        limpiar();
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }
        //CARGAR IMAGEN
        //private bool cargarImagen(byte[] imagen)
        //{
        //    try
        //    {
        //        //System.IO.MemoryStream ms = new System.IO.MemoryStream(imagen);
        //        //pbxImagen.Image = Image.FromStream(ms);
        //        ImgLogo.ImageUrl = Convert.ToBase64String(imagen);
        //        return true;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}
        //FUNCION PARA LIMPIAR LAS CAJAS DE TEXTO
        private void limpiar()
        {
            txtRUC.Text = "";
            txtCodigoEmpresa.Text = "";
            txtRazonSocial.Text = "";
            txtNombreComercial.Text = "";
            txtDireccionMatriz.Text = "";
            txtTelefono.Text = "";
            txtFax.Text = "";
            txtContribuyenteEspecial.Text = "";
            //txtLogo.Text = ""; IMAGEN
            txtTiempoMaxEspera.Text = "";
            txtNumeroPatronal.Text = "";
            txtActividadEconomica.Text = "";
            txtSectorMunicipal.Text = "";
            txtCiudad.Text = "";
            txtPais.Text = "";
            txtGerenteGeneral.Text = "";
            txtContadorGeneral.Text = "";
            txtRUCContador.Text = "";
            txtMatriculaContador.Text = "";
            chkContabilidad.Checked = false;
            txtEstado.Text = "";
            llenarTipoAmbiente();
            llenarTipoEmision();
            llenarCertificadoDigital();
            cargarInformacion();
            txtRUC.Focus();
        }

        //FUNCION PARA INSERTAR UN REGISTRO
        private void insertarRegistro()
        {
            try
            {
                //INICIAMOS UNA NUEVA TRANSACCION
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sSql = "";
                sSql += "insert into sis_empresa(" + Environment.NewLine;
                sSql += "numeroruc, codigo, razonsocial, nombrecomercial, direccionmatriz," + Environment.NewLine;
                sSql += "telefono, fax, numeroresolucioncontribuyenteespecial, archivologo," + Environment.NewLine;
                sSql += "id_tipo_emision, tiempodeespera, obligadollevarcontabilidad," + Environment.NewLine;
                sSql += "id_tipo_certificado_digital, id_tipo_ambiente, numeropatronal," + Environment.NewLine;
                sSql += "actividadeconomica, sectormunicipal, ciudad, pais, gerentegeneral," + Environment.NewLine;
                sSql += "contadorgeneral, ruccontador, matriculacontador, estado," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += "'" + txtRUC.Text.Trim() + "', '" + txtCodigoEmpresa.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtRazonSocial.Text.Trim() + "', '" + txtNombreComercial.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtDireccionMatriz.Text.Trim() + "', '" + txtTelefono.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtFax.Text.Trim() + "', ";

                if (txtContribuyenteEspecial.Text.Trim() == "")
                {
                    sSql += "null," + Environment.NewLine;
                }

                else
                {
                    sSql += Convert.ToInt32(txtContribuyenteEspecial.Text.Trim()) + "," + Environment.NewLine;
                }

                int iIdContabilidad;
                if (chkContabilidad.Checked == true){ iIdContabilidad = 1; } else { iIdContabilidad = 0; }

                //sSql += "'" + txtLogo.Text.Trim() + "', " + Convert.ToInt32(cmbTipoEmision.SelectedValue) + "," + Environment.NewLine;
                //sSql += "'" + txtLogo.Text.Trim() + "', " + Environment.NewLine; IMAGEN
                sSql += Convert.ToInt32(cmbTipoEmision.SelectedValue) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(txtTiempoMaxEspera.Text.Trim()) + ", " + iIdContabilidad + "," + Environment.NewLine;
                sSql += Convert.ToInt32(cmbTipoCertificadoDigital.SelectedValue) + ", " + Convert.ToInt32(cmbTipoAmbiente.SelectedValue) + "," + Environment.NewLine;
                sSql += "'" + txtNumeroPatronal.Text.Trim() + "', '" + txtActividadEconomica.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtSectorMunicipal.Text.Trim() + "', '" + txtCiudad.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtPais.Text.Trim() + "', '" + txtGerenteGeneral.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtContadorGeneral.Text.Trim() + "', '" + txtRUCContador.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtMatriculaContador.Text.Trim() + "', 'A', GETDATE(), " + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUTA EL QUERY DE INSERCIÓN
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_MSJ_TITULO_EXITO + "', '" + Resources.MESSAGES.TXT_MSJ_BODY_EXITO_INSERT + "', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };

        fin: { };
        }

        //FUNCION PARA ACTUALIZAR EL REGISTRO
        private void actualizarRegistro()
        {
            try
            {
                //INICIAMOS UNA NUEVA TRANSACCION
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sSql = "";
                sSql += "update sis_empresa Set" + Environment.NewLine;
                sSql += "numeroRuc = '" + txtRUC.Text.Trim() + "'," + Environment.NewLine;
                sSql += "codigo = '" + txtCodigoEmpresa.Text.Trim() + "'," + Environment.NewLine;
                sSql += "razonSocial = '" + txtRazonSocial.Text.Trim() + "'," + Environment.NewLine;
                sSql += "nombrecomercial ='" + txtNombreComercial.Text.Trim() + "'," + Environment.NewLine;
                sSql += "direccionMatriz = '" + txtDireccionMatriz.Text.Trim() + "'," + Environment.NewLine;
                sSql += "telefono = '" + txtTelefono.Text.Trim() + "', " + Environment.NewLine;
                sSql += "fax = '" + txtFax.Text.Trim() + "'," + Environment.NewLine;

                if (txtContribuyenteEspecial.Text.Trim() == "")
                {
                    sSql += "numeroresolucioncontribuyenteespecial = null," + Environment.NewLine;
                }

                else
                {
                    sSql += "numeroresolucioncontribuyenteespecial = " + Convert.ToInt32(txtContribuyenteEspecial.Text.Trim()) + "," + Environment.NewLine;
                }

                int iIdContabilidad;
                if (chkContabilidad.Checked == true) { iIdContabilidad = 1; } else { iIdContabilidad = 0; }

                //sSql += "archivologo = '" + txtLogo.Text.Trim() + "'," + Environment.NewLine;//IMAGEN
                sSql += "id_tipo_emision = " + Convert.ToInt32(cmbTipoEmision.SelectedValue) + "," + Environment.NewLine;
                sSql += "tiempodeespera = " + Convert.ToInt32(txtTiempoMaxEspera.Text.Trim()) + "," + Environment.NewLine;
                sSql += "obligadollevarcontabilidad = " + iIdContabilidad + "," + Environment.NewLine;
                sSql += "id_tipo_certificado_digital = " + Convert.ToInt32(cmbTipoCertificadoDigital.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_tipo_ambiente = " + Convert.ToInt32(cmbTipoAmbiente.SelectedValue) + "," + Environment.NewLine;
                sSql += "numeroPatronal = '" + txtNumeroPatronal.Text.Trim() + "'," + Environment.NewLine;
                sSql += "actividadEconomica = '" + txtActividadEconomica.Text.Trim() + "'," + Environment.NewLine;
                sSql += "sectorMunicipal = '" + txtSectorMunicipal.Text.Trim() + "'," + Environment.NewLine;
                sSql += "ciudad = '" + txtCiudad.Text.Trim() + "'," + Environment.NewLine;
                sSql += "pais = '" + txtPais.Text.Trim() + "'," + Environment.NewLine;
                sSql += "gerenteGeneral = '" + txtGerenteGeneral.Text.Trim() + "'," + Environment.NewLine;
                sSql += "contadorGeneral = '" + txtContadorGeneral.Text.Trim() + "'," + Environment.NewLine;
                sSql += "rucContador = '" + txtRUCContador.Text.Trim() + "'," + Environment.NewLine;
                sSql += "matriculaContador = '" + txtMatriculaContador.Text.Trim() + "'," + Environment.NewLine;
                sSql += "usuario_ingreso = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_ingreso= '" + sDatosMaximo[1] + "'," + Environment.NewLine;
                sSql += "fecha_ingreso= GETDATE()" + Environment.NewLine;
                sSql += "where idEmpresa = " + Convert.ToInt32(Session["iIdEmpresa"]);

                //EJECUTA EL QUERY DE ACTUALIZACION
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_MSJ_TITULO_EXITO + "', '" + Resources.MESSAGES.TXT_MSJ_BODY_EXITO_UPDATE + "', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };

        fin: { };
        }


        //FUNCION PARA ELIMINAR EL REGISTRO
        private void eliminarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sSql = "";
                sSql += "update sis_empresa Set" + Environment.NewLine;
                sSql += "estado = 'N'," + txtRUC.Text.Trim() + "'," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'," + Environment.NewLine;
                sSql += "fecha_anula = GetDate()" + Environment.NewLine;
                sSql += "where codigo ='" + txtCodigoEmpresa.Text.Trim() + "'";

                //EJECUTA EL QUERY DE ELIMINACION
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_MSJ_TITULO_EXITO + "', '" + Resources.MESSAGES.TXT_MSJ_BODY_EXITO_DELETE + "', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };

        fin: { };
        }

        #endregion

        protected void lbtnGuardar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["iIdEmpresa"]) != 0)
            {
                //ENVIAR A FUNCION PARA ACTUALIZAR EL REGISTRO
                actualizarRegistro();
            }
            else
            {
                //ENVIAR A FUNCION PARA INSERTAR EL REGISTRO
                insertarRegistro();
            }
        }

        protected void lbtnEliminar_Click(object sender, EventArgs e)
        {
            //eliminarRegistro();
            cargarImagen();
        }

        protected void lbtnCancelar_Click(object sender, EventArgs e)
        {
            llenarTipoAmbiente();
            llenarTipoEmision();
            llenarCertificadoDigital();
            cargarInformacion();
        }

        //IMAGEN CARGAR
        public void cargarImagen()
        {
            //int size = FuploadLogo.PostedFile.ContentLength;
            //byte[] imgOriginal = new byte[size];

            //FuploadLogo.PostedFile.InputStream.Read(imgOriginal, 0, size);
            //Bitmap imgBinary = new Bitmap(FuploadLogo.PostedFile.InputStream);
 
            //string imgBase64 = "data:image/jpg;base64," + Convert.ToInt64(imgOriginal);

            //ImgLogo.ImageUrl=imgBase64;
        }
        
    }
}