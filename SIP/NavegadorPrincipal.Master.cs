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


            int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());
            Usuario user = uow.UsuarioBusinessLogic.GetByID(idUser);

            if (user.Nivel == 2) {
                menuCompras.Style.Add("display", "none");
                menuVentas.Style.Add("display", "none");
                menuInventarios.Style.Add("display", "none");
                menuCatalogos.Style.Add("display", "none");
                menuInformes.Style.Add("display", "none");                
            }

            if (user.Nivel == 3)
            {
                menuCompras.Style.Add("display", "none");
                menuRecetas.Style.Add("display", "none");
                menuInventarios.Style.Add("display", "none");
                menuCatalogos.Style.Add("display", "none");
                menuInformes.Style.Add("display", "none");
            }

        }


  

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}