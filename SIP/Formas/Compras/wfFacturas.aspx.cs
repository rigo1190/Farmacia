using BusinessLogicLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SIP.Formas.Compras
{
    public partial class wfFacturas : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            if (!IsPostBack)
            {
                BindGrid();
                _URLVisor.Value = ResolveClientUrl("~/rpts/wfVerReporte.aspx");
            }
        }

        private void BindGrid()
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            this.grid.DataSource = uow.FacturasAlmacenBL.Get().OrderByDescending(p=>p.Id).ToList();
            this.grid.DataBind();
        }

        protected void imgVer_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            

            //Response.Redirect("wfFacturasAdd.aspx");
            Response.Redirect("wfFacturasNueva.aspx");
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                int id = Utilerias.StrToInt(grid.DataKeys[e.Row.RowIndex].Values["Id"].ToString());

                



                ImageButton imgBut = (ImageButton)e.Row.FindControl("imgVer");
                if (imgBut != null)
                    imgBut.Attributes["onclick"] = "fnc_AbrirReporte(" + id + ");return false;";






                 





               


            }
        }

        protected void linkPrecios_Click(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;

            Session["XFacturaId"] = grid.DataKeys[row.RowIndex].Values["Id"].ToString();

            Response.Redirect("wfFacturaCompararPreciosCatalogo.aspx");
        }
    }
}