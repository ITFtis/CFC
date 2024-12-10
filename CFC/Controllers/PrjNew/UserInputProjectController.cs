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
    [Dou.Misc.Attr.MenuDef(Id = "UserInputProject", Name = "專案盤查清冊檔案下載", MenuPath = "管理", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.Update, AllowAnonymous = false)]
    public class UserInputProjectController : AGenericModelController<vwe_UserInputProject>
    {
        // GET: UserInputProject
        public ActionResult Index()
        {            
            return View();
        }


        protected override Dou.Models.DB.IModelEntity<vwe_UserInputProject> GetModelEntity()
        {
            return null;
        }


        protected override IEnumerable<vwe_UserInputProject> GetDataDBObject(IModelEntity<vwe_UserInputProject> dbEntity, params KeyValueParams[] paras)
        {
            return new List<vwe_UserInputProject>().AsEnumerable();

            ////return base.GetDataDBObject(dbEntity, paras);
        }
    }

    public class vwe_UserInputProject
    {
        [Display(Name = "使用者帳號")]
        [ColumnDef(Visible = true, VisibleEdit = false)]
        public string UserID { get; set; }

        [Required]
        [Display(Name = "區間開始日期")]
        [ColumnDef(VisibleEdit = false)]
        public string StartDate { get; set; }

        [Required]
        [Display(Name = "區間結束日期")]
        [ColumnDef(VisibleEdit = false)]
        public string EndDate { get; set; }

    }
}