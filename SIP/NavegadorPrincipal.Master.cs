using BusinessLogicLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIP
{
    public partial class NavegadorPrincipal : System.Web.UI.MasterPage
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();

            lblUsuario.Text = Session["NombreUsuario"].ToString();
            
            //Ejercicio objEjercicio = uow.EjercicioBusinessLogic.GetByID(Utilerias.StrToInt(Session["EjercicioId"].ToString()));
            //lblEjercicio.Text = objEjercicio != null ? objEjercicio.Año.ToString() : string.Empty;

            //mnCatalogos.Visible = true;
            //mnControlFinanciero.Visible = true;

        }


  

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}