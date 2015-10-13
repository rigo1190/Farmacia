using BusinessLogicLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SIP.Formas.Ventas
{
    public partial class wfConsultarVentas : System.Web.UI.Page
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
                txtFechaI.Value = DateTime.Now.ToShortDateString();
                txtFechaT.Value = DateTime.Now.ToShortDateString();
                chkRango.Checked = true;

                BindDropDowns();
                string filtro = ArmarFiltro();
                List<DataAccessLayer.Models.Ventas> list = ObtenerVentasFiltro(filtro);
                BindGridVentas(list);

                divFechaI.Style.Add("display", "block");
                divFechaT.Style.Add("display", "block");

            }
        }


        private void BindDropDowns()
        {
            ddlClientes.DataSource = uow.ClientesBL.Get().ToList();
            ddlClientes.DataValueField = "Id";
            ddlClientes.DataTextField = "RazonSocial";
            ddlClientes.DataBind();

            var listArt = (from a in uow.ArticulosBL.Get()
                           join um in uow.UnidadesDeMedidaBL.Get()
                           on a.UnidadesDeMedidaId equals um.Id
                           join p in uow.PresentacionesBL.Get()
                           on a.PresentacionId equals p.Id
                           select new { Id = a.Id, Nombre = a.NombreCompleto }).OrderBy(e=>e.Nombre);

            ddlArticulos.DataSource = listArt;
            ddlArticulos.DataValueField = "Id";
            ddlArticulos.DataTextField = "Nombre";
            ddlArticulos.DataBind();


            ddlUsuarios.DataSource = uow.UsuarioBusinessLogic.Get().ToList();
            ddlUsuarios.DataValueField = "Id";
            ddlUsuarios.DataTextField = "Nombre";
            ddlUsuarios.DataBind();

        }

        private string ArmarFiltro()
        {
            string filtro = string.Empty;
            bool vacio = true;

            if (!txtFolio.Value.Trim().Equals(string.Empty))
            {
                filtro += "(Ventas.Folio= " + txtFolio.Value + " )";
                vacio = false;
            }


            if (chkRango.Checked)
            {
                DateTime fecha1 = Convert.ToDateTime(txtFechaI.Value);
                DateTime fecha2 = Convert.ToDateTime(txtFechaT.Value);

                string fechaInicio = fecha1.ToString("yyyy-MM-dd") + " 00:00:00";
                string fechaTermino = fecha2.ToString("yyyy-MM-dd") + " 00:00:00";

                if (vacio)
                {
                    filtro += "(Ventas.Fecha between '" + fechaInicio + "' and '" + fechaTermino + "')";
                    vacio = false;
                }
                else
                    filtro += " AND (Ventas.Fecha between '" + fechaInicio + "' and '" + fechaTermino + "')";

                //Se dejan visibles los divs correspondientes a los controles de los filtros
                divFechaI.Style.Add("display", "block");
                divFechaT.Style.Add("display", "block");

            }
            else 
            {
                divFechaI.Style.Add("display", "none");
                divFechaT.Style.Add("display", "none");
            }

            if (chkCliente.Checked)
            {
                string idCliente = ddlClientes.SelectedValue;

                if (vacio)
                {
                    filtro += "(Ventas.ClienteId= " + idCliente + " )";
                    vacio = false;
                }
                else
                    filtro += "AND (Ventas.ClienteId= " + idCliente + " )";


                //Se dejan visibles los divs correspondientes a los controles de los filtros
                divCliente.Style.Add("display", "block");
            }
            else 
            {
                divCliente.Style.Add("display", "none");
            }

            if (chkProducto.Checked) 
            {
                string idProducto = ddlArticulos.SelectedValue;

                if (vacio)
                {
                    filtro += "(Articulos.Id= " + idProducto + " )";
                    vacio = false;
                }
                else
                    filtro += "AND (Articulos.Id= " + idProducto + " )";

                //Se dejan visibles los divs correspondientes a los controles de los filtros
                divArticulos.Style.Add("display", "block");
            }
            else
            {
                divArticulos.Style.Add("display", "none");
            }

            if (chkUsuario.Checked)
            {
                string idUsuario = ddlUsuarios.SelectedValue;

                if (vacio)
                    filtro += "(Usuario.Id= " + idUsuario + " )";
                else
                    filtro += "AND (Usuario.Id= " + idUsuario + " )";

                //Se dejan visibles los divs correspondientes a los controles de los filtros
                divUsuario.Style.Add("display", "block");
            }
            else
            {
                divUsuario.Style.Add("display", "none");
            }

            //Se agrega la Clausula WHERE a el filtro
            if (!filtro.Equals(string.Empty))
                filtro = "WHERE " + filtro;// +" AND (Ventas.Status=1)";
            

            return filtro;


        }




        private List<DataAccessLayer.Models.Ventas> ObtenerVentasFiltro(string filtro)
        {
            List<DataAccessLayer.Models.Ventas> list= new List<DataAccessLayer.Models.Ventas>();
            //string connString = @"data source=RIGO-PC\SQLEXPRESS;user id=sa;password=081995;initial catalog=BD3SoftInventarios;Persist Security Info=true";//System.Configuration.ConfigurationManager.ConnectionStrings[0].ConnectionString;
            //string connString = System.Configuration.ConfigurationManager.ConnectionStrings[0].ConnectionString;
            DataAccessLayer.Models.Ventas venta;
            //SqlConnection conn=null;
            string ids = string.Empty; ;

            SqlConnection conn = new SqlConnection(uow.Contexto.Database.Connection.ConnectionString.ToString());
            try
            {
                //conn = new SqlConnection(connString);
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();//new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pa_ConsultaVentas";

                SqlParameter param = cmd.Parameters.Add("@Where", SqlDbType.NVarChar);
                param.Direction = ParameterDirection.Input;
                param.Value = filtro;


                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    venta = uow.VentasBL.GetByID(rdr[0]);
                    list.Add(venta);

                    //Se agrega ids
                    if (ids.Equals(string.Empty))
                        ids += rdr[0].ToString();
                    else
                        ids += "," + rdr[0].ToString();
                }

                _IDsVenta.Value = ids;


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
            finally 
            {
                if (conn!=null)
                    conn.Close();
            }
            
            return list;

        }


        private void BindGridVentas(List<DataAccessLayer.Models.Ventas> listVentas)
        {
            gridVentas.DataSource = listVentas;
            gridVentas.DataBind();
        }

        protected void gridVentas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridVentas.PageIndex = e.NewPageIndex;
            string filtro = ArmarFiltro();
            List<DataAccessLayer.Models.Ventas> list = ObtenerVentasFiltro(filtro);
            BindGridVentas(list);
        }

        protected void gridVentas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int id = Utilerias.StrToInt(gridVentas.DataKeys[e.Row.RowIndex].Values["Id"].ToString());
                HtmlButton btnVer = (HtmlButton)e.Row.FindControl("btnVer");

                if (btnVer != null)
                    btnVer.Attributes["onclick"] = "fnc_MostrarVenta(" + id + ")";

            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            string filtro = ArmarFiltro();
            List<DataAccessLayer.Models.Ventas> list = ObtenerVentasFiltro(filtro);
            BindGridVentas(list);
        }
    }
}