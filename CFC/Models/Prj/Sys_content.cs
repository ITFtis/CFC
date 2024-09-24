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
                ICollection<Sys_contentDetail> result = Sys_contentDetail.GetAllDatas().Where(a => a.MapId == this.Id).ToList();

                //if (result.Count == 0)
                //{
                //    result = new List<Sys_contentDetail>() { new Sys_contentDetail { Title = "無資料，不可修改" } };
                //}

                return result;
            }
        }
    }
}