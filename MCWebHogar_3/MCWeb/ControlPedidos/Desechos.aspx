<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Desechos.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ControlPedidos.Desechos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Desechos</title>
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

        function TXT_FechaDesechoDesdeChange() {
            var fechaDesechoDesde = $(<%= TXT_FechaDesechoDesde.ClientID %>)[0].value
            var fechaDesechoHasta = $(<%= TXT_FechaDesechoHasta.ClientID %>)[0].value

            if (fechaDesechoHasta === '1900-01-01') {
                $(<%= TXT_FechaDesechoHasta.ClientID %>)[0].value = fechaDesechoDesde
            }
            return true
        }

        function TXT_FechaDesechoHastaChange() {
            var fechaDesechoDesde = $(<%= TXT_FechaDesechoDesde.ClientID %>)[0].value
            var fechaDesechoHasta = $(<%= TXT_FechaDesechoHasta.ClientID %>)[0].value

            if (fechaDesechoDesde === '1900-01-01') {
                $(<%= TXT_FechaDesechoDesde.ClientID %>)[0].value = fechaDesechoHasta
            }
            return true
        }
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
                    <li>
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
                        <a href="Desecho.aspx">
                            <i class="fas fa-box-open"></i>
                            <p>Desecho</p>
                        </a>
                    </li>
                    <li>
                        <a href="Empaque.aspx">
                            <i class="fas fa-truck"></i>
                            <p>Empaque</p>
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
                    <li class="active">
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
                    <h1 class="h3 mb-2 text-gray-800">Desechos</h1>
                    <div class="card shadow mb-4">
                        <div class="card-body" style="padding-top: 0px;">
                            <div class="card-body">
                                <asp:UpdatePanel ID="UpdatePanel_FiltrosDesechos" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-md-6">                                            
                                                <div class="input-group no-border col-md-6">     
                                                    <label for="TXT_FechaDesechoDesde"> Fecha desecho desde:</label>                                          
                                                    <asp:TextBox class="form-control" style="flex: auto;" ID="TXT_FechaDesechoDesde" runat="server" TextMode="Date" onchange="TXT_FechaDesechoDesdeChange();" OnTextChanged="TXT_FiltrarDesechos_OnTextChanged" AutoPostBack="true"></asp:TextBox>                                                
                                                </div>
                                                <div class="input-group no-border col-md-6">
                                                    <label for="TXT_FechaDesechoHasta"> Fecha desecho hasta:</label>
                                                    <asp:TextBox class="form-control" style="flex: auto;" ID="TXT_FechaDesechoHasta" runat="server" TextMode="Date" onchange="TXT_FechaDesechoHastaChange();" OnTextChanged="TXT_FiltrarDesechos_OnTextChanged" AutoPostBack="true"></asp:TextBox>                                                
                                                </div>
                                            </div>
                                            <div class="col-md-6" style="text-align: right;">
                                                <div class="card-header" style="text-align: right;">
                                                    <asp:Button ID="BTN_CrearDesechos" runat="server" Text="Crear nuevo desecho" CssClass="btn btn-secondary" OnClick="BTN_CrearDesechos_Click"></asp:Button>                                    
                                                </div>
                                            </div>
                                        </div> 
                                        <div class="row">                         
                                            <div class="input-group no-border col-md-12">
                                                <asp:TextBox class="form-control" ID="TXT_Buscar" runat="server" placeholder="Buscar número desecho..." OnTextChanged="TXT_FiltrarDesechos_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <div class="input-group-text">
                                                        <i class="nc-icon nc-zoom-split"></i>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="table-responsive">
                                    <asp:UpdatePanel ID="UpdatePanel_ListaDesechos" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="DGV_ListaDesechos" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                AutoGenerateColumns="False" DataKeyNames="IDDesecho,UsuarioID" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                                OnSorting="DGV_ListaDesechos_Sorting"
                                                OnRowCommand="DGV_ListaDesechos_RowCommand"
                                                OnRowDataBound="DGV_ListaDesechos_OnRowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="NumeroDesecho" SortExpression="IDDesecho" HeaderText="Número Desecho" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="Nombre" SortExpression="Nombre" HeaderText="Solicitante" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                                                
                                                    <asp:BoundField DataField="FDesecho" SortExpression="FDesecho" HeaderText="Fecha" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                                                
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
    </div>
</asp:Content>
