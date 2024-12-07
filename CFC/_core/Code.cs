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

    #endregion
}