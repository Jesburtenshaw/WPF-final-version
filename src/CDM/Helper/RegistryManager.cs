using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Security.RightsManagement;

namespace CDM.Helper
{

    public enum RegistryKeys
    {
        Pins,
        Recent,
        WordRecent,
        PowerPointRecent,
        ExcelRecent,
        WordPins,
        PowerPointPins,
        ExcelPins,
    }

    public static class RegistryManager
    {
        private const string BasePath = @"Software\IAM Cloud\Cloud Drive Mapper\OfficeAdd-In";
        private static readonly string[] SubPaths = { "Pins", "Recent", "Word\\Recent", "PowerPoint\\Recent", "Excel\\Recent", "Word\\Pins", "PowerPoint\\Pins", "Excel\\Pins", };

        //public RegistryManager()
        //{
        //    foreach (var subPath in SubPaths)
        //    {
        //        EnsureKeyExists(BasePath + "\\" + subPath);
        //    }
        //}

        public static bool IsUsingRegistry = false;

        public static void CheckMostRecentAndEnsureKeyExists()
        {
            try
            {
                foreach (var subPath in SubPaths)
                {
                    string keyPath = BasePath + "\\" + subPath;
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(keyPath))
                    {
                        // The key is now created if it didn't exist
                    }
                }

                if (JsonHelper.GetMostRecentValue() == "Registry")
                {
                    IsUsingRegistry = true;
                }
                else
                {
                    IsUsingRegistry = false;
                    return;
                }

               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AddOrUpdateValue(RegistryKeys keyName, string filePath, DateTime lastOpenedDate)
        {
            try
            {
                string path = GetSubPath(keyName);
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path, writable: true))
                {
                    key?.SetValue(filePath, lastOpenedDate.ToString("o"));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<(string FilePath, DateTime? LastOpenedDate)> GetAllValues(RegistryKeys keyName)
        {
            List<(string FilePath, DateTime? LastOpenedDate)> values = new List<(string FilePath, DateTime? LastOpenedDate)>();

            try
            {
                string path = GetSubPath(keyName);
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path))
                {
                    if (key != null)
                    {
                        foreach (var valueName in key.GetValueNames())
                        {
                            try
                            {
                                string filePath = valueName;
                                string dateStr = key.GetValue(valueName) as string;
                                DateTime? lastOpenedDate = DateTime.TryParse(dateStr, out var date) ? date : (DateTime?)null;
                                values.Add((filePath, lastOpenedDate));
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return values;
        }

        public static DateTime? GetDateTimeFromObjectDate(Object dt)
        {
            string dateStr = dt as string;
            return DateTime.TryParse(dateStr, out var date) ? date : (DateTime?)null;
        }


        public static void DeleteValue(RegistryKeys keyName, string name)
        {
            try
            {
                string path = GetSubPath(keyName);
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path, writable: true))
                {
                    key?.DeleteValue(name, throwOnMissingValue: false);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static string GetSubPath(RegistryKeys keyName)
        {
            switch (keyName)
            {

                case RegistryKeys.Pins:
                    return BasePath + "\\Pins";

                case RegistryKeys.Recent:
                    return BasePath + "\\Recent";

                case RegistryKeys.WordRecent:
                    return BasePath + "\\Word\\Recent";

                case RegistryKeys.PowerPointRecent:
                    return BasePath + "\\PowerPoint\\Recent";

                case RegistryKeys.ExcelRecent:
                    return BasePath + "\\Excel\\Recent";

                case RegistryKeys.WordPins:
                    return BasePath + "\\Word\\Pins";

                case RegistryKeys.PowerPointPins:
                    return BasePath + "\\PowerPoint\\Pins";

                case RegistryKeys.ExcelPins:
                    return BasePath + "\\Excel\\Pins";

                default:
                    throw new ArgumentException("Invalid part specified");
            }

        }



    }
}
