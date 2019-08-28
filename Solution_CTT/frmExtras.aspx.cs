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
    public partial class frmExtras : System.Web.UI.Page
    {
        ENTPrecioPasajes precioE = new ENTPrecioPasajes();
        ENTComboDatos comboE = new ENTComboDatos();
        manejadorComboDatos comboM = new manejadorComboDatos();
        manejadorPrecioPasajes precioM = new manejadorPrecioPasajes();
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

            Session["modulo"] = "MÓDULO DE PRECIOS PARA VIAJES EXTRAS";
            Session["buscar"] = "0";

            if (!IsPostBack)
            {
                Session["idRegistro"] = null;
                llenarComboOrigen();
                llenarComboDestinos();
                llenarComboUnidades();
                datosListas();
                llenarGrid(0);
                validarCombos();
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
                sSql += "and aplica_extra = 1" + Environment.NewLine;
                sSql += "and cobros_tickets = 0" + Environment.NewLine;
                sSql += "order by P.codigo";

                //COMBO PARA GUARDAR
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE DESTINO
        private void llenarComboDestinos()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_pueblo, descripcion" + Environment.NewLine;
                sSql += "from ctt_pueblos" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "order by descripcion";

                comboE.ISSQL = sSql;
                cmbDestino.DataSource = comboM.listarCombo(comboE);
                cmbDestino.DataValueField = "IID";
                cmbDestino.DataTextField = "IDATO";
                cmbDestino.DataBind();
                cmbDestino.Items.Insert(0, new ListItem("Seleccione Destino", "0"));

                if (cmbDestino.Items.Count > 1)
                {
                    cmbDestino.SelectedIndex = 1;
                }

                else
                {
                    cmbDestino.SelectedIndex = 0;
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "union" + Environment.NewLine;
                sSql += "select id_lista_precio, fecha_fin_validez" + Environment.NewLine;
                sSql += "from cv403_listas_precios" + Environment.NewLine;
                sSql += "where lista_otros = 1" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count == 3)
                    {
                        Session["lista_base"] = dtConsulta.Rows[0]["id_lista_precio"].ToString();
                        Session["fecha_base"] = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_fin_validez"].ToString()).ToString("yyyy/MM/dd");
                        Session["lista_minorista"] = dtConsulta.Rows[1]["id_lista_precio"].ToString();
                        Session["fecha_minorista"] = Convert.ToDateTime(dtConsulta.Rows[1]["fecha_fin_validez"].ToString()).ToString("yyyy/MM/dd");
                        Session["lista_otros"] = dtConsulta.Rows[2]["id_lista_precio"].ToString();
                        Session["fecha_otros"] = Convert.ToDateTime(dtConsulta.Rows[2]["fecha_fin_validez"].ToString()).ToString("yyyy/MM/dd");
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se encuentran todas las listas de precios configuradas. Comuníquese con el administrador del sistema', 'danger');", true);
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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

            dgvDatos.Columns[5].ItemStyle.Width = 100;
            dgvDatos.Columns[6].ItemStyle.Width = 300;
            dgvDatos.Columns[7].ItemStyle.Width = 100;
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
                sSql += "select * from ctt_vw_lista_precios_pasajes_extras" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql += "where id_producto_padre = " + Convert.ToInt32(cmbFiltrarGrid.SelectedValue) + Environment.NewLine;
                }

                else if (iOp == 2)
                {
                    sSql += "where nombre like '%" + txtFiltrar.Text.Trim() + "%'" + Environment.NewLine;
                }

                sSql += "order by id_producto";

                columnasGrid(true);
                precioE.ISQL = sSql;
                dgvDatos.DataSource = precioM.listarPrecioPasajes(precioE);
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
                sSql += "idempresa, codigo, id_producto_padre, id_ctt_pueblo_destino, nivel, aplica_extra," + Environment.NewLine;
                sSql += "ultimo_nivel, paga_iva, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Application["idEmpresa"].ToString()) + ", '" + sSeparar[0].Trim() + "." + txtCodigo.Text.Trim() + "', ";
                sSql += Convert.ToInt32(cmbOrigen.SelectedValue) + ", " + Convert.ToInt32(cmbDestino.SelectedValue) + ", 3, 1, 0, " + iPagaIva + "," + Environment.NewLine;
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
                sSql += Convert.ToDouble(txtValor.Text.Trim()) + ", '" + DateTime.Now.ToString("yyyy/MM/dd") + "'," + Environment.NewLine;
                sSql += "'" + Convert.ToDateTime(Session["fecha_minorista"].ToString()).ToString("yyy/MM/dd") + "', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', 'A')";

                //EJECUCION DE LA INSTRUCCION SQL
                if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    goto reversa;
                }

                //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV403_PRECIOS_PRODUCTOS (PRECIO OTROS)
                sSql = "";
                sSql += "insert into cv403_precios_productos (" + Environment.NewLine;
                sSql += "id_lista_precio, id_producto, valor_porcentaje, valor, fecha_inicio," + Environment.NewLine;
                sSql += "fecha_final, fecha_ingreso, usuario_ingreso, terminal_ingreso, estado)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(Session["lista_otros"].ToString()) + ", " + iIdProducto + ", 0, ";
                sSql += Convert.ToDouble(txtValorOtros.Text.Trim()) + ", '" + DateTime.Now.ToString("yyyy/MM/dd") + "'," + Environment.NewLine;
                sSql += "'" + Convert.ToDateTime(Session["fecha_otros"].ToString()).ToString("yyy/MM/dd") + "', GETDATE()," + Environment.NewLine;
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
                sSql += "id_ctt_pueblo_destino = " + Convert.ToInt32(cmbDestino.SelectedValue) + "," + Environment.NewLine;
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
                if (Convert.ToDouble(Session["valorPasaje"].ToString()) != Convert.ToDouble(txtValor.Text.Trim()))
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
                    sSql += Convert.ToDouble(txtValor.Text.Trim()) + ", '" + DateTime.Now.ToString("yyyy/MM/dd") + "'," + Environment.NewLine;
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

                //PROCEDIMIENTO PARA ACTUALIZAR EL PRECIO DEL PRODUCTO VALOR OTROS
                if (Convert.ToDouble(Session["valorPasajeOtros"].ToString()) != Convert.ToDouble(txtValorOtros.Text.Trim()))
                {
                    //INSTRUCCION SQL PARA ACTUALIZAR A ESTADO E, EL PRECIO OTROS
                    sSql = "";
                    sSql += "update cv403_precios_productos set" + Environment.NewLine;
                    sSql += "estado = 'E'," + Environment.NewLine;
                    sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                    sSql += "usuario_anula = '" + sDatosMaximo[0] + "'," + Environment.NewLine;
                    sSql += "terminal_anula = '" + sDatosMaximo[1] + "'" + Environment.NewLine;
                    sSql += "where id_producto = " + iIdProducto + Environment.NewLine;
                    sSql += "and id_lista_precio = " + Convert.ToInt32(Session["lista_otros"].ToString());

                    //EJECUCION DE LA INSTRUCCION SQL
                    if (conexionM.ejecutarInstruccionSQL(sSql) == false)
                    {
                        lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                        goto reversa;
                    }


                    //INSTRUCCION SQL PARA INSERTAR EN LA TABLA CV403_PRECIOS_PRODUCTOS (PRECIO OTROS)
                    sSql = "";
                    sSql += "insert into cv403_precios_productos (" + Environment.NewLine;
                    sSql += "id_lista_precio, id_producto, valor_porcentaje, valor, fecha_inicio," + Environment.NewLine;
                    sSql += "fecha_final, fecha_ingreso, usuario_ingreso, terminal_ingreso, estado)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += Convert.ToInt32(Session["lista_otros"].ToString()) + ", " + iIdProducto + ", 0, ";
                    sSql += Convert.ToDouble(txtValorOtros.Text.Trim()) + ", '" + DateTime.Now.ToString("yyyy/MM/dd") + "'," + Environment.NewLine;
                    sSql += "'" + Convert.ToDateTime(Session["fecha_otros"].ToString()).ToString("yyy/MM/dd") + "', GETDATE()," + Environment.NewLine;
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                return -1;
            }
        }


        //VALIDAR COMBOBOX
        private void validarCombos()
        {
            try
            {
                string[] sSeparar = cmbOrigen.SelectedItem.ToString().Split('-');

                if ((Convert.ToInt32(cmbOrigen.SelectedValue) == 0) && (Convert.ToInt32(cmbDestino.SelectedValue) == 0))
                {
                    txtDescripcion.Text = "NINGUNO - NINGUNO";
                }

                else if ((Convert.ToInt32(cmbOrigen.SelectedValue) == 0) && (Convert.ToInt32(cmbDestino.SelectedValue) != 0))
                {
                    txtDescripcion.Text = "NINGUNO - " + cmbDestino.SelectedItem.ToString();
                }

                else if ((Convert.ToInt32(cmbOrigen.SelectedValue) != 0) && (Convert.ToInt32(cmbDestino.SelectedValue) == 0))
                {
                    txtDescripcion.Text = sSeparar[1].Trim() + " - NINGUNO";
                }

                else
                {
                    txtDescripcion.Text = sSeparar[1].Trim() + " - " + cmbDestino.SelectedItem.ToString();
                }
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtCodigo.Text = "";
            txtDescripcion.Text = "";
            txtValor.Text = "";
            txtValorOtros.Text = "";
            Session["idRegistro"] = null;
            Session["idUnidad"] = null;
            Session["valorPasaje"] = null;
            Session["valorPasajeOtros"] = null;
            Session["buscar"] = "0";
            btnSave.Text = "Crear";
            MsjValidarCampos.Visible = false;
            txtCodigo.ReadOnly = false;
            chkPagaIva.Checked = false;
            txtCodigo.Focus();
            llenarComboOrigen();
            llenarComboDestinos();
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
                    cmbDestino.Text = dgvDatos.Rows[a].Cells[2].Text;

                    if (dgvDatos.Rows[a].Cells[3].Text == "1")
                    {
                        chkPagaIva.Checked = true;
                    }

                    else
                    {
                        chkPagaIva.Checked = false;
                    }

                    Session["idUnidad"] = dgvDatos.Rows[a].Cells[4].Text;
                    cmbUnidad.SelectedValue = dgvDatos.Rows[a].Cells[4].Text;
                    txtCodigo.Text = dgvDatos.Rows[a].Cells[5].Text;
                    txtDescripcion.Text = dgvDatos.Rows[a].Cells[6].Text;
                    Session["valorPasaje"] = dgvDatos.Rows[a].Cells[7].Text;
                    txtValor.Text = dgvDatos.Rows[a].Cells[7].Text;
                    Session["valorPasajeOtros"] = dgvDatos.Rows[a].Cells[8].Text;
                    txtValorOtros.Text = dgvDatos.Rows[a].Cells[8].Text;

                    txtCodigo.ReadOnly = true;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbOrigen.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbOrigen.Focus();
            }

            else if (Convert.ToInt32(cmbDestino.SelectedValue) == 0)
            {
                MsjValidarCampos.Visible = true;
                cmbDestino.Focus();
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
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        protected void cmbOrigen_SelectedIndexChanged(object sender, EventArgs e)
        {
            validarCombos();
        }

        protected void cmbDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            validarCombos();
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
    }
}