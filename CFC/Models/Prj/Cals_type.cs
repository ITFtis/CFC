using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CFC.Models.Prj
{
    /// <summary>
    /// 3-6類別
    /// </summary>
    public class Cals_type
    {
        /// <summary>
        /// 編號
        /// </summary>
        [Key]
        [Display(Name = "編號")]
        public string Id { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [Display(Name = "名稱")]
        public string Name { get; set; }

        /// <summary>
        /// 順序
        /// </summary>
        [Display(Name = "順序")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 3-6類別項目
        /// </summary>
        [NotMapped]
        public ICollection<Cals_properties> Details
        {
            get
            {
                return Cals_properties.GetAllDatas().Where(a => a.Type == this.Id).ToList();
            }
        }
    }

    public class CalsTypeSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public const string AssemblyQualifiedName = "CFC.Models.Prj.CalsTypeSelectItems, CFC";

        protected static IEnumerable<Cals_type> _calsTypeSelects;
        internal static IEnumerable<Cals_type> CalsTypeSelects
        {
            get
            {
                if (_calsTypeSelects == null)
                {
                    using (var db = new DouModelContext())
                    {
                        _calsTypeSelects = db.CalsType.OrderBy(a => a.DisplayOrder).ToArray();
                    }
                }
                return _calsTypeSelects;
            }
        }


        public static void Reset()
        {
            _calsTypeSelects = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return CalsTypeSelects.Select(s => new KeyValuePair<string, object>(s.Id.ToString(), s.Name));
        }
    }
}