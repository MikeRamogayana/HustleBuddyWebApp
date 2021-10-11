function GetModel(url) {
    $.get(url, function (partial) {
        $("div.modal-content").empty().append(partial);
    });
}