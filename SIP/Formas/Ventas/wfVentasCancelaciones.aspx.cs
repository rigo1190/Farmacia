using BusinessLogicLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SIP.Formas.Ventas
{
    public partial class wfVentasCancelaciones : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            //bloqueo del contenido segun tipo de usuario
            int iduser = int.Parse(Session["IdUser"].ToString());
            Usuario usuario = uow.UsuarioBusinessLogic.GetByID(iduser);
            if (usuario.Nivel != 1)
                divMain.Style.Add("display", "none");
            //endBloqueo


            if (!IsPostBack)
            {
                BindGrid();


            }
        }

        private void BindGrid()
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());


            this.grid.DataSource = uow.VentasBL.Get(p => p.Status == 3).OrderByDescending(q => q.FechaCancelacion).ToList();
            this.grid.DataBind();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfVentasCancelacionesAdd.aspx");
        }

        protected void imgRPT_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}