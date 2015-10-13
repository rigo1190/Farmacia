using BusinessLogicLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SIP.Formas.Inventarios
{
    public partial class wfInventarioFisico : System.Web.UI.Page
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


            if (!IsPostBack)
            {
                _URLVisor.Value = ResolveClientUrl("~/rpts/wfVerReporte.aspx");

                cargarArticulos();
                BindGrid();
                txtCode.Focus();
            }

        }


        private void cargarArticulos()
        {

            int iduser = int.Parse(Session["IdUser"].ToString());

            uow.InventarioFisicoBL.DeleteAll();
            uow.SaveChanges();

            List<Articulos> lista = uow.ArticulosBL.Get().ToList();

            foreach (Articulos item in lista)
            {
                InventarioFisico obj = new InventarioFisico();

                obj.UsuarioId = iduser;
                obj.ArticuloId = item.Id;
                obj.Clave = item.Clave;
                obj.ExistenciaEnSistema = item.CantidadEnAlmacen;
                obj.Cantidad = 0;

                uow.InventarioFisicoBL.Insert(obj);
            }
            uow.SaveChanges();

        }


        private void BindGrid()
        {
            List<InventarioFisico> lista = uow.InventarioFisicoBL.Get(p => p.Cantidad > 0).OrderBy(q => q.Articulo.NombreCompleto).ToList();
            gridInventarioFisico.DataSource = lista;
            gridInventarioFisico.DataBind();
        }

        protected void gridInventarioFisico_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Utilerias.StrToInt(gridInventarioFisico.DataKeys[e.RowIndex].Value.ToString());
            
            InventarioFisico obj = uow.InventarioFisicoBL.GetByID(id);
            
            obj.Cantidad--;

            uow.InventarioFisicoBL.Update(obj);
            uow.SaveChanges();
            BindGrid();
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            int idUser = Utilerias.StrToInt(Session["IdUser"].ToString());


            List<InventarioFisico> lista = uow.InventarioFisicoBL.Get(p => p.Clave == txtCode.Text).ToList();

            foreach (InventarioFisico item in lista)
            {
                item.Cantidad++;
                uow.InventarioFisicoBL.Update(item);
            }

            uow.SaveChanges();
            BindGrid();
            txtCode.Text = string.Empty;
            txtCode.Focus();

        }

    }
}