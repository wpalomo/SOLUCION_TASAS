using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NEGOCIO;
using ENTIDADES;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Solution_CTT
{
    public partial class frmSincronizarTasasUsuario : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();
        manejadorSincronizarTasaUsuario tasasM = new manejadorSincronizarTasaUsuario();

        ENTSincronizarTasaUsuario tasasE = new ENTSincronizarTasaUsuario();

        DataTable dtConsulta;
        DataTable dtDatos;

        string sSql;
        string sTipoClienteTasa;
        string sIdentificacion;
        string sObjetoJson;
        string sObjetoOficina;
        string sObjetoTasa;
        string sObjetoInfo;
        string sObjetoCliente;
        string sTasaOficina;
        string sTasaCooperativa;
        string sTasaTerminal;
        string sTasaUsuario;
        string sCantidad;
        string sCuentaToken;
        string sToken;
        string sTipoTasa;
        string sIdInicio;
        string sIdDestino;
        string sInicio;
        string sDestino;
        string sCodigoTasaRespuesta;
        string sIdTasaRespuesta_P;
        string sErrorRespuesta_P;
        
        string sNombreCliente;
        string sDireccionCliente;
        string sMailCliente;
        string sTelefonoCliente;
        string sIdTasaRespuesta;
        string sUrlEnvio;
        string sObjetoJsonLote;

        string sPuebloOrigen;
        string sPuebloDestino;
        string sHoraSalida;
        string sFechaViaje;
        string sNumeroBus;
        string sIdentificacionPasajero;
        string sNombrePasajero;

        int iAmbienteTasa;
        int iIdFactura;
        int iIdPuebloOrigen;
        int iIdPuebloDestino;
        int iIdFacturaActualizar;
        int iBanderaMensaje_P;

        bool bRespuesta;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
                return;
            }

            Session["modulo"] = "MÓDULO SINCRONIZACI\x00d3N DE TASAS DE USUARIO EN LOTE - DEVESOFFT";

            if (!IsPostBack)
            {
                consultarParametros();
                llenarGrid();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA VERIFICAR LA CONEXION A INTERNET
        private bool conexionInternet()
        {
            try
            {
                Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //OCULTAR Y MOSTRAR COLUMNAS DEL GRIDVIEW
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[5].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[8].Visible = ok;
            dgvDatos.Columns[9].Visible = ok;
            dgvDatos.Columns[10].Visible = ok;
        }

        //LLENAR EL GRID
        private void llenarGrid()
        {
            try
            {
                sSql = "";
                sSql += "select id_factura, identificacion, cliente, fecha_factura, establecimiento, punto_emision," + Environment.NewLine;
                sSql += "numero_factura, tasa_usuario, cantidad_tasa_emitida, isnull(direccion_factura, '') direccion_factura," + Environment.NewLine;
                sSql += "isnull(telefono_factura, '') telefono_factura, isnull(correo_electronico, '') correo_electronico" + Environment.NewLine;
                sSql += "from ctt_vw_tasa_usuario_no_enviada" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(Application["idLocalidad"].ToString()) + Environment.NewLine;
                sSql += "and ambiente_tasa_usuario = " + Convert.ToInt32(Session["emision"].ToString()) + Environment.NewLine;
                sSql += "and emite_tasa_usuario = 1" + Environment.NewLine;   //SOLO PARA PROBAR, PONER EN 1 CUANDO YA ESTÉ LISTO
                sSql += "and id_tasa_emitida = 0" + Environment.NewLine;
                sSql += "order by numero_factura";

                Session["instruccion_SQL"] = sSql;

                tasasE.ISQL = sSql;
                columnasGrid(true);
                dgvDatos.DataSource = tasasM.listarTasaNoEnviada(tasasE);
                dgvDatos.DataBind();
                columnasGrid(false);

                if (dgvDatos.Rows.Count == 0)
                {
                    btnSincronizar.Visible = false;
                }

                else
                {
                    btnSincronizar.Visible = true;
                }
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
                sSql = "";
                sSql += "select id_ctt_tasa_terminal, id_oficina, id_cooperativa," + Environment.NewLine;
                sSql += "servidor_pruebas, servidor_produccion, webservice_tasa_lote, emision" + Environment.NewLine;
                sSql += "from ctt_tasa_parametros" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    Session["id_ctt_tasa_terminal"] = dtConsulta.Rows[0]["id_ctt_tasa_terminal"].ToString();
                    Session["id_oficina"] = dtConsulta.Rows[0]["id_oficina"].ToString();
                    Session["id_cooperativa"] = dtConsulta.Rows[0]["id_cooperativa"].ToString();
                    Session["servidor_pruebas"] = dtConsulta.Rows[0]["servidor_pruebas"].ToString();
                    Session["servidor_produccion"] = dtConsulta.Rows[0]["servidor_produccion"].ToString();
                    Session["tasa_usuario_lote"] = dtConsulta.Rows[0]["webservice_tasa_lote"].ToString();
                    Session["emision"] = dtConsulta.Rows[0]["emision"].ToString();

                    sObjetoOficina = "";
                    sObjetoOficina += "\"oficina\": {" + Environment.NewLine;
                    sObjetoOficina += "\"id_oficina\": \"" + Session["id_oficina"].ToString() + "\"," + Environment.NewLine;
                    sObjetoOficina += "\"id_coop\": \"" + Session["id_cooperativa"].ToString() + "\"," + Environment.NewLine;
                    sObjetoOficina += "\"id_terminal\": \"" + Session["id_ctt_tasa_terminal"].ToString() + "\"" + Environment.NewLine;
                    sObjetoOficina += "},";

                    Session["objetoOficina"] = sObjetoOficina;
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

        //CONSULTAR EN UN DATATABLE LA INFORMACION A SINCRONIZAR
        private void crearDataTable()
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(Session["instruccion_SQL"].ToString(), dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        if (recorrerDatatable() == true)
                        {
                            Session["dtDatos"] = dtConsulta;
                            //ENVIAR JSON
                            if (enviarJson() == true)
                            {
                                if (iBanderaMensaje_P != 1)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Informaci\x00f3n.!', 'Servidor remoto de DEVESOFFT no disponible. Intente m\x00e1s tarde.', 'info');", true);
                                    return;
                                }


                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.', 'Tasas de usuario pendientes sincronizadas éxitosamente.', 'success');", true);
                            }

                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información', 'Ocurrió un problema al sincronizar las tasas de usuario.', 'warning');", true);
                            }

                            llenarGrid();
                        }
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información', 'No existen registros a sincronizar.', 'info');", true);
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

        private bool recorrerDatatable()
        {
            try
            {
                sObjetoJsonLote = "";
                sObjetoJsonLote += "[" + Environment.NewLine;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    sIdentificacion = dtConsulta.Rows[i]["identificacion"].ToString();
                    sNombreCliente = dtConsulta.Rows[i]["cliente"].ToString();
                    sDireccionCliente = dtConsulta.Rows[i]["direccion_factura"].ToString();
                    sMailCliente = dtConsulta.Rows[i]["correo_electronico"].ToString();
                    sTelefonoCliente = dtConsulta.Rows[i]["telefono_factura"].ToString();

                    if (sIdentificacion.Trim() == "9999999999999")
                    {
                        sTipoClienteTasa = "07";
                    }

                    else if (sIdentificacion.Trim().Length == 10)
                    {
                        sTipoClienteTasa = "05";
                    }

                    else if (sIdentificacion.Trim().Length == 13)
                    {
                        sTipoClienteTasa = "04";
                    }

                    else
                    {
                        sTipoClienteTasa = "06";
                    }

                    consultarDatosParaJson(Convert.ToInt32(dtConsulta.Rows[i]["id_factura"].ToString()), dtConsulta.Rows[i]["tasa_usuario"].ToString());

                    if (i + 1 < dtConsulta.Rows.Count)
                    {
                        sObjetoJsonLote += "," + Environment.NewLine;
                    }
                }

                sObjetoJsonLote += Environment.NewLine + "]";
                Session["JsonLote"] = sObjetoJsonLote;

                return true;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        private bool consultarDatosParaJson(int iIdFactura_P, string sTasaUsuario_P)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_datos_tasa_usuario_lote" + Environment.NewLine;
                sSql += "where id_factura = " + iIdFactura_P;

                dtDatos = new DataTable();
                dtDatos.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtDatos);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        crearJson(sTasaUsuario_P);
                    }
                }

                else
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


        //FUNCION PARA CREAR EL JSON
        private bool crearJson(string sTasaUsuario_P)
        {
            try
            {
                sObjetoJson = "";
                sObjetoJson += "{" + Environment.NewLine;
                sObjetoJson += Session["objetoOficina"].ToString() + Environment.NewLine;

                sCantidad = sTasaUsuario_P.Substring(0, 2);
                sCuentaToken = sTasaUsuario_P.Substring(14, 4);
                sToken = sTasaUsuario_P.Substring(9, 5);
                sTipoTasa = sTasaUsuario_P.Substring(19, 1);

                sObjetoTasa = "";
                sObjetoTasa += "\"tasa\": {" + Environment.NewLine;
                sObjetoTasa += "\"cantidad\": \"" + sCantidad.Trim().PadLeft(2, '0') + "\"," + Environment.NewLine;
                sObjetoTasa += "\"secuencial\": \"" + sCuentaToken.Trim() + "\"," + Environment.NewLine;
                sObjetoTasa += "\"token\": \"" + sToken.Trim() + "\"," + Environment.NewLine;
                sObjetoTasa += "\"tipo\": \"" + sTipoTasa + "\"," + Environment.NewLine;
                sObjetoTasa += "\"codigo\": \"" + sTasaUsuario_P + "\"" + Environment.NewLine;
                sObjetoTasa += "},";

                sObjetoJson += sObjetoTasa + Environment.NewLine;

                sObjetoInfo = "";
                sObjetoInfo += "\"info\": {" + Environment.NewLine;
                sObjetoInfo += "\"id_inicio\": \"" + dtDatos.Rows[0]["id_ctt_pueblo_origen"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"id_destino\": \"" + dtDatos.Rows[0]["id_ctt_pueblo_destino"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_inicio\": \"" + dtDatos.Rows[0]["pueblo_origen"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_destino\": \"" + dtDatos.Rows[0]["pueblo_destino"].ToString() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_horaSalida\": \"" + Convert.ToDateTime(dtDatos.Rows[0]["hora_salida"].ToString()).ToString("HH:mm") + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_fechaSalida\": \"" + Convert.ToDateTime(dtDatos.Rows[0]["fecha_viaje"].ToString()).ToString("yyyy-MM-dd") + "\"," + Environment.NewLine;
                sObjetoInfo += "\"pasajeros\": \"" + sCantidad.Trim() + "\"," + Environment.NewLine;
                sObjetoInfo += "\"list_pasajeros\": [" + Environment.NewLine;

                for (int i = 0; i < dtDatos.Rows.Count; i++)
                {
                    sObjetoInfo += "{" + Environment.NewLine;
                    sObjetoInfo += "\"nombre\": \"" + dtDatos.Rows[i]["pasajero"].ToString() + "\"," + Environment.NewLine;
                    sObjetoInfo += "\"id\": \"" + dtDatos.Rows[i]["identificacion"].ToString() + "\"" + Environment.NewLine;

                    if (i + 1 == dtDatos.Rows.Count)
                    {
                        sObjetoInfo += "}" + Environment.NewLine;
                    }

                    else
                    {
                        sObjetoInfo += "}," + Environment.NewLine;
                    }
                }

                sObjetoInfo += "]," + Environment.NewLine;
                sObjetoInfo += "\"n_bus\": \"" + dtDatos.Rows[0]["disco"].ToString() + "\"" + Environment.NewLine;
                sObjetoInfo += "},";

                sObjetoJson += sObjetoInfo + Environment.NewLine;

                if (sDireccionCliente.Trim() == "")
                {
                    sDireccionCliente = Application["ciudad_default"].ToString().ToUpper();
                }

                if (sMailCliente.Trim() == "")
                {
                    sMailCliente = Application["correo_default"].ToString().ToLower();
                }

                if (sTelefonoCliente.Trim() == "")
                {
                    sTelefonoCliente = Application["telefono_default"].ToString();
                }

                sObjetoCliente = "";
                sObjetoCliente += "\"cliente\": {" + Environment.NewLine;
                sObjetoCliente += "\"ruc\": \"" + sIdentificacion + "\"," + Environment.NewLine;
                sObjetoCliente += "\"nombre\": \"" + sNombreCliente + "\"," + Environment.NewLine;
                sObjetoCliente += "\"direccion\": \"" + sDireccionCliente + "\"," + Environment.NewLine;
                sObjetoCliente += "\"correo\": \"" + sMailCliente + "\"," + Environment.NewLine;
                sObjetoCliente += "\"telefono\": \"" + sTelefonoCliente + "\"," + Environment.NewLine;
                sObjetoCliente += "\"tipo\": \"" + sTipoClienteTasa + "\"" + Environment.NewLine;
                sObjetoCliente += "}";

                sObjetoJson += sObjetoCliente + Environment.NewLine + "}";

                sObjetoJsonLote += sObjetoJson;

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
        private bool enviarJson()
        {
            try
            {
                string respuestaJson = "";

                if (Session["emision"].ToString() == "0")
                {
                    sUrlEnvio = Session["servidor_pruebas"].ToString() + Session["tasa_usuario_lote"].ToString();
                }

                else
                {
                    sUrlEnvio = Session["servidor_produccion"].ToString() + Session["tasa_usuario_lote"].ToString();
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
                string sb = Session["JsonLote"].ToString();

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

                    Clases_Tasa_Usuario.TasaUsuarioLote lote = JsonConvert.DeserializeObject<Clases_Tasa_Usuario.TasaUsuarioLote>(respuestaJson);

                    dtDatos = new DataTable();
                    dtDatos.Clear();

                    dtDatos = Session["dtDatos"] as DataTable;

                    for (int i = 0; i < lote.Msj.Length; i++)
                    {
                        sCodigoTasaRespuesta = lote.Msj[i].Codigo;
                        sIdTasaRespuesta_P = lote.Msj[i].Id.ToString();

                        DataRow[] dFila = dtDatos.Select("tasa_usuario = " + sCodigoTasaRespuesta);

                        if (dFila.Length != 0)
                        {
                            iIdFacturaActualizar = Convert.ToInt32(dFila[0][0].ToString());
                        }

                        actualizarRegistro(sIdTasaRespuesta_P, iIdFacturaActualizar);
                    }

                    for (int i = 0; i < lote.TasasError.Length; i++)
                    {
                        sCodigoTasaRespuesta = lote.TasasError[i].Codigo;
                        sIdTasaRespuesta_P = lote.TasasError[i].IdTasa.ToString();
                        sErrorRespuesta_P = lote.TasasError[i].Error[0].ToString();

                        if (sErrorRespuesta_P.Trim() == "Tasa ya registrada")
                        {
                            DataRow[] dFila = dtDatos.Select("tasa_usuario = " + sCodigoTasaRespuesta);

                            if (dFila.Length != 0)
                            {
                                iIdFacturaActualizar = Convert.ToInt32(dFila[0][0].ToString());
                            }

                            actualizarRegistro(sIdTasaRespuesta_P, iIdFacturaActualizar);
                        }
                    }

                    iBanderaMensaje_P = 1;
                }

                catch (Exception)
                {
                    iBanderaMensaje_P = 0;
                }
                                
                return true;
            }

            catch (Exception)
            {
                iBanderaMensaje_P = 0;
                return false;
            }
        }

        //Funcion para aceptar los certificados de la URL
        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        //FUNCION PARA ACTUALIZAR LAS TABLAS
        private bool actualizarRegistro(string sIdTasaRespuesta_P, int iIdFactura_P)
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    //lblMensajeError.Text = "No se pudo iniciar la transacción para guardar los registros.";
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal1').modal('show');</script>", false);
                    return false;
                }

                sSql = "";
                sSql += "update cv403_facturas set" + Environment.NewLine;
                sSql += "id_tasa_emitida = '" + sIdTasaRespuesta_P + "'," + Environment.NewLine;
                sSql += "tasa_emitida = 1" + Environment.NewLine;
                sSql += "where id_factura = " + iIdFactura_P + Environment.NewLine;
                //sSql += "and estado = 'A'";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    //lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    conexionM.reversaTransaccion();
                    return false;
                }

                sSql = "";
                sSql += "update ctt_movimiento_caja set" + Environment.NewLine;
                sSql += "id_tasa_usuario = '" + sIdTasaRespuesta + "'" + Environment.NewLine;
                sSql += "where id_factura = " + iIdFactura_P + Environment.NewLine;
                //sSql += "and estado = 'A'";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    //lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    conexionM.reversaTransaccion();
                    return false;
                }

                conexionM.terminaTransaccion();
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        protected void btnSincronizar_Click(object sender, EventArgs e)
        {
            crearDataTable();
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            llenarGrid();
        }
    }
}