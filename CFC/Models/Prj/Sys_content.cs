using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CFC.Models.Prj
{
    /// <summary>
    /// 系統內容
    /// </summary>
    public class Sys_content
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int Id { get; set; }

        [Display(Name = "代碼")]
        public string Code { get; set; }

        [Display(Name = "名稱")]
        public string Name { get; set; }

        /// <summary>
        /// 系統內容細項
        /// </summary>
        [NotMapped]
        public ICollection<Sys_contentDetail> Details 
        {
            get
            {
                return Sys_contentDetail.GetAllDatas().Where(a => a.ContentId == this.Id).ToList();
            }
        }
    }
}