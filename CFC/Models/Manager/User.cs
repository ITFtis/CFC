using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CFC.Models.Manager
{
    [Table("User")]
    public class User : Dou.Models.UserBase
    {
        [Display(Name = "密碼", Order = 0)]
        [StringLength(80)] //System.Web.Helpers.Crypto.HashPassword會超過預設50
        [Required]
        [ColumnDef(Visible = false)]
        public override string Password { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "E-mail")]
        public string Email { set; get; }
    }
}