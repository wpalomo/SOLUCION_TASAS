//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using NEGOCIO;


namespace Solution_CTT.Clases_Factura_Electronica
{
    class ClaseGenerarRIDE
    {
        manejadorConexion conexion = new manejadorConexion();
        DSReportes ds = new DSReportes();//INSTANCIO DS PARA REPORTE

        string sSql;
        string filename;

        DataTable dtConsulta;
        DataTable dtDatos;

        Double dbPorcentajeDescuento;

        bool bRespuesta;

        public bool generarRide(DataTable dtDatos, string filename, long iIdFactura)
        {
            try
            {
                //this.dtDatos = dtDatos;
                //this.filename = filename;

                //sSql = "";
                //sSql = sSql + "select consumo_alimentos, repartidor_externo, porcentaje_descuento_externo" + Environment.NewLine;
                //sSql = sSql + "from pos_vw_factura" + Environment.NewLine;
                //sSql = sSql + "where id_factura = " + iIdFactura;

                //dtConsulta = new DataTable();
                //dtConsulta.Clear();

                //bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                //if (bRespuesta == true)
                //{
                //    if (dtConsulta.Rows.Count > 0)
                //    {
                        //dbPorcentajeDescuento = Convert.ToDouble(dtConsulta.Rows[0].ItemArray[2].ToString());

                        //if ((dtConsulta.Rows[0].ItemArray[0].ToString() == "0") && (dtConsulta.Rows[0].ItemArray[1].ToString() == "0"))
                        //{
                            llenarFacturaCompleta();
                        //}

                //        else if ((dtConsulta.Rows[0].ItemArray[0].ToString() == "1") && (dtConsulta.Rows[0].ItemArray[1].ToString() == "0"))
                //        {
                //            llenarFacturaConsumoAlimentos();
                //        }

                //        else if ((dtConsulta.Rows[0].ItemArray[0].ToString() == "1") && (dtConsulta.Rows[0].ItemArray[1].ToString() == "1"))
                //        {
                //            llenarFacturaRepartidorExterno();
                //        }
                //    }
                //}

                //else
                //{
                    //catchMensaje.LblMensaje.Text = sSql;
                //}
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }

        }

