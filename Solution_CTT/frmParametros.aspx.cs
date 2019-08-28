using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NEGOCIO;
using ENTIDADES;

namespace Solution_CTT
{
    public partial class frmParametros : System.Web.UI.Page
    {
        ENTPasajeros personaE = new ENTPasajeros();
        ENTComboDatos comboE = new ENTComboDatos();

        manejadorConexion conexionM = new manejadorConexion();        
        manejadorPasajeros personaM = new manejadorPasajeros();
        manejadorComboDatos comboM = new manejadorComboDatos();

        string sSql;
        string sAccionFiltro;
        string []sDatosMaximo = new string[5];

        bool bRespuesta;

        int iVersionDemo;
        int iVistaPrevia;
        int iFacturacionElectronica;
        int iManejaNotaVenta;
        int iBaseClientes;
        int iRegistroCivil;

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

            Session["modulo"] = "MÓDULO DE PARAMETRIZACIÓN DEL SISTEMA";

            if (!IsPostBack)
            {
                llenarComboMoneda();
                cargarParametros();
            }
        }

        #region FUNCIONES DEL MODAL DE FILTRO DE ITEMS

        private void columnasGridItems(bool ok)
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
                    columnasGridItems(true);
                    dgvFiltrarItems.DataSource = dtConsulta;
                    dgvFiltrarItems.DataBind();
                    columnasGridItems(false);
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCIONES DEL MODAL DE FILTRO DE PERSONAS

        private void columnasGridFiltro(bool ok)
        {
            dgvFiltrarPersonas.Columns[0].Visible = ok;
            dgvFiltrarPersonas.Columns[1].ItemStyle.Width = 150;
            dgvFiltrarPersonas.Columns[2].ItemStyle.Width = 300;
            dgvFiltrarPersonas.Columns[3].ItemStyle.Width = 150;
            dgvFiltrarPersonas.Columns[4].ItemStyle.Width = 100;
        }

