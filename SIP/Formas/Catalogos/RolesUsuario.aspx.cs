using BusinessLogicLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;

namespace SIP.Formas.Catalogos
{
    public partial class RolesUsuario : System.Web.UI.Page
    {
        UnitOfWork uow;

        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());     
        }

      
        [WebMethod]
        public static IQueryable<Rol> GetRoles()
        {
            UnitOfWork uow = new UnitOfWork();
            IQueryable<Rol> list = uow.RolBusinessLogic.Get();          
            return list;

        }

        [WebMethod]
        public static Rol GetRecord(int id)
        {

            UnitOfWork uow = new UnitOfWork();

            Rol rol = uow.RolBusinessLogic.GetByID(id);

            return rol;
           
        }

        [WebMethod]
        public static result AddRecord(string clave, string nombre, int orden,bool dependencia,bool sefiplan) 
        {

            UnitOfWork uow = new UnitOfWork();

            Rol rol = new Rol { Clave = clave, Nombre = nombre, Orden = orden,EsDependencia=dependencia,EsSefiplan=sefiplan };

            uow.RolBusinessLogic.Insert(rol);

            uow.SaveChanges();

            if (uow.Errors.Count > 0) 
            {
                return new result { OK=false,Errors=uow.Errors};
            }
            
            return new result { OK = true };
        }

        [WebMethod]
        public static result UpdateRecord(int id,string clave, string nombre, int orden,bool dependencia,bool sefiplan)
        {

            UnitOfWork uow = new UnitOfWork();

            Rol rol = uow.RolBusinessLogic.GetByID(id);
            rol.Clave = clave;
            rol.Nombre = nombre;
            rol.Orden = orden;
            rol.EsDependencia = dependencia;
            rol.EsSefiplan = sefiplan;

            uow.RolBusinessLogic.Update(rol);

            uow.SaveChanges();

            if (uow.Errors.Count > 0)
            {
                return new result { OK = false, Errors = uow.Errors };
            }


            return new result { OK = true };
        }

        [WebMethod]
        public static result DeleteRecord(int id)
        {

            UnitOfWork uow = new UnitOfWork();

            Rol rol = uow.RolBusinessLogic.GetByID(id);

            uow.RolBusinessLogic.Delete(rol);

            uow.SaveChanges();

            if (uow.Errors.Count > 0)
            {
                return new result { OK = false, Errors = uow.Errors };
            }
            
            return new result { OK = true };
        }

        public class result 
        {
            public bool OK { get; set; }          
            public List<string> Errors { get; set; }
        }


    }
}