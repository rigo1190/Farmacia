<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfProveedores.aspx.cs" Inherits="SIP.Formas.Catalogos.wfProveedores" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">



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
            <h3 class="panel-title">Proveedores</h3>
        </div>


        <asp:GridView Height="25px" ShowHeaderWhenEmpty="true" CssClass="table" ID="grid" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                <Columns>



                    <asp:TemplateField HeaderText="RFC" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("RFC") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="Razón Social" ItemStyle-CssClass="col-md-5">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("RazonSocial") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Teléfonos" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Telefonos") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Celular" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("Celular") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="E-Mail" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("EMail") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                        <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="col-md-.5">
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
            <h3 class="panel-title">Proveedores  :: Registre en los campos los datos solicitados</h3>
        </div>


<div class="row">

    <div class="col-md-6">
        <div class="form-group"">                    
            <label for="Clave">RFC</label>
            <input type="text" class="input-sm required form-control" id="txtClave" runat="server" style="text-align: left; width:200px; align-items:flex-start" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtClave" ErrorMessage="El campo RFC es obligatorio" ValidationGroup="validateX">*</asp:RequiredFieldValidator>                     
        </div>

        <div class="form-group">
            <label for="Descripcion">Razón Social</label>
            <input type="text" class="input-sm required form-control" id="txtNombre" runat="server" style="text-align: left;   align-items:flex-start" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNombre" ErrorMessage="El campo Razón Social es obligatorio" ValidationGroup="validateX">*</asp:RequiredFieldValidator>                    
        </div>

        <div class="form-group">
            <label for="Representante">Representante</label>
            <input type="text" class="input-sm required form-control" id="txtRepresentante" runat="server" style="text-align: left; align-items:flex-start" />            
        </div>
        <div class="form-group">
            <label for="Calle">Calle y Número</label>
            <input type="text" class="input-sm required form-control" id="txtCalle" runat="server" style="text-align: left; align-items:flex-start" />            
        </div>
        <div class="form-group">
            <label for="Colonia">Colonia</label>
            <input type="text" class="input-sm required form-control" id="txtColonia" runat="server" style="text-align: left; align-items:flex-start" />            
        </div>

        <div class="form-group">
            <label for="Ciudad">Ciudad</label>
            <input type="text" class="input-sm required form-control" id="txtCiudad" runat="server" style="text-align: left; align-items:flex-start" />            
        </div>


    </div>


    <div class="col-md-6">

        <div class="form-group">
            <label for="Estado">Estado</label>
            <input type="text" class="input-sm required form-control" id="txtEstado" runat="server" style="text-align: left; align-items:flex-start" />            
        </div>
        <div class="form-group">
            <label for="CP">Código Postal</label>
            <input type="text" class="input-sm required form-control" id="txtCP" runat="server" style="text-align: left; align-items:flex-start" />            
        </div>


        <div class="form-group">
            <label for="Telefonos">Telefonos</label>
            <input type="text" class="input-sm required form-control" id="txtTelefonos" runat="server" style="text-align: left; align-items:flex-start" />            
        </div>            

        <div class="form-group">
            <label for="Celular">Celular</label>
            <input type="text" class="input-sm required form-control" id="txtCelular" runat="server" style="text-align: left; align-items:flex-start" />            
        </div>

        <div class="form-group">
            <label for="email">E-Mail</label>
            <input type="text" class="input-sm required form-control" id="txtEMail" runat="server" style="text-align: left; align-items:flex-start" />            
        </div>

        
            <div class="form-group">
                    <asp:Button  CssClass="btn btn-primary" Text="Guardar" ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" AutoPostBack="false" ValidationGroup="validateX" />
                    <asp:Button  CssClass="btn btn-default" Text="Cancelar" ID="btnCancelar" runat="server" OnClick="btnCancelar_Click"  AutoPostBack="false" />
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
