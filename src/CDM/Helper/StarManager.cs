using CDM.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDM.Helper
{
    public static class StarManager
    {
        private static string starFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CDM");

        static StarManager()
        {
            DirectoryHelper.CheckAndCreateDirectory(starFolder);
        }

        public static bool IsDefault(string folderPath)
        {
            return GetDefault(folderPath, out string driveStarFile) == folderPath;
        }

        public static string GetDefault(string folderPath, out string driveStarFile)
        {
            driveStarFile = Path.Combine(starFolder, $"{folderPath.Substring(0, 1)}.txt");
            if (!File.Exists(driveStarFile))
            {
                return string.Empty;
            }

            var driveStarPath = File.ReadAllText(driveStarFile);
            if (string.IsNullOrEmpty(driveStarPath))
            {
                return string.Empty;
            }
            if (!Directory.Exists(driveStarPath))
            {
                return string.Empty;
            }

            return driveStarPath;
        }


        public static string SetDefault(string folderPath)
        {
            string driveStarFile = "";
            var driveStarPath = GetDefault(folderPath, out driveStarFile);
            if (driveStarPath.Equals(folderPath))
            {
                folderPath = "";
            }
            File.WriteAllText(driveStarFile, folderPath);
            return driveStarPath;
        }
    }
}
