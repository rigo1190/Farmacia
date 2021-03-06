﻿<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfArticulos.aspx.cs" Inherits="SIP.Formas.Catalogos.wfArticulos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script type="text/javascript">



      function fnc_verReporte() {

          var izq = (screen.width - 1000) / 2
          var sup = (screen.height - 600) / 2
          var param = "91";
          var argumentos = "?c=" + 91 + "&p=" + param;

          url = $("#<%= _URLVisor.ClientID %>").val();

            url += argumentos;
            window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=1000,height=500,top=' + sup + ',left=' + izq);
        }

      function fnc_verReporteDos() {

          var izq = (screen.width - 1000) / 2
          var sup = (screen.height - 600) / 2
          var param = "92";
          var argumentos = "?c=" + 92 + "&p=" + param;

          url = $("#<%= _URLVisor.ClientID %>").val();

          url += argumentos;
          window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=1000,height=500,top=' + sup + ',left=' + izq);
      }

      function fnc_verReporteTres() {

          var izq = (screen.width - 1000) / 2
          var sup = (screen.height - 600) / 2
          var param = "93";
          var argumentos = "?c=" + 93 + "&p=" + param;

          url = $("#<%= _URLVisor.ClientID %>").val();

          url += argumentos;
          window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=1000,height=500,top=' + sup + ',left=' + izq);
      }

      function fnc_verReporteCuatro() {

          var izq = (screen.width - 1000) / 2
          var sup = (screen.height - 600) / 2
          var param = "94";
          var argumentos = "?c=" + 94 + "&p=" + param;

          url = $("#<%= _URLVisor.ClientID %>").val();

          url += argumentos;
          window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=1000,height=500,top=' + sup + ',left=' + izq);
      }

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container" id="divMain" runat="server">

   <div class="panel panel-success">
        <div class="panel-heading">

            <div class="row">
                <div class="col-md-4"><h3 class="panel-title">Productos</h3></div>
                
                <div class="col-md-2"> 
                    <asp:LinkButton ID="linkReporte" runat="server" PostBackUrl="#" OnClientClick="fnc_verReporte()">Ver reporte</asp:LinkButton>  
                </div>                            
                <div class="col-md-2"> 
                    <asp:LinkButton ID="linkProyeccion" runat="server" PostBackUrl="#" OnClientClick="fnc_verReporteDos()">Proyección Ganancias</asp:LinkButton>  
                </div>                            
                <div class="col-md-2"> 
                    <asp:LinkButton ID="linkXlaboratorio" runat="server" PostBackUrl="#" OnClientClick="fnc_verReporteTres()">Reporte X Laboratorio</asp:LinkButton>  
                </div>                            
                <div class="col-md-2"> 
                    <asp:LinkButton ID="LinkSinMov" runat="server" PostBackUrl="#" OnClientClick="fnc_verReporteCuatro()">Productos sin movimiento</asp:LinkButton>  
                </div>                            
             </div>
        </div>
    </div>
       
     <div style="display:none" runat="server">
            <input type="hidden" runat="server" id="_URLVisor" />                                                                     
        </div>
               
        <div id="divArticulos" class="row" runat ="server">
            




            <div class="bs-example">
            <div class="panel-group" id="accordion" runat="server">

                

            </div>
            </div>


        </div>


       

    </div>
</asp:Content>
