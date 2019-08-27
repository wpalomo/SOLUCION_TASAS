using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Data;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Reporting.WebForms;
using NEGOCIO;
using System.Web;

namespace Solution_CTT.Clases
{
    public class Impresor : IDisposable
    {
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;

        DataTable dtConsulta;

        bool bRespuesta;

        private int m_currentPageIndex;
        private IList<Stream> m_streams;
        private List<string> m_files;

        // Creamos el stream con el que vamos a trabajar y en el que meteremos el report
        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }
        
        // Exporta el report indicado a un archivo EMF (Enhanced Metafile).
        private void Export(LocalReport report)
        {
            string deviceInfo =
              @"<DeviceInfo>
              <OutputFormat>EMF</OutputFormat>
              <PageWidth>7.5cm</PageWidth>
              <PageHeight>16cm</PageHeight>
              <MarginTop>0cm</MarginTop>
              <MarginLeft>0cm</MarginLeft>
              <MarginRight>0cm</MarginRight>
              <MarginBottom>0cm</MarginBottom>
            </DeviceInfo>";

//            string deviceInfo =
//                @"<DeviceInfo>
//                <OutputFormat>PNG</OutputFormat>
//                <PageWidth>8cm</PageWidth>
//                <PageHeight>20cm</PageHeight>
//                <MarginTop>0cm</MarginTop>
//                <MarginLeft>0cm</MarginLeft>
//                <MarginRight>0cm</MarginRight>
//                <MarginBottom>0cm</MarginBottom>
//            </DeviceInfo>";

            Warning[] warnings;
            m_streams = new List<Stream>();
            //m_files = new List<string>();

            report.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
            { stream.Position = 0; }
        }

        // Handler para los eventos PrintPageEvents
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);

            //ev.Graphics.DrawImage(pageImage, ev.PageBounds);
            //m_currentPageIndex += 1;
            //ev.HasMorePages = (m_currentPageIndex < m_streams.Count);

            //Crea un marco que contiene el documento (No es necesario)
            //Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            //Rectangle adjustedRect = new Rectangle(0, 0, 8, 16);

            // Dibuja un fondo blanco para el report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Dibuja el contenido del report
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Lo prepara para la siguiente página y comprueba que no llegado al final
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        private void Print()
        {
            PrintDocument printDoc;

            //String printerName = ImpresoraPredeterminada();
            //String printerName = "EPSON TM-T20II Receipt";
            String printerName = consultarImpresora();

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
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }

            //foreach (Stream stream in m_streams)
            //{
            //    stream.Close();
            //    stream.Dispose();
            //}

            //foreach(String vFile in m_files)
            //{
            //    File.Delete(vFile);
            //}
        }

        //CONUSLTAR IMPRESORA
        private string consultarImpresora()
        {
            try
            {
                sSql = "";
                sSql += "select descripcion" + Environment.NewLine;
                sSql += "from ctt_impresora" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and id_localidad = " + Convert.ToInt32(HttpContext.Current.Application["idLocalidad"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == true)
                {
                    return dtConsulta.Rows[0]["descripcion"].ToString();
                }

                else
                {
                    return "";
                }
            }

            catch (Exception)
            {
                return "";
            }
        }

        private string ImpresoraPredeterminada()
        {
            for (Int32 i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                PrinterSettings a = new PrinterSettings();
                a.PrinterName = PrinterSettings.InstalledPrinters[i].ToString();
                if (a.IsDefaultPrinter)
                { return PrinterSettings.InstalledPrinters[i].ToString(); }
            }
            return "";
        }

        // Exporta el report a un archivo .emf y lo imprime
        public void Imprime(LocalReport rdlc)
        {
            Export(rdlc);
            //m_currentPageIndex = 0;
            Print();
        }

        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }
    }
}
