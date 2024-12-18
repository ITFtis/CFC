$(document).ready(function () {

    $(douoptions.fields).each(function () {
        //資料列click事件(工時)
        var columnName = this.field;

        if (columnName.indexOf("VClassTotal") > -1) {
            this.formatter = function (v, d) {
                value = d.VClass1 + d.VClass2 + d.VClass3 + d.VClass4 + d.VClass5 + d.VClass6;
                return MathRound(value, 2);
            }
        }
    })

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

    //讓系統跑預設值
    setTimeout(function () {
        $('.fixed-table-toolbar .btn-confirm').trigger("click");
    }, 0.3 * 1000)

})