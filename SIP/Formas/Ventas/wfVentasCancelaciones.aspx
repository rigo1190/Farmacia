<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfVentasCancelaciones.aspx.cs" Inherits="SIP.Formas.Ventas.wfVentasCancelaciones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container" id="divMain" runat="server">


        <div id="divDatos" runat="server" class="panel panel-success">
        <div class="panel-heading">
            <h3 class="panel-title">Ventas Canceladas</h3>
        </div>

            <br />
        <div id="divBtnNuevo" runat="server">
            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-primary" OnClick="btnNuevo_Click" AutoPostBack="false" />
        </div>

            <br />
        <asp:GridView Height="25px" ShowHeaderWhenEmpty="true" CssClass="table" ID="grid" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                <Columns>



                    <asp:TemplateField HeaderText="Folio" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("FolioCadena") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="Fecha" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("FechaCancelacion","{0:d}") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    

                    <asp:TemplateField HeaderText="Observaciones" ItemStyle-CssClass="col-md-5">
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("ObservacionCancelacion") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Quién Registro" ItemStyle-CssClass="col-md-2">
                        <ItemTemplate>
                            <asp:Label ID="Label31" runat="server" Text='<%# Bind("UsuarioCancelacion") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                        <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="col-md-1">
                        <ItemTemplate>
                            
                            <asp:ImageButton ID="imgRPT" ToolTip="Ver Reporte" runat="server" ImageUrl="~/img/Sub.png" OnClick="imgRPT_Click"/>
                            
                            
                        </ItemTemplate>
                        <HeaderStyle BackColor="#EEEEEE" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" BackColor="#EEEEEE" />
                    </asp:TemplateField>                                  


                </Columns>
                    
                
                    
        </asp:GridView>



    </div>


    </div>
</asp:Content>
