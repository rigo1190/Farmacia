<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TEST.aspx.cs" Inherits="SIP.Formas.POA.TEST" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <script src="<%= ResolveClientUrl("~/Scripts/jquery-1.10.2.js") %>"></script>
    <script type="text/javascript">
        function HandleIT() {
            PageMethods.Test("Cadena de prueba", fnc_Success);

            <%--$.ajax({
                type: 'POST',
                url: "<%=ResolveClientUrl("~/Ajax.aspx") %>",
                data: {
                    'accion': 'abrirPeriodo',
                    'mes': '1',
                    'anio': '2014'
                },
                dataType: "json",
                success: function (data, textStatus, XMLHttpRequest) {

                    if (data.g == "1") {
                        alert(data.g);
                    }


                },
                error: function (error) {
                    alert("---[ Error ]---" + error + "\n" + errorThrown);
                }
            });--%>

        }

        function fnc_Success(result) {
            alert(result);
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

            <asp:TextBox ID="txtname" runat="server"></asp:TextBox>
            <br />
            <asp:TextBox ID="txtaddress" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="btnCreateAccount" runat="server" Text="Signup" OnClientClick="HandleIT(); return false;" />
        </div>
    </form>
</body>
</html>
