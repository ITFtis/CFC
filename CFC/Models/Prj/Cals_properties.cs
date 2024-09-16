using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CFC.Models.Prj
{
    /// <summary>
    /// 3-6類別項目
    /// </summary>
    public class Cals_properties
    {
        /// <summary>
        /// 編號
        /// </summary>
        [Key]
        [Display(Name = "編號")]
        public string Id { get; set; }

        /// <summary>
        /// 3-6類別
        /// </summary>
        [Display(Name = "3-6類別")]
        [ColumnDef(VisibleView = false, VisibleEdit = true, EditType = EditType.Select,
            SelectSourceDbContextNamespace = "CFC.Models.DouModelContext, CFC",
            SelectSourceModelNamespace = "CFC.Models.Prj.Cals_type, CFC",
            SelectSourceModelValueField = "Id",
            SelectSourceModelDisplayField = "Name")]
        public string Type { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [Display(Name = "名稱")]
        public string Name { get; set; }

        /// <summary>
        /// 單位
        /// </summary>
        [Display(Name = "單位")]
        public string Unit { get; set; }

        /// <summary>
        /// 順序
        /// </summary>
        [Display(Name = "順序")]
        public int DisplayOrder { get; set; }
    }
}