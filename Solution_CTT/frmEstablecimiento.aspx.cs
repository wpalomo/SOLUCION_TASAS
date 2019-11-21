using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Data;
using ENTIDADES;
using NEGOCIO;
using System.Drawing;

namespace Solution_Encomiendas
{
    public partial class frmEstablecimiento : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();
        ENTEstablecimiento establecimientoE = new ENTEstablecimiento();
        manejadorEstablecimiento establecimientoM = new manejadorEstablecimiento();

        string sSql;
        string[] sDatosMaximo = new string[5];
        string sAccion;

        DataTable dtConsulta;
        bool bRespuesta;

        int iConsultarRegistro;
        int iIdLocalidad;
        bool bActualizar;

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

            Session["ModuloSistema"] = "Inicio / Facturación Electrónica / Configuración / Establecimeinto";

            if (!IsPostBack)
            {
                llenarGrid(0);
            }
        }
        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LAS COLUMNAS DEL GRID
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[7].Visible = ok;
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql = sSql + "Select TP.Codigo, TP.valor_texto  +" + Environment.NewLine;
                sSql = sSql + "case LOC.emite_comprobante_electronico when 1 then ' electronico' else '' end Nombres," + Environment.NewLine;
                sSql = sSql + "LOC.establecimiento 'Est.',LOC.punto_emision 'Pto. Emi.', LOC.Direccion," + Environment.NewLine;
                sSql = sSql + "case (LOC.Estado) when 'A' then 'ACTIVO' else 'INACTIVO' end Estado," + Environment.NewLine;
                sSql = sSql + "LOC.Id_localidad" + Environment.NewLine;
                sSql = sSql + "From tp_localidades LOC, tp_Codigos TP" + Environment.NewLine;
                sSql = sSql + "Where LOC.cg_localidad = TP.correlativo and LOC.Estado ='A'" + Environment.NewLine;
                

                if (iOp == 1)
                {
                    //sSql += "and codigo like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                    //sSql += "or nombres like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                }
                sSql = sSql + "order by TP.Codigo";

                columnasGrid(true);
                establecimientoE.ISQL = sSql;
                dgvDatos.DataSource = establecimientoM.listar(establecimientoE);
                dgvDatos.DataBind();
                columnasGrid(false);

            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }        

        //FUNVION INSERTAR
        
        //FUNCION PARA ACTUALIZAR EN LA BASE DE DATOS
        private void actualizarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sSql = "";
                sSql = sSql + "Update tp_localidades Set" + Environment.NewLine;
                sSql = sSql + "establecimiento= '" + txtEstablecimiento.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "punto_emision= '" + txtPuntoEmision.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "Direccion= '" + txtDireccion.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "Usuario_Ingreso = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql = sSql + "Terminal_Ingreso = '" + sDatosMaximo[1] + "'," + Environment.NewLine;
                sSql = sSql + "Fecha_Ingreso = GetDate()" + Environment.NewLine;
                sSql = sSql + "Where Id_localidad = " + iIdLocalidad;

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_MSJ_TITULO_EXITO + "', '" + Resources.MESSAGES.TXT_MSJ_BODY_EXITO_UPDATE + "', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };

        fin: { };
        }
        //FUNCION PARA ELIMINAR EN LA BASE DE DATOS
        private void eliminarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sSql = "";
                sSql = sSql + "Update tp_localidades Set" + Environment.NewLine;
                sSql = sSql + "estado = 'E'," + Environment.NewLine;
                sSql = sSql + "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql = sSql + "terminal_anula = '" + sDatosMaximo[1] + "'," + Environment.NewLine;
                sSql = sSql + "fecha_anula = GetDate()" + Environment.NewLine;
                sSql = sSql + "Where id_directorio = " + iIdLocalidad;

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_MSJ_TITULO_EXITO + "', '" + Resources.MESSAGES.TXT_MSJ_BODY_EXITO_DELETE + "', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };

        fin: { };
        }

        //VAlIDAR
        //private bool Validar()
        //{
            //if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtDescripcion.Text))
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_ADVERTENCIA + " ', '" + Resources.MESSAGES.TXT_ADVERTENCIA_MSJ + "', 'warning');", true);
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
        //}

        //FUNCION PARA CONSULTAR SI EXISTE EL REGISTRO EN LA BASE DE DATOS
        private int consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from cel_tipo_comprobante" + Environment.NewLine;
                sSql += "where codigo = '" + txtCodigo.Text.Trim() + "'" + Environment.NewLine;
                //sSql += "or nombres = '" + txtDescripcion.Text.Trim() + "'" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    return Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return -1;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return -1;
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtCodigo.Text = "";
            txtNombres.Text = "";
            txtEstablecimiento.Text = "";
            txtPuntoEmision.Text = "";
            txtDireccion.Text = "";
            txtEstado.Text = "";
            iIdLocalidad = 0;
            bActualizar = false;

            Session["idRegistro"] = null;
            btnSave.Text = "Crear";
            btnSave.Attributes.Add("Class", "form-control btn btn-block btn-primary");
            txtCodigo.ReadOnly = false;
            txtCodigo.Focus();
            llenarGrid(0);
        }

        #endregion

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            sAccion = "Editar";
            btnSave.Text = "Editar";
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            sAccion = "Eliminar";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
        }
        
        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);
                Session["idRegistro"] = dgvDatos.Rows[a].Cells[0].Text;

                if (sAccion == "Editar")
                {
                    txtCodigo.Text = dgvDatos.Rows[a].Cells[1].Text;
                    txtNombres.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[2].Text);
                    txtEstablecimiento.Text = dgvDatos.Rows[a].Cells[3].Text;
                    txtPuntoEmision.Text = dgvDatos.Rows[a].Cells[4].Text;
                    txtDireccion.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[5].Text);
                    txtEstado.Text = dgvDatos.Rows[a].Cells[6].Text;

                    //CAMBIO ESTILO AL BOTON CREAR/EDITAR
                    btnSave.Text = "Editar";
                    btnSave.Attributes.Add("Class", "form-control btn btn-block btn-warning");
                    dgvDatos.SelectedRow.BackColor = System.Drawing.Color.FromName("#ccf0cb");//PINTO CELDA SELECCIONADA

                    txtCodigo.ReadOnly = true;
                }
                columnasGrid(false);

                txtNombres.Focus();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtNombres.Text) || string.IsNullOrEmpty(txtEstablecimiento.Text) || string.IsNullOrEmpty(txtPuntoEmision.Text) || string.IsNullOrEmpty(txtDireccion.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('" + Resources.MESSAGES.TXT_ADVERTENCIA + "', '" + Resources.MESSAGES.TXT_ADVERTENCIA_MSJ + "', 'success');", true);
            }
            else
            {
                if (Session["idRegistro"] == null)
                {
                    //ENVIO A FUNCION DE INSERCION
                    //insertarRegistro();
                }

                else
                {
                    actualizarRegistro();
                }
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
    }
}