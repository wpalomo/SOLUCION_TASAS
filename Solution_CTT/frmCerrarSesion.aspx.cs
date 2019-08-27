using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution_Boletos_CTT
{
    public partial class frmCerrarSesion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();
            Session["idUsuario"] = null;
            Session["usuario"] = null;
            Response.Redirect("frmLogin.aspx");
        }
    }
}