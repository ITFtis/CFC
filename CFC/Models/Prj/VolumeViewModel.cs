using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFC.Models.Prj
{
    public class VolumeViewModel
    {
        [Key]
        public int IDX { get; set; }

        public string Category { get; set; }    // Fuel, Escape, Refrigerant, Specific 等
        public string RowId { get; set; }          // RowId
        public string UseVolume { get; set; }  // 使用量
        public string Type { get; set; }        // 類型
        public string TypeName { get; set; }    // 類型名稱
        public string TypeID { get; set; }         // 類型 ID
        public string Name { get; set; }        // 名稱
    }
}