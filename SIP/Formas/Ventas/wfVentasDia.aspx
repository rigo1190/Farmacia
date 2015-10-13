<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfVentasDia.aspx.cs" Inherits="SIP.Formas.Ventas.wfVentasDia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    function fnc_MostrarVentas() {

        var izq = (screen.width - 750) / 2
        var sup = (screen.height - 600) / 2

       

                
          var fechaInicio = $("#<%= txtFechaFiltro.ClientID %>").val();
          var fechaFin = $("#<%= txtFechaFiltro.ClientID %>").val();

          var url = "<%= ResolveClientUrl("~/rpts/wfVerReporte.aspx") %>";
          var argumentos = "?c=" + 11 + "&p=" + fechaInicio;
          url += argumentos;
          window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=750,height=500,top=' + sup + ',left=' + izq);

      }

    function fnc_MostrarVentasXproductos() {

        var izq = (screen.width - 750) / 2
        var sup = (screen.height - 600) / 2




        var fechaInicio = $("#<%= txtFechaFiltro.ClientID %>").val();
        var fechaFin = $("#<%= txtFechaFiltro.ClientID %>").val();

        var url = "<%= ResolveClientUrl("~/rpts/wfVerReporte.aspx") %>";
        var argumentos = "?c=" + 12 + "&p=" + fechaInicio;
        url += argumentos;
        window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=750,height=500,top=' + sup + ',left=' + izq);

    }


    function fnc_verVentaX(idVenta) {

        var izq = (screen.width - 750) / 2
        var sup = (screen.height - 600) / 2




        var fechaInicio = $("#<%= txtFechaFiltro.ClientID %>").val();
        var fechaFin = $("#<%= txtFechaFiltro.ClientID %>").val();

            var url = "<%= ResolveClientUrl("~/rpts/wfVerReporte.aspx") %>";
            var argumentos = "?c=" + 13 + "&p=" + idVenta;
            url += argumentos;
            window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=750,height=500,top=' + sup + ',left=' + izq);

        }



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        

        <div class="panel panel-success">
            <div class="panel-heading">

                <div class="row">
                <div class="col-md-4"><h3 class="panel-title">Ventas</h3></div>
                <div class="col-md-2">.</div>
                
                <div class="col-md-2"> 
                    <asp:LinkButton ID="linkVentasDelDia" runat="server" PostBackUrl="#" OnClientClick="fnc_MostrarVentas()">Reporte de Ventas</asp:LinkButton>  
                </div>                
                <div class="col-md-2"> 
                    <asp:LinkButton ID="linkVentasXProducto" runat="server" PostBackUrl="#" OnClientClick="fnc_MostrarVentasXproductos()">Reporte de Ventas X productos</asp:LinkButton>  
                </div>                
             </div>

            </div>

            <div class="panel-body">
                <div class="row">
                    <div class="panel panel-default">
                    
                    <div class="panel-body">

                        



                        <div class="row">

                            <div class="col-md-6">

                            </div>

                            <div class="col-md-6">
                               <%-- &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <asp:Button runat="server" ID="btnVR" OnClick="btnVR_Click" Text="Venta X Receta" CssClass="btn btn-default" />
                                <asp:Button runat="server" ID="btnV" OnClick="btnV_Click" Text="Venta de Productos" CssClass="btn btn-default" />
                                <asp:Button runat="server" ID="btnVentaCodeBar" OnClick="btnVentaCodeBar_Click" Text="Venta con lectora" CssClass="btn btn-default" />
                            </div>

                        </div>

                        <div style="height:330px; overflow:scroll">
                             <asp:GridView Width="1250px" OnRowDataBound="gridVentas_RowDataBound" PageSize="10" Height="25px" EnablePersistedSelection="true" ShowHeaderWhenEmpty="true" ID="gridVentas" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                                <Columns>
                                    <%--<asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer"  HeaderText="Acciones">
                                        <ItemTemplate>
                                            <%--<asp:ImageButton OnClick="imgBtnEdit_Click" ID="imgBtnEdit" ToolTip="Editar" runat="server" ImageUrl="~/img/Edit1.png" />
                                            <asp:ImageButton OnClick="imgBtnEliminarReceta_Click"  ID="imgBtnEliminar" ToolTip="Eliminar" runat="server" ImageUrl="~/img/close.png" OnClientClick="return fnc_Mensaje();"/>
                                        </ItemTemplate>
                                        <HeaderStyle BackColor="#EEEEEE" />
                                        <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="50px" BackColor="#EEEEEE" />
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Small" HeaderStyle-Width="150px" HeaderText="Número de Folio" SortExpression="Orden">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "FolioCadena") %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderText="Fecha" SortExpression="Orden">
                                        <ItemTemplate>
                                            <%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "Fecha")).ToString("d")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller"  HeaderText="Tipo Venta" SortExpression="NOAplica">
                                        <ItemTemplate>
                                            <%--<%# DataBinder.Eval(Container.DataItem, "Cliente.RazonSocial") %>--%>
                                            <asp:Label runat="server" ID="lblTipoVenta"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderText="Vendió" SortExpression="NoAplica">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Usuario.Nombre") %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Importe Total" SortExpression="NOAplica">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%#Bind("Importe","{0:C2}") %>' ID="lblPV"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                   <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" HeaderStyle-HorizontalAlign="Justify" HeaderText="Imprimir" SortExpression="NOAplica">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgRPT" ToolTip="Ver Cotizaciones" runat="server" ImageUrl="~/img/Sub.png" OnClick="imgRPT_Click"/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                </Columns>
                                <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                             </asp:GridView>
                        </div>
                       

                        <div class="row" runat="server" id="divConsultarOtrasFechas">  
                            <div class="form-group">  
                                <table>
                                    <tr>
                                        <td>
                                            <label>Fecha :</label>
                                        </td>
                                        <td>
                                            <div class="input-group">
                                                <input  class="form-control datepicker" runat="server" id="txtFechaFiltro"/>
                                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                            </div>
                                        </td>

                                        <td>

                                        </td>
                                        <td>

                                        </td>
                                        <td>

                                        </td>
                                        <td>

                                        </td>
                                        <td>
                                            

                                        </td>


                                        <td>

                                        </td>
                                        <td>

                                        </td>
                                        <td>

                                        </td>
                                        <td>

                                        </td>

                                        <td>
                                            
                                        </td>
                                        <td>

                                        </td>
                                        <td>

                                        </td>
                                        <td>

                                        </td>
                                        <td>

                                        </td>

                                        <td>
                                            <div>
                                                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click" CssClass="btn btn-primary" />
                                            </div>
                                        </td>
                                    </tr>
                                        
                                </table>
                            </div>
                        </div>


                    </div>
                </div>
                </div>
            </div>
        </div>

    </div>

</asp:Content>
