
<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfCatPacientes.aspx.cs" Inherits="SIP.Formas.Catalogos.wfCatPacientes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript">



     function fnc_Confirmar() {
         return confirm("¿Está seguro de eliminar el registro?");
     }




    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container" id="divMain" runat="server">

    <div class="panel-footer alert alert-success" id="divMsgSuccess" style="display:none" runat="server">
                <asp:Label ID="lblMensajeSuccess" runat="server" Text=""></asp:Label>
    </div>
    <div class="panel-footer alert alert-danger" id="divMsg" style="display:none" runat="server">
                <asp:Label ID="lblMensajes" runat="server" Text=""></asp:Label>
    </div>

    <div id="divDatos" runat="server" class="panel panel-success">
        <div class="panel-heading">
            <div class="row">

                    <div class="col-md-10">
                        <h3 class="panel-title">
                            Pacientes
                        </h3>
                    </div>
                    <div class="col-md-2">
                        <a href="<%=ResolveClientUrl("~/Formas/Ventas/wfRecetas.aspx") %>">Recetas</a>
                    </div>


                </div>
        </div>


        <asp:GridView Height="25px" ShowHeaderWhenEmpty="true" CssClass="table" ID="grid" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                <Columns>



                    <asp:TemplateField HeaderText="Nombre" ItemStyle-CssClass="col-md-6">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Nombre") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="Teléfono" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Telefono") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Correo" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Correo") %>'></asp:Label>
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
            <h3 class="panel-title">Pacientes</h3>
        </div>



                <div class="form-group"">
                    
                    <label for="Nombre">Nombre</label>
                    <input type="text" class="input-sm required form-control" id="txtNombre" runat="server" style="text-align: left; align-items:flex-start" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNombre" ErrorMessage="El campo Nombre es obligatorio" ValidationGroup="validateX">*</asp:RequiredFieldValidator>
                </div>
                       
                <div class="form-group">
                    <label for="Telefono">Teléfono</label>
                    <input type="text" class="input-sm required form-control" id="txtTelefono" runat="server" style="text-align: left;   align-items:flex-start" />                    
                </div>

                <div class="form-group">
                    <label for="Correo">Correo</label>
                    <input type="text" class="input-sm required form-control" id="txtCorreo" runat="server" style="text-align: left;   align-items:flex-start" />                    
                </div>

            <div class="form-group">
                    <asp:Button  CssClass="btn btn-primary" Text="Guardar" ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" AutoPostBack="false" ValidationGroup="validateX" />
                    <asp:Button  CssClass="btn btn-default" Text="Cancelar" ID="btnCancelar" runat="server" OnClick="btnCancelar_Click"  AutoPostBack="false" />
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
