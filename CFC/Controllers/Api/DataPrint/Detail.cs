using CFC.Models.Api;
using CFC.Models.Joined.Calculate;
using CFC.Models.Prj;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace CFC.Controllers.Api.DataPrint
{
    //public class Detail
    //{
    //    public List<DetailResult> CreateExcel(CalInputModel input)
    //    {
    //        List<DetailResult> outResults = new List<DetailResult>();

    //        // 基本輸入用
    //        var mps = typeof(User_InputAdvance).GetProperties().Where(x => x.CanRead).ToList();

    //        try
    //        {
    //            decimal type1 = 0; // 範疇1
    //            decimal type2 = 0; // 範疇2


    //            //R1、R2(燃料 - 直接排放)
    //            foreach (var inputFuel in input.fuelInputs)
    //            {
    //                //  判斷正常數值
    //                if (inputFuel.UseVolume > 0)
    //                {
    //                    var fuelProperty = DateViewController.AllFuelProperties.Where(e => e.Id == inputFuel.FuelId).FirstOrDefault();
    //                    if (fuelProperty == null) continue; //沒找到對應的就換下一個了

    //                    DetailResult result = new DetailResult()
    //                    {
    //                        index = outResults.Count,
    //                        emissionName = "", // 燃料無排放名稱
    //                        ingregientName = fuelProperty.Name,
    //                        volume = inputFuel.UseVolume,
    //                        unit = fuelProperty.Unit,
    //                        coe = fuelProperty.Co2e,
    //                        catlogType = "範疇一",
    //                        emissionVolume = Convert.ToDecimal(inputFuel.UseVolume) * fuelProperty.Co2e
    //                    };

    //                    // 分類
    //                    switch (fuelProperty.FuelType)
    //                    {

    //                        case "solid":
    //                            result.emissonType = "固定源";
    //                            break;
    //                        case "staticFluid":
    //                            result.emissonType = "固定源";
    //                            break;
    //                        case "dynamicFluid":
    //                            result.emissonType = "移動源";
    //                            break;
    //                        case "gas":
    //                            result.emissonType = "逸散";
    //                            break;
    //                    }

    //                    // 屬於範疇一
    //                    type1 = type1 + result.emissionVolume;
    //                    outResults.Add(result);
    //                }
    //            }

    //            //R3(冷媒逸散 - 直接排放)
    //            foreach (var refrigeInput in input.refrigerantInputs)
    //            {
    //                if (refrigeInput.UseVolume > 0)
    //                {

    //                    var equip = DateViewController.AllRefrigerantEquip.Where(e => e.Id == refrigeInput.RefrigerantEquip).FirstOrDefault();
    //                    var type = DateViewController.AllRefrigerantType.Where(e => e.Id == refrigeInput.RefrigerantType).FirstOrDefault();
    //                    if (equip == null || type == null) continue;

    //                    DetailResult result = new DetailResult()
    //                    {
    //                        index = outResults.Count,
    //                        emissionName = equip.Name,
    //                        ingregientName = type.Name,
    //                        volume = refrigeInput.UseVolume,
    //                        unit = "公斤",// 冷媒逸散固定為 "公斤"
    //                        coe = (decimal)type.GWP,
    //                        emissonType = "逸散",
    //                        emissionVolume = (decimal)refrigeInput.UseVolume * (decimal)equip.EscapeRate * (decimal)type.GWP * (decimal)0.001,
    //                        catlogType = "範疇一"
    //                    };

    //                    // 屬於範疇一
    //                    type1 = type1 + result.emissionVolume;
    //                    outResults.Add(result);
    //                }
    //            }

    //            //(其他逸散 - 直接排放 - 逸散)
    //            foreach (var escapeInput in input.escapeInputs)
    //            {
    //                var property = DateViewController.AllEscapeProperties.FirstOrDefault(e => e.Id.Equals(escapeInput.EscapeId));
    //                if (property != null)
    //                {
    //                    DetailResult result = new DetailResult()
    //                    {
    //                        index = outResults.Count,
    //                        emissionName = property.Name,
    //                        ingregientName = property.Name,
    //                        volume = escapeInput.UseVolume,
    //                        unit = property.Unit,
    //                        coe = property.Co2e,
    //                        emissonType = "逸散",
    //                        emissionVolume = property.Co2e * Convert.ToDecimal(escapeInput.UseVolume),
    //                        catlogType = "範疇一"
    //                    };

    //                    // 範疇一
    //                    type1 = type1 + result.emissionVolume;
    //                    outResults.Add(result);
    //                }
    //            }

    //            //(特殊製程 - 直接排放 - 固定)
    //            foreach (var specialInput in input.specialInputs)
    //            {
    //                var property = DateViewController.AllSpecificProperties.FirstOrDefault(e => e.Id.Equals(specialInput.CreateId));
    //                if (property != null)
    //                {
    //                    DetailResult result = new DetailResult()
    //                    {
    //                        index = outResults.Count,
    //                        emissionName = property.Name,
    //                        ingregientName = property.Name,
    //                        volume = specialInput.UseVolume,
    //                        unit = property.Unit,
    //                        coe = (decimal)property.Co2e,
    //                        emissonType = "製程",
    //                        emissionVolume = property.Co2e * Convert.ToDecimal(specialInput.UseVolume),
    //                        catlogType = "範疇一"
    //                    };

    //                    // 範疇一
    //                    type1 = type1 + result.emissionVolume;
    //                    outResults.Add(result);
    //                }
    //            }

    //            //R4(電力 - 間接排放)
    //            if (input.electInput.elecVolume > 0)
    //            {
    //                var selectYear = DateViewController.AllElecProperties.Where(e => e.year == input.electInput.elecYear).FirstOrDefault();

    //                if (selectYear != null)
    //                {
    //                    DetailResult result = new DetailResult()
    //                    {
    //                        index = outResults.Count,
    //                        emissionName = "其他未歸類設施",
    //                        ingregientName = "其他電力",
    //                        volume = input.electInput.elecVolume,
    //                        unit = "度",// 冷媒逸散固定為 "度"
    //                        coe = (decimal)selectYear.Co2e,
    //                        emissonType = "輸入能源",
    //                        emissionVolume = (decimal)input.electInput.elecVolume * selectYear.Co2e,
    //                        catlogType = "範疇二"
    //                    };

    //                    // 範疇二
    //                    type2 = type2 + result.emissionVolume;
    //                    outResults.Add(result);
    //                }
    //            }

    //            //(蒸氣 - 間接排放)
    //            if (input.steamInput.SteamVolume >= 0 && input.steamInput.SteamCoe > 0)
    //            {
    //                DetailResult result = new DetailResult()
    //                {
    //                    index = outResults.Count,
    //                    emissionName = "蒸氣",
    //                    ingregientName = "蒸氣",
    //                    volume = input.steamInput.SteamVolume,
    //                    unit = "公噸",
    //                    coe = (decimal)input.steamInput.SteamCoe,
    //                    emissonType = "輸入能源",
    //                    emissionVolume = (decimal)(input.steamInput.SteamVolume * (double)input.steamInput.SteamCoe),
    //                    catlogType = "範疇二"
    //                };

    //                // 範疇二
    //                type2 = type2 + result.emissionVolume;
    //                outResults.Add(result);
    //            }

    //            // 如果都沒輸入就直接回傳吧
    //            if (outResults.Count == 0)
    //                return outResults;
    //            // 不然就來排序一下，不過輸入的時候都是採用順序輸入的，所以這段應該無用
    //            else
    //                outResults = outResults.OrderBy(e => e.index).ToList();

    //            // 計算總和
    //            var total = type1 + type2;
    //            outResults.First().totalVolume = total;


    //            // 計算各別比例
    //            outResults.ForEach(e => e.emissionRate = e.emissionVolume / total * 100);

    //            // 計算範疇總和
    //            var firstCatlogs = outResults.Where(e => e.catlogType.Equals("範疇一"));
    //            if (firstCatlogs.Count() > 0)
    //                firstCatlogs.FirstOrDefault().catlogVolume = firstCatlogs.Sum(e => e.emissionVolume);

    //            var secondCatlogs = outResults.OrderBy(e => e.index).Where(e => e.catlogType.Equals("範疇二"));
    //            if (secondCatlogs.Count() > 0)
    //                secondCatlogs.FirstOrDefault().catlogVolume = secondCatlogs.Sum(e => e.emissionVolume);
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.WriteLine(ex.ToString());
    //        }
    //        return outResults;
    //    }
    //}

    //public class DetailResult
    //{
    //    public int index { get; set; }// 排序用

    //    public string emissionName { get; set; }//排放源名稱

    //    public string ingregientName { get; set; }// 原物料名稱

    //    public string emissonType { get; set; }// 排放種類，固定、移動、製成、逸散

    //    public double volume { get; set; } // 活動數據

    //    public string unit { get; set; }// 排放單位

    //    public decimal coe { get; set; }// 碳排系數

    //    public decimal emissionVolume { get; set; } //碳排體積

    //    public decimal emissionRate { get; set; }//排放所占比例

    //    public decimal totalVolume { get; set; }//總排放量

    //    public decimal catlogVolume { get; set; } // 範疇類別總排放量
    //    public string catlogType { get; set; } // 範疇一、二、三
    //}
}