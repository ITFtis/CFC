////$('.btn-confirm').click(function () {

////    var conditions = GetFilterParams($d)
////    var paras;
////    if (conditions.length > 0) {
////        paras = { key: 'filter', value: JSON.stringify(conditions) };
////    }

////    var url = window.siteroot + 'Cv/GetTabCalsPropertiesList';
////    var current = $d.instance;
////    current.$table.bootstrapTable("refresh", { url: url, filterColumns: paras }); //透過this.settings.tableOptions.url自動查詢
////});

function GetFilterParams($_MasterTable) {
    var _fielter = [];
    var that = $_MasterTable.instance;
    var current = $_MasterTable.instance;
    $.each(that.$___filterToolbar.find("[data-fn]"), function () {
        var _fn = $(this).attr("data-fn");
        var _filed = current.___getFiled(_fn);
        _fielter.push({ key: _fn, value: _filed.editFormtter.getValue.call(_filed, $(this)) });
    });

    return _fielter;
}
