using CFC.Controllers.PrjNew;
using CFC.Models.Manager;
using CFC.Models.Prj;
using System;
using System.Data.Entity;
using System.Linq;

namespace CFC.Models
{
    public class DouModelContext : Dou.Models.ModelContextBase<User, Role>
    {
        // 您的內容已設定為使用應用程式組態檔 (App.config 或 Web.config)
        // 中的 'DouModel' 連接字串。根據預設，這個連接字串的目標是
        // 您的 LocalDb 執行個體上的 'CFC.Models.DouModel' 資料庫。
        // 
        // 如果您的目標是其他資料庫和 (或) 提供者，請修改
        // 應用程式組態檔中的 'DouModel' 連接字串。
        public DouModelContext()
            : base("name=DouModelContext")
        {
            Database.SetInitializer<DouModelContext>(null); //關閉給定內容類型(code first)初始化資料庫 migration in Entity Framework
        }

        // 針對您要包含在模型中的每種實體類型新增 DbSet。如需有關設定和使用
        // Code First 模型的詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=390109。

        public virtual DbSet<Log_count> LogCount { get; set; }
        public virtual DbSet<User_Properties> UserProperties { get; set; }
        public virtual DbSet<Fuel_properties> FuelProperties { get; set; }
        public virtual DbSet<Fuel_volume> FuelVolumes { get; set; }
        public virtual DbSet<Elec_properties> ElecProperties { get; set; }

        // 逸散冷媒
        public virtual DbSet<Refrigerant_equip> RefrigerantEquip { get; set; }
        public virtual DbSet<Refrigerant_type> RefrigerantType { get; set; }
        public virtual DbSet<Refrigerant_map> RefrigerantMap { get; set; }
        public virtual DbSet<Refrigerant_volume> RefrigerantVolume { get; set; }
        

        // 其他逸散氣體
        public virtual DbSet<Escape_properties> EscapeProperties { get; set; }
        public virtual DbSet<Escape_type> EscapeType { get; set; }
        public virtual DbSet<Escape_volume> EscapeVolume { get; set; }

        // 特定製程
        public virtual DbSet<Specific_properties> SpecificProperties { get; set; }
        public virtual DbSet<Specific_type> SpecificType { get; set; }
        public virtual DbSet<Specific_volume> SpecificVolume { get; set; }

        // 使用者相關
        public virtual DbSet<User_InputAdvance> UserInput { get; set; }
        public virtual DbSet<User_Projects> UserProjects { get; set; }//紀錄專案基本資訊
        public virtual DbSet<User_Properties_Advance> userPropertiesAdvance { get; set; } // 第二版需要

        public virtual DbSet<User_Input_Advance> userInputAdvance { get; set; } // 第二版需要
        public virtual DbSet<Global_Industrial> GlobalIndustrial { get; set; }
        public virtual DbSet<Global_City> GlobalCity { get; set; } //縣市別
        public virtual DbSet<Global_IndustrialArea> GlobalIndustrialArea { get; set; } //工業園區

        public virtual DbSet<SYS_FACTORY> SysFactory { get; set; } //工廠

        public virtual DbSet<SYS_COMPANY> SysCompany { get; set; } //工廠

        public virtual DbSet<G_USER_FACTORY> UserFactory { get; set; } //會員跟工廠的關聯


        public virtual DbSet<VolumeViewModel> VolumeViewModel { get; set; } 

        public virtual DbSet<Cals_type> CalsType { get; set; } //3-6類別

        public virtual DbSet<Cals_properties> CalsProperties { get; set; } //3-6類別項目

        public virtual DbSet<Sys_content> Sys_content { get; set; } //系統內容

        public virtual DbSet<Sys_contentDetail> Sys_contentDetail { get; set; } //系統內容細項

        public virtual DbSet<City> City { get; set; } //縣市
        public virtual DbSet<Town> Town { get; set; } //鄉鎮
        public virtual DbSet<vw_UserInputCal> vw_UserInputCal { get; set; } //(view)報表 會員計算紀錄明細
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ////// 定義組合鍵 USER_ID 和 FACTORY_REGISTRATION
            ////modelBuilder.Entity<G_USER_FACTORY>()
            ////    .HasKey(uf => new { uf.USER_ID, uf.FACTORY_REGISTRATION });


            // 類別1 燃料
            modelBuilder.Entity<Fuel_properties>()
                .Property(e => e.CO2)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Fuel_properties>()
                .Property(e => e.CH4)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Fuel_properties>()
                .Property(e => e.NO2)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Fuel_properties>()
                .Property(e => e.GCO2R4)
                    .HasPrecision(18, 10);
            modelBuilder.Entity<Fuel_properties>()
                .Property(e => e.GCH4R4)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Fuel_properties>()
                .Property(e => e.GNO2R4)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Fuel_properties>()
                .Property(e => e.GCO2R5)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Fuel_properties>()
                .Property(e => e.GCH4R5)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Fuel_properties>()
                .Property(e => e.GNO2R5)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Fuel_properties>()
                .Property(e => e.GCO2R6)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Fuel_properties>()
                .Property(e => e.GCH4R6)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Fuel_properties>()
                .Property(e => e.GNO2R6)
                .HasPrecision(18, 10);

            // 類別1 逸散
            modelBuilder.Entity<Escape_properties>()
                .Property(e => e.CH4)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Escape_properties>()
                .Property(e => e.CO2)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Escape_properties>()
                .Property(e => e.N2O)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Escape_properties>()
                .Property(e => e.SF6)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Escape_properties>()
                .Property(e => e.HFCs)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Escape_properties>()
                .Property(e => e.NF3)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Escape_properties>()
                .Property(e => e.PFCs)
                .HasPrecision(18, 10);
            modelBuilder.Entity<Refrigerant_type>()
                .Property(e => e.GWP)
                .HasPrecision(18, 10);

            // 類別2 外購
            modelBuilder.Entity<Elec_properties>()
                .Property(e => e.Co2e)
                .HasPrecision(18, 10);
            modelBuilder.Entity<User_Input_Advance>()
                .Property(e => e.SteamCoe)
                .HasPrecision(18, 10);
                   
            base.OnModelCreating(modelBuilder);
        }
    }


    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}