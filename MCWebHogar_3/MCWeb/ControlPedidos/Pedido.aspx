<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pedido.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ControlPedidos.Pedido" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Pedido</title>
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

        function abrirModalCrearPedido() {
            document.getElementById('BTN_ModalCrearPedido').click()
        }

        function cerrarModalCrearPedido() {
            document.getElementById('BTN_ModalCrearPedido').click()
        }

        function validarCreacionPedido() {

        }

        function TXT_FechaCreacionDesdeChange() {
            var fechaCreacionDesde = $(<%= TXT_FechaCreacionDesde.ClientID %>)[0].value
            var fechaCreacionHasta = $(<%= TXT_FechaCreacionHasta.ClientID %>)[0].value
            
            if (fechaCreacionHasta === '1900-01-01') {
                $(<%= TXT_FechaCreacionHasta.ClientID %>)[0].value = fechaCreacionDesde
            }
            return true
        }

        function TXT_FechaCreacionHastaChange() {
            var fechaCreacionDesde = $(<%= TXT_FechaCreacionDesde.ClientID %>)[0].value
            var fechaCreacionHasta = $(<%= TXT_FechaCreacionHasta.ClientID %>)[0].value
            
            if (fechaCreacionDesde === '1900-01-01') {
                $(<%= TXT_FechaCreacionDesde.ClientID %>)[0].value = fechaCreacionHasta
            }
            return true
        }

        function TXT_FechaPedidoDesdeChange() {
            var fechaPedidoDesde = $(<%= TXT_FechaPedidoDesde.ClientID %>)[0].value
            var fechaPedidoHasta = $(<%= TXT_FechaPedidoHasta.ClientID %>)[0].value

            if (fechaPedidoHasta === '1900-01-01') {
                $(<%= TXT_FechaPedidoHasta.ClientID %>)[0].value = fechaPedidoDesde
            }
            return true
        }

        function TXT_FechaPedidoHastaChange() {
            var fechaPedidoDesde = $(<%= TXT_FechaPedidoDesde.ClientID %>)[0].value
            var fechaPedidoHasta = $(<%= TXT_FechaPedidoHasta.ClientID %>)[0].value

            if (fechaPedidoDesde === '1900-01-01') {
                $(<%= TXT_FechaPedidoDesde.ClientID %>)[0].value = fechaPedidoHasta
            }
            return true
        }

        function cargarFiltros() {
            $(<%= LB_Sucursal.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Sucursal' })
            $(<%= LB_PlantaProduccion.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Planta de Producción' })
            $(<%= LB_Estado.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Estado' })
            $(<%= LB_Solicitante.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Solicitante' })
        }

        $(document).ready(function () {
            cargarFiltros();          
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="modalloading" class="loading">
        <img src="../images/cargando5.gif" width="100" height="100" />
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
                    <li class="active">
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
                    <li>
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
                    <h1 class="h3 mb-2 text-gray-800">Pedidos</h1>
                    <!-- DataTales Example -->
                    <div class="card shadow mb-4">
                        <asp:UpdatePanel ID="UpdatePanel_PedidosEvents" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="card-header" style="text-align: right;">
                                    <asp:Button ID="BTN_CrearPedidos" runat="server" Text="Crear nuevo pedido" CssClass="btn btn-secondary" OnClick="BTN_CrearPedidos_Click"></asp:Button>
                                    <asp:Button ID="BTN_CrearOrdenes" runat="server" Text="Crear nueva orden" CssClass="btn btn-secondary" OnClick="BTN_CrearOrdenes_Click"></asp:Button>
                                    <asp:Button ID="BTN_CrearDespacho" runat="server" Text="Despachar pedido" CssClass="btn btn-secondary" OnClick="BTN_CrearDespacho_Click"></asp:Button>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="card-body" style="padding-top: 0px;">
                            <div class="card-body">
                                <asp:UpdatePanel ID="UpdatePanel_FiltrosPedidos" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate> 
                                        <div class="row">                         
                                            <div class="input-group no-border col-md-12">
                                                <asp:TextBox class="form-control" ID="TXT_Buscar" runat="server" placeholder="Buscar número pedido..." OnTextChanged="TXT_FiltrarPedidos_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <div class="input-group-text">
                                                        <i class="nc-icon nc-zoom-split"></i>
                                                    </div>
                                                </div>
                                            </div>
                                        </div> 
                                        <div class="row">
                                            <div class="input-group no-border col-md-3">     
                                                <label for="TXT_FechaCreacionDesde"> Fecha creación desde:</label>                                          
                                                <asp:TextBox class="form-control" style="flex: auto;" ID="TXT_FechaCreacionDesde" runat="server" TextMode="Date" onchange="TXT_FechaCreacionDesdeChange();" OnTextChanged="TXT_FiltrarPedidos_OnTextChanged" AutoPostBack="true"></asp:TextBox>                                                
                                            </div>
                                            <div class="input-group no-border col-md-3">
                                                <label for="TXT_FechaCreacionHasta"> Fecha creación hasta:</label>
                                                <asp:TextBox class="form-control" style="flex: auto;" ID="TXT_FechaCreacionHasta" runat="server" TextMode="Date" onchange="TXT_FechaCreacionHastaChange();" OnTextChanged="TXT_FiltrarPedidos_OnTextChanged" AutoPostBack="true"></asp:TextBox>                                                
                                            </div>
                                            <div class="input-group no-border col-md-3">     
                                                <label for="TXT_FechaPedidoDesde"> Fecha pedido desde:</label>                                          
                                                <asp:TextBox class="form-control" style="flex: auto;" ID="TXT_FechaPedidoDesde" runat="server" TextMode="Date" onchange="TXT_FechaPedidoDesdeChange();" OnTextChanged="TXT_FiltrarPedidos_OnTextChanged" AutoPostBack="true"></asp:TextBox>                                                
                                            </div>
                                            <div class="input-group no-border col-md-3">
                                                <label for="TXT_FechaPedidoHasta"> Fecha pedido hasta:</label>
                                                <asp:TextBox class="form-control" style="flex: auto;" ID="TXT_FechaPedidoHasta" runat="server" TextMode="Date" onchange="TXT_FechaPedidoHastaChange();" OnTextChanged="TXT_FiltrarPedidos_OnTextChanged" AutoPostBack="true"></asp:TextBox>                                                
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <asp:ListBox class="form-control" runat="server" ID="LB_Sucursal" SelectionMode="Multiple" OnTextChanged="TXT_FiltrarPedidos_OnTextChanged" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:ListBox class="form-control" runat="server" ID="LB_PlantaProduccion" SelectionMode="Multiple" OnTextChanged="TXT_FiltrarPedidos_OnTextChanged" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:ListBox class="form-control" runat="server" ID="LB_Estado" SelectionMode="Multiple" OnTextChanged="TXT_FiltrarPedidos_OnTextChanged" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:ListBox class="form-control" runat="server" ID="LB_Solicitante" SelectionMode="Multiple" OnTextChanged="TXT_FiltrarPedidos_OnTextChanged" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                        </div>
                                        <div class="row" style="float: right;">
                                            <asp:Button id="BTN_EliminarFiltro" style="float: right;" runat="server" CssClass="btn btn-danger" Text="Eliminar Filtro" OnClick="BTN_EliminarFiltro_Click" />
                                        </div>
                                        <div class="row" style="margin-left: 10px; margin-top: 10px;">
                                            <asp:Label id="LBL_Filtro" runat="server" Text="Filtros: Ninguno;"></asp:Label>                                                                                            
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="table">
                                <asp:UpdatePanel ID="UpdatePanel_ListaPedidos" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaPedidos" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDPedido,Estado,UsuarioID,PlantaProduccionID,PuntoVentaID" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnRowCommand="DGV_ListaPedidos_RowCommand"
                                            OnSorting="DGV_ListaPedidos_Sorting"
                                            OnRowDataBound="DGV_ListaPedidos_OnRowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="NumeroPedido" SortExpression="IDPedido" HeaderText="Número Pedido" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionPuntoVenta" SortExpression="DescripcionPuntoVenta" HeaderText="Sucursal" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionPlantaProduccion" SortExpression="DescripcionPlantaProduccion" HeaderText="Planta Producción" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="Estado" SortExpression="Estado" HeaderText="Estado" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="Nombre" SortExpression="Nombre" HeaderText="Solicitante" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Acciones" runat="server" Text="Seleccionar"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="seleccionarCheckBox" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Acciones" runat="server" Text="ACCIONES"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button class="btn btn-outline-primary btn-round" ID="BTN_VerDetalle" runat="server"
                                                            CommandName="VerDetalle"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            Text="Ver detalles" AutoPostBack="true" />
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

    <button type="button" id="BTN_ModalCrearPedido" data-toggle="modal" data-target="#ModalCrearPedido" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalCrearPedido" tabindex="-1" role="dialog" aria-labelledby="popCrearPedido" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalCrearPedido" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server">Crear pedido</h5>
                        </div>
                        <div class="modal-body">
                            <div class="form-row">
                                <div class="form-group col-md-4">
                                    <label for="DDL_Propietario">Solicitante</label>
                                    <asp:DropDownList class="form-control" ID="DDL_Propietario" runat="server"></asp:DropDownList>
                                </div>
                                <div class="form-group col-md-4">
                                    <label for="DDL_PlantaProduccion">Planta Producción</label>
                                    <asp:DropDownList class="form-control" ID="DDL_PlantaProduccion" runat="server"></asp:DropDownList>
                                </div>
                                <div class="form-group col-md-4">
                                    <label for="DDL_PuntoVenta">Punto Venta</label>
                                    <asp:DropDownList class="form-control" ID="DDL_PuntoVenta" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-group col-md-12">
                                    <label for="TXT_DescripcionPedido">Descripcion del pedido:</label>
                                    <asp:TextBox ID="TXT_DescripcionPedido" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="BTN_CerrarModalCrearPedido" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-secondary" />
                            <asp:Button ID="BTN_CrearPedido" runat="server" Text="Guardar pedido" CssClass="btn btn-success" OnClientClick="return validarCreacionPedido();" OnClick="BTN_CrearPedido_Click" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
