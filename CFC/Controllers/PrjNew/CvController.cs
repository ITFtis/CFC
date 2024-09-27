using CFC.Models;
using CFC.Models.Prj;
using Dou.Controllers;
using Dou.Misc;
using Dou.Misc.Extension;
using Dou.Models.DB;
using Microsoft.Office.Interop.Excel;
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
            opts.ctrlFieldAlign = "left";

            foreach (var field in opts.fields)
            {
                field.visible = false;
                field.visibleEdit = false;
            }

            //欄位控制
            List<string> fs = new List<string>();
            fs.AddRange(new List<string>() { "Id", "FuelType", "Name", "Unit", "displayOrder" });
            
            //係數option
            fs.AddRange(new List<string>() { "CO2", "CH4", "NO2" });

            //set
            foreach (var str in fs)
            {
                opts.GetFiled(str).visible = true;
                opts.GetFiled(str).visibleEdit = true;
            }

            opts.datas = model.GetAll().OrderBy(a => a.displayOrder);

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
            opts.ctrlFieldAlign = "left";

            foreach (var field in opts.fields)
            {
                field.visible = false;
                field.visibleEdit = false;
            }

            //欄位控制
            List<string> fs = new List<string>();
            fs.AddRange(new List<string>() { "Id", "Name", "EscapeRate", "displayOrder" });

            //係數option
            fs.AddRange(new List<string>() { "MinValue", "MaxValue"});

            //set
            foreach (var str in fs)
            {
                opts.GetFiled(str).visible = true;
                opts.GetFiled(str).visibleEdit = true;
            }

            opts.datas = model.GetAll().OrderBy(a => a.displayOrder);

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
            opts.ctrlFieldAlign = "left";

            foreach (var field in opts.fields)
            {
                field.visible = false;
                field.visibleEdit = false;
            }

            //欄位控制
            List<string> fs = new List<string>();
            fs.AddRange(new List<string>() { "Id", "Name", "displayOrder"});

            //////係數option
            ////fs.AddRange(new List<string>() { });

            //set
            foreach (var str in fs)
            {
                opts.GetFiled(str).visible = true;
                opts.GetFiled(str).visibleEdit = true;
            }

            opts.datas = model.GetAll().OrderBy(a => a.displayOrder);

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
            opts.ctrlFieldAlign = "left";

            foreach (var field in opts.fields)
            {
                field.visible = false;
                field.visibleEdit = false;
            }

            //欄位控制
            List<string> fs = new List<string>();
            fs.AddRange(new List<string>() { "Id", "Type", "Name", "Unit", "displayOrder" });

            //係數option
            fs.AddRange(new List<string>() { "CO2", "CH4", "N2O", "HFCs", "PFCs", "SF6", "NF3" });

            //set
            foreach (var str in fs)
            {
                opts.GetFiled(str).visible = true;
                opts.GetFiled(str).visibleEdit = true;
            }

            opts.datas = model.GetAll().OrderBy(a => a.displayOrder);

            var jstr = JsonConvert.SerializeObject(opts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }

        /// <summary>
        /// (類別一)製程種類
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetTabSpecificTypeList()
        {
            Dou.Models.DB.IModelEntity<Specific_type> model = new Dou.Models.DB.ModelEntity<Specific_type>(new DouModelContext());

            var opts = Dou.Misc.DataManagerScriptHelper.GetDataManagerOptions<Specific_type>();
            opts.ctrlFieldAlign = "left";

            foreach (var field in opts.fields)
            {
                field.visible = false;
                field.visibleEdit = false;
            }

            //欄位控制
            List<string> fs = new List<string>();
            fs.AddRange(new List<string>() { "Id", "Name", "displayOrder"});

            ////係數option
            //fs.AddRange(new List<string>() { });

            //set
            foreach (var str in fs)
            {
                opts.GetFiled(str).visible = true;
                opts.GetFiled(str).visibleEdit = true;
            }

            opts.datas = model.GetAll().OrderBy(a => a.displayOrder);

            var jstr = JsonConvert.SerializeObject(opts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }

        /// <summary>
        /// (類別一)製程原料
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetTabSpecificPropertiesList()
        {
            Dou.Models.DB.IModelEntity<Specific_properties> model = new Dou.Models.DB.ModelEntity<Specific_properties>(new DouModelContext());

            var opts = Dou.Misc.DataManagerScriptHelper.GetDataManagerOptions<Specific_properties>();
            opts.ctrlFieldAlign = "left";


            foreach (var field in opts.fields)
            {
                field.visible = false;
                field.visibleEdit = false;
            }

            //欄位控制
            List<string> fs = new List<string>();
            fs.AddRange(new List<string>() { "Id", "Type", "Name", "Unit", "displayOrder" });

            //係數option
            fs.AddRange(new List<string>() { "CO2", "CH4", "N2O", "HFCs", "PFCs", "SF6", "NF3", "CoeSource"});

            //set
            foreach (var str in fs)
            {
                opts.GetFiled(str).visible = true;
                opts.GetFiled(str).visibleEdit = true;
            }

            opts.datas = model.GetAll().OrderBy(a => a.displayOrder);

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
            opts.ctrlFieldAlign = "left";

            foreach (var field in opts.fields)
            {
                field.visible = false;
                field.visibleEdit = false;
            }

            //欄位控制
            List<string> fs = new List<string>();
            fs.AddRange(new List<string>() { "year", "Name", "Unit", "MinValue", "MaxValue" });

            ///係數option
            //fs.AddRange(new List<string>() { "xxxx", "xxxx" });

            //set
            foreach (var str in fs)
            {
                opts.GetFiled(str).visible = true;
                opts.GetFiled(str).visibleEdit = true;
            }

            opts.datas = model.GetAll().AsEnumerable()
                        .OrderBy(a => int.Parse(a.year));

            var jstr = JsonConvert.SerializeObject(opts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }

        /// <summary>
        /// 3-6類別
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetTabCalsTypeList()
        {
            Dou.Models.DB.IModelEntity<Cals_type> model = new Dou.Models.DB.ModelEntity<Cals_type>(new DouModelContext());

            var opts = Dou.Misc.DataManagerScriptHelper.GetDataManagerOptions<Cals_type>();
            opts.ctrlFieldAlign = "left";


            foreach (var field in opts.fields)
            {
                field.visible = false;
                field.visibleEdit = false;
            }

            //欄位控制
            List<string> fs = new List<string>();
            fs.AddRange(new List<string>() { "Id", "IdText", "Name", "DisplayOrder" });

            ////係數option
            //fs.AddRange(new List<string>() { });

            //set
            foreach (var str in fs)
            {
                opts.GetFiled(str).visible = true;
                opts.GetFiled(str).visibleEdit = true;
            }

            var datas = model.GetAll().ToList();
            opts.datas = datas.OrderBy(a => a.DisplayOrder);
            ////if (datas.Count() == 0)
            ////    opts.datas = new List<Cals_type>() { new Cals_type { Id = "無資料，不可修改" } };
            ////else
            ////    opts.datas = datas.OrderBy(a => a.DisplayOrder);

            var jstr = JsonConvert.SerializeObject(opts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }

        /// <summary>
        /// 3-6類別項目
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetTabCalsPropertiesList(params KeyValueParams[] paras)
        {
            Dou.Models.DB.IModelEntity<Cals_properties> model = new Dou.Models.DB.ModelEntity<Cals_properties>(new DouModelContext());

            var opts = Dou.Misc.DataManagerScriptHelper.GetDataManagerOptions<Cals_properties>();
            opts.ctrlFieldAlign = "left";


            foreach (var field in opts.fields)
            {
                field.visible = false;
                field.visibleEdit = false;
            }

            //欄位控制
            List<string> fs = new List<string>();
            fs.AddRange(new List<string>() { "Id", "Type", "Name", "Unit", "DisplayOrder" });

            ////係數option
            //fs.AddRange(new List<string>() { });

            //set
            foreach (var str in fs)
            {
                opts.GetFiled(str).visible = true;
                opts.GetFiled(str).visibleEdit = true;
            }

            //datas
            var query = model.GetAll();
            var Type = KeyValue.GetFilterParaValue(paras, "Type");
            if (!string.IsNullOrEmpty(Type))
            {
                query = query.Where(a => a.Type == Type);
            }
            
            var datas = query.ToList();
            opts.datas = datas.OrderBy(a => a.DisplayOrder);

            ////if (datas.Count() == 0)
            ////    opts.datas = new List<Cals_properties>() { new Cals_properties { Id = "無資料，不可修改" } };
            ////else
            ////    opts.datas = datas.OrderBy(a => a.DisplayOrder);

            var jstr = JsonConvert.SerializeObject(opts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }
    }
}