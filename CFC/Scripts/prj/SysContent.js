$(document).ready(function () {

    var $_editDataContainer = undefined;

    douoptions.afterCreateEditDataForm = function ($container, row) {
        hasChangeDetails = false;
        if (row.Id == undefined)
            return;
        $.getJSON($.AppConfigOptions.baseurl + 'SysContentDetail/GetDataManagerOptionsJson', function (_opt) {

            _opt.title = '細項';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;

            //給detail集合
            row.Details = row.Details || []; //無detail要實體參考，之後detail編輯才能跟master有關聯(前端物件)
            _opt.datas = row.Details;

            _opt.addServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'SysContentDetail/Add', callback);
            };

            _opt.updateServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'SysContentDetail/Update', callback);
            };

            _opt.deleteServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'SysContentDetail/Delete', callback);
            };

            //編輯後給detail.MasterId
            _opt.beforeCreateEditDataForm = function (drow, callback) {
                drow.ContentId = row.Id;

                callback();
            };


            //Master的編輯物件
            var $_oform = $container.find(".data-edit-form-group");

            //Detail的編輯物件
            $_editDataContainer = $('<div style="background-color: #FFFFf1;padding: .5rem;border-radius: .5rem;">').appendTo($_oform.parent());

            //實體Dou js
            var $_detailTable = $('<table>').appendTo($_editDataContainer).DouEditableTable(_opt)

            ////var $_detailTable = $('<table>').appendTo($_editDataContainer).DouEditableTable(_opt)
            ////    .on([$.dou.events.add, $.dou.events.update, $.dou.events.delete].join(' '), function () {
            ////        //dou舊版(無資料處理)
            ////        $('.syscontentcontroller .bootstrap-table').find('table .dou-field-Title:contains("無資料")').closest('tr').hide();
            ////    });

            //////dou舊版(無資料處理)
            ////$('.syscontentcontroller .bootstrap-table').find('table .dou-field-Title:contains("無資料")').closest('tr').hide();

        });
    }

    var $_masterTable = $("#_table").DouEditableTable(douoptions);
});
