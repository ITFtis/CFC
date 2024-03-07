

$(document).ready(function () {
    //$('.show-more-btn').on('click', function () {
    //    $(this).toggleClass("close");//.parent().find('rule-set')
    //});

    ////////////////////////////
        $('.btn-cal').on('click', function () {
            /*$("form")[0].submit();*/
            var m = {};

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

            // 電力部分
            m["electInput"] = {
                elecYear: $("#elecYear").val(),
                elevVolume: $("#elecVolume").val()
            };

            //冷媒設備
            var refrigerantInputs = [];
            $.each($(".refrigerant-row"), function (index, row) {
                refrigerantInputs.push({
                    RefrigerantEquip: $(item).find('select[name*="quipType"').val(),
                    RefrigerantType: $(item).find('select[name*="quipType"').val(),
                    UseVolume: $(item).find('input[name*="refrigeVolume"').val()
                });   
            });
            m["refrigerantInputs"] = refrigerantInputs;

            // 逸散氣體
            var escapeInputs = [];
            $.each($(".escape-row"), function (index, item) {
                escapeInputs.push({
                    EscapeId: $(item).find('select[name*="escpaeItem"]').val(),
                    UseVolume: $(item).find('input[name*="escpaeVolume"]').val(),
                });
            })
            m["escapeInputs"] = escapeInputs;

            // 蒸氣計算
            m["steamInput"] = {
                SteamCoe: $("#evap-coe").val(),
                SteamVolume: $("#evap-volume").val()
            };

            // 特殊製程
            var specialInputs = [];
            $.each($(".advance-row"), function (index, item) {
                specialInputs.push({
                    UseVolume: $(item).find('input[name*="advanceVolume"]'),
                    CreateId: $(item).find('select[name*="advanceItem"]')
                })
            })
            m["specialInputs"] = specialInputs;

            console.log(m);
            $('body').addClass('show-cal');
            //$('.cal-result').text(JSON.stringify(m));

            $.ajax({
                //url: '@(Url.Content("~/api/cfc/cal"))',
                url: site_root +'api/cfc/cal',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(m),
                method: 'POST'
            }).done(function (r) {
                var $_result = $('.cal-result').removeClass('err-message');
                $('.cal-result table td.d-content> label[dn^="d-R"]').text('--');
                if (r.Success) {
                    for (var f in r.Result) {
                        $_result.find('label[name="' + f + '"]').text(helper.format.formatNumber(parseFloat(r.Result[f]), 2));
                    }
                } else {
                    $('.cal-result').addClass('err-message').find('.reult-message').text(r.Message);
                }
            }).always(function (_data, _textStatus) {
            });

            //console.log(m);
        });
        $('.btn-cancel').on('click', function () {
        $.each($('.form-group input,.form-group select'), function () {
            $(this).val($(this).attr('data-dval'));
        });
        });
        $('.btn-return').on('click', function () {
            $('body').removeClass('show-cal');
        });
        $('.btn-view').on('click', function () {
            var $_btn = $(this);
            setTimeout(function () {
                var $_mainacc = $('#accordion-main-container').removeClass('view');
                $_mainacc.find('.view').removeClass('view');
                $_mainacc.find('.card-header button').removeAttr('disabled');
                if ($_btn.hasClass('active')) {
                   
                    var _flag = false;
                    $.each($(' .form-group input', $_mainacc), function () {
                        var $_this = $(this);
                        //console.log($_this.attr('data-dval') + '   ' + $_this.val())
                        if ($_this.attr('data-dval') != $_this.val()) {
                            $_this.closest('.form-group').addClass('view').parents('.card').addClass('view');
                            $_this.parents('.card-body').parent().addClass('view');
                            _flag = true;
                        }
                    });
                    if (_flag) {
                        $_mainacc.addClass('view');
                        $_mainacc.find('.card-header button').attr('disabled', 'disabled');
                    }
                    else {
                        $_btn.removeClass('active');
                        helper.jspanel.jspAlertMsg(undefined, { content: '尚未輸入任何資料，無法檢視!!', classes: 'modal-sm' });
                    }
                }
            });
            //var htmls = '';
            //console.log("$('.body-content > form input'):" + $('.body-content > form input').length)
            //$.each($('.body-content > form input'), function () {
            //    var $_this = $(this);
            //    console.log($_this.attr('data-dval') +'   '+ $_this.val())
            //    if ($_this.attr('data-dval') != $_this.val()) {
            //        htmls += '<div class="form-group-view">' + $_this.closest('.form-group')[0].outerHTML +'<div class="btn  btn-outline-info glyphicon glyphicon-pencil"></div></div>';
            //    }
            //});
            //if (htmls) {
            //    var $_m = helper.jspanel.jspAlertMsg(undefined, { autoclose: 9999999, title: '輸入資料', content: htmls }).addClass('modal-view');
            //    $_m.find('input, select').prop('disabled', true);
            //}
            //else
            //    helper.jspanel.jspAlertMsg(undefined, { content: '尚未輸入任何資料!!',classes: 'modal-sm' });
        });
        helper.jspanel.jspAlertMsg(undefined, {
            title: '使用前的貼心提醒', content: '<ol"><li>請注意輸入單位!!1.請注意輸入時的燃料、電力、冷媒<span class="import-remind">用量單位！</span></li>'+
                                                '<li>本計算工具僅供自行檢查溫室氣體排放量。<span class="import-remind">如需通過排放查證和盤查登錄要求</span>，須依照 ISO 相關規範和環保署的作業指引。</li>'+
                                                '<li>本計算工具所獲得的相關資料(一般或技術、商業資料) ，負有<span class="import-remind">保密責任</span>。</li>'+
                                                '<li>本計算工具所提供的相關技術資訊(含產品、技術或服務) ，在未經正式授權下，<span class="import-remind">不得任意擴散、複製、抄襲、引用</span>。</li>'+
                                                '<li>本計算工具所使用相關排放係數、逸散率因子皆是引用IPCC 2006年數據與環保署公告之溫室氣體排放係數管理表6.0.4版。</li></ol>'
                , autoclose:5000 });
});


