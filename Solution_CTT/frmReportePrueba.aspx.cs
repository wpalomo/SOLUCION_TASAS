using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using NEGOCIO;

namespace Solution_CTT
{
    public partial class frmReportePrueba : System.Web.UI.Page
    {
        string sSql;

        bool bRespuesta;

        DataTable dtConsulta;
        manejadorConexion conexionM = new manejadorConexion();

        Clases.ImpresionReporte impresiones = new Clases.ImpresionReporte();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
                return;
            }

            try
            {
                rptPruebas.ProcessingMode = ProcessingMode.Local;
                rptPruebas.LocalReport.ReportPath = Server.MapPath("~/Reportes/rptFactura.rdlc");

                rptPruebas.LocalReport.DataSources.Clear();
                rptPruebas.LocalReport.Refresh();

                //DSReportes ds = new DSReportes();

                //sSql = "";
                //sSql += "SELECT * FROM ctt_vw_reporte_pasajeros" + Environment.NewLine;
                ////sSql += "where id_ctt_progranacion = " + Convert.ToInt32(Session["idCttProgramacion"].ToString());
                //sSql += "where id_ctt_programacion = 11";

                //dtConsulta = new DataTable();
                //dtConsulta.Clear();

                //bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                //if (bRespuesta == true)
                //{
                //    DataTable dt = ds.Tables["RptPasajeros"];
                //    dt.Clear();

                //    DataRow dr;

                //    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                //    {
                //        dr = dt.NewRow();

                //        dr["id_ctt_programacion"] = dtConsulta.Rows[i][0].ToString();
                //        dr["descripcion_chofer"] = dtConsulta.Rows[i][1].ToString();
                //        dr["fecha_viaje"] = Convert.ToDateTime(dtConsulta.Rows[i][2].ToString()).ToString("dd/MMM/yyyy");
                //        dr["numero_viaje"] = dtConsulta.Rows[i][3].ToString();
                //        dr["hora_salida"] = Convert.ToDateTime(dtConsulta.Rows[i][4].ToString()).ToString("HH:mm");
                //        dr["disco_placa"] = dtConsulta.Rows[i][5].ToString();
                //        dr["identificacion"] = dtConsulta.Rows[i][6].ToString();
                //        dr["pasajero"] = dtConsulta.Rows[i][7].ToString();
                //        dr["destino"] = dtConsulta.Rows[i][8].ToString();
                //        dr["numero_asiento"] = dtConsulta.Rows[i][9].ToString();
                //        dr["precio"] = dtConsulta.Rows[i][10].ToString();
                //        dr["usuario_ingreso"] = dtConsulta.Rows[i][11].ToString();
                //        dr["tipo_cliente"] = dtConsulta.Rows[i][12].ToString();

                //        dt.Rows.Add(dr);
                //    }

                //    int j = dt.Rows.Count;

                //    Reportes.CRPasajeros reporte = new Reportes.CRPasajeros();
                //    reporte.SetDataSource(dt);
                //    //reporte.SetDataSource(ds);
                //    //reporte.SetDataSource(ds.Tables[0]);
                //    CRVer.ReportSource = reporte;
                //    CRVer.DataBind();

                //    //ReportDataSource rds = new ReportDataSource("", ds.Tables[0]);
                //    //visor.LocalReport.DataSources.Clear();
                //    //visor.LocalReport.DataSources.Add(rds);
                //    //visor.DataBind();
                //    //visor.LocalReport.Refresh();
                //}

                //else
                //{

                //}

            }

            catch (Exception ex)
            {
                
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            LocalReport reporte = new LocalReport();
            reporte.ReportPath = Server.MapPath("~/Reportes/rptCodigoBarras.rdlc");
            ReportParameter[] parametros = new ReportParameter[1];
            parametros[0] = new ReportParameter("P_St_Tasa_Usuario", "01446247751786000111");
            reporte.SetParameters(parametros);
            reporte.Refresh();
            impresiones.Imprime(reporte, "EPSON TM-T20II Receipt");
        }
    }
}