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
            ddlTipos.DataSource = uow.TipoSalidaBusinessLogic.Get();
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
                               Nombre = a.Nombre + " " + um.Nombre + " " + a.CantidadUDM + " " + p.Nombre + " " + a.Porcentaje,
                               EsMedicamento = a.esMedicamento,
                               PrecioVenta = a.PrecioVenta,
                               CantidadDisponible = a.CantidadDisponible
                           });

            gridProductosCatalogo.DataSource = listArt.ToList();//uow.ArticulosBL.Get().ToList();
            gridProductosCatalogo.DataBind();
        }

        private void BindGridProductosSalidas()
        {
            int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());
            List<ArticuloSalidaGenerica> listArticulos = uow.ArticuloSalidaGenericaBusinessLogic.Get(e => e.UsuarioId == idUser).ToList();

            gridProductos.DataSource = listArticulos;
            gridProductos.DataBind();
        }

        private void ResetearSalida()
        {
            //Se eliminan los articulos de la tabla temporal
            EliminarArticuloSalida();

            //Se bindea el grid
            BindGridProductosSalidas();

            //Se limpia la cadena de valores de la lista de productos del catalogo
            _CadValoresSeleccionados.Value = string.Empty;

            //Se limpian los campos de DAtos de la Salida
            txtFolio.Value = string.Empty;
            txtFecha.Value = string.Empty;
            txtObservaciones.Value = string.Empty;

        }


        private void ActualizarProductosCatalogo(List<ArticuloSalidaGenerica> listArticulos)
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

                    //return M;
                }

            }

            BindGridProductosCatalago();
        }

        private void CrearArticulosMovimientos(int salidaId, List<ArticuloSalidaGenerica> listArticulos)
        {
            //Crear registros para la tabla de ArticulosMovimientos y sus detalles ArticulosMovimientosSalidas
            string M = string.Empty;
            int movimiento = uow.ArticulosMovimientosBL.Get().Count() > 0 ? uow.ArticulosMovimientosBL.Get().Max(e => e.Movimiento) + 1 : 1;

            ArticulosMovimientosSalidas artMovSalida;

            ArticulosMovimientos artMovimiento = new ArticulosMovimientos();

            artMovimiento.Ejercicio = 2015; //Preguntar que se debe de poner aqui????
            artMovimiento.Tipo = 1; //preguntar cual tipo es????
            artMovimiento.AlmacenSalidaGenericaId = salidaId;
            artMovimiento.Fecha = DateTime.Now; //Preguntar cual es la fecha que se debe de colocar???
            artMovimiento.Status = 1; //Preguntar cual es la Status que se debe de colocar???
            artMovimiento.Movimiento = movimiento;

            uow.ArticulosMovimientosBL.Insert(artMovimiento);
            uow.SaveChanges();


            //SI HUBO ERORRES
            if (uow.Errors.Count > 0)
            {
                foreach (string m in uow.Errors)
                    M += m;

                //return M;
            }

            //Se prosigue a llenar el detalle en ArticulosMovimientosSalidas
            foreach (ArticuloSalidaGenerica artVenta in listArticulos)
            {
                artMovSalida = new ArticulosMovimientosSalidas();

                artMovSalida.ArticuloMovimientoId = artMovimiento.Id;
                artMovSalida.ArticuloId = artVenta.ArticuloId;
                artMovSalida.Cantidad = artVenta.Cantidad;

                uow.ArticulosMovimientosSalidasBL.Insert(artMovSalida);
                uow.SaveChanges();


                //SI HUBO ERORRES
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    //return M;
                }

            }


        }

        private void EliminarArticuloSalida()
        {
            int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());

            bool eliminados = uow.ArticuloSalidaGenericaBusinessLogic.DeleteAll(e => e.UsuarioId == idUser);
            string M = string.Empty;

            if (eliminados)
            {
                uow.SaveChanges();

                //SI HUBO ERORRES AL ELIMINAR REGISTROS PREVIOS
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    //return M;
                }
            }


        }

        private void CrearSalida()
        {
            string M = string.Empty;
            int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());

            AlmacenSalidasGenericas salida = new AlmacenSalidasGenericas();

            salida.Folio = Utilerias.StrToInt(txtFolio.Value);
            salida.Ejercicio = 2015; //???????
            salida.Fecha = Convert.ToDateTime(txtFecha.Value);
            salida.TipoSalidaId = Utilerias.StrToInt(ddlTipos.SelectedValue);
            salida.Observaciones = txtObservaciones.Value;

            uow.AlmacenSalidasGenericasBL.Insert(salida);
            uow.SaveChanges();

            //SI HUBO ERORRES
            if (uow.Errors.Count > 0)
            {
                foreach (string m in uow.Errors)
                    M += m;

                //return M;
            }

            //Se prosigue a llenar el detalle en AlmacenSalidasGenericasArticulos
            //con los articulos detallados

            List<ArticuloSalidaGenerica> listArticulos = uow.ArticuloSalidaGenericaBusinessLogic.Get(e => e.UsuarioId == idUser).ToList();

            foreach (ArticuloSalidaGenerica artSalida in listArticulos)
            {
                AlmacenSalidasGenericasArticulos salidaDetalle = new AlmacenSalidasGenericasArticulos();

                salidaDetalle.AlmacenSalidaGenericaId = salida.Id;
                salidaDetalle.ArticuloId = artSalida.ArticuloId;
                salidaDetalle.Cantidad = artSalida.Cantidad;

                uow.AlmacenSalidasGenericasArticulos.Insert(salidaDetalle);
                uow.SaveChanges();


                //SI HUBO ERORRES
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    //return M;
                }

            }


            ActualizarProductosCatalogo(listArticulos);  //Se actualizan cantidades del articulo en el catalogo

            CrearArticulosMovimientos(salida.Id, listArticulos);  //Se generan registros en ArticulosMovimientos y sus detalles

            ResetearSalida();  //Se limpian los campos para una nueva salida

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

                    //return M;
                }

            }

            _CadValoresSeleccionados.Value = string.Empty;

            BindGridProductosSalidas();
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
                           Nombre = a.Nombre + " " + um.Nombre + " " + a.CantidadUDM + " " + p.Nombre + " " + a.Porcentaje,
                       }).FirstOrDefault();

            nombre = art.Nombre;

            return nombre;

        }


        protected void gridProductosCatalogo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridProductosCatalogo.PageIndex = e.NewPageIndex;
            BindGridProductosCatalago();
        }

        protected void gridProductos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridProductos.EditIndex = e.NewEditIndex;
            BindGridProductosSalidas();
        }

        protected void gridProductos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridProductos.EditIndex = -1;
            BindGridProductosSalidas();
        }

        protected void gridProductos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string M = string.Empty;
            int idArticulo = Utilerias.StrToInt(gridProductos.DataKeys[e.RowIndex].Value.ToString());
            ArticuloSalidaGenerica objArticulo = uow.ArticuloSalidaGenericaBusinessLogic.GetByID(idArticulo);

            int cantidad = Utilerias.StrToInt(((HtmlInputGenericControl)gridProductos.Rows[e.RowIndex].FindControl("txtCantidad")).Value);

            objArticulo.Cantidad = cantidad;

            uow.ArticuloSalidaGenericaBusinessLogic.Update(objArticulo);
            uow.SaveChanges();

            //SI HUBO ERORRES AL ELIMINAR REGISTROS PREVIOS
            if (uow.Errors.Count > 0)
            {
                foreach (string m in uow.Errors)
                    M += m;

                //return M;
            }

            // Cancelamos la edicion del grid
            gridProductos.EditIndex = -1;

            BindGridProductosSalidas();
        }

        protected void gridProductos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string M = string.Empty;
            int idArticulo = Utilerias.StrToInt(gridProductos.DataKeys[e.RowIndex].Value.ToString());
            ArticuloSalidaGenerica objArticulo = uow.ArticuloSalidaGenericaBusinessLogic.GetByID(idArticulo);

            uow.ArticuloSalidaGenericaBusinessLogic.Delete(objArticulo);
            uow.SaveChanges();

            //SI HUBO ERORRES AL ELIMINAR REGISTROS PREVIOS
            if (uow.Errors.Count > 0)
            {
                foreach (string m in uow.Errors)
                    M += m;

                //return M;
            }

            BindGridProductosSalidas();
        }

        protected void btnGenerarSalida_Click(object sender, EventArgs e)
        {
            CrearSalida();
        }

        protected void btnCancelarSalida_Click(object sender, EventArgs e)
        {
            ResetearSalida();
        }
    }
}