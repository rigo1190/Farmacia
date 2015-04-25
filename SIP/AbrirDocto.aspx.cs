using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIP
{
    public partial class AbrirDocto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string carpeta = System.Configuration.ConfigurationManager.AppSettings["ImagenesRecetas"];
            int id = Utilerias.StrToInt(Request.Params["i"].ToString());
            string nombre = Request.Params["n"].ToString();

            Response.Redirect(carpeta + "/" + id + "/" + nombre);
        }
    }
}