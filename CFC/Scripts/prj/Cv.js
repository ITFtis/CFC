$(document).ready(function () {

    //設定Tabs1
    SetTabs1();

    function SetTabs1() {

        var $_oform = $("#_tabs1");

        $_d1EditDataContainer = $('<table>').appendTo($_oform.parent());
        $_d2EditDataContainer = $('<table>').appendTo($_oform.parent());
        $_d3EditDataContainer = $('<table>').appendTo($_oform.parent());

        //1-n 燃料計算
        SetFuel();

        //1-n 電力計算
        SetElec();

        //1-n 其它項目(3-6)
        SetOther3_6();

        helper.bootstrap.genBootstrapTabpanel($_d2EditDataContainer.parent(), undefined, undefined,
            ['燃料計算', '電力計算', '其它項目(3-6)'],
            [$_d1EditDataContainer, $_d2EditDataContainer, $_d3EditDataContainer]);
    }

    //燃料計算
    function SetFuel() {

        $.getJSON(window.siteroot + 'Cv/GetTabFuelList', function (_opt) { //取model option

            _opt.title = '燃料計算';

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

    //電力計算
    function SetElec() {

        $.getJSON(window.siteroot + 'Cv/GetTabElecList', function (_opt) { //取model option

            _opt.title = '電力計算';

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

    //其它項目(3-6)
    function SetOther3_6() {

    }
})