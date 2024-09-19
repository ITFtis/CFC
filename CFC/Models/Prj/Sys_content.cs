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
        public int Id { get; set; }

        [Display(Name = "代碼")]
        public int Code { get; set; }

        [Display(Name = "名稱")]
        public string Name { get; set; }

        /// <summary>
        /// 系統內容細項
        /// </summary>
        [NotMapped]
        public virtual ICollection<Sys_contentDetail> Details { get; set; }
    }
}