<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ControlPedidos.Reportes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Reportes</title>
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

        function graficoPedidosSucursal(dias, datos) {
            Highcharts.chart('containerPedidosSucursal', {
                chart: {
                    type: 'line'
                },
                title: {
                    text: 'Pedidos por día'
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
                    name: 'Cantidad',
                    data: datos
                }]
            });
        }

        function graficoDevoluciones(datos) {
            lista = []
            Highcharts.chart('containerDevolucionesSucursal', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Devoluciones por punto venta'
                },
                tooltip: {
                    pointFormat: 'Porcentaje: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: false
                        },
                        showInLegend: true,
                        point: {
                            events: {
                                click: function (e) {                                    
                                    detalle('Devoluciones', e.point.name, '')
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
                    name: 'Sucursal',
                    colorByPoint: true,
                    data: datos
                }]
            });
        }

        function graficoDesechos(datos) {
            lista = []
            Highcharts.chart('containerDesechosSucursal', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Desechos por punto venta'
                },
                tooltip: {
                    pointFormat: 'Porcentaje: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: false
                        },
                        showInLegend: true,
                        point: {
                            events: {
                                click: function (e) {
                                    detalle('Desechos', e.point.name, '')
                                }
                            }
                        }
                    }
                },
                series: [{
                    name: 'Sucursal',
                    colorByPoint: true,
                    data: datos
                }]
            });
        }

        function gracficoEmpaque(dias, montos) {
            Highcharts.chart('containerEmpaque', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Cantidad empaque'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (e) {
                                    detalleEmpaqueInsumo('Empaque', this.category)
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
                xAxis: {
                    categories: dias
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: montos
            });
        }

        function gracficoInsumo(dias, montos) {
            Highcharts.chart('containerInsumo', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Cantidad insumo'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (e) {
                                    detalleEmpaqueInsumo('Insumo', this.category)
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
                xAxis: {
                    categories: dias
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: montos
            });
        }

        function graficoCantidadODP(dias, montos) {
            Highcharts.chart('containerCantidadOrdenesProduccion', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Cantidad orden producción'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (e) {
                                    detalleProduccionDiaria('ODP', this.category, e.point.series.name)
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
                xAxis: {
                    categories: dias
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: montos
            });
        }

        function graficoMontosODP(dias, montos) {
            Highcharts.chart('containerMontoOrdenesProduccion', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Montos orden producción'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (e) {
                                    detalleProduccionDiaria('ODP', this.category, e.point.series.name)
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
                xAxis: {
                    categories: dias
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: montos
            });
        }

        function graficoMontoPedidos(puntosVenta, montos) {
            Highcharts.chart('containerMontoPedido', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Montos pedido'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (e) {
                                    detalleMontos('Pedidos', this.category, e.point.series.name)
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
                xAxis: {
                    categories: puntosVenta
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: montos
            });
        }

        function graficoMontoDespacho(puntosVenta, montos) {
            Highcharts.chart('containerMontoDespacho', {                
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Montos despacho'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (e) {
                                    detalleMontos('Despachos', this.category, e.point.series.name)
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
                xAxis: {
                    categories: puntosVenta
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: montos
            });
        }

        function graficoMontoPedidoRecibido(puntosVenta, montos) {
            Highcharts.chart('containerMontoPedidoRecibido', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Montos pedidos recibidos'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (e) {
                                    detalleMontos('Pedidos recibidos', this.category, e.point.series.name)
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
                xAxis: {
                    categories: puntosVenta
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: montos
            });
        }

        function graficoMontoPedidoSemana(semanas, montos) {
            Highcharts.chart('containerMontoPedidoSemana', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Montos pedido'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (e) {
                                    detalleSemana('Pedidos', this.category, e.point.series.name)
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
                xAxis: {
                    categories: semanas
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: montos
            });
        }

        function graficoCantidadPedidoSemana(semanas, montos) {
            Highcharts.chart('containerCantidadPedidoSemana', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Cantidad pedido'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (e) {
                                    detalleSemana('Pedidos', this.category, e.point.series.name)
                                }
                            }
                        }
                    }
                },
                xAxis: {
                    categories: semanas
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: montos
            });
        }

        function graficoMontoDespachoSemana(semanas, montos) {
            Highcharts.chart('containerMontoDespachoSemana', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Montos despacho'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (e) {
                                    detalleSemana('Despaho', this.category, e.point.series.name)
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
                xAxis: {
                    categories: semanas
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: montos
            });
        }

        function graficoCantidadDespachoSemana(semanas, montos) {
            Highcharts.chart('containerCantidadDespachoSemana', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Cantidad despacho'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (e) {
                                    detalleSemana('Despacho', this.category, e.point.series.name)
                                }
                            }
                        }
                    }
                },
                xAxis: {
                    categories: semanas
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: montos
            });
        }

        function graficoMontoRecibidoSemana(semanas, montos) {
            Highcharts.chart('containerMontoRecibidoSemana', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Montos recibido'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (e) {
                                    detalleSemana('Recibido', this.category, e.point.series.name)
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
                xAxis: {
                    categories: semanas
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: montos
            });
        }

        function graficoCantidadRecibidoSemana(semanas, montos) {
            Highcharts.chart('containerCantidadRecibidoSemana', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Cantidad recibido'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (e) {
                                    detalleSemana('Recibido', this.category, e.point.series.name)
                                }
                            }
                        }
                    }
                },
                xAxis: {
                    categories: semanas
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: montos
            });
        }

        function cargarGraficos(idUsuario) {
            var fechaDesde = document.getElementById('<%= TXT_FechaDesde.ClientID %>').value
            var fechaHasta = document.getElementById('<%= TXT_FechaHasta.ClientID %>').value + ' 23:59:59'

            var puntosVenta = document.getElementById('<%= LB_Sucursal.ClientID %>')
            var plantasProduccion = document.getElementById('<%= LB_PlantaProduccion.ClientID %>')
            var idsPuntosVenta = ''
            var idsPlantasProduccion = ''

            for (var i = 0; i < puntosVenta.length; i++) {
                if (puntosVenta[i].selected) {
                    idsPuntosVenta += puntosVenta[i].value + ','
                }
            }

            for (var i = 0; i < plantasProduccion.length; i++) {
                if (plantasProduccion[i].selected) {
                    idsPlantasProduccion += plantasProduccion[i].value + ','
                }
            }

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Reportes.aspx/cargarGraficoPedidos",
                data: JSON.stringify({
                    "idUsuario": idUsuario,
                    "fechaDesde": fechaDesde,
                    "fechaHasta": fechaHasta,
                    "idsPuntoVenta": idsPuntosVenta,
                    "idsPlantasProduccion": idsPlantasProduccion,
                }),
                dataType: "json",
                success: function (Result) {
                    listaDias = []
                    listaCantidadPedidos = []
                    for (var i in Result.d) {
                        listaDias.unshift(Result.d[i].dia)
                        listaCantidadPedidos.unshift(Result.d[i].cantidadPedidos)
                    }
                    graficoPedidosSucursal(listaDias, listaCantidadPedidos)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Reportes.aspx/cargarGraficoDevoluciones",
                data: JSON.stringify({
                    "idUsuario": idUsuario,
                    "fechaDesde": fechaDesde,
                    "fechaHasta": fechaHasta,
                    "idsPuntoVenta": idsPuntosVenta,
                }),
                dataType: "json",
                success: function (Result) {
                    listaDevoluciones = []
                    for (var i in Result.d) {
                        listaDevoluciones.unshift(
                            {
                                'name': Result.d[i].puntoVenta,
                                'y': Result.d[i].cantidadDevolucion
                            }
                        )
                    }
                    graficoDevoluciones(listaDevoluciones)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Reportes.aspx/cargarGraficoDesechos",
                data: JSON.stringify({
                    "idUsuario": idUsuario,
                    "fechaDesde": fechaDesde,
                    "fechaHasta": fechaHasta,
                    "idsPuntoVenta": idsPuntosVenta,
                }),
                dataType: "json",
                success: function (Result) {
                    listaDesechos = []
                    for (var i in Result.d) {
                        listaDesechos.unshift(
                            {
                                'name': Result.d[i].puntoVenta,
                                'y': Result.d[i].cantidadDesecho
                            }
                        )
                    }
                    graficoDesechos(listaDesechos)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Reportes.aspx/cargarGraficoEmpaque",
                data: JSON.stringify({
                    "idUsuario": idUsuario,
                    "fechaDesde": fechaDesde,
                    "fechaHasta": fechaHasta,
                }),
                dataType: "json",
                success: function (Result) {                                     
                    var listaDias = []
                    var detalle = ['Cantidad Empaque']
                    var datos = []
                    for (var i in Result.d) {
                        if (!listaDias.includes(Result.d[i].dia)) {
                            listaDias.push(Result.d[i].dia)
                        }
                    }

                    var values = []                        
                    for (var dia in listaDias) {
                        var index = 0
                        var index = (dia * 1)

                        values.push(Result.d[index].cantidadEmpaque)
                    }
                    var array = { name: 'Cantidad empaque', data: values };
                    datos.push(array)
                    
                    gracficoEmpaque(listaDias, datos)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Reportes.aspx/cargarGraficoInsumo",
                data: JSON.stringify({
                    "idUsuario": idUsuario,
                    "fechaDesde": fechaDesde,
                    "fechaHasta": fechaHasta,
                }),
                dataType: "json",
                success: function (Result) {
                    var listaDias = []
                    var detalle = ['Cantidad Insumo']
                    var datos = []
                    for (var i in Result.d) {
                        if (!listaDias.includes(Result.d[i].dia)) {
                            listaDias.push(Result.d[i].dia)
                        }
                    }

                    var values = []
                    for (var dia in listaDias) {
                        var index = 0
                        var index = (dia * 1)

                        values.push(Result.d[index].cantidadInsumo)
                    }
                    var array = { name: 'Cantidad insumo', data: values };
                    datos.push(array)

                    gracficoInsumo(listaDias, datos)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Reportes.aspx/cargarGraficoOrdenProduccion",
                data: JSON.stringify({
                    "idUsuario": idUsuario,
                    "fechaDesde": fechaDesde,
                    "fechaHasta": fechaHasta,
                    "idsPuntoVenta": idsPuntosVenta,
                    "idsPlantasProduccion": idsPlantasProduccion,
                }),
                dataType: "json",
                success: function (Result) {
                    var listaDias = []
                    var plantasProduccion = []
                    var datos = []
                    var datosMontos = []
                    for (var i in Result.d) {
                        if (!listaDias.includes(Result.d[i].dia)) {
                            listaDias.push(Result.d[i].dia)
                        }
                        if (!plantasProduccion.includes(Result.d[i].plantaProduccion)) {
                            plantasProduccion.push(Result.d[i].plantaProduccion)
                        }
                    }

                    for (planta in plantasProduccion) {
                        var values = []
                        var valuesMontos = []
                        for (var dia in listaDias) {
                            var index = 0
                            var index = (planta * 1) * plantasProduccion.length + (dia * 1)

                            values.push(Result.d[index].cantidadPedidos)
                            valuesMontos.push(Result.d[index].montoPedidos)
                        }
                        var array = { name: plantasProduccion[planta], data: values };
                        var arrayMontos = { name: plantasProduccion[planta], data: valuesMontos };
                        datos.push(array)
                        datosMontos.push(arrayMontos)
                    }
                    graficoCantidadODP(listaDias, datos)
                    graficoMontosODP(listaDias, datosMontos)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Reportes.aspx/cargarGraficoPedidosPuntoVenta",
                data: JSON.stringify({
                    "idUsuario": idUsuario,
                    "fechaDesde": fechaDesde,
                    "fechaHasta": fechaHasta,
                    "idsPuntoVenta": idsPuntosVenta,
                    "idsPlantasProduccion": idsPlantasProduccion,
                }),
                dataType: "json",
                success: function (Result) {
                    var puntosVenta = []
                    var plantasProduccion = []
                    var datos = []
                    for (var i in Result.d) {
                        if (!puntosVenta.includes(Result.d[i].puntoVenta)) {
                            puntosVenta.push(Result.d[i].puntoVenta)
                        }
                        if (!plantasProduccion.includes(Result.d[i].plantaProduccion)) {
                            plantasProduccion.push(Result.d[i].plantaProduccion)
                        }
                    }                
                    for (planta in plantasProduccion) {
                        var values = []                        
                        for (var punto in puntosVenta) {
                            var index = 0
                            if (planta * 1 === 0) {
                                index = (punto * 1) * ((planta * 1) + 1) * plantasProduccion.length
                            } else {
                                index = (punto * 1) * ((planta * 1) + 1) + plantasProduccion.length - 1
                            }
                            
                            values.push(Result.d[index].montoPedidos)                          
                        }
                        var array = { name: plantasProduccion[planta], data: values };
                        datos.push(array)                        
                    }
                    graficoMontoPedidos(puntosVenta, datos)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Reportes.aspx/cargarGraficoDespachosPuntoVenta",
                data: JSON.stringify({
                    "idUsuario": idUsuario,
                    "fechaDesde": fechaDesde,
                    "fechaHasta": fechaHasta,
                    "idsPuntoVenta": idsPuntosVenta,
                    "idsPlantasProduccion": idsPlantasProduccion,
                }),
                dataType: "json",
                success: function (Result) {
                    var puntosVenta = []
                    var plantasProduccion = []
                    var datos = []
                    for (var i in Result.d) {
                        if (!puntosVenta.includes(Result.d[i].puntoVenta)) {
                            puntosVenta.push(Result.d[i].puntoVenta)
                        }
                        if (!plantasProduccion.includes(Result.d[i].plantaProduccion)) {
                            plantasProduccion.push(Result.d[i].plantaProduccion)
                        }
                    }
                    for (planta in plantasProduccion) {
                        var values = []
                        for (var punto in puntosVenta) {
                            var index = 0
                            if (planta * 1 === 0) {
                                index = (punto * 1) * ((planta * 1) + 1) * plantasProduccion.length
                            } else {
                                index = (punto * 1) * ((planta * 1) + 1) + plantasProduccion.length - 1
                            }

                            values.push(Result.d[index].montoDespacho)
                        }
                        var array = { name: plantasProduccion[planta], data: values };
                        datos.push(array)
                    }
                    graficoMontoDespacho(puntosVenta, datos)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Reportes.aspx/cargarGraficoRecibidoPuntoVenta",
                data: JSON.stringify({
                    "idUsuario": idUsuario,
                    "fechaDesde": fechaDesde,
                    "fechaHasta": fechaHasta,
                    "idsPuntoVenta": idsPuntosVenta,
                    "idsPlantasProduccion": idsPlantasProduccion,
                }),
                dataType: "json",
                success: function (Result) {
                    var puntosVenta = []
                    var plantasProduccion = []
                    var datos = []
                    for (var i in Result.d) {
                        if (!puntosVenta.includes(Result.d[i].puntoVenta)) {
                            puntosVenta.push(Result.d[i].puntoVenta)
                        }
                        if (!plantasProduccion.includes(Result.d[i].plantaProduccion)) {
                            plantasProduccion.push(Result.d[i].plantaProduccion)
                        }
                    }
                    for (planta in plantasProduccion) {
                        var values = []
                        for (var punto in puntosVenta) {
                            var index = 0
                            if (planta * 1 === 0) {
                                index = (punto * 1) * ((planta * 1) + 1) * plantasProduccion.length
                            } else {
                                index = (punto * 1) * ((planta * 1) + 1) + plantasProduccion.length - 1
                            }

                            values.push(Result.d[index].montoDespacho)
                        }
                        var array = { name: plantasProduccion[planta], data: values };
                        datos.push(array)
                    }
                    graficoMontoPedidoRecibido(puntosVenta, datos)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Reportes.aspx/cargarGraficoPedidosSemana",
                data: JSON.stringify({
                    "idUsuario": idUsuario,
                    "fechaDesde": fechaDesde,
                    "fechaHasta": fechaHasta,
                    "idsPuntoVenta": idsPuntosVenta,
                    "idsPlantasProduccion": idsPlantasProduccion,
                }),
                dataType: "json",
                success: function (Result) {
                    var dias = []
                    var semanas = []
                    var datosPedidosCantidad = []
                    var datosPedidosMonto = []
                    var datosDespachoCantidad = []
                    var datosDespachoMonto = []
                    var datosRecibidoCantidad = []
                    var datosRecibidoMonto = []

                    for (var i in Result.d) {
                        if (!dias.includes(Result.d[i].dia)) {
                            dias.push(Result.d[i].dia)
                        }
                        if (!semanas.includes(Result.d[i].semana)) {
                            semanas.push(Result.d[i].semana)
                        }
                    }

                    //for (dia in dias) {
                    //    var values = []
                    //    for (var semana in semanas) {
                    //        var index = 0
                    //        if (dia * 1 === 0) {
                    //            index = (semana * 1) * ((dia * 1) + 1) * dias.length
                    //        } else {
                    //            index = (semana * 1) * ((dia * 1) + 1) + dias.length - 1
                    //        }
                    //        values.push(Result.d[index].montoPedidos)
                    //    }
                    //    var array = { name: dias[dia], data: values };
                    //    datos.push(array)
                    //}

                    //graficoMontoPedidoSemana(semanas, datos)
                    for (semana in semanas) {
                        var valuesPedidosCantidad = []
                        var valuesPedidosMonto = []
                        var valuesDespachoCantidad = []
                        var valuesDespachoMonto = []
                        var valuesRecibidosCantidad = []
                        var valuesRecibidosMonto = []
                        for (var dia in dias) {
                            var index = (semana * 1) * 7 + (dia * 1)
                            
                            valuesPedidosCantidad.push(Result.d[index] === undefined ? 0 : Result.d[index].cantidadPedidos)
                            valuesPedidosMonto.push(Result.d[index] === undefined ? 0 : Result.d[index].montoPedidos)
                            valuesDespachoCantidad.push(Result.d[index] === undefined ? 0 : Result.d[index].cantidadDespacho)
                            valuesDespachoMonto.push(Result.d[index] === undefined ? 0 : Result.d[index].montoDespacho)
                            valuesRecibidosCantidad.push(Result.d[index] === undefined ? 0 : Result.d[index].cantidadRecibido)
                            valuesRecibidosMonto.push(Result.d[index] === undefined ? 0 : Result.d[index].montoRecibido)
                        }
                        var arrayPedidosCantidad = { name: semanas[semana], data: valuesPedidosCantidad };
                        var arrayPedidosMonto = { name: semanas[semana], data: valuesPedidosMonto };
                        var arrayDespachoCantidad = { name: semanas[semana], data: valuesDespachoCantidad };
                        var arrayDespachoMonto = { name: semanas[semana], data: valuesDespachoMonto };
                        var arrayRecibidosCantidad = { name: semanas[semana], data: valuesRecibidosCantidad };
                        var arrayRecibidosMonto = { name: semanas[semana], data: valuesRecibidosMonto };
                        datosPedidosCantidad.push(arrayPedidosCantidad)
                        datosPedidosMonto.push(arrayPedidosMonto)
                        datosDespachoCantidad.push(arrayDespachoCantidad)
                        datosDespachoMonto.push(arrayDespachoMonto)
                        datosRecibidoCantidad.push(arrayRecibidosCantidad)
                        datosRecibidoMonto.push(arrayRecibidosMonto)
                    }

                    graficoCantidadPedidoSemana(dias, datosPedidosCantidad)
                    graficoCantidadDespachoSemana(dias, datosDespachoCantidad)
                    graficoCantidadRecibidoSemana(dias, datosRecibidoCantidad)
                    graficoMontoPedidoSemana(dias, datosPedidosMonto)
                    graficoMontoDespachoSemana(dias, datosDespachoMonto)
                    graficoMontoRecibidoSemana(dias, datosRecibidoMonto)
                },
                error: function (Result) {
                    alert("ERROR " + Result.status + ' ' + Result.statusText);
                }
            })            
        }

        function detalle(modulo, puntoVenta, plantaProduccion) {
            $(<%= HDF_Detalle.ClientID %>)[0].value = modulo
            $(<%= HDF_PuntoVenta.ClientID %>)[0].value = puntoVenta
            $(<%= HDF_PlantaProduccion.ClientID %>)[0].value = plantaProduccion
            __doPostBack('detalleCantidad')
        }

        function detalleSemana(modulo, dia, semana) {
            $(<%= HDF_Detalle.ClientID %>)[0].value = modulo
            $(<%= HDF_Dia.ClientID %>)[0].value = dia
            $(<%= HDF_Semana.ClientID %>)[0].value = semana
            __doPostBack('detalleSemanal')
        }

        function detalleMontos(modulo, puntoVenta, plantaProduccion) {
            $(<%= HDF_Detalle.ClientID %>)[0].value = modulo
            $(<%= HDF_PuntoVenta.ClientID %>)[0].value = puntoVenta
            $(<%= HDF_PlantaProduccion.ClientID %>)[0].value = plantaProduccion
            __doPostBack('detalleCantidad')
        }

        function detalleEmpaqueInsumo(modulo, dia) {           
            $(<%= HDF_Detalle.ClientID %>)[0].value = modulo
            $(<%= HDF_Dia.ClientID %>)[0].value = dia
            __doPostBack('detalleEmpaqueInsumo')
        }

        function detalleProduccionDiaria(modulo, puntoVenta, plantaProduccion) {
            console.log(modulo, puntoVenta, plantaProduccion)
        }

        function abrirModalDetalleCantidad() {
            document.getElementById('BTN_ModalDetalleCantidad').click()
        }

        function cerrarModalDetalleCantidad() {
            document.getElementById('BTN_ModalDetalleCantidad').click()
        }

        function abrirModalDetalle() {
            document.getElementById('BTN_ModalDetalle').click()
        }

        function cerrarModalDetalle() {
            document.getElementById('BTN_ModalDetalle').click()
        }

        function abrirModalDetalleCantidadPedido() {
            document.getElementById('BTN_ModalDetalleCantidadPedido').click()
        }

        function cerrarModalDetalleCantidadPedido() {
            document.getElementById('BTN_ModalDetalleCantidadPedido').click()
        }

        function abrirModalDetalleSemanal() {
            document.getElementById('BTN_ModalDetalleSemanal').click()
        }

        function cerrarModalDetalleSemanal() {
            document.getElementById('BTN_ModalDetalleSemanal').click()
        }

        function cargarFiltros() {
            $(<%= LB_Sucursal.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Sucursal' })
            $(<%= LB_PlantaProduccion.ClientID %>).SumoSelect({ selectAll: true, placeholder: 'Planta de Producción' })
        }

        function seleccionarReceptor(receptor) {
            if (receptor === "MiKFe") {
                __doPostBack('Identificacion;3101485961')
            } else if (receptor === "Esteban") {
                __doPostBack('Identificacion;115210651')
            }
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
        <div class="sidebar" data-color="white" data-active-color="danger">
            <div class="sidebar-wrapper scroll" style="overflow-y: auto;">
                <img style="width: 60%; display: block; margin-left: 30%; margin-top: 3%;" src="../Assets/img/logo.png" />
                <ul class="nav">
                    <li class="active">
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
                    <li>
                        <a href="Insumos.aspx">
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
                    <li>
                        <a href="../GestionCostos/CrearReceta.aspx">
                            <i class="fas fa-chart-line"></i>
                            <p>Gestión costos</p>
                        </a>
                    </li>
                </ul>
                <hr style="width: 230px; color: #2c2c2c;" />
                <%--<h5 style="text-align: center;">Mantenimiento</h5>--%>
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
                    </li>
                </ul>
            </div>
        </div>
        <div class="main-panel scroll" style="background: #f4f3ef;">
            <div class="content">
                <div class="container-fluid">
                    <!-- Page Heading -->
                    <h1 class="h3 mb-2 text-gray-800">Reportes</h1>
                    <div class="card shadow mb-4">
                        <asp:UpdatePanel ID="UpdatePanel_divEncabezados" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="HDF_Detalle" runat="server" Value="" Visible="true" />
                                <asp:HiddenField ID="HDF_Dia" runat="server" Value="" Visible="true" />
                                <asp:HiddenField ID="HDF_Semana" runat="server" Value="" Visible="true" />
                                <asp:HiddenField ID="HDF_PuntoVenta" runat="server" Value="" Visible="true" />
                                <asp:HiddenField ID="HDF_PlantaProduccion" runat="server" Value="" Visible="true" />
                                <div class="row" style="margin-top: 1rem; margin-bottom: 1rem;">
                                    <div class="col-xl-12 col-md-12 mb-4">                                                                                
                                        <div class="col-md-2">                                                                                               
                                            <label style="margin-top: 1%;">Fecha desde:</label> 
                                            <asp:TextBox class="form-control" style="flex: auto;" ID="TXT_FechaDesde" runat="server" TextMode="Date" OnTextChanged="Recargar_Click" AutoPostBack="true"></asp:TextBox>                                                
                                        </div>                                                                                                                    
                                        <div class="col-md-2">            
                                            <label style="margin-top: 1%;">Fecha hasta:</label>
                                            <asp:TextBox class="form-control" style="flex: auto;" ID="TXT_FechaHasta" runat="server" TextMode="Date" OnTextChanged="Recargar_Click" AutoPostBack="true"></asp:TextBox>                                                
                                        </div>
                                        <div class="col-md-2">
                                            <br />
                                            <asp:ListBox class="form-control" runat="server" ID="LB_Sucursal" SelectionMode="Multiple" OnSelectedIndexChanged="Recargar_Click" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                        <div class="col-md-2">
                                            <br />
                                            <asp:ListBox class="form-control" runat="server" ID="LB_PlantaProduccion" SelectionMode="Multiple" OnSelectedIndexChanged="Recargar_Click" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                        <div class="col-md-2" style="text-align: right;">  
                                            <asp:Button ID="BTN_ReporteExcel" runat="server" UseSubmitBehavior="false" Text="Reporte Excel" CssClass="btn btn-secondary" OnClientClick="cargarFiltros();" OnClick="BTN_ReporteExcel_Click"></asp:Button>
                                        </div>
                                        <div class="col-md-2">         
                                            <asp:Button ID="BTN_ReporteSemanal" runat="server" UseSubmitBehavior="false" Text="Reporte Semanal" CssClass="btn btn-secondary" OnClientClick="cargarFiltros();" OnClick="BTN_ReporteExcelSemanal_Click"></asp:Button>                                            
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xl-2 col-md-6 mb-4" style="margin-left: 2rem; padding-right: 1rem;">
                                        <div class="card border-left-primary shadow h-100 py-2" onclick="detalle('Pedidos', '', '');" style="cursor: pointer;">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                                            Pedidos
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold text-gray-800" style="text-align: center;" runat="server" id="div_CantidadPedidos"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-2 col-md-6 mb-4" style="padding-left: 0px; padding-right: 0.5rem;">
                                        <div class="card border-left-primary shadow h-100 py-2" onclick="detalle('Orden produccion', '', '');" style="cursor: pointer;">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                                            Producción
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold text-gray-800" style="text-align: center;" runat="server" id="div_CantidadOrdenProduccion"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-2 col-md-6 mb-4" style="padding-left: 0px; padding-right: 0.5rem;">
                                        <div class="card border-left-success shadow h-100 py-2" onclick="detalle('Despachos', '', '');" style="cursor: pointer;">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                                            Despachos
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold text-gray-800" style="text-align: center;" runat="server" id="div_CantidadDespachos"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-2 col-md-6 mb-4" style="padding-left: 0px; padding-right: 0.5rem;">
                                        <div class="card border-left-success shadow h-100 py-2" onclick="detalle('Pedidos recibidos', '', '');" style="cursor: pointer;">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                                            Pedidos recibidos
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold text-gray-800" style="text-align: center;" runat="server" id="div_CantidadPedidosRecibidos">11,887</div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-2 col-md-6 mb-4" style="padding-left: 0px; padding-right: 0.5rem;">
                                        <div class="card border-left-success shadow h-100 py-2" onclick="detalle('Devoluciones', '', '');" style="cursor: pointer;">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                                            Devoluciones
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold text-gray-800" style="text-align: center;" runat="server" id="div_CantidadDevoluciones"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-1 col-md-6 mb-4" style="padding-left: 0px; padding-right: 0.5rem; flex: 0 0 13%; max-width: 13%;">
                                        <div class="card border-left-warning shadow h-100 py-2" onclick="detalle('Desechos', '', '');" style="cursor: pointer;">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                                            Desecho
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold text-gray-800" style="text-align: center;" runat="server" id="div_CantidadDesecho"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="BTN_ReporteExcel" />
                                <asp:PostBackTrigger ControlID="BTN_ReporteSemanal" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <div class="row" id="CardPedidos">
                            <div class="col-md-6">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerPedidosSucursal"></div>
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerDevolucionesSucursal"></div>
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerDesechosSucursal"></div>
                                    </figure>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="CardOrdenesProduccion">
                            <div class="col-md-6">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerCantidadOrdenesProduccion"></div>
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerMontoOrdenesProduccion"></div>
                                    </figure>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="CardMontos">
                            <div class="col-md-4">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerMontoPedido"></div>                                       
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerMontoDespacho"></div>
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerMontoPedidoRecibido"></div>
                                    </figure>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="CardSemanalCantidad">
                            <div class="col-md-4">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerCantidadPedidoSemana"></div>                                       
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerCantidadDespachoSemana"></div>                                       
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerCantidadRecibidoSemana"></div>                                       
                                    </figure>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="CardSemanal">
                            <div class="col-md-4">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerMontoPedidoSemana"></div>                                       
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerMontoDespachoSemana"></div>                                       
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerMontoRecibidoSemana"></div>                                       
                                    </figure>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="CardEmpaqueInsumos">
                            <div class="col-md-6">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerEmpaque"></div>                                       
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="card card-chart">
                                    <figure class="highcharts-figure">
                                        <div id="containerInsumo"></div>                                       
                                    </figure>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <button type="button" id="BTN_ModalDetalleCantidad" data-toggle="modal" data-target="#ModalDetalleCantidad" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalDetalleCantidad" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="popDetalleCantidad" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalDetalleCantidad" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server" id="modalCantidadTitle"></h5>
                            <p runat="server" id="plantaProduccionCantidad"></p>
                            <p runat="server" id="puntoVentaCantidad"></p>
                            <p runat="server" id="fechaDesdeCantidad"></p>
                            <p runat="server" id="fechaHastaCantidad"></p>
                        </div>
                        <div class="modal-body">                            
                            <div class="table-responsive">
                                <asp:UpdatePanel ID="UpdatePanel_DetalleCantidad" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>                                        
                                        <asp:GridView ID="DGV_DetalleCantidad" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="false" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_DetalleCantidad_Sorting">
                                            <Columns>                                         
                                                <asp:BoundField DataField="DescripcionCategoria" SortExpression="DescripcionCategoria" HeaderText="Categoría" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionProducto" SortExpression="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="Cantidad" SortExpression="Cantidad" HeaderText="Cantidad" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                                <asp:BoundField DataField="Monto" SortExpression="Monto" HeaderText="Monto" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="row">
                                <div class="col-md-12" style="text-align: right;">
                                    <asp:Button ID="BTN_ImprimirReporte" runat="server" UseSubmitBehavior="false" Text="Imprimir reporte" CssClass="btn btn-info" OnClientClick="activarloading();" OnClick="BTN_ImprimirReporte_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
    <button type="button" id="BTN_ModalDetalleCantidadPedido" data-toggle="modal" data-target="#ModalDetalleCantidadPedido" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalDetalleCantidadPedido" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="popDetalleCantidadPedido" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalDetalleCantidadPedido" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" style="max-width: 1200px;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server" id="modalCantidadPedidoTitle"></h5>
                            <p runat="server" id="plantaProduccionCantidadPedido"></p>
                            <p runat="server" id="puntoVentaCantidadPedido"></p>
                            <p runat="server" id="fechaDesdeCantidadPedido"></p>
                            <p runat="server" id="fechaHastaCantidadPedido"></p>
                        </div>
                        <div class="modal-body">                            
                            <div class="table-responsive">
                                <asp:UpdatePanel ID="UpdatePanel_DetalleCantidadPedido" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>                                        
                                        <asp:GridView ID="DGV_DetalleCantidadPedido" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="false" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_DetalleCantidad_Sorting">
                                            <Columns>                                         
                                                <asp:BoundField DataField="DescripcionCategoria" SortExpression="DescripcionCategoria" HeaderText="Categoría" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionProducto" SortExpression="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="CantidadPedido" SortExpression="CantidadPedido" HeaderText="Cantidad solicitada" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="MontoPedido" SortExpression="MontoPedido" HeaderText="Monto solicitado" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="CantidadDespachada" SortExpression="CantidadDespachada" HeaderText="Cantidad despachada" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="MontoDespacho" SortExpression="MontoDespacho" HeaderText="Monto despachado" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="CantidadRecibida" SortExpression="CantidadRecibida" HeaderText="Cantidad recibida" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="MontoPedidoRecibido" SortExpression="MontoPedidoRecibido" HeaderText="Monto recibido" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="row">
                                <div class="col-md-12" style="text-align: right;">
                                    <asp:Button ID="BTN_ImprimirReportePedido" runat="server" UseSubmitBehavior="false" Text="Imprimir reporte" CssClass="btn btn-info" OnClientClick="activarloading();" OnClick="BTN_ImprimirReportePedido_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <button type="button" id="BTN_ModalDetalleSemanal" data-toggle="modal" data-target="#ModalDetalleSemanal" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalDetalleSemanal" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="popDetalleCantidadPedido" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalDetalleSemana" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" style="max-width: 1200px;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server" id="modalDetalleSemana"></h5>
                            <p runat="server" id="plantaProduccionSemana"></p>
                            <p runat="server" id="puntoVentaSemana"></p>
                            <p runat="server" id="fechaDesdeSemana"></p>
                            <p runat="server" id="fechaHastaSemana"></p>
                        </div>
                        <div class="modal-body">                            
                            <div class="table-responsive">
                                <asp:UpdatePanel ID="UpdatePanel_DetalleSemanal" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>                                        
                                        <asp:GridView ID="DGV_DetalleSemanal" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="false" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            OnSorting="DGV_DetalleSemanal_Sorting">
                                            <Columns>                                         
                                                <asp:BoundField DataField="Dia" SortExpression="Dia" HeaderText="Dia" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                <asp:BoundField DataField="Semana" SortExpression="Semana" HeaderText="Semana" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="170px"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionCategoria" SortExpression="DescripcionCategoria" HeaderText="Categoría" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionProducto" SortExpression="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="CantidadSolicitada" SortExpression="CantidadSolicitada" HeaderText="Cantidad solicitada" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="MontoPedido" SortExpression="MontoPedido" HeaderText="Monto solicitado" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="CantidadDespachada" SortExpression="CantidadDespachada" HeaderText="Cantidad despachada" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="MontoDespacho" SortExpression="MontoDespacho" HeaderText="Monto despachado" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="CantidadRecibida" SortExpression="CantidadRecibida" HeaderText="Cantidad recibida" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="MontoPedidoRecibido" SortExpression="MontoPedidoRecibido" HeaderText="Monto recibido" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <button type="button" id="BTN_ModalDetallePedido" data-toggle="modal" data-target="#ModalDetallePedido" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalDetallePedido" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="popDetallePedido" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalDetallePedido" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server">Detalle pedidos</h5>
                        </div>
                        <div class="modal-body">                         
                            <div class="table-responsive" id="tableCategorias">
                                <asp:UpdatePanel ID="UpdatePanel_DetallePedido" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>     
                                        <asp:GridView ID="DGV_Pedidos" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="false" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros.">
                                            <Columns>           
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="Lbl_VerDetalle" runat="server" Text="Ver detalle"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="table" id="tableProductos">
                                                            <img alt="" style="cursor: pointer" src="../Assets/img/plus.png" />
                                                            <asp:Panel ID="pnlListaProductos" runat="server" Style="display: none;">
                                                                <asp:GridView ID="DGV_ListaProductos" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid"
                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" 
                                                                    GridLines="None" ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="DescripcionCategoria" HeaderText="Categoría" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                             
                                                                        <asp:BoundField DataField="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                             
                                                                        <asp:BoundField DataField="CantidadDespachada" HeaderText="Cantidad despachada" DataFormatString="{0:n0}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                        <asp:BoundField DataField="PrecioProducto" HeaderText="Precio unitario" DataFormatString="{0:n0}" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>                               
                                                <asp:BoundField DataField="NumeroPedido" HeaderText="Número pedido" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="CantidadDespachada" HeaderText="Cantidad" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                               
                                                <asp:BoundField DataField="MontoDespacho" HeaderText="Monto" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <button type="button" id="BTN_ModalDetalle" data-toggle="modal" data-target="#ModalDetalle" style="visibility: hidden;">open</button>

    <div class="modal bd-example-modal-lg" id="ModalDetalle" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="popDetalle" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_ModalDetalle" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h5 class="modal-title" runat="server" id="modalCantidadDetalleTitle"></h5>
                            <p runat="server" id="Dia"></p>
                        </div>
                        <div class="modal-body">                            
                            <div class="table-responsive">
                                <asp:UpdatePanel ID="UpdatePanel_Detalle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>                                        
                                        <asp:GridView ID="DGV_Detalle" Width="100%" runat="server" CssClass="table" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            AutoGenerateColumns="false" HeaderStyle-CssClass="table" BorderWidth="0px" HeaderStyle-BorderColor="#51cbce" GridLines="None"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay registros." AllowSorting="true"
                                            >
                                            <Columns>                                         
                                                <asp:BoundField DataField="Fecha" SortExpression="Fecha" HeaderText="Día" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionCategoria" SortExpression="DescripcionCategoria" HeaderText="Categoría" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                <asp:BoundField DataField="DescripcionProducto" SortExpression="DescripcionProducto" HeaderText="Nombre producto" ItemStyle-ForeColor="black" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="Cantidad" SortExpression="Cantidad" HeaderText="Cantidad" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                                <asp:BoundField DataField="Monto" SortExpression="Monto" HeaderText="Monto" ItemStyle-ForeColor="black" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                                                
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="row">
                                <div class="col-md-12" style="text-align: right;">
                                    <asp:Button ID="BTN_ImprimirDetalle" runat="server" UseSubmitBehavior="false" Text="Imprimir reporte" CssClass="btn btn-info" OnClientClick="activarloading();" OnClick="BTN_ImprimirReporteEmpaqueInsumo_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
