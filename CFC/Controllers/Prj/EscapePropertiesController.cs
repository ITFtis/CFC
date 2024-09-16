using CFC.Models;
using CFC.Models.Prj;
using Dou.Controllers;
using Dou.Misc.Attr;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CFC.Controllers.Prj
{

    [MenuDef(Name = "逸散氣體", MenuPath = "資料管理", Action = "Index", Index = 8, Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class EscapePropertiesController : AGenericModelController<Escape_properties>
    {
        
        // GET: FuelProperties
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Escape_properties> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Escape_properties>(new DouModelContext());
        }
        public override Task<ActionResult> GetData(params KeyValueParams[] paras)
        {
            var s =paras.FirstOrDefault(p => p.key == "sort");
            if (s != null && s.value == null)
                s.value = "displayOrder";
            return base.GetData(paras);
        }

        ////[HttpPost]
        ////public async Task<ActionResult> Add(IEnumerable<Escape_properties> objs) {
        ////    var dbCon =  new Dou.Models.DB.ModelEntity<Escape_properties>(new DouModelContext());

        ////    foreach (var obj in objs) {
        ////        await dbCon.AddAsync(new Escape_properties
        ////        {
        ////            Id = obj.Id,
        ////            Name = obj.Name,
        ////            Type = obj.Type, //obj.TypeName, //這邊回來的是Type ID
        ////            displayOrder = obj.displayOrder,
        ////            Unit = obj.Unit,
        ////            CO2 = obj.CO2,
        ////            CH4 = obj.CH4,
        ////            N2O = obj.N2O,
        ////        });
        ////    }

        ////    return await Task.FromResult(Json(new { Success = "True" , Desc= "新增成功" }, JsonRequestBehavior.AllowGet));
        ////}

        internal IEnumerable<Escape_properties> GetAllData()
        {
           return GetModelEntity().GetAll().OrderBy(s=>s.displayOrder).ToArray();
        }

    }
}