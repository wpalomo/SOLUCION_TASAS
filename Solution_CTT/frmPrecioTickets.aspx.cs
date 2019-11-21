using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NEGOCIO;
using ENTIDADES;
using System.Drawing;

namespace Solution_CTT
{
    public partial class frmPrecioTickets : System.Web.UI.Page
    {
        ENTPrecioTickets ticketE = new ENTPrecioTickets();
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorPrecioTickets ticketM = new manejadorPrecioTickets();
        manejadorConexion conexionM = new manejadorConexion();

        Clases.ClaseParametros parametros = new Clases.ClaseParametros();

        string sSql;
        string[] sDatosMaximo = new string[5];
        string sAccion;
        string sTabla;
        string sCampo;


        DataTable dtConsulta;
        bool bRespuesta;

        int iPagaIva;
        int iConsultarRegistro;
        int iIdProductoPadre;
        int iIdProducto;
        int iAplicaRetencion;
        int iAplicaPago;

        double dbPorcentajeRetencion;
        double dbPrecio;

        long iMaximo;

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

            Session["modulo"] = "MÓDULO DE PRECIOS DE COBROS ADMINISTRATIVOS";
            Session["buscar"] = "";

            if (!IsPostBack)
            {
                llenarComboOrigen();
                llenarComboUnidades();
                datosListas();
                llenarGrid(0);
                //validarCombos();
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMBO ORIGEN
        private void llenarComboOrigen()
        {
            try
            {
                sSql = "";
                sSql += "select P.id_producto, P.codigo + ' - ' + NP.nombre origen" + Environment.NewLine;
                sSql += "from cv401_productos P, cv401_nombre_productos NP," + Environment.NewLine;
                sSql += "ctt_pueblos PU" + Environment.NewLine;
                sSql += "where NP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and PU.id_ctt_pueblo = P.id_ctt_pueblo_origen" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "and PU.estado = 'A'" + Environment.NewLine;
                sSql += "and P.nivel = 2" + Environment.NewLine;
                sSql += "and P.aplica_extra = 0" + Environment.NewLine;
                sSql += "and P.cobros_tickets = 1" + Environment.NewLine;
                sSql += "order by P.codigo";

                comboE.ISSQL = sSql;
                cmbOrigen.DataSource = comboM.listarCombo(comboE);
                cmbOrigen.DataValueField = "IID";
                cmbOrigen.DataTextField = "IDATO";
                cmbOrigen.DataBind();
                cmbOrigen.Items.Insert(0, new ListItem("Seleccione Origen", "0"));

                if (cmbOrigen.Items.Count > 1)
                {
                    cmbOrigen.SelectedIndex = 1;
                }

                else
                {
                    cmbOrigen.SelectedIndex = 0;
                }

                //COMBO PARA FILTRAR EL GRIDVIEW
                comboE.ISSQL = sSql;
                cmbFiltrarGrid.DataSource = comboM.listarCombo(comboE);
                cmbFiltrarGrid.DataValueField = "IID";
                cmbFiltrarGrid.DataTextField = "IDATO";
                cmbFiltrarGrid.DataBind();
                cmbFiltrarGrid.Items.Insert(0, new ListItem("Todos...!!!", "0"));
                cmbFiltrarGrid.SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }


        //FUNCION PARA LLENAR EL COMOBOX DE ORIGEN Y DESTINO
        private void llenarComboUnidades()
        {
            try
            {
                sSql = "";
                sSql += "select correlativo, valor_texto" + Environment.NewLine;
                sSql += "from tp_codigos" + Environment.NewLine;
                sSql += "where tabla='SYS$00042'" + Environment.NewLine;
                sSql += "and estado = 'A'";

                comboE.ISSQL = sSql;

                cmbUnidad.DataSource = comboM.listarCombo(comboE);
                cmbUnidad.DataValueField = "IID";
                cmbUnidad.DataTextField = "IDATO";
                cmbUnidad.DataBind();
                cmbUnidad.Items.Insert(0, new ListItem("Seleccione Unidad", "0"));

                if (cmbUnidad.Items.Count > 24)
                {
                    cmbUnidad.SelectedIndex = 24;
                }

                else
                {
                    cmbUnidad.SelectedIndex = 0;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA OBTENER LOS DATOS DE LA LISTA BASE Y MINORISTA
        private void datosListas()
        {
            try
            {
                sSql = "";
                sSql += "select id_lista_precio, fecha_fin_validez" + Environment.NewLine;
                sSql += "from cv403_listas_precios" + Environment.NewLine;
                sSql += "where lista_base = 1" + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "union" + Environment.NewLine;
                sSql += "select id_lista_precio, fecha_fin_validez" + Environment.NewLine;
                sSql += "from cv403_listas_precios" + Environment.NewLine;
                sSql += "where lista_minorista = 1" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Session["lista_base"] = dtConsulta.Rows[0]["id_lista_precio"].ToString();
                        Session["fecha_base"] = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_fin_validez"].ToString()).ToString("yyyy/MM/dd");
                        Session["lista_minorista"] = dtConsulta.Rows[1]["id_lista_precio"].ToString();
                        Session["fecha_minorista"] = Convert.ToDateTime(dtConsulta.Rows[1]["fecha_fin_validez"].ToString()).ToString("yyyy/MM/dd");
                    }
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LAS COLUMNAS DEL GRID
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[2].Visible = ok;
            dgvDatos.Columns[3].Visible = ok;
            dgvDatos.Columns[4].Visible = ok;
            dgvDatos.Columns[5].Visible = ok;
            dgvDatos.Columns[9].Visible = ok;

            dgvDatos.Columns[6].ItemStyle.Width = 100;
            dgvDatos.Columns[7].ItemStyle.Width = 300;
            dgvDatos.Columns[8].ItemStyle.Width = 100;
            dgvDatos.Columns[9].ItemStyle.Width = 100;
            dgvDatos.Columns[10].ItemStyle.Width = 100;
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select P.id_producto, P.id_producto_padre," + Environment.NewLine;
                sSql += "P.paga_iva, UP.cg_unidad, P.aplica_retencion_ticket," + Environment.NewLine;
                sSql += "P.porcentaje_retencion_ticket, P.codigo, NP.nombre," + Environment.NewLine;
                sSql += "ltrim(str(PP.valor, 10, 2)) valor, P.aplica_pago_administracion" + Environment.NewLine;
                sSql += "from cv401_productos P, cv401_nombre_productos NP," + Environment.NewLine;
                sSql += "cv403_precios_productos PP, cv401_unidades_productos UP," + Environment.NewLine;
                sSql += "cv401_productos PADRE" + Environment.NewLine;
                sSql += "where NP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and PP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and UP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and P.id_producto_padre = PADRE.id_producto" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "and PP.estado = 'A'" + Environment.NewLine;
                sSql += "and UP.estado = 'A'" + Environment.NewLine;
                sSql += "and PP.id_lista_precio = " + Convert.ToInt32(Session["lista_minorista"].ToString()) + Environment.NewLine;
                sSql += "and UP.unidad_compra = 0" + Environment.NewLine;
                sSql += "and PADRE.cobros_tickets = 1" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and P.id_producto_padre = " + Convert.ToInt32(cmbFiltrarGrid.SelectedValue) + Environment.NewLine;
                }

                else if (iOp == 2)
                {
                    sSql += "and NP.nombre like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by P.id_producto";

                columnasGrid(true);
                ticketE.ISQL = sSql;
                dgvDatos.DataSource = ticketM.listarPrecioTickets(ticketE);
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
        private void insertarRegistro(int iAplicaRetencion_P, int iAplicaPago_P, double dbPorcentajeRetencion_P, double dbPrecio_P)
        {
            try
            {
                //SECCION PARA VERIFICAR LOS REGISTROS POR CODIGO
                iConsultarRegistro = consultarRegistroCodigo();

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


                //INICIO DE INSERCION
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                string[] sSeparar = cmbOrigen.SelectedItem.ToString().Split('-');

                //INSTRUCCION SQL PARA INSERTAR EN  LA TABLA CV401_PRODUCTOS
                sSql = "";
                sSql += "insert into cv401_productos (" + Environment.NewLine;
                sSql += "idempresa, codigo, id_producto_padre, nivel," + Environment.NewLine;
                sSql += "ultimo_nivel, paga_iva, aplica_retencion_ticket, aplica_pago_administracion," + Environment.NewLine;
                sSql += "porcentaje_retencion_ticket, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idEmpresa"].ToString()) + ", '" + sSeparar[0].Trim() + "." + txtCodigo.Text.Trim() + "', ";
                sSql += Convert.ToInt32(cmbOrigen.SelectedValue) + ", 3, 0, " + iPagaIva + ", " + iAplicaRetencion_P + ", " + iAplicaPago_P + ", " + dbPorcentajeRetencion_P + "," + Environment.NewLine;
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //INSTRUCCIONES PARA EXTRER EL MÁXIMO DEL REGISTRO
                sTabla = "cv401_productos";
                sCampo = "id_producto";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo obtener el identificacion de la tabla cv401_productos.', 'danger');", true);
                    goto reversa;
                }

                else
                {
                    iIdProducto = Convert.ToInt32(iMaximo);
                }


                //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV401_NOMBRE_PRODUCTOS
                sSql = "";
                sSql += "insert into cv401_nombre_productos (" + Environment.NewLine;
                sSql += "id_producto, cg_tipo_nombre, nombre, nombre_interno, estado," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdProducto + ", 5076, '" + txtDescripcion.Text.Trim().ToUpper() + "', 1, 'A'," + Environment.NewLine;
                sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV401_UNIDADES_PRODUCTOS (UNIDAD DE COMPRA)
                sSql = "";
                sSql += "insert into cv401_unidades_productos (" + Environment.NewLine;
                sSql += "id_producto, cg_tipo_unidad, cg_unidad, unidad_compra, estado," + Environment.NewLine;
                sSql += "usuario_creacion, terminal_creacion, fecha_creacion, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdProducto + ", " + parametros.iUnidadCompra + ", " + Convert.ToInt32(cmbUnidad.SelectedValue) + ", ";
                sSql += "1, 'A', '" + sDatosMaximo[0] + "', '" + sDatosMaximo[0] + "', GETDATE()," + Environment.NewLine;
                sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }


                //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV401_UNIDADES_PRODUCTOS (UNIDAD DE CONSUMO)
                sSql = "";
                sSql += "insert into cv401_unidades_productos (" + Environment.NewLine;
                sSql += "id_producto, cg_tipo_unidad, cg_unidad, unidad_compra, estado," + Environment.NewLine;
                sSql += "usuario_creacion, terminal_creacion, fecha_creacion, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdProducto + ", " + parametros.iUnidadConsumo + ", " + Convert.ToInt32(cmbUnidad.SelectedValue) + ", ";
                sSql += "0, 'A', '" + sDatosMaximo[0] + "', '" + sDatosMaximo[0] + "', GETDATE()," + Environment.NewLine;
                sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV403_PRECIOS_PRODUCTOS (PRECIO DE COMPRA)
                sSql = "";
                sSql += "insert into cv403_precios_productos (" + Environment.NewLine;
                sSql += "id_lista_precio, id_producto, valor_porcentaje, valor, fecha_inicio," + Environment.NewLine;
                sSql += "fecha_final, fecha_ingreso, usuario_ingreso, terminal_ingreso, estado)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Session["lista_base"].ToString()) + ", " + iIdProducto + ", 0, 1,";
                sSql += "'" + DateTime.Now.ToString("yyyy/MM/dd") + "', '" + Convert.ToDateTime(Session["fecha_base"].ToString()).ToString("yyy/MM/dd") + "'," + Environment.NewLine;
                sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A')";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }


                //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV403_PRECIOS_PRODUCTOS (PRECIO MINORISTA)
                sSql = "";
                sSql += "insert into cv403_precios_productos (" + Environment.NewLine;
                sSql += "id_lista_precio, id_producto, valor_porcentaje, valor, fecha_inicio," + Environment.NewLine;
                sSql += "fecha_final, fecha_ingreso, usuario_ingreso, terminal_ingreso, estado)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Session["lista_minorista"].ToString()) + ", " + iIdProducto + ", 0, ";
                sSql += dbPrecio + ", '" + DateTime.Now.ToString("yyyy/MM/dd") + "'," + Environment.NewLine;
                sSql += "'" + Convert.ToDateTime(Session["fecha_minorista"].ToString()).ToString("yyy/MM/dd") + "', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A')";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //FINALIZACION DE INSERCION
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
        private void actualizarRegistro(int iAplicaRetencion_P, int iAplicaPago_P, double dbPorcentajeRetencion_P, double dbPrecio_P)
        {
            try
            {
                ////SECCION PARA VERIFICAR LOS REGISTROS POR TERMINALES
                //iConsultarRegistro = consultarRegistroTerminal();

                //if (iConsultarRegistro > 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'Ya existe un registro para el terminal seleccionado.', 'warning');", true);
                //    goto fin;
                //}

                //else if (iConsultarRegistro == -1)
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ocurrió un problema al consultar el terminal para el registro.', 'danger');", true);
                //    goto fin;
                //}

                //INICIO DE ACTUALIZACION
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                iIdProducto = Convert.ToInt32(Session["idRegistro"].ToString());

                //INSTRUCCION SQL PARA ACTUALIZAR EN LA TABLA CV401_PRODUCTOS
                sSql = "";
                sSql += "update cv401_productos set" + Environment.NewLine;
                sSql += "id_producto_padre = " + Convert.ToInt32(cmbOrigen.SelectedValue) + "," + Environment.NewLine;
                sSql += "aplica_retencion_ticket = " + iAplicaRetencion_P + "," + Environment.NewLine;
                sSql += "aplica_pago_administracion = " + iAplicaPago_P + "," + Environment.NewLine;
                sSql += "porcentaje_retencion_ticket = " + dbPorcentajeRetencion_P + "," + Environment.NewLine;
                sSql += "paga_iva = " + iPagaIva + Environment.NewLine;
                sSql += "where id_producto = " + iIdProducto + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //INSTRUCCION SQL PARA ACTUALIZAR EN LA TABLA CV401_NOMBRE_PRODUCTOS
                sSql = "";
                sSql += "update cv401_nombre_productos set" + Environment.NewLine;
                sSql += "nombre = '" + txtDescripcion.Text.Trim().ToUpper() + "'" + Environment.NewLine;
                sSql += "where id_producto = " + iIdProducto + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //PROCEDIMIENTO PARA ACTUALIZAR LAS UNIDADES DEL PRODUCTO
                if (Convert.ToInt32(Session["idUnidad"].ToString()) != Convert.ToInt32(cmbUnidad.SelectedValue))
                {
                    //INSTRUCCION SQL PARA ACTUALIZAR A ESTADO E, LAS UNIDADES DEL PRODUCTO
                    sSql = "";
                    sSql += "update cv401_unidades_productos set" + Environment.NewLine;
                    sSql += "estado = 'E'," + Environment.NewLine;
                    sSql += "fecha_anulacion = GETDATE()," + Environment.NewLine;
                    sSql += "usuario_anulacion = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                    sSql += "terminal_anulacion = '" + sDatosMaximo[1] + "'," + Environment.NewLine;
                    sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                    sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                    sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                    sSql += "where id_producto = " + iIdProducto + Environment.NewLine;
                    sSql += "and estado = 'A'";

                    //EJECUCION DE LA INSTRUCCION SQL
                    if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        goto reversa;
                    }


                    //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV401_UNIDADES_PRODUCTOS (UNIDAD DE COMPRA)
                    sSql = "";
                    sSql += "insert into cv401_unidades_productos (" + Environment.NewLine;
                    sSql += "id_producto, cg_tipo_unidad, cg_unidad, unidad_compra, estado," + Environment.NewLine;
                    sSql += "usuario_creacion, terminal_creacion, fecha_creacion, fecha_ingreso," + Environment.NewLine;
                    sSql += "usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += iIdProducto + ", " + parametros.iUnidadCompra + ", " + Convert.ToInt32(cmbUnidad.SelectedValue) + ", ";
                    sSql += "1, 'A', '" + sDatosMaximo[0] + "', '" + sDatosMaximo[0] + "', GETDATE()," + Environment.NewLine;
                    sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                    //EJECUCION DE LA INSTRUCCION SQL
                    if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        goto reversa;
                    }


                    //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV401_UNIDADES_PRODUCTOS (UNIDAD DE CONSUMO)
                    sSql = "";
                    sSql += "insert into cv401_unidades_productos (" + Environment.NewLine;
                    sSql += "id_producto, cg_tipo_unidad, cg_unidad, unidad_compra, estado," + Environment.NewLine;
                    sSql += "usuario_creacion, terminal_creacion, fecha_creacion, fecha_ingreso," + Environment.NewLine;
                    sSql += "usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += iIdProducto + ", " + parametros.iUnidadConsumo + ", " + Convert.ToInt32(cmbUnidad.SelectedValue) + ", ";
                    sSql += "0, 'A', '" + sDatosMaximo[0] + "', '" + sDatosMaximo[0] + "', GETDATE()," + Environment.NewLine;
                    sSql += "GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                    //EJECUCION DE LA INSTRUCCION SQL
                    if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        goto reversa;
                    }
                }

                //PROCEDIMIENTO PARA ACTUALIZAR EL PRECIO DEL PRODUCTO
                //if (Convert.ToDouble(Session["valorPasaje"].ToString()) != Convert.ToDouble(txtValor.Text.Trim()))
                if (Convert.ToDouble(Session["valorPasaje"].ToString()) != dbPrecio_P)
                {
                    //INSTRUCCION SQL PARA ACTUALIZAR A ESTADO E, EL PRECIO MINORISTA
                    sSql = "";
                    sSql += "update cv403_precios_productos set" + Environment.NewLine;
                    sSql += "estado = 'E'," + Environment.NewLine;
                    sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                    sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                    sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                    sSql += "where id_producto = " + iIdProducto + Environment.NewLine;
                    sSql += "and id_lista_precio = " + Convert.ToInt32(Session["lista_minorista"].ToString());

                    //EJECUCION DE LA INSTRUCCION SQL
                    if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        goto reversa;
                    }


                    //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV403_PRECIOS_PRODUCTOS (PRECIO MINORISTA)
                    sSql = "";
                    sSql += "insert into cv403_precios_productos (" + Environment.NewLine;
                    sSql += "id_lista_precio, id_producto, valor_porcentaje, valor, fecha_inicio," + Environment.NewLine;
                    sSql += "fecha_final, fecha_ingreso, usuario_ingreso, terminal_ingreso, estado)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += Convert.ToInt32(Session["lista_minorista"].ToString()) + ", " + iIdProducto + ", 0, ";
                    sSql += dbPrecio_P + ", '" + DateTime.Now.ToString("yyyy/MM/dd") + "'," + Environment.NewLine;
                    sSql += "'" + Convert.ToDateTime(Session["fecha_minorista"].ToString()).ToString("yyy/MM/dd") + "', GETDATE()," + Environment.NewLine;
                    sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A')";

                    //EJECUCION DE LA INSTRUCCION SQL
                    if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        goto reversa;
                    }
                }

                //FINALIZACION DE ACTUALIZACION
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
                iConsultarRegistro = consultarRegistroProduccion();

                if (iConsultarRegistro > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'Existen registros relacionados con el que desea eliminar.', 'warning');", true);
                    goto fin;
                }

