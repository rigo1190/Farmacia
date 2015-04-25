<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfArticulos.aspx.cs" Inherits="SIP.Formas.Catalogos.wfArticulos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container">

   <div class="panel panel-success">
        <div class="panel-heading">

            <div class="row">
                <div class="col-md-4"><h3 class="panel-title">Productos</h3></div>
                <div class="col-md-4">.</div>
                <div class="col-md-4">.</div>                
             </div>
        </div>
    </div>
       

               
        <div id="divArticulos" class="row" runat ="server">
            




            <div class="bs-example">
            <div class="panel-group" id="accordion" runat="server">

                

            </div>
            </div>


        </div>


       

    </div>
</asp:Content>
