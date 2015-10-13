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
    public partial class wfVentasByLector : System.Web.UI.Page
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
                txtFecha.Value = DateTime.Now.ToShortDateString();
                ResetearVenta();
                txtCode.Focus();
            }


        }


        private void ResetearVenta()
        {
            uow.ArticuloVentaBusinessLogic.DeleteAll();
            uow.SaveChanges();

            BindGridProductosVenta();
            
            txtTotalR.Value = "0.00";
            txtIVAR.Value = "0.00";
            txtCobrar.Value = "0.00";


        }


        private void BindGridProductosVenta()
        {
            int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());

            List<ArticuloVenta> listArticulos = uow.ArticuloVentaBusinessLogic.Get(p => p.UsuarioId == idUser).ToList();
            gridProductos.DataSource = listArticulos;
            gridProductos.DataBind();

            txtTotalR.Value = "0.00";
            txtIVAR.Value = "0.00";
            txtCobrar.Value = "0.00";

            if (listArticulos.Count > 0)
            {
                decimal total = listArticulos.Sum(e => e.SubTotal);
                decimal iva = listArticulos.Sum(e => e.IVA);
                decimal cobrar = listArticulos.Sum(e => e.Total);

                txtTotalR.Value = total.ToString("c");
                txtIVAR.Value = iva.ToString("c");
                txtCobrar.Value = cobrar.ToString("c");   
            }


        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());
            int idProducto;

            List<Articulos> lista = uow.ArticulosBL.Get(p => p.Clave == txtCode.Text).ToList();


            idProducto = 0;
            foreach (Articulos item in lista)            
                idProducto = item.Id;


            if (idProducto == 0)
            {
                txtCode.Text = string.Empty;
                txtCode.Focus();
                return;
            }


            ArticuloVenta obj;
            
                
            List<ArticuloVenta> listArticulos = uow.ArticuloVentaBusinessLogic.Get(p => p.UsuarioId == idUser && p.ArticuloId == idProducto).ToList();
            Articulos articulo = uow.ArticulosBL.GetByID(idProducto);
            int idAV=0;

            if (listArticulos.Count == 0)
            {
                
                if (articulo.CantidadEnAlmacen > 0)
                {
                    obj = new ArticuloVenta();
                    obj.UsuarioId = idUser;
                    obj.ArticuloId = idProducto;
                    obj.Nombre = articulo.NombreCompleto;
                    obj.Cantidad = 1;
                    obj.EsMedicamento = articulo.esMedicamento;
                    obj.PrecioCompra = articulo.PrecioCompraIVA;
                    obj.PrecioVenta = articulo.PrecioVentaIVA;
                                      

                    obj.SubTotal = articulo.PrecioVenta;
                    obj.IVA = articulo.PrecioVentaIVA - articulo.PrecioVenta;
                    obj.Total = articulo.PrecioVentaIVA;

                    uow.ArticuloVentaBusinessLogic.Insert(obj);
                    uow.SaveChanges();
                }

                
            }
            else
            {
                foreach (ArticuloVenta item in listArticulos)
                    idAV = item.Id;

                ArticuloVenta articuloventa = uow.ArticuloVentaBusinessLogic.GetByID(idAV);

                decimal subtotal=0;
                decimal iva = 0;
                decimal total=0;
                int cantidad = articuloventa.Cantidad;

                if (articulo.CantidadEnAlmacen > cantidad)
                {
                    cantidad++;

                    subtotal = (cantidad) * (articulo.PrecioVenta);
                    total = (cantidad) * (articulo.PrecioVentaIVA);
                    iva = total - subtotal;

                    articuloventa.Cantidad = cantidad;
                    articuloventa.SubTotal = subtotal;
                    articuloventa.IVA = iva;
                    articuloventa.Total = total;


                    uow.ArticuloVentaBusinessLogic.Update(articuloventa);
                    uow.SaveChanges();
                }


            }


            divMsgError.Style.Add("display","none");

                BindGridProductosVenta();
                txtCode.Text = string.Empty;
                txtCode.Focus();





        

        }


        protected void btnCancelarVenta_Click(object sender, EventArgs e)
        {
            uow.ArticuloVentaBusinessLogic.DeleteAll();
            uow.SaveChanges();
            Response.Redirect("wfVentasDia.aspx");
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



        private int ObtenerMaxFolio()
        {
            int max = 1;

            if (uow.VentasBL.Get().Count(e => e.Ejercicio == DateTime.Now.Year) > 0)
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

        protected void gridProductos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Utilerias.StrToInt( gridProductos.DataKeys[e.RowIndex].Value.ToString());

            ArticuloVenta obj = uow.ArticuloVentaBusinessLogic.GetByID(id);

            

            uow.ArticuloVentaBusinessLogic.Delete(obj);
            uow.SaveChanges();
            BindGridProductosVenta();
            txtCode.Text = string.Empty;
            txtCode.Focus();
        }


    }
}