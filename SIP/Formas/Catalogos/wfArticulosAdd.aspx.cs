using BusinessLogicLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SIP.Formas.Catalogos
{
    public partial class wfArticulosAdd : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            if (!IsPostBack)
            {
                BindGrid();
                BindCombos();
                ModoForma(false);
            }
        }


        #region metodos

        private void BindGrid()
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());
            int grupo = int.Parse(Request.QueryString["grupo"].ToString());

            this.grid.DataSource = uow.ArticulosBL.Get(p=> p.GruposPSId == grupo && p.Status == 1).ToList();
            this.grid.DataBind();


            GruposPS grupoPS = uow.GruposPSBL.GetByID(grupo);

            this.txtTitulo.Text = "Artículos del Grupo : " + grupoPS.Clave + " - " + grupoPS.Nombre;
            this.txtTituloBis.Text = "Artículos del Grupo : " + grupoPS.Clave + " - " + grupoPS.Nombre;


        }



        private void BindCombos()
        {

            ddlUM.DataSource = uow.UnidadesDeMedidaBL.Get();
            ddlUM.DataValueField = "Id";
            ddlUM.DataTextField = "Nombre";
            ddlUM.DataBind();

            //ddlUnidadMedida.Items.Insert(0, new ListItem("Seleccione...", "0"));


            ddlPresentacion.DataSource = uow.PresentacionesBL.Get();
            ddlPresentacion.DataValueField = "Id";
            ddlPresentacion.DataTextField = "Nombre";
            ddlPresentacion.DataBind();

            ddlFPS.DataSource = uow.FPSfactoresBL.Get();
            ddlFPS.DataValueField = "Id";
            ddlFPS.DataTextField = "Nombre";
            ddlFPS.DataBind();

        }              


        private void ModoForma(bool modoCaptura)
        {

            this.divMsg.Style.Add("display", "none");
            this.divMsgSuccess.Style.Add("display", "none");

            if (modoCaptura)
            {
                this.divDatos.Style.Add("display", "none");
                this.divBtnNuevo.Style.Add("display", "none");
                this.divCaptura.Style.Add("display", "block");

            }
            else
            {
                this.divDatos.Style.Add("display", "block");
                this.divBtnNuevo.Style.Add("display", "block");
                this.divCaptura.Style.Add("display", "none");
            }



        }


        #endregion


        #region eventos
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            _Accion.Text = "Nuevo";
            ModoForma(true);

            txtClave.Value = string.Empty;
            txtNombre.Value = string.Empty;

            
            ddlUM.SelectedIndex  = 0;
            ddlPresentacion.SelectedIndex = 0;
            ddlFPS.SelectedIndex = 0;

            txtPorcentaje.Value = string.Empty;
            txtSustanciaActiva.Value = string.Empty;
            txtObservaciones.Value = string.Empty;
            chkEsMedicamento.Checked = false;

            txtStockMinimo.Value = string.Empty;
            txtPrecioCompra.Value = string.Empty;
            txtPrecioVenta.Value = string.Empty; 
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _ElId.Text = grid.DataKeys[row.RowIndex].Values["Id"].ToString();



            Articulos obj = uow.ArticulosBL .GetByID(int.Parse(_ElId.Text));

            txtClave.Value = obj.Clave;
            txtNombre.Value = obj.Nombre;
            ddlUM.SelectedValue = obj.UnidadesDeMedidaId.ToString();
            ddlPresentacion.SelectedValue = obj.PresentacionId.ToString();
            ddlFPS.SelectedValue = obj.FPSfactorId.ToString();
            
            txtPorcentaje.Value = obj.Porcentaje.ToString() ;
            txtSustanciaActiva.Value = obj.SustanciaActiva;
            txtObservaciones.Value = obj.Observaciones;
            if (obj.esMedicamento == 1)
            {
                chkEsMedicamento.Checked = true;
            }
            else
            {
                chkEsMedicamento.Checked = false;
            }
            

            txtStockMinimo.Value = obj.StockMinimo.ToString();
            txtPrecioVenta.Value = obj.PrecioVenta.ToString();
            txtPrecioCompra.Value = obj.PrecioCompra.ToString(); 

            _Accion.Text = "Modificar";
            ModoForma(true);
        }

        protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _ElId.Text = grid.DataKeys[row.RowIndex].Values["Id"].ToString();


            if (_ElId.Text == "")
                return;

            Articulos obj = uow.ArticulosBL.GetByID(int.Parse(_ElId.Text));




            //uow.Errors.Clear();
            //List<Articulos> lista;
            //lista = uow.ArticulosBL.Get(p => p.GruposPSId == obj.Id).ToList();

            //if (lista.Count > 0)
            //    uow.Errors.Add("El registro no puede eliminarse porque ya ha sido usado en el sistema");



            //Se elimina el objeto
            if (uow.Errors.Count == 0)
            {
                //uow.ArticulosBL.Delete(obj);
                obj.Status = 3;
                uow.ArticulosBL.Update(obj);

                uow.SaveChanges();
                BindGrid();
            }



            if (uow.Errors.Count == 0)
            {
                lblMensajeSuccess.Text = "El registro se ha eliminado correctamente";
                divMsg.Style.Add("display", "none");
                divMsgSuccess.Style.Add("display", "block");

            }

            else
            {
                string mensaje;

                mensaje = string.Empty;
                foreach (string cad in uow.Errors)
                    mensaje = mensaje + cad + "<br>";

                lblMensajes.Text = mensaje;
                divMsg.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
            }
        }




        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Articulos obj;

            List<Articulos> lista;

            String mensaje = "";


            if (_Accion.Text == "Nuevo")
                obj = new Articulos();
            else
                obj = uow.ArticulosBL.GetByID(int.Parse(_ElId.Text));



            int grupo = int.Parse(Request.QueryString["grupo"].ToString());

            obj.GruposPSId = grupo;
            obj.Clave = txtClave.Value;
            obj.Nombre = txtNombre.Value;
            obj.UnidadesDeMedidaId = int.Parse(ddlUM.SelectedValue.ToString());
            obj.PresentacionId = int.Parse(ddlPresentacion.SelectedValue.ToString());
            obj.FPSfactorId = int.Parse(ddlFPS.SelectedValue.ToString());

            obj.Porcentaje = double.Parse( txtPorcentaje.Value);
            obj.SustanciaActiva = txtSustanciaActiva.Value;
            obj.Observaciones = txtObservaciones.Value;
            if (chkEsMedicamento.Checked) { 
                obj.esMedicamento = 1;
            } else { 
                obj.esMedicamento = 0;
            }
            

            obj.StockMinimo = double.Parse(txtPorcentaje.Value);
            obj.PrecioCompra = decimal.Parse( txtPrecioCompra.Value);
            obj.PrecioVenta = decimal.Parse(txtPrecioVenta.Value);
            obj.Status = 1;


            //validaciones
            uow.Errors.Clear();

            if (_Accion.Text == "Nuevo")
            {
                lista = uow.ArticulosBL.Get(p => p.Clave == obj.Clave).ToList();
                if (lista.Count > 0)
                    uow.Errors.Add("La Clave que capturo ya ha sido registrada anteriormente, verifique su información");

                lista = uow.ArticulosBL.Get(p => p.Nombre == obj.Nombre).ToList();
                if (lista.Count > 0)
                    uow.Errors.Add("La Descripción que capturo ya ha sido registrada anteriormente, verifique su información");

                uow.ArticulosBL.Insert(obj);
                mensaje = "El registro se ha  almacenado correctamente";

            }
            else
            { //update

                int xid;

                xid = int.Parse(_ElId.Text);

                lista = uow.ArticulosBL.Get(p => p.Clave == obj.Clave && p.Id != xid).ToList();
                if (lista.Count > 0)
                    uow.Errors.Add("La Clave que capturo ya ha sido registrada anteriormente, verifique su información");



                lista = uow.ArticulosBL.Get(p => p.Nombre == obj.Nombre && p.Id != xid).ToList();
                if (lista.Count > 0)
                    uow.Errors.Add("La Descripción que capturo ya ha sido registrada anteriormente, verifique su información");


                uow.ArticulosBL.Update(obj);


            }




            if (uow.Errors.Count == 0)
                uow.SaveChanges();



            if (uow.Errors.Count == 0)//Integrando el nuevo nodo en el arbol
            {
                BindGrid();
                ModoForma(false);

                lblMensajeSuccess.Text = "El registro se guardo correctamente";
                divMsgSuccess.Style.Add("display", "block");
                divMsg.Style.Add("display", "none");


            }
            else
            {
                mensaje = string.Empty;
                foreach (string cad in uow.Errors)
                    mensaje = mensaje + cad + "<br>";

                lblMensajes.Text = mensaje;
                divMsg.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
            }


        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoForma(false);
        }

        #endregion
    }
}