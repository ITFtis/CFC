using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DouHelper;
using System.Windows.Media.Media3D;

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

        static object lockGetAllDatas = new object();
        public static IEnumerable<Log_count> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;
   
            string key = "CFC.Models.Prj.Log_count";
            var allData = DouHelper.Misc.GetCache<IEnumerable<Log_count>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<Log_count> modle = new Dou.Models.DB.ModelEntity<Log_count>(new DouModelContext());
                    allData = modle.GetAll().OrderByDescending(a => a.BDate).ToArray();

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "CFC.Models.Prj.Log_count";
            Misc.ClearCache(key);
        }
    }
}