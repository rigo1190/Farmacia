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
    public partial class wfFacturasAdd : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            if (!IsPostBack)
            {
                //BindGrid();
                //BindCombos();

                //divMsg.Style.Add("display", "none");
            }
        }

    //    #region metodos
    //    private void BindGrid()
    //    {
    //        uow = new UnitOfWork(Session["IdUser"].ToString());


    //        int idUsuario = int.Parse(Session["IdUser"].ToString());

    //        this.grid.DataSource = uow.FacturasAlmacenArticulosTMPBL.Get(p => p.UsuarioId == idUsuario).ToList();
    //        this.grid.DataBind();


    //        List<FacturasAlmacenArticulosTMP> lista;
    //        lista = uow.FacturasAlmacenArticulosTMPBL.Get(p => p.UsuarioId == idUsuario).ToList();
    //        txtImporte.Value = lista.Sum(p => p.Total).ToString();

    //    }

    //    private void BindCombos()
    //    {
            
    //        ddlArticulo.DataSource = uow.ArticulosBL.Get(p => p.Status == 1).ToList().OrderBy(q => q.Nombre);
    //        ddlArticulo.DataValueField = "Id";
    //        ddlArticulo.DataTextField = "Nombre";
    //        ddlArticulo.DataBind();


    //        ddlProveedor.DataSource = uow.ProveedoresBL.Get(p => p.Status == 1).ToList().OrderBy(q => q.RazonSocial);
    //        ddlProveedor.DataValueField = "Id";
    //        ddlProveedor.DataTextField = "RazonSocial";
    //        ddlProveedor.DataBind();

    //    }              
    //#endregion


        //protected void btnAgregar_Click(object sender, EventArgs e)
        //{
        //    FacturasAlmacenArticulosTMP obj;
        //    List<FacturasAlmacenArticulosTMP> lista;
        //    int idArticulo;
        //    int idUsuario = int.Parse(Session["IdUser"].ToString());


            
        //    idArticulo = int.Parse(ddlArticulo.SelectedValue);


        //    lista = uow.FacturasAlmacenArticulosTMPBL.Get(p => p.ArticuloId == idArticulo && p.UsuarioId == idUsuario ).ToList();
        //    if (lista.Count > 0)
        //    {
        //        foreach (FacturasAlmacenArticulosTMP item in lista)
        //            uow.FacturasAlmacenArticulosTMPBL.Delete(item);
                
        //    }

        //    obj = new FacturasAlmacenArticulosTMP();

        //    obj.UsuarioId = idUsuario;
        //    obj.ArticuloId = idArticulo;
        //    obj.Cantidad = int.Parse(txtCantidad.Value.ToString());
        //    obj.Precio = decimal.Parse(txtPrecio.Value);
        //    obj.Subtotal = obj.Precio * decimal.Parse(txtCantidad.Value);

        //    Articulos articulo = uow.ArticulosBL.Get(p => p.Id == idArticulo).First();


        //    if (articulo.esMedicamento == 1)
        //    {
        //        obj.IVA = 0;
        //        obj.Total = obj.Subtotal;
        //    }
        //    else
        //    {
        //        obj.IVA = obj.Subtotal * decimal.Parse("0.16");
        //        obj.Total = obj.Subtotal * decimal.Parse("1.16");
        //    }
                
        //    uow.FacturasAlmacenArticulosTMPBL.Insert(obj);
            

        //    txtCantidad.Value = string.Empty;
        //    txtPrecio.Value = string.Empty;

        //    uow.SaveChanges();
        //    BindGrid();



        //}

        //protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        //{
        //    GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
        //    _ElId.Text = grid.DataKeys[row.RowIndex].Values["Id"].ToString();


        //    if (_ElId.Text == "")
        //        return;

        //    FacturasAlmacenArticulosTMP obj = uow.FacturasAlmacenArticulosTMPBL.GetByID(int.Parse(_ElId.Text));


        //    uow.FacturasAlmacenArticulosTMPBL.Delete(obj);
        //    uow.SaveChanges();
        //    BindGrid();

            
        
        //}

        //protected void btnGuardar_Click(object sender, EventArgs e)
        //{
        //    List<FacturasAlmacenArticulosTMP> lista;
        //    int idUsuario = int.Parse(Session["IdUser"].ToString());

        //    lista = uow.FacturasAlmacenArticulosTMPBL.Get(p => p.UsuarioId == idUsuario).ToList();

        //    if (lista.Count == 0) {
        //        lblMensajes.Text = "Para poder registrar la factura, es necesario tener el detalle de productos que integran la factura";
        //        divMsg.Style.Add("display","block");
        //        return;
        //    }


        //    FacturasAlmacen factura = new FacturasAlmacen();
        //        factura.Ejercicio = DateTime.Now.Year;
        //        factura.FolioFactura = txtFolio.Value;
        //        factura.FechaFactura = DateTime.Parse(dtpFecha.Value);
        //        factura.ImporteFactura = lista.Sum(p => p.Total);
        //        factura.Observaciones = txtObservaciones.Value;
        //        factura.Status = 1;
        //        factura.Tipo = 1;
        //        factura.ProveedorId = int.Parse( ddlProveedor.SelectedValue); 
        //    uow.FacturasAlmacenBL.Insert(factura);


        //    List<ArticulosMovimientos> listaBitacora = uow.ArticulosMovimientosBL.Get().ToList();
        //    int movimiento;
        //    if (listaBitacora.Count == 0)
        //        movimiento = 0;
        //    else
        //        movimiento = listaBitacora.Max(p => p.Movimiento);

        //    movimiento++;

        //    ArticulosMovimientos bitacora = new ArticulosMovimientos();
        //        bitacora.Ejercicio = DateTime.Now.Year;
        //        bitacora.Movimiento = movimiento;
        //        bitacora.Tipo = 1;
        //        bitacora.Fecha = DateTime.Now;
        //        bitacora.Status = 1;
        //        bitacora.FacturaAlmacen = factura;
        //    uow.ArticulosMovimientosBL.Insert(bitacora);

        //    foreach (FacturasAlmacenArticulosTMP item in lista)
        //    {
        //        FacturasAlmacenArticulos detalle = new FacturasAlmacenArticulos();
        //            detalle.FacturaAlmacen = factura;
        //            detalle.ArticuloId = item.ArticuloId;
        //            detalle.Cantidad = item.Cantidad;
        //            detalle.Precio = item.Precio;
        //            detalle.Subtotal = item.Subtotal;
        //            detalle.IVA = item.IVA;
        //            detalle.Total = item.Total;
        //        uow.FacturasAlmacenArticulosBL.Insert(detalle);


        //        ArticulosMovimientosEntradas bitDetalle = new ArticulosMovimientosEntradas();
        //            bitDetalle.ArticuloMovimiento = bitacora;
        //            bitDetalle.ArticuloId = item.ArticuloId;
        //            bitDetalle.Cantidad = item.Cantidad;
        //            bitDetalle.Importe = item.Precio;
        //        uow.ArticulosMovimientosEntradasBL.Insert(bitDetalle);

        //        Articulos articulo = uow.ArticulosBL.GetByID(item.ArticuloId);
        //            articulo.CantidadDisponible = articulo.CantidadDisponible + item.Cantidad;
        //            articulo.CantidadEnAlmacen = articulo.CantidadEnAlmacen + item.Cantidad;
        //            articulo.PrecioCompra = item.Precio;
        //        uow.ArticulosBL.Update(articulo);
        
        //    }

        //    uow.SaveChanges();
        //    Response.Redirect("wfFacturas.aspx");


        //}

        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    int idUsuario = int.Parse(Session["IdUser"].ToString());
        //    List<FacturasAlmacenArticulosTMP> lista;
        //    lista = uow.FacturasAlmacenArticulosTMPBL.Get(p => p.UsuarioId == idUsuario).ToList();

        //    foreach (FacturasAlmacenArticulosTMP item in lista)
        //        uow.FacturasAlmacenArticulosTMPBL.Delete(item);


        //    uow.SaveChanges();

        //    Response.Redirect("wfFacturas.aspx");
        //}
    }
}