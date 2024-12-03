using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFC.Models.Prj
{
    using CFC.Controllers.Api;
    using Dou.Misc.Attr;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SYS_COMPANY")]

    public partial class SYS_COMPANY
    {
        /// <summary>
        /// 主鍵，自動遞增的公司編號
        /// </summary>
        [Key]
        [Display(Name = "編號", Order = 0)]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int COMP_ID { get; set; }

        [Display(Name = "公司名稱", Order = 1)]
        [ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string COMP_NAME { get; set; }

        [Display(Name = "統一編號")] 
        //[ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string COMP_UNIFORM_NUMBER { get; set; }

        [Display(Name = "公司規模")] //中小企業, 或大企業
        //[ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string COMP_SIZE { get; set; }

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

        [Display(Name = "會員(帳號/密碼)")]
        [ColumnDef(VisibleEdit = false)]
        public string UserAccountPassword
        {
            get
            {
                var datas = DateViewController.AllUserProperties.Where(a => a.UniformNumber == this.COMP_UNIFORM_NUMBER);
                string str = string.Join("</br>", datas.Select((a, index) => "(" + (index + 1).ToString() + ")" + a.Id + "/" + a.Pass));

                return str;
            }
            
        }
    }

    public class SYS_COMPANYNameSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public const string AssemblyQualifiedName = "CFC.Models.Prj.SYS_COMPANYNameSelectItems, CFC";

        protected static IEnumerable<SYS_COMPANY> _sysCompanys;
        internal static IEnumerable<SYS_COMPANY> SysCompanys
        {
            get
            {
                if (_sysCompanys == null)
                {
                    using (var db = new DouModelContext())
                    {
                        _sysCompanys = db.SysCompany.ToArray();
                    }
                }
                return _sysCompanys;
            }
        }


        public static void Reset()
        {
            _sysCompanys = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return SysCompanys.Select(s => new KeyValuePair<string, object>(s.COMP_UNIFORM_NUMBER + "", s.COMP_NAME));
        }
    }
}