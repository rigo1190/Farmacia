using BusinessLogicLayer;
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
    public partial class wfConsultarSalidas : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork(Session["IdUser"].ToString());

            if (!IsPostBack)
            {
                txtFechaI.Value = DateTime.Now.ToShortDateString();
                txtFechaT.Value = DateTime.Now.ToShortDateString();

                BindDropDowns();

                string filtro = ArmarFiltro();
                List<DataAccessLayer.Models.AlmacenSalidasGenericas> list = ObtenerSalidasFiltro(filtro);

                BindGridSalidas(list);
            }

        }

        private void BindGridSalidas(List<DataAccessLayer.Models.AlmacenSalidasGenericas> list)
        {
            gridSalidas.DataSource = list; //uow.AlmacenSalidasGenericasBL.Get().ToList();
            gridSalidas.DataBind();
        }

        private void BindDropDowns()
        {

            var listArt = (from a in uow.ArticulosBL.Get()
                           join um in uow.UnidadesDeMedidaBL.Get()
                           on a.UnidadesDeMedidaId equals um.Id
                           join p in uow.PresentacionesBL.Get()
                           on a.PresentacionId equals p.Id
                           select new { Id = a.Id, Nombre = a.Nombre + " " + um.Nombre + " " + a.CantidadUDM + " " + p.Nombre + " " + a.Porcentaje });

            ddlArticulos.DataSource = listArt;
            ddlArticulos.DataValueField = "Id";
            ddlArticulos.DataTextField = "Nombre";
            ddlArticulos.DataBind();

            ddlTipos.DataSource = uow.TipoSalidaBusinessLogic.Get().ToList();
            ddlTipos.DataValueField = "Id";
            ddlTipos.DataTextField = "Nombre";
            ddlTipos.DataBind();
            
            

        }


        private string ArmarFiltro()
        {
            string filtro = string.Empty;
            bool vacio = true;

            if (!txtFolio.Value.Trim().Equals(string.Empty))
            {
                filtro += "(AlmacenSalidasGenericas.Folio= " + txtFolio.Value + " )";
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
                    filtro += "(AlmacenSalidasGenericas.Fecha between '" + fechaInicio + "' and '" + fechaTermino + "')";
                    vacio = false;
                }
                else
                    filtro += " AND (AlmacenSalidasGenericas.Fecha between '" + fechaInicio + "' and '" + fechaTermino + "')";

                //Se dejan visibles los divs correspondientes a los controles de los filtros
                divFechaI.Style.Add("display", "block");
                divFechaT.Style.Add("display", "block");

            }
            else
            {
                divFechaI.Style.Add("display", "none");
                divFechaT.Style.Add("display", "none");
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

            if (chkTipoSalida.Checked)
            {
                string idTipo = ddlTipos.SelectedValue;

                if (vacio)
                {
                    filtro += "(TipoSalida.Id= " + idTipo + ")";
                }
                else 
                {
                    filtro += " AND (TipoSalida.Id= " + idTipo + ")";
                }

                divTipoSalida.Style.Add("display", "block");
            }
            else 
            {
                divTipoSalida.Style.Add("display", "none");
            }



            //Se agrega la Clausula WHERE a el filtro
            if (!filtro.Equals(string.Empty))
                filtro = "WHERE " + filtro;


            return filtro;


        }


        private List<DataAccessLayer.Models.AlmacenSalidasGenericas> ObtenerSalidasFiltro(string filtro)
        {
            List<DataAccessLayer.Models.AlmacenSalidasGenericas> list = new List<DataAccessLayer.Models.AlmacenSalidasGenericas>();
            string connString = @"data source=RIGO-PC\SQLEXPRESS;user id=sa;password=081995;initial catalog=BD3SoftInventarios;Persist Security Info=true";
            DataAccessLayer.Models.AlmacenSalidasGenericas salida;
            SqlConnection conn = null;
            string ids = string.Empty; ;

            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();//new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pa_ConsultaSalidas";

                SqlParameter param = cmd.Parameters.Add("@Where", SqlDbType.NVarChar);
                param.Direction = ParameterDirection.Input;
                param.Value = filtro;


                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    salida = uow.AlmacenSalidasGenericasBL.GetByID(rdr[0]);
                    list.Add(salida);

                    //Se agrega ids
                    if (ids.Equals(string.Empty))
                        ids += rdr[0].ToString();
                    else
                        ids += "," + rdr[0].ToString();
                }

                _IDsSalidas.Value = ids;


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



        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            string filtro = ArmarFiltro();
            List<DataAccessLayer.Models.AlmacenSalidasGenericas> list = ObtenerSalidasFiltro(filtro);
            BindGridSalidas(list);
        }

        protected void gridSalidas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int id = Utilerias.StrToInt(gridSalidas.DataKeys[e.Row.RowIndex].Values["Id"].ToString());
                HtmlButton btnVer = (HtmlButton)e.Row.FindControl("btnVer");

                if (btnVer != null)
                    btnVer.Attributes["onclick"] = "fnc_MostrarVenta(" + id + ")";

            }
        }

        protected void gridSalidas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridSalidas.PageIndex = e.NewPageIndex;
            string filtro = ArmarFiltro();
            List<DataAccessLayer.Models.AlmacenSalidasGenericas> list = ObtenerSalidasFiltro(filtro);
            BindGridSalidas(list);
        }
    }
}