using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ENTIDADES;
using NEGOCIO;
using System.Data;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmPropietario : System.Web.UI.Page
    {
        ENTVehiculoPropietario propietarioE = new ENTVehiculoPropietario();
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorVehiculoPropietario propietarioM = new manejadorVehiculoPropietario();
        manejadorConexion conexionM = new manejadorConexion();
        ENTPasajeros personaE = new ENTPasajeros();
        manejadorPasajeros personaM = new manejadorPasajeros();
        ENTVehiculo vehiculoE = new ENTVehiculo();
        manejadorVehiculo vehiculoM = new manejadorVehiculo();

        string sSql;
        string sAccion;
        string sAccionFiltroPersona;
        string sAccionFiltroVehiculo;

        string []sDatosMaximo = new string[5];

        DataTable dtConsulta;
        bool bRespuesta;

        int iConsultarRegistro;

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

            Session["modulo"] = "MÓDULO DE PROPIETARIOS DE LOS VEHÍCULOS";

            if (!IsPostBack)
            {
                llenarGrid(0);
            }
        }

        #region FUNCIONES DEL MODAL DE FILTRO DE VEHICULOS


        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGridVehiculos()
        {
            try
            {
                sSql = "";
                sSql += "select V.id_ctt_vehiculo, V.id_ctt_tipo_vehiculo, TV.descripcion tipo_vehiculo," + Environment.NewLine;
                sSql += "V.id_ctt_marca_vehiculo, MV.descripcion marca_vehiculo, V.id_ctt_modelo_vehiculo," + Environment.NewLine;
                sSql += "MOV.descripcion modelo_vehiculo, V.id_ctt_tipo_asiento, TA.descripcion tipo_bus," + Environment.NewLine;
                sSql += "V.id_ctt_disco, D.descripcion disco, V.placa, V.chasis, V.motor, V.anio_produccion," + Environment.NewLine;
                sSql += "V.pais_origen, V.cilindraje, V.peso, V.numero_pasajeros," + Environment.NewLine;
                //sSql += "case V.estado when 'A' then 'ACTIVO' else 'INACTIVO' end estado, FA.id_ctt_formato_asiento," + Environment.NewLine;
                sSql += "case V.is_active when 1 then 'ACTIVO' else 'INACTIVO' end estado, FA.id_ctt_formato_asiento," + Environment.NewLine;
                sSql += "V.is_active" + Environment.NewLine;
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
                sSql += "where V.is_active = 1" + Environment.NewLine;

                if (txtFiltrarVehiculos.Text.Trim() != "")
                {
                    sSql += "and (D.descripcion like '%" + txtFiltrarVehiculos.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or V.placa like '%" + txtFiltrarVehiculos.Text.Trim() + "%')" + Environment.NewLine;
                }

                sSql += "order by D.descripcion" + Environment.NewLine;

                columnasGridVehiculo(true);
                vehiculoE.ISSQL = sSql;
                dgvFiltrarVehiculos.DataSource = vehiculoM.listarVehiculos(vehiculoE);
                dgvFiltrarVehiculos.DataBind();
                columnasGridVehiculo(false);

            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LAS COLUMNAS
        private void columnasGridVehiculo(bool ok)
        {
            dgvFiltrarVehiculos.Columns[2].ItemStyle.Width = 150;
            dgvFiltrarVehiculos.Columns[4].ItemStyle.Width = 150;
            dgvFiltrarVehiculos.Columns[6].ItemStyle.Width = 150;
            dgvFiltrarVehiculos.Columns[8].ItemStyle.Width = 150;
            dgvFiltrarVehiculos.Columns[10].ItemStyle.Width = 100;
            dgvFiltrarVehiculos.Columns[11].ItemStyle.Width = 150;
            dgvFiltrarVehiculos.Columns[19].ItemStyle.Width = 100;

            dgvFiltrarVehiculos.Columns[0].Visible = ok;
            dgvFiltrarVehiculos.Columns[1].Visible = ok;
            dgvFiltrarVehiculos.Columns[3].Visible = ok;
            dgvFiltrarVehiculos.Columns[5].Visible = ok;
            dgvFiltrarVehiculos.Columns[7].Visible = ok;
            dgvFiltrarVehiculos.Columns[9].Visible = ok;
            dgvFiltrarVehiculos.Columns[12].Visible = ok;
            dgvFiltrarVehiculos.Columns[13].Visible = ok;
            dgvFiltrarVehiculos.Columns[14].Visible = ok;
            dgvFiltrarVehiculos.Columns[15].Visible = ok;
            dgvFiltrarVehiculos.Columns[16].Visible = ok;
            dgvFiltrarVehiculos.Columns[17].Visible = ok;
            dgvFiltrarVehiculos.Columns[18].Visible = ok;
            dgvFiltrarVehiculos.Columns[20].Visible = ok;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion

        #region FUNCION DEL USUARIO

        //FUNCION PARA LAS COLUMNAS
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[3].ItemStyle.Width = 100;
            dgvDatos.Columns[4].ItemStyle.Width = 250;
            dgvDatos.Columns[5].ItemStyle.Width = 200;
            dgvDatos.Columns[6].ItemStyle.Width = 250;
            dgvDatos.Columns[7].ItemStyle.Width = 100;
            dgvDatos.Columns[8].ItemStyle.Width = 100;
            dgvDatos.Columns[9].ItemStyle.Width = 100;

            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[2].Visible = ok;
            dgvDatos.Columns[3].Visible = ok;
            dgvDatos.Columns[4].Visible = ok;
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select VP.id_ctt_vehiculo_propietario, VP.id_persona, VP.id_ctt_vehiculo," + Environment.NewLine;
                sSql += "VP.codigo,ltrim(TP.nombres + ' ' + TP.apellidos) propietario," + Environment.NewLine;
                sSql += "D.descripcion + ' ' + V.placa vehiculo, VP.descripcion," + Environment.NewLine;
                sSql += "case VP.estado when 'a' then 'ACTIVO' else 'ELIMINADO' end estado" + Environment.NewLine;
                sSql += "from tp_personas TP INNER JOIN" + Environment.NewLine;
                sSql += "ctt_vehiculo_propietario VP ON VP.id_persona = TP.id_persona" + Environment.NewLine;
                sSql += "and VP.estado = 'A'" + Environment.NewLine;
                sSql += "and TP.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_vehiculo V ON VP.id_ctt_vehiculo = V.id_ctt_vehiculo" + Environment.NewLine;
                sSql += "and V.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_tipo_vehiculo TV ON V.id_ctt_tipo_vehiculo = TV.id_ctt_tipo_vehiculo" + Environment.NewLine;
                sSql += "and TV.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_disco D ON V.id_ctt_disco = D.id_ctt_disco" + Environment.NewLine;
                sSql += "and D.estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "where VP.codigo like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or VP.descripcion like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by D.descripcion" + Environment.NewLine;

                columnasGrid(true);
                propietarioE.ISSQL = sSql;
                dgvDatos.DataSource = propietarioM.listarVehiculoPropietario(propietarioE);
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
                sSql += "insert into ctt_vehiculo_propietario (" + Environment.NewLine;
                sSql += "id_persona, id_ctt_vehiculo, codigo, descripcion, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Session["id_Persona"].ToString()) + "," + Convert.ToInt32(Session["id_Vehiculo"].ToString()) + ", '" + txtCodigo.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "'" + txtDescripcion.Text.Trim().ToUpper() + "', 'A', GETDATE(), '" + sDatosMaximo[0] + "'," + Environment.NewLine;
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
                sSql += "update ctt_vehiculo_propietario set" + Environment.NewLine;
                sSql += "id_persona = " + Convert.ToInt32(Session["id_Persona"].ToString()) + "," + Environment.NewLine;
                sSql += "id_ctt_vehiculo = " + Convert.ToInt32(Session["id_Vehiculo"].ToString()) + "," + Environment.NewLine;
                sSql += "codigo = '" + txtCodigo.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "descripcion = '" + txtDescripcion.Text.Trim().ToUpper() + "'" + Environment.NewLine;
                sSql += "where id_ctt_vehiculo_propietario = " + Convert.ToInt32(Session["idRegistro"]) + Environment.NewLine;
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
                sSql += "update ctt_vehiculo_propietario set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_ctt_vehiculo_propietario = " + Convert.ToInt32(Session["idRegistro"]);

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

        //FUNCION PARA CONSULTAR SI EXISTE EL REGISTRO EN LA BASE DE DATOS
        private int consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from ctt_vehiculo_propietario" + Environment.NewLine;
                sSql += "where id_ctt_vehiculo = " + Convert.ToInt32(Session["id_Vehiculo"].ToString()) + Environment.NewLine;
                sSql += "and codigo = '" + txtCodigo.Text.Trim() + "'" + Environment.NewLine;
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
            txtDescripcion.Text = "";
            TxtPersona.Text = "";
            txtVehiculo.Text = "";
            Session["idRegistro"] = null;
            Session["id_Persona"] = null;
            Session["id_Vehiculo"] = null;
            txtCodigo.ReadOnly = false;            
            txtCodigo.Focus();
            btnSave.Text = "Crear";
            llenarGrid(0);
        }

        #endregion

        //PARA EL GRID DE INFORMACION PRINCIPAL
        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvDatos.SelectedIndex;
                columnasGrid(true);
                Session["idRegistro"] = dgvDatos.Rows[a].Cells[0].Text.Trim();

                if (sAccion == "Editar")
                {
                    Session["id_Persona"] = dgvDatos.Rows[a].Cells[1].Text.Trim();
                    Session["id_Vehiculo"] = dgvDatos.Rows[a].Cells[2].Text.Trim();
                    txtCodigo.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[3].Text.Trim());
                    TxtPersona.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[4].Text.Trim());
                    txtVehiculo.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[5].Text.Trim());
                    txtDescripcion.Text = HttpUtility.HtmlDecode(dgvDatos.Rows[a].Cells[6].Text.Trim());
                    txtCodigo.ReadOnly = true;
                }

                columnasGrid(false);
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //PARA FILTRAR LOS DATOS DEL CLIENTE
        protected void dgvFiltrarPersonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvFiltrarPersonas.SelectedIndex;
                columnasGridFiltro(true);

                if (sAccionFiltroPersona == "Seleccion")
                {
                    Session["id_Persona"] = dgvFiltrarPersonas.Rows[a].Cells[0].Text.Trim();
                    TxtPersona.Text = HttpUtility.HtmlDecode(dgvFiltrarPersonas.Rows[a].Cells[2].Text.Trim());
                    txtFiltrarPersonas.Text = "";
                    btnPopUp_ModalPopupExtender.Hide();
                }

                columnasGridFiltro(false);
            }

            catch (Exception ex)
            {
                btnPopUp_ModalPopupExtender.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnSeleccion_Click(object sender, EventArgs e)
        {
            sAccionFiltroPersona = "Seleccion";
        }

        protected void lbtnEditarPersona_Click(object sender, EventArgs e)
        {
            sAccionFiltroPersona = "Editar";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
        }

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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnAbrirModalPersonas_Click(object sender, EventArgs e)
        {
            btnPopUp_ModalPopupExtender.Show();
            llenarGridPersonas(0);
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
            if (txtCodigo.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtCodigo.Focus();
            }

            else if (txtDescripcion.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtDescripcion.Focus();
            }

            else if (txtVehiculo.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtVehiculo.Focus();
            }

            else if (TxtPersona.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                btnAbrirModalPersonas.Focus();
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

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            eliminarRegistro();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void btnAbrirModalVehiculos_Click(object sender, EventArgs e)
        {
            btnPopUp_ModalPopupExtender2.Show();
            llenarGridVehiculos();
        }

        protected void btnCerrarModal2_Click(object sender, EventArgs e)
        {
            btnPopUp_ModalPopupExtender2.Hide();
        }

        protected void dgvFiltrarVehiculos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvFiltrarVehiculos.PageIndex = e.NewPageIndex;
                llenarGridVehiculos();
            }

            catch (Exception ex)
            {
                btnPopUp_ModalPopupExtender2.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }


        protected void btnFiltrarVehiculos_Click(object sender, EventArgs e)
        {
            try
            {
                llenarGridVehiculos();
            }

            catch (Exception ex)
            {
                btnPopUp_ModalPopupExtender2.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }


        //PARA FILTRAR LOS DATOS DE VEHICULOS
        protected void dgvFiltrarVehiculos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int a = dgvFiltrarVehiculos.SelectedIndex;
                columnasGridVehiculo(true);

                if (sAccionFiltroVehiculo == "Seleccion")
                {
                    Session["id_Vehiculo"] = dgvFiltrarVehiculos.Rows[a].Cells[0].Text.Trim();
                    txtVehiculo.Text = dgvFiltrarVehiculos.Rows[a].Cells[10].Text.Trim() + " - " + dgvFiltrarVehiculos.Rows[a].Cells[11].Text.Trim();
                    txtFiltrarVehiculos.Text = "";
                    btnPopUp_ModalPopupExtender2.Hide();
                }

                columnasGridVehiculo(false);
            }

            catch (Exception ex)
            {
                btnPopUp_ModalPopupExtender2.Hide();
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void lbtnSeleccionVehiculo_Click(object sender, EventArgs e)
        {
            sAccionFiltroVehiculo = "Seleccion";
        }

        protected void lbtnEditarVehiculo_Click(object sender, EventArgs e)
        {
            sAccionFiltroVehiculo = "Editar";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#QuestionModal').modal('show');</script>", false);
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

        protected void dgvFiltrarPersonas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < dgvFiltrarPersonas.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dgvFiltrarPersonas.Rows[i].BackColor = Color.FromName("#ccf0cb");
                }

                else
                {
                    dgvFiltrarPersonas.Rows[i].BackColor = Color.White;
                }
            }
        }
    }
}