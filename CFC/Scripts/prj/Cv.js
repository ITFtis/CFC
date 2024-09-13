$(document).ready(function () {
   
    var $_oform = $("#_tabs");

    $_d1EditDataContainer = $('<table>').appendTo($_oform.parent());
    $_d2EditDataContainer = $('<table>').appendTo($_oform.parent());
    $_d3EditDataContainer = $('<table>').appendTo($_oform.parent());

    //1-n 類別一
    SetDouDa1();

    //1-n 類別二
    SetDouDa2();

    //1-n 類別三-六
    SetDouDa3();

    helper.bootstrap.genBootstrapTabpanel($_d2EditDataContainer.parent(), undefined, undefined,
        ['類別一', '類別二', '類別三-六'],
        [$_d1EditDataContainer, $_d2EditDataContainer, $_d3EditDataContainer]);

    //類別一
    function SetDouDa1() {

        $.getJSON(window.siteroot + 'Cv/GetTabClass1List', function (_opt) { //取model option

            _opt.title = '類別一';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;
            _opt.editformSize = { minWidth: 700 };

            _opt.addServerData = function (row, callback) {
                    transactionDouClientDataToServer(row, window.siteroot + 'FuelProperties/Add', callback);
                };

            _opt.updateServerData = function (row, callback) {
                    transactionDouClientDataToServer(row, window.siteroot + 'FuelProperties/Update', callback);
                };

            _opt.deleteServerData = function (row, callback) {
                    transactionDouClientDataToServer(row, window.siteroot + 'FuelProperties/Delete', callback);
                };

            //實體Dou js
            $_d1Table = $_d1EditDataContainer.DouEditableTable(_opt);
        });
    };

    //類別二
    function SetDouDa2() {

        $.getJSON(window.siteroot + 'Cv/GetTabClass2List', function (_opt) { //取model option

            _opt.title = '類別二';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;
            _opt.editformSize = { minWidth: 700 };

            _opt.addServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'ElecProperties/Add', callback);
            };

            _opt.updateServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'ElecProperties/Update', callback);
            };

            _opt.deleteServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'ElecProperties/Delete', callback);
            };

            //實體Dou js
            $_d2Table = $_d2EditDataContainer.DouEditableTable(_opt);
        });
    };

    //類別三-六
    function SetDouDa3() {

    }
})