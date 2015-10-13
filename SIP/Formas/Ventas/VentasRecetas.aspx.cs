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
    public partial class VentasRecetas : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            if (!IsPostBack)
            {
                ResetearVenta();
                txtFechaFiltro.Value = DateTime.Now.ToShortDateString();
                BindGridRecetas();
                BindGridProductosCatalago();
                BindDropDownClientes();
            }
        }

        private void OcultarError()
        {
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
            lblMsgError.Text = "";
        }

        private void BindDropDownClientes()
        {
            ddlClientes.DataSource = uow.ClientesBL.Get().ToList();
            ddlClientes.DataValueField = "Id";
            ddlClientes.DataTextField = "RazonSocial";

            ddlClientes.DataBind();
        }
        private void BindGridProductosCatalago()
        {
            var listArt = (from a in uow.ArticulosBL.Get()
                           join um in uow.UnidadesDeMedidaBL.Get()
                           on a.UnidadesDeMedidaId equals um.Id
                           join p in uow.PresentacionesBL.Get()
                           on a.PresentacionId equals p.Id
                           select new { Id = a.Id, Nombre = a.NombreCompleto ,
                                        EsMedicamento = a.esMedicamento,
                                        PrecioVentaIVA = a.PrecioVentaIVA,
                                        Clave = a.Clave,
                                        CantidadDisponible=a.CantidadDisponible
                           }).OrderBy(e=>e.Nombre);


            gridProductosCatalogo.DataSource = listArt.ToList();// uow.ArticulosBL.Get().ToList();
            gridProductosCatalogo.DataBind();
        }
        private void BindGridRecetas()
        {
            DateTime fechaFiltro = DateTime.Now;
            if (!txtFechaFiltro.Value.Equals(string.Empty))
                fechaFiltro = Convert.ToDateTime(txtFechaFiltro.Value);

            gridRecetas.DataSource = uow.RecetasBusinessLogic.Get(e => e.Status == 1 && e.Fecha==fechaFiltro).ToList();
            gridRecetas.DataBind();
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

        private void ResetearVenta()
        {
            

            uow.ArticuloVentaBusinessLogic.DeleteAll();
            uow.SaveChanges();

            _ProductosVenta.Value = string.Empty;
            BindGridProductosVenta();

            //Se limpia el ID de la receta
            _IDReceta.Value = string.Empty;

            //Se limpia la cadena de valores de la lista de productos del catalogo
            _CadValoresSeleccionados.Value = string.Empty;


            DIVdetalleVenta.Style.Add("display","none");
            DIVTotales.Style.Add("display", "none");

            DIVacordeon.Style.Add("display", "none");
            DIVlinkAdd.Style.Add("display", "block");


            //Se limpian los campos de DAtos de la Venta
            txtFolio.Value = string.Empty;
            txtFecha.Value = DateTime.Now.ToShortDateString();
            txtTotalR.Value = "0.00";
            txtIVAR.Value = "0.00";
            txtCobrar.Value = "0.00";

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
                if (!_IDReceta.Value.Equals(string.Empty))
                    venta.RecetaId = Utilerias.StrToInt(_IDReceta.Value);
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
                       


            if (!_IDReceta.Value.Equals(string.Empty))
            {
                int idReceta = Utilerias.StrToInt(_IDReceta.Value);

                Recetas receta = uow.RecetasBusinessLogic.GetByID(idReceta);
                    receta.Status = 2; 
                uow.RecetasBusinessLogic.Update(receta);               
            }

            BindGridProductosCatalago();
            BindGridRecetas();

            uow.SaveChanges();


            if (uow.Errors.Count == 0)
            {
                uow.ArticuloVentaBusinessLogic.DeleteAll();
                uow.SaveChanges();
            }else
            {
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = "Hubo problemas al registrar la venta, intentelo nuevamente";
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

        
        private int ObtenerMaxFolio()
        {
            int max = 1;

            if (uow.VentasBL.Get(e=>e.Ejercicio==DateTime.Now.Year).Count() > 0)
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

        private bool ExisteProductoEnVenta(int idProducto)
        {
            string[] ids = _ProductosVenta.Value.Split('|');

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




        protected void btnProductos_ServerClick(object sender, EventArgs e)
        {
            //se muestra los productos de la receta
            GridViewRow row = (GridViewRow)((HtmlButton)sender).NamingContainer;
            int idReceta = Utilerias.StrToInt(gridRecetas.DataKeys[row.RowIndex].Value.ToString());
            _IDReceta.Value = idReceta.ToString();

            Recetas objReceta = uow.RecetasBusinessLogic.GetByID(idReceta);

            decimal iva=decimal.Parse(Session["IVA"].ToString());

            checkProductos.DataSource = objReceta.detalleReceta.Where(a => a.ArticuloId != null && a.Articulo.CantidadEnAlmacen > 0).Select(a => new {
                                            a.ArticuloId, NombreMedicamento= a.Articulo.esMedicamento==0 ? a.NombreMedicamento + " " + a.Articulo.PrecioVentaIVA.ToString("c") : a.Articulo.NombreCompleto + " " + a.Articulo.PrecioVentaIVA.ToString("c")} ).ToList();

            checkProductos.DataValueField = "ArticuloId";
            checkProductos.DataTextField = "NombreMedicamento";
            checkProductos.DataBind();

            OcultarError();

            //Se oculta el div de las recetas
            divGridRecetas.Style.Add("display", "none");
            divFiltrosRecetas.Style.Add("display", "none");

            DIVdetalleVenta.Style.Add("display", "block");
            DIVTotales.Style.Add("display", "block");

            ClientScript.RegisterStartupScript(this.GetType(), "script", "fnc_MostrarModal()", true);

        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Articulos articulo;
            string M = string.Empty;
            int idArticulo;
            int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());
        
            int idReceta = Utilerias.StrToInt(_IDReceta.Value);

            foreach (ListItem item in checkProductos.Items)
            {
                if (item.Selected)
                {
                    //ddlProductosVenta.Items.Add(new ListItem("Descripcion", item.Value));

                    idArticulo = Utilerias.StrToInt(item.Value);
                    articulo = uow.ArticulosBL.GetByID(idArticulo);

                    if (!_ProductosVenta.Value.Equals(string.Empty))
                    {
                        if (ExisteProductoEnVenta(articulo.Id))
                            continue;
                    }

                    ArticuloVenta artVenta = new ArticuloVenta();
                        artVenta.ArticuloId = idArticulo;
                        artVenta.Cantidad = 1;
                        artVenta.Nombre = articulo.NombreCompleto; 
                        artVenta.PrecioCompra = articulo.PrecioCompraIVA;
                        artVenta.PrecioVenta = articulo.PrecioVentaIVA;
                        artVenta.EsMedicamento = articulo.esMedicamento; 
                        artVenta.RecetaId = idReceta;
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

                        //Se muestra el mensaje de error
                        divMsgError.Style.Add("display", "block");
                        divMsgSuccess.Style.Add("display", "none");
                        lblMsgError.Text = M;

                        return;
                    }

                    if (_ProductosVenta.Value == string.Empty)
                        _ProductosVenta.Value = artVenta.Id.ToString();
                    else
                        _ProductosVenta.Value += "|" + artVenta.Id.ToString();

                    _SeleccionoDeReceta.Value = "S";
                }else
                    _SeleccionoDeReceta.Value = "N";
            }

            if (checkProductos.Items.Count == 0)
                _IDReceta.Value = string.Empty;

            BindGridProductosVenta();

            OcultarError();
        

        }

        private string ConcatenarNombreArticulo(int idArticulo)
        {
            string nombre = string.Empty;

            var art = (from a in uow.ArticulosBL.Get(e=>e.Id==idArticulo)
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

                if (_ProductosVenta.Value == string.Empty)
                    _ProductosVenta.Value = artVenta.Id.ToString();
                else
                    _ProductosVenta.Value += "|" + artVenta.Id.ToString();

            }

            _CadValoresSeleccionados.Value = string.Empty;

            BindGridProductosVenta();

            OcultarError();

            //if (_SeleccionoDeReceta.Value.Equals("S"))
            //    divGridRecetas.Style.Add("display", "none"); //Se oculta el div de las recetas
            //else
            //    divGridRecetas.Style.Add("display", "block");



            DIVacordeon.Style.Add("display", "none");
            DIVlinkAdd.Style.Add("display", "block");
 

        }

        

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            OcultarError();

            BindGridRecetas();

        }

        


        #region EventosDelGrids
        protected void gridProductosCatalogo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                decimal iva = decimal.Parse(Session["IVA"].ToString());
                int id = Utilerias.StrToInt(gridProductosCatalogo.DataKeys[e.Row.RowIndex].Values["Id"].ToString());
                HtmlControl chk = (HtmlControl)e.Row.FindControl("chkSeleccionar");
                Articulos art = uow.ArticulosBL.GetByID(id);
                Label lblPrecio = (Label)e.Row.FindControl("lblPrecio");

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

        protected void gridRecetas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gridRecetas.PageIndex = e.NewPageIndex;
            //BindGridRecetas();
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
                if (!id.Equals(idArticulo))
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

        protected void btnAddProducts_Click(object sender, EventArgs e)
        {
            DIVacordeon.Style.Add("display", "block");
            DIVlinkAdd.Style.Add("display", "none");
        }

    }


}