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
    public partial class wfFacturasNueva : System.Web.UI.Page
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
                BindGridPedidos();
                divPedidos.Style.Add("display", "block");
                divFactura.Style.Add("display", "none");

                divCantidadExtra.Style.Add("display","none");
                
                ddlModo.DataSource = uow.FacturaModoEntradaBL.Get();
                ddlModo.DataValueField = "Id";
                ddlModo.DataTextField = "Nombre";
                ddlModo.DataBind();

                //_URLVisor.Value = ResolveClientUrl("~/rpts/wfVerReporte.aspx");
            }
        }



        private void BindGridPedidos()
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            this.grid.DataSource = uow.PedidosBL.Get(p=>p.Status == 1).ToList();
            this.grid.DataBind();
        }


        private void BindGridDetalle()
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            int idPedido = int.Parse(_idPedido.Value);
            this.gridDetalle.DataSource = uow.FacturasAlmacenArticulosTMPBL.Get(p => p.PedidoId == idPedido && p.Precio > 0).ToList();
            this.gridDetalle.DataBind();


            List<FacturasAlmacenArticulosTMP> lista = uow.FacturasAlmacenArticulosTMPBL.Get(p => p.PedidoId == idPedido && p.Precio == 0 && p.Cantidad > 0).ToList();
            var listaArticulos = from cot in lista
                                 join art in uow.ArticulosBL.Get(p => p.Status == 1).ToList().OrderBy(q => q.Nombre)
                                 on cot.ArticuloId equals art.Id
                                 select art;


            ddlArticulo.DataSource = listaArticulos;// uow.ArticulosBL.Get(p => p.Status == 1).ToList().OrderBy(q => q.Nombre);
            ddlArticulo.DataValueField = "Id";
            ddlArticulo.DataTextField = "NombreCompleto";
            ddlArticulo.DataBind();

            if (ddlArticulo.Items.Count == 0) {
                DIVagregar.Style.Add("display","none");
                
                if (gridDetalle.Rows.Count != 0)
                    DIVCerrarProceso.Style.Add("display", "block");
                    
            }
            else
            {
                DIVagregar.Style.Add("display", "block");
                DIVCerrarProceso.Style.Add("display", "none");
            }


            lista = uow.FacturasAlmacenArticulosTMPBL.Get(p => p.PedidoId == idPedido && p.Precio > 0).ToList();
            txtImporte.Value = lista.Sum(p => p.Total).ToString("C2");




        }



        protected void LinkIniciar_Click(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;

           int idPedido  =int.Parse( grid.DataKeys[row.RowIndex].Values["Id"].ToString());


           _idPedido.Value = idPedido.ToString();
           divPedidos.Style.Add("display","none");
           divFactura.Style.Add("display", "block");

           Pedidos pedido = uow.PedidosBL.GetByID(idPedido);
           txtProveedor.Value = pedido.Proveedor.RazonSocial;

           List<FacturasAlmacenArticulosTMP> lista;
           lista = uow.FacturasAlmacenArticulosTMPBL.Get(p => p.PedidoId == idPedido).ToList();

           foreach (FacturasAlmacenArticulosTMP item in lista)
               uow.FacturasAlmacenArticulosTMPBL.Delete(item);


           List<PedidosArticulos> detallePedido = uow.PedidosArticulosBL.Get(p=>p.PedidoId == idPedido).ToList();

           foreach (PedidosArticulos item in detallePedido)
           {
               FacturasAlmacenArticulosTMP obj = new FacturasAlmacenArticulosTMP();
                   obj.PedidoId = idPedido;
                   obj.ArticuloId = item.ArticuloId;
                   obj.Cantidad = item.Cantidad;
                   obj.CantidadExtra =0;
                   obj.Precio = 0;
                   obj.Subtotal = 0;
                   obj.IVA=0;
                   obj.Total=0;
               uow.FacturasAlmacenArticulosTMPBL.Insert(obj);

           }


           uow.SaveChanges();

           BindGridDetalle();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try { 

            int idArticulo = int.Parse(ddlArticulo.SelectedValue);
            int idPedido = int.Parse(_idPedido.Value);
            int CantidadExtra=0;

            Articulos articulo = uow.ArticulosBL.GetByID(idArticulo);

            FacturasAlmacenArticulosTMP obj = uow.FacturasAlmacenArticulosTMPBL.Get(p => p.PedidoId == idPedido && p.ArticuloId == idArticulo).First();


            try {
                if (chkExtras.Checked)                
                    CantidadExtra = int.Parse(txtCantidadExtra.Value);
            }
            catch
            {
                CantidadExtra = 0;
            }
            obj.CantidadExtra = CantidadExtra;

            int idModo = int.Parse(ddlModo.SelectedValue);
            FacturaModoEntrada mode = uow.FacturaModoEntradaBL.GetByID(idModo);

            decimal factorIVA = decimal.Parse(Session["IVA"].ToString());
            factorIVA++;



            if (articulo.esMedicamento == 1)
            {
                obj.Precio = decimal.Parse(txtPrecio.Value);
                obj.PrecioIVA = decimal.Parse(txtPrecio.Value);
                obj.Subtotal = obj.Precio * decimal.Parse(obj.Cantidad.ToString());
                obj.Total = obj.Precio * decimal.Parse(obj.Cantidad.ToString());
                obj.IVA = 0;

            }
            else
            {

                if (mode.incluyeIVA == 1)
                {
                    obj.Precio = decimal.Parse(txtPrecio.Value) / factorIVA;
                    obj.PrecioIVA = decimal.Parse(txtPrecio.Value);
                    obj.Subtotal = obj.Precio * decimal.Parse(obj.Cantidad.ToString());
                    obj.Total = obj.PrecioIVA * decimal.Parse(obj.Cantidad.ToString());
                    obj.IVA = obj.Total - obj.Subtotal;
                }
                else
                {

                    obj.Precio = decimal.Parse(txtPrecio.Value);
                    obj.PrecioIVA = decimal.Parse(txtPrecio.Value) * factorIVA;
                    obj.Subtotal = obj.Precio * decimal.Parse(obj.Cantidad.ToString());
                    obj.Total = obj.PrecioIVA * decimal.Parse(obj.Cantidad.ToString());
                    obj.IVA = obj.Total - obj.Subtotal;
                }

                

            } 

                            
            

            uow.FacturasAlmacenArticulosTMPBL.Update(obj);
            uow.SaveChanges();
            BindGridDetalle();
            txtPrecio.Value = string.Empty;
            txtCantidadExtra.Value = string.Empty;
            divCantidadExtra.Style.Add("display", "none");
            chkExtras.Checked = false;

            }
            catch { }

        }


        protected void btnDescartar_Click(object sender, EventArgs e)
        {
            int idArticulo = int.Parse(ddlArticulo.SelectedValue);
            int idPedido = int.Parse(_idPedido.Value); 

            FacturasAlmacenArticulosTMP obj = uow.FacturasAlmacenArticulosTMPBL.Get(p => p.PedidoId == idPedido && p.ArticuloId == idArticulo).First();

            obj.Cantidad = 0;
            uow.FacturasAlmacenArticulosTMPBL.Update(obj);
            uow.SaveChanges();
            BindGridDetalle();
        }



        protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            int id = int.Parse(gridDetalle.DataKeys[row.RowIndex].Values["Id"].ToString());


            FacturasAlmacenArticulosTMP obj = uow.FacturasAlmacenArticulosTMPBL.GetByID(id);
            obj.Precio = 0;
            obj.Subtotal = 0;
            obj.IVA = 0;
                obj.Total=0;

                uow.FacturasAlmacenArticulosTMPBL.Update(obj);
            uow.SaveChanges();
            BindGridDetalle();
        }

  

        protected void btnGuardarFactura_Click(object sender, EventArgs e)
        {


            List<FacturasAlmacenArticulosTMP> lista;
            int idPedido = int.Parse(_idPedido.Value);

            lista = uow.FacturasAlmacenArticulosTMPBL.Get(p => p.PedidoId == idPedido && p.Precio == 0 && p.Cantidad > 0).ToList();

            if (lista.Count > 0)
            {
                lblMensajes.Text = "Para poder registrar la factura, es necesario registrar todos los productos";
                divMsg.Style.Add("display", "block");
                return;
            }

            if (txtFolio.Value == string.Empty)
            {
               // lblMensajes.Text = "No ha capturado el Número de Factura, capture ese dato para poder registrar la entrada";
               // divMsg.Style.Add("display", "block");
                return;
            }

            if (dtpFecha.Value.ToString() == string.Empty)
            {
                //lblMensajes.Text = "No ha indicado la fecha de la Factura, capture ese dato para poder registrar la entrada";
                //divMsg.Style.Add("display", "block");
                return;
            }


            lista = uow.FacturasAlmacenArticulosTMPBL.Get(p => p.PedidoId == idPedido && p.Precio > 0 && p.Cantidad > 0).ToList();

            Pedidos pedido = uow.PedidosBL.GetByID(idPedido);

            FacturasAlmacen factura = new FacturasAlmacen();
                factura.Ejercicio = DateTime.Now.Year;
                factura.FolioFactura = txtFolio.Value;
                factura.FechaFactura = DateTime.Parse(dtpFecha.Value);
                factura.ImporteFactura = lista.Sum(p => p.Total);
                factura.Observaciones = "";
                factura.Status = 1;
                factura.Tipo = 1;
                factura.PedidoId = pedido.Id;
                factura.ProveedorId = pedido.ProveedorId;
            uow.FacturasAlmacenBL.Insert(factura);


            List<ArticulosMovimientos> listaBitacora = uow.ArticulosMovimientosBL.Get().ToList();
            int movimiento;
            if (listaBitacora.Count == 0)
                movimiento = 0;
            else
                movimiento = listaBitacora.Max(p => p.Movimiento);

            movimiento++;

            ArticulosMovimientos bitacora = new ArticulosMovimientos();
            bitacora.Ejercicio = DateTime.Now.Year;
            bitacora.Movimiento = movimiento;
            bitacora.Tipo = 1;
            bitacora.Fecha = DateTime.Now;
            bitacora.Status = 1;
            bitacora.FacturaAlmacen = factura;
            uow.ArticulosMovimientosBL.Insert(bitacora);

            foreach (FacturasAlmacenArticulosTMP item in lista)
            {
                FacturasAlmacenArticulos detalle = new FacturasAlmacenArticulos();
                detalle.FacturaAlmacen = factura;
                detalle.ArticuloId = item.ArticuloId;
                detalle.Cantidad = item.Cantidad;
                detalle.Adicional = item.CantidadExtra;
                
                detalle.Precio = item.Precio;
                detalle.PrecioIVA = item.PrecioIVA;

                detalle.Subtotal = item.Subtotal;
                detalle.IVA = item.IVA;
                detalle.Total = item.Total;
                detalle.PrecioDeCompraAnterior = item.Articulo.PrecioCompraIVA;
                detalle.PrecioVenta = item.Articulo.PrecioVentaIVA;
                detalle.Diferencia = item.PrecioIVA - item.Articulo.PrecioCompraIVA;
                detalle.Status = 1;
                detalle.StatusNombre = "Pendiente";
                uow.FacturasAlmacenArticulosBL.Insert(detalle);


                ArticulosMovimientosEntradas bitDetalle = new ArticulosMovimientosEntradas();
                bitDetalle.ArticuloMovimiento = bitacora;
                bitDetalle.ArticuloId = item.ArticuloId;
                bitDetalle.Cantidad = item.Cantidad + item.CantidadExtra;
                bitDetalle.Importe = item.PrecioIVA;
                uow.ArticulosMovimientosEntradasBL.Insert(bitDetalle);

                Articulos articulo = uow.ArticulosBL.GetByID(item.ArticuloId);
                articulo.CantidadDisponible = articulo.CantidadDisponible + (item.Cantidad + item.CantidadExtra);
                articulo.CantidadEnAlmacen = articulo.CantidadEnAlmacen + (item.Cantidad + item.CantidadExtra);
                articulo.PrecioCompra = item.Precio;
                articulo.PrecioCompraIVA = item.PrecioIVA;
                uow.ArticulosBL.Update(articulo);

            }


            pedido.Status = 2;
            uow.PedidosBL.Update(pedido);


            uow.SaveChanges();
            Response.Redirect("wfFacturas.aspx");

        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfFacturas.aspx");
        }



    }
}