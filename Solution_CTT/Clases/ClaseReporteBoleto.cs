using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using NEGOCIO;

namespace Solution_CTT.Clases
{
    public class ClaseReporteBoleto
    {
        manejadorConexion conexionM = new manejadorConexion();
        Clases.ClaseManejoCaracteres caracter = new ClaseManejoCaracteres();
        Clases.ClaseParametros parametros = new ClaseParametros();

        DataTable dtConsulta;
        DataTable dtTipos;
        DataTable dtEmpresa;

        string sSql;
        string sTexto;
        string sNombrePasajero;
        string sTipoCliente;

        int iIdPedido;
        int iIdFactura;

        bool bRespuesta;

        double dbCantidad;
        double dbPrecioUnitario;
        double dbDescuento;
        double dbTotal;

        //VARIABLES CLAVE DE ACCESO
        string sClaveAcceso;
        string sIdTipoEmision;
        string sFechaEmision;
        string sCodigoDocumento;
        string sRuc;
        string sIdTipoAmbiente;
        string sEstablecimiento;
        string sPuntoEmision;
        string sSecuencial;
        string sCodigoNumerico = "12345678";
        string sDigitoVerificador;
        string sRazonSocial;
        string sNombreComercial;
        string sDireccionMatriz;
        string sDireccionEstablecimiento;

        string sRetorno;
        string sClaveAccesoAutorizacion;

        //FUNCION PARA CONSULTAR LOS DATOS DE LA EMPRESA
        private bool consultarDatosEmpresa(long P_Ln_Id_Factura, int iIdEmpresa_P)
        {
            try
            {
                sSql = "";
                sSql = sSql + "select" + Environment.NewLine;
                sSql = sSql + "F.razonSocial, F.nombrecomercial, F.ruc, F.codDoc, F.estab,F.ptoEmi," + Environment.NewLine;
                sSql = sSql + "F.secuencial, F.dirMatriz, F.fechaEmision, F.dirEstablecimiento," + Environment.NewLine;
                sSql = sSql + "F.contribuyenteEspecial, F.obligadoContabilidad, F.tipoIdentificacionComprador," + Environment.NewLine;
                sSql = sSql + "F.razonSocialComprador, F.identificacionComprador, F.moneda, F.clavetoken," + Environment.NewLine;
                sSql = sSql + "F.Direccion, F.telefono, F.email, F.referencia, F.fabricante, F.comentarios" + Environment.NewLine;
                sSql = sSql + "from cel_vw_infofactura F" + Environment.NewLine;
                sSql = sSql + "where F.idEmpresa = " + iIdEmpresa_P + Environment.NewLine;
                sSql = sSql + "and F.id_factura = " + P_Ln_Id_Factura + Environment.NewLine;

                dtEmpresa = new DataTable();
                dtEmpresa.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtEmpresa);

                if (bRespuesta == true)
                {
                    if (dtEmpresa.Rows.Count == 0)
                    {                        
                    //"Esta factura no es electrónica.";
                        return false;
                    }

                    else
                    {
                        sRazonSocial = dtEmpresa.Rows[0].ItemArray[0].ToString();
                        sNombreComercial = dtEmpresa.Rows[0].ItemArray[1].ToString();
                        sRuc = dtEmpresa.Rows[0].ItemArray[2].ToString();
                        sCodigoDocumento = dtEmpresa.Rows[0].ItemArray[3].ToString();
                        sEstablecimiento = dtEmpresa.Rows[0].ItemArray[4].ToString().PadLeft(3, '0');
                        sPuntoEmision = dtEmpresa.Rows[0].ItemArray[5].ToString().PadLeft(3, '0');
                        sSecuencial = dtEmpresa.Rows[0].ItemArray[6].ToString().PadLeft(9, '0');
                        sFechaEmision = Convert.ToDateTime(dtEmpresa.Rows[0].ItemArray[8].ToString()).ToString("ddMMyyyy");
                        sDireccionMatriz = dtEmpresa.Rows[0].ItemArray[7].ToString();
                        sDireccionEstablecimiento = dtEmpresa.Rows[0].ItemArray[9].ToString();
                    }
                }

                else
                {
                    //MENSAJE DE ERRORES
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                //MENSAJE DE ERRORES
                return false;
            }
        }


