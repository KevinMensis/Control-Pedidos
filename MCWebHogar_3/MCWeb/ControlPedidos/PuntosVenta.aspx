<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PuntosVenta.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ControlPedidos.PuntosVenta" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Puntos Venta</title>
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

        function abrirModalCrearPuntoVenta() {
            document.getElementById('BTN_ModalCrearPuntoVenta').click()
        }

        function cerrarModalCrearPuntoVenta() {
            document.getElementById('BTN_ModalCrearPuntoVenta').click()
        }

        function validarCrearPuntoVenta() {
            return true
        }

        $(document).ready(function () {
            
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
                    <li class="active">
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
                    <h1 class="h3 mb-2 text-gray-800">Mantenimiento puntos de venta</h1>
                    <br />
                    <div class="card shadow mb-4">
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel_FiltrosPuntosVenta" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>                           
                                    <div class="input-group no-border col-md-3">
                                        <asp:TextBox class="form-control" ID="TXT_Buscar" runat="server" placeholder="Buscar..." OnTextChanged="TXT_Buscar_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                        <div class="input-group-append">
                                            <div class="input-group-text">
                                                <i class="nc-icon nc-zoom-split"></i>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="input-group no-border col-md-9" style="text-align: right; display: inline-block;">
                                        <asp:Button ID="BTN_CrearPuntoVenta" style="margin: 0px;" runat="server" Text="Crear nuevo punto venta" CssClass="btn btn-secondary" OnClick="BTN_CrearPuntoVenta_OnClick"></asp:Button>                                        
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel> 
                            <div class="table">
                                <asp:UpdatePanel ID="UpdatePanel_ListaPuntosVenta" runat="server" UpdateMode="Conditional" style="margin-top: 7rem;">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaPuntosVenta" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDPuntoVenta,Activo" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_ListaPuntosVenta_Sorting"
                                            OnRowCommand="DGV_ListaPuntosVenta_RowCommand"
                                            OnRowDataBound="DGV_ListaPuntosVenta_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="DescripcionPuntoVenta" SortExpression="DescripcionPuntoVenta" HeaderText="Descripción" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="black"></asp:BoundField>
                                                <asp:BoundField DataField="UbicacionPuntoVenta" SortExpression="UbicacionPuntoVenta" HeaderText="Ubicación" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="black"></asp:BoundField>
                                                <asp:BoundField DataField="PorcentajeDescuento" SortExpression="PorcentajeDescuento" HeaderText="Porcentaje Descuento" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="black"></asp:BoundField>
                                                <asp:BoundField DataField="PedidoRecibido" SortExpression="PedidoRecibido" HeaderText="Crear pedido recibido" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="black"></asp:BoundField>
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

    <button type="button" id="BTN_ModalCrearPuntoVenta" data-toggle="modal" data-target="#ModalCrearPuntoVenta" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalCrearPuntoVenta" tabindex="-1" role="dialog" aria-labelledby="popCrearPuntoVenta" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalCrearPuntoVenta" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server" id="title_CrearPuntoVenta"></h5>
                        </div>
                        <div class="modal-body">
                            <asp:HiddenField ID="HDF_IDPuntoVenta" runat="server" Value="0" />
                            <div class="row">
                                <div class="col-md-12">
                                    <label for="TXT_DescripcionPuntoVenta">Descripcion del punto de venta:</label>
                                    <asp:TextBox ID="TXT_DescripcionPuntoVenta" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <label for="TXT_UbicacionPuntoVenta">Ubicación punto venta:</label>
                                    <asp:TextBox ID="TXT_UbicacionPuntoVenta" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </div>                                
                            </div> 
                            <div class="row">
                                <div class="col-md-6">
                                    <label for="TXT_PorcentajeDescuento">Porcentaje Descuento:</label>
                                    <asp:TextBox ID="TXT_PorcentajeDescuento" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                </div>   
                                <div class="col-md-6" style="padding-top: 1%;">
                                    <asp:CheckBox ID="CHK_CrearPedidoRecibido" runat="server" CssClass="form-check-input" />
                                    <label for="CHK_CrearPedidoRecibido"> Crear pedido recibido</label>
                                </div>                             
                            </div>                                                    
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="BTN_CerrarModalCrearPuntoVenta" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-primary" />
                            <asp:Button ID="BTN_GuardarPuntoVenta" runat="server" Text="Guardar punto venta" CssClass="btn btn-secondary" OnClientClick="return validarCrearPuntoVenta();" OnClick="BTN_GuardarPuntoVenta_OnClick" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
