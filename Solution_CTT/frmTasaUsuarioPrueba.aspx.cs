using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ENTIDADES;
using NEGOCIO;
using System.Data;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Solution_CTT
{
    class Tasa
    {
        public string id_tasa { get; set; }
    } 

    public partial class frmTasaUsuarioPrueba : System.Web.UI.Page
    {
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sTasaUsuario;
        string sObjetoOficina;
        string sObjetoTasa;
        string sObjetoInfo;
        string sObjetoCliente;
        string sObjetoJson;
        string sUrlCredenciales;
        string sUrlEnvio;
        string sUrlAnula;
        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;

        bool bRespuesta;

        int iIdTasaRecuperado;

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

            Session["modulo"] = "MÓDULO DE TASAS DE USUARIO - PRUEBAS";

            if (!IsPostBack)
            {
                llenarComboTipoCliente();
                llenarComboTipoTasa();
                consultarParametros();
                consultarToken();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMOBOX DE TIPO DE CLIENTE
        private void llenarComboTipoCliente()
        {
            try
            {
                sSql = "";
                sSql += "select codigo, descripcion" + Environment.NewLine;
                sSql += "from ctt_tasa_tipo_cliente" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbTipoCliente.DataSource = comboM.listarCombo(comboE);
                cmbTipoCliente.DataValueField = "IID";
                cmbTipoCliente.DataTextField = "IDATO";
                cmbTipoCliente.DataBind();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE TIPO DE TASA
        private void llenarComboTipoTasa()
        {
            try
            {
                sSql = "";
                sSql += "select codigo, descripcion" + Environment.NewLine;
                sSql += "from ctt_tipo_tasa" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbTipoTasa.DataSource = comboM.listarCombo(comboE);
                cmbTipoTasa.DataValueField = "IID";
                cmbTipoTasa.DataTextField = "IDATO";
                cmbTipoTasa.DataBind();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONSULTAR LOS PARAMETROS DEL SERVIDOR
        private void consultarParametros()
        {
            try
            {
                sSql = "select * from ctt_tasa_parametros";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    Session["id_ctt_tasa_parametro"] = dtConsulta.Rows[0][0].ToString();

                    txtTerminal.Text = dtConsulta.Rows[0][1].ToString();
                    txtOficina.Text = dtConsulta.Rows[0][2].ToString();
                    txtCooperativa.Text = dtConsulta.Rows[0][3].ToString();

                    Session["servidor_pruebas"] = dtConsulta.Rows[0][4].ToString();
                    Session["servidor_produccion"] = dtConsulta.Rows[0][5].ToString();
                    Session["credenciales"] = dtConsulta.Rows[0][6].ToString();
                    Session["tasa_usuario"] = dtConsulta.Rows[0][7].ToString();
                    Session["tasa_anulacion"] = dtConsulta.Rows[0][8].ToString();
                    Session["emision"] = dtConsulta.Rows[0][9].ToString();
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

        //FUNCION PARA CONSULTAR LOS VALORES DE CUENTAS DEL TOKEN

        private void consultarToken()
        {
            try
            {
                sSql = "";
                sSql += "select token, cuenta" + Environment.NewLine;
                sSql += "from ctt_tasa_token" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    txtToken.Text = dtConsulta.Rows[0][0].ToString();
                    txtSecuencial.Text = dtConsulta.Rows[0][1].ToString().Trim().PadLeft(4, '0');
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

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            consultarToken();
            txtIdentificacion.Text = "";
            txtNombreCliente.Text = "";
            txtDireccion.Text = "";
            txtMail.Text = "";
            txtTelefono.Text = "";
            txtJson.Text = "";
            cmbTipoCliente.SelectedIndex = 0;
            cmbTipoTasa.SelectedIndex = 0;
        }

        //FUNCION PARA CREAR EL CODIGO SECUENCIAS DE LA TASA DE USUARIO
        private void crearTasaUsuario()
        {
            sTasaUsuario = "";
            sTasaUsuario += txtCantidad.Text.Trim().PadLeft(2, '0');
            sTasaUsuario += txtCooperativa.Text.Trim();
            sTasaUsuario += txtOficina.Text.Trim();
            sTasaUsuario += txtToken.Text.Trim();
            sTasaUsuario += txtSecuencial.Text.Trim();
            sTasaUsuario += txtTerminal.Text.Trim();
            sTasaUsuario += cmbTipoTasa.SelectedValue;

            txtTasaUsuario.Text = sTasaUsuario;
            crearJson();
        }

        //FUNCION PARA CREAR EL JSON PARA ENVIAR
        private void crearJson()
        {
            try
            {
                sObjetoJson = "";
                sObjetoJson += "{" + Environment.NewLine;

                sObjetoOficina = "";
                sObjetoOficina += "\"oficina\": {" + Environment.NewLine;
                sObjetoOficina += "\"id_oficina\": \"" + txtOficina.Text.Trim() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_coop\": \"" + txtCooperativa.Text.Trim() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_terminal\": \"" + txtTerminal.Text.Trim() + "\"" + Environment.NewLine;
                sObjetoOficina += "},";

                sObjetoJson += sObjetoOficina + Environment.NewLine;

                sObjetoTasa = "";
                sObjetoTasa += "\"tasa\": {" + Environment.NewLine;
                sObjetoTasa += "\"cantidad\": \"" + txtCantidad.Text.Trim().PadLeft(2, '0') + "\"," + Environment.NewLine;
                sObjetoTasa += "\"secuencial\": \"" + txtSecuencial.Text.Trim() + "\"," + Environment.NewLine;
                sObjetoTasa += "\"token\": \"" + txtToken.Text.Trim() + "\"," + Environment.NewLine;
                sObjetoTasa += "\"tipo\": \"" + cmbTipoTasa.SelectedValue + "\"," + Environment.NewLine;
                sObjetoTasa += "\"codigo\": \"" + sTasaUsuario + "\"" + Environment.NewLine;
                sObjetoTasa += "},";

                sObjetoJson += sObjetoTasa + Environment.NewLine;

                sObjetoInfo = "";
                sObjetoInfo += "\"info\": {" + Environment.NewLine;
                sObjetoInfo += "\"id_inicio\": \"1\"," + Environment.NewLine;
                sObjetoInfo += "\"id_destino\": \"2\"," + Environment.NewLine;
                sObjetoInfo += "\"str_inicio\": \"QUITO\"," + Environment.NewLine;
                sObjetoInfo += "\"str_destino\": \"GUARANDA\"," + Environment.NewLine;
                sObjetoInfo += "\"str_horaSalida\": \"12:00\"," + Environment.NewLine;
                sObjetoInfo += "\"str_fechaSalida\": \"2019-05-04\"," + Environment.NewLine;
                sObjetoInfo += "\"pasajeros\": \"1\"" + Environment.NewLine;
                sObjetoInfo += "},";

                sObjetoJson += sObjetoInfo + Environment.NewLine;

                sObjetoCliente = "";
                sObjetoCliente += "\"cliente\": {" + Environment.NewLine;
                sObjetoCliente += "\"ruc\": \"" + txtIdentificacion.Text.Trim() + "\"," + Environment.NewLine;
                sObjetoCliente += "\"nombre\": \"" + txtNombreCliente.Text.Trim().ToUpper() + "\"," + Environment.NewLine;
                sObjetoCliente += "\"direccion\": \"" + txtDireccion.Text.Trim().ToUpper() + "\"," + Environment.NewLine;
                sObjetoCliente += "\"correo\": \"" + txtMail.Text.Trim() + "\"," + Environment.NewLine;
                sObjetoCliente += "\"telefono\": \"" + txtTelefono.Text.Trim() + "\"," + Environment.NewLine;
                sObjetoCliente += "\"tipo\": \"" + cmbTipoCliente.SelectedValue + "\"" + Environment.NewLine;
                sObjetoCliente += "}";

                sObjetoJson += sObjetoCliente + Environment.NewLine + "}";
                Session["Json"] = sObjetoJson;
                txtJson.Text = sObjetoJson;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA INSERTAR EN UNA TABLA DE PRUEBAS
        private void insertarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                //INSTRUCCION SQL PARA INSERTAR EN LA TABLA DE PRUEBAS
                sSql = "";
                sSql += "insert into ctt_tasa_insertar(" + Environment.NewLine;
                sSql += "id_ctt_tasa_parametro, identificacion, nombre, direccion, mail," + Environment.NewLine;
                sSql += "telefono, tipo_cliente, cantidad, secuencial, token, tipo_tasa," + Environment.NewLine;
                sSql += "tasa_usuario, id_tasa, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Session["id_ctt_tasa_parametro"].ToString()) + ", '" + txtIdentificacion.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtNombreCliente.Text.Trim() + "', '" + txtDireccion.Text.Trim() + "', '" + txtMail.Text.Trim() + "'," + Environment.NewLine;
                sSql += "'" + txtTelefono.Text.Trim() + "', '" + cmbTipoCliente.SelectedValue + "', " + Convert.ToInt32(txtCantidad.Text.Trim()) + "," + Environment.NewLine;
                sSql += "'" + txtSecuencial.Text.Trim() + "', '" + txtToken.Text.Trim() + "', '" + cmbTipoTasa.SelectedValue + "'," + Environment.NewLine;
                sSql += "'" + txtTasaUsuario.Text.Trim() + "', " + iIdTasaRecuperado + ", 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')"; ;

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //INSTRUCCION SQL PARA ACTUALIZAR LA SECUENCIA
                sSql = "";
                sSql += "update ctt_tasa_token set" + Environment.NewLine;
                sSql += "cuenta = cuenta + " + Convert.ToInt32(txtCantidad.Text.Trim()) + "," + Environment.NewLine;
                sSql += "emitidos = emititdos + " + Convert.ToInt32(txtCantidad.Text.Trim()) + Environment.NewLine;
                sSql += "where token = " + Convert.ToInt32(txtToken.Text.Trim()) + Environment.NewLine;
                sSql += "and estado = 'A'";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro ingresado correctamente', 'success');", true);
                limpiar();
                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); };
        }

        public string enviarJson()
        {
            try
            {
                string strsb = "";

                if (Session["emision"].ToString() == "0")
                {
                    sUrlEnvio = Session["servidor_pruebas"].ToString() + Session["tasa_usuario"].ToString();
                }

                else
                {
                    sUrlEnvio = Session["servidor_produccion"].ToString() + Session["tasa_usuario"].ToString();
                    return "";
                }

                //Llamar a funcion para aceptar los certificados de la URL
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                //Declara el objeto con el que haremos la llamada al servicio
                HttpWebRequest request = WebRequest.Create(sUrlEnvio) as HttpWebRequest;
                //Configurar las propiedad del objeto de llamada
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 5000;

                //Serializar el objeto a enviar. Para esto uso la libreria Newtonsoft
                //string sb = JsonConvert.SerializeObject(sAyuda);
                string sb = Session["Json"].ToString();

                //Convertir el objeto serializado a arreglo de byte
                Byte[] bt = Encoding.UTF8.GetBytes(sb);

                //Agregar el objeto Byte[] al request
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();

                //Hacer la llamada
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    //Leer el resultado de la llamada
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    strsb = sr.ReadToEnd();
                }

                Tasa tasa = JsonConvert.DeserializeObject<Tasa>(strsb);

                string id_tasa = tasa.id_tasa;

                txtTasaUsuario.Text = id_tasa;

                return id_tasa;
            }

            catch (Exception)
            {
                return "";
            }
        }

        //Funcion para aceptar los certificados de la URL
        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        #endregion

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            crearTasaUsuario();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (enviarJson() == "")
            {

            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }
    }
}