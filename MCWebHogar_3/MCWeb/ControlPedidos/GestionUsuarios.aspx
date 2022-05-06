<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionUsuarios.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ControlPedidos.GestionUsuarios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Gestion Usuarios</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <style>
        [id*=DGV_PermisosSinAsignar] tr:hover, [id*=DGV_PermisosAsignados] tr:hover {
          background-color: #51cbce;
        }
    </style>
    <script type="text/javascript">
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

        function abrirModalCrearUsuario() {
            document.getElementById('BTN_ModalCrearUsuario').click()
        }

        function cerrarModalCrearUsuario() {
            document.getElementById('BTN_ModalCrearUsuario').click()
        }

        function abrirModalPermisosUsuario() {
            document.getElementById('BTN_ModalPermisosUsuario').click()
        }

        function cerrarModalPermisosUsuario() {
            document.getElementById('BTN_ModalPermisosUsuario').click()
        }

        function validarCrearUsuario() {

        }

        function cargarFiltros() {
            $(<%= LB_Rol.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Rol' })
        }

        $(document).ready(function () {
            cargarFiltros();
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="modalloading" class="loading">
        <img src="../Assets/img/cargando.gif" width="100" height="100" /><br />
        <asp:Label runat="server" ID="LBL_GenerandoInforme" Style="color: white;" Text="Generando informe espere por favor..."></asp:Label>
    </div>
    <div id="fade2" class="overlayload"></div>
    <div class="wrapper ">
        <div class="sidebar" data-color="white" data-active-color="danger">
            <div class="sidebar-wrapper scroll" style="overflow-y: auto;">
                <img style="width: 60%; display: block; margin-left: 30%; margin-top: 3%;" src="../Assets/img/logo.png" />
                <ul class="nav">
                    <li>
                        <a href="Reportes.aspx">
                            <i class="fas fa-fw fa-tachometer-alt"></i>
                            <p>Reportes</p>
                        </a>
                    </li>
                    <li>
                        <a href="Pedido.aspx">
                            <i class="fas fa-shopping-cart"></i>
                            <p>Pedidos</p>
                        </a>
                    </li>
                    <li>
                        <a href="OrdenesProduccion.aspx">
                            <i class="fas fa-sort"></i>
                            <p>Ordenes de producción</p>
                        </a>
                    </li>
                    <li>
                        <a href="Despacho.aspx">
                            <i class="fas fa-truck"></i>
                            <p>Despacho</p>
                        </a>
                    </li>
                    <li>
                        <a href="PedidosRecibidos.aspx">
                            <i class="fas fa-check-double"></i>
                            <p>Pedidos recibidos</p>
                        </a>
                    </li>
                    <li>
                        <a href="Empaque.aspx">
                            <i class="fas fa-box-open"></i>
                            <p>Empaque</p>
                        </a>
                    </li>
                    <li>
                        <a href="Devoluciones.aspx">
                            <i class="fas fa-undo-alt"></i>
                            <p>Devoluciones</p>
                        </a>
                    </li>
                    <li>
                        <a href="Desechos.aspx">
                            <i class="fas fa-trash"></i>
                            <p>Desechos</p>
                        </a>
                    </li>
                    <li>
                        <a href="Insumos.aspx">
                            <i class="fas fa-box"></i>
                            <p>Insumos</p>
                        </a>
                    </li>
                    <li>
                        <a href="../GestionProveedores/Proveedores.aspx">
                            <i class="fas fa-cart-plus"></i>
                            <p>Proveedores</p>
                        </a>
                    </li>
                </ul>
                <hr style="width: 230px; color: #2c2c2c;" />
                <h5 style="text-align: center;">Mantenimiento</h5>
                <ul class="nav">
                    <li>
                        <a href="Productos.aspx">
                            <i class="fas fa-coffee"></i>
                            <p>Productos</p>
                        </a>
                    </li>
                    <li>
                        <a href="PuntosVenta.aspx">
                            <i class="fas fa-building"></i>
                            <p>Puntos de venta</p>
                        </a>
                    </li>
                    <li>
                        <a href="PlantasProduccion.aspx">
                            <i class="fas fa-industry"></i>
                            <p>Plantas de producción</p>
                        </a>
                    </li>
                    <li class="active">
                        <a href="GestionUsuarios.aspx">
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
                    <h1 class="h3 mb-2 text-gray-800">Mantenimiento usuarios</h1>
                    <br />
                    <div class="card shadow mb-4">
                        <asp:UpdatePanel ID="UpdatePanel_CrearUsuario" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="card-header py-3" style="text-align: right;">
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel_FiltrosUsuarios" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="input-group no-border col-md-3">
                                        <asp:TextBox class="form-control" ID="TXT_Buscar" runat="server" placeholder="Buscar..." OnTextChanged="FiltrarUsuarios_OnClick" AutoPostBack="true"></asp:TextBox>
                                        <div class="input-group-append">
                                            <div class="input-group-text">
                                                <i class="nc-icon nc-zoom-split"></i>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:ListBox class="form-control" runat="server" ID="LB_Rol" SelectionMode="Multiple" OnTextChanged="FiltrarUsuarios_OnClick" AutoPostBack="true"></asp:ListBox>
                                    </div>
                                    <div class="input-group no-border col-md-6" style="text-align: right; display: inline-block;">
                                        <asp:Button UseSubmitBehavior="false" ID="BTN_CrearUsuario" Style="margin: 0px;" runat="server" Text="Crear nuevo usuario" CssClass="btn btn-secondary" OnClick="BTN_CrearUsuario_OnClick"></asp:Button>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="table">
                                <asp:UpdatePanel ID="UpdatePanel_ListaUsuarios" runat="server" UpdateMode="Conditional" style="margin-top: 7rem;">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaUsuarios" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDUsuario,Activo,Usuario,RolID,Cargo,Nombre" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_ListaUsuarios_Sorting"
                                            OnRowCommand="DGV_ListaUsuarios_RowCommand"
                                            OnRowDataBound="DGV_ListaUsuarios_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="Nombre" SortExpression="Nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="black"></asp:BoundField>
                                                <asp:BoundField DataField="Usuario" SortExpression="Usuario" HeaderText="Usuario" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="black"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionRol" SortExpression="DescripcionRol" HeaderText="Rol" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="black"></asp:BoundField>
                                                <asp:BoundField DataField="Cargo" SortExpression="Cargo" HeaderText="Cargo" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="black"></asp:BoundField>
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
                                                        <asp:Button UseSubmitBehavior="false" class="btn btn-outline-info btn-round-mant" ID="BTN_Permisos" runat="server"
                                                            CommandName="permisos"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            Text="Permisos" AutoPostBack="true" />
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

    <button type="button" id="BTN_ModalCrearUsuario" data-toggle="modal" data-target="#ModalCrearUsuario" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalCrearUsuario" tabindex="-1" role="dialog" aria-labelledby="popCrearUsuario" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalCrearUsuario" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server" id="title_CrearPedido"></h5>
                        </div>
                        <div class="modal-body">
                            <asp:HiddenField ID="HDF_IDUsuario" runat="server" Value="0" />
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="TXT_NombreUsuario">Nombre del usuario:</label>
                                    <asp:TextBox ID="TXT_NombreUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="TXT_Usuario">Usuario:</label>
                                    <asp:TextBox ID="TXT_Usuario" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="DDL_Rol">Rol</label>
                                    <asp:DropDownList class="form-control" ID="DDL_Rol" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="TXT_Cargo">Cargo: </label>
                                    <asp:TextBox ID="TXT_Cargo" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="TXT_Contrasena">Contraseña:</label>
                                    <asp:TextBox ID="TXT_Contrasena" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="TXT_ConfirmarContrasena">Confirmar contraseña:</label>
                                    <asp:TextBox ID="TXT_ConfirmarContrasena" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button UseSubmitBehavior="false" ID="BTN_CerrarModalCrearUsuario" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-primary" />
                            <asp:Button UseSubmitBehavior="false" ID="BTN_GuardarUsuario" runat="server" Text="Guardar usuario" CssClass="btn btn-secondary" OnClientClick="return validarCrearUsuario();" OnClick="BTN_GuardarUsuario_OnClick" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <button type="button" id="BTN_ModalPermisosUsuario" data-toggle="modal" data-target="#ModalPermisosUsuario" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalPermisosUsuario" tabindex="-1" role="dialog" aria-labelledby="popModalPermisosUsuario" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalPermisosUsuario" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" style="max-width: 1200px;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server" id="title_Permisos"></h5>
                        </div>
                        <div class="modal-body">
                            <asp:HiddenField ID="HDF_IDUsuarioPermisos" runat="server" Value="0" />
                            <div class="row">
                                <div class="col-md-2" style="text-align: end;">
                                </div>
                                <div class="col-md-2" style="text-align: end;">
                                    <label for="DDL_Modulo">Permiso:</label>
                                </div>
                                <div class="col-md-4" style="padding-left: 0px;">
                                    <asp:DropDownList ID="DDL_Modulo" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDL_Modulo_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-4" style="text-align: end;">
                                </div>
                            </div>
                            <br />
                            <asp:UpdatePanel ID="UpdatePanel_TablaPermisos" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row table-responsive" style="text-align: center;">
                                        <div class="col-md-6">
                                            <label for="DGV_PermisosSinAsignar">Permisos sin asignar</label>
                                            <asp:GridView ID="DGV_PermisosSinAsignar" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                AutoGenerateColumns="False" DataKeyNames="IDPermiso" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                                OnSorting="DGV_PermisosSinAsignar_Sorting"
                                                OnRowCommand="DGV_PermisosSinAsignar_RowCommand"
                                                OnRowDataBound="DGV_PermisosSinAsignar_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="Formulario" SortExpression="Formulario" HeaderText="Módulo" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="black"></asp:BoundField>
                                                    <asp:BoundField DataField="Detalle" SortExpression="Detalle" HeaderText="Detalle" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="black"></asp:BoundField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="LBL_Acciones" runat="server" Text="Agregar permiso"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton UseSubmitBehavior="false" class="btn btn-outline-success btn-round-mant" ID="BTN_Asignar" runat="server"
                                                                CommandName="asignar"
                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                Text="" AutoPostBack="true"><i class="fas fa-angle-right"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div class="col-md-6">
                                            <label for="DGV_PermisosAsignados">Permisos asignados</label>
                                            <asp:GridView ID="DGV_PermisosAsignados" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                AutoGenerateColumns="False" DataKeyNames="IDPermiso,IDLinea,IDUsuario" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                                OnSorting="DGV_PermisosAsignados_Sorting"
                                                OnRowCommand="DGV_PermisosAsignados_RowCommand"
                                                OnRowDataBound="DGV_PermisosAsignados_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="LBL_Acciones" runat="server" Text="Eliminar permiso"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton UseSubmitBehavior="false" class="btn btn-outline-danger btn-round-mant" ID="BTN_Eliminar" runat="server"
                                                                CommandName="eliminar"
                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                Text="" AutoPostBack="true"><i class="fas fa-angle-left"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Formulario" SortExpression="Formulario" HeaderText="Módulo" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="black"></asp:BoundField>
                                                    <asp:BoundField DataField="Detalle" SortExpression="Detalle" HeaderText="Detalle" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="black"></asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button UseSubmitBehavior="false" ID="BTN_CerrarModalPermisosUsuario" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
