using CFC.Models.Prj;
using CFC.Models;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dou.Controllers;

namespace CFC.Controllers.PrjNew
{
    [Dou.Misc.Attr.MenuDef(Id = "Town", Name = "鄉鎮代碼", MenuPath = "代碼維護", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class TownController : AGenericModelController<Town>
    {
        // GET: Town
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Town> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Town>(new DouModelContext());
        }

        protected override IEnumerable<Town> GetDataDBObject(IModelEntity<Town> dbEntity, params KeyValueParams[] paras)
        {
            var result = base.GetDataDBObject(dbEntity, paras);

            result = result.OrderBy(a => a.CitySort);

            return result;
        }
    }
}