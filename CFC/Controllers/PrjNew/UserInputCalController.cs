﻿using CFC.Controllers.Api;
using CFC.Models;
using CFC.Models.Api;
using CFC.Models.Prj;
using Dou.Controllers;
using Dou.Misc;
using Dou.Misc.Attr;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace CFC.Controllers.PrjNew
{
    [Dou.Misc.Attr.MenuDef(Id = "UserInputCal", Name = "會員計算紀錄明細", MenuPath = "管理", Action = "Index", Index = 3, Func = Dou.Misc.Attr.FuncEnum.None, AllowAnonymous = false)]
    public class UserInputCalController : APaginationModelController<vw_UserInputCal>
    {
        // GET: UserInputProject
        public ActionResult Index()
        {
            return View();
        }


        protected override Dou.Models.DB.IModelEntity<vw_UserInputCal> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<vw_UserInputCal>(new DouModelContext());
        }

        protected override IQueryable<vw_UserInputCal> BeforeIQueryToPagedList(IQueryable<vw_UserInputCal> iquery, params KeyValueParams[] paras)
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

            //預設查詢帳號
            string defaultUserID = "";

            //有儲存專案
            var iquery = GetModelEntity().GetAll();
            iquery = iquery.Where(a => a.IsSave);
            iquery = iquery.Where(a => a.UserID != "查無此紀錄");
            //限定製造業
            var e = iquery.AsEnumerable();
            e = e.Where(a => a.IndustrialTypeId != "1");
            //有統一編號
            e = e.Where(a => !string.IsNullOrEmpty(a.UniformNumber));
            var v = e.FirstOrDefault();
            defaultUserID = v != null ? v.UserID : defaultUserID;

            //csts01
            opts.GetFiled("UserID").defaultvalue = defaultUserID;
            opts.GetFiled("StartDate_F").title = "開始日期";
            opts.GetFiled("ARType").title = "AR版本";

            opts.GetFiled("UniformNumber").visible = true;
            opts.GetFiled("UserID").visible = true;
            opts.GetFiled("StartDate_F").visible = true;
            opts.GetFiled("ProjectName").visible = true;
            opts.GetFiled("ARType").visible = true;
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

    public class vw_UserInputCal 
    {
        /// <summary>
        /// 編號(流水號)
        /// </summary>
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; set; }

        [Display(Name = "統一編號")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string UniformNumber
        {
            get
            {
                string str = "";
                var u = User_Properties_Advance.GetAllDatas().Where(a => a.Id == this.UserID).FirstOrDefault();
                if (u != null)
                {
                    str = u.UniformNumber;
                }

                return str;
            }
        }

        /// <summary>
        /// 使用者編號
        /// </summary>
        [Display(Name = "使用者編號")]
        [ColumnDef(FilterAssign = FilterAssignType.Contains)]
        public string UserID { get; set; }

        [NotMapped]
        public string UserName
        {
            get
            {
                string str = "";
                var v = User_Properties_Advance.GetAllDatas().Where(e => e.Id == this.UserID).FirstOrDefault();
                if (v != null)
                    str = v.Name;

                return str;

                //return new DouModelContext().userPropertiesAdvance.Where(e => e.Id == this.UserID).FirstOrDefault().Name;
            }
            set
            {
            }
        }

        /// <summary>
        /// 輸入時間
        /// </summary>
        [Display(Name = "輸入時間")]
        public System.DateTime Date { get; set; }

        [Display(Name = "電量年分")]
        public string elecYear { get; set; }

        [Display(Name = "用電量")]
        public Double elecVolume { get; set; }

        [Display(Name = "蒸氣用量")]
        public Double SteamVolume { get; set; }

        [Display(Name = "蒸氣係數")]
        public decimal SteamCoe { get; set; }
        
        [Display(Name = "工廠登記證")]
        public string FACTORY_REGISTRATION { get; set; }

        [Display(Name = "專案名稱")]
        public string ProjectName { get; set; }//專案名稱

        [Display(Name = "區間開始日期")]
        public string StartDate { get; set; }

        [Display(Name = "區間結束日期")]
        public string EndDate { get; set; }

        [Display(Name = "區間開始日期_日期格式")]
        public string StartDate_F
        {
            get
            {
                return DateFormat.ToDate14(this.StartDate);
            }
        }

        [Display(Name = "區間結束日期_日期格式")]
        public string EndDate_F
        {
            get
            {
                return DateFormat.ToDate14(this.EndDate);
            }
        }

        //虛擬欄位
        [Display(Name = "篩選開始日期(起)")]
        [ColumnDef(Visible = false, VisibleEdit = false, EditType = EditType.Date)]
        public DateTime FilterStartS
        {
            get
            {
                return DateFormat.ToDate14_2(this.StartDate);
            }
        }

        //虛擬欄位
        [Display(Name = "篩選開始日期(迄)")]
        [ColumnDef(Visible = false, VisibleEdit = false, EditType = EditType.Date)]
        public DateTime FilterStartE
        {
            get
            {
                return DateFormat.ToDate14_2(this.StartDate);
            }
        }

        [Display(Name = "ARType")]
        public string ARType { get; set; }

        //虛擬欄位
        [Display(Name = "行業別")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string IndustrialTypeId
        {
            get
            {
                string str = "";

                var v = User_Properties_Advance.GetAllDatas().Where(a => a.Id == this.UserID).FirstOrDefault();
                if (v != null)
                    str = v.IndustrialTypeId;

                return str;
            }
        }

        //虛擬欄位
        [Display(Name = "行業別名稱")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string IndustrialTypeName
        {
            get
            {
                string str = "";

                var v = User_Properties_Advance.GetAllDatas().Where(a => a.Id == this.UserID).FirstOrDefault();
                if (v != null)
                {
                    var fs = Code.GetYNGlobal_Industrial().Where(a => a.Key == v.IndustrialTypeId);
                    if (fs.Count() > 0)
                        str = fs.First().Value.ToString();                    
                }

                return str;
            }
        }

        [Display(Name = "建檔日")]
        public string BDate { get; set; }

        [Display(Name = "建檔者ID")]
        public string BId { get; set; }

        [Display(Name = "修改日")]
        public string UDate { get; set; }

        [Display(Name = "修改者ID")]
        public string UId { get; set; }

        [Display(Name = "Memo")]
        public string Memo { get; set; }



        // 類別3
        public double Tr01 { get; set; } // 類別3-運輸-上游原物料配送當量
        public double Tr02 { get; set; } // 類別3 - 運輸 - 商務旅遊
        public double Tr03 { get; set; } // 類別3 - 運輸 - 員工通勤
        public double Tr04 { get; set; } // 類別3 - 運輸 - 下游運輸及配送
        public double Cp01 { get; set; } // 類別4 - 組織使用產品 - 採購
        public double Cp02 { get; set; } // 類別4 - 組織使用產品 - 資本
        public double Cp03 { get; set; } // 類別4 - 組織使用產品 - 能源相關活動
        public double Cp04 { get; set; } // 類別4 - 組織使用產品 - 營運廢棄物
        public double Cp05 { get; set; } // 類別4 - 組織使用產品 - 上游資產租賃
        public double Us01 { get; set; } // 類別5 - 使用組織產品 - 加工
        public double Us02 { get; set; } // 類別5 - 使用組織產品 - 使用
        public double Us03 { get; set; } // 類別5 - 使用組織產品 - 報廢
        public double Us04 { get; set; } // 類別5 - 使用組織產品 - 下游租賃
        public double Us05 { get; set; } // 類別5 - 使用組織產品 - 加盟
        public double Us06 { get; set; } // 類別5 - 使用組織產品 - 投資
        public double Other { get; set; } // 類別6 - 其他排放

        [NotMapped]
        public double Type3Summary
        {
            get
            {
                return Tr01 + Tr02 + Tr03 + Tr04 +
                    Cp01 + Cp02 + Cp03 + Cp04 + Cp05 +
                    Us01 + Us02 + Us03 + Us04 + Us05 + Us06 +
                    Other;
            }
        }


        // 專案需求
        public bool IsSave { get; set; }// 是否儲存為專案

        public string ProjectIndustrialID { get; set; }//專案場登
        public string ProjectAddress { get; set; }//專案地址
        public string ProjectCity { get; set; }//專案縣市
        public string ProjectIndustrialType { get; set; }//專案行業別

        //會員計算記錄明細
        [Display(Name = "類別1")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public decimal VClass1
        {
            get
            {
                CalInputModel calInput = Rpt_UserInputCal.ToCalInputModel(this);

                decimal value = Rpt_UserInputCal.GetCal(1, calInput);
                return Math.Round(value, 2);
            }
        }

        [Display(Name = "類別2")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public decimal VClass2
        {
            get
            {
                CalInputModel calInput = Rpt_UserInputCal.ToCalInputModel(this);

                decimal value = Rpt_UserInputCal.GetCal(2, calInput);
                return Math.Round(value, 2);
            }
        }

        [Display(Name = "類別3")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public decimal VClass3
        {
            get
            {
                CalInputModel calInput = Rpt_UserInputCal.ToCalInputModel(this);

                decimal value = Rpt_UserInputCal.GetCal(3, calInput);
                return Math.Round(value, 2);
            }
        }

        [Display(Name = "類別4")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public decimal VClass4
        {
            get
            {
                CalInputModel calInput = Rpt_UserInputCal.ToCalInputModel(this);

                decimal value = Rpt_UserInputCal.GetCal(4, calInput);
                return Math.Round(value, 2);
            }
        }

        [Display(Name = "類別5")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public decimal VClass5
        {
            get
            {
                CalInputModel calInput = Rpt_UserInputCal.ToCalInputModel(this);

                decimal value = Rpt_UserInputCal.GetCal(5, calInput);
                return Math.Round(value, 2);
            }
        }

        [Display(Name = "類別6")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public decimal VClass6
        {
            get
            {
                CalInputModel calInput = Rpt_UserInputCal.ToCalInputModel(this);

                decimal value = Rpt_UserInputCal.GetCal(6, calInput);
                return Math.Round(value, 2);
            }
        }

        [Display(Name = "總排放量")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public decimal VClassTotal
        {
            get
            {
                return 0;
            }
        }
    }
}