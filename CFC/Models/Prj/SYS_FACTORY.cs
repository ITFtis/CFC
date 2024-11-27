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
        [Key]
        [Display(Name = "編號", Order = 0)]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int FACTORY_ID { get; set; }

        [Display(Name = "工廠名稱")]
        [ColumnDef(Visible = true, VisibleEdit = true)]
        public string FACTORY_NAME { get; set; }

        [Display(Name = "登記證")]
        //[ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string FACTORY_REGISTRATION { get; set; }

        [Display(Name = "縣市別")]
        public string FACTORY_CITY { get; set; }


        [Display(Name = "鄉鎮市區")]
        public string FACTORY_DISTRICT { get; set; }

        [Display(Name = "地址")]
        public string FACTORY_ADDRESS { get; set; }

        [Display(Name = "工業區")]
        public string FACTORY_INDUSTRIAL_AREA { get; set; }

        [Display(Name = "工廠產業類型")]
        public string FACTORY_INDUSTRIAL { get; set; }

        [Display(Name = "建檔日")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string BDate { get; set; }

        [Display(Name = "建檔者ID")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string BId { get; set; }

        [Display(Name = "修改日")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string UDate { get; set; }

        [Display(Name = "修改者ID")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string UId { get; set; }


    }
}