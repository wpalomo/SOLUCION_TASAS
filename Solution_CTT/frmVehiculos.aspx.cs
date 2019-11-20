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

namespace Solution_CTT
{
    public partial class frmVehiculos : System.Web.UI.Page
    {
        ENTVehiculo vehiculoE = new ENTVehiculo();
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorVehiculo vehiculoM = new manejadorVehiculo();
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sAccion;
        string[] sDatosMaximo = new string[5];

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

            Session["modulo"] = "MÓDULO DE VEHÍCULOS";

            if (!IsPostBack)
            {
                llenarGrid(0);
                llenarComboTipoVehiculo();
                llenarComboMarcas();
                llenarComboModelos();
                llenarComboTipoAsientos();
                llenarComboDiscos();
                llenarComboFormatoAsientos();
            }
        }

        #region FUNCION DEL USUARIO

        //FUNCION PARA LAS COLUMNAS
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[2].ItemStyle.Width = 150;
            dgvDatos.Columns[4].ItemStyle.Width = 150;
            dgvDatos.Columns[6].ItemStyle.Width = 150;
            dgvDatos.Columns[8].ItemStyle.Width = 150;
            dgvDatos.Columns[10].ItemStyle.Width = 100;
            dgvDatos.Columns[11].ItemStyle.Width = 150;
            dgvDatos.Columns[19].ItemStyle.Width = 100;

            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[3].Visible = ok;
            dgvDatos.Columns[5].Visible = ok;
            dgvDatos.Columns[7].Visible = ok;
            dgvDatos.Columns[9].Visible = ok;
            dgvDatos.Columns[12].Visible = ok;
            dgvDatos.Columns[13].Visible = ok;
            dgvDatos.Columns[14].Visible = ok;
            dgvDatos.Columns[15].Visible = ok;
            dgvDatos.Columns[16].Visible = ok;
            dgvDatos.Columns[17].Visible = ok;
            dgvDatos.Columns[18].Visible = ok;
            dgvDatos.Columns[20].Visible = ok;
        }

        //FUNCION PARA LLENAR EL COMOBOX DE FORMATO DE ASIENTOS
        private void llenarComboFormatoAsientos()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_formato_asiento, descripcion" + Environment.NewLine;
                sSql += "from ctt_formato_asientos" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbFormatoAsiento.DataSource = comboM.listarCombo(comboE);
                cmbFormatoAsiento.DataValueField = "IID";
                cmbFormatoAsiento.DataTextField = "IDATO";
                cmbFormatoAsiento.DataBind();
                cmbFormatoAsiento.Items.Insert(0, new ListItem("Seleccione..!!", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE TIPO DE VEHICULO
        private void llenarComboTipoVehiculo()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tipo_vehiculo, descripcion" + Environment.NewLine;
                sSql += "from ctt_tipo_vehiculo" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbTipoVehiculo.DataSource = comboM.listarCombo(comboE);
                cmbTipoVehiculo.DataValueField = "IID";
                cmbTipoVehiculo.DataTextField = "IDATO";
                cmbTipoVehiculo.DataBind();
                cmbTipoVehiculo.Items.Insert(0, new ListItem("Seleccione..!!", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE MARCAS
        private void llenarComboMarcas()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_marca_vehiculo, descripcion" + Environment.NewLine;
                sSql += "from ctt_marca_vehiculo" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbMarca.DataSource = comboM.listarCombo(comboE);
                cmbMarca.DataValueField = "IID";
                cmbMarca.DataTextField = "IDATO";
                cmbMarca.DataBind();
                cmbMarca.Items.Insert(0, new ListItem("Seleccione..!!", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE MODELOS
        private void llenarComboModelos()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_modelo_vehiculo, descripcion" + Environment.NewLine;
                sSql += "from ctt_modelo_vehiculo" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbModelo.DataSource = comboM.listarCombo(comboE);
                cmbModelo.DataValueField = "IID";
                cmbModelo.DataTextField = "IDATO";
                cmbModelo.DataBind();
                cmbModelo.Items.Insert(0, new ListItem("Seleccione..!!", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE TIPO DE ASIENTOS
        private void llenarComboTipoAsientos()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_tipo_asiento, descripcion" + Environment.NewLine;
                sSql += "from ctt_tipo_asiento" + Environment.NewLine;
                sSql += "where estado = 'A'";

                comboE.ISSQL = sSql;
                cmbTipoAsiento.DataSource = comboM.listarCombo(comboE);
                cmbTipoAsiento.DataValueField = "IID";
                cmbTipoAsiento.DataTextField = "IDATO";
                cmbTipoAsiento.DataBind();
                cmbTipoAsiento.Items.Insert(0, new ListItem("Seleccione..!!", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE DISCOS
        private void llenarComboDiscos()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_disco, descripcion" + Environment.NewLine;
                sSql += "from ctt_disco" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "order by descripcion";

                comboE.ISSQL = sSql;
                cmbDisco.DataSource = comboM.listarCombo(comboE);
                cmbDisco.DataValueField = "IID";
                cmbDisco.DataTextField = "IDATO";
                cmbDisco.DataBind();
                cmbDisco.Items.Insert(0, new ListItem("Seleccione..!!", "0"));
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select V.id_ctt_vehiculo, V.id_ctt_tipo_vehiculo, TV.descripcion tipo_vehiculo," + Environment.NewLine;
                sSql += "V.id_ctt_marca_vehiculo, MV.descripcion marca_vehiculo, V.id_ctt_modelo_vehiculo," + Environment.NewLine;
                sSql += "MOV.descripcion modelo_vehiculo, V.id_ctt_tipo_asiento, TA.descripcion tipo_bus," + Environment.NewLine;
                sSql += "V.id_ctt_disco, D.descripcion disco, V.placa, V.chasis, V.motor, V.anio_produccion," + Environment.NewLine;
                sSql += "V.pais_origen, V.cilindraje, V.peso, V.numero_pasajeros," + Environment.NewLine;
                sSql += "case V.estado when 'A' then 'ACTIVO' else 'INACTIVO' end estado, FA.id_ctt_formato_asiento" + Environment.NewLine;
                sSql += "from ctt_tipo_vehiculo TV INNER JOIN" + Environment.NewLine;
                sSql += "ctt_vehiculo V ON V.id_ctt_tipo_vehiculo = TV.id_ctt_tipo_vehiculo" + Environment.NewLine;
                sSql += "and V.estado = 'A'" + Environment.NewLine;
                sSql += "and TV.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_marca_vehiculo MV ON V.id_ctt_marca_vehiculo = MV.id_ctt_marca_vehiculo" + Environment.NewLine;
                sSql += "and MV.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_modelo_vehiculo MOV ON V.id_ctt_modelo_vehiculo = MOV.id_ctt_modelo_vehiculo" + Environment.NewLine;
                sSql += "and MOV.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_tipo_asiento TA ON V.id_ctt_tipo_asiento = TA.id_ctt_tipo_asiento" + Environment.NewLine;
                sSql += "and TA.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_disco D ON V.id_ctt_disco = D.id_ctt_disco" + Environment.NewLine;
                sSql += "and D.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_formato_asientos FA ON V.id_ctt_formato_asiento = FA.id_ctt_formato_asiento" + Environment.NewLine;
                sSql += "and FA.estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "" + Environment.NewLine;
                }

                sSql += "order by D.descripcion" + Environment.NewLine;

                columnasGrid(true);
                vehiculoE.ISSQL = sSql;
                dgvDatos.DataSource = vehiculoM.listarVehiculos(vehiculoE);
                dgvDatos.DataBind();
                columnasGrid(false);

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
                if (consultarRegistro() > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'Ya existe un registro con el codigo ingresado.', 'warning');", true);
                    return;
                }

                else if (consultarRegistro() == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ocurrió un problema al consultar el código para el registro.', 'danger');", true);
                    return;
                }

                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    return;
                }

                sSql = "";
                sSql += "insert into ctt_vehiculo (" + Environment.NewLine;
                sSql += "id_ctt_tipo_vehiculo, id_ctt_marca_vehiculo, id_ctt_modelo_vehiculo," + Environment.NewLine;
                sSql += "id_ctt_tipo_asiento, id_ctt_disco, id_ctt_formato_asiento, placa, chasis, motor," + Environment.NewLine;
                sSql += "anio_produccion, pais_origen, cilindraje, peso, numero_pasajeros, estado, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(cmbTipoVehiculo.SelectedValue) + ", " + Convert.ToInt32(cmbMarca.SelectedValue) + ", ";
                sSql += Convert.ToInt32(cmbModelo.SelectedValue) + ", " + Convert.ToInt32(cmbTipoAsiento.SelectedValue) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(cmbDisco.SelectedValue) + ", " + Convert.ToInt32(cmbFormatoAsiento.SelectedValue) + ", '" + txtPlaca.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'" + txtChasis.Text.Trim().ToUpper() + "', '" + txtMotor.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'" + txtAnioProduccion.Text.Trim().ToUpper() + "', '" + txtPaisOrigen.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'" + txtCilindraje.Text.Trim().ToUpper() + "', " + Convert.ToDecimal(txtPeso.Text.Trim()) + "," + Environment.NewLine;
                sSql += "'" + txtNumeroPasajeros.Text.Trim().ToUpper() + "', 'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "')";

                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro ingresado correctamente', 'success');", true);
                limpiar();
                pnlGrid.Visible = true;
                pnlRegistro.Visible = false;
                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); };
        }

        //FUNCION PARA ACTUALIZAR EN LA BASE DE DATOS
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
                sSql += "update ctt_vehiculo set" + Environment.NewLine;
                sSql += "id_ctt_tipo_vehiculo = " + Convert.ToInt32(cmbTipoVehiculo.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_ctt_marca_vehiculo = " + Convert.ToInt32(cmbMarca.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_ctt_modelo_vehiculo = " + Convert.ToInt32(cmbModelo.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_ctt_tipo_asiento = " + Convert.ToInt32(cmbTipoAsiento.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_ctt_disco = " + Convert.ToInt32(cmbDisco.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_ctt_formato_asiento = " + Convert.ToInt32(cmbFormatoAsiento.SelectedValue) + "," + Environment.NewLine;
                sSql += "placa = '" + txtPlaca.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "chasis = '" + txtChasis.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "motor = '" + txtMotor.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "anio_produccion = '" + txtAnioProduccion.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "pais_origen = '" + txtPaisOrigen.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "cilindraje = '" + txtCilindraje.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "peso = " + Convert.ToDecimal(txtPeso.Text.Trim()) + "," + Environment.NewLine;
                sSql += "numero_pasajeros = '" + txtNumeroPasajeros.Text.Trim().ToUpper() + "'" + Environment.NewLine;
                sSql += "where id_ctt_vehiculo = " + Convert.ToInt32(Session["idRegistro"]) + Environment.NewLine;
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
                pnlGrid.Visible = true;
                pnlRegistro.Visible = false;
                return;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); };
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
                sSql += "update ctt_vehiculo set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_ctt_vehiculo = " + Convert.ToInt32(Session["idRegistro"]);

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                goto reversa;
            }

            reversa: { conexionM.reversaTransaccion(); };

            fin: { };
        }

        //FUNCION PARA CONSULTAR SI EXISTE EL REGISTRO EN LA BASE DE DATOS
        private int consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from ctt_vehiculo" + Environment.NewLine;
                sSql += "where placa = '" + txtPlaca.Text.Trim() + "'" + Environment.NewLine;
                sSql += "and id_ctt_disco = " + Convert.ToInt32(cmbDisco.SelectedValue) + Environment.NewLine;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return -1;
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            cmbTipoVehiculo.SelectedIndex = 0;
            cmbMarca.SelectedIndex = 0;
            cmbModelo.SelectedIndex = 0;
            cmbTipoAsiento.SelectedIndex = 0;
            cmbDisco.SelectedIndex = 0;
            cmbFormatoAsiento.SelectedIndex = 0;
            txtChasis.Text = "";
            txtMotor.Text = "";
            txtAnioProduccion.Text = "";
            txtPaisOrigen.Text = "";
            txtNumeroPasajeros.Text = "";
            txtCilindraje.Text = "";
            txtPeso.Text = "";
            txtPlaca.Text = "";
            Session["idRegistro"] = null;
            //pnlRegistro.Visible = false;
            //pnlGrid.Visible = true;
            llenarGrid(0);
        }

        #endregion

        protected void lbtnNuevo_Click(object sender, EventArgs e)
        {
            pnlRegistro.Visible = true;
            pnlGrid.Visible = false;
            cmbTipoVehiculo.SelectedIndex = 0;
            cmbMarca.SelectedIndex = 0;
            cmbModelo.SelectedIndex = 0;
            cmbTipoAsiento.SelectedIndex = 0;
            cmbDisco.SelectedIndex = 0;
            cmbFormatoAsiento.SelectedIndex = 0;
            Session["idRegistro"] = null;            
            txtChasis.Text = "";
            txtMotor.Text = "";
            txtAnioProduccion.Text = "";
            txtPaisOrigen.Text = "";
            txtNumeroPasajeros.Text = "";
            txtCilindraje.Text = "";
            txtPeso.Text = "";
            txtPlaca.Text = "";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbTipoVehiculo.SelectedValue) == 0)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "mensaje", "alert('Favor seleccione el tipo de vehículo.');", true);
                cmbTipoVehiculo.Focus();
            }

            else if (Convert.ToInt32(cmbMarca.SelectedValue) == 0)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "mensaje", "alert('Favor seleccione la marca de vehículo.');", true);
                cmbMarca.Focus();
            }

            else if (Convert.ToInt32(cmbModelo.SelectedValue) == 0)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "mensaje", "alert('Favor seleccione el modelo de vehículo.');", true);
                cmbModelo.Focus();
            }

            else if (Convert.ToInt32(cmbTipoAsiento.SelectedValue) == 0)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "mensaje", "alert('Favor seleccione el tipo de asientos del vehículo.');", true);
                cmbTipoAsiento.Focus();
            }

            else if (Convert.ToInt32(cmbDisco.SelectedValue) == 0)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "mensaje", "alert('Favor asigne un disco al vehículo.');", true);
                cmbDisco.Focus();
            }

            else if (txtChasis.Text.Trim() == "")
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "mensaje", "alert('Favor ingrese el chasis del vehículo.');", true);
                txtChasis.Focus();
            }

            else if (txtMotor.Text.Trim() == "")
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "mensaje", "alert('Favor ingrese el motor del vehículo.');", true);
                txtMotor.Focus();
            }

            else if (txtAnioProduccion.Text.Trim() == "")
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "mensaje", "alert('Favor ingrese el año de producción del vehículo.');", true);
                txtAnioProduccion.Focus();
            }

            else if (txtPaisOrigen.Text.Trim() == "")
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "mensaje", "alert('Favor ingrese el país del origen del vehículo.');", true);
                txtPaisOrigen.Focus();
            }

            else if (txtNumeroPasajeros.Text.Trim() == "")
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "mensaje", "alert('Favor ingrese la capacidad de pasajeros del vehiculo.');", true);
                txtNumeroPasajeros.Focus();
            }

            else if (txtCilindraje.Text.Trim() == "")
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "mensaje", "alert('Favor ingrese el cilindraje del vehículo.');", true);
                txtCilindraje.Focus();
            }

            else if (txtPlaca.Text.Trim() == "")
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "mensaje", "alert('Favor ingrese la placa del vehículo.');", true);
                txtPlaca.Focus();
            }

            else
            {
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

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                Session["idRegistro"] = dgvDatos.Rows[a].Cells[0].Text;

                if (sAccion == "Editar")
                {
                    cmbTipoVehiculo.SelectedValue = dgvDatos.Rows[a].Cells[1].Text;
                    cmbMarca.SelectedValue = dgvDatos.Rows[a].Cells[3].Text;
                    cmbModelo.SelectedValue = dgvDatos.Rows[a].Cells[5].Text;
                    cmbTipoAsiento.SelectedValue = dgvDatos.Rows[a].Cells[7].Text;
                    cmbDisco.SelectedValue = dgvDatos.Rows[a].Cells[9].Text;
                    cmbFormatoAsiento.SelectedValue = dgvDatos.Rows[a].Cells[20].Text;

                    txtPlaca.Text = dgvDatos.Rows[a].Cells[11].Text;
                    txtChasis.Text = dgvDatos.Rows[a].Cells[12].Text;
                    txtMotor.Text = dgvDatos.Rows[a].Cells[13].Text;
                    txtAnioProduccion.Text = dgvDatos.Rows[a].Cells[14].Text;
                    txtPaisOrigen.Text = dgvDatos.Rows[a].Cells[15].Text;
                    txtCilindraje.Text = dgvDatos.Rows[a].Cells[16].Text;
                    txtPeso.Text = Convert.ToDouble(dgvDatos.Rows[a].Cells[17].Text).ToString("N2");
                    txtNumeroPasajeros.Text = dgvDatos.Rows[a].Cells[18].Text;

                    pnlGrid.Visible = false;
                    pnlRegistro.Visible = true;
                }

                columnasGrid(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            sAccion = "Editar";            
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            sAccion = "Eliminar";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //limpiar();
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            eliminarRegistro();
            //limpiar();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            limpiar();
            pnlGrid.Visible = true;
            pnlRegistro.Visible = false;
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