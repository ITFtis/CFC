using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFC.Models.Prj
{
    using Dou.Misc.Attr;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SYS_FACTORY")]

    public partial class SYS_FACTORY
    {

        /// <summary>
        /// 主鍵，自動遞增的工廠編號
        /// </summary>
        //[Key]
        //[Display(Name = "主鍵，自動遞增的工廠編號", Order = 0)]
        //[ColumnDef(Visible = false, VisibleEdit = false)]
        //[ColumnDef(Visible = false)]
        //public string FACTORY_ID { get; set; }

        [Display(Name = "工廠名稱", Order = 0)]
        [ColumnDef(Visible = true, VisibleEdit = true)]
        public string FACTORY_NAME { get; set; }

        [Key]
        [Display(Name = "登記證", Order = 2)]
        //[ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string FACTORY_REGISTRATION { get; set; }

        [Display(Name = "縣市別", Order = 3)]
        public string FACTORY_CITY { get; set; }


        [Display(Name = "鄉鎮市區", Order = 4)]
        public string FACTORY_DISTRICT { get; set; }

        [Display(Name = "地址", Order = 5)]
        public string FACTORY_ADDRESS { get; set; }

        [Display(Name = "工業區", Order = 6)]
        public string FACTORY_INDUSTRIAL_AREA { get; set; }

        [Display(Name = "工廠產業類型", Order = 7)]
        public string FACTORY_INDUSTRIAL { get; set; }

        [Display(Name = "建檔日", Order = 8)]
        public string BDate { get; set; }

        [Display(Name = "建檔者ID", Order = 9)]
        public string BId { get; set; }

        [Display(Name = "修改日", Order = 10)]
        public string UDate { get; set; }

        [Display(Name = "修改者ID", Order = 11)]
        public string UId { get; set; }


    }
}