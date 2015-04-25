<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfCotizacionesAdd.aspx.cs" Inherits="SIP.Formas.Compras.wfCotizacionesAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    $(document).ready(function () {
        $('.campoNumerico').autoNumeric('init');
    });


    function fnc_Confirmar() {
        return confirm("¿Está seguro de eliminar el registro?");
    }




    function fnc_AbrirReporte(id) {

        var izq = (screen.width - 750) / 2
        var sup = (screen.height - 600) / 2
        var param = id;

        url = $("#<%= _URLVisor.ClientID %>").val();
        var argumentos = "?c=" + 101 + "&p=" + param;
        url += argumentos;
        window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=750,height=500,top=' + sup + ',left=' + izq);
    }

    function fnc_AbrirReporteCostos(id) {

        var izq = (screen.width - 750) / 2
        var sup = (screen.height - 600) / 2
        var param = id;

        url = $("#<%= _URLVisor.ClientID %>").val();
        var argumentos = "?c=" + 102 + "&p=" + param;
        url += argumentos;
        window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=750,height=500,top=' + sup + ',left=' + izq);
    }






    function fnc_ObtenerValoresSeleccionados() {
        var cadena = "";
        var index = 0;
        var grid = document.getElementById('<%=gridProductosCatalogo.ClientID %>'); //Se recupera el grid
            var primera = true;

            for (i = 1; i < grid.rows.length; i++) { //Se recorren las filas
                var idProducto = "";
                var respuesta = 0;

                idProducto = $("input#ContentPlaceHolder1_gridProductosCatalogo" + "_idProducto_" + index).val();

                if (idProducto != null && idProducto != "" && idProducto != undefined) {

                    if ($("input#ContentPlaceHolder1_gridProductosCatalogo" + "_chkSeleccionar_" + index).is(':checked')) {
                        if (primera) {
                            cadena += idProducto;
                            primera = false;
                        } else
                            cadena += "|" + idProducto;
                    }
                }

                index++;
            }

         //Se desmarcan las casillas
            index = 0;
            for (i = 1; i < grid.rows.length; i++) { //Se recorren las filas

                $("input#ContentPlaceHolder1_gridProductosCatalogo" + "_chkSeleccionar_" + index).prop('checked', false);

                index++;
            }



            $("#<%= _CadValoresSeleccionados.ClientID %>").val(cadena);

        }










    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div class="container">

    <div class="panel-footer alert alert-success" id="divMsgSuccess" style="display:none" runat="server">
                <asp:Label ID="lblMensajeSuccess" runat="server" Text=""></asp:Label>
    </div>
    <div class="panel-footer alert alert-danger" id="divMsg" style="display:none" runat="server">
                <asp:Label ID="lblMensajes" runat="server" Text=""></asp:Label>
    </div>


     <div id="divEncabezado" runat="server" class="panel panel-success">
      <div class="panel-heading">
             <div class="row">
                <div class="col-md-8"><h3 class="panel-title"> Creando nueva cotización   </h3></div>
                <div class="col-md-2"> . </div>
                <div class="col-md-2"><a href="<%=ResolveClientUrl("wfCotizaciones.aspx") %>">Regresar</a></div>
             </div>
       </div>
     </div>


     <div id="divBtnNuevo" runat="server">
        <asp:Button ID="btnNuevo" runat="server" Text="Guardar Cotización" CssClass="btn btn-primary" OnClick="btnNuevo_Click" AutoPostBack="false" />
    </div>



    <div id="div1" runat="server" class="panel panel-success">
        <div class="panel-heading">
            <h3 class="panel-title">Proveedores</h3>
        </div>

           <asp:GridView Height="25px" ShowHeaderWhenEmpty="true" CssClass="table" ID="gridProveedores" DataKeyNames="Id" AutoGenerateColumns="False" runat="server" OnRowDataBound="gridProveedores_RowDataBound">
                <Columns>


                    
                    <asp:TemplateField HeaderText="RFC" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="lblRFC" runat="server" Text='<%# Bind("Proveedor.RFC") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="Razón Social" ItemStyle-CssClass="col-md-6">
                        <ItemTemplate>
                            <asp:Label ID="lblRazonSocial" runat="server" Text='<%# Bind("Proveedor.RazonSocial") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Teléfonos" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                            <asp:Label ID="lblTel" runat="server" Text='<%# Bind("Proveedor.Telefonos") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Celular" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                            <asp:Label ID="lblCel" runat="server" Text='<%# Bind("Proveedor.Celular") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="E-Mail" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                            <asp:Label ID="lblMail" runat="server" Text='<%# Bind("Proveedor.EMail") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                        <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                            
                            <asp:ImageButton ID="imgImprimir" ToolTip="Ver" runat="server" ImageUrl="~/img/Sub.png" OnClick="imgImprimir_Click"/>
                         <%--   <asp:ImageButton ID="imgImprimirCostos" ToolTip="Ver" runat="server" ImageUrl="~/img/Sub.png" OnClick="imgImprimirCostos_Click"/>--%>
                            <asp:ImageButton ID="imgBtnEliminarProveedor" ToolTip="Borrar" runat="server" ImageUrl="~/img/close.png"  OnClick="imgBtnEliminarProveedor_Click"/>
                            
                        </ItemTemplate>
                        <HeaderStyle BackColor="#EEEEEE" />
                        <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" BackColor="#EEEEEE" />
                    </asp:TemplateField>                                  


                </Columns>
                    
                
                    
        </asp:GridView>



         <div class="row">

            <div class="col-md-8">
                <div class="form-group">
                    <label for="Proveedor">Proveedor</label>
                    <div>
                        <asp:DropDownList ID="ddlProveedores" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                </div>
            </div>
         
            <div class="col-md-1">
                <div class="form-group">    
                    <label for="asd">.</label><br />                
                        <asp:Button  CssClass="btn btn-default" Text="Agregar" ID="btnAddProveedor" runat="server" OnClick="btnAddProveedor_Click" AutoPostBack="false"  />            
                </div>
            </div>
    
        </div>

    </div>    
     
     
      <div style="display:none" runat="server">
            <asp:TextBox ID="_ElId" runat="server" Enable="false" BorderColor="White" BorderStyle="None" ForeColor="White"></asp:TextBox>
            <asp:TextBox ID="_Accion" runat="server" Enable="false" BorderColor="White" BorderStyle="None" ForeColor="White"></asp:TextBox>                                    
            <input type="hidden" runat="server" id="_URLVisor" />
          <input type="hidden" runat="server" id="_CadValoresSeleccionados" />
        </div>

    <div id="divDatos" runat="server" class="panel panel-success">
        <div class="panel-heading">
            <div class="row">
            <div class="col-md-8"><h3 class="panel-title">Productos</h3></div>
                <div class="col-md-1"> . </div>
                <div class="col-md-2"> Total : <asp:Label ID="txtImporteTotal" runat="server" Text="Total"></asp:Label>   </div>
            </div>
            </div>
        

        <asp:GridView ID="gridProductos" OnRowDeleting="gridProductos_RowDeleting" OnRowUpdating="gridProductos_RowUpdating" OnRowCancelingEdit="gridProductos_RowCancelingEdit" OnRowEditing="gridProductos_RowEditing" PageSize="10" Height="25px" EnablePersistedSelection="true" Width="1200px"  ShowHeaderWhenEmpty="true"  DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                                    <Columns>
                                        <asp:CommandField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ControlStyle-Font-Size="Smaller" ShowDeleteButton="true" HeaderText="Eliminar" />
                                        
                                        
                                        <asp:TemplateField HeaderText="Código" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"  HeaderStyle-Width="150px" HeaderStyle-CssClass="panel-footer" SortExpression="NOAplica">
                                            <EditItemTemplate>
                                                <input type="text" disabled="disabled" runat="server" value='<%# Bind("Articulo.Clave") %>' id="txtEsMedicamento" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:label runat="server" ID="lblMedicamento" Text='<%# Bind("Articulo.Clave") %>'></asp:label>
                                            </ItemTemplate>
                                             <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        
                                        
                                        <asp:TemplateField HeaderText="Producto" SortExpression="Orden" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"  HeaderStyle-CssClass="panel-footer" >
                                            <EditItemTemplate>
                                                <input type="text" disabled="disabled" runat="server" value='<%# Bind("Articulo.NombreCompleto") %>' id="txtNombre" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblNombre" Text='<%# Bind("Articulo.NombreCompleto") %>'></asp:Label>
                                            </ItemTemplate> 
                                        </asp:TemplateField>

                                        

                                        <asp:TemplateField HeaderText="Cantidad" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" HeaderStyle-CssClass="panel-footer"  SortExpression="NOAplica">
                                            <EditItemTemplate>
                                                <input type="number" runat="server" value='<%# Bind("Cantidad") %>' id="txtCantidad" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:label runat="server" Text='<%# Bind("Cantidad") %>' ID="lblCantidad"></asp:label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:CommandField HeaderStyle-Font-Size="Smaller" ControlStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ShowEditButton="True" />
                                        



                                        <asp:TemplateField HeaderText="$ Compra + IVA" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="120px"  HeaderStyle-CssClass="panel-footer"  SortExpression="NOAplica">
                                            <EditItemTemplate>
                                                <input type="text" disabled="disabled" runat="server" value='<%# Bind("Articulo.PrecioCompraIVA","{0:C2}") %>' id="txtPV" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Bind("Articulo.PrecioCompraIVA","{0:C2}") %>' ID="lblPV"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="SubTotal" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="120px" HeaderStyle-CssClass="panel-footer"  SortExpression="NOAplica">
                                            <EditItemTemplate>
                                                <input type="text" disabled="disabled" value='<%#Bind("Subtotal","{0:C2}") %>' runat="server" id="txtSubtotal" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSubtotal" Text='<%#Bind("Subtotal","{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        
                                        <asp:TemplateField HeaderText="IVA" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="120px" HeaderStyle-CssClass="panel-footer"  SortExpression="NOAplica">
                                            <EditItemTemplate>
                                                <input type="text" disabled="disabled" value='<%#Bind("IVA","{0:C2}") %>' runat="server" id="txtIVA" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblIVA" Text='<%#Bind("IVA","{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="120px" HeaderStyle-CssClass="panel-footer"  SortExpression="NOAplica">
                                             <EditItemTemplate>
                                                <input type="text" disabled="disabled" value='<%#Bind("Total","{0:C2}") %>' runat="server" id="txtTotal" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblTotal" Text='<%#Bind("Total","{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        
                                    </Columns>
                                    <%--<PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />--%>
                                </asp:GridView>
 


       <br>


     <div class="row">
                     <div class="col-md-12">
                         <div class="panel panel-default">
                            <div class="panel-heading">
                                <h3 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">Seleccione los productos</a>
                                </h3>
                            </div>

                            <div id="collapse1" class="panel-collapse">
                                <div class="panel-body">
                                    <div class="row" style="height:330px; overflow:scroll">

                                        <asp:GridView  ID="gridProductosCatalogo" PageSize="10" Height="15px" Width="1250px" EnablePersistedSelection="true" ShowHeaderWhenEmpty="true"  DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-Width="90px" HeaderStyle-CssClass="panel-footer" HeaderStyle-HorizontalAlign="Center" HeaderText="Seleccionar" SortExpression="SI">
                                                <ItemTemplate>
                                                    <input type="checkbox" value="false" runat="server" id="chkSeleccionar" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" ItemStyle-Width="450px" HeaderText="Nombre Producto" HeaderStyle-CssClass="panel-footer" SortExpression="Orden">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "NombreCompleto") %>
                                                    <input type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" id="idProducto" />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" ItemStyle-Width="110px" ItemStyle-Font-Size="Smaller" HeaderText="Es Medicamento" HeaderStyle-Width="150px" HeaderStyle-CssClass="panel-footer" SortExpression="Orden">
                                                <ItemTemplate>
                                                    <asp:label runat="server" ID="lblMedicamento" Text='<%# Convert.ToInt16(Eval("EsMedicamento")) == 0 ? "NO":"SI" %>'></asp:label>
                                                </ItemTemplate>
                                                <ItemStyle  HorizontalAlign="Center" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller"  HeaderStyle-Width="90px"  ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="$ Compra + IVA" HeaderStyle-CssClass="panel-footer" SortExpression="NOAplica">
                                                <ItemTemplate>                                                    
                                                    <asp:Label ID="lblPrecio2" Text='<%# Bind("PrecioCompraIVA","{0:C2}") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller"  HeaderStyle-Width="90px"  ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="$ Venta + IVA" HeaderStyle-CssClass="panel-footer" SortExpression="NOAplica">
                                                <ItemTemplate>                                                    
                                                    <asp:Label ID="lblPrecio" Text='<%# Bind("PrecioVentaIVA","{0:C2}") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-Width="90px" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Existencias" HeaderStyle-CssClass="panel-footer" SortExpression="NOAplica">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "CantidadEnAlmacen") %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
           
                                        </Columns>
                                        <%--<PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />--%>
                                    </asp:GridView>
                                    </div>
                                    <div class="row">
                                        <asp:Button runat="server" OnClick="btnAgregarDeCat_Click" OnClientClick="return fnc_ObtenerValoresSeleccionados();" ID="btnAgregarDeCat" Text="Agregar +" CssClass="btn btn-primary" />
                                    </div>
                                </div>
                            </div>

                        </div>
                     </div>








    </div>


    
    </div>
    



        


</div>    
</asp:Content>
