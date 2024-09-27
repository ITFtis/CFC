using CFC.Models.Prj;
using CFC.Models;
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
    public class SysContentDetailController : Dou.Controllers.AGenericModelController<Sys_contentDetail>
    {
        // GET: SysContentDetail
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Sys_contentDetail> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Sys_contentDetail>(new DouModelContext());
        }

        protected override void AddDBObject(IModelEntity<Sys_contentDetail> dbEntity, IEnumerable<Sys_contentDetail> objs)
        {
            base.AddDBObject(dbEntity, objs);
            Sys_contentDetail.ResetGetAllDatas();
        }

        protected override void UpdateDBObject(IModelEntity<Sys_contentDetail> dbEntity, IEnumerable<Sys_contentDetail> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            Sys_contentDetail.ResetGetAllDatas();
        }

        protected override void DeleteDBObject(IModelEntity<Sys_contentDetail> dbEntity, IEnumerable<Sys_contentDetail> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            Sys_contentDetail.ResetGetAllDatas();
        }

        /// <summary>
        /// 3-6類別
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetDataManagerOptionsJson()
        {
            var opts = Dou.Misc.DataManagerScriptHelper.GetDataManagerOptions<Sys_contentDetail>();
            opts.ctrlFieldAlign = "left";

            var jstr = JsonConvert.SerializeObject(opts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }
    }
}