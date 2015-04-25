<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AbrirDocto.aspx.cs" Inherits="SIP.AbrirDocto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <title></title>
       <%: Scripts.Render("~/bundles/jquery") %>
       <%: Scripts.Render("~/bundles/bootstrap") %>
       <%: Styles.Render("~/Content/css") %> 
       <%: Styles.Render("~/Content/themes/base/css") %>  
    </head>
    <body>
    <form id="form1" runat="server">
        <div runat="server">
            <div class="alert alert-danger" runat="server" id="divMsgError" style="display:none">
                <asp:Label ID="lblMsgError" EnableViewState="false" runat="server" CssClass="font-weight:bold"></asp:Label>
            </div>
        </div>
    </form>
    </body>
</html>

