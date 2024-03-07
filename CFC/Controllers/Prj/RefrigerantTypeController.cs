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
    [MenuDef(Name = "冷媒種類", MenuPath = "資料管理", Action = "Index", Index = 4, Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class RefrigerantTypeController : AGenericModelController<Refrigerant_type>
    {
        // GET: RefrigerantEquip
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Refrigerant_type> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Refrigerant_type>(new DouModelContext());
        }

        internal IEnumerable<Refrigerant_type> GetAllData()
        {
            return GetModelEntity().GetAll().OrderBy(s => s.displayOrder).ToArray();
        }
    }
}