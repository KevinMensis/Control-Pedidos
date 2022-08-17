<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ControlPedidos.Productos" %>

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
            return true
        }

        function seleccionarReceptor(receptor) {
            if (receptor === "MiKFe") {
                __doPostBack('Identificacion;3101485961')
            } else if (receptor === "Esteban") {
                __doPostBack('Identificacion;115210651')
            }
        }

        function cargarFiltros() {
            $(<%= LB_Categoria.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Categoria' })            
        }

        $(document).ready(function () {
            cargarFiltros();          
        });

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="modalloading" class="loading">
        <img src="../Assets/img/cargando.gif" width="100" height="100" /><br />
        <asp:Label runat="server" ID="LBL_GenerandoInforme" style="color: white;" Text="Generando informe espere por favor..."></asp:Label>
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
                        <a href="#" onclick="seleccionarReceptor('MiKFe');">
                            <i class="fas fa-cart-plus"></i>
                            <p>Proveedores - Mi K Fe</p>
                        </a>
                    </li>
                    <li>
                        <a href="#" onclick="seleccionarReceptor('Esteban');">
                            <i class="fas fa-cart-plus"></i>
                            <p>Proveedores - Esteban</p>
                        </a>
                    </li>
                    <li>
                        <a href="../GestionCostos/CrearReceta.aspx">
                            <i class="fas fa-chart-line"></i>
                            <p>Gestión costos</p>
                        </a>
                    </li>
                </ul>
                <hr style="width: 230px; color: #2c2c2c;" />
                <%--<h5 style="text-align: center;">Mantenimiento</h5>--%>
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
                            <p>Puntos de venta</p>
                        </a>
                    </li>
                    <li>
                        <a href="PlantasProduccion.aspx">
                            <i class="fas fa-industry"></i>
                            <p>Plantas de producción</p>
                        </a>
                    </li>
                    <li>
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
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel_FiltrosProductos" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>                           
                                    <div class="input-group no-border col-md-3">
                                        <asp:TextBox class="form-control" ID="TXT_Buscar" runat="server" placeholder="Buscar..." OnTextChanged="FiltrarProductos_OnClick" AutoPostBack="true"></asp:TextBox>
                                        <div class="input-group-append">
                                            <div class="input-group-text">
                                                <i class="nc-icon nc-zoom-split"></i>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:ListBox class="form-control" runat="server" ID="LB_Categoria" SelectionMode="Multiple" OnTextChanged="FiltrarProductos_OnClick" AutoPostBack="true"></asp:ListBox>
                                    </div>
                                    <div class="input-group no-border col-md-6" style="text-align: right; display: inline-block;">
                                        <asp:Button ID="BTN_CrearProducto" style="margin: 0px;" runat="server" Text="Crear nuevo producto" CssClass="btn btn-secondary" OnClick="BTN_CrearProducto_OnClick"></asp:Button>
                                        <asp:Button ID="BTN_DescargarProducto" runat="server" UseSubmitBehavior="false" Text="Descargar productos" CssClass="btn btn-info" OnClientClick="activarloading();desactivarloading();" OnClick="BTN_DescargarProducto_OnClick"></asp:Button>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="BTN_DescargarProducto" />
                                </Triggers>
                            </asp:UpdatePanel> 
                            <div class="table">
                                <asp:UpdatePanel ID="UpdatePanel_ListaProductos" runat="server" UpdateMode="Conditional" style="margin-top: 7rem;">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaProductos" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDProducto,Activo,IDCategoria,CodigoBarra,MedidaVenta,MedidaProduccion,UnidadMedida,EsEmpaque,EsInsumo" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_ListaProductos_Sorting"
                                            OnRowCommand="DGV_ListaProductos_RowCommand"
                                            OnRowDataBound="DGV_ListaProductos_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="DescripcionProducto" SortExpression="DescripcionProducto" HeaderText="Descripción" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="black"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionCategoria" SortExpression="DescripcionCategoria" HeaderText="Categoría" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="black"></asp:BoundField>
                                                <asp:BoundField DataField="Costo" SortExpression="Costo" HeaderText="Costo" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:n}" ItemStyle-ForeColor="black"></asp:BoundField>
                                                <asp:BoundField DataField="PrecioVenta" SortExpression="PrecioVenta" HeaderText="Precio al público" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:n}" ItemStyle-ForeColor="black"></asp:BoundField>
                                                <asp:BoundField DataField="Estado" SortExpression="Estado" HeaderText="Estado" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="black"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Acciones" runat="server" Text="Acciones"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button class="btn btn-outline-success btn-round-mant" ID="BTN_Activar" runat="server"
                                                            CommandName="activar"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            Text="Activar" AutoPostBack="true" />
                                                        <asp:Button class="btn btn-outline-primary btn-round-mant" ID="BTN_Editar" runat="server"
                                                            CommandName="editar"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            Text="Editar" AutoPostBack="true" />
                                                        <asp:Button class="btn btn-outline-danger btn-round-mant" ID="BTN_Eliminar" runat="server"
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
                                    <label for="TXT_PrecioVenta">Precio al público:</label>
                                    <asp:TextBox ID="TXT_PrecioVenta" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
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
                                    <label for="CHK_EsEmpaque"> Producto de Empaque</label>
                                </div>
                                <div class="col-md-4" style="padding-top: 1%;">
                                    <asp:CheckBox ID="CHK_EsInsumo" runat="server" CssClass="form-check-input" />
                                    <label for="CHK_EsInsumo"> Producto Insumo</label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="BTN_CerrarModalCrearProducto" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-primary" />
                            <asp:Button ID="BTN_GuardarProducto" runat="server" Text="Guardar producto" CssClass="btn btn-secondary" OnClientClick="return validarCrearProducto();" OnClick="BTN_GuardarProducto_OnClick" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
