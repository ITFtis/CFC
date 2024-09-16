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

    [Table("SYS_COMPANY")]

    public partial class SYS_COMPANY
    {
        /// <summary>
        /// 主鍵，自動遞增的公司編號
        /// </summary>
        [Key]
        [Display(Name = "主鍵，自動遞增的公司編號", Order = 0)]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        //[ColumnDef(Visible = false)]
        public string COMP_ID { get; set; }

        [Display(Name = "工廠名稱", Order = 1)]
        [ColumnDef(Visible = true, VisibleEdit = true)]
        public string COMP_NAME { get; set; }

        [Display(Name = "統一編號")] 
        //[ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string COMP_UNIFORM_NUMBER { get; set; }

        [Display(Name = "公司規模")] //中小企業, 或大企業
        //[ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string COMP_SIZE { get; set; }

        [Display(Name = "建檔日")]
        public string BDate { get; set; }

        [Display(Name = "建檔者ID")]
        public string BId { get; set; }

        [Display(Name = "修改日")]
        public string UDate { get; set; }

        [Display(Name = "修改者ID")]
        public string UId { get; set; }


    }
}