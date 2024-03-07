using CFC.Controllers.Prj;
using CFC.Models;
using CFC.Models.Api;
using CFC.Models.Joined.Calculate;
using CFC.Models.Prj;
using Dou.Models.DB;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static CFC.Models.Api.ApiResultReturn;

namespace CFC.Controllers.Api
{

    public class CFCDataController : ApiController
    {
        private DouModelContext db = new DouModelContext();


        [Route("api/cfc/cal")]
        [HttpGet]
        public CFCResult Cal()
        {
            return new CFCResult { Success = false };
        }

        internal const string GUEST = "guest";

        #region 計算
        [Route("api/cfc/cal")]
        [HttpPost]
        public async Task<CFCResult> CalAsync(CalInputModel input)
        {
            var result = new CFCResult { Success = true };

            try
            {
                CalResult cr = new CalResult
                {
                    R1 = 0,
                    R2 = 0,
                    R3 = 0,
                    R4 = 0,
                    R5 = 0,
                    R6 = 0, //其他排放
                };

                //R1、R2(燃料 - 直接排放)
                foreach (var inputFuel in input.fuelInputs)
                {
                    //  判斷正常數值
                    if (inputFuel.UseVolume > 0)
                    {
                        var fuelProperty = DateViewController.AllFuelProperties.Where(e => e.Id == inputFuel.FuelId).FirstOrDefault();
                        if (fuelProperty == null) continue; //沒找到對應的就換下一個了

                        // 設定AR4、AR5、AR6
                        fuelProperty.ARType = (RType)Enum.Parse(typeof(RType), input.calModel, false);

                        // 計算碳排
                        if (fuelProperty.FuelType == "dynamicFluid")
                            cr.R2 = cr.R2 + Convert.ToDecimal(inputFuel.UseVolume) * fuelProperty.Co2e;
                        else
                            cr.R1 = cr.R1 + Convert.ToDecimal(inputFuel.UseVolume) * fuelProperty.Co2e;
                    }
                }

                //R3(冷媒逸散 - 直接排放)
                foreach (var refrigeInput in input.refrigerantInputs)
                {
                    if (refrigeInput.UseVolume > 0)
                    {

                        var equip = DateViewController.AllRefrigerantEquip.Where(e => e.Id == refrigeInput.RefrigerantEquip).FirstOrDefault();
                        var type = DateViewController.AllRefrigerantType.Where(e => e.Id == refrigeInput.RefrigerantType).FirstOrDefault();
                        if (equip == null || type == null) continue;

                        cr.R3 += Convert.ToDecimal(refrigeInput.UseVolume) * (decimal)equip.EscapeRate * (decimal)type.GWP * (decimal)0.001;
                    }
                }

                //(其他逸散 - 直接排放 - 逸散)
                foreach (var escapeInput in input.escapeInputs)
                {
                    var property = DateViewController.AllEscapeProperties.FirstOrDefault(e => e.Id.Equals(escapeInput.EscapeId));
                    if (property != null)
                        cr.R3 = cr.R3 + Convert.ToDecimal(property.Co2e) * Convert.ToDecimal(escapeInput.UseVolume);
                }

                //(特殊製程 - 直接排放 - 固定)
                foreach (var specialInput in input.specialInputs)
                {
                    var property = DateViewController.AllSpecificProperties.FirstOrDefault(e => e.Id.Equals(specialInput.CreateId));
                    if (property != null)
                        cr.R1 = cr.R1 + property.Co2e * Convert.ToDecimal(specialInput.UseVolume);
                }

                //(蒸氣 - 間接排放)
                if (input.steamInput.SteamVolume >= 0 && input.steamInput.SteamCoe > 0)
                {
                    cr.R4 = cr.R4 + (decimal)(input.steamInput.SteamVolume * (double)input.steamInput.SteamCoe);
                }

                //R4(電力 - 間接排放)
                if (input.electInput.elecVolume > 0)
                {
                    var selectYear = DateViewController.AllElecProperties.Where(e => e.year == input.electInput.elecYear).FirstOrDefault();

                    if (selectYear != null)
                        cr.R4 += (decimal)input.electInput.elecVolume * selectYear.Co2e;
                }

                // R6(其他排放)
                cr.R6 += (decimal)input.Tr01;
                cr.R6 += (decimal)input.Tr02;
                cr.R6 += (decimal)input.Tr03;
                cr.R6 += (decimal)input.Tr04;
                cr.R6 += (decimal)input.Cp01;
                cr.R6 += (decimal)input.Cp02;
                cr.R6 += (decimal)input.Cp03;
                cr.R6 += (decimal)input.Cp04;
                cr.R6 += (decimal)input.Cp05;
                cr.R6 += (decimal)input.Us01;
                cr.R6 += (decimal)input.Us02;
                cr.R6 += (decimal)input.Us03;
                cr.R6 += (decimal)input.Us04;
                cr.R6 += (decimal)input.Us05;
                cr.R6 += (decimal)input.Us06;
                cr.R6 += (decimal)input.Other;


                //R5(總排放量 = 間接排放 + 直接排放)
                cr.R5 = cr.R1 + cr.R2 + cr.R3 + cr.R4 + cr.R6;
                if (cr.R5 == 0)
                {
                    result.Success = false;
                    result.Message = "無輸入計算參數資料!!";
                }
                else
                {
                    cr.R1_3 = cr.R1 + cr.R2 + cr.R3;
                    cr.R1_3_R = (cr.R1_3 / cr.R5) * 100;
                    cr.R1_R = (cr.R1 / cr.R5) * 100;
                    cr.R2_R = (cr.R2 / cr.R5) * 100;
                    cr.R3_R = (cr.R3 / cr.R5) * 100;
                    cr.R4_R = (cr.R4 / cr.R5) * 100;
                    cr.R6_R = (cr.R6 / cr.R5) * 100;
                }

                result.Result = cr;
                if (result.Success && input.UserID != null && input.UserID.Trim().Length > 0)
                {
                    var userInput = new User_Input_Advance
                    {
                        UserID = input.UserID,
                        Date = DateTime.Now,
                        elecYear = input.electInput.elecYear,
                        elecVolume = input.electInput.elecVolume,
                        SteamVolume = input.steamInput.SteamVolume,
                        SteamCoe = input.steamInput.SteamCoe,
                        ARType = input.calModel.ToString(),
                        //ARType = Enum.GetName(typeof(RType), input.calModel),
                        Tr01 = input.Tr01,
                        Tr02 = input.Tr02,
                        Tr03 = input.Tr03,
                        Tr04 = input.Tr04,
                        Cp01 = input.Cp01,
                        Cp02 = input.Cp02,
                        Cp03 = input.Cp03,
                        Cp04 = input.Cp04,
                        Cp05 = input.Cp05,
                        Us01 = input.Us01,
                        Us02 = input.Us02,
                        Us03 = input.Us03,
                        Us04 = input.Us04,
                        Us05 = input.Us05,
                        Us06 = input.Us06,
                        Other = input.Other
                    };
                    await this.addUserInputAdvance(userInput);

                    input.fuelInputs.ForEach(fuelInput =>
                    {
                        this.db.FuelVolumes.Add(new Fuel_volume
                        {
                            RowId = userInput.RowID,
                            FuelId = fuelInput.FuelId,
                            UseVolume = fuelInput.UseVolume
                        });
                    });
                    this.db.SaveChanges();

                    for (int index = 0; index < input.refrigerantInputs.Count; index++)
                    {
                        var refriInput = input.refrigerantInputs[index];
                        this.db.RefrigerantVolume.Add(new Refrigerant_volume
                        {
                            Id = index,
                            RowId = userInput.RowID,
                            RefrigerantType = refriInput.RefrigerantType,
                            RefrigerantEquip = refriInput.RefrigerantEquip,
                            UseVolume = refriInput.UseVolume
                        });
                    }
                    this.db.SaveChanges();

                    for (int index = 0; index < input.escapeInputs.Count; index++)
                    {
                        var escapeInpu = input.escapeInputs[index];
                        this.db.EscapeVolume.Add(new Escape_volume
                        {
                            Id = index,
                            RowId = userInput.RowID,
                            EscapeId = escapeInpu.EscapeId,
                            EscapeType = escapeInpu.EscapeType,
                            UseVolume = escapeInpu.UseVolume,
                        });
                    }
                    this.db.SaveChanges();
                    for (int index = 0; index < input.specialInputs.Count; index++)
                    {
                        var specialInput = input.specialInputs[index];
                        this.db.SpecificVolume.Add(new Specific_volume
                        {
                            Id = index,
                            RowId = userInput.RowID,
                            CreateId = specialInput.CreateId,
                            CreateType = specialInput.CreateType,
                            UseVolume = specialInput.UseVolume
                        });
                    }
                    this.db.SaveChanges();

                    result.Result.RowID = userInput.RowID;
                }


            }
            catch (Exception ex)
            {
                result.Success = false;
                Debug.Write(ex.ToString());
            }
            return result;
        }

