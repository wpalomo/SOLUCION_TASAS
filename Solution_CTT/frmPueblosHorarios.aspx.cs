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

namespace Solution_CTT
{
    public partial class frmPueblosHorarios : System.Web.UI.Page
    {
        ENTPueblosHorarios puebloHorarioE = new ENTPueblosHorarios();
        ENTHoraPueblos horaPuebloE = new ENTHoraPueblos();

        manejadorConexion conexionM = new manejadorConexion();
        manejadorPueblosHorarios puebloHorarioM = new manejadorPueblosHorarios();
        manejadorHoraPueblos horaPuebloM = new manejadorHoraPueblos();

        string sSql;
        string[] sDatosMaximo = new string[5];

        DataTable dtConsulta;

        bool bRespuesta;

        int iIdHorarioGrid;
        int iIdHorarioConsulta;
        int iHabilitado;
        int iIdPueblo;
        int iIdPuebloConsulta;
        int iBandera;

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

            Session["modulo"] = "MÓDULO DE ASIGNACIÓN DE HORARIOS PARA LAS OFICINAS";

            if (!IsPostBack)
            {
                limpiar();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA MANIPULAR LAS COLUMNAS DEL GRID
        private void columnasGrid(bool ok)
        {
            try
            {
                dgvDatos.Columns[1].Visible = ok;
                dgvDatos.Columns[0].ItemStyle.Width = 50;
                dgvDatos.Columns[2].ItemStyle.Width = 150;
                dgvDatos.Columns[3].ItemStyle.Width = 150;
                dgvDatos.Columns[4].ItemStyle.Width = 150;
                dgvDatos.Columns[5].ItemStyle.Width = 100;
                dgvDatos.Columns[6].ItemStyle.Width = 100;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }
        
        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_pueblo, descripcion, provincia, estado_pueblo," + Environment.NewLine;
                sSql += "count(id_ctt_pueblo_horario) cuenta" + Environment.NewLine;
                sSql += "from ctt_vw_pueblos_horarios" + Environment.NewLine;
                sSql += "group by id_ctt_pueblo, descripcion, provincia, estado_pueblo";

                columnasGrid(true);
                puebloHorarioE.ISQL = sSql;
                dgvDatos.DataSource = puebloHorarioM.listarPueblosHorarios(puebloHorarioE);
                dgvDatos.DataBind();
                columnasGrid(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }


        //FUNCION PARA MANIPULAR LAS COLUMNAS DEL GRID DE HORARIOS
        private void columnasGridHorarios(bool ok)
        {
            try
            {
                dgvHorarios.Columns[0].Visible = ok;
                dgvHorarios.Columns[1].ItemStyle.Width = 100;
                dgvHorarios.Columns[2].ItemStyle.Width = 150;
                dgvHorarios.Columns[3].ItemStyle.Width = 50;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL GRID DE HORARIOS
        private void llenarGridHorarios()
        {
            try
            {
                sSql = "";
                sSql += "select H.id_ctt_horario, H.hora_salida, J.descripcion" + Environment.NewLine;
                sSql += "from ctt_horarios H, ctt_jornada J" + Environment.NewLine;
                sSql += "where H.id_ctt_jornada = J.id_ctt_jornada" + Environment.NewLine;
                sSql += "and H.estado = 'A'" + Environment.NewLine;
                sSql += "and J.estado = 'A'" + Environment.NewLine;
                sSql += "order by H.hora_salida";

                columnasGridHorarios(true);
                horaPuebloE.ISQL = sSql;
                dgvHorarios.DataSource = horaPuebloM.listarHoraPueblos(horaPuebloE);
                dgvHorarios.DataBind();
                columnasGridHorarios(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }


        //FUNCION PARA CONSULTAR LOS REGISTROS DEL TERMINAL
        private void consultarRegistrosTerminal(int iIdPueblo)
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_pueblo_horario, id_ctt_horario, habilitado, id_ctt_pueblo" + Environment.NewLine;
                sSql += "from ctt_pueblos_horarios" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + iIdPueblo + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    Session["dtConsulta"] = dtConsulta;
                    pnlHorarios.Enabled = true;

                    if (dtConsulta.Rows.Count > 0)
                    {
                        //INSTRUCCION PARA CARGAR LOS CHECKBOX DEL GRIDVIEW
                        recorrerGrid();
                    }

                    else
                    {
                        foreach (GridViewRow row in dgvHorarios.Rows)
                        {
                            iIdHorarioGrid = Convert.ToInt32(row.Cells[0].Text);
                            //CheckBox chkSeleccion = row.FindControl("chkSeleccionar") as CheckBox;
                            CheckBox chkSeleccion = (CheckBox)row.FindControl("chkSeleccionar");
                            chkSeleccion.Checked = false;
                        }
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

        //FUNCION PARA COMPROBAR LOS CHECK CON EL GRIDVIEW
        private void recorrerGrid()
        {
            try
            {
                columnasGridHorarios(true);

                foreach (GridViewRow row in dgvHorarios.Rows)
                {
                    iIdHorarioGrid = Convert.ToInt32(row.Cells[0].Text);
                    //CheckBox chkSeleccion = row.FindControl("chkSeleccionar") as CheckBox;
                    CheckBox chkSeleccion = (CheckBox)row.FindControl("chkSeleccionar");

                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        iIdHorarioConsulta = Convert.ToInt32(dtConsulta.Rows[i][1].ToString());
                        iHabilitado = Convert.ToInt32(dtConsulta.Rows[i][2].ToString());

                        if (iIdHorarioConsulta == iIdHorarioGrid)
                            {
                            if (iHabilitado == 1)
                            {
                                chkSeleccion.Checked = true;
                                break;
                            }

                            else
                            {
                                chkSeleccion.Checked = false;
                                break;
                            }
                        }

                    }
                }

                columnasGridHorarios(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }


        //FUNCION PARA INSERTAR LOS REGISTROS
        private bool insertarRegistro(int iIdHorario_P, int iHabilitado_P)
        {
            try
            {
                //if (conexionM.iniciarTransaccion() == false)
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                //    goto fin;
                //}

                sSql = "";
                sSql += "insert into ctt_pueblos_horarios (" + Environment.NewLine;
                sSql += "id_ctt_pueblo, id_ctt_horario, habilitado, estado," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Session["idPueblo"].ToString()) + ", " + iIdHorario_P + ", ";
                sSql += iHabilitado_P + ", 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUCION DE LA INSTRUCCION SQL
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); return false; }
        }

        //FUNCION PARA ACTUALIZAR LOS REGISTROS
        private bool actualizarRegistro(int iIdHorario_P, int iHabilitado_P)
        {
            try
            {
                sSql = "";
                sSql += "update ctt_pueblos_horarios set" + Environment.NewLine;
                sSql += "habilitado = " + iHabilitado_P + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(Session["idPueblo"].ToString()) + Environment.NewLine;
                sSql += "and id_ctt_horario = " + iIdHorario_P + Environment.NewLine;
                sSql += "and estado  = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); return false; }
        }

        //FUNCION PARA LIMPIAR EL FORMULARIO
        private void limpiar()
        {
            llenarGrid(0);
            llenarGridHorarios();
            Session["idPueblo"] = null;
        }

        #endregion

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                Session["idPueblo"] = dgvDatos.Rows[a].Cells[1].Text;
                lblHorarios.Text = ("HORARIOS " + dgvDatos.Rows[a].Cells[2].Text).Trim().ToUpper();
                consultarRegistrosTerminal(Convert.ToInt32(dgvDatos.Rows[a].Cells[0].Text));
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

            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
           if (txtFiltrar.Text.Trim() == "")
           {
               llenarGrid(0);
           }

            else
           {
               llenarGrid(1);
           }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["idPueblo"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Aviso.!', 'No puede procesar información si no ha seleccionado un terminal.', 'warning');", true);
                    goto fin;
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                dtConsulta = Session["dtConsulta"] as DataTable;

                iIdPueblo = Convert.ToInt32(Session["idPueblo"].ToString());

                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                foreach (GridViewRow row in dgvHorarios.Rows)
                {
                    iIdHorarioGrid = Convert.ToInt32(row.Cells[0].Text);

                    CheckBox chkSeleccion = (CheckBox)row.FindControl("chkSeleccionar");

                    if (chkSeleccion.Checked == true)
                    {
                        iHabilitado = 1;
                    }

                    else
                    {
                        iHabilitado = 0;
                    }

                    iBandera = 0;

                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        iIdHorarioConsulta = Convert.ToInt32(dtConsulta.Rows[i][1].ToString());
                        iIdPuebloConsulta = Convert.ToInt32(dtConsulta.Rows[i][3].ToString());
                        
                        if ((iIdHorarioConsulta == iIdHorarioGrid) && (iIdPuebloConsulta == iIdPueblo))
                        {
                            if (actualizarRegistro(iIdHorarioGrid, iHabilitado) == true)
                            {
                                iBandera = 1;
                                break;
                            }

                            else
                            {
                                goto fin;
                            }
                        }
                    }


                    if (iBandera == 0)
                    {
                        if (insertarRegistro(iIdHorarioGrid, iHabilitado) == false)
                        {
                            goto fin;
                        }
                    }
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registros procesado éxitosamente.', 'success');", true);
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }

            //reversa: { conexionM.reversaTransaccion(); }
            fin: { }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmPueblosHorarios.aspx");
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