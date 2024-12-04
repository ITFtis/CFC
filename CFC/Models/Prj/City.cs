using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Antlr.Runtime;
using Newtonsoft.Json;

namespace CFC.Models.Prj
{
    /// <summary>
    /// 縣市
    /// </summary>
    [Table("City")]
    public class City
    {
        [Key]
        [ColumnDef(Display = "縣市代碼")]
        [StringLength(6)]
        public string CityCode { get; set; }

        [Required]
        [StringLength(30)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "縣市名稱(中)")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "排序")]
        public int Sort { get; set; }
    }

    public class CitySelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public const string AssemblyQualifiedName = "CFC.Models.Prj.CitySelectItems, CFC";

        protected static IEnumerable<City> _cites;
        internal static IEnumerable<City> CITIES
        {
            get
            {
                if (_cites == null)
                {
                    using (var db = new DouModelContext())
                    {
                        _cites = db.City.OrderBy(a => a.Sort).ToArray();
                    }
                }
                return _cites;
            }
        }


        public static void Reset()
        {
            _cites = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            //return CITIES.Select(s => new KeyValuePair<string, object>(s.CityCode + "", s.Name));
            return CITIES.Select(s => new KeyValuePair<string, object>(s.CityCode, JsonConvert.SerializeObject(new { v = s.Name, s = s.Sort })));
        }
    }

    public class CityByNameSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public const string AssemblyQualifiedName = "CFC.Models.Prj.CityByNameSelectItems, CFC";
       
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            //return CitySelectItems.CITIES.Select(s => new KeyValuePair<string, object>(s.Name + "", s.Name));
            return CitySelectItems.CITIES.Select(s => new KeyValuePair<string, object>(s.Name, JsonConvert.SerializeObject(new { v = s.Name, s = s.Sort })));
        }
    }
}