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
    public partial class wfRecetas : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            if (!IsPostBack)
            {
                BindGridRecetas();
                BindDropDownMedicamentos();
            }

        }

        

        private void BindDropDownMedicamentos()
        {
            //ddlMedicamentos.DataSource = uow.ArticulosBL.Get();
            //ddlMedicamentos.DataValueField = "Id";
            //ddlMedicamentos.DataTextField = "Nombre";
            //ddlMedicamentos.DataBind();

            var listArt = (from a in uow.ArticulosBL.Get()
                           join um in uow.UnidadesDeMedidaBL.Get()
                           on a.UnidadesDeMedidaId equals um.Id
                           join p in uow.PresentacionesBL.Get()
                           on a.PresentacionId equals p.Id
                           select new { Id = a.Id, Nombre = a.Nombre + " " + um.Nombre + " " + a.CantidadUDM + " " + p.Nombre + " " + a.Porcentaje });

            ddlMedicamentos.DataSource = listArt;
            ddlMedicamentos.DataValueField = "Id";
            ddlMedicamentos.DataTextField = "Nombre";
            ddlMedicamentos.DataBind();
            ddlMedicamentos.Items.Insert(0, new ListItem("Ninguno...", "0"));
            ddlMedicamentos.SelectedValue = "0";
        }

        private void BindGridRecetas()
        {
            gridRecetas.DataSource = uow.RecetasBusinessLogic.Get(e=>e.Status==1).ToList();
            gridRecetas.DataBind();
        }

        private void BindGridDetalleRecetas()
        {
            int idReceta = Utilerias.StrToInt(_IDReceta.Value);

            gridDetalleRecetas.DataSource = uow.RecetasArticulosBusinessLogic.Get(e => e.RecetaId == idReceta).ToList();
            gridDetalleRecetas.DataBind();

        }

        private void BindControlesReceta()
        {
            int idReceta = Utilerias.StrToInt(_IDReceta.Value);

            Recetas obj = uow.RecetasBusinessLogic.GetByID(idReceta);
            txtFolio.Value = obj.Folio.ToString();
            txtNombre.Value = obj.NombrePaciente;
            txtObservaciones.Value = obj.Observaciones;
            txtFecha.Value = obj.Fecha.ToShortDateString();

        }


        private void ModoEdicion()
        {
            divEncabezado.Style.Add("display", "none");
            divCaptura.Style.Add("display", "block");
            divGuardarReceta.Style.Add("display", "none");
            divReceta.Style.Add("display", "block");
            divDetalleReceta.Style.Add("display", "block");

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");

        }

        private void ModoNuevo()
        {
            divEncabezado.Style.Add("display", "none");
            divCaptura.Style.Add("display", "block");
            divGuardarReceta.Style.Add("display", "none");
            divReceta.Style.Add("display", "none");
            divDetalleReceta.Style.Add("display", "block");

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        private void GuardarReceta()
        {
            Recetas obj;
            int idReceta = Utilerias.StrToInt(_IDReceta.Value);
            string M = string.Empty;

            if (_Accion.Value.Equals("N"))
                obj = new Recetas();
            else
                obj = uow.RecetasBusinessLogic.GetByID(idReceta);

            obj.Ejercicio = 2015;
            obj.Folio = Utilerias.StrToInt(txtFolio.Value);
            obj.Observaciones = txtObservaciones.Value;
            obj.NombrePaciente = txtNombre.Value;
            obj.Fecha = Convert.ToDateTime(txtFecha.Value);
            obj.Status = 1;

            if (_Accion.Value.Equals("N"))
                uow.RecetasBusinessLogic.Insert(obj);
            else
                uow.RecetasBusinessLogic.Update(obj);

            uow.SaveChanges();

            if (uow.Errors.Count > 0)
            {
                foreach (string err in uow.Errors)
                    M += err;

                //MANEJAR EL ERROR
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = M;

                if (_Accion.Value.Equals("A"))
                    ModoEdicion();
                else
                {
                    divEncabezado.Style.Add("display", "none");
                    divCaptura.Style.Add("display", "block");
                    divReceta.Style.Add("display", "block");
                    divGuardarReceta.Style.Add("display", "block");
                    divDetalleReceta.Style.Add("display", "none");
                }
                    

                return;
            }

            //Se tiene que bindear el grid de recetas
            BindGridRecetas();
            _IDReceta.Value = obj.Id.ToString();

            if (_Accion.Value.Equals("N"))
            {
                BindGridDetalleRecetas();
                ModoNuevo();
            }
            else
                ModoEdicion();
            

        }


        private void GuardarDetalleReceta()
        {
            RecetaArticulos obj;
            int idReceta = Utilerias.StrToInt(_IDReceta.Value);
            string M = string.Empty;

            obj = new RecetaArticulos();

            obj.RecetaId = idReceta;
            obj.NombreMedicamento = txtNombreMedicamento.Value;
            obj.CantidadATomar = txtCandidad.Value;
            obj.Frecuenca = txtFrecuencia.Value;
            obj.Durante = txtDurante.Value;

            if (!ddlMedicamentos.SelectedValue.Equals("0"))
                obj.ArticuloId = Utilerias.StrToInt(ddlMedicamentos.SelectedValue);

            uow.RecetasArticulosBusinessLogic.Insert(obj);
            uow.SaveChanges();

            if (uow.Errors.Count > 0)
            {
                foreach (string err in uow.Errors)
                    M += err;

                //MANEJAR EL ERROR
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = M;

                if (_Accion.Value.Equals("A"))
                    ModoEdicion();
                else
                    ModoNuevo();


                return;
            }

            BindGridDetalleRecetas();

            txtNombreMedicamento.Value = string.Empty;
            txtCandidad.Value = string.Empty;
            txtFrecuencia.Value = string.Empty;
            txtDurante.Value = string.Empty;
            ddlMedicamentos.SelectedValue = "0";
            txtNombreMedicamento.Disabled = false;


            if (_Accion.Value.Equals("N"))
                //Mostramos la parte de detalle de receta, donde se indicaran los
                //articulos que conformaran la receta
                ModoNuevo();
            else
                ModoEdicion();
            

        }

        protected void gridRecetas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int idReceta = Utilerias.StrToInt(gridRecetas.DataKeys[e.Row.RowIndex].Values["Id"].ToString());
                HtmlButton btnVer = (HtmlButton)e.Row.FindControl("btnVer");

                if (btnVer!=null)
                    btnVer.Attributes["onclick"] = "fnc_MostrarReceta(" + idReceta + ")";

            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarReceta();
        }

        protected void ddlMedicamentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMedicamentos.SelectedValue.Equals("0"))
            {
                txtNombreMedicamento.Value = "";
                txtNombreMedicamento.Disabled = false;
            }
            else
            {
                txtNombreMedicamento.Value = ddlMedicamentos.SelectedItem.Text;
                txtNombreMedicamento.Disabled = true;
            }
                

            if (_Accion.Value.Equals("A"))
                ModoEdicion();
            else
                ModoNuevo();
            
        }

        protected void btnGuardarDetalle_Click(object sender, EventArgs e)
        {
            if (_Accion.Value.Equals("A"))
                GuardarReceta();

            GuardarDetalleReceta();
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDReceta.Value = gridRecetas.DataKeys[row.RowIndex].Value.ToString();

            Recetas obj = uow.RecetasBusinessLogic.GetByID(Utilerias.StrToInt(_IDReceta.Value));

            if (obj.CreatedAt.Value.Date == DateTime.Now.Date)
            {
                _Accion.Value = "A";
                BindControlesReceta();
                BindGridDetalleRecetas();

                //Limpiar los campos del detalle de la receta
                txtNombreMedicamento.Value = string.Empty;
                txtCandidad.Value = string.Empty;
                txtFrecuencia.Value = string.Empty;
                txtDurante.Value = string.Empty;
                ddlMedicamentos.SelectedValue = "0";
                txtNombreMedicamento.Disabled = false;


                ModoEdicion();
            }
            else
            {
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "block");
                divCaptura.Style.Add("display", "none");
                
                lblMsgError.Text = "No se puede modificar la receta, esta fuera de fecha";

            }

        }

        protected void gridRecetas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridRecetas.PageIndex = e.NewPageIndex;
            BindGridRecetas();
            divCaptura.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void gridDetalleRecetas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridDetalleRecetas.PageIndex = e.NewPageIndex;
            BindGridDetalleRecetas();

            if (_Accion.Value.Equals("N"))
                ModoNuevo();
            else
                ModoEdicion();
        }

        protected void imgBtnEliminarReceta_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDReceta.Value = gridRecetas.DataKeys[row.RowIndex].Value.ToString();
            string M = string.Empty;

            Recetas obj = uow.RecetasBusinessLogic.GetByID(Utilerias.StrToInt(_IDReceta.Value));

            if (obj.CreatedAt.Value.Date == DateTime.Now.Date)
            {
                obj.Status = 3;

                uow.RecetasBusinessLogic.Update(obj);
                uow.SaveChanges();

                if (uow.Errors.Count > 0)
                {
                    foreach (string err in uow.Errors)
                        M += err;

                    //MANEJAR EL ERROR
                    divMsgError.Style.Add("display", "block");
                    divMsgSuccess.Style.Add("display", "none");
                    lblMsgError.Text = M;
                    divEncabezado.Style.Add("display", "block");
                    divCaptura.Style.Add("display", "none");


                    return;
                }

                BindGridRecetas();

                divMsgError.Style.Add("display", "none");
                divMsgSuccess.Style.Add("display", "block");
                lblMsgSuccess.Text = "Se ha eliminado correctamente la receta";
                divEncabezado.Style.Add("display", "block");
                divCaptura.Style.Add("display", "none");

            }
            else
            {
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "block");
                divCaptura.Style.Add("display", "none");

                lblMsgError.Text = "No se puede Eliminar la receta, esta fuera de fecha";

            }
        }

        protected void imgBtnEliminarDetalle_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            int idRecetaDetalle =Utilerias.StrToInt(gridDetalleRecetas.DataKeys[row.RowIndex].Value.ToString());
            string M = string.Empty;


            RecetaArticulos obj = uow.RecetasArticulosBusinessLogic.GetByID(idRecetaDetalle);

            uow.RecetasArticulosBusinessLogic.Delete(obj);
            uow.SaveChanges();

            if (uow.Errors.Count > 0)
            {
                foreach (string err in uow.Errors)
                    M += err;

                //MANEJAR EL ERROR
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = M;

                if (_Accion.Value.Equals("N"))
                    ModoNuevo();
                else
                    ModoEdicion();


                return;
            }

            BindGridDetalleRecetas();

            if (_Accion.Value.Equals("N"))
                ModoNuevo();
            else
                ModoEdicion();


        }

        protected void gridDetalleRecetas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //int id = Utilerias.StrToInt(gridDetalleRecetas.DataKeys[e.Row.RowIndex].Values["Id"].ToString());
                

                //if (btnVer != null)
                //    btnVer.Attributes["onclick"] = "fnc_MostrarReceta(" + idReceta + ")";

            }
        }

        
    }
}