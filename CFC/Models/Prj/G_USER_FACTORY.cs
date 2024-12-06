using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFC.Models.Prj
{
    using Dou.Misc.Attr;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("G_USER_FACTORY")]

    public partial class G_USER_FACTORY
    {
        [Key]
        [Display(Name = "IDX")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int IDX { get; set; }

        [Display(Name = "會員帳號")]
        //[ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string USER_ID { get; set; }

        /// <summary>
        /// 聯絡人
        /// </summary>
        [Display(Name = "聯絡人")]
        [ColumnDef(VisibleEdit = false)]
        public string Contact
        {
            get
            {
                string str = "";
                var u = User_Properties_Advance.GetAllDatas().Where(a => a.Id == this.USER_ID).FirstOrDefault();
                if (u != null)
                    str = u.Contact;

                return str;
            }
        }

        [Display(Name = "工廠登記證及名稱")]
        [ColumnDef(EditType = EditType.Select, SelectItemsClassNamespace = SYS_FACTORYSelectItems.AssemblyQualifiedName)]
        public string FACTORY_REGISTRATION { get; set; }

        /// <summary>
        /// 工廠地址
        /// </summary>
        [Display(Name = "工廠地址")]
        [ColumnDef(VisibleEdit = false)]
        public string FACTORY_NAME
        {
            get
            {
                string str = "";
                var u = SYS_FACTORY.GetAllDatas().Where(a => a.FACTORY_REGISTRATION == this.FACTORY_REGISTRATION).FirstOrDefault();
                if (u != null)
                {
                    string cname = "";
                    string tname = "";
                    string addr = u.FACTORY_ADDRESS;

                    var c = CitySelectItems.CITIES.Where(a => a.Name == u.FACTORY_CITY).FirstOrDefault();
                    if(c != null)
                        cname = c.Name;

                    var t = TownSelectItems.Towns.Where(a => a.Name == u.FACTORY_CITY).FirstOrDefault();
                    if (t != null)
                        tname = t.Name;

                    str = cname + tname + addr;
                }
                    

                return str;
            }
        }

        [Display(Name = "工業區")]
        [ColumnDef(VisibleEdit = false,
            EditType = EditType.Select, SelectItemsClassNamespace = CFC.Models.Prj.Global_IndustrialAreaSelectItems.AssemblyQualifiedName)]
        public string FACTORY_INDUSTRIAL_AREA 
        {
            get
            {
                string str = "";
                var u = SYS_FACTORY.GetAllDatas().Where(a => a.FACTORY_REGISTRATION == this.FACTORY_REGISTRATION).FirstOrDefault();
                if (u != null)
                {
                    var code = Global_IndustrialAreaSelectItems.GlobalIndustrialAreas.Where(a => a.Id == u.FACTORY_INDUSTRIAL_AREA).FirstOrDefault();
                    if (code != null)
                        str = code.Name;
                }

                return str;
            }
        }

        [Display(Name = "工廠產業類型")]
        [ColumnDef(VisibleEdit = false, 
            EditType = EditType.Select, SelectItemsClassNamespace = CFC.Models.Prj.Global_IndustrialSelectItems.AssemblyQualifiedName)]
        public string FACTORY_INDUSTRIAL
        {
            get
            {
                string str = "";
                var u = SYS_FACTORY.GetAllDatas().Where(a => a.FACTORY_REGISTRATION == this.FACTORY_REGISTRATION).FirstOrDefault();
                if (u != null)
                {
                    var code = Global_IndustrialSelectItems.GlobalIndustrials.Where(a => a.Id == u.FACTORY_INDUSTRIAL).FirstOrDefault();
                    if (code != null)
                        str = code.Name;
                }

                return str;
            }
        }

        [Display(Name = "建檔日")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string BDate { get; set; }

        [Display(Name = "建檔者ID")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string BId { get; set; }

        [Display(Name = "修改日")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string UDate { get; set; }

        [Display(Name = "修改者ID")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string UId { get; set; }

        /// <summary>
        /// 行業別 1.非製造業,其它製造業(ex.2,3,...99等)
        /// </summary>
        [Display(Name = "行業別")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string IndustrialTypeId
        {
            get 
            {
                string str = "";
                var u = User_Properties_Advance.GetAllDatas().Where(a => a.Id == this.USER_ID).FirstOrDefault();
                if (u != null)
                    str = u.IndustrialTypeId;

                return str;
            }  
        }
    }
}