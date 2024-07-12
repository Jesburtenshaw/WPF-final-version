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
    public class PinManager
    {

        #region :: Variables ::
        // Path to the Taskbar pinned items folder
        private string pinFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar");

        public ObservableCollection<FileFolderModel> PinnedItemList = new ObservableCollection<FileFolderModel>();
        private DriveManager _driveManager;
        #endregion

        public PinManager(DriveManager driveManager)
        {
            _driveManager = driveManager;
        }

        #region :: Methods ::
        public bool IsPinLimitReached { get; set; } = false;
        public ObservableCollection<FileFolderModel> GetPinnedItems(StarManager starManager)
        {
            PinnedItemList.Clear();
            //if (RegistryManager.IsUsingRegistry)
            //{

            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //});

            // Get all files in the pinned registry
            List<(string FilePath, DateTime? LastOpenedDate)> defaultPinFiles = RegistryManager.GetAllValues(RegistryKeys.Pins);

            foreach ((string FilePath, DateTime? LastOpenedDate) pinnedFile in defaultPinFiles)
            {
                var file = pinnedFile.FilePath;
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
                        LastModifiedDateTime = pinnedFile.LastOpenedDate ?? DateTime.Now,
                        Path = fileInfo.FullName,
                        IconSource = IconHelper.GetIcon(fileInfo.FullName),
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
                        LastModifiedDateTime = pinnedFile.LastOpenedDate ?? DateTime.Now,
                        Path = dirInfo.FullName,
                        IconSource = IconHelper.GetIcon(dirInfo.FullName),
                        IsPined = true,
                        IsDefault = starManager.IsDefault(dirInfo.FullName),
                        Type = "Dir",
                        IsDrive = IsThisPathDrive(file)
                    });
                    //});
                }
                else
                {
                    //remove from registry
                    RegistryManager.DeleteValue(RegistryKeys.Pins, file);
                }
            }
            //}
            //else
            //{
            //    // Get all files in the pinned items folder
            //    string[] pinnedFiles = Directory.GetFiles(pinFolder);

            //    //// Get all folders in the pinned items folder
            //    //string[] pinnedFolders = Directory.GetDirectories(pinFolder);

            //    foreach (string pinnedFile in pinnedFiles)
            //    {
            //        var file = ShortcutHelper.GetLnkTarget(pinnedFile);
            //        if (String.IsNullOrEmpty(file))
            //        {
            //            continue;
            //        }
            //        if (File.Exists(file))
            //        {
            //            FileInfo fileInfo = new FileInfo(file);
            //            FileAttributes f = File.GetAttributes(file);
            //            //Application.Current.Dispatcher.Invoke(() =>
            //            //{
            //            PinnedItemList.Add(new FileFolderModel()
            //            {
            //                Name = fileInfo.Name,
            //                LastModifiedDateTime = fileInfo.LastWriteTime,
            //                Path = fileInfo.FullName,
            //                IconSource = IconHelper.GetIcon(fileInfo.FullName),
            //                OriginalPath = pinnedFile,
            //                IsPined = true,
            //                Type = "File"
            //            });
            //            //});
            //        }
            //        else if (Directory.Exists(file))
            //        {

            //            DirectoryInfo dirInfo = new DirectoryInfo(file);
            //            //Application.Current.Dispatcher.Invoke(() =>
            //            //{
            //            PinnedItemList.Add(new FileFolderModel()
            //            {
            //                Name = dirInfo.Name,
            //                LastModifiedDateTime = dirInfo.LastWriteTime,
            //                Path = dirInfo.FullName,
            //                IconSource = IconHelper.GetIcon(dirInfo.FullName),
            //                OriginalPath = pinnedFile,
            //                IsPined = true,
            //                IsDefault = StarManager.IsDefault(dirInfo.FullName),
            //                Type = "Dir",
            //                IsDrive = IsThisPathDrive(file)
            //            });
            //            //});
            //        }

            //    }
            //}

            UpdatePinLimitReached();
            return PinnedItemList;
        }

        private void UpdatePinLimitReached()
        {
            if (PinnedItemList != null && PinnedItemList.Count >= 100)
            {
                IsPinLimitReached = true;
            }
            else
            {
                IsPinLimitReached = false;
            }
        }

        public bool IsPined(string fileOrFolderName)
        {
            return PinnedItemList.FirstOrDefault(item => item.Path.Equals(fileOrFolderName)) != null;
        }

        public void Pin(FileFolderModel item)
        {
            //if (RegistryManager.IsUsingRegistry)
            //{
            //TODO add pin by application  type, add conditions
            RegistryManager.AddOrUpdateValue(RegistryKeys.Pins, item.Path, DateTime.Now);
            //}
            //else
            //{
            //    var t = PinnedItemList.FirstOrDefault(e => e.Path.Equals(item.Path));
            //    if (null != t)
            //    {
            //        return;
            //    }

            //    if (!string.IsNullOrEmpty(item.OriginalPath))
            //    {
            //        File.Copy(item.OriginalPath, Path.Combine(pinFolder, Path.GetFileName(item.OriginalPath)), true);
            //        item.OriginalPath = Path.Combine(pinFolder, Path.GetFileName(item.OriginalPath));
            //    }
            //    else
            //    {
            //        if (item.Type == "File")
            //        {
            //            item.OriginalPath = Path.Combine(pinFolder, $"{Path.GetFileNameWithoutExtension(item.Path)}.lnk");
            //        }
            //        else
            //        {
            //            item.OriginalPath = Path.Combine(pinFolder, $"{DirectoryHelper.GetDirectoryName(item.Path)}.lnk");
            //        }
            //        ShortcutHelper.CreateLnk(item.OriginalPath, item.Path);
            //    }
            //}

            if (!PinnedItemList.Contains(item))
            {
                PinnedItemList.Insert(0, item);
            }
            CollectionViewSource.GetDefaultView(PinnedItemList).Refresh();
            UpdatePinLimitReached();
            if (item.IsDrive)
            {
                UpdatePinnedDrives();
            }
        }

        public void Unpin(FileFolderModel item)
        {
            //if (RegistryManager.IsUsingRegistry)
            //{
            RegistryManager.DeleteValue(RegistryKeys.Pins, item.Path);
            //RegistryManager.DeleteValue(RegistryKeys.WordPins, item.Path);
            //RegistryManager.DeleteValue(RegistryKeys.PowerPointPins, item.Path);
            //RegistryManager.DeleteValue(RegistryKeys.ExcelPins, item.Path);
            //}
            //else
            //{
            //    var t = PinnedItemList.FirstOrDefault(e => e.Path.Equals(item.Path));
            //    if (null == t)
            //    {
            //        return;
            //    }
            //    File.Delete(item.OriginalPath);
            //}

            PinnedItemList.Remove(item);
            CollectionViewSource.GetDefaultView(PinnedItemList).Refresh();
            UpdatePinLimitReached();
            if (item.IsDrive)
            {
                UpdatePinnedDrives();
            }
        }

        private bool IsThisPathDrive(string path)
        {
            if (!string.IsNullOrWhiteSpace(path) && _driveManager.Drives != null && _driveManager.Drives?.Count(s => s.Name == path) > 0)
            {
                return true;
            }
            return false;
        }


        public void UpdatePinnedDrives()
        {
            try
            {
                foreach (DriveModel drive in _driveManager.DriveList)
                {
                    drive.IsPined = IsPined(drive.DriveName);
                }
                _driveManager.DriveList = new ObservableCollection<DriveModel>(_driveManager.DriveList.OrderByDescending(s => s.IsPined).ThenBy(s => s.DriveName).ToList());
                _driveManager.DrivesUpdatedFromPinManger();
            }
            catch
            {
            }
        }

        #endregion
    }
}
