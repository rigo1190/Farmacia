<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfFacturasNueva.aspx.cs" Inherits="SIP.Formas.Compras.wfFacturasNueva" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {

            $('.datepicker').datepicker(
            {
                format: "dd/mm/yyyy"
            });

            $('.campoNumerico').autoNumeric('init');

        });

 

    
        function fncChecked() {

            if ($("#<%= chkExtras.ClientID %>").is(':checked')) {

                $("#<%= divCantidadExtra.ClientID %>").css("display", "block");                
                
            } else {

                $("#<%= divCantidadExtra.ClientID %>").css("display", "none");                
            }


        }

    </script>


</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container">




     <div id="divEncabezado" runat="server" class="panel panel-success">
      <div class="panel-heading">
             <div class="row">
                <div class="col-md-8"><h3 class="panel-title"> Registro de Facturas... </h3></div>
                <div class="col-md-2"> . </div>
                <div class="col-md-2"><a href="<%=ResolveClientUrl("wfFacturas.aspx") %>">Regresar</a></div>
             </div>
       </div>
     </div>


    <div class="panel-footer alert alert-danger" id="divMsg" style="display:none" runat="server">
        <asp:Label ID="lblMensajes" runat="server" Text=""></asp:Label>
    </div>


       
    <div id="divPedidos" runat="server" >

        <div class="panel panel-heading">
            <h3 class="panel-title">Seleccione el pedido a registrar</h3>
        </div>

        <asp:GridView Height="25px" ShowHeaderWhenEmpty="true" CssClass="table" ID="grid" DataKeyNames="Id" AutoGenerateColumns="False" runat="server" >
                <Columns>

                    <asp:TemplateField HeaderText="Pedido" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Folio") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Proveedor" ItemStyle-CssClass="col-md-6">
                        <ItemTemplate>
                            <asp:Label ID="Label2Prov" runat="server" Text='<%# Bind("Proveedor.RazonSocial") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Fecha" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Fecha","{0:d}") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    

                   
                    <asp:TemplateField HeaderText="Operaciones">
                        <ItemTemplate>                           

                        <div id = "divIniciar" runat="server">
                        <asp:LinkButton ID="LinkPedido" runat="server" PostBackUrl="#" OnClick ="LinkIniciar_Click">Iniciar...</asp:LinkButton>  
                        </div>
                        </ItemTemplate>
                    </asp:TemplateField> 


                </Columns>
                    
                
                    
        </asp:GridView>
    </div>



    <div id="divFactura" runat="server" >

        <div class="panel panel-heading">

            <div class="row">
                <div class="col-md-8"><h3 class="panel-title">Indique los precios de los productos de la factura</h3></div>
                <div class="col-md-4"> <asp:DropDownList ID="ddlModo" CssClass="form-control" runat="server"></asp:DropDownList> </div>
                
             </div>


            
        </div>

        <div class="row" id="DIVCerrarProceso" runat="server"> 
            <div class="col-md-2">
                <div class="form-group">
                <a>Folio</a>
                <input type="text" class="input-sm required form-control" id="txtFolio" runat="server" style="text-align: left; align-items:flex-start" />                                        
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFolio" ErrorMessage="El campo Folio es obligatorio" ValidationGroup="validaFactura">*</asp:RequiredFieldValidator>                     
                </div>                                         
            </div>


            <div class="col-md-2">
                <div class="form-group">
                    <a>Fecha</a> 
                    <input type="text" class="required form-control date-picker" id="dtpFecha" runat="server" data-date-format = "dd/mm/yyyy"  autocomplete="off" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="dtpFecha" ErrorMessage="La fecha es obligatoria" ValidationGroup="validaFactura">*</asp:RequiredFieldValidator>
                </div>
            </div>

                
            <div class="col-md-4">    
                <div class="form-group">
                    <a>Proveedor</a> 
                    <input type="text" class="input-sm required form-control" id="txtProveedor" runat="server" style="text-align: left; align-items:flex-start" disabled="disabled" />                                        
                </div>                    
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <a>Importe</a> 
                    <input type="text" class="input-sm required form-control" id="txtImporte" runat="server" style="text-align: left; align-items:flex-start" disabled="disabled" />                                                            
                </div>            
            </div>

            <div class="col-md-2">                    
                <br/>
                <div class="form-group">
                    <div class="row"> 
                        <div class="col-md-6"><asp:Button  CssClass="btn btn-primary" Text="Guardar" ID="btnGuardarFactura" runat="server" OnClick="btnGuardarFactura_Click" AutoPostBack="false" ValidationGroup="validaFactura"/></div>
                        <div class="col-md-6"><asp:Button  CssClass="btn btn-default" Text="Cancelar" ID="btnRegresar" runat="server" OnClick="btnRegresar_Click" AutoPostBack="false" />                    </div>
                    </div>
                </div>
            </div>
                
                
            </div>
        
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="validaFactura" ViewStateMode="Disabled" />
    
        <div class="row" id="DIVagregar" runat="server">

                    <div class="col-md-6">
                        <div class="form-group">
                            <a>Seleccione el producto</a> 
                            <asp:DropDownList ID="ddlArticulo" CssClass="form-control" runat="server"></asp:DropDownList>                                
                        </div>
                    </div>

                    <div class="col-md-2">
                        <div class="form-group"">                    
                            <a>Precio</a>
                            <input type="text" class="input-sm required form-control campoNumerico" id="txtPrecio" runat="server" style="text-align: left;  align-items:flex-start" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPrecio" ErrorMessage="El campo Precio es obligatorio" ValidationGroup="validateX">*</asp:RequiredFieldValidator>                                                     
                        </div>
                    </div>

                    
                      
                    <div class="col-md-2"  >
                        


                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group"> <br />
                                        <asp:Button  CssClass="btn btn-default" Text="Agregar" ID="btnAgregar" runat="server" OnClick="btnAgregar_Click" AutoPostBack="false"/>            
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group"><br />
                                        <asp:Button  CssClass="btn btn-default" Text="Descartar" ID="btnDescartar" runat="server" OnClick="btnDescartar_Click" AutoPostBack="false"/>            
                                </div>
                            </div>
                        </div>




                    </div>

                    <div class="col-md-2"  >                        
                        <input onclick="fncChecked()" type="checkbox" value="false" runat="server" id="chkExtras" title="asd" /> <a> Cantidad Adicional</a>
                        <div class="form-group" runat="server" id="divCantidadExtra">
                            <input type="text" class="input-sm required form-control campoNumerico" id="txtCantidadExtra" runat="server" style="text-align: left;  align-items:flex-start" />                                
                        </div>
                    </div>
  

                </div>

        <asp:GridView Height="25px" ShowHeaderWhenEmpty="true" CssClass="table" ID="gridDetalle" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                    <Columns>

                        <asp:TemplateField HeaderText="Código" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="lblCodigo" runat="server" Text='<%# Bind("Articulo.Clave") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="Nombre" ItemStyle-CssClass="col-md-4">
                            <ItemTemplate>
                                <asp:Label ID="lblNombre" runat="server" Text='<%# Bind("Articulo.NombreCompleto") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>
                    
                        <asp:TemplateField HeaderText="Cantidad" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="lblCantidad" runat="server" Text='<%# Bind("Cantidad") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Adicional" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="lblAdicional" runat="server" Text='<%# Bind("CantidadExtra") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Precio" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="lblPrecio" runat="server" Text='<%# Bind("Precio","{0:C2}") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Precio + IVA" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="lblPrecio2" runat="server" Text='<%# Bind("PrecioIVA","{0:C2}") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>

                        
                        <asp:TemplateField HeaderText="Subtotal" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="lblSubtotal" runat="server" Text='<%# Bind("Subtotal","{0:C2}") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>

                        
                        <asp:TemplateField HeaderText="IVA" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="lblIVA" runat="server" Text='<%# Bind("IVA","{0:C2}") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>

                        
                        <asp:TemplateField HeaderText="Total" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("Total","{0:C2}") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>

                            <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                                        
                                <asp:ImageButton ID="imgBtnEliminar" ToolTip="Borrar" runat="server" ImageUrl="~/img/close.png"  OnClick="imgBtnEliminar_Click"/>
                            
                            </ItemTemplate>
                            <HeaderStyle BackColor="#EEEEEE" />
                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" BackColor="#EEEEEE" />
                        </asp:TemplateField>                                  


                    </Columns>
                    
                
                    
            </asp:GridView>
    
    </div>



        

      <div style="display:none" runat="server">
    <input type="hidden" runat="server" id="_idPedido" />    
    </div>


    </div>

  

     

</asp:Content>
