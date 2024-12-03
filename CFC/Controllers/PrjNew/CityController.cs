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
    [Dou.Misc.Attr.MenuDef(Id = "City", Name = "縣市代碼", MenuPath = "代碼維護", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class CityController : AGenericModelController<City>
    {
        // GET: City
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<City> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<City>(new DouModelContext());
        }

        protected override IEnumerable<City> GetDataDBObject(IModelEntity<City> dbEntity, params KeyValueParams[] paras)
        {
            var result = base.GetDataDBObject(dbEntity, paras);

            result = result.OrderBy(a => a.Sort);

            return result;
        }
    }
}