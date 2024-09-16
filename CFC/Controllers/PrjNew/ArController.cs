using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFC.Controllers.PrjNew
{
    [Dou.Misc.Attr.MenuDef(Id = "Ar", Name = "IPCC_AR版本管理", MenuPath = "計算", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class ArController : Controller
    {
        // GET: Ar
        public ActionResult Index()
        {
            return View();
        }
    }
}