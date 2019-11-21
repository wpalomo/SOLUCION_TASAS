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
    public partial class frmCategorias : System.Web.UI.Page
    {
        ENTCategorias categoriaE = new ENTCategorias();
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorCategorias categoriaM = new manejadorCategorias();
        manejadorConexion conexionM = new manejadorConexion();

        Clases.ClaseParametros parametros = new Clases.ClaseParametros();

        string sSql;
        string[] sDatosMaximo = new string[5];
        string sAccion;
        string sTabla;
        string sCampo;

        DataTable dtConsulta;
        bool bRespuesta;

        int iModificable;
        int iPrecioModificable;
        int iPagaIva;
        int iConsultarRegistro;
        int iIdProductoPadre;
        int iIdProducto;
        int iAplicaExtra;
        int iCobroTickets;

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

            Session["modulo"] = "MÓDULO DE CATEGORÍAS";

            if (!IsPostBack)
            {                
                llenarComboProductoPadre();
                llenarComboOrigen();
                llenarComboUnidades();
                llenarGrid(0);
            }
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMOBOX DE TERMINALES
        private void llenarComboProductoPadre()
        {
            try
            {
                sSql = "";
                sSql += "select P.codigo, '[' + P.codigo + '] ' + NP.nombre nombre_producto" + Environment.NewLine;
                sSql += "from cv401_productos P," + Environment.NewLine;
                sSql += "cv401_nombre_productos NP" + Environment.NewLine;
                sSql += "where NP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and P.nivel = 1" + Environment.NewLine;
                sSql += "and NP.nombre_interno = 1" + Environment.NewLine;
                sSql += "order by P.codigo" + Environment.NewLine;

                comboE.ISSQL = sSql;
                cmbProductoPadre.DataSource = comboM.listarCombo(comboE);
                cmbProductoPadre.DataValueField = "IID";
                cmbProductoPadre.DataTextField = "IDATO";
                cmbProductoPadre.DataBind();
                cmbProductoPadre.Items.Insert(0, new ListItem("Seleccione Producto Padre", "0"));

                if (cmbProductoPadre.Items.Count > 2)
                {
                    cmbProductoPadre.SelectedIndex = 2;
                }

                else
                {
                    cmbProductoPadre.SelectedIndex = 0;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMOBOX DE ORIGEN Y DESTINO
        private void llenarComboOrigen()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_pueblo, descripcion" + Environment.NewLine;
                sSql += "from ctt_pueblos" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and terminal = 1";

                comboE.ISSQL = sSql;

                cmbOrigen.DataSource = comboM.listarCombo(comboE);
                cmbOrigen.DataValueField = "IID";
                cmbOrigen.DataTextField = "IDATO";
                cmbOrigen.DataBind();
                cmbOrigen.Items.Insert(0, new ListItem("Seleccione Origen", "0"));

                if (cmbOrigen.Items.Count > 0)
                {
                    cmbOrigen.SelectedIndex = 1;
                }

                cmbFiltrarGrid.DataSource = comboM.listarCombo(comboE);
                cmbFiltrarGrid.DataValueField = "IID";
                cmbFiltrarGrid.DataTextField = "IDATO";
                cmbFiltrarGrid.DataBind();
                cmbFiltrarGrid.Items.Insert(0, new ListItem("Seleccione Origen", "0"));
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

        //FUNCION PARA LAS COLUMNAS
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[1].Visible = ok;
            dgvDatos.Columns[5].Visible = ok;
            dgvDatos.Columns[6].Visible = ok;
            dgvDatos.Columns[7].Visible = ok;
            dgvDatos.Columns[8].Visible = ok;
            dgvDatos.Columns[9].Visible = ok;
            dgvDatos.Columns[10].Visible = ok;

            dgvDatos.Columns[2].ItemStyle.Width = 100;
            dgvDatos.Columns[3].ItemStyle.Width = 200;
            dgvDatos.Columns[4].ItemStyle.Width = 200;
            dgvDatos.Columns[11].ItemStyle.Width = 100;
            dgvDatos.Columns[12].ItemStyle.Width = 100;
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select P.id_producto, P.id_ctt_pueblo_origen, P.codigo," + Environment.NewLine;
                sSql += "NP.nombre, PU.descripcion, P.modificable, P.precio_modificable," + Environment.NewLine;
                sSql += "P.paga_iva, UP.cg_unidad unidad_consumo, P.aplica_extra, P.cobros_tickets" + Environment.NewLine;
                sSql += "from cv401_productos P, cv401_nombre_productos NP," + Environment.NewLine;
                sSql += "cv401_unidades_productos UP, ctt_pueblos PU" + Environment.NewLine;
                sSql += "where NP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and UP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and P.id_ctt_pueblo_origen = PU.id_ctt_pueblo" + Environment.NewLine;
                sSql += "and id_producto_padre in (" + Environment.NewLine;
                sSql += "select id_producto from cv401_productos" + Environment.NewLine;
                sSql += "where codigo = '" + Convert.ToInt32(cmbProductoPadre.SelectedValue) + "')" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "and PU.estado = 'A'" + Environment.NewLine;
                sSql += "and UP.estado = 'A'" + Environment.NewLine;
                sSql += "and P.nivel = 2" + Environment.NewLine;
                sSql += "and UP.unidad_compra = 0" + Environment.NewLine;
                sSql += "and P.ctt_encomienda = 0" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "and P.id_ctt_pueblo_origen = " + Convert.ToInt32(cmbFiltrarGrid.SelectedValue) + Environment.NewLine;
                }

                else if (iOp == 2)
                {
                    sSql += "and (NP.nombre like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or PU.descripcion like '%" + txtFiltrar.Text.Trim() + "%')" + Environment.NewLine;
                }

                sSql += "order by P.id_producto";

                columnasGrid(true);
                categoriaE.ISQL = sSql;
                dgvDatos.DataSource = categoriaM.listarCategorias(categoriaE);
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

                //INICIO DE INSERCION
                if (conexionM.iniciarTransaccion() == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo iniciar la transacción para el proceso de información.', 'danger');", true);
                    goto fin;
                }

                //INSTRUCCION PARA EXTRAER EL ID DEL PRODUCTO PADRE
                sSql = "";
                sSql += "select id_producto" + Environment.NewLine;
                sSql += "from cv401_productos" + Environment.NewLine;
                sSql += "where codigo = '" + cmbProductoPadre.SelectedValue + "'" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iIdProductoPadre = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'No existe un registro guardado con el item seleccionado perteneciente al producto padre. Favor comuníquese con el administrador.', 'warning');", true);
                        goto reversa;
                    }
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Advertencia.!', 'Ocurrió un problema al consultar el identificador del producto padre..', 'warning');", true);
                    goto reversa;
                }

                //INSTRUCCION SQL PARA INSERTAR EN  LA TABLA CV401_PRODUCTOS
                sSql = "";
                sSql += "insert into cv401_productos (" + Environment.NewLine;
                sSql += "idempresa, codigo, id_producto_padre, id_ctt_pueblo_origen, nivel," + Environment.NewLine;
                sSql += "ultimo_nivel, paga_iva, precio_modificable, modificable, aplica_extra, cobros_tickets," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idEmpresa"].ToString()) + ", '" + Convert.ToInt32(cmbProductoPadre.SelectedValue) + "." + txtCodigo.Text.Trim() + "', ";
                sSql += iIdProductoPadre + ", " + Convert.ToInt32(cmbOrigen.SelectedValue) + ", 2, 0, " + iPagaIva + "," + Environment.NewLine;
                sSql += iPrecioModificable + ", " + iModificable + ", " + iAplicaExtra + ", " + iCobroTickets + ", 'A', GETDATE(), '"+ sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[1] + "')";                

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

                //FINALIZACION DE INSERCION
                conexionM.terminaTransaccion();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', 'Registro ingresado correctamente', 'success');", true);
                limpiar();
                goto fin;
            }

            catch(Exception ex)
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
                sSql += "id_ctt_pueblo_origen = " + Convert.ToInt32(cmbOrigen.SelectedValue) + "," + Environment.NewLine;
                sSql += "modificable = " + iModificable + "," + Environment.NewLine;
                sSql += "precio_modificable = " + iPrecioModificable + "," + Environment.NewLine;
                sSql += "aplica_extra = " + iAplicaExtra + "," + Environment.NewLine;
                sSql += "cobros_tickets = " + iCobroTickets + "," + Environment.NewLine;
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
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from cv401_productos" + Environment.NewLine;
                sSql += "where codigo = '" + cmbProductoPadre.SelectedValue + "." + txtCodigo.Text.Trim() + "'" + Environment.NewLine;
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

        ////FUNCION PARA CONSULTAR SI EXISTE EL REGISTRO EN LA BASE DE DATOS POR TERMINAL
        //private int consultarRegistroTerminal()
        //{
        //    try
        //    {
        //        sSql = "";
        //        sSql += "select count(*) cuenta" + Environment.NewLine;
        //        sSql += "from cv401_productos" + Environment.NewLine;
        //        sSql += "where id_ctt_pueblo_origen = " + Convert.ToInt32(cmbOrigen.SelectedValue) + Environment.NewLine;
        //        sSql += "and estado = 'A'";

        //        dtConsulta = new DataTable();
        //        dtConsulta.Clear();
        //        bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

        //        if (bRespuesta == true)
        //        {
        //            return Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());
        //        }

        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
        //            return -1;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Éxito.!', '" + ex.Message + "', 'danger');", true);
        //        return -1;
        //    }
        //}


        //FUNCION PARA CONSULTAR EL REGISTRO EN CASO DE ELIMINACION
        private int consultarRegistroProduccion()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from cv401_productos" + Environment.NewLine;
                sSql += "where id_producto_padre = " + Convert.ToInt32(Session["idRegistro"].ToString()) + Environment.NewLine;
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

            catch(Exception ex)
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
            Session["buscar"] = "0";
            Session["idRegistro"] = null;            
            btnSave.Text = "Crear";
            MsjValidarCampos.Visible = false;
            txtCodigo.ReadOnly = false;
            chkModificable.Checked = false;
            chkPrecioModificable.Checked = false;
            chkPagaIva.Checked = false;
            chkAplicaExtra.Checked = false;
            chkTickets.Checked = false;
            txtCodigo.Focus();
            llenarComboProductoPadre();
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
                    txtCodigo.Text = dgvDatos.Rows[a].Cells[2].Text;
                    txtDescripcion.Text = dgvDatos.Rows[a].Cells[3].Text;

                    if (dgvDatos.Rows[a].Cells[5].Text == "True")
                    {
                        chkModificable.Checked = true;
                    }

                    else
                    {
                        chkModificable.Checked = false;
                    }

                    if (dgvDatos.Rows[a].Cells[6].Text == "True")
                    {
                        chkPrecioModificable.Checked = true;
                    }

                    else
                    {
                        chkPrecioModificable.Checked = false;
                    }

                    if (dgvDatos.Rows[a].Cells[7].Text == "1")
                    {
                        chkPagaIva.Checked = true;
                    }

                    else
                    {
                        chkPagaIva.Checked = false;
                    }

                    if (dgvDatos.Rows[a].Cells[9].Text == "1")
                    {
                        chkAplicaExtra.Checked = true;
                    }

                    else
                    {
                        chkAplicaExtra.Checked = false;
                    }

                    if (dgvDatos.Rows[a].Cells[10].Text == "1")
                    {
                        chkTickets.Checked = true;
                    }

                    else
                    {
                        chkTickets.Checked = false;
                    }

                    Session["idUnidad"] = dgvDatos.Rows[a].Cells[8].Text;
                    cmbUnidad.SelectedValue = dgvDatos.Rows[a].Cells[8].Text;
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
            if (Convert.ToInt32(cmbProductoPadre.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbProductoPadre.Focus();
            }

            else if (Convert.ToInt32(cmbOrigen.SelectedValue) == 0)
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

            else
            {
                if (chkModificable.Checked == true)
                {
                    iModificable = 1;
                }

                else
                {
                    iModificable = 0;
                }

                if (chkPrecioModificable.Checked == true)
                {
                    iPrecioModificable = 1;
                }

                else
                {
                    iPrecioModificable = 0;
                }

                if (chkPagaIva.Checked == true)
                {
                    iPagaIva = 1;
                }

                else
                {
                    iPagaIva = 0;
                }

                if (chkAplicaExtra.Checked == true)
                {
                   iAplicaExtra = 1;
                }

                else
                {
                    iAplicaExtra = 0;
                }


                if (chkTickets.Checked == true)
                {
                    iCobroTickets = 1;
                }

                else
                {
                    iCobroTickets = 0;
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