        public async Task addUserInputAdvance(User_Input_Advance input)
        {
            var returnInput = this.db.userInputAdvance.Add(input);
            await this.db.SaveChangesAsync();
        }

        public static async Task<UserInput_Advance> AddUserInput(UserInput_Advance input)
        {
            var theModelEntity = DateViewController.UserInputModelEntity;

            // 先塞入基本輸入後，取得自動生成的ID
            var basicInput = input.basicInput;
            var last = DateViewController.GetLastUserInput(basicInput.UserId);
            int currentRowID;

            // 如果過去5分鐘內有重複輸入，則採用更新方法

            if (last != null && (basicInput.Date - last.Date).TotalSeconds < 5 * 60)
            {
                Dou.Misc.HelperUtilities.CopyProperty<User_InputAdvance>(theModelEntity._context, basicInput, last, true);
                await theModelEntity.UpdateAsync(last);
                currentRowID = last.Id;

                /*
                 *  刪除既有的所有 特殊製程、逸散氣體
                 */
                // 逸散
                foreach (var item in DateViewController.GetEscapeContext().GetAll(e => e.RowId == last.Id))
                {
                    await DateViewController.GetEscapeContext().DeleteAsync(item);
                }

                // 製成
                foreach (var item in DateViewController.GetSpecificContext().GetAll(e => e.RowId == last.Id))
                {
                    await DateViewController.GetSpecificContext().DeleteAsync(item);
                }
            }

            // 新增一筆數據
            else
            {
                await theModelEntity.AddAsync(basicInput);
                currentRowID = basicInput.Id;
            }


            /*
             * 重新輸入進階的資訊
             */
            // 逸散
            for (int index = 0; index < input.escapeVolumes.Count; index++)
            {
                var volume = input.escapeVolumes[index];
                volume.RowId = currentRowID; // 關聯性到基本的輸入內容編號
                volume.Id = index; // 本次輸入的編號(排序用)

                await DateViewController.GetEscapeContext().AddAsync(volume); //匯入
            }

            // 製成
            for (int index = 0; index < input.specificVolumes.Count; index++)
            {
                var volume = input.specificVolumes[index];
                volume.RowId = currentRowID; // 關聯性到基本的輸入內容編號
                volume.Id = index; // 本次輸入的編號(排序用)

                await DateViewController.GetSpecificContext().AddAsync(volume); //匯入
            }
            return input;
        }

        [Route("api/cfc/cal2")]
        [HttpGet]
        public async Task<CFCResult> CalByJson(string input)
        {
            var inp = Newtonsoft.Json.JsonConvert.DeserializeObject<CalInputModel>(input);
            return await CalAsync(inp);
        }
        #endregion

    }
}