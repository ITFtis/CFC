//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CFC.Models.Prj
{
    using Dou.Misc.Attr;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("User_Properties_Advance")]
    public partial class User_Properties_Advance
    {
        /// <summary>
        /// 統一編號
        /// </summary>
        [Key]
        [Display(Name = "登入帳號", Order = 0)]
        //[ColumnDef(Visible = false)]
        public string Id { get; set; }

        /// <summary>
        /// 登入密碼
        /// </summary>
        [Display(Name = "登入密碼", Order = 1)]
        [ColumnDef(Visible = true , VisibleEdit =true)]
        public string Pass { get; set; }

        /// <summary>
        /// 統一編號
        /// </summary>
        [Display(Name = "統一編號")]
        [ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string UniformNumber { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        [Display(Name = "公司名稱")]
        [ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string Name { get; set; }

        /// <summary>
        /// 工商登記編號
        /// </summary>
       
        [Display(Name = "工商登記編號")]
        [ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string IndustryId { get; set; }

        [Display(Name = "縣市別")]
        [ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string CountyId { get; set; }

        [Display(Name = "工業園區")]
        [ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string IndustrialAreaId { get; set; }

        [Display(Name = "行業別")]
        [ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string IndustrialTypeId { get; set; }

        /// <summary>
        /// 公司規模
        /// </summary>
        [Display(Name = "公司規模")]
        public string CompanySize { get; set; }

        /// <summary>
        /// 聯絡人
        /// </summary>
        [Display(Name = "聯絡人")]
        public string Contact { get; set; }

        /// <summary>
        /// 連絡電話
        /// </summary>
        [Display(Name = "連絡電話")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 聯絡email
        /// </summary>
        [Display(Name = "聯絡email")]
        public string Email { get; set; }

        /// <summary>
        /// xxx
        /// </summary>
        [NotMapped]
        [Display(Name = "工廠")]
        public List<SYS_FACTORY> FactoryList { get; set; }

        [NotMapped]
        [Display(Name = "製造或非製造業")]
        public string Manufacturing { get; set; }

        [Display(Name = "縣市")]
        public string CITY { get; set; }

        [Display(Name = "鄉鎮市區")]
        public string DISTRICT { get; set; }

        [Display(Name = "地址")]
        public string ADDRESS { get; set; }

        [Display(Name = "單位性質")]
        public string UNIT_TYPE { get; set; }

        [Display(Name = "職稱")]
        public string POSITION { get; set; }
    }

    public class Fac
    {
        public string name { get; set; }
        public string sid { get; set; }
    }
}
