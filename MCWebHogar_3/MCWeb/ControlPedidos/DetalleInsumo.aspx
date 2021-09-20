<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleInsumo.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ControlPedidos.DetalleInsumo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Detalle Insumo</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <style>
    </style>
    <script type="text/javascript">
        var productosAgregar;

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

        function abrirModalAgregarProductos() {
            productosAgregar = new Map();
            document.getElementById('BTN_ModalAgregarProductos').click()
        }

        function cerrarModalAgregarProductos() {
            productosAgregar = new Map();
            document.getElementById('BTN_ModalAgregarProductos').click()
        }

        function TXT_Cantidad_onKeyUp(txtCantidad, e) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1 + 1
            if (e.keyCode === 13) {
                var rows = $(<%= DGV_ListaProductosInsumo.ClientID %>)[0].rows.length - 1
                var id = ''
                if (index === rows) {
                    id = 'Content_DGV_ListaProductosInsumo_TXT_Cantidad_' + 0
                } else {
                    id = 'Content_DGV_ListaProductosInsumo_TXT_Cantidad_' + index
                }
                document.getElementById(id).autofocus = true;
                document.getElementById(id).focus();
                document.getElementById(id).select();
            }
        }

        function TXT_Cantidad_onChange(txtCantidad) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1
            
            var cantidadProducto = txtCantidad.value
            if (cantidadProducto !== '') {                
                if (cantidadProducto < 0 && cantidadProducto >= 1000) {
                    cantidadProducto = 0;
                }
                txtCantidad.value = cantidadProducto;
                guardarCantidadProductoInsumo(index, cantidadProducto);                
            } else {
                txtCantidad.value = '0';
                guardarCantidadProductoInsumo(index, 0);
            }
        }

        function guardarCantidadProductoInsumo(index, cantidad) {
            var id = 'Content_DGV_ListaProductosInsumo_HDF_IDInsumoDetalle_' + index
            var HDF_IDInsumoDetalle = document.getElementById(id)
            var idInsumoDetalle = HDF_IDInsumoDetalle.value
            var usuario = document.getElementById('Content_HDF_IDUsuario').value;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "DetalleInsumo.aspx/guardarCantidadProductoInsumo",
                data: JSON.stringify({
                    "idInsumoDetalle": idInsumoDetalle,
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

        function TXT_CantidadAgregar_onKeyUp(txtCantidad, e) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1 + 1
            if (e.keyCode === 13) {
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
        }

        function TXT_CantidadAgregar_onChange(txtCantidad) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1
            var idSelect = 'Content_DGV_ListaProductosSinAgregar_CHK_Producto_' + index
            var CHK_Producto = document.getElementById(idSelect)
            var id = 'Content_DGV_ListaProductosSinAgregar_HDF_IDProducto_' + index
            var HDF_IDProducto = document.getElementById(id)
            var idProducto = HDF_IDProducto.value
            var cantidadProducto = txtCantidad.value * 1

            if (cantidadProducto === '' || cantidadProducto === 0 || cantidadProducto === '0') {
                txtCantidad.value = 0
            } else {
                if (cantidadProducto < 0 || cantidadProducto > 999) {
                    cantidadProducto = 0
                    txtCantidad.value = 0
                }
            }
            if (cantidadProducto >= 0 && cantidadProducto < 1000) {
                productosAgregar.set(idProducto, cantidadProducto)
                CHK_Producto.checked = true;
            }
        }

        function CHK_Producto_onChange(CHK_Producto) {
            var values = CHK_Producto.childNodes[0].id.split('_')
            var index = values.pop() * 1

            var idSelect = 'Content_DGV_ListaProductosSinAgregar_CHK_Producto_' + index
            var CHK_Producto = document.getElementById(idSelect)

            var idTXT_Cantidad = 'Content_DGV_ListaProductosSinAgregar_TXT_CantidadAgregar_' + index
            var TXT_Cantidad = document.getElementById(idTXT_Cantidad)

            var id = 'Content_DGV_ListaProductosSinAgregar_HDF_IDProducto_' + index
            var HDF_IDProducto = document.getElementById(id)
            var idProducto = HDF_IDProducto.value

            if (CHK_Producto.checked) {
                productosAgregar.set(idProducto, 0)
            } else {
                productosAgregar.delete(idProducto)
            }
            TXT_Cantidad.value = 0
        }

        function productosMarcados() {
            var table, tbody, i, rowLen, row, j, colLen, cell;

            table = document.getElementById('Content_DGV_ListaProductosSinAgregar');
            tbody = table.tBodies[0];

            for (i = 0, rowLen = tbody.rows.length - 1; i < rowLen; i++) {
                var id = 'Content_DGV_ListaProductosSinAgregar_HDF_IDProducto_' + i

                var HDF_IDProducto = document.getElementById(id)
                var idProducto = HDF_IDProducto.value
                productosAgregar.forEach(function (valor, clave) {
                    if (idProducto === clave) {
                        var idSelect = 'Content_DGV_ListaProductosSinAgregar_CHK_Producto_' + i
                        var CHK_Producto = document.getElementById(idSelect)
                        var idTXT_Cantidad = 'Content_DGV_ListaProductosSinAgregar_TXT_CantidadAgregar_' + i
                        var TXT_Cantidad = document.getElementById(idTXT_Cantidad)
                        CHK_Producto.checked = true
                        TXT_Cantidad.value = valor
                    }
                })
            }
        }

        function cargarProductosAgregar() {
            document.getElementById('Content_LBL_GenerandoInforme').innerText = 'Agregando productos, espere por favor.'
            activarloading();
            var usuario = document.getElementById('Content_HDF_IDUsuario').value;
            var idUsuario = document.getElementById('Content_HDF_UsuarioID').value;
            var idInsumo = document.getElementById('Content_HDF_IDInsumo').value;
            var promises = [];
            productosAgregar.forEach(function (valor, clave) {
                promises.push(
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "DetalleInsumo.aspx/BTN_Agregar_Click",
                        data: JSON.stringify({
                            "idInsumo": idInsumo,
                            "idProducto": clave,
                            "idUsuario": idUsuario,
                            "cantidadProducto": valor,
                            "usuario": usuario
                        }),
                        dataType: "json",
                        success: function (Result) {
                            console.dir(Result)
                        },
                        error: function (Result) {
                            alert("ERROR " + Result.status + ' ' + Result.statusText);
                        }
                    })
                )
            })
            Promise.all(promises).then(function () {
                if (productosAgregar.size > 0) {
                    cerrarModalAgregarProductos();
                    __doPostBack('CargarInsumo')
                    alertifysuccess('Productos agregados con éxito.');
                    desactivarloading();
                } else {
                    alertifywarning('Por favor, seleccione al menos un producto.');
                    cargarFiltros();
                    desactivarloading();
                }
            });

        }
        
        function estilosElementosBloqueados() {
            document.getElementById('<%= TXT_CodigoInsumo.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_CodigoInsumo.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_TotalProductos.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_TotalProductos.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_MontoInsumo.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_MontoInsumo.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_FechaInsumo.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_FechaInsumo.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_HoraInsumo.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_HoraInsumo.ClientID %>').classList.add('form-control')
            document.getElementById('<%= DDL_Propietario.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= DDL_Propietario.ClientID %>').classList.add('form-control')
        }

        function cargarFiltros() {
            $(<%= LB_Categoria.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Categoria' })
        }

        $(document).ready(function () {
            estilosElementosBloqueados();
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
    <div class="wrapper">
        <asp:HiddenField ID="HDF_UsuarioID" runat="server" Value="0" Visible="true" />
        <asp:HiddenField ID="HDF_IDUsuario" runat="server" Value="0" Visible="true" />
        <asp:HiddenField ID="HDF_IDInsumo" runat="server" Value="0" Visible="true" />
        <asp:HiddenField ID="HDF_EstadoInsumo" runat="server" Value="" Visible="false" />
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
                    <li class="active">
                        <a href="Insumos.aspx">
                            <i class="fas fa-box"></i>
                            <p>Insumos</p>
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
                            <img style="width: 75%; display: block; margin-left: 10%;" src="https://mensis.cr/svg/logos/logoMensis.jpg" />
                        </a>
                    </li>
                </ul>
            </div>
        </div>
        <div class="main-panel scroll" style="background: #f4f3ef;">
            <div class="content">
                <div class="container-fluid">
                    <!-- Page Heading -->
                    <h1 class="h3 mb-2 text-gray-800">Detalle insumo</h1>
                    <br />
                    <!-- DataTales Example -->
                    <div class="card shadow mb-4">
                        <asp:UpdatePanel ID="UpdatePanel_Header" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="card-header py-3">
                                    <div class="form-row">
                                        <div class="form-group col-md-2">
                                            <label for="TXT_CodigoInsumo">Número insumo</label>
                                            <asp:TextBox class="form-control" ID="TXT_CodigoInsumo" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="TXT_TotalProductos">Total de productos</label>
                                            <asp:TextBox class="form-control" ID="TXT_TotalProductos" runat="server" TextMode="Number" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="TXT_MontoInsumo">Monto insumo</label>
                                            <asp:TextBox class="form-control" ID="TXT_MontoInsumo" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-4">                                            
                                            <div class="form-row">
                                                <div class="col-md-7">
                                                    <label for="TXT_FechaInsumo">Fecha insumo</label>
                                                    <asp:TextBox ID="TXT_FechaInsumo" runat="server" CssClass="form-control" TextMode="Date" format="dd/MM/yyyy" Enabled="false"></asp:TextBox>
                                                </div>
                                                <div class="col-md-5">
                                                    <label for="TXT_HoraInsumo">Hora insumo</label>
                                                    <asp:TextBox ID="TXT_HoraInsumo" runat="server" CssClass="form-control" TextMode="Time" format="HH:mm" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="DDL_Propietario">Solicitante</label>
                                            <asp:DropDownList class="form-control" ID="DDL_Propietario" runat="server" Enabled="false"></asp:DropDownList>
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
                                            <asp:Button UseSubmitBehavior="false" ID="BTN_AgregarProducto" runat="server" Text="Agregar productos" CssClass="btn btn-secondary" OnClientClick="estilosElementosBloqueados();" OnClick="BTN_CargarProductos_Click"></asp:Button>                                        
                                        </div>                                        
                                        <div class="col-md-6" style="text-align: right;"> 
                                            <asp:Button UseSubmitBehavior="false" ID="BTN_ReporteInsumo" runat="server" Text="Reporte insumo" CssClass="btn btn-secondary" OnClientClick="activarloading();estilosElementosBloqueados();" OnClick="BTN_ReporteInsumo_Click"></asp:Button>                                                                                
                                            <asp:Button UseSubmitBehavior="false" ID="BTN_DescargarInsumo" runat="server" Text="Descargar insumo" CssClass="btn btn-primary" OnClientClick="estilosElementosBloqueados();" OnClick="BTN_DescargarInsumo_Click"></asp:Button>                                        
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="BTN_DescargarInsumo" />
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
                                <asp:UpdatePanel ID="UpdatePanel_ListaProductosInsumo" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="DGV_ListaProductosInsumo" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDInsumoDetalle,InsumoID,ProductoID,UsuarioID" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_ListaProductosInsumo_Sorting"
                                            OnRowCommand="DGV_ListaProductosInsumo_RowCommand"
                                            OnRowDataBound="DGV_ListaProductosInsumo_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="Nombre" SortExpression="Nombre" HeaderText="Usuario" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionProducto" SortExpression="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="CostoProducto" SortExpression="CostoProducto" HeaderText="Costo producto" DataFormatString="{0:n2}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="PrecioProducto" SortExpression="PrecioProducto" HeaderText="Precio producto" DataFormatString="{0:n2}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Cantidad" runat="server" Text="Cantidad insumo"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox class="form-control" TextMode="Number" MaxLength="6" min="0" max="999" style="width: 35%" runat="server" ID="TXT_Cantidad" 
                                                            onkeyup="TXT_Cantidad_onKeyUp(this,event);" onchange="TXT_Cantidad_onChange(this);" Text='<%#Eval("CantidadInsumo") %>' />                                                                                                                                                                                                                                        
                                                        <asp:HiddenField ID="HDF_IDInsumoDetalle" runat="server" Value='<%# Eval("IDInsumoDetalle") %>' />
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
                            <h5 class="modal-title" runat="server">Agregar productos</h5>
                        </div>
                        <div class="modal-body"> 
                            <div class="row">
                                <div class="col-md-5">
                                    <div class="input-group no-border">
                                        <asp:TextBox class="form-control" ID="TXT_BuscarProductosSinAsignar" runat="server" placeholder="Buscar..." OnTextChanged="FiltrarProductos_OnClick" AutoPostBack="true"></asp:TextBox>
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
                            <div class="table-responsive">
                                <asp:UpdatePanel ID="UpdatePanel_ListaProductosSinAgregar" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
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
                                                        <asp:CheckBox ID="CHK_Producto" runat="server" onchange="CHK_Producto_onChange(this);" />
                                                        <asp:HiddenField ID="HDF_IDProducto" runat="server" Value='<%# Eval("IDProducto") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DescripcionProducto" SortExpression="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="PrecioVentaFinal" SortExpression="PrecioVentaFinal" HeaderText="Precio unitario" DataFormatString="{0:n2}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionCategoria" SortExpression="DescripcionCategoria" HeaderText="Categoria" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Cantidad" runat="server" Text="Cantidad"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox class="form-control" TextMode="Number" MaxLength="0" min="0" max="999" style="width: 100%" runat="server" ID="TXT_CantidadAgregar" 
                                                           onkeyup="TXT_CantidadAgregar_onKeyUp(this,event);" onchange="TXT_CantidadAgregar_onChange(this)" Text='0' />
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
                            <asp:Button UseSubmitBehavior="false" ID="BTN_CerrarModalCrearPedido" runat="server" Text="Cerrar" data-dismiss="modal" CssClass="btn btn-primary" />
                            <asp:Button UseSubmitBehavior="false" ID="BTN_Agregar" runat="server" Text="Agregar" CssClass="btn btn-secondary" OnClientClick="cargarProductosAgregar();" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
