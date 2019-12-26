using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using NEGOCIO;
using ENTIDADES;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace Solution_CTT
{
    public partial class frmLogin : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();
        manejadorComboDatos comboM = new manejadorComboDatos();
        ENTComboDatos comboE = new ENTComboDatos();

        Clases.ClaseParametros parametros = new Clases.ClaseParametros();
        Clases_Contifico.ClaseAutenticacion autenticacion;

        DataTable dtConsulta;

        string sSql;
        string sFecha;
        string sHora;
        string sTabla;
        string sCampo;
        string sEstadoCaja;
        string sFechaApertura;
        string sHoraApertura;
        string sUsuarioApertura;
        string sJornadaApertura;
        string sSaldoInicial;
        string sObervaciones;
        string path = "C:\\palatium\\config.ini";
        string[] sDatosMaximo = new string[5];

        bool bRespuesta;

        int iOp;
        int iJornada;
        int iRedireccion;
        int iCuentaJornadasCerradas;
        int iIdCierreCaja;
        int iBanderaNoJornada;
        int iIdCierreCajaApertura;
        int iIdJornadaApertura;

        DateTime fechaSistema;
        DateTime fechaCaja;

        long iMaximo;

        protected void Page_Load(object sender, EventArgs e)
        {
            lecturaConfiguracion(path);
            Session["ver_notificacion"] = "0";
            //conexionM.conectar();
            //consultarSucursal();
            //cargarParametrosGenerales();
            //cargarParametrosTerminal();

            if (!IsPostBack)
            {                
                //llenarComboTerminales();                

                //compararHoras();
                conexionM.conectar();
                consultarSucursal();
                cargarParametrosGenerales();
                cargarParametrosTerminal();
            }
        }

        #region FUNCIONES PARA MANEJO DE LA JORNADA

        //CONSULTAR LAS JORNADAS REGISTRADAS
        private void consultarJornadas()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_jornada, descripcion, secuencia" + Environment.NewLine;
                sSql += "from ctt_jornada" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "order by secuencia";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {                    
                    Session["matutina"] = dtConsulta.Rows[0]["descripcion"].ToString();
                    Session["tarde"] = dtConsulta.Rows[1]["descripcion"].ToString();
                }

                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'Error al cargar los parámetros de las jornadas.', 'error')</script>");
                }
            }

            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'Error al cargar los parámetros de las jornadas.', 'error')</script>");
            }
        }
        
        #endregion

        #region FUNCIONES DEL USUARIO

        class Util
        {
            [DllImport("kernel32")]
            public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filepath);

            [DllImport("kernel32")]
            public static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        }

        public bool lecturaConfiguracion(string archivo)
        {
            try
            {
                StringBuilder Empresa_Defecto = new StringBuilder();
                StringBuilder idLocalidad = new StringBuilder();
                StringBuilder MotivoDespacho = new StringBuilder();
                StringBuilder CgEmpresa = new StringBuilder();
                StringBuilder IdEmpresa = new StringBuilder();
                //StringBuilder IdPueblo = new StringBuilder();

                if (File.Exists(archivo))
                {
                    Util.GetPrivateProfileString("conexion", "id_localidad", "", idLocalidad, idLocalidad.Capacity, archivo);                 
                    Application["idLocalidad"] = idLocalidad.ToString();                    

                    Util.GetPrivateProfileString("conexion", "Cg_Empresa", "", CgEmpresa, CgEmpresa.Capacity, archivo);
                    Application["cgEmpresa"] = CgEmpresa.ToString();

                    Util.GetPrivateProfileString("conexion", "id_Empresa", "", IdEmpresa, IdEmpresa.Capacity, archivo);
                    Application["idEmpresa"] = IdEmpresa.ToString();

                    //Util.GetPrivateProfileString("conexion", "id_pueblo", "", IdPueblo, IdPueblo.Capacity, archivo);
                    //Session["id_pueblo"] = IdPueblo.ToString();

                    return true;
                }

                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                return false;
            }
        }

        //CONSULTAR EL NOMBRE DE LA SUCURSAL
        private void consultarSucursal()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_pueblo, descripcion" + Environment.NewLine;
                sSql += "from ctt_pueblos" + Environment.NewLine;
                sSql += "where id_localidad_terminal = " + Convert.ToInt32(Application["idLocalidad"].ToString()) + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    Session["id_pueblo"] = dtConsulta.Rows[0]["id_ctt_pueblo"].ToString();
                    Session["nombre_terminal"] = dtConsulta.Rows[0]["descripcion"].ToString();

                    lblSucursal.Text = "OFICINA: " + dtConsulta.Rows[0]["descripcion"].ToString().ToUpper().Trim();
                    Session["oficina"] = dtConsulta.Rows[0]["descripcion"].ToString().ToUpper().Trim();
                }

                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'Error al cargar los parámetros del terminal.', 'error')</script>");
                }
            }

            catch (Exception)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'Error al cargar los parámetros del terminal.', 'error')</script>");
            }
        }

        //FUNCION PARA CARGAR LOS PARÁMETROS GENERALES
        private void cargarParametrosGenerales()
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
                        ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'No se encuentra una configuración de parámetros.', 'error')</script>");
                    }
                }

                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'Error al cargar los parámetros del sistema.', 'error')</script>");
                }

                //DIRECTORIOS DE LA FACTURACION ELECTRONICA
                sSql = "";
                sSql += "select id_directorio, id_tipo_comprobante, orden, codigo, nombres" + Environment.NewLine;
                sSql += "from cel_directorio" + Environment.NewLine;
                sSql += "where id_tipo_comprobante = 1" + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "order by orden";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Application["RutaFacturasGeneradas"] = dtConsulta.Rows[0][4].ToString();
                        Application["RutaFacturasFirmadas"] = dtConsulta.Rows[1][4].ToString();
                        Application["RutaFacturasAutorizadas"] = dtConsulta.Rows[2][4].ToString();
                        Application["RutaFacturasNoAutorizadas"] = dtConsulta.Rows[3][4].ToString();
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'No se encuentra una configuración de parámetros.', 'error')</script>");
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'Error al cargar los parámetros del sistema.', 'error')</script>");
                }
                //FIN DIRECTORIOS
            }

            catch (Exception)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'Error al cargar los parámetros del sistema.', 'error')</script>");
            }
        }
        
        //FUNCION PARA CARGAR LOS PARÁMETROS POR TERMINAL
        private void cargarParametrosTerminal()
        {
            try
            {
                sSql = "";
                sSql += "select * from ctt_vw_parametros_localidad" + Environment.NewLine;
                sSql += "where id_ctt_pueblo = " + Convert.ToInt32(Session["id_pueblo"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        //Session["id_pueblo"] = dtConsulta.Rows[0][1].ToString();
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
                        Session["ejecuta_cobro_administrativo"] = dtConsulta.Rows[0][16].ToString();
                        Session["idLocalidadSMARTT"] = dtConsulta.Rows[0]["id_smartt"].ToString();

                        Session["tasaDevesofft"] = null;

                        if (dtConsulta.Rows[0]["codigo_proveedor"].ToString().Trim() == "01")
                        {
                            Session["tasaDevesofft"] = dtConsulta.Rows[0]["codigo_proveedor"].ToString();
                        }

                        else if (dtConsulta.Rows[0]["codigo_proveedor"].ToString().Trim() == "02")
                        {
                            Session["tasaContifico"] = dtConsulta.Rows[0]["codigo_proveedor"].ToString();
                        }
                    }

                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'No se encuentra una configuración de parámetros del terminal.', 'error')</script>");
                    }
                }

                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'Error al cargar los parámetros del terminal.', 'error')</script>");
                }
            }

            catch (Exception)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'Error al cargar los parámetros del terminal.', 'error')</script>");
            }
        }

        //FUNCION PARA CONSULTAR LOS DATOS DEL USUARIO
        private void consultarUsuario()
        {
            try
            {
                sSql = "";
                sSql += "select id_ctt_oficinista, descripcion, id_persona, claveacceso," + Environment.NewLine;
                sSql += "isnull(pos_secret, '') pos_secret" + Environment.NewLine;
                sSql += "from ctt_oficinista" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and usuario = '" + txtUsuario.Text.Trim().ToLower() + "'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {                    
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', '" + sSql + "', 'error')</script>");                    
                    goto fin;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', '" + Resources.MESSAGES.ERR_LOGIN + "', 'error')</script>");
                    txtUsuario.Text = "";
                    txtPassword.Text = "";
                    txtUsuario.Focus();
                    goto fin;
                }

                if (txtPassword.Text.Trim() == dtConsulta.Rows[0][3].ToString().Trim())
                {
                    Session["idUsuario"] = dtConsulta.Rows[0][0].ToString().ToUpper();
                    Session["usuario"] = dtConsulta.Rows[0][1].ToString().ToUpper();
                    Session["pos_secret"] = dtConsulta.Rows[0]["pos_secret"].ToString();

                    sDatosMaximo[0] = Session["usuario"].ToString().ToUpper();
                    sDatosMaximo[1] = Environment.MachineName.ToString();
                    sDatosMaximo[2] = "A";

                    if (Session["tasaContifico"] != null)
                    {
                        if (Session["pos_secret"].ToString() == "")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Información.!', 'El usuario no dispone de la credenciales para el uso de las tasas de usuario SMARTT', 'info')</script>");
                            return;
                        }

                        if (Session["tasaContifico"].ToString() == "02")
                        {
                            autenticacion = new Clases_Contifico.ClaseAutenticacion();

                            string sRespuesta_A = autenticacion.recuperarToken(dtConsulta.Rows[0][1].ToString().Trim().ToLower(), dtConsulta.Rows[0][3].ToString().Trim(), dtConsulta.Rows[0]["pos_secret"].ToString().Trim());

                            if (sRespuesta_A == "ERROR")
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'No se pudo obtener registros para la tasa de usuario SMARTT', 'error')</script>");
                                return;
                            }

                            if (sRespuesta_A == "ISNULL")
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Información.!', 'No se proporcionaron credenciales de autenticación. Tasa de Usuario SMARTT', 'info')</script>");
                                return;
                            }

                            Session["tokenSMARTT"] = sRespuesta_A;
                        }
                    }

                    if (!GetVariablesFE()) return;//CARGO RESTO DE PARAMETROS PARA LA FE

                    if (consultarEstadoCierreCaja() == false)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'No se pudo cargar los parámetros para el cierre de caja.', 'error')</script>");
                    }

                    else
                    {
                        Response.Redirect("frmPrincipal.aspx");                      
                    }
                }

                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', '"+ Resources.MESSAGES.ERR_LOGIN +"', 'error')</script>");
                    txtPassword.Text = "";
                    txtPassword.Focus();
                }
            }

            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', '" + ex.Message + "', 'error')</script>");
            }

        fin: { }
        }

        //FUNCION PARA CONSULTAR EL ESTADO DE CIERRE DE CAJA NUEVA VERSION
        private bool consultarEstadoCierreCaja()
        {
            try
            {
                consultarJornadas();

                sFecha = DateTime.Now.ToString("yyyy/MM/dd");
                sHora = DateTime.Now.ToString("HH:mm");

                sSql = "";
                sSql += "select top 1 CC.id_ctt_jornada, CC.id_ctt_oficinista, CC.estado_cierre_caja, " + Environment.NewLine;
                sSql += "J.secuencia, CC.fecha_apertura, CC.id_ctt_cierre_caja, CC.hora_apertura," + Environment.NewLine;
                sSql += "O.descripcion oficinista, ltrim(str(isnull(CC.saldo_inicial, 0), 10, 2))saldo_inicial," + Environment.NewLine;
                sSql += "isnull(CC.observaciones, '') observaciones, J.descripcion jornada" + Environment.NewLine;
                sSql += "from ctt_cierre_caja CC INNER JOIN" + Environment.NewLine;
                sSql += "ctt_jornada J ON  J.id_ctt_jornada = CC.id_ctt_jornada" + Environment.NewLine;
                sSql += "and J.estado = 'A'" + Environment.NewLine;
                sSql += "and CC.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "ctt_oficinista O ON O.id_ctt_oficinista = CC.id_ctt_oficinista" + Environment.NewLine;
                sSql += "and O.estado = 'A'" + Environment.NewLine;
                sSql += "where CC.id_ctt_pueblo = " + Session["id_pueblo"].ToString().Trim() + Environment.NewLine;
                sSql += "order by CC.id_ctt_cierre_caja desc";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', '" + sSql + "', 'danger');", true);
                    return false;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    Session["idCierreCaja"] = "0";
                    Session["fechaApertura"] = sFecha;
                    Session["horaApertura"] = sHora;
                    Session["usuarioApertura"] = Session["usuario"].ToString();
                    Session["idJornadaApertura"] = "0";
                    Session["JornadaApertura"] = "";
                    Session["saldoInicialApertura"] = "0.00";
                    Session["observacionesApertura"] = "";
                    Session["nombreJornada"] = "";
                    return true;
                }

                sEstadoCaja = dtConsulta.Rows[0]["estado_cierre_caja"].ToString().Trim().ToUpper();
                sFechaApertura = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_apertura"].ToString().Trim()).ToString("dd/MM/yyyy");
                sHoraApertura = dtConsulta.Rows[0]["hora_apertura"].ToString().Trim();
                sUsuarioApertura = dtConsulta.Rows[0]["oficinista"].ToString().Trim().ToUpper();
                sSaldoInicial = dtConsulta.Rows[0]["saldo_inicial"].ToString().Trim();
                sObervaciones = dtConsulta.Rows[0]["observaciones"].ToString().Trim().ToUpper();
                sJornadaApertura = dtConsulta.Rows[0]["jornada"].ToString().Trim().ToUpper();
                iIdCierreCajaApertura = Convert.ToInt32(dtConsulta.Rows[0]["id_ctt_cierre_caja"].ToString().Trim());
                iIdJornadaApertura = Convert.ToInt32(dtConsulta.Rows[0]["id_ctt_jornada"].ToString().Trim());

                fechaCaja = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_apertura"].ToString());
                fechaSistema = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

                Session["idJornadaConsulta"] = dtConsulta.Rows[0]["id_ctt_jornada"].ToString().Trim();
                Session["idCierreCajeroConsulta"] = dtConsulta.Rows[0]["id_ctt_cierre_caja"].ToString().Trim();

                if (sEstadoCaja == "CERRADA")
                {                    
                    Session["idCierreCaja"] = "0";
                    Session["fechaApertura"] = sFecha;
                    Session["horaApertura"] = sHora;
                    Session["usuarioApertura"] = Session["usuario"].ToString();
                    Session["idJornadaApertura"] = "0";
                    Session["JornadaApertura"] = "";
                    Session["saldoInicialApertura"] = "0.00";
                    Session["observacionesApertura"] = "";
                    Session["nombreJornada"] = "";
                }

                else
                {
                    Session["idCierreCaja"] = iIdCierreCajaApertura;
                    Session["idJornadaApertura"] = iIdJornadaApertura.ToString();
                    Session["idJornada"] = iIdJornadaApertura.ToString();

                    Session["idCierreCaja"] = dtConsulta.Rows[0]["id_ctt_cierre_caja"].ToString();
                    Session["fechaApertura"] = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_apertura"].ToString()).ToString("dd/MM/yyyy");
                    Session["horaApertura"] = Convert.ToDateTime(dtConsulta.Rows[0]["hora_apertura"].ToString()).ToString("HH:mm");
                    Session["usuarioApertura"] = dtConsulta.Rows[0]["oficinista"].ToString();
                    Session["idJornadaApertura"] = dtConsulta.Rows[0]["id_ctt_jornada"].ToString();
                    Session["JornadaApertura"] = dtConsulta.Rows[0]["jornada"].ToString();
                    Session["saldoInicialApertura"] = dtConsulta.Rows[0]["saldo_inicial"].ToString();
                    Session["observacionesApertura"] = dtConsulta.Rows[0]["observaciones"].ToString();
                    Session["nombreJornada"] = dtConsulta.Rows[0]["jornada"].ToString();
                    Session["idJornada"] = dtConsulta.Rows[0]["id_ctt_jornada"].ToString();
                }

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        //VARIABLES FACTURACION ELECTRONICA
        public bool GetVariablesFE()
        {
            try
            {
                sSql = "";
                sSql += "select id_tipo_emision, id_tipo_ambiente, id_tipo_certificado_digital, numeroruc from sis_empresa" + Environment.NewLine;
                sSql += "where idempresa =" + Application["idEmpresa"].ToString() + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    Application["IDTipoEmisionFE"] = dtConsulta.Rows[0].ItemArray[0].ToString();
                    Application["IDTipoAmbienteFE"] = dtConsulta.Rows[0].ItemArray[1].ToString();
                    Application["IDTipoCDFE"] = dtConsulta.Rows[0].ItemArray[2].ToString();
                    Application["NumeroRUC"] = dtConsulta.Rows[0].ItemArray[3].ToString();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'Error al obtener Vaiables Facturacion Electroónica', 'error')</script>");
                    return false;
                }

                //SERIE Y PUNTO DE EMISION
                sSql = "";
                sSql += "select establecimiento, punto_emision, emite_comprobante_electronico from tp_localidades" + Environment.NewLine;
                sSql += "where idempresa =" + Application["idEmpresa"].ToString() + Environment.NewLine;
                sSql += " and id_localidad =" + Application["idLocalidad"].ToString();

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    Application["Establecimiento"] = dtConsulta.Rows[0].ItemArray[0].ToString();
                    Application["PuntoEmision"] = dtConsulta.Rows[0].ItemArray[1].ToString();
                    Application["EmiteComprobanteElectronico"] = dtConsulta.Rows[0].ItemArray[2].ToString();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'Error al obtener Vaiables Facturacion Electroónica', 'error')</script>");
                    return false;
                }
                //FIN SERIE Y PUNTO DE EMISION

                //URL DEL SERVICIO WEB
                sSql = "";
                sSql += "select wsdl_pruebas, url_pruebas, wsdl_produccion, url_produccion" + Environment.NewLine;
                sSql += "from cel_parametro" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Application["WSEnvioPruebas"] = dtConsulta.Rows[0][0].ToString();
                        Application["WSConsultaPruebas"] = dtConsulta.Rows[0][1].ToString();
                        Application["WSEnvioProduccion"] = dtConsulta.Rows[0][2].ToString();
                        Application["WSConsultaProduccion"] = dtConsulta.Rows[0][3].ToString();
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'Error al obtener Vaiables Facturacion Electrónica', 'error')</script>");
                    return false;
                }
                //FIN URL DEL SERVICIO WEB

                return true;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "mensaje", "<script>swal('Error.!', 'Error al obtener Vaiables Facturacion Electroónica', 'error')</script>");
                return false;
            }
        }

        #endregion

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            consultarUsuario();
        }
    }
}