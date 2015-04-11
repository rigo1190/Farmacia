using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIP.Formas.POA
{
    public partial class TEST : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtaddress.Text = "MI DIRECCION";
            txtname.Text = "MI NOMBRE";

        }


        [WebMethod]
        public static string Test(string args)
        {
            string M = string.Empty;



            M = "Funciona";

            return M;
        }

    }
}