        //FUNCION PARA GENERAR LA CLAVE DE ACCESO
        private string claveAcceso()
        {

            //GENERAR LA CLAVE DE ACCESO
            sClaveAcceso = "";
            //sFechaddmmaaaa = Convert.ToDateTime(sFechaEmision).ToString("ddMMyyyy");
            consultarTipoAmbiente();
            consultarTipoEmision();

            //EMISION NORMAL- SISTEMA DEL SRI DISPONIBLE
            if (sIdTipoEmision == "1")
            {
                sClaveAcceso = sClaveAcceso + sFechaEmision + sCodigoDocumento + sRuc + sIdTipoAmbiente;
                sClaveAcceso = sClaveAcceso + sEstablecimiento + sPuntoEmision + sSecuencial + sCodigoNumerico + sIdTipoEmision;
            }

            //EMISION POR INDISPONIBILIDAD DEL SISTEMA - SISTEMA DEL SRI DISPONIBLE
            else
            {
                //sClaveAcceso = sClaveAcceso + sFechaddmmaaaa + sCodigoDocumento;
                //FALTA FUNCION PARA CONSULTAR CLAVES DE CONTINGENCIA
            }

            sDigitoVerificador = sDigitoVerificarModulo11(sClaveAcceso);
            sClaveAcceso = sClaveAcceso + sDigitoVerificador;

            return sClaveAcceso;
        }

        //FUNCION PARA EL DIGITO VERIFICADOR MODULO 11
        private string sDigitoVerificarModulo11(string sClaveAcceso)
        {
            Int32 suma = 0;
            int inicio = 7;

            for (int i = 0; i < sClaveAcceso.Length; i++)
            {
                suma = suma + Convert.ToInt32(sClaveAcceso.Substring(i, 1)) * inicio;
                inicio--;
                if (inicio == 1)
                    inicio = 7;
            }

            Decimal modulo = suma % 11;
            suma = 11 - Convert.ToInt32(modulo);

            if (suma == 11)
            {
                suma = 0;
            }
            else if (suma == 10)
            {
                suma = 1;
            }
            //sClaveAcceso = sClaveAcceso + Convert.ToString(suma);

            return suma.ToString();
        }

        //FUNCION PARA CONSULTAR EL TIPO DE AMBIENTE CONFIGURADO EN EL SISTEMA
        private void consultarTipoAmbiente()
        {
            try
            {
                sSql = "";
                sSql = sSql + "select TA.codigo" + Environment.NewLine;
                sSql = sSql + "from sis_empresa E,cel_tipo_ambiente TA" + Environment.NewLine;
                sSql = sSql + "where E.id_tipo_ambiente = TA.id_tipo_ambiente" + Environment.NewLine;
                sSql = sSql + "and E.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "and TA.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "order By TA.codigo";

                dtTipos = new DataTable();
                dtTipos.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtTipos);

                if (bRespuesta == true)
                {
                    if (dtTipos.Rows.Count > 0)
                    {
                        sIdTipoAmbiente = dtTipos.Rows[0].ItemArray[0].ToString();
                    }

                    else
                    {
                        //"No se encuentra información de configuración del Tipo de Ambiente";
                    }
                }

                else
                {
                    //MENSAJE DE ERRORES
                }
            }

            catch (Exception ex)
            {
                //MENSAJE DE ERRORES
            }
        }

