using CFC.Models;
using CFC.Models.Prj;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CFC.Controllers.FileDownload.ExcelManagerF
{
    internal class StasticsManager
    {
        private DouModelContext db = new DouModelContext();

        public void SetGraph(ExcelWorksheet sheet)
        {
            // 添加圓餅圖
            var pieChart = sheet.Drawings.AddChart("PieChart", eChartType.Pie) as ExcelPieChart;

            // 設定圖表的數據範圍
            pieChart.Series.Add(sheet.Cells["T3:T8"], sheet.Cells["S3:S8"]);

            // 設定圖表的標題
            pieChart.Title.Text = "Category Distribution";

            // 設定圖表的顯示位置 (X1, Y1, X2, Y2)
            pieChart.SetPosition(12, 0, 10, 0);
            pieChart.SetSize(400, 300);
        }

        public void SetStatistics(ExcelWorksheet sheet , User_Input_Advance input) {

            var excelManager = new ExcelManager();

            // 類別一
            //==========================================
            //燃料
            var fuelInputs = excelManager.GetFuelInputs(input);
            //var fuelTotalCo2 = fuelInputs.Select(e => e.volume.UseVolume * (double)e.property.Co2e).Sum(); // CO2當量
            double fuelTotalCo2 = 0;
            //改成AR4、AR5、AR6
            if (input.ARType == "AR4")
                fuelTotalCo2 = fuelInputs.Select(e => e.volume.UseVolume * (double)e.property.Co2e_AR4).Sum(); // CO2當量
            else if (input.ARType == "AR5")
                fuelTotalCo2 = fuelInputs.Select(e => e.volume.UseVolume * (double)e.property.Co2e_AR5).Sum(); // CO2當量
            else if (input.ARType == "AR6")
                fuelTotalCo2 = fuelInputs.Select(e => e.volume.UseVolume * (double)e.property.Co2e_AR6).Sum(); // CO2當量

            // 冷媒逸散
            var refrigInputs = excelManager.GetRefrigInputs(input);
            //var refrigTotalCo2 = refrigInputs.Select(e => e.volume.UseVolume * e.equip.EscapeRate * (double)e.property.GWP).Sum();// CO2當量
            double refrigTotalCo2 = 0;
            if (input.ARType == "AR4")
                refrigTotalCo2 = refrigInputs.Select(e => e.volume.UseVolume * e.equip.EscapeRate * (double)e.property.GWP_AR4).Sum();// CO2當量
            else if (input.ARType == "AR5")
                refrigTotalCo2 = refrigInputs.Select(e => e.volume.UseVolume * e.equip.EscapeRate * (double)e.property.GWP_AR5).Sum();// CO2當量
            else if (input.ARType == "AR6")
                refrigTotalCo2 = refrigInputs.Select(e => e.volume.UseVolume * e.equip.EscapeRate * (double)e.property.GWP_AR6).Sum();// CO2當量

            // 其他逸散
            var escapeInputs = excelManager.GetEscapeInputs(input);
            //var escapeTotalCO2 = escapeInputs.Select(e => e.volume.UseVolume * (double)e.property.Co2e).Sum(); //CO2當量
            double escapeTotalCO2 = 0;
            if (input.ARType == "AR4")
                escapeTotalCO2 = escapeInputs.Select(e => e.volume.UseVolume * (double)e.property.Co2e_AR4).Sum(); //CO2當量
            else if (input.ARType == "AR5")
                escapeTotalCO2 = escapeInputs.Select(e => e.volume.UseVolume * (double)e.property.Co2e_AR5).Sum(); //CO2當量
            else if (input.ARType == "AR6")
                escapeTotalCO2 = escapeInputs.Select(e => e.volume.UseVolume * (double)e.property.Co2e_AR6).Sum(); //CO2當量

            //製程
            var createInputs = excelManager.GetCreateInputs(input);
            //var createTotalCO2 = createInputs.Select(e => e.volume.UseVolume * (double)e.property.Co2e).Sum();//CO2 當量
            double createTotalCO2 = 0;
            if (input.ARType == "AR4")
                createTotalCO2 = createInputs.Select(e => e.volume.UseVolume * (double)e.property.Co2e_AR4).Sum();//CO2 當量
            else if (input.ARType == "AR5")
                createTotalCO2 = createInputs.Select(e => e.volume.UseVolume * (double)e.property.Co2e_AR5).Sum();//CO2 當量
            else if (input.ARType == "AR6")
                createTotalCO2 = createInputs.Select(e => e.volume.UseVolume * (double)e.property.Co2e_AR6).Sum();//CO2 當量

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


            // 取得範疇一統計
            var type1Co2 =
                fuelInputs.Sum(e => e.volume.UseVolume * (double)e.property.CO2 * (double) e.property.CO2GWP) +
                escapeInputs.Sum(e => e.volume.UseVolume * (double)e.property.CO2 * (double)e.property.CO2GWP) +
                createInputs.Sum(e => e.volume.UseVolume * (double)e.property.CO2 * (double)e.property.CO2GWP);

            var type1CH4 =
                fuelInputs.Sum(e => e.volume.UseVolume * (double)e.property.CH4  *(double)e.property.CH4GWP) +
                escapeInputs.Sum(e => e.volume.UseVolume * (double)e.property.CH4 * (double)e.property.CH4GWP) +
                createInputs.Sum(e => e.volume.UseVolume * (double)e.property.CH4 * (double)e.property.CH4GWP);

            var type1NO2 =
                fuelInputs.Sum(e => e.volume.UseVolume * (double)e.property.NO2 * (double)e.property.N2OGWP) +
                escapeInputs.Sum(e => e.volume.UseVolume * (double)e.property.N2O * (double)e.property.N2OGWP) +
                createInputs.Sum(e => e.volume.UseVolume * (double)e.property.N2O * (double)e.property.N2OGWP);

            var type1HFCs =
                refrigInputs.Sum(e => e.volume.UseVolume * (double) e.equip.EscapeRate * 0.001 * (double)e.property.GWP) +
                escapeInputs.Sum(e => e.volume.UseVolume * (double)e.property.HFCs * (double) e.property.HFCsGWP) +
                createInputs.Sum(e => e.volume.UseVolume * (double)e.property.HFCs * (double)e.property.HFCsGWP);

            var type1PFCs =
                escapeInputs.Sum(e => e.volume.UseVolume * (double)e.property.PFCs * (double)e.property.PFCsGWP) +
                createInputs.Sum(e => e.volume.UseVolume * (double)e.property.PFCs * (double)e.property.PFCsGWP);

            var type1SF6 =
                escapeInputs.Sum(e => e.volume.UseVolume * (double)e.property.SF6 * (double)e.property.SF6GWP) +
                createInputs.Sum(e => e.volume.UseVolume * (double)e.property.SF6 * (double)e.property.SF6GWP);

            var type1NF3 =
               escapeInputs.Sum(e => e.volume.UseVolume * (double)e.property.NF3 * (double)e.property.NF3GWP) +
               createInputs.Sum(e => e.volume.UseVolume * (double)e.property.NF3 * (double)e.property.NF3GWP);

            var type1Total = type1Co2 + type1CH4 + type1NO2 + type1HFCs + type1PFCs + type1SF6 + type1NF3;

            // 取得範疇一二
            var allCO2 = type1Co2 + steamTotal + electTotalCo2;
            var allTotal = type1Total + steamTotal + electTotalCo2;

            // 取得範疇全部
            var type3Total = input.Tr01 + input.Tr02 + input.Tr03 + input.Tr04;
            var type4Total = input.Cp01 + input.Cp02 + input.Cp03 + input.Cp04 + input.Cp05;
            var type5Total = input.Us01 + input.Us02 + input.Us03 + input.Us04 + input.Us05 + +input.Us06;
            var type6Total = input.Other;
            var extensionTotal = allTotal + type3Total + type4Total + type5Total + type6Total;

            // 取得固定/移動/製程/逸散
            var staticCo2e = fuelInputs.Where(e => e.property.FuelType != "dynamicFluid").Sum(e => e.volume.UseVolume * (double)e.property.Co2e);
            var dynamicCo2e = fuelInputs.Where(e => e.property.FuelType == "dynamicFluid").Sum(e => e.volume.UseVolume * (double)e.property.Co2e);
            var createCo2e = createInputs.Sum(e => e.volume.UseVolume * (double)e.property.Co2e);
            var escapeCo2e = escapeInputs.Sum(e => e.volume.UseVolume * (double)e.property.Co2e) + 
                            refrigInputs.Sum(e=> e.volume.UseVolume * e.equip.EscapeRate*0.001* (double)e.property.GWP);


            // 製作統計表

            // 彙整表二
            sheet.Cells[4, 3].Value = allCO2;
            sheet.Cells[5, 3].Value = allCO2 / allTotal;
            sheet.Cells[4, 4].Value = type1CH4;
            sheet.Cells[5, 4].Value = type1CH4 / allTotal;
            sheet.Cells[4, 5].Value = type1NO2;
            sheet.Cells[5, 5].Value = type1NO2 / allTotal;
            sheet.Cells[4, 6].Value = type1HFCs;
            sheet.Cells[5, 6].Value = type1HFCs / allTotal;
            sheet.Cells[4, 7].Value = type1PFCs;
            sheet.Cells[5, 7].Value = type1PFCs / allTotal;
            sheet.Cells[4, 8].Value = type1SF6;
            sheet.Cells[5, 8].Value = type1SF6 / allTotal;
            sheet.Cells[4, 9].Value = type1NF3;
            sheet.Cells[5, 9].Value = type1NF3 / allTotal;
            sheet.Cells[4, 10].Value = allTotal;
            sheet.Cells[5, 10].Value = 1.0;

            // 彙整表三
            sheet.Cells[10, 3].Value = type1Co2;
            sheet.Cells[11, 3].Value = type1Co2 / type1Total;
            sheet.Cells[10, 4].Value = type1CH4;
            sheet.Cells[11, 4].Value = type1CH4 / type1Total;
            sheet.Cells[10, 5].Value = type1NO2;
            sheet.Cells[11, 5].Value = type1NO2 / type1Total;
            sheet.Cells[10, 6].Value = type1HFCs;
            sheet.Cells[11, 6].Value = type1HFCs / type1Total;
            sheet.Cells[10, 7].Value = type1PFCs;
            sheet.Cells[11, 7].Value = type1PFCs / type1Total;
            sheet.Cells[10, 8].Value = type1SF6;
            sheet.Cells[11, 8].Value = type1SF6 / type1Total;
            sheet.Cells[10, 9].Value = type1NF3;
            sheet.Cells[11, 9].Value = type1NF3 / type1Total;
            sheet.Cells[10, 10].Value = type1Total;
            sheet.Cells[11, 10].Value = 1.0;

            // 彙整表四
            var type1Co2eTotal = staticCo2e + dynamicCo2e + createCo2e + escapeCo2e;

            // type1
            sheet.Cells[16, 3].Value = type1Co2eTotal;
            sheet.Cells[17, 3].Value = staticCo2e;
            sheet.Cells[17, 4].Value = dynamicCo2e;
            sheet.Cells[17, 5].Value = createCo2e;
            sheet.Cells[17, 6].Value = escapeCo2e;
            sheet.Cells[18, 3].Value = type1Co2eTotal / extensionTotal;
            sheet.Cells[19, 3].Value = staticCo2e / extensionTotal;
            sheet.Cells[19, 4].Value = dynamicCo2e / extensionTotal;
            sheet.Cells[19, 5].Value = createCo2e / extensionTotal;
            sheet.Cells[19, 6].Value = escapeCo2e / extensionTotal;

            // type2
            sheet.Cells[16, 7].Value = steamTotal + electTotalCo2;
            sheet.Cells[18, 7].Value = (steamTotal + electTotalCo2) / extensionTotal;

            // type3
            sheet.Cells[16, 8].Value = type3Total;
            sheet.Cells[18, 8].Value = type3Total / extensionTotal;

            // type4
            sheet.Cells[16, 9].Value = type4Total;
            sheet.Cells[18, 9].Value = type4Total / extensionTotal;

            // type5
            sheet.Cells[16, 10].Value = type5Total;
            sheet.Cells[18, 10].Value = type5Total / extensionTotal;

            // type6
            sheet.Cells[16, 11].Value = type6Total;
            sheet.Cells[18, 11].Value = type6Total / extensionTotal;

            // total
            sheet.Cells[16, 12].Value = extensionTotal;
            sheet.Cells[18, 12].Value = 1.0;
        }
    }
}