        //FUNCION PARA LLENAR EL GRID DE CLIENTES
        private void llenarGridPersonas(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select id_persona, identificacion," + Environment.NewLine;
                sSql += "ltrim(isnull(nombres, '') + ' ' + apellidos) personas," + Environment.NewLine;
                sSql += "isnull(fecha_nacimiento, GETDATE()) fecha_nacimiento" + Environment.NewLine;
                sSql += "from tp_personas" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and identificacion like '%" + txtFiltrarPersonas.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or nombres like '%" + txtFiltrarPersonas.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or apellidos like '%" + txtFiltrarPersonas.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by id_persona";

                columnasGridFiltro(true);
                personaE.ISQL = sSql;
                dgvFiltrarPersonas.DataSource = personaM.listarPasajeros(personaE);
                dgvFiltrarPersonas.DataBind();
                columnasGridFiltro(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMBOBX DE MONEDAS
        private void llenarComboMoneda()
        {
            try
            {
                sSql = "";
                sSql += "select * from tp_vw_moneda";

                comboE.ISSQL = sSql;
                cmbMoneda.DataSource = comboM.listarCombo(comboE);
                cmbMoneda.DataValueField = "IID";
                cmbMoneda.DataTextField = "IDATO";
                cmbMoneda.DataBind();
                cmbMoneda.Items.Insert(0, new ListItem("Seleccione Moneda", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CONSULTAR EL REGISTRO
        private void cargarParametros()
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_parametros";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["idParametro"] = dtConsulta.Rows[0][0].ToString();
                        txtPorcentajeIva.Text = dtConsulta.Rows[0][1].ToString();
                        txtPorcentajeIce.Text = dtConsulta.Rows[0][2].ToString();
                        txtNumeroDecimales.Text = dtConsulta.Rows[0][4].ToString();

                        //CONSULTA FACTURACION ELECTRONICA
                        if (dtConsulta.Rows[0][3].ToString() == "1")
                        {
                            chkFacturacionElectronica.Checked = true;
                        }

                        else
                        {
                            chkFacturacionElectronica.Checked = false;
                        }

                        //CONSULTA VISTA PREVIA DE IMPRESIONES
                        if (dtConsulta.Rows[0][5].ToString() == "1")
                        {
                            chkVistaPrevia.Checked = true;
                        }

                        else
                        {
                            chkVistaPrevia.Checked = false;
                        }

                        //CONSULTA VERSION DEMO
                        if (dtConsulta.Rows[0][9].ToString() == "1")
                        {
                            chkVersionDemo.Checked = true;
                        }

                        else
                        {
                            chkVersionDemo.Checked = false;
                        }

                        //CONSULTA MANEJA NOTA DE VENTA
                        if (dtConsulta.Rows[0][5].ToString() == "1")
                        {
                            chkNotaVenta.Checked = true;
                        }

                        else
                        {
                            chkNotaVenta.Checked = false;
                        }

                        Session["idPersonaSinDatos"] = dtConsulta.Rows[0][7].ToString();
                        txtPersonaSinDatos.Text = dtConsulta.Rows[0][13].ToString();
                        Session["idPersonaMenorEdad"] = dtConsulta.Rows[0][8].ToString();
                        txtPersonaMenorEdad.Text = dtConsulta.Rows[0][14].ToString();
                        Session["idPersonaConsumidorFinal"] = dtConsulta.Rows[0][11].ToString();
                        txtPersonaConsumidorFinal.Text = dtConsulta.Rows[0][15].ToString();

                        cmbMoneda.SelectedValue = dtConsulta.Rows[0][10].ToString();
                        cmbTipoComprobante.SelectedValue = dtConsulta.Rows[0][12].ToString();
                        Session["idProducto"] = dtConsulta.Rows[0][17].ToString();
                        txtProductoExtra.Text = dtConsulta.Rows[0][18].ToString();
                        txtCiudad.Text = dtConsulta.Rows[0][20].ToString();
                        txtCorreoElectronico.Text = dtConsulta.Rows[0][21].ToString();
                        txtTelefono.Text = dtConsulta.Rows[0][22].ToString();

                        //CONSULTAR A BASE DE DATOS CLIENTES
                        if (dtConsulta.Rows[0][23].ToString() == "1")
                        {
                            chkBaseClientes.Checked = true;
                        }

                        else
                        {
                            chkBaseClientes.Checked = false;
                        }

                        //CONSULTA AL REGISTRO CIVIL
                        if (dtConsulta.Rows[0][24].ToString() == "1")
                        {
                            chkRegistroCivil.Checked = true;
                        }

                        else
                        {
                            chkRegistroCivil.Checked = false;
                        }

                        pnlRegistro.Enabled = false;
                        MsjValidarCampos.Visible = false;
                        btnGuardar.Text = "Editar";
                    }

                    else
                    {
                        Session["idParametro"] = null;
                        txtPorcentajeIva.Text = "";
                        txtPorcentajeIce.Text = "";
                        txtNumeroDecimales.Text = "";
                        txtProductoExtra.Text = "";
                        txtCiudad.Text = "";
                        txtCorreoElectronico.Text = "";
                        txtTelefono.Text = "";
                        chkVersionDemo.Checked = false;
                        chkFacturacionElectronica.Checked = false;
                        chkVistaPrevia.Checked = false;
                        pnlRegistro.Enabled = true;
                        btnGuardar.Text = "Guardar";
                        MsjValidarCampos.Visible = false;
                        txtPorcentajeIva.Focus();
                    }
                }

                else
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                }
            }

            catch(Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA CARGAR LOS PARÁMETROS GENERALES
        private void cargarParametrosGenerales()
        {
            try
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    //Application["iva"] = dtConsulta.Rows[0][0].ToString();
                    //Application["ice"] = dtConsulta.Rows[0][1].ToString();
                    //Application["facturacion_electronica"] = dtConsulta.Rows[0][2].ToString();
                    //Application["configuracion_decimales"] = dtConsulta.Rows[0][3].ToString();
                    //Application["maneja_nota_venta"] = dtConsulta.Rows[0][4].ToString();
                    //Application["vista_previa_impresion"] = dtConsulta.Rows[0][5].ToString();
                    //Application["idPersonaSinDatos"] = dtConsulta.Rows[0][7].ToString();
                    //Application["idPersonaMenorEdad"] = dtConsulta.Rows[0][8].ToString();
                    //Application["demo"] = dtConsulta.Rows[0][9].ToString();
                    //Application["cgMoneda"] = dtConsulta.Rows[0][10].ToString();
                    //Application["consumidor_final"] = dtConsulta.Rows[0][11].ToString();
                    //Application["numero_consumidor_final"] = dtConsulta.Rows[0][16].ToString();
                    //Application["id_comprobante"] = dtConsulta.Rows[0][12].ToString();
                    //Application["id_producto_extra"] = dtConsulta.Rows[0][17].ToString();
                    //Application["nombre_producto_extra"] = dtConsulta.Rows[0][18].ToString();
                    //Application["precio_producto_extra"] = dtConsulta.Rows[0][19].ToString();
                    //Application["ciudad_default"] = dtConsulta.Rows[0][20].ToString();
                    //Application["telefono_default"] = dtConsulta.Rows[0][21].ToString();
                    //Application["correo_default"] = dtConsulta.Rows[0][22].ToString();
                    //Application["base_clientes"] = dtConsulta.Rows[0][23].ToString();
                    //Application["registro_civil"] = dtConsulta.Rows[0][24].ToString();

                    Application["iva"] = dtConsulta.Rows[0][1].ToString();
                    Application["ice"] = dtConsulta.Rows[0][2].ToString();
                    Application["facturacion_electronica"] = dtConsulta.Rows[0][3].ToString();
                    Application["configuracion_decimales"] = dtConsulta.Rows[0][4].ToString();
                    Application["maneja_nota_venta"] = dtConsulta.Rows[0][5].ToString();
                    Application["vista_previa_impresion"] = dtConsulta.Rows[0][6].ToString();
                    Application["idPersonaSinDatos"] = dtConsulta.Rows[0][7].ToString();
                    Application["idPersonaMenorEdad"] = dtConsulta.Rows[0][8].ToString();
                    Application["demo"] = dtConsulta.Rows[0][9].ToString();
                    Application["cgMoneda"] = dtConsulta.Rows[0][10].ToString();
                    Application["consumidor_final"] = dtConsulta.Rows[0][11].ToString();
                    Application["id_comprobante"] = dtConsulta.Rows[0][12].ToString();
                    Application["numero_consumidor_final"] = dtConsulta.Rows[0][16].ToString();
                    Application["id_producto_extra"] = dtConsulta.Rows[0][17].ToString();
                    Application["nombre_producto_extra"] = dtConsulta.Rows[0][18].ToString();
                    Application["precio_producto_extra"] = dtConsulta.Rows[0][19].ToString();
                    Application["ciudad_default"] = dtConsulta.Rows[0][20].ToString();
                    Application["correo_default"] = dtConsulta.Rows[0][21].ToString();
                    Application["telefono_default"] = dtConsulta.Rows[0][22].ToString();
                    Application["base_clientes"] = dtConsulta.Rows[0][23].ToString();
                    Application["registro_civil"] = dtConsulta.Rows[0][24].ToString();
                    Application["numero_id_sin_datos"] = dtConsulta.Rows[0][25].ToString();
                    Application["numero_id_menor_edad"] = dtConsulta.Rows[0][26].ToString();
                }

                else
                {
                    Application["iva"] = null;
                    Application["ice"] = null;
                    Application["facturacion_electronica"] = null;
                    Application["configuracion_decimales"] = null;
                    Application["maneja_nota_venta"] = null;
                    Application["vista_previa_impresion"] = null;
                    Application["idPersonaSinDatos"] = null;
                    Application["idPersonaMenorEdad"] = null;
                    Application["demo"] = null;
                    Application["cgMoneda"] = null;
                    Application["consumidor_final"] = null;
                    Application["numero_consumidor_final"] = null;
                    Application["id_comprobante"] = null;
                    Application["id_producto_extra"] = null;
                    Application["nombre_producto_extra"] = null;
                    Application["precio_producto_extra"] = null;
                    Application["ciudad_default"] = null;
                    Application["telefono_default"] = null;
                    Application["correo_default"] = null;
                    Application["base_clientes"] = null;
                    Application["registro_civil"] = null;
                    Application["numero_id_sin_datos"] = null;
                    Application["numero_id_menor_edad"] = null;
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'No se encuentra una configuración de parámetros.', 'error')</script>");
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                sSql += "insert into ctt_parametro (" + Environment.NewLine;
                sSql += "iva, ice, maneja_facturacion_electronica, configuracion_decimales," + Environment.NewLine;
                sSql += "maneja_nota_venta, vista_previa_impresion, id_persona_sin_datos," + Environment.NewLine;
                sSql += "id_persona_menor_edad, demo, cg_moneda, consumidor_final, id_comprobante," + Environment.NewLine;
                sSql += "id_producto, ciudad_default, correo_electronico_default, telefono_default," + Environment.NewLine;
                sSql += "maneja_base_clientes, consulta_registro_civil," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToDouble(txtPorcentajeIva.Text.Trim()) + ", " + Convert.ToDouble(txtPorcentajeIce.Text.Trim()) + ", ";
                sSql += iFacturacionElectronica + ", " + Convert.ToInt32(txtNumeroDecimales.Text.Trim()) + ", " + iManejaNotaVenta + "," + Environment.NewLine;
                sSql += iVistaPrevia + ", " + Convert.ToInt32(Session["idPersonaSinDatos"].ToString()) + ", ";
                sSql += Convert.ToInt32(Session["idPersonaMenorEdad"].ToString()) + ", " + iVersionDemo + "," + Environment.NewLine;
                sSql += Convert.ToInt32(cmbMoneda.SelectedValue) + ", " + Convert.ToInt32(Session["idPersonaConsumidorFinal"].ToString()) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(cmbTipoComprobante.SelectedValue) + ", " + Convert.ToInt32(Session["idProducto"].ToString()) + "," + Environment.NewLine;
                sSql += "'" + txtCiudad.Text.Trim().ToUpper() + "', '"+ txtCorreoElectronico.Text.Trim().ToLower() + "'," + Environment.NewLine;
                sSql += "'" + txtTelefono.Text.Trim() + "', " + iBaseClientes + ", " + iRegistroCivil + "," + Environment.NewLine;
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();                
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro ingresado correctamente', 'success');", true);
                cargarParametros();
                cargarParametrosGenerales();                
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                sSql += "update ctt_parametro set" + Environment.NewLine;
                sSql += "iva = " + Convert.ToInt32(txtPorcentajeIva.Text.Trim()) + "," + Environment.NewLine;
                sSql += "ice = " + Convert.ToInt32(txtPorcentajeIce.Text.Trim()) + "," + Environment.NewLine;
                sSql += "maneja_facturacion_electronica = " + iFacturacionElectronica + "," + Environment.NewLine;
                sSql += "configuracion_decimales = " + Convert.ToDouble(txtNumeroDecimales.Text.Trim()) + "," + Environment.NewLine;
                sSql += "maneja_nota_venta = " + iManejaNotaVenta + "," + Environment.NewLine;
                sSql += "vista_previa_impresion = " + iVistaPrevia + "," + Environment.NewLine;
                sSql += "id_persona_sin_datos = " + Convert.ToInt32(Session["idPersonaSinDatos"].ToString()) + "," + Environment.NewLine;
                sSql += "id_persona_menor_edad = " + Convert.ToInt32(Session["idPersonaMenorEdad"].ToString()) + "," + Environment.NewLine;
                sSql += "demo = " + iVersionDemo + "," + Environment.NewLine;
                sSql += "cg_moneda = " + Convert.ToInt32(cmbMoneda.SelectedValue) + "," + Environment.NewLine;
                sSql += "consumidor_final = " + Convert.ToInt32(Session["idPersonaConsumidorFinal"].ToString()) + "," + Environment.NewLine;
                sSql += "id_comprobante = " + Convert.ToInt32(cmbTipoComprobante.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_producto = " + Convert.ToInt32(Session["idProducto"].ToString()) + "," + Environment.NewLine;
                sSql += "ciudad_default = '" + txtCiudad.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "correo_electronico_default = '" + txtCorreoElectronico.Text.Trim().ToLower() + "'," + Environment.NewLine;
                sSql += "telefono_default = '" + txtTelefono.Text.Trim() + "'," + Environment.NewLine;
                sSql += "maneja_base_clientes = " + iBaseClientes + "," + Environment.NewLine;
                sSql += "consulta_registro_civil = " + iRegistroCivil + Environment.NewLine;
                sSql += "where id_ctt_parametro = " + Convert.ToInt32(Session["idParametro"]) + Environment.NewLine;
                sSql += "and estado = 'A'";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro actualizado correctamente', 'success');", true);
                cargarParametros();
                cargarParametrosGenerales();  
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                sSql += "update ctt_parametro set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_ctt_parametro = " + Convert.ToInt32(Session["idParametro"]);

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro eliminado correctamente', 'success');", true);
                cargarParametrosGenerales();
                cargarParametros();
                goto fin;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

        reversa: { conexionM.reversaTransaccion(); };

        fin: { };
        }

        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnGuardar.Text == "Editar")
                {
                    pnlRegistro.Enabled = true;
                    txtPorcentajeIva.Focus();
                    btnGuardar.Text = "Guardar";
                }

                else
                {
                    if (txtPorcentajeIva.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtPorcentajeIva.Focus();
                    }

                    else if (txtPorcentajeIce.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtPorcentajeIce.Focus();
                    }

                    else if (Convert.ToInt32(cmbTipoComprobante.SelectedValue) == 0)
                    {
                        MsjValidarCampos.Visible = true;
                        cmbTipoComprobante.Focus();
                    }

                    else if (txtNumeroDecimales.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtNumeroDecimales.Focus();
                    }

                    else if (Session["idPersonaSinDatos"] == null)
                    {
                        MsjValidarCampos.Visible = true;
                        btnBuscarPersonaSinDatos.Focus();
                    }

                    else if (Convert.ToInt32(Session["idPersonaSinDatos"].ToString()) == 0)
                    {
                        MsjValidarCampos.Visible = true;
                        btnBuscarPersonaSinDatos.Focus();
                    }

                    else if (Session["idPersonaMenorEdad"] == null)
                    {
                        MsjValidarCampos.Visible = true;
                        btnBuscarPersonaMenorEdad.Focus();
                    }

                    else if (Convert.ToInt32(Session["idPersonaMenorEdad"].ToString()) == 0)
                    {
                        MsjValidarCampos.Visible = true;
                        btnBuscarPersonaMenorEdad.Focus();
                    }

                    else if (Session["idPersonaConsumidorFinal"] == null)
                    {
                        MsjValidarCampos.Visible = true;
                        btnPersonaConsumidorFinal.Focus();
                    }

                    else if (Convert.ToInt32(Session["idPersonaConsumidorFinal"].ToString()) == 0)
                    {
                        MsjValidarCampos.Visible = true;
                        btnPersonaConsumidorFinal.Focus();
                    }

                    else if (Session["idProducto"] == null)
                    {
                        MsjValidarCampos.Visible = true;
                        btnBuscarPersonaSinDatos.Focus();
                    }

                    else if (Convert.ToInt32(Session["idProducto"].ToString()) == 0)
                    {
                        MsjValidarCampos.Visible = true;
                        btnBuscarPersonaSinDatos.Focus();
                    }

                    else if (Convert.ToInt32(cmbMoneda.SelectedValue) == 0)
                    {
                        MsjValidarCampos.Visible = true;
                        cmbMoneda.Focus();
                    }

                    else if (txtCiudad.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtCiudad.Focus();
                    }

                    else if (txtTelefono.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtTelefono.Focus();
                    }

                    else if (txtCorreoElectronico.Text.Trim() == "")
                    {
                        MsjValidarCampos.Visible = true;
                        txtCorreoElectronico.Focus();
                    }

                    else
                    {
                        //VISTA PREVIA
                        if (chkVistaPrevia.Checked == true)
                        {
                            iVistaPrevia = 1;
                        }

                        else
                        {
                            iVistaPrevia = 0;
                        }

                        //NOTA DE VENTA
                        if (chkNotaVenta.Checked == true)
                        {
                            iManejaNotaVenta = 1;
                        }

                        else
                        {
                            iManejaNotaVenta = 0;
                        }

                        //FACTURACION ELECTRONICA
                        if (chkFacturacionElectronica.Checked == true)
                        {
                            iFacturacionElectronica = 1;
                        }

                        else
                        {
                            iFacturacionElectronica = 0;
                        }

                        //VERSION DEMO
                        if (chkVersionDemo.Checked == true)
                        {
                            iVersionDemo = 1;
                        }

                        else
                        {
                            iVersionDemo = 0;
                        }

                        //BASE CLIENTES
                        if (chkBaseClientes.Checked == true)
                        {
                            iBaseClientes = 1;
                        }

                        else
                        {
                            iBaseClientes = 0;
                        }

                        //REGISTRO CIVIL
                        if (chkRegistroCivil.Checked == true)
                        {
                            iRegistroCivil = 1;
                        }

                        else
                        {
                            iRegistroCivil = 0;
                        }

                        if (Session["idParametro"] == null)
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
            }

            catch(Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            cargarParametros();
        }

        protected void btnBuscarPersonaSinDatos_Click(object sender, EventArgs e)
        {
            Session["registroPersonas"] = "1";
            btnPopUp_ModalPopupExtender.Show();
            llenarGridPersonas(0);
        }

        protected void btnBuscarPersonaMenorEdad_Click(object sender, EventArgs e)
        {
            Session["registroPersonas"] = "2";
            btnPopUp_ModalPopupExtender.Show();
            llenarGridPersonas(0);
        }

        protected void btnPersonaConsumidorFinal_Click(object sender, EventArgs e)
        {
            Session["registroPersonas"] = "3";
            btnPopUp_ModalPopupExtender.Show();
            llenarGridPersonas(0);
        }


        //EVENTOS DE LA PANTALLA MODAL

        protected void btnCerrarModal_Click(object sender, EventArgs e)
        {
            btnPopUp_ModalPopupExtender.Hide();
        }

        protected void btnFiltrarPersonas_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFiltrarPersonas.Text.Trim() == "")
                {
                    llenarGridPersonas(0);
                }

                else
                {
                    llenarGridPersonas(1);
                }
            }

            catch (Exception ex)
            {
                btnPopUp_ModalPopupExtender.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnAbrirModalPersonas_Click(object sender, EventArgs e)
        {
            btnPopUp_ModalPopupExtender.Show();
            llenarGridPersonas(0);
        }

        protected void btnFiltarChoferAsistente_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFiltrarPersonas.Text.Trim() == "")
                {
                    llenarGridPersonas(0);
                }

                else
                {
                    llenarGridPersonas(1);
                }
            }

