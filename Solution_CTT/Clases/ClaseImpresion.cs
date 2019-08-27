using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace Solution_CTT.Clases
{
    public class ClaseImpresion
    {
        public StringBuilder linea = new StringBuilder();
        StringBuilder linea1 = new StringBuilder();
        StringBuilder linea2 = new StringBuilder();
        int maxCar = 40, cortar;

        /*
         * PRIMERA VERSION DE IMPRESIONES EN TEXTO PLANO PARA TICKETERAS
         * AUTOR: ELVIS GUAIGUA
         * VERSION 1.0
         * FECHA DE CREACION: 2018/07/14
         * PROCESO:
         * 1. SI CREARÁ UNA FUNCION PARA INICIAR UNA SECUENCIA DE ESCAPE (ESC/POS)
         * 2. EL ARCHIVO PARA UTILIZACION SE UTILIZARÁN CIERTAS FASES.
         *    PRIMERA FASE: CREA EL ARCHIVO CON UN ESPACIADO 1/8. ESTE SI NO SE AGREGARÁ MAS INFORMACION, SE PROCEDERA A IMPRIMIR.
         *    SEGUNDA FASE: SI SE NECESITA AUMENTAR EL TAMAÑO DE FUENTE, SE CREARÁN SECUENCIAS PARA AUMENTAR LA FUENTE Y LUEGO SE LAS ANULA.
         */

        //METODO PARA INICIAR
        public void iniciarImpresion()
        {
            linea.Clear();
            //linea.Append("" + (char)27 + (char)116 + (char)2);            //SELECCIONA TIPO DE CARACTERES 
            //linea.Append("" + (char)27 + (char)40 + (char)116 + (char)51 + (char)48 + (char)51 + (char)48);   //SELECCIONA TIPO DE CARACTERES 
            linea.Append("" + (char)27 + (char)116 + (char)16);
        }

        //METODO PARA ESCRIBIR UNA SECUENCIA CON ESPACIADO 1/8 Y LETRA DEFAULT
        public void escritoEspaciadoCorto(string sLinea)
        {
            linea.Append("" + (char)27 + (char)51 + (char)18);
            linea.Append(sLinea);
        }

        //METODO PARA ESCRIBIR UNA SECUENCIA EN NEGRITA CON LETRA DE FUENTE DOBLE
        public void escritoFuenteAlta(string sLinea)
        {
            linea.AppendLine("" + (char)27 + (char)33 + (char)20); //AUMENTA TAMAÑO DE FUENTE
            linea.AppendLine(sLinea);
            linea.Append("" + (char)27 + (char)64);  //CANCELA EL TAMAÑO DE FUENTE
        }

        public void iniciarImpresionCocina()
        {
            linea.Clear();
            linea.AppendLine("" + (char)27 + (char)116 + (char)2);          //SELECCIONA TIPO DE CARACTERES 
        }

        //METODO PARA ESCRIBIR UNA SECUENCIA CON ESPACIADO 1/8 Y LETRA DEFAULT
        public void escritoEspaciadoCortoCocina(string sLinea)
        {
            linea.AppendLine("" + (char)27 + (char)51 + (char)18);
            linea.AppendLine(sLinea);
        }

        //METODO PARA ESCRIBIR UNA SECUENCIA EN NEGRITA CON LETRA DE FUENTE DOBLE
        public void escritoFuenteAltaCocina(string sLinea)
        {
            linea.AppendLine("" + (char)27 + (char)33 + (char)20); //AUMENTA TAMAÑO DE FUENTE
            linea.AppendLine(sLinea);
            linea.AppendLine("" + (char)27 + (char)33 + (char)0);  //CANCELA EL TAMAÑO DE FUENTE
        }

        //METODO PARA ESCRIBIR UNA SECUENCIA DE CIERRE 
        public void cerrarImpresion()
        {
            linea.Append("" + (char)29 + (char)86 + (char)66 + (char)0);
            linea.Append("" + (char)27 + (char)112 + (char)0 + (char)60 + (char)120);
        }

        //METODO PARA CORTAR EL PAPEL
        public void cortarPapel(int iOp)
        {
            linea.Append("\x1B" + "d" + "\x09"); //avanza 9 renglones

            //EN ESTA SECCION CONTROLAMOS QUE NO CORTE EL PAPEL EN EL FACTURADOR
            if (iOp == 1)
            {
                linea.Append("" + (char)27 + (char)105); //  //CORTAR PAPEL
            }
        }

        //FUNCION PARA ABRIR EL CAJON
        public void AbreCajon()
        {
            linea.Append("" + (char)27 + (char)112 + (char)0 + (char)5 + (char)50);
            //linea.Append("" + (char)27 + (char)100 + (char)1); //ESTE CODIGO VERIFICAR
        }

        //FUNCION PARA IMPRIMIR
        public void imprimirReporte(string sImpresora)
        {
            RawPrinterHelper.SendStringToPrinter(sImpresora, linea.ToString());
        }

        //CLASE PARA ENVIAR A IMPRIMIR TEXTO PLANO
        public class RawPrinterHelper
        {
            // Structure and API declarions:
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public class DOCINFOA
            {
                [MarshalAs(UnmanagedType.LPStr)]
                public string pDocName;
                [MarshalAs(UnmanagedType.LPStr)]
                public string pOutputFile;
                [MarshalAs(UnmanagedType.LPStr)]
                public string pDataType;
            }
            [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

            [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool ClosePrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

            [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool EndDocPrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool StartPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool EndPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

            // SendBytesToPrinter()
            // When the function is given a printer name and an unmanaged array
            // of bytes, the function sends those bytes to the print queue.
            // Returns true on success, false on failure.
            public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
            {
                Int32 dwError = 0, dwWritten = 0;
                IntPtr hPrinter = new IntPtr(0);
                DOCINFOA di = new DOCINFOA();
                bool bSuccess = false; // Assume failure unless you specifically succeed.

                di.pDocName = "REPORTE";
                di.pDataType = "RAW";

                // Open the printer.
                if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
                {
                    // Start a document.
                    if (StartDocPrinter(hPrinter, 1, di))
                    {
                        // Start a page.
                        if (StartPagePrinter(hPrinter))
                        {
                            // Write your bytes.
                            bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                            EndPagePrinter(hPrinter);
                        }
                        EndDocPrinter(hPrinter);
                    }
                    ClosePrinter(hPrinter);
                }
                // If you did not succeed, GetLastError may give more information
                // about why not.
                if (bSuccess == false)
                {
                    dwError = Marshal.GetLastWin32Error();
                }
                return bSuccess;
            }

            public static bool SendStringToPrinter(string szPrinterName, string szString)
            {
                IntPtr pBytes;
                Int32 dwCount;
                // How many characters are in the string?
                dwCount = szString.Length;
                // Assume that the printer is expecting ANSI text, and then convert
                // the string to ANSI text.
                pBytes = Marshal.StringToCoTaskMemAnsi(szString);
                // Send the converted ANSI string to the printer.
                SendBytesToPrinter(szPrinterName, pBytes, dwCount);
                Marshal.FreeCoTaskMem(pBytes);
                return true;
            }
        }
    }
}