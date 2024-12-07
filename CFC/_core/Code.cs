using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFC
{
    public class Code
    {
        /// <summary>
        /// 行業別(Y/N),其它製造業(ex.2,3,...99等)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetYNGlobal_Industrial()
        {
            IEnumerable<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            result = result.Append(new KeyValuePair<string, object>("1", "非製造業"));
            result = result.Append(new KeyValuePair<string, object>("99", "製造業"));

            return result;
        }

        /// <summary>
        /// 單位性質
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetUserUNIT_TYPE()
        {
            IEnumerable<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            result = result.Append(new KeyValuePair<string, object>("一般公司", "一般公司"));
            result = result.Append(new KeyValuePair<string, object>("管顧公司", "管顧公司"));
            result = result.Append(new KeyValuePair<string, object>("法人", "法人"));
            result = result.Append(new KeyValuePair<string, object>("學校", "學校"));
            result = result.Append(new KeyValuePair<string, object>("其他", "其他"));

            return result;
        }
    }

    #region  下拉

    /// <summary>
    /// 行業別(Y/N),其它製造業(ex.2,3,...99等)
    /// </summary>
    public class GetYNGlobal_IndustrialSelectItems : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "CFC.GetYNGlobal_IndustrialSelectItems, CFC";

        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return Code.GetYNGlobal_Industrial().Select(a => new KeyValuePair<string, object>(a.Key, a.Value));
        }
    }

    /// <summary>
    /// 單位性質
    /// </summary>
    public class GetUserUNIT_TYPESelectItems : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "CFC.GetUserUNIT_TYPESelectItems, CFC";

        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return Code.GetUserUNIT_TYPE().Select(a => new KeyValuePair<string, object>(a.Key, a.Value));
        }
    }

    #endregion
}