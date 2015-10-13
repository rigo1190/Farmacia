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
    public partial class wfSalidasGenericas : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            if (!IsPostBack)
            {
                ResetearSalida();
                BindGridProductosCatalago();
                


                int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());
                Usuario user = uow.UsuarioBusinessLogic.GetByID(idUser);

                if (user.Nivel == 3)
                {
                    ddlTipos.DataSource = uow.TipoSalidaBusinessLogic.Get(p => p.EsUsoInterno == true);
                    DIVSoloGuardar.Style.Add("display", "none");
                }

                else
                {
                    ddlTipos.DataSource = uow.TipoSalidaBusinessLogic.Get();                    
                    DIVguardarWithModal.Style.Add("display", "none");
                    txtPassword.Text = user.Password;
                }

                ddlTipos.DataValueField = "Id";
                ddlTipos.DataTextField = "Nombre";
                ddlTipos.DataBind();

            }
        }




        private void BindGridProductosCatalago()
        {
            var listArt = (from a in uow.ArticulosBL.Get()
                           join um in uow.UnidadesDeMedidaBL.Get()
                           on a.UnidadesDeMedidaId equals um.Id
                           join p in uow.PresentacionesBL.Get()
                           on a.PresentacionId equals p.Id
                           select new
                           {
                               Id = a.Id,
                               NombreCompleto = a.NombreCompleto,
                               EsMedicamento = a.esMedicamento,
                               PrecioVentaIVA = a.PrecioVentaIVA,
                               Clave = a.Clave,
                               CantidadDisponible = a.CantidadDisponible
                           }).OrderBy(e => e.NombreCompleto);

            gridProductosCatalogo.DataSource = listArt.ToList();//uow.ArticulosBL.Get().ToList();
            gridProductosCatalogo.DataBind();
        }

        private void BindGridProductosSalidas()
        {
            //int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());
            List<ArticuloSalidaGenerica> listArticulos = new List<ArticuloSalidaGenerica>();//uow.ArticuloSalidaGenericaBusinessLogic.Get(e => e.UsuarioId == idUser).ToList();

            //Se lee la cadena de _ProductosVenta
            if (!_ProductosVenta.Value.Equals(string.Empty))
            {
                string[] ids = _ProductosVenta.Value.Split('|');

                foreach (string id in ids)
                    listArticulos.Add(uow.ArticuloSalidaGenericaBusinessLogic.GetByID(Utilerias.StrToInt(id)));

            }


            gridProductos.DataSource = listArticulos;
            gridProductos.DataBind();
        }

        private void ResetearSalida()
        {
            string M = string.Empty;

            //Se eliminan los articulos de la tabla temporal

            uow.ArticuloSalidaGenericaBusinessLogic.DeleteAll();
            uow.SaveChanges();

            

            //Se bindea el grid
            _ProductosVenta.Value = string.Empty;
            BindGridProductosSalidas();

            //Se limpia la cadena de valores de la lista de productos del catalogo
            _CadValoresSeleccionados.Value = string.Empty;

            //Se limpian los campos de DAtos de la Salida
            txtFolio.Value = string.Empty;
            txtFecha.Value = DateTime.Now.ToShortDateString();
            txtObservaciones.Value = string.Empty;

            
        }

        private int ObtenerMaxFolio()
        {
            int max = 1;

            if (uow.AlmacenSalidasGenericasBL.Get(e=>e.Ejercicio==DateTime.Now.Year).Count() > 0)
                max = uow.AlmacenSalidasGenericasBL.Get().Max(e => e.Folio) + 1;

            return max;

        }

        private string ArmarFolioCadena(int folio)
        {
            string folioCad = string.Empty;
            string num = string.Format("{0:0000}", folio);
            folioCad = "SAL/" + num + "/" + DateTime.Now.Year;

            return folioCad;
        }

        private void OcultarError()
        {
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
            lblMsgError.Text = "";
        }

        
        

        private bool ExisteProductoEnSalida(int idProducto)
        {
            string[] ids = _ProductosVenta.Value.Split('|');

            foreach (string id in ids)
            {
                int idArtSalida = Utilerias.StrToInt(id);
                ArticuloSalidaGenerica artVenta = uow.ArticuloSalidaGenericaBusinessLogic.Get(e => e.Id == idArtSalida && e.ArticuloId == idProducto).FirstOrDefault();

                if (artVenta != null)
                    return true;
                else
                    continue;
            }

            return false;

        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            string cadenaValores = _CadValoresSeleccionados.Value;

            if (cadenaValores.Equals(string.Empty))
                return;

            string[] ids = cadenaValores.Split('|');
            int idProducto;
            Articulos articulo;
            string M = string.Empty;
            int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());
           


            foreach (string id in ids)
            {
                idProducto = Utilerias.StrToInt(id);

                articulo = uow.ArticulosBL.GetByID(idProducto);

                if (!_ProductosVenta.Value.Equals(string.Empty))
                {
                    if (ExisteProductoEnSalida(articulo.Id))
                        continue;
                }

                ArticuloSalidaGenerica artSalida = new ArticuloSalidaGenerica();
                    artSalida.ArticuloId = idProducto;
                    artSalida.Cantidad = 1;
                    artSalida.Nombre = ConcatenarNombreArticulo(idProducto);//articulo.Nombre;
                    artSalida.UsuarioId = idUser;
                uow.ArticuloSalidaGenericaBusinessLogic.Insert(artSalida);
                uow.SaveChanges();

                //SI HUBO ERORRES 
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    divMsgError.Style.Add("display", "block");
                    divMsgSuccess.Style.Add("display", "none");
                    lblMsgError.Text = M;

                    return;
                }

                if (_ProductosVenta.Value == string.Empty)
                    _ProductosVenta.Value = artSalida.Id.ToString();
                else
                    _ProductosVenta.Value += "|" + artSalida.Id.ToString();

            }

            _CadValoresSeleccionados.Value = string.Empty;

            BindGridProductosSalidas();

            OcultarError();
        }


        private string ConcatenarNombreArticulo(int idArticulo)
        {
            string nombre = string.Empty;

            var art = (from a in uow.ArticulosBL.Get(e => e.Id == idArticulo)
                       join um in uow.UnidadesDeMedidaBL.Get()
                       on a.UnidadesDeMedidaId equals um.Id
                       join p in uow.PresentacionesBL.Get()
                       on a.PresentacionId equals p.Id
                       select new
                       {
                           Nombre = a.Nombre + " (" + p.Nombre + " " + a.CantidadUnidadMedida + " " + um.Nombre + ")",
                       }).FirstOrDefault();

            nombre = art.Nombre;

            return nombre;

        }


        

        protected void btnGenerarSalida_Click(object sender, EventArgs e)
        {

            int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());
            Usuario usuario = uow.UsuarioBusinessLogic.GetByID(idUser);

            if (usuario.Nivel == 3)
            {
                string pass = txtPassword.Text;
                Usuario user = uow.UsuarioBusinessLogic.Get(u => u.Password == pass).FirstOrDefault();

                if (user == null)
                {
                    divMsgError.Style.Add("display", "block");
                    divMsgSuccess.Style.Add("display", "none");
                    lblMsgError.Text = "El password es incorrecto, intente de nuevo.";

                    return;
                }

                idUser = user.Id;

            }


            

            
            


            //detalle de la venta
            int usuarioSesion = int.Parse(Session["IdUser"].ToString());
            List<ArticuloSalidaGenerica> listArticulos = uow.ArticuloSalidaGenericaBusinessLogic.Get(p => p.UsuarioId == usuarioSesion).ToList();




            AlmacenSalidasGenericas salida = new AlmacenSalidasGenericas();
                salida.Folio = ObtenerMaxFolio();
                salida.Ejercicio = DateTime.Now.Year;  
                salida.Fecha = Convert.ToDateTime(txtFecha.Value);
                salida.TipoSalidaId = int.Parse(ddlTipos.SelectedValue);
                salida.Observaciones = txtObservaciones.Value;
                salida.FolioCadena = ArmarFolioCadena(salida.Folio);
                salida.UsuarioId = idUser;
            uow.AlmacenSalidasGenericasBL.Insert(salida);



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
            bitacora.AlmacenSalidaGenerica = salida;
            bitacora.Fecha = DateTime.Now;
            bitacora.Status = 1;
            bitacora.Movimiento = movimiento;
            uow.ArticulosMovimientosBL.Insert(bitacora);


             

            foreach (ArticuloSalidaGenerica item in listArticulos)
            {
                
                AlmacenSalidasGenericasArticulos salidaDetalle = new AlmacenSalidasGenericasArticulos();
                    salidaDetalle.AlmacenSalidaGenerica = salida;
                    salidaDetalle.ArticuloId = item.ArticuloId;
                    salidaDetalle.Cantidad = item.Cantidad;
                uow.AlmacenSalidasGenericasArticulos.Insert(salidaDetalle);


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
                uow.ArticuloSalidaGenericaBusinessLogic.DeleteAll();
                uow.SaveChanges();
            }
            else
            {
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = "Hubo problemas al registrar la salida, intentelo nuevamente";
                return;
            }

            
            //Se muestra la Salida
            //ClientScript.RegisterStartupScript(this.GetType(), "script", "fnc_MostrarSalida(" + salida.Id + ")", true);

            Response.Redirect("wfListaSalidasGen.aspx");

            

        }

        protected void btnCancelarSalida_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfListaSalidasGen.aspx");
        }

        


        #region EventosDelGrid


        protected void gridProductosCatalogo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridProductosCatalogo.PageIndex = e.NewPageIndex;
            BindGridProductosCatalago();
            OcultarError();
        }

        protected void gridProductos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridProductos.EditIndex = e.NewEditIndex;
            BindGridProductosSalidas();
            OcultarError();
        }

        protected void gridProductos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridProductos.EditIndex = -1;
            BindGridProductosSalidas();
            OcultarError();
        }

        protected void gridProductos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string M = string.Empty;
            int idArticulo = Utilerias.StrToInt(gridProductos.DataKeys[e.RowIndex].Value.ToString());
            ArticuloSalidaGenerica objArticulo = uow.ArticuloSalidaGenericaBusinessLogic.GetByID(idArticulo);

            int cantidad = Utilerias.StrToInt(((HtmlInputGenericControl)gridProductos.Rows[e.RowIndex].FindControl("txtCantidad")).Value);

            if (cantidad == 0)
                cantidad = 1;

            Articulos artCat = uow.ArticulosBL.GetByID(objArticulo.ArticuloId);

            if (cantidad > artCat.CantidadDisponible)
            {
                //gridProductos.EditIndex = -1;
                //BindGridProductosSalidas();
                //return;
                cantidad = artCat.CantidadDisponible;
            }


            objArticulo.Cantidad = cantidad;

            uow.ArticuloSalidaGenericaBusinessLogic.Update(objArticulo);
            uow.SaveChanges();

            //SI HUBO ERORRES AL ELIMINAR REGISTROS PREVIOS
            if (uow.Errors.Count > 0)
            {
                foreach (string m in uow.Errors)
                    M += m;

                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = M;

                return;
            }

            // Cancelamos la edicion del grid
            gridProductos.EditIndex = -1;
            BindGridProductosSalidas();
            OcultarError();
        }

        protected void gridProductos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string M = string.Empty;
            int idArticulo = Utilerias.StrToInt(gridProductos.DataKeys[e.RowIndex].Value.ToString());
            ArticuloSalidaGenerica objArticulo = uow.ArticuloSalidaGenericaBusinessLogic.GetByID(idArticulo);
            string[] idsProducto = _ProductosVenta.Value.Split('|');


            uow.ArticuloSalidaGenericaBusinessLogic.Delete(objArticulo);
            uow.SaveChanges();

            //SI HUBO ERORRES AL ELIMINAR REGISTROS PREVIOS
            if (uow.Errors.Count > 0)
            {
                foreach (string m in uow.Errors)
                    M += m;

                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = M;

                return;
            }

            _ProductosVenta.Value = string.Empty;

            foreach (string id in idsProducto)
            {
                if (!id.Equals(idArticulo.ToString()))
                {
                    if (_ProductosVenta.Value.Equals(string.Empty))
                        _ProductosVenta.Value = id;
                    else
                        _ProductosVenta.Value += "|" + id;
                }
            }

            BindGridProductosSalidas();

            OcultarError();
        }

        protected void gridProductosCatalogo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int id = Utilerias.StrToInt(gridProductosCatalogo.DataKeys[e.Row.RowIndex].Values["Id"].ToString());
                HtmlControl chk = (HtmlControl)e.Row.FindControl("chkSeleccionar");
                Articulos art = uow.ArticulosBL.GetByID(id);

                if (art.CantidadDisponible <= 0)
                {
                    if (chk != null)
                    {
                        chk.Disabled = true;
                    }
                }



                //if (btnVer != null)
                //    btnVer.Attributes["onclick"] = "fnc_MostrarReceta(" + idReceta + ")";

            }
        }
        
        #endregion

    }
}