        public DataTable llenarFacturaCompleta()
        {
            try
            {
                DataTable dt = ds.Tables["dtFactura_FE"];
                dt.Clear();

                DataRow dr;
                int iColumna;


                for (int i = 0; i < dtDatos.Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    iColumna = 0;

                    dr["id_Factura"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["fecha_factura"] = dtDatos.Rows[i].ItemArray[iColumna].ToString().Substring(0, 10);
                    iColumna++;
                    dr["fecha_vencimiento"] = dtDatos.Rows[i].ItemArray[iColumna].ToString().Substring(0, 10);
                    iColumna++;
                    dr["plazo"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["direccion_factura"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["sector"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["telefono_factura"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["ciudad_factura"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["fabricante"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["referencia"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["placa"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["kilometraje"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["comentarios"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["usuario_ingreso"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["fecha_ingreso"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["codigo_alterno"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["identificacion"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["nombres"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["apellidos"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["nombre_comercial"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["valida_stock"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["valida_stock_descripcion"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["id_det_pedido"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["codigo"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["nombre"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["cantidad"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["precio_unitario"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["valor_dscto"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["valor_ice"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["paga_ice"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["porcentaje_iva"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["valor_iva"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["paga_iva"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["estab"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["ptoemi"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["numero_factura"] = dtDatos.Rows[i].ItemArray[iColumna].ToString().PadLeft(9, '0');
                    iColumna++;
                    dr["descripcion_pago"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["codigo_vendedor"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["abreviacion_titulo"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["vendedor"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["cargo"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["descripcion"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["id_especificacion"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["linea"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["numero_linea"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["unidad"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["clave_acceso"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["autorizacion"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["fecha_autorizacion"] = dtDatos.Rows[i].ItemArray[iColumna].ToString().Substring(0, 10);

                    iColumna++;

                    if (dtDatos.Rows[i].ItemArray[iColumna].ToString().Length != 0)
                    {
                        dr["hora_autorizacion"] = dtDatos.Rows[i].ItemArray[iColumna].ToString().Substring(0, 8);
                    }

                    else
                    {
                        dr["hora_autorizacion"] = "";
                    }

                    iColumna++;
                    dr["ambiente"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["emision"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["email_factura"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["direccionmatriz"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["direccionsucursal"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["numeroresolucioncontribuyenteespecial"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["obligadollevarcontabilidad"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["tipo_comprobante"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["numeroruc"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["razonsocial"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["nombrecomercial"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["codigo_sri_forma_pago"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["descripcion_sri_forma_pago"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["propina"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["numero_orden"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["numero_cuenta"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["tipo_orden"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["hora"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["cajero"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["forma_pago"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();
                    iColumna++;
                    dr["cambio"] = dtDatos.Rows[i].ItemArray[iColumna].ToString();

                    dt.Rows.Add(dr);
                }

                //Facturacion_Electronica.frmReporteFacturaElectronica ver = new Facturacion_Electronica.frmReporteFacturaElectronica(dt, filename);
                //ver.ShowDialog();

                //SECCION PARA CREAR EL CRYSTAL REPORT PARA EXPORTARLO A PDF

                //if (filename != "")
                //{
                    //rptGenerarRIDE rpt = new rptGenerarRIDE();//JONA
                //Facturacion_Electronica.rptFacturaEletronica reporte = new Facturacion_Electronica.rptFacturaEletronica();
                //reporte.SetDataSource(dt);
                //reporte.ExportToDisk(ExportFormatType.PortableDocFormat, filename);
                //}

                //else
                //{
                    //Facturacion_Electronica.frmReporteFacturaElectronica ver = new Facturacion_Electronica.frmReporteFacturaElectronica(dt);
                    //ver.ShowDialog();
                //}

                return dt;//DEVUELVO DT JONA
            }

            catch (Exception ex)
            {
                DataTable dt = new DataTable();
                return dt;


                //catchMensaje.LblMensaje.Text = ex.ToString();
                //catchMensaje.ShowDialog();
            }
        }


        //FUNCION PARA CREAR UN RIDE DE CONSUMO ALIMENTOS
        public void llenarFacturaConsumoAlimentos()
        {
            try
            {
                int iColumna;
                Double dPrecioUnitario = 0;
                Double dValorDescuento = 0;
                Double dValorIce = 0;
                Double dValorIva = 0;

                for (int i = 0; i < dtDatos.Rows.Count; i++)
                {
                    dPrecioUnitario = dPrecioUnitario + Convert.ToDouble(dtDatos.Rows[i].ItemArray[26].ToString());
                    dValorDescuento = dValorDescuento + Convert.ToDouble(dtDatos.Rows[i].ItemArray[27].ToString());
                    dValorIce = dValorIce + Convert.ToDouble(dtDatos.Rows[i].ItemArray[28].ToString());
                    dValorIva = dValorIva + Convert.ToDouble(dtDatos.Rows[i].ItemArray[31].ToString());
                }

                DataTable dt = ds.Tables["dtFactura_FE"];
                dt.Clear();
                DataRow dr = dt.NewRow();
                iColumna = 0;

                dr["id_Factura"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["fecha_factura"] = dtDatos.Rows[0].ItemArray[iColumna].ToString().Substring(0, 10);
                iColumna++;
                dr["fecha_vencimiento"] = dtDatos.Rows[0].ItemArray[iColumna].ToString().Substring(0, 10);
                iColumna++;
                dr["plazo"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["direccion_factura"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["sector"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["telefono_factura"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["ciudad_factura"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["fabricante"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["referencia"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["placa"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["kilometraje"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["comentarios"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["usuario_ingreso"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["fecha_ingreso"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["codigo_alterno"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["identificacion"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["nombres"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["apellidos"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["nombre_comercial"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["valida_stock"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["valida_stock_descripcion"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["id_det_pedido"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["codigo"] = "CONSALI";
                iColumna++;
                dr["nombre"] = "CONSUMO ALIMENTOS";
                iColumna++;
                dr["cantidad"] = "1";
                iColumna++;
                dr["precio_unitario"] = dPrecioUnitario;
                iColumna++;
                dr["valor_dscto"] = dValorDescuento;
                iColumna++;
                dr["valor_ice"] = dValorIce;
                iColumna++;
                dr["paga_ice"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["porcentaje_iva"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["valor_iva"] = dValorIva;
                iColumna++;
                dr["paga_iva"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["estab"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["ptoemi"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["numero_factura"] = dtDatos.Rows[0].ItemArray[iColumna].ToString().PadLeft(9, '0');
                iColumna++;
                dr["descripcion_pago"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["codigo_vendedor"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["abreviacion_titulo"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["vendedor"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["cargo"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["descripcion"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["id_especificacion"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["linea"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["numero_linea"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["unidad"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["clave_acceso"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["autorizacion"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["fecha_autorizacion"] = dtDatos.Rows[0].ItemArray[iColumna].ToString().Substring(0, 10);

                iColumna++;

                if (dtDatos.Rows[0].ItemArray[iColumna].ToString().Length != 0)
                {
                    dr["hora_autorizacion"] = dtDatos.Rows[0].ItemArray[iColumna].ToString().Substring(0, 8);
                }

                else
                {
                    dr["hora_autorizacion"] = "";
                }

                iColumna++;
                dr["ambiente"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["emision"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["email_factura"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["direccionmatriz"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["direccionsucursal"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["numeroresolucioncontribuyenteespecial"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["obligadollevarcontabilidad"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["tipo_comprobante"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["numeroruc"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["razonsocial"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["nombrecomercial"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["codigo_sri_forma_pago"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["descripcion_sri_forma_pago"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["propina"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["numero_orden"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["numero_cuenta"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["tipo_orden"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["hora"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["cajero"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["forma_pago"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["cambio"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();

                dt.Rows.Add(dr);

                ////SECCION PARA CREAR EL CRYSTAL REPORT PARA EXPORTARLO A PDF
                //Facturacion_Electronica.rptFacturaEletronica reporte = new Facturacion_Electronica.rptFacturaEletronica();
                //reporte.SetDataSource(dt);
                //reporte.ExportToDisk(ExportFormatType.PortableDocFormat, filename);

                //SECCION PARA CREAR EL CRYSTAL REPORT PARA EXPORTARLO A PDF

                //if (filename != "")
                //{
                //    Facturacion_Electronica.rptFacturaEletronica reporte = new Facturacion_Electronica.rptFacturaEletronica();
                //    reporte.SetDataSource(dt);
                //    reporte.ExportToDisk(ExportFormatType.PortableDocFormat, filename);
                //}

                //else
                //{
                //    Facturacion_Electronica.frmReporteFacturaElectronica ver = new Facturacion_Electronica.frmReporteFacturaElectronica(dt);
                //    ver.ShowDialog();
                //}


            }

            catch (Exception ex)
            {
                //catchMensaje.LblMensaje.Text = ex.ToString();
                //catchMensaje.ShowDialog();
            }
        }


        //FUNCION PARA CREAR UN RIDE DE CONSUMO ALIMENTOS CON REPARTIDOR EXTERNO
        public void llenarFacturaRepartidorExterno()
        {
            try
            {
                int iColumna;
                Double dbCantidad = 0;
                Double dPrecioUnitario = 0;
                Double dValorDescuento = 0;
                Double dValorIce = 0;
                Double dValorIva = 0;
                Double dbServicio = 0;
                Double dValorTotal = 0;
                Double dbSubtotal = 0;
                Double dbSumaServicio;

                for (int i = 0; i < dtDatos.Rows.Count; i++)
                {
                    dbCantidad = Convert.ToDouble(dtDatos.Rows[i].ItemArray[25].ToString());
                    dPrecioUnitario = Convert.ToDouble(dtDatos.Rows[i].ItemArray[26].ToString());
                    //dbServicio = dbServicio + (dbCantidad * dPrecioUnitario * Program.servicio);
                    dValorIce = Convert.ToDouble(dtDatos.Rows[i].ItemArray[28].ToString());
                    dValorIva = Convert.ToDouble(dtDatos.Rows[i].ItemArray[31].ToString());

                    dValorTotal = dValorTotal + (dbCantidad * (dPrecioUnitario + dValorIce + dValorIva));
                }

                dValorTotal = dValorTotal + dbServicio;
                dValorDescuento = dValorTotal * (dbPorcentajeDescuento / 100);
                dbSubtotal = dValorTotal - dValorDescuento;
                dValorIva = dbSubtotal * (Convert.ToDouble(dtDatos.Rows[0].ItemArray[30].ToString()) / 100);

                DataTable dt = ds.Tables["dtFactura_FE"];
                dt.Clear();
                DataRow dr = dt.NewRow();
                iColumna = 0;

                dr["id_Factura"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["fecha_factura"] = dtDatos.Rows[0].ItemArray[iColumna].ToString().Substring(0, 10);
                iColumna++;
                dr["fecha_vencimiento"] = dtDatos.Rows[0].ItemArray[iColumna].ToString().Substring(0, 10);
                iColumna++;
                dr["plazo"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["direccion_factura"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["sector"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["telefono_factura"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["ciudad_factura"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["fabricante"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["referencia"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["placa"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["kilometraje"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["comentarios"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["usuario_ingreso"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["fecha_ingreso"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["codigo_alterno"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["identificacion"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["nombres"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["apellidos"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["nombre_comercial"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["valida_stock"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["valida_stock_descripcion"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["id_det_pedido"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["codigo"] = "CONSALI";
                iColumna++;
                dr["nombre"] = "CONSUMO ALIMENTOS";
                iColumna++;
                dr["cantidad"] = "1";
                iColumna++;
                dr["precio_unitario"] = dValorTotal;
                iColumna++;
                dr["valor_dscto"] = dValorDescuento; ;
                iColumna++;
                dr["valor_ice"] = 0.00;
                iColumna++;
                dr["paga_ice"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["porcentaje_iva"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["valor_iva"] = dValorIva;
                iColumna++;
                dr["paga_iva"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["estab"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["ptoemi"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["numero_factura"] = dtDatos.Rows[0].ItemArray[iColumna].ToString().PadLeft(9, '0');
                iColumna++;
                dr["descripcion_pago"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["codigo_vendedor"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["abreviacion_titulo"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["vendedor"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["cargo"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["descripcion"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["id_especificacion"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["linea"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["numero_linea"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["unidad"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["clave_acceso"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["autorizacion"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["fecha_autorizacion"] = dtDatos.Rows[0].ItemArray[iColumna].ToString().Substring(0, 10);

                iColumna++;

                if (dtDatos.Rows[0].ItemArray[iColumna].ToString().Length != 0)
                {
                    dr["hora_autorizacion"] = dtDatos.Rows[0].ItemArray[iColumna].ToString().Substring(0, 8);
                }

                else
                {
                    dr["hora_autorizacion"] = "";
                }

                iColumna++;
                dr["ambiente"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["emision"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["email_factura"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["direccionmatriz"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["direccionsucursal"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["numeroresolucioncontribuyenteespecial"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["obligadollevarcontabilidad"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["tipo_comprobante"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["numeroruc"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["razonsocial"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["nombrecomercial"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["codigo_sri_forma_pago"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["descripcion_sri_forma_pago"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["propina"] = dbServicio;
                iColumna++;
                dr["numero_orden"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["numero_cuenta"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["tipo_orden"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["hora"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["cajero"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["forma_pago"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();
                iColumna++;
                dr["cambio"] = dtDatos.Rows[0].ItemArray[iColumna].ToString();

                dt.Rows.Add(dr);

                //SECCION PARA CREAR EL CRYSTAL REPORT PARA EXPORTARLO A PDF

                if (filename != "")
                {
                    //Facturacion_Electronica.rptFacturaEletronicaRepartidores reporte = new Facturacion_Electronica.rptFacturaEletronicaRepartidores();
                    //reporte.SetDataSource(dt);
                    //reporte.ExportToDisk(ExportFormatType.PortableDocFormat, filename);
                }

                else
                {
                    //Facturacion_Electronica.frmFacturaElectronicaExterno ver = new Facturacion_Electronica.frmFacturaElectronicaExterno(dt);
                    //ver.ShowDialog();
                }


            }

            catch (Exception ex)
            {
                //catchMensaje.LblMensaje.Text = ex.ToString();
                //catchMensaje.ShowDialog();
            }
        }
    }
}
