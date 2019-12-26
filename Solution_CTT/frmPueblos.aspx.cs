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
    public partial class frmPueblos : System.Web.UI.Page
    {
        ENTPueblos puebloE = new ENTPueblos();
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorPueblos puebloM = new manejadorPueblos();
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string[] sDatosMaximo = new string[5];
        string sAccion;

        int iEsTerminal;
        int iEsEncomienda;
        int iIdLocalidadTerminal;
        int iIdLocalidadEncomienda;
        int iCobrosAdministracion;
        int iCobrosOtros;
        int iAplicaTasaUsuario;
        int iIdProveedorTasa;

        DataTable dtConsulta;
        bool bRespuesta;

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

            Session["modulo"] = "MÓDULO DE OFICINAS";

            if (!IsPostBack)
            {
                limpiar();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMOBOX DE PROVINCIAS
        private void llenarComboProvincias()
        {
            try
            {
                sSql = "";
                sSql += "select idsisprovincia, nombres" + Environment.NewLine;
                sSql += "from sisprovincia" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "order by nombres";

                comboE.ISSQL = sSql;
                cmbProvincia.DataSource = comboM.listarCombo(comboE);
                cmbProvincia.DataValueField = "IID";
                cmbProvincia.DataTextField = "IDATO";
                cmbProvincia.DataBind();
                cmbProvincia.Items.Insert(0, new ListItem("Seleccione Provincia", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE LOCALIDADES
        private void llenarComboLocalidades()
        {
            try
            {
                sSql = "";
                sSql += "select id_localidad, nombre_localidad" + Environment.NewLine;
                sSql += "from tp_vw_localidades";

                comboE.ISSQL = sSql;
                cmbLocalidadTerminal.DataSource = comboM.listarCombo(comboE);
                cmbLocalidadTerminal.DataValueField = "IID";
                cmbLocalidadTerminal.DataTextField = "IDATO";
                cmbLocalidadTerminal.DataBind();
                cmbLocalidadTerminal.Items.Insert(0, new ListItem("Seleccione Localidad", "0"));

                cmbLocalidadEncomienda.DataSource = comboM.listarCombo(comboE);
                cmbLocalidadEncomienda.DataValueField = "IID";
                cmbLocalidadEncomienda.DataTextField = "IDATO";
                cmbLocalidadEncomienda.DataBind();
                cmbLocalidadEncomienda.Items.Insert(0, new ListItem("Seleccione Localidad", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE PROVEEDORES DE TASAS
        //private void llenarComboProveedoresTasas()
        //{
        //    try
        //    {
        //        sSql = "";
        //        sSql += "select id_ctt_proveedor_tasa, descripcion" + Environment.NewLine;
        //        sSql += "from ctt_proveedores_tasas" + Environment.NewLine;
        //        sSql += "where estado = 'A'" + Environment.NewLine;
        //        sSql += "order by descripcion";

        //        comboE.ISSQL = sSql;
        //        cmbProveedoresTasas.DataSource = comboM.listarCombo(comboE);
        //        cmbProveedoresTasas.DataValueField = "IID";
        //        cmbProveedoresTasas.DataTextField = "IDATO";
        //        cmbProveedoresTasas.DataBind();
        //        cmbProveedoresTasas.Items.Insert(0, new ListItem("Seleccione Proveedor", "0"));

        //    }

        //    catch (Exception ex)
        //    {
        //        lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
        //    }
        //}


        //FUNCION PARA LAS COLUMNAS
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[2].Visible = ok;
            dgvDatos.Columns[3].Visible = ok;
            dgvDatos.Columns[4].Visible = ok;
            dgvDatos.Columns[9].Visible = ok;
            dgvDatos.Columns[10].Visible = ok;

            dgvDatos.Columns[0].ItemStyle.Width = 50;
            dgvDatos.Columns[5].ItemStyle.Width = 200;
            dgvDatos.Columns[6].ItemStyle.Width = 150;
            dgvDatos.Columns[7].ItemStyle.Width = 100;
            dgvDatos.Columns[8].ItemStyle.Width = 100;
            dgvDatos.Columns[11].ItemStyle.Width = 100;
            dgvDatos.Columns[12].ItemStyle.Width = 100;
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_pueblos_terminales_encomiendas" + Environment.NewLine;

                //OPCION 1 PARA FILTRAR POR LOCALIDADES
                //OPCION 2 PARA FILTRAR POR PROVINCIAS
                //OPCION 3 PARA FILTRAR POR LOCALIDAD Y PROVINCIA
                //OPCION 4 PARA FILTRAR POR BUSQUEDA EN CAJA DE TEXTO
                if (iOp == 1)
                {
                    sSql += "where descripcion like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by id_ctt_pueblo" + Environment.NewLine;

                columnasGrid(true);
                puebloE.ISQL = sSql;
                dgvDatos.DataSource = puebloM.listarPueblos(puebloE);
                dgvDatos.DataBind();
                columnasGrid(false);

            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private void insertarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sSql = "";
                sSql += "insert into ctt_pueblos (" + Environment.NewLine;
                sSql += "id_localidad_terminal, id_localidad_encomienda, idsisprovincia," + Environment.NewLine;
                sSql += "descripcion, terminal, encomienda, cobros_administracion, cobros_otros," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdLocalidadTerminal + ", " + iIdLocalidadEncomienda + ", " + Convert.ToInt32(cmbProvincia.SelectedValue) + ", ";
                sSql += "'" + txtDescripcion.Text.Trim().ToUpper() + "', " + iEsTerminal + ", " + iEsEncomienda + "," + Environment.NewLine;
                sSql += iCobrosAdministracion + ", " + iCobrosOtros + ", 'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro ingresado correctamente', 'success');", true);
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
                sSql += "update ctt_pueblos set" + Environment.NewLine;
                sSql += "id_localidad_terminal = " + iIdLocalidadTerminal + "," + Environment.NewLine;
                sSql += "id_localidad_encomienda = " + iIdLocalidadEncomienda + "," + Environment.NewLine;
                sSql += "idsisprovincia = " + Convert.ToInt32(cmbProvincia.SelectedValue) + "," + Environment.NewLine;                
                sSql += "descripcion = '" + txtDescripcion.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "terminal = " + iEsTerminal + "," + Environment.NewLine;
                sSql += "encomienda = " + iEsEncomienda + "," + Environment.NewLine;
                sSql += "cobros_administracion = " + iCobrosAdministracion + "," + Environment.NewLine;
                sSql += "cobros_otros = " + iCobrosOtros + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(Session["idRegistroPUEBLO"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro actualizado correctamente', 'success');", true);
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
                sSql += "update ctt_pueblos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(Session["idRegistroPUEBLO"]);

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro eliminado correctamente', 'success');", true);
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

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtDescripcion.Text = "";
            Session["idRegistroPUEBLO"] = null;
            llenarComboLocalidades();
            llenarComboProvincias();
            btnSave.Text = "Crear";
            MsjValidarCampos.Visible = false;

            chkAplicaTerminal.Checked = false;
            chkAplicaEncomienda.Checked = false;
            chkCobrosAdministracion.Checked = false;
            chkCobrosOtros.Checked = false;
            pnlTerminal.Visible = false;
            pnlEncomienda.Visible = false;

            llenarGrid(0);
        }

        #endregion

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);
                Session["idRegistroPUEBLO"] = dgvDatos.Rows[a].Cells[1].Text;

                if (sAccion == "Editar")
                {
                    cmbProvincia.SelectedValue = dgvDatos.Rows[a].Cells[4].Text;
                    txtDescripcion.Text = dgvDatos.Rows[a].Cells[5].Text.Trim();

                    //INSTRUCCIONES PARA TERMINALES
                    if (dgvDatos.Rows[a].Cells[7].Text == "SI")
                    {
                        chkAplicaTerminal.Checked = true;
                        pnlTerminal.Visible = true;
                        cmbLocalidadTerminal.SelectedValue = dgvDatos.Rows[a].Cells[2].Text;
                    }

                    else
                    {
                        chkAplicaTerminal.Checked = false;
                        pnlTerminal.Visible = false;
                        cmbLocalidadTerminal.SelectedValue = "0";
                    }

                    //INSTRUCCIONES PARA TERMINALES
                    if (dgvDatos.Rows[a].Cells[8].Text == "SI")
                    {
                        chkAplicaEncomienda.Checked = true;
                        pnlEncomienda.Visible = true;
                        cmbLocalidadEncomienda.SelectedValue = dgvDatos.Rows[a].Cells[3].Text;
                    }

                    else
                    {
                        chkAplicaEncomienda.Checked = false;
                        pnlEncomienda.Visible = false;
                        cmbLocalidadEncomienda.SelectedValue = "0";
                    }

                    //INSTRUCCIONES PARA COBROS ADMINISTRACION
                    if (dgvDatos.Rows[a].Cells[9].Text == "1")
                    {
                        chkCobrosAdministracion.Checked = true;                        
                    }

                    else
                    {
                        chkCobrosAdministracion.Checked = false;
                    }

                    //INSTRUCCIONES PARA COBROS OTROS
                    if (dgvDatos.Rows[a].Cells[10].Text == "1")
                    {
                        chkCobrosOtros.Checked = true;
                    }

                    else
                    {
                        chkCobrosOtros.Checked = false;
                    }                    
                }

                columnasGrid(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            limpiar();
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
            if (Convert.ToInt32(cmbProvincia.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbProvincia.Focus();
                return;
            }

            if (txtDescripcion.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtDescripcion.Focus();
                return;
            }

            iIdLocalidadTerminal = 0;
            iIdLocalidadEncomienda = 0;
            iIdProveedorTasa = 0;

            if (chkAplicaTerminal.Checked == true)
            {
                if (Convert.ToInt32(cmbLocalidadTerminal.SelectedValue) == 0)
                {
                    MsjValidarCampos.Visible = true;
                    cmbLocalidadTerminal.Focus();
                    return;
                }

                else
                {
                    iIdLocalidadTerminal = Convert.ToInt32(cmbLocalidadTerminal.SelectedValue);
                }
            }

            if (chkAplicaEncomienda.Checked == true)
            {
                if (Convert.ToInt32(cmbLocalidadEncomienda.SelectedValue) == 0)
                {
                    MsjValidarCampos.Visible = true;
                    cmbLocalidadEncomienda.Focus();
                    return;
                }

                else
                {
                    iIdLocalidadEncomienda = Convert.ToInt32(cmbLocalidadEncomienda.SelectedValue);
                }
            }

            if (chkAplicaTerminal.Checked == true)
            {
                iEsTerminal = 1;
            }

            else
            {
                iEsTerminal = 0;
            }

            if (chkAplicaEncomienda.Checked == true)
            {
                iEsEncomienda = 1;
            }

            else
            {
                iEsEncomienda = 0;
            }

            if (chkCobrosAdministracion.Checked == true)
            {
                iCobrosAdministracion = 1;
            }

            else
            {
                iCobrosAdministracion = 0;
            }

            if (chkCobrosOtros.Checked == true)
            {
                iCobrosOtros = 1;
            }

            else
            {
                iCobrosOtros = 0;
            }

            if (Session["idRegistroPUEBLO"] == null)
            {
                //ENVIO A FUNCION DE INSERCION
                insertarRegistro();
            }

            else
            {
                actualizarRegistro();
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            eliminarRegistro();
        }

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvDatos.PageIndex = e.NewPageIndex;

                if (txtFiltrar.Text.Trim() == "")
                {
                    llenarGrid(0);
                }

                else
                {
                    llenarGrid(1);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void chkAplicaTerminal_OnCheckedChanged(object sender, EventArgs e)
        {
            if (chkAplicaTerminal.Checked == true)
            {
                pnlTerminal.Visible = true;
                cmbLocalidadTerminal.Focus();
            }

            else
            {
                pnlTerminal.Visible = false;
                cmbLocalidadTerminal.SelectedValue = "0";
            }
        }

        protected void chkAplicaEncomienda_OnCheckedChanged(object sender, EventArgs e)
        {
            if (chkAplicaEncomienda.Checked == true)
            {
                pnlEncomienda.Visible = true;
                cmbLocalidadEncomienda.Focus();
            }

            else
            {
                pnlEncomienda.Visible = false;
                cmbLocalidadEncomienda.SelectedValue = "0";
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