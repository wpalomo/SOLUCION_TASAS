using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NEGOCIO;

namespace Solution_CTT
{
    public partial class frmPrincipal : System.Web.UI.Page
    {
        manejadorConexion conexionM = new manejadorConexion();

        string sSql;
        string sFecha;

        DataTable dtConsulta;

        bool bRespuesta;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("frmPermisos.aspx");
                return;
            }

            Session["modulo"] = "PÁGINA PRINCIPAL";
            DateTime ReportDate = DateTime.Now;
            lblReportDate.Text = ReportDate.ToString("MMMM dd, yyyy") + " - " + ReportDate.AddMonths(1).ToString("MMMM dd, yyyy");

            if (!IsPostBack)
            {
                sFecha = DateTime.Now.ToString("yyyy-MM-dd");
                cantidadTransacciones(sFecha);
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

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA CONSULTAR LA CANTIDAD DE TRANSACCIONES POR USUARI
        private void cantidadTransacciones(string sFecha_P)
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta " + Environment.NewLine;
                sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                //sSql += "where fecha_pedido = '" + sFecha_P + "'" + Environment.NewLine;
                sSql += "where cobro_boletos = 1" + Environment.NewLine;
                sSql += "and cobro_retencion = 0" + Environment.NewLine;
                sSql += "and cobro_administrativo = 0" + Environment.NewLine;
                sSql += "and id_ctt_cierre_caja = " + Convert.ToInt32(Session["idCierreCaja"].ToString());

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    lblMensajeError.Text = "<b>Error en la instrucción SQL:</b><br/><br/>" + sSql.Replace("\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
                    return;
                }

                lblCantidadTransacciones.Text = dtConsulta.Rows[0]["cuenta"].ToString();
            }

            catch (Exception ex)
            {
                lblMensajeError.Text = "<b>Se ha producido el siguiente error:</b><br/><br/>" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalError').modal('show');</script>", false);
            }
        }

        #endregion
    }
}