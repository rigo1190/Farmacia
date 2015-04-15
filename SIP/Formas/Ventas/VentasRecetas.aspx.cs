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


        private string EliminarArticuloVenta()
        {
            int idUser=Utilerias.StrToInt(Session["IdUser"].ToString());

            bool eliminados=uow.ArticuloVentaBusinessLogic.DeleteAll(e=>e.UsuarioId==idUser);
            string M=string.Empty;

            if (eliminados)
            {
                uow.SaveChanges();

                //SI HUBO ERORRES AL ELIMINAR REGISTROS PREVIOS
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    return M;
                }
            }

            return M;
        }

        private void BindGridProductosCatalago()
        {
            var listArt = (from a in uow.ArticulosBL.Get()
                           join um in uow.UnidadesDeMedidaBL.Get()
                           on a.UnidadesDeMedidaId equals um.Id
                           join p in uow.PresentacionesBL.Get()
                           on a.PresentacionId equals p.Id
                           select new { Id = a.Id, Nombre = a.Nombre + " " + um.Nombre + " " + p.Nombre + " " + a.Porcentaje,
                                        EsMedicamento = a.esMedicamento,
                                        PrecioVenta = a.PrecioVenta,
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
            List<ArticuloVenta> listArticulos = uow.ArticuloVentaBusinessLogic.Get(e => e.UsuarioId == idUser).OrderBy(e => e.Nombre).ToList();


            //Llenar los campos de totales
            decimal total = listArticulos.Sum(e => e.Total);
            decimal iva = listArticulos.Sum(e => e.IVA);
            decimal cobrar = total + iva;

            txtTotalR.Value = total.ToString();
            txtIVAR.Value = iva.ToString();
            txtCobrar.Value = cobrar.ToString();


            gridProductos.DataSource = listArticulos;
            gridProductos.DataBind();
        }


        private string ResetearVenta()
        {
            string M = string.Empty;
            //Se eliminan los articulos de la tabla temporal
            M=EliminarArticuloVenta();

            if (!M.Equals(string.Empty))
                return M;

            //Se bindea el grid
            BindGridProductosVenta();

            //Se limpia la cadena de valores de la lista de productos del catalogo
            _CadValoresSeleccionados.Value = string.Empty;

            //Se limpia el ID de la receta
            _IDReceta.Value = string.Empty;


            //Se limpian los campos de DAtos de la Venta
            txtFolio.Value = string.Empty;
            txtFecha.Value = DateTime.Now.ToShortDateString();
            txtTotalR.Value = "0.00";
            txtIVAR.Value = "0.00";
            txtCobrar.Value = "0.00";

            return M;
        }


        private string ActualizarReceta()
        {
            string M = string.Empty;
            if (!_IDReceta.Value.Equals(string.Empty))
            {
                int idReceta=Utilerias.StrToInt(_IDReceta.Value);
                
                
                Recetas receta = uow.RecetasBusinessLogic.GetByID(idReceta);
                
                receta.Status = 2; //Se pone como surtida 
                
                uow.RecetasBusinessLogic.Update(receta);
                uow.SaveChanges();
                
                //SI HUBO ERORRES
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    return M;
                }

                BindGridRecetas();
                
            }

            return M;
        }

        private string ActualizarProductosCatalogo(List<ArticuloVenta> listArticulos)
        {
            //Actualizar los articulos del catalalogo

            Articulos artCatalogo;
            string M = string.Empty;

            foreach (ArticuloVenta artVenta in listArticulos)
            {
                artCatalogo = uow.ArticulosBL.GetByID(artVenta.ArticuloId);

                artCatalogo.CantidadEnAlmacen -= artVenta.Cantidad;
                artCatalogo.CantidadDisponible -= artVenta.Cantidad;


                uow.ArticulosBL.Update(artCatalogo);
                uow.SaveChanges();


                //SI HUBO ERORRES
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    return M;
                }

            }

            BindGridProductosCatalago();

            return M;
        }


        private string CrearArticulosMovimientos(int ventaId, List<ArticuloVenta> listArticulos)
        {
            //Crear registros para la tabla de ArticulosMovimientos y sus detalles ArticulosMovimientosSalidas
            string M = string.Empty;
            int movimiento = uow.ArticulosMovimientosBL.Get().Count() > 0 ? uow.ArticulosMovimientosBL.Get().Max(e => e.Movimiento) + 1 : 1;
           
            ArticulosMovimientosSalidas artMovSalida;

            ArticulosMovimientos artMovimiento = new ArticulosMovimientos();

            artMovimiento.Ejercicio = 2015; //Preguntar que se debe de poner aqui????
            artMovimiento.Tipo = 2;
            artMovimiento.VentaId = ventaId;
            artMovimiento.Fecha = DateTime.Now; //Preguntar cual es la fecha que se debe de colocar???
            artMovimiento.Status = 1; //Preguntar cual es la Status que se debe de colocar???
            artMovimiento.Movimiento = movimiento;

            uow.ArticulosMovimientosBL.Insert(artMovimiento);
            uow.SaveChanges();


            //SI HUBO ERORRES
            if (uow.Errors.Count > 0)
            {
                foreach (string m in uow.Errors)
                    M += m;

                return M;
            }

            //Se prosigue a llenar el detalle en ArticulosMovimientosSalidas
            foreach (ArticuloVenta artVenta in listArticulos)
            {
                artMovSalida = new ArticulosMovimientosSalidas();

                artMovSalida.ArticuloMovimientoId = artMovimiento.Id;
                artMovSalida.ArticuloId = artVenta.ArticuloId;
                artMovSalida.Cantidad = artVenta.Cantidad;

                uow.ArticulosMovimientosSalidasBL.Insert(artMovSalida);
                uow.SaveChanges();


                //SI HUBO ERORRES
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    return M;
                }

            }

            return M;
        }

        private int ObtenerMaxFolio()
        {
            int max = 1;

            if (uow.VentasBL.Get().Count() > 0)
                max = uow.VentasBL.Get().Max(e => e.Folio) + 1;

            return max;

        }


        private string CrearVenta()
        {
            string M = string.Empty;
            int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());

            DataAccessLayer.Models.Ventas venta = new DataAccessLayer.Models.Ventas();
            venta.Folio = ObtenerMaxFolio();//Utilerias.StrToInt(txtFolio.Value);
            venta.Ejercicio = 2015; //???????

            if (!_IDReceta.Value.Equals(string.Empty))
                venta.RecetaId = Utilerias.StrToInt(_IDReceta.Value);

            venta.Status = 1;
            venta.ClienteId = Utilerias.StrToInt(ddlClientes.SelectedValue);
            venta.Fecha = Convert.ToDateTime(txtFecha.Value);
            
            //Se almacena el encabezado de venta
            uow.VentasBL.Insert(venta);
            uow.SaveChanges();

            //SI HUBO ERORRES
            if (uow.Errors.Count > 0)
            {
                foreach (string m in uow.Errors)
                    M += m;

                return M;
            }


            //Se prosigue a guardar el detalle de la venta
            //Se buscan los ARTICULOS a VENDER en la tabla temporal de ARTICULOVENTA
            //Se recorren y se crean objetos de tipo VENTASARTICULOS

            List<ArticuloVenta> listArticulos = uow.ArticuloVentaBusinessLogic.Get(a => a.UsuarioId == idUser).ToList();

            foreach (ArticuloVenta art in listArticulos)
            {
                VentasArticulos ventaArt = new VentasArticulos();

                ventaArt.VentaId = venta.Id;
                ventaArt.ArticuloId = art.ArticuloId;
                ventaArt.Cantidad = art.Cantidad;
                ventaArt.PrecioCompra = art.PrecioCompra;
                ventaArt.PrecioVenta = art.PrecioVenta;
                ventaArt.Subtotal = art.SubTotal;
                ventaArt.IVA = art.IVA;
                ventaArt.Total = art.Total;

                uow.VentasArticulosBL.Insert(ventaArt);
                uow.SaveChanges();


                //SI HUBO ERORRES 
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    return M;
                }

            }

            //Se actualiza el IMPORTE del objeto VENTA, a partir de la sumatoria de la Lista, Preguntar?????
            DataAccessLayer.Models.Ventas objVenta = uow.VentasBL.GetByID(venta.Id);
            objVenta.Importe = listArticulos.Sum(e => e.Total) + listArticulos.Sum(e=>e.IVA);

            uow.VentasBL.Update(objVenta);
            uow.SaveChanges();


            //SI HUBO ERORRES 
            if (uow.Errors.Count > 0)
            {
                foreach (string m in uow.Errors)
                    M += m;

                return M;
            }

            M=ActualizarProductosCatalogo(listArticulos); //Se actualizan cantidades del articulo en el catalogo

            if (!M.Equals(string.Empty))
                return M;

            M=CrearArticulosMovimientos(venta.Id, listArticulos); //Se generan registros en ArticulosMovimientos y sus detalles

            if (!M.Equals(string.Empty))
                return M;

            M=ActualizarReceta(); //Se cambia el estatus a la receta que se vio involucrada en la venta

            if (!M.Equals(string.Empty))
                return M;

            M=ResetearVenta(); //Se limpian los campos para una nueva venta

            if (!M.Equals(string.Empty))
                return M;

            //Se muestra la venta
            ClientScript.RegisterStartupScript(this.GetType(), "script", "fnc_MostrarVenta(" + venta.Id + ")", true);

            return M;
        }


        protected void btnProductos_ServerClick(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)((HtmlButton)sender).NamingContainer;
            int idReceta = Utilerias.StrToInt(gridRecetas.DataKeys[row.RowIndex].Value.ToString());
            _IDReceta.Value = idReceta.ToString();

            Recetas objReceta = uow.RecetasBusinessLogic.GetByID(idReceta);
            
            checkProductos.DataSource = objReceta.detalleReceta.Where(a=>a.ArticuloId != null).ToList();
            checkProductos.DataValueField = "ArticuloId";
            checkProductos.DataTextField = "NombreMedicamento";
            checkProductos.DataBind();

            OcultarError();

            ClientScript.RegisterStartupScript(this.GetType(), "script", "fnc_MostrarModal()", true);

        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Articulos articulo;
            string M = string.Empty;
            int idArticulo;
            int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());
            decimal subtotal;
            decimal iva = 0;
            decimal total;
            int idReceta = Utilerias.StrToInt(_IDReceta.Value);

            foreach (ListItem item in checkProductos.Items)
            {
                if (item.Selected)
                {
                    //ddlProductosVenta.Items.Add(new ListItem("Descripcion", item.Value));

                    idArticulo = Utilerias.StrToInt(item.Value);
                    articulo = uow.ArticulosBL.GetByID(idArticulo);

                    ArticuloVenta artVenta = new ArticuloVenta();
                    artVenta.ArticuloId = idArticulo;
                    artVenta.Cantidad = 1;
                    artVenta.Nombre = ConcatenarNombreArticulo(idArticulo);//articulo.Nombre;
                    artVenta.PrecioCompra = articulo.PrecioCompra;
                    artVenta.PrecioVenta = articulo.PrecioVenta;
                    artVenta.EsMedicamento = articulo.esMedicamento; //Verificar tipo de campo
                    artVenta.RecetaId = idReceta;
                    artVenta.UsuarioId = idUser;

                    subtotal = articulo.PrecioVenta;

                    //Preguntar como obtener el IVA ?????
                    if (articulo.esMedicamento == 0)
                    {
                        iva = Convert.ToDecimal(Convert.ToDouble(subtotal) * .16);
                    }
                    artVenta.IVA = iva;
                    artVenta.SubTotal = subtotal;
                    artVenta.Total = subtotal + iva;
                    

                    uow.ArticuloVentaBusinessLogic.Insert(artVenta);
                    uow.SaveChanges();

                    //SI HUBO ERORRES AL ELIMINAR REGISTROS PREVIOS
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
                   

                }
            }

            if (checkProductos.Items.Count == 0)
                _IDReceta.Value = string.Empty;

            OcultarError();

            BindGridProductosVenta();

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
                               Nombre = a.Nombre + " " + um.Nombre + " " + p.Nombre + " " + a.Porcentaje,
                           }).FirstOrDefault();

            nombre = art.Nombre;

            return nombre;

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
            decimal iva=0;
            decimal total;

            int cantidad = Utilerias.StrToInt(((HtmlInputGenericControl)gridProductos.Rows[e.RowIndex].FindControl("txtCantidad")).Value);

            subtotal = (cantidad) * (objArticulo.PrecioVenta);

            if (objArticulo.EsMedicamento == 0)
            {
                iva = Convert.ToDecimal(Convert.ToDouble(subtotal) * .16);
            }

            total = subtotal + iva;

            objArticulo.IVA = iva;
            objArticulo.SubTotal = subtotal;
            objArticulo.Total = total;
            objArticulo.Cantidad = cantidad;

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

            BindGridProductosVenta();

            OcultarError();
        }

        protected void gridProductosCatalogo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridProductosCatalogo.PageIndex = e.NewPageIndex;
            BindGridProductosCatalago();
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
            decimal subtotal;
            decimal iva = 0;
            decimal total;

            foreach (string id in ids)
            {
                idProducto = Utilerias.StrToInt(id);

                articulo = uow.ArticulosBL.GetByID(idProducto);

                ArticuloVenta artVenta = new ArticuloVenta();
                artVenta.ArticuloId = idProducto;
                artVenta.Cantidad = 1;
                artVenta.Nombre = ConcatenarNombreArticulo(idProducto);//articulo.Nombre;
                artVenta.PrecioCompra = articulo.PrecioCompra;
                artVenta.PrecioVenta = articulo.PrecioVenta;
                artVenta.EsMedicamento = articulo.esMedicamento; //Verificar tipo de campo
                artVenta.UsuarioId = idUser;

                subtotal = articulo.PrecioVenta;

                //Preguntar como obtener el IVA ?????
                if (articulo.esMedicamento == 0)
                {
                    iva = Convert.ToDecimal(Convert.ToDouble(subtotal) * .16);
                }

                artVenta.IVA = iva;
                artVenta.SubTotal = subtotal;
                artVenta.Total = subtotal + iva;
                

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

            }

            _CadValoresSeleccionados.Value = string.Empty;

            BindGridProductosVenta();

            OcultarError();
        }

        protected void btnAceptarVenta_Click(object sender, EventArgs e)
        {
            string M = string.Empty;

            M=CrearVenta();

            //Se muestra el mensaje de error, si es que existe
            if (!M.Equals(string.Empty))
            {
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = M;

                return;
            }

            OcultarError();
        }

        protected void btnCancelarVenta_Click(object sender, EventArgs e)
        {
            string M = string.Empty;

            M=ResetearVenta();

            //Se muestra el mensaje de error
            if (!M.Equals(string.Empty))
            {
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = M;

                return;
            }

            OcultarError();
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            OcultarError();

            BindGridRecetas();

        }

       

    }


}