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
        $_temp = $('<table>').appendTo($_oform.parent());

        //1-n 燃料計算
        SetFuel();

        //1-n 冷媒設備
        SetRefrigerantEquip();

        //////1-n 其它項目(3-6)
        ////SetOther3_6();

        helper.bootstrap.genBootstrapTabpanel($_temp.parent(), "tabPanel_1", "tabPanel",
            ['燃料計算', '冷媒設備', '空空1'],
            [$_dFuelContainer, $_dRefrigerantEquipContainer, $_temp]);
    }

    function SetTabs2() {

        var $_oform = $("#_tabs");

        $_dElecContainer = $('<table>').appendTo($_oform.parent());
        $_temp = $('<table>').appendTo($_oform.parent());

        //1-n 電力計算
        SetElec();

        helper.bootstrap.genBootstrapTabpanel($_temp.parent(), "tabPanel_2", "tabPanel",
            ['電力計算', '空空2'],
            [$_dElecContainer, $_temp]);
    }

    function SetTabs3() {

        var $_oform = $("#_tabs");

        $_temp = $('<table>').appendTo($_oform.parent());

        //////1-n 其它項目(3-6)
        ////SetOther3_6();

        helper.bootstrap.genBootstrapTabpanel($_temp.parent(), "tabPanel_3", "tabPanel",
            ['3-6其它項目'],
            [$_temp]);
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
            $_dElecContainer.DouEditableTable(_opt);
        });
    };

    //其它項目(3-6)
    function SetOther3_6() {

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