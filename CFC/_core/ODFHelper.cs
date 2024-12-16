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

            var ExcelApp = new Microsoft.Office.Interop.Excel.Application();

            try
            {
                Microsoft.Office.Interop.Excel.Workbook book = ExcelApp.Workbooks.Open(FromPath);

                string ODFPath = TargetPath + ".ods";

                book.SaveAs(ODFPath, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenDocumentSpreadsheet);

                //finally關閉excel
                ////book.SaveAs(PDFPath, xlFormatPDF);
                ExcelApp.Visible = false;
                ExcelApp.Quit();

                result = true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;

                Logger.Log.For(null).Error("Excel轉ODF(FromPath)：" + FromPath);
                Logger.Log.For(null).Error("Excel轉ODF(TargetPath)：" + TargetPath);
                Logger.Log.For(null).Error("ExcelToODF：" + error);

                //finally關閉excel
                ExcelApp.Quit();
            }
            finally
            {
                ExcelHelper.KillExcel(ExcelApp);
                System.Threading.Thread.Sleep(100);
            }

            return result;
        }

        
    }
}