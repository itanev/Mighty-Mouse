/// <reference path="Scripts/q.js" />
/// <reference path="Scripts/jquery-2.0.2.js" />

var HttpRequester = (function () {

    var makeRequest = function (url, requestType, data, success, error) {
        
        if(data){
            data = JSON.stringify({type: data});
        }

        $.ajax({
            url: url,
            type: requestType,
            data: data,
            contentType: "application/json",
            success: function (data) {
                success(data);
            },
            error: function (err) {
                error(err);
            }
        });
    }

    var get = function (url, success, error) {
        makeRequest(url, "get", undefined, success, error);
    }

    var post = function (url, data, success, error) {
        makeRequest(url, "post", data, success, error);
    }

    return {
        getJson: get,
        postJson: post
    }
}())