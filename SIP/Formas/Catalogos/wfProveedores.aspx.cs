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
    public partial class wfProveedores : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            if (!IsPostBack)
            {
                BindGrid();

                ModoForma(false);
            }
        }


        #region metodos

        private void BindGrid()
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            this.grid.DataSource = uow.ProveedoresBL.Get(p=>p.Status==1).ToList();
            this.grid.DataBind();
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
            txtRepresentante.Value = string.Empty;
            txtCalle.Value = string.Empty;
            txtColonia.Value = string.Empty;
            txtCiudad.Value = string.Empty;
            txtEstado.Value = string.Empty;
            txtCP.Value = string.Empty;
            txtTelefonos.Value = string.Empty;
            txtCelular.Value = string.Empty;
            txtEMail.Value = string.Empty;
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _ElId.Text = grid.DataKeys[row.RowIndex].Values["Id"].ToString();

            Proveedores obj = uow.ProveedoresBL.GetByID(int.Parse(_ElId.Text));
            txtClave.Value = obj.RFC;
            txtNombre.Value = obj.RazonSocial;
            txtRepresentante.Value = obj.RepresentanteLegal;
            txtCalle.Value = obj.Calle;
            txtColonia.Value = obj.Colonia;
            txtCiudad.Value = obj.Ciudad;
            txtCP.Value = obj.CodigoPostal.ToString();
            txtTelefonos.Value = obj.Telefonos;
            txtCelular.Value = obj.Celular;
            txtEMail.Value = obj.EMail;

            _Accion.Text = "Modificar";
            ModoForma(true);
        }

        protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _ElId.Text = grid.DataKeys[row.RowIndex].Values["Id"].ToString();


            if (_ElId.Text == "")
                return;

            Proveedores obj = uow.ProveedoresBL.GetByID(int.Parse(_ElId.Text));




            //uow.Errors.Clear();
            //List<Articulos> lista;
            //lista = uow.ArticulosBL.Get(p => p.UnidadesDeMedidaId == obj.Id).ToList();




            //if (lista.Count > 0)
            //    uow.Errors.Add("El registro no puede eliminarse porque ya ha sido usado en el sistema");



            //Se elimina el objeto
            if (uow.Errors.Count == 0)
            {
                //uow.ProveedoresBL.Delete(obj);
                obj.Status = 3;
                uow.ProveedoresBL.Update(obj);
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
            Proveedores obj;

            List<Proveedores> lista;

            String mensaje = "";


            if (_Accion.Text == "Nuevo")
                obj = new Proveedores();
            else
                obj = uow.ProveedoresBL.GetByID(int.Parse(_ElId.Text));


            obj.RFC = txtClave.Value;
            obj.RazonSocial = txtNombre.Value;
            obj.RepresentanteLegal = txtRepresentante.Value;
            obj.Calle = txtCalle.Value;
            obj.Colonia = txtColonia.Value;
            obj.Ciudad = txtCiudad.Value;
            obj.Estado = txtEstado.Value;
            obj.CodigoPostal = int.Parse ( txtCP.Value);
            obj.Telefonos = txtTelefonos.Value;
            obj.Celular = txtCelular.Value;
            obj.EMail = txtEMail.Value;
            obj.Status = 1;

            //validaciones
            uow.Errors.Clear();

            if (_Accion.Text == "Nuevo")
            {
                lista = uow.ProveedoresBL.Get(p => p.RFC == obj.RFC).ToList();
                if (lista.Count > 0)
                    uow.Errors.Add("El RFC que capturo ya ha sido registrada anteriormente, verifique su información");

                lista = uow.ProveedoresBL.Get(p => p.RazonSocial == obj.RazonSocial).ToList();
                if (lista.Count > 0)
                    uow.Errors.Add("La Razón Social que capturo ya ha sido registrada anteriormente, verifique su información");

                uow.ProveedoresBL.Insert(obj);
                mensaje = "El registro se ha  almacenado correctamente";

            }
            else
            { //update

                int xid;

                xid = int.Parse(_ElId.Text);

                lista = uow.ProveedoresBL.Get(p => p.RFC == obj.RFC && p.Id != xid).ToList();
                if (lista.Count > 0)
                    uow.Errors.Add("El RFC que capturo ya ha sido registrada anteriormente, verifique su información");



                lista = uow.ProveedoresBL.Get(p => p.RazonSocial == obj.RazonSocial && p.Id != xid).ToList();
                if (lista.Count > 0)
                    uow.Errors.Add("La Razón Social que capturo ya ha sido registrada anteriormente, verifique su información");


                uow.ProveedoresBL.Update(obj);


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