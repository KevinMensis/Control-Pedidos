﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlantasProduccion.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ControlPedidos.PlantasProduccion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Plantas Produccion</title>
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

        function abrirModalCrearPlantaProduccion() {
            document.getElementById('BTN_ModalCrearPlantaProduccion').click()
        }

        function cerrarModalCrearPlantaProduccion() {
            document.getElementById('BTN_ModalCrearPlantaProduccion').click()
        }

        function validarCrearPlantaProduccion() {

        }

        $(document).ready(function () {
           
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
                    <li class="active">
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
                    <h1 class="h3 mb-2 text-gray-800">Mantenimiento plantas de produccion</h1>
                    <br />
                    <div class="card shadow mb-4">
                        <asp:UpdatePanel ID="UpdatePanel_CrearPlantaProduccion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="card-header py-3" style="text-align: right;">
                                    <asp:Button ID="BTN_CrearPlantaProduccion" runat="server" Text="Crear nueva planta produccion" CssClass="btn btn-secondary" OnClick="BTN_CrearPlantaProduccion_OnClick"></asp:Button>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel_FiltrosPlantasProduccion" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>                           
                                    <div class="input-group no-border">
                                        <asp:TextBox class="form-control" ID="TXT_Buscar" runat="server" placeholder="Buscar..." OnTextChanged="TXT_Buscar_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                        <div class="input-group-append">
                                            <div class="input-group-text">
                                                <i class="nc-icon nc-zoom-split"></i>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel> 
                            <div class="table-responsive">
                                <asp:UpdatePanel ID="UpdatePanel_ListaPlantasProduccion" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaPlantasProduccion" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDPlantaProduccion,Activo" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_ListaPlantasProduccion_Sorting"
                                            OnRowCommand="DGV_ListaPlantasProduccion_RowCommand"
                                            OnRowDataBound="DGV_ListaPlantasProduccion_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="DescripcionPlantaProduccion" SortExpression="DescripcionPlantaProduccion" HeaderText="Descripción" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="UbicacionPlantaProduccion" SortExpression="UbicacionPlantaProduccion" HeaderText="Ubicación" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Acciones" runat="server" Text="ACCIONES"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button class="btn btn-outline-success btn-round" ID="BTN_Activar" runat="server"
                                                            CommandName="activar"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            Text="Activar" AutoPostBack="true" />
                                                        <asp:Button class="btn btn-outline-primary btn-round" ID="BTN_Editar" runat="server"
                                                            CommandName="editar"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            Text="Editar" AutoPostBack="true" />
                                                        <asp:Button class="btn btn-outline-danger btn-round" ID="BTN_Eliminar" runat="server"
                                                            CommandName="desactivar"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            Text="Desactivar" AutoPostBack="true" />
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

    <button type="button" id="BTN_ModalCrearPlantaProduccion" data-toggle="modal" data-target="#ModalCrearPlantaProduccion" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalCrearPlantaProduccion" tabindex="-1" role="dialog" aria-labelledby="popCrearPlantaProduccion" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalCrearPlantaProduccion" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server" id="title_CrearPlantaProduccion"></h5>
                        </div>
                        <div class="modal-body">
                            <asp:HiddenField ID="HDF_IDPlantaProduccion" runat="server" Value="0" />
                            <div class="row">
                                <div class="col-md-12">
                                    <label for="TXT_DescripcionPlantaProduccion">Descripcion de la planta producción:</label>
                                    <asp:TextBox ID="TXT_DescripcionPlantaProduccion" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <label for="TXT_UbicacionPlantaProduccion">Ubicación de la planta producción:</label>
                                    <asp:TextBox ID="TXT_UbicacionPlantaProduccion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </div>                                
                            </div>                                                       
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="BTN_CerrarModalCrearPlantaProduccion" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-secondary" />
                            <asp:Button ID="BTN_GuardarPlantaProduccion" runat="server" Text="Guardar planta producción" CssClass="btn btn-success" OnClientClick="return validarCrearPlantaProduccion();" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>