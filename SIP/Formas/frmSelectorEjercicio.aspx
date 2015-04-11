<%@ Page Title="" Language="C#" MasterPageFile="~/MSelectorEjercicio.Master" AutoEventWireup="true" CodeBehind="frmSelectorEjercicio.aspx.cs" Inherits="SIP.Formas.frmSelectorEjercicio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

     <script type="text/javascript">
                

         function fnc_Validar() {
                         
            
            var unidadpresupuestal = $("#<%= ddlUnidadPresupuestal.ClientID %>").val();
             if (unidadpresupuestal == null || unidadpresupuestal.length == 0 || unidadpresupuestal == undefined || unidadpresupuestal == 0) {
                alert("Indique la Unidad presupuestal");
                return false;
            }
           
             var ejercicio = $("#<%= ddlEjercicios.ClientID %>").val();
             if (ejercicio == null || ejercicio.length == 0 || ejercicio == undefined || ejercicio == 0) {
                 alert("Debe indicar el ejercicio");
                 return false;
             }           
             
             return true;
         }
              

    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      
   <br />
   <br />

    <div class="container col-md-offset-4 col-md-4">
                     

        <div class="panel panel-success">

            <div class="panel-heading">
                <h3 class="panel-title">Seleccione el ejercicio y la unidad presupuestal</h3>
            </div>

            <div class="panel-body">

                                 

                     <div class="form-group">
                           <label for="ddlUnidadPresupuestal" class="control-label" runat="server">Unidad presupuestal:</label>
                            <div>
                                <asp:DropDownList ID="ddlUnidadPresupuestal" runat="server" CssClass ="form-control" AutoPostBack="True" ></asp:DropDownList>                                         
                            </div>
                       </div>      
                                         

                 <div class="form-group">
                   <label for="ddlEjercicios" class="control-label" runat="server">Ejercicios:</label>
                    <div>
                        <asp:DropDownList ID="ddlEjercicios" runat="server" CssClass="form-control" AutoPostBack="True" ></asp:DropDownList>                                         
                    </div>
                 </div>

            </div>

            <div class="panel-footer clearfix">

                <div class="pull-right">                
                    <asp:Button ID="btnSeleccionar" runat="server" Text="Seleccionar" CssClass="btn btn-primary" OnClientClick="return fnc_Validar()"  OnClick="btnSeleccionar_Click"></asp:Button>
                </div>

            </div>
           
    </div>
    

</asp:Content>
