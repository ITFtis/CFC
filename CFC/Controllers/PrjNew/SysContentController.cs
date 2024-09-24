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

namespace CFC.Controllers.Prj
{
    [Dou.Misc.Attr.MenuDef(Id = "SysContent", Name = "圖文編輯", MenuPath = "計算", Action = "Index", Index = 3, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]

    public class SysContentController : AGenericModelController<Sys_content>
    {
        // GET: SysContent
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Sys_content> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Sys_content>(new DouModelContext());
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();
            opts.ctrlFieldAlign = "left";

            return opts;
        }

        protected override void DeleteDBObject(IModelEntity<Sys_content> dbEntity, IEnumerable<Sys_content> objs)
        {
            var obj = objs.FirstOrDefault();

            //DB沒關聯
            var dbContext = new DouModelContext();

            if (obj.Details != null)
            {
                Dou.Models.DB.IModelEntity<Sys_contentDetail> details = new Dou.Models.DB.ModelEntity<Sys_contentDetail>(dbContext);
                details.Delete(obj.Details);
            }

            base.DeleteDBObject(dbEntity, objs);
        }
    }
}