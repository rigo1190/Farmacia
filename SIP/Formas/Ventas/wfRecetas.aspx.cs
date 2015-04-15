using BusinessLogicLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
                txtFechaFiltro.Value = DateTime.Now.ToShortDateString();
                txtFecha.Value = DateTime.Now.ToShortDateString();
                txtFecha.Disabled = true;
                BindGridRecetas();
                BindDropDownMedicamentos();
                BindDropDownPacientes();

            }

        }

        private void BindDropDownPacientes()
        {
            ddlPaciente.DataSource = uow.PacienteBusinessLogic.Get().OrderBy(e=>e.Nombre).ToList();
            ddlPaciente.DataValueField = "Id";
            ddlPaciente.DataTextField = "Nombre";
            ddlPaciente.DataBind();

            ddlPaciente.Items.Insert(0, new ListItem("Ninguno...", "0"));
            ddlPaciente.SelectedValue = "0";
        }

        private void BindDropDownMedicamentos()
        {
            var listArt = (from a in uow.ArticulosBL.Get()
                           join um in uow.UnidadesDeMedidaBL.Get()
                           on a.UnidadesDeMedidaId equals um.Id
                           join p in uow.PresentacionesBL.Get()
                           on a.PresentacionId equals p.Id
                           select new { Id = a.Id, Nombre = a.Nombre + " " + um.Nombre + " " + p.Nombre + " " + a.Porcentaje }).OrderBy(e=>e.Nombre);

            ddlMedicamentos.DataSource = listArt;
            ddlMedicamentos.DataValueField = "Id";
            ddlMedicamentos.DataTextField = "Nombre";
            ddlMedicamentos.DataBind();
            ddlMedicamentos.Items.Insert(0, new ListItem("Ninguno...", "0"));
            ddlMedicamentos.SelectedValue = "0";
        }

        private void BindGridRecetas()
        {
            DateTime fechaFiltro = DateTime.Now;
            if (!txtFechaFiltro.Value.Equals(string.Empty))
                fechaFiltro = Convert.ToDateTime(txtFechaFiltro.Value);

            gridRecetas.DataSource = uow.RecetasBusinessLogic.Get(e=>e.Fecha==fechaFiltro).ToList();
            gridRecetas.DataBind();
        }

        private void BindGridDetalleRecetas()
        {
            int idReceta = Utilerias.StrToInt(_IDReceta.Value);

            gridDetalleRecetas.DataSource = uow.RecetasArticulosBusinessLogic.Get(e => e.RecetaId == idReceta).ToList();
            gridDetalleRecetas.DataBind();

        }

        private int ObtenerMaxFolio()
        {
            int max = 1;

            if (uow.RecetasBusinessLogic.Get(e=>e.Ejercicio==DateTime.Now.Year).Count() > 0)
                max = uow.RecetasBusinessLogic.Get().Max(e => e.Folio) + 1;

            return max;

        }

        private void BindControlesReceta()
        {
            int idReceta = Utilerias.StrToInt(_IDReceta.Value);

            Recetas obj = uow.RecetasBusinessLogic.GetByID(idReceta);

            txtFolio.Value = obj.Folio.ToString();
            txtNombre.Value = obj.NombrePaciente;
            txtObservaciones.Value = obj.Observaciones;
            txtFecha.Value = obj.Fecha.ToShortDateString();
            ddlPaciente.SelectedValue = obj.PacienteId != null ? obj.PacienteId.ToString() : "0";
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
            divBtnImagen.Style.Add("display", "block");
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
            divBtnImagen.Style.Add("display", "none");
        }

        private string ArmarFolioCadena(int folio)
        {
            string folioCad = string.Empty;
            string num = string.Format("{0:0000}", folio);
            folioCad = "REC/" + num + "/" + DateTime.Now.Year;

            return folioCad;
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

            obj.Observaciones = txtObservaciones.Value;
            obj.NombrePaciente = txtNombre.Value;
            obj.Status = 1;

            if (_Accion.Value.Equals("N")){

                obj.Ejercicio = DateTime.Now.Year;
                obj.Folio = ObtenerMaxFolio();
                obj.FolioCadena = ArmarFolioCadena(obj.Folio);
                obj.Fecha = Convert.ToDateTime(txtFecha.Value);

                uow.RecetasBusinessLogic.Insert(obj);
            }
                
            else
                uow.RecetasBusinessLogic.Update(obj);

            M = GuardarImagenReceta();

            if (!M.Equals(string.Empty))
            {
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

        private string EliminarArchivo(int id, string nombreArchivo)
        {
            string M = string.Empty;
            try
            {
                string ruta = string.Empty;

                //eliminar archivo
                ruta = System.Configuration.ConfigurationManager.AppSettings["ImagenesRecetas"];

                if (!ruta.EndsWith("/"))
                    ruta += "/";

                ruta += id.ToString() + "/";

                if (ruta.StartsWith("~") || ruta.StartsWith("/"))   //Es una ruta relativa al sitio
                    ruta = Server.MapPath(ruta);

                File.Delete(ruta + "\\" + nombreArchivo);
                Directory.Delete(ruta);

            }
            catch (Exception ex)
            {
                M = ex.Message;
            }


            return M;
        }

        public string GuardarArchivo(HttpPostedFile postedFile, int idFicha)
        {

            string M = string.Empty;
            string ruta = string.Empty;

            try
            {
                ruta = System.Configuration.ConfigurationManager.AppSettings["ImagenesRecetas"];

                if (!ruta.EndsWith("/"))
                    ruta += "/";

                ruta += idFicha.ToString() + "/";

                if (ruta.StartsWith("~") || ruta.StartsWith("/"))   //Es una ruta relativa al sitio
                    ruta = Server.MapPath(ruta);


                if (!Directory.Exists(ruta))
                    Directory.CreateDirectory(ruta);

                ruta += postedFile.FileName;

                postedFile.SaveAs(ruta);

            }
            catch (Exception ex)
            {
                M = ex.Message;
            }

            return M;

        }


        private string GuardarImagenReceta()
        {
            //Se almacena el archivo

            RecetasImagenes newImg=null;
            string M=string.Empty;
            int idReceta=Utilerias.StrToInt(_IDReceta.Value);

            Recetas obj = uow.RecetasBusinessLogic.GetByID(idReceta);

            if (!fileUpload.PostedFile.FileName.Equals(string.Empty))
            {
                if (fileUpload.FileBytes.Length > 10485296)
                {
                    M = "Se ha excedido en el tamaño del archivo, el máximo permitido es de 10 Mb";
                    return M;
                }

                if (obj.detalleImagenes!=null)
                    newImg = obj.detalleImagenes.Where(e => e.NombreArchivo == fileUpload.FileName && e.TipoArchivo == fileUpload.PostedFile.ContentType).FirstOrDefault();


                if (newImg == null)
                {
                    newImg = new RecetasImagenes();
                    newImg.RecetaId = obj.Id;
                    newImg.TipoArchivo = fileUpload.PostedFile.ContentType;
                    newImg.NombreArchivo = Path.GetFileName(fileUpload.FileName);

                    uow.RecetasImagenesBusinessLogic.Insert(newImg);
                    //obj.detalleImagenes.Add(newImg);
                }

                M = GuardarArchivo(fileUpload.PostedFile, obj.Id);

                if (!M.Equals(string.Empty))
                {
                    return M;
                }
            }

            return M;
        }

        private void GuardarDetalleReceta()
        {
            RecetaArticulos obj;
            int idReceta = Utilerias.StrToInt(_IDReceta.Value);
            string M = string.Empty;

            M = GuardarImagenReceta();

            if (!M.Equals(string.Empty))
            {
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = M;

                if (_Accion.Value.Equals("A"))
                    ModoEdicion();
                else
                    ModoNuevo();

                return;
            }
            
            
            obj = new RecetaArticulos();

            obj.RecetaId = idReceta;
            obj.NombreMedicamento = txtNombreMedicamento.Value;
            obj.CantidadATomar = txtCandidad.Value;
            obj.Frecuenca = txtFrecuencia.Value;
            obj.Durante = txtDurante.Value;
            obj.Observaciones = txtObsParticulares.Value;

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
            txtObsParticulares.Value = string.Empty;

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

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            BindGridRecetas();

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void btnGuardarImagen_Click(object sender, EventArgs e)
        {
            string M = GuardarImagenReceta();

            if (!M.Equals(string.Empty))
            {
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = M;

                if (_Accion.Value.Equals("A"))
                    ModoEdicion();
                else
                    ModoNuevo();

                return;
            }

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
        }

        
    }
}