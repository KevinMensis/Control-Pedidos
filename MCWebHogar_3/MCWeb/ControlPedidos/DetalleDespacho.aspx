<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleDespacho.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ControlPedidos.DetalleDespacho" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Detalle despacho</title>
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

        function activarloadingConfirmacion() {
            document.getElementById('Content_LBL_GenerandoInforme').innerHTML = 'Confirmado el despacho, espere por favor.'
            document.getElementById('fade2').style.display = 'block';
            document.getElementById('modalloading').style.display = 'block';
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

        function TXT_Cantidad_onKeyUp(txtCantidad, e) {        
            var values = txtCantidad.id.split('_')
            var indexGrid = values[5] * 1
            var index = values.pop() * 1 + 1
            if (e.keyCode === 13) {
                var idGrid = 'Content_DGV_ListaPedidosDespacho_DGV_ListaProductos_' + indexGrid                 
                var rows = document.getElementById(idGrid).rows.length - 1
                var id = ''
                indexGrid += 2
                indexGrid = indexGrid < 10 ? '0' + indexGrid.toString() : indexGrid
                if (index === rows) {
                    index = 0
                }
                index += 2
                index = index < 10 ? '0' + index.toString() : index 
                id = 'ctl00$Content$DGV_ListaPedidosDespacho$ctl' + indexGrid + '$DGV_ListaProductos$ctl' + index + '$TXT_Cantidad'

                document.getElementsByName(id)[1].autofocus = true;
                document.getElementsByName(id)[1].focus();
                document.getElementsByName(id)[1].select();
            }
        }

        function TXT_Cantidad_onChange(txtCantidad) {
            var values = txtCantidad.id.split('_')
            var indexGrid = values[5] * 1 + 2
            var index = values.pop() * 1 + 2
            indexGrid = indexGrid < 10 ? '0' + indexGrid.toString() : indexGrid
            index = index < 10 ? '0' + index.toString() : index
            var idUnidades = 'ctl00$Content$DGV_ListaPedidosDespacho$ctl' + indexGrid + '$DGV_ListaProductos$ctl' + index + '$DDL_Unidades'
            var idDecenas = 'ctl00$Content$DGV_ListaPedidosDespacho$ctl' + indexGrid + '$DGV_ListaProductos$ctl' + index + '$DDL_Decenas'
            var cantidadProducto = txtCantidad.value
            if (cantidadProducto !== '') {
                var unds = cantidadProducto % 10;
                var decs = Math.trunc(cantidadProducto / 10);
                if (cantidadProducto >= 0 && cantidadProducto < 100) {
                    document.getElementsByName(idUnidades)[1].value = unds;
                    document.getElementsByName(idDecenas)[1].value = decs;
                    txtCantidad.value = cantidadProducto;
                } else {
                    unds = document.getElementsByName(idUnidades)[1].value * 1;
                    decs = document.getElementsByName(idDecenas)[1].value * 10;
                    cantidadProducto = decs + unds;
                    txtCantidad.value = cantidadProducto;
                }
            }
            else {
                txtCantidad.value = '0';
                document.getElementsByName(idUnidades)[1].value = '0';
                document.getElementsByName(idDecenas)[1].value = '0';
            }
        }

        function DDL_UnidadesDecenas_onChange(ddlUnidadesDecenas) {
            var values = ddlUnidadesDecenas.id.split('_')
            var indexGrid = values[5] * 1 + 2
            var index = values.pop() * 1 + 2
            indexGrid = indexGrid < 10 ? '0' + indexGrid.toString() : indexGrid
            index = index < 10 ? '0' + index.toString() : index
            var idUnidades = 'ctl00$Content$DGV_ListaPedidosDespacho$ctl' + indexGrid + '$DGV_ListaProductos$ctl' + index + '$DDL_Unidades'
            var idDecenas = 'ctl00$Content$DGV_ListaPedidosDespacho$ctl' + indexGrid + '$DGV_ListaProductos$ctl' + index + '$DDL_Decenas'
            var txtCantidad = 'ctl00$Content$DGV_ListaPedidosDespacho$ctl' + indexGrid + '$DGV_ListaProductos$ctl' + index + '$TXT_Cantidad'
            var cantUnidades = document.getElementsByName(idUnidades)[1].value * 1
            var cantDecenas = document.getElementsByName(idDecenas)[1].value * 10
            var cantidadProducto = cantDecenas + cantUnidades
            document.getElementsByName(txtCantidad)[1].value = cantidadProducto           
        }

        function estilosElementosBloqueados() {
            document.getElementById('<%= TXT_CodigoDespacho.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_CodigoDespacho.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_TotalProductos.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_TotalProductos.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_MontoDespacho.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_MontoDespacho.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_EstadoDespacho.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_EstadoDespacho.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_FechaDespacho.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_FechaDespacho.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_HoraDespacho.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_HoraDespacho.ClientID %>').classList.add('form-control')
            document.getElementById('<%= DDL_Propietario.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= DDL_Propietario.ClientID %>').classList.add('form-control')
        }
        
        function cargarFiltros() {
            $(<%= LB_Pedido.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Número pedido' })
            $(<%= LB_PuntoVenta.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Punto venta' })
        }

        $(document).ready(function () {
            estilosElementosBloqueados();
            cargarFiltros();

            $(document).on('click', '[src*=plus]', function () {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                $(this).attr("src", "../Assets/img/minus.png");
            });

            $(document).on('click', '[src*=minus]', function () {
                $(this).attr("src", "../Assets/img/plus.png");
                $(this).closest("tr").next().remove();
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="modalloading" class="loading">
        <img src="../Assets/img/cargando.gif" width="100" height="100" /><br />
        <asp:Label runat="server" ID="LBL_GenerandoInforme" style="color: white;" Text="Generando informe espere por favor..."></asp:Label>
    </div>
    <div id="fade2" class="overlayload"></div>
    <div class="wrapper">
        <asp:HiddenField ID="HDF_IDDespacho" runat="server" Value="0" Visible="false" />
        <asp:HiddenField ID="HDF_EstadoDespacho" runat="server" Value="" Visible="false" />
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
                    <li class="active">
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
                    <h1 class="h3 mb-2 text-gray-800">Detalle despacho</h1>
                    <br />
                    <!-- DataTales Example -->
                    <div class="card shadow mb-4">
                        <asp:UpdatePanel ID="UpdatePanel_Header" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="card-header py-3">
                                    <div class="form-row">
                                        <div class="form-group col-md-2">
                                            <label for="TXT_CodigoDespacho">Número despacho</label>
                                            <asp:TextBox class="form-control" ID="TXT_CodigoDespacho" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="TXT_TotalProductos">Total de productos</label>
                                            <asp:TextBox class="form-control" ID="TXT_TotalProductos" runat="server" TextMode="Number" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="TXT_MontoDespacho">Monto despacho</label>
                                            <asp:TextBox class="form-control" ID="TXT_MontoDespacho" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="TXT_EstadoDespacho">Estado despacho</label>
                                            <asp:TextBox class="form-control" ID="TXT_EstadoDespacho" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-4">                                            
                                            <div class="form-row">
                                                <div class="col-md-7">
                                                    <label for="TXT_FechaDespacho">Fecha despacho</label>
                                                    <asp:TextBox ID="TXT_FechaDespacho" runat="server" CssClass="form-control" TextMode="Date" format="dd/MM/yyyy" Enabled="false"></asp:TextBox>
                                                </div>
                                                <div class="col-md-5">
                                                    <label for="TXT_HoraDespacho">Hora despacho</label>
                                                    <asp:TextBox ID="TXT_HoraDespacho" runat="server" CssClass="form-control" TextMode="Time" format="HH:mm" Enabled="false"></asp:TextBox>
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
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label for="DDL_Reportes">Reportes</label>
                                            <asp:DropDownList class="form-control" ID="DDL_Reportes" runat="server" AutoPostBack="true" onchange="activarloading();estilosElementosBloqueados();" OnSelectedIndexChanged="DDL_Reportes_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Seleccione</asp:ListItem>
                                                <asp:ListItem Value="1">Reporte despacho</asp:ListItem>
                                                <asp:ListItem Value="2">Descargar despacho</asp:ListItem>
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
                                    <div class="row">
                                        <div class="col-md-6">                                                                               
                                            <asp:Button UseSubmitBehavior="false" ID="BTN_ConfirmarDespacho" runat="server" Text="Confirmar despacho" CssClass="btn btn-secondary" OnClientClick="activarloadingConfirmacion();" OnClick="BTN_ConfirmarDespacho_Click"></asp:Button>                                                                                    
                                        </div>                                        
                                        <div class="col-md-6" style="text-align: right;"> 
                                            <asp:Button UseSubmitBehavior="false" ID="BTN_CompletarDespacho" runat="server" Text="Completar despacho" CssClass="btn btn-success" OnClick="BTN_CompletarDespacho_Click"></asp:Button>                                            
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
                                         <div class="row">                          
                                            <div class="input-group no-border col-md-6">
                                                <asp:TextBox class="form-control" ID="TXT_Buscar" runat="server" placeholder="Buscar producto..." OnTextChanged="TXT_Buscar_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <div class="input-group-text">
                                                        <i class="nc-icon nc-zoom-split"></i>
                                                    </div>
                                                </div>
                                            </div>                                       
                                            <div class="input-group no-border col-md-3">     
                                                <asp:ListBox class="form-control" runat="server" ID="LB_Pedido" SelectionMode="Multiple" OnTextChanged="TXT_Buscar_OnTextChanged" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                            <div class="input-group no-border col-md-3">     
                                                <asp:ListBox class="form-control" runat="server" ID="LB_PuntoVenta" SelectionMode="Multiple" OnTextChanged="TXT_Buscar_OnTextChanged" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="table">
                                <asp:UpdatePanel ID="UpdatePanel_ListaPedidosDespacho" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaPedidosDespacho" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="DespachoID,PedidoID,PuntoVentaID" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_ListaPedidosDespacho_Sorting"
                                            OnRowCommand="DGV_ListaPedidosDespacho_RowCommand"
                                            OnRowDataBound="DGV_ListaPedidosDespacho_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="Lbl_VerDetalle" runat="server" Text="Ver detalle"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="table" id="tableProductos">
                                                            <img alt="" style="cursor: pointer" src="../Assets/img/plus.png" />
                                                            <asp:Panel ID="pnlListaProductos" runat="server" Style="display: none;">
                                                                <asp:GridView ID="DGV_ListaProductos" runat="server" AutoGenerateColumns="false" DataKeyNames="IDDespachoDetalle,ProductoID,PedidoID,DespachoID,DescripcionProducto" CssClass="ChildGrid"
                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" 
                                                                    GridLines="None" ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                             
                                                                        <asp:BoundField DataField="CantidadSolicitada" HeaderText="Cantidad solicitada" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                        <asp:BoundField DataField="CantidadDespachada" HeaderText="Cantidad despachada" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="LBL_Cantidad" runat="server" Text="Cantidad a despachar"></asp:Label>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div class="row">
                                                                                    <asp:TextBox class="form-control" TextMode="Number" MaxLength="2" min="0" max="99" style="width: 40%" runat="server" ID="TXT_Cantidad" name="TXT_Cantidad"
                                                                                        onkeyup="TXT_Cantidad_onKeyUp(this, event);" onchange="TXT_Cantidad_onChange(this);" Text='0' /> 
                                                                                    <asp:DropDownList class="form-control" style="width: 30%" runat="server" ID="DDL_Decenas" onchange="DDL_UnidadesDecenas_onChange(this);">
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
                                                                                    <asp:DropDownList class="form-control" style="width: 30%" runat="server" ID="DDL_Unidades" onchange="DDL_UnidadesDecenas_onChange(this);">
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
                                                                                    <asp:HiddenField ID="HDF_IDDespachoDetalle" runat="server" Value='<%# Eval("IDDespachoDetalle") %>' />                                                        
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField> 
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="NumeroPedido" SortExpression="PedidoID" HeaderText="Número pedido" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionPuntoVenta" SortExpression="DescripcionPuntoVenta" HeaderText="Punto venta" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                                <%--<asp:BoundField DataField="DescripcionProducto" SortExpression="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>--%>
                                                <asp:BoundField DataField="CantidadSolicitada" SortExpression="CantidadSolicitada" HeaderText="Cantidad solicitada" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                                <asp:BoundField DataField="CantidadDespachada" SortExpression="CantidadDespachada" HeaderText="Cantidad despachada" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                                                
                                                <%--<asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Cantidad" runat="server" Text="Cantidad a despachar"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="row">
                                                            <asp:TextBox class="form-control" TextMode="Number" MaxLength="2" min="0" max="99" style="width: 40%" runat="server" ID="TXT_Cantidad" 
                                                                onkeyup="TXT_Cantidad_onKeyUp(this, event);" onchange="TXT_Cantidad_onChange(this);" Text='0' />                                                            
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
                                                </asp:TemplateField>--%>
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
</asp:Content>
