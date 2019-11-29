using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NEGOCIO;
using System.Data;
using ENTIDADES;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmDetalleTransaccionesTasa : System.Web.UI.Page
    {
        ENTComboDatos comboE = new ENTComboDatos();

        manejadorConexion conexionM = new manejadorConexion();
        manejadorComboDatos comboM = new manejadorComboDatos();

        DataTable dtAlmacenar;
        DataTable dtAgrupado;

        string sSql;
        string sObjetoJson;
        string sObjetoOficina;
        string sUrlEnvio;
        string sFecha;

        Decimal dValorTasa_P;
        Decimal dCantidad_P;
        Decimal dTotal_P;

        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;

        bool bRespuesta;

        int iEstatusID_P;
        int iBanderaMensaje_P;

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

            Session["modulo"] = "MÓDULO DE DETALLE DE TRANSACCIONES - DEVESOFFT";

            if (!IsPostBack)
            {
                verificarPermiso();
                consultarParametros();
                txtFechaFinal.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaInicial.Text = DateTime.Now.ToString("dd/MM/yyyy");
                consultarCantidadTasasNoSincronizadas();
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
                    Session["webservice_detalle_transacciones"] = dtConsulta.Rows[0]["webservice_detalle_transacciones"].ToString();
                    Session["emision"] = dtConsulta.Rows[0]["emision"].ToString();
                    Session["id_ctt_tasa_terminal"] = dtConsulta.Rows[0]["id_ctt_tasa_terminal"].ToString();
                    Session["valor_tasa"] = dtConsulta.Rows[0]["valor_tasa"].ToString();

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

        //FUNCION PARA CREAR EL JSON PARA ENVIAR
        private bool crearJson()
        {
            try
            {
                sObjetoJson = "";
                sObjetoJson += "{" + Environment.NewLine;

                sObjetoOficina = "";
                sObjetoOficina += "\"oficina\": {" + Environment.NewLine;
                sObjetoOficina += "\"id_oficina\": \"" + Session["tasa_oficina"].ToString() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_coop\": \"" + Session["tasa_cooperativa"].ToString() + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_terminal\": \"" + Session["tasa_terminal"].ToString() + "\"" + Environment.NewLine;
                sObjetoOficina += "}," + Environment.NewLine;
                sObjetoOficina += "\"fecha_inicio\": \"" + Convert.ToDateTime(txtFechaInicial.Text.Trim()).ToString("yyyy-MM-dd") + "\"," + Environment.NewLine;
                sObjetoOficina += "\"fecha_fin\": \"" + Convert.ToDateTime(txtFechaFinal.Text.Trim()).ToString("yyyy-MM-dd") + "\"";
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return false;
            }
        }

        //FUNCION PARA ENVIAR EL JSON AL SERVIDOR PARA VALIDAR EL TOKEN
        private string enviarJson()
        {
            try
            {
                string respuestaJson = "";

                if (Session["emision"].ToString() == "0")
                {
                    sUrlEnvio = Session["servidor_pruebas"].ToString() + Session["webservice_detalle_transacciones"].ToString();
                }

                else
                {
                    sUrlEnvio = Session["servidor_produccion"].ToString() + Session["webservice_detalle_transacciones"].ToString();
                }

                //Llamar a funcion para aceptar los certificados de la URL
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                //Declara el objeto con el que haremos la llamada al servicio            
                HttpWebRequest request = WebRequest.Create(sUrlEnvio) as HttpWebRequest;
                //Configurar las propiedad del objeto de llamada
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 15000;

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

                    Clases_Tasa_Usuario.TasaDetalleTransacciones detalle = JsonConvert.DeserializeObject<Clases_Tasa_Usuario.TasaDetalleTransacciones>(respuestaJson);
                    dValorTasa_P = Convert.ToDecimal(Session["valor_tasa"].ToString());

                    if (detalle.Msj.Length > 0)
                    {
                        crearDataTable();                        

                        for (int i = 0; i < detalle.Msj.Length; i++)
                        {
                            iEstatusID_P = Convert.ToInt32(detalle.Msj[i].EstatusId.ToString());

                            if (iEstatusID_P == 1)
                            {
                                dCantidad_P = Convert.ToDecimal(detalle.Msj[i].Cantidad.ToString());
                                dTotal_P = dValorTasa_P * dCantidad_P;

                                DataRow row = dtAlmacenar.NewRow();
                                row["fecha_creacion"] = Convert.ToDateTime(detalle.Msj[i].CreatedAt.ToString()).ToString("dd-MM-yyyy");
                                row["cantidad"] = detalle.Msj[i].Cantidad.ToString();
                                row["usos"] = detalle.Msj[i].Usos.ToString();
                                row["total"] = dTotal_P.ToString("N2");
                                dtAlmacenar.Rows.Add(row);
                            }
                        }

                        IEnumerable<IGrouping<string, DataRow>> query = from item in dtAlmacenar.AsEnumerable()
                                                                        group item by item["fecha_creacion"].ToString() into g
                                                                        select g;
                        dtAgrupado = Transformar(query);

                        Session["dtAlmacenaTasaDetalle"] = dtAgrupado;

                        dgvDatos.DataSource = dtAgrupado;
                        dgvDatos.DataBind();
                        Scroll.Visible = true;

                        dgvDatos.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        dgvDatos.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        dgvDatos.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        dgvDatos.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Right;

                        Decimal num2 = 0;

                        if (dgvDatos.Rows.Count == 0)
                        {
                            lblSuma.Text = "Total: 0.00 $";
                        }

                        else
                        {
                            for (int j = 0; j < dgvDatos.Rows.Count; j++)
                            {
                                num2 += Convert.ToDecimal(dgvDatos.Rows[j].Cells[3].Text);
                            }

                            lblSuma.Text = "Total: " + num2.ToString("N2") + " $";
                        }

                    }

                    else
                    {
                        Scroll.Visible = false;
                    }

                    iBanderaMensaje_P = 1;
                }

                catch (Exception)
                {
                    iBanderaMensaje_P = 0;
                }

                return "OK";
            }

            catch (Exception)
            {
                iBanderaMensaje_P = 0;
                return "ERROR";
            }
        }
        
        private DataTable Transformar(IEnumerable<IGrouping<string, DataRow>> datos)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("fecha_creacion");
                dt.Columns.Add("cantidad");
                dt.Columns.Add("usos");
                dt.Columns.Add("total");

                foreach (IGrouping<string, DataRow> item in datos)
                {
                    DataRow row = dt.NewRow();
                    row["fecha_creacion"] = item.Key;
                    row["cantidad"] = item.Sum<DataRow>(x => Convert.ToDecimal(x["cantidad"]));
                    row["usos"] = item.Sum<DataRow>(x => Convert.ToDecimal(x["usos"]));
                    row["total"] = item.Sum<DataRow>(x => Convert.ToDecimal(x["total"]));

                    dt.Rows.Add(row);
                }

                return dt;
            }

            catch (Exception)
            {
                return null;
            }
        }

        //Funcion para aceptar los certificados de la URL
        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        //FUNCION PARA CREAR EL DATATABLE ORIGINAL
        private void crearDataTable()
        {
            try
            {
                dtAlmacenar = new DataTable();
                dtAlmacenar.Columns.Add("fecha_creacion");
                dtAlmacenar.Columns.Add("cantidad");
                dtAlmacenar.Columns.Add("usos");
                dtAlmacenar.Columns.Add("total");
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CREAR EL DATATABLE AGRUPADO
        private void crearDataTableAgrupado()
        {
            try
            {
                dtAgrupado = new DataTable();
                dtAgrupado.Columns.Add("fecha_creacion");
                dtAgrupado.Columns.Add("cantidad");
                dtAgrupado.Columns.Add("usos");
                dtAgrupado.Columns.Add("total");
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        private void consultarCantidadTasasNoSincronizadas()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(sum(isnull(cantidad_tasa_emitida, 0)), 0) suma" + Environment.NewLine;
                sSql += "from cv403_facturas" + Environment.NewLine;
                sSql += "where emite_tasa_usuario = 1" + Environment.NewLine;
                sSql += "and ambiente_tasa_usuario = " + Convert.ToInt32(Session["emision"].ToString()) + Environment.NewLine;
                sSql += "and id_tasa_anulada = 0" + Environment.NewLine;
                sSql += "and id_tasa_emitida = 0" + Environment.NewLine;
                sSql += "and tasa_emitida = 1" + Environment.NewLine;
                sSql += "and estado in ('A', 'E')" + Environment.NewLine;
                sSql += "and fecha_factura between '" + Convert.ToDateTime(txtFechaInicial.Text.Trim()).ToString("yyyy-MM-dd") + "'" + Environment.NewLine;
                sSql += "and '" + Convert.ToDateTime(txtFechaFinal.Text.Trim()).ToString("yyyy-MM-dd") + "'";
                
                dtConsulta = new DataTable();
                dtConsulta.Clear();
                
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);
                
                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        txtTotalTasasSinSincronizar.Text = dtConsulta.Rows[0][0].ToString();
                        Session["tasas_faltantes"] = dtConsulta.Rows[0][0].ToString();
                    }

                    else
                    {
                        txtTotalTasasSinSincronizar.Text = "0";
                        Session["tasas_faltantes"] = 0;
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



        #endregion

        #region FUNCIONES PARA IMPRIMIR

        //FUNCION PARA IMPRIMIR
        private void imprimirReporte()
        {
            try
            {
                sFecha = DateTime.Now.ToString();

                DSReportes ds = new DSReportes();                

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                dtConsulta = Session["dtAlmacenaTasaDetalle"] as DataTable;

                DataTable dt = ds.Tables["dtReporteDetalleTasas"];
                
                ////AGREGANDO COLUMNAS
                //DataColumn fecha_creacion = new DataColumn("fecha_creacion");
                //fecha_creacion.DataType = System.Type.GetType("System.DateTime");
                //dt.Columns.Add(fecha_creacion);

                //DataColumn cantidad = new DataColumn("cantidad");
                //cantidad.DataType = System.Type.GetType("System.Decimal");
                //dt.Columns.Add(cantidad);

                //DataColumn total = new DataColumn("total");
                //total.DataType = System.Type.GetType("System.Decimal");
                //dt.Columns.Add(total);

                Decimal _dbCantidad, _dbTotal;
                DateTime _dtFecha;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    _dtFecha = Convert.ToDateTime(dtConsulta.Rows[i]["fecha_creacion"].ToString());
                    _dbCantidad = Convert.ToDecimal(dtConsulta.Rows[i]["cantidad"].ToString());
                    _dbTotal= Convert.ToDecimal(dtConsulta.Rows[i]["total"].ToString());

                    DataRow row = dt.NewRow();
                    row["fecha_creacion"] = _dtFecha;
                    row["cantidad"] = _dbCantidad;
                    row["total"] = _dbTotal;
                    dt.Rows.Add(row);
                }

                LocalReport reporteLocal = new LocalReport();
                reporteLocal.ReportPath = Server.MapPath("~/Reportes/rptDetalleTransaccionesTasa.rdlc");

                ReportParameter[] parametros = new ReportParameter[5];
                parametros[0] = new ReportParameter("P_Fecha_Emision", "FECHA DE EMISIÓN: " + Convert.ToDateTime(sFecha).ToString("dd-MM-yyyy"));
                parametros[1] = new ReportParameter("P_Hora_Emision", "HORA: " + Convert.ToDateTime(sFecha).ToString("HH:mm"));
                parametros[2] = new ReportParameter("P_Total_Dias", dt.Rows.Count.ToString() + " Días");
                parametros[3] = new ReportParameter("P_Usuario", Session["usuario"].ToString());
                parametros[4] = new ReportParameter("P_Faltantes_Sincronizar", Session["tasas_faltantes"].ToString());

                ReportDataSource datasource = new ReportDataSource("dsDetalle", dt);
                reporteLocal.DataSources.Add(datasource);
                reporteLocal.SetParameters(parametros);

                Clases.Impresor imp = new Clases.Impresor();
                imp.Imprime(reporteLocal);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

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

        #endregion

        protected void btnExtraerInformacion_Click(object sender, EventArgs e)
        {
            try
            {
                consultarCantidadTasasNoSincronizadas();

                if (conexionInternet() == false)
                {
                    ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "Popup", "swal('Información', 'No hay conexión a Internet. Favor verifique.', 'warning');", true);
                    return;
                }

                else if (Convert.ToDateTime(this.txtFechaInicial.Text.Trim()) > Convert.ToDateTime(this.txtFechaFinal.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "Popup", "swal('Información.!', 'La fecha inicial del rango no puede ser superior a la fecha final.', 'info');", true);
                }

                else
                {
                    DateTime time = Convert.ToDateTime(this.txtFechaInicial.Text.Trim());
                    
                    TimeSpan span = (TimeSpan)(Convert.ToDateTime(this.txtFechaFinal.Text.Trim()) - time);
                    
                    if (span.Days > 15)
                    {
                        ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "Popup", "swal('Información.!', 'La consulta solo se puede efectuar en un rango de 15 días.', 'info');", true);
                    }
                    
                    else
                    {
                        crearJson();

                        if (iBanderaMensaje_P == 0)
                        {
                            ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "Popup", "swal('Informaci\x00f3n.!', 'Servidor remoto de DEVESOFFT no disponible. Intente m\x00e1s tarde.', 'info');", true);
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

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            if (Session["dtAlmacenaTasaDetalle"] == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No hay registros para imprimir el reporte.', 'info');", true);
            }

            else
            {
                dtConsulta = new DataTable();
                dtConsulta = Session["dtAlmacenaTasaDetalle"] as DataTable;

                if (dtConsulta.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No hay registros para imprimir el reporte.', 'info');", true);
                }

                else
                {
                    imprimirReporte();
                }
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            consultarParametros();
            txtFechaFinal.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFechaInicial.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtTotalTasasSinSincronizar.Text = "0";
            Session["dtAlmacenaTasaDetalle"] = null;
            Session["tasas_faltantes"] = null;
            dtAlmacenar = new DataTable();
            dtAlmacenar.Clear();
            crearDataTable();
            dgvDatos.DataSource = dtAlmacenar;
            dgvDatos.DataBind();
            Scroll.Visible = false;
            lblSuma.Text = "Total: 0.00 $";
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