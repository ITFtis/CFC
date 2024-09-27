using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DouHelper;

namespace CFC.Models.Prj
{
    /// <summary>
    /// 3-6類別項目
    /// </summary>
    public class Cals_properties
    {
        /// <summary>
        /// 編號
        /// </summary>
        [Key]
        [Display(Name = "編號")]
        public string Id { get; set; }

        /// <summary>
        /// 3-6類別
        /// </summary>
        [Display(Name = "類別")]
        [ColumnDef(VisibleView = false, VisibleEdit = true, 
            EditType = EditType.Select, Filter = true,
            SelectItemsClassNamespace = CalsTypeSelectItems.AssemblyQualifiedName)]
        public string Type { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [Display(Name = "名稱")]
        public string Name { get; set; }

        /// <summary>
        /// 單位
        /// </summary>
        [Display(Name = "單位")]
        public string Unit { get; set; }

        /// <summary>
        /// 順序
        /// </summary>
        [Display(Name = "順序")]
        public int DisplayOrder { get; set; }

        static object lockGetAllDatas = new object();
        public static IEnumerable<Cals_properties> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;

            string key = "CFC.Models.Prj.Cals_properties";
            var allData = DouHelper.Misc.GetCache<IEnumerable<Cals_properties>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<Cals_properties> modle = new Dou.Models.DB.ModelEntity<Cals_properties>(new DouModelContext());
                    allData = modle.GetAll().ToArray();

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "CFC.Models.Prj.Cals_properties";
            Misc.ClearCache(key);
        }
    }
}