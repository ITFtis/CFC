using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CFC
{
    public class ZipHelper
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ///
        /// 壓縮檔案
        ///
        ///壓縮檔案路徑
        ///密碼
        ///註解
        public static bool ZipFiles(string path, string zipName, string password, string comment)
        {
            bool result = false;

            ZipOutputStream zos = null;

            try
            {
                string zipPath = path + @"\" + zipName + ".zip";
                ArrayList files = GetFiles(path);
                zos = new ZipOutputStream(File.Create(zipPath));
                if (password != null && password != string.Empty) zos.Password = password;
                if (comment != null && comment != "") zos.SetComment(comment);
                zos.SetLevel(9);//Compression level 0-9 (9 is highest)
                byte[] buffer = new byte[4096];

                foreach (string f in files)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(f));
                    entry.DateTime = DateTime.Now;
                    zos.PutNextEntry(entry);
                    FileStream fs = File.OpenRead(f);
                    int sourceBytes;

                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        zos.Write(buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);

                    fs.Close();
                    fs.Dispose();
                }

                result = true;
            }

            catch (Exception ex)
            {
                logger.Error("Zip壓縮Error");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            finally
            {
                zos.Finish();
                zos.Close();
                zos.Dispose();
            }

            return result;
        }

        ///
        /// 解壓縮檔案
        ///
        ///解壓縮檔案目錄路徑
        ///密碼
        public static bool UnZipFiles(string path, string password)
        {
            bool result = false;

            ZipInputStream zis = null;

            try
            {
                string unZipPath = path.Replace(".zip", "");
                CreateDirectory(unZipPath);
                zis = new ZipInputStream(File.OpenRead(path));
                if (password != null && password != string.Empty) zis.Password = password;
                ZipEntry entry;

                while ((entry = zis.GetNextEntry()) != null)
                {
                    string filePath = unZipPath + @"\" + entry.Name;

                    if (entry.Name != "")
                    {
                        FileStream fs = File.Create(filePath);
                        int size = 2048;
                        byte[] buffer = new byte[2048];
                        while (true)
                        {
                            size = zis.Read(buffer, 0, buffer.Length);
                            if (size > 0) { fs.Write(buffer, 0, size); }
                            else { break; }
                        }

                        fs.Close();
                        fs.Dispose();
                    }
                }

                result = true;
            }

            catch (Exception ex)
            {
                logger.Error("Zip解壓縮Error");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            finally
            {
                zis.Close();
                zis.Dispose();
            }

            return result;
        }

        //讀取目錄下所有檔案
        public static ArrayList GetFiles(string path)
        {
            ArrayList files = new ArrayList();

            if (Directory.Exists(path))
            {
                files.AddRange(Directory.GetFiles(path));
            }

            return files;
        }

        //建立目錄
        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}