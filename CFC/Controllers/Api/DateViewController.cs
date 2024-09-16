using CFC.Controllers.Prj;
using CFC.Models;
using CFC.Models.Prj;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace CFC.Controllers.Api
{
    public class DateViewController : ApiController
    {
        private DouModelContext db = new DouModelContext();

        #region 取得使用者輸入
        internal static User_InputAdvance GetLastUserInput(string userid)
        {
            var sdd = UserInputModelEntity.GetAll(s => s.UserId == userid).Take(1).FirstOrDefault();
            return sdd;
        }
        internal static async Task<User_InputAdvance> GetLastUserInputaAsync(string userid, Dou.Models.DB.ModelEntity<User_InputAdvance> ModelEntity)
        {
            var r = await ModelEntity.GetAsync(s => s.UserId == userid);
            return r;
        }
        internal static User_InputAdvance DefaultUserInput
        {
            get
            {
                User_InputAdvance r = new User_InputAdvance();

                var mps = typeof(User_InputAdvance).GetProperties().Where(x => x.CanRead).ToList();
                var p1 = typeof(Fuel_properties).GetProperties().Where(x => x.CanRead).ToList();
                //燃料計算預設值
                foreach (var fp in AllFuelProperties)
                {
                    var mp = mps.FirstOrDefault(x => x.Name == fp.Id);
                    if (mp != null)
                    {
                        mp.SetValue(r, fp.MinValue, null);
                    }
                }
                //電力計算預設值
                var theElecProperties = AllElecProperties;
                r.elecVolume = theElecProperties.First().MinValue;
                r.elecYear = theElecProperties.First().year;

                //冷媒逸散計算預設值
                var theRefrigerantType = AllRefrigerantType;
                foreach (var re in AllRefrigerantEquip)
                {
                    var mp = mps.FirstOrDefault(x => x.Name == re.Id);
                    if (mp != null)
                    {
                        mp.SetValue(r, re.MinValue, null);
                        //mps.First(s => s.Name == mp.Name + "_T").SetValue(r, theRefrigerantType.First().Id, null);
                        //每一設備冷媒順序不一樣
                        mps.First(s => s.Name == mp.Name + "_T").SetValue(r, re.EquipTypeMap.First().Refrigerant_typeId, null);
                    }
                }
                return r;
            }
        }

        internal static Dou.Models.DB.ModelEntity<User_InputAdvance> UserInputModelEntity
        {
            get
            {
                return new Dou.Models.DB.ModelEntity<User_InputAdvance>(new DouModelContext());
            }
        }
        #endregion

        #region  企業資料

        public static IEnumerable<Global_Industrial> AllIndustrialType
        {
            get
            {
                IEnumerable<Global_Industrial> datas = DouHelper.Misc.GetCache<IEnumerable<Global_Industrial>>(5 * 1000); //先不cache
                if (datas == null || datas.Count() == 0)
                {
                    datas = new Dou.Models.DB.ModelEntity<Global_Industrial>(new DouModelContext()).GetAll().OrderBy(e => e.DisplayOrder).ToArray();
                }
                return datas;
            }
        }

        public static IEnumerable<Global_City> AllCity
        {
            get
            {
                IEnumerable<Global_City> datas = DouHelper.Misc.GetCache<IEnumerable<Global_City>>(5 * 1000); //先不cache
                if (datas == null || datas.Count() == 0)
                {
                    datas = new Dou.Models.DB.ModelEntity<Global_City>(new DouModelContext()).GetAll().OrderBy(e => e.Id).ToArray();
                }
                return datas;
            }
        }

        public static IEnumerable<Global_IndustrialArea> AllIndustrialArea
        {
            get
            {
                IEnumerable<Global_IndustrialArea> datas = DouHelper.Misc.GetCache<IEnumerable<Global_IndustrialArea>>(5 * 1000); //先不cache
                if (datas == null || datas.Count() == 0)
                {
                    datas = new Dou.Models.DB.ModelEntity<Global_IndustrialArea>(new DouModelContext()).GetAll().OrderBy(e => e.Id).ToArray();
                }
                return datas;
            }
        }

        internal static Dou.Models.DB.ModelEntity<User_Properties_Advance> UserPropertiesModelEntity
        {
            get
            {
                return new Dou.Models.DB.ModelEntity<User_Properties_Advance>(new DouModelContext());
            }
        }
        internal static User_Properties_Advance GetUserProperties(string id, bool insertnew = false)
        {
            var kvs = Dou.Misc.HelperUtilities.GetKeyValues<User_Properties>(new User_Properties { IndustryId = id }, UserPropertiesModelEntity._context);
            var user = UserPropertiesModelEntity.Find(kvs);
            if (insertnew && user == null)
            {
                user = new User_Properties_Advance { IndustryId = id, Name = "系統自動產" };
                UserPropertiesModelEntity.Add(user);
            }
            return user;
        }
        internal static IEnumerable<User_Properties_Advance> AllUserProperties
        {
            get
            {
                IEnumerable<User_Properties_Advance> datas = DouHelper.Misc.GetCache<IEnumerable<User_Properties_Advance>>(5 * 1000); //先不cache
                if (datas == null)
                {
                    datas = new UserPropertiesController().GetAllData();
                }
                return datas;
            }
        }

        
        internal static IEnumerable<SYS_FACTORY> All_SYS_FACTORY_properties
        {
            get
            {
                IEnumerable<SYS_FACTORY> datas = DouHelper.Misc.GetCache<IEnumerable<SYS_FACTORY>>(5 * 1000); //先不cache
                if (datas == null)
                {
                    datas = new SYS_FACTORYController().GetAllData();
                }
                return datas;
            }
        }

        internal static IEnumerable<SYS_COMPANY> All_SYS_COMPANY_properties
        {
            get
            {
                IEnumerable<SYS_COMPANY> datas = DouHelper.Misc.GetCache<IEnumerable<SYS_COMPANY>>(5 * 1000); //先不cache
                if (datas == null)
                {
                    datas = new SYS_COMPANYController().GetAllData();
                }
                return datas;
            }
        }

        #endregion

        #region 計算資料

        /// <summary>
        /// 燃料資料
        /// </summary>
        internal static IEnumerable<Fuel_properties> AllFuelProperties
        {
            get
            {
                IEnumerable<Fuel_properties> datas = DouHelper.Misc.GetCache<IEnumerable<Fuel_properties>>(5 * 1000); //先不cache
                if (datas == null)
                {
                    datas = new FuelPropertiesController().GetAllData();

                    //datas = datas.OrderBy(s => s.seq.HasValue ? s.seq : int.MaxValue);
                }
                return datas;
            }
        }

        /// <summary>
        /// 電力
        /// </summary>
        internal static IEnumerable<Elec_properties> AllElecProperties
        {
            get
            {
                IEnumerable<Elec_properties> datas = DouHelper.Misc.GetCache<IEnumerable<Elec_properties>>(5 * 1000); //先不cache
                if (datas == null)
                {
                    datas = new ElecPropertiesController().GetAllData();
                }
                return datas;
            }
        }
        /// <summary>
        /// 設備資料
        /// </summary>
        internal static IEnumerable<Refrigerant_equip> AllRefrigerantEquip
        {
            get
            {
                IEnumerable<Refrigerant_equip> datas = DouHelper.Misc.GetCache<IEnumerable<Refrigerant_equip>>(5 * 1000, "GetAllDataIncludeMap"); //先不cache
                if (datas == null)
                {
                    datas = new RefrigerantEquipController().GetAllDataIncludeMap();
                    foreach (var f in datas)
                        f.EquipTypeMap = f.EquipTypeMap.OrderBy(s => s.displayOrder).ToList();
                }
                return datas;
            }
        }
        /// <summary>
        /// 冷媒資料
        /// </summary>
        public static IEnumerable<Refrigerant_type> AllRefrigerantType
        {
            get
            {
                IEnumerable<Refrigerant_type> datas = DouHelper.Misc.GetCache<IEnumerable<Refrigerant_type>>(5 * 1000); //先不cache
                if (datas == null)
                {
                    datas = new RefrigerantTypeController().GetAllData();
                }
                return datas;
            }
        }

        /// <summary>
        /// 其他逸散
        /// </summary>
        public static IEnumerable<Escape_type> AllEscapeType
        {
            get
            {
                IEnumerable<Escape_type> datas = DouHelper.Misc.GetCache<IEnumerable<Escape_type>>(5 * 1000); //先不cache
                if (datas == null)
                {
                    datas = new Dou.Models.DB.ModelEntity<Escape_type>(new DouModelContext()).GetAll().OrderBy(e => e.displayOrder).ToArray();
                }
                return datas;
            }
        }

        public static IEnumerable<Escape_properties> AllEscapeProperties
        {
            get
            {
                IEnumerable<Escape_properties> datas = DouHelper.Misc.GetCache<IEnumerable<Escape_properties>>(5 * 1000); //先不cache
                if (datas == null)
                {
                    datas = new Dou.Models.DB.ModelEntity<Escape_properties>(new DouModelContext()).GetAll().OrderBy(e => e.displayOrder).ToArray();
                }
                return datas;
            }
        }

        public static IModelEntity<Escape_volume> GetEscapeContext()
        {
            return new Dou.Models.DB.ModelEntity<Escape_volume>(new DouModelContext());
        }

        /// <summary>
        /// 特殊製程
        /// </summary>
        public static IEnumerable<Specific_type> AllSpecificType
        {
            get
            {
                IEnumerable<Specific_type> datas = DouHelper.Misc.GetCache<IEnumerable<Specific_type>>(5 * 1000); //先不cache
                if (datas == null)
                {
                    datas = new Dou.Models.DB.ModelEntity<Specific_type>(new DouModelContext()).GetAll().OrderBy(e => e.displayOrder).ToArray();
                }
                return datas;
            }
        }

        public static IEnumerable<Specific_properties> AllSpecificProperties
        {
            get
            {
                IEnumerable<Specific_properties> datas = DouHelper.Misc.GetCache<IEnumerable<Specific_properties>>(5 * 1000); //先不cache
                if (datas == null)
                {
                    datas = new Dou.Models.DB.ModelEntity<Specific_properties>(new DouModelContext()).GetAll().OrderBy(e => e.displayOrder).ToArray();
                }
                return datas;
            }
        }

        public static IModelEntity<Specific_volume> GetSpecificContext()
        {
            return new Dou.Models.DB.ModelEntity<Specific_volume>(new DouModelContext());
        }
        #endregion


    }
}
