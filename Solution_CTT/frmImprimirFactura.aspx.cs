using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NEGOCIO;
using Microsoft.Reporting.WebForms;
using BarcodeLib;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Drawing.Imaging;

namespace Solution_CTT
{
    public partial class frmImprimirFactura : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sNumeroFactura;
        string sAsientos;
        string sTasaUsuarioRecuperado;

        bool bRespuesta;

        DataTable dtConsulta;

        int iVendidos;
        int iCuenta;

        decimal dbCantidad;
        decimal dbPrecioUnitario;
        decimal dbDescuento;
        decimal dbIva;
        decimal dbSumaTotal;

        Byte[] Logo { get; set; }

        //VARIABLES PARA IMPRIMIR
        int m_currentPageIndex;
        private List<Stream> m_streams;
        private List<string> m_files;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        //FUNCION PARA ABRIR UNA NUEVA PESTAÑA
        private void abreVentana(string ventana)
        {
            string Clientscript = "<script>window.open('" +
                                  ventana +
                                  "')</script>";

            if (!this.IsStartupScriptRegistered("WOpen"))
            {
                this.RegisterStartupScript("WOpen", Clientscript);
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

        #region FUNCIONES DE IMPRESION

        //CREAR CREATESTREAM
        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            //string vFileName;
            //name += DateTime.Now.ToFileTimeUtc().ToString();

            //vFileName = Request.MapPath(@"Temp\") + name + "." + fileNameExtension;
            //Stream stream = new FileStream(vFileName, FileMode.Create);
            //m_files.Add(vFileName);
            //m_streams.Add(stream);
            //return stream;

            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        //FUNCION DE EXPORTAR
        private void Exportar(LocalReport reporte_P)
        {
            string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>8cm</PageWidth>
                <PageHeight>20cm</PageHeight>
                <MarginTop>0cm</MarginTop>
                <MarginLeft>0cm</MarginLeft>
                <MarginRight>0cm</MarginRight>
                <MarginBottom>0cm</MarginBottom>
            </DeviceInfo>";

            Warning[] warnings;
            m_streams = new List<Stream>();
            reporte_P.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
            { stream.Position = 0; }

            //Warning[] warnings;
            //m_streams = new List<Stream>();
            ////m_files = new List<string>();

            //reporte_P.Render("Image", deviceInfo, CreateStream, out warnings);

            //foreach (Stream stream in m_streams)
            //{ stream.Position = 0; }
        }

        // Handler para los eventos PrintPageEvents
        private void ImprimirPagina(object sender, PrintPageEventArgs ev)
        {
            //Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);
            //ev.Graphics.DrawImage(pageImage, ev.PageBounds);
            //m_currentPageIndex += 1;
            //ev.HasMorePages = (m_currentPageIndex < m_streams.Count);

            Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);

            //Crea un marco que contiene el documento (No es necesario)
            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Dibuja un fondo blanco para el report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Dibuja el contenido del report
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Lo prepara para la siguiente página y comprueba que no llegado al final
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        private void imprimir()
        {
            PrintDocument printDoc;
            //busca el nombre de la impresora predeterminada
            //String printerName = ImpresoraPredeterminada();
            String printerName = "EPSON TM-T20II Receipt";

            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: No hay datos que imprimir.");

            printDoc = new PrintDocument();
            printDoc.PrinterSettings.PrinterName = printerName;
            
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception(String.Format("No puedo encontrar la impresora \"{0}\".", printerName));
            }

            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(ImprimirPagina);
                //m_currentPageIndex = 0;
                printDoc.Print();
            }

            foreach(Stream stream in m_streams)
            {
                stream.Close();
                stream.Dispose();
            }

            foreach(String vFile in m_files)
            {
                File.Delete(vFile);
            }
        }


