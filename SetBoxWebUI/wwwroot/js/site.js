
//Show Load Page
function PleaseWaitShow() {
    $("#pleaseWait").css("display", "block");
    $('#pleaseWait').show();
}
//Hide Load Page
function PleaseWaitHide() {
    $("#pleaseWait").css("display", "none");
    $('#pleaseWait').hide();
}

//Load page in ajax
$(document).ajaxStart(function (event, request, settings) {
    PleaseWaitShow();
});
$(document).ajaxSend(function (event, request, settings) {
    PleaseWaitShow();
});
$(document).ajaxComplete(function (event, request, settings) {
    PleaseWaitHide();
});
$(document).ajaxError(function (event, request, settings) {
    PleaseWaitHide();
});

$(document).ready(function () {
    PleaseWaitHide();
});