<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfVentasCancelacionesAdd.aspx.cs" Inherits="SIP.Formas.Ventas.wfVentasCancelacionesAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
       



        function fnc_ConfirmarCancelarVenta() {
            return confirm("¿Está seguro de realizar la cancelar la venta seleccionada?");
        }


      

    </script>


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container" id="divMain" runat="server">


    <div id="divVentas" runat="server" >

        <div class="panel panel-heading">
            <h3 class="panel-title">Seleccione la venta a cancelar</h3>
        </div>

        <asp:GridView Height="25px" ShowHeaderWhenEmpty="true" CssClass="table" ID="grid" DataKeyNames="Id" AutoGenerateColumns="False" runat="server" >
                <Columns>

                    <asp:TemplateField HeaderText="Venta" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("FolioCadena") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    

                    <asp:TemplateField HeaderText="Fecha" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Fecha","{0:d}") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Importe" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Importe","{0:C2}") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>


                     <asp:TemplateField HeaderText="Vendio" ItemStyle-CssClass="col-md-4">
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("Usuario.Nombre") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>


                   
                    <asp:TemplateField HeaderText="Operaciones">
                        <ItemTemplate>                           

                        <div id = "divIniciar" runat="server">
                        <asp:LinkButton ID="linkVenta" runat="server" PostBackUrl="#" OnClick ="linkVenta_Click">Iniciar...</asp:LinkButton>  
                        </div>
                        </ItemTemplate>
                    </asp:TemplateField> 


                </Columns>
                    
                
                    
        </asp:GridView>
    </div>



    <div class="row" id="divCancelacion" runat="server">

     
            <div class="col-md-8">
                <div class="form-group">
                <a>Observaciones</a>
                <input type="text" class="input-sm required form-control" id="txtObservaciones" runat="server" style="text-align: left; align-items:flex-start" />                                        
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtObservaciones" ErrorMessage="Las obsefvaciones de la cancelación son requeridas" ValidationGroup="validaFactura">*</asp:RequiredFieldValidator>                     
                </div>                                         
            </div>

            <div class="col-md-4">                    
                <br/>
                <div class="form-group">
                    <div class="row"> 
                        <div class="col-md-6"><asp:Button  CssClass="btn btn-primary" Text="Cancelar Venta" ID="btnCancelarVenta" runat="server" OnClick="btnCancelarVenta_Click" OnClientClick="return fnc_ConfirmarCancelarVenta()" AutoPostBack="false" ValidationGroup="validaFactura"/></div>
                        <div class="col-md-6"><asp:Button  CssClass="btn btn-default" Text="Regresar" ID="btnRegresar" runat="server" OnClick="btnRegresar_Click" AutoPostBack="false" />                    </div>
                    </div>
                </div>
            </div>
                
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="validaFactura" ViewStateMode="Disabled" />

    </div>


    <div style="display:none" runat="server">
    <input type="hidden" runat="server" id="_idVenta" />    
    </div>



</div>
</asp:Content>
