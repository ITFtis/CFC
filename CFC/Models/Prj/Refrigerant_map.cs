using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CFC.Models.Prj
{
    public class Refrigerant_map
    {
        /// <summary>
        /// 冷媒設備編號
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [Display(Name = "冷媒設備編號")]
        public string Refrigerant_equipId { get; set; }
        /// <summary>
        /// 冷媒種類編號
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [Display(Name = "冷媒種類編號")]
        public string Refrigerant_typeId { get; set; }
        /// <summary>
        /// 順序
        /// </summary>
        [Display(Name = "順序")]
        public int displayOrder { get; set; }
    }
}