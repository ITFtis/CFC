$(document).ready(function () {

    douoptions.afterCreateEditDataForm = function ($container, row) {

        var isAdd = JSON.stringify(row) == '{}';

        if (!isAdd) {
            //只有分非製造業與製造業
            if (row.IndustrialTypeId > 1) {
                $('.modal-dialog [data-fn="IndustrialTypeId"]').val(99);
            }
        }
    }

    var $_masterTable = $("#_table").DouEditableTable(douoptions);

})