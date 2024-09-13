using CFC.Models;
using CFC.Models.Prj;
using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFC.Controllers.PrjNew
{
    [Dou.Misc.Attr.MenuDef(Id = "Cv", Name = "係數管理", MenuPath = "計算", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class CvController : AGenericModelController<object>
    {
        // GET: Cv
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<object> GetModelEntity()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// (類別一)燃料計算
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetTabFuelList()
        {
            Dou.Models.DB.IModelEntity<Fuel_properties> model = new Dou.Models.DB.ModelEntity<Fuel_properties>(new DouModelContext());

            var opts = Dou.Misc.DataManagerScriptHelper.GetDataManagerOptions<Fuel_properties>();

            foreach (var field in opts.fields)
                field.visible = false;

            opts.GetFiled("Id").visible = true;
            opts.GetFiled("FuelType").visible = true;
            opts.GetFiled("Name").visible = true;
            opts.GetFiled("Unit").visible = true;
            opts.GetFiled("CO2").visible = true;
            opts.GetFiled("CH4").visible = true;
            opts.GetFiled("NO2").visible = true;
            opts.datas = model.GetAll();

            var jstr = JsonConvert.SerializeObject(opts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }

        /// <summary>
        /// (類別一)冷媒設備
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetTabRefrigerantEquipList()
        {
            Dou.Models.DB.IModelEntity<Refrigerant_equip> model = new Dou.Models.DB.ModelEntity<Refrigerant_equip>(new DouModelContext());

            var opts = Dou.Misc.DataManagerScriptHelper.GetDataManagerOptions<Refrigerant_equip>();

            foreach (var field in opts.fields)
                field.visible = false;

            opts.GetFiled("Id").visible = true;
            opts.GetFiled("Name").visible = true;
            opts.GetFiled("EscapeRate").visible = true;
            opts.GetFiled("MinValue").visible = true;
            opts.GetFiled("MaxValue").visible = true;
            opts.GetFiled("displayOrder").visible = true;
            opts.datas = model.GetAll();

            var jstr = JsonConvert.SerializeObject(opts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }

        /// <summary>
        /// (類別一)逸散種類
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetTabEscapeTypeList()
        {
            Dou.Models.DB.IModelEntity<Escape_type> model = new Dou.Models.DB.ModelEntity<Escape_type>(new DouModelContext());

            var opts = Dou.Misc.DataManagerScriptHelper.GetDataManagerOptions<Escape_type>();

            foreach (var field in opts.fields)
                field.visible = false;

            opts.GetFiled("Id").visible = true;
            opts.GetFiled("Name").visible = true;
            opts.GetFiled("displayOrder").visible = true;
            opts.datas = model.GetAll();

            var jstr = JsonConvert.SerializeObject(opts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }

        /// <summary>
        /// (類別一)逸散氣體
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetTabEscapePropertiesList()
        {
            Dou.Models.DB.IModelEntity<Escape_properties> model = new Dou.Models.DB.ModelEntity<Escape_properties>(new DouModelContext());

            var opts = Dou.Misc.DataManagerScriptHelper.GetDataManagerOptions<Escape_properties>();

            foreach (var field in opts.fields)
                field.visible = false;

            opts.GetFiled("Id").visible = true;
            opts.GetFiled("Type").visible = true;
            //opts.GetFiled("TypeName").visible = true;
            opts.GetFiled("Name").visible = true;
            opts.GetFiled("Unit").visible = true;
            opts.GetFiled("Co2e").visible = true;
            opts.GetFiled("CO2").visible = true;
            opts.GetFiled("CH4").visible = true;
            opts.GetFiled("N2O").visible = true;
            opts.GetFiled("HFCs").visible = true;
            opts.GetFiled("PFCs").visible = true;
            opts.GetFiled("SF6").visible = true;
            opts.GetFiled("NF3").visible = true;
            opts.GetFiled("displayOrder").visible = true;
            opts.datas = model.GetAll();

            var jstr = JsonConvert.SerializeObject(opts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }

        /// <summary>
        /// (類別二)電力計算
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetTabElecList()
        {
            Dou.Models.DB.IModelEntity<Elec_properties> model = new Dou.Models.DB.ModelEntity<Elec_properties>(new DouModelContext());

            var opts = Dou.Misc.DataManagerScriptHelper.GetDataManagerOptions<Elec_properties>();

            ////全部欄位排序
            //foreach (var field in opts.fields)
            //    field.sortable = true;

            //opts.GetFiled("Wyear").visible = false;
            opts.datas = model.GetAll();

            var jstr = JsonConvert.SerializeObject(opts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }
    }
}