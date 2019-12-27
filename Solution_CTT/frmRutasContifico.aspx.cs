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
using Newtonsoft.Json;

namespace Solution_CTT
{
    public partial class frmRutasContifico : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        Clases_Contifico.ClaseRutas rutas;
        Clase_Variables_Contifico.Rutas ruta;

        string sSql;
        string sAccion;
        string sRespuesta_A;
        string[] sDatosMaximo = new string[5];

        bool bRespuesta;

        DataTable dtConsulta;

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

            Session["modulo"] = "MÓDULO DE RUTAS - SMARTT";

            if (!IsPostBack)
            {
                consultarRutasJson();
                //llenarGrid();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION POSTBACK PARA LAS VIAS
        private void postbackVias()
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                dtConsulta.Columns.Add("via");
                dtConsulta.Columns.Add("descripcion");

                for (int i = 0; i < ruta.results.Length; i++)
                {
                    DataRow row = dtConsulta.NewRow();
                    row["via"] = ruta.results[i].via;
                    row["descripcion"] = ruta.results[i].via;
                    dtConsulta.Rows.Add(row);
                }

                cmbVia.DataSource = dtConsulta;
                cmbVia.DataValueField = "via";
                cmbVia.DataTextField = "descripcion";
                cmbVia.DataBind();

                for (int i = 0; i < ruta.results.Length; i++)
                {
                    string sVia_P = cmbVia.SelectedValue.ToString().Trim().ToLower();

                    if (ruta.results[i].via.ToString().Trim().ToLower() == sVia_P)
                    {
                        Session["PosIRuta"] = i.ToString();

                        Session["viaJson"] = ruta.results[i].via.ToString();
                        txtNombreDestino.Text = ruta.results[i].destino_nombre.Trim().ToUpper();
                        txtNumeroAnden.Text = ruta.results[i].anden.ToString();

                        //CREAR EL COMBO DE PARADAS
                        dtConsulta = new DataTable();
                        dtConsulta.Clear();

                        dtConsulta.Columns.Add("parada_nombre");
                        dtConsulta.Columns.Add("descripcion");

                        for (int j = 0; j < ruta.results[i].paradas.Length; j++)
                        {
                            DataRow row = dtConsulta.NewRow();
                            row["parada_nombre"] = ruta.results[i].paradas[j].parada_nombre.Trim().ToUpper();
                            row["descripcion"] = ruta.results[i].paradas[j].parada_nombre.Trim().ToUpper();
                            dtConsulta.Rows.Add(row);
                        }

                        cmbParadas.DataSource = dtConsulta;
                        cmbParadas.DataValueField = "parada_nombre";
                        cmbParadas.DataTextField = "descripcion";
                        cmbParadas.DataBind();

                        //AQUI LLAMAR LA FUNCION POSTBACK PARADAS
                        postbackParadas(Convert.ToInt32(Session["PosIRuta"].ToString()));

                        break;
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION POSTBACK PARA LAS PARADAS
        private void postbackParadas(int i)
        {
            try
            {
                for (int k = 0; k < ruta.results[i].paradas.Length; k++)
                {
                    string sParada_P = cmbParadas.SelectedValue.ToString().Trim().ToUpper();

                    if (ruta.results[i].paradas[k].parada_nombre.ToString().Trim().ToUpper() == sParada_P)
                    {
                        Session["PosKRuta"] = k.ToString();

                        txtOrdenLlegada.Text = ruta.results[i].paradas[k].orden_llegada.ToString();
                        chkHabilitado.Checked = ruta.results[i].paradas[k].is_enable;

                        //AQUI LLAMAR LA FUNCION POSTBACK TARIFAS
                        postbackTarifas(Convert.ToInt32(Session["PosIRuta"].ToString()), Convert.ToInt32(Session["PosKRuta"].ToString()));

                        break;
                    }
                }

                //for (int k = 0; k < ruta.results[i].paradas.Length; k++)
                //{
                //    string sParada_P = cmbParadas.SelectedValue.ToString().Trim().ToUpper();

                //    if (ruta.results[i].paradas[k].parada_nombre.ToString().Trim().ToUpper() == sParada_P)
                //    {
                //        Session["PosKRuta"] = k.ToString();

                //        txtOrdenLlegada.Text = ruta.results[i].paradas[k].orden_llegada.ToString();
                //        chkHabilitado.Checked = ruta.results[i].paradas[k].is_enable;

                //        //CREAR EL COMBO DE TARIFAS
                //        dtConsulta = new DataTable();
                //        dtConsulta.Clear();

                //        dtConsulta.Columns.Add("id");
                //        dtConsulta.Columns.Add("descripcion");

                //        for (int l = 0; l < ruta.results[i].paradas[k].tarifas.Length; l++)
                //        {
                //            DataRow row = dtConsulta.NewRow();
                //            row["id"] = ruta.results[i].paradas[k].tarifas[l].id;
                //            row["descripcion"] = ruta.results[i].paradas[k].tarifas[l].tipo_servicio_nombre + " - " + ruta.results[i].paradas[k].tarifas[l].tipo_cliente_nombre;
                //            dtConsulta.Rows.Add(row);
                //        }

                //        cmbTarifa.DataSource = dtConsulta;
                //        cmbTarifa.DataValueField = "id";
                //        cmbTarifa.DataTextField = "descripcion";
                //        cmbTarifa.DataBind();

                //        //AQUI LLAMAR LA FUNCION POSTBACK TARIFAS
                //        postbackTarifas(Convert.ToInt32(Session["PosIRuta"].ToString()), Convert.ToInt32(Session["PosKRuta"].ToString()));

                //        break;
                //    }                    
                //}
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION POSTBACK PARA LAS TARIFAS
        private void postbackTarifas(int i, int k)
        {
            try
            {
                dgvDatos.DataSource = ruta.results[i].paradas[k].tarifas;
                dgvDatos.DataBind();

                //for (int m = 0; m < ruta.results[i].paradas[k].tarifas.Length; m++)
                //{
                //    int iTarifa_P = ruta.results[i].paradas[k].tarifas[m].id;

                //    if (Convert.ToInt32(cmbTarifa.SelectedValue) == iTarifa_P)
                //    {
                //        txtIdTarifa.Text = ruta.results[i].paradas[k].tarifas[m].id.ToString();
                //        txtTipoServicio.Text = ruta.results[i].paradas[k].tarifas[m].tipo_servicio.ToString();
                //        txtNombreServicio.Text = ruta.results[i].paradas[k].tarifas[m].tipo_servicio_nombre.Trim().ToUpper();
                //        txtTipoCliente.Text = ruta.results[i].paradas[k].tarifas[m].tipo_cliente.ToString();
                //        txtClienteNombre.Text = ruta.results[i].paradas[k].tarifas[m].tipo_cliente_nombre.Trim().ToUpper();
                //        txtTarifa.Text = ruta.results[i].paradas[k].tarifas[m].tarifa.ToString("N2");
                //        txtActualizacion.Text = ruta.results[i].paradas[k].tarifas[m].actualizacion.ToString();
                //        chkEspecialTarifa.Checked = ruta.results[i].paradas[k].tarifas[m].especial;
                //        chkActivoTarifa.Checked = ruta.results[i].paradas[k].tarifas[m].is_active;
                //        chkHabilitadoTarifa.Checked = ruta.results[i].paradas[k].tarifas[m].is_enable;
                //        break;
                //    }
                //}
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA EXTRAER EL JSON DE LOCALIDADES
        private void consultarRutasJson()
        {
            try
            {
                rutas = new Clases_Contifico.ClaseRutas();

                sRespuesta_A = rutas.recuperarJson(Session["tokenSMARTT"].ToString().Trim());

                if (sRespuesta_A == "ERROR")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'No se pudo obtener registros para la tasa de usuario SMARTT', 'error')</script>");
                    return;
                }

                if (sRespuesta_A == "ISNULL")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Información.!', 'No se proporcionaron credenciales de autenticación. Tasa de Usuario SMARTT', 'info')</script>");
                    return;
                }

                Session["JsonRutas"] = sRespuesta_A;                
                ruta = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Rutas>(sRespuesta_A);
                postbackVias();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            
        }

        protected void cmbTarifa_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AQUI LLAMAR LA FUNCION POSTBACK TARIFAS
            sRespuesta_A = Session["JsonRutas"].ToString();
            ruta = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Rutas>(sRespuesta_A);

            postbackTarifas(Convert.ToInt32(Session["PosIRuta"].ToString()), Convert.ToInt32(Session["PosKRuta"].ToString()));
        }

        protected void cmbParadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AQUI LLAMAR LA FUNCION POSTBACK PARADAS
            sRespuesta_A = Session["JsonRutas"].ToString();
            ruta = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Rutas>(sRespuesta_A);

            postbackParadas(Convert.ToInt32(Session["PosIRuta"].ToString()));
        }

        protected void cmbVia_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AQUI LLAMAR LA FUNCION POSTBACK VIAS
            sRespuesta_A = Session["JsonRutas"].ToString();
            ruta = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Rutas>(sRespuesta_A);

            //postbackVias();
            //postbackParadas(Convert.ToInt32(cmbVia.SelectedValue));

            for (int i = 0; i < ruta.results.Length; i++)
            {
                string sVia_P = cmbVia.SelectedValue.ToString().Trim().ToLower();

                if (ruta.results[i].via.ToString().Trim().ToLower() == sVia_P)
                {
                    Session["PosIRuta"] = i.ToString();

                    Session["viaJson"] = ruta.results[i].via.ToString();
                    txtNombreDestino.Text = ruta.results[i].destino_nombre.Trim().ToUpper();
                    txtNumeroAnden.Text = ruta.results[i].anden.ToString();

                    //CREAR EL COMBO DE PARADAS
                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    dtConsulta.Columns.Add("parada_nombre");
                    dtConsulta.Columns.Add("descripcion");

                    for (int j = 0; j < ruta.results[i].paradas.Length; j++)
                    {
                        DataRow row = dtConsulta.NewRow();
                        row["parada_nombre"] = ruta.results[i].paradas[j].parada_nombre.Trim().ToUpper();
                        row["descripcion"] = ruta.results[i].paradas[j].parada_nombre.Trim().ToUpper();
                        dtConsulta.Rows.Add(row);
                    }

                    cmbParadas.DataSource = dtConsulta;
                    cmbParadas.DataValueField = "parada_nombre";
                    cmbParadas.DataTextField = "descripcion";
                    cmbParadas.DataBind();

                    //AQUI LLAMAR LA FUNCION POSTBACK PARADAS
                    postbackParadas(Convert.ToInt32(Session["PosIRuta"].ToString()));

                    break;
                }
            }
        }
    }
}