using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NEGOCIO;
using ENTIDADES;
using System.Data;
using System.Drawing.Printing;
using System.Net;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmImpresoras : System.Web.UI.Page
    {
        manejadorImpresiones impresionM = new manejadorImpresiones();
        manejadorComboDatos comboM = new manejadorComboDatos();
        ENTComboDatos comboE = new ENTComboDatos();
        ENTImpresoras impresionE = new ENTImpresoras();

        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sPathImpresora;
        string sAccion;

        string[] sDatosMaximo = new string[5];

        int iCortarPapel;
        int iAbrirCajon;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
                return;
            }

            Session["modulo"] = "MÓDULO DE IMPRESORAS";

            sDatosMaximo[0] = Session["usuario"].ToString();
            sDatosMaximo[1] = Environment.MachineName.ToString();
            sDatosMaximo[2] = "A";

            if (!IsPostBack)
            {
                llenarComboPueblos();
                llenarGrid(0);
                llenarComboImpresoras();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMOBOX DE PUEBLOS
        private void llenarComboPueblos()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_pueblo, descripcion" + Environment.NewLine;
                sSql += "from ctt_pueblos";

                comboE.ISSQL = sSql;
                cmbPueblos.DataSource = comboM.listarCombo(comboE);
                cmbPueblos.DataValueField = "IID";
                cmbPueblos.DataTextField = "IDATO";
                cmbPueblos.DataBind();
                cmbPueblos.Items.Insert(0, new ListItem("Seleccione Oficina...!!!", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE TERMINALES
        private void llenarComboLocalidades(int iIdPueblo_P)
        {
            try
            {
                sSql = "";
                sSql += "select id_localidad_terminal id_localidad, descripcion + ' - TERMINAL' oficina" + Environment.NewLine;
                sSql += "from ctt_pueblos" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + iIdPueblo_P + Environment.NewLine;
                sSql += "and id_localidad_terminal <> 0" + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "union" + Environment.NewLine;
                sSql += "select id_localidad_encomienda id_localidad, descripcion + ' - ENCOMIENDA' oficina" + Environment.NewLine;
                sSql += "from ctt_pueblos" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + iIdPueblo_P + Environment.NewLine;
                sSql += "and id_localidad_encomienda <> 0" + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "and estado = 'A'";

                comboE.ISSQL = sSql;
                cmbLocalidad.DataSource = comboM.listarCombo(comboE);
                cmbLocalidad.DataValueField = "IID";
                cmbLocalidad.DataTextField = "IDATO";
                cmbLocalidad.DataBind();
                cmbLocalidad.Items.Insert(0, new ListItem("Seleccione Servicio...!!!", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }


        //LLENAR COMBOBOX DE IMPRESORAS
        private void llenarComboImpresoras()
        {
            try
            {
                String pkInstalledPrinters;
                for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                {
                    pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                    cmbImpresoras.Items.Add(pkInstalledPrinters);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA INSERTAR
        private void insertarRegistro()
        {
            try
            {
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sPathImpresora = @"\\" + txtIpLocal.Text.Trim() + @"\" + cmbImpresoras.SelectedItem.ToString();

                sSql = "";
                sSql += "insert into ctt_impresora (" + Environment.NewLine;
                sSql += "descripcion, path_url, id_ctt_pueblo, id_localidad, numero_impresion," + Environment.NewLine;
                sSql += "cortar_papel, abrir_cajon, ip_local, estado, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += "'" + cmbImpresoras.SelectedItem.ToString() + "', '" + sPathImpresora + "', " + Convert.ToInt32(cmbPueblos.SelectedValue) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(cmbLocalidad.SelectedValue) + ", " + Convert.ToInt32(txtCantidad.Text.Trim()) + ", ";
                sSql += iCortarPapel + ", " + iAbrirCajon + ", '" + txtIpLocal.Text.Trim() + "', 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUTAR LA INSTRUCCIÓN SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //COMMIT DE LA TRANSACCIÓN
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

            reversa: { conexionM.reversaTransaccion(); }

            fin: { }
        }


        //FUNCION PARA ACTUALIZAR EN LA BASE DE DATOS
        private void actualizarRegistro()
        {
            try
            {
                //SE INICIA UNA TRANSACCION
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sPathImpresora = @"\\" + txtIpLocal.Text.Trim() + @"\" + cmbImpresoras.SelectedItem.ToString();

                sSql = "";
                sSql += "update ctt_impresora set" + Environment.NewLine;
                sSql += "id_ctt_pueblo = " + Convert.ToInt32(cmbPueblos.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_localidad = " + Convert.ToInt32(cmbLocalidad.SelectedValue) + "," + Environment.NewLine;
                sSql += "descripcion = '" + cmbImpresoras.SelectedItem.ToString() + "'," + Environment.NewLine;
                sSql += "path_url = '" + sPathImpresora + "'," + Environment.NewLine;
                sSql += "numero_impresion = " + Convert.ToInt32(txtCantidad.Text.Trim()) + "," + Environment.NewLine;
                sSql += "cortar_papel = " + iCortarPapel + "," + Environment.NewLine;
                sSql += "abrir_cajon = " + iAbrirCajon + "," + Environment.NewLine;
                sSql += "ip_local = '" + txtIpLocal.Text.Trim() + "'" + Environment.NewLine;
                sSql += "where id_ctt_impresora = " + Convert.ToInt32(Session["idRegistro"]) + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUTAR LA INSTRUCCIÓN SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //COMMIT DE LA TRANSACCIÓN
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

            reversa: { conexionM.reversaTransaccion(); }

            fin: { }
        }

        //FUNCION PARA ELIMINAR EN LA BASE DE DATOS
        private void eliminarRegistro()
        {
            try
            {
                //SE INICIA UNA TRANSACCION
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sSql = "";
                sSql += "update ctt_impresora set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_ctt_impresora = " + Convert.ToInt32(Session["idRegistro"]);

                //EJECUTAR LA INSTRUCCIÓN SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto fin;
                }

                //COMMIT DE LA TRANSACCIÓN
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

            reversa: { conexionM.reversaTransaccion(); }

            fin: { }
        }


        //FUNCION PARA LAS COLUMNAS
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[1].ItemStyle.Width = 200;
            dgvDatos.Columns[8].ItemStyle.Width = 100;
            
            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[3].Visible = ok;
            dgvDatos.Columns[4].Visible = ok;
            dgvDatos.Columns[5].Visible = ok;
            dgvDatos.Columns[6].Visible = ok;
            dgvDatos.Columns[7].Visible = ok;
            dgvDatos.Columns[9].Visible = ok;
        }

        //FUNCION PARA LLENAR EL DATAGRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select I.id_ctt_impresora, I.descripcion, I.path_url," + Environment.NewLine;
                sSql += "I.cortar_papel, I.abrir_cajon, I.numero_impresion," + Environment.NewLine;
                sSql += "I.id_localidad, I.id_ctt_pueblo, P.descripcion, isnull(I.ip_local, '') ip_local" + Environment.NewLine;
                sSql += "from ctt_impresora I, ctt_pueblos P" + Environment.NewLine;
                sSql += "where I.id_ctt_pueblo = P.id_ctt_pueblo" + Environment.NewLine;
                sSql += "and I.estado = 'A'" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and I.descripcion like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by I.id_ctt_impresora";

                columnasGrid(true);
                impresionE.ISQL = sSql;
                dgvDatos.DataSource = impresionM.listarImpresoras(impresionE);
                dgvDatos.DataBind();
                columnasGrid(false);          
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtCantidad.Text = "";
            Session["idRegistro"] = null;
            cmbLocalidad.SelectedIndex = 0;
            cmbImpresoras.SelectedIndex = 0;
            chkAbrirCajon.Checked = false;
            chkCortarPapel.Checked = false;
            btnSave.Text = "Crear";
            MsjValidarCampos.Visible = false;
            pnlImpresoras.Visible = false;
            txtIpLocal.Text = "";

            llenarComboPueblos();
            llenarGrid(0);
            cmbPueblos.Focus();
        }
        #endregion

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);
                Session["idRegistro"] = dgvDatos.Rows[a].Cells[0].Text;                

                if (sAccion == "Editar")
                {
                    pnlImpresoras.Visible = true;
                    //cmbImpresoras.Text = dgvDatos.Rows[a].Cells[1].Text;
                    cmbPueblos.SelectedValue = dgvDatos.Rows[a].Cells[7].Text;
                    llenarComboLocalidades(Convert.ToInt32(cmbPueblos.SelectedValue));

                    if (Convert.ToInt32(dgvDatos.Rows[a].Cells[3].Text) == 1)
                    {
                        chkCortarPapel.Checked = true;
                    }

                    else
                    {
                        chkCortarPapel.Checked = false;
                    }

                    if (Convert.ToInt32(dgvDatos.Rows[a].Cells[4].Text) == 1)
                    {
                        chkAbrirCajon.Checked = true;
                    }

                    else
                    {
                        chkAbrirCajon.Checked = false;
                    }

                    txtCantidad.Text = dgvDatos.Rows[a].Cells[5].Text;
                    cmbLocalidad.SelectedValue = dgvDatos.Rows[a].Cells[6].Text;
                    txtIpLocal.Text = dgvDatos.Rows[a].Cells[9].Text;
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
            if (Convert.ToInt32(Session["privilegio"].ToString()) == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Información.!', 'No tiene permisos para realizar esta acción.', 'warning');", true);
            }

            else
            {
                sAccion = "Eliminar";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
            }
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
            if (Convert.ToInt32(cmbPueblos.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbPueblos.Focus();
            }

            else if (Convert.ToInt32(cmbLocalidad.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbLocalidad.Focus();
            }

            else if (txtCantidad.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtCantidad.Focus();
            }

            else if (txtIpLocal.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtIpLocal.Focus();
            }

            else
            {
                //sPathImpresora = @"\\" + Environment.MachineName.ToString() + @"\" + cmbImpresoras.Text.Trim();

                if (chkCortarPapel.Checked == true)
                {
                    iCortarPapel = 1;
                }

                else
                {
                    iCortarPapel = 0;
                }

                if (chkAbrirCajon.Checked == true)
                {
                    iAbrirCajon = 1;
                }

                else
                {
                    iAbrirCajon = 0;
                }

                if (Session["idRegistro"] == null)
                {
                    //ENVIO A FUNCION DE INSERCION
                    insertarRegistro();
                }

                else
                {
                    actualizarRegistro();
                }
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

        protected void cmbPueblos_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbPueblos.SelectedValue) == 0)
            {
                cmbLocalidad.SelectedValue = "0";
                cmbImpresoras.SelectedIndex = 0;
                pnlImpresoras.Visible = false;
                txtCantidad.Text = "";
                txtIpLocal.Text = "";
                chkCortarPapel.Checked = false;
                chkAbrirCajon.Checked = false;
            }

            else
            {
                llenarComboLocalidades(Convert.ToInt32(cmbPueblos.SelectedValue));
                cmbImpresoras.SelectedIndex = 0;
                pnlImpresoras.Visible = true;
                txtCantidad.Text = "";
                txtIpLocal.Text = "";
                chkCortarPapel.Checked = false;
                chkAbrirCajon.Checked = false;
            }
        }

        protected void btnSeleccionarIP_Click(object sender, EventArgs e)
        {
            //IPHostEntry host;

            //host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (IPAddress ip in host.AddressList)
            //{
            //    if (ip.AddressFamily.ToString() == "InterNetwork")
            //    {
            //        txtIpLocal.Text = ip.ToString();
            //    }
            //}

            string host = Dns.GetHostName();
            IPAddress[] ip = Dns.GetHostAddresses(host);
            txtIpLocal.Text = ip[ip.Length - 1].ToString();
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