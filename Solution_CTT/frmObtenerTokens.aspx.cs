using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NEGOCIO;
using System.Data;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Solution_CTT
{
    public partial class frmObtenerTokens : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sObjetoJson;
        string sObjetoOficina;
        string sUrlEnvio;
        string sCadena;

        DataTable dtConsulta;
        DataTable dtAlmacenar;

        bool bRespuesta;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["modulo"] = "MÓDULO DE OBTENCIÓN DE TOKENS ACTIVOS - DEVESOFFT";

            if (!IsPostBack)
            {
                verificarPermiso();
                cargarParametros();
            }
        }

        #region FUNCIONES DEL USUARIO

        //CONSULTAR PERMISOS
        private void verificarPermiso()
        {
            try
            {
                if ((Session["tasaDevesofft"] == null) || (Session["tasaDevesofft"].ToString() == "0"))
                {
                    Response.Redirect("frmMensajePermisos.aspx");
                }

                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //Funcion para aceptar los certificados de la URL
        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        //FUNCION PARA CARGAR LOS PARAMETROS PARA OBTENER INFORMACION DE DEVESOFFT
        private void cargarParametros()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tasa_parametro, id_ctt_tasa_terminal," + Environment.NewLine;
                sSql += "id_oficina, id_cooperativa, emision, servidor_pruebas," + Environment.NewLine;
                sSql += "servidor_produccion, webservice_get_tokens" + Environment.NewLine;
                sSql += "from ctt_tasa_parametros" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["id_ctt_tasa_parametro"] = dtConsulta.Rows[0]["id_ctt_tasa_parametro"].ToString();
                        Session["id_ctt_tasa_terminal"] = dtConsulta.Rows[0]["id_ctt_tasa_terminal"].ToString();
                        Session["id_oficina"] = dtConsulta.Rows[0]["id_oficina"].ToString();
                        Session["id_cooperativa"] = dtConsulta.Rows[0]["id_cooperativa"].ToString();
                        Session["emision"] = dtConsulta.Rows[0]["emision"].ToString();
                        Session["servidor_pruebas"] = dtConsulta.Rows[0]["servidor_pruebas"].ToString();
                        Session["servidor_produccion"] = dtConsulta.Rows[0]["servidor_produccion"].ToString();
                        Session["webservice_get_tokens"] = dtConsulta.Rows[0]["webservice_get_tokens"].ToString();
                    }

                    else
                    {
                        Session["id_ctt_tasa_parametro"] = null;
                        Session["id_ctt_tasa_terminal"] = null;
                        Session["id_oficina"] = null;
                        Session["id_cooperativa"] = null;
                        Session["emision"] = null;
                        Session["servidor_pruebas"] = null;
                        Session["servidor_produccion"] = null;
                        Session["webservice_get_tokens"] = null;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA MANIPULAR LAS COLUMNAS DEL GRIDVIEW DEVESOFFT
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[5].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[8].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[9].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[2].Visible = ok;
            dgvDatos.Columns[3].Visible = ok;
            dgvDatos.Columns[4].Visible = ok;
            dgvDatos.Columns[6].Visible = ok;
        }

        //FUNCION PARA MANIPULAR LAS COLUMNAS DEL GRIDVIEW DEL SISTEMA
        private void columnasGridSistema(bool ok)
        {
            dgvDatosSistema.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosSistema.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosSistema.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosSistema.Columns[5].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosSistema.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosSistema.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatosSistema.Columns[0].Visible = ok;
            dgvDatosSistema.Columns[1].Visible = ok;
            dgvDatosSistema.Columns[8].Visible = ok;
        }

        //FUNCION PARA SABER SI HAY CONEXION A INTERNET
        private bool conexionInternet()
        {
            try
            {
                IPHostEntry host = Dns.GetHostEntry("www.google.com");
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        //FUNCION PARA CREAR EL DATATABLE
        private void crearDataTable()
        {
            try
            {
                dtAlmacenar = new DataTable();
                dtAlmacenar.Columns.Add("Id");
                dtAlmacenar.Columns.Add("Token");
                dtAlmacenar.Columns.Add("OficinaId");
                dtAlmacenar.Columns.Add("EstatusId");
                dtAlmacenar.Columns.Add("MaxSec");
                dtAlmacenar.Columns.Add("CreatedAt");
                dtAlmacenar.Columns.Add("UpdatedAt");
                dtAlmacenar.Columns.Add("MaxCant");
                dtAlmacenar.Columns.Add("CantActual");
                dtAlmacenar.Columns.Add("Estado");
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CREAR EL JSON
        private bool crearJson()
        {
            try
            {
                sObjetoJson = "";
                sObjetoJson += "{" + Environment.NewLine;

                sObjetoOficina = "";
                sObjetoOficina += "\"oficina\": {" + Environment.NewLine;
                sObjetoOficina += "\"id_oficina\": \"" + Session["id_oficina"].ToString() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_coop\": \"" + Session["id_cooperativa"].ToString() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_terminal\": \"" + Session["id_ctt_tasa_terminal"].ToString() + "\"" + Environment.NewLine;
                sObjetoOficina += "}" + Environment.NewLine;
                sObjetoOficina += "}";
                sObjetoJson += sObjetoOficina;

                Session["Json"] = sObjetoJson;

                if (enviarJson() == "ERROR")
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

        //FUNCION PARA ENVIAR EL JSON
        private string enviarJson()
        {
            try
            {
                string respuestaJson = "";

                if (Session["emision"].ToString() == "0")
                {
                    sUrlEnvio = Session["servidor_pruebas"].ToString() + Session["webservice_get_tokens"].ToString();
                }

                else
                {
                    sUrlEnvio = Session["servidor_produccion"].ToString() + Session["webservice_get_tokens"].ToString();
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

                try
                {
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
                        respuestaJson = sr.ReadToEnd();
                    }

                    Clases_Tasa_Usuario.Get_Tokens tokens = JsonConvert.DeserializeObject<Clases_Tasa_Usuario.Get_Tokens>(respuestaJson);

                    if (tokens.Msj.Length != 0)
                    {
                        crearDataTable();

                        for (int i = 0; i < tokens.Msj.Length; i++)
                        {
                            DataRow row = dtAlmacenar.NewRow();
                            row["Id"] = tokens.Msj[i].Id.ToString();
                            row["Token"] = tokens.Msj[i].Token.ToString();
                            row["OficinaId"] = tokens.Msj[i].OficinaId.ToString();
                            row["EstatusId"] = tokens.Msj[i].EstatusId.ToString();
                            row["MaxSec"] = tokens.Msj[i].MaxSec.ToString();
                            row["CreatedAt"] = Convert.ToDateTime(tokens.Msj[i].CreatedAt.ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                            row["UpdatedAt"] = Convert.ToDateTime(tokens.Msj[i].UpdatedAt.ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                            row["MaxCant"] = tokens.Msj[i].MaxCant.ToString();
                            row["CantActual"] = tokens.Msj[i].CantActual.ToString();

                            if (Convert.ToInt32(tokens.Msj[i].EstatusId.ToString()) == 1)
                            {
                                row["Estado"] = "ACTIVO";
                            }

                            else if (Convert.ToInt32(tokens.Msj[i].EstatusId.ToString()) == 12)
                            {
                                row["Estado"] = "SIN VERIFICAR";
                            }

                            dtAlmacenar.Rows.Add(row);
                        }
                    }
                    columnasGrid(true);
                    dgvDatos.DataSource = dtAlmacenar;
                    dgvDatos.DataBind();
                    columnasGrid(false);
                    llenarGrid(1);

                }

                catch (Exception)
                { }

                return "OK";
            }

            catch (Exception)
            {
                return "ERROR";
            }
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select TT.id_ctt_tasa_token, TT.id_ctt_oficinista, TT.token, TT.fecha_generacion," + Environment.NewLine;
                sSql += "TT.maximo_secuencial, TT.emitidos, TT.anulados, O.descripcion oficinista, TT.estado_token" + Environment.NewLine;
                sSql += "from ctt_tasa_token TT INNER JOIN" + Environment.NewLine;
                sSql += "ctt_oficinista O ON O.id_ctt_oficinista = TT.id_ctt_oficinista" + Environment.NewLine;
                sSql += "and TT.estado = 'A'" + Environment.NewLine;
                sSql += "and O.estado = 'A'" + Environment.NewLine;
                sSql += "where TT.validado = 1" + Environment.NewLine;
                sSql += "and TT.estado_token = 'Abierta'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and TT.token in (";
                    sCadena = "";

                    for (int i = 0; i < dtAlmacenar.Rows.Count; i++)
                    {
                        sCadena = sCadena + dtAlmacenar.Rows[i]["Token"].ToString();

                        if ((i + 1) == dtAlmacenar.Rows.Count)
                        {
                            sCadena = sCadena + ")";
                        }
                        else
                        {
                            sCadena = sCadena + ", ";
                        }
                    }

                    sSql += sCadena;
                }

                else
                {
                    sSql += "and TT.token = 0";
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta)
                {
                    columnasGridSistema(true);
                    dgvDatosSistema.DataSource = dtConsulta;
                    dgvDatosSistema.DataBind();
                    columnasGridSistema(false);
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









        #endregion

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            cargarParametros();
            crearDataTable();
            columnasGrid(true);
            dgvDatos.DataSource = dtAlmacenar;
            dgvDatos.DataBind();
            columnasGrid(false);
            llenarGrid(0);
        }

        protected void btnEjecutar_Click(object sender, EventArgs e)
        {
            if (conexionInternet() == false)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información', 'No hay conexión a Internet. Favor verifique.', 'warning');", true);
            }
            else
            {
                crearJson();
            }
        }



    }
}