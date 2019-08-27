using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Solution_CTT.Clases_Factura_Electronica
{
    class ClaseReporteFacturaElectronica
    {
        //ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        //VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        //VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        //Clases.ClaseManejoCaracteres caracteres = new Clases.ClaseManejoCaracteres();

        //int iCantidadCaracteres;

        //DataTable dtPagosClase;
        //DataTable dtEmpresa;
        //DataTable dtTipos;

        //string sTexto;
        //string sRespuesta;
        //string sNombreCliente;
        //string sNombreProducto;
        //string sIdTipoAmbiente;
        //string sIdTipoEmision;
        //string sTextoEmpresa;
        //string sTextoPagos;
        //string sSecuencial;
        //string sOrigen;
        //string sFecha;
        //string sHoraIngreso;
        //string sHoraSalida;
        //string sSql;
        //string sEstablecimiento;
        //string sPuntoEmision;
        //string sDireccionCliente;
        //string sCorreoCliente;
        //string sRuc;
        //string sCodigoDocumento;
        //string sFechaEmision;
        //string sClaveAcceso;
        //string sDigitoVerificador;
        //string sDireccionMatriz;
        //string sDireccionEstablecimiento;
        //string sRazonSocial;
        //string sNombreComercial;
        //string sCodigoNumerico = "12345678";  //OJO: ESTO HAY QUE PARAMETRIZAR

        //bool bRespuesta;

        //Double subtotal = 0;
        //Double iva = 0;
        //Double servicio = 0;
        //Double descuento = 0;

        //Double dPrecioUnitario;
        //Double dPrecioTotal;
        //Double dCantidad;
        //public Double dTotal;
        //Double dValorRecibido;
        //Double dCambio;

        //double dbPorcentajeServicio;

        //int j;
        //int iContador;

        //public string llenarFactura(DataTable dtConsulta, DataTable dtPagos)
        //{
        //    try
        //    {
        //        dtPagosClase = new DataTable();
        //        this.dtPagosClase = dtPagos;

        //        subtotal = 0;
        //        iva = 0;
        //        servicio = 0;
        //        descuento = 0;

        //        dbPorcentajeServicio = Convert.ToDouble(dtConsulta.Rows[0].ItemArray[70].ToString());

        //        string sNumeroOrden = dtConsulta.Rows[0].ItemArray[46].ToString();

        //        //for (int j = 0; j < dtConsulta.Rows.Count; j++)
        //        //{
        //        //    if ((dtConsulta.Rows[j].ItemArray[60].ToString() != "1") && (dtConsulta.Rows[j].ItemArray[61].ToString() != "1"))
        //        //    {
        //        //        ////subtotal = subtotal + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[4].ToString()) * (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[5].ToString()) - Convert.ToDouble(dtConsulta.Rows[j].ItemArray[7].ToString())));
        //        //        //subtotal = subtotal + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[27].ToString()) * (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[28].ToString())));
        //        //        //iva = iva + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[27].ToString()) * Convert.ToDouble(dtConsulta.Rows[j].ItemArray[33].ToString()));
        //        //        ////servicio = servicio + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[11].ToString()) * Convert.ToDouble(dtConsulta.Rows[j].ItemArray[4].ToString()));

        //        //        subtotal = subtotal + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[27].ToString()) * (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[28].ToString())));
        //        //        iva = iva + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[27].ToString()) * Convert.ToDouble(dtConsulta.Rows[j].ItemArray[33].ToString()));
        //        //        servicio = servicio + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[27].ToString()) * Convert.ToDouble(dtConsulta.Rows[j].ItemArray[64].ToString()));
        //        //        descuento = descuento + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[27].ToString()) * Convert.ToDouble(dtConsulta.Rows[j].ItemArray[29].ToString()));

        //        //    }
        //        //}

        //        sOrigen = dtConsulta.Rows[0].ItemArray[56].ToString();

        //        sFecha = Convert.ToDateTime(dtConsulta.Rows[0].ItemArray[51].ToString()).ToString("dd/MM/yyyy");
        //        sHoraIngreso = Convert.ToDateTime(dtConsulta.Rows[0].ItemArray[51].ToString()).ToString("yyyy/MM/dd HH:mm:ss");
        //        sHoraSalida = Convert.ToDateTime(dtConsulta.Rows[0].ItemArray[52].ToString()).ToString("yyyy/MM/dd HH:mm:ss");

        //        sTexto = "";
        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;
        //        sTexto = sTexto + "--------- FACTURA ELECTRONICA ----------" + Environment.NewLine;
        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;
                
        //        sRespuesta = consultarDatosEmpresa(Convert.ToInt64(dtConsulta.Rows[0].ItemArray[0].ToString()));

        //        sTexto = sTexto + sRespuesta + Environment.NewLine;

        //        sTexto = sTexto + "FECHA: " + dtConsulta.Rows[0].ItemArray[1].ToString().Substring(0, 10) + Environment.NewLine;
        //        sTexto = sTexto + "IDENTIFICACION: " + dtConsulta.Rows[0].ItemArray[16].ToString() + Environment.NewLine;

        //        sNombreCliente = (dtConsulta.Rows[0].ItemArray[17].ToString() + " " + dtConsulta.Rows[0].ItemArray[18].ToString()).Trim();

        //        if (sNombreCliente.Length <= 31)
        //        {
        //            sTexto = sTexto + "CLIENTE: " + sNombreCliente + Environment.NewLine;
        //        }

        //        else
        //        {
        //            sTexto = sTexto + "CLIENTE:".PadRight(9, ' ');
        //            sTexto = sTexto + caracteres.saltoLinea(sNombreCliente, 9);
        //        }

        //        sTexto = sTexto + "TELEFONO: " + dtConsulta.Rows[0].ItemArray[4].ToString() + Environment.NewLine;

        //        sCorreoCliente = dtConsulta.Rows[0].ItemArray[6].ToString();

        //        sTexto = sTexto + "E-MAIL:".PadRight(8, ' ');

        //        if (sCorreoCliente.Length <= 32)
        //        {
        //            sTexto = sTexto + sCorreoCliente + Environment.NewLine;
        //        }

        //        else
        //        {
        //            sTexto = sTexto + caracteres.saltoLinea(sCorreoCliente, 8);
        //        }

                
        //        sDireccionCliente = dtConsulta.Rows[0].ItemArray[3].ToString();

        //        if (sDireccionCliente.Length <= 29)
        //        {
        //            sTexto = sTexto + "DIRECCION: " + sDireccionCliente + Environment.NewLine;
        //        }

        //        else
        //        {
        //            sTexto = sTexto + "DIRECCION:".PadRight(11, ' ');
        //            sTexto = sTexto + caracteres.saltoLinea(sDireccionCliente, 11);
        //        }

        //        //PROCESO PARA DOMICILIO AGREGAR REFERENCIA

        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;
        //        sTexto = sTexto + "FACTURA ELECTRONICA: " + sEstablecimiento + "-" + sPuntoEmision + "-" + sSecuencial + Environment.NewLine;
        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;

        //        sRespuesta = claveAcceso();

        //        sTexto = sTexto + "CLAVE ACCESO: " + sRespuesta.Substring(0, 26) + Environment.NewLine;
        //        sTexto = sTexto + "".PadLeft(14, ' ') + sRespuesta.Substring(26) + Environment.NewLine;

        //        sTexto = sTexto + "AUTORIZACION: " + sRespuesta.Substring(0, 26) + Environment.NewLine;
        //        sTexto = sTexto + "".PadLeft(14, ' ') + sRespuesta.Substring(26) + Environment.NewLine;

        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;
        //        sTexto = sTexto + "La notificacion electronica sera enviada" + Environment.NewLine;
        //        sTexto = sTexto + "a su correo electronico." + Environment.NewLine;
        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;
        //        sTexto = sTexto + "TIPO ORDEN".PadRight(11, ' ') + ": " + sOrigen + Environment.NewLine;

        //        if (sOrigen == "MESAS")
        //        {
        //            sTexto = sTexto + "NUM. MESA".PadRight(11, ' ') + ": " + dtConsulta.Rows[0].ItemArray[48].ToString().PadRight(9, ' ') + "NUM. PERSONAS: " + dtConsulta.Rows[0].ItemArray[55].ToString() + Environment.NewLine;
        //        }

        //        sTexto = sTexto + "No. ORDEN".PadRight(11, ' ') + ": " + dtConsulta.Rows[0].ItemArray[62].ToString().PadRight(9, ' ') + "CUENTA: " + dtConsulta.Rows[0].ItemArray[57].ToString() + Environment.NewLine;
        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;

        //        //EN ESTA SECCION SE VA A DETALLAR LOS ITEMS PEDIDOS
        //        sTexto = sTexto + "CANT DESCRIPCION            V.UNI.  TOT." + Environment.NewLine;
        //        sTexto = sTexto + "".PadLeft(40, '-') + Environment.NewLine;

        //        if (dtConsulta.Rows[0].ItemArray[66].ToString() == "1")
        //        {
        //            for (int i = 0; i < dtConsulta.Rows.Count; i++)
        //            {
        //                dCantidad = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[27].ToString());
        //                dPrecioUnitario = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[28].ToString());
        //                dPrecioTotal = dCantidad * dPrecioUnitario;

        //                //ACUMULADOR DE VALORES
        //                //---------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //                subtotal = subtotal + dPrecioTotal;
        //                iva = iva + (dCantidad * Convert.ToDouble(dtConsulta.Rows[i].ItemArray[33].ToString()));
        //                servicio = servicio + (dCantidad * Convert.ToDouble(dtConsulta.Rows[i].ItemArray[64].ToString()));
        //                descuento = descuento + (dCantidad * Convert.ToDouble(dtConsulta.Rows[i].ItemArray[29].ToString()));

        //                //---------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //            }

        //            sTexto = sTexto + "  1  " + "CONSUMO ALIMENTOS".PadRight(21, ' ') + subtotal.ToString("N2").PadLeft(7, ' ') + subtotal.ToString("N2").PadLeft(7, ' ') + Environment.NewLine;
        //        }

        //        else
        //        {
        //            for (int i = 0; i < dtConsulta.Rows.Count; i++)
        //            {
        //                dCantidad = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[27].ToString());
        //                dPrecioUnitario = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[28].ToString());
        //                dPrecioTotal = dCantidad * dPrecioUnitario;

        //                //if (dCantidad == 0.5)
        //                //{
        //                //    dPrecioUnitario = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[28].ToString()) * dCantidad;
        //                //}
        //                //else
        //                //{
                            
        //                //}

        //                //if (dCantidad < 1)
        //                //{
        //                //    dPrecioUnitario = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[28].ToString()) * dCantidad;
        //                //    dPrecioTotal = dPrecioUnitario;
        //                //}

        //                //else
        //                //{
        //                //    dPrecioUnitario = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[28].ToString());
        //                //    
        //                //}

                        

        //                //ACUMULADOR DE VALORES
        //                //---------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //                subtotal = subtotal + dPrecioTotal;
        //                iva = iva + (dCantidad * Convert.ToDouble(dtConsulta.Rows[i].ItemArray[33].ToString()));
        //                servicio = servicio + (dCantidad * Convert.ToDouble(dtConsulta.Rows[i].ItemArray[64].ToString()));
        //                descuento = descuento + (dCantidad * Convert.ToDouble(dtConsulta.Rows[i].ItemArray[29].ToString()));

        //                //---------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //                sTexto = sTexto + dtConsulta.Rows[i].ItemArray[27].ToString().PadLeft(3, ' ');


        //                if (dtConsulta.Rows[i].ItemArray[58].ToString() == "")
        //                {
        //                    sNombreProducto = dtConsulta.Rows[i].ItemArray[25].ToString();
        //                }

        //                else
        //                {
        //                    sNombreProducto = dtConsulta.Rows[i].ItemArray[58].ToString();
        //                }


        //                iCantidadCaracteres = sNombreProducto.Length;

        //                if (iCantidadCaracteres <= 21)
        //                {
        //                    sTexto = sTexto + "  " + sNombreProducto.PadRight(21, ' ') + dPrecioUnitario.ToString("N2").PadLeft(7, ' ') + dPrecioTotal.ToString("N2").PadLeft(7, ' ') + Environment.NewLine;
        //                }

        //                else
        //                {
        //                    sTexto = sTexto + "  " + sNombreProducto.Substring(0, 21) + dPrecioUnitario.ToString("N2").PadLeft(7, ' ') + dPrecioTotal.ToString("N2").PadLeft(7, ' ') + Environment.NewLine;

        //                    j = iCantidadCaracteres - 21;
        //                    iContador = 21;

        //                    while (j > 0)
        //                    {
        //                        if (j >= 21)
        //                        {
        //                            sTexto = sTexto + "".PadLeft(5, ' ') + sNombreProducto.Substring(iContador, 21) + Environment.NewLine;
        //                            j = j - 21;
        //                            iContador = iContador + 21;
        //                        }

        //                        else
        //                        {
        //                            sTexto = sTexto + "".PadLeft(5, ' ') + sNombreProducto.Substring(iContador) + Environment.NewLine;
        //                            j = 0;
        //                        }
        //                    }

        //                }
        //            }
        //        }

        //            dTotal = subtotal + iva + servicio - descuento;

        //            sTexto = sTexto + "".PadLeft(19, ' ') + "".PadLeft(21, '-') + Environment.NewLine;
        //            sTexto = sTexto + "Subtotal:".PadLeft(28, ' ') + subtotal.ToString("N2").PadLeft(12, ' ') + Environment.NewLine;
        //            sTexto = sTexto + "Descuento".PadLeft(28, ' ') + " " + (dtConsulta.Rows[0].ItemArray[59].ToString() + "%:").PadRight(4, ' ') + descuento.ToString("N2").PadLeft(7, ' ') + Environment.NewLine;
        //            sTexto = sTexto + "Tarifa".PadLeft(25, ' ') + " " + dtConsulta.Rows[0].ItemArray[32].ToString() + "%:" + (subtotal - descuento).ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
        //            sTexto = sTexto + "Tarifa 0%:".PadLeft(29, ' ') + "0.00".PadLeft(11, ' ') + Environment.NewLine;
        //            sTexto = sTexto + dtConsulta.Rows[0].ItemArray[32].ToString().PadLeft(21, ' ') + "% IVA:" + iva.ToString("N2").PadLeft(13, ' ') + Environment.NewLine;

        //            if (dbPorcentajeServicio != 0)
        //            {
        //                sTexto = sTexto + "".PadLeft(19, ' ') + (dbPorcentajeServicio.ToString("N0") + "% Servicio:").PadRight(13, ' ') + servicio.ToString("N2").PadLeft(8, ' ') + Environment.NewLine;
        //            }
                    
        //            sTexto = sTexto + "".PadLeft(19, ' ') + "".PadLeft(21, '-') + Environment.NewLine;
        //            sTexto = sTexto + "TOTAL:".PadLeft(25, ' ') + dTotal.ToString("N2").PadLeft(15, ' ') + Environment.NewLine + Environment.NewLine;

        //            sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;

        //            //AQUI SE DETALLARAN LAS FORMAS DE  PAGO
        //            sRespuesta = formasPagos(dTotal);
        //            sTexto = sTexto + sRespuesta;
        //            sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;
        //            sTexto = sTexto + "CREADA : " + dtConsulta.Rows[0].ItemArray[51].ToString() + Environment.NewLine;
        //            sTexto = sTexto + "PAGADA : " + dtConsulta.Rows[0].ItemArray[52].ToString() + Environment.NewLine;
        //            sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;
        //            sTexto = sTexto + "CAJERO : " + dtConsulta.Rows[0].ItemArray[49].ToString() + Environment.NewLine;
        //            sTexto = sTexto + "MESERO : " + dtConsulta.Rows[0].ItemArray[50].ToString() + Environment.NewLine + Environment.NewLine;
        //            sTexto = sTexto + "Estimado Cliente, puede consultar su" + Environment.NewLine;
        //            sTexto = sTexto + "factura electronica en las siguientes 24" + Environment.NewLine;
        //            sTexto = sTexto + "horas en la pagina:" + Environment.NewLine;
        //            sTexto = sTexto + "www.sri.gob.ec" + Environment.NewLine;

        //        return sTexto;
        //    }

        //    catch (Exception ex)
        //    {
        //        catchMensaje.LblMensaje.Text = ex.ToString();
        //        catchMensaje.ShowDialog();
        //        return "";
        //    }
        //}


        ////FUNCION PARA CONSULTAR LOS DATOS DE LA EMPRESA
        //private string consultarDatosEmpresa(long P_Ln_Id_Factura)
        //{
        //    try
        //    {
        //        sSql = "";
        //        sSql = sSql + "select" + Environment.NewLine;
        //        sSql = sSql + "F.razonSocial, F.nombrecomercial, F.ruc, F.codDoc, F.estab,F.ptoEmi," + Environment.NewLine;
        //        sSql = sSql + "F.secuencial, F.dirMatriz, F.fechaEmision, F.dirEstablecimiento," + Environment.NewLine;
        //        sSql = sSql + "F.contribuyenteEspecial, F.obligadoContabilidad, F.tipoIdentificacionComprador," + Environment.NewLine;
        //        sSql = sSql + "F.razonSocialComprador, F.identificacionComprador, F.moneda, F.clavetoken," + Environment.NewLine;
        //        sSql = sSql + "F.Direccion, F.telefono, F.email, F.referencia, F.fabricante, F.comentarios" + Environment.NewLine;
        //        sSql = sSql + "from cel_vw_infofactura F" + Environment.NewLine;
        //        sSql = sSql + "where F.idEmpresa = " + Program.iIdEmpresa + Environment.NewLine;
        //        sSql = sSql + "and F.id_factura = " + P_Ln_Id_Factura + Environment.NewLine;

        //        dtEmpresa = new DataTable();
        //        dtEmpresa.Clear();

        //        bRespuesta = conexion.GFun_Lo_Busca_Registro(dtEmpresa, sSql);

        //        if (bRespuesta == true)
        //        {
        //            if (dtEmpresa.Rows.Count > 0)
        //            {
        //                sRazonSocial = dtEmpresa.Rows[0].ItemArray[0].ToString();
        //                sNombreComercial = dtEmpresa.Rows[0].ItemArray[1].ToString();
        //                sRuc = dtEmpresa.Rows[0].ItemArray[2].ToString();
        //                sCodigoDocumento = dtEmpresa.Rows[0].ItemArray[3].ToString();
        //                sEstablecimiento = dtEmpresa.Rows[0].ItemArray[4].ToString().PadLeft(3, '0');
        //                sPuntoEmision = dtEmpresa.Rows[0].ItemArray[5].ToString().PadLeft(3, '0');
        //                sSecuencial = dtEmpresa.Rows[0].ItemArray[6].ToString().PadLeft(9, '0');
        //                sFechaEmision = Convert.ToDateTime(dtEmpresa.Rows[0].ItemArray[8].ToString()).ToString("ddMMyyyy");
        //                sDireccionMatriz = dtEmpresa.Rows[0].ItemArray[7].ToString();
        //                sDireccionEstablecimiento = dtEmpresa.Rows[0].ItemArray[9].ToString();

        //                sTextoEmpresa = "";

        //                if (sNombreComercial == "")
        //                {
        //                    sTextoEmpresa = sTextoEmpresa + sRazonSocial + Environment.NewLine;
        //                }

        //                else
        //                {
        //                    sTextoEmpresa = sTextoEmpresa + sNombreComercial + Environment.NewLine;
        //                }

                        
        //                sTextoEmpresa = sTextoEmpresa + "RUC: " + sRuc + Environment.NewLine;
        //                sTextoEmpresa = sTextoEmpresa + "Telefono: " + Program.telefono1 + Environment.NewLine;

        //                //CREAR LINEAS DEL ESTABLECIMIENTO
        //                sTextoEmpresa = sTextoEmpresa + "Direccion:".PadRight(11, ' ');

        //                if (sDireccionEstablecimiento.Length <= 29)
        //                {
        //                    sTextoEmpresa = sTextoEmpresa + sDireccionEstablecimiento + Environment.NewLine;
        //                }

        //                else
        //                {
        //                    sTextoEmpresa = sTextoEmpresa + caracteres.saltoLinea(sDireccionEstablecimiento, 11);
        //                }

        //                //CREAR LINEAS DE LA MATRIZ
        //                sTextoEmpresa = sTextoEmpresa + "Matriz:".PadRight(10, ' ');

        //                if (sDireccionMatriz.Length <= 30)
        //                {
        //                    sTextoEmpresa = sTextoEmpresa + sDireccionMatriz + Environment.NewLine;
        //                }

        //                else
        //                {
        //                    sTextoEmpresa = sTextoEmpresa + caracteres.saltoLinea(sDireccionMatriz, 10);
        //                }

        //                sTextoEmpresa = sTextoEmpresa + "".PadLeft(40, '=');

        //                return sTextoEmpresa;
        //            }

        //            else
        //            {
        //                ok.LblMensaje.Text = "Esta factura no es electrónica.";
        //                ok.ShowInTaskbar = false;
        //                ok.ShowDialog();
        //                return "";
        //            }
        //        }

        //        else
        //        {
        //            catchMensaje.LblMensaje.Text = sSql;
        //            catchMensaje.ShowDialog();
        //            return "";
        //        }


        //    }

        //    catch (Exception ex)
        //    {
        //        catchMensaje.LblMensaje.Text = ex.ToString();
        //        catchMensaje.ShowDialog();
        //        return "";
        //    }
        //}


        //private string formasPagos(Double dTotalPago)
        //{
        //    try
        //    {
        //        sTextoPagos = "";
        //        sTextoPagos = sTextoPagos + "FORMAS DE PAGO" + Environment.NewLine;

        //        for (int i = 0; i < dtPagosClase.Rows.Count; i++)
        //        {
        //            dValorRecibido = Convert.ToDouble(dtPagosClase.Rows[i].ItemArray[4].ToString());
        //            sTextoPagos = sTextoPagos + dtPagosClase.Rows[i].ItemArray[0].ToString().PadRight(20, ' ') + ":" + dValorRecibido.ToString("N2").PadLeft(9, ' ') + Environment.NewLine;
        //        }

        //        dCambio = Convert.ToDouble(dtPagosClase.Rows[0].ItemArray[2].ToString());
        //        sTextoPagos = sTextoPagos + "".PadLeft(20, ' ') + "".PadLeft(10, '-') + Environment.NewLine;
        //        sTextoPagos = sTextoPagos + "CANTIDAD DEBIDA".PadRight(20, ' ') + ":" + dTotalPago.ToString("N2").PadLeft(9, ' ') + Environment.NewLine;
        //        sTextoPagos = sTextoPagos + "CAMBIO".PadRight(20, ' ') + ":" + dCambio.ToString("N2").PadLeft(9, ' ') + Environment.NewLine;

        //        return sTextoPagos;
        //    }

        //    catch (Exception ex)
        //    {
        //        catchMensaje.LblMensaje.Text = ex.ToString();
        //        catchMensaje.ShowDialog();
        //        return "";
        //    }
        //}


        ////FUNCION PARA GENERAR LA CLAVE DE ACCESO
        //private string claveAcceso()
        //{
        //    //GENERAR LA CLAVE DE ACCESO
        //    sClaveAcceso = "";
        //    //sFechaddmmaaaa = Convert.ToDateTime(sFechaEmision).ToString("ddMMyyyy");
        //    consultarTipoAmbiente();
        //    consultarTipoEmision();

        //    //EMISION NORMAL- SISTEMA DEL SRI DISPONIBLE
        //    if (sIdTipoEmision == "1")
        //    {
        //        sClaveAcceso = sClaveAcceso + sFechaEmision + sCodigoDocumento + sRuc + sIdTipoAmbiente;
        //        sClaveAcceso = sClaveAcceso + sEstablecimiento + sPuntoEmision + sSecuencial + sCodigoNumerico + sIdTipoEmision;
        //    }

        //    //EMISION POR INDISPONIBILIDAD DEL SISTEMA - SISTEMA DEL SRI DISPONIBLE
        //    else
        //    {
        //        //sClaveAcceso = sClaveAcceso + sFechaddmmaaaa + sCodigoDocumento;
        //        //FALTA FUNCION PARA CONSULTAR CLAVES DE CONTINGENCIA
        //    }

        //    sDigitoVerificador = sDigitoVerificarModulo11(sClaveAcceso);
        //    sClaveAcceso = sClaveAcceso + sDigitoVerificador;

        //    return sClaveAcceso;
        //}

        ////FUNCION PARA EL DIGITO VERIFICADOR MODULO 11
        //private string sDigitoVerificarModulo11(string sClaveAcceso)
        //{
        //    Int32 suma = 0;
        //    int inicio = 7;

        //    for (int i = 0; i < sClaveAcceso.Length; i++)
        //    {
        //        suma = suma + Convert.ToInt32(sClaveAcceso.Substring(i, 1)) * inicio;
        //        inicio--;
        //        if (inicio == 1)
        //            inicio = 7;
        //    }

        //    Decimal modulo = suma % 11;
        //    suma = 11 - Convert.ToInt32(modulo);

        //    if (suma == 11)
        //    {
        //        suma = 0;
        //    }
        //    else if (suma == 10)
        //    {
        //        suma = 1;
        //    }
        //    //sClaveAcceso = sClaveAcceso + Convert.ToString(suma);

        //    return suma.ToString();
        //}

        ////FUNCION PARA CONSULTAR EL TIPO DE AMBIENTE CONFIGURADO EN EL SISTEMA
        //private void consultarTipoAmbiente()
        //{
        //    try
        //    {
        //        sSql = "";
        //        sSql = sSql + "select TA.codigo" + Environment.NewLine;
        //        sSql = sSql + "from sis_empresa E,cel_tipo_ambiente TA" + Environment.NewLine;
        //        sSql = sSql + "where E.id_tipo_ambiente = TA.id_tipo_ambiente" + Environment.NewLine;
        //        sSql = sSql + "and E.estado = 'A'" + Environment.NewLine;
        //        sSql = sSql + "and TA.estado = 'A'" + Environment.NewLine;
        //        sSql = sSql + "order By TA.codigo";

        //        dtTipos = new DataTable();
        //        dtTipos.Clear();

        //        bRespuesta = conexion.GFun_Lo_Busca_Registro(dtTipos, sSql);

        //        if (bRespuesta == true)
        //        {
        //            if (dtTipos.Rows.Count > 0)
        //            {
        //                sIdTipoAmbiente = dtTipos.Rows[0].ItemArray[0].ToString();
        //            }

        //            else
        //            {
        //                ok.LblMensaje.Text = "No se encuentra información de configuración del Tipo de Ambiente";
        //                ok.ShowDialog();
        //            }
        //        }

        //        else
        //        {
        //            catchMensaje.LblMensaje.Text = sSql;
        //            catchMensaje.ShowDialog();
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        catchMensaje.LblMensaje.Text = ex.ToString();
        //        catchMensaje.ShowDialog();
        //    }
        //}

        ////FUNCION PARA CONSULTAR EL TIPO DE EMISION CONFIGURADO EN EL SISTEMA
        //private void consultarTipoEmision()
        //{
        //    try
        //    {
        //        sSql = "";
        //        sSql = sSql + "select TE.codigo" + Environment.NewLine;
        //        sSql = sSql + "from sis_empresa E,cel_tipo_emision TE" + Environment.NewLine;
        //        sSql = sSql + "where E.id_tipo_emision = TE.id_tipo_emision" + Environment.NewLine;
        //        sSql = sSql + "and E.estado = 'A'" + Environment.NewLine;
        //        sSql = sSql + "and TE.estado = 'A'" + Environment.NewLine;
        //        sSql = sSql + "order By TE.codigo";

        //        dtTipos = new DataTable();
        //        dtTipos.Clear();

        //        bRespuesta = conexion.GFun_Lo_Busca_Registro(dtTipos, sSql);

        //        if (bRespuesta == true)
        //        {
        //            if (dtTipos.Rows.Count > 0)
        //            {
        //                sIdTipoEmision = dtTipos.Rows[0].ItemArray[0].ToString();
        //            }

        //            else
        //            {
        //                ok.LblMensaje.Text = "No se encuentra información de configuración del Tipo de Emisión";
        //                ok.ShowDialog();
        //            }
        //        }

        //        else
        //        {
        //            catchMensaje.LblMensaje.Text = sSql;
        //            catchMensaje.ShowDialog();
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        catchMensaje.LblMensaje.Text = ex.ToString();
        //        catchMensaje.ShowDialog();
        //    }
        //}

        ////FUNCION PARA ARMAR LA PRIMERA SECCION  DE LA FACTURA ELECTRONICA
        //public string llenarFacturaDatos(DataTable dtConsulta, DataTable dtPagos)
        //{
        //    try
        //    {
        //        dtPagosClase = new DataTable();
        //        this.dtPagosClase = dtPagos;

        //        subtotal = 0;
        //        iva = 0;
        //        servicio = 0;
        //        descuento = 0;

        //        dbPorcentajeServicio = Convert.ToDouble(dtConsulta.Rows[0].ItemArray[70].ToString());
        //        string sNumeroOrden = dtConsulta.Rows[0].ItemArray[46].ToString();

        //        for (int j = 0; j < dtConsulta.Rows.Count; j++)
        //        {
        //            if ((dtConsulta.Rows[j].ItemArray[60].ToString() != "1") && (dtConsulta.Rows[j].ItemArray[61].ToString() != "1"))
        //            {
        //                ////subtotal = subtotal + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[4].ToString()) * (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[5].ToString()) - Convert.ToDouble(dtConsulta.Rows[j].ItemArray[7].ToString())));
        //                //subtotal = subtotal + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[27].ToString()) * (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[28].ToString())));
        //                //iva = iva + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[27].ToString()) * Convert.ToDouble(dtConsulta.Rows[j].ItemArray[33].ToString()));
        //                ////servicio = servicio + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[11].ToString()) * Convert.ToDouble(dtConsulta.Rows[j].ItemArray[4].ToString()));

        //                subtotal = subtotal + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[27].ToString()) * (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[28].ToString())));
        //                iva = iva + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[27].ToString()) * Convert.ToDouble(dtConsulta.Rows[j].ItemArray[33].ToString()));
        //                servicio = servicio + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[27].ToString()) * Convert.ToDouble(dtConsulta.Rows[j].ItemArray[64].ToString()));
        //                descuento = descuento + (Convert.ToDouble(dtConsulta.Rows[j].ItemArray[27].ToString()) * Convert.ToDouble(dtConsulta.Rows[j].ItemArray[29].ToString()));

        //            }
        //        }

        //        sOrigen = dtConsulta.Rows[0].ItemArray[56].ToString();

        //        sFecha = Convert.ToDateTime(dtConsulta.Rows[0].ItemArray[51].ToString()).ToString("dd/MM/yyyy");
        //        sHoraIngreso = Convert.ToDateTime(dtConsulta.Rows[0].ItemArray[51].ToString()).ToString("yyyy/MM/dd HH:mm:ss");
        //        sHoraSalida = Convert.ToDateTime(dtConsulta.Rows[0].ItemArray[52].ToString()).ToString("yyyy/MM/dd HH:mm:ss");

        //        sTexto = "";
        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;
        //        sTexto = sTexto + "--------- FACTURA ELECTRONICA ----------" + Environment.NewLine;
        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;

        //        sRespuesta = consultarDatosEmpresa(Convert.ToInt64(dtConsulta.Rows[0].ItemArray[0].ToString()));

        //        sTexto = sTexto + sRespuesta + Environment.NewLine;

        //        sTexto = sTexto + "FECHA: " + dtConsulta.Rows[0].ItemArray[1].ToString().Substring(0, 10) + Environment.NewLine;
        //        sTexto = sTexto + "IDENTIFICACION: " + dtConsulta.Rows[0].ItemArray[16].ToString() + Environment.NewLine;

        //        sNombreCliente = (dtConsulta.Rows[0].ItemArray[17].ToString() + " " + dtConsulta.Rows[0].ItemArray[18].ToString()).Trim();

        //        if (sNombreCliente.Length <= 31)
        //        {
        //            sTexto = sTexto + "CLIENTE: " + sNombreCliente + Environment.NewLine;
        //        }

        //        else
        //        {
        //            sTexto = sTexto + "CLIENTE: " + sNombreCliente.Substring(0, 31) + Environment.NewLine;
        //            sTexto = sTexto + "".PadLeft(9, ' ') + sNombreCliente.Substring(31) + Environment.NewLine;
        //        }

        //        sTexto = sTexto + "TELEFONO: " + dtConsulta.Rows[0].ItemArray[4].ToString() + Environment.NewLine;
        //        sTexto = sTexto + "E-MAIL: " + dtConsulta.Rows[0].ItemArray[6].ToString() + Environment.NewLine;

        //        sDireccionCliente = dtConsulta.Rows[0].ItemArray[3].ToString();

        //        if (sDireccionCliente.Length <= 29)
        //        {
        //            sTexto = sTexto + "DIRECCION: " + sDireccionCliente + Environment.NewLine;
        //        }

        //        else
        //        {
        //            sTexto = sTexto + "DIRECCION: " + sDireccionCliente.Substring(0, 29) + Environment.NewLine;
        //            sTexto = sTexto + "".PadLeft(11, ' ') + sDireccionCliente.Substring(29) + Environment.NewLine;
        //        }

        //        //PROCESO PARA DOMICILIO AGREGAR REFERENCIA

        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;
        //        sTexto = sTexto + "FACTURA ELECTRONICA: " + sEstablecimiento + "-" + sPuntoEmision + "-" + sSecuencial + Environment.NewLine;
        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;

        //        sRespuesta = claveAcceso();

        //        sTexto = sTexto + "CLAVE ACCESO: " + sRespuesta.Substring(0, 26) + Environment.NewLine;
        //        sTexto = sTexto + "".PadLeft(14, ' ') + sRespuesta.Substring(26) + Environment.NewLine;

        //        sTexto = sTexto + "AUTORIZACION: " + sRespuesta.Substring(0, 26) + Environment.NewLine;
        //        sTexto = sTexto + "".PadLeft(14, ' ') + sRespuesta.Substring(26) + Environment.NewLine;

        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;
        //        sTexto = sTexto + "La notificacion electronica sera enviada" + Environment.NewLine;
        //        sTexto = sTexto + "a su correo electronico." + Environment.NewLine;
        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;
        //        sTexto = sTexto + "TIPO ORDEN".PadRight(11, ' ') + ": " + sOrigen + Environment.NewLine;

        //        if (sOrigen == "MESAS")
        //        {
        //            sTexto = sTexto + "NUM. MESA".PadRight(11, ' ') + ": " + dtConsulta.Rows[0].ItemArray[48].ToString().PadRight(9, ' ') + "NUM. PERSONAS: " + dtConsulta.Rows[0].ItemArray[55].ToString() + Environment.NewLine;
        //        }

        //        sTexto = sTexto + "No. ORDEN".PadRight(11, ' ') + ": " + dtConsulta.Rows[0].ItemArray[62].ToString().PadRight(9, ' ') + "CUENTA: " + dtConsulta.Rows[0].ItemArray[57].ToString() + Environment.NewLine;
        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;

        //        //EN ESTA SECCION SE VA A DETALLAR LOS ITEMS PEDIDOS
        //        sTexto = sTexto + "CANT DESCRIPCION            V.UNI.  TOT." + Environment.NewLine;
        //        sTexto = sTexto + "".PadLeft(40, '-') + Environment.NewLine;

        //        for (int i = 0; i < dtConsulta.Rows.Count; i++)
        //        {
        //            dCantidad = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[27].ToString());

        //            if (dCantidad == 0.5)
        //            {
        //                dPrecioUnitario = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[28].ToString()) * dCantidad;
        //            }
        //            else
        //            {
        //                dPrecioUnitario = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[28].ToString());
        //            }

        //            dPrecioTotal = dCantidad * dPrecioUnitario;

        //            sTexto = sTexto + dtConsulta.Rows[i].ItemArray[27].ToString().PadLeft(3, ' ');

        //            if (dtConsulta.Rows[i].ItemArray[58].ToString() == "")
        //            {
        //                sNombreProducto = dtConsulta.Rows[i].ItemArray[25].ToString();
        //            }

        //            else
        //            {
        //                sNombreProducto = dtConsulta.Rows[i].ItemArray[58].ToString();
        //            }

        //            iCantidadCaracteres = sNombreProducto.Length;

        //            if (iCantidadCaracteres <= 21)
        //            {
        //                sTexto = sTexto + "  " + sNombreProducto.PadRight(21, ' ') + dPrecioUnitario.ToString("N2").PadLeft(7, ' ') + dPrecioTotal.ToString("N2").PadLeft(7, ' ') + Environment.NewLine;
        //            }

        //            else
        //            {
        //                sTexto = sTexto + "  " + sNombreProducto.Substring(0, 21) + dPrecioUnitario.ToString("N2").PadLeft(7, ' ') + dPrecioTotal.ToString("N2").PadLeft(7, ' ') + Environment.NewLine;

        //                j = iCantidadCaracteres - 21;
        //                iContador = 21;

        //                while (j > 0)
        //                {
        //                    if (j >= 21)
        //                    {
        //                        sTexto = sTexto + "".PadLeft(5, ' ') + sNombreProducto.Substring(iContador, 21) + Environment.NewLine;
        //                        j = j - 21;
        //                        iContador = iContador + 21;
        //                    }

        //                    else
        //                    {
        //                        sTexto = sTexto + "".PadLeft(5, ' ') + sNombreProducto.Substring(iContador) + Environment.NewLine;
        //                        j = 0;
        //                    }
        //                }

        //            }
        //        }

        //        dTotal = subtotal + iva + servicio - descuento;

        //        sTexto = sTexto + "".PadLeft(19, ' ') + "".PadLeft(21, '-') + Environment.NewLine;
        //        sTexto = sTexto + "Subtotal:".PadLeft(28, ' ') + subtotal.ToString("N2").PadLeft(12, ' ') + Environment.NewLine;
        //        sTexto = sTexto + "Descuento".PadLeft(28, ' ') + " " + (dtConsulta.Rows[0].ItemArray[59].ToString() + "%:").PadRight(4, ' ') + descuento.ToString("N2").PadLeft(7, ' ') + Environment.NewLine;
        //        sTexto = sTexto + "Tarifa".PadLeft(25, ' ') + " " + dtConsulta.Rows[0].ItemArray[32].ToString() + "%:" + (subtotal - descuento).ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
        //        sTexto = sTexto + "Tarifa 0%:".PadLeft(29, ' ') + "0.00".PadLeft(11, ' ') + Environment.NewLine;
        //        sTexto = sTexto + dtConsulta.Rows[0].ItemArray[32].ToString().PadLeft(21, ' ') + "% IVA:" + iva.ToString("N2").PadLeft(13, ' ') + Environment.NewLine;

        //        if (dbPorcentajeServicio != 0)
        //        {
        //            sTexto = sTexto + "".PadLeft(19, ' ') + (dbPorcentajeServicio.ToString("N0") + "% Servicio:").PadRight(13, ' ') + servicio.ToString("N2").PadLeft(8, ' ') + Environment.NewLine;
        //        }
        //        sTexto = sTexto + "".PadLeft(19, ' ') + "".PadLeft(21, '-') + Environment.NewLine;

        //        return sTexto;
        //    }

        //    catch (Exception ex)
        //    {
        //        catchMensaje.LblMensaje.Text = ex.ToString();
        //        catchMensaje.ShowDialog();
        //        return "";
        //    }
        //}

        //FUNCION PARA COMPLETAR EL DETALLE DE LA FACTURA ELECTRONICA
        //public string llenarFacturaDetalle(DataTable dtConsulta, DataTable dtPagos)
        //{
        //    try
        //    {
        //        sTexto = "";
        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;

        //        //AQUI SE DETALLARAN LAS FORMAS DE  PAGO
        //        sRespuesta = formasPagos(dTotal);
        //        sTexto = sTexto + sRespuesta;
        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;
        //        sTexto = sTexto + "CREADA : " + dtConsulta.Rows[0].ItemArray[51].ToString() + Environment.NewLine;
        //        sTexto = sTexto + "PAGADA : " + dtConsulta.Rows[0].ItemArray[52].ToString() + Environment.NewLine;
        //        sTexto = sTexto + "".PadLeft(40, '=') + Environment.NewLine;
        //        sTexto = sTexto + "CAJERO : " + dtConsulta.Rows[0].ItemArray[49].ToString() + Environment.NewLine;
        //        sTexto = sTexto + "MESERO : " + dtConsulta.Rows[0].ItemArray[50].ToString() + Environment.NewLine + Environment.NewLine;
        //        sTexto = sTexto + "Estimado Cliente, puede consultar su" + Environment.NewLine;
        //        sTexto = sTexto + "factura electronica en las siguientes 24" + Environment.NewLine;
        //        sTexto = sTexto + "horas en la pagina:" + Environment.NewLine;
        //        sTexto = sTexto + "www.sri.gob.ec" + Environment.NewLine;

        //        return sTexto;
        //    }

        //    catch (Exception ex)
        //    {
        //        catchMensaje.LblMensaje.Text = ex.ToString();
        //        catchMensaje.ShowDialog();
        //        return "";
        //    }
        //}
    }
}
