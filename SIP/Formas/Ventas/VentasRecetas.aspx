﻿<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="VentasRecetas.aspx.cs" Inherits="SIP.Formas.Ventas.VentasRecetas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {

            $('.datepicker').datepicker(
            {
                format: "dd/mm/yyyy"
            });

        });


        function fnc_Validar() {

            var error = false;
            var folio = $("#<%= txtFolio.ClientID %>").val();
            var fecha = $("#<%= txtFecha.ClientID %>").val();
            var grid = document.getElementById('<%=gridProductos.ClientID %>');
            var filas = grid.rows.length;


            if (folio == "" || folio == null || folio == undefined)
                error = true;
            else if (fecha == "" || fecha == null || fecha == undefined)
                error = true;
            else if (filas == 1)
                error = true;

            if (error == true) {

                alert("Los datos de la venta Número de Folio y Fecha son obligatorios. Verifique si existen productos para la venta");

                return false;

            } else {

                return true;

            }


        }

        function fnc_MostrarVenta(idVenta) {

            <%--if (idReceta == "" || idReceta == null || idReceta == undefined) {

                if ($("#<%= _IDReceta.ClientID %>").val() != "")
                    idReceta = parseInt($("#<%= _IDReceta.ClientID %>").val());
                else
                    return false;
            }--%>


            var izq = (screen.width - 750) / 2
            var sup = (screen.height - 600) / 2

            var url = "<%= ResolveClientUrl("~/rpts/wfVerReporte.aspx") %>";
            var argumentos = "?c=" + 2 + "&p=" + idVenta;
            url += argumentos;
            window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=750,height=500,top=' + sup + ',left=' + izq);

        }


        function fnc_LimpiarIdReceta() {
            $("#<%= _IDReceta.ClientID %>").val("");
        }

        function fnc_MostrarModal() {
            $("#myModal").modal('show'); //Se muestra el modal
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

                    if ($("input#ContentPlaceHolder1_gridProductosCatalogo" + "_chkSeleccionar_" + index).is(':checked'))
                    {
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
        <div class="page-header"">
             <h3>Surtir Recetas</h3>
        </div>

         <div class="panel panel-success">
            <div class="panel-heading">
                <h3 class="panel-title">
                    Ventas a través de Recetas
                </h3>
            </div>

             <div class="panel-body">

                 <div class="row">
                     <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">Productos del Catálogo</a>
                            </h4>
                        </div>

                         <div id="collapse1" class="panel-collapse">
                            <div class="panel-body">

                                <div class="row">
                                    <asp:GridView AllowPaging="true" OnPageIndexChanging="gridProductosCatalogo_PageIndexChanging"  PageSize="10" Height="15px" Width="1250px" EnablePersistedSelection="true" ShowHeaderWhenEmpty="true" ID="gridProductosCatalogo" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="90px" HeaderStyle-CssClass="panel-footer" HeaderStyle-HorizontalAlign="Center" HeaderText="Seleccionar" SortExpression="SI">
                                            <ItemTemplate>
                                                <input type="checkbox" value="false" runat="server" id="chkSeleccionar" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Font-Size="Smaller" ItemStyle-Width="450px" HeaderText="Nombre Producto" HeaderStyle-CssClass="panel-footer" SortExpression="Orden">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Nombre") %>
                                                <input type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" id="idProducto" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Width="110px" ItemStyle-Font-Size="Smaller" HeaderText="Es Medicamento" HeaderStyle-Width="150px" HeaderStyle-CssClass="panel-footer" SortExpression="Orden">
                                            <ItemTemplate>
                                                <asp:label runat="server" ID="lblMedicamento" Text='<%# Convert.ToInt16(Eval("EsMedicamento")) == 0 ? "NO":"SI" %>'></asp:label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-Width="90px" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Existencias" HeaderStyle-CssClass="panel-footer" SortExpression="NOAplica">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "CantidadDisponible") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField  HeaderStyle-Width="90px"  ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Precio Venta" HeaderStyle-CssClass="panel-footer" SortExpression="NOAplica">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "PrecioVenta") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <%--<asp:TemplateField ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Unidad Medida" HeaderStyle-CssClass="panel-footer" SortExpression="NOAplica">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "UnidadesDeMedida.Clave") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>
                                    </Columns>
                                    <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                </asp:GridView>
                                </div>
                                  

                                <div class="row">
                                    <asp:Button runat="server" OnClick="btnAgregarDeCat_Click" OnClientClick="return fnc_ObtenerValoresSeleccionados();" ID="btnAgregarDeCat" Text="Agregar a Venta" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>

                    </div>

                 </div>

                 <div class="row">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapse0">Listado de Recetas</a>
                            </h4>
                        </div>
                        <div id="collapse0" class="panel-collapse">

                            <div class="panel-body">
                                <div class="row">
                                    <asp:GridView AllowPaging="true" OnPageIndexChanging="gridRecetas_PageIndexChanging"  PageSize="10" Height="15px" Width="1250px" EnablePersistedSelection="true" ShowHeaderWhenEmpty="true" ID="gridRecetas" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Font-Size="Smaller" ItemStyle-Width="180px" HeaderText="Número de Folio" HeaderStyle-CssClass="panel-footer" SortExpression="Orden">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "Folio") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Nombre Paciente" HeaderStyle-CssClass="panel-footer" SortExpression="NOAplica">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "NombrePaciente") %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Observaciones" HeaderStyle-CssClass="panel-footer" SortExpression="NOAplica">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "Observaciones") %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="180px" ItemStyle-Font-Size="Smaller" HeaderText="Detalle Productos" ItemStyle-CssClass="col-md-2" HeaderStyle-CssClass="panel-footer">
                                                <ItemTemplate>
                                                        <button type="button" onserverclick="btnProductos_ServerClick" id="btnProductos" runat="server"> <span class="glyphicon glyphicon-list-alt"></span></button> 
                                                </ItemTemplate>                          
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />                                            
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                    </asp:GridView>
                                </div>
                            </div>

                        </div>
                        
                    </div>
                </div>

                 <div class="row">
                     <div class="panel panel-default">
                        <div class="panel-heading">
                            Datos de la Venta
                        </div>
                        <div class="panel-body">
                            <div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Número de Folio:</label>
                                        <input type="text" name="prueba" runat="server" class="form-control" id="txtFolio" />
                                    </div>
                                    
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Fecha:</label>
                                        <div class="input-group">
                                            <input class="form-control datepicker" runat="server" id="txtFecha"/>
                                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Cliente:</label>
                                        <asp:DropDownList runat="server" ID="ddlClientes" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    
                                </div>
                            </div>
                        </div>
                    </div>

                 </div>

                 <div class="row">
                     <div class="panel panel-default">
                        <div class="panel-heading">
                            Productos agregados a la venta
                        </div>
                        <div class="panel-body">
                            <div>
                                <asp:GridView AllowPaging="true" OnRowDeleting="gridProductos_RowDeleting" OnRowUpdating="gridProductos_RowUpdating" OnRowCancelingEdit="gridProductos_RowCancelingEdit" OnRowEditing="gridProductos_RowEditing" PageSize="10" Height="25px" EnablePersistedSelection="true" Width="1250px"  ShowHeaderWhenEmpty="true" ID="gridProductos" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                                    <Columns>
                                        <asp:CommandField HeaderStyle-CssClass="panel-footer" ControlStyle-Font-Size="Smaller" ShowDeleteButton="true" HeaderText="Eliminar" />
                                        <asp:TemplateField ItemStyle-Font-Size="Smaller" HeaderText="Nombre producto" HeaderStyle-CssClass="panel-footer" SortExpression="Orden">
                                            <EditItemTemplate>
                                                <input type="text" disabled="disabled" runat="server" value='<%# Bind("Nombre") %>' id="txtNombre" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblNombre" Text='<%# Bind("Nombre") %>'></asp:Label>
                                            </ItemTemplate> 
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Font-Size="Smaller" HeaderText="Es Medicamento" HeaderStyle-Width="150px" HeaderStyle-CssClass="panel-footer" SortExpression="Orden">
                                            <EditItemTemplate>
                                                <input type="text" disabled="disabled" runat="server" value='<%# Convert.ToInt16(Eval("EsMedicamento")) == 0 ? "NO":"SI" %>' id="txtEsMedicamento" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:label runat="server" ID="lblMedicamento" Text='<%# Convert.ToInt16(Eval("EsMedicamento")) == 0 ? "NO":"SI" %>'></asp:label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" HeaderStyle-CssClass="panel-footer" HeaderText="Cantidad" SortExpression="NOAplica">
                                            <EditItemTemplate>
                                                <input type="number" runat="server" value='<%# Bind("Cantidad") %>' id="txtCantidad" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:label runat="server" Text='<%# Bind("Cantidad") %>' ID="lblCantidad"></asp:label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:CommandField ControlStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ShowEditButton="True" />
                                        
                                        <asp:TemplateField ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="120px"  HeaderStyle-CssClass="panel-footer" HeaderText="Precio Venta" SortExpression="NOAplica">
                                            <EditItemTemplate>
                                                <input type="text" disabled="disabled" runat="server" value='<%# Bind("PrecioVenta") %>' id="txtPV" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Bind("PrecioVenta") %>' ID="lblPV"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="120px" HeaderStyle-CssClass="panel-footer" HeaderText="SubTotal" SortExpression="NOAplica">
                                            <EditItemTemplate>
                                                <input type="text" disabled="disabled" value='<%#Bind("SubTotal") %>' runat="server" id="txtSubtotal" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSubtotal" Text='<%#Bind("SubTotal") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="120px" HeaderStyle-CssClass="panel-footer" HeaderText="IVA" SortExpression="NOAplica">
                                            <EditItemTemplate>
                                                <input type="text" disabled="disabled" value='<%#Bind("IVA") %>' runat="server" id="txtIVA" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblIVA" Text='<%#Bind("IVA") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="120px" HeaderStyle-CssClass="panel-footer" HeaderText="Total" SortExpression="NOAplica">
                                             <EditItemTemplate>
                                                <input type="text" disabled="disabled" value='<%#Bind("Total") %>' runat="server" id="txtTotal" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblTotal" Text='<%#Bind("Total") %>'></asp:Label>
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

                 <div class="row">
                     <div class="col-md-8">
                         
                     </div>

                     <div class="col-md-4">
                         <div class="form-group">
                            <label>Total:</label>
                            <input type="text" value="0.00" style="font-size:x-large; font-style:italic" disabled="disabled" name="prueba" runat="server" class="form-control" id="txtTotalR" />
                         </div>

                         <div class="form-group">
                            <label>IVA:</label>
                            <input type="text" value="0.00" style="font-size:x-large; font-style:italic" disabled="disabled" name="prueba" runat="server" class="form-control" id="txtIVAR" />
                         </div>

                         <div class="form-group">
                            <label>A COBRAR:</label>
                            <input type="text" value="0.00" style="font-size:x-large; font-style:italic" disabled="disabled" name="prueba" runat="server" class="form-control" id="txtCobrar" />
                         </div>

                     </div>

                 </div>

                 <div class="row">
                     <div class="col-md-8">
                         
                     </div>

                     <div class="col-md-2">
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         <asp:Button runat="server" Text="Generar Venta" OnClick="btnAceptarVenta_Click" ID="btnAceptarVenta" OnClientClick="return fnc_Validar();" CssClass="btn btn-primary" />
                     </div>

                     <div class="col-md-2">
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         <asp:Button runat="server" Text="Cancelar Venta" OnClick="btnCancelarVenta_Click" ID="btnCancelarVenta" CssClass="btn btn-default" />
                     </div>

                 </div>

             </div>

         </div>

     </div>


     <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="smallModal" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="myModalLabel">Seleccionar Productos</h4>
              </div>
              <div class="modal-body">
                  <asp:CheckBoxList CssClass="form-control" ID="checkProductos" runat="server">
                      
                  </asp:CheckBoxList> 
              </div>
              <div class="modal-footer">
                <asp:Button runat="server" OnClick="btnAgregar_Click" ID="btnAgregar" Text="Agregar a Venta" CssClass="btn btn-primary" />
                <button type="button" onclick="fnc_LimpiarIdReceta();" class="btn btn-default" data-dismiss="modal">Cancelar</button>
              </div>
        
            </div>
        </div>
    </div>

    <div runat="server" style="display:none">
        <input type="hidden" runat="server" id="_IDReceta" />
        <input type="hidden" runat="server" id="_Accion" />
        <input type="hidden" runat="server" id="_CadValoresSeleccionados" />
    </div>


</asp:Content>