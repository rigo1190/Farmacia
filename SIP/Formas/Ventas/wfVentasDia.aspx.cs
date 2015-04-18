using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIP.Formas.Ventas
{
    public partial class wfVentasDia : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            if (!IsPostBack)
            {
                BindGridVentas();
            }
        }


        private void BindGridVentas()
        {
             DateTime fechaFiltro =Convert.ToDateTime(DateTime.Now.ToShortDateString());
             gridVentas.DataSource = uow.VentasBL.Get(e => e.Fecha == fechaFiltro);
             gridVentas.DataBind();

        }

        protected void btnVR_Click(object sender, EventArgs e)
        {
            Response.Redirect("VentasRecetas.aspx");
        }

        protected void btnV_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfVentas.aspx");
        }
    }
}