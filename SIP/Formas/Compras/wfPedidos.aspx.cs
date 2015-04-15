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
    public partial class wfPedidos : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            if (!IsPostBack)
            {
                _URLVisor.Value = ResolveClientUrl("~/rpts/wfVerReporte.aspx");

                cargarForma();
                BindGrid();
                BindComboProveedores();
                BindComboProductos();
        

                
            }
        }


        private void cargarForma()
        {
            int idCotizacion = int.Parse(Session["XCotizacionId"].ToString());

            Cotizaciones cotizacion = uow.CotizacionesBL.GetByID(idCotizacion);

            if (cotizacion.Status == 1)// no tiene  generado sus pedidos
            {
                uow.CotizacionesTMPAsignacionesBL.DeleteAll();

                List<CotizacionesArticulos> lista = uow.CotizacionesArticulosBL.Get(p => p.CotizacionId == idCotizacion).ToList();
                foreach (CotizacionesArticulos item in lista)
                {
                    CotizacionesTMPasignaciones obj = new CotizacionesTMPasignaciones();

                    obj.CotizacionId = item.CotizacionId;
                    obj.ArticuloId = item.ArticuloId;
                    obj.Cantidad = item.Cantidad;
                    uow.CotizacionesTMPAsignacionesBL.Insert(obj);
                }

                uow.SaveChanges();



                divAsignar.Style.Add("display", "block");
                divPedidos.Style.Add("display", "none");

            }
            else
            {
                divAsignar.Style.Add("display", "none");
                divPedidos.Style.Add("display", "block");
            }


        }



        private void BindComboProveedores()
        {
            int idCotizacion = int.Parse(Session["XCotizacionId"].ToString());

            var ListaProvs = from cot in uow.CotizacionesProveedoresBL.Get(p => p.CotizacionId == idCotizacion).ToList()
                             join proveedores in uow.ProveedoresBL.Get(p => p.Status == 1).ToList().OrderBy(q => q.RazonSocial)
                           on cot.ProveedorId equals proveedores.Id
                             select proveedores;



            ddlProveedor.DataSource = ListaProvs;//  uow.ProveedoresBL.Get(p => p.Status == 1).ToList().OrderBy(q => q.RazonSocial);
            ddlProveedor.DataValueField = "Id";
            ddlProveedor.DataTextField = "RazonSocial";
            ddlProveedor.DataBind();
        }

        private void BindComboProductos()
        {
            int idCotizacion = int.Parse(Session["XCotizacionId"].ToString());
            List<CotizacionesTMPasignaciones> lista = uow.CotizacionesTMPAsignacionesBL.Get(p => p.CotizacionId == idCotizacion && p.ProveedorId == null).ToList();


            var listaArticulos = from cot in lista
                             join art in uow.ArticulosBL.Get(p => p.Status == 1).ToList().OrderBy(q => q.Nombre)
                             on cot.ArticuloId equals art.Id
                             select art;


            ddlArticulo.DataSource = listaArticulos;// uow.ArticulosBL.Get(p => p.Status == 1).ToList().OrderBy(q => q.Nombre);
            ddlArticulo.DataValueField = "Id";
            ddlArticulo.DataTextField = "Nombre";
            ddlArticulo.DataBind();



            if (ddlArticulo.Items.Count==0)
            {
                DIVgenerarPedidos.Style.Add("display", "block");
                DIVagregar.Style.Add("display", "none");
            }
            else
            {
                DIVgenerarPedidos.Style.Add("display", "none");
                DIVagregar.Style.Add("display", "block");
            }


        }

        private void BindGrid()
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());


            int idCotizacion = int.Parse(Session["XCotizacionId"].ToString());
            List<CotizacionesTMPasignaciones> lista = uow.CotizacionesTMPAsignacionesBL.Get(p => p.CotizacionId == idCotizacion && p.ProveedorId != null).ToList();

            this.gridAsignaciones.DataSource = lista;
            this.gridAsignaciones.DataBind();

            
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {

            if (ddlArticulo.Items.Count == 0)
                return;

            int idCotizacion = int.Parse(Session["XCotizacionId"].ToString());
            int idArticulo = int.Parse(ddlArticulo.SelectedValue);

            CotizacionesTMPasignaciones obj = uow.CotizacionesTMPAsignacionesBL.Get(p => p.CotizacionId == idCotizacion && p.ArticuloId == idArticulo).First();
                       
            obj.ProveedorId = int.Parse( ddlProveedor.SelectedValue);
            
            uow.CotizacionesTMPAsignacionesBL.Update(obj);
            uow.SaveChanges();


            BindGrid();
            BindComboProductos();

        }

        protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            int id = int.Parse( gridAsignaciones.DataKeys[row.RowIndex].Values["Id"].ToString());




            CotizacionesTMPasignaciones obj = uow.CotizacionesTMPAsignacionesBL.GetByID(id);
            obj.ProveedorId = null;
            uow.CotizacionesTMPAsignacionesBL.Update(obj);
            uow.SaveChanges();
            BindGrid();
            BindComboProductos();
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            

            int ejercicio;
            int idFolio = 0;
            string folio;

            int idusuario = int.Parse(Session["IdUser"].ToString());

            ejercicio = DateTime.Now.Year;


            //Get Folio del pedido
            List<Pedidos> lista = uow.PedidosBL.Get(p => p.Ejercicio == ejercicio).ToList();

            if (lista.Count > 0)
                idFolio = lista.Max(p => p.IdFolio);

            

            int idCotizacion = int.Parse(Session["XCotizacionId"].ToString());


            var query = from p in uow.CotizacionesTMPAsignacionesBL.Get(p => p.CotizacionId == idCotizacion).ToList()
                        group p by new { id = p.ProveedorId} into g
                        select new { id = g.Key.id, count = g.Count() };


            List<CotizacionesTMPasignaciones> listaAsignaciones;


            
            foreach (var item in query)
            {
                idFolio++;
                folio = "000" + idFolio.ToString();
                folio = folio.Substring(folio.Length - 4);
                folio = "PED/" + folio + "/" + ejercicio.ToString();

                Pedidos pedido = new Pedidos();
                    pedido.CotizacionId = idCotizacion;
                    pedido.Ejercicio = ejercicio;
                    pedido.ProveedorId = int.Parse(item.id.ToString());
                    pedido.Folio = folio;
                    pedido.IdFolio = idFolio;
                    pedido.Fecha = DateTime.Now;
                    pedido.Status = 1;
                    pedido.Observaciones = "";
                uow.PedidosBL.Insert(pedido);

                listaAsignaciones = uow.CotizacionesTMPAsignacionesBL.Get(p=>p.CotizacionId == idCotizacion &&  p.ProveedorId == item.id).ToList();

                foreach (CotizacionesTMPasignaciones elemento in listaAsignaciones)
                {
                    PedidosArticulos detalle = new PedidosArticulos();
                        detalle.Pedido = pedido;
                        detalle.ArticuloId = elemento.ArticuloId;
                        detalle.Cantidad = elemento.Cantidad;
                    uow.PedidosArticulosBL.Insert(detalle);

                }



                


            }

            Cotizaciones cotizacion = uow.CotizacionesBL.GetByID(idCotizacion);
                cotizacion.Status = 2;
            uow.CotizacionesBL.Update(cotizacion);
            uow.SaveChanges();

            Response.Redirect("wfCotizaciones.aspx");

            //List<CotizacionesTMPasignaciones> listaProveedores = uow.CotizacionesTMPAsignacionesBL.Get(p => p.CotizacionId == idCotizacion).Select(q => q.ProveedorId).Distinct().ToList();














        }

    }
}