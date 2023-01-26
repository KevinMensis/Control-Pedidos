<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CostosIndirectos.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.GestionCostos.CostosIndirectos" EnableEventValidation="true" %>

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

        function editarFactor(element) {
            var idFactor = element.parentElement.children[0].value
            var detalleFactor = element.parentElement.children[1].value
            var monto = element.parentElement.children[2].value
            var idCategoria = element.parentElement.children[3].value

            var HDF_IDFactorCargaFabril = $(<%= HDF_IDFactorCargaFabril.ClientID %>)[0]
            HDF_IDFactorCargaFabril.value = idFactor;
            
            var TXT_DetelleFactor = $(<%= TXT_DetalleFactor.ClientID %>)[0]
            var DDL_Categoria = $(<%= DDL_CategoriasFactores.ClientID %>)[0]
            var TXT_Monto = $(<%= TXT_Monto.ClientID %>)[0]

            TXT_DetelleFactor.value = detalleFactor
            DDL_Categoria.value = idCategoria
            TXT_Monto.value = monto

            abrirModalAgregarEditarFactor();
        }

        function eliminarFactor(element) {
            var idFactor = element.parentElement.children[0].value

            var HDF_IDFactorCargaFabril = $(<%= HDF_IDFactorCargaFabril.ClientID %>)[0]
            HDF_IDFactorCargaFabril.value = idFactor;
        }

        function abrirModalAgregarEditarFactor() {
            document.getElementById('BTN_ModalAgregarEditarFactor').click()
        }

        function cerrarModalAgregarEditarFactor() {
            document.getElementById('BTN_ModalAgregarEditarFactor').click()
        }

        function validarGuardarFactor() {
            var detalle = $(<%= TXT_DetalleFactor.ClientID %>)[0].value
            var monto = $(<%= TXT_Monto.ClientID %>)[0].value

            if (monto === '' || monto === '0' || monto === 0) {
                alertifyerror('Por favor, ingrese el monto.')
                return false
            }
            if (detalle === '0' || detalle === 0) {
                alertifyerror('Por favor, ingrese el detalle.')
                return false
            }

            return true
        }

        function cargarFiltros() {
            
        }

        $(document).ready(function () {
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
                                    <h1 class="h3 mb-2 text-gray-800" runat="server" id="H1_Title">Costos indirectos</h1>
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
                                                <a href="CostosIndirectos.aspx" class="btn btn-info">
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
                                                <a href="VerReceta.aspx" class="btn btn-primary">
                                                    <i class=""></i>Ver recetas
                                                </a>
                                            </div>
                                            <div class="input-group no-border col-md-2" style="text-align: center; display: block;">
                                                <asp:LinkButton UseSubmitBehavior="false" ID="BTN_Actualizar" runat="server" CssClass="btn btn-secundary" OnClientClick="activarloading();" OnClick="BTN_ActualizarCostosProductosTerminados_OnClick">
                                                    <i class="fas fa-sync"></i> Actualizar
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <hr />                        
                            <asp:UpdatePanel ID="UpdatePanel_Ingresos" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="input-group no-border col-md-3" style="text-align: center; display: inline-block;">
                                            <h1 class="h3 mb-2 text-gray-800" runat="server" id="H1">Ingresos</h1>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <asp:TextBox class="form-control" ID="TXT_MontoIngreso" runat="server" OnTextChanged="TXT_MontoIngreso_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="table">
                                <div class="col-md-12">                                        
                                    <asp:UpdatePanel ID="UpdatePanel_CostosIndirectos" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="input-group no-border col-md-6" style="text-align: left; display: inline-block;">
                                                    <h1 class="h3 mb-2 text-gray-800" runat="server" id="H2" style="display: initial;">Gastos</h1>
                                                    <asp:LinkButton UseSubmitBehavior="false" class="btn btn-round-mant" ID="BTN_AgregarFactor" runat="server" OnClick="BTN_AgregarFactor_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>
                                                </div>
                                            </div> 
                                            <asp:HiddenField ID="HDF_IDCategoriaCargaFabril" runat="server" Value="0" Visible="true" />
                                            <asp:GridView ID="DGV_ListaCostosIndirectos" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                AutoGenerateColumns="False" DataKeyNames="CategoriaCargaFabrilID" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true" ShowFooter="true"
                                                OnSorting="DGV_ListaPedidosDespacho_Sorting"
                                                OnRowDataBound="DGV_ListaCostosIndirectos_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Lbl_VerDetalle" runat="server" Text="Ver detalle"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div class="table" id="tableDetalleFactores">                                                                
                                                                <img alt="" style="cursor: pointer" src="../Assets/img/plus.png" />
                                                                <asp:Panel ID="pnlDetalleFactores" runat="server" Style="display: none;">
                                                                    <asp:GridView ID="DGV_DetalleFactores" runat="server" AutoGenerateColumns="false" DataKeyNames="IDFactorCargaFabril" CssClass="ChildGrid"
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" 
                                                                        GridLines="None" ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="DetalleFactor" HeaderText="Factor" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left"></asp:BoundField>                                             
                                                                            <asp:BoundField DataField="Monto" HeaderText="Monto" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                            <asp:BoundField DataField="Porcentaje" HeaderText="Porcentaje" DataFormatString="{0:n}%" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="LBL_Accion" runat="server" Text="Acciones"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>                                                                                                                                      
                                                                                    <asp:HiddenField ID="HDF_IDFactorCargaFabril" runat="server" Value='<%# Eval("IDFactorCargaFabril") %>' /> 
                                                                                    <asp:HiddenField ID="HDF_DetalleFactor" runat="server" Value='<%# Eval("DetalleFactor") %>' />                                                        
                                                                                    <asp:HiddenField ID="HDF_Monto" runat="server" Value='<%# Eval("Monto") %>' />                                                        
                                                                                    <asp:HiddenField ID="HDF_IDCategoriaCargaFabril" runat="server" Value='<%# Eval("CategoriaCargaFabrilID") %>' />                                                                  
                                                                                    <asp:LinkButton UseSubmitBehavior="false" class="btn btn-outline-primary btn-round-mant" ID="BTN_Editar" runat="server" OnClientClick="editarFactor(this);">Editar</asp:LinkButton>
                                                                                    <asp:LinkButton UseSubmitBehavior="false" class="btn btn-outline-danger btn-round-mant" ID="BTN_Eliminar" runat="server" OnClientClick="eliminarFactor(this);">Elimiar</asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                <ItemStyle Width="270px" />
                                                                            </asp:TemplateField> 
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </div>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CategoriaCargaFabril" SortExpression="CategoriaCargaFabril" HeaderText="Categoría" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="LinkButtonMonto" runat="server" Text="Monto" CommandName="Sort" CommandArgument="Monto"></asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:Label ID="LBL_Monto" runat="server" Text='<%# Eval("Monto") %>' DataFormatString="{0:n}" />
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="LBL_FOO_Total" Text="Total carga fabril" runat="server" />
                                                        </FooterTemplate>
                                                        <ItemStyle ForeColor="Black" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="LinkButtonPorcentaje" runat="server" Text="Porcentaje" CommandName="Sort" CommandArgument="Porcentaje"></asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:Label ID="LBL_Porcentaje" runat="server" Text='<%# Eval("Porcentaje") %>' DataFormatString="{0:n}%" />
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="LBL_FOO_Porcentaje" DataFormatString="{0:n}%" runat="server" />
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
            </div>
        </div>
    </div>

    <button type="button" id="BTN_ModalAgregarEditarFactor" data-toggle="modal" data-target="#ModalAgregarEditarFactor" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-md" id="ModalAgregarEditarFactor" tabindex="-1" role="dialog" aria-labelledby="popAgregarEditarFactor" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_AgregarEditarFactor" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server" id="Title_ModalFactor"></h5>
                        </div>
                        <div class="modal-body">
                            <asp:HiddenField ID="HDF_IDFactorCargaFabril" runat="server" Value="0" Visible="true" /> 
                            <div class="row">
                                <div class="col-md-5">
                                    <label for="TXT_DetalleFactor">Factor</label>
                                    <asp:TextBox class="form-control" ID="TXT_DetalleFactor" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="DDL_CategoriasFactores">Categoría</label>
                                    <asp:DropDownList class="form-control" ID="DDL_CategoriasFactores" runat="server"></asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <label for="TXT_Monto">Monto</label>
                            <asp:TextBox class="form-control" ID="TXT_Monto" runat="server" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div style="text-align: right;">
                                <asp:Button ID="BTN_CerrarModalAgregarEditarFactor" UseSubmitBehavior="false" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-primary" />
                            <asp:Button ID="BTN_GuardarFactor" UseSubmitBehavior="false" runat="server" Text="Guardar" CssClass="btn btn-secondary" OnClick="BTN_GuardarFactor_Click" />                               
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
