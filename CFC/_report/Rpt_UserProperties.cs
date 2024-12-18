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
                string fileTitle = "會員清單";                
                string folder = WebConfigurationManager.AppSettings["FileRoot"].ToString() + "File/ExcelCreater/tempFolder/";

                //產出Dynamic資料 (給Excel)
                List<dynamic> list = new List<dynamic>();

                int serial = 1;
                foreach (var data in datas)
                {
                    dynamic f = new ExpandoObject();
                    f.序號 = serial;
                    serial++;
                    f.姓名 = data.Name;   //ooooooooooo                    
                    f.公司名稱 = data.UniformNumber;
                    f.統一編號 = data.UniformNumberNo;
                    f.公司規模 = data.CompanySizeNew;
                    f.聯絡人 = data.Contact;
                    f.職稱 = data.POSITION;
                    f.連絡電話 = data.PhoneNumber;
                    f.行業別 = data.IndustrialTypeName;
                    f.單位性質 = data.UNIT_TYPE;
                    f.縣市 = data.CITY;
                    f.鄉鎮市區 = data.DISTRICT;
                    f.地址 = data.ADDRESS;

                    f.SheetName = fileTitle;//sheep.名稱;
                    list.Add(f);
                }

                //查無符合資料表數
                if (list.Count == 0)
                {
                    _errorMessage = "查無符合資料表數";
                }

                List<string> titles = new List<string>();

                //"0":不調整width,"1":自動調整長度(效能差:資料量多),"2":字串長度調整width,"3":字串長度調整width(展開)
                int autoSizeColumn = 2;

                //產出excel
                string fileName = ExcelSpecHelper.GenerateExcelByLinqF1(fileTitle, titles, list, folder, autoSizeColumn);
                string path = folder + fileName;

                string tmpRootDir = WebConfigurationManager.AppSettings["FileRoot"].ToString();
                url = WebConfigurationManager.AppSettings["SiteRoot"].ToString() + Cm.PhysicalToUrl(path, tmpRootDir);
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