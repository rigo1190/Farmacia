<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfSalidasGenericas.aspx.cs" Inherits="SIP.Formas.Ventas.wfSalidasGenericas" %>
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

                alert("Los datos de la salida Número de Folio y Fecha son obligatorios. Verifique si existen productos para la salida");

                return false;

            } else {

                return true;

            }
               

        }


        function fnc_ObtenerValoresSeleccionados() {
            var cadena = "";
            var index = 0;
            var grid = document.getElementById('<%=gridProductosCatalogo.ClientID %>'); //Se recupera el grid
            

            for (i = 1; i < grid.rows.length; i++) { //Se recorren las filas
                var idProducto = "";
                var respuesta = 0;

                idProducto = $("input#ContentPlaceHolder1_gridProductosCatalogo" + "_idProducto_" + index).val();

                if (idProducto != null && idProducto != "" && idProducto != undefined) {

                    if ($("input#ContentPlaceHolder1_gridProductosCatalogo" + "_chkSeleccionar_" + index).is(':checked')) {

                        if (cadena == "")
                            cadena += idProducto;
                        else
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
             <h3>Salidas Internas</h3>
        </div>

         <div class="panel panel-success">
            <div class="panel-heading">
                <h3 class="panel-title">
                    Salidas Genéricas
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

                                        <asp:TemplateField ItemStyle-Font-Size="Smaller" ItemStyle-Width="450px"  HeaderText="Nombre Producto" HeaderStyle-CssClass="panel-footer" SortExpression="Orden">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Nombre") %>
                                                <input type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" id="idProducto" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Font-Size="Smaller" HeaderText="Es Medicamento" HeaderStyle-Width="150px" HeaderStyle-CssClass="panel-footer" SortExpression="Orden">
                                            <ItemTemplate>
                                                <asp:label runat="server" ID="lblMedicamento" Text='<%# Convert.ToInt16(Eval("EsMedicamento")) == 0 ? "NO":"SI" %>'></asp:label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Font-Size="Smaller"  HeaderStyle-HorizontalAlign="Center" HeaderText="Existencias" HeaderStyle-CssClass="panel-footer" SortExpression="NOAplica">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "CantidadDisponible") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Font-Size="Smaller"  HeaderStyle-HorizontalAlign="Center" HeaderText="Precio Venta" HeaderStyle-CssClass="panel-footer" SortExpression="NOAplica">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "PrecioVenta") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <%--<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Precio Venta" HeaderStyle-CssClass="panel-footer" SortExpression="NOAplica">
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
                                    <asp:Button runat="server" OnClick="btnAgregar_Click" OnClientClick="return fnc_ObtenerValoresSeleccionados();" ID="btnAgregar" Text="Agregar Salida" CssClass="btn btn-primary" />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                 <div class="row">
                     <div class="panel panel-default">
                        <div class="panel-heading">
                            Datos de la Salida
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
                                        <label>Tipo Salida:</label>
                                        <asp:DropDownList runat="server" ID="ddlTipos" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    
                                </div>
                            </div>

                            <div class="col-md-12">
                                <div class="form-group">
                                    <label>Observaciones:</label>
                                    <textarea type="text" name="prueba" style="height:100px" runat="server" class="form-control" id="txtObservaciones" />
                                </div>

                            </div>

                        </div>
                    </div>

                 </div>

                 <div class="row">
                     <div class="panel panel-default">
                        <div class="panel-heading">
                            Productos agregados a la salida
                        </div>
                        <div class="panel-body">
                            <div>
                                <asp:GridView AllowPaging="true" OnRowDeleting="gridProductos_RowDeleting" OnRowUpdating="gridProductos_RowUpdating" OnRowCancelingEdit="gridProductos_RowCancelingEdit" OnRowEditing="gridProductos_RowEditing" PageSize="10" Height="25px" EnablePersistedSelection="true" Width="1250px"  ShowHeaderWhenEmpty="true" ID="gridProductos" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                                    <Columns>
                                        <asp:CommandField HeaderStyle-CssClass="panel-footer" ShowDeleteButton="true" HeaderText="Eliminar" />
                                        <asp:TemplateField ItemStyle-Font-Size="Smaller" HeaderText="Nombre producto" HeaderStyle-CssClass="panel-footer" SortExpression="Orden">
                                            <EditItemTemplate>
                                                <input type="text" disabled="disabled" runat="server" value='<%# Bind("Nombre") %>' id="txtNombre" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblNombre" Text='<%# Bind("Nombre") %>'></asp:Label>
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

                                        <asp:CommandField HeaderStyle-CssClass="panel-footer" ShowEditButton="True" />

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

                     <div class="col-md-2">
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         <asp:Button runat="server" Text="Generar Salida" OnClick="btnGenerarSalida_Click" OnClientClick="return fnc_Validar(); " ID="btnGenerarSalida" CssClass="btn btn-primary" />
                     </div>

                     <div class="col-md-2">
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         <asp:Button runat="server" Text="Cancelar Salida" OnClick="btnCancelarSalida_Click" ID="btnCancelarSalida" CssClass="btn btn-default" />
                     </div>

                 </div>
            </div>
         </div>
    </div>
       
    <div runat="server" style="display:none">
        <input type="hidden" runat="server" id="_Accion" />
        <input type="hidden" runat="server" id="_CadValoresSeleccionados" />
    </div>

</asp:Content>
