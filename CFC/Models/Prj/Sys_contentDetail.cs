using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DouHelper;
using Dou.Misc.Attr;

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
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int Id { get; set; }

        [Display(Name = "對應系統內容編號")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int ContentId { get; set; }

        [Display(Name = "標題")]
        public string Title { get; set; }

        [Display(Name = "說明")]
        public string Note { get; set; }

        static object lockGetAllDatas = new object();
        public static IEnumerable<Sys_contentDetail> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;
            
            string key = "CFC.Models.Prj.Sys_contentDetail";
            var allData = DouHelper.Misc.GetCache<IEnumerable<Sys_contentDetail>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<Sys_contentDetail> modle = new Dou.Models.DB.ModelEntity<Sys_contentDetail>(new DouModelContext());
                    allData = modle.GetAll().ToArray();

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "CFC.Models.Prj.Sys_contentDetail";
            Misc.ClearCache(key);
        }
    }
}