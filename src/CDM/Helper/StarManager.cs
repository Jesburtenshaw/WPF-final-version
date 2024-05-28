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
        #region :: Variable ::
        private static string starFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CDM");
        #endregion
        #region :: Constructor ::
        static StarManager()
        {
            DirectoryHelper.CheckAndCreateDirectory(starFolder);
        }
        #endregion
        #region :: Methods ::
        /// <summary>
        /// This method return true or false 
        /// based on supplied folder path is default or not
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static bool IsDefault(string folderPath)
        {
            return GetDefault(folderPath, out string driveStarFile) == folderPath;
        }
        /// <summary>
        /// This method returns default path of file
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="driveStarFile"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method set as default path of folder path
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
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
        #endregion
    }
}
