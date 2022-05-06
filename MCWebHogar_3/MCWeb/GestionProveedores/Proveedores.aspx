<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Proveedores.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.GestionProveedores.Proveedores" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Emisores</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <style>
        
    </style>
    <script type="text/javascript">
        var unidadesMedida

        function alertifysuccess(msj) {
            alertify.notify(msj, 'success', 5, null);
            return false
        }

        function alertifyerror(msj) {
            alertify.notify(msj, 'error', 5, null);
            return false
        }

        function alertifywarning(msj) {
            alertify.notify(msj, 'warning', 5, null);
            return false
        }

        function activarloading() {
            document.getElementById('fade2').style.display = 'block';
            document.getElementById('modalloading').style.display = 'block';
        }
       
        function desactivarloading() {
            document.getElementById('fade2').style.display = 'none';
            document.getElementById('modalloading').style.display = 'none';
        }

        function abrirModalEditarEmisor() {
            unidadesMedida = new Map()
            document.getElementById('<%= BTN_GuardarEmisor.ClientID %>').hidden = true
            document.getElementById('BTN_ModalEditarEmisor').click()
        }

        function cerrarModalEditarEmisor() {
            unidadesMedida = new Map()
            document.getElementById('BTN_ModalEditarEmisor').click()
        }

        function abrirModalVerFacturas() {
            document.getElementById('BTN_ModalVerFacturas').click()
        }

        function cerrarModalVerFacturas() {
            document.getElementById('BTN_ModalVerFacturas').click()
        }

        function abrirModalVerProductos() {
            document.getElementById('BTN_ModalVerProductos').click()
        }

        function cerrarModalVerProductos() {
            document.getElementById('BTN_ModalVerProductos').click()
        }

        function volverFacturas() {
            cerrarModalVerProductos()
            abrirModalVerFacturas()
        }

        function TXT_NombreComercial_onKeyUp() {
            document.getElementById('<%= BTN_GuardarEmisor.ClientID %>').hidden = false
        }

        function TXT_CantidadEquivalente_onKeyUp(txtCantidad, e) {
           var values = txtCantidad.id.split('_')
            var index = values.pop() * 1 + 1
            if (e.keyCode === 13) {
                var rows = $(<%= DGV_UnidadesMedida.ClientID %>)[0].rows.length - 1
                var id = ''
                if (index === rows) {
                    id = 'Content_DGV_UnidadesMedida_TXT_CantidadEquivalente_' + 0
                } else {
                    id = 'Content_DGV_UnidadesMedida_TXT_CantidadEquivalente_' + index
                }

                document.getElementById(id).autofocus = true;
                document.getElementById(id).focus();
                document.getElementById(id).select();
            }

            document.getElementById('<%= BTN_GuardarEmisor.ClientID %>').hidden = false
        }

        function TXT_CantidadEquivalente_onChange(txtCantidad) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1
            var id = 'Content_DGV_UnidadesMedida_HDF_IDUnidadMedida_' + index
            var HDF_IDUnidadMedida = document.getElementById(id)
            var idUnidadMedida = HDF_IDUnidadMedida.value
            var cantidadEquivalente = txtCantidad.value * 1

            if (cantidadEquivalente === '' || cantidadEquivalente === 0 || cantidadEquivalente === '0') {
                txtCantidad.value = 1
            } else {
                if (cantidadEquivalente < 0 || cantidadEquivalente > 999) {
                    cantidadEquivalente = 1
                    txtCantidad.value = 1
                }
            }
            if (cantidadEquivalente >= 0 && cantidadEquivalente < 1000) {
                unidadesMedida.set(idUnidadMedida, cantidadEquivalente)
            }

            document.getElementById('<%= BTN_GuardarEmisor.ClientID %>').hidden = false
        }

        function actualizarEmisor() {
            // document.getElementById('Content_LBL_GenerandoInforme').innerText = 'Agregando productos, espere por favor.'
            // activarloading();
            var usuario = document.getElementById('Content_HDF_IDUsuario').value;
            var idEmisor = document.getElementById('Content_HDF_IDEmisor').value;
            var numeroIdentificacion = document.getElementById('<%= TXT_NumeroIdentificacion.ClientID %>').value;
            var nombreComercial = document.getElementById('<%= TXT_NombreComercial.ClientID %>').value;
            var promises = [];
            promises.push(
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Proveedores.aspx/BTN_GuardarEmisor_Click",
                    data: JSON.stringify({
                        "idEmisor": idEmisor,
                        "numeroIdentificacion": numeroIdentificacion,
                        "nombreComercial": nombreComercial,
                        "usuario": usuario
                    }),
                    dataType: "json",
                    success: function (Result) {
                        // console.dir(Result)
                    },
                    error: function (Result) {
                        alert("ERROR " + Result.status + ' ' + Result.statusText);
                    }
                })
            )
            unidadesMedida.forEach(function (valor, clave) {
                promises.push(
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "Proveedores.aspx/BTN_GuardarUnidadesMedida_Click",
                        data: JSON.stringify({
                            "idEmisor": idEmisor,
                            "idUnidadMedida": clave,
                            "cantidadEquivalente": valor,
                            "usuario": usuario
                        }),
                        dataType: "json",
                        success: function (Result) {
                            // console.dir(Result)
                        },
                        error: function (Result) {
                            alert("ERROR " + Result.status + ' ' + Result.statusText);
                        }
                    })
                )
            })
            Promise.all(promises).then(function () {
                cerrarModalEditarEmisor();
                alertifysuccess('Actualizacion exitosa');                    
                 
                // cargarFiltros();
                // desactivarloading();
            });
        }

        function validarEditarEmisor() {
            return true
        }

        function cargarFiltros() {        
        }

        $(document).ready(function () {
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="modalloading" class="loading">
        <asp:UpdatePanel ID="UpdatePanel_Progress" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <img src="../Assets/img/cargando.gif" width="100" height="100" /><br />
                <asp:Label runat="server" ID="LBL_GenerandoInforme" Style="color: white;" Text="Generando informe espere por favor..."></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="fade2" class="overlayload"></div>
    <div class="wrapper ">
        <asp:HiddenField ID="HDF_IDUsuario" runat="server" Value="0" Visible="true" />
        <div class="sidebar" data-color="white" data-active-color="danger">
            <div class="sidebar-wrapper scroll" style="overflow-y: auto;">
                <img style="width: 60%; display: block; margin-left: 30%; margin-top: 3%;" src="../Assets/img/logo.png" />
                <ul class="nav">
                    <li>
                        <a href="../ControlPedidos/Reportes.aspx">
                            <i class="fas fa-fw fa-tachometer-alt"></i>
                            <p>Reportes</p>
                        </a>
                    </li>
                    <li>
                        <a href="../ControlPedidos/Pedido.aspx">
                            <i class="fas fa-shopping-cart"></i>
                            <p>Pedidos</p>
                        </a>
                    </li>
                    <li>
                        <a href="../ControlPedidos/OrdenesProduccion.aspx">
                            <i class="fas fa-sort"></i>
                           <p>Ordenes de producción</p>
                        </a>
                    </li>
                    <li>
                        <a href="../ControlPedidos/Despacho.aspx">
                            <i class="fas fa-truck"></i>
                            <p>Despacho</p>
                        </a>
                    </li>
                    <li>
                        <a href="../ControlPedidos/PedidosRecibidos.aspx">
                            <i class="fas fa-check-double"></i>
                            <p>Pedidos recibidos</p>
                        </a>
                    </li>
                    <li>
                        <a href="../ControlPedidos/Empaque.aspx">
                            <i class="fas fa-box-open"></i>
                            <p>Empaque</p>
                        </a>
                    </li>
                    <li>
                        <a href="../ControlPedidos/Devoluciones.aspx">
                            <i class="fas fa-undo-alt"></i>
                            <p>Devoluciones</p>
                        </a>
                    </li>
                    <li>
                        <a href="../ControlPedidos/Desechos.aspx">
                            <i class="fas fa-trash"></i>
                            <p>Desechos</p>
                        </a>
                    </li>
                    <li>
                        <a href="../ControlPedidos/Insumos.aspx">
                            <i class="fas fa-box"></i>
                            <p>Insumos</p>
                        </a>
                    </li>
                    <li class="active">
                        <a href="Proveedores.aspx">
                            <i class="fas fa-cart-plus"></i>
                            <p>Proveedores</p>
                        </a>
                    </li>
                </ul>
                <hr style="width: 230px; color: #2c2c2c;" />
                <h5 style="text-align: center;">Mantenimiento</h5>
                <ul class="nav">
                    <li>
                        <a href="../ControlPedidos/Productos.aspx">
                            <i class="fas fa-coffee"></i>
                            <p>Productos</p>
                        </a>
                    </li>
                    <li>
                        <a href="../ControlPedidos/PuntosVenta.aspx">
                            <i class="fas fa-building"></i>
                            <p>Puntos de venta</p>
                        </a>
                    </li>
                    <li>
                        <a href="../ControlPedidos/PlantasProduccion.aspx">
                            <i class="fas fa-industry"></i>
                            <p>Plantas de producción</p>
                        </a>
                    </li>
                    <li>
                        <a href="../ControlPedidos/GestionUsuarios.aspx">
                            <i class="fas fa-user"></i>
                            <p>Gestión de usuarios</p>
                        </a>
                    </li>
                    <hr style="width: 230px; color: #2c2c2c;" />
                    <li>
                        <asp:LinkButton ID="LNK_CerrarSession" runat="server" OnClick="LNK_CerrarSesion_Cick">
                            <i class="fas fa-sign-out-alt"></i>
                            <p>Cerrar sessión</p>
                        </asp:LinkButton>
                        <a href="https://mensis.cr/" target="_blank" style="margin-top: 0px !important;">
                            <p style="margin-left: 29%; font-size: 7px;">Desarrollado por</p>
                            <img style="width: 75%; display: block; margin-left: 10%;" src="https://mensis.cr/svg/logos/logoMensis.jpg" />
                        </a>
                    </li>
                </ul>
            </div>
        </div>
        <div class="main-panel scroll" style="background: #f4f3ef;">
            <div class="content">
                <div class="container-fluid">
                    <!-- Page Heading -->
                    <div class="row">
                        <div class="input-group no-border col-md-6" style="text-align: left; display: inline-block;">
                            <h1 class="h3 mb-2 text-gray-800">Emisores</h1>
                        </div>
                        <div class="input-group no-border col-md-6" style="text-align: right; display: inline-block;">
                            <asp:LinkButton ID="BTN_Sincronizar" runat="server" CssClass="btn btn-secundary" OnClick="BTN_Sincronizar_Click" OnClientClick="activarloading();">
                                <i class="fas fa-sync"></i> Sincronizar
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div class="card shadow mb-4">
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel_FiltrosEmisores" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row" style="height: 50px;">   
                                        <div class="input-group no-border col-md-3" style="text-align:center; display: block;">
                                            <a href="Proveedores.aspx" class="btn btn-info">
                                                <i class="fas fa-cart-plus"></i> Emisores
                                            </a>
                                        </div>       
                                        <div class="input-group no-border col-md-3" style="text-align:center; display: block;">
                                            <a href="Facturas.aspx" class="btn btn-primary">
                                                <i class="fas fa-file-invoice"></i> Facturas
                                            </a>
                                        </div>       
                                        <div class="input-group no-border col-md-3" style="text-align:center; display: block;">
                                            <a href="Productos.aspx" class="btn btn-primary">
                                                <i class="fas fa-barcode"></i> Productos
                                            </a>
                                        </div>   
                                        <div class="input-group no-border col-md-3" style="text-align:center; display: block;">
                                            <a href="Reportes.aspx" class="btn btn-primary">
                                                <i class="fas fa-fw fa-tachometer-alt"></i> Reportes
                                            </a>
                                        </div>    
                                    </div>
                                    <hr />                   
                                    <div class="input-group no-border col-md-6">
                                        <asp:TextBox class="form-control" ID="TXT_Buscar" runat="server" placeholder="Buscar..." OnTextChanged="FiltrarEmisores_OnClick" AutoPostBack="true"></asp:TextBox>
                                        <div class="input-group-append">
                                            <div class="input-group-text">
                                                <i class="nc-icon nc-zoom-split"></i>
                                            </div>
                                        </div>
                                    </div>                                    
                                    <div class="input-group no-border col-md-6" style="text-align: right; display: inline-block;">
                                        <asp:Button ID="BTN_DescargarEmisores" runat="server" UseSubmitBehavior="false" Text="Descargar emisores" CssClass="btn btn-info" OnClientClick="activarloading();desactivarloading();" OnClick="BTN_DescargarEmisores_OnClick" AutoPostBack="true"></asp:Button>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="BTN_DescargarEmisores" />
                                </Triggers>
                            </asp:UpdatePanel> 
                            <div class="table">
                               <asp:UpdatePanel ID="UpdatePanel_ListaEmisores" runat="server" UpdateMode="Conditional" style="margin-top: 7rem;">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaEmisores" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDEmisor,Activo,NumeroIdentificacion,TipoIdentificacion,Nombre,NombreComercial" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"                                            
                                            OnSorting="DGV_ListaEmisores_Sorting"
                                            OnRowDataBound="DGV_ListaEmisores_OnRowDataBound"
                                            OnRowCommand="DGV_ListaEmisores_RowCommand">
                                            <Columns>      
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Ver" runat="server" Text="Facturas"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton class="btn btn-outline-info btn-round-mant" ID="BTN_VerFacturas" runat="server"
                                                            CommandName="VerFacturas"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            AutoPostBack="true" > 
                                                        <i class="fas fa-eye"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>                                         
                                                <asp:BoundField DataField="NombreComercial" SortExpression="NombreComercial" HeaderText="Nombre" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                                <asp:BoundField DataField="NumeroIdentificacion" SortExpression="NumeroIdentificacion" HeaderText="Identificacion" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                                <asp:BoundField DataField="CorreoEmisor" SortExpression="CorreoEmisor" HeaderText="Correo" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="Telefono" SortExpression="Telefono" HeaderText="Teléfono" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Acciones" runat="server" Text="Acciones"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button UseSubmitBehavior="false" class="btn btn-outline-success btn-round-mant" ID="BTN_Activar" runat="server"
                                                            CommandName="activar"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            Text="Activar" AutoPostBack="true" /> 
                                                        <asp:Button UseSubmitBehavior="false" class="btn btn-outline-primary btn-round-mant" ID="BTN_Editar" runat="server"
                                                            CommandName="editar"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            Text="Editar" AutoPostBack="true" />                                                      
                                                        <asp:Button UseSubmitBehavior="false" class="btn btn-outline-danger btn-round-mant" ID="BTN_Eliminar" runat="server"
                                                            CommandName="desactivar"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            Text="Desactivar" AutoPostBack="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <button type="button" id="BTN_ModalEditarEmisor" data-toggle="modal" data-target="#ModalEditarEmisor" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalEditarEmisor" tabindex="-1" role="dialog" aria-labelledby="popEditarEmisor" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalEditarEmisor" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        
                        <div class="modal-body">
                            <asp:HiddenField ID="HDF_IDEmisor" runat="server" Value="0" />
                            <div class="row">
                                <div class="col-md-6">
                                    <label for="TXT_NombreComercial">Nombre comercial:</label>
                                    <asp:TextBox ID="TXT_NombreComercial" runat="server" CssClass="form-control" onkeyup="TXT_NombreComercial_onKeyUp()"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label for="TXT_NumeroIdentificacion">Identificacion:</label>
                                    <asp:TextBox ID="TXT_NumeroIdentificacion" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label for="TXT_CorreoEmisor">Correo emisor:</label>
                                    <asp:TextBox ID="TXT_CorreoEmisor" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-3">
                                    <label for="TXT_Provincia">Provincia:</label>
                                    <asp:TextBox ID="TXT_Provincia" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label for="TXT_Canton">Canton:</label>
                                    <asp:TextBox ID="TXT_Canton" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label for="TXT_Distrito">Distrito:</label>
                                    <asp:TextBox ID="TXT_Distrito" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label for="TXT_Barrio">Barrio:</label>
                                    <asp:TextBox ID="TXT_Barrio" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-3">
                                    <label for="TXT_Telefono">Teléfono:</label>
                                    <asp:TextBox ID="TXT_Telefono" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-9">
                                    <label for="TXT_OtrasSenas">Otras señas:</label>
                                    <asp:TextBox ID="TXT_OtrasSenas" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <asp:GridView ID="DGV_UnidadesMedida" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                AutoGenerateColumns="False" DataKeyNames="IDUnidadMedida,EmisorID,UnidadMedida,CantidadEquivalente" HeaderStyle-CssClass="table" 
                                BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true">
                                <%--OnSorting="DGV_UnidadesMedida_Sorting"
                                    OnRowDataBound="DGV_UnidadesMedida_OnRowDataBound"
                                    OnRowCommand="DGV_UnidadesMedida_RowCommand">--%>
                                <Columns>                                               
                                    <asp:BoundField DataField="UnidadMedida" SortExpression="UnidadMedida" HeaderText="Unidad medida" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="LBL_Cantidad" runat="server" Text="Cantidad equivalente"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="HDF_IDUnidadMedida" runat="server" Value='<%# Eval("IDUnidadMedida") %>' />
                                            <asp:TextBox class="form-control" TextMode="Number" MaxLength="0" min="0" max="999" style="width: 100%" runat="server" ID="TXT_CantidadEquivalente" 
                                                onkeyup="TXT_CantidadEquivalente_onKeyUp(this,event);" onchange="TXT_CantidadEquivalente_onChange(this)" Text='<%#Eval("CantidadEquivalente") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="20px" />                                          
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="BTN_CerrarModalEditarEmisor" runat="server" UseSubmitBehavior="false" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-primary" />
                            <asp:Button ID="BTN_GuardarEmisor" runat="server" UseSubmitBehavior="false" Text="Guardar" CssClass="btn btn-secondary" OnClientClick="actualizarEmisor();" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <button type="button" id="BTN_ModalVerFacturas" data-toggle="modal" data-target="#ModalVerFacturas" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalVerFacturas" tabindex="-1" role="dialog" aria-labelledby="popVerFacturas" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_VerFacturas" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" style="max-width: 1000px;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <div style="display: inline-block; width: 75%;">
                                <h5 class="modal-title" runat="server" id="H2">Facturas</h5>
                                <h6 class="modal-title" runat="server" id="H1"></h6>
                            </div>
                            <div style="display: inline-block;">
                                <asp:Button ID="BTN_DescargarEmisor" runat="server" UseSubmitBehavior="false" Text="Descargar reporte" CssClass="btn btn-info" OnClientClick="activarloading();desactivarloading();" OnClick="BTN_DescargarEmisor_OnClick" AutoPostBack="true"></asp:Button>
                            </div>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel_ListaFacturas" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="DGV_ListaFacturas" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        AutoGenerateColumns="False" DataKeyNames="IDFactura,ClaveFactura,NumeroConsecutivoFactura,FechaFactura,FechaSincronizacion,NombreComercial,TotalVenta,TotalDescuento,TotalImpuesto,TotalComprobante" 
                                        HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true" 
                                        EmptyDataText="No hay registros." AllowSorting="true" 
                                        OnRowCommand="DGV_ListaFacturas_RowCommand">
                                        <%--OnSorting="DGV_ListaFacturas_Sorting"
                                            OnRowDataBound="DGV_ListaFacturas_OnRowDataBound">--%>
                                        <Columns>  
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="LBL_Ver" runat="server" Text="Productos"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton class="btn btn-outline-info btn-round-mant" ID="BTN_VerProductos" runat="server"
                                                        CommandName="VerProductos"
                                                        CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                        AutoPostBack="true" > 
                                                    <i class="fas fa-eye"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>                                            
                                            <asp:BoundField DataField="NumeroConsecutivoFactura" SortExpression="NumeroConsecutivoFactura" HeaderText="Número factura" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                            <asp:BoundField DataField="FechaFactura" SortExpression="FechaFactura" HeaderText="Fecha factura" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="FechaSincronizacion" SortExpression="FechaSincronizacion" HeaderText="Fecha sincronizacion" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="TotalVenta" SortExpression="TotalVenta" HeaderText="Total venta" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="TotalDescuento" SortExpression="TotalDescuento" HeaderText="Total descuento" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="TotalImpuesto" SortExpression="TotalImpuesto" HeaderText="Total impuesto" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="TotalComprobante" SortExpression="TotalComprobante" HeaderText="Total comprobante" ItemStyle-ForeColor="black" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                                                                                                                                               
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="BTN_CerrarModalVerFacturas" runat="server" UseSubmitBehavior="false" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-primary" />                            
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="BTN_DescargarEmisor" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <button type="button" id="BTN_ModalVerProductos" data-toggle="modal" data-target="#ModalVerProductos" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalVerProductos" tabindex="-1" role="dialog" aria-labelledby="popVerProductos" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_VerProductos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" style="max-width: 1400px;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <div style="display: inline-block; width: 83%;">
                                <h5 class="modal-title" runat="server" id="H3">Productos</h5>
                            </div>
                            <div style="display: inline-block;">
                                <asp:Button ID="BTN_VolverFacturas" runat="server" UseSubmitBehavior="false" Text="Volver" CssClass="btn btn-secondary" OnClientClick="return volverFacturas();" />
                            </div>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel_ListaProductos" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:HiddenField ID="HDF_IDFactura" runat="server" Value="0" />
                                    <div class="row">
                                        <div class="col-md-6">
                                            <label for="TXT_NombreComercialFactura">Nombre emisor:</label>
                                            <asp:TextBox ID="TXT_NombreComercialFactura" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <label for="TXT_NumeroConsecutivo">Número factura:</label>
                                            <asp:TextBox ID="TXT_NumeroConsecutivo" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <br />
                                     <div class="row">
                                        <div class="col-md-2">
                                            <label for="TXT_FechaFactura">Fecha factura:</label>
                                            <asp:TextBox ID="TXT_FechaFactura" runat="server" style="text-align: right;" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label for="TXT_FechaSincronizacion">Fecha sincronizacion:</label>
                                            <asp:TextBox ID="TXT_FechaSincronizacion" runat="server" style="text-align: right;" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label for="TXT_TotalVenta">Total venta:</label>
                                            <asp:TextBox ID="TXT_TotalVenta" runat="server" style="text-align: right;" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label for="TXT_TotalDescuento">Total descuento:</label>
                                            <asp:TextBox ID="TXT_TotalDescuento" runat="server" style="text-align: right;" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label for="TXT_TotalImpuesto">Total impuesto:</label>
                                            <asp:TextBox ID="TXT_TotalImpuesto" runat="server" style="text-align: right;" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label for="TXT_TotalComprobante">Total comprobante:</label>
                                            <asp:TextBox ID="TXT_TotalComprobante" runat="server" style="text-align: right;" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <br />
                                    <asp:GridView ID="DGV_ListaProductos" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        AutoGenerateColumns="False" DataKeyNames="IDLineaDetalle,ProductoID,FacturaID,CodigoProducto,DetalleProducto,UnidadMedida" HeaderStyle-CssClass="table" 
                                        BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true">
                                        <%--OnSorting="DGV_ListaFacturas_Sorting"
                                            OnRowDataBound="DGV_ListaFacturas_OnRowDataBound"
                                            OnRowCommand="DGV_ListaFacturas_RowCommand">--%>
                                        <Columns>                                               
                                            <asp:BoundField DataField="CodigoProducto" SortExpression="CodigoProducto" HeaderText="Código" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                            <asp:BoundField DataField="DetalleProducto" SortExpression="DetalleProducto" HeaderText="Detalle" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="UnidadMedida" SortExpression="UnidadMedida" HeaderText="Unidad medida" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="Cantidad" SortExpression="Cantidad" HeaderText="Cantidad" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="PrecioUnitarioFinal" SortExpression="PrecioUnitarioFinal" HeaderText="Precio unitario" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="MontoDescuento" SortExpression="MontoDescuento" HeaderText="Descuento" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="PorcentajeImpuesto" SortExpression="PorcentajeImpuesto" HeaderText="Impuesto" ItemStyle-ForeColor="black" DataFormatString="{0:n0}%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                                                                                                                                               
                                            <asp:BoundField DataField="MontoTotalIVA" SortExpression="MontoTotalIVA" HeaderText="Monto total" ItemStyle-ForeColor="black" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                                                                                                                                               
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="BTN_CerrarModalVerProductos" runat="server" UseSubmitBehavior="false" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-primary" />                            
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
