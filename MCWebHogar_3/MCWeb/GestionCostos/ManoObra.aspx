<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManoObra.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.GestionCostos.ManoObra" %>

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

        function TXT_Valor_onKeyUp(txtCantidad, e) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1 + 1
            if (e.keyCode === 13) {
                var rows = $(<%= DGV_ListaCostos.ClientID %>)[0].rows.length - 1
                var id = ''
                if (index === rows) {
                    id = 'Content_DGV_ListaCostos_TXT_Valor_' + 0
                } else {
                    id = 'Content_DGV_ListaCostos_TXT_Valor_' + index
                }
                document.getElementById(id).autofocus = true;
                document.getElementById(id).focus();
                document.getElementById(id).select();
            }
        }

        function TXT_Salario_onKeyUp(txtCantidad, e) {
            if ((e.keyCode > 95 && e.keyCode < 106) || (e.keyCode > 47 && e.keyCode < 58) || e.keyCode === 13 || e.keyCode === 8 || e.keyCode === 46) {
                
            } else {
                txtCantidad.value = txtCantidad.defaultValue
                return false
            }
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1 + 1
            if (e.keyCode === 13) {
                var rows = $(<%= DGV_ListaEmpleados.ClientID %>)[0].rows.length - 1
                var id = ''
                if (index === rows) {
                    id = 'Content_DGV_ListaEmpleados_TXT_Salario_' + 0
                } else {
                    id = 'Content_DGV_ListaEmpleados_TXT_Salario_' + index
                }
                document.getElementById(id).autofocus = true;
                document.getElementById(id).focus();
                document.getElementById(id).select();
            }
        }

        function TXT_Descripcion_onKeyUp(txtDescripcion, e) {            
            if (e.keyCode === 13) {
                TXT_Descripcion_onChange(txtDescripcion)
            } else {
                return false
            }
        }

        function TXT_Valor_onChange(txtCantidad) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1
            
            var id = 'Content_DGV_ListaCostos_HDF_IDCosto_' + index
            var HDF_IDCosto = document.getElementById(id)
            var idCosto = HDF_IDCosto.value
            var valor = txtCantidad.value * 1

            if (valor === '' || valor === 0 || valor === '0') {
                txtCantidad.value = 1
            } else {
                if (valor < 0 || valor > 99) {
                    valor = 1
                    txtCantidad.value = 1
                }
            }
            if (valor > 0 && valor < 100) {
                actualizarValorCosto(idCosto, valor)
            }
        }

        function abrirModalAgregarEmpleado() {
            document.getElementById('BTN_ModalAgregarEmpleado').click()
        }

        function cerrarModalAgregarEmpleado() {
            document.getElementById('BTN_ModalAgregarEmpleado').click()
        }

        function TXT_Salario_onChange(txtCantidad) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1

            var id = 'Content_DGV_ListaEmpleados_HDF_IDEmpleado_' + index
            var HDF_IDEmpleado = document.getElementById(id)
            var idEmpleado = HDF_IDEmpleado.value
            var salario = txtCantidad.value.replace(",", "") * 1
            
            if (salario === '' || salario === 0 || salario === '0') {
                txtCantidad.value = 1
            } else {
                if (salario < 0) {
                    salario = 1
                    txtCantidad.value = 1
                }
            }
            if (salario > 0) {
                actualizarSalarioEmpleado(idEmpleado, salario)
            }
        }

        function TXT_Descripcion_onChange(txtDescripcion) {
            var values = txtDescripcion.id.split('_')
            var index = values.pop() * 1

            var id = 'Content_DGV_ListaEmpleados_HDF_IDEmpleado_' + index
            var HDF_IDEmpleado = document.getElementById(id)
            var idEmpleado = HDF_IDEmpleado.value
            var descripcionPuesto = txtDescripcion.value

            if (descripcionPuesto === '') {
                alertifywarning('Por favor, ingrese la descripción del puesto')
            } else {
                actualizarDescripcionEmpleado(idEmpleado, descripcionPuesto)
            }
        }

        function actualizarValorCosto(idCosto, valor) {
            var usuario = document.getElementById('Content_HDF_IDUsuario').value;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Costos.aspx/BTN_ActualizarValorCosto_Click",
                data: JSON.stringify({
                    "idCosto": idCosto,
                    "valor": valor,
                    "usuario": usuario
                }),
                dataType: "json",
                success: function (Result) {
                    __doPostBack('CargarCostos')
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })
        }

        function actualizarSalarioEmpleado(idEmpleado, salario) {
            var usuario = document.getElementById('Content_HDF_IDUsuario').value;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ManoObra.aspx/BTN_ActualizarSalarioEmpleado_Click",
                data: JSON.stringify({
                    "idEmpleado": idEmpleado,
                    "salario": salario,
                    "usuario": usuario
                }),
                dataType: "json",
                success: function (Result) {
                    __doPostBack('CargarCostos')
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })
        }

        function actualizarDescripcionEmpleado(idEmpleado, descripcion) {
            var usuario = document.getElementById('Content_HDF_IDUsuario').value;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ManoObra.aspx/BTN_ActualizarDescripcionEmpleado_Click",
                data: JSON.stringify({
                    "idEmpleado": idEmpleado,
                    "descripcion": descripcion,
                    "usuario": usuario
                }),
                dataType: "json",
                success: function (Result) {
                    __doPostBack('CargarCostos')
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })
        }
        
        function cargarFiltros() {
            
        }

        $(document).ready(function () {
            cargarFiltros();
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
                                    <h1 class="h3 mb-2 text-gray-800" runat="server" id="H1_Title">Mano de obra</h1>
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
                                                <a href="ManoObra.aspx" class="btn btn-info">
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
                            <div class="table">
                                <div class="col-md-12">                                        
                                    <asp:UpdatePanel ID="UpdatePanel_ListaCostos" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="input-group no-border col-md-6" style="text-align: left; display: inline-block;">
                                                    <h1 class="h3 mb-2 text-gray-800" runat="server" id="H2">Factores</h1>
                                                </div>
                                            </div>  
                                            <asp:GridView ID="DGV_ListaCostos" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                AutoGenerateColumns="False" DataKeyNames="ID,Modulo,Codigo,Valor" 
                                                HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true" 
                                                EmptyDataText="No hay costos que cargar." AllowSorting="true">
                                                <Columns>
                                                    <asp:BoundField DataField="Descripcion" SortExpression="Descripcion" HeaderText="Descripción" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="LBL_Valor" runat="server" Text="Valor porcentaje %"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="HDF_IDCosto" runat="server" Value='<%# Eval("ID") %>' />
                                                            <asp:TextBox class="form-control" TextMode="Number" MaxLength="0" min="0" max="100" Style="width: 15%;text-align: end;" runat="server" ID="TXT_Valor" Text='<%# Eval("Valor") %>' 
                                                                onkeyup="TXT_Valor_onKeyUp(this,event);" onchange="TXT_Valor_onChange(this)" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <hr />
                                <div class="col-md-12">                                        
                                    <asp:UpdatePanel ID="UpdatePanel_ListaEmpleados" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="input-group no-border col-md-6" style="text-align: left; display: inline-block;">
                                                    <h1 class="h3 mb-2 text-gray-800" runat="server" id="H1" style="display: initial;">Empleados</h1>
                                                    <asp:LinkButton UseSubmitBehavior="false" class="btn btn-round-mant" ID="BTN_AgregarEmpleado" runat="server" OnClick="BTN_AgregarEmpleado_Click"><i class="fas fa-plus-circle"></i></asp:LinkButton>
                                                </div>
                                            </div>  
                                            <asp:GridView ID="DGV_ListaEmpleados" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                AutoGenerateColumns="False" DataKeyNames="" 
                                                HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true" 
                                                EmptyDataText="No hay empleados que cargar." AllowSorting="true">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="LBL_Descripcion" runat="server" Text="Descripción del puesto"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox class="form-control" MaxLength="0" min="0" Style="width: 100%; text-align: left;" runat="server" ID="TXT_Descripcion" Text='<%# Eval("Descripcion") %>'
                                                                onkeyup="TXT_Descripcion_onKeyUp(this,event);" onchange="TXT_Descripcion_onChange(this)" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="LBL_Salario" runat="server" Text="Salario"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="HDF_IDEmpleado" runat="server" Value='<%# Eval("IDEmpleado") %>' />
                                                            <asp:TextBox class="form-control" MaxLength="0" min="0" Style="width: 100%; text-align: end;" runat="server" ID="TXT_Salario" Text='<%# Eval("Salario", "{0:n}") %>'
                                                                onkeyup="TXT_Salario_onKeyUp(this,event);" onchange="TXT_Salario_onChange(this)" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CargasSociales" SortExpression="CargasSociales" HeaderText="Cargas sociales" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="Aguinaldo" SortExpression="Aguinaldo" HeaderText="Aguinaldo" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="RiesgosTrabajo" SortExpression="RiesgosTrabajo" HeaderText="Riesgos trabajo" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="Cesantia" SortExpression="Cesantia" HeaderText="Cesantia" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="SalarioTotal" SortExpression="SalarioTotal" HeaderText="Salario total" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="SalarioDiario" SortExpression="SalarioDiario" HeaderText="Salario diario" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="SalarioHora" SortExpression="SalarioHora" HeaderText="Salario hora" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="SalarioMinuto" SortExpression="SalarioMinuto" HeaderText="Salario minuto" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                    
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
    
    <button type="button" id="BTN_ModalAgregarEmpleado" data-toggle="modal" data-target="#ModalAgregarEmpleado" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-md" id="ModalAgregarEmpleado" tabindex="-1" role="dialog" aria-labelledby="popAgregarEmpleado" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_AgregarEmpleado" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server" id="Title_ModalEmpleado">Agregar empleado</h5>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <label for="TXT_DetalleFactor">Descripción puesto</label>
                                    <asp:TextBox class="form-control" ID="TXT_DetalleEmpleado" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label for="TXT_MontoSalario">Salario</label>
                                    <asp:TextBox class="form-control" ID="TXT_MontoSalario" runat="server" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div style="text-align: right;">
                                <asp:Button ID="BTN_CerrarModalAgregarEmpleado" UseSubmitBehavior="false" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-primary" />
                                <asp:Button ID="BTN_GuardarEmpleado" UseSubmitBehavior="false" runat="server" Text="Guardar" CssClass="btn btn-secondary" OnClick="BTN_GuardarEmpleado_Click" />                               
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
