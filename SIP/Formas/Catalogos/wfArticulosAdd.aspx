<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfArticulosAdd.aspx.cs" Inherits="SIP.Formas.Catalogos.wfArticulosAdd" %>
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
<div class="container">

    <div class="panel-footer alert alert-success" id="divMsgSuccess" style="display:none" runat="server">
                <asp:Label ID="lblMensajeSuccess" runat="server" Text=""></asp:Label>
    </div>
    <div class="panel-footer alert alert-danger" id="divMsg" style="display:none" runat="server">
                <asp:Label ID="lblMensajes" runat="server" Text=""></asp:Label>
    </div>

    <div id="divDatos" runat="server" class="panel panel-success">

        <div class="panel-heading">
             <div class="row">
                <div class="col-md-8"><h3 class="panel-title"> <asp:Label ID="txtTitulo" runat="server" Text="Subprograma"></asp:Label>   </h3></div>
                <div class="col-md-2"> . </div>
                <div class="col-md-2"><a href="<%=ResolveClientUrl("~/Formas/Catalogos/wfArticulos.aspx") %>">Regresar</a></div>
             </div>
        </div>

        <asp:GridView Height="25px" ShowHeaderWhenEmpty="true" CssClass="table" ID="grid" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                <Columns>



                    <asp:TemplateField HeaderText="Clave" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                            <asp:Label ID="lblClave" runat="server" Text='<%# Bind("Clave") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="Nombre" ItemStyle-CssClass="col-md-6">
                        <ItemTemplate>
                            <asp:Label ID="lblNombre" runat="server" Text='<%# Bind("Nombre") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Unidad de Medida" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="lblUM" runat="server" Text='<%# Bind("UnidadesDeMedida.Nombre") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Presentación" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="lblPresentacion" runat="server" Text='<%# Bind("Presentacion.Nombre") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                        <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                            
                            <asp:ImageButton ID="imgBtnEdit" ToolTip="Editar" runat="server" ImageUrl="~/img/Edit1.png" OnClick="imgBtnEdit_Click"/>
                            <asp:ImageButton ID="imgBtnEliminar" ToolTip="Borrar" runat="server" ImageUrl="~/img/close.png" OnClientClick="return fnc_Confirmar()" OnClick="imgBtnEliminar_Click"/>
                            
                        </ItemTemplate>
                        <HeaderStyle BackColor="#EEEEEE" />
                        <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" BackColor="#EEEEEE" />
                    </asp:TemplateField>                                  


                </Columns>
                    
                
                    
        </asp:GridView>



    </div>






    <div id="divBtnNuevo" runat="server">
        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-primary" OnClick="btnNuevo_Click" AutoPostBack="false" />
    </div>
    
    
       <div class="row"> 

       <div id="divCaptura" runat="server" class="panel panel-success">

        

        <div class="panel-heading">
             <div class="row">
                <div class="col-md-8"><h3 class="panel-title"> <asp:Label ID="txtTituloBis" runat="server" Text="Subprograma"></asp:Label>   </h3></div>
                <div class="col-md-2"> . </div>
                <div class="col-md-2"><a href="<%=ResolveClientUrl("~/Formas/Catalogos/wfArticulos.aspx") %>">Regresar</a></div>
             </div>
        </div>




