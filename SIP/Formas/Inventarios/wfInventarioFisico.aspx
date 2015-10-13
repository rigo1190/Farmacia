<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfInventarioFisico.aspx.cs" Inherits="SIP.Formas.Inventarios.wfInventarioFisico" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
    function KeyDownHandler(btn) {
        if (event.keyCode == 13) {
            event.returnValue = false;
            event.cancel = true;

            btn.click();
        }
    }


    function fnc_verReporte() {

        var izq = (screen.width - 1000) / 2
        var sup = (screen.height - 600) / 2
        var param = "126";
        var argumentos = "?c=" + 126 + "&p=" + param;

        url = $("#<%= _URLVisor.ClientID %>").val();

          url += argumentos;
          window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=1000,height=500,top=' + sup + ',left=' + izq);
      }



</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container" id="divMain" runat="server">


                     <div class="row">
                     <div class="col-md-8">
                        

                         <div class="form-group">
                            <label>Código</label>
                            <asp:TextBox ID="txtCode" runat="server" Width="200px" onkeydown="KeyDownHandler(BtnBuscar)" ></asp:TextBox>
                        </div>
                         
                    
                         <div id="divBotones" style="display:none">
                             <asp:Button ID="BtnBuscar" runat="server" CssClass="Arial11_55585B" Text="Buscar" OnClick="BtnBuscar_Click"></asp:Button>                         
                         </div>
                         
                         
                                     
                                
                     </div>

                     

                 </div>

        <div id="divDatos" runat="server" class="panel panel-success">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-8"><h3 class="panel-title">Inventario Físico</h3></div>
                <div class="col-md-2"> . </div>
                <div class="col-md-2"> <asp:LinkButton ID="linkReporte" runat="server" PostBackUrl="#" OnClientClick="fnc_verReporte()">Ver reporte</asp:LinkButton>   </div>
            </div>
        </div>
        
                <div class="row">
                    <asp:GridView  ID="gridInventarioFisico" OnRowDeleting="gridInventarioFisico_RowDeleting"  EnablePersistedSelection="true" ShowHeaderWhenEmpty="true"  DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                    <Columns>
                        
                        <asp:CommandField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ControlStyle-Font-Size="Smaller" ShowDeleteButton="true" HeaderText="Eliminar" />

                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" ItemStyle-Width="100px" HeaderText="Código" HeaderStyle-CssClass="panel-footer" SortExpression="Orden">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "Clave") %>
                                <input type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" ItemStyle-Width="600px" HeaderText="Nombre Producto" HeaderStyle-CssClass="panel-footer" SortExpression="Orden">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "Articulo.NombreCompleto") %>
                                <input type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" id="idProducto" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-Width="100px" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Existencia en Sistema" HeaderStyle-CssClass="panel-footer" SortExpression="NOAplica">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "ExistenciaEnSistema") %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-Width="100px" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Inventario Físico" HeaderStyle-CssClass="panel-footer" SortExpression="NOAplica">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "Cantidad") %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                                           
           
                    </Columns>
                </asp:GridView>
                </div>
        
        </div>




    <div style="display:none" runat="server">           
        <input type="hidden" runat="server" id="_URLVisor" />          
    </div>







</div>
</asp:Content>
