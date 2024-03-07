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

    [MenuDef(Name = "製程原料", MenuPath = "資料管理", Action = "Index", Index = 6, Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class SpecificPropertiesController : AGenericModelController<Specific_properties>
    {
        
        // GET: FuelProperties
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Specific_properties> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Specific_properties>(new DouModelContext());
        }
        public override Task<ActionResult> GetData(params KeyValueParams[] paras)
        {
            var s =paras.FirstOrDefault(p => p.key == "sort");
            if (s != null && s.value == null)
                s.value = "displayOrder";
            return base.GetData(paras);
        }

        //[HttpPost]
        //public async Task<ActionResult> Add(IEnumerable<Specific_properties> objs) {
        //    var dbCon =  new Dou.Models.DB.ModelEntity<Specific_properties>(new DouModelContext());

        //    foreach (var obj in objs) {
        //        await dbCon.AddAsync(new Specific_properties
        //        {
        //            Id = obj.Id,
        //            Name = obj.Name,
        //            Type = obj.TypeName, //這邊回來的是Type ID
        //            displayOrder = obj.displayOrder,
        //            Unit = obj.Unit
        //        });
        //    }

        //    return await Task.FromResult(Json(new { Success = "True" , Desc= "新增成功" }, JsonRequestBehavior.AllowGet));
        //}



        internal IEnumerable<Specific_properties> GetAllData()
        {
           return GetModelEntity().GetAll().OrderBy(s=>s.displayOrder).ToArray();
        }

    }
}