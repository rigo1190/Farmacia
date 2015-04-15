<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfConsultarVentas.aspx.cs" Inherits="SIP.Formas.Ventas.wfConsultarVentas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">

        $(document).ready(function () {

            $('.datepicker').datepicker(
            {
                format: "dd/mm/yyyy"
            });

        });


        <%--function fnc_ValidarFiltros() {
            var error = false;

            if ($("#<%= chkRango.ClientID %>").is(':checked')) {
                if ($("#<%= txtFechaI.ClientID %>").val() == "" || $("#<%= txtFechaT.ClientID %>").val() == "")
                    error = true;
            }
        }--%>


        function fnc_MostrarVentas() {

            var izq = (screen.width - 750) / 2
            var sup = (screen.height - 600) / 2

            if ($("#<%= _IDsVenta.ClientID %>").val() == "")
                return false;

            var ids = $("#<%= _IDsVenta.ClientID %>").val(); 

            var url = "<%= ResolveClientUrl("~/rpts/wfVerReporte.aspx") %>";
            var argumentos = "?c=" + 3 + "&p=" + ids;
            url += argumentos;
            window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=750,height=500,top=' + sup + ',left=' + izq);

        }


        function fncMostrarDivsFiltros(opcion) {

            switch (opcion) {
                case 1: 
                    if ($("#<%= chkRango.ClientID %>").is(':checked')) {

                        $("#<%= divFechaI.ClientID %>").css("display", "block");
                        $("#<%= divFechaT.ClientID %>").css("display", "block");

                    } else {

                        $("#<%= divFechaI.ClientID %>").css("display", "none");
                        $("#<%= divFechaT.ClientID %>").css("display", "none");
                    }
                    break;
                case 2:
                    if ($("#<%= chkCliente.ClientID %>").is(':checked')) {

                        $("#<%= divCliente.ClientID %>").css("display", "block");

                    } else {

                        $("#<%= divCliente.ClientID %>").css("display", "none");
                    }
                    
                    break;
                case 3:
                    if ($("#<%= chkProducto.ClientID %>").is(':checked')) {

                        $("#<%= divArticulos.ClientID %>").css("display", "block");

                    } else {

                        $("#<%= divArticulos.ClientID %>").css("display", "none");
                    }
                    break;
                case 4:
                    if ($("#<%= chkUsuario.ClientID %>").is(':checked')) {

                        $("#<%= divUsuario.ClientID %>").css("display", "block");
                    }
                    else {
                        $("#<%= divUsuario.ClientID %>").css("display", "none");
                    }
                    
                    break;
            }


        }


        function fnc_MostrarVenta(idVenta) {

            var izq = (screen.width - 750) / 2
            var sup = (screen.height - 600) / 2

            var url = "<%= ResolveClientUrl("~/rpts/wfVerReporte.aspx") %>";
            var argumentos = "?c=" + 2 + "&p=" + idVenta;
            url += argumentos;
            window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=750,height=500,top=' + sup + ',left=' + izq);

        }
    </script>
    
    


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="container">
        <div class="page-header"">
             <h3>Consultar Ventas</h3>
        </div>

        <div class="panel panel-success">
            <div class="panel-heading">
                <h3 class="panel-title">
                   Filtros para consultar Ventas
                </h3>
            </div>

            <div class="panel-body">

            <div class="row">
                <div class="panel panel-default">
                    <div class="panel-heading">
                       Filtrar por:
                    </div>
                    <div class="panel-body">
                        <div class="col-md-3">
                            <div class="form-group">
                                 <label>Rango de Fechas</label>
                                 <input onclick="fncMostrarDivsFiltros(1)" type="checkbox" value="false" runat="server" id="chkRango" />
                            </div>
                        </div>
                       
                        <div class="col-md-3">
                            <div class="form-group">
                                 <label>Cliente</label>
                                 <input onclick="fncMostrarDivsFiltros(2)" type="checkbox" value="false" runat="server" id="chkCliente" />
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="form-group">
                                 <label>Producto</label>
                                 <input onclick="fncMostrarDivsFiltros(3)" type="checkbox" value="false" runat="server" id="chkProducto" />
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="form-group">
                                 <label>Usuario Vendió</label>
                                 <input onclick="fncMostrarDivsFiltros(4)" type="checkbox" value="false" runat="server" id="chkUsuario" />
                            </div>
                        </div>

                    </div>
                   
                </div>
               
            </div>

            <div  class="row">
                <div id="divFolio"  runat="server" class="col-md-3">
                    <div  class="form-group">
                        <label>Número de Folio:</label>
                        <input type="text" name="prueba" runat="server" class="form-control" id="txtFolio" />
                    </div>                 
                </div>
               
                <div style="display:none"  id="divFechaI" runat="server" class="col-md-3">
                    <div class="form-group">
                        <label>Fecha Inicio:</label>
                        <div class="input-group">
                            <input class="form-control datepicker" runat="server" id="txtFechaI"/>
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                        </div>
                    </div>
                </div>

                <div style="display:none"  id="divFechaT" runat="server" class="col-md-3">
                    <div class="form-group">
                        <label>Fecha Término:</label>
                        <div class="input-group">
                            <input class="form-control datepicker" runat="server" id="txtFechaT"/>
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                        </div>
                    </div>
                </div>

                <div style="display:none"  id="divCliente" runat="server" class="col-md-3">
                    <div class="form-group">
                        <label>Cliente:</label>
                        <asp:DropDownList runat="server" ID="ddlClientes" CssClass="form-control"></asp:DropDownList>
                    </div>
                                    
                </div>

            </div>

            <div class="row">
                <div style="display:none"  id="divArticulos" runat="server" class="col-md-3">
                    <div class="form-group">
                        <label>Producto:</label>
                        <asp:DropDownList runat="server" ID="ddlArticulos" CssClass="form-control"></asp:DropDownList>
                    </div>                 
                </div>


                <div style="display:none"  id="divUsuario" runat="server" class="col-md-3">
                    <div class="form-group">
                        <label>Usuarios:</label>
                        <asp:DropDownList runat="server" ID="ddlUsuarios" CssClass="form-control"></asp:DropDownList>
                    </div>                 
                </div>

               

            </div>

            <div class="row">
                 <div id="divBtnConsultar" runat="server" class="col-md-4">
                    <div class="form-group">
                        <asp:Button runat="server" OnClick="btnConsultar_Click" OnClientClick="return fnc_ValidarFiltros();" ID="btnConsultar" Text="Consultar" CssClass="btn btn-primary" />
                    </div>                 
                </div>
            </div>

            <div class="row">
                 <div class="panel panel-default">
                    <div class="panel-heading">
                        

                        <div class="row">
                            <div class="col-md-11">
                                <h3 class="panel-title"><i class="fa"></i>Listado de Ventas</h3>
                            </div>
                            <div class="col-md-1">
                                <button type="button" id="btnImprimir0" onclick="fnc_MostrarVentas();"><span class="glyphicon glyphicon-print"></span></button>
                            </div>
                        </div>


                    </div>
                    <div class="panel-body">
                        <div style="height:330px; overflow:scroll">
                             <asp:GridView Width="1250px" OnPageIndexChanging="gridVentas_PageIndexChanging" OnRowDataBound="gridVentas_RowDataBound" PageSize="10" Height="25px" EnablePersistedSelection="true" ShowHeaderWhenEmpty="true" ID="gridVentas" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer"  HeaderText="Acciones">
                                        <ItemTemplate>
                                            <%--<asp:ImageButton OnClick="imgBtnEdit_Click" ID="imgBtnEdit" ToolTip="Editar" runat="server" ImageUrl="~/img/Edit1.png" />
                                            <asp:ImageButton OnClick="imgBtnEliminarReceta_Click"  ID="imgBtnEliminar" ToolTip="Eliminar" runat="server" ImageUrl="~/img/close.png" OnClientClick="return fnc_Mensaje();"/>--%>
                                        </ItemTemplate>
                                        <HeaderStyle BackColor="#EEEEEE" />
                                        <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="50px" BackColor="#EEEEEE" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Small" HeaderStyle-Width="150px" HeaderText="Número de Folio" SortExpression="Orden">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Folio") %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderText="Fecha" SortExpression="Orden">
                                        <ItemTemplate>
                                            <%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "Fecha")).ToString("d")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller"  HeaderText="Cliente" SortExpression="NOAplica">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Cliente.RazonSocial") %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Importe Total" SortExpression="NOAplica">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Importe") %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" HeaderStyle-HorizontalAlign="Justify" HeaderText="Imprimir" SortExpression="NOAplica">
                                    <ItemTemplate>
                                        <button type="button" runat="server" id="btnVer"><span class="glyphicon glyphicon-print"></span></button>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                </Columns>
                                <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                             </asp:GridView>
                        </div>
                       
                    </div>
                </div>
                
                    
            
            </div>

        </div>
    
    
        </div>
    </div>
    
    <div runat="server" style="display:none">
         <input type="hidden" runat="server" id="_IDsVenta" />
    </div>

</asp:Content>
