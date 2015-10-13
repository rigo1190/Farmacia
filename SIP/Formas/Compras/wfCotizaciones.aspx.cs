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
    public partial class wfCotizaciones : System.Web.UI.Page
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
                _URLVisor.Value = ResolveClientUrl("~/rpts/wfVerReporte.aspx");
            }
        }


        private void BindGrid()
        {
            
            

            int idusuario = int.Parse(Session["IdUser"].ToString());

            int ejercicio = DateTime.Now.Year;

            uow = new UnitOfWork(Session["IdUser"].ToString());

            this.grid.DataSource = uow.CotizacionesBL.Get(p=>p.Ejercicio== ejercicio).OrderByDescending(p=>p.IdFolio).ToList();
            this.grid.DataBind();
        }



        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfCotizacionesAdd.aspx");
        }

        protected void imgRPT_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                int id = Utilerias.StrToInt(grid.DataKeys[e.Row.RowIndex].Values["Id"].ToString());


                ImageButton imgBut = (ImageButton)e.Row.FindControl("imgRPT");
                if (imgBut != null)
                    imgBut.Attributes["onclick"] = "fnc_AbrirReporte(" + id + ");return false;";


                ImageButton imgButPedido = (ImageButton)e.Row.FindControl("imgRptPedidos");
                if (imgButPedido != null)
                    imgButPedido.Attributes["onclick"] = "fnc_VerPedidos(" + id + ");return false;";


                


                LinkButton linkPedido = (LinkButton)e.Row.FindControl("linkPedidos");
                linkPedido.Text = "Pendiente";

                List<Pedidos> listaPedidos = uow.PedidosBL.Get(p => p.CotizacionId == id).ToList();



                if (listaPedidos.Count > 0)
                {

                
                
                string cad = " ";
                foreach (Pedidos item in listaPedidos)
                {
                    cad = cad + item.IdFolio + "-";
                }
                cad = cad.Substring(0, cad.Length - 1);

                linkPedido.Text = "Pedidos : " + cad;

                }



                




            }
        }

       
        protected void linkPedidos_Click(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;

            Session["XCotizacionId"] = grid.DataKeys[row.RowIndex].Values["Id"].ToString();

            Response.Redirect("wfPedidos.aspx");
        }

        protected void imgRptPedidos_Click(object sender, ImageClickEventArgs e)
        {

        }



    }
}