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
    public partial class wfVentas : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            int idUsuario = int.Parse(Session["IdUser"].ToString());
            Usuario user = uow.UsuarioBusinessLogic.GetByID(idUsuario);

            if (user.EsAdmin)
                DIVFechaVenta.Style.Add("display", "block");
            else
                DIVFechaVenta.Style.Add("display", "none");

                if (!IsPostBack)
                {
                    ResetearVenta();
                    txtFecha.Value = DateTime.Now.ToShortDateString();
                    BindGridProductosCatalago();
                }

        }



        private void BindGridProductosVenta()
        {
            int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());

            List<ArticuloVenta> listArticulos = uow.ArticuloVentaBusinessLogic.Get(p => p.UsuarioId == idUser).ToList();
            gridProductos.DataSource = listArticulos;
            gridProductos.DataBind();


            if (listArticulos.Count > 0)
            {
                decimal total = listArticulos.Sum(e => e.SubTotal);
                decimal iva = listArticulos.Sum(e => e.IVA);
                decimal cobrar = listArticulos.Sum(e => e.Total);


                txtTotalR.Value = total.ToString("c");
                txtIVAR.Value = iva.ToString("c");
                txtCobrar.Value = cobrar.ToString("c");



            }
            else
            {
                txtTotalR.Value = "0.00";
                txtIVAR.Value = "0.00";
                txtCobrar.Value = "0.00";
            }

            
        }
        private void BindGridProductosCatalago()
        {
            var listArt = (from a in uow.ArticulosBL.Get()
                           join um in uow.UnidadesDeMedidaBL.Get()
                           on a.UnidadesDeMedidaId equals um.Id
                           join p in uow.PresentacionesBL.Get()
                           on a.PresentacionId equals p.Id
                           select new
                           {
                               Id = a.Id,
                               Nombre = a.NombreCompleto,
                               EsMedicamento = a.esMedicamento,
                               PrecioVentaIVA = a.PrecioVentaIVA,
                               Clave = a.Clave,
                               CantidadDisponible = a.CantidadDisponible
                               
                           }).OrderBy(e => e.Nombre);


            gridProductosCatalogo.DataSource = listArt.ToList();// uow.ArticulosBL.Get().ToList();
            gridProductosCatalogo.DataBind();
        }


               
        private void ResetearVenta()
        {
            uow.ArticuloVentaBusinessLogic.DeleteAll();
            uow.SaveChanges();
            
            
            //Se bindea el grid
            _ProductosVenta.Value = string.Empty;
            BindGridProductosVenta();

            //Se limpia la cadena de valores de la lista de productos del catalogo
            _CadValoresSeleccionados.Value = string.Empty;

            //Se limpian los campos de DAtos de la Venta
            txtTotalR.Value = "0.00";
            txtIVAR.Value = "0.00";
            txtCobrar.Value = "0.00";

             
        }

        

        


        

        private int ObtenerMaxFolio()
        {
            int max = 1;

            if (uow.VentasBL.Get().Count(e=>e.Ejercicio==DateTime.Now.Year) > 0)
                max = uow.VentasBL.Get().Max(e => e.Folio) + 1;

            return max;

        }

        private string ArmarFolioCadena(int folio)
        {
            string folioCad = string.Empty;
            string num = string.Format("{0:0000}", folio);
            folioCad = "VEN/" + num + "/" + DateTime.Now.Year;

            return folioCad;
        }

                

        private string ConcatenarNombreArticulo(int idArticulo)
        {
            string nombre = string.Empty;

            var art = (from a in uow.ArticulosBL.Get(e => e.Id == idArticulo)
                       join um in uow.UnidadesDeMedidaBL.Get()
                       on a.UnidadesDeMedidaId equals um.Id
                       join p in uow.PresentacionesBL.Get()
                       on a.PresentacionId equals p.Id
                       select new
                       {
                           Nombre = a.Nombre + " (" + p.Nombre + " " + a.CantidadUnidadMedida + " " + um.Nombre + ")",
                       }).FirstOrDefault();

            nombre = art.Nombre;

            return nombre;

        }
        private void OcultarError()
        {
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
            lblMsgError.Text = "";
        }

        


        private bool ExisteProductoEnVenta(int idProducto)
        {
            string [] ids=_ProductosVenta.Value.Split('|');

            foreach (string id in ids)
            {
                int idArtVenta = Utilerias.StrToInt(id);
                ArticuloVenta artVenta = uow.ArticuloVentaBusinessLogic.Get(e => e.Id == idArtVenta && e.ArticuloId == idProducto).FirstOrDefault();

                if (artVenta != null)
                    return true;
                else
                    continue;
            }

            return false;

        }


        

        protected void btnAgregarDeCat_Click(object sender, EventArgs e)
        {
            string cadenaValores = _CadValoresSeleccionados.Value;

            if (cadenaValores.Equals(string.Empty))
                return;

            string[] ids = cadenaValores.Split('|');
            int idProducto;
            Articulos articulo;
            string M = string.Empty;
            int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());
            


            foreach (string id in ids)
            {

                //Se valida que el articulo seleccionado no este agregado ya
                //al grid de las ventas
                idProducto = Utilerias.StrToInt(id);
                articulo = uow.ArticulosBL.GetByID(idProducto);

                if (!_ProductosVenta.Value.Equals(string.Empty))
                {
                    if (ExisteProductoEnVenta(articulo.Id))
                        continue;
                }

                ArticuloVenta artVenta = new ArticuloVenta();
                    artVenta.ArticuloId = idProducto;
                    artVenta.Cantidad = 1;
                    artVenta.Nombre = articulo.NombreCompleto;
                    artVenta.PrecioCompra = articulo.PrecioCompraIVA;
                    artVenta.PrecioVenta = articulo.PrecioVentaIVA;
                    artVenta.EsMedicamento = articulo.esMedicamento;
                    artVenta.UsuarioId = idUser;

                    artVenta.SubTotal = articulo.PrecioVenta;
                    artVenta.IVA = articulo.PrecioVentaIVA - articulo.PrecioVenta;
                    artVenta.Total = articulo.PrecioVentaIVA;

                uow.ArticuloVentaBusinessLogic.Insert(artVenta);
                uow.SaveChanges();

                //SI HUBO ERORRES 
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    //Se muestra el error 

                    divMsgError.Style.Add("display", "block");
                    divMsgSuccess.Style.Add("display", "none");
                    lblMsgError.Text = M;
                    return;
                }

                //Se agrega a la lista de productos
                if (_ProductosVenta.Value == string.Empty)
                    _ProductosVenta.Value = artVenta.Id.ToString();
                else
                    _ProductosVenta.Value += "|" + artVenta.Id.ToString();

            }

            _CadValoresSeleccionados.Value = string.Empty;

            BindGridProductosVenta();

            OcultarError();
        }
        
        protected void btnAceptarVenta_Click(object sender, EventArgs e)
        {
            string pass = txtPassword.Text;
            Usuario user = uow.UsuarioBusinessLogic.Get(u => u.Password == pass && u.Activo == true).FirstOrDefault();





            if (user == null)
            {
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = "El password es incorrecto, intente de nuevo.";

                return;
            }


            //detalle de la venta
            int usuarioSesion = int.Parse(Session["IdUser"].ToString());
            List<ArticuloVenta> listArticulos = uow.ArticuloVentaBusinessLogic.Get(p => p.UsuarioId == usuarioSesion).ToList();


            if (listArticulos.Count == 0)
            {
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = "La venta no ha sido detallada, agregue productos a la venta para poder proceder a registrarla";

                return;
            }


        


            DataAccessLayer.Models.Ventas venta = new DataAccessLayer.Models.Ventas();
            venta.Folio = ObtenerMaxFolio();
            venta.Ejercicio = DateTime.Now.Year;
            venta.FolioCadena = ArmarFolioCadena(venta.Folio);            
            venta.Importe = listArticulos.Sum(p => p.Total);
            venta.Status = 1;
            venta.Fecha = Convert.ToDateTime(txtFecha.Value);
            venta.UsuarioId = user.Id;
            venta.FechaCancelacion = DateTime.Now;
            uow.VentasBL.Insert(venta);


            //bitacora
            List<ArticulosMovimientos> listaBitacora = uow.ArticulosMovimientosBL.Get().ToList();
            int movimiento;
            if (listaBitacora.Count == 0)
                movimiento = 0;
            else
                movimiento = listaBitacora.Max(p => p.Movimiento);

            movimiento++;

            ArticulosMovimientos bitacora = new ArticulosMovimientos();
            bitacora.Ejercicio = DateTime.Now.Year;
            bitacora.Tipo = 2;
            bitacora.Venta = venta;
            bitacora.Fecha = DateTime.Now;
            bitacora.Status = 1;
            bitacora.Movimiento = movimiento;
            uow.ArticulosMovimientosBL.Insert(bitacora);




            foreach (ArticuloVenta item in listArticulos)
            {
                VentasArticulos ventaArt = new VentasArticulos();
                ventaArt.VentaId = venta.Id;
                ventaArt.ArticuloId = item.ArticuloId;
                ventaArt.Cantidad = item.Cantidad;
                ventaArt.PrecioCompra = item.PrecioCompra;
                ventaArt.PrecioVenta = item.PrecioVenta;
                ventaArt.Subtotal = item.SubTotal;
                ventaArt.IVA = item.IVA;
                ventaArt.Total = item.Total;
                uow.VentasArticulosBL.Insert(ventaArt);
                
                ArticulosMovimientosSalidas bitDetalle = new ArticulosMovimientosSalidas();
                bitDetalle.ArticuloMovimiento = bitacora;
                bitDetalle.ArticuloId = item.ArticuloId;
                bitDetalle.Cantidad = item.Cantidad;
                uow.ArticulosMovimientosSalidasBL.Insert(bitDetalle);
                
                Articulos articulo = uow.ArticulosBL.GetByID(item.ArticuloId);
                articulo.CantidadEnAlmacen -= item.Cantidad;
                articulo.CantidadDisponible -= item.Cantidad;
                uow.ArticulosBL.Update(articulo);
            }



            

            
            

            uow.SaveChanges();
            BindGridProductosCatalago();

            if (uow.Errors.Count == 0)
            {
                uow.ArticuloVentaBusinessLogic.DeleteAll();
                uow.SaveChanges();
            }
            else
            {

                string mensaje;
                mensaje = string.Empty;
                foreach (string cad in uow.Errors)
                    mensaje = mensaje + cad + "<br>";

                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = mensaje;
                return;
            }

            //ClientScript.RegisterStartupScript(this.GetType(), "script", "fnc_MostrarVenta(" + venta.Id + ")", true);
            Response.Redirect("wfVentasDia.aspx");
        }

        protected void btnCancelarVenta_Click(object sender, EventArgs e)
        {
            uow.ArticuloVentaBusinessLogic.DeleteAll();
            uow.SaveChanges();
            Response.Redirect("wfVentasDia.aspx");
        }

        
        
        


