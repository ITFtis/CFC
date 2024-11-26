using CFC.Controllers.FileDownload.ExcelManagerF;
using CFC.Models;
using CFC.Models.Api;
using CFC.Models.Prj;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
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

        public List<VolumeViewModel> GetVolumeData(int rowId)
        {
            string query = @"
                        SELECT CAST(ROW_NUMBER() OVER (ORDER BY Category, RowId) AS INT) AS IDX,  -- 生成自動遞增的 Key 值
                               -- 這裡會包括 Category, RowId, UseVolume, Type, TypeID, TypeName, Name 等欄位
                                CAST(Category AS VARCHAR(50)) AS Category,
                                CAST(RowId AS VARCHAR(50)) AS RowId,
                                CAST(UseVolume AS VARCHAR(50)) AS UseVolume,  -- 將 UseVolume 轉為字串
                                CAST(Type AS VARCHAR(50)) AS Type,
                                CAST(TypeID AS VARCHAR(50)) AS TypeID,
                                CAST(TypeName AS VARCHAR(50)) AS TypeName,
                                CAST(NAME AS VARCHAR(50)) AS NAME
                FROM (
                        SELECT '01_Fuel' AS Category, A.RowId, A.UseVolume,'' AS Type ,'' AS TypeName, A.FuelId AS TypeID, 
                               B.Name AS Name
                        FROM [dbo].[Fuel_volume] A 
                        JOIN [dbo].[Fuel_properties] B 
                        ON A.FuelId = B.Id 
                        WHERE A.RowId = @p0
                        UNION
                        SELECT '02_Escape' AS Category, A.RowId, A.UseVolume, A.EscapeType AS Type, A.EscapeId AS TypeID, 
                               B.Name AS TypeName, C.Name AS Name
                        FROM [CFC_test].[dbo].[Escape_volume] A 
                        JOIN [dbo].[Escape_type] B 
                        ON A.EscapeType = B.Id 
                        JOIN [dbo].[Escape_properties] C 
                        ON A.EscapeId = C.Id
                        WHERE A.RowId = @p0
                        UNION
                        SELECT '03_Refrigerant' AS Category, A.RowId, A.UseVolume, A.RefrigerantEquip AS Type, A.RefrigerantType AS TypeID,
                               B.Name AS TypeName, C.Name AS Name
                        FROM [CFC_test].[dbo].[Refrigerant_volume] A
                        JOIN [dbo].[Refrigerant_equip] B
                        ON A.RefrigerantEquip = B.Id
                        JOIN [dbo].[Refrigerant_type] C
                        ON A.RefrigerantType = C.Id
                        WHERE A.RowId = @p0
                        UNION
                        SELECT '04_Specific' AS Category, A.RowId, A.UseVolume, A.CreateType AS Type, A.CreateId AS TypeID,
                               B.Name AS TypeName, C.Name AS Name
                        FROM [CFC_test].[dbo].[Specific_volume] A
                        JOIN [dbo].[Specific_type] B
                        ON A.CreateType = B.Id
                        JOIN [dbo].[Specific_properties] C
                        ON A.CreateId = C.Id
                        WHERE A.RowId = @p0 
                   ) AS CombinedResults
               ORDER BY Category ASC ";

            var result = db.Database.SqlQuery<VolumeViewModel>(query, rowId).ToList();

            return result;
        }


        public ReturnModel DownloadExcel(SaveProjectModel saveProject) {

            try
            {
                // 確認輸入資料是否正確
                var userInput = this.db.userInputAdvance.Where(e => e.RowID == saveProject.RowID
                            && e.UserID == saveProject.UserID).FirstOrDefault();
                if (userInput == null)
                    return new ReturnModel { isSucess = false, fileAdd = "查無此紀錄" };

                // 取得模板檔案
                String temptFileAdd = WebConfigurationManager.AppSettings["ExcelTemplate"].ToString();

                // 取得工作目錄
                String newTemptFolder = WebConfigurationManager.AppSettings["ExcelFolder"].ToString();
                String newTemptAdd = getTemptFile(newTemptFolder, "xlsx");

                // 複製到工作目錄中
                var newFileInfo = new FileInfo(temptFileAdd).CopyTo(newTemptFolder + "\\" + newTemptAdd);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


                //取得工廠資料
                var factory = db.SysFactory
                                //.Where(f => f.FACTORY_REGISTRATION == "78699002")
                                .Where(f => f.FACTORY_REGISTRATION == ((saveProject.FactoryRegistration == null) ? "NONE" : saveProject.FactoryRegistration))
                                .Select(f => new
                                {
                                    f.FACTORY_NAME,
                                    f.FACTORY_REGISTRATION,
                                    f.FACTORY_CITY,
                                    f.FACTORY_DISTRICT,
                                    f.FACTORY_ADDRESS,
                                    f.FACTORY_INDUSTRIAL,
                                    f.FACTORY_INDUSTRIAL_AREA
                                }).FirstOrDefault();

                //找INDUSTRIAL中文名字
                var factoryIndustrialName = db.GlobalIndustrial
                    .Where(f => f.Id == factory.FACTORY_INDUSTRIAL)
                    .Select(f => new
                    {
                        f.Name
                    }).FirstOrDefault();

                var factoryIndustrialAreaName = db.GlobalIndustrialArea
                    .Where(f => f.Id == factory.FACTORY_INDUSTRIAL_AREA)
                    .Select(f => new
                    {
                        f.Name
                    }).FirstOrDefault();




                //取得人員資料
                var userInfo = db.userPropertiesAdvance
                                .Where(f => f.Id == saveProject.UserID)
                                .Select(f => new
                                {
                                    f.Id,
                                    //f.Name, //單位名稱
                                    f.UniformNumber, //統一編號
                                    f.Contact,
                                    f.POSITION,
                                    f.PhoneNumber,
                                    f.Email,
                                    Manufacturing = string.IsNullOrEmpty(f.UNIT_TYPE) ? "製造業" : "非製造業", //行業別, 如果是製造業, 會是空的, 非製製業才有值
                                    f.UNIT_TYPE
                                }).FirstOrDefault();

                //取得公司資料
                var company = db.SysCompany
                                .Where(f => f.COMP_UNIFORM_NUMBER == userInfo.UniformNumber)
                                .Select(f => new
                                {
                                    f.COMP_NAME,
                                    f.COMP_UNIFORM_NUMBER,
                                    f.COMP_SIZE
                                }).FirstOrDefault();

                //取得所有輸入的類別
                //var totalCategory = this.GetVolumeData(saveProject.RowID);

                // 產製檔案內容
                try
                {
                    ExcelPackage Ep = new ExcelPackage(newFileInfo);



                    var userInfoSheet = Ep.Workbook.Worksheets["廠商資料"];
                    
                    string cUnitType = userInfo.UNIT_TYPE != null ? userInfo.UNIT_TYPE : string.Empty;
                    string cUniformNumber = userInfo.UniformNumber != null ? userInfo.UniformNumber : string.Empty;
                    string cContact = userInfo.Contact != null ? userInfo.Contact : string.Empty;
                    string cPosition = userInfo.POSITION != null ? userInfo.POSITION : string.Empty;
                    string cPhoneNumber = userInfo.PhoneNumber != null ? userInfo.PhoneNumber : string.Empty;
                    string cEmail = userInfo.Email != null ? userInfo.Email : string.Empty;
                    string cfactoryIndustrialName = factoryIndustrialName != null ? factoryIndustrialName.Name : string.Empty;
                    string cfactoryIndustrialAreaName = factoryIndustrialAreaName != null ? factoryIndustrialAreaName.Name : string.Empty;

                    if (userInfo.Manufacturing == "製造業")
                    {
                        new ItemManger().SetUserInfo(userInfoSheet, factory, company,
                                                     userInfo.Manufacturing, cUnitType, cUniformNumber,
                                                     cContact, cPosition, cPhoneNumber, cEmail,
                                                     cfactoryIndustrialName,
                                                     cfactoryIndustrialAreaName);
                    }
                    else
                    {
                        new ItemManger().SetUserInfo(userInfoSheet, factory, company,
                                                     userInfo.Manufacturing, cUnitType, cUniformNumber,
                                                     cContact, cPosition, cPhoneNumber, cEmail,
                                                     "",
                                                     "");
                    }



                    var itemsSheet = Ep.Workbook.Worksheets["排放量計算"];
                    List<string[]> allCategory = new ItemManger().SetItems(itemsSheet, userInput);


                    var allCategorySheet = Ep.Workbook.Worksheets["排放源鑑別"];
                    new ItemManger().cleanAllCategory(allCategorySheet);
                    new ItemManger().setAllCategory(allCategorySheet, allCategory);

                    //var itemsSheet = Ep.Workbook.Worksheets["碳盤查彙整表"];
                    //new ItemManger().SetItems(itemsSheet, userInput);
                    //SetGraph

                    var stasticSheet = Ep.Workbook.Worksheets["碳盤查彙整表"];
                    //new StasticsManager().SetStatistics(stasticSheet, userInput);

                    new StasticsManager().SetGraph(stasticSheet);

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
                            //(LogCount次數) 下載計算3
                            this.db.LogCount.Add(new Log_count()
                            {
                                Type = 2,                                
                                BDate = DateTime.Now,
                                BId = userInput.UserID,
                            });
                            this.db.SaveChanges();

                            string fileAdd = WebConfigurationManager.AppSettings["SiteRoot"].ToString() + Cm.PhysicalToUrl(to);
                            //string fileAdd = Cm.PhysicalToUrl(to);
                            return new ReturnModel { isSucess = true, fileAdd = fileAdd };
                        }
                    }

                    //return new ReturnModel { isSucess = true, fileAdd = fileAdd };
                }
                catch (Exception e)
                {
                    Logger.Log.For(null).Error("下載錯誤：" + e.Message);
                    Logger.Log.For(null).Error(e.StackTrace);
                    return new ReturnModel { isSucess = false, fileAdd = e.Message };
                }

            }
            catch (Exception ex)
            {
                Logger.Log.For(null).Error("執行錯誤：" + ex.Message);
                Logger.Log.For(null).Error(ex.StackTrace);
                return new ReturnModel { isSucess = false, fileAdd = ex.Message };

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
                   orderby property.Name ascending // 根據 property 的 Name 進行升冪排序
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
                       orderby property.Name ascending // 根據 property 的 Name 進行升冪排序
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
                       orderby property.Name ascending // 根據 property 的 Name 進行升冪排序
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
                       orderby property.Name ascending // 根據 property 的 Name 進行升冪排序
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