using CFC.Models.Manager;
using CFC.Models.Prj;
using DouHelper;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http.Results;

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
                var oldFiles = ZipHelper.GetFiles(epFolder);
                foreach (var fName in oldFiles)
                {
                    string str = Path.GetFileNameWithoutExtension(fName.ToString()).Split('_')[0];
                    DateTime delDate = DateTime.MinValue;
                    DateTime today = DateTime.Now.Date;
                    if (DateTime.TryParse(str, out delDate))
                    {
                        if (delDate < today)
                        {
                            //移除舊的.zip
                            if (Path.GetExtension(fName.ToString()) == ".zip")
                                System.IO.File.Delete(fName.ToString());
                        }
                    }
                }

                //指定儲存資料夾路徑
                DateTime date = DateTime.Now;
                string new_folderName = DateFormat.ToDate15(date) + "_" + Dou.Context.CurrentUser<User>().Id;
                
                string to_folder = epFolder + new_folderName + "/";
                if (!Directory.Exists(to_folder))
                {
                    Directory.CreateDirectory(to_folder);
                }

                //2.清冊ListFolder (to_folder目錄下全部)
                int sno = 1;
                foreach (var f in datas)
                {
                    string newTemptAdd = sno.ToString() + "_" + f.UserID + "_" + f.ProjectName + "_" + f.StartDate_F;
                    Controllers.FileDownload.ExcelManagerF.ReturnModel result = new Controllers.FileDownload.ExcelManager().GetReportValExcel(to_folder, f.UserID, f.FACTORY_REGISTRATION, f, newTemptAdd);
                    sno++;

                    if (sno > 2)
                        break;
                }

                //資料夾(to_folder)使用中，卡住
                System.Threading.Thread.Sleep(300);

                //3.壓縮(.zip)
                bool doZip = ZipHelper.ZipFiles(to_folder, new_folderName, string.Empty, string.Empty);
                if (!doZip)
                {
                    _errorMessage = "壓縮zip失敗";
                    return "";
                }

                //移動壓縮檔案(.zip)
                string FromPath = to_folder + new_folderName + ".zip";
                string GoPath = epFolder + new_folderName + ".zip";
                System.IO.File.Move(FromPath, GoPath);

                //4.(目錄)刪除ListFolder (to_folder目錄)
                Directory.Delete(to_folder, true);

                //5.回傳zip路徑
                url = WebConfigurationManager.AppSettings["SiteRoot"].ToString() + Cm.PhysicalToUrl(GoPath);
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