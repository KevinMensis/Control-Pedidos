﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Devoluciones.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ControlPedidos.Devoluciones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Devoluciones</title>
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

        function TXT_FechaDevolucionDesdeChange() {
            var fechaDevolucionDesde = $(<%= TXT_FechaDevolucionDesde.ClientID %>)[0].value
            var fechaDevolucionHasta = $(<%= TXT_FechaDevolucionHasta.ClientID %>)[0].value

            if (fechaDevolucionHasta === '1900-01-01') {
                $(<%= TXT_FechaDevolucionHasta.ClientID %>)[0].value = fechaDevolucionDesde
            }
            return true
        }

        function TXT_FechaDevolucionHastaChange() {
            var fechaDevolucionDesde = $(<%= TXT_FechaDevolucionDesde.ClientID %>)[0].value
            var fechaDevolucionHasta = $(<%= TXT_FechaDevolucionHasta.ClientID %>)[0].value

            if (fechaDevolucionDesde === '1900-01-01') {
                $(<%= TXT_FechaDevolucionDesde.ClientID %>)[0].value = fechaDevolucionHasta
            }
            return true
        }
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
                    <li class="active">
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
                    <h1 class="h3 mb-2 text-gray-800">Devoluciones</h1>
                    <div class="card shadow mb-4">
                        <div class="card-body" style="padding-top: 0px;">
                            <div class="card-body">
                                <asp:UpdatePanel ID="UpdatePanel_FiltrosDevoluciones" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate> 
                                        <div class="row">
                                            <div class="col-md-6">                                                                                            
                                            </div>
                                            <div class="col-md-6" style="text-align: right;">
                                                <div class="card-header" style="text-align: right;">
                                                    <asp:Button ID="BTN_CrearDevoluciones" runat="server" Text="Crear nueva devolución" CssClass="btn btn-secondary" OnClick="BTN_CrearDevoluciones_Click"></asp:Button>                                    
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">                         
                                            <div class="input-group no-border col-md-6">
                                                <asp:TextBox class="form-control" ID="TXT_Buscar" runat="server" placeholder="Buscar número devolución..." OnTextChanged="TXT_FiltrarDevoluciones_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <div class="input-group-text">
                                                        <i class="nc-icon nc-zoom-split"></i>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="input-group no-border col-md-3">     
                                                <label for="TXT_FechaDevolucionDesde"> Fecha devolución desde:</label>                                          
                                                <asp:TextBox class="form-control" style="flex: auto;" ID="TXT_FechaDevolucionDesde" runat="server" TextMode="Date" onchange="TXT_FechaDevolucionDesdeChange();" OnTextChanged="TXT_FiltrarDevoluciones_OnTextChanged" AutoPostBack="true"></asp:TextBox>                                                
                                            </div>
                                            <div class="input-group no-border col-md-3">
                                                <label for="TXT_FechaDevolucionHasta"> Fecha devolución hasta:</label>
                                                <asp:TextBox class="form-control" style="flex: auto;" ID="TXT_FechaDevolucionHasta" runat="server" TextMode="Date" onchange="TXT_FechaDevolucionHastaChange();" OnTextChanged="TXT_FiltrarDevoluciones_OnTextChanged" AutoPostBack="true"></asp:TextBox>                                                
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="table">
                                    <asp:UpdatePanel ID="UpdatePanel_ListaDevoluciones" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="DGV_ListaDevoluciones" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                AutoGenerateColumns="False" DataKeyNames="IDDevolucion,UsuarioID" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                                OnSorting="DGV_ListaDevoluciones_Sorting"
                                                OnRowCommand="DGV_ListaDevoluciones_RowCommand"
                                                OnRowDataBound="DGV_ListaDevoluciones_OnRowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="NumeroDevolucion" SortExpression="IDDevolucion" HeaderText="Número devolución" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="Nombre" SortExpression="Nombre" HeaderText="Solicitante" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                                                
                                                    <asp:BoundField DataField="FDevolucion" SortExpression="FDevolucion" HeaderText="Fecha" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                                                
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="LBL_Acciones" runat="server" Text="Acciones"></asp:Label>
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
