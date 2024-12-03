using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace CFC.Models.Prj
{
    /// <summary>
    /// 鄉鎮
    /// </summary>
    [Table("Town")]
    public class Town
    {
        [Key]
        [Display(Name = "鄉鎮代碼")]
        [ColumnDef(Index = 4)]
        [StringLength(5)]
        public string ZIP { get; set; }  //郵遞區號

        [Required]
        [Display(Name = "縣市")]
        [ColumnDef(Index = 1, Filter = true, Sortable = true, EditType = EditType.Select, SelectItemsClassNamespace =  CitySelectItems.AssemblyQualifiedName)]
        [StringLength(6)]
        public string CityCode { get; set; }

        [Required]
        [Display(Name = "鄉鎮名稱(中)")]
        [Column(TypeName = "nvarchar")]
        [ColumnDef(Index = 2)]
        [StringLength(20)]
        public string Name { get; set; }

        [Display(Name = "縣市排序")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int CitySort
        {
            get
            {
                int index = 0;
                var city = CitySelectItems.CITIES.Where(a => a.CityCode == this.CityCode).FirstOrDefault();
                if (city != null)
                    index = city.Sort;

                return index;
            }
        }
    }

    public class TownSelectItems : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "CFC.Models.Prj.TownSelectItems, CFC";

        protected static IEnumerable<Town> _towns;
        internal static IEnumerable<Town> Towns
        {
            get
            {
                if (_towns == null || _towns.Count() == 0)
                {
                    using (var db = new DouModelContext())
                    {
                        _towns = db.Town.ToArray();
                    }
                }
                return _towns;
            }
        }


        public static void Reset()
        {
            _towns = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            //return Towns.Select(s => new KeyValuePair<string, object>(s.ZIP, "{\"v\":\"" + s.Name + "\",\"CityCode\":\"" + s.CityCode + "\",\"PCityCode\":\"" + s.CityCode + "\"}"));
            return Towns.Select(s => new KeyValuePair<string, object>(s.CityCode + "", s.Name));            
        }
    }
}