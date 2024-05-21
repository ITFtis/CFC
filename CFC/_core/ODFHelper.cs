using System;
using System.Collections.Generic;
using System.Linq;
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
            Microsoft.Office.Interop.Excel.Workbook book = ExcelApp.Workbooks.Open(FromPath);
            Microsoft.Office.Interop.Excel.XlFileFormat xlFormatPDF = (Microsoft.Office.Interop.Excel.XlFileFormat)57;

            string PDFPath = TargetPath + ".pdf";
            string ODFPath = TargetPath + ".ods";
            try
            {
                book.SaveAs(ODFPath, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenDocumentSpreadsheet);
                book.SaveAs(PDFPath, xlFormatPDF);
                ExcelApp.Visible = false;
                ExcelApp.Quit();

                result = true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                ExcelApp.Quit();

                return false;
            }

            return result;
        }
    }
}