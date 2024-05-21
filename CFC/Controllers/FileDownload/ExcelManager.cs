using CFC.Controllers.FileDownload.ExcelManagerF;
using CFC.Models;
using CFC.Models.Api;
using CFC.Models.Prj;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using LicenseContext = OfficeOpenXml.LicenseContext;


namespace CFC.Controllers.FileDownload
{
    internal class ExcelManager
    {
        private DouModelContext db = new DouModelContext();
        public ReturnModel DownloadExcel(SaveProjectModel saveProject) {

            // 確認輸入資料是否正確
            var userInput = this.db.userInputAdvance.Where(e => e.RowID == saveProject.RowID
                        && e.UserID == saveProject.UserID).FirstOrDefault();
            if (userInput == null)
                return new ReturnModel { isSucess = false, fileAdd = "查無此紀錄" };

            // 取得模板檔案
            String temptFileAdd = WebConfigurationManager.AppSettings["ExcelTemplate"].ToString();

            // 取得工作目錄
            String newTemptFolder = WebConfigurationManager.AppSettings["ExcelFolder"].ToString();
            String newTemptAdd = getTemptFile(newTemptFolder , "xlsx");

            // 複製到工作目錄中
            var newFileInfo = new FileInfo(temptFileAdd).CopyTo(newTemptFolder + "\\" + newTemptAdd);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // 產製檔案內容
            try {
                ExcelPackage Ep = new ExcelPackage(newFileInfo);

                
                var itemsSheet = Ep.Workbook.Worksheets["排放量計算"];
                new ItemManger().SetItems(itemsSheet , userInput);

                var stasticSheet = Ep.Workbook.Worksheets["碳盤查彙整表"];
                new StasticsManager().SetStatistics(stasticSheet , userInput);

                Ep.Save();


                // 取得回傳路徑
                string ppp = "File/ExcelCreater/tempFolder/" + newTemptAdd;
                
                ////string siteRoot = WebConfigurationManager.AppSettings["SiteRoot"].ToString();                
                ////string fileAdd = siteRoot + ppp;
                
                //to(ODF轉檔路徑)
                string from = WebConfigurationManager.AppSettings["FileRoot"].ToString() + ppp;
                string to_noExt = Path.GetDirectoryName(from) + "/" + Path.GetFileNameWithoutExtension(from);
                string to = "";

                //轉ODF
                switch (Path.GetExtension(from))
                {
                    case ".xlsx":
                        to = to_noExt + ".ods";                        
                        break;
                    default: 
                        break;
                }

                if (to == "")
                {
                    return new ReturnModel { isSucess = false, fileAdd = "查無ODF轉換程式碼：" + from };
                }
                else
                {
                    bool done = ODFHelper.ExcelToODF(from, to_noExt);
                    if (!done)
                    {
                        return new ReturnModel { isSucess = false, fileAdd = "ODF轉換失敗" };
                    }
                    else
                    {
                        string fileAdd = WebConfigurationManager.AppSettings["SiteRoot"].ToString() + Cm.PhysicalToUrl(to);
                        //string fileAdd = Cm.PhysicalToUrl(to);
                        return new ReturnModel { isSucess = true, fileAdd = fileAdd };
                    }
                }

                //return new ReturnModel { isSucess = true, fileAdd = fileAdd };
            }
            catch (Exception e) {
                Logger.Log.For(null).Error("下載錯誤：" + e.Message);
                Logger.Log.For(null).Error(e.StackTrace);
                return new ReturnModel { isSucess = false, fileAdd = e.Message };
            }
        }
        

        /*
            統計資訊相關
         */
        //========================================================================

        public List<FuelInput> GetFuelInputs(User_Input_Advance input)
        {
            var list =  from volume in this.db.FuelVolumes
                   join property in this.db.FuelProperties
                       on volume.FuelId equals property.Id
                   where volume.RowId == input.RowID
                   select new FuelInput
                   {
                       volume = volume,
                       property = property
                   };

            return list.ToList();
        }

        public List<RefrigInput> GetRefrigInputs(User_Input_Advance input)
        {
            var list = from volumn in this.db.RefrigerantVolume
                       join equip in this.db.RefrigerantEquip
                           on volumn.RefrigerantEquip equals equip.Id
                       join property in this.db.RefrigerantType
                           on volumn.RefrigerantType equals property.Id
                       where volumn.RowId == input.RowID
                       select new RefrigInput
                       {
                           volume = volumn,
                           equip = equip,
                           property = property
                       };
            return list.ToList();
        }

        public List<ExcelManagerF.EscapeInput> GetEscapeInputs(User_Input_Advance input)
        {
            var list = from volumn in this.db.EscapeVolume
                       join property in this.db.EscapeProperties
                        on volumn.EscapeId equals property.Id
                       where volumn.RowId == input.RowID
                       select new ExcelManagerF.EscapeInput
                       {
                           volume = volumn,
                           property = property
                       };
            return list.ToList();
        }

        public List<CreateInput> GetCreateInputs(User_Input_Advance input) { 
            var list = from volumn in this.db.SpecificVolume
                       join property in this.db.SpecificProperties
                       on volumn.CreateId equals property.Id
                       where volumn.RowId == input.RowID
                       select new CreateInput
                       {
                           volume = volumn,
                           property = property
                       };
            return list.ToList();
        }

        /*
         檔案複製相關
         */
        //=========================================================================
        public string getTemptFile(String folder, String extension) {
            String randomString = this.getRandom(10);

            var newFile = new FileInfo(folder + "\\" + randomString + "." + extension);
            if(newFile.Exists)
                return this.getTemptFile(folder, extension);

            return randomString + "." + extension;
        }

        public string getRandom(int num) {
            StringBuilder sb = new StringBuilder();
            Random random = new Random();
            for (int index = 0; index < num; index++) {
                sb.Append(random.Next(0,9));
            }
            return sb.ToString();
        }

    }
}