<div class="row">

    <div class="col-md-4">
                <div class="form-group"">                    
                    <label for="Clave">Clave</label>
                    <input type="text" class="input-sm required form-control" id="txtClave" runat="server" style="text-align: left; width:200px; align-items:flex-start" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtClave" ErrorMessage="El campo clave es obligatorio" ValidationGroup="validateX">*</asp:RequiredFieldValidator>                     
                </div>

                <div class="form-group">
                    <label for="Descripcion">Nombre</label>
                    <textarea id="txtNombre" class="input-sm required form-control" runat="server" style="text-align: left; align-items:flex-start" rows="2" ></textarea>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNombre" ErrorMessage="El campo Nombre es obligatorio" ValidationGroup="validateX">*</asp:RequiredFieldValidator>                    
                </div>

                

                <div class="form-group">
                    <label for="UdM">Unidad de Medida</label>
                    <div>
                        <asp:DropDownList ID="ddlUM" CssClass="form-control" runat="server"></asp:DropDownList>
                </div>
                </div>


                <div class="form-group">
                    <label for="Presentacion">Presentación</label>
                    <div>
                        <asp:DropDownList ID="ddlPresentacion" CssClass="form-control" runat="server"></asp:DropDownList>
                </div>
                </div>

                <div class="form-group">
                    <label for="FPS">FPS</label>
                    <div>
                        <asp:DropDownList ID="ddlFPS" CssClass="form-control" runat="server"></asp:DropDownList>
                </div>
                </div>

    </div>

    <div class="col-md-4">




                <div class="form-group"">                    
                    <label for="Porcentaje">Porcentaje</label>
                    <input type="text" class="input-sm required form-control campoNumerico" id="txtPorcentaje" runat="server" style="text-align: left; width:200px; align-items:flex-start" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPorcentaje" ErrorMessage="El campo Porcentaje es obligatorio" ValidationGroup="validateX">*</asp:RequiredFieldValidator>                     
                </div>

                <div class="form-group"">                    
                    <label for="SA">Sustancia Activa</label>
                    <textarea id="txtSustanciaActiva" class="input-sm required form-control" runat="server" style="text-align: left; align-items:flex-start" rows="2" ></textarea>
                </div>

                

                <div class="form-group"">                    
                    <label for="Observaciones">Observaciones</label>
                    <textarea id="txtObservaciones" class="input-sm required form-control" runat="server" style="text-align: left; align-items:flex-start" rows="2" ></textarea>
                </div>

                <div class="form-group">
                    <label for="esMedicamento">Es Medicamento?</label>
                    <div>
                    <input type="checkbox" class="input-sm required form-control campoNumerico" id="chkEsMedicamento" runat="server" style="text-align: left; align-items:flex-start" data-v-min="0" data-v-max="100"/>
                </div>
                </div>

    </div>

    <div class="col-md-4">
                <div class="form-group"">                    
                    <label for="Cantidad">Cantidad</label>
                    <input type="text" class="input-sm required form-control campoNumerico" id="txtCantidad" runat="server" style="text-align: left; width:200px; align-items:flex-start" disabled="disabled" />                    
                </div>


                <div class="form-group"">                    
                    <label for="stockMinimo">Stock Mínimo</label>
                    <input type="text" class="input-sm required form-control campoNumerico" id="txtStockMinimo" runat="server" style="text-align: left; width:200px; align-items:flex-start" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtStockMinimo" ErrorMessage="El campo Stock Mínimo es obligatorio" ValidationGroup="validateX">*</asp:RequiredFieldValidator>                     
                </div>
    
    
                <div class="form-group"">                    
                    <label for="PrecioCompra">Precio de Compra</label>
                    <input type="text" class="input-sm required form-control campoNumerico" id="txtPrecioCompra" runat="server" style="text-align: left; width:200px; align-items:flex-start" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPrecioCompra" ErrorMessage="El campo Precio de Compra es obligatorio" ValidationGroup="validateX">*</asp:RequiredFieldValidator>                     
                </div>
    
                <div class="form-group"">                    
                    <label for="PrecioVenta">Precio de Venta</label>
                    <input type="text" class="input-sm required form-control campoNumerico" id="txtPrecioVenta" runat="server" style="text-align: left; width:200px; align-items:flex-start" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtPrecioVenta" ErrorMessage="El campo Precio de Venta es obligatorio" ValidationGroup="validateX">*</asp:RequiredFieldValidator>                     
                </div>

                   <div class="form-group">
                    <asp:Button  CssClass="btn btn-primary" Text="Guardar" ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" AutoPostBack="false" ValidationGroup="validateX" />
                    <asp:Button  CssClass="btn btn-default" Text="Cancelar" ID="btnCancelar" runat="server" OnClick="btnCancelar_Click"  AutoPostBack="false" />
            </div>
    
    </div>

</div>




            









 

                <div style="display:none" runat="server">
                    <asp:TextBox ID="_ElId" runat="server" Enable="false" BorderColor="White" BorderStyle="None" ForeColor="White"></asp:TextBox>
                    <asp:TextBox ID="_Accion" runat="server" Enable="false" BorderColor="White" BorderStyle="None" ForeColor="White"></asp:TextBox>
                                    
                </div>

                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="validateX" ViewStateMode="Disabled" />

            </div>
    </div>


</div>    

</asp:Content>
