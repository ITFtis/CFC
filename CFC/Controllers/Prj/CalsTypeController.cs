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
    [MenuDef(Name = "3-6類別", MenuPath = "資料管理", Action = "Index", Index = 9, Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class CalsTypeController : AGenericModelController<Cals_type>
    {
        // GET: CalsType
        public ActionResult Index()
        {
            return View();
        }
        protected override IModelEntity<Cals_type> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Cals_type>(new DouModelContext());
        }
        internal IEnumerable<Cals_type> GetAllData()
        {
            return GetModelEntity().GetAll().OrderBy(s => s.DisplayOrder).ToArray();
        }

        protected override void AddDBObject(IModelEntity<Cals_type> dbEntity, IEnumerable<Cals_type> objs)
        {
            base.AddDBObject(dbEntity, objs);
            CalsTypeSelectItems.Reset();
        }

        protected override void UpdateDBObject(IModelEntity<Cals_type> dbEntity, IEnumerable<Cals_type> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            CalsTypeSelectItems.Reset();
        }

        protected override void DeleteDBObject(IModelEntity<Cals_type> dbEntity, IEnumerable<Cals_type> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            CalsTypeSelectItems.Reset();
        }
    }
}