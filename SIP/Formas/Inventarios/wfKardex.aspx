<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfKardex.aspx.cs" Inherits="SIP.Formas.Inventarios.wfKardex" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">



    function fnc_verReporte() {

        var izq = (screen.width - 1000) / 2
        var sup = (screen.height - 600) / 2
        var param = "123";
        var argumentos = "?c=" + 123 + "&p=" + param;

        url = $("#<%= _URLVisor.ClientID %>").val();

            url += argumentos;
            window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=1000,height=500,top=' + sup + ',left=' + izq);
        }




</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container">


   <div class="panel panel-success">
        <div class="panel-heading">

            <div class="row">
                <div class="col-md-4"><h3 class="panel-title">Kardex...</h3></div>
                <div class="col-md-4">.</div>
                <div class="col-md-4"> 
                    <asp:LinkButton ID="linkReporte" runat="server" PostBackUrl="#" OnClientClick="fnc_verReporte()">Ver reporte</asp:LinkButton>  
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
