<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfFacturas.aspx.cs" Inherits="SIP.Formas.Compras.wfFacturas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">

 
 



    function fnc_AbrirReporte(id) {

        var izq = (screen.width - 750) / 2
        var sup = (screen.height - 600) / 2
        var param = id;

        url = $("#<%= _URLVisor.ClientID %>").val();
        var argumentos = "?c=" + 111 + "&p=" + param;
        url += argumentos;
        window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=750,height=500,top=' + sup + ',left=' + izq);
    }

   



    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div class="container">

    
     <div id="divDatos" runat="server" class="panel panel-success">
        <div class="panel-heading">
            <h3 class="panel-title">Facturas</h3>
        </div>

        <div id="divBtnNuevo" runat="server">
            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-primary" OnClick="btnNuevo_Click" AutoPostBack="false" />
        </div>

        <asp:GridView Height="25px" ShowHeaderWhenEmpty="true" CssClass="table" ID="grid" DataKeyNames="Id" AutoGenerateColumns="False" runat="server" OnRowDataBound="grid_RowDataBound">
                <Columns>

                    <asp:TemplateField HeaderText="Pedido" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("Pedido.Folio") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Proveedor" ItemStyle-CssClass="col-md-4">
                        <ItemTemplate>
                            <asp:Label ID="Label2Prov" runat="server" Text='<%# Bind("Proveedor.RazonSocial") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Factura" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("FolioFactura") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    

                    <asp:TemplateField HeaderText="Fecha" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("FechaFactura","{0:d}") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    

                    <asp:TemplateField HeaderText="Importe" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("ImporteFactura","{0:C2}")  %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Comparar Precios" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>                           
                            <div id = "DIVPrecios" runat="server">
                                <asp:LinkButton ID="linkPrecios" runat="server" PostBackUrl="#"  OnClick ="linkPrecios_Click">Comparar</asp:LinkButton>  
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    


                        <asp:TemplateField HeaderText="Ver Reporte" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                            
                            <asp:ImageButton ID="imgVer" ToolTip="Ver" runat="server" ImageUrl="~/img/Sub.png" OnClick="imgVer_Click"/>
                            
                            
                        </ItemTemplate>
                        <HeaderStyle BackColor="#EEEEEE" />
                        <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" BackColor="#EEEEEE" />
                    </asp:TemplateField>                                  


                </Columns>
                    
                
                    
        </asp:GridView>

        <input type="hidden" runat="server" id="_URLVisor" />

    </div>



    
     


</div>    
</asp:Content>
