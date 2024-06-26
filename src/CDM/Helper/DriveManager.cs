﻿using CDM.Common;
using CDM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CDM.Helper
{
    public static class DriveManager
    {
        public static ObservableCollection<DriveModel> DriveList = new ObservableCollection<DriveModel>();
        public static ObservableCollection<FilterConditionModel> Drives = new ObservableCollection<FilterConditionModel>();

        public static Tuple<ObservableCollection<DriveModel>, ObservableCollection<FilterConditionModel>> GetDrivesItem()
        {
            var fcm = new FilterConditionModel
            {
                Code = "",
                Name = "All drives"
            };
            fcm.PropertyChanged += FilterConditionModel_PropertyChanged;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Drives.Add(fcm);
            });

            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives().Where(item => item.DriveType == DriveType.Network).ToArray();
                foreach (DriveInfo drive in drives)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DriveList.Add(new DriveModel()
                        {
                            DriveName = drive.Name,//.TrimEnd('\\'),
                            DriveDescription = drive.VolumeLabel,
                            IsPined = PinManager.IsPined(drive.Name)
                        });
                    });
                    fcm = new FilterConditionModel
                    {
                        Code = drive.Name,
                        Name = drive.Name
                    };
                    fcm.PropertyChanged += FilterConditionModel_PropertyChanged;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Drives.Add(fcm);
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex);
            }
            return new Tuple<ObservableCollection<DriveModel>, ObservableCollection<FilterConditionModel>>(DriveList, Drives);
        }

        public static event EventHandler DriveIsSelectedChanged;

        private static void FilterConditionModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("IsSelected"))
            {
                return;
            }
            var fcm = sender as FilterConditionModel;
            if (string.IsNullOrEmpty(fcm.Code))
            {
                foreach (var drive in Drives)
                {
                    if (string.IsNullOrEmpty(drive.Code))
                    {
                        continue;
                    }
                    if (drive.IsSelected != fcm.IsSelected)
                    {
                        drive.IsSelected = fcm.IsSelected;
                    }
                }
            }
            else
            {
                if (Drives.Count(item => item.IsSelected && !string.IsNullOrEmpty(item.Code)) == 0)
                {
                    if (Drives.First().IsSelected) Drives.First().IsSelected = false;
                }
                else if (Drives.Count(item => !item.IsSelected && !string.IsNullOrEmpty(item.Code)) == 0)
                {
                    if (!Drives.First().IsSelected) Drives.First().IsSelected = true;
                }
            }
            DriveIsSelectedChanged?.Invoke(sender, e);
        }

        public static event EventHandler<bool> DrivesStateChanged;

        public static async Task Check(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(1000);
                if (ct.IsCancellationRequested)
                {
                    break;
                }
                DriveInfo[] _drives = DriveInfo.GetDrives().Where(item => item.DriveType == DriveType.Network).ToArray();
                var _driveNameList = _drives.Select(item => item.Name).ToList();
                var result = true;
                foreach (var drive in DriveList)
                {
                    result = result && _driveNameList.Contains(drive.DriveName);
                    if (!result)
                    {
                        DrivesStateChanged?.Invoke(null, false);
                        break;
                    }
                }
                if (result)
                {
                    DrivesStateChanged?.Invoke(null, true);
                }
            }
        }
    }
}
