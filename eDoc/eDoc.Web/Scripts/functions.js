(function () {
    $("#document-types").change(function (e) {
        var dropDown = $("#document-types");
        var type = dropDown.children()[dropDown.val() - 1].innerText;
       
        $.get(@Url.Action("Document","SelectType", new { type = type } ), function (data) {
            $('#document-template').replaceWith(data);
        });
    });
})();