                else if (iConsultarRegistro == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'Ocurrió un problema al consultar los registro para eliminación.', 'danger');", true);
                    goto fin;
                }

                //INICIO DE ELIMINACION
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                iIdProducto = Convert.ToInt32(Session["idRegistro"].ToString());

                //INSTRUCCION SQL PARA ACTUALIZAR A ESTADO E, EL REGISTRO EN LA TABLA CV401_PRODUCTOS
                sSql = "";
                sSql += "update cv401_productos set" + Environment.NewLine;
                sSql += "codigo = 'codigo." + Convert.ToInt32(Session["idRegistro"].ToString()) + "'," + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_producto = " + iIdProducto;

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }


                //INSTRUCCION SQL PARA ACTUALIZAR A ESTADO E, EL REGISTRO EN LA TABLA CV401_NOMBRE_PRODUCTOS
                sSql = "";
                sSql += "update cv401_nombre_productos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_producto = " + iIdProducto;

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //INSTRUCCION SQL PARA ACTUALIZAR A ESTADO E, LAS UNIDADES DEL PRODUCTO
                sSql = "";
                sSql += "update cv401_unidades_productos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anulacion = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anulacion = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anulacion = '" + sDatosMaximo[1] + "'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_producto = " + iIdProducto + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }


                //INSTRUCCION SQL PARA ACTUALIZAR A ESTADO E, LOS PRECIOS
                sSql = "";
                sSql += "update cv403_precios_productos set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_producto = " + iIdProducto;

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //FINALIZACION DEL ELIMINACION
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


        //FUNCION PARA CONSULTAR SI EXISTE EL REGISTRO EN LA BASE DE DATOS POR CODIGO
        private int consultarRegistroCodigo()
        {
            try
            {
                string[] sSeparar = cmbOrigen.SelectedItem.ToString().Split('-');

                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from cv401_productos" + Environment.NewLine;
                sSql += "where codigo = '" + sSeparar[0].Trim() + "." + txtCodigo.Text.Trim() + "'" + Environment.NewLine;
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

        //FUNCION PARA CONSULTAR EL REGISTRO EN CASO DE ELIMINACION
        private int consultarRegistroProduccion()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from cv403_det_pedidos" + Environment.NewLine;
                sSql += "where id_producto = " + Convert.ToInt32(Session["idRegistro"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;

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


        //VALIDAR COMBOBOX
        //private void validarCombos()
        //{
        //    try
        //    {
        //        string[] sSeparar = cmbOrigen.SelectedItem.ToString().Split('-');

        //        if (Convert.ToInt32(cmbOrigen.SelectedValue) == 0)
        //        {
        //            txtDescripcion.Text = "NINGUNO";
        //        }

        //        else
        //        {
        //            txtDescripcion.Text = sSeparar[1].Trim();
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
        //    }
        //}

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtCodigo.Text = "";
            txtDescripcion.Text = "";
            txtPorcentajeRetencion.Text = "";
            txtValor.Text = "0";
            Session["idRegistro"] = null;
            Session["idUnidad"] = null;
            Session["valorPasaje"] = null;
            Session["buscar"] = "0";
            btnSave.Text = "Crear";
            MsjValidarCampos.Visible = false;
            txtCodigo.ReadOnly = false;
            chkPagaIva.Checked = false;
            rdbCobroRetencion.Checked = true;
            rdbCobroAdministracion.Checked = false;
            pnlAdministracion.Visible = false;
            pnlRetencion.Visible = true;
            txtPorcentajeRetencion.Text = "0";
            txtCodigo.Focus();
            llenarComboOrigen();
            llenarGrid(0);
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
                    cmbOrigen.SelectedValue = dgvDatos.Rows[a].Cells[1].Text;

                    if (dgvDatos.Rows[a].Cells[2].Text == "1")
                    {
                        chkPagaIva.Checked = true;
                    }

                    else
                    {
                        chkPagaIva.Checked = false;
                    }

                    Session["idUnidad"] = dgvDatos.Rows[a].Cells[3].Text;
                    cmbUnidad.SelectedValue = dgvDatos.Rows[a].Cells[3].Text;

                    if (dgvDatos.Rows[a].Cells[4].Text == "1")
                    {                        
                        pnlRetencion.Visible = true;
                        pnlAdministracion.Visible = false;
                        txtPorcentajeRetencion.Text = dgvDatos.Rows[a].Cells[5].Text;
                        rdbCobroRetencion.Checked = true;
                        rdbCobroAdministracion.Checked = false;
                    }

                    else if (dgvDatos.Rows[a].Cells[9].Text == "1")
                    {
                        pnlRetencion.Visible = false;
                        pnlAdministracion.Visible = true;
                        txtPorcentajeRetencion.Text = dgvDatos.Rows[a].Cells[5].Text;
                        rdbCobroRetencion.Checked = false;
                        rdbCobroAdministracion.Checked = true;
                    }

                    else 
                    {
                        pnlRetencion.Visible = true;
                        pnlAdministracion.Visible = false;
                        txtPorcentajeRetencion.Text = dgvDatos.Rows[a].Cells[5].Text;
                        rdbCobroRetencion.Checked = true;
                        rdbCobroAdministracion.Checked = false;
                    }
                    
                    txtCodigo.Text = dgvDatos.Rows[a].Cells[6].Text;
                    txtDescripcion.Text = dgvDatos.Rows[a].Cells[7].Text;
                    Session["valorPasaje"] = dgvDatos.Rows[a].Cells[8].Text;
                    txtValor.Text = dgvDatos.Rows[a].Cells[8].Text;

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
            Session["buscar"] = "0";

            if (txtFiltrar.Text.Trim() == "")
            {
                llenarGrid(0);
            }

            else
            {
                llenarGrid(2);
            }
        }

        protected void btnFiltrarGrid_Click(object sender, EventArgs e)
        {
            Session["buscar"] = "1";


            if (txtFiltrar.Text.Trim() == "")
            {
                llenarGrid(0);
            }

            else
            {
                llenarGrid(2);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbOrigen.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbOrigen.Focus();
            }

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

            else if (txtValor.Text.Trim() == "")
            {
                MsjValidarCampos.Visible = true;
                txtDescripcion.Focus();
            }

            else
            {
                if (chkPagaIva.Checked == true)
                {
                    iPagaIva = 1;
                }

                else
                {
                    iPagaIva = 0;
                }


                if (rdbCobroRetencion.Checked == true)
                {
                    iAplicaRetencion = 1;
                    iAplicaPago = 0;
                    dbPorcentajeRetencion = Convert.ToDouble(txtPorcentajeRetencion.Text.Trim());
                    dbPrecio = 1;

                }

                else if (rdbCobroAdministracion.Checked == true)
                {
                    iAplicaRetencion = 0;
                    iAplicaPago = 1;
                    dbPorcentajeRetencion = 0;
                    dbPrecio = Convert.ToDouble(txtValor.Text.Trim());
                }

                if (Session["idRegistro"] == null)
                {
                    //ENVIO A FUNCION DE INSERCION
                    insertarRegistro(iAplicaRetencion, iAplicaPago, dbPorcentajeRetencion, dbPrecio);
                }

                else
                {
                    actualizarRegistro(iAplicaRetencion, iAplicaPago, dbPorcentajeRetencion, dbPrecio);
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

                else if (Convert.ToInt32(Session["buscar"].ToString()) == 1)
                {
                    llenarGrid(1);
                }

                else
                {
                    llenarGrid(2);
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void cmbFiltrarGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbFiltrarGrid.SelectedValue) == 0)
            {
                Session["buscar"] = "0";
                llenarGrid(0);
            }

            else
            {
                Session["buscar"] = "1";
                llenarGrid(1);
            }
        }


        protected void rdbCobroRetencion_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCobroRetencion.Checked == true)
            {
                pnlRetencion.Visible = true;
                pnlAdministracion.Visible = false;
                txtPorcentajeRetencion.Text = "0";
                txtValor.Text = "0";
                rdbCobroAdministracion.Checked = false;
                txtPorcentajeRetencion.Focus();
            }            
        }

        protected void rdbCobroAdministracion_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCobroAdministracion.Checked == true)
            {
                pnlRetencion.Visible = false;
                pnlAdministracion.Visible = true;
                txtPorcentajeRetencion.Text = "0";
                txtValor.Text = "0";
                rdbCobroRetencion.Checked = false;
                txtValor.Focus();
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