using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ENTIDADES;
using NEGOCIO;

namespace Solution_CTT
{
    public partial class frmParametrosLocalidad : System.Web.UI.Page
    {
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorConexion conexionM = new manejadorConexion();
        manejadorPasajeros personaM = new manejadorPasajeros();
        manejadorParametrosLocalidad parametroM = new manejadorParametrosLocalidad();
        ENTPasajeros personaE = new ENTPasajeros();
        ENTComboDatos comboE = new ENTComboDatos();
        ENTParametrosLocalidad parametroE = new ENTParametrosLocalidad();

        string sSql;
        string sAccion;
        string sAccionFiltro;
        string []sDatosMaximo = new string[5];

        DataTable dtConsulta;

        bool bRespuesta;

        int iConsultarRegistro;
        int iTasaUsuario;
        int iEjecutaCobroAdministrativo;
        int iIdProveedorTasa;
        
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

            Session["modulo"] = "MóDULO DE PARAMETRIZACIóN POR LOCALIDAD";

            if (!IsPostBack)
            {
                llenarComboTerminales();
                llenarComboCiudad();
                llenarComboVendedores();
                llenarComboProveedoresTasas();
                llenarGrid(0);
            }
        }

        #region FUNCIONES DEL MODAL DE FILTRO DE ITEMS

        private void columnasGridFiltro(bool ok)
        {
            dgvFiltrarItems.Columns[0].Visible = ok;
        }

