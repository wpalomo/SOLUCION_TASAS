using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Data;

namespace Solution_CTT
{
    public partial class frmPrincipal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
                return;
            }

            else
            {
                Session["modulo"] = "PÁGINA PRINCIPAL";
                DateTime ReportDate = DateTime.Now;
                lblReportDate.Text = ReportDate.ToString("MMMM dd, yyyy") + " - " + ReportDate.AddMonths(1).ToString("MMMM dd, yyyy");
            }

            
        }
        protected string ObtenerDatos()
        {
            DataTable Datos = new DataTable();

            Datos.Columns.Add(new DataColumn("Terminales", typeof(string)));
            Datos.Columns.Add(new DataColumn("Viajes", typeof(string)));

            Datos.Rows.Add(new Object[] {"Quitumbe", 11});
            Datos.Rows.Add(new Object[] { "Guaranda", 6 });
            Datos.Rows.Add(new Object[] { "Chillanes", 4 });
            Datos.Rows.Add(new Object[] { "Riobamba", 3 });
            Datos.Rows.Add(new Object[] { "Guayaquil", 9 });

            string strDatos;

            strDatos = "[['Task', 'Hours per Day'],";

            foreach (DataRow dr in Datos.Rows)
            {
                strDatos = strDatos + "[";
                strDatos = strDatos + "'" + dr[0] + "'" + "," + dr[1];
                strDatos = strDatos + "],";
            }
            strDatos = strDatos + "]";
            return strDatos;
        }
    }
}