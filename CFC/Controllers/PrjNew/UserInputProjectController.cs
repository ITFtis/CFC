using CFC.Models;
using CFC.Models.Prj;
using Dou.Controllers;
using Dou.Misc;
using Dou.Misc.Attr;
using Dou.Models.DB;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace CFC.Controllers.PrjNew
{
    [Dou.Misc.Attr.MenuDef(Id = "UserInputProject", Name = "專案盤查清冊檔案下載", MenuPath = "管理", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.None, AllowAnonymous = false)]
    public class UserInputProjectController : APaginationModelController<User_Input_Advance>
    {
        // GET: UserInputProject
        public ActionResult Index()
        {            
            return View();
        }


        protected override Dou.Models.DB.IModelEntity<User_Input_Advance> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<User_Input_Advance>(new DouModelContext());
        }

        protected override IQueryable<User_Input_Advance> BeforeIQueryToPagedList(IQueryable<User_Input_Advance> iquery, params KeyValueParams[] paras)
        {
            Dou.Help.DouUnobtrusiveSession.Session.Remove("SessionList");

            var filterStartS = KeyValue.GetFilterParaValue(paras, "FilterStartS");
            var filterStartE = KeyValue.GetFilterParaValue(paras, "FilterStartE");

            //有儲存專案
            iquery = iquery.Where(a => a.IsSave);
            iquery = iquery.Where(a => a.UserID != "查無此紀錄");

            if (!string.IsNullOrEmpty(filterStartS))
            {
                var e = iquery.AsEnumerable();
                DateTime date = DateTime.Parse(filterStartS);
                e = e.Where(a => a.FilterStartS != DateTime.MinValue);
                e = e.Where(a => a.FilterStartS >= date);
                iquery = e.AsQueryable();
            }
            if (!string.IsNullOrEmpty(filterStartE))
            {
                var e = iquery.AsEnumerable();
                DateTime date = DateTime.Parse(filterStartE);
                e = e.Where(a => a.FilterStartE != DateTime.MinValue);
                e = e.Where(a => a.FilterStartE <= date);
                iquery = e.AsQueryable();
            }

            //限定製造業
            if (1 == 1)
            {
                var e = iquery.AsEnumerable();
                e = e.Where(a => a.IndustrialTypeId != "1");
                iquery = e.AsQueryable();
            }

            ////try
            ////{
            ////    var aaa = iquery.ToList();
            ////}
            ////catch (Exception ex)
            ////{
            ////    string str = ex.Message;
            ////}

            Dou.Help.DouUnobtrusiveSession.Session.Add("SessionList", iquery.ToList());

            return base.BeforeIQueryToPagedList(iquery, paras);
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
            {
                field.visible = false;
                field.filter = false;
            }

            opts.GetFiled("UserID").defaultvalue = "csts01";

            opts.GetFiled("UserID").visible = true;
            opts.GetFiled("StartDate_F").visible = true;
            opts.GetFiled("EndDate_F").visible = true;
            opts.GetFiled("ProjectName").visible = true;
            opts.GetFiled("IndustrialTypeName").visible = true;

            opts.GetFiled("UserID").filter = true;
            opts.GetFiled("FilterStartS").filter = true;
            opts.GetFiled("FilterStartE").filter = true;

            return opts;
        }

        //產出專案清冊
        public ActionResult ExportProjectList()
        {
            //Valid
            var sessionList = Dou.Help.DouUnobtrusiveSession.Session["SessionList"];
            if (sessionList == null)
            {
                return Json(new { result = false, errorMessage = "session(sessionList)：null，請通知系統管理者" });
            }

            List<User_Input_Advance> datas = (List<User_Input_Advance>)sessionList;
            if (datas.Count == 0)
            {
                return Json(new { result = false, errorMessage = "清單無資料" });
            }

            Rpt_ProjectList rep = new Rpt_ProjectList();
            string url = rep.Export(datas);

            if (url == "")
            {                
                return Json(new { result = false, errorMessage = rep.ErrorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = true, url = url }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}