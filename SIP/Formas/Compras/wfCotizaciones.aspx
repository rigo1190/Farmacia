<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfCotizaciones.aspx.cs" Inherits="SIP.Formas.Compras.wfCotizaciones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
 
 

        function fnc_AbrirReporte(id) {

            var izq = (screen.width - 750) / 2
            var sup = (screen.height - 600) / 2
            var param = id;

            url = $("#<%= _URLVisor.ClientID %>").val();
        var argumentos = "?c=" + 103 + "&p=" + param;
        url += argumentos;
        window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=750,height=500,top=' + sup + ',left=' + izq);
    }
 


        function fnc_VerPedidos(id) {

            var izq = (screen.width - 1000) / 2
            var sup = (screen.height - 600) / 2
            var param = id;
                var argumentos = "?c=" + 104 + "&p=" + param;

                url = $("#<%= _URLVisor.ClientID %>").val();

                url += argumentos;
                window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=1000,height=500,top=' + sup + ',left=' + izq);
            }



    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


 <div class="container">
     <div id="divBtnNuevo" runat="server">
        <asp:Button ID="btnNuevo" runat="server" Text="Nueva Cotización..." CssClass="btn btn-primary" OnClick="btnNuevo_Click" AutoPostBack="false" />
    </div>


    <div style="display:none" runat="server">
    <input type="hidden" runat="server" id="_URLVisor" />
    </div>
 

     <asp:GridView Height="25px" ShowHeaderWhenEmpty="true" CssClass="table" ID="grid" DataKeyNames="Id" AutoGenerateColumns="False" runat="server" OnRowDataBound="grid_RowDataBound">
                <Columns>



                    <asp:TemplateField HeaderText="Folio" ItemStyle-CssClass="col-md-3">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Folio") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="Fecha" ItemStyle-CssClass="col-md-3">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Fecha","{0:d}") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Costo Aproximado" ItemStyle-CssClass="col-md-3">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("CostoAproximado","{0:C2}") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Pedidos" ItemStyle-CssClass="col-md-3">
                        <ItemTemplate>                           
                            <div id = "DIVpedidos" runat="server">
                                <asp:LinkButton ID="linkPedidos" runat="server" PostBackUrl="#"  OnClick ="linkPedidos_Click">Pendiente</asp:LinkButton>  
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>


                        <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                            
                            <asp:ImageButton ID="imgRPT" ToolTip="Ver Cotizaciones" runat="server" ImageUrl="~/img/Sub.png" OnClick="imgRPT_Click"/>
                            
                            <asp:ImageButton ID="imgRptPedidos" ToolTip="Ver Pedidos" runat="server" ImageUrl="~/img/Sub.png" OnClick="imgRptPedidos_Click"/>

                        </ItemTemplate>
                        <HeaderStyle BackColor="#EEEEEE" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" BackColor="#EEEEEE" />
                    </asp:TemplateField>                                  


                </Columns>
                    
                
                    
        </asp:GridView>
        


</div>    
</asp:Content>
