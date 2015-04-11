<%@ Page Title="" Language="C#" MasterPageFile="~/NavegadorPrincipal.Master" AutoEventWireup="true" CodeBehind="RolesUsuario.aspx.cs" Inherits="SIP.Formas.Catalogos.RolesUsuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <style type="text/css">

        .edit-control 
        {
                 background: url('/img/edit1.png') no-repeat center center;
                 cursor: pointer;
        }

        .delete-control 
        {
                 background: url('/img/close.png') no-repeat center center;
                 cursor: pointer;
        }

    </style>

    <script type="text/javascript">

        $(document).ready(function () {
           
                                  
            $('.campoNumerico').autoNumeric('init');

            $('#tblRoles').dataTable();

            GetList();

            $('#btnAdd').click(function () 
            {
                AddRecord();
            });

            $('#btnUpdate').click(function () 
            {
                UpdateRecord();
            });

            $('#btnDelete').click(function ()
            {
                DeleteRecord();
            });
            
            
            $('#tblRoles tbody').on('click', '.edit-control.btn', function () {
                
                var rowId = $(this).data("rowid");                
                BeforeEditRecord(rowId);

            });
           
            $('#tblRoles tbody').on('click', '.delete-control.btn', function () {
                         
                var rowId = $(this).data("rowid");
                BeforeDeleteRecord(rowId);

            });
                      

        }); //$(document).ready

       


               
        
        function GetList()
        {
            $.ajax({

                type: "POST",
                url: "RolesUsuario.aspx/GetRoles",
                contentType: "application/json;charset=utf-8",
                data: {},
                dataType: "json",
                success: function (data) {

                    //On success

                    $('#tblRoles').dataTable({
                        "stateSave": true,
                        "destroy": true,                       
                        "data": data.d,
                        "language": {
                            "lengthMenu": "Mostrando _MENU_ registros por página",
                            "zeroRecords": "- Sin resultados -",
                            "info": "Mostrando página _PAGE_ de _PAGES_",
                            "infoEmpty": "No se encontraron registros",
                            "infoFiltered": "(filtrado de _MAX_ registros en total)",
                            "search": "<span class='glyphicon glyphicon-search'></span>",
                            "paginate":
                                {
                                    "next": "siguiente",
                                    "previous":"anterior"
				                }
                        },
                        "columns": [
                             {                               
                                 "orderable": false,
                                 "data": null,
                                 "defaultContent": "<button type='button' class='edit-control btn'>&nbsp;</button><button type='button' class='delete-control btn'>&nbsp;</button>"
                            },                         
                            {  "data": "Clave" },
                            {  "data": "Nombre" },
                            {  "data": "EsSefiplan", "orderable": false },
                            {  "data": "EsDependencia", "orderable": false },
                            {  "data": "Orden" }

                        ],
                        "createdRow": function (row, data, index)
                        {
                            $('.edit-control.btn', row).attr('data-rowid', data.Id);
                            $('.delete-control.btn', row).attr('data-rowid', data.Id);

                            if (data.EsSefiplan) {
                                $('td', row).eq(3).html('<span class="glyphicon glyphicon-ok text-center"></span>');
                            }
                            else
                            {
                                $('td', row).eq(3).empty();
                            }

                            if (data.EsDependencia) {
                                $('td', row).eq(4).html('<span class="glyphicon glyphicon-ok text-center"></span>');
                            }
                            else {
                                $('td', row).eq(4).empty();
                            }
                           
                        }

                    });


                },
                error: function (result)
                {
                    //On Error
                    var r = jQuery.parseJSON(result.responseText);
                    alert("On Error : \n" + r.Message);
                }

            });
        }
     
        function AddRecord()
        {           

            var clave=$('#txtClave').val();
            var nombre=$('#txtNombre').val();
            var orden = $('#txtOrden').val();
            var dependencia = $('#rbDependencia').is(":checked");
            var sefiplan = $('#rbSefiplan').is(":checked");
            
            $.ajax({
                type: "POST",
                url: "RolesUsuario.aspx/AddRecord",
                data: JSON.stringify({ clave: clave , nombre: nombre , orden: orden, dependencia: dependencia, sefiplan: sefiplan }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response)
                {

                    if (response.d.OK) 
                    {

                        $('#modalEdicion').modal('hide');

                        GetList();

                    }
                    else 
                    {
                        $('#divErroresEdit ul:first').empty();

                        $.map(response.d.Errors, function (n) {
                            $('#divErroresEdit ul:first').append($("<li>").text(n));
                        });

                        $('#divErroresEdit').show();
                    }

                },
                error: function (response)
                {
                    var r = jQuery.parseJSON(response.responseText);
                    
                    $('#divErroresEdicion ul:first').empty();
                    $('#divErroresEdicion ul:first').append($("<li>").text(r.Message));
                    $('#divErroresEdicion').show();


                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
        
        function UpdateRecord() 
        {
          
            var rowId = $('#rowId').val();
            var clave = $('#txtClave').val();
            var nombre = $('#txtNombre').val();
            var orden = $('#txtOrden').val();
            var dependencia = $('#rbDependencia').is(":checked");
            var sefiplan = $('#rbSefiplan').is(":checked");
                     

            $.ajax({
                type: "POST",
                url: "RolesUsuario.aspx/UpdateRecord",
                data: JSON.stringify({ id: rowId, clave: clave, nombre: nombre, orden: orden, dependencia: dependencia, sefiplan: sefiplan }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    if (response.d.OK)
                    {
                        $('#modalEdicion').modal('hide');
                        GetList();
                    }
                    else
                    {
                        $('#divErroresEdit ul:first').empty();

                        $.map(response.d.Errors, function (n) {
                            $('#divErroresEdit ul:first').append($("<li>").text(n));
                        });

                        $('#divErroresEdit').show();
                    }

                },
                error: function (response) {
                    var r = jQuery.parseJSON(response.responseText);

                    $('#divErroresEdit ul:first').empty();
                    $('#divErroresEdit ul:first').append($("<li>").text(r.Message));
                    $('#divErroresEdit').show();


                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        function DeleteRecord()
        {
                      
            var rowId = $('#rowId').val();
            var clave = $('#txtClave').val();
            var nombre = $('#txtNombre').val();
            var orden = $('#txtOrden').val();

            $.ajax({
                type: "POST",
                url: "RolesUsuario.aspx/DeleteRecord",
                data: JSON.stringify({ id: rowId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    if (response.d.OK)
                    {
                        $('#modalBorrar').modal('hide');
                        GetList();
                    }
                    else
                    {
                        $('#divErroresDelete ul:first').empty();

                        $.map(response.d.Errors, function (n) {
                            $('#divErroresDelete ul:first').append($("<li>").text(n));
                        });

                        $('#divErroresDelete').show();
                    }

                },
                error: function (response) {
                    var r = jQuery.parseJSON(response.responseText);

                    $('#divErroresDelete ul:first').empty();
                    $('#divErroresDelete ul:first').append($("<li>").text(r.Message));
                    $('#divErroresDelete').show();


                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        function GetRecord(recordId) 
        {            

            $.ajax(
                {

                type: "POST",
                url: "RolesUsuario.aspx/GetRecord",
                data: JSON.stringify({ id: recordId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    $('#txtClave').val(response.d.Clave);
                    $('#txtNombre').val(response.d.Nombre);
                    $('#txtOrden').val(response.d.Orden);
                                     
                    $('#rbDependencia').prop('checked', response.d.EsDependencia);
                    $('#rbSefiplan').prop('checked', response.d.EsSefiplan);
                    
                }
               
            });
        }
        
        function BeforeAddRecord()
        {

            $('#txtClave').val('');
            $('#txtNombre').val('');
            $('#txtOrden').val('');
            $('#rbSefiplan').prop("checked",true);           

            $('#btnUpdate').hide();
            $('#btnAdd').show();
            
            $('#divErroresEdicion').hide();
            $('#modalEdicion').modal('show');

           
        }

        function BeforeEditRecord(rowId) 
        {

            $('#rowId').val(rowId);         

            GetRecord(rowId);

            $('#btnAdd').hide();
            $('#btnUpdate').show();

            $('#divErroresEdit').hide();
            $('#modalEdicion').modal('show');

        }

        function BeforeDeleteRecord(rowId)
        {           
            $('#rowId').val(rowId);
            $('#divErroresDelete').hide();
            $('#modalBorrar').modal('show');
        }
        

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>

    <div class="container">
             
        <div class="panel panel-success">
          <div class="panel-heading">
            <h3 class="panel-title"><strong>Roles de usuario</strong></h3>
          </div>
          <div class="panel-body">
             

                <table id="tblRoles" class="table table-condensed table-bordered">
                    <thead>
                        <tr>
                          <th class="col-md-1 panel-footer">Acciones</th>
                          <th class="col-md-1 panel-footer">Clave</th>
                          <th class="panel-footer">Nombre</th>
                          <th class="col-md-1 panel-footer text-center">SEFIPLAN</th>
                          <th class="col-md-1 panel-footer text-center">Dependencia</th>
                          <th class="col-md-1 panel-footer">Orden</th>
                        </tr>                        
                    </thead>
                </table>
              
            
            <asp:Button ID="btnNuevo" runat="server" Text="Agregar rol" CssClass="btn btn-default" OnClientClick="BeforeAddRecord(); return false;" />             
            

          </div><!-- panel-body -->
        </div><!-- panel -->
        
    </div><!-- container -->



    <div class="modal fade" id="modalEdicion">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header alert alert-success">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title">Modificando registro</h4>
              </div>
              <div class="modal-body">

                  <div class="alert alert-danger" id="divErroresEdit" style="display:none">
                      <ul></ul>
                  </div>


                  <div class="form-group">
                     <label for="Clave">Clave</label>
                     <div>
                         <input type="text" class="input-sm required form-control" id="txtClave"  autofocus="autofocus" autocomplete="off" />                           
                     </div>
                  </div>

                  <div class="form-group">
                     <label for="Nombre">Nombre</label>
                     <div>
                         <input type="text" class="input-sm required form-control" id="txtNombre" autocomplete="off" />                           
                     </div>
                  </div>

                  <div class="form-group">
                     <label for="Orden">Orden</label>
                     <div>
                         <input type="text" class="input-sm required form-control campoNumerico" id="txtOrden" data-m-dec="0" autocomplete="off" />                           
                     </div>
                  </div>                   
                         
                  <div class="form-group">
                      <label class="radio-inline"><input type="radio" name="optradio" id="rbSefiplan"/>Sefiplan</label>
                      <label class="radio-inline"><input type="radio" name="optradio" id="rbDependencia"/>Dependencias</label>                                                      
                  </div>          
                         
            
              </div>
              <div class="modal-footer">
                            
                  <button type="button" class="btn btn-primary" id="btnAdd" style="display:none">Guardar</button>
                  <button type="button" class="btn btn-primary" id="btnUpdate" style="display:none">Guardar</button>                         
                  <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                          
              </div>
            </div><!-- modal-content -->
          </div><!-- modal-dialog -->
        </div> <!-- modal edicion -->

    <div class="modal fade" id="modalBorrar">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header alert alert-success">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title">Confirmar operación</h4>
          </div>
          <div class="modal-body">

              <div class="alert alert-danger" id="divErroresDelete" style="display:none">
                  <ul></ul>
              </div>

              ¿Desea borrar realmente este registro?
            
          </div>
          <div class="modal-footer">
                            
              <button type="button" class="btn btn-primary" id="btnDelete">Borrar</button>                                
              <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                          
          </div>
        </div><!-- modal-content -->
      </div><!-- modal-dialog -->
    </div>  <!-- modal borrar -->
    
    <input type="hidden" id="rowId" />

</asp:Content>
