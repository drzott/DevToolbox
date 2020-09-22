using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Report.Util
{
    public static class FileUtil
    {
        public static string GetReportFolderByKpiId(long id, string baseReportPath, string kpiName)
        {
            if (baseReportPath == null)
                return string.Empty;

            if (!Directory.Exists(baseReportPath))
                return string.Empty;

            DirectoryInfo[] d = new DirectoryInfo(baseReportPath).GetDirectories("(" + id + ")*", SearchOption.TopDirectoryOnly);

            if(d == null || d.Count() < 1)
            {
                string newDir = Path.Combine(baseReportPath, "(" + id + ") " + kpiName);
                Directory.CreateDirectory(newDir);
                return newDir;
            }

            return d[0].FullName;
        }

        public static string GetValidFilename(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;

            return (fileName.Replace('/', ' ')
                        .Replace('\\', ' ')
                        .Replace('"', ' ')
                        .Replace('*', ' ')
                        .Replace('#', ' ')
                        .Replace('<', ' ')
                        .Replace('>', ' ')
                        .Replace('|', ' '))
                        ;
        }

        public static void DeleteAllFilesInDirectory(string directory)
        {
            string[] files = Directory.GetFiles(directory);

            if (files == null || files.Count() == 0)
                return;

            foreach (string f in files)
            {
                try
                {
                    File.Delete(f);
                }
                catch (Exception)
                {
                    ;
                }
            }
        }
    }
}
