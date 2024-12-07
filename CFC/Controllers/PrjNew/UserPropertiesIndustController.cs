using CFC.Models;
using CFC.Models.Prj;
using Dou.Controllers;
using Dou.Misc;
using Dou.Misc.Attr;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFC.Controllers.PrjNew
{
    [MenuDef(Id = "UserPropertiesIndust", Name = "會員(製造業管理)", MenuPath = "工商登記資料", Action = "Index", Index = 2, Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class UserPropertiesIndustController : AGenericModelController<G_USER_FACTORY>
    {
        // GET: UserPropertiesIndust
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<G_USER_FACTORY> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<G_USER_FACTORY>(new DouModelContext());
        }

        protected override IEnumerable<G_USER_FACTORY> GetDataDBObject(IModelEntity<G_USER_FACTORY> dbEntity, params KeyValueParams[] paras)
        {
            var result = base.GetDataDBObject(dbEntity, paras);

            //製造業會員
            result = result.Where(a => a.IndustrialTypeId != "1");

            return result;
        }

        protected override void AddDBObject(IModelEntity<G_USER_FACTORY> dbEntity, IEnumerable<G_USER_FACTORY> objs)
        {
            if (!ToValidate(objs.First(), "Add"))
                return;

            base.AddDBObject(dbEntity, objs);
        }

        protected override void UpdateDBObject(IModelEntity<G_USER_FACTORY> dbEntity, IEnumerable<G_USER_FACTORY> objs)
        {
            if (!ToValidate(objs.First(), "Update"))
                return;

            base.UpdateDBObject(dbEntity, objs);
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            opts.ctrlFieldAlign = "left";

            return opts;
        }

        private bool ToValidate(G_USER_FACTORY f, string type)
        {
            bool result = false;

            //會員工廠對應表
            var datas = GetModelEntity().GetAll();
            var us = User_Properties_Advance.GetAllDatas();
            var factorys = SYS_FACTORY.GetAllDatas();

            if (us.Where(a => a.Id == f.USER_ID).Count() == 0)
            {
                string errorMessage = string.Format("查無此會員：{0}", f.USER_ID);
                throw new Exception(errorMessage);
            }

            if (factorys.Where(a => a.FACTORY_REGISTRATION == f.FACTORY_REGISTRATION).Count() == 0)
            {
                string errorMessage = string.Format("查無此工廠登記證：{0}", f.FACTORY_REGISTRATION);
                throw new Exception(errorMessage);
            }

            if (datas.Where(a => a.IDX != f.IDX)
                        .Where(a => a.USER_ID == f.USER_ID && a.FACTORY_REGISTRATION == f.FACTORY_REGISTRATION).Count() > 0)
            {
                string errorMessage = string.Format("會員({0})已加入工廠{1}，不可重複：{0}", f.USER_ID, f.FACTORY_REGISTRATION);
                throw new Exception(errorMessage);
            }

            result = true;

            return result;
        }
    }
}