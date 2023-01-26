<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleODP.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ControlPedidos.DetalleODP" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Detalle orden producción</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <style>
        .ir-arriba {
            display:block;
            background-repeat:no-repeat;
            font-size:20px;
            color:black;
            cursor:pointer;
            position:fixed;
            bottom:10px;
            right:10px;
            z-index:2;
        }
    </style>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.7/jquery.min.js"></script>
    <script src="../Assets/js/qz-tray.js"></script>
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
            var value = $(<%= DDL_Reportes.ClientID %>)[0].value
            if (value !== 2 && value !== '2') {
                document.getElementById('fade2').style.display = 'block';
                document.getElementById('modalloading').style.display = 'block';
            }
        }

        function desactivarloading() {
            document.getElementById('fade2').style.display = 'none';
            document.getElementById('modalloading').style.display = 'none';
        }

        function abrirModalOrdenProduccion() {
            document.getElementById('BTN_ModalOrdenProduccion').click()
        }

        function cerrarModalOrdenProduccion() {
            document.getElementById('BTN_ModalOrdenProduccion').click()
        }

        function abrirModalPedidosODP() {
            document.getElementById('BTN_ModalPedidosODP').click()
        }

        function cerrarModalPedidosODP() {
            document.getElementById('BTN_ModalPedidosODP').click()
        }

        function abrirModalSeleccionarImpresora() {
            document.getElementById('BTN_ModalSeleccionarImpresora').click()
        }

        function cerrarModalSeleccionarImpresora() {
            document.getElementById('BTN_ModalSeleccionarImpresora').click()
        }

        function imprimir(categoria, codigoODP, index, printer) {
            imprimir2(codigoODP, printer, categoria, index, 0, 31)
        }

        function imprimir2(codigoODP, printer, categoria, index, indexInicio, indexFin) {
            var listaProductos = 'Content_DGV_ListaCategorias_DGV_ListaProductos_' + index;
            var table, tbody, i, rowLen, row, j, colLen, cell, resultHTML;

            resultHTML = '<tbody><tr class="table" align="center" style="border-color:#51CBCE;">'

            table = document.getElementById(listaProductos);
            tbody = table.tBodies[0];
            var pag = Math.trunc(index / 30 + 1)
            var totalPags = Math.trunc(tbody.rows.length / 30)
            totalPags += 0 < tbody.rows.length % 31 ? 1 : 0

            if (indexInicio < tbody.rows.length) {
                for (i = indexInicio, rowLen = tbody.rows.length; i < rowLen; i++) {
                    if (i < indexFin) {
                        row = tbody.rows[i];
                        for (j = 0, colLen = row.cells.length; j < colLen; j++) {
                            cell = row.cells[j];
                            if (i == 0) {
                                if (j == 0 || j == 1) {
                                    resultHTML += '<th scope="col">' + cell.innerHTML + '</th>'
                                }
                            } else {
                                if (j == 0) {
                                    resultHTML += '</tr><tr>'
                                    resultHTML += '<td align="center" style="color:Black;"><strong>' + cell.innerHTML + '</strong></td>'
                                } else if (j == 1) {
                                    resultHTML += '<td align="center" style="color:Black;"><strong>' + cell.innerHTML + '</strong></td>'
                                    resultHTML += '</tr>'
                                }
                            }
                        }
                    }                    
                }
                resultHTML += '</tbody>'
                var d = new Date(),
                year = d.getFullYear(),
                month = d.getMonth() + 1,
                day = d.getDate(),
                hours = d.getHours(),
                minute = d.getMinutes(),
                second = d.getSeconds(),
                ap = 'AM';
                if (hours > 11) { ap = 'PM'; }
                if (hours > 12) { hours = hours - 12; }
                if (hours == 0) { hours = 12; }
                if (month < 10) { month = "0" + month; }
                if (day < 10) { day = "0" + day; }
                if (minute < 10) { minute = "0" + minute; }

                var fecha = day + '/' + month + '/' + year + ' ' + hours + ':' + minute + ' ' + ap

                qz.websocket.connect().then(function () {
                    return qz.printers.find(printer);
                }).then(function (found) {
                    var config = qz.configs.create(found);
                    var data = [{
                        type: 'pixel',
                        format: 'html',
                        flavor: 'plain',
                        data: '<html>' +
                                '<head><title>' + document.title + '</title></head>' +
                                '<body>' +
                                    '<h2><strong>' + codigoODP + '</strong></h2><br /><br />' +
                                    '<h2><strong>Categoría:</strong> ' + categoria + '</h2><br /><br />' +
                                    '<table>' + resultHTML + '</table><br />' +
                                    '<h3 style="text-align: center;"><strong> *** FIN *** </strong></h3>' +
                                    '<h4 style="text-align: left;"><strong> Página: ' + pag + ' de ' + totalPags + '</strong></h4><br />' +
                                    '<h4 style="text-align: left;"><strong> Tiquete generado el: ' + fecha + '</strong></h4><br />' +
                                '</body>' +
                               '</html>'
                    }];
                    return qz.print(config, data).catch(function (e) { console.error(e); });
                }).catch(function (error) {
                    alert(error);
                }).finally(function () {
                    return qz.websocket.disconnect().then(function () {
                        imprimir2(codigoODP, printer, categoria, index, indexInicio + 31, indexFin + 31)
                    });
                });
            } else {
                alertifysuccess('Impresión finalizada.');
            }
        }

        function TXT_Cantidad_onKeyUp(txtCantidad, e) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1 + 1
            if (e.keyCode === 13) {
                var rows = $(<%= DGV_ListaProductosODP.ClientID %>)[0].rows.length - 1
                var id = ''
                if (index === rows) {
                    id = 'Content_DGV_ListaProductosODP_TXT_Cantidad_' + 0
                } else {
                    id = 'Content_DGV_ListaProductosODP_TXT_Cantidad_' + index
                }
                document.getElementById(id).autofocus = true;
                document.getElementById(id).focus();
                document.getElementById(id).select();
            }
        }

        function TXT_Cantidad_onChange(txtCantidad) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1
            var idUnidades = 'Content_DGV_ListaProductosODP_DDL_Unidades_' + index
            var idDecenas = 'Content_DGV_ListaProductosODP_DDL_Decenas_' + index
            var idCentenas = 'Content_DGV_ListaProductosODP_DDL_Centenas_' + index
            var cantidadProducto = txtCantidad.value
            if (cantidadProducto !== '') {
                var unds = cantidadProducto % 10;
                var decs = Math.trunc(cantidadProducto / 10) % 10;
                var cents = Math.trunc(cantidadProducto / 100);
                if (cantidadProducto >= 0 && cantidadProducto < 1000) {
                    document.getElementById(idUnidades).value = unds;
                    document.getElementById(idDecenas).value = decs;
                    document.getElementById(idCentenas).value = cents;
                    txtCantidad.value = cantidadProducto;
                    guardarProductoODP(index, cantidadProducto);
                } else {
                    unds = document.getElementById(idUnidades).value * 1;
                    decs = document.getElementById(idDecenas).value * 10;
                    cents = document.getElementById(idCentenas).value * 100;
                    cantidadProducto = cents + decs + unds;
                    txtCantidad.value = cantidadProducto;
                }
            }
            else {
                txtCantidad.value = '0';
                document.getElementById(idUnidades).value = '0';
                document.getElementById(idDecenas).value = '0';
                document.getElementById(idCentenas).value = '0';
                guardarProductoODP(index, 0);
            }
        }

        function guardarProductoODP(index, cantidad) {
            var id = 'Content_DGV_ListaProductosODP_HDF_IDODPDetalle_' + index
            var HDF_IDODPDetalle = document.getElementById(id)
            var idODPDetalle = HDF_IDODPDetalle.value
            var usuario = document.getElementById('Content_HDF_IDUsuario').value;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "DetalleODP.aspx/guardarProductoODP",
                data: JSON.stringify({
                    "idODPDetalle": idODPDetalle,
                    "cantidadProducto": cantidad,
                    "usuario": usuario
                }),
                dataType: "json",
                success: function (Result) {
                    console.log(Result)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })
        }

        function estilosElementosBloqueados() {
            document.getElementById('<%= TXT_CodigoODP.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_CodigoODP.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_TotalProductos.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_TotalProductos.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_CostoODP.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_CostoODP.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_EstadoODP.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_EstadoODP.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_FechaODP.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_FechaODP.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_HoraODP.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_HoraODP.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_NombreImpresora.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_NombreImpresora.ClientID %>').classList.add('form-control')
            document.getElementById('<%= DDL_PlantaProduccion.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= DDL_PlantaProduccion.ClientID %>').classList.add('form-control')
            document.getElementById('<%= DDL_Propietario.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= DDL_Propietario.ClientID %>').classList.add('form-control')
        }

        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Assets/img/minus.png");
        });

        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../Assets/img/plus.png");
            $(this).closest("tr").next().remove();
        });

        function configurarImpresora() {
            qz.websocket.connect().then(function () {
                return qz.printers.find();
            }).then(function (found) {
                __doPostBack('DDL_ImpresorasLoad', found)
            }).catch(function (error) {
                alert(error);
            }).finally(function () {
                return qz.websocket.disconnect();
            });
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
        
        $(document).ready(function () {
            estilosElementosBloqueados();
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="modalloading" class="loading">
        <img src="../Assets/img/cargando.gif" width="100" height="100" /><br />
        <asp:Label runat="server" ID="LBL_GenerandoInforme" style="color: white;" Text="Generando informe espere por favor..."></asp:Label>
    </div>
    <div id="fade2" class="overlayload"></div>
    <a class="ir-arriba"  href="javascript:configurarImpresora();" title="Impresora">
        <span class="fa-stack">
            <i class="fa fa-circle fa-stack-2x"></i>
            <i class="fa fa-print fa-stack-1x fa-inverse"></i>
        </span>
    </a>
    <div class="wrapper">
        <asp:HiddenField ID="HDF_IDUsuario" runat="server" Value="0" Visible="true" />
        <asp:HiddenField ID="HDF_IDODP" runat="server" Value="0" Visible="false" />
        <asp:HiddenField ID="HDF_EstadoODP" runat="server" Value="" Visible="false" />     
        <asp:HiddenField ID="HDF_IDsPedidos" runat="server" Value="" Visible="false" />
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
                    <li class="active">
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
                        <a href="#" onclick="seleccionarNegocio('panaderia');">
                            <i class="fas fa-chart-line"></i>
                            <p>Costos panadería</p>
                        </a>
                    </li>
                    <li>
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
                    <h1 class="h3 mb-2 text-gray-800">Detalle orden de producción</h1>
                    <br />
                    <!-- DataTales Example -->
                    <div class="card shadow mb-4">
                        <asp:UpdatePanel ID="UpdatePanel_Header" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="card-header py-3">
                                    <div class="form-row">
                                        <div class="form-group col-md-2.5">
                                            <label for="TXT_CodigoODP">Número orden producción</label>
                                            <asp:TextBox class="form-control" ID="TXT_CodigoODP" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="TXT_TotalProductos">Total de productos</label>
                                            <asp:TextBox class="form-control" ID="TXT_TotalProductos" runat="server" TextMode="Number" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2.5">
                                            <label for="TXT_CostoODP">Monto orden producción</label>
                                            <asp:TextBox class="form-control" ID="TXT_CostoODP" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="TXT_EstadoODP">Estado</label>
                                            <asp:TextBox class="form-control" ID="TXT_EstadoODP" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <label for="TXT_FechaODP">Fecha y hora orden producción</label>
                                            <div class="form-row">
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="TXT_FechaODP" runat="server" CssClass="form-control" TextMode="Date" format="dd/MM/yyyy" Enabled="false"></asp:TextBox>
                                                </div>
                                                <div class="col-md-5">
                                                    <asp:TextBox ID="TXT_HoraODP" runat="server" CssClass="form-control" TextMode="Time" format="HH:mm" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-4">
                                            <label for="DDL_Propietario">Solicitante</label>
                                            <asp:DropDownList class="form-control" ID="DDL_Propietario" runat="server" Enabled="false"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label for="DDL_PlantaProduccion">Planta Producción</label>
                                            <asp:DropDownList class="form-control" ID="DDL_PlantaProduccion" runat="server" Enabled="false"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label for="DDL_Reportes">Reportes</label>
                                            <asp:DropDownList class="form-control" ID="DDL_Reportes" runat="server" AutoPostBack="true" onchange="activarloading();estilosElementosBloqueados();" OnSelectedIndexChanged="DDL_Reportes_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Seleccione</asp:ListItem>
                                                <asp:ListItem Value="1">Reporte orden producción</asp:ListItem>
                                                <asp:ListItem Value="2">Descargar orden producción</asp:ListItem>
                                            </asp:DropDownList>
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
                                        <div class="col-md-6">                                                                                  
                                            <asp:Button UseSubmitBehavior="false" ID="BTN_ConfirmarODP" runat="server" Text="Confirmar orden producción" CssClass="btn btn-success" OnClick="BTN_ConfirmarODP_Click"></asp:Button>
                                            <asp:Button UseSubmitBehavior="false" ID="BTN_ImprimirOrdenProduccion" runat="server" Text="Imprimir orden producción" CssClass="btn btn-info" OnClick="BTN_ImprimirOrdenProduccion_Click"></asp:Button>
                                        </div>
                                        <div class="col-md-6" style="text-align: right;">                                             
                                            <asp:Button UseSubmitBehavior="false" ID="BTN_VerDetallePedidos" runat="server" Text="Ver pedidos" CssClass="btn btn-primary" OnClick="BTN_VerDetallePedidos_Click"></asp:Button>
                                            <asp:Button UseSubmitBehavior="false" ID="BTN_CompletarODP" runat="server" Text="Completar orden producción" CssClass="btn btn-success" OnClick="BTN_CompletarODP_Click"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="DDL_Reportes" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <div class="card-body">
                            <div class="card-body">
                                <asp:UpdatePanel ID="UpdatePanel_FiltrosProductos" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>                           
                                        <div class="input-group no-border col-md-6">
                                            <asp:TextBox class="form-control" ID="TXT_Buscar" runat="server" placeholder="Buscar producto..." OnTextChanged="TXT_Buscar_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                            <div class="input-group-append">
                                                <div class="input-group-text">
                                                    <i class="nc-icon nc-zoom-split"></i>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="table">
                                <asp:UpdatePanel ID="UpdatePanel_ListaProductosODP" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaProductosODP" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDODPDetalle,ODPID,ProductoID" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_ListaProductosODP_Sorting"
                                            OnRowCommand="DGV_ListaProductosODP_RowCommand"
                                            OnRowDataBound="DGV_ListaProductosODP_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="DescripcionProducto" SortExpression="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="CostoProducto" SortExpression="CostoProducto" HeaderText="Costo unitario" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:n2}"></asp:BoundField>
                                                <asp:BoundField DataField="CantidadSolicitada" SortExpression="CantidadSolicitada" HeaderText="Cantidad solicitada" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:n0}"></asp:BoundField>                                                
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Cantidad" runat="server" Text="Cantidad producida"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="row">
                                                            <asp:TextBox class="form-control" TextMode="Number" MaxLength="2" min="0" max="999" style="width: 30%" runat="server" ID="TXT_Cantidad" 
                                                                onkeyup="TXT_Cantidad_onKeyUp(this, event);" onchange="TXT_Cantidad_onChange(this);" Text='<%#Eval("CantidadProducida") %>' />                                                            
                                                            <asp:DropDownList class="form-control" style="width: 20%" runat="server" ID="DDL_Centenas" 
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
                                                            <asp:DropDownList class="form-control" style="width: 25%" runat="server" ID="DDL_Decenas" 
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
                                                            <asp:DropDownList class="form-control" style="width: 25%" runat="server" ID="DDL_Unidades"
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
                                                            <asp:HiddenField ID="HDF_IDODPDetalle" runat="server" Value='<%# Eval("IDODPDetalle") %>' />                                                        
                                                        </div>
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

    <button type="button" id="BTN_ModalOrdenProduccion" data-toggle="modal" data-target="#ModalOrdenProduccion" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalOrdenProduccion" tabindex="-1" role="dialog" aria-labelledby="popOrdenProduccion" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalOrdenProduccion" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server">Orden de producción</h5>
                        </div>
                        <div class="modal-body">                            
                            <div class="table-responsive" id="tableCategorias">
                                <asp:UpdatePanel ID="UpdatePanel_ListaProductos" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="col-md-6" style="margin-bottom: 2rem;">
                                            <label for="TXT_NombreImpresora">Nombre impresora</label>
                                            <asp:TextBox class="form-control" ID="TXT_NombreImpresora" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <br />
                                        <br />
                                        <asp:GridView ID="DGV_ListaCategorias" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="false" DataKeyNames="IDCategoria,DescripcionCategoria" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_ListaCategorias_Sorting"
                                            OnRowCommand="DGV_ListaCategorias_RowCommand"
                                            OnRowDataBound="DGV_ListaCategorias_RowDataBound">
                                            <Columns>   
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="Lbl_VerDetalle" runat="server" Text="Ver detalle"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="table" id="tableProductos">
                                                            <img alt="" style="cursor: pointer" src="../Assets/img/plus.png" />
                                                            <asp:Panel ID="pnlListaProductos" runat="server" Style="display: none;">
                                                                <asp:GridView ID="DGV_ListaProductos" runat="server" AutoGenerateColumns="false" DataKeyNames="IDProducto,DescripcionProducto,Categoria" CssClass="ChildGrid"
                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" 
                                                                    GridLines="None" ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                             
                                                                        <asp:BoundField DataField="CantidadSolicitada" HeaderText="Cantidad solicitada" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>                                             
                                                <asp:BoundField DataField="DescripcionCategoria" SortExpression="DescripcionCategoria" HeaderText="Categoría" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="Lbl_VerDetalle" runat="server" Text="Acciones"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnk_Imprimir" runat="server" class="btn btn-secondary" CommandName="imprimir" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>">
                                                            <span>Imprimir</span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="modal-footer">                            
                            <div style="text-align: right;">
                                <asp:Button UseSubmitBehavior="false" ID="BTN_CerrarModalCrearPedido" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-primary" />                                
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <button type="button" id="BTN_ModalPedidosODP" data-toggle="modal" data-target="#ModalPedidosODP" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalPedidosODP" tabindex="-1" role="dialog" aria-labelledby="popModalPedidosODP" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalPedidosODP" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server">Pedidos de la orden de producción</h5>
                        </div>
                        <div class="modal-body">                            
                            <div class="table-responsive" id="tablePedidosODP">
                                <asp:UpdatePanel ID="UpdatePanel_DGVPedidosODP" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaPedidosODP" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="ODPID,PedidoID" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnRowDataBound="DGV_ListaPedidosODP_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="Lbl_VerDetalle" runat="server" Text="Ver detalle"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="table" id="tableProductos">
                                                            <img alt="" style="cursor: pointer" src="../Assets/img/plus.png" />
                                                            <asp:Panel ID="pnlListaProductos" runat="server" Style="display: none;">
                                                                <asp:GridView ID="DGV_ListaProductos" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductoID,PedidoID" CssClass="ChildGrid"
                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" 
                                                                    GridLines="None" ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                             
                                                                        <asp:BoundField DataField="CantidadProduccion" HeaderText="Cantidad solicitada" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="PedidoID" SortExpression="PedidoID" HeaderText="Número pedido" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionPuntoVenta" SortExpression="DescripcionPuntoVenta" HeaderText="Punto venta" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="modal-footer">                            
                            <div style="text-align: right;">
                                <asp:Button UseSubmitBehavior="false" ID="Button1" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-primary" />                                
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <button type="button" id="BTN_ModalSeleccionarImpresora" data-toggle="modal" data-target="#ModalSeleccionarImpresora" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-md" id="ModalSeleccionarImpresora" tabindex="-1" role="dialog" aria-labelledby="popSeleccionarImpresora" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_SeleccionarImpresora" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-md">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server">Seleccionar impresora</h5>
                        </div>
                        <div class="modal-body">   
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>                         
                                    <div style="text-align: left;">
                                        <asp:DropDownList class="form-control" ID="DDL_Impresoras" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_Impresoras_OnSelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <div style="text-align: right;">
                                <asp:Button UseSubmitBehavior="false" ID="BTN_CerrarModalSeleccionarImpresora" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-secondary" />                                
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
