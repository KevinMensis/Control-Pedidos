﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetallePedido.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ControlPedidos.DetallePedido" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Detalle pedido</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <style>
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

        function abrirModalAgregarProductos() {
            document.getElementById('BTN_ModalAgregarProductos').click()
        }

        function cerrarModalAgregarProductos() {
            document.getElementById('BTN_ModalAgregarProductos').click()
        }

        function abrirModalConfirmarPedido() {
            document.getElementById('BTN_ModalConfirmacionPedido').click()
        }

        function cerrarModalConfirmarPedido() {
            document.getElementById('BTN_ModalConfirmacionPedido').click()
        }

        function estilosElementosBloqueados() {
            document.getElementById('<%= TXT_CodigoPedido.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_CodigoPedido.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_TotalProductos.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_TotalProductos.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_MontoPedido.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_MontoPedido.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_EstadoPedido.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_EstadoPedido.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_FechaPedido.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_FechaPedido.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_HoraPedido.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_HoraPedido.ClientID %>').classList.add('form-control')
        }

        $(document).ready(function () {
            estilosElementosBloqueados()
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="modalloading" class="loading">
        <img src="../images/cargando5.gif" width="100" height="100" />
    </div>
    <div id="fade2" class="overlayload"></div>
    <div class="wrapper">
        <asp:HiddenField ID="HDF_IDPedido" runat="server" Value="0" Visible="false" />
        <asp:HiddenField ID="HDF_EstadoPedido" runat="server" Value="" Visible="false" />
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
                    <li class="active">
                        <a href="Pedido.aspx">
                            <i class="fas fa-shopping-cart"></i>
                            <p>Pedidos</p>
                        </a>
                    </li>
                    <li>
                        <a href="OrdenesProduccion.aspx">
                            <i class="fas fa-sort"></i>
                            <p>Ordenes de Producción</p>
                        </a>
                    </li>
                    <li>
                        <a href="Empaque.aspx">
                            <i class="fas fa-box-open"></i>
                            <p>Empaque</p>
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
                            <p>Puntos de Venta</p>
                        </a>
                    </li>
                    <li>
                        <a href="PlantasProduccion.aspx">
                            <i class="fas fa-industry"></i>
                            <p>Plantas de Producción</p>
                        </a>
                    </li>
                    <li>
                        <a href="GestionUsuarios.aspx">
                            <i class="fas fa-user"></i>
                            <p>GESTIÓN DE USUARIOS</p>
                        </a>
                    </li>
                    <hr style="width: 230px; color: #2c2c2c;" />
                    <li>
                        <asp:LinkButton ID="LNK_CerrarSession" runat="server" OnClick="LNK_CerrarSesion_Cick">
                            <i class="fas fa-sign-out-alt"></i>
                            <p>Cerrar sessión</p>
                        </asp:LinkButton>
                        <a href="http://mensis.cr/" target="_blank">
                            <p style="margin-left: 25%; font-size: 7px;">Desarrollado por</p>
                            <img style="width: 25%; display: block; margin-left: 30%; margin-top: 3%;" src="../Assets/img/logoMensis.png" />
                        </a>
                    </li>
                </ul>
            </div>
        </div>
        <div class="main-panel scroll" style="background: #f4f3ef;">
            <div class="content">
                <div class="container-fluid">
                    <!-- Page Heading -->
                    <h1 class="h3 mb-2 text-gray-800">Detalle pedido</h1>
                    <br />
                    <!-- DataTales Example -->
                    <div class="card shadow mb-4">
                        <asp:UpdatePanel ID="UpdatePanel_Header" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="card-header py-3">
                                    <div class="form-row">
                                        <div class="form-group col-md-3">
                                            <label for="TXT_CodigoPedido">Número pedido</label>
                                            <asp:TextBox class="form-control" style="text-align: right;" ID="TXT_CodigoPedido" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="TXT_TotalProductos">Total de productos</label>
                                            <asp:TextBox class="form-control" style="text-align: right;" ID="TXT_TotalProductos" runat="server" TextMode="Number" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="TXT_MontoPedido">Monto del pedido</label>
                                            <asp:TextBox class="form-control" style="text-align: right;" ID="TXT_MontoPedido" runat="server" TextMode="Number" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="TXT_EstadoPedido">Estado del pedido</label>
                                            <asp:TextBox class="form-control" style="text-align: right;" ID="TXT_EstadoPedido" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <label for="TXT_FechaPedido">Fecha pedido</label>
                                            <div class="form-row">
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="TXT_FechaPedido" runat="server" CssClass="form-control" TextMode="Date" format="dd/MM/yyyy" Enabled="false"></asp:TextBox>
                                                </div>
                                                <div class="col-md-5">
                                                    <asp:TextBox ID="TXT_HoraPedido" runat="server" CssClass="form-control" TextMode="Time" format="HH:mm" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-12">
                                            <label for="TXT_DescripcionPedido">Descripcion del pedido:</label>
                                            <asp:TextBox ID="TXT_DescripcionPedido" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-4">
                                            <label for="DDL_Propietario">Solicitante</label>
                                            <asp:DropDownList class="form-control" ID="DDL_Propietario" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label for="DDL_PlantaProduccion">Planta Producción</label>
                                            <asp:DropDownList class="form-control" ID="DDL_PlantaProduccion" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label for="DDL_PuntoVenta">Punto Venta</label>
                                            <asp:DropDownList class="form-control" ID="DDL_PuntoVenta" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-6">
						                    <asp:Label ID="LBL_CreadoPor" runat="server"></asp:Label>
                                        </div>
                                        <div class="form-group col-md-6">
							                <asp:Label ID="LBL_UltimaModificacion" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <asp:Button ID="BTN_AgregarProductos" runat="server" Text="Agregar productos" CssClass="btn btn-secondary" OnClientClick="estilosElementosBloqueados();" OnClick="BTN_CargarProductos_Click"></asp:Button>                                        
                                        <asp:Button ID="BTN_GuardarPedido" runat="server" Text="Guardar pedido" CssClass="btn btn-secondary" OnClientClick="estilosElementosBloqueados();" OnClick="BTN_GuardarPedido_Click"></asp:Button>
                                        <asp:Button ID="BTN_ConfirmarPedido" runat="server" Text="Confirmar pedido" CssClass="btn btn-secondary" OnClick="BTN_ConfirmarPedido_Click"></asp:Button>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="card-body">
                            <div class="card-body">
                                <asp:UpdatePanel ID="UpdatePanel_FiltrosProductos" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>                           
                                        <div class="input-group no-border">
                                            <asp:TextBox class="form-control" ID="TXT_Buscar" runat="server" placeholder="Buscar..." OnTextChanged="TXT_Buscar_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                            <div class="input-group-append">
                                                <div class="input-group-text">
                                                    <i class="nc-icon nc-zoom-split"></i>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="table-responsive">
                                <asp:UpdatePanel ID="UpdatePanel_ListaProductos" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaProductos" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDPedidoDetalle,PedidoID,ProductoID" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_ListaProductos_Sorting"
                                            OnRowCommand="DGV_ListaProductos_RowCommand"
                                            OnRowDataBound="DGV_ListaProductos_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ProductoID" SortExpression="ProductoID" HeaderText="Código producto" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionProducto" SortExpression="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="PrecioProducto" SortExpression="PrecioProducto" HeaderText="Precio unitario" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Cantidad" runat="server" Text="Cantidad"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="row">
                                                            <asp:TextBox class="form-control" TextMode="Number" MaxLength="2" min="0" max="99" style="width: 40%" runat="server" ID="TXT_Cantidad" 
                                                                OnTextChanged="TXT_Cantidad_OnTextChanged" AutoPostBack="true" Text='<%#Eval("CantidadProduccion") %>' />                                                            
                                                            <asp:DropDownList class="form-control" style="width: 30%" runat="server" ID="DDL_Decenas" 
                                                                OnSelectedIndexChanged="DDL_DecenasUnidades_OnSelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Value="0">0</asp:ListItem>
                                                                <asp:ListItem Value="1">1</asp:ListItem>
                                                                <asp:ListItem Value="2">2</asp:ListItem>
                                                                <asp:ListItem Value="3">3</asp:ListItem>
                                                                <asp:ListItem Value="4">4</asp:ListItem>
                                                                <asp:ListItem Value="5">5</asp:ListItem>
                                                                <asp:ListItem Value="6">6</asp:ListItem>
                                                                <asp:ListItem Value="7">7</asp:ListItem>
                                                                <asp:ListItem Value="8">8</asp:ListItem>
                                                                <asp:ListItem Value="9">9</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:DropDownList class="form-control" style="width: 30%" runat="server" ID="DDL_Unidades"
                                                                OnSelectedIndexChanged="DDL_DecenasUnidades_OnSelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Value="0">0</asp:ListItem>
                                                                <asp:ListItem Value="1">1</asp:ListItem>
                                                                <asp:ListItem Value="2">2</asp:ListItem>
                                                                <asp:ListItem Value="3">3</asp:ListItem>
                                                                <asp:ListItem Value="4">4</asp:ListItem>
                                                                <asp:ListItem Value="5">5</asp:ListItem>
                                                                <asp:ListItem Value="6">6</asp:ListItem>
                                                                <asp:ListItem Value="7">7</asp:ListItem>
                                                                <asp:ListItem Value="8">8</asp:ListItem>
                                                                <asp:ListItem Value="9">9</asp:ListItem>
                                                            </asp:DropDownList>                                                            
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Disminuir" runat="server" Text="Disminuir"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button class="btn btn-outline-primary btn-round" ID="BTN_Minus" runat="server"
                                                                CommandName="minus"
                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                Text="-" AutoPostBack="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Aumentar" runat="server" Text="Aumentar"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button class="btn btn-outline-primary btn-round" style="font-size: 10px;" ID="BTN_Plus" runat="server"
                                                                CommandName="plus"
                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                Text="+" AutoPostBack="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Acciones" runat="server" Text="ACCIONES"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button class="btn btn-outline-danger btn-round" ID="BTN_EliminarProducto" runat="server"
                                                            CommandName="Eliminar"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            Text="Eliminar" AutoPostBack="true" />
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

    <button type="button" id="BTN_ModalAgregarProductos" data-toggle="modal" data-target="#ModalAgregarProductos" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalAgregarProductos" tabindex="-1" role="dialog" aria-labelledby="popAgregarProductos" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalAgregarProductos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server">Agregar productos a la orden</h5>
                        </div>
                        <div class="modal-body">                            
                            <div class="table-responsive">
                                <asp:UpdatePanel ID="UpdatePanel_ListaProductosSinAgregar" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaProductosSinAgregar" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDProducto,DescripcionProducto,Categoria" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_ListaProductosSinAsignar_Sorting">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Acciones" runat="server" Text="Seleccionar"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CHK_Prodcuto" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DescripcionProducto" SortExpression="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="PrecioVentaFinal" SortExpression="PrecioVentaFinal" HeaderText="Precio unitario" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionCategoria" SortExpression="DescripcionCategoria" HeaderText="Categoria" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="UnidadMedida" SortExpression="UnidadMedida" HeaderText="Unidad Medida" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="BTN_CerrarModalCrearPedido" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-secondary" />
                            <asp:Button ID="BTN_Agregar" runat="server" Text="Agregar" CssClass="btn btn-success" OnClick="BTN_Agregar_Click" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <button type="button" id="BTN_ModalConfirmacionPedido" data-toggle="modal" data-target="#ModalConfirmacionPedido" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalConfirmacionPedido" tabindex="-1" role="dialog" aria-labelledby="popConfirmacionPedido" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ConfirmacionPedido" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server">Confirmación del pedido</h5>
                        </div>
                        <div class="modal-body">                            
                            <p>La confirmación del pedido no permitirá agregar nuevos productos o editar la información del pedido. ¿Desea confirmar el pedido?</p>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="BTN_CerrarModalConfirmacionPedido" runat="server" Text="Cancelar" data-dismiss="modal" CssClass="btn btn-secondary" />
                            <asp:Button ID="BTN_ConfirmacionPedido" runat="server" Text="Confirmar" CssClass="btn btn-success" OnClick="BTN_ConfirmacionPedido_Click" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>