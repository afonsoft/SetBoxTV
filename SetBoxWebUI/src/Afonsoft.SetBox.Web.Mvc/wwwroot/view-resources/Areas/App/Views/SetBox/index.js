var translate = {
    infos: "Exibindo {{ctx.start}} Até {{ctx.end}} de {{ctx.total}} registros",
    loading: "Carregando, isso pode levar alguns segundos...",
    noResults: "Não há dados para exibir",
    refresh: "Atualizar",
    search: "Pesquisar"
};

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