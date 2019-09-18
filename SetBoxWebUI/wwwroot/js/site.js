
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
$(document).ajaxStart(function () {
    PleaseWaitShow();
});
$(document).ajaxSend(function () {
    PleaseWaitShow();
});
$(document).ajaxComplete(function () {
    PleaseWaitHide();
});
$(document).ajaxError(function () {
    PleaseWaitHide();
});
$(document).ajaxStop(function () {
    PleaseWaitHide();
})
$(document).ajaxSuccess(function () {
    PleaseWaitHide();
})


$(document).ready(function () {
    PleaseWaitHide();
});

function FormatJsonDateToJavaScriptDate(value) {
    var dt = new Date(value);
    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
}

function FormatsonSize(fileSizeInBytes) {
    var i = -1;
    var byteUnits = [' kB', ' MB', ' GB', ' TB', 'PB', 'EB', 'ZB', 'YB'];
    do {
        fileSizeInBytes = fileSizeInBytes / 1024;
        i++;
    } while (fileSizeInBytes > 1024);

    return Math.max(fileSizeInBytes, 0.1).toFixed(1) + byteUnits[i];
}


var translate = {
    infos: "Exibindo {{ctx.start}} Até {{ctx.end}} de {{ctx.total}} registros",
    loading: "Carregando, isso pode levar alguns segundos...",
    noResults: "Não há dados para exibir",
    refresh: "Atualizar",
    search: "Pesquisar"
};