<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfFacturaCompararPreciosCatalogo.aspx.cs" Inherits="SIP.Formas.Compras.wfFacturaCompararPreciosCatalogo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    $(document).ready(function () {
        $('.campoNumerico').autoNumeric('init');
    });


   



    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container">

    <div id="divDatos" runat="server" class="panel panel-success">
        <div class="panel-heading">
            <h3 class="panel-title">Comparativo de Precios de Compra</h3>
        </div>

        <asp:GridView Height="25px" ShowHeaderWhenEmpty="true" CssClass="table" ID="grid" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                    <Columns>

                       
                        <asp:TemplateField HeaderText="Código" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Articulo.Clave") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Producto" ItemStyle-CssClass="col-md-4">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Articulo.Nombre") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>

                       

                        <asp:TemplateField HeaderText="Precio Compra Anterior" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("PrecioDeCompraAnterior","{0:C2}") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>
                    
                        <asp:TemplateField HeaderText="Precio Compra Actual" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("Precio","{0:C2}")  %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Diferencia" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("Diferencia","{0:C2}")  %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Precio Venta" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("PrecioVenta","{0:C2}")  %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Operación" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>                                                       
                                <asp:LinkButton ID="linkModificar" runat="server" PostBackUrl="#"  OnClick ="linkModificar_Click">Modificar</asp:LinkButton>                              
                        </ItemTemplate>
                    </asp:TemplateField>
                        
                            


                    </Columns>
                    
                
                    
            </asp:GridView>

    </div>



    <div class="row" id="divModificar" runat="server"> 
            <div class="col-md-4">.</div>

            <div class="col-md-4">
                <div class="form-group"">                    
                    <label for="PrecioVenta">Precio de Venta</label>
                    <input type="text" class="input-sm required form-control campoNumerico" id="txtPrecioVenta" runat="server" style="text-align: left; width:200px; align-items:flex-start" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtPrecioVenta" ErrorMessage="El campo Precio de Venta es obligatorio" ValidationGroup="validateX">*</asp:RequiredFieldValidator>                     
                </div>
            </div>


       
            <div class="col-md-4">                    
                <br/>
                <div class="form-group">
                    <div class="row"> 
                        <div class="col-md-6"><asp:Button  CssClass="btn btn-primary" Text="Guardar" ID="btnGuardarFactura" runat="server" OnClick="btnGuardarFactura_Click" AutoPostBack="false" ValidationGroup="validateX"/></div>
                        <div class="col-md-6"><asp:Button  CssClass="btn btn-default" Text="Cancelar" ID="btnRegresar" runat="server" OnClick="btnRegresar_Click" AutoPostBack="false" />                    </div>
                    </div>
                </div>
            </div>
                
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="validateX" ViewStateMode="Disabled" />
            </div>



      <div style="display:none" runat="server">
    <input type="hidden" runat="server" id="_ElId" />    
    </div>



</div>
</asp:Content>
