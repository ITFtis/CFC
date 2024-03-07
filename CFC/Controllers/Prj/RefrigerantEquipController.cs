using CFC.Models;
using CFC.Models.Prj;
using Dou.Controllers;
using Dou.Misc;
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
    [MenuDef(Name = "冷媒設備", MenuPath = "資料管理", Action = "Index", Index = 4, Func = FuncEnum.ALL, AllowAnonymous = true)]
    public class RefrigerantEquipController : AGenericModelController<Refrigerant_equip>
    {
        // GET: RefrigerantEquip
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Refrigerant_equip> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Refrigerant_equip>(new DouModelContext());
        }

        protected override IEnumerable<Refrigerant_equip> GetDataDBObject(IModelEntity<Refrigerant_equip> dbEntity, params KeyValueParams[] paras)
        {
            IEnumerable<Refrigerant_equip> result = dbEntity.GetAllWithInclude(u => u.EquipTypeMap);
            return result;
        }
        protected override void UpdateDBObject(IModelEntity<Refrigerant_equip> dbEntity, IEnumerable<Refrigerant_equip> objs)
        {
            Refrigerant_equip nd = objs.First();
            Refrigerant_equip od = null;
            od = dbEntity.GetAll(s => s.Id.Equals(nd.Id), s => s.EquipTypeMap).FirstOrDefault(); //在Dbcontext是cache才能用find，不然要用GetAll
        
            if (nd != od && od.EquipTypeMap != null)
                od.EquipTypeMap.Clear(); //會刪除RoleUsers，再用objs裡的RoleUsers重新新增
            base.UpdateDBObject(dbEntity, objs);
            DouHelper.Misc.ClearCache("GetAllDataIncludeMap");
        }
        internal IEnumerable<Refrigerant_equip> GetAllData()
        {
            return GetModelEntity().GetAll().OrderBy(s => s.displayOrder).ToArray();
        }
        internal IEnumerable<Refrigerant_equip> GetAllDataIncludeMap()
        {
            //return GetModelEntity().GetAll(null, s=>s.EquipTypeMap.i,)(s => s.EquipTypeMap).OrderBy(s => s.displayOrder).ToArray();
            return GetModelEntity().GetAllWithInclude(s=>s.EquipTypeMap).OrderBy(s => s.displayOrder).ToArray();
        }
        public ActionResult EditForm(Refrigerant_equip equip)
        {
            return PartialView(equip);
        }

        public override DataManagerOptions GetDataManagerOptions()
        {

            DataManagerOptions options = base.GetDataManagerOptions();
            options.editformLayoutUrl = new UrlHelper(this.ControllerContext.RequestContext).Action("EditForm");
            options.editformWindowClasses = "modal-lg";
            return options;
        }
    }
}