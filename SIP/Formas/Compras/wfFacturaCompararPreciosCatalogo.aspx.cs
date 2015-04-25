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
    public partial class wfFacturaCompararPreciosCatalogo : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            if (!IsPostBack)
            {
                BindGrid();
                //_URLVisor.Value = ResolveClientUrl("~/rpts/wfVerReporte.aspx");
                divModificar.Style.Add("display", "none");
            }
        }


        private void BindGrid()
        {
            int idFactura = int.Parse(Session["XFacturaId"].ToString());
            uow = new UnitOfWork(Session["IdUser"].ToString());

            this.grid.DataSource = uow.FacturasAlmacenArticulosBL.Get(p => p.FacturaAlmacenId == idFactura).ToList();
            this.grid.DataBind();
        }

        protected void linkModificar_Click(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;

           _ElId.Value = grid.DataKeys[row.RowIndex].Values["Id"].ToString();

            divModificar.Style.Add("display", "block");
        }

        protected void btnGuardarFactura_Click(object sender, EventArgs e)
        {
            int id = int.Parse(_ElId.Value);

            uow = new UnitOfWork(Session["IdUser"].ToString());

            FacturasAlmacenArticulos obj = uow.FacturasAlmacenArticulosBL.GetByID(id);
            Articulos articulo = uow.ArticulosBL.GetByID(obj.ArticuloId);

            obj.PrecioVenta = decimal.Parse(txtPrecioVenta.Value);

            decimal factorIVA = decimal.Parse(Session["IVA"].ToString());
            factorIVA++;

            articulo.PrecioVenta = Math.Round( decimal.Parse(txtPrecioVenta.Value) / factorIVA,2);
            articulo.PrecioVentaIVA = decimal.Parse(txtPrecioVenta.Value);
            

            uow.FacturasAlmacenArticulosBL.Update(obj);
            uow.ArticulosBL.Update(articulo);

            uow.SaveChanges();

            divModificar.Style.Add("display", "none");
            BindGrid();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            divModificar.Style.Add("display", "none");
        }



    }
}