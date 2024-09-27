
$(document).ready(function () {
    //$('input').on('input', function (e, args) {
    //    console.log('sss');
    //});

    //展開
    $('.show-more-btn').on('click', function () {
        //alert($(this).parent().html());
        $(this).toggleClass("close").parent().find('.rule-set').toggleClass('show-all');
    });

    //清除
    $('.count-btn-group .btn-gray').on('click', function () {

        // 清空自訂新增項目
        $('i[data-icon="additionTrash"]').each((index, item) => {
            $(item).click();
        })

        // 回到預設值
        $.each($('.rule-row input,.rule-row select'), function (index, item) {
            if ($(item).is("select"))
                $(item).val($(this).find("option:first").val());
            else
                $(this).val($(this).attr('min'));
        });
    });

    //檢視
    $('.count-btn-group .btn-green').on('click', function () {
        var $_btn = $(this).toggleClass('active');
        var $_rulegroup = $('.count-rule-group').removeClass('view');
        $_rulegroup.find('.view').removeClass('view');
        //console.log($_btn.hasClass('active'));
        if ($_btn.hasClass('active')) {

            var _flag = false;
            $.each($('.rule-row input'), function () {
                var $_this = $(this);
                if ($_this.attr('min') != $_this.val()) {
                    $_this.closest('.rule-row').addClass('view');//.parents('.card').addClass('view');
                    if ($_this.attr('name') == "elecVolume") {
                        $_this.closest('.rule-set').find('.rule-row').addClass('view');
                    }
                    _flag = true;
                }
            });
            if (_flag) {
                $_rulegroup.addClass('view');
            }
            else {
                $_btn.removeClass('active');
                helper.jspanel.jspAlertMsg(undefined, { content: '尚未輸入任何資料，無法檢視!!', classes: 'modal-sm', autoclose: 5000 });
            }
        }
    });

    //計算
    $(".count-btn-group .btn-red").on("click", function () {

        // 自填係數不可超過1
        if ($('input[class*="coe"]').filter((index, e) => {
            var value = $(e).val();
            return value < 0 || value > 1;
        }).length > 0)
            alert("排放係數必須大於0且小於1");

        // 自填係數不可超過1
        else if ($('input[type*="number"]').filter((index, e) => $(e).val() < 0).length > 0)
            alert("填入用量不可小於0");


        // 燃料計算ARType為必填
        else if ($(".calCoeGroup .coeActive").length == 0)
            alert(" 燃料計算ARType為必填");

        else {
            helper.misc.showBusyIndicator();

            //往後丟，呈現計算結果
            $('body').addClass('show-cal');
            $('#CalRowID').attr("value", "0");
            $.ajax({
                url: site_root + 'api/cfc/cal',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(getUserInput()),
                method: 'POST'
            }).done(function (r) {
                var $_result = $('.count-result-content').removeClass('err-message');
                $('table td.d-content> label[dn^="d-R"]', $_result).text('--');
                if (r.Success) {
                    for (var f in r.Result) {
                        $_result.find('label[name="' + f + '"]').text(helper.format.formatNumber(parseFloat(r.Result[f]), 2));
                    }
                    $('#CalRowID').attr("value", r.Result.RowID);
                } else {
                    $_result.addClass('err-message').find('.result-hint').text(r.Message);
                }
            }).always(function (_data, _textStatus) {
                helper.misc.hideBusyIndicator();
            });
        }
    });

    // 儲存專案
    $('.download-container button[name="saveProject"]').on("click", function () {
        $(".saveProject-cover").attr("style", "");
    });

    // 查看歷史專案
    $('.download-container button[name="showHisProject"] , .login-info button[name="showHisProject"]').on("click", function () {
        $(".viewProject-cover").attr("style", "");
    });

    // 會員資料修改
    $('.download-container button[name="momberModify"] , .login-info button[name="memberModify"]').on("click", function () {
        $(".memberModify-cover").attr("style", "");
    });



    // 下載細部內容
    $(".download-container .download-cal").on("click", function () {

        helper.misc.showBusyIndicator();
        fetch(site_root + 'api/DataPrint/Detail', {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                RowID: $("#CalRowID").attr("value"),
                UserID: $("#UserID").attr("value"),
                FactoryRegistration: $("#factoryDropdown").val(),
            })
        }).then(e => e.json())
            .then(response => {
                helper.misc.hideBusyIndicator();
                if (!response.isSucess)
                    alert("檔案產製失敗，請洽管理人員。");

                console.log(response.fileAdd);
                //window.open(response.fileAdd);
                location.href = response.fileAdd;
            });
    });

    // 取得使用者輸入參數
    function getUserInput() {
        var m = { UserID: $("#UserID").attr("value") };
        // 燃料部分
        var fuelInputs = [];
        $.each($('.fuel-row input'), function (index, item) {
            if ($(this).val() > 0)
                fuelInputs.push({
                    FuelId: $(this).attr('name'),
                    UseVolume: $(this).val()
                });
        })
        m["fuelInputs"] = fuelInputs;

        // 燃料係數
        m["calModel"] = $(".calCoeGroup .coeActive").text();

        // 電力部分
        var elevYear = $("#elecYear").val();
        var elecVolume = $("#elecVolume").val();
        m["electInput"] = {
            elecYear: elevYear != null && elevYear.trim().length > 0 ? elevYear : 110,
            elecVolume: elecVolume != null && elecVolume.trim().length > 0 && elecVolume > 0 ? elecVolume : 0,
        };

        //冷媒設備
        var refrigerantInputs = [];
        $.each($(".refrigerant-row"), function (index, item) {
            var UseVolume = $(item).find('input[name="refrigeVolume"').val();
            var RefrigerantEquip = $(item).find('select[name="quipType"').val();
            var RefrigerantType = $(item).find('select[name="refrigeType"').val();

            if (UseVolume != null && UseVolume.trim().length > 0 && UseVolume.trim()
                && RefrigerantEquip && RefrigerantEquip != "null"
                && RefrigerantType && RefrigerantType != "null")
                refrigerantInputs.push({
                    RefrigerantEquip: RefrigerantEquip,
                    RefrigerantType: RefrigerantType,
                    UseVolume: UseVolume
                });
        });
        m["refrigerantInputs"] = refrigerantInputs;

        // 逸散氣體
        var escapeInputs = [];
        $.each($(".escape-row"), function (index, item) {
            var UseVolume = $(item).find('input[name="escpaeVolume"]').val();
            var EscapeId = $(item).find('select[name="escpaeItem"]').val();
            var EscapeType = $(item).find('select[name="escapeType"]').val();
            if (UseVolume != null && UseVolume.trim().length > 0 && UseVolume > 0
                && EscapeId && EscapeId != "null"
                && EscapeType && EscapeType != "null"
            )
                escapeInputs.push({
                    EscapeId: EscapeId,
                    EscapeType: EscapeType,
                    UseVolume: UseVolume,
                });
        })
        m["escapeInputs"] = escapeInputs;

        // 蒸氣計算
        var steamCoe = $("#evap-coe").val();
        var steamVolume = $("#evap-volume").val();
        m["steamInput"] = {
            SteamCoe: steamCoe != null && steamCoe.trim().length > 0 && steamCoe > 0 ? steamCoe : 0,
            SteamVolume: steamVolume != null && steamVolume.trim().length > 0 && steamVolume > 0 ? steamVolume : 0,
        };

        // 特殊製程
        var specialInputs = [];
        $.each($(".advance-row"), function (index, item) {
            var UseVolume = $(item).find('input[name="advanceVolume"]').val();
            var CreateId = $(item).find('select[name="advanceItem"]').val();
            var CreateType = $(item).find('select[name="advanceType"]').val();

            if (UseVolume != null && UseVolume.trim().length > 0 && UseVolume > 0
                && CreateId && CreateId != "null"
                && CreateType && CreateType != "null"
            )
                specialInputs.push({
                    UseVolume: UseVolume,
                    CreateId: CreateId,
                    CreateType: CreateType
                })
        })
        m["specialInputs"] = specialInputs;

        // 範疇三
        $.each($(".type3-block input"), function (index, item) {
            var $_this = $(item);
            m[$_this.attr("name")] = $_this.val() ? $_this.val() : 0;
        });

        return m;
    }

    //結果視窗關閉
    $('.count-result-block .close-btn').on('click', function () {
        //helper.misc.tableToExcel($('.count-result-block table').first(), ['e1', 'e2']);
        $('body').removeClass('show-cal');
    });

    //小提醒
    helper.jspanel.jspAlertMsg(undefined, {
        classes: 'reminder-info modal-lg', title: '使用前的貼心提醒', content: '<ol><li>請注意輸入的<span class="import-remind">用量單位</span>(燃料、電力、冷媒用量)。</li>' +
            '<li>本計算工具僅供自行檢查溫室氣體排放量。<span class="import-remind">如需通過排放查證和盤查登錄要求</span>，須依照 ISO 相關規範和環保署的作業指引。</li>' +
            '<li>本計算工具所獲得的相關資料(一般或技術、商業資料) ，負有<span class="import-remind">保密責任</span>。</li>' +
            '<li>本計算工具所提供的相關技術資訊(含產品、技術或服務) ，在未經正式授權下，<span class="import-remind">不得任意擴散、複製、抄襲、引用</span>。</li>' +
            '<li>本計算工具所使用相關排放係數、GWP值、熱值與逸散率因子皆是引用IPCC 2006年數據、AR4報告與環保署公告之溫室氣體排放係數管理表6.0.4版。</li></ol>'
        , autoclose: Number.MAX_VALUE
    });
    $('.fuel-alert-info').on('click', function () {
        helper.jspanel.jspAlertMsg(undefined, {
            classes: 'reminder-info p-alert-info', title: '燃料計算小提醒', content: '<ol><li>只要是公司內的作業含括標準燃料燃燒的項目，就該進行查核。申報單位的組織邊界內的所有石化燃料的溫室氣體排放量。</li>' +
                '<li><span class="import-remind">*計算之方法為排放係數法：『石化燃料使用量』乘以『石化燃料的溫室氣體排放係數』乘以『溫室氣體種類的GWP值』，就可得出以二氧化碳當量公噸為單位的溫室氣體排放量。</span></li></ol>'
            , autoclose: Number.MAX_VALUE
        });
    });
    $('.fuel-fix-alert-info').on('click', function () {
        helper.jspanel.jspAlertMsg(undefined, {
            classes: 'reminder-info p-alert-info', title: '燃料計算(固定源)小提醒', content: '<ol><li>在此提到的排放源包括鍋爐、加熱器、燃燒爐、窯爐、烘爐、烘乾機與其他使用燃料的設備或機器設備。</li></ol>'
            , autoclose: Number.MAX_VALUE
        });
    });
    $('.fuel-move-alert-info').on('click', function () {
        helper.jspanel.jspAlertMsg(undefined, {
            classes: 'reminder-info p-alert-info', title: '燃料計算(移動源)小提醒', content: '<ol><li>在此提到的移動源有：公路交通、鐵路交通、航空交通、水上交通。另也包含非陸路機動車與設備，如起重機、堆高機、破碎機、割草機、挖土機、鏈鋸等機器。</li></ol>'
            , autoclose: Number.MAX_VALUE
        });
    });
    $('.elec-alert-info').on('click', function () {
        helper.jspanel.jspAlertMsg(undefined, {
            classes: 'reminder-info p-alert-info', title: '電力計算小提醒', content: '<ol><li>在此提到的移動源有：公路交通、鐵路交通、航空交通、水上交通。另也包含非陸路機動車與設備，如起重機、堆高機、破碎機、割草機、挖土機、鏈鋸等機器。</li></ol>'
            , autoclose: Number.MAX_VALUE
        });
    });
    $('.ref-alert-info').on('click', function () {
        helper.jspanel.jspAlertMsg(undefined, {
            classes: 'reminder-info p-alert-info', title: '燃料計算小提醒', content: '<ol><li>工廠使用的空調與冷凍設備因冷媒外洩而須補充時，就會造成氟氯化合物氣體的逸散，由於這些物質的全球暖化潛勢經常是CO2的數千倍，成為工廠溫室氣體盤查時不可忽略的項目。</li>' +
                '<li>一般而言，冷媒之逸散量無法直接量取獲得，加上工廠之空調及冷凍設備多為委外維修，而維修商僅負責將冷媒填滿而無任何相關紀錄時，此時就會採取排放因子估算法。</li>' +
                '<li>計算方式為設備的『冷媒原始填充量』乘以設備的『逸散排放因子』乘以使用的『冷媒種類GWP值』。</li>' +
                '<li><span class="import-remind">*冷媒原始填充量為冷凍冷藏設備之設備銘牌上所標示的冷媒填充量。</span></li></ol>'
            , autoclose: Number.MAX_VALUE
        });
    });
    window.linkmessage = '登出中.';
    $('.login-info a').on('click', function (w, ee) {
        helper.misc.showBusyIndicator(undefined, { content: window.linkmessage ? window.linkmessage : undefined });
    });
});