using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CFC.Models.Prj
{
    /// <summary>
    /// 系統內容細項
    /// </summary>
    public class Sys_contentDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }

        [Display(Name = "對應系統內容編號")]
        public int MapId { get; set; }

        [Display(Name = "標題")]
        public string Title { get; set; }

        [Display(Name = "說明")]
        public string Note { get; set; }
    }
}