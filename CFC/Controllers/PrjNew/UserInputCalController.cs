using CFC.Controllers.Api;
using CFC.Models;
using CFC.Models.Prj;
using Dou.Controllers;
using Dou.Misc;
using Dou.Misc.Attr;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace CFC.Controllers.PrjNew
{
    [Dou.Misc.Attr.MenuDef(Id = "UserInputCal", Name = "會員計算紀錄明細", MenuPath = "管理", Action = "Index", Index = 3, Func = Dou.Misc.Attr.FuncEnum.None, AllowAnonymous = false)]
    public class UserInputCalController : APaginationModelController<User_Input_Advance>
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

            var UniformNumber = KeyValue.GetFilterParaValue(paras, "UniformNumber");
            var filterStartS = KeyValue.GetFilterParaValue(paras, "FilterStartS");
            var filterStartE = KeyValue.GetFilterParaValue(paras, "FilterStartE");

            //有儲存專案
            iquery = iquery.Where(a => a.IsSave);
            iquery = iquery.Where(a => a.UserID != "查無此紀錄");

            if (!string.IsNullOrEmpty(UniformNumber))
            {
                var e = iquery.AsEnumerable();
                e = e.Where(a => a.UniformNumber.Contains(UniformNumber));
                iquery = e.AsQueryable();
            }
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

            ///var aaa = iquery.ToList();
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
            opts.GetFiled("StartDate_F").title = "開始日期";

            opts.GetFiled("UniformNumber").visible = true;
            opts.GetFiled("UserID").visible = true;
            opts.GetFiled("StartDate_F").visible = true;
            opts.GetFiled("ProjectName").visible = true;
            opts.GetFiled("VClass1").visible = true;
            opts.GetFiled("VClass2").visible = true;
            opts.GetFiled("VClass3").visible = true;
            opts.GetFiled("VClass4").visible = true;
            opts.GetFiled("VClass5").visible = true;
            opts.GetFiled("VClass6").visible = true;
            opts.GetFiled("VClassTotal").visible = true;

            opts.GetFiled("UniformNumber").filter = true;
            opts.GetFiled("UserID").filter = true;
            opts.GetFiled("FilterStartS").filter = true;
            opts.GetFiled("FilterStartE").filter = true;

            return opts;
        }
    }
}