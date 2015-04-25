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
                BindDropDownTipos();
            }
        }


        private void BindDropDownTipos()
        {
            int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());
            Usuario user = uow.UsuarioBusinessLogic.GetByID(idUser);

            if (user.EsAdmin)
                ddlTipos.DataSource = uow.TipoSalidaBusinessLogic.Get();
            else
                ddlTipos.DataSource = uow.TipoSalidaBusinessLogic.Get(e=>e.EsUsoInterno==true);

            
            ddlTipos.DataValueField = "Id";
            ddlTipos.DataTextField = "Nombre";
            ddlTipos.DataBind();
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
                               Nombre = a.Nombre + " " + a.CantidadUnidadMedida + " " + um.Nombre + " " + p.Nombre + " " + a.Porcentaje,
                               EsMedicamento = a.esMedicamento,
                               PrecioVenta = a.PrecioVenta,
                               CantidadDisponible = a.CantidadDisponible
                           }).OrderBy(e=>e.Nombre);

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

        private string ResetearSalida()
        {
            string M = string.Empty;

            //Se eliminan los articulos de la tabla temporal
            M=EliminarArticuloSalida();

            if (!M.Equals(string.Empty))
                return M;

            //Se bindea el grid
            _ProductosVenta.Value = string.Empty;
            BindGridProductosSalidas();

            //Se limpia la cadena de valores de la lista de productos del catalogo
            _CadValoresSeleccionados.Value = string.Empty;

            //Se limpian los campos de DAtos de la Salida
            txtFolio.Value = string.Empty;
            txtFecha.Value = DateTime.Now.ToShortDateString();
            txtObservaciones.Value = string.Empty;

            return M;
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

        private string ActualizarProductosCatalogo(List<ArticuloSalidaGenerica> listArticulos)
        {
            //Actualizar los articulos del catalalogo

            Articulos artCatalogo;
            string M = string.Empty;

            foreach (ArticuloSalidaGenerica artSalida in listArticulos)
            {
                artCatalogo = uow.ArticulosBL.GetByID(artSalida.ArticuloId);

                artCatalogo.CantidadEnAlmacen -= artSalida.Cantidad;
                artCatalogo.CantidadDisponible -= artSalida.Cantidad;


                uow.ArticulosBL.Update(artCatalogo);
                uow.SaveChanges();


                //SI HUBO ERORRES
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    return M;
                }

            }

            BindGridProductosCatalago();

            return M;
        }

        private string CrearArticulosMovimientos(int salidaId, List<ArticuloSalidaGenerica> listArticulos, int idUser)
        {
            //Crear registros para la tabla de ArticulosMovimientos y sus detalles ArticulosMovimientosSalidas
            string M = string.Empty;
            int movimiento = uow.ArticulosMovimientosBL.Get().Count() > 0 ? uow.ArticulosMovimientosBL.Get().Max(e => e.Movimiento) + 1 : 1;

            ArticulosMovimientosSalidas artMovSalida;

            ArticulosMovimientos artMovimiento = new ArticulosMovimientos();

            artMovimiento.Ejercicio = DateTime.Now.Year; //Preguntar que se debe de poner aqui????
            artMovimiento.Tipo = 2; //preguntar cual tipo es????
            artMovimiento.AlmacenSalidaGenericaId = salidaId;
            artMovimiento.Fecha = DateTime.Now; //Preguntar cual es la fecha que se debe de colocar???
            artMovimiento.Status = 1; //Preguntar cual es la Status que se debe de colocar???
            artMovimiento.Movimiento = movimiento;
            artMovimiento.CreatedById = idUser;

            uow.ArticulosMovimientosBL.Insert(artMovimiento);
            uow.SaveChanges();


            //SI HUBO ERORRES
            if (uow.Errors.Count > 0)
            {
                foreach (string m in uow.Errors)
                    M += m;

                return M;
            }

            //Se prosigue a llenar el detalle en ArticulosMovimientosSalidas
            foreach (ArticuloSalidaGenerica artVenta in listArticulos)
            {
                artMovSalida = new ArticulosMovimientosSalidas();

                artMovSalida.ArticuloMovimientoId = artMovimiento.Id;
                artMovSalida.ArticuloId = artVenta.ArticuloId;
                artMovSalida.Cantidad = artVenta.Cantidad;
                artMovSalida.CreatedById = idUser;

                uow.ArticulosMovimientosSalidasBL.Insert(artMovSalida);
                uow.SaveChanges();


                //SI HUBO ERORRES
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    return M;
                }

            }

            return M;


        }

        private string EliminarArticuloSalida()
        {
            //int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());
            string M = string.Empty;
            //Se recorre la lista de productos de la venta
            string[] idsProductos = _ProductosVenta.Value.Split('|');

            foreach (string id in idsProductos)
            {
                if (!id.Equals(string.Empty))
                {
                    uow.ArticuloSalidaGenericaBusinessLogic.Delete(Utilerias.StrToInt(id));
                    uow.SaveChanges();

                    //SI HUBO ERORRES AL ELIMINAR REGISTROS PREVIOS
                    if (uow.Errors.Count > 0)
                    {
                        foreach (string m in uow.Errors)
                            M += m;

                        return M;
                    }
                }
            }

            return M;
        }

        private string CrearSalida(int userId)
        {
            string M = string.Empty;
            int idUser = userId;//Utilerias.StrToInt(Session["IdUser"].ToString());

            AlmacenSalidasGenericas salida = new AlmacenSalidasGenericas();

            salida.Folio = ObtenerMaxFolio();
            salida.Ejercicio = DateTime.Now.Year; //???????
            salida.Fecha = Convert.ToDateTime(txtFecha.Value);
            salida.TipoSalidaId = Utilerias.StrToInt(ddlTipos.SelectedValue);
            salida.Observaciones = txtObservaciones.Value;
            salida.FolioCadena = ArmarFolioCadena(salida.Folio);
            salida.CreatedById = idUser;

            uow.AlmacenSalidasGenericasBL.Insert(salida);
            uow.SaveChanges();

            //SI HUBO ERORRES
            if (uow.Errors.Count > 0)
            {
                foreach (string m in uow.Errors)
                    M += m;

                return M;
            }

            //Se prosigue a llenar el detalle en AlmacenSalidasGenericasArticulos
            //con los articulos detallados

            List<ArticuloSalidaGenerica> listArticulos = new List<ArticuloSalidaGenerica>();//uow.ArticuloSalidaGenericaBusinessLogic.Get(e => e.UsuarioId == idUser).ToList();
            string[] idsProductos = _ProductosVenta.Value.Split('|');

            foreach (string id in idsProductos)
            {
                ArticuloSalidaGenerica artSalida = uow.ArticuloSalidaGenericaBusinessLogic.GetByID(Utilerias.StrToInt(id));
                AlmacenSalidasGenericasArticulos salidaDetalle = new AlmacenSalidasGenericasArticulos();

                salidaDetalle.AlmacenSalidaGenericaId = salida.Id;
                salidaDetalle.ArticuloId = artSalida.ArticuloId;
                salidaDetalle.Cantidad = artSalida.Cantidad;
                salidaDetalle.CreatedById = idUser;

                uow.AlmacenSalidasGenericasArticulos.Insert(salidaDetalle);
                uow.SaveChanges();


                //SI HUBO ERORRES
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    return M;
                }

            }


            M=ActualizarProductosCatalogo(listArticulos);  //Se actualizan cantidades del articulo en el catalogo

            if (!M.Equals(string.Empty))
                return M;

            M=CrearArticulosMovimientos(salida.Id, listArticulos, idUser);  //Se generan registros en ArticulosMovimientos y sus detalles

            if (!M.Equals(string.Empty))
                return M;

            M=ResetearSalida();  //Se limpian los campos para una nueva salida


            if (!M.Equals(string.Empty))
                return M;

            //Se muestra la Salida
            ClientScript.RegisterStartupScript(this.GetType(), "script", "fnc_MostrarSalida(" + salida.Id + ")", true);

            return M;
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
                           Nombre = a.Nombre + " " + a.CantidadUnidadMedida + " " + um.Nombre + " " + p.Nombre + " " + a.Porcentaje,
                       }).FirstOrDefault();

            nombre = art.Nombre;

            return nombre;

        }


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

            Articulos artCat = uow.ArticulosBL.GetByID(objArticulo.ArticuloId);

            if (artCat.CantidadDisponible - cantidad < 0)
                cantidad = (artCat.CantidadDisponible - cantidad) + cantidad;
            
            
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

        protected void btnGenerarSalida_Click(object sender, EventArgs e)
        {
            string M = string.Empty;

            string pass = txtPassword.Text;

            Usuario user = uow.UsuarioBusinessLogic.Get(u => u.Password == pass).FirstOrDefault();

            if (user == null)
            {
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = "El password es incorrecto, intente de nuevo.";

                return;
            }


            M=CrearSalida(user.Id);

            //Se muestra el mensaje de error, si es que existe
            if (!M.Equals(string.Empty))
            {
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = M;

                return;
            }

            OcultarError();
        }

        protected void btnCancelarSalida_Click(object sender, EventArgs e)
        {
            string M = string.Empty;

            M=ResetearSalida();

            //Se muestra el mensaje de error
            if (!M.Equals(string.Empty))
            {
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = M;

                return;
            }

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
    }
}