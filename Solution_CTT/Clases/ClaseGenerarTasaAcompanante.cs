using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using NEGOCIO;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Drawing;
using Microsoft.Reporting.WebForms;

namespace Solution_CTT.Clases
{
    public class ClaseGenerarTasaAcompanante
    {
        manejadorConexion conexionM = new manejadorConexion();

        string sObjetoOficina;
        string sObjetoTasa;
        string sObjetoInfo;
        string sObjetoCliente;
        string sObjetoJson;
        string sUrlCredenciales;
        string sUrlEnvio;
        string sUrlAnula;
        string sSql;
        string sTipoClienteTasa;
        string sToken;
        string sCuentaToken;
        string sIdentificacion;
        string sTasaOficina;
        string sTasaCooperativa;
        string sTasaTerminal;
        string sTasaUsuario;
        string sServidorPruebas;
        string sServidorProduccion;
        string sUrlTasaUsuario;
        string sUrlTasaAnulacion;
        string sTipoEmision;
        string sValorTasaUsuario;
        string sIdTasaRespuesta;
        string sTabla;
        string sCampo;
        string sFecha;

        string[] sDatosMaximo;

        Byte[] Logo { get; set; }

        DataTable dtConsulta;

        bool bRespuesta;

        int iNumeroMovimientoCaja;
        int iIdMovimientoCaja;

        decimal dbValorTasa;

        long iMaximo;

