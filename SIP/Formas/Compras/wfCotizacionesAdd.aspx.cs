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

            if (!IsPostBack)
            {
                _URLVisor.Value = ResolveClientUrl("~/rpts/wfVerReporte.aspx");

                BindGrid();
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

            this.grid.DataSource = lista;
            this.grid.DataBind();

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

        private void BindCombos()
        {


            ddlArticulo.DataSource = uow.ArticulosBL.Get(p => p.Status == 1).ToList().OrderBy(q => q.Nombre);
            ddlArticulo.DataValueField = "Id";
            ddlArticulo.DataTextField = "Nombre";
            ddlArticulo.DataBind();

            ddlProveedores.DataSource = uow.ProveedoresBL.Get(p => p.Status == 1).ToList().OrderBy(q => q.RazonSocial);
            ddlProveedores.DataValueField = "Id";
            ddlProveedores.DataTextField = "RazonSocial";
            ddlProveedores.DataBind();

        }


        private void ModoForma(bool modoCaptura)
        {

            this.divMsg.Style.Add("display", "none");
            this.divMsgSuccess.Style.Add("display", "none");

            if (modoCaptura)
            {
                this.divDatos.Style.Add("display", "none");
                this.divBtnNuevo.Style.Add("display", "none");
                this.divCaptura.Style.Add("display", "block");

            }
            else
            {
                this.divDatos.Style.Add("display", "block");
                this.divBtnNuevo.Style.Add("display", "block");
                this.divCaptura.Style.Add("display", "none");
            }

            this.divBtnNuevo.Style.Add("display", "block");
            this.divCaptura.Style.Add("display", "block");

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



        protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _ElId.Text = grid.DataKeys[row.RowIndex].Values["Id"].ToString();


            if (_ElId.Text == "")
                return;

            CotizacionesTMP obj = uow.CotizacionesTMPBL.GetByID(int.Parse(_ElId.Text));

            
            uow.CotizacionesTMPBL.Delete(obj);
            uow.SaveChanges();
            BindGrid();
        }




        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            CotizacionesTMP obj;
            List<CotizacionesTMP> lista;
            int idArticulo;
            int idUsuario = int.Parse(Session["IdUser"].ToString());

            
            idArticulo = int.Parse(ddlArticulo.SelectedValue);


            lista = uow.CotizacionesTMPBL.Get(p => p.UsuarioId == idUsuario && p.ArticuloId == idArticulo).ToList();
            if (lista.Count > 0)
            {
                foreach (CotizacionesTMP item in lista)
                    uow.CotizacionesTMPBL.Delete(item);

            }

            Articulos articulo = uow.ArticulosBL.Get(p => p.Id == idArticulo).First();
            obj = new CotizacionesTMP();
            obj.UsuarioId = int.Parse(Session["IdUser"].ToString());
            obj.ArticuloId = idArticulo;
            obj.Cantidad = int.Parse(txtCantidad.Value.ToString());
            obj.esMedicamento = articulo.esMedicamento;
            obj.Precio = articulo.PrecioCompra;
            obj.Subtotal = articulo.PrecioCompra * decimal.Parse(txtCantidad.Value);
            if (articulo.esMedicamento == 1)
            {
                obj.IVA = 0;
                obj.Total = obj.Subtotal;
            }
            else
            {
                obj.IVA = obj.Subtotal * decimal.Parse(Session["IVA"].ToString());
                obj.Total = obj.Subtotal + obj.IVA;
            }
            uow.CotizacionesTMPBL.Insert(obj);


            txtCantidad.Value = string.Empty;

            uow.SaveChanges();
            BindGrid();





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
        #endregion
    }
}