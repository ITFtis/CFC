using CFC.Models;
using CFC.Models.Prj;
using Dou.Controllers;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFC.Controllers.PrjNew
{
    [Dou.Misc.Attr.MenuDef(Id = "Cv", Name = "係數管理", MenuPath = "計算", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class CvController : AGenericModelController<Fuel_properties>
    {
        // GET: Cv
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Fuel_properties> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Fuel_properties>(new DouModelContext());
        }


    }
}