            catch (Exception ex)
            {
                btnPopUp_ModalPopupExtender.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnSeleccion_Click(object sender, EventArgs e)
        {
            sAccionFiltro = "Seleccion";
        }

        protected void dgvFiltrarPersonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvFiltrarPersonas.SelectedIndex;
                columnasGridFiltro(true);

                if (sAccionFiltro == "Seleccion")
                {
                    if (Session["registroPersonas"].ToString() == "1")
                    {
                        Session["idPersonaSinDatos"] = dgvFiltrarPersonas.Rows[a].Cells[0].Text.Trim();
                        txtPersonaSinDatos.Text = dgvFiltrarPersonas.Rows[a].Cells[2].Text.Trim();
                    }

                    else if (Session["registroPersonas"].ToString() == "2")
                    {
                        Session["idPersonaMenorEdad"] = dgvFiltrarPersonas.Rows[a].Cells[0].Text.Trim();
                        txtPersonaMenorEdad.Text = dgvFiltrarPersonas.Rows[a].Cells[2].Text.Trim();
                    }

                    else if (Session["registroPersonas"].ToString() == "3")
                    {
                        Session["idPersonaConsumidorFinal"] = dgvFiltrarPersonas.Rows[a].Cells[0].Text.Trim();                        
                        txtPersonaConsumidorFinal.Text = dgvFiltrarPersonas.Rows[a].Cells[2].Text.Trim();
                    }

                    txtFiltrarPersonas.Text = "";
                    btnPopUp_ModalPopupExtender.Hide();
                    Session["registroPersonas"] = null;
                }

                columnasGridFiltro(false);
            }