        public bool tasa(string sCantidad, string sIdOrigen, string sOrigen, string sIdDestino, string sDestino, string sFechaViaje, string sHoraViaje, string[] sDatos)
        {
            try
            {
                sDatosMaximo = sDatos;

                //CONSULTAR PARAMETROS DE LA TASA DE USUARIO
                if (consultarParametrosTasa() == false)
                {
                    return false;
                }

                //RECUPERAR INFORMACION DEL TOKEN
                if (consultarToken() == false)
                {
                    return false;
                }

                //GENERAR LA TASA DE USUARIO
                if (generarTasaUsuario(sCantidad) == false)
                {
                    return false;
                }

                sTipoClienteTasa = "07";
                sIdentificacion = "9999999999999";

                sObjetoJson = "";
                sObjetoJson += "{" + Environment.NewLine;

                sObjetoOficina = "";
                sObjetoOficina += "\"oficina\": {" + Environment.NewLine;
                sObjetoOficina += "\"id_oficina\": \"" + sTasaOficina + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_coop\": \"" + sTasaCooperativa + "\"," + Environment.NewLine;
                sObjetoOficina += "\"id_terminal\": \"" + sTasaTerminal + "\"" + Environment.NewLine;
                sObjetoOficina += "},";

                sObjetoJson += sObjetoOficina + Environment.NewLine;

                sObjetoTasa = "";
                sObjetoTasa += "\"tasa\": {" + Environment.NewLine;
                sObjetoTasa += "\"cantidad\": \"" + sCantidad.Trim().PadLeft(2, '0') + "\"," + Environment.NewLine;
                sObjetoTasa += "\"secuencial\": \"" + sCuentaToken.Trim() + "\"," + Environment.NewLine;
                sObjetoTasa += "\"token\": \"" + sToken.Trim() + "\"," + Environment.NewLine;
                sObjetoTasa += "\"tipo\": \"1\"," + Environment.NewLine;
                sObjetoTasa += "\"codigo\": \"" + sTasaUsuario + "\"" + Environment.NewLine;
                sObjetoTasa += "},";

                sObjetoJson += sObjetoTasa + Environment.NewLine;

                sObjetoInfo = "";
                sObjetoInfo += "\"info\": {" + Environment.NewLine;
                sObjetoInfo += "\"id_inicio\": \"" + sIdOrigen + "\"," + Environment.NewLine;
                sObjetoInfo += "\"id_destino\": \"" + sIdDestino + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_inicio\": \"" + sOrigen + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_destino\": \"" + sDestino + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_horaSalida\": \"" + sHoraViaje + "\"," + Environment.NewLine;
                sObjetoInfo += "\"str_fechaSalida\": \"" + Convert.ToDateTime(sFecha).ToString("yyyy-MM-dd") + "\"," + Environment.NewLine;
                sObjetoInfo += "\"pasajeros\": \"" + sCantidad.Trim() + "\"" + Environment.NewLine;
                sObjetoInfo += "},";

                sObjetoJson += sObjetoInfo + Environment.NewLine;

                sObjetoCliente = "";
                sObjetoCliente += "\"cliente\": {" + Environment.NewLine;
                sObjetoCliente += "\"ruc\": \"" + sIdentificacion + "\"," + Environment.NewLine;
                sObjetoCliente += "\"nombre\": \"" + "CONSUMIDOR FINAL" + "\"," + Environment.NewLine;
                sObjetoCliente += "\"direccion\": \"" + sOrigen + "\"," + Environment.NewLine;
                sObjetoCliente += "\"correo\": \"" + "contabilidad@expressatenas.com" + "\"," + Environment.NewLine;
                sObjetoCliente += "\"telefono\": \"" + "9999999999" + "\"," + Environment.NewLine;
                sObjetoCliente += "\"tipo\": \"" + sTipoClienteTasa + "\"" + Environment.NewLine;
                sObjetoCliente += "}";

                sObjetoJson += sObjetoCliente + Environment.NewLine + "}";

                if (enviarJson() == "ERROR")
                {
                    return false;
                }

                //INICIA TRANSACCION SQL
                if (conexionM.iniciarTransaccion() == false)
                {
                    return false;
                }

                if (procesarBaseDatos(sCantidad) == false)
                {
                    conexionM.reversaTransaccion();
                    return false;
                }

                conexionM.terminaTransaccion();

                if (crearReporteImprimir() == false)
                {
                    return false;
                }

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        private bool consultarToken()
        {
            try
            {
                sSql = "";
                sSql += "select token, cuenta, id_ctt_tasa_token" + Environment.NewLine;
                sSql += "from ctt_tasa_token" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and estado_token = 'Abierta'" + Environment.NewLine;
                sSql += "and ambiente_token = " + Convert.ToInt32(sTipoEmision);

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    sToken = dtConsulta.Rows[0][0].ToString();
                    sCuentaToken = dtConsulta.Rows[0][1].ToString().Trim().PadLeft(4, '0');
                }

                else
                {
                    return false;
                }

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        //FUNCION PARA CONSULTAR LOS PARAMETROS DEL SERVIDOR
        private bool consultarParametrosTasa()
        {
            try
            {
                sSql = "select * from ctt_tasa_parametros";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {

                    sTasaTerminal = dtConsulta.Rows[0][1].ToString();
                    sTasaOficina = dtConsulta.Rows[0][2].ToString();
                    sTasaCooperativa = dtConsulta.Rows[0][3].ToString();
                    sServidorPruebas = dtConsulta.Rows[0][4].ToString();
                    sServidorProduccion = dtConsulta.Rows[0][5].ToString();
                    sUrlCredenciales = dtConsulta.Rows[0][6].ToString();
                    sUrlTasaUsuario = dtConsulta.Rows[0][7].ToString();
                    sUrlTasaAnulacion = dtConsulta.Rows[0][8].ToString();
                    sTipoEmision = dtConsulta.Rows[0][9].ToString();
                    sValorTasaUsuario = dtConsulta.Rows[0][10].ToString();

                    return true;
                }

                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        //FUNCION PARA ENVIAR EL JSON AL SERVIDOR PARA AUTORIZACION
        private string enviarJson()
        {
            try
            {
                string respuestaJson = "";

                if (sTipoEmision == "0")
                {
                    sUrlEnvio = sServidorPruebas + sUrlTasaUsuario;
                }

                else
                {
                    sUrlEnvio = sServidorProduccion + sUrlTasaUsuario;
                }

                //Llamar a funcion para aceptar los certificados de la URL
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                //Declara el objeto con el que haremos la llamada al servicio
                HttpWebRequest request = WebRequest.Create(sUrlEnvio) as HttpWebRequest;
                //Configurar las propiedad del objeto de llamada
                request.Method = "POST";
                request.ContentType = "application/json";

                //Serializar el objeto a enviar. Para esto uso la libreria Newtonsoft
                //string sb = JsonConvert.SerializeObject(sAyuda);
                string sb = sObjetoJson;

                //Convertir el objeto serializado a arreglo de byte
                Byte[] bt = Encoding.UTF8.GetBytes(sb);

                //Agregar el objeto Byte[] al request
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();

                //Hacer la llamada
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    //Leer el resultado de la llamada
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    respuestaJson = sr.ReadToEnd();
                }

                TasaUsuario tasa = JsonConvert.DeserializeObject<TasaUsuario>(respuestaJson);

                sIdTasaRespuesta = tasa.id_tasa;

                return "OK";
            }

            catch (Exception)
            {
                return "ERROR";
            }
        }

        //Funcion para aceptar los certificados de la URL
        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        //FUNCION PARA GENERAR LA TASA DE USUARIO
        private bool generarTasaUsuario(string sCantidad_P)
        {
            try
            {
                sTasaUsuario = "";
                sTasaUsuario += sCantidad_P.PadLeft(2, '0') + sTasaCooperativa.Trim();
                sTasaUsuario += sTasaOficina.Trim() + sToken.Trim() + sCuentaToken.Trim();
                sTasaUsuario += sTasaTerminal.Trim() + "1";

                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        //FUNCION PARA INSERTAR
        private bool procesarBaseDatos(string sCantidad_P)
        {
            try
            {
                sFecha = DateTime.Now.ToString("yyyy/MM/dd");

                //ACTUALIZAR LOS CONTADORES DE LA TABLA CTT_TASA_TOKEN
                sSql = "";
                sSql += "update ctt_tasa_token set" + Environment.NewLine;
                sSql += "cuenta = cuenta + 1," + Environment.NewLine;
                sSql += "emitidos = emitidos + 1" + Environment.NewLine;
                sSql += "where token = " + Convert.ToInt32(sToken) + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "and estado_token = 'Abierta'";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //PROCEDIMIENTO PARA EXTRAER EL NUMERO DE MOVIMIENTO
                sSql = "";
                sSql += "select numeromovimientocaja" + Environment.NewLine;
                sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and id_localidad = " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    iNumeroMovimientoCaja = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                }

                else
                {
                    return false;
                }

                //dbValorTasa = Convert.ToDecimal(txtTasaUsuario.Text.Trim()) * Convert.ToDecimal(Session["valor_tasa_usuario"].ToString());
                dbValorTasa = Convert.ToDecimal(sCantidad_P, System.Globalization.CultureInfo.InvariantCulture) * Convert.ToDecimal(sValorTasaUsuario, System.Globalization.CultureInfo.InvariantCulture);

                //INSERTAR EN LA TABLA CTT_MOVIMIENTO_CAJA
                sSql = "";
                sSql += "insert into ctt_movimiento_caja (" + Environment.NewLine;
                sSql += "tipo_movimiento, idempresa, id_localidad, id_factura, id_caja, id_ctt_jornada," + Environment.NewLine;
                sSql += "cg_moneda, fecha, hora, cantidad, valor, tasa_usuario, estado, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso, id_tasa_usuario)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "1, " + Convert.ToInt32(HttpContext.Current.Application["idEmpresa"].ToString()) + ", " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString()) + "," + Environment.NewLine;
                sSql += "0, 30, 1, " + Convert.ToInt32(HttpContext.Current.Application["cgMoneda"].ToString()) + ", '" + sFecha + "', GETDATE()," + Environment.NewLine;
                //sSql += Convert.ToInt32(txtTasaUsuario.Text.Trim()) + ", " + dbValorTasa + ", '" + sTasaUsuario + "'," + Environment.NewLine;
                sSql += Convert.ToInt32(sCantidad_P) + ", " + dbValorTasa.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", '" + sTasaUsuario + "'," + Environment.NewLine;
                sSql += "'A', GETDATE(), '" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "', '" + sIdTasaRespuesta + "')";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //PROCEDIMINTO PARA EXTRAER EL ID DE LA TABLA CTT_MOVIMIENTO_CAJA
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sTabla = "ctt_movimiento_caja";
                sCampo = "id_ctt_movimiento_caja";

                iMaximo = conexionM.sacarMaximo(sTabla, sCampo, "", sDatosMaximo);

                if (iMaximo == -1)
                {
                    return false;
                }

                else
                {
                    iIdMovimientoCaja = Convert.ToInt32(iMaximo);
                }

                //INSTRUCCION INSERTAR EN LA TABLA CTT_NUMERO_MOVIMIENTO_CAJA
                sSql = "";
                sSql += "insert into ctt_numero_movimiento_caja (" + Environment.NewLine;
                sSql += "id_ctt_movimiento_caja, numero_movimiento_caja, estado," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdMovimientoCaja + ", " + iNumeroMovimientoCaja + ", 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + sDatosMaximo[0] + "', '" + sDatosMaximo[1] + "')";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                //QUERY PARA ACTUALIZAR EL NUMERO DE MOVIMIENTO EN LA TABLA TP_LOCALIDADES_IMPRESORAS
                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numeromovimientocaja = numeromovimientocaja + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());

                //EJECUCION DE INSTRUCCION SQL
                if (!conexionM.ejecutarInstruccionSQL(sSql))
                {
                    return false;
                }

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        //FUNCION PARA IMPRIMIR DIRECTAMENTE EL REPORT VIEWER
        private bool crearReporteImprimir()
        {
            try
            {
                Logo = barcode(sTasaUsuario);

                DataTable dt = new DataTable();
                dt.Clear();

                DataColumn fecha = new DataColumn("fecha");
                fecha.DataType = System.Type.GetType("System.DateTime");
                dt.Columns.Add(fecha);

                DataColumn imagen = new DataColumn("tasa_generada");
                imagen.DataType = System.Type.GetType("System.Byte[]");
                dt.Columns.Add(imagen);

                DataRow row;
                row = dt.NewRow();
                row["fecha"] = DateTime.Now.ToString("yyyy/MM/dd");
                row["tasa_generada"] = Logo;
                dt.Rows.Add(row);

                DSReportes ds = new DSReportes();

                DataTable dtEnviar = ds.Tables["dtCodigoBarras"];
                dtEnviar.Clear();

                dtEnviar = dt;

                LocalReport reporteLocal = new LocalReport();
                reporteLocal.ReportPath = HttpContext.Current.Server.MapPath("~/Reportes/rptCodigoBarras.rdlc");
                ReportDataSource datasource = new ReportDataSource("DataSet1", dtEnviar);
                reporteLocal.DataSources.Add(datasource);

                Clases.Impresor imp = new Clases.Impresor();
                imp.Imprime(reporteLocal);

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        //FUNCION DE PRUEBA
        private byte[] barcode(string sTasa)
        {
            BarcodeLib.Barcode codigo = new BarcodeLib.Barcode();
            codigo.IncludeLabel = true;

            var ms = new MemoryStream();

            Bitmap imgOK = new Bitmap(codigo.Encode(BarcodeLib.TYPE.CODE128, sTasa.ToString(), Color.Black, Color.White, 500, 150));

            imgOK.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            return ms.ToArray();
        }
    }
}