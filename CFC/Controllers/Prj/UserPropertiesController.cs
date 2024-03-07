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

        internal IEnumerable<User_Properties_Advance> GetAllData()
        {
            return GetModelEntity().GetAll().ToArray();
        }
    }
}