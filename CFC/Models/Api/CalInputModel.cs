using CFC.Models.Prj;
using System.Collections.Generic;

namespace CFC.Models.Api
{
    public class CalInputModel
    {

        public string UserID { get; set; }

        public int RowID { get; set; } // 給使用者建立專案實用的

        public string calModel { get; set; }

        // 專案建立需求
        public bool IsSave { get; set; }// 是否儲存為專案
        public string ProjectName { get; set; }//專案名稱
        public string ProjectIndustrialID { get; set; }//專案場登
        public string ProjectAddress { get; set; }//專案地址
        public string ProjectCity { get; set; }//專案縣市
        public string ProjectIndustrialType { get; set; }//專案行業別

        // 其他輸入源

        //燃料
        public List<FuelInputs> fuelInputs { get; set; }

        //電力
        public ElectInput electInput { get; set; }

        // 冷媒
        public List<RefrigerantInput> refrigerantInputs { get; set; }

        //逸散
        public List<EscapeInput> escapeInputs { get; set; }

        // 蒸氣
        public SteamInput steamInput { get; set; }

        // 特殊製程
        public List<SpecialInput> specialInputs { get; set; }



        // 類別3
        public double Tr01 { get; set; } // 類別3-運輸-上游原物料配送當量
        public double Tr02 { get; set; } // 類別3 - 運輸 - 商務旅遊
        public double Tr03 { get; set; } // 類別3 - 運輸 - 員工通勤
        public double Tr04 { get; set; } // 類別3 - 運輸 - 下游運輸及配送
        public double Cp01 { get; set; } // 類別3 - 組織使用產品 - 採購
        public double Cp02 { get; set; } // 類別3 - 組織使用產品 - 資本
        public double Cp03 { get; set; } // 類別3 - 組織使用產品 - 能源相關活動
        public double Cp04 { get; set; } // 類別3 - 組織使用產品 - 營運廢棄物
        public double Cp05 { get; set; } // 類別3 - 組織使用產品 - 上游資產租賃
        public double Us01 { get; set; } // 類別3 - 使用組織產品 - 加工
        public double Us02 { get; set; } // 類別3 - 使用組織產品 - 使用
        public double Us03 { get; set; } // 類別3 - 使用組織產品 - 報廢
        public double Us04 { get; set; } // 類別3 - 使用組織產品 - 下游租賃
        public double Us05 { get; set; } // 類別3 - 使用組織產品 - 加盟
        public double Us06 { get; set; } // 類別3 - 使用組織產品 - 投資
        public double Other { get; set; } // 類別3 - 其他排放

    }
}