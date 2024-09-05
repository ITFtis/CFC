using CFC.Models;
using CFC.Models.Prj;
using Dou.Controllers;
using Dou.Misc;
using Dou.Misc.Attr;
using Dou.Models.DB;
using DouHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFC.Controllers.Prj
{
    [MenuDef(Name = "電力計算", MenuPath = "資料管理", Action = "Index", Index = 4, Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class ElecPropertiesController : AGenericModelController<Elec_properties>
    {
        private DouModelContext db = new DouModelContext();

        // GET: ElecProperties
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Elec_properties> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Elec_properties>(new DouModelContext());
        }
        internal IEnumerable<Elec_properties> GetAllData()
        {
            return GetModelEntity().GetAll().OrderByDescending(e => e.year).ToArray();
        }

        /// <summary>
        /// 取得標籤清單
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetTabList()
        {
            var opts = Dou.Misc.DataManagerScriptHelper.GetDataManagerOptions<Elec_properties>();

            ////全部欄位排序
            //foreach (var field in opts.fields)
            //    field.sortable = true;

            //opts.GetFiled("Wyear").visible = false;
            opts.datas = this.GetAllData();

            var jstr = JsonConvert.SerializeObject(opts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }

    }
}