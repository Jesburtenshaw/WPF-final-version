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
    public static class PinManager
    {
        #region :: Variables ::
        // Path to the Taskbar pinned items folder
        private static string pinFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar");

        public static ObservableCollection<FileFolderModel> PinnedItemList = new ObservableCollection<FileFolderModel>();
        #endregion

        #region :: Methods ::
        public static ObservableCollection<FileFolderModel> GetPinnedItems()
        {
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            PinnedItemList.Clear();
            //});

            // Get all files in the pinned items folder
            string[] pinnedFiles = Directory.GetFiles(pinFolder);

            //// Get all folders in the pinned items folder
            //string[] pinnedFolders = Directory.GetDirectories(pinFolder);

            foreach (string pinnedFile in pinnedFiles)
            {
                var file = ShortcutHelper.GetLnkTarget(pinnedFile);
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
                    PinnedItemList.Add(new FileFolderModel()
                    {
                        Name = fileInfo.Name,
                        LastModifiedDateTime = fileInfo.LastWriteTime,
                        Path = fileInfo.FullName,
                        IconSource = IconHelper.GetIcon(fileInfo.FullName),
                        OriginalPath = pinnedFile,
                        IsPined = true,
                        Type = "File"
                    });
                    //});
                }
                else if (Directory.Exists(file))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(file);
                    //Application.Current.Dispatcher.Invoke(() =>
                    //{
                    PinnedItemList.Add(new FileFolderModel()
                    {
                        Name = dirInfo.Name,
                        LastModifiedDateTime = dirInfo.LastWriteTime,
                        Path = dirInfo.FullName,
                        IconSource = IconHelper.GetIcon(dirInfo.FullName),
                        OriginalPath = pinnedFile,
                        IsPined = true,
                        IsDefault = StarManager.IsDefault(dirInfo.FullName),
                        Type = "Dir"
                    });
                    //});
                }

            }
            // Get all folders in the Recent folder
            //string[] recentFolders = Directory.GetDirectories(pinnedItemsFolderPath);
            return PinnedItemList;
        }

        public static bool IsPined(string fileOrFolderName)
        {
            return PinnedItemList.FirstOrDefault(item => item.Path.Equals(fileOrFolderName)) != null;
        }

        public static void Pin(FileFolderModel item)
        {
            var t = PinnedItemList.FirstOrDefault(e => e.Path.Equals(item.Path));
            if (null != t)
            {
                return;
            }

            if (!string.IsNullOrEmpty(item.OriginalPath))
            {
                File.Copy(item.OriginalPath, Path.Combine(pinFolder, Path.GetFileName(item.OriginalPath)), true);
                item.OriginalPath = Path.Combine(pinFolder, Path.GetFileName(item.OriginalPath));
            }
            else
            {
                if (item.Type == "File")
                {
                    item.OriginalPath = Path.Combine(pinFolder, $"{Path.GetFileNameWithoutExtension(item.Path)}.lnk");
                }
                else
                {
                    item.OriginalPath = Path.Combine(pinFolder, $"{DirectoryHelper.GetDirectoryName(item.Path)}.lnk");
                }
                ShortcutHelper.CreateLnk(item.OriginalPath, item.Path);
            }

            PinnedItemList.Insert(0, item);
            CollectionViewSource.GetDefaultView(PinnedItemList).Refresh();
        }

        public static void Unpin(FileFolderModel item)
        {
            var t = PinnedItemList.FirstOrDefault(e => e.Path.Equals(item.Path));
            if (null == t)
            {
                return;
            }
            File.Delete(item.OriginalPath);
            PinnedItemList.Remove(item);
            CollectionViewSource.GetDefaultView(PinnedItemList).Refresh();
        }
        #endregion
    }
}
