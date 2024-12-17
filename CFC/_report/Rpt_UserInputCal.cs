using CFC.Controllers.Api;
using CFC.Controllers.FileDownload.ExcelManagerF;
using CFC.Models.Api;
using CFC.Models.Prj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFC
{
    public class Rpt_UserInputCal
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static CalInputModel ToCalInputModel(User_Input_Advance input)
        {
            CalInputModel result = null;

            try
            {
                CalInputModel o = new CalInputModel();
                o.calModel = input.ARType;

                //類別(1)
                //燃料計算 - 固態燃料
                o.fuelInputs = new List<FuelInputs>();
                var fuelInputs = Fuel_volume.GetAllDatas().Where(a => a.RowId == input.RowID).ToList();
                o.fuelInputs = fuelInputs.Select(a => new FuelInputs
                {
                    FuelId = a.FuelId,
                    UseVolume = a.UseVolume
                }).ToList();

                //////燃料計算 - 液態燃料
                //////燃料計算 - 氣態燃料
                //////移動源

                //冷媒逸散計算(冷媒設備)
                o.refrigerantInputs = new List<RefrigerantInput>();
                var refrigerantInputs = Refrigerant_volume.GetAllDatas().Where(a => a.RowId == input.RowID).ToList();
                o.refrigerantInputs = refrigerantInputs.Select(a => new RefrigerantInput
                {
                    RefrigerantType = a.RefrigerantType,
                    RefrigerantEquip = a.RefrigerantEquip,
                    UseVolume = a.UseVolume
                }).ToList();

                //其他逸散
                o.escapeInputs = new List<CFC.Models.Api.EscapeInput>();
                var escapeInputs = Escape_volume.GetAllDatas().Where(a => a.RowId == input.RowID).ToList();
                o.escapeInputs = escapeInputs.Select(a => new CFC.Models.Api.EscapeInput
                {
                    EscapeId = a.EscapeId,
                    EscapeType = a.EscapeType,
                    UseVolume = a.UseVolume,
                }).ToList();

                //特殊製程計算
                o.specialInputs = new List<SpecialInput>();
                var specialInputs = Specific_volume.GetAllDatas().Where(a => a.RowId == input.RowID).ToList();
                o.specialInputs = specialInputs.Select(a => new SpecialInput
                {
                    CreateId = a.CreateId,
                    CreateType = a.CreateType,
                    UseVolume = a.UseVolume
                }).ToList();

                //類別(2)
                //電力計算


                //蒸氣計算

                //類別(3)

                //類別(4)

                //類別(5)

                //類別(6)

                result = o;
            }
            catch (Exception ex)
            {
                logger.Error("Model轉換失敗：ToCalInputModel()");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);

                return null;
            }

            return result;
        }

        /// <summary>
        /// 後台，取得專案各類別計算值
        /// </summary>
        /// <returns></returns>
        public static decimal GetCal(int vType, CalInputModel input)
        {
            if (input == null)
            {
                logger.Error(string.Format("vType={0},input(CalInputModel)不可為null", vType.ToString()));
                return 0;
            }
            
            decimal result = 0;

            //合計
            decimal sum = 0;

            try
            {
                if (vType == 1)
                {
                    //*******類別1*******
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
                            //20250905, 改成要依據AR4、AR5、AR6，來乘上相關的系數
                            if (fuelProperty.FuelType == "dynamicFluid")
                            {
                                if (fuelProperty.ARType == RType.AR4)
                                    sum = sum + Convert.ToDecimal(inputFuel.UseVolume) * fuelProperty.Co2e_AR4;

                                else if (fuelProperty.ARType == RType.AR5)
                                    sum = sum + Convert.ToDecimal(inputFuel.UseVolume) * fuelProperty.Co2e_AR5;

                                else if (fuelProperty.ARType == RType.AR6)
                                    sum = sum + Convert.ToDecimal(inputFuel.UseVolume) * fuelProperty.Co2e_AR6;

                            }
                            else
                            {
                                if (fuelProperty.ARType == RType.AR4)
                                    sum = sum + Convert.ToDecimal(inputFuel.UseVolume) * fuelProperty.Co2e_AR4;
                                else if (fuelProperty.ARType == RType.AR5)
                                    sum = sum + Convert.ToDecimal(inputFuel.UseVolume) * fuelProperty.Co2e_AR5;
                                else if (fuelProperty.ARType == RType.AR6)
                                    sum = sum + Convert.ToDecimal(inputFuel.UseVolume) * fuelProperty.Co2e_AR6;
                            }


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

                            //20250905, 改成要依據AR4、AR5、AR6，來乘上相關的系數
                            if (input.calModel == "AR4")
                                sum += Convert.ToDecimal(refrigeInput.UseVolume) * (decimal)equip.EscapeRate * (decimal)type.GWP_AR4 * (decimal)0.001;
                            else if (input.calModel == "AR5")
                                sum += Convert.ToDecimal(refrigeInput.UseVolume) * (decimal)equip.EscapeRate * (decimal)type.GWP_AR5 * (decimal)0.001;
                            else if (input.calModel == "AR6")
                                sum += Convert.ToDecimal(refrigeInput.UseVolume) * (decimal)equip.EscapeRate * (decimal)type.GWP_AR6 * (decimal)0.001;
                        }
                    }

                    //(其他逸散 - 直接排放 - 逸散)
                    foreach (var escapeInput in input.escapeInputs)
                    {
                        var property = DateViewController.AllEscapeProperties.FirstOrDefault(e => e.Id.Equals(escapeInput.EscapeId));
                        if (property != null)
                        {
                            //20250905, 改成要依據AR4、AR5、AR6，來乘上相關的系數
                            if (input.calModel == "AR4")
                                sum = sum + Convert.ToDecimal(property.Co2e_AR4) * Convert.ToDecimal(escapeInput.UseVolume);
                            else if (input.calModel == "AR5")
                                sum = sum + Convert.ToDecimal(property.Co2e_AR5) * Convert.ToDecimal(escapeInput.UseVolume);
                            else if (input.calModel == "AR6")
                                sum = sum + Convert.ToDecimal(property.Co2e_AR6) * Convert.ToDecimal(escapeInput.UseVolume);
                        }
                    }

                    //(特殊製程 - 直接排放 - 固定)
                    foreach (var specialInput in input.specialInputs)
                    {
                        var property = DateViewController.AllSpecificProperties.FirstOrDefault(e => e.Id.Equals(specialInput.CreateId));
                        if (property != null)
                        {
                            //20250909, 改成要依據AR4、AR5、AR6，來乘上相關的系數
                            if (input.calModel == "AR4")
                            {
                                sum = sum + property.Co2e_AR4 * Convert.ToDecimal(specialInput.UseVolume);
                            }
                            else if (input.calModel == "AR5")
                            {
                                sum = sum + property.Co2e_AR5 * Convert.ToDecimal(specialInput.UseVolume);
                            }
                            else if (input.calModel == "AR6")
                            {
                                sum = sum + property.Co2e_AR6 * Convert.ToDecimal(specialInput.UseVolume);
                            }

                        }
                    }
                }
                else if (vType == 2)
                {
                    //*******類別2*******
                }
                else if (vType == 3)
                {
                    //*******類別3*******
                }
                else if (vType == 4)
                {
                    //*******類別4*******
                }
                else if (vType == 5)
                {
                    //*******類別5*******
                }
                else if (vType == 6)
                {
                    //*******類別6*******
                }

                result = sum;
            }
            catch (Exception ex)
            {
                logger.Error("Model轉換失敗：ToCalInputModel()");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);

                return 0;
            }

            return result;
        }
    }
}