using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CFC.Models.Api
{
    public class ApiResultReturn
    {
        #region 計算回傳model
        /// <summary>
        /// 計算結果物件
        /// </summary>
        public class CFCResult
        {
            /// <summary>
            /// 計算成功(bool)
            /// </summary>
            public bool Success { set; get; }
            /// <summary>
            /// 訊息
            /// </summary>
            public string Message { set; get; }

            public CalResult Result { set; get; }
        }
        /// <summary>
        /// 各排放值資料
        /// </summary>
        public class CalResult
        {
            // 回傳對應的
            public int RowID { get; set; }

            /// <summary>
            /// 直接排放-固定排放
            /// </summary>
            public decimal R1 { set; get; }
            /// <summary>
            /// 直接排放-固定排放占總比
            /// </summary>
            public decimal R1_R { set; get; }

            /// <summary>
            /// 直接排放-移動排放占總比
            /// </summary>
            public decimal R2_R { set; get; }
            /// <summary>
            /// 直接排放-移動排放
            /// </summary>
            public decimal R2 { set; get; }

            /// <summary>
            /// 直接排放-逸散排放占總比
            /// </summary>
            public decimal R3_R { set; get; }
            /// <summary>
            /// 直接排放-逸散排放
            /// </summary>
            public decimal R3 { set; get; }


            /// <summary>
            /// 能源間接排放
            /// </summary>
            public decimal R4 { set; get; }
            /// <summary>
            /// 能源間接排放占總比
            /// </summary>
            public decimal R4_R { set; get; }
            /// <summary>
            /// 總排放
            /// </summary>
            public decimal R5 { set; get; }

            /// <summary>
            /// 其他排放
            /// </summary>
            public decimal R6 { get; set; }

            /// <summary>
            /// 其他排放(比例)
            /// </summary>
            public decimal R6_R { get; set; }


            /// <summary>
            /// 直接排放量
            /// </summary>
            public decimal R1_3 { set; get; }
            /// <summary>
            /// 直接排放量占總比
            /// </summary>
            public decimal R1_3_R { set; get; }
        }
        #endregion
    }
}