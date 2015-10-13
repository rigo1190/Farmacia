using DataAccessLayer;
using DataAccessLayer.Models;
using BusinessLogicLayer;

using System.Data;
using System.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIP.Formas.Inventarios
{
    public partial class wfKardex : System.Web.UI.Page
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

            _URLVisor.Value = ResolveClientUrl("~/rpts/wfVerReporte.aspx");

            if (!IsPostBack)
            {
                BindCombos();
            }

        }

        private void BindCombos()
        {
            List<Articulos> lista = uow.ArticulosBL.Get(p => p.Status == 1).OrderBy(q => q.NombreCompleto).ToList();
            ddlArticulo.DataSource = lista;
            ddlArticulo.DataValueField = "Id";
            ddlArticulo.DataTextField = "NombreCompleto";
            ddlArticulo.DataBind();
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(ddlArticulo.SelectedValue);

            EjecutaStoreProcedure(id);
            ClientScript.RegisterStartupScript(this.GetType(), "script", "fnc_verReporte()", true);
        }

        private void EjecutaStoreProcedure(int producto)
        {
            SqlConnection sqlConnection1 = new SqlConnection(uow.Contexto.Database.Connection.ConnectionString.ToString());
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            SqlDataReader rs;
            SqlCommand com2; 
            string sql;
            try
            {                

                cmd.CommandText = "sp_kardex";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection1;

                cmd.Parameters.Add("@articulo", producto);                
                sqlConnection1.Open();

                reader = cmd.ExecuteReader();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
            finally
            {
                
                sqlConnection1.Close();
            }

        }

        




    }
}