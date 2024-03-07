using CFC.Models;
using CFC.Models.Prj;
using Dou.Controllers;
using Dou.Misc.Attr;
using Dou.Models.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CFC.Controllers.Prj
{

    [MenuDef(Name = "製程種類", MenuPath = "資料管理", Action = "Index", Index = 7, Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class SpecificTypeController : AGenericModelController<Specific_type>
    {
        
        // GET: FuelProperties
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Specific_type> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Specific_type>(new DouModelContext());
        }
        public override Task<ActionResult> GetData(params KeyValueParams[] paras)
        {
            var s =paras.FirstOrDefault(p => p.key == "sort");
            if (s != null && s.value == null)
                s.value = "displayOrder";
            return base.GetData(paras);
        }
        internal IEnumerable<Specific_type> GetAllData()
        {
           return GetModelEntity().GetAll().OrderBy(s=>s.displayOrder).ToArray();
        }

        public static String getTypeKeyValues() {
            var outObject = new JObject();
            Api.DateViewController.AllSpecificType.ToList().ForEach(e =>
            {
                outObject[e.Id] = e.Name;
            });
            return outObject.ToString();
        }

    }
}