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