<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfPedidos.aspx.cs" Inherits="SIP.Formas.Compras.wfPedidos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 


</asp:Content>


    
 

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
    
 


     <div id="divEncabezado" runat="server" class="panel panel-success">
      <div class="panel-heading">
             <div class="row">
                <div class="col-md-8"><h3 class="panel-title"> Pedidos </h3></div>
                <div class="col-md-2"> . </div>
                <div class="col-md-2"><a href="<%=ResolveClientUrl("wfCotizaciones.aspx") %>">Regresar</a></div>
             </div>
       </div>
     </div>




    <div id="divAsignar" runat="server" class="panel panel-success">





        <div class="row">


            <div class="col-md-4">
                <div class="form-group">
                    <a>Seleccione el proveedor</a> 
                    <asp:DropDownList ID="ddlProveedor" CssClass="form-control" runat="server"></asp:DropDownList>                                
                </div>
            </div>

            <div class="col-md-4">
                <div class="form-group">
                    <a>Seleccione el producto</a> 
                    <asp:DropDownList ID="ddlArticulo" CssClass="form-control" runat="server"></asp:DropDownList>                                
                </div>
            </div>
                            
            <div class="col-md-2" id="DIVagregar" runat="server" >

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group"><br />
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
    
            <div class="col-md-2" id="DIVgenerarPedidos" runat="server" >

                

                <div class="form-group">
                    <br />
                        <asp:Button ID="btnGenerar" runat="server" Text="Generar Pedidos" CssClass="btn btn-primary" OnClick="btnGenerar_Click" AutoPostBack="false" />
                </div>
            </div>


        </div>



         <asp:GridView Height="25px" ShowHeaderWhenEmpty="true" CssClass="table" ID="gridAsignaciones" DataKeyNames="Id" AutoGenerateColumns="False" runat="server" >
                <Columns>


                    
                    <asp:TemplateField HeaderText="RFC" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                            <asp:Label ID="lblRFC" runat="server" Text='<%# Bind("Proveedor.RFC") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="Razón Social" ItemStyle-CssClass="col-md-3">
                        <ItemTemplate>
                            <asp:Label ID="lblRazonSocial" runat="server" Text='<%# Bind("Proveedor.RazonSocial") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Código" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                            <asp:Label ID="lblTel" runat="server" Text='<%# Bind("Articulo.Clave") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Producto" ItemStyle-CssClass="col-md-4">
                        <ItemTemplate>
                            <asp:Label ID="lblCel" runat="server" Text='<%# Bind("Articulo.NombreCompleto") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Cantidad" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                            <asp:Label ID="lblMail" runat="server" Text='<%# Bind("Cantidad") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                        <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                         
                            <asp:ImageButton ID="imgBtnEliminar" ToolTip="Borrar" runat="server" ImageUrl="~/img/close.png"  OnClick="imgBtnEliminar_Click"/>
                            
                        </ItemTemplate>
                        <HeaderStyle BackColor="#EEEEEE" />
                        <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" BackColor="#EEEEEE" />
                    </asp:TemplateField>                                  


                </Columns>
                    
                
                    
        </asp:GridView>




    </div>


    <div id="divPedidos" runat="server" class="panel panel-success">
        
        <div class="bs-example">
            <div class="panel-group" id="accordion" runat="server">

                

            </div>
            </div>

    </div>




     

<div style="display:none" runat="server">
    <input type="hidden" runat="server" id="_URLVisor" />    
    </div>



        </div>
</asp:Content>
