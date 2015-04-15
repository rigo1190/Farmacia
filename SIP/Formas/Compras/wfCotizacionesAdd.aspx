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
     
     

    <div id="divDatos" runat="server" class="panel panel-success">
        <div class="panel-heading">
            <div class="row">
            <div class="col-md-8"><h3 class="panel-title">Artículos</h3></div>
                <div class="col-md-1"> . </div>
                <div class="col-md-2"> Total : <asp:Label ID="txtImporteTotal" runat="server" Text="Total"></asp:Label>   </div>
            </div>
            </div>
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

                     <asp:TemplateField HeaderText="U.M." ItemStyle-CssClass="col-md-5">
                            <ItemTemplate>
                                <asp:Label ID="Label21" runat="server" Text='<%# Bind("Articulo.UnidadesDeMedida.Nombre") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>

                     <asp:TemplateField HeaderText="Presentación" ItemStyle-CssClass="col-md-5">
                            <ItemTemplate>
                                <asp:Label ID="Label22" runat="server" Text='<%# Bind("Articulo.Presentacion.Nombre") %>'></asp:Label>
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

    <div class="col-md-11">
       <div id="divCaptura" runat="server" class="panel panel-success">

        


        <div class="row">

            <div class="col-md-8">
                <div class="form-group">
                    <label for="articulo">Seleccione un artículo</label>
                    <div>
                        <asp:DropDownList ID="ddlArticulo" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group"">                    
                    <label for="Cantidad">Cantidad</label>
                    <input type="text" class="input-sm required form-control" id="txtCantidad" runat="server" style="text-align: left;  align-items:flex-start" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCantidad" ErrorMessage="El campo Cantidad es obligatorio" ValidationGroup="validateX">*</asp:RequiredFieldValidator>                     
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtCantidad" ErrorMessage="El Campo cantidad requiere un valor númerico" MaximumValue="10000" MinimumValue="1" Type="Integer" ValidationGroup="validateX">*</asp:RangeValidator>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <label for="Cantidad">.</label><br />
                        <asp:Button  CssClass="btn btn-default" Text="Agregar" ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" AutoPostBack="false" ValidationGroup="validateX"/>            
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


    
    
    



        


</div>    
</asp:Content>
