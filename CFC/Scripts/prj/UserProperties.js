$(document).ready(function () {

    //匯出Excel
    var a = {};
    a.item = '<span class="btn btn-secondary glyphicon glyphicon-download-alt"> 匯出Excel</span>';
    a.event = 'click .glyphicon-download-alt';
    a.callback = function importQdate(evt) {
        var $element = $('body');
        helper.misc.showBusyIndicator($element, { timeout: 3 * 60 * 60 * 1000 });
        $.ajax({
            url: app.siteRoot + 'UserProperties/ExportUserList',
            datatype: "json",
            type: "Post",
            timeout: 0,
            success: function (data) {
                if (data.result) {                    
                    location.href = data.url;
                    //alert("會員匯出清單成功");
                } else {
                    alert("會員匯出清單失敗：\n" + data.errorMessage);
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

    douoptions.afterCreateEditDataForm = function ($container, row) {

        var isAdd = JSON.stringify(row) == '{}';

        if (!isAdd) {
            //只有分非製造業與製造業
            if (row.IndustrialTypeId > 1) {
                $('.modal-dialog [data-fn="IndustrialTypeId"]').val(99);
            }
        }

        //縣市(citycode)：$('.modal-dialog [data-fn="CITY"]').find('option:selected').attr('data-citycode')
        //鄉鎮(citycode)：$('.modal-dialog [data-fn="DISTRICT"]').find('option:selected').attr('data-citycode')

        //設定鄉鎮
        var oCity = $('.modal-dialog [data-fn="CITY"]')[0];
        var oTown = $('.modal-dialog [data-fn="DISTRICT"]')[0];

        var city = $(oCity).find('option:selected').attr('data-citycode');
        var town = $(oTown).find('option:selected').val();
        FilterCityTown(city, oTown);
        $(oTown).val(town);

        $(oCity).change(function () {
            var city = $(this).find('option:selected').attr('data-citycode');
            FilterCityTown(city, oTown);
        });

        //(下拉)非製造業
        SetUIDialog();
        $('.modal-dialog [data-fn="IndustrialTypeId"]').change(function () {
            SetUIDialog();
        });
    }

    var $_masterTable = $("#_table").DouEditableTable(douoptions);

    function SetUIDialog() {
        //(下拉)非製造業
        var oIndustrialTypeId = $('.modal-dialog [data-fn="IndustrialTypeId"]');
        var IndustrialTypeId = $(oIndustrialTypeId).find(':checked').val();
        if (IndustrialTypeId == 1) {
            //非製造業
            $('.modal-dialog').find("[data-field=UNIT_TYPE]").show();
            $('.modal-dialog').find("[data-field=CITY]").show();
            $('.modal-dialog').find("[data-field=DISTRICT]").show();
            $('.modal-dialog').find("[data-field=ADDRESS]").show();
        }
        else {
            //製造業
            $('.modal-dialog [data-fn="UNIT_TYPE"]').val('');
            $('.modal-dialog [data-fn="CITY"]').val('');
            $('.modal-dialog [data-fn="DISTRICT"]').val('');
            $('.modal-dialog [data-fn="ADDRESS"]').val('');

            $('.modal-dialog').find("[data-field=UNIT_TYPE]").hide();
            $('.modal-dialog').find("[data-field=CITY]").hide();
            $('.modal-dialog').find("[data-field=DISTRICT]").hide();
            $('.modal-dialog').find("[data-field=ADDRESS]").hide();
        }
    }
})