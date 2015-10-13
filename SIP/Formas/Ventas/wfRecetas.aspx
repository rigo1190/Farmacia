<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfRecetas.aspx.cs" Inherits="SIP.Formas.Ventas.wfRecetas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            $('.datepicker').datepicker(
            {
                format: "dd/mm/yyyy"
            });

            $("#<%= ddlPaciente.ClientID %>").change(function () {
                var value = $(this).val();

                if (value!="0"){
                    $("#<%= txtNombre.ClientID %>").val($(this).find('option:selected').text());
                    $("#<%= txtNombre.ClientID %>").prop("disabled", true);
                }
                    
                else{
                    $("#<%= txtNombre.ClientID %>").val("");
                    $("#<%= txtNombre.ClientID %>").prop("disabled", false);
                }
                    
            });


            $("#<%= ddlMedicamentos.ClientID %>").change(function(){
                var value = $(this).val();

                if (value!="0"){
                    $("#<%= txtNombreMedicamento.ClientID %>").val($(this).find('option:selected').text());
                    $("#<%= txtNombreMedicamento.ClientID %>").prop("disabled", true);
                }
                else {
                    $("#<%= txtNombreMedicamento.ClientID %>").val("");
                    $("#<%= txtNombreMedicamento.ClientID %>").prop("disabled", false);
                }
                    
            });


        });

        function fnc_MostrarModal() {
            $("#myModal").modal('show'); //Se muestra el modal
        }


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

        function fnc_AbrirArchivo(idReceta,nombre) {

            var ruta = '<%= ResolveClientUrl("~/AbrirDocto.aspx") %>';
            var izq = (screen.width - 750) / 2
            var sup = (screen.height - 600) / 2
            window.open(ruta + '?n=' + nombre + '&i='+idReceta, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=750,height=500,top=' + sup + ',left=' + izq);
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
            $("#<%= txtObsParticulares.ClientID %>").val("");
            $("#<%= txtNombre.ClientID %>").val("");
            $("#<%= txtFolio.ClientID %>").val("");
            $("#<%= _Accion.ClientID %>").val("N");
            $("#<%= _IDReceta.ClientID %>").val("");
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
            $("#<%= divBtnImagen.ClientID %>").css("display", "none");
            

            var date = new Date();
            $("#<%= txtFecha.ClientID %>").val(date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear());
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
            var fecha = $("#<%= txtFecha.ClientID %>").val();
            var paciente = $("#<%= txtNombre.ClientID %>").val();

            if (fecha == "" || fecha == null || fecha == undefined)
                error = true;
            else if (paciente == "" || paciente == null || paciente == undefined)
                error = true;
           

            if (error == true) {
                var accion = $("#<%= _Accion.ClientID %>").val();

                if (accion == "N") {
                    alert("Los datos de la receta Fecha y Nombre del Paciente son obligatorios.");
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

                    mensaje = "Los datos de la receta Fecha y Nombre del Paciente son obligatorios."
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

    <div class="container" id="divMain" runat="server">

        <div class="panel panel-success">
            <div class="panel-heading">
                <div class="row">

                    <div class="col-md-10">
                        <h3 class="panel-title">
                            Recetas
                        </h3>
                    </div>
                    <div class="col-md-2">
                        <a href="<%=ResolveClientUrl("~/Formas/Catalogos/wfCatPacientes.aspx") %>">Catálogo de Pacientes</a>
                    </div>


                </div>
                
            </div>

            

            

            <div class="panel-body">
                <div id="divEncabezado" runat="server">
                     <div class="row">
                        <div class="panel panel-default">
                            
                            <div class="panel-body">
                                <div class="row">  

                                    <div class="col-md-4">
                                        <div id="divBtnNuevo" runat="server">
                                            <button type="button" onclick="fnc_Nuevo();" id="btnNuevo" class="btn btn-primary" value="Nuevo">Nuevo</button>
                                        </div>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-6"></div>



                                </div>

                                

                                <div style="height:330px; overflow:scroll">
                                     <asp:GridView Width="1250px" OnPageIndexChanging="gridRecetas_PageIndexChanging" OnRowDataBound="gridRecetas_RowDataBound" PageSize="10" Height="25px" EnablePersistedSelection="true" ShowHeaderWhenEmpty="true" ID="gridRecetas" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderText="Acciones">
                                                <ItemTemplate>
                                                    <asp:ImageButton OnClick="imgBtnEdit_Click" ID="imgBtnEdit" ToolTip="Editar" runat="server" ImageUrl="~/img/Edit1.png" />
                                                    <asp:ImageButton OnClick="imgBtnEliminarReceta_Click"  ID="imgBtnEliminarReceta" ToolTip="Eliminar" runat="server" ImageUrl="~/img/close.png" OnClientClick="return fnc_Mensaje();"/>
                                                </ItemTemplate>
                                                <HeaderStyle BackColor="#EEEEEE" />
                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="50px" BackColor="#EEEEEE" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="120px" HeaderText="Número de Folio" SortExpression="Orden">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "FolioCadena") %>
                                                    <%--<input type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" id="idPregunta" />--%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderText="Status" SortExpression="Orden">
                                                <ItemTemplate>
                                                    <asp:label runat="server" ID="lblStatus" Text='<%# Convert.ToInt16(Eval("Status")) == 1 ? "NO SURTIDA":"SURTIDA" %>'></asp:label>
                                                </ItemTemplate>
                                                 <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderText="Fecha" SortExpression="Orden">
                                                <ItemTemplate>
                                                    <%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "Fecha")).ToString("d")%>
                                                </ItemTemplate>
                                                 <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Nombre Paciente" SortExpression="NOAplica">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "NombrePaciente") %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Observaciones" SortExpression="NOAplica">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "Observaciones") %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Imágenes" SortExpression="NOAplica">
                                                <ItemTemplate>
                                                    <button type="button" runat="server" id="btnImagenes" onserverclick="btnImagenes_ServerClick"><span class="glyphicon glyphicon-camera"></span></button>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Imprimir" SortExpression="NOAplica">
                                                <ItemTemplate>
                                                    <button type="button" runat="server" id="btnVer"><span class="glyphicon glyphicon-print"></span></button>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                        </Columns>
                                        <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                    </asp:GridView>
                                </div>

                                
                                <table>
                                            <tr>                                                
                                                <td>
                                                    <label>Fecha recetas:</label>
                                                </td>
                                                <td>
                                                    <div class="input-group">
                                                        <input  class="form-control datepicker" runat="server" id="txtFechaFiltro"/>
                                                        <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                                    </div>
                                                </td>
                                                <td>
                                                     <div>
                                                        <asp:Button ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click" CssClass="btn btn-default" />
                                                    </div>
                                                </td>
                                                
                                            </tr>
                                        
                                        </table>


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
                            <div class="row">

                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Número de Folio:</label>
                                        <input type="text" name="prueba" runat="server" disabled="disabled" class="form-control" id="txtFolio" />
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
                                        <label>Paciente:</label>
                                        <asp:DropDownList ID="ddlPaciente" runat="server" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                    </div>
                                </div>

                                
                            </div>

                            <div class="row">

                                <div class="col-md-6">
                                     
                                    <div class="form-group">
                                        <label>Nombre del Paciente:</label>
                                        <input type="text" name="prueba" runat="server" class="form-control" id="txtNombre" />
                                    </div>

                                    <div class="form-group">
                                        <label>Observaciones:</label>
                                        <textarea type="text" name="prueba" style="height:70px" runat="server" class="form-control" id="txtObservaciones" />
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Adjuntar Imagen:</label>
                                        <asp:FileUpload ID="fileUpload" runat="server" />
                                    </div>

                                    <div class="form-group" id="divBtnImagen" runat="server">
                                        <asp:Button runat="server" ID="btnGuardarImagen" OnClick="btnGuardarImagen_Click" Text="Guardar Imagen" CssClass="btn btn-primary" />
                                    </div>

                                </div>

                            </div>

                            <div class="row">

                                <div class="col-md-9">

                                </div>
                                <div class="col-md-3">
                                     <div id="divGuardarReceta" runat="server" class="form-group">
                                        <div class="form-group">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; 
                                            <asp:Button OnClick="btnGuardar_Click" ID="btnGuardar" OnClientClick="return fnc_Validar();" runat="server" Text="Guardar" CssClass="btn btn-primary" ></asp:Button>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                            <button type="button" onclick="fnc_Cancelar();" class="btn btn-default">Cancelar</button> 
                                        </div>
                                    </div>
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

                            <div style="height:170px; overflow:scroll">
                                <asp:GridView AllowPaging="true" OnPageIndexChanging="gridDetalleRecetas_PageIndexChanging" OnRowDataBound="gridDetalleRecetas_RowDataBound" PageSize="10" Height="25px" EnablePersistedSelection="true" Width="1250px" ShowHeaderWhenEmpty="true" ID="gridDetalleRecetas" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                                        <Columns>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderText="Eliminar">
                                            <ItemTemplate>
                                                <asp:ImageButton  ID="imgBtnEliminarDetalle" ToolTip="Eliminar" runat="server" ImageUrl="~/img/close.png" OnClick="imgBtnEliminarDetalle_Click" />
                                            </ItemTemplate>
                                            <HeaderStyle BackColor="#EEEEEE" />
                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="50px" BackColor="#EEEEEE" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderText="Nombre" SortExpression="Orden">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "NombreMedicamento") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderText="Aplicar" SortExpression="Orden">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "CantidadATomar") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Frecuencia" SortExpression="NOAplica">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Frecuenca") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Durante" SortExpression="NOAplica">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Durante") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Observaciones" SortExpression="NOAplica">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Observaciones") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                    </Columns>
                                        <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                </asp:GridView>
                            </div>

                            

                            <div class="row">
                                <div class="col-md-6">
                                    <label>Lista de Productos:</label>
                                    <asp:DropDownList ID="ddlMedicamentos" runat="server" CssClass="form-control" AutoPostBack="False"></asp:DropDownList>                                         
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Nombre Producto:</label>
                                        <textarea type="text" name="prueba" style="height:50px" runat="server" class="form-control" id="txtNombreMedicamento" />
                                    </div>
                                </div>
                            </div>
                            
                            <div class="row">

                                <div class="col-md-3">
                                     <div class="form-group">
                                        <label>Aplicar:</label>
                                        <input type="text" name="prueba" runat="server" class="form-control" id="txtCandidad" />
                                    </div>
                                </div>
                               
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Frecuencia:</label>
                                        <input type="text" name="prueba" runat="server" class="form-control" id="txtFrecuencia" />
                                    </div>
                                </div>
                                
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Durante:</label>
                                        <input type="text" name="prueba" runat="server" class="form-control" id="txtDurante" />
                                    </div>
                                </div>
                                
                               

                            </div>
                                 
                            <div class="row">
                                <div class="col-md-9">
                                    <div class="form-group">
                                        <label>Observaciones particulares:</label>
                                        <textarea type="text" name="prueba" style="height:50px" runat="server" class="form-control" id="txtObsParticulares" />
                                    </div>
                                </div>
                                
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <asp:Button OnClick="btnGuardarDetalle_Click" ID="btnGuardarDetalle" OnClientClick="return fnc_ValidarDetalle();" runat="server" Text="Agregar Producto" CssClass="btn btn-default" ></asp:Button>                                       
                                         
                                    </div>
                                    <div class="form-group">
                                        <button type="button" onclick="fnc_Cancelar();" class="btn btn-primary">Terminar Receta</button> 
                                    </div>
                                </div>
                                

                            </div>
                               
                                
                            </div>


                            <%--<div class="form-group">
                               
                            </div>--%>

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
        

     <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="smallModal" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="myModalLabel">Imágenes</h4>
              </div>
              <div class="modal-body" runat="server" id="divImagenes">
                  
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Aceptar</button>
              </div>
        
            </div>
        </div>
    </div>


    
    
    <div runat="server" style="display:none">
        <input type="hidden" runat="server" id="_IDReceta" />
        <input type="hidden" runat="server" id="_Accion" />
    </div>

</asp:Content>
