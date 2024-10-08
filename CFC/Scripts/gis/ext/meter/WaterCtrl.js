﻿//$(document).ready(function () {
//setTimeout(function () {

//    }, 1000);
//});
if (!$.BasePinCtrl) {
    alert("未引用(WaterCtrl)BasePinCtrl");
}

(function ($) {
    var watersource = {
        basefunc: function (callback) {
            $.BasePinCtrl.helper.getWraFhyApi('WaterLevel/Station', null, function (ds) {
                var datas = $.map(ds.Data, function (val, idx) {
                    if (!val.Point) { console.log("水位基本資料" + val.StationName + "無座標"); }
                    return $.extend({ Station: val}, {
                        ST_NO: val.StationNo, NAME_C: val.StationName, BASIN_NO: val.BasinCode, COUN_ID: val.CityCode, ADDR_C: val.Address, TopLine: val.TopLevel,
                        WarningLine1: val.WarningLevel1, WarningLine2: val.WarningLevel2, WarningLine3: val.WarningLevel3, Long: val.Point.Longitude, Lat: val.Point.Latitude
                    });
                });
                callback(datas);
            });
            //抓meter
            //$.BasePinCtrl.helper.getWraMeterData('WaterStations', undefined, 'ArrayOfWaterStationBase.WaterStationBase', undefined, function (ds) {
            //    callback( $.grep(ds, function (d) { return d.Exist==1 }));
            //});
        },
        infofunc: function (dt, callback) {
            $.BasePinCtrl.helper.getWraFhyApi('WaterLevel/Info/RealTime', null, function (ds) {
                var datas = $.map(ds.Data, function (val, idx) {
                    var r = {Station:val, ST_NO: val.StationNo, DATE: val.Time, Info: val.WaterLevel};//, WaterWarnInfo: $.BasePinCtrl.pinIcons.water.normal.name };
                    return r;
                });
                callback(datas);
            });
            //$.BasePinCtrl.helper.getWraMeterData('wsCwbNewestWater', undefined, 'ArrayOfNewestWater.NewestWater', undefined, callback);
        },
        hourlyInfofunc: function (data, callback) {
            if (!data["Datetime"] )
                callback([]);
            else {
                var startdt = new Date(data["Datetime"]).addHours(-24).DateFormat('yyyy/MM/dd HH:mm:ss');
                var enddt = new Date(data["Datetime"]).DateFormat('yyyy/MM/dd HH:mm:ss');

                $.BasePinCtrl.helper.getWraFhyApi('WaterLevel/Info/Last24Hours/StationNo/' + data.ST_NO, null, function (ds) {
                    var datas = [];
                    if (ds.Data && ds.Data.length > 0 && ds.Data[0].Hours) {
                        var bdt = helper.format.JsonDateStr2Datetime(ds.Data[0].Time);
                        var datas = $.map(ds.Data[0].Hours, function (val, idx) {
                            var dt = new Date(bdt.getFullYear(), bdt.getMonth(), bdt.getDate(), val);
                            if (dt > bdt) dt = dt.addHours(-24);
                            var d = { ST_NO: data.ST_NO, DATE: dt, Info: ds.Data[0].WaterLevels[idx] };
                            return d;
                        });
                    }
                    datas.sort(function (a, b) { return a.DATE - b.DATE });
                    callback(datas);
                });
                //抓meter
                //$.BasePinCtrl.helper.getWraMeterData('WaterHistoryStage', { 'stationNo': data["StationID"], 'startDate': startdt, 'endDate': enddt }, 'ArrayOfNewestWater.NewestWater', undefined, function (ds) {
                //    ds.sort(function (a, b) {
                //        return JsonDateStr2Datetime(a.DATE).getTime() - JsonDateStr2Datetime(b.DATE).getTime();
                //    });
                //    callback(ds);
                //});
            }
        }
    };

    $.waterFormatter = {
        float: function (value, row) {
            if (value ===undefined || value ===null || value ==="" || parseFloat(value) < -99)
                return '---'
            else return parseFloat(value).toFixed(2);
        },
        datetime: function (value, row, source) {
            if (value && value.getMonth) //row物件是清單, row物件是string==InfoWindow
                return (source === undefined || source != "InfoWindow") ? value.DateFormat("dd HH:mm") : value.DateFormat("yyyy/MM/dd HH:mm");
            else
                return '---';
        }
    }
    $.WaterCtrl = {

        defaultSettings: {
            name: "水位站",
            layerid: "water_", map: undefined,
            stTitle: function (data) { return data.CName },
            loadBase: "https://140.124.60.39/Map/WS/Meter.asmx/WaterStations",
            loadInfo: "https://140.124.60.39/Map/WS/Meter.asmx/wsCwbNewestWater",
            loadHourlyInfo: "https://140.124.60.39/Map/WS/Meter.asmx/WaterHistoryStage",
            loadBase: watersource.basefunc,
            loadInfo: watersource.infofunc,
            loadHourlyInfo: watersource.hourlyInfofunc,
            hourlyFieldsInfo: { DateTime: "DATE", WaterLevel: "Info" },
            useTimeSeriesData :true,
            infoFields: [
                { field: 'CName', title: '站名' }, { field: 'Datetime', title: '時間', formatter: $.waterFormatter.datetime },
                { field: 'WaterLevel', title: '水位', halign:'center', align: 'right', formatter: $.waterFormatter.float, unit: "公尺", sortable: true },
                { field: 'TopLine', title: '堤頂', visible: false, halign: 'center', align: 'right', formatter: $.waterFormatter.float, unit: "公尺", showInList: false }, { field: 'WarningLine1', title: '一級', visible: false, formatter: $.waterFormatter.float, unit: "公尺", showInList: false },
                { field: 'WarningLine2', title: '二級', visible: false, halign: 'center', align: 'right', formatter: $.waterFormatter.float, unit: "公尺", showInList: false }, { field: 'WarningLine3', title: '三級', visible: false, formatter: $.waterFormatter.float, unit: "公尺", showInList: false },
                ],
            legendIcons: [$.BasePinCtrl.pinIcons.water.normal, $.BasePinCtrl.pinIcons.water.warnLevel1, $.BasePinCtrl.pinIcons.water.warnLevel2, $.BasePinCtrl.pinIcons.water.warnLevel3, $.BasePinCtrl.pinIcons.water.noData],
            checkDataStatus: function (data, index) {
                var stas = '無資料';
                if (data.Status)
                    stas = data.Status;
               
                return $.BasePinCtrl.helper.getDataStatusLegendIcon(this.settings.legendIcons, stas);
                //return stas;
            },
            transformData: function (_base, _info) {
                var datas = [];
                $.each(_base, function (idxb, b) {
                    //var d = $.extend({}, $.WaterCtrl.defaultData);
                    var d = $.extend(JSON.parse(JSON.stringify(b)), $.WaterCtrl.defaultData);
                    d.CName = b.NAME_C;
                    d.StationID = b.ST_NO;
                    d.TopLine = b.TopLine;
                    d.WarningLine1 = b.WarningLine1;
                    d.WarningLine2 = b.WarningLine2;
                    d.WarningLine3 = b.WarningLine3;
                    d.X = b.Long;
                    d.Y = b.Lat;
                    d.Status = $.BasePinCtrl.pinIcons.water.noData.name;
                    d.Datetime = undefined;
                    $.each(_info, function (idxi, i) {
                        if (d.StationID == i.ST_NO) {
                            d.Datetime = JsonDateStr2Datetime(i.DATE);
                            d.WaterLevel = i.Info;
                            //d.Voltage = i.Voltage;
                            d.Status = $.BasePinCtrl.pinIcons.water.normal.name;
                            if (d.WarningLine1 != undefined && d.WaterLevel >= d.WarningLine1)
                                d.Status = $.BasePinCtrl.pinIcons.water.warnLevel1.name;
                            else if (d.WarningLine2 != undefined && d.WaterLevel >= d.WarningLine2)
                                d.Status = $.BasePinCtrl.pinIcons.water.warnLevel2.name;
                            else if (d.WarningLine3 != undefined && d.WaterLevel >= d.WarningLine3)
                                d.Status = $.BasePinCtrl.pinIcons.water.warnLevel3.name;
                            return false;
                        }
                    });
                    datas.push(d);
                });
                //console.log(JSON.stringify(datas));
                return datas;
            },
            pinInfoContent: function (data, infofields) {
                var current = this;
                var currentsetting = this.settings;
                var chartOptionsTemp, $chartdiv;
                infofields = this.infoFields;
                var constr = $.BasePinCtrl.defaultSettings.pinInfoContent.call(this, data);
                var iid = geguid();// sid + "_" + new Date().getTime();

                /*************/
                //carousel
                var $div = $('<div id="carousel_' + iid + '" class="carousel slide meterinfo" data-ride="carousel" data-interval="99999999" style="width:100%;">');// style="width:' + this.cctvSize.width + 'px;height:' + this.cctvSize.height + 'px">');
                var $ol = $('<ol class="carousel-indicators">');
                var $sdiv = $('<div class="carousel-inner">');

                $ol.append(' <li data-target="#carousel_' + iid + '" data-slide-to="0"  class="active"></li>');
                $ol.append(' <li data-target="#carousel_' + iid + '" data-slide-to="1"  class="active"></li>');

                $sdiv.append('<div class="carousel-item item active" style="min-height:140px;">' + constr + '</div>');
                $sdiv.append('<div class="carousel-item item" style="min-height:140px;"><div id="chart_' + iid + '" style="min-height:140px;min-width:1px;width:100%; "></div><div>');
                
                $div.append($ol);
                $div.append($sdiv);

                $div.append('<a class="left carousel-control carousel-control-prev" href="#carousel_' + iid + '" role="button" data-slide="prev">' +
                        '<span class="glyphicon glyphicon-chevron-left"></span>' +
                    '</a>' +
                    '<a class="right carousel-control carousel-control-next" href="#carousel_' + iid + '" role="button" data-slide="next">' +
                        '<span class="glyphicon glyphicon-chevron-right"></span>' +
                    '</a>');
                if (!this.settings.useTimeSeriesData)
                    $div.find('.carousel-control').hide();
                $ol.hide();

                //chart
                setTimeout(function(){
                    $("#carousel_" + iid).on("slide.bs.carousel", function () {
                        if (!chartOptionsTemp) {
                            chartOptionsTemp = {
                                //tooltip: {
                                //    show:true,
                                tooltip: true,
                                tooltipOpts: {
                                    content: function (x, y, z, o, we, wfe) {
                                        var d = new Date(y); d.setHours(d.getHours() - 8);
                                        var v = $.waterFormatter.float((z - (o.series.shift ? o.series.shift : 0)), null);
                                        if (!x) //即時水位area
                                            return "即時水位:" + v + "m";
                                        else if ("水位" == x || o.series.isTimeseries)
                                            return "時間:" + (d.getMonth() + 1) + "/" + d.getDate() + " " + d.getHours() + "<br>"+o.series.label+":" + v + "m"
                                        else
                                            return x + "(" + v + "m)";
                                    },//"%p.0%, %s", // show percentages, rounding to 2 decimal places
                                    shifts: {
                                        x: 20,
                                        y: 0
                                    },
                                    defaultTheme: true,
                                },
                                series: { //每一data皆會show此設定的的type
                                    shadowSize: 0
                                },
                                xaxis: {
                                    mode: "time",
                                    tickSize: [3, "hour"],
                                    timeformat: "%h",
                                },
                                yaxis: {
                                    axisLabelUseCanvas: true,
                                    axisLabelFontSizePixels: 12,
                                    axisLabelFontFamily: 'Verdana, Arial',
                                    axisLabelPadding: -3,
                                },
                                legend: {
                                    show: false,
                                    noColumns: 0,
                                    ticks: [],
                                    labelBoxBorderColor: "#000000",
                                    backgroundOpacity: 0.1,
                                    position: "nw"
                                },
                                grid: {
                                    hoverable: true,
                                    clickable: true,
                                    borderWidth: 2,
                                    borderColor: "#AAAAAA",
                                    backgroundColor: { colors: ["#F6F6F6", "#E4E4E4"] },
                                    //backgroundColor: { colors: ["#ffffff", "#EDF5FF"] }
                                },
                                spreadsheet: {
                                    show: true
                                }

                            };
                            if (!$chartdiv)
                                $chartdiv = $("#chart_" + iid, this);
                            //if($.plot)
                            //    $.plot($chartdiv, [], chartOptions);
                        }
                    });

                    var _displayTimeFlag;
                    $("#carousel_" + iid).on('slid.bs.carousel', function (df, rwr, qwe) {
                        
                        var activechart = $(".active #chart_" + iid).length > 0;
                        if (!activechart)
                            return;
                        
                        var customoptions = currentsetting.getDurationOptions.call(current, data);

                        var chartOptions = $.extend({}, chartOptionsTemp, customoptions);

                        //GMT+0800 chart 會變成UTC時間，所以先加8在讓chart減，這樣呈現的時間才會正確
                        var st = new Date(customoptions.startdt.setHours(customoptions.startdt.getHours()+8)).DateFormat("yyyy/MM/dd HH:mm:ss");
                        var et = new Date(customoptions.enddt.setHours(customoptions.enddt.getHours() + 8)).DateFormat("yyyy/MM/dd HH:mm:ss");

                        if (st + et == _displayTimeFlag) //同時間區間資料不重Load
                            return;
                        _displayTimeFlag = st + et; 
                        $chartdiv.show_busyIndicator();

                        helper.misc.getJavaScripts([$.AppConfigOptions.script.gispath + "/flot/jquery.flot.min.js"], function () {
                            helper.misc.getJavaScripts([ $.AppConfigOptions.script.gispath + "/flot/jquery.flot.time.js",
                               $.AppConfigOptions.script.gispath + "/flot/jquery.flot.animator.js", $.AppConfigOptions.script.gispath + "/flot/jquery.flot.symbol.js",
                               $.AppConfigOptions.script.gispath + "/flot/jquery.flot.axislabels.js", $.AppConfigOptions.script.gispath + "/flot/jquery.flot.tooltip.js"], function () {
                                   //$.plot($chartdiv, [], chartOptions);
                             

                                   var painChart = function (ddatas) {  //畫chart
                                       console.log("歷線資料" + ddatas.length + "筆");//call chart ajax end"+JSON.stringify(ddatas));
                                       ddatas = ddatas.sort(function (a, b) {
                                           if (a[current.settings.hourlyFieldsInfo.DateTime] != undefined && b[current.settings.hourlyFieldsInfo.DateTime] != undefined)
                                               return JsonDateStr2Datetime(a[current.settings.hourlyFieldsInfo.DateTime]).getTime() - JsonDateStr2Datetime(b[current.settings.hourlyFieldsInfo.DateTime]).getTime();
                                           else return 0;
                                       });
                                       //劃chart
                                       var maxvalue = -9999;
                                       var minvalue = 99999;
                                       var wdatas = [];
                                       $.each(ddatas, function (idx, d) {
                                           //dtd.setHours(dtd.getHours() + 8); //GMT+0800 chart 會變成UTC時間，所以先加8在讓chart減，這樣呈現的時間才會正確
                                           var dtd= JsonDateStr2Datetime(d[current.settings.hourlyFieldsInfo.DateTime]);
                                           if (dtd == undefined)
                                               return;
                                           dtd.setHours(dtd.getHours() + 8); //GMT+0800 chart 會變成UTC時間，所以先加8在讓chart減，這樣呈現的時間才會正確

                                           var v = parseFloat(d[current.settings.hourlyFieldsInfo.WaterLevel]);

                                           var hhh = dtd.getMinutes();
                                           //if (dtd.getMinutes() != 0 || v < -99)
                                           if (currentsetting.filterChartInfo.call(current, d, customoptions))
                                           {
                                               if (v > maxvalue) maxvalue = v;
                                               if (v < minvalue) minvalue = v;
                                               wdatas.push([dtd.getTime(), v]);
                                           }
                                       });
                                       //var tdatas = [[st.getTime(), parseFloat(g.attributes["TopLine"])], [st.getTime(), parseFloat(g.attributes["TopLine"])]];
                                       if (wdatas.length == 0) {
                                           $chartdiv.hide_busyIndicator();
                                            return;
                                        }
                                       var cdatas = [
                                                {
                                               label: customoptions.mainSeriesName ? customoptions.mainSeriesName: '水位',
                                                    data: wdatas,
                                               color: customoptions.mainSeriesColor ? customoptions.mainSeriesColor : '#0000FF',
                                                    showTooltip: true,
                                                    isTimeseries :true,
                                                    lines: { show: true, fill: false },//不用設定，直接用options的series
                                                    points: { show: true },
                                                    //xaxis: 1,
                                                    animator: { start: 0, steps: wdatas.length, duration: wdatas.length * 50 > 2500 ? 2500 : wdatas.length * 50, direction: 'right' }
                                                }];
                                       var paintLeveLine = function (l, label, color, fill, lwidth) {
                                           if (!l || parseFloat(l) <= -99)
                                               return;
                                           var v = parseFloat(l);
                                           if (v > maxvalue) maxvalue = v;
                                           if (v < minvalue) minvalue = v;
                                           var opt = {
                                               label: label,
                                               data: [[new Date(st).getTime(), v], [new Date(et).getTime(), v]],
                                               lines: { show: true, fill: fill, fillColor: color, lineWidth: lwidth },
                                               color: color
                                           };
                                           if (fill)
                                               cdatas.splice(0, 0, opt);
                                           else
                                               cdatas.push(opt);
                                       }
                                       $.each(customoptions.seriespara, function () {
                                           if (this.data) {
                                               cdatas.push(this);  //已由外部定義series
                                               $.grep(this.data, function (vs) {
                                                   var v = parseFloat(vs[1]);
                                                   if (v > maxvalue) maxvalue = v;
                                                   if (v < minvalue) minvalue = v;
                                               });
                                               //this.animator = { start: 0, steps: this.data.length, duration: this.data.length * 50, direction: 'right' }
                                           }
                                           else
                                                paintLeveLine(this.value, this.name, this.color, this.fill, this.width);
                                       });


                                       chartOptions.xaxis.max = new Date(et).getTime();
                                       chartOptions.xaxis.min = new Date(st).getTime();

                                       if (customoptions.miny != undefined)
                                           minvalue = customoptions.miny;

                                       var ra = maxvalue - minvalue;
                                       chartOptions.yaxis.max = maxvalue + ra * 0.1;
                                       chartOptions.yaxis.min = minvalue >= 0 && minvalue - ra * 0.1 <0  ? minvalue : minvalue - ra * 0.1;
                                       var shift = 0;//劃area(fillColor)有<0的值有問題，
                                       if (chartOptions.yaxis.min < 0) {
                                           shift = -(chartOptions.yaxis.min);
                                           minvalue = 0;
                                           maxvalue = maxvalue + shift;
                                           minvalue = minvalue + shift;
                                           chartOptions.yaxis.max = chartOptions.yaxis.max + shift;
                                           chartOptions.yaxis.min = chartOptions.yaxis.min + shift;
                                           $.each(cdatas, function () {
                                               $.each(this.data, function () {
                                                   this[1] = this[1] + shift;
                                               });
                                               this.shift = shift;
                                           });
                                       }

                                       var tmepdatas = JSON.parse(JSON.stringify(cdatas));
                                       var plot = $.plot($("#chart_" + iid), cdatas, chartOptions);
                                       //以下只為了show 一、二、三在label
                                       chartOptions.yaxis.ticks = [];// plot.getAxes().yaxis.ticks;//.push([8, 'sdsds']);'<span style="color:\'#FF0000\';opacity:0.7;">B級</span>'
                                       $.each(plot.getAxes().yaxis.ticks, function (idx, tick) {
                                           chartOptions.yaxis.ticks.push([tick.v, (tick.v - shift).toFixed(((tick.v - shift) + "").indexOf('.')>=0?1:0   )]);
                                       });
                                       $.each(customoptions.seriespara, function () {
                                           if (this.name && this.value >-99) {
                                               chartOptions.yaxis.ticks.push([parseFloat(this.value + shift), '<div style="color:'+this.color+';opacity:0.7;">'+this.name+'</div>']);
                                           }
                                       });

                                       var ctrpPlot = $.plotAnimator($("#chart_" + iid), cdatas, chartOptions);


                                       $("#chart_" + iid + " .tickLabel").css("color", "black");
                                       //setTimeout(function () {
                                       //    var myCanvas = ctrpPlot.getCanvas();
                                       //    var image = myCanvas.toDataURL();
                                       //    image = image.replace("image/png", "image/octet-stream");
                                       //    document.location.href = image
                                       //}, 3000);
                                       $chartdiv.hide_busyIndicator();
                                   };

                                   if (typeof customoptions.getDurationData === 'function')
                                   {
                                       customoptions.getDurationData(data,jQuery.proxy(painChart, current));
                                   }
                                   else if (typeof customoptions.getDurationData === 'object') {
                                       $.ajax(customoptions.getDurationData).done(function (result, status) {
                                           painChart(result.d);
                                       }).fail(function (data, status) {
                                           console.log(data);
                                           $chartdiv.hide_busyIndicator();
                                       });
                                   }

                               });
                          });
                    });
                },300);

                return $div[0].outerHTML;
            },
            chartOption:{

            },
            getDurationOptions: function (data) { //{hourlyFieldsInfo:{DateTime:"DATE", WaterLevel:"Info"},}
                //this指的是 current
                var result = {
                    //hourlyFieldsInfo: {
                    //    DateTime: "DATE",
                    //    WaterLevel: "Info",
                    //},
                    seriespara: {
                        TopLine: { value: data["TopLine"], name: "堤頂", fill: false, width: 2, color: '#000000' },
                        WarningLine1: { value: data["WarningLine1"], name: "一級", fill: false, width: 2, color: '#FF0000' },
                        WarningLine2: { value: data["WarningLine2"], name: "二級", fill: false, width: 2, color: '#FFA500' },
                        WaterLevel: { value: data["WaterLevel"], name: undefined, fill: true, width: 0, color: 'rgba(0,0,255,0.2)' }
                    }
                };

                result.startdt = new Date(data["Datetime"]).addHours(-24);
                result.enddt = new Date(data["Datetime"]);
                result.stationNo = data["StationID"]
                if (typeof this.settings.loadHourlyInfo === "function") {
                    result.getDurationData = this.settings.loadHourlyInfo;
                } else {
                    result.getDurationData = {
                        //url: "https://water.tainan.gov.tw/WebServices/WaterService.asmx/GetHourInfos",
                        url: this.settings.loadHourlyInfo,
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        type: "POST",
                        data: "{ 'stationNo':'" + data["StationID"] + "', 'startDate':'" + result.startdt.DateFormat("yyyy/MM/dd HH:mm:ss") + "', 'endDate':'" + result.enddt.DateFormat("yyyy/MM/dd HH:mm:ss") + "' }"
                    };
                }
                return result;
            },
            filterChartInfo: function (info, customoptions) {
                var dtd = JsonDateStr2Datetime(info[this.settings.hourlyFieldsInfo.DateTime]);
                var v = parseFloat(info[this.settings.hourlyFieldsInfo.WaterLevel]);
                var hhh = dtd.getMinutes();
                if (dtd.getMinutes() != 0 || v < -99)
                    return false;
                else return true;
            }
        },
        defaultData: { CName: '', Datetime: undefined, WaterLevel: -998, Voltage: undefined, X: 0, Y: 0 },
        leaflet: {
            markerCluster_defaultIconCreateFunction: function (markerClusterGroup, mg) { //複寫 for leaflet markerCluster
                var divIcon = markerClusterGroup._defaultIconCreateFunction(mg);
                var _class = $.BasePinCtrl.pinIcons.water.noData.classes;
                if ($.grep(mg.getAllChildMarkers(), function (r) {return r.pinstatus.classes == $.BasePinCtrl.pinIcons.water.warnLevel1.classes}).length > 0)
                    _class = $.BasePinCtrl.pinIcons.water.warnLevel1.classes;
                else if ($.grep(mg.getAllChildMarkers(), function (r) { return r.pinstatus.classes == $.BasePinCtrl.pinIcons.water.warnLevel2.classes }).length > 0)
                    _class = $.BasePinCtrl.pinIcons.water.warnLevel2.classes;
                else if ($.grep(mg.getAllChildMarkers(), function (r) { return r.pinstatus.classes == $.BasePinCtrl.pinIcons.water.warnLevel3.classes }).length > 0)
                    _class = $.BasePinCtrl.pinIcons.water.warnLevel3.classes;
                else if ($.grep(mg.getAllChildMarkers(), function (r) { return r.pinstatus.classes == $.BasePinCtrl.pinIcons.water.normal.classes }).length > 0)
                    _class = $.BasePinCtrl.pinIcons.water.normal.classes;
                divIcon.options.className += " water " + _class;
                return divIcon;
            }
        }
    }
    var pluginName = 'WaterCtrl'
    var pluginclass = function (element, e) {
        if (e) {
            e.stopPropagation();
            e.preventDefault();
        }
        this.$element = $(element);
        this.settings = $.extend({}, $.WaterCtrl.defaultSettings);// {map:undefined, width:240};
        this.__pinctrl = undefined;
        this.__baseData = undefined;
        this.__infoData = undefined;
        this.currentDatetime = new Date();
        this.isInitCompleted = false;
    };
    pluginclass.prototype = {
        constructor: pluginclass,
        init: function (options) {
            $.extend(this.settings, options);
            var current = this;
            this.__pinctrl = this.$element.BasePinCtrl(this.settings).on($.BasePinCtrl.eventKeys.initLayer, function (ss) {
                current.isInitCompleted = true;
                current.__pinctrl.instance._mapctrl._defaultIconCreateFunction = $.WaterCtrl.leaflet.markerCluster_defaultIconCreateFunction; //複寫 for leaflet markerCluster
                current.reload(current.currentDatetime);
            });

        },
        reload: function (dt) {
            this.currentDatetime = this.__pinctrl.instance.currentDatetime = dt;
            if (!this.isInitCompleted)
                return;
            $.BasePinCtrl.helper.reload.call(this, dt);
        },
        setFilter: function (filter) {
            this.__pinctrl.instance.setFilter(filter);
        },
        fitBounds: function () {
            this.__pinctrl.instance.fitBounds();
        },
        setBoundary: function (inBoundary) {
            this.__pinctrl.instance.setBoundary(inBoundary);
        },
        __loadBaseCompleted: function (results) {
            this.__baseData = results;
            this.refreshData();
        },
        __loadInfoCompleted: function (results) {
            this.__infoData = results;
            this.refreshData();
        },
        refreshData: function () {
            var current = this;
            if (this.__baseData && this.__infoData) {
                current.__pinctrl.instance.setData(this.settings.transformData(this.__baseData, this.__infoData));
            }
        }
    }


    $.fn[pluginName] = function (arg) {

        var args, instance;

        if (!(this.data(pluginName) instanceof pluginclass)) {

            this.data(pluginName, new pluginclass(this[0]));
        }

        instance = this.data(pluginName);


        if (typeof arg === 'undefined' || typeof arg === 'object') {

            if (typeof instance.init === 'function') {
                instance.init(arg);
            }
            this.instance = instance;
            return this;

        } else if (typeof arg === 'string' && typeof instance[arg] === 'function') {

            args = Array.prototype.slice.call(arguments, 1);

            return instance[arg].apply(instance, args);

        } else {

            $.error('Method ' + arg + ' does not exist on jQuery.' + pluginName);

        }
    };
})(jQuery);