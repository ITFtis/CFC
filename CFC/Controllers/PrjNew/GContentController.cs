using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFC.Controllers.PrjNew
{
    [Dou.Misc.Attr.MenuDef(Id = "GContent", Name = "圖文編輯", MenuPath = "計算", Action = "Index", Index = 3, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class GContentController : Controller
    {
        // GET: GContent
        public ActionResult Index()
        {
            return View();
        }
    }
}