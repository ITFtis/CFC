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
            //沒資料
            //return new List<UserInputCalList>();

            //假資料
            List<UserInputCalList> result = GetFakes();

            return result;
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            opts.editable = false;

            return opts;
        }

        private List<UserInputCalList> GetFakes()
        {
            List<UserInputCalList> result = new List<UserInputCalList>();

            //假資料
            DateTime date1 = DateTime.Parse("2024/09/03 10:00");
            result.Add(new UserInputCalList()
            {
                UniformNumber = "12341111",
                Id = "測試人員1號",
                ProjectName = "測試台積碳排量紀錄1",
                ProjectDate = date1,
                ProjectTime = DateFormat.ToDate12(date1),
                T1Co2 = 50,
                T2Co2 = 30,
                T3Co2 = 24.26,
                T4Co2 = 20,
                T5Co2 = 10,
                T6Co2 = 30,
                TotalCo2 = 164.26,
            });

            DateTime date2 = DateTime.Parse("2024/10/05 11:15");
            result.Add(new UserInputCalList()
            {
                UniformNumber = "12341111",
                Id = "測試人員1號",
                ProjectName = "測試台積碳排量紀錄2",
                ProjectDate = date2,
                ProjectTime = DateFormat.ToDate12(date2),
                T1Co2 = 20,
                T2Co2 = 70,
                T3Co2 = 30.5,
                T4Co2 = 5,
                T5Co2 = 8,
                T6Co2 = 11,
                TotalCo2 = 144.5,
            });

            DateTime date3 = DateTime.Parse("2024/11/01 15:20");
            result.Add(new UserInputCalList()
            {
                UniformNumber = "12341111",
                Id = "測試人員1號",
                ProjectName = "測試台積碳排量紀錄3",
                ProjectDate = date3,
                ProjectTime = DateFormat.ToDate12(date3),
                T1Co2 = 18,
                T2Co2 = 52,
                T3Co2 = 33,
                T4Co2 = 3,
                T5Co2 = 7,
                T6Co2 = 10,
                TotalCo2 = 123,
            });

            DateTime date4 = DateTime.Parse("2024/11/01 16:00");
            result.Add(new UserInputCalList()
            {
                UniformNumber = "12341111",
                Id = "測試人員1號",
                ProjectName = "測試台積碳排量紀錄4",
                ProjectDate = date4,
                ProjectTime = DateFormat.ToDate12(date4),
                T1Co2 = 20,
                T2Co2 = 40,
                T3Co2 = 50,
                T4Co2 = 6,
                T5Co2 = 9,
                T6Co2 = 15,
                TotalCo2 = 140,
            });
            
            return result;
        }
    }

    public class UserInputCalList
    {
        //SYS_COMPANY
        [Display(Name = "統一編號")]
        [ColumnDef(Filter = true)]
        public string UniformNumber { get; set; }

        //User_Properties_Advance
        [Display(Name = "會員帳號")]
        [ColumnDef(Filter = true)]
        public string Id { get; set; }

        //User_Input_Advance
        [Display(Name = "專案名稱")]
        [ColumnDef(Filter = true)]
        public string ProjectName { get; set; }

        //User_Input_Advance
        [Display(Name = "日期")]
        [ColumnDef(EditType = EditType.Date)]
        public DateTime ProjectDate { get; set; }

        //User_Input_Advance
        [Display(Name = "時間")]
        public string ProjectTime { get; set; }

        [Display(Name = "類別1")]
        public double T1Co2 { get; set; }

        [Display(Name = "類別2")]
        public double T2Co2 { get; set; }

        [Display(Name = "類別3")]
        public double T3Co2 { get; set; }

        [Display(Name = "類別4")]
        public double T4Co2 { get; set; }

        [Display(Name = "類別5")]
        public double T5Co2 { get; set; }

        [Display(Name = "類別6")]
        public double T6Co2 { get; set; }

        //User_Input_Advance
        [Display(Name = "總排放量")]
        public double TotalCo2 { get; set; }
    }
}