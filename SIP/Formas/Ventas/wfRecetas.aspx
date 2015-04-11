<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfRecetas.aspx.cs" Inherits="SIP.Formas.Ventas.wfRecetas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            $('.datepicker').datepicker(
            {
                format: "dd/mm/yyyy"
            });

        });

        function fnc_MostrarReceta(idReceta) {

            if (idReceta == "" || idReceta == null || idReceta == undefined) {

                if ($("#<%= _IDReceta.ClientID %>").val() != "")
                    idReceta = parseInt($("#<%= _IDReceta.ClientID %>").val());
                else
                    return false;
            }
                

            var izq = (screen.width - 750) / 2
            var sup = (screen.height - 600) / 2

            var url = "<%= ResolveClientUrl("~/rpts/wfVerReporte.aspx") %>";
            var argumentos = "?c=" + 1 + "&p=" + idReceta;
            url += argumentos;
            window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=750,height=500,top=' + sup + ',left=' + izq);

        }


        function fnc_Mensaje() {

            //jAlert('¿Está seguro de eliminar la receta y todos sus detalles?', 'Eliminar Receta', function (r) {
            //    if (r) {
            //        return false;
            //    }
            //});

            return confirm("¿Está seguro de eliminar la receta y todos sus detalles?");
            

        }

        function fnc_MensajeDetalle() {
            return confirm("¿Está seguro de eliminar el registro?");
        }

        function fnc_Nuevo() {
            $("#<%= divCaptura.ClientID %>").css("display", "block");
            $("#<%= divReceta.ClientID %>").css("display", "block");
            $("#<%= divGuardarReceta.ClientID %>").css("display", "block");
            $("#<%= divDetalleReceta.ClientID %>").css("display", "none");
            $("#<%= divEncabezado.ClientID %>").css("display", "none");
            $("#<%= txtObservaciones.ClientID %>").val("");
            $("#<%= txtNombre.ClientID %>").val("");
            $("#<%= txtFolio.ClientID %>").val("");
            $("#<%= txtFecha.ClientID %>").val("");
            $("#<%= _Accion.ClientID %>").val("N");
            $("#<%= _IDReceta.ClientID %>").val("");
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
        }


        function fnc_Cancelar() {
            $("#<%= divCaptura.ClientID %>").css("display", "none");
            $("#<%= divEncabezado.ClientID %>").css("display", "block");
            $("#<%= _Accion.ClientID %>").val("");
            $("#<%= _IDReceta.ClientID %>").val("");
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
        }

        
        function fnc_Validar() {

            var error = false;
            var folio = $("#<%= txtFolio.ClientID %>").val();
            var fecha = $("#<%= txtFecha.ClientID %>").val();
            var paciente = $("#<%= txtNombre.ClientID %>").val();

            if (folio == "" || folio == null || folio == undefined)
                error = true;
            else if (fecha == "" || fecha == null || fecha == undefined)
                error = true;
            else if (paciente == "" || paciente == null || paciente == undefined)
                error = true;
           

            if (error == true) {
                var accion = $("#<%= _Accion.ClientID %>").val();

                if (accion == "N") {
                    alert("Los datos de la venta Número de Folio, Fecha y Nombre del Paciente son obligatorios.");
                }

                return false;

            } else {

                return true;

            }


        }


        function fnc_ValidarDetalle() {
            var error = false;
            var resultado;
            var mensaje = "";
            var producto = $("#<%= txtNombreMedicamento.ClientID %>").val();
            var accion = $("#<%= _Accion.ClientID %>").val();

            if (accion != "N") {
                resultado = fnc_Validar();
                
                if (!resultado) {

                    mensaje = "Los datos de la venta Número de Folio, Fecha y Nombre del Paciente son obligatorios."
                    alert(mensaje);

                    return false;
                }
                
            }

            if (producto == "" || producto == null || producto == undefined)
                error = true;
            
            if (error == true) {
                alert("El dato de Nombre del Producto es obligatorio.");
                return false;
            }
                


        }




    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

    <div class="container">
        <div class="page-header"">
             <h3>Recetas</h3>
        </div>

        <div class="panel panel-success">
            <div class="panel-heading">
                <h3 class="panel-title">
                    Elaboración de Recetas
                </h3>
            </div>

            <div class="panel-body">
                <div id="divEncabezado" runat="server">
                     <div class="row">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Listado de Recetas
                            </div>
                            <div class="panel-body">
                                
                                <asp:GridView Width="1250px" AllowPaging="true" OnPageIndexChanging="gridRecetas_PageIndexChanging" OnRowDataBound="gridRecetas_RowDataBound" PageSize="10" Height="25px" EnablePersistedSelection="true" ShowHeaderWhenEmpty="true" ID="gridRecetas" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                                     <Columns>
                                            <asp:TemplateField HeaderText="Acciones">
                                                <ItemTemplate>
                                                    <asp:ImageButton OnClick="imgBtnEdit_Click" ID="imgBtnEdit" ToolTip="Editar" runat="server" ImageUrl="~/img/Edit1.png" />
                                                    <asp:ImageButton OnClick="imgBtnEliminarReceta_Click"  ID="imgBtnEliminarReceta" ToolTip="Eliminar" runat="server" ImageUrl="~/img/close.png" OnClientClick="return fnc_Mensaje();"/>
                                                </ItemTemplate>
                                                <HeaderStyle BackColor="#EEEEEE" />
                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="50px" BackColor="#EEEEEE" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="120px" HeaderText="Número de Folio" SortExpression="Orden">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "Folio") %>
                                                    <%--<input type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" id="idPregunta" />--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderText="Fecha" SortExpression="Orden">
                                                <ItemTemplate>
                                                    <%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "Fecha")).ToString("d")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Nombre Paciente" SortExpression="NOAplica">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "NombrePaciente") %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Observaciones" SortExpression="NOAplica">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "Observaciones") %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Imprimir" SortExpression="NOAplica">
                                                <ItemTemplate>
                                                    <button type="button" runat="server" id="btnVer"><span class="glyphicon glyphicon-print"></span></button>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                        </Columns>
                                     <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                </asp:GridView>

                                <div id="divBtnNuevo" runat="server">
                                    <button type="button" onclick="fnc_Nuevo();" id="btnNuevo" class="btn btn-primary" value="Nuevo">Nuevo</button>
                                </div>

                            </div>
                        </div>
                    </div>

                </div>

                <div class="row" style="display:none" runat="server" id="divCaptura" >
                   <div class="col-lg-12">
                    <div runat="server" id="divReceta" class="panel panel-default">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-md-11">
                                <h3 class="panel-title"><i class="fa"></i>Datos de la Receta</h3>
                                </div>
                                <div class="col-md-1">
                                    <button type="button" id="btnImprimir0" onclick="fnc_MostrarReceta();"><span class="glyphicon glyphicon-print"></span></button>
                                </div>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Número de Folio:</label>
                                        <input type="text" name="prueba" runat="server" class="form-control" id="txtFolio" />
                                    </div>

                                    <div class="form-group">
                                        <label>Fecha:</label>
                                        <div class="input-group">
                                            <input class="form-control datepicker" runat="server" id="txtFecha"/>
                                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                        </div>
                                    </div>

                                    

                                </div>

                                <div class="col-md-6">
                                     <div class="form-group">
                                        <label>Nombre del Paciente:</label>
                                        <input type="text" name="prueba" runat="server" class="form-control" id="txtNombre" />
                                    </div>

                                    <div class="form-group">
                                        <label>Observaciones:</label>
                                        <textarea type="text" name="prueba" style="height:100px" runat="server" class="form-control" id="txtObservaciones" />
                                    </div>
                                </div>

                                

                                <div id="divGuardarReceta" runat="server" class="form-group">
                                    <asp:Button OnClick="btnGuardar_Click" ID="btnGuardar" OnClientClick="return fnc_Validar();" runat="server" Text="Guardar" CssClass="btn btn-primary" ></asp:Button>
                                    <button type="button" onclick="fnc_Cancelar();" class="btn btn-default">Cancelar</button> 
                                </div>
                            </div>
                        </div>
                    </div>

                    <div style="display:none" runat="server" id="divDetalleReceta" class="panel panel-default">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-md-11">
                                <h3 class="panel-title"><i class="fa"></i>Detalle de la Receta</h3>
                                </div>
                                <div class="col-md-1">
                                    <button type="button" onclick="fnc_MostrarReceta();" id="btnImprimir"><span class="glyphicon glyphicon-print"></span></button>
                                </div>
                            </div>
                            
                            
                        </div>
                        <div class="panel-body">
                            <asp:GridView AllowPaging="true" OnPageIndexChanging="gridDetalleRecetas_PageIndexChanging" OnRowDataBound="gridDetalleRecetas_RowDataBound" PageSize="10" Height="25px" EnablePersistedSelection="true" Width="1250px" ShowHeaderWhenEmpty="true" ID="gridDetalleRecetas" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Eliminar">
                                            <ItemTemplate>
                                                <asp:ImageButton  ID="imgBtnEliminarDetalle" ToolTip="Eliminar" runat="server" ImageUrl="~/img/close.png" OnClick="imgBtnEliminarDetalle_Click" OnClientClick="return fnc_MensajeDetalle();"/>
                                            </ItemTemplate>
                                            <HeaderStyle BackColor="#EEEEEE" />
                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="50px" BackColor="#EEEEEE" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderText="Nombre" SortExpression="Orden">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "NombreMedicamento") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderText="Cantidad" SortExpression="Orden">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "CantidadATomar") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Frecuencia" SortExpression="NOAplica">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Frecuenca") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Durante" SortExpression="NOAplica">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Durante") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                </asp:GridView>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Nombre Producto:</label>
                                        <textarea type="text" name="prueba" style="height:50px" runat="server" class="form-control" id="txtNombreMedicamento" />
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <label>Lista de Productos:</label>
                                    <asp:DropDownList ID="ddlMedicamentos" OnSelectedIndexChanged="ddlMedicamentos_SelectedIndexChanged" runat="server" CssClass="form-control" AutoPostBack="True"></asp:DropDownList>                                         
                                </div>

                            </div>
                            
                            <div>
                                <div class="form-group">
                                    <label>Cantidad a tomar:</label>
                                    <input type="text" name="prueba" runat="server" class="form-control" id="txtCandidad" />
                                </div>
                                
                                <div class="form-group">
                                    <label>Frecuencia:</label>
                                    <input type="text" name="prueba" runat="server" class="form-control" id="txtFrecuencia" />
                                </div>

                                <div class="form-group">
                                    <label>Durante:</label>
                                    <input type="text" name="prueba" runat="server" class="form-control" id="txtDurante" />
                                </div>

                            </div>


                            <div class="form-group">
                                <asp:Button OnClick="btnGuardarDetalle_Click" ID="btnGuardarDetalle" OnClientClick="return fnc_ValidarDetalle();" runat="server" Text="Agregar/Guardar" CssClass="btn btn-primary" ></asp:Button>
                                <button type="button" onclick="fnc_Cancelar();" class="btn btn-default">Cancelar</button> 
                            </div>

                        </div>

                     </div>


                </div>
                </div>

                <div class="row">
                    <div class="col-lg-12">
                        <div class="panel-footer">
                            <div class="alert alert-danger" runat="server" id="divMsgError" style="display:none">
                                <asp:Label ID="lblMsgError" EnableViewState="false" runat="server" Text="" CssClass="font-weight:bold"></asp:Label>
                            </div>
                            <div class="alert alert-success" runat="server" id="divMsgSuccess" style="display:none">
                                <asp:Label ID="lblMsgSuccess" EnableViewState="false" runat="server" Text="" CssClass="font-weight:bold"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>


            </div>


        </div>
        


    </div>

    <div runat="server" style="display:none">
        <input type="hidden" runat="server" id="_IDReceta" />
        <input type="hidden" runat="server" id="_Accion" />
    </div>

</asp:Content>
