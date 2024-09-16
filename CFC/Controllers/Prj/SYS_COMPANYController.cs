using CFC.Models;
using CFC.Models.Prj;
using Dou.Controllers;
using Dou.Misc.Attr;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFC.Controllers.Prj
{


    [MenuDef(Name = "工商登記資料", MenuPath = "資料管理", Action = "Index", Index = 4, Func = FuncEnum.ALL, AllowAnonymous = false)]
    //[AutoLogger(Content = AutoLoggerAttribute.LogContent.All)]
    public class SYS_COMPANYController : AGenericModelController<SYS_COMPANY>
    {
        // GET: SYS_FACTORY Properties
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<SYS_COMPANY> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity< SYS_COMPANY>(new DouModelContext());
        }

        internal IEnumerable<SYS_COMPANY> GetAllData()
        {
            return GetModelEntity().GetAll().ToArray();
        }
    }

}