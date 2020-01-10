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
        Clase_Variables_Contifico.ErrorRespuesta errorRespuesta;

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
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION POSTBACK PARA LAS VIAS
        private void postbackVias_2()
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                dtConsulta.Columns.Add("id");
                dtConsulta.Columns.Add("descripcion");

                for (int i = 0; i < ruta.results.Length; i++)
                {
                    DataRow row = dtConsulta.NewRow();
                    row["id"] = (i + 1).ToString();
                    row["descripcion"] = ruta.results[i].via;
                    dtConsulta.Rows.Add(row);
                }

                cmbVia.DataSource = dtConsulta;
                cmbVia.DataValueField = "id";
                cmbVia.DataTextField = "descripcion";
                cmbVia.DataBind();

                int iIdVia = Convert.ToInt32(cmbVia.SelectedValue) - 1;
                txtNombreDestino.Text = ruta.results[iIdVia].destino_nombre.Trim().ToUpper();
                txtNumeroAnden.Text = ruta.results[iIdVia].anden.ToString();

                postbackParadas_2(iIdVia);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMBO DE PARADAS
        private void postbackParadas_2(int iPosicion_P)
        {
            try
            {
                //CREAR EL COMBO DE PARADAS
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                dtConsulta.Columns.Add("id");
                dtConsulta.Columns.Add("descripcion");

                for (int j = 0; j < ruta.results[iPosicion_P].paradas.Length; j++)
                {
                    DataRow row = dtConsulta.NewRow();
                    row["id"] = (j + 1).ToString();
                    row["descripcion"] = ruta.results[iPosicion_P].paradas[j].parada_nombre.Trim().ToUpper();
                    dtConsulta.Rows.Add(row);
                }

                cmbParadas.DataSource = dtConsulta;
                cmbParadas.DataValueField = "id";
                cmbParadas.DataTextField = "descripcion";
                cmbParadas.DataBind();

                int iIdParada = Convert.ToInt32(cmbParadas.SelectedValue) - 1;
                txtOrdenLlegada.Text = ruta.results[iPosicion_P].paradas[iIdParada].orden_llegada.ToString();
                chkHabilitado.Checked = ruta.results[iPosicion_P].paradas[iIdParada].is_enable;

                postbackTarifas_2(iPosicion_P, iIdParada);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        private void postbackTarifas_2(int iPosicion_X, int iPosicion_Y)
        {
            try
            {
                dgvDatos.DataSource = ruta.results[iPosicion_X].paradas[iPosicion_Y].tarifas;
                dgvDatos.DataBind();
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
                    if (rutas.iTipoError == 1)
                    {
                        errorRespuesta = JsonConvert.DeserializeObject<Clase_Variables_Contifico.ErrorRespuesta>(rutas.sError);
                        lblMensajeError.Text = "<b>SMARTT - Información:</b><br/><br/>" + errorRespuesta.detail.Trim(); ;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    }

                    else if (rutas.iTipoError == 2)
                    {
                        lblMensajeError.Text = "<b>SMARTT - Información:</b><br/><br/>" + rutas.sError;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    }

                    return;
                }

                if (sRespuesta_A == "ISNULL")
                {
                    ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "Popup", "swal('Información', 'No se proporcionaron credenciales de autenticación. Tasa de Usuario SMARTT', 'info');", true);
                    return;
                }

                Session["JsonRutas"] = sRespuesta_A;                
                ruta = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Rutas>(sRespuesta_A);
                postbackVias_2();
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

        protected void cmbParadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AQUI LLAMAR LA FUNCION POSTBACK PARADAS
            sRespuesta_A = Session["JsonRutas"].ToString();
            ruta = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Rutas>(sRespuesta_A);

            int iIdVia = Convert.ToInt32(cmbVia.SelectedValue) - 1;
            int iIdParada = Convert.ToInt32(cmbParadas.SelectedValue) - 1;

            postbackTarifas_2(iIdVia, iIdParada);
        }

        protected void cmbVia_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AQUI LLAMAR LA FUNCION POSTBACK VIAS
            sRespuesta_A = Session["JsonRutas"].ToString();
            ruta = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Rutas>(sRespuesta_A);

            int iIdVia = Convert.ToInt32(cmbVia.SelectedValue) - 1;
            txtNombreDestino.Text = ruta.results[iIdVia].destino_nombre.Trim().ToUpper();
            txtNumeroAnden.Text = ruta.results[iIdVia].anden.ToString();

            postbackParadas_2(iIdVia);
        }
    }
}