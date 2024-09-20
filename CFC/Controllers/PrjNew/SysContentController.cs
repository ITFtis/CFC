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
    }
}