using CFC.Models;
using Dou.Controllers;
using Dou.Misc.Attr;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFC.Controllers.PrjNew
{
    [Dou.Misc.Attr.MenuDef(Id = "UserInputCal", Name = "會員計算紀錄明細", MenuPath = "管理", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.Update, AllowAnonymous = false)]
    public class UserInputCalController : AGenericModelController<UserInputCalList>
    {
        // GET: UserInputCal
        public ActionResult Index()
        {
            return View();
        }
        protected override IModelEntity<UserInputCalList> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<UserInputCalList>(new DouModelContext());
        }

        protected override IEnumerable<UserInputCalList> GetDataDBObject(IModelEntity<UserInputCalList> dbEntity, params KeyValueParams[] paras)
        {
            return new List<UserInputCalList>();

            //return base.GetDataDBObject(dbEntity, paras);
        }
    }

    public class UserInputCalList
    {
        //SYS_COMPANY
        [Display(Name = "統一編號")]
        [ColumnDef(Visible = false, Filter = true)]
        public string UniformNumber { get; set; }

        //User_Properties_Advance
        [Display(Name = "會員帳號")]
        [ColumnDef(Visible = false, Filter = true)]
        public string Id { get; set; }

        //User_Input_Advance
        [Display(Name = "專案名稱")]
        [ColumnDef(Visible = false, Filter = true)]
        public string ProjectName { get; set; }
    }
}