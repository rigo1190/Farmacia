<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfVentasByLector.aspx.cs" Inherits="SIP.Formas.Ventas.wfVentasByLector" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">

    $(document).ready(function () {

        $('.datepicker').datepicker(
        {
            format: "dd/mm/yyyy"
        });

    });


    function fnc_MostrarPassword() {
        $("#modalPassword").modal('show'); //Se muestra el modal
        $("#<%= txtPassword.ClientID %>").val("");
            $("#<%= divMsgError.ClientID %>").css("display", "none");
                $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
                $("#<%= lblMsgError.ClientID %>").val("");

                return false;
    }


    function fnc_Validar() {

        var error = false;
        var fecha = $("#<%= txtFecha.ClientID %>").val();
            var grid = document.getElementById('<%=gridProductos.ClientID %>');
        var filas = grid.rows.length;


        if (fecha == "" || fecha == null || fecha == undefined)
            error = true;
        else if (filas == 1)
            error = true;

        if (error == true) {

            alert("Los datos de la venta Fecha son obligatorios. Verifique si existen productos para la venta");

            return false;

        } else {

            return true;

        }


    }





    function KeyDownHandler(btn) {
        if (event.keyCode == 13) {
            event.returnValue = false;
            event.cancel = true;
            // aca llamas al evento click del boton que queres que actue, en tu caso es como si apretara el boton de conseguir los datos.
            btn.click();
        }
    }


</script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container">
        



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

         <div class="panel panel-success">
            <div class="panel-heading">


                <div class="row">
                    <div class="col-md-8"><h3 class="panel-title">Venta con lectora de códigos de barras</h3></div>
                        <div class="col-md-4 text-right">
                            <a href="<%=ResolveClientUrl("~/Formas/Ventas/wfVentasDia.aspx") %>" >Regresar</a>
                        </div>
                  </div>        

            </div>

             <div class="panel-body">

                  <div class="row" style="display:none">
                     <div class="panel panel-default">
                        <div class="panel-heading">
                            Detalle de la venta
                        </div>
                        <div class="panel-body">
                            <div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Número de Folio:</label>
                                        <input type="text" name="prueba" runat="server" disabled="disabled" class="form-control" id="txtFolio" />
                                    </div>
                                    
                                </div>
                                <div class="col-md-4">
                                    
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
                        
                        <div class="panel-body">
                            <div style="height:170px; overflow:scroll">

                                
                                <asp:GridView Height="25px" ShowHeaderWhenEmpty="true" CssClass="table" ID="gridProductos" DataKeyNames="Id" AutoGenerateColumns="False" runat="server" OnRowDeleting="gridProductos_RowDeleting" >
                                    <Columns>


                                        <asp:CommandField HeaderText="Eliminar" HeaderStyle-Font-Size="Smaller" ItemStyle-CssClass="col-md-1" HeaderStyle-CssClass="panel-footer" ControlStyle-Font-Size="Smaller" ShowDeleteButton="true"  />
                                        
                                        
                                        <asp:TemplateField HeaderText="Nombre Producto" ItemStyle-CssClass="col-md-5" HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Nombre") %>'></asp:Label>
                                            </ItemTemplate>                        
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Medicamento" ItemStyle-CssClass="col-md-1" HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer">
                                            <ItemTemplate>
                                                <asp:label runat="server" ID="lblMedicamento" Text='<%# Convert.ToInt16(Eval("EsMedicamento")) == 0 ? "NO":"SI" %>'></asp:label>
                                            </ItemTemplate>                        
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Cantidad" ItemStyle-CssClass="col-md-1" HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Cantidad") %>'></asp:Label>
                                            </ItemTemplate>                        
                                        </asp:TemplateField>



                                        <asp:TemplateField HeaderText="Precio Venta" ItemStyle-CssClass="col-md-1" HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer">
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("PrecioVenta","{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>                        
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Subtotal" ItemStyle-CssClass="col-md-1" HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer">
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("Subtotal","{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>                        
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="IVA" ItemStyle-CssClass="col-md-1" HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("IVA","{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>                        
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Total" ItemStyle-CssClass="col-md-1" HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer">
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("Total","{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>                        
                                        </asp:TemplateField>

                                        
                                    </Columns>
                                    
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                 </div>



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

                     <div class="col-md-4">
                        <div class="form-group">
                            <label>SUBTOTAL:</label>
                            <input type="text" value="0.00" style="font-size:x-large; font-style:italic; text-align:right" disabled="disabled" name="prueba" runat="server" class="form-control" id="txtTotalR" />
                        </div>

                        <div class="form-group">
                            <label>IVA:</label>
                            <input type="text" value="0.00" style="font-size:x-large; font-style:italic; text-align:right" disabled="disabled" name="prueba" runat="server" class="form-control" id="txtIVAR" />
                        </div>

                        <div class="form-group">
                            <label>TOTAL:</label>
                            <input type="text" value="0.00" style="font-size:x-large; font-style:italic; text-align:right" disabled="disabled" name="prueba" runat="server" class="form-control" id="txtCobrar" />
                        </div>
                         <button class="btn btn-primary" onclick="fnc_MostrarPassword(); return false;">Generar Ventas</button>
                         <asp:Button runat="server" Text="Cancelar Venta" OnClick="btnCancelarVenta_Click" ID="btnCancelarVenta" CssClass="btn btn-default" />



                         <div class="form-group" runat="server" id="DIVFechaVenta">
                            <label>Fecha:</label>
                            <div class="input-group">
                                <input class="form-control datepicker" runat="server" id="txtFecha"/>
                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                            </div>
                         </div>


                    </div>

                 </div>

                 

            </div>


        </div>


    <div runat="server" style="display:none">
        <input type="hidden" runat="server" id="_producto" />        
    </div>


    <div class="modal fade" id="modalPassword" tabindex="-1" role="dialog" aria-labelledby="smallModal" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="ModalLabel">Ingrese Password de confirmación</h4>
              </div>
              <div class="modal-body">
                  <asp:TextBox CssClass="form-control" runat="server" ID="txtPassword" TextMode="Password"></asp:TextBox>
              </div>
              <div class="modal-footer">
                 <asp:Button runat="server" Text="Generar Venta" OnClick="btnAceptarVenta_Click" ID="btnAceptarVenta" OnClientClick="return fnc_Validar();" CssClass="btn btn-primary" />
                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
              </div>
        
            </div>
        </div>
    </div>




    </div>

</asp:Content>
