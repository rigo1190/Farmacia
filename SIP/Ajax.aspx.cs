

using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIP
{
    public partial class Ajax : System.Web.UI.Page
    {
        private String accion;
        private char llaveAbre = '{';

        private char llaveCierra = '}';
        protected void Page_Load(object sender, EventArgs e)
        {
            UnitOfWork uow;

            accion = Request.Params.Get("accion");

            switch (accion)
            {
                case "guardarPlantillas":
                    string cad = Request.Params.Get("cad");
                    GuardarPlantillaPOA(cad);
                    break;
                case "abrirPeriodo":
                    Int16 mes = Int16.Parse(Request.Params.Get("mes"));
                    Int16 anio = Int16.Parse(Request.Params.Get("anio"));
                    this.AjaxAbrirPeriodo(mes, anio);
                    break;

            }




        }

        private void AjaxAbrirPeriodo(Int16 mes, Int16 anio)
        {
            Response.Clear();
            Response.ContentType = "application/json";

            bool guardado;


            String jsonTodods = "";
            String jsonObj;

            guardado = true;


            if (guardado)
            {
                jsonObj = "{";
                jsonObj += "\"g\": \"" + "1" + "\"";
                jsonObj += "}";
                jsonTodods += jsonObj;
            }
            else
            {
                jsonObj = "{";
                jsonObj += "\"g\": \"" + "0" + "\"";
                jsonObj += "}";
                jsonTodods += jsonObj;
            }

            jsonTodods = jsonTodods.TrimEnd(new char[] { ',' });
            jsonTodods += "";

            Response.Write(jsonTodods);
            Response.Flush();
            Response.End();
        }


        private void GuardarPlantillaPOA(string cad)
        {
            Response.Clear();

            string json = cad;

            Response.Write(json);
            Response.Flush();
            Response.End();

        }
    }
}