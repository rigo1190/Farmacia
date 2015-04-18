﻿<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="wfVentasDia.aspx.cs" Inherits="SIP.Formas.Ventas.wfVentasDia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <div class="page-header"">
             <h3>Ventas del Día</h3>
        </div>

        <div class="panel panel-success">
            <div class="panel-heading">
                <h3 class="panel-title">
                   Ventas
                </h3>
            </div>

            <div class="panel-body">
                <div class="row">
                    <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="row">
                            <h3 class="panel-title"><i class="fa"></i>Listado de Ventas</h3>
                        </div>
                    </div>
                    <div class="panel-body">

                        <div class="row">

                            <div class="col-md-8">

                            </div>

                            <div class="col-md-4">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btnVR" OnClick="btnVR_Click" Text="Venta por recetas" CssClass="btn btn-default" />
                                <asp:Button runat="server" ID="btnV" OnClick="btnV_Click" Text="Venta por catálogo" CssClass="btn btn-default" />
                            </div>

                        </div>

                        <div style="height:330px; overflow:scroll">
                             <asp:GridView Width="1250px" PageSize="10" Height="25px" EnablePersistedSelection="true" ShowHeaderWhenEmpty="true" ID="gridVentas" DataKeyNames="Id" AutoGenerateColumns="False" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer"  HeaderText="Acciones">
                                        <ItemTemplate>
                                            <%--<asp:ImageButton OnClick="imgBtnEdit_Click" ID="imgBtnEdit" ToolTip="Editar" runat="server" ImageUrl="~/img/Edit1.png" />
                                            <asp:ImageButton OnClick="imgBtnEliminarReceta_Click"  ID="imgBtnEliminar" ToolTip="Eliminar" runat="server" ImageUrl="~/img/close.png" OnClientClick="return fnc_Mensaje();"/>--%>
                                        </ItemTemplate>
                                        <HeaderStyle BackColor="#EEEEEE" />
                                        <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="50px" BackColor="#EEEEEE" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Small" HeaderStyle-Width="150px" HeaderText="Número de Folio" SortExpression="Orden">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Folio") %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderText="Fecha" SortExpression="Orden">
                                        <ItemTemplate>
                                            <%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "Fecha")).ToString("d")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller"  HeaderText="Cliente" SortExpression="NOAplica">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Cliente.RazonSocial") %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" ItemStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" HeaderText="Importe Total" SortExpression="NOAplica">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Importe") %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                  <%--  <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="panel-footer" HeaderStyle-HorizontalAlign="Justify" HeaderText="Imprimir" SortExpression="NOAplica">
                                    <ItemTemplate>
                                        <button type="button" runat="server" id="btnVer"><span class="glyphicon glyphicon-print"></span></button>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>--%>

                                </Columns>
                                <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                             </asp:GridView>
                        </div>
                       
                    </div>
                </div>
                </div>
            </div>
        </div>

    </div>

</asp:Content>
