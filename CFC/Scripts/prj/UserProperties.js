$(document).ready(function () {

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