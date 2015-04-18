<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfFacturasAdd.aspx.cs" Inherits="SIP.Formas.Compras.wfFacturasAdd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    $(document).ready(function () {
        $('.campoNumerico').autoNumeric('init');
    });


    function fnc_Confirmar() {
        return confirm("¿Está seguro de eliminar el registro?");
    }




    </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<div class="container">

            <div class="panel-footer alert alert-danger" id="divMsg" style="display:none" runat="server">
                <asp:Label ID="lblMensajes" runat="server" Text=""></asp:Label>
            </div>

        <div id="divEncabezado" runat="server" class="panel panel-success">
            <div class="panel-heading">
                <h3 class="panel-title">Datos de la Factura</h3>
            </div>


            <div class="row"> 
                <div class="col-md-3">

                    <div class="form-group">
                    <a>Folio</a>
                    <input type="text" class="input-sm required form-control" id="txtFolio" runat="server" style="text-align: left; align-items:flex-start" />                                        
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFolio" ErrorMessage="El campo Folio es obligatorio" ValidationGroup="validateY">*</asp:RequiredFieldValidator>                     
                    </div>

                    <div class="form-group">
                    <a>Fecha</a> 
                    <input type="text" class="required form-control date-picker" id="dtpFecha" runat="server" data-date-format = "dd/mm/yyyy"  autocomplete="off" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="dtpFecha" ErrorMessage="La fecha es obligatoria" ValidationGroup="validateY">*</asp:RequiredFieldValidator>
                    </div>

                     
                </div>


                <div class="col-md-6">    
                    <div class="form-group">
                        <a>Proveedor</a> 
                        <asp:DropDownList ID="ddlProveedor" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                    
                    <br/>
                    
                    <div class="form-group">
                        <a>Observaciones</a> 
                        <input type="text" class="input-sm required form-control" id="txtObservaciones" runat="server" style="text-align: left; align-items:flex-start" />                                                            
                    </div>

                </div>

                <div class="col-md-2"> 
                    <div class="form-group">
                    <a>Importe</a> 
                    <input type="text" class="input-sm required form-control" id="txtImporte" runat="server" style="text-align: left; align-items:flex-start" disabled="disabled" />                                                            
                    </div>
                    <br/><br/>
                     <div class="form-group">
                    <div class="row"> 
                        <div class="col-md-6"><asp:Button  CssClass="btn btn-primary" Text="Guardar" ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" AutoPostBack="false" ValidationGroup="validateY"/></div>
                        <div class="col-md-6"><asp:Button  CssClass="btn btn-default" Text="Cancelar" ID="btnCancel" runat="server" OnClick="btnCancel_Click" AutoPostBack="false" />                    </div>
                    </div>
                    </div>
                </div>
      
                
            </div>
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="validateY" ViewStateMode="Disabled" />
        </div>




        <div id="divDetalle" runat="server" class="panel panel-success">
            <div class="panel-heading">
                <h3 class="panel-title">Detalle de la Factura</h3>
            </div>
            
            <asp:GridView Height="25px" ShowHeaderWhenEmpty="true" CssClass="table" ID="grid" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                    <Columns>

                        <asp:TemplateField HeaderText="Clave" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Articulo.Clave") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="Nombre" ItemStyle-CssClass="col-md-5">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Articulo.Nombre") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>
                    
                        <asp:TemplateField HeaderText="Cantidad" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Cantidad") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Precio" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("Precio","{0:C2}") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>

                        
                        <asp:TemplateField HeaderText="Subtotal" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("Subtotal","{0:C2}") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>

                        
                        <asp:TemplateField HeaderText="IVA" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("IVA","{0:C2}") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>

                        
                        <asp:TemplateField HeaderText="Total" ItemStyle-CssClass="col-md-1">
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("Total","{0:C2}") %>'></asp:Label>
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


            <div class="row"> 

                <div class="col-md-12">
                   <div id="divCaptura" runat="server" class="panel panel-success">

        
                    <div class="row">



                        <div class="col-md-6">
                            <div class="form-group">
                                <a>Seleccione el producto</a> 
                                <asp:DropDownList ID="ddlArticulo" CssClass="form-control" runat="server"></asp:DropDownList>
                                
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group"">                    
                                <a>Cantidad</a>
                                <input type="text" class="input-sm required form-control" id="txtCantidad" runat="server" style="text-align: left;  align-items:flex-start" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCantidad" ErrorMessage="El campo Cantidad es obligatorio" ValidationGroup="validateX">*</asp:RequiredFieldValidator>                     
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtCantidad" ErrorMessage="El Campo cantidad requiere un valor númerico" MaximumValue="10000" MinimumValue="1" Type="Integer" ValidationGroup="validateX">*</asp:RangeValidator>
                            </div>
                        </div>


                        <div class="col-md-2">
                            <div class="form-group"">                    
                                <a>Precio</a>
                                <input type="text" class="input-sm required form-control campoNumerico" id="txtPrecio" runat="server" style="text-align: left;  align-items:flex-start" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPrecio" ErrorMessage="El campo Precio es obligatorio" ValidationGroup="validateX">*</asp:RequiredFieldValidator>                                                     
                            </div>
                        </div>


                        <div class="col-md-1">
                            <div class="form-group">
                                <a> .</a>
                                    <asp:Button  CssClass="btn btn-default" Text="Agregar" ID="btnAgregar" runat="server" OnClick="btnAgregar_Click" AutoPostBack="false" ValidationGroup="validateX"/>            
                            </div>
                        </div>
    
                    </div>
                
                      

                    <div style="display:none" runat="server">
                        <asp:TextBox ID="_ElId" runat="server" Enable="false" BorderColor="White" BorderStyle="None" ForeColor="White"></asp:TextBox>
                        <asp:TextBox ID="_Accion" runat="server" Enable="false" BorderColor="White" BorderStyle="None" ForeColor="White"></asp:TextBox>                                    
                        <input type="hidden" runat="server" id="_URLVisor" />
                    </div>

                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="validateX" ViewStateMode="Disabled" />

                        </div>
                </div>
                </div>
        </div>   

    </div>--%>

</asp:Content>
