jQuery.fn.simpleDialog = function (width, height, title) {
    $(this).okcancelSmartDialog(width, height, title);
    getDialogButton($(this), "Ok").hide();
    getDialogButton($(this), "Cancel").hide();
};
jQuery.fn.okDialog = function (width, height, title) {
    $(this).okcancelSmartDialog(width, height, title);
    getDialogButton($(this), "Cancel").hide();
};
jQuery.fn.okcancelDialog = function (width, height, title) {
    $(this).okcancelSmartDialog(width, height, title);
};

jQuery.fn.okcancelSmartDialog = function (width, height, title) {
    $(this).dialog({
        autoOpen: false,
        width: width,
        height: height,
        title: title,
        modal: true,
        resizable: false,
        buttons: {
            Ok: function () {
                var formId = $(this).data("formId");
                if (formId) {
                    $(formId).submit();
                } else {
                    $(this).data("success", true).dialog("close");
                }
            },
            Cancel: function () {
                $(this).data("success", false).dialog("close");
            }
        },
        close: function () {
            var preClosingFunction = $(this).data("preClosingFunction");
            if (preClosingFunction) {
                if ($(this).data("preClosingData")) {
                    preClosingFunction($(this).data("preClosingData"));
                } else {
                    preClosingFunction();
                }
            }
            var postFunction = $(this).data("postFunction");
            if ($(this).data("success") && postFunction) {
                if ($(this).data("postData")) {
                    postFunction($(this).data("postData"));
                } else {
                    postFunction();
                }
            }
            var autoRemove = $(this).data("autoRemove");
            if (autoRemove) {
                $(this).remove();
            }
        }
    });
};

jQuery.fn.divDialogContent = function () {
    return $(this).parents('.ui-dialog-content');
};

//Select the Dialog Buttons
function getDialogButton(dialog, buttonName) {
    var buttons = $(dialog).parents('.ui-dialog').find('.ui-dialog-buttonpane button');
    for (var i = 0; i < buttons.length; ++i) {
        var jButton = $(buttons[i]);
        if (jButton.text() == buttonName) {
            return jButton;
        }
    }
    return null;
}

//Button Controller for AJAX Loading:
function setDialogButton(dialog, buttonName, newButtonName, disabled) {
    var dialogButton = getDialogButton(dialog, buttonName);
    if (dialogButton) {
        dialogButton.find('.ui-button-text').html(newButtonName);
        if (disabled) {
            dialogButton.attr('disabled', 'true');
            dialogButton.addClass('ui-state-disabled');
        } else {
            dialogButton.removeAttr('disabled', 'true');
            dialogButton.removeClass('ui-state-disabled');
        }
    }
}