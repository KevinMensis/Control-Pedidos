<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.GestionProveedores.Reportes" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Reportes</title>
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

        function graficoHistoricoPrecios(dias, datos, producto) {
            Highcharts.chart('containerHistoricoPrecios', {
                chart: {
                    type: 'line'
                },
                title: {
                    text: 'Histórico precios'
                },
                yAxis: {
                    title: {
                        text: ''
                    }
                },
                xAxis: {
                    categories: dias
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function () {
                                    console.log('Fecha: ' + this.category);
                                }
                            }
                        }
                    },
                    column: {
                        dataLabels: {
                            align: 'left',
                            enabled: true,
                            rotation: 270,
                            x: 2,
                            y: -10
                        }
                    }
                },
                series: [{
                    name: producto,
                    data: datos
                }]
            });
        }

        function graficoHistoricoVariacion(dias, datos, producto) {
            Highcharts.chart('containerHistoricoVariacion', {
                chart: {
                    type: 'line'
                },
                title: {
                    text: 'Histórico variación'
                },
                yAxis: {
                    title: {
                        text: ''
                    }
                },
                xAxis: {
                    categories: dias
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true,
                            format: '{point.y:.2f}%'
                        },
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function () {
                                    console.log('Fecha: ' + this.category);
                                }
                            }
                        }
                    },
                    column: {
                        dataLabels: {
                            align: 'left',
                            enabled: true,
                            rotation: 270,
                            x: 2,
                            y: -10
                        }
                    }
                },
                series: [{
                    name: producto,
                    data: datos
                }]
            });
        }

        function cargarGraficos(idUsuario, idProducto) {
            var descripcionProducto = document.getElementById('<%= TXT_DetalleProductoD.ClientID %>').value
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Reportes.aspx/cargarGraficoHistoricoPrecios",
                data: JSON.stringify({
                    "idUsuario": idUsuario,
                    "idProducto": idProducto,
                }),
                dataType: "json",
                success: function (Result) {
                    listaDias = []
                    listaPrecios = []
                    listaDiferencias = []
                    var precio = 0
                    var diferencia = 0

                    for (var i in Result.d) {
                        var dia = Result.d[i].dia

                        if (precio !== Result.d[i].precioUnitario) {
                            if (precio > 0) {
                                diferencia = (Result.d[i].precioUnitario - precio) / precio * 100
                            } else {
                                diferencia = 0
                            }
                            precio = Result.d[i].precioUnitario
                            listaDiferencias.push(diferencia)
                            listaDias.push(dia)
                            listaPrecios.push(precio)
                        }
                    }
                    graficoHistoricoVariacion(listaDias, listaDiferencias, descripcionProducto)
                    graficoHistoricoPrecios(listaDias, listaPrecios, descripcionProducto)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })
        }

        function abrirModalVerProductos() {
            document.getElementById('BTN_ModalVerProductos').click()
        }

        function cerrarModalVerProductos() {
            document.getElementById('BTN_ModalVerProductos').click()
        }

        function cargarFiltros() {
            $(<%= LB_Categorias.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Categoria' })
        }

        function freezeEmisorHeader() {
            var GridId = "<%= DGV_ListaEmisores.ClientID %>";

            var idEmisor = document.getElementById('<%= HDF_IDEmisor.ClientID %>').value
            var idFactura = document.getElementById('<%= HDF_IDFactura.ClientID %>').value
            var idProducto = document.getElementById('<%= HDF_IDProducto.ClientID %>').value

            var ScrollHeight = 500;
            if (idEmisor != "0" || idFactura != "0") {
                ScrollHeight = 75;
            }
            if (idProducto != "0") {
                ScrollHeight = 150;
            }

            var grid = document.getElementById(GridId);
            var gridWidth = grid.offsetWidth;
            var gridHeight = grid.offsetHeight;
            var headerCellWidths = new Array();
            for (var i = 0; i < grid.getElementsByTagName("TH").length; i++) {
                headerCellWidths[i] = grid.getElementsByTagName("TH")[i].offsetWidth;
            }
            grid.parentNode.appendChild(document.createElement("div"));
            var parentDiv = grid.parentNode;

            var table = document.createElement("table");
            for (i = 0; i < grid.attributes.length; i++) {
                if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                    table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
                }
            }
            table.style.cssText = grid.style.cssText;
            table.style.width = gridWidth + "px";
            table.appendChild(document.createElement("tbody"));
            table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
            var cells = table.getElementsByTagName("TH");

            var gridRow = grid.getElementsByTagName("TR")[0];
            for (var i = 0; i < cells.length; i++) {
                var width;
                if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].offsetWidth) {
                    width = headerCellWidths[i];
                }
                else {
                    width = gridRow.getElementsByTagName("TD")[i].offsetWidth;
                }
                cells[i].style.width = parseInt(width - 3) + "px";
                gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width - 3) + "px";
            }
            parentDiv.removeChild(grid);

            var dummyHeader = document.createElement("div");
            dummyHeader.appendChild(table);
            parentDiv.appendChild(dummyHeader);
            var scrollableDiv = document.createElement("div");
            if (parseInt(gridHeight) > ScrollHeight) {
                gridWidth = parseInt(gridWidth) + 12;
            }
            scrollableDiv.style.cssText = "overflow:auto;height:" + ScrollHeight + "px;width:" + gridWidth + "px";
            scrollableDiv.appendChild(grid);
            parentDiv.appendChild(scrollableDiv);
        }

        function freezeFacturaHeader() {
            var GridId = "<%= DGV_ListaFacturas.ClientID %>";

            var idFactura = document.getElementById('<%= HDF_IDFactura.ClientID %>').value

            var ScrollHeight = 500;
            if (idFactura != "0") {
                ScrollHeight = 100;
            }

            var grid = document.getElementById(GridId);
            var gridWidth = grid.offsetWidth;
            var gridHeight = grid.offsetHeight;
            var headerCellWidths = new Array();
            for (var i = 0; i < grid.getElementsByTagName("TH").length; i++) {
                headerCellWidths[i] = grid.getElementsByTagName("TH")[i].offsetWidth;
            }
            grid.parentNode.appendChild(document.createElement("div"));
            var parentDiv = grid.parentNode;

            var table = document.createElement("table");
            for (i = 0; i < grid.attributes.length; i++) {
                if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                    table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
                }
            }
            table.style.cssText = grid.style.cssText;
            table.style.width = gridWidth + "px";
            table.appendChild(document.createElement("tbody"));
            table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
            var cells = table.getElementsByTagName("TH");

            var gridRow = grid.getElementsByTagName("TR")[0];
            for (var i = 0; i < cells.length; i++) {
                var width;
                if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].offsetWidth) {
                    width = headerCellWidths[i];
                }
                else {
                    width = gridRow.getElementsByTagName("TD")[i].offsetWidth;
                }
                cells[i].style.width = parseInt(width - 3) + "px";
                gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width - 3) + "px";
            }
            parentDiv.removeChild(grid);

            var dummyHeader = document.createElement("div");
            dummyHeader.appendChild(table);
            parentDiv.appendChild(dummyHeader);
            var scrollableDiv = document.createElement("div");
            if (parseInt(gridHeight) > ScrollHeight) {
                gridWidth = parseInt(gridWidth) + 12;
            }
            scrollableDiv.style.cssText = "overflow:auto;height:" + ScrollHeight + "px;width:" + gridWidth + "px";
            scrollableDiv.appendChild(grid);
            parentDiv.appendChild(scrollableDiv);
        }

        function freezeProductoHeader() {
            var GridId = "<%= DGV_ListaProductos.ClientID %>";
            
            var idProducto = document.getElementById('<%= HDF_IDProducto.ClientID %>').value

            var ScrollHeight = 500;
            if (idProducto != "0") {
                ScrollHeight = 75;
            }

            var grid = document.getElementById(GridId);
            var gridWidth = grid.offsetWidth;
            var gridHeight = grid.offsetHeight;
            var headerCellWidths = new Array();
            for (var i = 0; i < grid.getElementsByTagName("TH").length; i++) {
                headerCellWidths[i] = grid.getElementsByTagName("TH")[i].offsetWidth;
            }
            grid.parentNode.appendChild(document.createElement("div"));
            var parentDiv = grid.parentNode;

            var table = document.createElement("table");
            for (i = 0; i < grid.attributes.length; i++) {
                if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                    table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
                }
            }
            table.style.cssText = grid.style.cssText;
            table.style.width = gridWidth + "px";
            table.appendChild(document.createElement("tbody"));
            table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
            var cells = table.getElementsByTagName("TH");

            var gridRow = grid.getElementsByTagName("TR")[0];
            for (var i = 0; i < cells.length; i++) {
                var width;
                if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].offsetWidth) {
                    width = headerCellWidths[i];
                }
                else {
                    width = gridRow.getElementsByTagName("TD")[i].offsetWidth;
                }
                cells[i].style.width = parseInt(width - 3) + "px";
                gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width - 3) + "px";
            }
            parentDiv.removeChild(grid);

            var dummyHeader = document.createElement("div");
            dummyHeader.appendChild(table);
            parentDiv.appendChild(dummyHeader);
            var scrollableDiv = document.createElement("div");
            if (parseInt(gridHeight) > ScrollHeight) {
                gridWidth = parseInt(gridWidth) + 12;
            }
            scrollableDiv.style.cssText = "overflow:auto;height:" + ScrollHeight + "px;width:" + gridWidth + "px";
            scrollableDiv.appendChild(grid);
            parentDiv.appendChild(scrollableDiv);
        }

        function freezeProductoDetalleHeader() {
            var idProducto = document.getElementById('<%= HDF_IDProducto.ClientID %>').value

            if (idProducto != "0") {
                var GridId = "<%= DGV_DetelleProductos.ClientID %>";

                var ScrollHeight = 500;

                var grid = document.getElementById(GridId);
                var gridWidth = grid.offsetWidth;
                var gridHeight = grid.offsetHeight;
                var headerCellWidths = new Array();
                for (var i = 0; i < grid.getElementsByTagName("TH").length; i++) {
                    headerCellWidths[i] = grid.getElementsByTagName("TH")[i].offsetWidth;
                }
                grid.parentNode.appendChild(document.createElement("div"));
                var parentDiv = grid.parentNode;

                var table = document.createElement("table");
                for (i = 0; i < grid.attributes.length; i++) {
                    if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                        table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
                    }
                }
                table.style.cssText = grid.style.cssText;
                table.style.width = gridWidth + "px";
                table.appendChild(document.createElement("tbody"));
                table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
                var cells = table.getElementsByTagName("TH");

                var gridRow = grid.getElementsByTagName("TR")[0];
                for (var i = 0; i < cells.length; i++) {
                    var width;
                    if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].offsetWidth) {
                        width = headerCellWidths[i];
                    }
                    else {
                        width = gridRow.getElementsByTagName("TD")[i].offsetWidth;
                    }
                    cells[i].style.width = parseInt(width - 3) + "px";
                    gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width - 3) + "px";
                }
                parentDiv.removeChild(grid);

                var dummyHeader = document.createElement("div");
                dummyHeader.appendChild(table);
                parentDiv.appendChild(dummyHeader);
                var scrollableDiv = document.createElement("div");
                if (parseInt(gridHeight) > ScrollHeight) {
                    gridWidth = parseInt(gridWidth) + 12;
                }
                scrollableDiv.style.cssText = "overflow:auto;height:" + ScrollHeight + "px;width:" + gridWidth + "px";
                scrollableDiv.appendChild(grid);
                parentDiv.appendChild(scrollableDiv);
            }            
        }

        function freezeComprasMensualesCantidadHeader() {
            var GridId = "<%= DGV_ComprasMensualesCantidad.ClientID %>";

            var idProducto = document.getElementById('<%= HDF_IDProducto.ClientID %>').value

            var ScrollHeight = 500;
            if (idProducto != "0") {
                ScrollHeight = 75;
            }

            var grid = document.getElementById(GridId);
            var gridWidth = grid.offsetWidth;
            var gridHeight = grid.offsetHeight;
            var headerCellWidths = new Array();
            for (var i = 0; i < grid.getElementsByTagName("TH").length; i++) {
                headerCellWidths[i] = grid.getElementsByTagName("TH")[i].offsetWidth;
            }
            grid.parentNode.appendChild(document.createElement("div"));
            var parentDiv = grid.parentNode;

            var table = document.createElement("table");
            for (i = 0; i < grid.attributes.length; i++) {
                if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                    table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
                }
            }
            table.style.cssText = grid.style.cssText;
            table.style.width = gridWidth + "px";
            table.appendChild(document.createElement("tbody"));
            table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
            var cells = table.getElementsByTagName("TH");

            var gridRow = grid.getElementsByTagName("TR")[0];
            for (var i = 0; i < cells.length; i++) {
                var width;
                if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].offsetWidth) {
                    width = headerCellWidths[i];
                }
                else {
                    width = gridRow.getElementsByTagName("TD")[i].offsetWidth;
                }
                cells[i].style.width = parseInt(width - 3) + "px";
                gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width - 3) + "px";
            }
            parentDiv.removeChild(grid);

            var dummyHeader = document.createElement("div");
            dummyHeader.appendChild(table);
            parentDiv.appendChild(dummyHeader);
            var scrollableDiv = document.createElement("div");
            if (parseInt(gridHeight) > ScrollHeight) {
                gridWidth = parseInt(gridWidth) + 12;
            }
            scrollableDiv.style.cssText = "overflow:auto;height:" + ScrollHeight + "px;width:" + gridWidth + "px";
            scrollableDiv.appendChild(grid);
            parentDiv.appendChild(scrollableDiv);
        }

        function freezeComprasMensualesCostosHeader() {
            var GridId = "<%= DGV_ComprasMensualesCostos.ClientID %>";

            var idProducto = document.getElementById('<%= HDF_IDProducto.ClientID %>').value

            var ScrollHeight = 500;
            if (idProducto != "0") {
                ScrollHeight = 75;
            }

            var grid = document.getElementById(GridId);
            var gridWidth = grid.offsetWidth;
            var gridHeight = grid.offsetHeight;
            var headerCellWidths = new Array();
            for (var i = 0; i < grid.getElementsByTagName("TH").length; i++) {
                headerCellWidths[i] = grid.getElementsByTagName("TH")[i].offsetWidth;
            }
            grid.parentNode.appendChild(document.createElement("div"));
            var parentDiv = grid.parentNode;

            var table = document.createElement("table");
            for (i = 0; i < grid.attributes.length; i++) {
                if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                    table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
                }
            }
            table.style.cssText = grid.style.cssText;
            table.style.width = gridWidth + "px";
            table.appendChild(document.createElement("tbody"));
            table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
            var cells = table.getElementsByTagName("TH");

            var gridRow = grid.getElementsByTagName("TR")[0];
            for (var i = 0; i < cells.length; i++) {
                var width;
                if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].offsetWidth) {
                    width = headerCellWidths[i];
                }
                else {
                    width = gridRow.getElementsByTagName("TD")[i].offsetWidth;
                }
                cells[i].style.width = parseInt(width - 3) + "px";
                gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width - 3) + "px";
            }
            parentDiv.removeChild(grid);

            var dummyHeader = document.createElement("div");
            dummyHeader.appendChild(table);
            parentDiv.appendChild(dummyHeader);
            var scrollableDiv = document.createElement("div");
            if (parseInt(gridHeight) > ScrollHeight) {
                gridWidth = parseInt(gridWidth) + 12;
            }
            scrollableDiv.style.cssText = "overflow:auto;height:" + ScrollHeight + "px;width:" + gridWidth + "px";
            scrollableDiv.appendChild(grid);
            parentDiv.appendChild(scrollableDiv);
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
                            <h1 class="h3 mb-2 text-gray-800">Reportes</h1>
                        </div>
                        <div class="input-group no-border col-md-6" style="text-align: right; display: inline-block;">
                            <asp:LinkButton ID="BTN_Sincronizar" runat="server" CssClass="btn btn-secundary" OnClick="BTN_Sincronizar_Click" OnClientClick="activarloading();">
                                <i class="fas fa-sync"></i> Sincronizar
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div class="card shadow mb-4">
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel_FiltrosReportes" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:HiddenField ID="HDF_IDEmisor" runat="server" Value="0" />
                                    <asp:HiddenField ID="HDF_IDFactura" runat="server" Value="0" />
                                    <asp:HiddenField ID="HDF_IDProducto" runat="server" Value="0" />
                                    <div class="row" style="height: 50px;">
                                        <div class="input-group no-border col-md-3" style="text-align: center; display: block;">
                                            <a href="Proveedores.aspx" class="btn btn-primary">
                                                <i class="fas fa-cart-plus"></i> Emisores
                                            </a>
                                        </div>
                                        <div class="input-group no-border col-md-3" style="text-align: center; display: block;">
                                            <a href="Facturas.aspx" class="btn btn-primary">
                                                <i class="fas fa-file-invoice"></i> Facturas
                                            </a>
                                        </div>
                                        <div class="input-group no-border col-md-3" style="text-align: center; display: block;">
                                            <a href="Productos.aspx" class="btn btn-primary">
                                                <i class="fas fa-barcode"></i> Productos
                                            </a>
                                        </div>
                                        <div class="input-group no-border col-md-3" style="text-align: center; display: block;">
                                            <a href="Reportes.aspx" class="btn btn-info">
                                                <i class="fas fa-fw fa-tachometer-alt"></i> Reportes
                                            </a>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="col-md-2">
                                            <label style="margin-top: 1%;">Fecha desde:</label>
                                            <asp:TextBox class="form-control" Style="flex: auto;" ID="TXT_FechaDesde" runat="server" TextMode="Date" OnTextChanged="Recargar_Click" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label style="margin-top: 1%;">Fecha hasta:</label>
                                            <asp:TextBox class="form-control" Style="flex: auto;" ID="TXT_FechaHasta" runat="server" TextMode="Date" OnTextChanged="Recargar_Click" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <br />
                                            <asp:ListBox class="form-control" runat="server" ID="LB_Categorias" SelectionMode="Multiple" OnTextChanged="Recargar_Click" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                        <div class="col-md-2">                                        
                                            <br />
                                            <asp:DropDownList class="form-control" style="font-size: 18px;" ID="DDL_TipoProducto" runat="server" OnSelectedIndexChanged="Recargar_Click" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="card" style="border: solid; border-color: #00BB2D;">
                                        <div class="card-header">
                                            <h4 style="text-align: center;" class="card-title">Emisores </h4>
                                        </div>
                                        <div class="card-body">
                                            <asp:UpdatePanel ID="UpdatePanel_EmisoresHead" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="input-group no-border col-md-6">
                                                        <asp:TextBox class="form-control" ID="TXT_BuscarEmisor" runat="server" placeholder="Buscar..." OnTextChanged="Recargar_Click" AutoPostBack="true"></asp:TextBox>
                                                        <div class="input-group-append">
                                                            <div class="input-group-text">
                                                                <i class="nc-icon nc-zoom-split"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <div class="table">
                                                <asp:UpdatePanel ID="UpdatePanel_ListaEmisores" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="DGV_ListaEmisores" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            AutoGenerateColumns="False" DataKeyNames="IDEmisor,Activo,NumeroIdentificacion,TipoIdentificacion,Nombre,NombreComercial" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                                            OnSorting="DGV_ListaEmisores_Sorting"
                                                            OnRowDataBound="DGV_ListaEmisores_RowDataBound"
                                                            OnRowCommand="DGV_ListaEmisores_RowCommand">
                                                            <Columns>
                                                                <asp:BoundField DataField="NombreComercial" SortExpression="NombreComercial" HeaderText="Nombre" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="NumeroIdentificacion" SortExpression="NumeroIdentificacion" HeaderText="Identificacion" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="CorreoEmisor" SortExpression="CorreoEmisor" HeaderText="Correo" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="Telefono" SortExpression="Telefono" HeaderText="Teléfono" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="DGV_ListaEmisores" EventName="selectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="card" style="border: solid; border-color: #51bcda;">
                                        <div class="card-header">
                                            <h4 style="text-align: center;" class="card-title">Facturas </h4>
                                        </div>
                                        <div class="card-body">
                                            <asp:UpdatePanel ID="UpdatePanel_FacturaHead" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="input-group no-border col-md-6">
                                                        <asp:TextBox class="form-control" ID="TXT_BuscarFactura" runat="server" placeholder="Buscar..." OnTextChanged="Recargar_Click" AutoPostBack="true"></asp:TextBox>
                                                        <div class="input-group-append">
                                                            <div class="input-group-text">
                                                                <i class="nc-icon nc-zoom-split"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <div class="table">
                                                <asp:UpdatePanel ID="UpdatePanel_ListaFacturas" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="DGV_ListaFacturas" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            AutoGenerateColumns="False" DataKeyNames="IDFactura,EmisorID,ClaveFactura,NumeroConsecutivoFactura,FechaFactura,FechaSincronizacion,NombreComercial,TotalVenta,TotalDescuento,TotalImpuesto,TotalComprobante"
                                                            HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true" ShowFooter="true"
                                                            EmptyDataText="No hay registros." AllowSorting="true"
                                                            OnSorting="DGV_ListaFacturas_Sorting"
                                                            OnRowDataBound="DGV_ListaFacturas_RowDataBound"
                                                            OnRowCommand="DGV_ListaFacturas_RowCommand">
                                                            <Columns>
                                                                <asp:BoundField DataField="NombreComercial" SortExpression="NombreComercial" HeaderText="Emisor" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="NumeroConsecutivoFactura" SortExpression="NumeroConsecutivoFactura" HeaderText="Número factura" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="FechaFactura" SortExpression="FechaFactura" HeaderText="Fecha factura" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="FechaSincronizacion" SortExpression="FechaSincronizacion" HeaderText="Fecha sincronizacion" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:TemplateField HeaderText="Total venta">
                                                                    <ItemTemplate>
                                                                        <div style="text-align: center;">
                                                                            <asp:Label ID="LBL_TotalVenta" runat="server" Text='<%# Eval("TotalVenta") %>' DataFormatString="{0:n}" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="LBL_FOO_TotalVenta" DataFormatString="{0:n}" runat="server" />
                                                                    </FooterTemplate>
                                                                    <ItemStyle ForeColor="Black" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total descuento">
                                                                    <ItemTemplate>
                                                                        <div style="text-align: center;">
                                                                            <asp:Label ID="LBL_TotalDescuento" runat="server" Text='<%# Eval("TotalDescuento") %>' DataFormatString="{0:n}" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="LBL_FOO_TotalDescuento" DataFormatString="{0:n}" runat="server" />
                                                                    </FooterTemplate>
                                                                    <ItemStyle ForeColor="Black" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total impuesto">
                                                                    <ItemTemplate>
                                                                        <div style="text-align: center;">
                                                                            <asp:Label ID="LBL_TotalImpuesto" runat="server" Text='<%# Eval("TotalImpuesto") %>' DataFormatString="{0:n}" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="LBL_FOO_TotalImpuesto" DataFormatString="{0:n}" runat="server" />
                                                                    </FooterTemplate>
                                                                    <ItemStyle ForeColor="Black" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total comprobante">
                                                                    <ItemTemplate>
                                                                        <div style="text-align: center;">
                                                                            <asp:Label ID="LBL_TotalComprobante" runat="server" Text='<%# Eval("TotalComprobante") %>' DataFormatString="{0:n}" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="LBL_FOO_TotalComprobante" DataFormatString="{0:n}" runat="server" />
                                                                    </FooterTemplate>
                                                                    <ItemStyle ForeColor="Black" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <FooterStyle BackColor="#cccccc" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center" CssClass="visible-lg" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="card" style="border: solid; border-color: #00BB2D">
                                        <div class="card-header">
                                            <h4 style="text-align: center;" class="card-title">Productos </h4>
                                        </div>
                                        <div class="card-body">
                                            <asp:UpdatePanel ID="UpdatePanel_ProductoHead" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="input-group no-border col-md-6">
                                                        <asp:TextBox class="form-control" ID="TXT_BuscarProducto" runat="server" placeholder="Buscar..." OnTextChanged="Recargar_Click" AutoPostBack="true"></asp:TextBox>
                                                        <div class="input-group-append">
                                                            <div class="input-group-text">
                                                                <i class="nc-icon nc-zoom-split"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <div class="table">
                                                <asp:UpdatePanel ID="UpdatePanel_ListaProductos" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="DGV_ListaProductos" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            AutoGenerateColumns="False" DataKeyNames="IDProducto,PrecioUnitario,PorcentajeImpuesto,FacturaID" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                                            OnSorting="DGV_ListaProductos_Sorting"
                                                            OnRowDataBound="DGV_ListaProductos_RowDataBound"
                                                            OnRowCommand="DGV_ListaProductos_RowCommand">
                                                            <Columns>
                                                                <asp:BoundField DataField="DetalleProducto" SortExpression="DetalleProducto" HeaderText="Descripcion" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="FechaActualizacion" SortExpression="FechaActualizacion" HeaderText="Fecha actualizado" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="PrecioUnitario" SortExpression="PrecioUnitario" HeaderText="Precio unitario" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="MontoDescuento" SortExpression="MontoDescuento" HeaderText="Descuento" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="PorcentajeImpuesto" SortExpression="PorcentajeImpuesto" HeaderText="Impuesto" DataFormatString="{0:n0}%" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="MontoImpuesto" SortExpression="MontoImpuesto" HeaderText="Monto impuesto" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="MontoImpuestoIncluido" SortExpression="MontoImpuestoIncluido" HeaderText="Monto IVAI" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="Porcentaje25" SortExpression="Porcentaje25" HeaderText="25%" ItemStyle-ForeColor="black" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="PrecioVenta" SortExpression="PrecioVenta" HeaderText="Precio venta" ItemStyle-ForeColor="black" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="ProductoMateriaPrima" SortExpression="ProductoMateriaPrima" HeaderText="Materia prima" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="ProductoVenta" SortExpression="ProductoVenta" HeaderText="Venta" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="card" style="border: solid; border-color: #51bcda">
                                        <div class="card-header">
                                            <h4 style="text-align: center;" class="card-title">Compras mensuales cantidad </h4>
                                        </div>
                                        <div class="card-body">
                                            <div class="table">
                                                <asp:UpdatePanel ID="UpdatePanel_ComprasMensualesCantidad" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="DGV_ComprasMensualesCantidad" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            AutoGenerateColumns="False" DataKeyNames="ProductoID" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                                            OnRowDataBound="DGV_ComprasMensualesCantidad_RowDataBound"
                                                            OnRowCommand="DGV_ComprasMensualesCantidad_RowCommand">
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="card" style="border: solid; border-color: #00BB2D">
                                        <div class="card-header">
                                            <h4 style="text-align: center;" class="card-title">Compras mensuales costos </h4>
                                        </div>
                                        <div class="card-body">
                                            <div class="table">
                                                <asp:UpdatePanel ID="UpdatePanel_ComprasMensualesCostos" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="DGV_ComprasMensualesCostos" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            AutoGenerateColumns="false" DataKeyNames="ProductoID" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                                            OnRowDataBound="DGV_ComprasMensualesCostos_RowDataBound"
                                                            OnRowCommand="DGV_ComprasMensualesCostos_RowCommand">
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="card" style="border: solid; border-color: #51bcda">
                                        <div class="card-header">
                                            <h4 style="text-align: center;" class="card-title">Productos histórico </h4>
                                        </div>
                                        <div class="card-body">
                                            <div class="table">
                                                <asp:UpdatePanel ID="UpdatePanel_DetalleProductos" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="DGV_DetelleProductos" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            AutoGenerateColumns="False" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                                            ShowHeaderWhenEmpty="true" EmptyDataText="Seleccione un producto para ver su histórico de compras." AllowSorting="true" ShowFooter="true"
                                                            OnRowDataBound="DGV_DetelleProductos_RowDataBound">
                                                            <Columns>
                                                                <asp:BoundField DataField="DetalleProducto" SortExpression="DetalleProducto" HeaderText="Descripcion" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px"></asp:BoundField>
                                                                <asp:BoundField DataField="FechaFactura" SortExpression="FechaFactura" HeaderText="Fecha factura" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <%--<asp:BoundField DataField="UnidadMedida" SortExpression="UnidadMedida" HeaderText="Unidad medida" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>--%>
                                                                <asp:TemplateField HeaderText="Cantidad">
                                                                    <ItemTemplate>
                                                                        <div style="text-align: center;">
                                                                            <asp:Label ID="LBL_Cantidad" runat="server" Text='<%# Eval("Cantidad") %>' DataFormatString="{0:n}" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="LBL_FOO_Cantidad" DataFormatString="{0:n}" runat="server" />
                                                                    </FooterTemplate>
                                                                    <ItemStyle ForeColor="Black" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="PrecioUnitarioFinal" SortExpression="PrecioUnitarioFinal" HeaderText="Precio unitario" DataFormatString="{0:n2}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:TemplateField HeaderText="Monto descuento">
                                                                    <ItemTemplate>
                                                                        <div style="text-align: center;">
                                                                            <asp:Label ID="LBL_MontoDescuentoUnitario" runat="server" Text='<%# Eval("MontoDescuentoUnitario") %>' DataFormatString="{0:n}" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="LBL_FOO_MontoDescuentoUnitario" DataFormatString="{0:n}" runat="server" />
                                                                    </FooterTemplate>
                                                                    <ItemStyle ForeColor="Black" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="PrecioUnitarioDescuento" SortExpression="PrecioUnitarioDescuento" HeaderText="Precio descuento" DataFormatString="{0:n2}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="PorcentajeImpuesto" SortExpression="PorcentajeImpuesto" HeaderText="Impuesto" DataFormatString="{0:n0}%" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:TemplateField HeaderText="Monto impuesto">
                                                                    <ItemTemplate>
                                                                        <div style="text-align: center;">
                                                                            <asp:Label ID="LBL_MontoImpuestoUnitario" runat="server" Text='<%# Eval("MontoImpuestoUnitario") %>' DataFormatString="{0:n}" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="LBL_FOO_MontoImpuestoUnitario" DataFormatString="{0:n}" runat="server" />
                                                                    </FooterTemplate>
                                                                    <ItemStyle ForeColor="Black" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Precio impuesto">
                                                                    <ItemTemplate>
                                                                        <div style="text-align: center;">
                                                                            <asp:Label ID="LBL_PrecioUnitarioImpuesto" runat="server" Text='<%# Eval("PrecioUnitarioImpuesto") %>' DataFormatString="{0:n}" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="LBL_FOO_PrecioUnitarioImpuesto" DataFormatString="{0:n}" runat="server" />
                                                                    </FooterTemplate>
                                                                    <ItemStyle ForeColor="Black" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Porcentaje25" SortExpression="Porcentaje25" HeaderText="25%" DataFormatString="{0:n2}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                <asp:BoundField DataField="PrecioVenta" SortExpression="PrecioVenta" HeaderText="Precio venta" DataFormatString="{0:n2}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                            </Columns>
                                                            <FooterStyle BackColor="#cccccc" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center" CssClass="visible-lg" Font-Size="13px" />
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
                            <h5 class="modal-title" runat="server" id="H2">Producto</h5>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel_DetalleProducto" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <label for="TXT_DetalleProductoD">Detalle del producto:</label>
                                            <asp:TextBox ID="TXT_DetalleProductoD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label for="TXT_FechaActualizadoD">Fecha actualizado:</label>
                                            <asp:TextBox ID="TXT_FechaActualizadoD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-2">
                                            <label for="TXT_PrecioUnitarioD">Precio unitario:</label>
                                            <asp:TextBox ID="TXT_PrecioUnitarioD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label for="TXT_MontoDescuentoD">Monto descuento:</label>
                                            <asp:TextBox ID="TXT_MontoDescuentoD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <label for="TXT_ImpuestoD">Impuesto:</label>
                                            <asp:TextBox ID="TXT_ImpuestoD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label for="TXT_MontoImpuestoD">Monto impuesto:</label>
                                            <asp:TextBox ID="TXT_MontoImpuestoD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label for="TXT_MontoImpuestoIncluidoD">Monto IVAI:</label>
                                            <asp:TextBox ID="TXT_MontoImpuestoIncluidoD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <label for="TXT_Porcentaje25D">25%:</label>
                                            <asp:TextBox ID="TXT_Porcentaje25D" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label for="TXT_PrecioVentaD">Precio venta:</label>
                                            <asp:TextBox ID="TXT_PrecioVentaD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div> 
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="card card-chart">
                                                <figure class="highcharts-figure">
                                                    <div id="containerHistoricoPrecios"></div>
                                                </figure>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="card card-chart">
                                                <figure class="highcharts-figure">
                                                    <div id="containerHistoricoVariacion"></div>
                                                </figure>
                                            </div>
                                        </div>
                                    </div>
                                    <br />                                    
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
