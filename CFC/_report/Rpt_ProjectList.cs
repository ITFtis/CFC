using CFC.Models.Manager;
using CFC.Models.Prj;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace CFC
{
    public class Rpt_ProjectList
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
        /// 產出專案清冊
        /// </summary>
        /// <returns></returns>
        public string Export(List<User_Input_Advance> datas)
        {
            string url = "";

            if (datas == null)
            {
                _errorMessage = "執行失敗(datas)：Null";
                return "";
            }
            else if (datas.Count == 0)
            {
                _errorMessage = "執行失敗：無專案資料";
                return "";
            }

            try
            {
                int n = 0;

                //var f = datas.First();

                //指定儲存資料夾路徑(清冊匯出作業區)
                string epFolder = WebConfigurationManager.AppSettings["FileRoot"].ToString() + "File/ExcelCreater/tempFolder/epFolder/";
                if (!Directory.Exists(epFolder))
                {
                    Directory.CreateDirectory(epFolder);
                }

                //1.刪除非今日.zip

                //指定儲存資料夾路徑
                DateTime date = DateTime.Now;
                string to_folder = epFolder + DateFormat.ToDate15(date) + "_" + Dou.Context.CurrentUser<User>().Id + "/";
                if (!Directory.Exists(to_folder))
                {
                    Directory.CreateDirectory(to_folder);
                }

                //2.清冊ListFolder (to_folder目錄下全部)
                int sno = 1;
                foreach (var f in datas)
                {
                    //////debug
                    ////if (sno >= 180 && sno <= 200)
                    ////{
                    ////    string newTemptAdd = sno.ToString() + "_" + f.UserID + "_" + f.ProjectName + "_" + f.StartDate_F;
                    ////    Controllers.FileDownload.ExcelManagerF.ReturnModel result = new Controllers.FileDownload.ExcelManager().GetReportValExcel(to_folder, f.UserID, f.FACTORY_REGISTRATION, f, newTemptAdd);                      
                    ////}

                    string newTemptAdd = sno.ToString() + "_" + f.UserID + "_" + f.ProjectName + "_" + f.StartDate_F;
                    Controllers.FileDownload.ExcelManagerF.ReturnModel result = new Controllers.FileDownload.ExcelManager().GetReportValExcel(to_folder, f.UserID, f.FACTORY_REGISTRATION, f, newTemptAdd);
                    sno++;

                    ////if (sno > 2)
                    ////    break;
                }

                //3.壓縮(.zip)

                //4.刪除ListFolder

                //5.回傳zip路徑

                url = "sssss";

                //if (result.isSucess)
                //{
                //    url = result.fileAdd;
                //}
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