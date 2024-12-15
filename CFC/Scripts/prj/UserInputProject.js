$(document).ready(function () {

    //產出清冊
    var a = {};
    a.item = '<span class="btn btn-success glyphicon glyphicon-download-alt"> 產出清冊</span>';
    a.event = 'click .glyphicon-download-alt';
    a.callback = function importQdate(evt) {

        var $element = $('body');
        helper.misc.showBusyIndicator($element, { timeout: 3 * 60 * 60 * 1000 });
        $.ajax({
            url: app.siteRoot + 'UserInputProject/ExportProjectList',
            datatype: "json",
            type: "Post",
            timeout: 0,
            success: function (data) {
                if (data.result) {
                    //window.open(data.url, '_blank');
                    location.href = data.url;
                    //alert("產出清冊成功");
                } else {
                    alert("產出清冊失敗：\n" + data.errorMessage);
                }

                helper.misc.hideBusyIndicator();
            },
            complete: function () {
                helper.misc.hideBusyIndicator();
            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);
                helper.misc.hideBusyIndicator();
            }
        });
    };

    douoptions.appendCustomToolbars = [a];

    douoptions.queryFilter = function (params, callback) {

        var FilterStartS = params.find(a => a.key == "FilterStartS");
        FilterStartS.value = $('.filter-toolbar-plus [data-fn="FilterStartS"] input').val();

        var FilterStartE = params.find(a => a.key == "FilterStartE");
        FilterStartE.value = $('.filter-toolbar-plus [data-fn="FilterStartE"] input').val();

        callback();
    }

    $("#_table").DouEditableTable(douoptions);

    //Fitler日期
    $('.filter-toolbar-plus [data-fn="FilterStartS"] input').val('');
    $('.filter-toolbar-plus [data-fn="FilterStartE"] input').val('');
})