        //FUNCION PARA LLENAR EL GRID DE ITEMS
        private void llenarGridItems(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_items_productos_retenciones" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "where nombre like '%" + txtFiltrarItems.Text.Trim() + "%'";
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    columnasGridFiltro(true);
                    dgvFiltrarItems.DataSource = dtConsulta;
                    dgvFiltrarItems.DataBind();
                    columnasGridFiltro(false);
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

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMBOBX DE TERMINALES
        private void llenarComboTerminales()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_pueblo, descripcion" + Environment.NewLine;
                sSql += "from ctt_pueblos" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and terminal = 1" + Environment.NewLine;
                sSql += "order by id_ctt_pueblo";

                comboE.ISSQL = sSql;
                cmbTerminales.DataSource = comboM.listarCombo(comboE);
                cmbTerminales.DataValueField = "IID";
                cmbTerminales.DataTextField = "IDATO";
                cmbTerminales.DataBind();
                cmbTerminales.Items.Insert(0, new ListItem("Seleccione Terminal", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMBOBX DE CIUDADES
        private void llenarComboCiudad()
        {
            try
            {
                sSql = "";
                sSql += "select * from tp_vw_ciudad";

                comboE.ISSQL = sSql;
                cmbCiudad.DataSource = comboM.listarCombo(comboE);
                cmbCiudad.DataValueField = "IID";
                cmbCiudad.DataTextField = "IDATO";
                cmbCiudad.DataBind();
                cmbCiudad.Items.Insert(0, new ListItem("Seleccione Ciudad", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMBOBX DE VENDEDORES
        private void llenarComboVendedores()
        {
            try
            {
                sSql = "";
                sSql += "select id_vendedor, descripcion" + Environment.NewLine;
                sSql += "from cv403_vendedores" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbVendedor.DataSource = comboM.listarCombo(comboE);
                cmbVendedor.DataValueField = "IID";
                cmbVendedor.DataTextField = "IDATO";
                cmbVendedor.DataBind();
                cmbVendedor.Items.Insert(0, new ListItem("Seleccione Vendedor", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE PROVEEDORES DE TASAS
        private void llenarComboProveedoresTasas()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_proveedor_tasa, descripcion" + Environment.NewLine;
                sSql += "from ctt_proveedores_tasas" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "order by descripcion";

                comboE.ISSQL = sSql;
                cmbProveedoresTasas.DataSource = comboM.listarCombo(comboE);
                cmbProveedoresTasas.DataValueField = "IID";
                cmbProveedoresTasas.DataTextField = "IDATO";
                cmbProveedoresTasas.DataBind();
                cmbProveedoresTasas.Items.Insert(0, new ListItem("Seleccione Proveedor", "0"));

            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LAS COLUMNAS
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[2].Visible = ok;
            dgvDatos.Columns[3].Visible = ok;
            dgvDatos.Columns[4].Visible = ok;
            dgvDatos.Columns[8].Visible = ok;
            dgvDatos.Columns[9].Visible = ok;
            dgvDatos.Columns[10].Visible = ok;
            dgvDatos.Columns[11].Visible = ok;
            dgvDatos.Columns[12].Visible = ok;
            dgvDatos.Columns[13].Visible = ok;
            dgvDatos.Columns[14].Visible = ok;
            dgvDatos.Columns[15].Visible = ok;
            dgvDatos.Columns[16].Visible = ok;

            dgvDatos.Columns[0].ItemStyle.Width = 75;
            dgvDatos.Columns[5].ItemStyle.Width = 200;
            dgvDatos.Columns[6].ItemStyle.Width = 150;
            dgvDatos.Columns[7].ItemStyle.Width = 150;
            dgvDatos.Columns[8].ItemStyle.Width = 100;
            dgvDatos.Columns[9].ItemStyle.Width = 100;
        }


        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_parametros_localidad" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += " where descripcion like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by id_ctt_parametro_localidad" + Environment.NewLine;

                columnasGrid(true);
                parametroE.ISQL = sSql;
                dgvDatos.DataSource = parametroM.listarParametrosLocalidad(parametroE);
                dgvDatos.DataBind();
                columnasGrid(false);

            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CARGAR LOS PARÁMETROS POR TERMINAL
        private void cargarParametrosTerminal()
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_parametros_localidad" + Environment.NewLine;
                sSql += "where id_localidad_terminal = " + Convert.ToInt32(Application["idLocalidad"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        //Application["id_pueblo"] = dtConsulta.Rows[0][1].ToString();
                        //Application["cgCiudad"] = dtConsulta.Rows[0][2].ToString();
                        //Application["idVendedor"] = dtConsulta.Rows[0][3].ToString();
                        //Application["pago_administracion"] = dtConsulta.Rows[0][5].ToString();
                        //Application["porcentaje_retencion"] = dtConsulta.Rows[0][6].ToString();
                        //Application["id_producto_retencion"] = dtConsulta.Rows[0][7].ToString();
                        //Application["id_producto_pagos"] = dtConsulta.Rows[0][8].ToString();
                        //Application["paga_iva_retencion"] = dtConsulta.Rows[0][12].ToString();
                        //Application["paga_iva_pagos"] = dtConsulta.Rows[0][13].ToString();

                        Session["id_pueblo"] = dtConsulta.Rows[0][1].ToString();
                        Session["cgCiudad"] = dtConsulta.Rows[0][2].ToString();
                        Session["idVendedor"] = dtConsulta.Rows[0][3].ToString();
                        Session["pago_administracion"] = dtConsulta.Rows[0][5].ToString();
                        Session["porcentaje_retencion"] = dtConsulta.Rows[0][6].ToString();
                        Session["id_producto_retencion"] = dtConsulta.Rows[0][7].ToString();
                        Session["id_producto_pagos"] = dtConsulta.Rows[0][8].ToString();
                        Session["paga_iva_retencion"] = dtConsulta.Rows[0][12].ToString();
                        Session["paga_iva_pagos"] = dtConsulta.Rows[0][13].ToString();
                        Session["genera_tasa_usuario"] = dtConsulta.Rows[0][14].ToString();
                        Session["cantidad_manifiesto"] = dtConsulta.Rows[0][15].ToString();
                        Session["ejecuta_cobro_administrativo"] = dtConsulta.Rows[0][18].ToString();
                        Session["tasaDevesofft"] = null;

                        if (dtConsulta.Rows[0]["codigo_proveedor"].ToString().Trim() == "01")
                        {
                            Session["tasaDevesofft"] = dtConsulta.Rows[0]["codigo_proveedor"].ToString();
                        }
                    }

                    else
                    {
                        //Application["id_pueblo"] = null;
                        //Application["cgCiudad"] = null;
                        //Application["idVendedor"] = null;
                        //Application["pago_administracion"] = null;a
                        //Application["porcentaje_retencion"] = null;
                        //Application["id_producto_retencion"] = null;
                        //Application["id_producto_pagos"] = null;
                        //Application["paga_iva_retencion"] = null;
                        //Application["paga_iva_pagos"] = null;

                        Session["id_pueblo"] = null;
                        Session["cgCiudad"] = null;
                        Session["idVendedor"] = null;
                        Session["pago_administracion"] = null;
                        Session["porcentaje_retencion"] = null;
                        Session["id_producto_retencion"] = null;
                        Session["id_producto_pagos"] = null;
                        Session["paga_iva_retencion"] = null;
                        Session["paga_iva_pagos"] = null;
                        Session["genera_tasa_usuario"] = null;
                        Session["cantidad_manifiesto"] = null;
                        Session["ejecuta_cobro_administrativo"] = null;
                        Session["tasaDevesofft"] = null;

                        ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'No se encuentra una configuración de parámetros del terminal.', 'error')</script>");
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

        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private void insertarRegistro()
        {
            try
            {
                iConsultarRegistro = consultarRegistro();

                if (iConsultarRegistro > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'Ya existe un registro con el codigo ingresado.', 'warning');", true);
                    goto fin;
                }

                else if (iConsultarRegistro == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ocurrió un problema al consultar el código para el registro.', 'danger');", true);
                    goto fin;
                }

                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                sSql = "";
                sSql += "insert into ctt_parametro_localidad (" + Environment.NewLine;
                sSql += "id_ctt_pueblo, id_producto_retencion, id_producto_pagos, cg_ciudad," + Environment.NewLine;
                sSql += "id_vendedor, genera_tasa_usuario, cantidad_manifiesto, ejecuta_cobro_administrativo," + Environment.NewLine;
                sSql += "id_ctt_proveedor_tasa, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(cmbTerminales.SelectedValue) + ", " + Convert.ToInt32(Session["idProductoRetencion"].ToString()) + ", ";
                sSql += Convert.ToInt32(Session["idProductoPago"].ToString()) + ", " + Convert.ToInt32(cmbCiudad.SelectedValue) + ", " + Environment.NewLine;
                sSql += Convert.ToInt32(cmbVendedor.SelectedValue) + ", " + iTasaUsuario + ", " + Convert.ToInt32(txtCantidadManifiesto.Text.Trim()) + "," + Environment.NewLine;
                sSql += + iEjecutaCobroAdministrativo + ", " + iIdProveedorTasa + ", 'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUCIÓN DE INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro ingresado correctamente', 'success');", true);

                //if (Convert.ToInt32(cmbTerminales.SelectedValue) == Convert.ToInt32(Application["id_pueblo"].ToString()))
                //{
                //    cargarParametrosTerminal();
                //}

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


        //FUNCION PARA ACTUALIZAR EL REGISTRO
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
                sSql += "update ctt_parametro_localidad set" + Environment.NewLine;
                sSql += "id_ctt_pueblo = " + Convert.ToInt32(cmbTerminales.SelectedValue) + "," + Environment.NewLine;
                sSql += "cg_ciudad = " + Convert.ToInt32(cmbCiudad.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_vendedor = " + Convert.ToInt32(cmbVendedor.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_producto_retencion = " + Convert.ToInt32(Session["idProductoRetencion"].ToString()) + "," + Environment.NewLine;
                sSql += "genera_tasa_usuario = " + iTasaUsuario + "," + Environment.NewLine;
                sSql += "id_producto_pagos = " + Convert.ToInt32(Session["idProductoPago"].ToString()) + "," + Environment.NewLine;
                sSql += "cantidad_manifiesto = " + Convert.ToInt32(txtCantidadManifiesto.Text.Trim()) + "," + Environment.NewLine;
                sSql += "ejecuta_cobro_administrativo = " + iEjecutaCobroAdministrativo + "," + Environment.NewLine;
                sSql += "id_ctt_proveedor_tasa = " + iIdProveedorTasa + Environment.NewLine;
                sSql += "where id_ctt_parametro_localidad = " + Convert.ToInt32(Session["idRegistroPLOCALIDAD"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCIÓN DE INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro actualizado correctamente', 'success');", true);

                //if (Convert.ToInt32(cmbTerminales.SelectedValue) == Convert.ToInt32(Application["id_pueblo"].ToString()))
                //{
                //    cargarParametrosTerminal();
                //}

                if (Convert.ToInt32(cmbTerminales.SelectedValue) == Convert.ToInt32(Session["id_pueblo"].ToString()))
                {
                    cargarParametrosTerminal();
                }

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


        //FUNCION PARA ELIMINAR EL REGISTRO
        private void eliminarRegistro()
        {
            try
            {
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                    goto reversa;
                }

                sSql = "";
                sSql += "update ctt_parametro_localidad set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_ctt_parametro_localidad = " + Convert.ToInt32(Session["idRegistroPLOCALIDAD"].ToString());

                //EJECUCIÓN DE INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro eliminado correctamente', 'success');", true);

                if (Convert.ToInt32(cmbTerminales.SelectedValue) == Convert.ToInt32(Application["id_pueblo"].ToString()))
                {
                    cargarParametrosTerminal();
                }

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

        //FUNCION PARA CONSULTAR EL REGISTRO
        private int consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from ctt_parametro_localidad" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(cmbTerminales.SelectedValue) + Environment.NewLine;
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
            llenarComboTerminales();
            llenarComboCiudad();
            llenarComboVendedores();
            llenarComboProveedoresTasas();
            llenarGrid(0);

            Session["idRegistroPLOCALIDAD"] = null;
            Session["id_Persona"] = null;
            Session["idProductoRetencion"] = null;
            Session["idProductoPago"] = null;
            btnSave.Text = "Crear";
            MsjValidarCampos.Visible = false;
            txtPago.Text = "";
            txtPorcentaje.Text = "";
            txtModalPago.Text = "";
            txtModalRetencion.Text = "";
            chkManejaTasaUsuario.Checked = false;
            chkEjecutaCobrosAdministrativos.Checked = false;
            cmbProveedoresTasas.Enabled = false;
        }

        #endregion

        //PARA EL GRID DE INFORMACION PRINCIPAL
        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);
                Session["idRegistroPLOCALIDAD"] = dgvDatos.Rows[a].Cells[1].Text.Trim();

                if (sAccion == "Editar")
                {
                    cmbTerminales.SelectedValue = dgvDatos.Rows[a].Cells[2].Text.Trim();
                    cmbCiudad.SelectedValue = dgvDatos.Rows[a].Cells[3].Text.Trim();
                    cmbVendedor.SelectedValue = dgvDatos.Rows[a].Cells[4].Text.Trim();                    
                    txtPago.Text = dgvDatos.Rows[a].Cells[6].Text.Trim();
                    txtPorcentaje.Text = dgvDatos.Rows[a].Cells[7].Text.Trim();
                    Session["idProductoRetencion"] = dgvDatos.Rows[a].Cells[8].Text.Trim();
                    Session["idProductoPago"] = dgvDatos.Rows[a].Cells[9].Text.Trim();
                    txtModalRetencion.Text = dgvDatos.Rows[a].Cells[10].Text.Trim();
                    txtModalPago.Text = dgvDatos.Rows[a].Cells[11].Text.Trim();
                    txtCantidadManifiesto.Text = dgvDatos.Rows[a].Cells[13].Text.Trim();

                    if (dgvDatos.Rows[a].Cells[12].Text.Trim() == "1")
                    {
                        chkManejaTasaUsuario.Checked = true;
                        cmbProveedoresTasas.SelectedValue = dgvDatos.Rows[a].Cells[15].Text.Trim();
                        cmbProveedoresTasas.Enabled = true;
                    }

                    else
                    {
                        chkManejaTasaUsuario.Checked = false;
                        cmbProveedoresTasas.SelectedValue = "0";
                        cmbProveedoresTasas.Enabled = false;
                    }

                    if (dgvDatos.Rows[a].Cells[14].Text.Trim() == "1")
                    {
                        chkEjecutaCobrosAdministrativos.Checked = true;
                    }

                    else
                    {
                        chkEjecutaCobrosAdministrativos.Checked = false;
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

        protected void lbtnSeleccion_Click(object sender, EventArgs e)
        {
            sAccionFiltro = "Seleccion";
        }

        protected void lbtnEditarPersona_Click(object sender, EventArgs e)
        {
            sAccionFiltro = "Editar";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
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
            if (Convert.ToInt32(cmbTerminales.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbTerminales.Focus();
            }

            else if (Convert.ToInt32(cmbCiudad.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbCiudad.Focus();
            }

            else if (Convert.ToInt32(cmbVendedor.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbVendedor.Focus();
            }

            else if (txtPago.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtPago.Focus();
            }

            else if (txtPorcentaje.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtPorcentaje.Focus();
            }

            else if (txtCantidadManifiesto.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtPorcentaje.Focus();
            }

            else
            {
                if (chkManejaTasaUsuario.Checked == true)
                {
                    iTasaUsuario = 1;
                }

                else
                {
                    iTasaUsuario = 0;
                }

                if (chkEjecutaCobrosAdministrativos.Checked == true)
                {
                    iEjecutaCobroAdministrativo = 1;
                }

                else
                {
                    iEjecutaCobroAdministrativo = 0;
                }

                iIdProveedorTasa = Convert.ToInt32(cmbProveedoresTasas.SelectedValue);

                if (Session["idRegistroPLOCALIDAD"] == null)
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

        //FUNCIONES DEL MODAL
        protected void btnAbrirModalPago_Click(object sender, EventArgs e)
        {
            Session["opcion"] = "1";
            modalExtenderItems.Show();
            txtFiltrarItems.Text = "";
            llenarGridItems(0);
        }

        protected void btnAbrirModalRetencion_Click(object sender, EventArgs e)
        {
            Session["opcion"] = "2";
            modalExtenderItems.Show();
            txtFiltrarItems.Text = "";
            llenarGridItems(0);
        }

        protected void btnFiltrarItems_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFiltrarItems.Text.Trim() == "")
                {
                    llenarGridItems(0);
                }

                else
                {
                    llenarGridItems(1);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvFiltrarItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvFiltrarItems.PageIndex = e.NewPageIndex;

                if (txtFiltrarItems.Text.Trim() == "")
                {
                    llenarGridItems(0);
                }

                else
                {
                    llenarGridItems(1);
                }
            }

            catch (Exception ex)
            {
                modalExtenderItems.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvFiltrarItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvFiltrarItems.SelectedIndex;
                columnasGridFiltro(true);

                if (sAccionFiltro == "Seleccion")
                {
                    if (Convert.ToInt32(Session["opcion"].ToString()) == 1)
                    {
                        Session["idProductoPago"] = dgvFiltrarItems.Rows[a].Cells[0].Text;
                        txtModalPago.Text = dgvFiltrarItems.Rows[a].Cells[2].Text;
                        txtPago.Text = dgvFiltrarItems.Rows[a].Cells[4].Text;
                    }

                    else if (Convert.ToInt32(Session["opcion"].ToString()) == 2)
                    {
                        Session["idProductoRetencion"] = dgvFiltrarItems.Rows[a].Cells[0].Text;
                        txtModalRetencion.Text = dgvFiltrarItems.Rows[a].Cells[2].Text;
                        txtPorcentaje.Text = dgvFiltrarItems.Rows[a].Cells[3].Text;
                    }
                }

                Session["opcion"] = null;
                columnasGridFiltro(false);
                modalExtenderItems.Hide();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnSeleccionItem_Click(object sender, EventArgs e)
        {
            sAccionFiltro = "Seleccion";
        }

        protected void btnCerrarModal_Click(object sender, EventArgs e)
        {
            Session["opcion"] = null;
            modalExtenderItems.Hide();
        }

        protected void chkManejaTasaUsuario_OnCheckedChanged(object sender, EventArgs e)
        {
            if (chkManejaTasaUsuario.Checked == true)
            {
                cmbProveedoresTasas.Enabled = true;
            }

            else
            {
                cmbProveedoresTasas.Enabled = false;
                cmbProveedoresTasas.SelectedValue = "0";
            }
        }
    }
}