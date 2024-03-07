if (!$.BasePinCtrl) {
    alert("未引用(PolygonCtrl)BasePinCtrl");
}

(function ($) {

    $.PolygonCtrl = {

        defaultSettings: {
            name: "PolygonCtrl",
            layerid: "PolygonCtrl_", map: undefined,

            stTitle: function (data) { return data[0] },
            useLabel: true,
            useList: true,
            useCardview: true,
            useSearch: true,
            useSilder:false,
            polyStyles: [{ name: '圖例', strokeColor: '#FF0000', strokeOpacity: 1, strokeWeight: 1, fillColor: '#FF0000', fillOpacity: .7, classes: 'water_normal' }],
            legendIcons: [],
            infoFields: [],
            checkDataStatus: function (data, index) {return  this.settings.polyStyles.length > 0 ? this.settings.polyStyles[0] : $.PolygonCtrl.defaultSettings.polyStyles[0];},
            transformData: function (_base, _info) { return (_base && _base.length > 0 ? _base : _info); },
            pinInfoContent: function (data, infofields) { return $.BasePinCtrl.defaultSettings.pinInfoContent.call(this, data, infofields); },
            loadBase: undefined,
            loadInfo: undefined,
            type: $.BasePinCtrl.type.polygon
        },
        defaultData: { CName: '', Datetime: undefined, WaterLevel: -998, Voltage: undefined, X: 0, Y: 0 },
        eventKeys: { opacity_change: "opacity_change" }
    }
    var pluginName = 'PolygonCtrl'
    var pluginclass = function (element, e) {
        if (e) {
            e.stopPropagation();
            e.preventDefault();
        }
        this.$element = $(element);
        this.settings = $.extend({}, $.PolygonCtrl.defaultSettings);// {map:undefined, width:240};
        //this.settings.legendIcons = this.settings.polyStyles;

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
            if (!this.settings.legendIcons || this.settings.legendIcons.length==0)
                this.settings.legendIcons = this.settings.polyStyles;
            var current = this;
            //this.__pinctrl = $("#" + this.$element[0].id).BasePinCtrl(this.settings).on($.BasePinCtrl.eventKeys.initLayer, function (ss) {
            this.__pinctrl = this.$element.BasePinCtrl(this.settings).on($.BasePinCtrl.eventKeys.initLayer, function (ss) {
                current.isInitCompleted = true;
                current.reload(current.currentDatetime);
            });
            this.$element.on($.BasePinCtrl.eventKeys.displayLayer, function (evt, _display) {
                current.__initOpacitySlider(_display);
            });
        },
        reload: function (dt) { //觸發loadBase , loadInfo
            var current = this;
            this.currentDatetime = dt;
            if (!this.isInitCompleted)
                return;
            $.BasePinCtrl.helper.reload.call(this, dt);
        },
        setFilter: function (filter) {
            this.__pinctrl.instance.setFilter(filter);
        },
        setOpacity: function (_opacity) {
            this.__pinctrl.instance.setOpacity(_opacity);
            this.$element.trigger($.PolygonCtrl.eventKeys.opacity_change, [_opacity]);
        },
        fitBounds: function () {
            this.__pinctrl.instance.fitBounds();
        },
        //setBoundary: function (inBoundary) {
        //    this.__pinctrl.instance.setBoundary(inBoundary);
        //},
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
        },
        setData: function (datas) {
            this.__pinctrl.instance.setData(datas);
        },
        __initOpacitySlider: function (_display) {
            if (!this.settings.useSilder)
                return;
            var current = this;
           
            var $slider = this.$element.find('.opacity-slider').first();
            if ($slider.length == 0) {
                $slider = $('<div class="col-xs-12"><div class="opacity-slider" title="透明度"></div></div>').appendTo(this.$element).find('.opacity-slider')
                    .gis_layer_opacity_slider({
                        map:this.settings.map,
                        range: "min",
                        max: 100,
                        min: 5,
                        value: 90,
                        setOpacity: $.proxy(current.setOpacity, current)// function (_op) { current.setOpacity(_op); }
                    });
            }
            if (_display)
                $slider.show();
            else
                $slider.hide();
        }
    }


    $.fn[pluginName] = function (arg) {

        var args, instance;

        if (!(this.data(pluginName) instanceof pluginclass)) {

            this.data(pluginName, new pluginclass(this[0]));
        }

        instance = this.data(pluginName);
        if (!instance)
            console.log("請確認selector(" + this.selector + ")是否有問題");


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