        //FUNCION PARA IMPRIMIR DIRECTAMENTE EL REPORT VIEWER
        private void crearReporteImprimir()
        {
            try
            {
                sSql = "";
                sSql += "select establecimiento, punto_emision, numero_factura, fecha_ingreso," + Environment.NewLine;
                sSql += "identificacion, isnull(nombres, '') + ' ' + apellidos cliente, descripcion_ruta," + Environment.NewLine;
                sSql += "tipo_viaje, fecha_viaje, hora_salida, descripcion_anden, descripcion_disco," + Environment.NewLine;
                sSql += "tasa_usuario, cantidad, precio_unitario, valor_dscto, valor_iva, clave_acceso," + Environment.NewLine;
                sSql += "oficinista, numero_asiento, '' as valor_total, '' as vendidos, '' as asientos, '' as secuencia_factura" + Environment.NewLine;
                sSql += "from ctt_vw_factura" + Environment.NewLine;
                sSql += "where id_pedido = " + Convert.ToInt32(txtIdPedido.Text.Trim());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sNumeroFactura = dtConsulta.Rows[0][0].ToString() + "-" + dtConsulta.Rows[0][1].ToString() + "-" + dtConsulta.Rows[0][2].ToString().PadLeft(9, '0');
                        iVendidos = dtConsulta.Rows.Count;

                        if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                        {
                            sTasaUsuarioRecuperado = dtConsulta.Rows[0][12].ToString();
                            Logo = barcode(sTasaUsuarioRecuperado);
                        }

                        sAsientos = "";
                        dbSumaTotal = 0;

                        //RECORRER LOS ASIENTOS Y SUMAR TOTAL
                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            sAsientos += dtConsulta.Rows[i]["numero_asiento"].ToString().Trim();

                            if (i + 1 != dtConsulta.Rows.Count)
                            {
                                sAsientos += " - ";
                            }

                            dbCantidad = Convert.ToDecimal(dtConsulta.Rows[i]["cantidad"].ToString());
                            dbPrecioUnitario = Convert.ToDecimal(dtConsulta.Rows[i]["precio_unitario"].ToString());
                            dbDescuento = Convert.ToDecimal(dtConsulta.Rows[i]["valor_dscto"].ToString());
                            dbIva = Convert.ToDecimal(dtConsulta.Rows[i]["valor_iva"].ToString());

                            dbSumaTotal += dbCantidad * (dbPrecioUnitario - dbDescuento + dbIva);
                        }

                        DataColumn imagen = new DataColumn("tasa_generada");
                        imagen.DataType = System.Type.GetType("System.Byte[]");
                        dtConsulta.Columns.Add(imagen);

                        //RECORRER EL DATATABLE PARA LLENAR DE DATOS
                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            dtConsulta.Rows[i]["tasa_generada"] = Logo;
                            dtConsulta.Rows[i]["valor_total"] = dbSumaTotal.ToString("N2");
                            dtConsulta.Rows[i]["vendidos"] = iVendidos.ToString();
                            dtConsulta.Rows[i]["asientos"] = sAsientos.Trim();
                            dtConsulta.Rows[i]["secuencia_factura"] = sNumeroFactura;
                        }

                        DSReportes ds = new DSReportes();

                        DataTable dt = ds.Tables["dtFactura"];
                        dt.Clear();

                        dt = dtConsulta;

                        //AGREGAR EL DETALLE DE BOLETOS VENDIDOS
                        sSql = "";
                        sSql += "select tipo_cliente, count(*) cuenta" + Environment.NewLine;
                        sSql += "from ctt_vw_factura" + Environment.NewLine;
                        sSql += "where id_pedido = " + Convert.ToInt32(txtIdPedido.Text.Trim());
                        sSql += "group by tipo_cliente";

                        DataTable dt2 = ds.Tables["dtTarifas"];
                        dt2.Clear();

                        bRespuesta = conexionM.consultarRegistro(sSql, dt2);

