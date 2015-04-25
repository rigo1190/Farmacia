using DataAccessLayer;
using DataAccessLayer.Models;
using BusinessLogicLayer;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIP.Formas.Catalogos
{
    public partial class wfArticulos : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            cargarGruposArticulos();
        }


        private void cargarGruposArticulos()
        {

            

            List<GruposPS> listaPadres = uow.GruposPSBL.Get().ToList();
            //List<GruposConceptosDeObra> listaHijos;

            int i = 0;
            foreach (GruposPS padre in listaPadres)
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
                a.InnerText = padre.Clave + " : " + padre.Nombre;

                h4.Controls.Add(a);
                divPanelHeading.Controls.Add(h4);






                //Collapse
                divPanelCollapse.Attributes.Add("id", "collapse" + i.ToString());
                divPanelCollapse.Attributes.Add("class", "panel-collapse collapse");


                divPanelBody.Attributes.Add("class", "panel-body");


                addConcepto.Attributes.Add("href", ResolveClientUrl("~/Formas/Catalogos/wfArticulosAdd.aspx?grupo=" + padre.Id));
                addConcepto.InnerText = "Actualizar...";



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
            
            List<Articulos> detalle = uow.ArticulosBL.Get(q => q.GruposPSId == grupo && q.Status == 1).ToList();

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


            thOne.InnerText = "Código";
            thTwo.InnerText = "Producto";
            thThree.InnerText = "Existencia";
            thFour.InnerText = "$ Compra + IVA";
            thFive.InnerText = "$ Venta + IVA";

            trHead.Controls.Add(thOne);
            trHead.Controls.Add(thTwo);
            trHead.Controls.Add(thThree);
            trHead.Controls.Add(thFour);
            trHead.Controls.Add(thFive);
            
            tabla.Controls.Add(trHead);


            foreach (Articulos item in detalle)
            {

                System.Web.UI.HtmlControls.HtmlGenericControl tr = new System.Web.UI.HtmlControls.HtmlGenericControl("TR");
                System.Web.UI.HtmlControls.HtmlGenericControl tdOne = new System.Web.UI.HtmlControls.HtmlGenericControl("TD");
                System.Web.UI.HtmlControls.HtmlGenericControl tdTwo = new System.Web.UI.HtmlControls.HtmlGenericControl("TD");
                System.Web.UI.HtmlControls.HtmlGenericControl tdThree = new System.Web.UI.HtmlControls.HtmlGenericControl("TD");
                System.Web.UI.HtmlControls.HtmlGenericControl tdFour = new System.Web.UI.HtmlControls.HtmlGenericControl("TD");
                System.Web.UI.HtmlControls.HtmlGenericControl tdFive = new System.Web.UI.HtmlControls.HtmlGenericControl("TD");
                

                tdOne.Attributes.Add("align", "left");
                tdOne.InnerText = item.Clave;
                tdTwo.InnerText = item.NombreCompleto;
                tdThree.InnerText = item.CantidadEnAlmacen.ToString();
                tdFour.InnerText = item.PrecioCompraIVA.ToString("C2");
                tdFive.InnerText = item.PrecioVentaIVA.ToString("C2");

                

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