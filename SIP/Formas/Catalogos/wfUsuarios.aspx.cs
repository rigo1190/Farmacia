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
    public partial class wfUsuarios : System.Web.UI.Page
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
                BindGrid();

                ModoForma(false);
            }

        }




        #region metodos

        private void BindGrid()
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            this.grid.DataSource = uow.UsuarioBusinessLogic.Get(p=>p.Activo==true).ToList();
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

            txtLogin.Value = string.Empty;
            txtPassword.Text = string.Empty;
            txtNombre.Value = string.Empty;
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _ElId.Text = grid.DataKeys[row.RowIndex].Values["Id"].ToString();

            Usuario obj = uow.UsuarioBusinessLogic.GetByID(int.Parse(_ElId.Text));
            txtLogin.Value = obj.Login;
            txtPassword.Text = obj.Password;
            txtNombre.Value = obj.Nombre;


            _Accion.Text = "Modificar";
            ModoForma(true);
        }

        protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _ElId.Text = grid.DataKeys[row.RowIndex].Values["Id"].ToString();


            if (_ElId.Text == "")
                return;
            Usuario obj = uow.UsuarioBusinessLogic.GetByID(int.Parse(_ElId.Text));




            uow.Errors.Clear();
            


            if (obj.Nivel < 3)
                uow.Errors.Add("Los usuarios administradores no pueden ser eliminados");



            //Se elimina el objeto
            if (uow.Errors.Count == 0)
            {
                obj.Activo = false;
                uow.UsuarioBusinessLogic.Update(obj);
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
            Usuario obj;

            List<Usuario> lista;

            String mensaje = "";


            if (_Accion.Text == "Nuevo")
                obj = new Usuario();
            else
                obj = uow.UsuarioBusinessLogic.GetByID(int.Parse(_ElId.Text));


            obj.Login= txtLogin.Value;
            obj.Password = txtPassword.Text;
            obj.Nombre = txtNombre.Value;
            obj.Activo = true;

            if (_Accion.Text == "Nuevo")
                obj.Nivel = 3;


            //validaciones
            uow.Errors.Clear();

            if (_Accion.Text == "Nuevo")
            {
                lista = uow.UsuarioBusinessLogic.Get(p => p.Login == obj.Login).ToList();
                if (lista.Count > 0)
                    uow.Errors.Add("El login que capturo ya ha sido registrada anteriormente, verifique su información");

                lista = uow.UsuarioBusinessLogic.Get(p => p.Nombre == obj.Nombre).ToList();
                if (lista.Count > 0)
                    uow.Errors.Add("El Nombre que capturo ya ha sido registrada anteriormente, verifique su información");

                uow.UsuarioBusinessLogic.Insert(obj);
                mensaje = "El registro se ha  almacenado correctamente";

            }
            else
            { //update

                int xid;

                xid = int.Parse(_ElId.Text);

                lista = uow.UsuarioBusinessLogic.Get(p => p.Login == obj.Login && p.Id != xid).ToList();
                if (lista.Count > 0)
                    uow.Errors.Add("La Clave que capturo ya ha sido registrada anteriormente, verifique su información");



                lista = uow.UsuarioBusinessLogic.Get(p => p.Nombre == obj.Nombre && p.Id != xid).ToList();
                if (lista.Count > 0)
                    uow.Errors.Add("La Descripción que capturo ya ha sido registrada anteriormente, verifique su información");


                uow.UsuarioBusinessLogic.Update(obj);


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