using NEGOCIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution_CTT
{
    public partial class frmAnularTasaUsuarioPrueba : System.Web.UI.Page
    {
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

            if (!IsPostBack)
            {
                consultarParametros();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtTasa.Text = "";
            txtIdTasaEmitida.Text = "";
            txtIdTasaAnulada.Text = "";
            txtRespuestaJson.Text = "";
            btnAnular.Enabled = true;
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
                sSql = "";
                sSql += "select id_ctt_tasa_parametro, id_ctt_tasa_terminal, id_oficina, id_cooperativa," + Environment.NewLine;
                sSql += "servidor_pruebas, servidor_produccion, webservice_tasa_anulacion, emision" + Environment.NewLine;
                sSql += "from ctt_tasa_parametros" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    Session["id_ctt_tasa_parametro"] = dtConsulta.Rows[0]["id_ctt_tasa_parametro"].ToString();

                    Session["id_tasa_terminal"] = dtConsulta.Rows[0]["id_ctt_tasa_terminal"].ToString();
                    Session["id_tasa_oficina"] = dtConsulta.Rows[0]["id_oficina"].ToString();
                    Session["id_tasa_cooperativa"] = dtConsulta.Rows[0]["id_cooperativa"].ToString();

                    Session["servidor_pruebas"] = dtConsulta.Rows[0]["servidor_pruebas"].ToString();
                    Session["servidor_produccion"] = dtConsulta.Rows[0]["servidor_produccion"].ToString();
                    //Session["credenciales"] = dtConsulta.Rows[0][6].ToString();
                    //Session["tasa_usuario"] = dtConsulta.Rows[0][7].ToString();
                    Session["tasa_anulacion"] = dtConsulta.Rows[0]["webservice_tasa_anulacion"].ToString();
                    Session["emision"] = dtConsulta.Rows[0]["emision"].ToString();
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

        //FUNCION PARA CREAR EL JSON PARA ENVIAR
        private bool crearJsonEliminar()
        {
            try
            {
                sObjetoJson = "";
                sObjetoJson += "{" + Environment.NewLine;

                sObjetoOficina = "";
                sObjetoOficina += "\"oficina\": {" + Environment.NewLine;
                sObjetoOficina += "\"id_oficina\": \"" + Session["id_tasa_oficina"].ToString() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_coop\": \"" + Session["id_tasa_cooperativa"].ToString() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_terminal\": \"" + Session["id_tasa_terminal"].ToString() + "\"" + Environment.NewLine;
                sObjetoOficina += "},";

                sObjetoJson += sObjetoOficina + Environment.NewLine;

                sObjetoTasa = "";
                sObjetoTasa += "\"tasa\": {" + Environment.NewLine;
                sObjetoTasa += "\"id_tasa\": \"" + txtIdTasaEmitida.Text.Trim() + "\"," + Environment.NewLine;
                sObjetoTasa += "\"n_tasa\": \"" + txtTasa.Text.Trim() + "\"" + Environment.NewLine;
                sObjetoTasa += "}";

                sObjetoJson += sObjetoTasa + Environment.NewLine;
                sObjetoJson += "}";

                Session["JsonElimina"] = sObjetoJson;

                if (enviarJsonElimina() == "ERROR")
                {
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA ENVIAR EL JSON AL SERVIDOR PARA AUTORIZACION
        private string enviarJsonElimina()
        {
            try
            {
                if (Session["emision"].ToString() == "0")
                {
                    sUrlEnvio = Session["servidor_pruebas"].ToString() + Session["tasa_anulacion"].ToString();
                }

                else
                {
                    sUrlEnvio = Session["servidor_produccion"].ToString() + Session["tasa_anulacion"].ToString();
                }


                //Llamar a funcion para aceptar los certificados de la URL
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                //Declara el objeto con el que haremos la llamada al servicio
                HttpWebRequest request = WebRequest.Create(sUrlEnvio) as HttpWebRequest;
                //Configurar las propiedad del objeto de llamada
                request.Method = "POST";
                request.ContentType = "application/json";

                //Serializar el objeto a enviar. Para esto uso la libreria Newtonsoft
                //string sb = JsonConvert.SerializeObject(sAyuda);
                string sb = Session["JsonElimina"].ToString();

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

                //TasaUsuarioAnula resultado = JsonConvert.DeserializeObject<TasaUsuarioAnula>(sRespuestaJson);

                //txtRespuestaJson.Text = sRespuestaJson;
                //txtIdTasaAnulada.Text = resultado.id_tasa.ToString();
                //Session["id_tasa_anulada"] = resultado.id_tasa.ToString();
                return "OK";
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                sSql += "fecha_generacion, ambiente_token, estado, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                sSql += "terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Session["token_respuesta"].ToString()) + ", " + Convert.ToInt32(Session["secuencia_respuesta"].ToString()) + "," + Environment.NewLine;
                sSql += "1, 0, 0, 'Abierta', GETDATE(), " + Convert.ToInt32(Session["emision"].ToString()) + ", 'A'," + Environment.NewLine;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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

                if (actualizarRegistro() == false)
                {
                    goto reversa;
                }

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

        reversa: { conexionM.reversaTransaccion(); };
        }

        #endregion

        protected void btnAnular_Click(object sender, EventArgs e)
        {
            if (txtTasa.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese la tasa de usuario.', 'dinfo');", true);
                txtTasa.Focus();
            }

            else if (txtIdTasaEmitida.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'Favor ingrese el ID de la tasa de usuario.', 'dinfo');", true);
                txtIdTasaEmitida.Focus();
            }

            else
            {
                if (crearJsonEliminar() == true)
                {
                    btnAnular.Enabled = false;
                    btnGuardar.Enabled = true;
                }
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