﻿<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfExistencias.aspx.cs" Inherits="SIP.Formas.Inventarios.wfExistencias" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

       

        function fnc_verReporte() {

            var izq = (screen.width - 1000) / 2
            var sup = (screen.height - 600) / 2
            var param = "121";
            var argumentos = "?c=" + 121 + "&p=" + param;

                url = $("#<%= _URLVisor.ClientID %>").val();

                url += argumentos;
                window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=1000,height=500,top=' + sup + ',left=' + izq);
            }

        function fnc_verReporteDos() {

            var izq = (screen.width - 1000) / 2
            var sup = (screen.height - 600) / 2
            var param = "123";
            var argumentos = "?c=" + 123 + "&p=" + param;

            url = $("#<%= _URLVisor.ClientID %>").val();

            url += argumentos;
            window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=1000,height=500,top=' + sup + ',left=' + izq);
        }

        function fnc_verReporteTres() {

            var izq = (screen.width - 1000) / 2
            var sup = (screen.height - 600) / 2
            var param = "124";
            var argumentos = "?c=" + 124 + "&p=" + param;

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
                <div class="col-md-4"><h3 class="panel-title">Existencias...</h3></div>
                <div class="col-md-2">.</div>
                <div class="col-md-2"> 
                    <asp:LinkButton ID="linkReporte" runat="server" PostBackUrl="#" OnClientClick="fnc_verReporte()">Ver reporte</asp:LinkButton>  
                </div>                
                <div class="col-md-2"> 
                    <asp:LinkButton ID="linkInventarioFisico" runat="server" PostBackUrl="#" OnClientClick="fnc_verReporteDos()">Inventario Físico</asp:LinkButton>  
                </div>                
                <div class="col-md-2"> 
                    <asp:LinkButton ID="linkConcentradoInputOutput" runat="server" PostBackUrl="#" OnClick="linkConcentradoInputOutput_Click" OnClientClick="fnc_verReporteTres()">Concentrado E/S</asp:LinkButton>  
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
