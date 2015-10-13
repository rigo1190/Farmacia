﻿using DataAccessLayer;
using DataAccessLayer.Models;
using BusinessLogicLayer;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;


namespace SIP.Formas.Inventarios
{
    public partial class wfExistencias : System.Web.UI.Page
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
            
                cargarGruposArticulos(); 
                _URLVisor.Value = ResolveClientUrl("~/rpts/wfVerReporte.aspx");

                if (!IsPostBack)
                {
                    SqlConnection sqlConnection1 = new SqlConnection(uow.Contexto.Database.Connection.ConnectionString.ToString());
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader reader;

                    cmd.CommandText = "sp_RPTconcentradoEntradasSalidas";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection1;

                    //cmd.Parameters.Add("@usuario", usuario);
                    //cmd.Parameters.Add("@contrato", contrato.Id);
                    sqlConnection1.Open();

                    reader = cmd.ExecuteReader();
                    sqlConnection1.Close();

                    uow = new UnitOfWork(Session["IdUser"].ToString());
                }
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

            List<Articulos> detalle = uow.ArticulosBL.Get(q => q.GruposPSId == grupo && q.CantidadEnAlmacen > 0).OrderBy(q=>q.Nombre).ToList();

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


            thOne.InnerText = "Cons.";
            thTwo.InnerText = "Código";
            thThree.InnerText = "Nombre";
            thFour.InnerText = "Existencia";
            thFive.InnerText = "";

            trHead.Controls.Add(thOne);
            trHead.Controls.Add(thTwo);
            trHead.Controls.Add(thThree);
            trHead.Controls.Add(thFour);
            //trHead.Controls.Add(thFive);
            tabla.Controls.Add(trHead);

            int consecutivo = 0;
            foreach (Articulos item in detalle)
            {
                consecutivo++;

                System.Web.UI.HtmlControls.HtmlGenericControl tr = new System.Web.UI.HtmlControls.HtmlGenericControl("TR");
                System.Web.UI.HtmlControls.HtmlGenericControl tdOne = new System.Web.UI.HtmlControls.HtmlGenericControl("TD");
                System.Web.UI.HtmlControls.HtmlGenericControl tdTwo = new System.Web.UI.HtmlControls.HtmlGenericControl("TD");
                System.Web.UI.HtmlControls.HtmlGenericControl tdThree = new System.Web.UI.HtmlControls.HtmlGenericControl("TD");
                System.Web.UI.HtmlControls.HtmlGenericControl tdFour = new System.Web.UI.HtmlControls.HtmlGenericControl("TD");
                System.Web.UI.HtmlControls.HtmlGenericControl tdFive = new System.Web.UI.HtmlControls.HtmlGenericControl("TD");


                //tdOne.Attributes.Add("align", "left");
                tdOne.InnerText = consecutivo.ToString();
                tdTwo.InnerText = item.Clave;
                tdThree.InnerText = item.NombreCompleto;
                tdFour.InnerText = item.CantidadEnAlmacen.ToString();
                tdFive.InnerText = "";



                tr.Controls.Add(tdOne);
                tr.Controls.Add(tdTwo);
                tr.Controls.Add(tdThree);
                tr.Controls.Add(tdFour);
                //tr.Controls.Add(tdFive);

                tabla.Controls.Add(tr);
            }
        }

        protected void linkConcentradoInputOutput_Click(object sender, EventArgs e)
        {

            SqlConnection sqlConnection1 = new SqlConnection(uow.Contexto.Database.Connection.ConnectionString.ToString());
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            SqlDataReader rs;
            SqlCommand com2;
            string sql;
            try
            {

                cmd.CommandText = "sp_RPTconcentradoEntradasSalidas";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection1;

                //cmd.Parameters.Add("@articulo", producto);
                sqlConnection1.Open();

                reader = cmd.ExecuteReader();





            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
            finally
            {

                sqlConnection1.Close();
            }



        }

    }
}