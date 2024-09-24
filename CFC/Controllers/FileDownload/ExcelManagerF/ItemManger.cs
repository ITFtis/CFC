using CFC.Models;
using CFC.Models.Prj;
using Microsoft.Ajax.Utilities;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;

namespace CFC.Controllers.FileDownload.ExcelManagerF
{
    internal class ItemManger
    {
        private DouModelContext db = new DouModelContext();

        public void SetUserInfo(ExcelWorksheet sheet,  dynamic cFactory, dynamic cCompany, 
                                string manufacturing, string unitType, string uniformNumber,
                                string cContact, string cPosition, string cPhoneNumber, string cEmail,
                                string factoryIndustrialName, string factoryIndustrialAreaName)
        {
            sheet.Cells[3, 3].Value = uniformNumber;
            sheet.Cells[4, 3].Value = cCompany.COMP_NAME;
            sheet.Cells[5, 3].Value = manufacturing;
            sheet.Cells[6, 3].Value = cCompany.COMP_SIZE;
            sheet.Cells[7, 3].Value = unitType;

            sheet.Cells[10, 3].Value = cFactory.FACTORY_NAME;
            sheet.Cells[11, 3].Value = cFactory.FACTORY_REGISTRATION;
            sheet.Cells[12, 3].Value = cFactory.FACTORY_CITY + cFactory.FACTORY_DISTRICT + cFactory.FACTORY_ADDRESS;
            sheet.Cells[13, 3].Value = factoryIndustrialAreaName;
            sheet.Cells[14, 3].Value = factoryIndustrialName;

            sheet.Cells[17, 3].Value = cContact;
            sheet.Cells[18, 3].Value = cPosition;
            sheet.Cells[19, 3].Value = cPhoneNumber;
            sheet.Cells[20, 3].Value = cEmail;
        }

        