                        if (bRespuesta == true)
                        {
                            LocalReport reporteLocal = new LocalReport();

                            if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                            {
                                reporteLocal.ReportPath = Server.MapPath("~/Reportes/rptFactura.rdlc");
                            }

                            else
                            {
                                reporteLocal.ReportPath = Server.MapPath("~/Reportes/rptFactura_2.rdlc");
                            }

                            ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
                            ReportDataSource datasource2 = new ReportDataSource("DataSet2", dt2);
                            reporteLocal.DataSources.Add(datasource);
                            reporteLocal.DataSources.Add(datasource2);

                            Exportar(reporteLocal);

                            m_currentPageIndex = 0;

                            imprimir();
                        }
                    }
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo crear el reporte. Comuníquese con el administrador.', 'danger');", true);
                }   
            }

            catch (Exception ex)
            {

            }
        }

        #endregion

        protected void btnVer_Click(object sender, EventArgs e)
        {
            crearReporteImprimir();
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            sSql = "";
            sSql += "select establecimiento, punto_emision, numero_factura, fecha_ingreso," + Environment.NewLine;
            sSql += "identificacion, isnull(nombres, '') + ' ' + apellidos cliente, descripcion_ruta," + Environment.NewLine;
            sSql += "tipo_viaje, fecha_viaje, hora_salida, descripcion_anden, descripcion_disco," + Environment.NewLine;
            sSql += "tasa_usuario, cantidad, precio_unitario, valor_dscto, valor_iva, clave_acceso," + Environment.NewLine;
            sSql += "oficinista, numero_asiento, '' as valor_total, '' as vendidos, '' as asientos, '' as secuencia_factura" + Environment.NewLine;
            sSql += "from ctt_vw_factura" + Environment.NewLine;
            sSql += "where id_pedido = " + Convert.ToInt32(txtIdPedido.Text.Trim());

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    sNumeroFactura = dtConsulta.Rows[0][0].ToString() + "-" + dtConsulta.Rows[0][1].ToString() + "-" + dtConsulta.Rows[0][2].ToString().PadLeft(9, '0');
                    iVendidos = dtConsulta.Rows.Count;

                    if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                    {
                        sTasaUsuarioRecuperado = dtConsulta.Rows[0][12].ToString();
                        Logo = barcode(sTasaUsuarioRecuperado);
                    }

                    sAsientos = "";
                    dbSumaTotal = 0;

                    //RECORRER LOS ASIENTOS Y SUMAR TOTAL
                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        sAsientos += dtConsulta.Rows[i]["numero_asiento"].ToString().Trim();

                        if (i + 1 != dtConsulta.Rows.Count)
                        {
                            sAsientos += " - ";
                        }

                        dbCantidad = Convert.ToDecimal(dtConsulta.Rows[i]["cantidad"].ToString());
                        dbPrecioUnitario = Convert.ToDecimal(dtConsulta.Rows[i]["precio_unitario"].ToString());
                        dbDescuento = Convert.ToDecimal(dtConsulta.Rows[i]["valor_dscto"].ToString());
                        dbIva = Convert.ToDecimal(dtConsulta.Rows[i]["valor_iva"].ToString());

                        dbSumaTotal += dbCantidad * (dbPrecioUnitario - dbDescuento + dbIva);
                    }

                    DataColumn imagen = new DataColumn("tasa_generada");
                    imagen.DataType = System.Type.GetType("System.Byte[]");
                    dtConsulta.Columns.Add(imagen);

                    //RECORRER EL DATATABLE PARA LLENAR DE DATOS
                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        dtConsulta.Rows[i]["tasa_generada"] = Logo;
                        dtConsulta.Rows[i]["valor_total"] = dbSumaTotal.ToString("N2");
                        dtConsulta.Rows[i]["vendidos"] = iVendidos.ToString();
                        dtConsulta.Rows[i]["asientos"] = sAsientos.Trim();
                        dtConsulta.Rows[i]["secuencia_factura"] = sNumeroFactura;
                    }                    

                    DSReportes ds = new DSReportes();

                    DataTable dt = ds.Tables["dtFactura"];
                    dt.Clear();

                    dt = dtConsulta;

                    //AGREGAR EL DETALLE DE BOLETOS VENDIDOS
                    sSql = "";
                    sSql += "select tipo_cliente, count(*) cuenta" + Environment.NewLine;
                    sSql += "from ctt_vw_factura" + Environment.NewLine;
                    sSql += "where id_pedido = " + Convert.ToInt32(txtIdPedido.Text.Trim());
                    sSql += "group by tipo_cliente";

                    DataTable dt2 = ds.Tables["dtTarifas"];
                    dt2.Clear();

                    bRespuesta = conexionM.consultarRegistro(sSql, dt2);

                    if (bRespuesta == true)
                    {
                        rptFactura.ProcessingMode = ProcessingMode.Local;

                        if (Convert.ToInt32(Session["genera_tasa_usuario"].ToString()) == 1)
                        {
                            rptFactura.LocalReport.ReportPath = Server.MapPath("~/Reportes/rptFactura.rdlc");
                        }

                        else
                        {
                            rptFactura.LocalReport.ReportPath = Server.MapPath("~/Reportes/rptFactura_2.rdlc");
                        }

                        rptFactura.LocalReport.DataSources.Clear();
                        ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
                        ReportDataSource datasource2 = new ReportDataSource("DataSet2", dt2);
                        rptFactura.LocalReport.DataSources.Add(datasource);
                        rptFactura.LocalReport.DataSources.Add(datasource2);
                        rptFactura.LocalReport.Refresh();
                    }
                }
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "swal('Error.!', 'No se pudo crear el reporte. Comuníquese con el administrador.', 'danger');", true);
            }            
        }


        protected void btnManifiesto_Click(object sender, EventArgs e)
        {
            Clases.ClaseImprimirManifiesto imp = new Clases.ClaseImprimirManifiesto();
            imp.llenarReporte(Convert.ToInt32(txtVer.Text.Trim()), "ELVIS", 1, 0, 0, 0);
        }

        protected void btnCierreCaja_Click(object sender, EventArgs e)
        {
            dtConsulta = new DataTable();
            dtConsulta.Clear();
            Clases.ClaseCierreBoleteria imp = new Clases.ClaseCierreBoleteria();
            imp.llenarReporte(Convert.ToDateTime(TxtFechaViaje.Text.Trim()).ToString("yyyy/MM/dd"), 1, Session["nombreJornada"].ToString(), Session["usuario"].ToString(), dtConsulta, Convert.ToInt32(Session["idUsuario"].ToString()), 1);
        }

        protected void btnImprimirSoloTasa_Click(object sender, EventArgs e)
        {
            string[] sDatosMaximo= new string[5];
            sDatosMaximo[0] = Session["usuario"].ToString();
            sDatosMaximo[1] = Environment.MachineName.ToString();
            sDatosMaximo[2] = "A";

            Clases.ClaseCrearTasaUsuario tasa = new Clases.ClaseCrearTasaUsuario();
            tasa.tasa(txtVer.Text.Trim(), sDatosMaximo);
        }
    }
}