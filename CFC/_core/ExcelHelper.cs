using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace CFC
{
    public class ExcelHelper
    {
        /// <summary>
        /// 關閉excel (Microsoft.Office.Interop.Excel.Workbook)
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="ProcessId"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int ProcessId);
        public static void KillExcel(Microsoft.Office.Interop.Excel.Application theApp)
        {
            int id = 0;
            IntPtr intptr = new IntPtr(theApp.Hwnd);
            System.Diagnostics.Process p = null;
            try
            {
                GetWindowThreadProcessId(intptr, out id);
                p = System.Diagnostics.Process.GetProcessById(id);
                if (p != null)
                {
                    p.Kill();
                    p.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Log.For(null).Error("KillExcel:" + ex.Message);
            }
        }
    }
}