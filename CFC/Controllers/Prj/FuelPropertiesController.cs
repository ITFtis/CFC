using CFC.Models;
using CFC.Models.Prj;
using Dou.Controllers;
using Dou.Misc;
using Dou.Misc.Attr;
using Dou.Models.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CFC.Controllers.Prj
{

    [MenuDef(Name = "燃料計算", MenuPath = "資料管理", Action = "Index", Index = 4, Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class FuelPropertiesController : AGenericModelController<Fuel_properties>
    {
        
        // GET: FuelProperties
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Fuel_properties> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Fuel_properties>(new DouModelContext());
        }
        public override Task<ActionResult> GetData(params KeyValueParams[] paras)
        {
            var s =paras.FirstOrDefault(p => p.key == "sort");
            if (s != null && s.value == null)
                s.value = "displayOrder";
            return base.GetData(paras);
        }
        internal IEnumerable<Fuel_properties> GetAllData()
        {
           return GetModelEntity().GetAll().OrderBy(s=>s.displayOrder).ToArray();
        }
      
    }
}