

$(function () {

    $('#popuporderedit').dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        width: 500,
        heigth: 250,
        title: 'Edicion Orden Nro:',
        open: function (event, ui) {

            initialize();
//            loadOrder($(this).data('orderId'));

        },
        close: function (event, ui) {

            //limpia todos los textbox del popup
//            $('#popuporderedit :text').val('');

        },
        buttons: {
            Actualizar: function () {

//                updateOrder();

            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });



    $("#txtRequiredDate, #txtOrderDate, #txtShippedDate").datepicker({
        showOn: "button",
        buttonImage: "images/Calendar_schedule.png",
        changeYear: true,
        changeMonth: true,
        buttonImageOnly: true
    });


});


function RegisterEditOrderEvent() {

    $("[id*='gvOrders'] [id*='imgeditorder']").click(function () {

        var orderId = $(this).attr("idorder");

        $('#popuporderedit').dialog({ title: 'Edicion Orden Nro: ' + orderId });
        $('#popuporderedit').data('orderId', orderId);
        $('#popuporderedit').dialog('open');

    });

}


function initialize() {

    loadComboBox('ddlCustomer', 'GetAllCustomer');
    loadComboBox('ddlEmployeer', 'GetAllEmploye');
    loadComboBox('ddlShipVia', 'GetAllShipper');
    
}

function loadComboBox(comboname, webmethodname) {

    var template = "{#foreach $T as record}\
                        <option value='{$T.record.id}'>{$T.record.name}</option>\
                    {#/for}";

    var combo = $('#' + comboname);

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "Default.aspx/" + webmethodname,
        data: '{}',
        dataType: "json",
        async: true,
        success: function (data) {

            //combo.setTemplate($("#SelectTemplate").html());
            combo.setTemplate(template);
            combo.processTemplate(JSON.parse(data.d));
           
        },
        error: function (request, status, error) {
            alert(JSON.parse(request.responseText).Message);
        }
    });

}

function loadOrder(orderId) {

    var params = new Object();
    params.orderId = orderId;

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "Default.aspx/GetOrder",
        data: JSON.stringify(params),
        dataType: "json",
        async: true,
        success: function (data) {

            var order = JSON.parse(data.d);

            $('#ddlCustomer').val(order.CustomerID);
            $('#ddlEmployeer').val(order.EmployeeID);

            $('#txtOrderDate').val(order.OrderDate);
            $('#txtRequiredDate').val(order.RequiredDate);
            $('#txtShippedDate').val(order.ShippedDate);

            $('#ddlShipVia').val(order.ShipVia);
            $('#txtFreight').val(order.Freight);
            $('#txtShipName').val(order.ShipName);
            $('#txtShipAddress').val(order.ShipAddress);
            $('#txtShipCity').val(order.ShipCity);
            $('#txtShipRegion').val(order.ShipRegion);
            $('#txtShipPostalCode').val(order.ShipPostalCode);
            $('#txtShipCountry').val(order.ShipCountry);


        },
        error: function (request, status, error) {
            alert(JSON.parse(request.responseText).Message);
        }
    });


}

function updateOrder() {

    var order = new Object();
    order.OrderID = $('#popuporderedit').data('orderId');

    order.CustomerID = $('#ddlCustomer').val();
    order.EmployeeID = $('#ddlEmployeer').val();

    order.OrderDate = moment($('#txtOrderDate').val(), "DD/MM/YYYY").toDate();
    order.RequiredDate = moment($('#txtRequiredDate').val(), "DD/MM/YYYY").toDate();

    var shippedDate = moment($('#txtShippedDate').val(), "DD/MM/YYYY");
    order.ShippedDate = shippedDate == null ? null : shippedDate.toDate(); 

    order.ShipVia = $('#ddlShipVia').val();
    order.Freight = $('#txtFreight').val();
    order.ShipName = $('#txtShipName').val();
    order.ShipAddress = $('#txtShipAddress').val();
    order.ShipCity = $('#txtShipCity').val();
    order.ShipRegion = $('#txtShipRegion').val();
    order.ShipPostalCode = $('#txtShipPostalCode').val();
    order.ShipCountry = $('#txtShipCountry').val();

    var params = new Object();
    params.order = order;

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "Default.aspx/ActualizarOrder",
        data: JSON.stringify(params),
        dataType: "json",
        async: true,
        success: function (data) {

            alert('Actualizacion Correcta');
            $('#popuporderedit').dialog("close");

        },
        error: function (request, status, error) {
            alert(JSON.parse(request.responseText).Message);
        }
    });


}