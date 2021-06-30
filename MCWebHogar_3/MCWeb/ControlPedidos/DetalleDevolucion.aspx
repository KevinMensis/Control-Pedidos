<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleDevolucion.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ControlPedidos.DetalleDevolucion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Detalle Devolucion</title>
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
                var rows = $(<%= DGV_ListaProductosDevolucion.ClientID %>)[0].rows.length - 1
                var id = ''
                if (index === rows) {
                    id = 'Content_DGV_ListaProductosDevolucion_TXT_Cantidad_' + 0
                } else {
                    id = 'Content_DGV_ListaProductosDevolucion_TXT_Cantidad_' + index
                }
                document.getElementById(id).autofocus = true;
                document.getElementById(id).focus();
                document.getElementById(id).select();
            }
        }

        function TXT_Cantidad_onChange(txtCantidad) {
            var values = txtCantidad.id.split('_')
            var index = values.pop() * 1
            var idUnidades = 'Content_DGV_ListaProductosDevolucion_DDL_Unidades_' + index
            var idDecenas = 'Content_DGV_ListaProductosDevolucion_DDL_Decenas_' + index
            var idCentenas = 'Content_DGV_ListaProductosDevolucion_DDL_Centenas_' + index
            var cantidadProducto = txtCantidad.value
            if (cantidadProducto !== '') {
                var unds = cantidadProducto % 10;
                var decs = Math.trunc(cantidadProducto / 10) % 10;
                var cents = Math.trunc(cantidadProducto / 100);
                if (cantidadProducto >= 0 && cantidadProducto < 1000) {
                    document.getElementById(idUnidades).value = unds;
                    document.getElementById(idDecenas).value = decs;
                    document.getElementById(idCentenas).value = cents;
                    txtCantidad.value = cantidadProducto;
                    guardarCantidadProductoDevolucion(index, cantidadProducto);
                }
                else {
                    unds = document.getElementById(idUnidades).value * 1;
                    decs = document.getElementById(idDecenas).value * 10;
                    cents = document.getElementById(idCentenas).value * 100;
                    cantidadProducto = cents + decs + unds;
                    txtCantidad.value = cantidadProducto;
                }
            }
            else {
                txtCantidad.value = '0';
                document.getElementById(idUnidades).value = '0';
                document.getElementById(idDecenas).value = '0';
                document.getElementById(idCentenas).value = '0';
                guardarCantidadProductoDevolucion(index, 0);
            }
        }

        function guardarCantidadProductoDevolucion(index, cantidad) {
            var id = 'Content_DGV_ListaProductosDevolucion_HDF_IDDevolucionDetalle_' + index
            var HDF_IDDevolucionDetalle = document.getElementById(id)
            var idDevolucionDetalle = HDF_IDDevolucionDetalle.value
            var usuario = document.getElementById('Content_HDF_IDUsuario').value;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "DetalleDevolucion.aspx/guardarProductoDevolucion",
                data: JSON.stringify({
                    "idDevolucionDetalle": idDevolucionDetalle,
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
            console.dir(productosAgregar)
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
            var idDevolucion = document.getElementById('Content_HDF_IDDevolucion').value;
            var idPuntoVenta = document.getElementById('Content_DDL_PuntoVenta').value;
            if (idPuntoVenta !== 0 && idPuntoVenta !== "0") {
                var promises = [];
                productosAgregar.forEach(function (valor, clave) {
                    promises.push(
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "DetalleDevolucion.aspx/BTN_Agregar_Click",
                            data: JSON.stringify({
                                "idDevolucion": idDevolucion,
                                "idPuntoVenta": idPuntoVenta,
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
                        __doPostBack('CargarDevolucion')
                        desactivarloading();
                        alertifysuccess('Productos agregados con éxito.');                        
                    } else {
                        cargarFiltros();
                        desactivarloading();
                        alertifywarning('Por favor, seleccione al menos un producto.');
                    }
                });
            } else {
                cargarFiltros();
                desactivarloading();
                document.getElementById('Content_DDL_PuntoVenta').focus();
                alertifywarning('Por favor, seleccione el punto de venta.');                
            }
        }
        
        function estilosElementosBloqueados() {
            document.getElementById('<%= TXT_CodigoDevolucion.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_CodigoDevolucion.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_TotalProductos.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_TotalProductos.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_MontoDevolucion.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_MontoDevolucion.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_FechaDevolucion.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_FechaDevolucion.ClientID %>').classList.add('form-control')
            document.getElementById('<%= TXT_HoraDevolucion.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= TXT_HoraDevolucion.ClientID %>').classList.add('form-control')
            document.getElementById('<%= DDL_Propietario.ClientID %>').classList.remove('aspNetDisabled')
            document.getElementById('<%= DDL_Propietario.ClientID %>').classList.add('form-control')
        }

        function cargarFiltros() {
            $(<%= LB_Categoria.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Categoria' })
            $(<%= LB_PuntoVenta.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Punto venta' })
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
        <asp:HiddenField ID="HDF_IDDevolucion" runat="server" Value="0" Visible="true" />
        <asp:HiddenField ID="HDF_EstadoDevolucion" runat="server" Value="" Visible="false" />
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
                    <li class="active">
                        <a href="Devoluciones.aspx">
                            <i class="fas fa-trash"></i>
                            <p>Devoluciones</p>
                        </a>
                    </li>
                    <li>
                        <a href="Desechos.aspx">
                            <i class="fas fa-undo-alt"></i>
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
                    <h1 class="h3 mb-2 text-gray-800">Detalle devolución</h1>
                    <br />
                    <!-- DataTales Example -->
                    <div class="card shadow mb-4">
                        <asp:UpdatePanel ID="UpdatePanel_Header" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="card-header py-3">
                                    <div class="form-row">
                                        <div class="form-group col-md-2">
                                            <label for="TXT_CodigoDevolucion">Número devolución</label>
                                            <asp:TextBox class="form-control" ID="TXT_CodigoDevolucion" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="TXT_TotalProductos">Total de productos</label>
                                            <asp:TextBox class="form-control" ID="TXT_TotalProductos" runat="server" TextMode="Number" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="TXT_MontoDevolucion">Monto devolución</label>
                                            <asp:TextBox class="form-control" ID="TXT_MontoDevolucion" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-4">                                            
                                            <div class="form-row">
                                                <div class="col-md-7">
                                                    <label for="TXT_FechaDevolucion">Fecha devolución</label>
                                                    <asp:TextBox ID="TXT_FechaDevolucion" runat="server" CssClass="form-control" TextMode="Date" format="dd/MM/yyyy" Enabled="false"></asp:TextBox>
                                                </div>
                                                <div class="col-md-5">
                                                    <label for="TXT_FechaDevolucion">Hora devolución</label>
                                                    <asp:TextBox ID="TXT_HoraDevolucion" runat="server" CssClass="form-control" TextMode="Time" format="HH:mm" Enabled="false"></asp:TextBox>
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
                                            <asp:Button UseSubmitBehavior="false" ID="BTN_ReporteDevolucion" runat="server" Text="Reporte devolución" CssClass="btn btn-secondary" OnClientClick="activarloading();estilosElementosBloqueados();" OnClick="BTN_ReporteDevolucion_Click"></asp:Button>                                                                                
                                            <asp:Button UseSubmitBehavior="false" ID="BTN_DescargarDevolucion" runat="server" Text="Descargar devolución" CssClass="btn btn-primary" OnClientClick="estilosElementosBloqueados();" OnClick="BTN_DescargarDevolucion_Click"></asp:Button>                                        
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="BTN_DescargarDevolucion" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <div class="card-body">
                            <div class="card-body">
                                <asp:UpdatePanel ID="UpdatePanel_FiltrosProductos" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>                           
                                        <div class="input-group no-border col-md-6">
                                            <asp:TextBox class="form-control" ID="TXT_Buscar" runat="server" placeholder="Buscar..." OnTextChanged="TXT_Buscar_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                            <div class="input-group-append">
                                                <div class="input-group-text">
                                                    <i class="nc-icon nc-zoom-split"></i>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="input-group no-border col-md-3">     
                                            <asp:ListBox class="form-control" runat="server" ID="LB_PuntoVenta" SelectionMode="Multiple" OnTextChanged="TXT_Buscar_OnTextChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="table">
                                <asp:UpdatePanel ID="UpdatePanel_ListaProductosDevolucion" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <%--<asp:GridView ID="DGV_ListaPuntosVenta" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                            AutoGenerateColumns="False" DataKeyNames="DevolucionID,PuntoVentaID" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="Lbl_VerDetalle" runat="server" Text="Ver detalle"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="table" id="tableProductos">
                                                            <img alt="" style="cursor: pointer" src="../Assets/img/plus.png" />
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DescripcionPuntoVenta" SortExpression="DescripcionPuntoVenta" HeaderText="Punto venta" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                            </Columns>
                                        </asp:GridView>--%>
                                        <asp:GridView ID="DGV_ListaProductosDevolucion" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="False" DataKeyNames="IDDevolucionDetalle,DevolucionID,ProductoID,UsuarioID,PuntoVentaID" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_ListaProductosDevolucion_Sorting"
                                            OnRowCommand="DGV_ListaProductosDevolucion_RowCommand"
                                            OnRowDataBound="DGV_ListaProductosDevolucion_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="Nombre" SortExpression="Nombre" HeaderText="Usuario" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionProducto" SortExpression="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="CostoProducto" SortExpression="CostoProducto" HeaderText="Costo producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="PrecioProducto" SortExpression="PrecioProducto" HeaderText="Precio producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                                <asp:BoundField DataField="DescripcionPuntoVenta" SortExpression="DescripcionPuntoVenta" HeaderText="Punto venta" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Cantidad" runat="server" Text="Cantidad Devolucion"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="row">
                                                            <asp:TextBox class="form-control" TextMode="Number" MaxLength="2" min="0" max="999" style="width: 30%" runat="server" ID="TXT_Cantidad" 
                                                                onkeyup="TXT_Cantidad_onKeyUp(this,event);" onchange="TXT_Cantidad_onChange(this);" Text='<%#Eval("CantidadDevolucion") %>' />                                                            
                                                            <asp:DropDownList class="form-control" style="width: 20%" runat="server" ID="DDL_Centenas" 
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
                                                            <asp:DropDownList class="form-control" style="width: 25%" runat="server" ID="DDL_Decenas" 
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
                                                            <asp:DropDownList class="form-control" style="width: 25%" runat="server" ID="DDL_Unidades"
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
                                                        <asp:HiddenField ID="HDF_IDDevolucionDetalle" runat="server" Value='<%# Eval("IDDevolucionDetalle") %>' />
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
                <div class="modal-dialog modal-lg" style="padding-top: 5px;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server">Agregar productos a devolver</h5>
                        </div>
                        <div class="modal-body" style="padding-bottom: 0px;">                                                                                   
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="input-group no-border">
                                        <asp:TextBox class="form-control" ID="TXT_BuscarProductosSinAsignar" runat="server" placeholder="Buscar..." OnTextChanged="FiltrarProductos_OnClick" AutoPostBack="true"></asp:TextBox>
                                        <div class="input-group-append">
                                            <div class="input-group-text">
                                                <i class="nc-icon nc-zoom-split"></i>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-4">
                                    <asp:DropDownList class="form-control" ID="DDL_PuntoVenta" runat="server" OnSelectedIndexChanged="FiltrarProductos_OnClick" AutoPostBack="true"></asp:DropDownList>
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
                                                <asp:BoundField DataField="PrecioVentaFinal" SortExpression="PrecioVentaFinal" HeaderText="Precio unitario" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionCategoria" SortExpression="DescripcionCategoria" HeaderText="Categoria" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="LBL_Cantidad" runat="server" Text="Cantidad"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox class="form-control" TextMode="Number" MaxLength="0" min="0" max="99" style="width: 100%" runat="server" ID="TXT_CantidadAgregar" 
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
