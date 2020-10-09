
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
    var byteUnits = [' KB', ' MB', ' GB', ' TB', 'PB', 'EB', 'ZB', 'YB'];
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

function ShowName(id) {
    $("#File_Name" + id).text($("#fileToUpload" + id).val());
    $("#FileSelected" + id).val($("#fileToUpload" + id).val());
    return UploadSubimit(id);
}

function UploadSubimit(id) {
    var formData = new FormData(document.getElementById('uploadForm' + id));
    $("#progress" + id).show();
    $("#progressbar" + id).css("width", "0%");
    $("#progressbar" + id).html("0%");
    $.ajax({
        url: '/Streaming/UploadFileStream',
        type: 'POST',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        enctype: 'multipart/form-data',
        timeout: 5 * 60 * 1000,
        xhr: function () {
            //upload Progress
            var xhr = $.ajaxSettings.xhr();

            xhr.upload.onprogress = function (event) {
                var percent = 0;
                var position = event.loaded || event.position;
                var total = event.total;
                if (event.lengthComputable) {
                    percent = Math.ceil(position / total * 100);
                }
                //update progressbar
                $("#progressbar" + id).css("width", + percent + "%");
                $("#progressbar" + id).html(percent + "%");
            }
            return xhr;
        },
        success: function (data) {
            $("#progress" + id).hide();
            $("#fileToUpload" + id).hide();
            $("#fileId" + id).text(data);
            FixFileIds();
        }, 
        error: function (xhr, ajaxOptions, thrownError) {
            $("#progress" + id).hide();
            console.log(xhr.status + ' - ' + xhr.statusText + ' - ' + xhr.responseText);
            console.log(thrownError);
            var output = '';
            for (var entry in xhr.responseJSON) {
                output += entry + ' - ' + xhr.responseJSON[entry] + '<br/>';
            }
            console.log(output);
            bootbox.alert(xhr.status + ' - ' + xhr.statusText + '<br/>' + output);
        }
    });
    return false;
};

function FixFileIds() {
    $("#FilesIds").val($("#fileId1").text() + ',' + $("#fileId2").text() + ',' + $("#fileId3").text());
}

function UpdateFilesFolder() {
    $.ajax({
        url: '/Files/ProcessFilesInDirectory',
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        timeout: 5 * 60 * 1000,
        success: function (data) {
            PleaseWaitHide();
            bootbox.alert(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            PleaseWaitHide();
            console.log(xhr.status + ' - ' + xhr.statusText + ' - ' + xhr.responseText);
            console.log(thrownError);
            var output = '';
            for (var entry in xhr.responseJSON) {
                output += entry + ' - ' + xhr.responseJSON[entry] + '<br/>';
            }
            console.log(output);
            bootbox.alert(xhr.status + ' - ' + xhr.statusText + '<br/>' + output);}
    });
}