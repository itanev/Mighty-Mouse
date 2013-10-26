(function () {
    $("#document-types").change(function (e) {
        var dropDown = $("#document-types");
        var type = dropDown.children()[dropDown.val() - 1].innerText;
       
        HttpRequester.postJson("SelectType", type, function (data) { console.log(data); });
    });
})();

