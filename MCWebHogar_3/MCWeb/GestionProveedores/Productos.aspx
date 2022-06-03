<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.GestionProveedores.Productos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Productos</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <style>
        
    </style>
    <script type="text/javascript">
        var unidadesMedida

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

        function graficoHistoricoPrecios(dias, datos, producto) {
            Highcharts.chart('containerHistoricoPrecios', {
                chart: {
                    type: 'line'
                },
                title: {
                    text: 'Histórico precios'
                },
                yAxis: {
                    title: {
                        text: ''
                    }
                },
                xAxis: {
                    categories: dias
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function () {
                                    console.log('Fecha: ' + this.category);
                                }
                            }
                        }
                    },
                    column: {
                        dataLabels: {
                            align: 'left',
                            enabled: true,
                            rotation: 270,
                            x: 2,
                            y: -10
                        }
                    }
                },
                series: [{
                    name: producto,
                    data: datos
                }]
            });
        }

        function graficoHistoricoVariacion(dias, datos, producto) {
            Highcharts.chart('containerHistoricoVariacion', {
                chart: {
                    type: 'line'
                },
                title: {
                    text: 'Histórico variación'
                },
                yAxis: {
                    title: {
                        text: ''
                    }
                },
                xAxis: {
                    categories: dias
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true,
                            format: '{point.y:.2f}%'
                        },
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function () {
                                    console.log('Fecha: ' + this.category);
                                }
                            }
                        }
                    },
                    column: {
                        dataLabels: {
                            align: 'left',
                            enabled: true,
                            rotation: 270,
                            x: 2,
                            y: -10
                        }
                    }
                },
                series: [{
                    name: producto,
                    data: datos
                }]
            });
        }

        function cargarGraficos(idUsuario, idProducto) {
            var descripcionProducto = document.getElementById('<%= TXT_DetalleProductoD.ClientID %>').value
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Productos.aspx/cargarGraficoHistoricoPrecios",
                data: JSON.stringify({
                    "idUsuario": idUsuario,
                    "idProducto": idProducto,
                }),
                dataType: "json",
                success: function (Result) {
                    listaDias = []
                    listaPrecios = []
                    listaDiferencias = []
                    var precio = 0
                    var diferencia = 0
                    for (var i in Result.d) {
                        var dia = Result.d[i].dia
                        
                        if (precio !== Result.d[i].precioUnitario) {
                            if (precio > 0) {
                                diferencia = (Result.d[i].precioUnitario - precio) / precio * 100
                            } else {
                                diferencia = 0
                            }
                            precio = Result.d[i].precioUnitario
                            listaDiferencias.push(diferencia)
                            listaDias.push(dia)
                            listaPrecios.push(precio)
                        }                        
                    }
                    graficoHistoricoVariacion(listaDias, listaDiferencias, descripcionProducto)
                    graficoHistoricoPrecios(listaDias, listaPrecios, descripcionProducto)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })
        }

        function abrirModalVerProductos() {
            document.getElementById('BTN_ModalVerProductos').click()
        }

        function cerrarModalVerProductos() {
            document.getElementById('BTN_ModalVerProductos').click()
        }

        function abrirModalEditarProducto() {
            unidadesMedida = new Map()
            document.getElementById('<%= BTN_GuardarProducto.ClientID %>').hidden = true
            document.getElementById('BTN_ModalEditarProducto').click()
        }

        function cerrarModalEditarProducto() {
            unidadesMedida = new Map()
            document.getElementById('BTN_ModalEditarProducto').click()
        }

        function TXT_DetalleProducto_onKeyUp() {
            document.getElementById('<%= BTN_GuardarProducto.ClientID %>').hidden = false
        }

        function TXT_CantidadEquivalente_onKeyUp(txtCantidad, e) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1 + 1
            if (e.keyCode === 13) {
                var rows = $(<%= DGV_UnidadesMedida.ClientID %>)[0].rows.length - 1
                var id = ''
                if (index === rows) {
                    id = 'Content_DGV_UnidadesMedida_TXT_CantidadEquivalente_' + 0
                } else {
                    id = 'Content_DGV_UnidadesMedida_TXT_CantidadEquivalente_' + index
                }

                document.getElementById(id).autofocus = true;
                document.getElementById(id).focus();
                document.getElementById(id).select();
            }

            document.getElementById('<%= BTN_GuardarProducto.ClientID %>').hidden = false
        }

        function TXT_CantidadEquivalente_onChange(txtCantidad) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1
            var id = 'Content_DGV_UnidadesMedida_HDF_IDUnidadMedida_' + index
            var HDF_IDUnidadMedida = document.getElementById(id)
            var idUnidadMedida = HDF_IDUnidadMedida.value
            var cantidadEquivalente = txtCantidad.value * 1

            if (cantidadEquivalente === '' || cantidadEquivalente === 0 || cantidadEquivalente === '0') {
                txtCantidad.value = 1
            } else {
                if (cantidadEquivalente < 0 || cantidadEquivalente > 999) {
                    cantidadEquivalente = 1
                    txtCantidad.value = 1
                }
            }
            if (cantidadEquivalente >= 0 && cantidadEquivalente < 1000) {
                unidadesMedida.set(idUnidadMedida, cantidadEquivalente)
            }

            document.getElementById('<%= BTN_GuardarProducto.ClientID %>').hidden = false
        }

        function actualizarProducto() {
            // document.getElementById('Content_LBL_GenerandoInforme').innerText = 'Agregando productos, espere por favor.'
            // activarloading();
            var usuario = document.getElementById('Content_HDF_IDUsuario').value;
            var idProducto = document.getElementById('Content_HDF_IDProducto').value;

            var detalleProducto = document.getElementById('<%= TXT_DetalleProducto.ClientID %>').value;
            var codigoProductoWebpos = document.getElementById('<%= TXT_CodigoPOS.ClientID %>').value;
            var categoria = document.getElementById('<%= DDL_Categoria.ClientID %>').value;
            var promises = [];
            promises.push(
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Productos.aspx/BTN_GuardarProducto_Click",
                    data: JSON.stringify({
                        "idProducto": idProducto,
                        "detalleProducto": detalleProducto,
                        "codigoProductoWebpos": codigoProductoWebpos,
                        "categoria": categoria,
                        "usuario": usuario
                    }),
                    dataType: "json",
                    success: function (Result) {
                        // console.dir(Result)
                    },
                    error: function (Result) {
                        alert("ERROR " + Result.status + ' ' + Result.statusText);
                    }
                })
            )
            unidadesMedida.forEach(function (valor, clave) {
                promises.push(
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "Productos.aspx/BTN_GuardarUnidadesMedida_Click",
                        data: JSON.stringify({
                            "idProducto": idProducto,
                            "idUnidadMedida": clave,
                            "cantidadEquivalente": valor,
                            "usuario": usuario
                        }),
                        dataType: "json",
                        success: function (Result) {
                            // console.dir(Result)
                        },
                        error: function (Result) {
                            alert("ERROR " + Result.status + ' ' + Result.statusText);
                        }
                    })
                )
            })
            Promise.all(promises).then(function () {
                cerrarModalEditarProducto();
                alertifysuccess('Actualizacion exitosa');

                // cargarFiltros();
                // desactivarloading();
            });
        }

        function seleccionarReceptor(receptor) {
            if (receptor === "MiKFe") {
                __doPostBack('Identificacion;3101485961')
            } else if (receptor === "Esteban") {
                __doPostBack('Identificacion;115210651')
            }
        }

        function validarEditarProducto() {
            return true
        }

        function cargarFiltros() {
            $(<%= LB_Emisores.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Proveedor' })
            $(<%= LB_Categorias.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Categoria' })
        }

        $(document).ready(function () {
            
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
        <div class="sidebar" data-color="white" data-active-color="danger">
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
                    <li id="li_MiKFe" runat="server">
                        <a href="#" onclick="seleccionarReceptor('MiKFe');">
                            <i class="fas fa-cart-plus"></i>
                            <p>Proveedores - Mi K Fe</p>
                        </a>
                    </li>
                    <li id="li_Esteban" runat="server">
                        <a href="#" onclick="seleccionarReceptor('Esteban');">
                            <i class="fas fa-cart-plus"></i>
                            <p>Proveedores - Esteban</p>
                        </a>
                    </li>
                </ul>
                <hr style="width: 230px; color: #2c2c2c;" />
                <h5 style="text-align: center;">Mantenimiento</h5>
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
                    <div class="row">
                        <div class="input-group no-border col-md-6" style="text-align: left; display: inline-block;">
                            <h1 class="h3 mb-2 text-gray-800" runat="server" id="H1_Title">Productos</h1>
                        </div>
                        <div class="input-group no-border col-md-6" style="text-align: right; display: inline-block;">
                            <asp:LinkButton ID="BTN_Sincronizar" runat="server" CssClass="btn btn-secundary" OnClick="BTN_Sincronizar_Click" OnClientClick="activarloading();">
                        <i class="fas fa-sync"></i> Sincronizar
                            </asp:LinkButton>
                        </div>
                    </div>

                    <div class="card shadow mb-4">
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel_FiltrosProductos" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>                           
                                    <div class="row" style="height: 50px;">                         
                                        <div class="input-group no-border col-md-3" style="text-align:center; display: block;">
                                            <a href="Proveedores.aspx" class="btn btn-primary">
                                                <i class="fas fa-cart-plus"></i> Proveedores
                                            </a>
                                        </div>       
                                        <div class="input-group no-border col-md-3" style="text-align:center; display: block;">
                                            <a href="Facturas.aspx" class="btn btn-primary">
                                                <i class="fas fa-file-invoice"></i> Facturas
                                            </a>
                                        </div>       
                                        <div class="input-group no-border col-md-3" style="text-align:center; display: block;">
                                            <a href="Productos.aspx" class="btn btn-info">
                                                <i class="fas fa-barcode"></i> Productos
                                            </a>
                                        </div>
                                        <div class="input-group no-border col-md-3" style="text-align:center; display: block;">
                                            <a href="Reportes.aspx" class="btn btn-primary">
                                                <i class="fas fa-fw fa-tachometer-alt"></i> Reportes
                                            </a>
                                        </div> 
                                    </div>    
                                    <hr />                    
                                    <div class="input-group no-border col-md-3">
                                        <asp:TextBox class="form-control" ID="TXT_Buscar" runat="server" placeholder="Buscar..." OnTextChanged="FiltrarProductos_OnClick" AutoPostBack="true"></asp:TextBox>
                                        <div class="input-group-append">
                                            <div class="input-group-text">
                                                <i class="nc-icon nc-zoom-split"></i>
                                            </div>
                                        </div>                                        
                                    </div>
                                    <div class="col-md-2">
                                        <asp:ListBox class="form-control" runat="server" ID="LB_Emisores" SelectionMode="Multiple" OnTextChanged="FiltrarProductos_OnClick" AutoPostBack="true"></asp:ListBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:ListBox class="form-control" runat="server" ID="LB_Categorias" SelectionMode="Multiple" OnTextChanged="FiltrarProductos_OnClick" AutoPostBack="true"></asp:ListBox>
                                    </div>
                                    <div class="col-md-2">                                        
                                        <asp:DropDownList class="form-control" style="font-size: 18px;" ID="DDL_TipoProducto" runat="server" OnSelectedIndexChanged="FiltrarProductos_OnClick" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                    <div class="input-group no-border col-md-3" style="text-align: right; display: inline-block;">
                                        <asp:Button ID="BTN_DescargarProducto" runat="server" UseSubmitBehavior="false" Text="Descargar productos" CssClass="btn btn-info" style="margin-top: 0px;" OnClientClick="activarloading();desactivarloading();" OnClick="BTN_DescargarProducto_OnClick" AutoPostBack="true"></asp:Button>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="BTN_DescargarProducto" />
                                </Triggers>
                            </asp:UpdatePanel> 
                            <div class="table">
                                <asp:UpdatePanel ID="UpdatePanel_ListaProductos" runat="server" UpdateMode="Conditional" style="margin-top: 7rem;">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaProductos" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDProducto,PrecioUnitario,PorcentajeImpuesto,FacturaID" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnRowCommand="DGV_ListaProductos_RowCommand"
                                            OnSorting="DGV_ListaProductos_Sorting">
                                            <%--OnRowDataBound="DGV_ListaProductos_OnRowDataBound">--%>
                                            <Columns> 
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Ver" runat="server" Text="Detalle"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton class="btn btn-outline-info btn-round-mant" ID="BTN_VerProductos" runat="server"
                                                            CommandName="VerProductos"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            AutoPostBack="true" > 
                                                        <i class="fas fa-eye"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>                                              
                                                <asp:BoundField DataField="DetalleProducto" SortExpression="DetalleProducto" HeaderText="Descripcion" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                                <asp:BoundField DataField="FechaActualizacion" SortExpression="FechaActualizacion" HeaderText="Fecha actualizado" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="PrecioUnitario" SortExpression="PrecioUnitario" HeaderText="Precio unitario" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="MontoDescuento" SortExpression="MontoDescuento" HeaderText="Descuento" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="PorcentajeImpuesto" SortExpression="PorcentajeImpuesto" HeaderText="Impuesto" DataFormatString="{0:n0}%" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="MontoImpuesto" SortExpression="MontoImpuesto" HeaderText="Monto impuesto" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="MontoImpuestoIncluido" SortExpression="MontoImpuestoIncluido" HeaderText="Monto IVAI" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="Porcentaje25" SortExpression="Porcentaje25" HeaderText="25%" ItemStyle-ForeColor="black" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                                                                                               
                                                <asp:BoundField DataField="PrecioVenta" SortExpression="PrecioVenta" HeaderText="Precio venta" ItemStyle-ForeColor="black" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                                                                                               
                                                <asp:BoundField DataField="ProductoMateriaPrima" SortExpression="ProductoMateriaPrima" HeaderText="Materia prima" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                                                                                               
                                                <asp:BoundField DataField="ProductoVenta" SortExpression="ProductoVenta" HeaderText="Venta" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                                                                                               
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Acciones" runat="server" Text="Acciones"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>                                                        
                                                        <asp:Button UseSubmitBehavior="false" class="btn btn-outline-primary btn-round-mant" ID="BTN_Editar" runat="server"
                                                            CommandName="editar"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            Text="Editar" AutoPostBack="true" />                                                        
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

    <button type="button" id="BTN_ModalVerProductos" data-toggle="modal" data-target="#ModalVerProductos" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalVerProductos" tabindex="-1" role="dialog" aria-labelledby="popVerProductos" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_VerProductos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" style="max-width: 1400px;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server" id="H2">Producto</h5>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel_DetalleProductos" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <label for="TXT_DetalleProductoD">Detalle del producto:</label>
                                            <asp:TextBox ID="TXT_DetalleProductoD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label for="TXT_FechaActualizadoD">Fecha actualizado:</label>
                                            <asp:TextBox ID="TXT_FechaActualizadoD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-2">
                                            <label for="TXT_PrecioUnitarioD">Precio unitario:</label>
                                            <asp:TextBox ID="TXT_PrecioUnitarioD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label for="TXT_MontoDescuentoD">Monto descuento:</label>
                                            <asp:TextBox ID="TXT_MontoDescuentoD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <label for="TXT_ImpuestoD">Impuesto:</label>
                                            <asp:TextBox ID="TXT_ImpuestoD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label for="TXT_MontoImpuestoD">Monto impuesto:</label>
                                            <asp:TextBox ID="TXT_MontoImpuestoD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label for="TXT_MontoImpuestoIncluidoD">Monto IVAI:</label>
                                            <asp:TextBox ID="TXT_MontoImpuestoIncluidoD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <label for="TXT_Porcentaje25D">25%:</label>
                                            <asp:TextBox ID="TXT_Porcentaje25D" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label for="TXT_PrecioVentaD">Precio venta:</label>
                                            <asp:TextBox ID="TXT_PrecioVentaD" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div> 
                                    </div>
                                    <br />
                                    <div class="row" id="CardReportes">
                                        <div class="col-md-6">
                                            <div class="card card-chart">
                                                <figure class="highcharts-figure">
                                                    <div id="containerHistoricoPrecios"></div>
                                                </figure>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="card card-chart">
                                                <figure class="highcharts-figure">
                                                    <div id="containerHistoricoVariacion"></div>
                                                </figure>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <asp:GridView ID="DGV_DetelleProductos" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        AutoGenerateColumns="False" DataKeyNames="IDLineaDetalle,ProductoID,FacturaID,CodigoProducto,DetalleProducto,UnidadMedida,NumeroFactura" HeaderStyle-CssClass="table" 
                                        BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true">
                                        <%--OnSorting="DGV_ListaFacturas_Sorting"
                                            OnRowDataBound="DGV_ListaFacturas_OnRowDataBound"
                                            OnRowCommand="DGV_ListaFacturas_RowCommand">--%>
                                        <Columns>                                               
                                            <asp:BoundField DataField="NumeroFactura" SortExpression="NumeroFactura" HeaderText="Factura" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                            <asp:BoundField DataField="FechaFactura" SortExpression="FechaFactura" HeaderText="Fecha Factura" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                            <asp:BoundField DataField="UnidadMedida" SortExpression="UnidadMedida" HeaderText="Unidad medida" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="Cantidad" SortExpression="Cantidad" HeaderText="Cantidad" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="PrecioUnitarioFinal" SortExpression="PrecioUnitarioFinal" HeaderText="Precio unitario" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="MontoDescuento" SortExpression="MontoDescuento" HeaderText="Descuento" DataFormatString="{0:n}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="PorcentajeImpuesto" SortExpression="PorcentajeImpuesto" HeaderText="Impuesto" ItemStyle-ForeColor="black" DataFormatString="{0:n0}%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                                                                                                                                               
                                            <asp:BoundField DataField="MontoTotalIVA" SortExpression="MontoTotalIVA" HeaderText="Monto total" ItemStyle-ForeColor="black" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                                                                                                                                                               
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="BTN_CerrarModalVerProductos" runat="server" UseSubmitBehavior="false" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <button type="button" id="BTN_ModalEditarProducto" data-toggle="modal" data-target="#ModalEditarProducto" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalEditarProducto" tabindex="-1" role="dialog" aria-labelledby="popEditarProducto" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalEditarProducto" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server" id="title_CrearPedido">Editar producto</h5>
                        </div>
                        <div class="modal-body">
                            <asp:HiddenField ID="HDF_IDProducto" runat="server" Value="0" />
                            <div class="row">
                                <div class="col-md-6">
                                    <label for="TXT_DetalleProducto">Detalle del producto:</label>
                                    <asp:TextBox ID="TXT_DetalleProducto" runat="server" CssClass="form-control" onkeyup="TXT_DetalleProducto_onKeyUp();"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label for="DDL_Categoria">Categoria:</label>
                                    <asp:DropDownList ID="DDL_Categoria" runat="server" CssClass="form-control" onchange="TXT_DetalleProducto_onKeyUp();"></asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <label for="TXT_CodigoPOS">Codigo Webpos:</label>
                                    <asp:TextBox ID="TXT_CodigoPOS" runat="server" CssClass="form-control" onkeyup="TXT_DetalleProducto_onKeyUp();"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-3">
                                    <label for="TXT_FechaActualizado">Fecha actualizado:</label>
                                    <asp:TextBox ID="TXT_FechaActualizado" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label for="TXT_PrecioUnitario">Precio unitario:</label>
                                    <asp:TextBox ID="TXT_PrecioUnitario" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label for="TXT_MontoDescuento">Monto descuento:</label>
                                    <asp:TextBox ID="TXT_MontoDescuento" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label for="TXT_Impuesto">Impuesto:</label>
                                    <asp:TextBox ID="TXT_Impuesto" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-3">
                                    <label for="TXT_MontoImpuesto">Monto impuesto:</label>
                                    <asp:TextBox ID="TXT_MontoImpuesto" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label for="TXT_MontoImpuestoIncluido">Monto IVAI:</label>
                                    <asp:TextBox ID="TXT_MontoImpuestoIncluido" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label for="TXT_Porcentaje25">25%:</label>
                                    <asp:TextBox ID="TXT_Porcentaje25" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label for="TXT_PrecioVenta">Precio venta:</label>
                                    <asp:TextBox ID="TXT_PrecioVenta" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <asp:GridView ID="DGV_UnidadesMedida" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                AutoGenerateColumns="False" DataKeyNames="IDUnidadMedida,ProductoID,UnidadMedida,CantidadEquivalente" HeaderStyle-CssClass="table" 
                                BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None" ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true">
                                <%--OnSorting="DGV_UnidadesMedida_Sorting"
                                    OnRowDataBound="DGV_UnidadesMedida_OnRowDataBound"
                                    OnRowCommand="DGV_UnidadesMedida_RowCommand">--%>
                                <Columns>                                               
                                    <asp:BoundField DataField="UnidadMedida" SortExpression="UnidadMedida" HeaderText="Unidad medida" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="LBL_Cantidad" runat="server" Text="Cantidad equivalente"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="HDF_IDUnidadMedida" runat="server" Value='<%# Eval("IDUnidadMedida") %>' />
                                            <asp:TextBox class="form-control" TextMode="Number" MaxLength="0" min="0" max="999" style="width: 100%" runat="server" ID="TXT_CantidadEquivalente" 
                                                onkeyup="TXT_CantidadEquivalente_onKeyUp(this,event);" onchange="TXT_CantidadEquivalente_onChange(this)" Text='<%#Eval("CantidadEquivalente") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="20px" />                                          
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="BTN_CerrarModalEditarProducto" runat="server" UseSubmitBehavior="false" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-primary" />
                            <asp:Button ID="BTN_GuardarProducto" runat="server" UseSubmitBehavior="false" Text="Guardar producto" CssClass="btn btn-secondary" OnClientClick="actualizarProducto();" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
