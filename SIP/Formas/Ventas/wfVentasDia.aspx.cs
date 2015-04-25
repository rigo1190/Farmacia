using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIP.Formas.Ventas
{
    public partial class wfVentasDia : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            if (!IsPostBack)
            {
                txtFechaFiltro.Value = DateTime.Now.ToShortDateString();

                string filtro = ArmarFiltro();
                List<DataAccessLayer.Models.Ventas> list = ObtenerVentasFiltro(filtro);

                BindGridVentas(list);

                
            }
        }

        private string ArmarFiltro()
        {
            string filtro = string.Empty;
            bool vacio = true;

            DateTime fecha1 = Convert.ToDateTime(txtFechaFiltro.Value);
            string fecha = fecha1.ToString("yyyy-MM-dd") + " 00:00:00";


            if (chkRecetas.Checked)
            {

               if (vacio)
                {
                    filtro += "(Ventas.RecetaId is not null)";
                    vacio = false;
                }
                else
                   filtro += " AND (Ventas.RecetaId is not null)";
            }


            if (chkCat.Checked)
            {
                if (vacio)
                {
                    filtro += "(Ventas.RecetaId is null)";
                    vacio = false;
                }
                else
                    filtro += "AND (Ventas.RecetaId is null)";
            }


            if (chkRecetas.Checked && chkCat.Checked)
                filtro = string.Empty;


            if (filtro.Equals(string.Empty))
                filtro += "(Ventas.Fecha = '"+ fecha + "')";
            else
                filtro += "AND (Ventas.Fecha = '" + fecha + "')";
            
            //Se agrega la Clausula WHERE a el filtro
            if (!filtro.Equals(string.Empty))
                filtro = "WHERE " + filtro;


            return filtro;


        }

        private List<DataAccessLayer.Models.Ventas> ObtenerVentasFiltro(string filtro)
        {
            List<DataAccessLayer.Models.Ventas> list = new List<DataAccessLayer.Models.Ventas>();
            //string connString = @"data source=RIGO-PC\SQLEXPRESS;user id=sa;password=081995;initial catalog=BD3SoftInventarios;Persist Security Info=true";//System.Configuration.ConfigurationManager.ConnectionStrings[0].ConnectionString;
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings[2].ConnectionString;
            DataAccessLayer.Models.Ventas venta;
            SqlConnection conn = null;
            string ids = string.Empty; ;

            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();//new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pa_VentasPorDia";

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

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

            return list;

        }


        private void BindGridVentas(List<DataAccessLayer.Models.Ventas> listVentas)
        {
            //DateTime fechaFiltro = Convert.ToDateTime(txtFechaFiltro.Value);
            gridVentas.DataSource = listVentas;//uow.VentasBL.Get(e => e.Fecha == fechaFiltro);
             gridVentas.DataBind();

        }

        protected void btnVR_Click(object sender, EventArgs e)
        {
            Response.Redirect("VentasRecetas.aspx");
        }

        protected void btnV_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfVentas.aspx");
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            string filtro = ArmarFiltro();
            List<DataAccessLayer.Models.Ventas> list = ObtenerVentasFiltro(filtro);
            BindGridVentas(list);
        }

        protected void gridVentas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int id = Utilerias.StrToInt(gridVentas.DataKeys[e.Row.RowIndex].Values["Id"].ToString());
                Label lblTipoVenta = (Label)e.Row.FindControl("lblTipoVenta");

                DataAccessLayer.Models.Ventas obj = uow.VentasBL.GetByID(id);

                lblTipoVenta.Text = obj.RecetaId != null ? "POR RECETA" : "POR CATÁLOGO";

            }
        }



    }
}