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
    public partial class wfCotizacionesAdd : System.Web.UI.Page
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
                _URLVisor.Value = ResolveClientUrl("~/rpts/wfVerReporte.aspx");

                BindGrid();
                BindGridCatalogo();
                BindCombos();

                ModoForma(false);
                LimpiarTemporales();
            }
        }



        #region metodos

        private void BindGrid()
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            int idusuario;

            idusuario = int.Parse(Session["IdUser"].ToString());

            List<CotizacionesTMP> lista = uow.CotizacionesTMPBL.Get(p => p.UsuarioId == idusuario).ToList();
            gridProductos.DataSource = lista;
            gridProductos.DataBind();


            if (lista.Count == 0)
            {
                txtImporteTotal.Text = "$0.00";
            }
            else
            {
                txtImporteTotal.Text = lista.Sum(p => p.Total).ToString("C2"); ;
            }




             
            this.gridProveedores.DataSource = uow.CotizacionesTMPproveedoresBL.Get(p => p.UsuarioId == idusuario);
            this.gridProveedores.DataBind();

            divMsg.Style.Add("display", "none");
        }

        private void BindGridCatalogo()
        {
            List<Articulos> lista = uow.ArticulosBL.Get(p=>p.Status == 1).OrderBy(q=>q.NombreCompleto).ToList();
            gridProductosCatalogo.DataSource = lista; 
            gridProductosCatalogo.DataBind();
        }
        private void BindCombos()
        {


            //ddlArticulo.DataSource = uow.ArticulosBL.Get(p => p.Status == 1).ToList().OrderBy(q => q.Nombre);
            //ddlArticulo.DataValueField = "Id";
            //ddlArticulo.DataTextField = "Nombre";
            //ddlArticulo.DataBind();

            ddlProveedores.DataSource = uow.ProveedoresBL.Get(p => p.Status == 1).ToList().OrderBy(q => q.RazonSocial);
            ddlProveedores.DataValueField = "Id";
            ddlProveedores.DataTextField = "RazonSocial";
            ddlProveedores.DataBind();

        }




        private void ModoForma(bool modoCaptura)
        {

            this.divMsg.Style.Add("display", "none");
            this.divMsgSuccess.Style.Add("display", "none");

            //if (modoCaptura)
            //{
            //    this.divDatos.Style.Add("display", "none");
            //    this.divBtnNuevo.Style.Add("display", "none");
                

            //}
            //else
            //{
            //    this.divDatos.Style.Add("display", "block");
            //    this.divBtnNuevo.Style.Add("display", "block");
                
            //}

            //this.divBtnNuevo.Style.Add("display", "block")
             ;

        }

        private void LimpiarTemporales() {
            List<CotizacionesTMPproveedores> listaProveedores = uow.CotizacionesTMPproveedoresBL.Get().ToList();
            List<CotizacionesTMP> listaArticulos = uow.CotizacionesTMPBL.Get().ToList();


            foreach (CotizacionesTMPproveedores item in listaProveedores)
                uow.CotizacionesTMPproveedoresBL.Delete(item);

            foreach (CotizacionesTMP item in listaArticulos)
                uow.CotizacionesTMPBL.Delete(item);

            uow.SaveChanges();
            BindGrid();
        }

        #endregion


        #region eventos
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            //LimpiarTemporales();            
            
            int idFolio=0;
            string folio;

            int idusuario = int.Parse(Session["IdUser"].ToString());
            int ejercicio = DateTime.Now.Year;

            List<CotizacionesTMP> listaArticulos = uow.CotizacionesTMPBL.Get(p => p.UsuarioId == idusuario).ToList();
            List<CotizacionesTMPproveedores> listaProv = uow.CotizacionesTMPproveedoresBL.Get(p => p.UsuarioId == idusuario).ToList();

            if (listaArticulos.Count == 0)
            {
                lblMensajes.Text = "Para guardar la cotización necesita detallar los productos a cotizar";
                divMsg.Style.Add("display", "block");
                return;
            }

            if (listaProv.Count == 0)
            {
                lblMensajes.Text = "Para guardar la cotización necesita detallar los proveedores a invitar";
                divMsg.Style.Add("display", "block");
                return;
            }


            List<Cotizaciones> lista = uow.CotizacionesBL.Get(p => p.Ejercicio == ejercicio).ToList();

            if (lista.Count > 0)            
                idFolio = lista.Max(p => p.IdFolio);

            idFolio++;

            folio = "0000" + idFolio.ToString();
            folio = folio.Substring(folio.Length - 4);
            folio = "COT/" + folio + "/" + ejercicio.ToString();

            Cotizaciones cotizacion = new Cotizaciones();
                cotizacion.Ejercicio = ejercicio;
                cotizacion.IdFolio = idFolio;
                cotizacion.Folio = folio;
                cotizacion.Fecha = DateTime.Now;
                cotizacion.Status = 1;
                cotizacion.CostoAproximado = listaArticulos.Sum(p => p.Total);
            uow.CotizacionesBL.Insert(cotizacion);


            
            



            foreach (CotizacionesTMP item in listaArticulos)
            {
                CotizacionesArticulos detalle = new CotizacionesArticulos();
                    detalle.Cotizacion = cotizacion;
                    detalle.ArticuloId = item.ArticuloId;
                    detalle.Cantidad = item.Cantidad;
                    detalle.esMedicamento = item.esMedicamento;
                    detalle.Precio = item.Precio;
                    detalle.Subtotal = item.Subtotal;
                    detalle.IVA = item.IVA;
                    detalle.Total = item.Total;
                uow.CotizacionesArticulosBL.Insert(detalle);
            }


            foreach (CotizacionesTMPproveedores item in listaProv)
            {
                CotizacionesProveedores detProv = new CotizacionesProveedores();
                detProv.Cotizacion = cotizacion;
                detProv.ProveedorId = item.ProveedorId;

                uow.CotizacionesProveedoresBL.Insert(detProv);
            }




            uow.SaveChanges();

            if (uow.Errors.Count == 0)
            {
                Response.Redirect("wfCotizaciones.aspx");
            }


        }


         


        protected void imgImprimir_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void imgBtnEliminarProveedor_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _ElId.Text = gridProveedores.DataKeys[row.RowIndex].Values["Id"].ToString();


            if (_ElId.Text == "")
                return;

            CotizacionesTMPproveedores obj = uow.CotizacionesTMPproveedoresBL.GetByID(int.Parse(_ElId.Text));

            uow.CotizacionesTMPproveedoresBL.Delete(obj);
            uow.SaveChanges();
            BindGrid();

            
        }

        protected void btnAddProveedor_Click(object sender, EventArgs e)
        {
            CotizacionesTMPproveedores obj;
            List<CotizacionesTMPproveedores> lista;
            int id;



            id = int.Parse(ddlProveedores.SelectedValue);


            lista = uow.CotizacionesTMPproveedoresBL.Get(p => p.ProveedorId == id).ToList();
            if (lista.Count == 0)
            {
                obj = new CotizacionesTMPproveedores();

                obj.ProveedorId = id;
                obj.UsuarioId = int.Parse(Session["IdUser"].ToString());
                uow.CotizacionesTMPproveedoresBL.Insert(obj);

            }



            uow.SaveChanges();

            BindGrid();


        }

        protected void gridProveedores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                int id = Utilerias.StrToInt(gridProveedores.DataKeys[e.Row.RowIndex].Values["Id"].ToString());



                CotizacionesTMPproveedores obj = uow.CotizacionesTMPproveedoresBL.GetByID(id);


                ImageButton imgBut = (ImageButton)e.Row.FindControl("imgImprimir");
                if (imgBut != null)
                    imgBut.Attributes["onclick"] = "fnc_AbrirReporte(" + obj.ProveedorId + ");return false;";


                //ImageButton imgButCostos = (ImageButton)e.Row.FindControl("imgImprimirCostos");
                //if (imgButCostos != null)
                //    imgButCostos.Attributes["onclick"] = "fnc_AbrirReporteCostos(" + obj.ProveedorId + ");return false;";


            }
        }
        


        



    

         

        protected void btnAgregarDeCat_Click(object sender, EventArgs e)
        {
            string cadenaValores = _CadValoresSeleccionados.Value;

            if (cadenaValores.Equals(string.Empty))
                return;

            string[] ids = cadenaValores.Split('|');
            int idProducto;
            
            Articulos articulo;            
            int idUser = int.Parse(Session["IdUser"].ToString());


            foreach (string id in ids)
            {
                idProducto = int.Parse(id);
                articulo = uow.ArticulosBL.GetByID(idProducto);

                CotizacionesTMP cotizacion = new CotizacionesTMP();
                cotizacion.UsuarioId = idUser;
                cotizacion.ArticuloId = idProducto;
                cotizacion.Cantidad = 1;
                cotizacion.Precio = articulo.PrecioCompra;
                cotizacion.Subtotal = articulo.PrecioCompra;
                cotizacion.Total = articulo.PrecioCompraIVA;
                cotizacion.IVA = articulo.PrecioCompraIVA - articulo.PrecioCompra;
                cotizacion.esMedicamento = articulo.esMedicamento;
                

                uow.CotizacionesTMPBL.Insert(cotizacion);
                uow.SaveChanges();

                 

            }

            _CadValoresSeleccionados.Value = string.Empty;

            BindGrid();


        }
        
        protected void gridProductos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridProductos.EditIndex = e.NewEditIndex;
            BindGrid();

        }
        protected void gridProductos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            int id = int.Parse(gridProductos.DataKeys[e.RowIndex].Value.ToString());
            CotizacionesTMP obj = uow.CotizacionesTMPBL.GetByID(id);
            
            int cantidad = Utilerias.StrToInt(((HtmlInputGenericControl)gridProductos.Rows[e.RowIndex].FindControl("txtCantidad")).Value);

            decimal factorIVA = decimal.Parse(Session["IVA"].ToString());
            factorIVA++;

            Articulos articulo = uow.ArticulosBL.GetByID(obj.ArticuloId);

            obj.Cantidad = cantidad;
            obj.Precio = articulo.PrecioCompraIVA;
            
            
            if (articulo.esMedicamento == 1)
            {
                obj.Subtotal = cantidad * obj.Articulo.PrecioCompraIVA;
                obj.IVA = 0;
                obj.Total = cantidad * obj.Articulo.PrecioCompraIVA;
            }
            else
            {
                
                obj.Total = cantidad * obj.Articulo.PrecioCompraIVA;
                obj.Subtotal =  Math.Round( obj.Total / factorIVA,2);
                obj.IVA = obj.Total - obj.Subtotal;
            }

            
            
                



            uow.CotizacionesTMPBL.Update(obj); 
            uow.SaveChanges();
             

            // Cancelamos la edicion del grid
            gridProductos.EditIndex = -1;

            BindGrid(); 

             

        }
        protected void gridProductos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridProductos.EditIndex = -1;
            BindGrid();
        }
        protected void gridProductos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            int id = Utilerias.StrToInt(gridProductos.DataKeys[e.RowIndex].Value.ToString());
            CotizacionesTMP obj = uow.CotizacionesTMPBL.GetByID(id);

            uow.CotizacionesTMPBL.Delete(obj);
            uow.SaveChanges();
            BindGrid();


        }

        #endregion


    }
}