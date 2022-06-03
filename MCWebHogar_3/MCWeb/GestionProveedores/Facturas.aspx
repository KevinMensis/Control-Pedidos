<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Facturas.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.GestionProveedores.Facturas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Facturas</title>
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

        function abrirModalVerProductos() {
            document.getElementById('BTN_ModalVerProductos').click()
        }

        function cerrarModalVerProductos() {
            document.getElementById('BTN_ModalVerProductos').click()
        }

        function seleccionarReceptor(receptor) {
            if (receptor === "MiKFe") {
                __doPostBack('Identificacion;3101485961')
            } else if (receptor === "Esteban") {
                __doPostBack('Identificacion;115210651')
            }
        }

        function cargarFiltros() {
            $(<%= LB_Emisores.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Proveedor' })
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
                    <li id="li_MiKFe" runat="server">
                        <a href="#" onclick="seleccionarReceptor('MiKFe');">
                            <i class="fas fa-cart-plus"></i>
                            <p>Proveedores - Mi K Fe</p>
                        </a>
                    </li>
                    <li id="li_Esteban" runat="server">
                        <a href="#" onclick="seleccionarReceptor('Esteban');">
                            <i class="fas fa-cart-plus"></i>
                            <p>Proveedores - Esteban</p>
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
                            <h1 class="h3 mb-2 text-gray-800" runat="server" id="H1_Title">Facturas</h1>
                        </div>
                        <div class="input-group no-border col-md-6" style="text-align: right; display: inline-block;">
                            <asp:LinkButton ID="BTN_Sincronizar" runat="server" CssClass="btn btn-secundary" OnClick="BTN_Sincronizar_Click" OnClientClick="activarloading();">
                                <i class="fas fa-sync"></i> Sincronizar
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div class="card shadow mb-4">
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel_FiltrosFacturas" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row" style="height: 50px;">                            
                                        <div class="input-group no-border col-md-3" style="text-align:center; display: block;">
                                            <a href="Proveedores.aspx" class="btn btn-primary">
                                                <i class="fas fa-cart-plus"></i> Proveedores
                                            </a>
                                        </div>       
                                        <div class="input-group no-border col-md-3" style="text-align:center; display: block;">
                                            <a href="Facturas.aspx" class="btn btn-info">
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
                                    <div class="input-group no-border col-md-3">
                                        <asp:TextBox class="form-control" ID="TXT_Buscar" runat="server" placeholder="Buscar..." OnTextChanged="FiltrarFacturas_OnClick" AutoPostBack="true"></asp:TextBox>
                                        <div class="input-group-append">
                                            <div class="input-group-text">
                                                <i class="nc-icon nc-zoom-split"></i>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:ListBox class="form-control" runat="server" ID="LB_Emisores" SelectionMode="Multiple" OnTextChanged="FiltrarFacturas_OnClick" AutoPostBack="true"></asp:ListBox>
                                    </div>
                                    <div class="col-md-2">                                                                                               
                                        <label style="margin-top: 1%;">Fecha desde:</label> 
                                        <asp:TextBox class="form-control" style="flex: auto;" ID="TXT_FechaDesde" runat="server" TextMode="Date" OnTextChanged="Recargar_Click" AutoPostBack="true"></asp:TextBox>                                                
                                    </div>                                                                                                                    
                                    <div class="col-md-2">            
                                        <label style="margin-top: 1%;">Fecha hasta:</label>
                                        <asp:TextBox class="form-control" style="flex: auto;" ID="TXT_FechaHasta" runat="server" TextMode="Date" OnTextChanged="Recargar_Click" AutoPostBack="true"></asp:TextBox>                                                
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel> 
                            <div class="table">
                                <asp:UpdatePanel ID="UpdatePanel_ListaFacturas" runat="server" UpdateMode="Conditional" style="margin-top: 7rem;">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaFacturas" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDFactura,EmisorID,ClaveFactura,NumeroConsecutivoFactura,FechaFactura,FechaSincronizacion,NombreComercial,TotalVenta,TotalDescuento,TotalImpuesto,TotalComprobante" 
                                            HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true" 
                                            EmptyDataText="No hay registros." AllowSorting="true"                                            
                                            OnSorting="DGV_ListaFacturas_Sorting"
                                            OnRowCommand="DGV_ListaFacturas_RowCommand">
                                            <%--OnRowDataBound="DGV_ListaFacturas_OnRowDataBound">--%>
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
                                                <asp:BoundField DataField="NombreComercial" SortExpression="NombreComercial" HeaderText="Proveedor" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
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
                        </div>
                    </div>
                </div>
            </div>
        </div>
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
                            <h5 class="modal-title" runat="server" id="H2">Productos</h5>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel_ListaProductos" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:HiddenField ID="HDF_IDFactura" runat="server" Value="0" />
                                    <div class="row">
                                        <div class="col-md-6">
                                            <label for="TXT_NombreComercial">Nombre proveedor:</label>
                                            <asp:TextBox ID="TXT_NombreComercial" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
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
