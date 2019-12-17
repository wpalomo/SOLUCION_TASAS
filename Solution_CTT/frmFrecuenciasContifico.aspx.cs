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
    public partial class frmFrecuenciasContifico : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        Clases_Contifico.ClaseFrecuencias frecuencias;
        Clase_Variables_Contifico.Frecuencias frecuencia;

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

            Session["modulo"] = "MÓDULO DE FRECUENCIAS - SMARTT";

            if (!IsPostBack)
            {
                consultarFrecuenciasJson();
                //llenarGrid();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA EXTRAER EL JSON DE FRECUENCIA
        private void consultarFrecuenciasJson()
        {
            try
            {
                frecuencias = new Clases_Contifico.ClaseFrecuencias();

                sRespuesta_A = frecuencias.recuperarJson(Session["tokenSMARTT"].ToString().Trim());

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

                Session["JsonFrecuencias"] = sRespuesta_A;

                frecuencia = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Frecuencias>(sRespuesta_A);

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                dtConsulta.Columns.Add("id");
                dtConsulta.Columns.Add("descripcion");

                for (int i = 0; i < frecuencia.results.Length; i++)
                {
                    DataRow row = dtConsulta.NewRow();
                    row["id"] = (i + 1).ToString();
                    row["descripcion"] = frecuencia.results[i].hora_salida;
                    dtConsulta.Rows.Add(row);
                }

                cmbFrecuencias.DataSource = dtConsulta;
                cmbFrecuencias.DataValueField = "id";
                cmbFrecuencias.DataTextField = "descripcion";
                cmbFrecuencias.DataBind();

                postbackFrecuencia();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //POSTBACK DEL COMBOBOX DE FRECUENCIAS
        private void postbackFrecuencia()
        {
            try
            {
                sRespuesta_A = Session["JsonFrecuencias"].ToString();
                frecuencia = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Frecuencias>(sRespuesta_A);

                int iPosicion = Convert.ToInt32(cmbFrecuencias.SelectedValue) - 1;

                for (int i = 0; i < frecuencia.results.Length; i++)
                {
                    if (i == iPosicion)
                    {
                        txtNombreDestino.Text = frecuencia.results[i].destino_nombre.Trim().ToUpper();
                        txtVia.Text = frecuencia.results[i].via.Trim().ToUpper();
                        txtTipo.Text = frecuencia.results[i].tipo.ToString();
                        txtTipoNombre.Text = frecuencia.results[i].tipo_nombre.Trim().ToUpper();
                        
                        if (string.IsNullOrEmpty(frecuencia.results[i].fecha_validez))
                        {
                            txtFechaValidez.Text = "";
                        }

                        else
                        {
                            txtFechaValidez.Text = frecuencia.results[i].fecha_validez;
                        }

                        chkDomingo.Checked = false;
                        chkLunes.Checked = false;
                        chkMartes.Checked = false;
                        chkMiercoles.Checked = false;
                        chkJueves.Checked = false;
                        chkViernes.Checked = false;
                        chkSabado.Checked = false;

                        //RECORRER LOS DIAS
                        foreach (int iDia in frecuencia.results[i].dias)
                        {
                            if (iDia == 0)
                            {
                                chkDomingo.Checked = true;
                            }

                            if (iDia == 1)
                            {
                                chkLunes.Checked = true;
                            }

                            if (iDia == 2)
                            {
                                chkMartes.Checked = true;
                            }

                            if (iDia == 3)
                            {
                                chkMiercoles.Checked = true;
                            }

                            if (iDia == 4)
                            {
                                chkJueves.Checked = true;
                            }

                            if (iDia == 5)
                            {
                                chkViernes.Checked = true;
                            }

                            if (iDia == 6)
                            {
                                chkSabado.Checked = true;
                            }
                        }

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

        #endregion

        protected void btnAccept_Click(object sender, EventArgs e)
        {

        }

        protected void cmbFrecuencias_SelectedIndexChanged(object sender, EventArgs e)
        {
            postbackFrecuencia();
        }
    }
}