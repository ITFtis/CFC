using Dou.Misc.Attr;
using DouHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dou.Controllers;
using Dou.Models.DB;
using CFC.Models;
using CFC.Controllers.Api;
using CFC.Models.Prj;

namespace CFC.Controllers.PrjNew
{
    [Dou.Misc.Attr.MenuDef(Id = "UserCal", Name = "會員統計數據", MenuPath = "管理", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.None, AllowAnonymous = false)]
    public class UserCalController : AGenericModelController<UserCalList>
    {
        // GET: UserCal
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<UserCalList> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<UserCalList>(new DouModelContext());
        }

        protected override IEnumerable<UserCalList> GetDataDBObject(IModelEntity<UserCalList> dbEntity, params KeyValueParams[] paras)
        {
            List<UserCalList> result = new List<UserCalList>();

            List<User_Properties_Advance> totalUser = DateViewController.AllUserProperties.ToList();

            //會員總人數
            result.Add(new UserCalList()
            {
                Name = "會員總人數",
                Count = totalUser.Count,
            });

            //製造業會員人數
            result.Add(new UserCalList() { 
                Name = "製造業會員人數",
                Count = totalUser.Where(a => a.IndustrialTypeId != "1").Count(),
            });

            //非製造業會員人數
            result.Add(new UserCalList()
            {
                Name = "非製造業會員人數",
                Count = totalUser.Where(a => a.IndustrialTypeId == "1").Count(),
            });

            return result;
        }
    }

    public class UserCalList
    {        
        [Display(Name = "項目名稱")]
        public string Name { get; set; }

        [Display(Name = "會員人數")]
        public int Count { get; set; }
    }
}