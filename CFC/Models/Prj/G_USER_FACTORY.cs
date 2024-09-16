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



        //[Key]
        //[Display(Name = "IDX", Order = 0)]
        //[ColumnDef(Visible = false, VisibleEdit = true)]
        //public string IDX { get; set; }

        [Display(Name = "會員號碼", Order = 1)]
        //[ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string USER_ID { get; set; }

        [Display(Name = "工廠登記證", Order = 2)]
        public string FACTORY_REGISTRATION { get; set; }


        [Display(Name = "建檔日", Order = 3)]
        public string BDate { get; set; }

        [Display(Name = "建檔者ID", Order = 4)]
        public string BId { get; set; }

        [Display(Name = "修改日", Order = 5)]
        public string UDate { get; set; }

        [Display(Name = "修改者ID", Order = 6)]
        public string UId { get; set; }


    }
}