$(document).ready(function () {

    //設定Tabs1
    SetTabs1();

    //設定Tabs2
    SetTabs2();

    //設定Tabs3
    SetTabs3();

    setTimeout(function () {
        //Reset頁籤內容
        ResetTabContent();
    }, 100);

    $('#divClass button').click(function () {

        //////Remove class selected on all buttons
        ////$(this).siblings().removeClass('selected').removeClass("btn-primary")
        ////$(this).addClass("btn-default");

        //Remove class selected
        $(this).siblings('.selected').removeClass('selected').removeClass("btn-primary").addClass("btn-default");

        //Add class the clicked button
        $(this).removeClass("btn-default").addClass('selected').addClass("btn-primary");

        //Update the hidden field of the value    
        $(this).parents('fieldset').find('input[type="hidden"]').val($(this).text());

        //Reset頁籤內容
        ResetTabContent();
    })

    function SetTabs1() {

        var $_oform = $("#_tabs");

        $_dFuelContainer = $('<table>').appendTo($_oform.parent());
        $_dRefrigerantEquipContainer = $('<table>').appendTo($_oform.parent());
        $_dSetEscapeTypeContainer = $('<table>').appendTo($_oform.parent());
        $_dSetEscapePropertiesContainer = $('<table>').appendTo($_oform.parent());
        $_dSetSpecificTypeContainer = $('<table>').appendTo($_oform.parent());
        $_dSetSpecificPropertiesContainer = $('<table>').appendTo($_oform.parent());
        $_temp = $('<table>').appendTo($_oform.parent());

        //1-n 燃料計算
        SetFuel();

        //1-n 冷媒設備
        SetRefrigerantEquip();

        //1-n 逸散種類
        SetEscapeType();

        //1-n 逸散氣體
        SetEscapeProperties();

        //1-n 製程種類
        SetSpecificType();

        //1-n 製程原料
        SetSpecificProperties();

        helper.bootstrap.genBootstrapTabpanel($_temp.parent(), "tabPanel_1", "tabPanel",
            ['燃料計算', '冷媒設備', '逸散種類',
                '逸散氣體', '製程種類', '製程原料'],
            [$_dFuelContainer, $_dRefrigerantEquipContainer, $_dSetEscapeTypeContainer,
                $_dSetEscapePropertiesContainer, $_dSetSpecificTypeContainer, $_dSetSpecificPropertiesContainer]);
    }

    function SetTabs2() {

        var $_oform = $("#_tabs");

        $_temp = $('<table>').appendTo($_oform.parent());


        helper.bootstrap.genBootstrapTabpanel($_temp.parent(), "tabPanel_2", "tabPanel",
            ['空空'],
            [$_temp]);
    }

    function SetTabs3() {

        var $_oform = $("#_tabs");
        
        $_dSetCalsTypeContainer = $('<table>').appendTo($_oform.parent());

        //1-n 類別3-6
        SetCalsType();

        helper.bootstrap.genBootstrapTabpanel($_dSetCalsTypeContainer.parent(), "tabPanel_3", "tabPanel",
            ['類別3-6'],
            [$_dSetCalsTypeContainer]);
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
            $_dFuelContainer.DouEditableTable(_opt);
        });
    };

    //冷媒設備
    function SetRefrigerantEquip() {

        $.getJSON(window.siteroot + 'Cv/GetTabRefrigerantEquipList', function (_opt) { //取model option

            _opt.title = '冷媒設備';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;
            _opt.editformSize = { minWidth: 700 };

            _opt.addServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'RefrigerantEquip/Add', callback);
            };

            _opt.updateServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'RefrigerantEquip/Update', callback);
            };

            _opt.deleteServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'RefrigerantEquip/Delete', callback);
            };

            //實體Dou js
            $_dRefrigerantEquipContainer.DouEditableTable(_opt);
        });
    };

    //逸散種類
    function SetEscapeType() {

        $.getJSON(window.siteroot + 'Cv/GetTabEscapeTypeList', function (_opt) { //取model option

            _opt.title = '逸散種類';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;
            _opt.editformSize = { minWidth: 700 };

            _opt.addServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'EscapeType/Add', callback);
            };

            _opt.updateServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'EscapeType/Update', callback);
            };

            _opt.deleteServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'EscapeType/Delete', callback);
            };

            //實體Dou js
            $_dSetEscapeTypeContainer.DouEditableTable(_opt);
        });
    };

    //逸散氣體
    function SetEscapeProperties() {

        $.getJSON(window.siteroot + 'Cv/GetTabEscapePropertiesList', function (_opt) { //取model option

            _opt.title = '逸散氣體';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;
            _opt.editformSize = { minWidth: 700 };

            _opt.addServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'EscapeProperties/Add', callback);
            };

            _opt.updateServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'EscapeProperties/Update', callback);
            };

            _opt.deleteServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'EscapeProperties/Delete', callback);
            };

            //實體Dou js
            $_dSetEscapePropertiesContainer.DouEditableTable(_opt);
        });
    };

    //製程種類
    function SetSpecificType() {

        $.getJSON(window.siteroot + 'Cv/GetTabSpecificTypeList', function (_opt) { //取model option

            _opt.title = '逸散種類';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;
            _opt.editformSize = { minWidth: 700 };

            _opt.addServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'SpecificType/Add', callback);
            };

            _opt.updateServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'SpecificType/Update', callback);
            };

            _opt.deleteServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'SpecificType/Delete', callback);
            };

            //實體Dou js
            $_dSetSpecificTypeContainer.DouEditableTable(_opt);
        });
    };

    //製程原料
    function SetSpecificProperties() {

        $.getJSON(window.siteroot + 'Cv/GetTabSpecificPropertiesList', function (_opt) { //取model option

            _opt.title = '逸散氣體';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;
            _opt.editformSize = { minWidth: 700 };

            _opt.addServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'SpecificProperties/Add', callback);
            };

            _opt.updateServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'SpecificProperties/Update', callback);
            };

            _opt.deleteServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'SpecificProperties/Delete', callback);
            };

            //實體Dou js
            $_dSetSpecificPropertiesContainer.DouEditableTable(_opt);
        });
    };

    //類別3-6
    function SetCalsType() {
        $.getJSON(window.siteroot + 'Cv/GetTabCalsTypeList', function (_opt) { //取model option

            _opt.title = '類別3-6';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;
            _opt.editformSize = { minWidth: 700 };

            _opt.addServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'CalsType/Add', callback);
            };

            _opt.updateServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'CalsType/Update', callback);
            };

            _opt.deleteServerData = function (row, callback) {
                transactionDouClientDataToServer(row, window.siteroot + 'CalsType/Delete', callback);
            };

            //實體Dou js
            $_dSetCalsTypeContainer.DouEditableTable(_opt);
        });
    }
})

//Reset頁籤內容
function ResetTabContent() {
    $('[name="cvTabs"]').hide();
    $('.tabPanel').hide();
    var btn = $('#divClass').find('button.selected')[0];
    if (btn != null) {
        if (btn.id == 'tab1') {
            $('#tabPanel_1').show();
        }
        else if (btn.id == 'tab2') {
            $('#tabPanel_2').show();
        }
        else if (btn.id == 'tab3') {
            $('#tabPanel_3').show();
        }
    }
}