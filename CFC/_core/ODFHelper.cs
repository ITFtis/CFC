using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace CFC
{
    public class ODFHelper
    {
        /// <summary>
        /// Excel轉ODF(ods)
        /// </summary>
        /// <param name="FromPath">來源</param>
        /// <param name="TargetPath">目的</param>
        public static bool ExcelToODF(string FromPath, string TargetPath)
        {
            bool result = false;

            try
            {
                var application = new Microsoft.Office.Interop.Excel.Application();
                var workbooks = application.Workbooks;
                var workbook = workbooks.Open(FromPath);

                string ODFPath = TargetPath + ".ods";
                Logger.Log.For(null).Error("aaa");
                workbook.SaveAs(ODFPath, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenDocumentSpreadsheet);
                
                workbook.Close(false, null, null);
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(workbooks);
                application.Visible = false;
                Marshal.ReleaseComObject(application);

                System.Threading.Thread.Sleep(100);

                result = true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;

                Logger.Log.For(null).Error("Excel轉ODF(FromPath)：" + FromPath);
                Logger.Log.For(null).Error("Excel轉ODF(TargetPath)：" + TargetPath);
                Logger.Log.For(null).Error("ExcelToODF：" + error);
            }

            return result;
        }

        
    }
}