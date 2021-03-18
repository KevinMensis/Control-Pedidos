﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ControlPedidos.Productos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Productos</title>
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

        function abrirModalCrearProducto() {
            document.getElementById('BTN_ModalCrearProducto').click()
        }

        function cerrarModalCrearProducto() {
            document.getElementById('BTN_ModalCrearProducto').click()
        }

        function validarCrearProducto() {

        }

        function irArriba() {
            $('.ir-arriba').click(function () { $('body,html').animate({ scrollTop: '0px' }, 1000); });
            $(window).scroll(function () {
                if ($(this).scrollTop() > 0) { $('.ir-arriba').slideDown(600); } else { $('.ir-arriba').slideUp(600); }
            });
            $('.ir-abajo').click(function () { $('body,html').animate({ scrollTop: '1000px' }, 1000); });
        }

        function cargarFiltros() {
            $(<%= LB_Categoria.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Categoria' })            
        }

        $(document).ready(function () {
            cargarFiltros();
            irArriba();            
        });

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="modalloading" class="loading">
        <img src="../images/cargando5.gif" width="100" height="100" />
    </div>
    <div id="fade2" class="overlayload"></div>
    <a class="ir-arriba"  javascript:void(0) title="Volver arriba">
        <span class="fa-stack">
            <i class="fa fa-circle fa-stack-2x"></i>
            <i class="fa fa-arrow-up fa-stack-1x fa-inverse"></i>
        </span>
    </a>
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
                    <li class="active">
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
                    <h1 class="h3 mb-2 text-gray-800">Mantenimiento productos</h1>
                    <br />
                    <div class="card shadow mb-4">
                        <asp:UpdatePanel ID="UpdatePanel_CrearProducto" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="card-header py-3" style="text-align: right;">
                                    <asp:Button ID="BTN_CrearProducto" runat="server" Text="Crear nuevo producto" CssClass="btn btn-secondary" OnClick="BTN_CrearProducto_OnClick"></asp:Button>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel_FiltrosProductos" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>                           
                                    <div class="input-group no-border">
                                        <asp:TextBox class="form-control" ID="TXT_Buscar" runat="server" placeholder="Buscar..." OnTextChanged="FiltrarProductos_OnClick" AutoPostBack="true"></asp:TextBox>
                                        <div class="input-group-append">
                                            <div class="input-group-text">
                                                <i class="nc-icon nc-zoom-split"></i>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <asp:ListBox class="form-control" runat="server" ID="LB_Categoria" SelectionMode="Multiple" OnTextChanged="FiltrarProductos_OnClick" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div style="margin-top: 10px; margin-left: 10px;" class="row">
                                        <asp:Label id="LBL_Filtro" runat="server" Text="Filtros: Ninguno;"></asp:Label>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel> 
                            <div class="table-responsive">
                                <asp:UpdatePanel ID="UpdatePanel_ListaProductos" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaProductos" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDProducto,Activo,IDCategoria,CodigoBarra,MedidaVenta,MedidaProduccion,UnidadMedida,EsEmpaque" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_ListaProductos_Sorting"
                                            OnRowCommand="DGV_ListaProductos_RowCommand"
                                            OnRowDataBound="DGV_ListaProductos_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="DescripcionProducto" SortExpression="DescripcionProducto" HeaderText="Descripción" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionCategoria" SortExpression="DescripcionCategoria" HeaderText="Categoría" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="Costo" SortExpression="Costo" HeaderText="Costo" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:n}"></asp:BoundField>
                                                <asp:BoundField DataField="PrecioVenta" SortExpression="PrecioVenta" HeaderText="PrecioVenta" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:n}"></asp:BoundField>
                                                <asp:BoundField DataField="PrecioVentaFinal" SortExpression="PrecioVentaFinal" HeaderText="PrecioVenta" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:n}"></asp:BoundField>
                                                <asp:BoundField DataField="Estado" SortExpression="Estado" HeaderText="Estado" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Acciones" runat="server" Text="ACCIONES"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button class="btn btn-outline-success btn-round" ID="BTN_Activar" runat="server"
                                                            CommandName="activar"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            Text="Activar" AutoPostBack="true" />
                                                        <asp:Button class="btn btn-outline-primary btn-round" ID="BTN_Editar" runat="server"
                                                            CommandName="editar"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            Text="Editar" AutoPostBack="true" />
                                                        <asp:Button class="btn btn-outline-danger btn-round" ID="BTN_Eliminar" runat="server"
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

    <button type="button" id="BTN_ModalCrearProducto" data-toggle="modal" data-target="#ModalCrearProducto" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalCrearProducto" tabindex="-1" role="dialog" aria-labelledby="popCrearProducto" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalCrearProducto" runat="server" UpdateMode="Conditional">
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
                            <asp:HiddenField ID="HDF_IDProducto" runat="server" Value="0" />
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="TXT_DescripcionProducto">Descripcion del producto:</label>
                                    <asp:TextBox ID="TXT_DescripcionProducto" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="DDL_Categoria">Categoría</label>
                                    <asp:DropDownList class="form-control" ID="DDL_Categoria" runat="server"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="TXT_CodigoBarra">Código de barra:</label>
                                    <asp:TextBox ID="TXT_CodigoBarra" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="TXT_Costo">Costo:</label>
                                    <asp:TextBox ID="TXT_Costo" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="TXT_PrecioVenta">Precio de venta:</label>
                                    <asp:TextBox ID="TXT_PrecioVenta" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                </div>
                                <div class="col-md-4">                                    
                                    <label for="TXT_PrecioVentaFinal">Precio de venta final:</label>
                                    <asp:TextBox ID="TXT_PrecioVentaFinal" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="TXT_MedidaVenta">Medida de venta final:</label>
                                    <asp:TextBox ID="TXT_MedidaVenta" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="TXT_MedidaProduccion">Medida de producción:</label>
                                    <asp:TextBox ID="TXT_MedidaProduccion" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="TXT_UnidadMedida">Unidad de medida:</label>
                                    <asp:TextBox ID="TXT_UnidadMedida" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4" style="padding-top: 1%;">
                                    <asp:CheckBox ID="CHK_EsEmpaque" runat="server" CssClass="form-check-input" />
                                    <label for="CHK_EsEmpaque"> Producto de empaque</label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="BTN_CerrarModalCrearProducto" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-secondary" />
                            <asp:Button ID="BTN_GuardarProducto" runat="server" Text="Guardar producto" CssClass="btn btn-success" OnClientClick="return validarCrearProducto();" OnClick="BTN_GuardarProducto_OnClick" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>