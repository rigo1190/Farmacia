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

            this.grid.DataSource = uow.FacturasAlmacenBL.Get();
            this.grid.DataBind();
        }

        protected void imgVer_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            int idUsuario = int.Parse(Session["IdUser"].ToString());
            List<FacturasAlmacenArticulosTMP> lista;
            lista = uow.FacturasAlmacenArticulosTMPBL.Get(p => p.UsuarioId == idUsuario).ToList();

            foreach (FacturasAlmacenArticulosTMP item in lista)
                uow.FacturasAlmacenArticulosTMPBL.Delete(item);

            
            uow.SaveChanges();

            Response.Redirect("wfFacturasAdd.aspx");
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
    }
}