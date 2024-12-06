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

namespace CFC.Controllers.PrjNew
{
    [MenuDef(Id = "UserPropertiesIndust", Name = "會員(製造業管理)", MenuPath = "工商登記資料", Action = "Index", Index = 2, Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class UserPropertiesIndustController : AGenericModelController<G_USER_FACTORY>
    {
        // GET: UserPropertiesIndust
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<G_USER_FACTORY> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<G_USER_FACTORY>(new DouModelContext());
        }

        protected override IEnumerable<G_USER_FACTORY> GetDataDBObject(IModelEntity<G_USER_FACTORY> dbEntity, params KeyValueParams[] paras)
        {
            var result = base.GetDataDBObject(dbEntity, paras);

            //製造業會員
            result = result.Where(a => a.IndustrialTypeId != "1");

            return result;
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            opts.ctrlFieldAlign = "left";

            return opts;
        }
    }
}