        public void SetItems(ExcelWorksheet sheet, User_Input_Advance input) {
            int currentIndex = 1;
            var excelManager = new ExcelManager();


            // 類別一
            //==========================================
            //燃料
            var fuelInputs = excelManager.GetFuelInputs(input);
            var fuelTotalCo2 = fuelInputs.Select(e => e.volume.UseVolume * (double)e.property.Co2e).Sum(); // CO2當量

            // 冷媒逸散
            var refrigInputs = excelManager.GetRefrigInputs(input);
            var refrigTotalCo2 = refrigInputs.Select(e => e.volume.UseVolume * e.equip.EscapeRate *0.001 * (double)e.property.GWP).Sum();// CO2當量

            // 其他逸散
            var escapeInputs = excelManager.GetEscapeInputs(input);
            var escapeTotalCO2 = escapeInputs.Select(e => e.volume.UseVolume * (double)e.property.Co2e).Sum(); //CO2當量

            //製程
            var createInputs = excelManager.GetCreateInputs(input);
            var createTotalCO2 = createInputs.Select(e => e.volume.UseVolume * (double)e.property.Co2e).Sum();//CO2 當量

            // 類別二
            //===========================================
            // 蒸氣
            var steamTotal = input.SteamVolume * (double)input.SteamCoe; // 蒸氣當量

            // 電力
            var electYear = this.db.ElecProperties.Where(e => e.year == input.elecYear).FirstOrDefault();
            var electTotalCo2 = input.elecVolume * (double)electYear.Co2e;// 電力當量

            // 總當量
            var totalCo2 =
                fuelTotalCo2 +
                refrigTotalCo2 +
                escapeTotalCO2 +
                createTotalCO2 +
                steamTotal +
                electTotalCo2 + input.Type3Summary;

            #region 類別1
            fuelInputs.ForEach(fuel =>
            {
                sheet.Cells[currentIndex + 5, 2].Value = currentIndex;
                sheet.Cells[currentIndex + 5, 3].Value = fuel.property.Name;
                sheet.Cells[currentIndex + 5, 4].Value = "類別1";
                sheet.Cells[currentIndex + 5, 5].Value = getFeulTypeName(fuel.property.FuelType);
                sheet.Cells[currentIndex + 5, 6].Value = fuel.volume.UseVolume;
                sheet.Cells[currentIndex + 5, 7].Value = getUnit(fuel.property.Unit);

                // CO2
                var CO2GWP = fuel.property.GCO2R4;
                sheet.Cells[currentIndex + 5, 11].Value = "CO2";
                sheet.Cells[currentIndex + 5, 12].Value = fuel.property.CO2; //係數
                sheet.Cells[currentIndex + 5, 13].Value = fuel.property.Unit;
                sheet.Cells[currentIndex + 5, 14].Value = "能源局公告值";
                sheet.Cells[currentIndex + 5, 15].Value = "(5)國家排放係數";
                sheet.Cells[currentIndex + 5, 16].Value = fuel.volume.UseVolume * (double)fuel.property.CO2;
                sheet.Cells[currentIndex + 5, 17].Value = CO2GWP; //GWP
                sheet.Cells[currentIndex + 5, 18].Value = fuel.volume.UseVolume * (double)fuel.property.CO2 * (double)CO2GWP;

                // CH4
                var CH4GWP = fuel.property.CH4;
                sheet.Cells[currentIndex + 5, 19].Value = "CH4";
                sheet.Cells[currentIndex + 5, 20].Value = fuel.property.CH4; //係數
                sheet.Cells[currentIndex + 5, 21].Value = fuel.property.Unit;
                sheet.Cells[currentIndex + 5, 22].Value = "能源局公告值";
                sheet.Cells[currentIndex + 5, 23].Value = "(5)國家排放係數";
                sheet.Cells[currentIndex + 5, 24].Value = fuel.volume.UseVolume * (double)fuel.property.CH4;
                sheet.Cells[currentIndex + 5, 25].Value = CH4GWP; //GWP
                sheet.Cells[currentIndex + 5, 26].Value = fuel.volume.UseVolume * (double)fuel.property.CH4 * (double)CH4GWP;


                // N2O
                var N2OGWP = fuel.property.NO2;
                sheet.Cells[currentIndex + 5, 27].Value = "N2O";
                sheet.Cells[currentIndex + 5, 28].Value = fuel.property.NO2; //係數
                sheet.Cells[currentIndex + 5, 29].Value = fuel.property.Unit;
                sheet.Cells[currentIndex + 5, 30].Value = "能源局公告值";
                sheet.Cells[currentIndex + 5, 31].Value = "(5)國家排放係數";
                sheet.Cells[currentIndex + 5, 32].Value = fuel.volume.UseVolume * (double)fuel.property.NO2;
                sheet.Cells[currentIndex + 5, 33].Value = N2OGWP; //GWP
                sheet.Cells[currentIndex + 5, 34].Value = fuel.volume.UseVolume * (double)fuel.property.NO2 * (double)N2OGWP;

                // 當量
                var co2Cal = fuel.volume.UseVolume * (double)fuel.property.Co2e;
                sheet.Cells[currentIndex + 5, 35].Value = co2Cal; //當量
                sheet.Cells[currentIndex + 5, 36].Value = co2Cal / totalCo2; //百分比

                currentIndex++;
            });
            refrigInputs.ForEach(refrig =>
            {
                sheet.Cells[currentIndex + 5, 2].Value = currentIndex;
                sheet.Cells[currentIndex + 5, 3].Value = refrig.equip.Name + "(" +  refrig.property.Name + ")";
                sheet.Cells[currentIndex + 5, 4].Value = "類別1";
                sheet.Cells[currentIndex + 5, 5].Value = "逸散";
                sheet.Cells[currentIndex + 5, 6].Value = refrig.volume.UseVolume;
                sheet.Cells[currentIndex + 5, 7].Value = "公斤";

                // HFCS
                var co2Total = refrig.volume.UseVolume * (double)refrig.equip.EscapeRate * 0.001 * (double)refrig.property.GWP;
                sheet.Cells[currentIndex + 5, 11].Value = "HFCS";
                sheet.Cells[currentIndex + 5, 12].Value = refrig.equip.EscapeRate * 0.001; //係數
                sheet.Cells[currentIndex + 5, 13].Value = "公噸/公噸";
                sheet.Cells[currentIndex + 5, 14].Value = "IPCC 建議值，取中間值計算";
                sheet.Cells[currentIndex + 5, 15].Value = "(1)自廠發展係數/質量平衡所得係數";
                sheet.Cells[currentIndex + 5, 16].Value = refrig.volume.UseVolume * (double)refrig.equip.EscapeRate * 0.001;
                sheet.Cells[currentIndex + 5, 17].Value = refrig.property.GWP; //GWP
                sheet.Cells[currentIndex + 5, 18].Value = co2Total;

                //當量
                sheet.Cells[currentIndex + 5, 35].Value = co2Total; //CO2當量
                sheet.Cells[currentIndex + 5, 36].Value = co2Total / totalCo2;

                currentIndex++;
            });
            escapeInputs.ForEach(escape =>
            {
                sheet.Cells[currentIndex + 5, 2].Value = currentIndex;
                sheet.Cells[currentIndex + 5, 3].Value = escape.property.Name;
                sheet.Cells[currentIndex + 5, 4].Value = "類別1";
                sheet.Cells[currentIndex + 5, 5].Value = "逸散";
                sheet.Cells[currentIndex + 5, 6].Value = escape.volume.UseVolume;
                sheet.Cells[currentIndex + 5, 7].Value = getUnit(escape.property.Unit);

                /*
                 逸散分類中6種逸散氣體都有可能，因此僅取前三個
                    順序為 CO2、CH4、N2O、HFCs、PFCs、SF6、NF3
                 */
                int countIndex = 0;

                // CO2
                if (countIndex < 3 && escape.property.CO2 > 0) {
                    sheet.Cells[currentIndex + 5, 11 + countIndex * 8].Value = "CO2";
                    sheet.Cells[currentIndex + 5, 12 + countIndex * 8].Value = escape.property.CO2; //係數
                    sheet.Cells[currentIndex + 5, 13 + countIndex * 8].Value = escape.property.Unit;
                    sheet.Cells[currentIndex + 5, 14 + countIndex * 8].Value = escape.property.CoeSource;
                    sheet.Cells[currentIndex + 5, 15 + countIndex * 8].Value = "(1)自廠發展係數/質量平衡所得係數";
                    sheet.Cells[currentIndex + 5, 16 + countIndex * 8].Value = escape.volume.UseVolume * (double)escape.property.CO2;
                    sheet.Cells[currentIndex + 5, 17 + countIndex * 8].Value = escape.property.CO2GWP; //GWP
                    sheet.Cells[currentIndex + 5, 18 + countIndex * 8].Value = escape.volume.UseVolume * (double)escape.property.CO2 * escape.property.CO2GWP;
                    countIndex++;
                }

                // CH4
                if (countIndex < 3 && escape.property.CH4 > 0)
                {
                    sheet.Cells[currentIndex + 5, 11 + countIndex * 8].Value = "CH4";
                    sheet.Cells[currentIndex + 5, 12 + countIndex * 8].Value = escape.property.CH4; //係數
                    sheet.Cells[currentIndex + 5, 13 + countIndex * 8].Value = escape.property.Unit;
                    sheet.Cells[currentIndex + 5, 14 + countIndex * 8].Value = escape.property.CoeSource;
                    sheet.Cells[currentIndex + 5, 15 + countIndex * 8].Value = "(1)自廠發展係數/質量平衡所得係數";
                    sheet.Cells[currentIndex + 5, 16 + countIndex * 8].Value = escape.volume.UseVolume * (double)escape.property.CH4;
                    sheet.Cells[currentIndex + 5, 17 + countIndex * 8].Value = escape.property.CH4GWP; //GWP
                    sheet.Cells[currentIndex + 5, 18 + countIndex * 8].Value = escape.volume.UseVolume * (double)escape.property.CH4 * escape.property.CH4GWP;
                    countIndex++;
                }

                // N2O
                if (countIndex < 3 && escape.property.N2O > 0)
                {
                    sheet.Cells[currentIndex + 5, 11 + countIndex * 8].Value = "N2O";
                    sheet.Cells[currentIndex + 5, 12 + countIndex * 8].Value = escape.property.N2O; //係數
                    sheet.Cells[currentIndex + 5, 13 + countIndex * 8].Value = escape.property.Unit;
                    sheet.Cells[currentIndex + 5, 14 + countIndex * 8].Value = escape.property.CoeSource;
                    sheet.Cells[currentIndex + 5, 15 + countIndex * 8].Value = "(1)自廠發展係數/質量平衡所得係數";
                    sheet.Cells[currentIndex + 5, 16 + countIndex * 8].Value = escape.volume.UseVolume * (double)escape.property.N2O;
                    sheet.Cells[currentIndex + 5, 17 + countIndex * 8].Value = escape.property.N2OGWP; //GWP
                    sheet.Cells[currentIndex + 5, 18 + countIndex * 8].Value = escape.volume.UseVolume * (double)escape.property.N2O * escape.property.N2OGWP;
                    countIndex++;
                }

                // HFCs
                if (countIndex < 3 && escape.property.HFCs > 0)
                {
                    sheet.Cells[currentIndex + 5, 11 + countIndex * 8].Value = "HFCs";
                    sheet.Cells[currentIndex + 5, 12 + countIndex * 8].Value = escape.property.HFCs; //係數
                    sheet.Cells[currentIndex + 5, 13 + countIndex * 8].Value = escape.property.Unit;
                    sheet.Cells[currentIndex + 5, 14 + countIndex * 8].Value = escape.property.CoeSource;
                    sheet.Cells[currentIndex + 5, 15 + countIndex * 8].Value = "(1)自廠發展係數/質量平衡所得係數";
                    sheet.Cells[currentIndex + 5, 16 + countIndex * 8].Value = escape.volume.UseVolume * (double)escape.property.HFCs;
                    sheet.Cells[currentIndex + 5, 17 + countIndex * 8].Value = escape.property.HFCsGWP; //GWP
                    sheet.Cells[currentIndex + 5, 18 + countIndex * 8].Value = escape.volume.UseVolume * (double)escape.property.HFCs * escape.property.HFCsGWP;
                    countIndex++;
                }

                // PFCs
                if (countIndex < 3 && escape.property.PFCs > 0)
                {
                    sheet.Cells[currentIndex + 5, 11 + countIndex * 8].Value = "PFCs";
                    sheet.Cells[currentIndex + 5, 12 + countIndex * 8].Value = escape.property.PFCs; //係數
                    sheet.Cells[currentIndex + 5, 13 + countIndex * 8].Value = escape.property.Unit;
                    sheet.Cells[currentIndex + 5, 14 + countIndex * 8].Value = escape.property.CoeSource;
                    sheet.Cells[currentIndex + 5, 15 + countIndex * 8].Value = "(1)自廠發展係數/質量平衡所得係數";
                    sheet.Cells[currentIndex + 5, 16 + countIndex * 8].Value = escape.volume.UseVolume * (double)escape.property.PFCs;
                    sheet.Cells[currentIndex + 5, 17 + countIndex * 8].Value = escape.property.PFCsGWP; //GWP
                    sheet.Cells[currentIndex + 5, 18 + countIndex * 8].Value = escape.volume.UseVolume * (double)escape.property.PFCs * escape.property.PFCsGWP;
                    countIndex++;
                }

                // SF6
                if (countIndex < 3 && escape.property.SF6 > 0)
                {
                    sheet.Cells[currentIndex + 5, 11 + countIndex * 8].Value = "SF6";
                    sheet.Cells[currentIndex + 5, 12 + countIndex * 8].Value = escape.property.SF6; //係數
                    sheet.Cells[currentIndex + 5, 13 + countIndex * 8].Value = escape.property.Unit;
                    sheet.Cells[currentIndex + 5, 14 + countIndex * 8].Value = escape.property.CoeSource;
                    sheet.Cells[currentIndex + 5, 15 + countIndex * 8].Value = "(1)自廠發展係數/質量平衡所得係數";
                    sheet.Cells[currentIndex + 5, 16 + countIndex * 8].Value = escape.volume.UseVolume * (double)escape.property.SF6;
                    sheet.Cells[currentIndex + 5, 17 + countIndex * 8].Value = escape.property.SF6GWP; //GWP
                    sheet.Cells[currentIndex + 5, 18 + countIndex * 8].Value = escape.volume.UseVolume * (double)escape.property.SF6 * escape.property.SF6GWP;
                    countIndex++;
                }

                // NF3
                if (countIndex < 3 && escape.property.NF3 > 0)
                {
                    sheet.Cells[currentIndex + 5, 11 + countIndex * 8].Value = "NF3";
                    sheet.Cells[currentIndex + 5, 12 + countIndex * 8].Value = escape.property.NF3; //係數
                    sheet.Cells[currentIndex + 5, 13 + countIndex * 8].Value = escape.property.Unit;
                    sheet.Cells[currentIndex + 5, 14 + countIndex * 8].Value = escape.property.CoeSource;
                    sheet.Cells[currentIndex + 5, 15 + countIndex * 8].Value = "(1)自廠發展係數/質量平衡所得係數";
                    sheet.Cells[currentIndex + 5, 16 + countIndex * 8].Value = escape.volume.UseVolume * (double)escape.property.NF3;
                    sheet.Cells[currentIndex + 5, 17 + countIndex * 8].Value = escape.property.NF3GWP; //GWP
                    sheet.Cells[currentIndex + 5, 18 + countIndex * 8].Value = escape.volume.UseVolume * (double)escape.property.NF3 * escape.property.NF3GWP;
                    countIndex++;
                }

                //當量
                var co2Total = escape.volume.UseVolume * (double)escape.property.Co2e;
                sheet.Cells[currentIndex + 5, 35].Value = co2Total; //CO2當量
                sheet.Cells[currentIndex + 5, 36].Value = co2Total / totalCo2;
                currentIndex++;
            });
            createInputs.ForEach(create =>
            {
                sheet.Cells[currentIndex + 5, 2].Value = currentIndex;
                sheet.Cells[currentIndex + 5, 3].Value = create.property.Name;
                sheet.Cells[currentIndex + 5, 4].Value = "類別1";
                sheet.Cells[currentIndex + 5, 5].Value = "逸散";
                sheet.Cells[currentIndex + 5, 6].Value = create.volume.UseVolume;
                sheet.Cells[currentIndex + 5, 7].Value = getUnit(create.property.Unit);

                /*
                 逸散分類中6種逸散氣體都有可能，因此僅取前三個
                    順序為 CO2、CH4、N2O、HFCs、PFCs、SF6、NF3
                 */
                int countIndex = 0;

                // CO2
                if (countIndex < 3 && create.property.CO2 > 0)
                {
                    sheet.Cells[currentIndex + 5, 11 + countIndex * 8].Value = "CO2";
                    sheet.Cells[currentIndex + 5, 12 + countIndex * 8].Value = create.property.CO2; //係數
                    sheet.Cells[currentIndex + 5, 13 + countIndex * 8].Value = create.property.Unit;
                    sheet.Cells[currentIndex + 5, 14 + countIndex * 8].Value = create.property.CoeSource;
                    sheet.Cells[currentIndex + 5, 15 + countIndex * 8].Value = "(1)自廠發展係數/質量平衡所得係數";
                    sheet.Cells[currentIndex + 5, 16 + countIndex * 8].Value = create.volume.UseVolume * (double)create.property.CO2;
                    sheet.Cells[currentIndex + 5, 17 + countIndex * 8].Value = create.property.CO2GWP; //GWP
                    sheet.Cells[currentIndex + 5, 18 + countIndex * 8].Value = create.volume.UseVolume * (double)create.property.CO2 * create.property.CO2GWP;
                    countIndex++;
                }

                // CH4
                if (countIndex < 3 && create.property.CH4 > 0)
                {
                    sheet.Cells[currentIndex + 5, 11 + countIndex * 8].Value = "CH4";
                    sheet.Cells[currentIndex + 5, 12 + countIndex * 8].Value = create.property.CH4; //係數
                    sheet.Cells[currentIndex + 5, 13 + countIndex * 8].Value = create.property.Unit;
                    sheet.Cells[currentIndex + 5, 14 + countIndex * 8].Value = create.property.CoeSource;
                    sheet.Cells[currentIndex + 5, 15 + countIndex * 8].Value = "(1)自廠發展係數/質量平衡所得係數";
                    sheet.Cells[currentIndex + 5, 16 + countIndex * 8].Value = create.volume.UseVolume * (double)create.property.CH4;
                    sheet.Cells[currentIndex + 5, 17 + countIndex * 8].Value = create.property.CH4GWP; //GWP
                    sheet.Cells[currentIndex + 5, 18 + countIndex * 8].Value = create.volume.UseVolume * (double)create.property.CH4 * create.property.CH4GWP;
                    countIndex++;
                }

                // N2O
                if (countIndex < 3 && create.property.N2O > 0)
                {
                    sheet.Cells[currentIndex + 5, 11 + countIndex * 8].Value = "N2O";
                    sheet.Cells[currentIndex + 5, 12 + countIndex * 8].Value = create.property.N2O; //係數
                    sheet.Cells[currentIndex + 5, 13 + countIndex * 8].Value = create.property.Unit;
                    sheet.Cells[currentIndex + 5, 14 + countIndex * 8].Value = create.property.CoeSource;
                    sheet.Cells[currentIndex + 5, 15 + countIndex * 8].Value = "(1)自廠發展係數/質量平衡所得係數";
                    sheet.Cells[currentIndex + 5, 16 + countIndex * 8].Value = create.volume.UseVolume * (double)create.property.N2O;
                    sheet.Cells[currentIndex + 5, 17 + countIndex * 8].Value = create.property.N2OGWP; //GWP
                    sheet.Cells[currentIndex + 5, 18 + countIndex * 8].Value = create.volume.UseVolume * (double)create.property.N2O * create.property.N2OGWP;
                    countIndex++;
                }

                // HFCs
                if (countIndex < 3 && create.property.HFCs > 0)
                {
                    sheet.Cells[currentIndex + 5, 11 + countIndex * 8].Value = "HFCs";
                    sheet.Cells[currentIndex + 5, 12 + countIndex * 8].Value = create.property.HFCs; //係數
                    sheet.Cells[currentIndex + 5, 13 + countIndex * 8].Value = create.property.Unit;
                    sheet.Cells[currentIndex + 5, 14 + countIndex * 8].Value = create.property.CoeSource;
                    sheet.Cells[currentIndex + 5, 15 + countIndex * 8].Value = "(1)自廠發展係數/質量平衡所得係數";
                    sheet.Cells[currentIndex + 5, 16 + countIndex * 8].Value = create.volume.UseVolume * (double)create.property.HFCs;
                    sheet.Cells[currentIndex + 5, 17 + countIndex * 8].Value = create.property.HFCsGWP; //GWP
                    sheet.Cells[currentIndex + 5, 18 + countIndex * 8].Value = create.volume.UseVolume * (double)create.property.HFCs * create.property.HFCsGWP;
                    countIndex++;
                }

                // PFCs
                if (countIndex < 3 && create.property.PFCs > 0)
                {
                    sheet.Cells[currentIndex + 5, 11 + countIndex * 8].Value = "PFCs";
                    sheet.Cells[currentIndex + 5, 12 + countIndex * 8].Value = create.property.PFCs; //係數
                    sheet.Cells[currentIndex + 5, 13 + countIndex * 8].Value = create.property.Unit;
                    sheet.Cells[currentIndex + 5, 14 + countIndex * 8].Value = create.property.CoeSource;
                    sheet.Cells[currentIndex + 5, 15 + countIndex * 8].Value = "(1)自廠發展係數/質量平衡所得係數";
                    sheet.Cells[currentIndex + 5, 16 + countIndex * 8].Value = create.volume.UseVolume * (double)create.property.PFCs;
                    sheet.Cells[currentIndex + 5, 17 + countIndex * 8].Value = create.property.PFCsGWP; //GWP
                    sheet.Cells[currentIndex + 5, 18 + countIndex * 8].Value = create.volume.UseVolume * (double)create.property.PFCs * create.property.PFCsGWP;
                    countIndex++;
                }

                // SF6
                if (countIndex < 3 && create.property.SF6 > 0)
                {
                    sheet.Cells[currentIndex + 5, 11 + countIndex * 8].Value = "SF6";
                    sheet.Cells[currentIndex + 5, 12 + countIndex * 8].Value = create.property.SF6; //係數
                    sheet.Cells[currentIndex + 5, 13 + countIndex * 8].Value = create.property.Unit;
                    sheet.Cells[currentIndex + 5, 14 + countIndex * 8].Value = create.property.CoeSource;
                    sheet.Cells[currentIndex + 5, 15 + countIndex * 8].Value = "(1)自廠發展係數/質量平衡所得係數";
                    sheet.Cells[currentIndex + 5, 16 + countIndex * 8].Value = create.volume.UseVolume * (double)create.property.SF6;
                    sheet.Cells[currentIndex + 5, 17 + countIndex * 8].Value = create.property.SF6GWP; //GWP
                    sheet.Cells[currentIndex + 5, 18 + countIndex * 8].Value = create.volume.UseVolume * (double)create.property.SF6 * create.property.SF6GWP;
                    countIndex++;
                }

                // NF3
                if (countIndex < 3 && create.property.NF3 > 0)
                {
                    sheet.Cells[currentIndex + 5, 11 + countIndex * 8].Value = "NF3";
                    sheet.Cells[currentIndex + 5, 12 + countIndex * 8].Value = create.property.NF3; //係數
                    sheet.Cells[currentIndex + 5, 13 + countIndex * 8].Value = create.property.Unit;
                    sheet.Cells[currentIndex + 5, 14 + countIndex * 8].Value = create.property.CoeSource;
                    sheet.Cells[currentIndex + 5, 15 + countIndex * 8].Value = "(1)自廠發展係數/質量平衡所得係數";
                    sheet.Cells[currentIndex + 5, 16 + countIndex * 8].Value = create.volume.UseVolume * (double)create.property.NF3;
                    sheet.Cells[currentIndex + 5, 17 + countIndex * 8].Value = create.property.NF3GWP; //GWP
                    sheet.Cells[currentIndex + 5, 18 + countIndex * 8].Value = create.volume.UseVolume * (double)create.property.NF3 * create.property.NF3GWP;
                    countIndex++;
                }

                //當量
                var co2Total = create.volume.UseVolume * (double)create.property.Co2e;
                sheet.Cells[currentIndex + 5, 35].Value = co2Total; //CO2當量
                sheet.Cells[currentIndex + 5, 36].Value = co2Total / totalCo2;
                currentIndex++;
            });
            #endregion

            #region 類別2
            // 電力
            if (electTotalCo2 > 0)
            {
                sheet.Cells[currentIndex + 5, 2].Value = currentIndex;
                sheet.Cells[currentIndex + 5, 3].Value = "外購電力";
                sheet.Cells[currentIndex + 5, 4].Value = "類別2";
                sheet.Cells[currentIndex + 5, 5].Value = "外購電力";
                sheet.Cells[currentIndex + 5, 6].Value = input.elecVolume;
                sheet.Cells[currentIndex + 5, 7].Value = getFeulTypeName(electYear.Unit);

                // CO2
                var co2total = (double)electYear.Co2e * input.elecVolume;
                sheet.Cells[currentIndex + 5, 11].Value = "CO2";
                sheet.Cells[currentIndex + 5, 12].Value = electYear.Co2e; //係數
                sheet.Cells[currentIndex + 5, 13].Value = electYear.Unit;
                sheet.Cells[currentIndex + 5, 14].Value = "能源局公告電力排放係數(year年度)".Replace("year", electYear.year);
                sheet.Cells[currentIndex + 5, 15].Value = "(5)國家排放係數";
                sheet.Cells[currentIndex + 5, 16].Value = co2total;
                sheet.Cells[currentIndex + 5, 17].Value = 1; //GWP
                sheet.Cells[currentIndex + 5, 18].Value = co2total;

                //當量
                sheet.Cells[currentIndex + 5, 35].Value = co2total; //CO2當量
                sheet.Cells[currentIndex + 5, 36].Value = co2total / totalCo2;
                currentIndex++;
            }

            // 蒸氣
            if (input.SteamCoe > 0 && input.SteamVolume > 0)
            {
                sheet.Cells[currentIndex + 5, 2].Value = currentIndex;
                sheet.Cells[currentIndex + 5, 3].Value = "外購蒸氣";
                sheet.Cells[currentIndex + 5, 4].Value = "類別2";
                sheet.Cells[currentIndex + 5, 5].Value = "外購蒸氣";
                sheet.Cells[currentIndex + 5, 6].Value = input.SteamVolume;
                sheet.Cells[currentIndex + 5, 7].Value = "公噸";

                // CO2
                var co2total = (double)input.SteamCoe * input.SteamVolume;
                sheet.Cells[currentIndex + 5, 11].Value = "CO2";
                sheet.Cells[currentIndex + 5, 12].Value = input.SteamCoe; //係數
                sheet.Cells[currentIndex + 5, 13].Value = "公噸/公噸";
                sheet.Cells[currentIndex + 5, 14].Value = "供應商提供";
                sheet.Cells[currentIndex + 5, 15].Value = "(1)自廠發展係數/質量平衡所得係數";
                sheet.Cells[currentIndex + 5, 16].Value = co2total;
                sheet.Cells[currentIndex + 5, 17].Value = 1; //GWP
                sheet.Cells[currentIndex + 5, 18].Value = co2total;

                //當量
                sheet.Cells[currentIndex + 5, 35].Value = co2total; //CO2當量
                sheet.Cells[currentIndex + 5, 36].Value = co2total / totalCo2;
                currentIndex++;
            }
            #endregion

            #region 類別3
            if (input.Tr01 > 0) {
                setType3(sheet , currentIndex , "上游原物料配送當量" , "運輸" , input.Tr01, totalCo2);
                currentIndex++;
            }
            if (input.Tr02 > 0)
            {
                setType3(sheet, currentIndex, "商務旅遊", "運輸", input.Tr02, totalCo2);
                currentIndex++;
            }
            if (input.Tr03 > 0)
            {
                setType3(sheet, currentIndex, "員工通勤", "運輸", input.Tr03, totalCo2);
                currentIndex++;
            }
            if (input.Tr04 > 0)
            {
                setType3(sheet, currentIndex, "下游運輸及配送", "運輸", input.Tr04, totalCo2);
                currentIndex++;
            }
            if (input.Cp01 > 0)
            {
                setType3(sheet, currentIndex, "採購", "組織使用產品", input.Cp01, totalCo2);
                currentIndex++;
            }
            if (input.Cp02 > 0)
            {
                setType3(sheet, currentIndex, "資本", "組織使用產品", input.Cp02, totalCo2);
                currentIndex++;
            }
            if (input.Cp03 > 0)
            {
                setType3(sheet, currentIndex, "能源相關活動", "組織使用產品", input.Cp03, totalCo2);
                currentIndex++;
            }
            if (input.Cp04 > 0)
            {
                setType3(sheet, currentIndex, "營運廢棄物", "組織使用產品", input.Cp04, totalCo2);
                currentIndex++;
            }
            if (input.Cp05 > 0)
            {
                setType3(sheet, currentIndex, "上游資產租賃", "組織使用產品", input.Cp05, totalCo2);
                currentIndex++;
            }
            if (input.Us01 > 0)
            {
                setType3(sheet, currentIndex, "加工", "使用組織產品", input.Us01, totalCo2);
                currentIndex++;
            }
            if (input.Us02 > 0)
            {
                setType3(sheet, currentIndex, "使用", "使用組織產品", input.Us02, totalCo2);
                currentIndex++;
            }
            if (input.Us03 > 0)
            {
                setType3(sheet, currentIndex, "報廢", "使用組織產品", input.Us03, totalCo2);
                currentIndex++;
            }
            if (input.Us04 > 0)
            {
                setType3(sheet, currentIndex, "下游租賃", "使用組織產品", input.Us04, totalCo2);
                currentIndex++;
            }
            if (input.Us05 > 0)
            {
                setType3(sheet, currentIndex, "加盟", "使用組織產品", input.Us05, totalCo2);
                currentIndex++;
            }
            if (input.Us06 > 0)
            {
                setType3(sheet, currentIndex, "投資", "使用組織產品", input.Us06, totalCo2);
                currentIndex++;
            }
            if (input.Other > 0)
            {
                setType3(sheet, currentIndex, "其他排放", "其他", input.Other, totalCo2);
                currentIndex++;
            }
            #endregion

            // 總計
            sheet.Cells[currentIndex + 5, 34].Value = "總計";
            sheet.Cells[currentIndex + 5, 35].Value = totalCo2;
            sheet.Cells[currentIndex + 5, 36].Value = "100%";


        }


        public void setType3(ExcelWorksheet sheet , int currentIndex, string item , string type , double co2e  , double total) {
            sheet.Cells[currentIndex + 5, 2].Value = currentIndex;
            sheet.Cells[currentIndex + 5, 3].Value = item;
            sheet.Cells[currentIndex + 5, 4].Value = "類別3";
            sheet.Cells[currentIndex + 5, 5].Value = type;

            //當量
            sheet.Cells[currentIndex + 5, 35].Value = co2e; //CO2當量
            sheet.Cells[currentIndex + 5, 36].Value = co2e / total;
        }



        // 取得活動數據單位
        public static String getUnit(String unit) {
            return unit.Split('/')[0]; 
        }

        //取得排放類型
        public static String getFeulTypeName(String type)
        {
            switch (type) {
                case "solid":
                    return "固定";
                case "staticFluid":
                    return "固定";
                case "gas":
                    return "固定";
                case "dynamicFluid":
                    return "移動";
                default:
                    return "固定";
            }
        }
    }
}