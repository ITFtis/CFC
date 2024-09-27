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
    [MenuDef(Name = "3-6類別項目", MenuPath = "資料管理", Action = "Index", Index = 10, Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class CalsPropertiesController : AGenericModelController<Cals_properties>
    {
        // GET: CalsProperties
        public ActionResult Index()
        {
            return View();
        }
        protected override IModelEntity<Cals_properties> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Cals_properties>(new DouModelContext());
        }
        internal IEnumerable<Cals_properties> GetAllData()
        {
            return GetModelEntity().GetAll().OrderBy(s => s.DisplayOrder).ToArray();
        }

        protected override void AddDBObject(IModelEntity<Cals_properties> dbEntity, IEnumerable<Cals_properties> objs)
        {
            base.AddDBObject(dbEntity, objs);
            Cals_properties.ResetGetAllDatas();
        }

        protected override void UpdateDBObject(IModelEntity<Cals_properties> dbEntity, IEnumerable<Cals_properties> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            Cals_properties.ResetGetAllDatas();
        }

        protected override void DeleteDBObject(IModelEntity<Cals_properties> dbEntity, IEnumerable<Cals_properties> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            Cals_properties.ResetGetAllDatas();
        }
    }
}