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


    [MenuDef(Name = "工商登記資料", MenuPath = "資料管理", Action = "Index", Index = 4, Func = FuncEnum.ALL, AllowAnonymous = false)]
    //[AutoLogger(Content = AutoLoggerAttribute.LogContent.All)]
    public class SYS_COMPANYController : APaginationModelController<SYS_COMPANY>
    {
        // GET: SYS_FACTORY Properties
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<SYS_COMPANY> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity< SYS_COMPANY>(new DouModelContext());
        }

        protected override IQueryable<SYS_COMPANY> BeforeIQueryToPagedList(IQueryable<SYS_COMPANY> iquery, params KeyValueParams[] paras)
        {
            //預設排序
            iquery = iquery.OrderByDescending(a => a.COMP_ID);

            return base.BeforeIQueryToPagedList(iquery, paras);
        }

        protected override void AddDBObject(IModelEntity<SYS_COMPANY> dbEntity, IEnumerable<SYS_COMPANY> objs)
        {
            var f = objs.First();

            f.BDate = DateFormat.ToDate4(DateTime.Now);
            f.BId = Dou.Context.CurrentUserBase.Id;

            base.AddDBObject(dbEntity, objs);
        }

        protected override void UpdateDBObject(IModelEntity<SYS_COMPANY> dbEntity, IEnumerable<SYS_COMPANY> objs)
        {
            var f = objs.First();

            f.UDate = DateFormat.ToDate4(DateTime.Now);
            f.UId = Dou.Context.CurrentUserBase.Id;            

            base.UpdateDBObject(dbEntity, objs);
        }

        internal IEnumerable<SYS_COMPANY> GetAllData()
        {
            return GetModelEntity().GetAll().ToArray();
        }
    }

}