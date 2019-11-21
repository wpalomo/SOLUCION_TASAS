using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NEGOCIO;
using ENTIDADES;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Solution_CTT
{
    class msj
    {
        public string token { get; set; }
        public int max_sec { get; set; }
    }

    class RootObject
    {
        public string error { get; set; }
        public msj msj { get; set; }
    }

    public partial class frmGenerarToken : System.Web.UI.Page
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
        string sRespuestaJson;
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

            Session["modulo"] = "MÓDULO PARA GENERAR UN TOKEN - DEVESOFFT";

            if (!IsPostBack)
            {
                consultarParametros();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtTerminal.Text = "";
            txtAmbiente.Text = "";
            txtTokenGenerado.Text = "";
            txtSecuencial.Text = "";
            btnGenerar.Enabled = true;
            btnGuardar.Enabled = false;
            Session["token_respuesta"] = null;
            Session["secuencia_respuesta"] = null;
            consultarParametros();
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
                    Session["id_ctt_tasa_parametro"] = dtConsulta.Rows[0]["id_ctt_tasa_parametro"].ToString();
                    Session["tasa_terminal"] = dtConsulta.Rows[0]["id_ctt_tasa_terminal"].ToString();
                    Session["tasa_oficina"] = dtConsulta.Rows[0]["id_oficina"].ToString();
                    Session["tasa_cooperativa"] = dtConsulta.Rows[0]["id_cooperativa"].ToString();
                    Session["servidor_pruebas"] = dtConsulta.Rows[0]["servidor_pruebas"].ToString();
                    Session["servidor_produccion"] = dtConsulta.Rows[0]["servidor_produccion"].ToString();
                    Session["credenciales"] = dtConsulta.Rows[0]["webservice_tasa_credenciales"].ToString();
                    Session["emision"] = dtConsulta.Rows[0]["emision"].ToString();

                    if (dtConsulta.Rows[0]["id_ctt_tasa_terminal"].ToString() == "1")
                    {
                        txtTerminal.Text = "QUITUMBE";
                    }

                    else
                    {
                        txtTerminal.Text = "CARCELÉN";
                    }

                    if (dtConsulta.Rows[0]["emision"].ToString() == "0")
                    {
                        txtAmbiente.Text = "PRUEBAS";
                    }

                    else
                    {
                        txtAmbiente.Text = "PRODUCCIÓN";
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONSUMIR EL WEB SERVICE
        public string consumirWebService()
        {
            try
            {
                if (Session["emision"].ToString() == "0")
                {
                    sUrlEnvio = Session["servidor_pruebas"].ToString() + Session["credenciales"].ToString();
                }

                else
                {
                    sUrlEnvio = Session["servidor_produccion"].ToString() + Session["credenciales"].ToString();

                }

                sObjetoJson = "";
                sObjetoJson += "{" + Environment.NewLine;
                sObjetoJson += "\"oficina\": {" + Environment.NewLine;
                sObjetoJson += "\"id_oficina\": \"" + Session["tasa_oficina"].ToString() + "\"," + Environment.NewLine;
                sObjetoJson += "\"id_coop\": \"" + Session["tasa_cooperativa"].ToString() + "\"," + Environment.NewLine;
                sObjetoJson += "\"id_terminal\": \"" + Session["tasa_terminal"].ToString() + "\"" + Environment.NewLine;
                sObjetoJson += "}}";

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
                string sb = sObjetoJson;

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
                    sRespuestaJson = sr.ReadToEnd();
                }

                //sRespuestaJson = "{\"error\":\"\",\"msj\":{\"token\":30684,\"max_sec\":9231}}";

                RootObject resultado = JsonConvert.DeserializeObject<RootObject>(sRespuestaJson);

                txtTokenGenerado.Text = resultado.msj.token.ToString();
                txtSecuencial.Text = resultado.msj.max_sec.ToString();

                Session["token_respuesta"] = resultado.msj.token.ToString();
                Session["secuencia_respuesta"] = resultado.msj.max_sec.ToString();

                return "OK";
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return "ERROR";
            }
        }

        //Funcion para aceptar los certificados de la URL
        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private bool insertarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "insert into ctt_tasa_token (" + Environment.NewLine;
                sSql += "token, maximo_secuencial, cuenta, emitidos, anulados, estado_token," + Environment.NewLine;
                sSql += "fecha_generacion, ambiente_token, creado, validado, id_ctt_oficinista," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                sSql += "terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "'" + Session["token_respuesta"].ToString() + "', " + Convert.ToInt32(Session["secuencia_respuesta"].ToString()) + "," + Environment.NewLine;
                sSql += "1, 0, 0, 'Abierta', GETDATE(), " + Convert.ToInt32(Session["emision"].ToString()) + ", 1, 0, " + Convert.ToInt32(Session["idUsuario"].ToString()) + ", 'A'," + Environment.NewLine;
                sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
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

        //FUNCION PARA ACTUALIZAR LOS TOKENS GENERADOS
        private bool actualizarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "update ctt_tasa_token set" + Environment.NewLine;
                sSql += "estado_token = 'Cerrada'" + Environment.NewLine;
                sSql += "where ambiente_token = " + Convert.ToInt32(Session["emision"].ToString()) + Environment.NewLine;
                sSql += "and estado_token = 'Abierta'" + Environment.NewLine;
                sSql += "and estado = 'A'";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return false;
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

        //PROCESO DE INSERTAR DATOS
        private void insertarActualizar()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                //if (actualizarRegistro() == false)
                //{
                //    goto reversa;
                //}

                if (insertarRegistro() == false)
                {
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro ingresado correctamente', 'success');", true);
                limpiar();
                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

            reversa: { conexionM.reversaTransaccion(); };
        }

        #endregion

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            if (consumirWebService() == "OK")
            {
                btnGenerar.Enabled = false;
                btnGuardar.Enabled = true;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            insertarActualizar();
        }
    }
}