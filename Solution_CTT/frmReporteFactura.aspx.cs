using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NEGOCIO;

namespace Solution_CTT
{
    public partial class frmReporteFactura : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sNumeroFactura;
        string sAsientos;

        bool bRespuesta;

        DataTable dtConsulta;

        int iVendidos;
        int iCuenta;

        decimal dbCantidad;
        decimal dbPrecioUnitario;
        decimal dbDescuento;
        decimal dbIva;
        decimal dbSumaTotal;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
                return;
            }
        }

        //FUNCION PARA CARGAR EL REPORTE
        private void cargarReporte()
        {
            sSql = "";
            sSql += "select establecimiento, punto_emision, numero_factura, fecha_factura," + Environment.NewLine;
            sSql += "identificacion, isnull(nombres, '') + ' ' + apellidos cliente, descripcion_ruta," + Environment.NewLine;
            sSql += "tipo_viaje, fecha_viaje, hora_salida, descripcion_anden, descripcion_disco," + Environment.NewLine;
            sSql += "tasa_usuario, cantidad, precio_unitario, valor_dscto, valor_iva, clave_acceso," + Environment.NewLine;
            sSql += "oficinista, numero_asiento, '' as valor_total, '' as vendidos, '' as asientos, '' as secuencia_factura" + Environment.NewLine;
            sSql += "from ctt_vw_factura" + Environment.NewLine;
            sSql += "where id_pedido = " + Convert.ToInt32(Session["id_pedido_reporte"]);

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    sNumeroFactura = dtConsulta.Rows[0][0].ToString() + "-" + dtConsulta.Rows[0][1].ToString() + "-" + dtConsulta.Rows[0][2].ToString().PadLeft(9, '0');
                    iVendidos = dtConsulta.Rows.Count;
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

                    //RECORRER EL DATATABLE PARA LLENAR DE DATOS
                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
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
                    sSql += "where id_pedido = " + Convert.ToInt32(Session["id_pedido_reporte"]);
                    sSql += "group by tipo_cliente";

                    DataTable dt2 = ds.Tables["dtTarifas"];
                    dt2.Clear();

                    bRespuesta = conexionM.consultarRegistro(sSql, dt2);

                    if (bRespuesta == true)
                    {
                        rptFactura.ProcessingMode = ProcessingMode.Local;
                        rptFactura.LocalReport.ReportPath = Server.MapPath("~/Reportes/rptFactura.rdlc");
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
    }
}