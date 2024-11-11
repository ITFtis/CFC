using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFC.Controllers.PrjNew
{
    [Dou.Misc.Attr.MenuDef(Id = "UserCal", Name = "會員統計數據", MenuPath = "管理", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class UserCalController : Controller
    {
        // GET: UserCal
        public ActionResult Index()
        {
            return View();
        }
    }
}