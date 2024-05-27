using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDM.Common
{
    public static class DirectoryHelper
    {
        public static string GetDirectoryName(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                return string.Empty;
            }

            folderPath = folderPath.TrimEnd('/', '\\');
            var index = folderPath.LastIndexOf('\\');
            if (index < 0)
            {
                index = folderPath.LastIndexOf('/');
            }
            if (index < 0)
            {
                return string.Empty;
            }

            return folderPath.Substring(index + 1);
        }

        public static void CheckAndCreateDirectory(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
    }
}
