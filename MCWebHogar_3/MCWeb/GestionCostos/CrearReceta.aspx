<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrearReceta.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.GestionCostos.CrearReceta" %>

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
        var productosAgregar;
        var productosEliminar;

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

        function CHK_Producto_onChange(CHK_Producto) {
            var values = CHK_Producto.childNodes[0].id.split('_')
            var index = values.pop() * 1

            var idSelect = 'Content_DGV_ListaMateriasPrimas_CHK_Producto_' + index
            var CHK_Producto = document.getElementById(idSelect)

            var idTXT_Cantidad = 'Content_DGV_ListaMateriasPrimas_TXT_MedidaUds_' + index
            var TXT_Cantidad = document.getElementById(idTXT_Cantidad)

            var id = 'Content_DGV_ListaMateriasPrimas_HDF_IDProductoMateriaPrima_' + index
            var HDF_IDProductoMateriaPrima = document.getElementById(id)
            var idProducto = HDF_IDProductoMateriaPrima.value

            if (CHK_Producto.checked) {
                productosAgregar.set(idProducto, TXT_Cantidad)
            } else {
                productosAgregar.delete(idProducto)
            }
        }

        function CHK_ProductoEliminar_onChange(CHK_Producto) {
            var values = CHK_Producto.childNodes[0].id.split('_')
            var index = values.pop() * 1

            var idSelect = 'Content_DGV_ListaMateriasPrimasAsignadas_CHK_ProductoMateriaPrimaAsignada_' + index
            var CHK_Producto = document.getElementById(idSelect)

            var idTXT_Cantidad = 'Content_DGV_ListaMateriasPrimasAsignadas_TXT_CantidadNecesaria_' + index
            var TXT_Cantidad = document.getElementById(idTXT_Cantidad)

            var id = 'Content_DGV_ListaMateriasPrimasAsignadas_HDF_IDConsecutivoReceta_' + index
            var HDF_IDProductoMateriaPrimaAsignada = document.getElementById(id)
            var idProducto = HDF_IDProductoMateriaPrimaAsignada.value

            if (CHK_Producto.checked) {
                productosEliminar.set(idProducto, TXT_Cantidad)
            } else {
                productosEliminar.delete(idProducto)
            }
        }

        function TXT_CantidadAgregar_onKeyUp(txtCantidad, e) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1 + 1
            if (e.keyCode === 13) {
                var rows = $(<%= DGV_ListaMateriasPrimas.ClientID %>)[0].rows.length - 1
                var id = ''
                if (index === rows) {
                    id = 'Content_DGV_ListaMateriasPrimas_TXT_MedidaUds_' + 0
                } else {
                    id = 'Content_DGV_ListaMateriasPrimas_TXT_MedidaUds_' + index
                }
                document.getElementById(id).autofocus = true;
                document.getElementById(id).focus();
                document.getElementById(id).select();
            }
        }

        function TXT_CantidadNecesaria_onKeyUp(txtCantidad, e) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1 + 1
            if (e.keyCode === 13) {
                var rows = $(<%= DGV_ListaMateriasPrimasAsignadas.ClientID %>)[0].rows.length - 1
                var id = ''
                if (index === rows) {
                    id = 'Content_DGV_ListaMateriasPrimasAsignadas_TXT_CantidadNecesaria_' + 0
                } else {
                    id = 'Content_DGV_ListaMateriasPrimasAsignadas_TXT_CantidadNecesaria_' + index
                }
                document.getElementById(id).autofocus = true;
                document.getElementById(id).focus();
                document.getElementById(id).select();
            }
        }

        function TXT_CantidadAgregar_onChange(txtCantidad) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1
            var idSelect = 'Content_DGV_ListaMateriasPrimas_CHK_Producto_' + index
            var CHK_Producto = document.getElementById(idSelect)
            var id = 'Content_DGV_ListaMateriasPrimas_HDF_IDProductoMateriaPrima_' + index
            var HDF_IDProducto = document.getElementById(id)
            var idProducto = HDF_IDProducto.value
            var cantidadProducto = txtCantidad.value * 1

            if (cantidadProducto === '' || cantidadProducto === 0 || cantidadProducto === '0') {
                txtCantidad.value = 1
            } else {
                if (cantidadProducto < 0 || cantidadProducto > 999) {
                    cantidadProducto = 1
                    txtCantidad.value = 1
                }
            }
            if (cantidadProducto > 0 && cantidadProducto < 1000) {
                productosAgregar.set(idProducto, cantidadProducto)
                actualizarMedidaUnidadesProductos(idProducto, cantidadProducto)
                CHK_Producto.checked = true;
            } 
        }

        function TXT_CantidadNecesaria_onChange(txtCantidad) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1

            var id = 'Content_DGV_ListaMateriasPrimasAsignadas_HDF_IDConsecutivoReceta_' + index
            var HDF_IDConsecutivoReceta = document.getElementById(id)
            var idConsecutivoReceta = HDF_IDConsecutivoReceta.value
            var cantidadNecesaria = txtCantidad.value * 1

            if (cantidadNecesaria === '' || cantidadNecesaria === 0 || cantidadNecesaria === '0') {
                txtCantidad.value = 1
            } else {
                if (cantidadNecesaria < 0 || cantidadNecesaria > 999) {
                    cantidadNecesaria = 1
                    txtCantidad.value = 1
                }
            }
            if (cantidadNecesaria >= 0 && cantidadNecesaria < 1000) {
                actualizarCantidadNecesariaProductos(idConsecutivoReceta, cantidadNecesaria)
            }
        }

        function TXT_UnidadesProducidas_OnTextChanged(txtCantidad) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1

            var id = 'Content_DGV_ListaMateriasPrimasAsignadas_HDF_IDConsecutivoReceta_' + index
            var HDF_IDConsecutivoReceta = document.getElementById(id)
            var idConsecutivoReceta = HDF_IDConsecutivoReceta.value
            var cantidadNecesaria = txtCantidad.value * 1

            if (cantidadNecesaria === '' || cantidadNecesaria === 0 || cantidadNecesaria === '0') {
                txtCantidad.value = 1
            } else {
                if (cantidadNecesaria < 0 || cantidadNecesaria > 999) {
                    cantidadNecesaria = 1
                    txtCantidad.value = 1
                }
            }
            if (cantidadNecesaria >= 0 && cantidadNecesaria < 1000) {
                actualizarCantidadNecesariaProductos(idConsecutivoReceta, cantidadNecesaria)
            }
        }

        function productosMarcados() {
            var table, tbody, i, rowLen, row, j, colLen, cell;

            table = document.getElementById('Content_DGV_ListaMateriasPrimas');
            tbody = table.tBodies[0];

            for (i = 0, rowLen = tbody.rows.length - 1; i < rowLen; i++) {
                var id = 'Content_DGV_ListaMateriasPrimas_HDF_IDProductoMateriaPrima_' + i

                var HDF_IDProducto = document.getElementById(id)
                var idProducto = HDF_IDProducto.value
                productosAgregar.forEach(function (valor, clave) {
                    if (idProducto === clave) {
                        var idSelect = 'Content_DGV_ListaMateriasPrimas_CHK_Producto_' + i
                        var CHK_Producto = document.getElementById(idSelect)
                        var idTXT_Cantidad = 'Content_DGV_ListaMateriasPrimas_TXT_MedidaUds_' + i
                        var TXT_Cantidad = document.getElementById(idTXT_Cantidad)
                        CHK_Producto.checked = true
                        TXT_Cantidad.value = valor
                    }
                })
            }
        }

        function cargarProductosAgregar() {
            document.getElementById('Content_LBL_GenerandoInforme').innerText = 'Agregando productos, espere por favor.'
            activarloading();
            var usuario = document.getElementById('Content_HDF_IDUsuario').value;
            var idProductoTerminado = document.getElementById('Content_HDF_IDProductoTerminado').value;
            var promises = [];
            productosAgregar.forEach(function (valor, clave) {
                promises.push(
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CrearReceta.aspx/BTN_Agregar_Click",
                        data: JSON.stringify({
                            "idProductoTerminado": idProductoTerminado,
                            "idProductoMateriaPrima": clave,
                            "usuario": usuario
                        }),
                        dataType: "json",
                        success: function (Result) {
                            console.dir(Result)
                        },
                        error: function (Result) {
                            alert("ERROR " + Result.status + ' ' + Result.statusText);
                        }
                    })
                )
            })
            Promise.all(promises).then(function () {
                if (productosAgregar.size > 0) {
                    __doPostBack('CargarMateriasPrimas')
                    alertifysuccess('Productos agregados con éxito.');
                    desactivarloading();
                } else {
                    alertifywarning('Por favor, seleccione al menos un producto para agregar a la receta.');
                    cargarFiltros();
                    desactivarloading();
                }
            });
        }

        function cargarProductosEliminar() {
            document.getElementById('Content_LBL_GenerandoInforme').innerText = 'Eliminando productos, espere por favor.'
            activarloading();
            var usuario = document.getElementById('Content_HDF_IDUsuario').value;

            var promises = [];
            productosEliminar.forEach(function (valor, clave) {
                promises.push(
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CrearReceta.aspx/BTN_Eliminar_Click",
                        data: JSON.stringify({
                            "idConsecutivoReceta": clave,
                            "usuario": usuario
                        }),
                        dataType: "json",
                        success: function (Result) {
                            console.dir(Result)
                        },
                        error: function (Result) {
                            alert("ERROR " + Result.status + ' ' + Result.statusText);
                        }
                    })
                )
            })
            Promise.all(promises).then(function () {
                if (productosEliminar.size > 0) {
                    __doPostBack('CargarMateriasPrimas')
                    alertifysuccess('Productos eliminados con éxito.');
                    desactivarloading();
                } else {
                    alertifywarning('Por favor, seleccione al menos un producto para eliminar de la receta.');
                    cargarFiltros();
                    desactivarloading();
                }
            });
        }

        function actualizarMedidaUnidadesProductos(idProducto, medidaUnidades) {
            var usuario = document.getElementById('Content_HDF_IDUsuario').value;
            
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "CrearReceta.aspx/BTN_ActualizarMedidaUnidades_Click",
                data: JSON.stringify({
                    "idProducto": idProducto,
                    "medidaUnidades": medidaUnidades,
                    "usuario": usuario
                }),
                dataType: "json",
                success: function (Result) {
                    // __doPostBack('CargarMateriasPrimas')
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })
        }

        function actualizarCantidadNecesariaProductos(idConsecutivoReceta, cantidadNecesaria) {
            var usuario = document.getElementById('Content_HDF_IDUsuario').value;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "CrearReceta.aspx/BTN_ActualizarCantidadNecesaria_Click",
                data: JSON.stringify({
                    "idConsecutivoReceta": idConsecutivoReceta,
                    "cantidadNecesaria": cantidadNecesaria,
                    "usuario": usuario
                }),
                dataType: "json",
                success: function (Result) {
                    // __doPostBack('CargarMateriasPrimas')
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })
        }

        function freezeMateriasPrimasHeader() {
            var GridId = "<%= DGV_ListaMateriasPrimas.ClientID %>";
            var ScrollHeight = 500;

            <%--var idProducto = document.getElementById('<%= HDF_IDProducto.ClientID %>').value

            
            if (idProducto != "0") {
                ScrollHeight = 75;
            }--%>

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
            $(<%= LB_Productos.ClientID %>).SumoSelect({ selectAll: false, placeholder: 'Producto' })
            $(<%= LB_Emisores.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Proveedor' })
        }

        $(document).ready(function () {
            cargarFiltros();
            productosAgregar = new Map();
            productosEliminar = new Map();
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
                    <li class="active">
                        <a href="CrearReceta.aspx">
                            <i class="fas fa-chart-line"></i>
                            <p>Gestión costos</p>
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
                                    <h1 class="h3 mb-2 text-gray-800" runat="server" id="H1_Title">Crear receta</h1>
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
                                            <div class="input-group no-border col-md-3" style="text-align: center; display: block;">
                                                <a href="Costos.aspx" class="btn btn-primary">
                                                    <i class=""></i>Costos directos
                                                </a>
                                            </div>
                                            <div class="input-group no-border col-md-3" style="text-align: center; display: block;">
                                                <a href="CrearReceta.aspx" class="btn btn-info">
                                                    <i class=""></i>Crear receta
                                                </a>
                                            </div>
                                            <div class="input-group no-border col-md-3" style="text-align: center; display: block;">
                                                <a href="VerReceta.aspx" class="btn btn-primary">
                                                    <i class=""></i>Ver recetas
                                                </a>
                                            </div>
                                        </div>
                                        <hr />
                                        <div class="row">
                                           <%-- <div class="input-group no-border col-md-3" style="text-align: center; display: block;">
                                                Seleccionar producto:
                                            </div>--%>
                                            <div class="input-group no-border col-md-3">
                                            </div>
                                            <div class="input-group no-border col-md-3">
                                                <asp:TextBox class="form-control" ID="TXT_BuscarProducto" runat="server" placeholder="Buscar producto..." OnTextChanged="FiltrarProductos_OnClick" AutoPostBack="true"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <div class="input-group-text">
                                                        <i class="nc-icon nc-zoom-split"></i>
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:HiddenField ID="HDF_IDProductoTerminado" runat="server" Value="0" Visible="true" />
                                            <div class="input-group no-border col-md-6" style="text-align: left; display: block;">
                                                <asp:ListBox class="form-control" runat="server" ID="LB_Productos" SelectionMode="Single" OnTextChanged="LB_Productos_OnClick" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <hr />
                            <div class="table">
                                <div class="col-md-7">                                        
                                    <asp:UpdatePanel ID="UpdatePanel_ListaMateriasPrimas" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="input-group no-border col-md-4">
                                                <asp:TextBox class="form-control" ID="TXT_BuscarMateriaPrima" runat="server" placeholder="Buscar materia prima..." OnTextChanged="FiltrarMateriaPrima_OnClick" AutoPostBack="true"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <div class="input-group-text">
                                                        <i class="nc-icon nc-zoom-split"></i>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:ListBox class="form-control" runat="server" ID="LB_Emisores" SelectionMode="Multiple" OnTextChanged="FiltrarMateriaPrima_OnClick" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                            <asp:GridView ID="DGV_ListaMateriasPrimas" Width="100%" runat="server" CssClass="table-responsive" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                AutoGenerateColumns="False" DataKeyNames="" 
                                                HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true" 
                                                EmptyDataText="Seleccione un producto para crear su receta." AllowSorting="true">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CHK_Producto" runat="server" onchange="CHK_Producto_onChange(this);" />
                                                            <asp:HiddenField ID="HDF_IDProductoMateriaPrima" runat="server" Value='<%# Eval("IDProductoMateriaPrima") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="NombreComercial" SortExpression="NombreComercial" HeaderText="Proveedor" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="DetalleProducto" SortExpression="DetalleProducto" HeaderText="Producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="PorcentajeImpuesto" SortExpression="PorcentajeImpuesto" HeaderText="IVA" DataFormatString="{0:n0}%" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="MontoImpuestoIncluido" SortExpression="MontoImpuestoIncluido" HeaderText="Monto IVAI" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="LBL_MedidaUds" runat="server" Text="Medida kls"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox class="form-control" TextMode="Number" MaxLength="0" min="0" max="999" Style="width: 130%" runat="server" ID="TXT_MedidaUds" Text='<%# Eval("MedidaUnidades") %>' 
                                                                onkeyup="TXT_CantidadAgregar_onKeyUp(this,event);" onchange="TXT_CantidadAgregar_onChange(this)" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CostoKilo" SortExpression="CostoKilo" HeaderText="Costo kilo" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="CostoGramo" SortExpression="CostoGramo" HeaderText="Costo gramo" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="col-md-1" style="padding-top: 17rem;">
                                    <asp:UpdatePanel ID="UpdatePanel_AsignarMateriaPrima" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>                                      
                                            <div class="input-group no-border col-md-12" style="text-align: center; display: block;">
                                                <asp:LinkButton UseSubmitBehavior="false" class="btn btn-outline-info btn-round-mant" ID="BTN_Agregar" runat="server" OnClientClick="cargarProductosAgregar();"> 
                                                <i class="fas fa-caret-right" style="font-size: xx-large;"></i></asp:LinkButton>
                                            </div>
                                            <div class="input-group no-border col-md-12" style="text-align: center; display: block;">
                                                <asp:LinkButton UseSubmitBehavior="false" class="btn btn-outline-danger btn-round-mant" ID="BTN_Eliminar" runat="server" OnClientClick="cargarProductosEliminar();"> 
                                                <i class="fas fa-caret-left" style="font-size: xx-large;"></i></asp:LinkButton>
                                            </div>                                        
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="col-md-4">
                                    <asp:UpdatePanel ID="UpdatePanel_MateriasPrimasAsignadas" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <h5 class="modal-title" runat="server" id="H5_Title" visible="false"></h5>
                                            <asp:GridView ID="DGV_ListaMateriasPrimasAsignadas" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                AutoGenerateColumns="False" DataKeyNames="" ShowFooter="true"
                                                HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true"
                                                EmptyDataText="Sin materia prima asignada." AllowSorting="true"
                                                OnRowDataBound="DGV_ListaMateriasPrimasAsignadas_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CHK_ProductoMateriaPrimaAsignada" runat="server" onchange="CHK_ProductoEliminar_onChange(this);" />
                                                            <asp:HiddenField ID="HDF_IDConsecutivoReceta" runat="server" Value='<%# Eval("IDConsecutivoReceta") %>' />
                                                            <asp:HiddenField ID="HDF_IDProductoMateriaPrimaAsignada" runat="server" Value='<%# Eval("ProductoMateriaPrimaID") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DetalleProducto" SortExpression="DetalleProducto" HeaderText="Descripcion" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="CostoKilo" SortExpression="CostoKilo" HeaderText="Costo x Kilo" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="CostoGramo" SortExpression="CostoGramo" HeaderText="Costo x Gramo" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                        
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="LBL_CantidadNecesaria" runat="server" Text="Cantidad necesaria"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox class="form-control" TextMode="Number" MaxLength="0" min="0" max="999" Style="width: 100%" runat="server" ID="TXT_CantidadNecesaria" Text='<%# Eval("CantidadNecesaria") %>'
                                                                onkeyup="TXT_CantidadNecesaria_onKeyUp(this,event);" onchange="TXT_CantidadNecesaria_onChange(this)" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
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
                                            <div class="row">
                                                <div class="col-md-9" style="text-align: right;">
                                                    <asp:Label ID="LBL_CantidadProducida" runat="server" Visible="false">Unidades producidas:</asp:Label>                                                    
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="TXT_CantidadProducida" runat="server" TextMode="Number" CssClass="form-control" Visible="false" onchange="TXT_UnidadesProducidas_OnTextChanged();" AutoPostBack="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-9">                                        
                                                    <asp:Label runat="server" ID="LBL_Empleado" Visible="false">Seleccionar empleado:</asp:Label>
                                                    <asp:DropDownList class="form-control" style="font-size: 18px;" ID="DDL_Empleado" runat="server" Visible="false" OnSelectedIndexChanged="DDL_Empleado_OnChange" AutoPostBack="true"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="LBL_CantidadMinutos" runat="server" Visible="false">Cantidad minutos:</asp:Label>
                                                    <asp:TextBox ID="TXT_CantidadMinutos" runat="server" TextMode="Number" CssClass="form-control" Visible="false" AutoPostBack="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <asp:GridView ID="DGV_ListaEmpleados" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                AutoGenerateColumns="False" DataKeyNames="" ShowFooter="true"
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
                                            <h5 class="modal-title" runat="server" id="H5_Subtitle" visible="false"></h5>
                                            <asp:GridView ID="DGV_ListaCostosProduccion" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                AutoGenerateColumns="False" DataKeyNames="" ShowFooter="true"
                                                HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true"
                                                EmptyDataText="Sin costos registrados." AllowSorting="true"
                                                OnRowDataBound="DGV_ListaCostosProduccion_RowDataBound">
                                                <Columns>                                                    
                                                    <asp:BoundField DataField="Descripcion" SortExpression="Descripcion" HeaderText="Factor" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="Valor" SortExpression="Valor" HeaderText="Porcentaje" DataFormatString="{0:n0}%" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
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
                                            <br />
                                            <h5 class="modal-title" runat="server" id="H5_Resumen" visible="false"></h5>
                                            <asp:GridView ID="DGV_ListaResumen" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                AutoGenerateColumns="False" DataKeyNames="" 
                                                HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true"
                                                EmptyDataText="Sin registros." AllowSorting="true">
                                                <Columns>                                                    
                                                    <asp:BoundField DataField="Descripcion" SortExpression="Descripcion" HeaderText="Descripción" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="Valor" SortExpression="Valor" HeaderText="" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="Porcentaje" SortExpression="Porcentaje" HeaderText="" DataFormatString="{0:n0}%" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                    
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
    </div>
</asp:Content>