            catch (Exception ex)
            {
                btnPopUp_ModalPopupExtender.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvFiltrarPersonas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvFiltrarPersonas.PageIndex = e.NewPageIndex;

                if (txtFiltrarPersonas.Text.Trim() == "")
                {
                    llenarGridPersonas(0);
                }

                else
                {
                    llenarGridPersonas(1);
                }
            }

            catch (Exception ex)
            {
                btnPopUp_ModalPopupExtender.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FIN PANTALLA DE MODALS


        //MODAL DE PRODUCTOS

        protected void btnProductoExtra_Click(object sender, EventArgs e)
        {
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void dgvFiltrarItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvFiltrarItems.SelectedIndex;
                columnasGridItems(true);

                if (sAccionFiltro == "Seleccion")
                {
                    Session["idProducto"] = dgvFiltrarItems.Rows[a].Cells[0].Text;
                    txtProductoExtra.Text = dgvFiltrarItems.Rows[a].Cells[2].Text;
                }

                columnasGridItems(false);
                modalExtenderItems.Hide();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnSeleccionItem_Click(object sender, EventArgs e)
        {
            sAccionFiltro = "Seleccion";
        }

        protected void btnCerrarModalItems_Click(object sender, EventArgs e)
        {
            modalExtenderItems.Hide();
        }

        //FIN DE MODAL
    }
}