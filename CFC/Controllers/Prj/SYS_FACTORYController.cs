using CFC.Models;
using CFC.Models.Prj;
using Dou.Controllers;
using Dou.Misc;
using Dou.Misc.Attr;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFC.Controllers.Prj
{

    [MenuDef(Id = "SYS_FACTORY", Name = "工廠", MenuPath = "工商登記資料", Action = "Index", Index = 2, Func = FuncEnum.ALL, AllowAnonymous = false)]
    //[AutoLogger(Content = AutoLoggerAttribute.LogContent.All)]
    public class SYS_FACTORYController : AGenericModelController<SYS_FACTORY>
    {
        // GET: SYS_FACTORY Properties
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<SYS_FACTORY> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<SYS_FACTORY>(new DouModelContext());
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            opts.ctrlFieldAlign = "left";

            return opts;            
        }

        internal IEnumerable<SYS_FACTORY> GetAllData()
        {
            return GetModelEntity().GetAll().ToArray();
        }
    }
}