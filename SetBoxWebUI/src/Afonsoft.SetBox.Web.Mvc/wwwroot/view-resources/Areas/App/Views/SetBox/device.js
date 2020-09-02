
function openModal(action, id) {

    $("#ModalTitle").val(action + " - DeviceId: " + id);
    //pegar as informações do Device

    if (action == "Details") {
        //Desabilitar para ediçao os campos e ocultar o botão salvar
        $("#buttonSave").hide();
        $("#DivPainelConfig").hide();
        $("#DivPainelLog").show();

        $("#grid-data-device-logError").bootgrid(
            {
                ajax: true,
                post: function () {
                    return {
                        id: id
                    };
                },
                url: '/Devices/ListLogError',
                labels: translate,
                searchSettings: {
                    delay: 100,
                    characters: 3
                },

                converters: {
                    datetime: {  // Tratar os campos data que vem no formato incorreto
                        from: function (value) { return moment(value); },
                        to: function (value) { return moment(FormatJsonDateToJavaScriptDate(value)).format("DD/MM/YYYY"); }
                    }
                }
            });

        $("#grid-data-device-log").bootgrid(
            {
                ajax: true,
                post: function () {
                    return {
                        id: id
                    };
                },
                url: '/Devices/ListLog',
                labels: translate,
                searchSettings: {
                    delay: 100,
                    characters: 3
                },

                converters: {
                    datetime: {  // Tratar os campos data que vem no formato incorreto
                        from: function (value) { return moment(value); },
                        to: function (value) { return moment(FormatJsonDateToJavaScriptDate(value)).format("DD/MM/YYYY"); }
                    }
                }
            });


        $("#sortable").sortable({
            axis: 'y',
            update: function (event, ui) {
                var data = $(this).sortable('serialize');
                $.post('/Devices/UpdateOrderFiles', { id: id, order: data });
            }
        });
        $("#sortable").disableSelection();

    } else {
        $("#DivPainelConfig").show();
        $("#buttonSave").show();
        $("#DivPainelLog").hide();
    }
    $('#ModalEdit').modal('show');
}


$(document).ready(function () {

    $('#SliderTransactionTime').slider({
        formatter: function (value) {
            return 'Time: ' + value + ' Segunds';
        }
    });

    $("#SliderTransactionTime").on("slide", function (slideEvt) {
        $("#TransactionTimeVal").text(slideEvt.value);
    });

    var grid = $("#grid-data-device").bootgrid(
        {
            ajax: true,
            url: '/Devices/List',
            labels: translate,
            searchSettings: {
                delay: 100,
                characters: 3
            },

            formatters: {
                "actions": function (column, row) {
                    return "<div class='btn-group btn-group-sm' role='group'>" +
                        "<a href='#' alt='Details' class='btn btn-info btn-sm' data-command='Details' data-row-id = '" + row.deviceId + "'>" +
                        "<span class='glyphicon glyphicon-list'></span>" + "</a>" +
                        "<a href='#' alt='Edit' class='btn btn-warning btn-sm' data-command='Edit' data-row-id = '" + row.deviceId + "'>" +
                        "<span class='glyphicon glyphicon-edit'></span>" + "</a>" +
                        "<a href='#' alt='Delete' class='btn btn-danger btn-sm' data-command='Delete' data-row-id = '" + row.deviceId + "'>" +
                        "<span class='glyphicon glyphicon-trash'></span>" + "</a></div>";
                }
            },
            converters: {
                datetime: {  // Tratar os campos data que vem no formato incorreto
                    from: function (value) { return moment(value); },
                    to: function (value) { return moment(FormatJsonDateToJavaScriptDate(value)).format("DD/MM/YYYY"); }
                }
            }
        });

    grid.on("loaded.rs.jquery.bootgrid", function () {
        grid.find("a.btn").each(function (index, element) {
            var buttonAction = $(element);
            var command = buttonAction.data("command");
            var id = buttonAction.data("row-id");
            buttonAction.on("click", function () {
                if (command == 'Delete') {
                    bootbox.confirm({
                        title: "Excluir",
                        message: "Deseja excluir esse device?<br/> Todo o historico será apagado!",
                        buttons: {
                            cancel: {
                                label: '<i class="fa fa-times"></i> Cancelar'
                            },
                            confirm: {
                                label: '<i class="fa fa-check"></i> Confirmar'
                            }
                        },
                        callback: function (result) {
                            if (result) {
                                $.ajax({
                                    url: '/Devices/Delete?id=' + id,
                                    cache: false,
                                    success: function (result) {
                                        PleaseWaitHide();
                                        bootbox.alert({ title: "Info", message: result });
                                        grid.reload();
                                    },
                                    error: function (xhr, ajaxOptions, thrownError) {
                                        PleaseWaitHide();
                                        bootbox.alert({ title: "Error", message: xhr.responseText });
                                        grid.reload();
                                    }
                                });
                            }
                        }
                    });
                } else {
                    $("#id").val(id);
                    $("#command").val(command);
                    $("#FormEdit").submit();
                }
            });
        });
    });
});