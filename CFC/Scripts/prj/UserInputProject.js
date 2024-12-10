$(document).ready(function () {
    $("#_table").DouEditableTable(douoptions);

    //Fitler日期
    $('.filter-toolbar-plus [data-fn="FilterStartS"] input').val('');
    $('.filter-toolbar-plus [data-fn="FilterStartE"] input').val('');
})