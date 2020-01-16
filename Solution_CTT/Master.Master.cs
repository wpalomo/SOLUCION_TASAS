using Newtonsoft.Json;
using Solution_CTT.Clases_Tasa_Usuario;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NEGOCIO;

namespace Solution_CTT
{
    public partial class Master : System.Web.UI.MasterPage
    {
        int iEmision;
        int iEnviado_P;
        int iIdCooperativa;
        int iIdNotificacion_API;
        int iIdOficina;
        int iIdTerminal;
        int iPorcentajeNotificacionEntero;

        string[] sDatosMaximo = new string[5];
        string sError_P;
        string sObjetoJson;
        string sObjetoNotificacion;
        string sObjetoOficina;
        string sServidorProduccion;
        string sServidorPruebas;
        string sSql;
        string sUrlEnvio;
        string sApiNotificacion;

        Decimal dbCantidad_Notificacion;
        Decimal dbDisponible_Notificacion;

        DataTable dtConsulta;
        
        bool bRespuesta;

        manejadorConexion conexionM = new manejadorConexion();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["transacciones"] = Session["personas"] = Session["rutas"] = Session["itinerarios"] = Session["vehiculos"] = "treeview";
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "Nombre", "<script> startclock(); </script>", false);
            
            if (Session["idUsuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
            }

            else if (Session["usuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
            }

            else if (Session["modulo"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
            }

            else
            {
                lblNombreUsuario_1.Text = lblNombreUsuario_2.Text = lblNombreUsuario_3.Text = Session["usuario"].ToString().ToUpper();
                lblModulo.Text = Session["modulo"].ToString().ToUpper();
            }

            if (Session["tasaDevesofft"] == null)
            {
                devesofft.Visible = false;
            }

            else
            {
                devesofft.Visible = true;
            }

            if (Session["tasaContifico"] == null)
            {
                venta_normal.Visible = true;
                devolucion_normal.Visible = true;
                venta_smartt.Visible = false;
                devolucion_smartt.Visible = false;
                contifico.Visible = false;
            }

            else
            {
                venta_normal.Visible = false;
                devolucion_normal.Visible = false;
                venta_smartt.Visible = true;
                devolucion_smartt.Visible = true;
                contifico.Visible = true;
            }

            if (Session["ejecuta_cobro_administrativo"].ToString() == "1")
            {
                ingreso_pago_pendiente.Visible = true;
                cobrar_pago_pendiente.Visible = true;
                reporte_pagos_administrativos.Visible = true;
            }

            else
            {
                ingreso_pago_pendiente.Visible = false;
                cobrar_pago_pendiente.Visible = false;
                reporte_pagos_administrativos.Visible = false;
            }

            sDatosMaximo[0] = Session["usuario"].ToString();
            sDatosMaximo[1] = Environment.MachineName.ToString();
            sDatosMaximo[2] = "A";

            if ((Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1) && (Convert.ToInt32(Session["ver_notificacion"].ToString()) == 1))
            {
                consultarParametrosTasa();
                mostrarNotificacionEmergente();
            }

            if (Convert.ToInt32(Session["privilegio"].ToString()) == 0)
            {
                facturacion_electronica.Visible = false;
                parametrizacion_general.Visible = false;
                localidades.Visible = false;
            }

            else
            {
                facturacion_electronica.Visible = true;
                parametrizacion_general.Visible = true;
                localidades.Visible = true;
            }
        }

        protected void btnOkNotificacionAutomatica_Click(object sender, EventArgs e)
        {
            if (!chkConfirmacionVisualizacion.Checked)
            {
                chkConfirmacionVisualizacion.ForeColor = Color.Red;
                chkConfirmacionVisualizacion.Focus();
            }
            else
            {
                enviarJsonNotificaciones();
                insertarNotificacion(sError_P, iIdNotificacion_API);
                chkConfirmacionVisualizacion.Checked = false;
                chkConfirmacionVisualizacion.ForeColor = Color.Black;
                ModalPopupExtender_NotificacionAutomatica.Hide();
            }
        }

        private void consultarParametrosTasa()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tasa_terminal, id_oficina, id_cooperativa, servidor_pruebas," + Environment.NewLine;
                sSql += "servidor_produccion, emision, webservice_tasa_notificacion" + Environment.NewLine;
                sSql += "from ctt_tasa_parametros" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    Session["id_ctt_tasa_terminal_Notificacion"] = dtConsulta.Rows[0]["id_ctt_tasa_terminal"].ToString();
                    Session["id_oficina_Notificacion"] = dtConsulta.Rows[0]["id_oficina"].ToString();
                    Session["id_cooperativa_Notificacion"] = dtConsulta.Rows[0]["id_cooperativa"].ToString();
                    Session["servidor_pruebas_Notificacion"] = dtConsulta.Rows[0]["servidor_pruebas"].ToString();
                    Session["servidor_produccion_Notificacion"] = dtConsulta.Rows[0]["servidor_produccion"].ToString();
                    Session["webservice_tasa_notificacion_Notificacion"] = dtConsulta.Rows[0]["webservice_tasa_notificacion"].ToString();
                    Session["emision_Notificacion"] = dtConsulta.Rows[0]["emision"].ToString();
                }
            }
            catch (Exception)
            {
            }
        }

        private string enviarJsonNotificaciones()
        {
            try
            {
                iIdNotificacion_API = 0;
                sError_P = "";
                sObjetoJson = "";
                sObjetoJson = sObjetoJson + "{" + Environment.NewLine;

                sObjetoOficina = "";
                sObjetoOficina += "\"oficina\": {" + Environment.NewLine;
                sObjetoOficina += "\"id_oficina\": \"" + Session["id_oficina_Notificacion"].ToString() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_coop\": \"" + Session["id_cooperativa_Notificacion"].ToString() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_terminal\": \"" + Session["id_ctt_tasa_terminal_Notificacion"].ToString() + "\"" + Environment.NewLine;
                sObjetoOficina += "}," + Environment.NewLine;
                sObjetoJson += sObjetoOficina;

                sObjetoNotificacion = "";
                sObjetoNotificacion += "\"notificacion\": {" + Environment.NewLine;
                sObjetoNotificacion += "\"cant_restante\": \"" + Convert.ToInt32(Session["dbDisponible_Notificacion"].ToString()).ToString() + "\"," + Environment.NewLine;
                sObjetoNotificacion += "\"mensaje\": \"Por favor recargue, tiene " + Convert.ToInt32(Session["iPorcentajeNotificacionEntero"].ToString()) + "% de tasas restantes.\"" + Environment.NewLine;
                sObjetoNotificacion += "}" + Environment.NewLine;
                sObjetoJson += sObjetoNotificacion + "}";

                Session["Json"] = sObjetoJson;

                string str = "";

                if (Convert.ToInt32(Session["emision_Notificacion"].ToString()) == 0)
                {
                    sUrlEnvio = Session["servidor_pruebas_Notificacion"].ToString() + Session["webservice_tasa_notificacion_Notificacion"].ToString();
                }
                else
                {
                    sUrlEnvio = Session["servidor_produccion_Notificacion"].ToString() + Session["webservice_tasa_notificacion_Notificacion"].ToString();
                }

                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                HttpWebRequest request = WebRequest.Create(sUrlEnvio) as HttpWebRequest;
                request.Timeout = 5000;
                request.Method = "POST";
                request.ContentType = "application/json";
                string s = Session["Json"].ToString();

                byte[] bytes = Encoding.UTF8.GetBytes(s);
                try
                {
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        str = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    }

                    TasaNotificacion notificacion = JsonConvert.DeserializeObject<TasaNotificacion>(str);
                    sError_P = notificacion.Error;
                    iIdNotificacion_API = Convert.ToInt32(notificacion.IdNotificacion);
                    iEnviado_P = 1;
                }
                catch
                {
                    iEnviado_P = 0;
                }
                return "OK";
            }

            catch (Exception)
            {
                iEnviado_P = 0;
                return "ERROR";
            }
        }

        //Funcion para aceptar los certificados de la URL
        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void insertarNotificacion(string sError_P, int iIdNotificacion_P)
        {
            try
            {
                if (conexionM.iniciarTransaccion())
                {
                    sSql = "";
                    sSql += "insert into ctt_tasa_notificaciones (" + Environment.NewLine;
                    sSql += "id_ctt_jornada, id_ctt_oficinista, id_localidad, cantidad, disponible," + Environment.NewLine;
                    sSql += "porcentaje, id_notificacion, error, fecha_notificacion, hora_notificacion," + Environment.NewLine;
                    sSql += "ambiente_notificacion, enviado, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += Convert.ToInt32(Session["idJornada"].ToString()) + ", " + Convert.ToInt32(Session["idUsuario"].ToString()) +", ";
                    sSql += Convert.ToInt32(Application["idLocalidad"].ToString()) + ", " + Convert.ToInt32(Session["dbCantidad_Notificacion"].ToString()) + "," + Environment.NewLine;
                    sSql += Convert.ToInt32(Session["dbDisponible_Notificacion"].ToString()) + ", " + Convert.ToInt32(Session["iPorcentajeNotificacionEntero"].ToString()) + ", ";
                    sSql += iIdNotificacion_P + ", '" + sError_P.Trim() + "', '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + Environment.NewLine;
                    sSql += "GETDATE(), " + Convert.ToInt32(Session["emision"].ToString()) + ", " + iEnviado_P + ", 'A', GETDATE()," + Environment.NewLine;
                    sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                    if (!conexionM.ejecutarInstruccionSQL(sSql))
                    {
                        conexionM.reversaTransaccion();
                    }
                    else
                    {
                        conexionM.terminaTransaccion();
                        Session["Json"] = null;
                        Session["dbCantidad_Notificacion"] = null;
                        Session["dbDisponible_Notificacion"] = null;
                        Session["iPorcentajeNotificacionEntero"] = null;
                        Session["ver_notificacion"] = "0";
                        ModalPopupExtender_NotificacionAutomatica.Hide();
                    }
                }
            }
            catch (Exception)
            {
                conexionM.reversaTransaccion();
            }
        }

        public void mostrarNotificacionEmergente()
        {
            dbCantidad_Notificacion = Convert.ToDecimal(Session["dbCantidad_Notificacion"].ToString());
            dbDisponible_Notificacion = Convert.ToDecimal(Session["dbDisponible_Notificacion"].ToString());
            iPorcentajeNotificacionEntero = Convert.ToInt32(Session["iPorcentajeNotificacionEntero"].ToString());
            lblMensajeNotificacion.Text = "Has consumido el " + ((100 - iPorcentajeNotificacionEntero)).ToString() + "% de la cantidad de tasas de usuario";
            lblDatosMensajeNotificacion.Text = Session["lblMensajeNotificacion"].ToString();
            lblCantidadMensajeNotificacion.Text = Convert.ToInt32(dbDisponible_Notificacion).ToString();
            lblMensajeNotificacion.ForeColor = Color.White;

            if (iPorcentajeNotificacionEntero >= 0x33)
            {
                divTitulo.Attributes.Add("style", "background-color:#00369C;");
            }

            else if (iPorcentajeNotificacionEntero >= 0x1a)
            {
                divTitulo.Attributes.Add("style", "background-color:#F3E212;");
                lblMensajeNotificacion.ForeColor = Color.Black;
            }

            else if (iPorcentajeNotificacionEntero >= 0x10)
            {
                divTitulo.Attributes.Add("style", "background-color:#F16A10;");
            }
            else if (iPorcentajeNotificacionEntero >= 10)
            {
                divTitulo.Attributes.Add("style", "background-color:#A31F11;");
            }

            else if (iPorcentajeNotificacionEntero >= 0)
            {
                divTitulo.Attributes.Add("style", "background-color:#FF0000;");
            }

            ModalPopupExtender_NotificacionAutomatica.Show();
        }

    }
}