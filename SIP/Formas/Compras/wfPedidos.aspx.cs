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

                cargarPedidos();
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


            divAsignar.Style.Add("display", "none");
            divPedidos.Style.Add("display", "block");

            cargarPedidos();


          }




        private void cargarPedidos()
        {

            int idCotizacion = int.Parse(Session["XCotizacionId"].ToString());


            List<Pedidos> listaPedidos = uow.PedidosBL.Get(p => p.CotizacionId == idCotizacion).ToList();
            

            int i = 0;
            foreach (Pedidos padre in listaPedidos)
            {
                i++;


                System.Web.UI.HtmlControls.HtmlGenericControl divPanel = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
                System.Web.UI.HtmlControls.HtmlGenericControl divPanelHeading = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
                System.Web.UI.HtmlControls.HtmlGenericControl divPanelCollapse = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
                System.Web.UI.HtmlControls.HtmlGenericControl divPanelBody = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");

                System.Web.UI.HtmlControls.HtmlGenericControl h4 = new System.Web.UI.HtmlControls.HtmlGenericControl("H4");
                System.Web.UI.HtmlControls.HtmlGenericControl a = new System.Web.UI.HtmlControls.HtmlGenericControl("A");

                System.Web.UI.HtmlControls.HtmlGenericControl p = new System.Web.UI.HtmlControls.HtmlGenericControl("P");

                System.Web.UI.HtmlControls.HtmlGenericControl addConcepto = new System.Web.UI.HtmlControls.HtmlGenericControl("A");

                //para el detalle
                System.Web.UI.HtmlControls.HtmlGenericControl tabla = new System.Web.UI.HtmlControls.HtmlGenericControl("TABLE");

                //para el subacordeon
                System.Web.UI.HtmlControls.HtmlGenericControl subAcordeon = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");



                //heading
                divPanelHeading.Attributes.Add("class", "panel-heading");

                h4.Attributes.Add("class", "panel-title");

                a.Attributes.Add("data-toggle", "collapse");
                a.Attributes.Add("data-parent", "#accordion");
                a.Attributes.Add("href", "#collapse" + i.ToString());
                a.InnerText = padre.Folio + " : " + padre.Proveedor.RazonSocial;

                h4.Controls.Add(a);
                divPanelHeading.Controls.Add(h4);






                //Collapse
                divPanelCollapse.Attributes.Add("id", "collapse" + i.ToString());
                divPanelCollapse.Attributes.Add("class", "panel-collapse collapse");


                divPanelBody.Attributes.Add("class", "panel-body");


                addConcepto.Attributes.Add("href", ResolveClientUrl("~/Formas/Catalogos/wfArticulosAdd.aspx?grupo=" + padre.Id));
                addConcepto.InnerText = "Ver Reporte";



                divPanelBody.Controls.Add(addConcepto);
                cargardetalle(padre.Id, tabla);
                divPanelBody.Controls.Add(tabla);






                divPanelCollapse.Controls.Add(divPanelBody);


                //Agregar Elemento
                divPanel.Attributes.Add("class", "panel panel-default");
                divPanel.Controls.Add(divPanelHeading);
                divPanel.Controls.Add(divPanelCollapse);

                this.accordion.Controls.Add(divPanel);



            }


        }





        private void cargardetalle(int grupo, System.Web.UI.HtmlControls.HtmlGenericControl tabla)
        {

            List<PedidosArticulos> detalle = uow.PedidosArticulosBL.Get(q => q.PedidoId == grupo ).ToList();

            if (detalle.Count == 0)
                return;


            tabla.Attributes.Add("class", "table");
            tabla.Attributes.Add("cellspacing", "0");

            System.Web.UI.HtmlControls.HtmlGenericControl trHead = new System.Web.UI.HtmlControls.HtmlGenericControl("TR");
            System.Web.UI.HtmlControls.HtmlGenericControl thOne = new System.Web.UI.HtmlControls.HtmlGenericControl("TH");
            System.Web.UI.HtmlControls.HtmlGenericControl thTwo = new System.Web.UI.HtmlControls.HtmlGenericControl("TH");
            System.Web.UI.HtmlControls.HtmlGenericControl thThree = new System.Web.UI.HtmlControls.HtmlGenericControl("TH");
            System.Web.UI.HtmlControls.HtmlGenericControl thFour = new System.Web.UI.HtmlControls.HtmlGenericControl("TH");
            System.Web.UI.HtmlControls.HtmlGenericControl thFive = new System.Web.UI.HtmlControls.HtmlGenericControl("TH");


            trHead.Attributes.Add("align", "center");


            thOne.InnerText = "Codigo";
            thTwo.InnerText = "Nombre";
            thThree.InnerText = "Unidad de Medida";
            thFour.InnerText = "Presentación";
            thFive.InnerText = "Cantidad";

            trHead.Controls.Add(thOne);
            trHead.Controls.Add(thTwo);
            trHead.Controls.Add(thThree);
            trHead.Controls.Add(thFour);
            trHead.Controls.Add(thFive);

            tabla.Controls.Add(trHead);


            foreach (PedidosArticulos item in detalle)
            {

                System.Web.UI.HtmlControls.HtmlGenericControl tr = new System.Web.UI.HtmlControls.HtmlGenericControl("TR");
                System.Web.UI.HtmlControls.HtmlGenericControl tdOne = new System.Web.UI.HtmlControls.HtmlGenericControl("TD");
                System.Web.UI.HtmlControls.HtmlGenericControl tdTwo = new System.Web.UI.HtmlControls.HtmlGenericControl("TD");
                System.Web.UI.HtmlControls.HtmlGenericControl tdThree = new System.Web.UI.HtmlControls.HtmlGenericControl("TD");
                System.Web.UI.HtmlControls.HtmlGenericControl tdFour = new System.Web.UI.HtmlControls.HtmlGenericControl("TD");
                System.Web.UI.HtmlControls.HtmlGenericControl tdFive = new System.Web.UI.HtmlControls.HtmlGenericControl("TD");


                tdOne.Attributes.Add("align", "left");
                tdOne.InnerText = item.Articulo.Clave;
                tdTwo.InnerText = item.Articulo.Nombre;
                tdThree.InnerText = item.Articulo.UnidadesDeMedida.Nombre;
                tdFour.InnerText = item.Articulo.Presentacion.Nombre;
                tdFive.InnerText = item.Cantidad.ToString();



                tr.Controls.Add(tdOne);
                tr.Controls.Add(tdTwo);
                tr.Controls.Add(tdThree);
                tr.Controls.Add(tdFour);
                tr.Controls.Add(tdFive);


                tabla.Controls.Add(tr);
            }
        }





    }
}