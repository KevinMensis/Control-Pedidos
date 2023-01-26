<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerReceta.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.GestionCostos.VerReceta" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Receta</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <style>
        @media (max-width: 991px) {
            .btn-show {
                display: inline-block !important;
            }
        }

        .btn-show, .btn-close {
            display: none;
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

        function seleccionarReceptor(receptor) {
            if (receptor === "MiKFe") {
                __doPostBack('Identificacion;3101485961')
            } else if (receptor === "Esteban") {
                __doPostBack('Identificacion;115210651')
            }
        }

        function seleccionarNegocio(tipoNegocio) {
            __doPostBack('Receta;' + tipoNegocio)
        }

        function graficoCostos(datos) {
            lista = []
            Highcharts.chart('containerCostos', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Costo del producto (%)'
                },
                tooltip: {
                    pointFormat: 'Porcentaje: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: false
                        },
                        showInLegend: true
                    }
                },
                series: [{
                    name: 'Factor',
                    colorByPoint: true,
                    data: datos
                }]
            });
        }

        function graficoCostosMonto(factores, datos) {
            Highcharts.chart('containerCostosMonto', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Costo del producto (monto)'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer'
                    },
                    column: {
                        dataLabels: {
                            align: 'left',
                            enabled: true,
                            rotation: 270,
                            x: 2,
                            y: -10,
                            format: "{point.y:.2f}"
                        }
                    }
                },
                xAxis: {
                    categories: factores
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                tooltip: {
                    pointFormat: '<b>{point.y:.2f}</b>'
                },
                credits: {
                    enabled: false
                },
                series: datos
            });
        }

        function graficoCostosProduccion(datos) {
            lista = []
            Highcharts.chart('containerCargaFabril', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Costos carga fabril (%)'
                },
                tooltip: {
                    pointFormat: 'Porcentaje: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: false
                        },
                        showInLegend: true
                    }
                },
                series: [{
                    name: 'Factor',
                    colorByPoint: true,
                    data: datos
                }]
            });
        }

        function graficoCostosProduccionMonto(factores, datos) {
            Highcharts.chart('containerCargaFabrilMonto', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Costos carga fabril (monto)'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer'
                    },
                    column: {
                        dataLabels: {
                            align: 'left',
                            enabled: true,
                            rotation: 270,
                            x: 2,
                            y: -10,
                            format: "{point.y:.2f}"
                        }
                    }
                },
                xAxis: {
                    categories: factores
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                tooltip: {
                    pointFormat: '<b>{point.y:.2f}</b>'
                },
                credits: {
                    enabled: false
                },
                series: datos
            });
        }

        function graficoResumen(datos) {
            lista = []
            Highcharts.chart('containerMargenUtilidad', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Costo vs Margen (%)'
                },
                tooltip: {
                    pointFormat: 'Porcentaje: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: false
                        },
                        showInLegend: true
                    }
                },
                series: [{
                    name: 'Factor',
                    colorByPoint: true,
                    data: datos
                }]
            });
        }

        function graficoResumenMonto(factores, datos) {
            Highcharts.chart('containerMargenUtilidadMonto', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Costo vs Margen (monto)'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer'
                    },
                    column: {
                        dataLabels: {
                            align: 'left',
                            enabled: true,
                            rotation: 270,
                            x: 2,
                            y: -10,
                            format: "{point.y:.2f}"
                        }
                    }
                },
                xAxis: {
                    categories: factores
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                tooltip: {
                    pointFormat: '<b>{point.y:.2f}</b>'
                },
                credits: {
                    enabled: false
                },
                series: datos
            });
        }

        function cargarGraficos(idUsuario) {
            idProductoTerminado = $(<%= HDF_IDProductoTerminado.ClientID %>)[0].value
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VerReceta.aspx/cargarGraficoCostos",
                data: JSON.stringify({
                    "idUsuario": idUsuario,
                    "idProductoTerminado": idProductoTerminado,
                }),
                dataType: "json",
                success: function (Result) {
                    listaCostos = []
                    factores = []
                    listaCostosMontos = []
                    var values = []
                    for (var i in Result.d) {
                        listaCostos.unshift(
                            {
                                'name': Result.d[i].factor,
                                'y': Result.d[i].porcentaje
                            }
                        )
                        factores.push(Result.d[i].factor)
                        values.push(Result.d[i].monto)
                    }
                    var array = { name: 'Costos', data: values };
                    listaCostosMontos.push(array)
                    graficoCostos(listaCostos)
                    graficoCostosMonto(factores, listaCostosMontos)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VerReceta.aspx/cargarGraficoCostosProduccion",
                data: JSON.stringify({
                    "idUsuario": idUsuario,
                    "idProductoTerminado": idProductoTerminado,
                }),
                dataType: "json",
                success: function (Result) {
                    listaCostos = []
                    factores = []
                    listaCostosMontos = []
                    var values = []
                    for (var i in Result.d) {
                        listaCostos.unshift(
                            {
                                'name': Result.d[i].factor,
                                'y': Result.d[i].porcentaje
                            }
                        )
                        factores.push(Result.d[i].factor)
                        values.push(Result.d[i].monto)
                    }
                    var array = { name: 'Costos', data: values };
                    listaCostosMontos.push(array)
                    graficoCostosProduccion(listaCostos)
                    graficoCostosProduccionMonto(factores, listaCostosMontos)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VerReceta.aspx/cargarGraficoCostoResumen",
                data: JSON.stringify({
                    "idUsuario": idUsuario,
                    "idProductoTerminado": idProductoTerminado,
                }),
                dataType: "json",
                success: function (Result) {
                    listaCostos = []
                    factores = []
                    listaCostosMontos = []
                    var values = []
                    for (var i in Result.d) {
                        listaCostos.unshift(
                            {
                                'name': Result.d[i].factor,
                                'y': Result.d[i].porcentaje
                            }
                        )
                        factores.push(Result.d[i].factor)
                        values.push(Result.d[i].monto)
                    }
                    var array = { name: 'Costos', data: values };
                    listaCostosMontos.push(array)
                    graficoResumen(listaCostos)
                    graficoResumenMonto(factores, listaCostosMontos)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })
        }

        function freezeMateriasPrimasAsignadasHeader() {
            console.log('freeze')
            var GridId = "<%= DGV_ListaMateriasPrimasAsignadas.ClientID %>";

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

        function cargarFiltros() {

        }

        $(document).ready(function () {
            cargarFiltros();
            freezeMateriasPrimasAsignadasHeader();
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
        <asp:HiddenField ID="HDF_IDUsuario" runat="server" Value="0" Visible="true" />
        <div class="sidebar" id="div_Menu" data-color="white" data-active-color="danger">
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
                    <li id="li_panaderia" runat="server">
                        <a href="#" onclick="seleccionarNegocio('panaderia');">
                            <i class="fas fa-chart-line"></i>
                            <p>Costos panadería</p>
                        </a>
                    </li>
                    <li id="li_restaurante" runat="server">
                        <a href="#" onclick="seleccionarNegocio('restaurante');">
                            <i class="fas fa-chart-line"></i>
                            <p>Costos restaurante</p>
                        </a>
                    </li>
                </ul>
                <hr style="width: 230px; color: #2c2c2c;" />
                <%--<h5 style="text-align: center;">Mantenimiento</h5>--%>
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
                    <asp:UpdatePanel ID="UpdatePanel_Header" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="input-group no-border col-md-6" style="text-align: left; display: inline-block;">
                                    <h1 class="h3 mb-2 text-gray-800" runat="server" id="H1_Title">Ver recetas</h1>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="card shadow mb-4">
                        <div class="card-body" style="padding-top: 0px;">
                            <div class="card-body">
                                <asp:UpdatePanel ID="UpdatePanel_Filtros" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="input-group no-border col-md-2" style="text-align: center; display: block;">
                                                <a href="CostosIndirectos.aspx" class="btn btn-primary">
                                                    <i class=""></i>Costos indirectos
                                                </a>
                                            </div>
                                            <div class="input-group no-border col-md-2" style="text-align: center; display: block;">
                                                <a href="ManoObra.aspx" class="btn btn-primary">
                                                    <i class=""></i>Mano de obra
                                                </a>
                                            </div>
                                            <div class="input-group no-border col-md-2" style="text-align: center; display: block;">
                                                <a href="ProductosIntermedios.aspx" class="btn btn-primary">
                                                    <i class=""></i>Recetas intermedias
                                                </a>
                                            </div>
                                            <div class="input-group no-border col-md-2" style="text-align: center; display: block;">
                                                <a href="CrearReceta.aspx" class="btn btn-primary">
                                                    <i class=""></i>Crear receta
                                                </a>
                                            </div>
                                            <div class="input-group no-border col-md-2" style="text-align: center; display: block;">
                                                <a href="VerReceta.aspx" class="btn btn-info">
                                                    <i class=""></i>Ver recetas
                                                </a>
                                            </div>
                                            <div class="input-group no-border col-md-2" style="text-align: center; display: block;">
                                                <asp:LinkButton UseSubmitBehavior="false" ID="BTN_Actualizar" runat="server" CssClass="btn btn-secundary" OnClientClick="activarloading();" OnClick="BTN_ActualizarCostosProductosTerminados_OnClick">
                                                    <i class="fas fa-sync"></i> Actualizar
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                        <hr />
                                        <div class="row">
                                            <asp:HiddenField ID="HDF_DetalleProducto" runat="server" Value="0" Visible="true" />
                                            <asp:HiddenField ID="HDF_IDProductoTerminado" runat="server" Value="0" Visible="true" />
                                            <div class="input-group no-border col-md-6">
                                                <asp:TextBox class="form-control" ID="TXT_BuscarProducto" runat="server" placeholder="Buscar producto..." OnTextChanged="FiltrarProductos_OnClick" AutoPostBack="true"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <div class="input-group-text">
                                                        <i class="nc-icon nc-zoom-split"></i>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6" style="text-align: right;">
                                                <asp:Button ID="BTN_ReporteProductos" runat="server" UseSubmitBehavior="false" Text="Descargar productos" CssClass="btn btn-info" OnClick="BTN_ReporteProductos_OnClick"></asp:Button>                                            
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="table-responsive">
                                                <asp:GridView ID="DGV_ProductosTerminados" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    AutoGenerateColumns="False" DataKeyNames="IDProductoTerminado,DetalleProducto"
                                                    HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true"
                                                    EmptyDataText="Sin productos registrados." AllowSorting="true"
                                                    OnSorting="DGV_ProductosTerminados_Sorting"
                                                    OnRowCommand="DGV_ProductosTerminados_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="LBL_Ver" runat="server" Text="Detalle"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton class="btn btn-outline-info btn-round-mant" ID="BTN_VerProductoTerminado" runat="server"
                                                                    CommandName="VerProductoTerminado"
                                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    AutoPostBack="true"> 
                                                                    <i class="fas fa-eye"></i>
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="DetalleProducto" SortExpression="DetalleProducto" HeaderText="Descripcion" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="PrecioUnitario" SortExpression="PrecioUnitario" HeaderText="Precio venta" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="PorcentajeImpuesto" SortExpression="PorcentajeImpuesto" HeaderText="Impuesto" DataFormatString="{0:n0}%" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                        
                                                        <asp:BoundField DataField="PrecioUnitarioSinIVA" SortExpression="PrecioUnitarioSinIVA" HeaderText="Precio sin IVA" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                        
                                                        <asp:BoundField DataField="CostoMateriaPrima" SortExpression="CostoMateriaPrima" HeaderText="Materia prima" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                        
                                                        <asp:BoundField DataField="CostoManoObraDirecta" SortExpression="CostoManoObraDirecta" HeaderText="Costo mano obra" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                        
                                                        <asp:BoundField DataField="CostoProduccion" SortExpression="CostoProduccion" HeaderText="Costo producción" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                        
                                                        <asp:BoundField DataField="CostoTotal" SortExpression="CostoTotal" HeaderText="Costo total" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                        
                                                        <asp:BoundField DataField="PorcentajeCosto" SortExpression="PorcentajeCosto" HeaderText="Costo total" DataFormatString="{0:n}%" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                        
                                                        <asp:BoundField DataField="MargenGanacia" SortExpression="MargenGanacia" HeaderText="Margen ganancia" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                        
                                                        <asp:BoundField DataField="PorcentajeMargen" SortExpression="PorcentajeMargen" HeaderText="Margen ganancia" DataFormatString="{0:n}%" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                        
                                                        <asp:BoundField DataField="MedidaUnidades" SortExpression="MedidaUnidades" HeaderText="Unidades producidas" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                        
                                                    </Columns>
                                                </asp:GridView>
                                                <br />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="BTN_ReporteProductos" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <hr />
                            <div class="table">
                                <div class="row">                                  
                                    <div class="col-md-6">
                                        <asp:UpdatePanel ID="UpdatePanel_MateriasPrimasAsignadas" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <h5 class="modal-title" runat="server" id="H5_Title" visible="false"></h5>
                                                <h6 class="modal-title" runat="server" id="H6_Subtitle" visible="false"></h6>
                                                <asp:GridView ID="DGV_ListaMateriasPrimasAsignadas" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    AutoGenerateColumns="False" DataKeyNames="" ShowFooter="true"
                                                    HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true"
                                                    EmptyDataText="Sin materia prima asignada." AllowSorting="true"
                                                    OnRowDataBound="DGV_ListaMateriasPrimasAsignadas_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="DetalleProducto" SortExpression="DetalleProducto" HeaderText="Descripcion" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="CostoKilo" SortExpression="CostoKilo" HeaderText="Costo x Kilo" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="CostoGramo" SortExpression="CostoGramo" HeaderText="Costo x Gramo" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="Costo total">
                                                            <ItemTemplate>
                                                                <div style="text-align: center;">
                                                                    <asp:Label ID="LBL_CostoTotal" runat="server" Text='<%# Eval("CostoTotal") %>' DataFormatString="{0:n}" />
                                                                </div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="LBL_FOO_CostoTotal" DataFormatString="{0:n}" runat="server" />
                                                            </FooterTemplate>
                                                            <ItemStyle ForeColor="Black" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#cccccc" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center" CssClass="visible-lg" />
                                                </asp:GridView>
                                                <br />                                                
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:UpdatePanel ID="UpdatePanel_ListaEmpleados" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <h5 class="modal-title" runat="server" id="H5_TitleEmpleado" visible="false"></h5>
                                                <h6 class="modal-title" runat="server" id="H6_SubtitleEmpleado" visible="false"></h6>
                                                <asp:GridView ID="DGV_ListaEmpleados" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    AutoGenerateColumns="False" DataKeyNames="IDConsecutivoReceta" ShowFooter="true"
                                                    HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true"
                                                    EmptyDataText="Sin colaborador asignado." AllowSorting="true"
                                                    OnRowDataBound="DGV_ListaEmpleados_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="Descripcion" SortExpression="Descripcion" HeaderText="Colaborador" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="CantidadMinutos" SortExpression="CantidadMinutos" HeaderText="Minutos aplicados" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="SalarioMinuto" SortExpression="SalarioMinuto" HeaderText="Costo x minuto" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="Total MOD">
                                                            <ItemTemplate>
                                                                <div style="text-align: center;">
                                                                    <asp:Label ID="LBL_TotalMOD" runat="server" Text='<%# Eval("TotalMOD") %>' DataFormatString="{0:n}" />
                                                                </div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="LBL_FOO_TotalMOD" DataFormatString="{0:n}" runat="server" />
                                                            </FooterTemplate>
                                                            <ItemStyle ForeColor="Black" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#cccccc" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center" CssClass="visible-lg" />
                                                </asp:GridView>
                                                <br />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:UpdatePanel ID="UpdatePanel_Resumen" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <br />
                                                <div class="col-md-6">
                                                    <h5 class="modal-title" runat="server" id="H5_Subtitle" visible="false"></h5>
                                                    <asp:GridView ID="DGV_ListaCostosProduccion" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        AutoGenerateColumns="False" DataKeyNames="" ShowFooter="true"
                                                        HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true"
                                                        EmptyDataText="Sin costos registrados." AllowSorting="true"
                                                        OnRowDataBound="DGV_ListaCostosProduccion_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="Descripcion" SortExpression="Descripcion" HeaderText="Factor" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                            <asp:BoundField DataField="Valor" SortExpression="Valor" HeaderText="Porcentaje" DataFormatString="{0:n2}%" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                            <asp:TemplateField HeaderText="Costo">
                                                                <ItemTemplate>
                                                                    <div style="text-align: center;">
                                                                        <asp:Label ID="LBL_CostoProduccion" runat="server" Text='<%# Eval("CostoProduccion") %>' DataFormatString="{0:n}" />
                                                                    </div>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="LBL_FOO_CostoProduccion" DataFormatString="{0:n}" runat="server" />
                                                                </FooterTemplate>
                                                                <ItemStyle ForeColor="Black" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle BackColor="#cccccc" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center" CssClass="visible-lg" />
                                                    </asp:GridView>
                                                </div>
                                                <div class="col-md-6">
                                                    <h5 class="modal-title" runat="server" id="H5_Resumen" visible="false"></h5>
                                                    <asp:GridView ID="DGV_ListaResumen" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        AutoGenerateColumns="False" DataKeyNames=""
                                                        HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true"
                                                        EmptyDataText="Sin registros." AllowSorting="true">
                                                        <Columns>
                                                            <asp:BoundField DataField="Descripcion" SortExpression="Descripcion" HeaderText="Descripción" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                            <asp:BoundField DataField="Valor" SortExpression="Valor" HeaderText="" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                            <asp:BoundField DataField="Porcentaje" SortExpression="Porcentaje" HeaderText="" DataFormatString="{0:n}%" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>                                                    
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-4">
                                            <div class="card card-chart">
                                                <figure class="highcharts-figure">
                                                    <div id="containerCostos"></div>
                                                </figure>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="card card-chart">
                                                <figure class="highcharts-figure">
                                                    <div id="containerCargaFabril"></div>
                                                </figure>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="card card-chart">
                                                <figure class="highcharts-figure">
                                                    <div id="containerMargenUtilidad"></div>
                                                </figure>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-4">
                                            <div class="card card-chart">
                                                <figure class="highcharts-figure">
                                                    <div id="containerCostosMonto"></div>
                                                </figure>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="card card-chart">
                                                <figure class="highcharts-figure">
                                                    <div id="containerCargaFabrilMonto"></div>
                                                </figure>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="card card-chart">
                                                <figure class="highcharts-figure">
                                                    <div id="containerMargenUtilidadMonto"></div>
                                                </figure>
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
</asp:Content>
