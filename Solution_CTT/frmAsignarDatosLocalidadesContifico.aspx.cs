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
    public partial class frmAsignarDatosLocalidadesContifico : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        Clases_Contifico.ClaseLocalidades localidades;
        Clase_Variables_Contifico.ErrorRespuesta errorRespuesta;

        string sSql;
        string sAccion;
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

            Session["modulo"] = "MóDULO DE ASIGNACIÓN DE VALORES A LAS LOCALIDADES";

            if (!IsPostBack)
            {
                consultarLocalidadesJson();
                llenarGrid();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid()
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_asignar_parametros_contifico";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }

                dgvDatos.Columns[0].Visible = true;
                dgvDatos.DataSource = dtConsulta;
                dgvDatos.DataBind();
                dgvDatos.Columns[0].Visible = false;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA EXTRAER EL JSON DE LOCALIDADES
        private void consultarLocalidadesJson()
        {
            try
            {
                localidades = new Clases_Contifico.ClaseLocalidades();

                string sRespuesta_A = localidades.recuperarJson(Session["tokenSMARTT"].ToString().Trim());

                if (sRespuesta_A == "ERROR")
                {
                    if (localidades.iTipoError == 1)
                    {
                        errorRespuesta = JsonConvert.DeserializeObject<Clase_Variables_Contifico.ErrorRespuesta>(localidades.sError);
                        lblMensajeError.Text = "<b>SMARTT - Información:</b><br/><br/>" + errorRespuesta.detail.Trim(); ;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    }

                    else if (localidades.iTipoError == 2)
                    {
                        lblMensajeError.Text = "<b>SMARTT - Información:</b><br/><br/>" + localidades.sError;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    }

                    return;
                }

                if (sRespuesta_A == "ISNULL")
                {
                    ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "Popup", "swal('Información', 'No se proporcionaron credenciales de autenticación. Tasa de Usuario SMARTT', 'info');", true);
                    return;
                }

                Session["JsonLocalidades"] = sRespuesta_A;

                Clase_Variables_Contifico.Localidades localidad = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Localidades>(sRespuesta_A);

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                dtConsulta.Columns.Add("id");
                dtConsulta.Columns.Add("descripcion");

                for (int i = 0; i < localidad.results.Length; i++)
                {
                    DataRow row = dtConsulta.NewRow();
                    row["id"] = localidad.results[i].id;
                    row["descripcion"] = localidad.results[i].nombre + " - " + localidad.results[i].nombre_comercial;
                    dtConsulta.Rows.Add(row);
                }

                cmbLocalidades.DataSource = dtConsulta;
                cmbLocalidades.DataValueField = "id";
                cmbLocalidades.DataTextField = "descripcion";
                cmbLocalidades.DataBind();

                cmbLocalidades.SelectedIndexChanged -= new EventHandler(cmbLocalidades_SelectedIndexChanged);

                for (int i = 0; i < localidad.results.Length; i++)
                {
                    int iId_P = Convert.ToInt32(cmbLocalidades.SelectedValue);

                    if (Convert.ToInt32(localidad.results[i].id) == iId_P)
                    {
                        Session["idLocalidadJson"] = localidad.results[i].id.ToString();
                        txtNombreLocalidad.Text = localidad.results[i].nombre.Trim().ToUpper();
                        txtRucLocalidad.Text = localidad.results[i].ruc.Trim().ToUpper();
                        txtNombreComercial.Text = localidad.results[i].nombre_comercial.Trim().ToUpper();
                        txtDireccionMatriz.Text = localidad.results[i].direccion_matriz.Trim().ToUpper();
                        txtTarifa.Text = localidad.results[i].tarifa_tasa.ToString().Trim();
                        txtTiempoGracia.Text = localidad.results[i].tiempo_gracia.ToString().Trim();
                        break;
                    }
                }

                cmbLocalidades.SelectedIndexChanged += new EventHandler(cmbLocalidades_SelectedIndexChanged);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA ACTUALIZAR LOS REGISTROS
        private void actualizarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sSql = "";
                sSql += "update ctt_parametro_localidad set" + Environment.NewLine;
                sSql += "id_smartt = " + Session["idLocalidadJson"].ToString() + "," + Environment.NewLine;
                sSql += "nombre_smartt = '" + txtNombreLocalidad.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "ruc_smartt = '" + txtRucLocalidad.Text.Trim() + "'," + Environment.NewLine;
                sSql += "nombre_comercial_smartt = '" + txtNombreComercial.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "direccion_matriz_smartt = '" + txtDireccionMatriz.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "tarifa_tasa_smartt = " + Convert.ToDecimal(txtTarifa.Text.Trim()) + "," + Environment.NewLine;
                sSql += "tiempo_gracia_smartt = " + txtTiempoGracia.Text.Trim() + Environment.NewLine;
                sSql += "where id_ctt_parametro_localidad = " + Session["idParametroLocalidadJson"].ToString();

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    conexionM.reversaTransaccion();
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro actualizado correctamente', 'success');", true);
                return;
            }

            catch (Exception ex)
            {
                conexionM.reversaTransaccion();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                dgvDatos.Columns[0].Visible = true;
                Session["idParametroLocalidadJson"] = dgvDatos.Rows[a].Cells[0].Text;

                if (sAccion == "Editar")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
                }

                dgvDatos.Columns[0].Visible = false;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvDatos.PageIndex = e.NewPageIndex;

                llenarGrid();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
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

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            sAccion = "Editar";
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            actualizarRegistro();
        }

        protected void cmbLocalidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clase_Variables_Contifico.Localidades localidad = JsonConvert.DeserializeObject<Clase_Variables_Contifico.Localidades>(Session["JsonLocalidades"].ToString());

            for (int i = 0; i < localidad.results.Length; i++)
            {
                int iId_P = Convert.ToInt32(cmbLocalidades.SelectedValue);

                if (Convert.ToInt32(localidad.results[i].id) == iId_P)
                {
                    Session["idLocalidadJson"] = localidad.results[i].id.ToString();
                    txtNombreLocalidad.Text = localidad.results[i].nombre.Trim().ToUpper();
                    txtRucLocalidad.Text = localidad.results[i].ruc.Trim().ToUpper();
                    txtNombreComercial.Text = localidad.results[i].nombre_comercial.Trim().ToUpper();
                    txtDireccionMatriz.Text = localidad.results[i].direccion_matriz.Trim().ToUpper();
                    txtTarifa.Text = localidad.results[i].tarifa_tasa.ToString().Trim();
                    txtTiempoGracia.Text = localidad.results[i].tiempo_gracia.ToString().Trim();
                    break;
                }
            }
        }
    }
}