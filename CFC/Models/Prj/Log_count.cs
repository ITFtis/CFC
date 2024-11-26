using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CFC.Models.Prj
{
    public class Log_count
    {
        /// <summary>
        /// 編號(流水號)
        /// </summary>
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }

        /// <summary>
        /// 類型(1.計算2.下載計算3.儲存專案)
        /// </summary>
        [Display(Name = "類型")]
        public int Type { get; set; }

        /// <summary>
        /// 對應關聯編號(ex.User_Input_Advance=>RowId)
        /// </summary>
        [Display(Name = "對應關聯編號")]
        public int? MapId { get; set; }

        /// <summary>
        /// 建檔日
        /// </summary>
        [Display(Name = "建檔日")]
        public DateTime BDate { get; set; }

        /// <summary>
        /// 建檔者
        /// </summary>
        [Display(Name = "建檔者")]
        public string BId { get; set; }
    }
}