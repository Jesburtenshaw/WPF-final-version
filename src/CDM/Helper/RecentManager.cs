using CDM.Common;
using CDM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CDM.Helper
{
    public static class RecentManager
    {
        #region :: Variables ::
        private static string recentFolder = Environment.GetFolderPath(Environment.SpecialFolder.Recent);

        public static ObservableCollection<FileFolderModel> RecentItemList = new ObservableCollection<FileFolderModel>();
        #endregion

        #region :: Methods ::
        public static ObservableCollection<FileFolderModel> GetRecentItems()
        {

            RecentItemList.Clear();

            if (RegistryManager.IsUsingRegistry)
            {
                List<(string FilePath, DateTime? LastOpenedDate)> defaultRecentFiles = RegistryManager.GetAllValues(RegistryKeys.Recent);

                foreach ((string FilePath, DateTime? LastOpenedDate) recentFile in defaultRecentFiles)
                {

                    if (String.IsNullOrEmpty(recentFile.FilePath))
                    {
                        continue;
                    }
                    if (File.Exists(recentFile.FilePath))
                    {
                        FileInfo fileInfo = new FileInfo(recentFile.FilePath);
                        FileAttributes f = File.GetAttributes(recentFile.FilePath);

                        RecentItemList.Add(new FileFolderModel()
                        {
                            Name = fileInfo.Name,
                            LastModifiedDateTime = recentFile.LastOpenedDate ?? DateTime.Now,
                            Path = fileInfo.FullName,
                            IconSource = IconHelper.GetIcon(fileInfo.FullName),
                            IsPined = PinManager.IsPined(fileInfo.FullName),
                            Type = "File"
                        });

                    }
                    else
                    {
                        //remove from registry as not found
                        RegistryManager.DeleteValue(RegistryKeys.Recent, recentFile.FilePath);
                    }

                }
            }
            else
            {
                // Get all files in the Recent folder
                string[] recentFiles = Directory.GetFiles(recentFolder);
                foreach (string recentFile in recentFiles)
                {
                    var file = ShortcutHelper.GetLnkTarget(recentFile);
                    if (String.IsNullOrEmpty(file))
                    {
                        continue;
                    }
                    if (File.Exists(file))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        FileAttributes f = File.GetAttributes(file);
                        //Application.Current.Dispatcher.Invoke(() =>
                        //{
                        RecentItemList.Add(new FileFolderModel()
                        {

                            Name = fileInfo.Name,
                            LastModifiedDateTime = fileInfo.LastWriteTime,
                            Path = fileInfo.FullName,
                            IconSource = IconHelper.GetIcon(fileInfo.FullName),
                            OriginalPath = recentFile,
                            IsPined = PinManager.IsPined(fileInfo.FullName),
                            Type = "File"
                        });
                        //});
                    }
                    
                }
            }

            if (RecentItemList != null && RecentItemList.Count() > 100)
            {
                RemoveRecentItemsMoreThan100();
            }

            return RecentItemList;
        }

        private static void RemoveRecentItemsMoreThan100()
        {
            //Remove old items 
            var allRecentItems = RecentItemList.OrderByDescending(s => s.LastModifiedDateTime).ToList();
            for (int i = allRecentItems.Count() - 1; i >= 100; i--)
            {
                Remove(allRecentItems[i]);
            }
        }

        public static void Add(FileFolderModel item)
        {
            if (RegistryManager.IsUsingRegistry)
            {
                //TODO add recent by application type, add conditions
                RegistryManager.AddOrUpdateValue(RegistryKeys.Recent, item.Path, DateTime.Now);

            }
            else
            {
                var t = RecentItemList.FirstOrDefault(e => e.Path.Equals(item.Path));
                if (null != t)
                {
                    return;
                }

                if (item.Type == "File")
                {
                    item.OriginalPath = Path.Combine(recentFolder, $"{Path.GetFileNameWithoutExtension(item.Path)}.lnk");
                }
                ShortcutHelper.CreateLnk(item.OriginalPath, item.Path);

            }
            if (!RecentItemList.Contains(item))
            {
                RecentItemList.Insert(0, item);
            }
            CollectionViewSource.GetDefaultView(RecentItemList).Refresh();
        }

        public static void Remove(FileFolderModel item)
        {
            if (RegistryManager.IsUsingRegistry)
            {
                RegistryManager.DeleteValue(RegistryKeys.Recent, item.Path);
                //RegistryManager.DeleteValue(RegistryKeys.WordRecent, item.Path);
                //RegistryManager.DeleteValue(RegistryKeys.PowerPointRecent, item.Path);
                //RegistryManager.DeleteValue(RegistryKeys.ExcelRecent, item.Path);
            }
            else
            {
                var t = RecentItemList.FirstOrDefault(e => e.Path.Equals(item.Path));
                if (null == t)
                {
                    return;
                }
                File.Delete(item.OriginalPath);
            }

            RecentItemList.Remove(item);
            CollectionViewSource.GetDefaultView(RecentItemList).Refresh();
        }


        #endregion
    }
}
