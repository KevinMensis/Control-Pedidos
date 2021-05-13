<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetallePedido.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ControlPedidos.DetallePedido" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Detalle pedido</title>
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

        function marcarProductosCantidadCero(index) {
            id = 'Content_DGV_ListaProductos_TXT_Cantidad_' + 0
            document.getElementById(id).autofocus = true;
            document.getElementById(id).focus();
            document.getElementById(id).select();
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
            document.getElementById('<%= TXT_NombreImpresora.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_NombreImpresora.ClientID %>').classList.add('form-control')
        }

        function abrirModalDetallePedido() {
            document.getElementById('BTN_ModalDetallePedido').click()
        }

        function cerrarModalDetallePedido() {
            document.getElementById('BTN_ModalDetallePedido').click()
        }

        function abrirModalSeleccionarImpresora() {
            document.getElementById('BTN_ModalSeleccionarImpresora').click()
        }

        function cerrarModalSeleccionarImpresora() {
            document.getElementById('BTN_ModalSeleccionarImpresora').click()
        }

        function enterClickAgregar(txtCantidad, e) {
            console.dir(e)
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1 + 1
            var rows = $(<%= DGV_ListaProductos.ClientID %>)[0].rows.length - 1
            var id = ''
            if (index === rows) {
                id = 'Content_DGV_ListaProductos_TXT_Cantidad_' + 0
            } else {
                id = 'Content_DGV_ListaProductos_TXT_Cantidad_' + index
            }
            document.getElementById(id).autofocus = true;
            document.getElementById(id).focus();
            document.getElementById(id).select();
            return false;
        }

        function enterClickAgregar2(txtCantidad) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1 + 1
            var rows = $(<%= DGV_ListaProductosSinAgregar.ClientID %>)[0].rows.length - 1
            var id = ''
            if (index === rows) {
                id = 'Content_DGV_ListaProductosSinAgregar_TXT_CantidadAgregar_' + 0
            } else {
                id = 'Content_DGV_ListaProductosSinAgregar_TXT_CantidadAgregar_' + index
            }
            document.getElementById(id).autofocus = true;
            document.getElementById(id).focus();
            document.getElementById(id).select();
            return false;
        }

        function enterCantidad(index) {
            console.log(123)
            var index = index + 1
            var rows = $(<%= DGV_ListaProductos.ClientID %>)[0].rows.length - 1
            var id = ''
            if (index === rows) {
                id = 'Content_DGV_ListaProductos_TXT_Cantidad_' + 0
            } else {
                id = 'Content_DGV_ListaProductos_TXT_Cantidad_' + index
            }
            document.getElementById(id).autofocus = true;
            document.getElementById(id).focus();
            document.getElementById(id).select();
        }

        function enterCantidad2(index) {
            var index = index + 1
            var rows = $(<%= DGV_ListaProductosSinAgregar.ClientID %>)[0].rows.length - 1
            var id = ''
            if (index === rows) {
                id = 'Content_DGV_ListaProductosSinAgregar_TXT_CantidadAgregar_' + 0
            } else {
                id = 'Content_DGV_ListaProductosSinAgregar_TXT_CantidadAgregar_' + index
            }
            document.getElementById(id).autofocus = true;
            document.getElementById(id).focus();
            document.getElementById(id).select();
        }

        function imprimir(estado, codigoPedido, sucursal, plantaProduccion, printer) {
            var listaProductos = 'Content_DGV_DetallePedido';
            var categorias = []
            table = document.getElementById(listaProductos);
            tbody = table.tBodies[0];

            for (i = 0, rowLen = tbody.rows.length; i < rowLen; i++) {
                row = tbody.rows[i];
                for (j = 0, colLen = row.cells.length; j < colLen; j++) {
                    cell = row.cells[j];
                    if (j == 0) {
                        if (!categorias.includes(cell.innerHTML) && (cell.innerHTML !== 'Categoría' && cell.innerHTML !== 'Categoria')) {
                            categorias.push(cell.innerHTML)
                        }
                    }
                }
            }
            imprimir2(estado, codigoPedido, sucursal, plantaProduccion, printer, categorias, 0);
        }

        function imprimir2(estado, codigoPedido, sucursal, plantaProduccion, printer, categorias, index) {
            if (index < categorias.length) {
                var listaProductos = 'Content_DGV_DetallePedido';
                var table, tbody, i, rowLen, row, j, colLen, cell, resultHTML;

                resultHTML = '<tbody><tr class="table" align="center" style="border-color:#51CBCE;">'

                table = document.getElementById(listaProductos);
                tbody = table.tBodies[0];

                for (i = 0, rowLen = tbody.rows.length; i < rowLen; i++) {
                    row = tbody.rows[i];
                    for (j = 0, colLen = row.cells.length; j < colLen; j++) {
                        cell = row.cells[j];
                        if (categorias[index] === row.cells[0].innerHTML || i === 0){
                            if (i == 0) {
                                if (j == 2 || j == 3) {
                                    resultHTML += '<th scope="col">' + cell.innerHTML + '</th>'
                                }
                            } else {
                                if (j == 2) {
                                    resultHTML += '</tr><tr>'
                                    resultHTML += '<td align="center" style="color:Black;"><strong>' + cell.innerHTML + '</strong></td>'
                                } else if (j == 3) {
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
                                    '<h2><strong>' + codigoPedido + ' - ' + 'Estado pedido: ' + estado + '</strong></h2>' +
                                    '<h2><strong>Sucursal:</strong> ' + sucursal + '</h2>' +
                                    '<h2><strong>Planta produccion:</strong> ' + plantaProduccion + '</h2><br /><br />' +
                                    '<table>' + resultHTML + '</table><br />' +
                                    '<h3 style="text-align: center;"><strong> *** FIN *** </strong></h3>' +
                                    '<h4 style="text-align: left;"><strong> Tiquete generado el: ' + fecha + '</strong></h4><br />' +
                                '</body>' +
                               '</html>'
                    }];
                    return qz.print(config, data).catch(function (e) { console.error(e); });
                }).catch(function (error) {
                    alert(error);
                }).finally(function () {
                    return qz.websocket.disconnect().then(function () { imprimir2(estado, codigoPedido, sucursal, plantaProduccion, printer, categorias, index + 1) });
                });
            } else {
                cerrarModalDetallePedido();
                alertifysuccess('Impresión finalizada.');
            }
        }

        function configurarImpresora() {
            qz.websocket.connect().then(function () {
                return qz.printers.find();
            }).then(function (found) {
                __doPostBack('DDL_ImpresorasLoad', found)
            }).catch(function (error) {
                alert(error);
            }).finally(function () {
                return qz.websocket.disconnect()
            });
        }

        function cargarFiltros() {
            $(<%= LB_Categoria.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Categoria' })
        }

        $(document).ready(function () {
            estilosElementosBloqueados()
            cargarFiltros()
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
                           <p>Ordenes de producción</p>
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
                            <img style="width: 25%; display: block; margin-left: 30%;" src="../Assets/img/logoMensis.png" />
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
                                        <div class="form-group col-md-2">
                                            <label for="TXT_CodigoPedido">Número pedido</label>
                                            <asp:TextBox class="form-control" ID="TXT_CodigoPedido" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="TXT_TotalProductos">Total de productos</label>
                                            <asp:TextBox class="form-control" ID="TXT_TotalProductos" runat="server" TextMode="Number" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="TXT_MontoPedido">Monto del pedido</label>
                                            <asp:TextBox class="form-control" ID="TXT_MontoPedido" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="TXT_EstadoPedido">Estado del pedido</label>
                                            <asp:TextBox class="form-control" ID="TXT_EstadoPedido" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-4">                                            
                                            <div class="form-row">                                                
                                                <div class="col-md-7">
                                                    <label for="TXT_FechaPedido">Fecha pedido</label>
                                                    <asp:TextBox ID="TXT_FechaPedido" runat="server" CssClass="form-control" TextMode="Date" format="dd/MM/yyyy" Enabled="false"></asp:TextBox>
                                                </div>                                                
                                                <div class="col-md-5">
                                                    <label for="TXT_HoraPedido">Hora pedido</label>
                                                    <asp:TextBox ID="TXT_HoraPedido" runat="server" CssClass="form-control" TextMode="Time" format="HH:mm" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-3">
                                            <label for="DDL_Propietario">Solicitante</label>
                                            <asp:DropDownList class="form-control" ID="DDL_Propietario" runat="server" AutoPostBack="true" OnSelectedIndexChanged="BTN_GuardarPedido_Click"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <label for="DDL_PlantaProduccion">Planta Producción</label>
                                            <asp:DropDownList class="form-control" ID="DDL_PlantaProduccion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="BTN_GuardarPedido_Click"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <label for="DDL_PuntoVenta">Punto Venta</label>
                                            <asp:DropDownList class="form-control" ID="DDL_PuntoVenta" runat="server" AutoPostBack="true" OnSelectedIndexChanged="BTN_GuardarPedido_Click"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <label for="DDL_Reportes">Reportes</label>
                                            <asp:DropDownList class="form-control" ID="DDL_Reportes" runat="server" AutoPostBack="true" onchange="activarloading();estilosElementosBloqueados();" OnSelectedIndexChanged="DDL_Reportes_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Seleccione</asp:ListItem>
                                                <asp:ListItem Value="1">Reporte pedido</asp:ListItem>
                                                <asp:ListItem Value="2">Descargar pedido</asp:ListItem>
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
                                        <div class="col-md-7">
                                            <asp:Button ID="BTN_AgregarProductos" runat="server" UseSubmitBehavior="false" Text="Agregar productos" CssClass="btn btn-primary" OnClientClick="estilosElementosBloqueados();" OnClick="BTN_CargarProductos_Click"></asp:Button>                                        
                                            <asp:Button ID="BTN_ConfirmarPedido" runat="server" UseSubmitBehavior="false" Text="Confirmar pedido" CssClass="btn btn-secondary" OnClick="BTN_ConfirmarPedido_Click"></asp:Button>
                                        </div>
                                        <div class="col-md-5" style="text-align: right;">
                                            <asp:Button ID="BTN_AbrirModalDetallePedido" runat="server" UseSubmitBehavior="false" Text="Imprimir pedido" CssClass="btn btn-info" OnClick="BTN_AbrirModalDetallePedido_Click"></asp:Button>                                            
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
                                        <div class="input-group no-border col-md-3">
                                            <asp:Button ID="BTN_VerTodosProductos" Visible="false" runat="server" UseSubmitBehavior="false" Text="Ver todos los productos" CssClass="btn btn-secondary" style="margin: 0px;" OnClientClick="estilosElementosBloqueados();" OnClick="BTN_VerTodosProductos_Click"></asp:Button>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="table">
                                <asp:UpdatePanel ID="UpdatePanel_ListaProductos" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaProductos" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDPedidoDetalle,PedidoID,ProductoID,Categoria" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_ListaProductos_Sorting"
                                            OnRowCommand="DGV_ListaProductos_RowCommand"
                                            OnRowDataBound="DGV_ListaProductos_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="DescripcionProducto" SortExpression="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="PrecioProducto" SortExpression="PrecioProducto" HeaderText="Precio unitario" DataFormatString="{0:n2}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Cantidad" runat="server" Text="Cantidad"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="row">
                                                            <asp:TextBox class="form-control" TextMode="Number" MaxLength="2" min="0" max="99" style="width: 40%" runat="server" ID="TXT_Cantidad" 
                                                               OnTextChanged="TXT_Cantidad_OnTextChanged" AutoPostBack="true" onchange="enterClickAgregar(this,event);" Text='<%#Eval("CantidadProduccion") %>' />                                                            
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
                                                        <asp:Label ID="LBL_Acciones" runat="server" Text="Acciones"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button UseSubmitBehavior="false" class="btn btn-outline-danger btn-round" ID="BTN_EliminarProducto" runat="server"
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
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="input-group no-border">
                                                    <asp:TextBox class="form-control" ID="TXT_BuscarProductosSinAsignar" runat="server" placeholder="Buscar..." OnTextChanged="FiltrarProductos_OnClick" AutoPostBack="true" onchange="cargarFiltros();"></asp:TextBox>
                                                    <div class="input-group-append">
                                                        <div class="input-group-text">
                                                            <i class="nc-icon nc-zoom-split"></i>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:ListBox class="form-control" runat="server" ID="LB_Categoria" SelectionMode="Multiple" OnTextChanged="FiltrarProductos_OnClick" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                        </div>
                                        <asp:GridView ID="DGV_ListaProductosSinAgregar" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDProducto,DescripcionProducto,Categoria" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_ListaProductosSinAsignar_Sorting"
                                            OnRowDataBound="DGV_ListaProductosSinAsignar_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Acciones" runat="server" Text="Seleccionar"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CHK_Producto" runat="server" AutoPostBack="true" OnCheckedChanged="CHK_Producto_OnCheckedChanged" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="50px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DescripcionProducto" SortExpression="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px"></asp:BoundField>
                                                <asp:BoundField DataField="PrecioVentaFinal" SortExpression="PrecioVentaFinal" HeaderText="Precio unitario" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px" DataFormatString="{0:n2}"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionCategoria" SortExpression="DescripcionCategoria" HeaderText="Categoria" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Cantidad" runat="server" Text="Cantidad"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox class="form-control" TextMode="Number" MaxLength="0" min="0" max="99" style="width: 100%" runat="server" ID="TXT_CantidadAgregar" 
                                                            OnTextChanged="TXT_CantidadAgregar_OnTextChanged" onchange="enterClickAgregar2(this);" Text='0' AutoPostBack="true" />
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
                            <asp:Button ID="BTN_CerrarModalCrearPedido" UseSubmitBehavior="false" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-primary" />
                            <asp:Button ID="BTN_Agregar" runat="server" UseSubmitBehavior="false" Text="Agregar" CssClass="btn btn-secondary" OnClick="BTN_Agregar_Click" />
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
                            <asp:Button ID="BTN_CerrarModalConfirmacionPedido" UseSubmitBehavior="false" runat="server" Text="Cancelar" data-dismiss="modal" CssClass="btn btn-secondary" />
                            <asp:Button ID="BTN_ConfirmacionPedido" runat="server" UseSubmitBehavior="false" Text="Confirmar" CssClass="btn btn-success" OnClick="BTN_ConfirmacionPedido_Click" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <button type="button" id="BTN_ModalDetallePedido" data-toggle="modal" data-target="#ModalDetallePedido" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalDetallePedido" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="popDetallePedido" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalDetallePedido" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server">Detalle Pedido</h5>
                        </div>
                        <div class="modal-body">                            
                            <div class="table-responsive" id="tableCategorias">
                                <asp:UpdatePanel ID="UpdatePanel_DetallePedido" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="col-md-6" style="margin-bottom: 2rem;">
                                            <label for="TXT_NombreImpresora">Nombre impresora</label>
                                            <asp:TextBox class="form-control" ID="TXT_NombreImpresora" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <br />
                                        <br />
                                        <asp:GridView ID="DGV_DetallePedido" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="false" DataKeyNames="IDPedidoDetalle,PedidoID,ProductoID,Categoria" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros.">
                                            <Columns>                                         
                                                <asp:BoundField DataField="DescripcionCategoria" HeaderText="Categoría" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="ProductoID" HeaderText="Código producto" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="CantidadProduccion" HeaderText="Cantidad solicitada" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="modal-footer">                            
                            <div style="text-align: right;">
                                <asp:Button ID="BTN_CerrarModalDetallePedido" UseSubmitBehavior="false" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-secondary" />  
                                <asp:Button ID="BTN_ImprimirDetallePedido" UseSubmitBehavior="false" runat="server" Text="Imprimir" CssClass="btn btn-success" OnClick="BTN_ImprimirDetallePedido_Click" />                              
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
                                <asp:Button ID="BTN_CerrarModalSeleccionarImpresora" UseSubmitBehavior="false" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-secondary" />                                
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