        //FUNCION PARA CONSULTAR EL TIPO DE EMISION CONFIGURADO EN EL SISTEMA
        private void consultarTipoEmision()
        {
            try
            {
                sSql = "";
                sSql = sSql + "select TE.codigo" + Environment.NewLine;
                sSql = sSql + "from sis_empresa E,cel_tipo_emision TE" + Environment.NewLine;
                sSql = sSql + "where E.id_tipo_emision = TE.id_tipo_emision" + Environment.NewLine;
                sSql = sSql + "and E.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "and TE.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "order By TE.codigo";

                dtTipos = new DataTable();
                dtTipos.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtTipos);

                if (bRespuesta == true)
                {
                    if (dtTipos.Rows.Count > 0)
                    {
                        sIdTipoEmision = dtTipos.Rows[0].ItemArray[0].ToString();
                    }

                    else
                    {
                        //"No se encuentra información de configuración del Tipo de Emisión";
                    }
                }

                else
                {
                    //MENSAJE DE ERRORES
                }
            }

            catch (Exception ex)
            {
                //MENSAJE DE ERRORES
            }
        }

        //FUNCION PARA CALCULAR LA EDAD
        private string calcularEdad(DateTime nacimiento, DateTime ahora)
        {
            try
            {                
                int edad = ahora.Year - nacimiento.Year;

                if (ahora.Month < nacimiento.Month || (ahora.Month == nacimiento.Month && ahora.Day < nacimiento.Day))
                {
                    edad--;
                }

                if (edad >= 65)
                {
                    sTipoCliente = "TERCERA EDAD";
                }

                else if (edad >= 18)
                {
                    sTipoCliente = "CONVENCIONAL";
                }

                else
                {
                    sTipoCliente = "MENOR DE EDAD";                    
                }

                return sTipoCliente;
            }

            catch (Exception ex)
            {
                return "ERROR";
            }
        }


        public string llenarBoleto(int iIdPedido_P, int iIdEmpresa)
        {
            try
            {
                this.iIdPedido = iIdPedido_P;
                sTexto = "";

                sSql = "";
                sSql += "select * from ctt_vw_factura" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    return "ERROR";
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    return "ERROR";
                }

                iIdFactura = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                if (consultarDatosEmpresa(iIdFactura, iIdEmpresa) == false)
                {
                    return "ERROR";
                }

                sClaveAccesoAutorizacion = claveAcceso();

                sRetorno = caracter.saltoLinea("CLAVE ACCESO: " + claveAcceso(), 0);

                //CREACION DEL REPORTE
                sTexto += "".PadLeft(40, '=') + Environment.NewLine;
                sTexto += "COMPAÑÍA DE TRANSPORTES".PadLeft(33, ' ') + Environment.NewLine;
                sTexto += "\"EXPRESS ATENAS S.A.\"".PadLeft(32, ' ') + Environment.NewLine;
                sTexto += "Sirviendo a la Provincia de Bolívar".PadLeft(38, ' ') + Environment.NewLine + Environment.NewLine;
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "Estimado Cliente, puede consultar su" + Environment.NewLine;
                sTexto += "factura electronica en las siguientes 24" + Environment.NewLine;
                sTexto += "horas en la pagina:" + Environment.NewLine;
                sTexto += "www.sri.gob.ec" + Environment.NewLine;
                sTexto += caracter.saltoLinea("CLAVE ACCESO: " + sClaveAccesoAutorizacion, 0);
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "Factura #:".PadRight(18, ' ') + dtConsulta.Rows[0][48].ToString().Trim() + "-" + dtConsulta.Rows[0][49].ToString().Trim() + "-" + dtConsulta.Rows[0][37].ToString().Trim().PadLeft(9, '0') + Environment.NewLine;
                sTexto += "Fecha Factura:".PadRight(18, ' ') + Convert.ToDateTime(dtConsulta.Rows[0][1].ToString().Trim()).ToString("dd-MM-yyyy") + Environment.NewLine;
                sTexto += "RUC Factura:".PadRight(18, ' ') + dtConsulta.Rows[0][16].ToString().Trim() + Environment.NewLine;
                sTexto += "Nombre:".PadRight(18, ' ');

                sNombrePasajero = (dtConsulta.Rows[0][17].ToString().Trim() + " " + dtConsulta.Rows[0][18].ToString().Trim()).Trim();                

                if (sNombrePasajero.Trim().Length <= 22)
                {
                    sTexto += sNombrePasajero.Trim() + Environment.NewLine;
                }

                else
                {
                    sTexto += caracter.saltoLinea(sNombrePasajero, 18);
                }

                sTexto += "Ruta:".PadRight(18, ' ') + dtConsulta.Rows[0][51].ToString().Trim() + Environment.NewLine;
                sTexto += "Tipo Viaje:".PadRight(18, ' ') + dtConsulta.Rows[0][74].ToString().Trim() + Environment.NewLine;

                dbTotal = 0;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {                    
                    dbCantidad = Convert.ToDouble(dtConsulta.Rows[i][27].ToString());
                    dbPrecioUnitario = Convert.ToDouble(dtConsulta.Rows[i][28].ToString());
                    dbDescuento = Convert.ToDouble(dtConsulta.Rows[i][29].ToString());

                    dbTotal = dbTotal + ((dbPrecioUnitario - dbDescuento) * dbCantidad);
                }

                sTexto += "Valor:".PadRight(18, ' ') + dbTotal.ToString("N2") + Environment.NewLine;
                sTexto += "Fecha Viaje:".PadRight(18, ' ') + Convert.ToDateTime(dtConsulta.Rows[0][52].ToString()).ToString("dd-MM-yyyy") + Environment.NewLine;
                sTexto += "Hora Viaje:".PadRight(18, ' ') + Convert.ToDateTime(dtConsulta.Rows[0][53].ToString()).ToString("HH:mm") + Environment.NewLine;
                sTexto += "Andén:".PadRight(18, ' ') + dtConsulta.Rows[0][54].ToString().Trim() + Environment.NewLine + Environment.NewLine;
                sTexto += "BUS #:".PadRight(18, ' ') + dtConsulta.Rows[0][50].ToString().Trim() + Environment.NewLine;

                if (dtConsulta.Rows.Count > 1)
                {
                    sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                    sTexto += "DETALLE DE PASAJES VENDIDOS".PadLeft(34, ' ') + Environment.NewLine;
                    sTexto += "".PadLeft(40, '-') + Environment.NewLine;

                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        sNombrePasajero = (dtConsulta.Rows[i][59].ToString().Trim() + " " + dtConsulta.Rows[i][58].ToString().Trim()).Trim();
                        sNombrePasajero = "CLIENTE: " + sNombrePasajero;

                        sTexto += "TIPO CLIENTE:".PadRight(18, ' ') + calcularEdad(Convert.ToDateTime(dtConsulta.Rows[i][66].ToString().Trim()), Convert.ToDateTime(dtConsulta.Rows[i][67].ToString().Trim())) + Environment.NewLine;

                        if (sNombrePasajero.Trim().Length <= 40)
                        {
                            sTexto += sNombrePasajero.Trim() + Environment.NewLine;
                        }

                        else
                        {
                            sTexto += caracter.saltoLinea(sNombrePasajero, 0);
                        }

                        sTexto += "ASIENTO: " + dtConsulta.Rows[i][61].ToString().Trim() + Environment.NewLine + Environment.NewLine;
                    }
                }

                else
                {
                    sTexto += "Asiento:".PadRight(18, ' ') + dtConsulta.Rows[0][61].ToString().Trim() + Environment.NewLine;
                    sTexto += "TIPO CLIENTE:".PadRight(18, ' ') + calcularEdad(Convert.ToDateTime(dtConsulta.Rows[0][66].ToString().Trim()), Convert.ToDateTime(dtConsulta.Rows[0][67].ToString().Trim())) + Environment.NewLine;
                }

                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += "OFICINISTA: " + dtConsulta.Rows[0][55].ToString().Trim() + Environment.NewLine + Environment.NewLine;
                sTexto += "Una vez emitido el boleto, no se acepta" + Environment.NewLine;
                sTexto += "cambio ni devoluciones" + Environment.NewLine;                
                sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                sTexto += Environment.NewLine + Environment.NewLine + ".";

                return sTexto;
            }

            catch(Exception)
            {
                return "ERROR";
            }
        }
    }
}