#region EventosDelGrid
        protected void gridProductosCatalogo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                decimal iva = decimal.Parse(Session["IVA"].ToString());
                int id = Utilerias.StrToInt(gridProductosCatalogo.DataKeys[e.Row.RowIndex].Values["Id"].ToString());
                HtmlControl chk = (HtmlControl)e.Row.FindControl("chkSeleccionar");
                Label lblPrecio = (Label)e.Row.FindControl("lblPrecio");

                Articulos art = uow.ArticulosBL.GetByID(id);

                if (art.CantidadDisponible <= 0)
                {
                    if (chk != null)
                    {
                        chk.Disabled = true;
                    }
                }

                lblPrecio.Text = (art.PrecioVentaIVA).ToString("c");
                    
            }
        }


        protected void gridProductos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridProductos.EditIndex = -1;
            BindGridProductosVenta();

            OcultarError();
        }

        protected void gridProductos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string M = string.Empty;
            int idArticulo = Utilerias.StrToInt(gridProductos.DataKeys[e.RowIndex].Value.ToString());
            ArticuloVenta objArticulo = uow.ArticuloVentaBusinessLogic.GetByID(idArticulo);
            decimal subtotal;
            decimal iva = 0;
            decimal total;

            int cantidad = Utilerias.StrToInt(((HtmlInputGenericControl)gridProductos.Rows[e.RowIndex].FindControl("txtCantidad")).Value);

            if (cantidad == 0)
                cantidad = 1;

            Articulos artCat = uow.ArticulosBL.GetByID(objArticulo.ArticuloId);

            if (cantidad > artCat.CantidadDisponible)
            {
                //gridProductos.EditIndex = -1;
                //BindGridProductosVenta();
                //return;
                cantidad = artCat.CantidadDisponible;
            }


            subtotal = (cantidad) * (artCat.PrecioVenta);
            total = (cantidad) * (artCat.PrecioVentaIVA);
            iva = total - subtotal;

            objArticulo.Cantidad = cantidad;
            objArticulo.SubTotal = subtotal;
            objArticulo.IVA = iva;
            objArticulo.Total = total;


            uow.ArticuloVentaBusinessLogic.Update(objArticulo);
            uow.SaveChanges();

            //SI HUBO ERORRES 
            if (uow.Errors.Count > 0)
            {
                foreach (string m in uow.Errors)
                    M += m;

                //Se muestra el error 
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = M;

                return;
            }

            // Cancelamos la edicion del grid
            gridProductos.EditIndex = -1;
            BindGridProductosVenta();
            OcultarError();



        }

        protected void gridProductos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridProductos.EditIndex = e.NewEditIndex;
            BindGridProductosVenta();

            OcultarError();
        }


        protected void gridProductos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string M = string.Empty;
            int idArticulo = Utilerias.StrToInt(gridProductos.DataKeys[e.RowIndex].Value.ToString());
            ArticuloVenta objArticulo = uow.ArticuloVentaBusinessLogic.GetByID(idArticulo);
            string[] idsProducto = _ProductosVenta.Value.Split('|');

            uow.ArticuloVentaBusinessLogic.Delete(objArticulo);
            uow.SaveChanges();

            //SI HUBO ERORRES AL ELIMINAR REGISTROS PREVIOS
            if (uow.Errors.Count > 0)
            {
                foreach (string m in uow.Errors)
                    M += m;

                //Se muestra el error
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = M;

                return;
            }

            _ProductosVenta.Value = string.Empty;

            foreach (string id in idsProducto)
            {
                if (!id.Equals(idArticulo.ToString()))
                {
                    if (_ProductosVenta.Value.Equals(string.Empty))
                        _ProductosVenta.Value = id;
                    else
                        _ProductosVenta.Value += "|" + id;
                }
            }

            BindGridProductosVenta();

            OcultarError();
        }

        protected void gridProductosCatalogo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridProductosCatalogo.PageIndex = e.NewPageIndex;
            BindGridProductosCatalago();
        }

#endregion


    }
}