using CFC.Models.Manager;
using CFC.Models.Prj;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace CFC
{
    public class Rpt_UserProperties
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string _errorMessage = "";

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
        }

        /// <summary>
        /// 會員匯出清單
        /// </summary>
        /// <returns></returns>
        public string Export(List<User_Properties_Advance> datas)
        {
            string url = "";

            try
            {
                url = "xxsss";
            }
            catch (Exception ex)
            {
                _errorMessage = "執行失敗：" + ex.Message;
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);

                return "";
            }

            return url;
        }
    }
}