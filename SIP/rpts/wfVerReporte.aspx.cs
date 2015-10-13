using BusinessLogicLayer;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace SIP.rpts
{
    public partial class wfVerReporte : System.Web.UI.Page
    {

        public string nombreReporte;
        protected void Page_Load(object sender, EventArgs e)
        {
            int caller = Utilerias.StrToInt(Request.Params["c"].ToString());
            string parametros = Request.Params["p"].ToString();
            string nomReporte = GetNombreReporte(caller);
            
            ReportDocument rdc = new ReportDocument();

            rdc.FileName = Server.MapPath("~/rpts/" + nomReporte);

            if (!parametros.Equals(string.Empty))
                CargarParametros(caller, parametros, ref rdc);

            CargarReporte(rdc);


        }


        private void CargarParametros(int caller, string parametros, ref ReportDocument rdc)
        {
            string[] primerArray = parametros.Split('-');
            DateTime fechaInicio;
            DateTime fechaFin; 

            switch (caller)
            {
                case 1: //RECETA
                    rdc.SetParameterValue("receta", primerArray[0]);
                    break;

                case 2: //VENTA
                    rdc.SetParameterValue("@VentaID", primerArray[0]);
                    break;

                case 3: //VENTAS GENERALES
                    rdc.RecordSelectionFormula = "{pa_VentasGenerales;1.VentaId} in [" + primerArray[0] + "]";
                    fechaInicio = Convert.ToDateTime(primerArray[1]);
                    fechaFin = Convert.ToDateTime(primerArray[2]);

                    rdc.SetParameterValue("fechaIni", fechaInicio);
                    rdc.SetParameterValue("fechaFin", fechaFin);

                    break;

				case 4: //SALIDA
                    rdc.SetParameterValue("@SalidaID", primerArray[0]);
                    break;

                case 5: //SALIDAS GENERALES
                    rdc.RecordSelectionFormula = "{pa_SalidasGenerales;1.SalidaId} in [" + primerArray[0] + "]";
                    break;

                case 6: //ARTICULOS VENDIDOS
                    fechaInicio = Convert.ToDateTime(primerArray[0]);
                    fechaFin = Convert.ToDateTime(primerArray[1]);
                    rdc.SetParameterValue("@fechaInicio", fechaInicio.ToString("yyyy-MM-dd") + " 00:00:00");
                    rdc.SetParameterValue("@fechaFin", fechaFin.ToString("yyyy-MM-dd") + " 00:00:00");
                    break;


                case 11://reporte de ventas x dia
                    fechaInicio = Convert.ToDateTime(primerArray[0]);
                    fechaFin = Convert.ToDateTime(primerArray[0]);

                    rdc.RecordSelectionFormula = "{Ventas.Status} = 1 AND {Ventas.Fecha} in Date (" + fechaInicio.Year + ", " + fechaInicio.Month + ", " + fechaInicio.Day + ") to Date(" + fechaFin.Year + ", " + fechaFin.Month + ", " + fechaFin.Day + ")";
                    rdc.SetParameterValue("fecha", fechaInicio);                  
                    break;

                case 12://reporte de ventas x dia x producto
                    fechaInicio = Convert.ToDateTime(primerArray[0]);
                    fechaFin = Convert.ToDateTime(primerArray[0]);

                    rdc.RecordSelectionFormula = "{Ventas.Status} = 1 AND {Ventas.Fecha} in Date (" + fechaInicio.Year + ", " + fechaInicio.Month + ", " + fechaInicio.Day + ") to Date(" + fechaFin.Year + ", " + fechaFin.Month + ", " + fechaFin.Day + ")";
                    rdc.SetParameterValue("fecha", fechaInicio);
                    break;

                case 13://reporte de venta X
                    rdc.SetParameterValue("venta", primerArray[0]);
                    break;
                    


                case 91://CatProductos
                    break;

                case 92://proyeccion de ganancias
                    break;
                case 93://prod por laboratorio
                    break;
                case 94://prod sin movimientos
                    break;

                case 101: //COTIZACIONES TMP
                    rdc.SetParameterValue("proveedor", primerArray[0]);
                    break;

                case 102: //COTIZACIONES --> Precio
                    rdc.SetParameterValue("proveedor", primerArray[0]);
                    break;

                case 103: //COTIZACIONES 
                    rdc.SetParameterValue("cotizacion", primerArray[0]);
                    break;


                case 104: //Pedidos
                    rdc.SetParameterValue("cotizacion", primerArray[0]);
                    break;


                case 111: //Facturas de Pedidos
                    rdc.RecordSelectionFormula = "{FacturasAlmacen.Id} = " + primerArray[0] ;
                    break;

                case 112://concentrado de facturas
                    break;
                case 113://detallado de facturas
                    break;
                case 121: //Existencias
                    //rdc.RecordSelectionFormula = "{FacturasAlmacen.Id} = " + primerArray[0];
                    break;
                case 122: //Stocks Mínimos
                    //rdc.RecordSelectionFormula = "{FacturasAlmacen.Id} = " + primerArray[0];
                    break;


                case 123://formato para levantar inventario fisico
                    break;

                case 124://concentrado entradas salidas
                    break;
                case 125:                    
                    break;

                case 126:
                    break;

            }
        }




        private void CargarReporte(ReportDocument rdc)
        {
            CrystalReportViewer1.ReportSource = rdc;
            string user = System.Configuration.ConfigurationManager.AppSettings["user"];
            string pass = System.Configuration.ConfigurationManager.AppSettings["pass"];
            string server = @System.Configuration.ConfigurationManager.AppSettings["server"];
            string db = System.Configuration.ConfigurationManager.AppSettings["db"];

          


            TableLogOnInfo Logon = new TableLogOnInfo();

            foreach (CrystalDecisions.CrystalReports.Engine.Table t in rdc.Database.Tables)
            {
                Logon = t.LogOnInfo;
                Logon.ConnectionInfo.ServerName = server;
                Logon.ConnectionInfo.DatabaseName = db;
                Logon.ConnectionInfo.UserID = user;
                Logon.ConnectionInfo.Password = pass;
                t.ApplyLogOnInfo(Logon);
            }



            foreach (ReportDocument subreport in rdc.Subreports)
            {
                foreach (CrystalDecisions.CrystalReports.Engine.Table t in rdc.Database.Tables)
                {
                    Logon = t.LogOnInfo;
                    Logon.ConnectionInfo.ServerName = server;
                    Logon.ConnectionInfo.DatabaseName = db;
                    Logon.ConnectionInfo.UserID = user;
                    Logon.ConnectionInfo.Password = pass;
                    t.ApplyLogOnInfo(Logon);
                }
            }

            CrystalReportViewer1.DataBind();
        }


        private string GetNombreReporte(int caller)
        {
            string nombreReporte = string.Empty;

            switch (caller)
            {
                case 1:
                    //nombreReporte = "rptReceta.rpt";
                    nombreReporte = "receta.rpt";
                    break;

                case 2:
                    nombreReporte = "rptVenta.rpt";
                    break;

                case 3:
                    nombreReporte = "rptVentasTotales.rpt";
                    break;
					
				case 4:
                    nombreReporte = "rptSalida.rpt";
                    break;

                case 5:
                    nombreReporte = "rptSalidasTotales.rpt";
                    break;

                case 6:
                    nombreReporte = "rptArticulosVendidos.rpt";
                    break;


                case 11:
                    nombreReporte = "rptVentasDelDiaConcentrado.rpt";
                    break;


                case 12:
                    nombreReporte = "rptVentasDelDiaConcentradoXproducto.rpt";
                    break;

                case 13:
                    nombreReporte = "rptVentaX.rpt";
                    break;

                case 91:
                    nombreReporte = "Productos.rpt";
                    break;
                case 92:
                    nombreReporte = "ProductosProyeccionGanancias.rpt";
                    break;
                case 93:
                    nombreReporte = "ProductosXlaboratorio.rpt";
                    break;
                case 94:
                    nombreReporte = "ProductosSinMovimientos.rpt";
                    break;

                case 101:
                    nombreReporte = "cotizacionesTMP.rpt";
                    break;

                case 102:
                    nombreReporte = "cotizacionesConPrecio.rpt";
                    break;

                case 103:
                    nombreReporte = "Cotizaciones.rpt";
                    break;

                case 104:
                    nombreReporte = "Pedidos.rpt";
                    break;



               

                case 111:
                    nombreReporte = "FacturasCompras.rpt";
                    break;
                case 112:
                    nombreReporte = "FacturasConcentrado.rpt";
                    break;
                case 113:
                    nombreReporte = "FacturasDetallado.rpt";
                    break;


                case 121:
                    nombreReporte = "ProductosExistencias.rpt";
                    break;

                case 122:
                    nombreReporte = "ProductosStocksMinimos.rpt";
                    break;

                case 123:
                    nombreReporte = "InventarioFisico.rpt";
                    break;
                case 124:
                    nombreReporte = "ProductosConcentradoEntradasSalidas.rpt";
                    break;

                case 125:
                    nombreReporte = "ProductosKardex.rpt";
                    break;

                case 126:
                    nombreReporte = "InventarioFisicoBarCode.rpt";
                    break;
            }


            return nombreReporte;

        }




    }
}