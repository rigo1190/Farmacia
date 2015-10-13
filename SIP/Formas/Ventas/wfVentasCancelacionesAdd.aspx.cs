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
    public partial class wfVentasCancelacionesAdd : System.Web.UI.Page
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

            divCancelacion.Style.Add("display", "none");

            if (!IsPostBack)
            {
                BindGrid();


            }
        }

        private void BindGrid()
        {
            DateTime fecha = DateTime.Now;

            List<DataAccessLayer.Models.Ventas> lista = uow.VentasBL.Get(p => p.Status == 1).OrderByDescending(q=>q.Id).ToList();

            grid.DataSource = lista;
            grid.DataBind();


        }

        protected void linkVenta_Click(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;

            int idVenta = int.Parse(grid.DataKeys[row.RowIndex].Values["Id"].ToString());

            _idVenta.Value = idVenta.ToString();

            divVentas.Style.Add("display", "none");
            divCancelacion.Style.Add("display","block");


        }


        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            BindGrid();
            
            divVentas.Style.Add("display","block");
            divCancelacion.Style.Add("display","none");
            _idVenta.Value = string.Empty;
            Response.Redirect("wfVentasCancelaciones.aspx");
        }

        

        protected void btnCancelarVenta_Click(object sender, EventArgs e)
        {
            
            
            int iduser = int.Parse(Session["IdUser"].ToString());
            Usuario usuario = uow.UsuarioBusinessLogic.GetByID(iduser);

            DataAccessLayer.Models.Ventas venta = uow.VentasBL.GetByID(int.Parse(_idVenta.Value));
                venta.Status = 3;
                venta.ObservacionCancelacion = txtObservaciones.Value;
                venta.UsuarioCancelacion = usuario.Nombre;
                venta.FechaCancelacion = DateTime.Now;
            uow.VentasBL.Update(venta);



            ArticulosMovimientos bitacora = uow.ArticulosMovimientosBL.Get(p => p.VentaId == venta.Id).First();
                bitacora.Status = 3;
            uow.ArticulosMovimientosBL.Update(bitacora);

            List<VentasArticulos> lista = uow.VentasArticulosBL.Get(p => p.VentaId == venta.Id).ToList();

            foreach (VentasArticulos item in lista)
            {
                Articulos articulo = uow.ArticulosBL.GetByID(item.ArticuloId);
                    articulo.CantidadDisponible = articulo.CantidadDisponible + item.Cantidad;
                    articulo.CantidadEnAlmacen = articulo.CantidadEnAlmacen + item.Cantidad;
                uow.ArticulosBL.Update(articulo);


            }


            uow.SaveChanges();


            BindGrid();
            txtObservaciones.Value = string.Empty;
            divVentas.Style.Add("display", "block");
            divCancelacion.Style.Add("display", "none");
            _idVenta.Value = string.Empty;

            Response.Redirect("wfVentasCancelaciones.aspx");

        }







    }
}