﻿using BusinessLogicLayer;
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

            switch (caller)
            {
                case 1: //RECETA
                    rdc.SetParameterValue("@RecetaID", primerArray[0]);
                    break;

                case 2: //VENTA
                    rdc.SetParameterValue("@VentaID", primerArray[0]);
                    break;

                case 3: //VENTAS GENERALES
                    rdc.RecordSelectionFormula = "{pa_VentasGenerales;1.VentaId} in [" + primerArray[0] + "]";
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
                    nombreReporte = "rptReceta.rpt";
                    break;

                case 2:
                    nombreReporte = "rptVenta.rpt";
                    break;

                case 3:
                    nombreReporte = "rptVentasTotales.rpt";
                    break;
            }


            return nombreReporte;

        }




    }
}