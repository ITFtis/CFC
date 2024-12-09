using CFC.Controllers.FileDownload.ExcelManagerF;
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

namespace CFC.Controllers.Prj
{
    [MenuDef(Id = "UserProperties", Name = "會員", MenuPath = "工商登記資料", Action = "Index", Index = 1, Func = FuncEnum.ALL, AllowAnonymous = false)]
    //[AutoLogger(Content = AutoLoggerAttribute.LogContent.All)]
    public class UserPropertiesController : AGenericModelController<User_Properties_Advance>
    {
        // GET: UserProperties
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<User_Properties_Advance> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<User_Properties_Advance>(new DouModelContext());
        }

        protected override IEnumerable<User_Properties_Advance> GetDataDBObject(IModelEntity<User_Properties_Advance> dbEntity, params KeyValueParams[] paras)
        {
            var result = base.GetDataDBObject(dbEntity, paras);

            var industrialTypeName = KeyValue.GetFilterParaValue(paras, "IndustrialTypeName");
            var filterName = KeyValue.GetFilterParaValue(paras, "FilterName");

            //行業別條件
            if (!string.IsNullOrEmpty(industrialTypeName))
            {
                if (industrialTypeName == "1")
                {
                    result = result.Where(a => a.IndustrialTypeId == "1");
                }
                else if (industrialTypeName != "1")
                {
                    result = result.Where(a => a.IndustrialTypeId != "1");
                }
            }

            //公司名稱
            if (!string.IsNullOrEmpty(filterName))
            {
                result = result.Where(a => a.FilterName.Contains(filterName));
            }

            int n = result.Count();

            return result;
        }

        protected override void AddDBObject(IModelEntity<User_Properties_Advance> dbEntity, IEnumerable<User_Properties_Advance> objs)
        {
            var f = objs.First();
            if (!SetIni(f))
                throw new Exception("資料更新失敗，請通知網站負責人");

            base.AddDBObject(dbEntity, objs);
        }

        protected override void UpdateDBObject(IModelEntity<User_Properties_Advance> dbEntity, IEnumerable<User_Properties_Advance> objs)
        {
            var f = objs.First();
            if (!SetIni(f))
                throw new Exception("資料更新失敗，請通知網站負責人");

            base.UpdateDBObject(dbEntity, objs);
        }

        internal IEnumerable<User_Properties_Advance> GetAllData()
        {
            return GetModelEntity().GetAll().ToArray();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            opts.ctrlFieldAlign = "left";

            return opts;
        }

        private bool SetIni(User_Properties_Advance f)
        {
            bool result = false;

            try
            {
                if (f.IndustrialTypeId == "1")
                {
                    //非製造業
                }
                else
                {
                    //製造業
                    f.UNIT_TYPE = null;
                    f.CITY = null;
                    f.DISTRICT = null;
                    f.ADDRESS = null;
                }

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Log.For(null).Error("執行錯誤：" + ex.Message);
                Logger.Log.For(null).Error(ex.StackTrace);                
            }

            return result;
        }
    }
}