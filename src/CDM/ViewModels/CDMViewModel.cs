﻿using CDM.Common;
using CDM.Helper;
using CDM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CDM.ViewModels
{
    internal class CDMViewModel : ViewModelBase
    {
        #region :: Constructor ::

        public CDMViewModel()
        {
            DoubleClickCommand = new RelayCommand(DoubleDriveClick);
            BackWindowCommand = new RelayCommand(BackNavigationClick);
            SearchBoxTextChangedCommand = new RelayCommand(searchBoxTextChanged);
            FolderItemDoubleClickCommand = new RelayCommand(FolderItemDoubleClick);
            RecentItemDoubleClickCommand = new RelayCommand(RecentItemDoubleClick);
            PinnedItemDoubleClickCommand = new RelayCommand(PinnedItemDoubleClick);

            //RecentItemSortCommand = new RelayCommand(RecentItemSort);

            DragDropTaskList = new ObservableCollection<DragDropTaskModel>();
            PagedDrivesList = new ObservableCollection<DriveModel>();
            DriveSelectedItem = new DriveModel();
            FoldersItemList = new ObservableCollection<FileFolderModel>();
            CurFolder = nullFolder;

            //SearchBox();

            TxtSearchGotFocusCommand = new RelayCommand(searchBoxGotFocus);
            TxtSearchLostFocusCommand = new RelayCommand(searchBoxLostFocus);
            //SearchItemCommand = new RelayCommand(searchItem);
            PinCommand = new RelayCommand(Pin);
            UnpinCommand = new RelayCommand(Unpin);
            StarCommand = new RelayCommand(Star);
            UnstarCommand = new RelayCommand(Unstar);
            OpenItemCommand = new RelayCommand(OpenItem);
            CopyItemPathCommand = new RelayCommand(CopyItemPath);
            RenameItemCommand = new RelayCommand(RenameItem);
            DeleteItemCommand = new RelayCommand(DeleteItem);
            DoRenameCommand = new RelayCommand(DoRename);
            CancelRenameCommand = new RelayCommand(CancelRename);
            RenameTextChangedCommand = new RelayCommand(RenameTextChanged);
            ClearSearchCommand = new RelayCommand(ClearSearch);
            ClearSearchErrorCommand = new RelayCommand(ClearSearchError);
            ShowDrivesCommand = new RelayCommand(ShowDrives);
            ShowTypesCommand = new RelayCommand(ShowTypes);
            ShowLocationsCommand = new RelayCommand(ShowLocations);
            DoFilterCommand = new RelayCommand(DoFilter);
            ResetFilterCommand = new RelayCommand(ResetFilter);
            DoSearchCommand = new RelayCommand(DoSearch);
            CancelSearchCommand = new RelayCommand(CancelSearch);
            PrevDrivesCommand = new RelayCommand(PrevDrives);
            NextDrivesCommand = new RelayCommand(NextDrives);

            //DriveCommand = new RelayCommand(driveCommand);
            IsSearchBoxPlaceholderVisible = Visibility.Visible;
            IsDriveWindowVisible = Visibility.Visible;
            IsDriveFoldersVisible = Visibility.Collapsed;

            this.PropertyChanged += CDMViewModel_PropertyChanged;
            DriveManager.DriveIsSelectedChanged += DriveManager_DriveIsSelectedChanged;

            CurSearchStatus.PropertyChanged += SearchStatus_PropertyChanged;
            CurFilterStatus.PropertyChanged += FilterStatus_PropertyChanged;
        }

        #endregion

        #region :: Properties ::

        public bool EntereingDrives { get; set; } = false;
        private string forbiddenChars = "\\/:*\"<>|";
        private string curNavigatingFolderPath = "";

        public int DrivesPageSize { get; set; }
        public int DrivesPagesCount { get; set; }

        private int curDrivesPagesIndex;
        public int CurDrivesPagesIndex
        {
            get { return curDrivesPagesIndex; }
            set
            {
                curDrivesPagesIndex = value;
                OnPropertyChanged(nameof(CurDrivesPagesIndex));
            }
        }

        private bool isFirstDrivesPage = true;
        public bool IsFirstDrivesPage
        {
            get { return isFirstDrivesPage; }
            set
            {
                isFirstDrivesPage = value;
                OnPropertyChanged(nameof(IsFirstDrivesPage));
            }
        }

        private bool isLastDrivesPage = true;
        public bool IsLastDrivesPage
        {
            get { return isLastDrivesPage; }
            set
            {
                isLastDrivesPage = value;
                OnPropertyChanged(nameof(IsLastDrivesPage));
            }
        }
        private ObservableCollection<DragDropTaskModel> _dragDropTaskList;
        public ObservableCollection<DragDropTaskModel> DragDropTaskList
        {
            get { return _dragDropTaskList; }
            set
            {
                _dragDropTaskList = value;
                OnPropertyChanged(nameof(DragDropTaskList));
            }
        }

        private CancellationTokenSource ctsSearch;

        public string CurrentDrivePath { get; set; }

        private static Stack<string> directoryHistory = new Stack<string>();

        private DriveModel _driveSelectedItem;
        public DriveModel DriveSelectedItem
        {
            get
            {
                return _driveSelectedItem;
            }
            set
            {
                if (null != value)
                {
                    _driveSelectedItem = value;
                    OnPropertyChanged(nameof(DriveSelectedItem));
                }
            }
        }

        private string _txtSearchBoxItem;
        public string TxtSearchBoxItem
        {
            get
            {
                return _txtSearchBoxItem;
            }
            set
            {
                _txtSearchBoxItem = value;
                OnPropertyChanged(nameof(TxtSearchBoxItem));
            }
        }

        private ObservableCollection<DriveModel> _driveList;
        public ObservableCollection<DriveModel> DriveList
        {
            get { return _driveList; }
            set
            {
                _driveList = value;
                OnPropertyChanged(nameof(DriveList));
            }
        }

        private ObservableCollection<FileFolderModel> _recentItemList;
        public ObservableCollection<FileFolderModel> RecentItemList
        {
            get { return _recentItemList; }
            set
            {
                _recentItemList = value;
                OnPropertyChanged(nameof(RecentItemList));
            }
        }

        private FileFolderModel _selectedRecentItem;
        public FileFolderModel SelectedRecentItem
        {
            get { return _selectedRecentItem; }
            set
            {
                if (null != value)
                {
                    _selectedRecentItem = value;
                    OnPropertyChanged(nameof(SelectedRecentItem));
                }
            }
        }

        private FileFolderModel _selectedPinnedItem;
        public FileFolderModel SelectedPinnedItem
        {
            get { return _selectedPinnedItem; }
            set
            {
                if (null != value)
                {
                    _selectedPinnedItem = value;
                    OnPropertyChanged(nameof(SelectedPinnedItem));
                }
            }
        }

        private ObservableCollection<FileFolderModel> _pinnedItemList;
        public ObservableCollection<FileFolderModel> PinnedItemList
        {
            get { return _pinnedItemList; }
            set
            {
                _pinnedItemList = value;
                OnPropertyChanged(nameof(PinnedItemList));
            }
        }

        private ObservableCollection<FileFolderModel> _foldersItemList;
        public ObservableCollection<FileFolderModel> FoldersItemList
        {
            get { return _foldersItemList; }
            set
            {
                _foldersItemList = value;
                OnPropertyChanged(nameof(FoldersItemList));
            }
        }

        private FileFolderModel _selectedFileFolderItem;
        public FileFolderModel SelectedFileFolderItem
        {
            get { return _selectedFileFolderItem; }
            set
            {
                if (null != value)
                {
                    _selectedFileFolderItem = value;
                    OnPropertyChanged(nameof(SelectedFileFolderItem));
                }
            }
        }

        private Visibility _isDriveWindowVisible;
        public Visibility IsDriveWindowVisible
        {
            get { return _isDriveWindowVisible; }
            set
            {
                _isDriveWindowVisible = value;
                OnPropertyChanged(nameof(IsDriveWindowVisible));
            }
        }

        private Visibility _isDriveFoldersVisible;
        public Visibility IsDriveFoldersVisible
        {
            get { return _isDriveFoldersVisible; }
            set
            {
                _isDriveFoldersVisible = value;
                OnPropertyChanged(nameof(IsDriveFoldersVisible));
            }
        }

        private FileFolderModel curRenameItem;
        public FileFolderModel CurRenameItem
        {
            get
            {
                return curRenameItem;
            }
            set
            {
                curRenameItem = value;
                OnPropertyChanged(nameof(CurRenameItem));
            }
        }

        private bool offline;
        public bool Offline
        {
            get
            {
                return offline;
            }
            set
            {
                offline = value;
                OnPropertyChanged(nameof(Offline));
            }
        }

        private FileFolderModel nullFolder = new FileFolderModel();
        private FileFolderModel curFolder;
        public FileFolderModel CurFolder
        {
            get
            {
                return curFolder;
            }
            set
            {
                curFolder = value;
                OnPropertyChanged(nameof(CurFolder));
            }
        }

        private SearchStatusModel curSearchStatus = new SearchStatusModel { CanSearch = true };
        public SearchStatusModel CurSearchStatus
        {
            get
            {
                return curSearchStatus;
            }
            set
            {
                curSearchStatus = value;
                OnPropertyChanged(nameof(CurSearchStatus));
            }
        }

        private FilterStatusModel curFilterStatus = new FilterStatusModel { };
        public FilterStatusModel CurFilterStatus
        {
            get
            {
                return curFilterStatus;
            }
            set
            {
                curFilterStatus = value;
                OnPropertyChanged(nameof(CurFilterStatus));
            }
        }

        private ObservableCollection<FilterConditionModel> types;
        public ObservableCollection<FilterConditionModel> Types
        {
            get
            {
                return types;
            }
            set
            {
                types = value;
                OnPropertyChanged(nameof(Types));
            }
        }

        private ObservableCollection<FilterConditionModel> drives;
        public ObservableCollection<FilterConditionModel> Drives
        {
            get
            {
                return drives;
            }
            set
            {
                drives = value;
                OnPropertyChanged(nameof(Drives));
            }
        }

        private ObservableCollection<DriveModel> pagedDrivesList;
        public ObservableCollection<DriveModel> PagedDrivesList
        {
            get
            {
                return pagedDrivesList;
            }
            set
            {
                pagedDrivesList = value;
                OnPropertyChanged(nameof(PagedDrivesList));
            }
        }

        private ObservableCollection<FilterConditionModel> locations;
        public ObservableCollection<FilterConditionModel> Locations
        {
            get
            {
                return locations;
            }
            set
            {
                locations = value;
                OnPropertyChanged(nameof(Locations));
            }
        }

        private FilterConditionModel selectedType;
        public FilterConditionModel SelectedType
        {
            get
            {
                return selectedType;
            }
            set
            {
                selectedType = value;
                OnPropertyChanged(nameof(SelectedType));
            }
        }

        private FilterConditionModel selectedLocation;
        public FilterConditionModel SelectedLocation
        {
            get
            {
                return selectedLocation;
            }
            set
            {
                selectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
            }
        }

        private Visibility _isSearchBoxPlaceholderVisible;
        public Visibility IsSearchBoxPlaceholderVisible
        {
            get { return _isSearchBoxPlaceholderVisible; }
            set
            {
                _isSearchBoxPlaceholderVisible = value;
                OnPropertyChanged(nameof(IsSearchBoxPlaceholderVisible));
            }
        }

        private RenameStatusModel curRenameStatus = new RenameStatusModel();
        public RenameStatusModel CurRenameStatus
        {
            get
            {
                return curRenameStatus;
            }
            set
            {
                curRenameStatus = value;
                OnPropertyChanged(nameof(CurRenameStatus));
            }
        }

        //private bool _isAscending = true;
        //public bool IsAscending
        //{
        //    get => _isAscending;
        //    set
        //    {
        //        _isAscending = value;
        //        OnPropertyChanged(nameof(IsAscending));
        //    }
        //}

        #endregion

        #region :: Commands ::

        public RelayCommand BackWindowCommand { get; set; }
        public RelayCommand DoubleClickCommand { get; set; }
        public RelayCommand SearchBoxTextChangedCommand { get; set; }
        public RelayCommand TxtSearchGotFocusCommand { get; set; }
        public RelayCommand TxtSearchLostFocusCommand { get; set; }
        public RelayCommand FolderItemDoubleClickCommand { get; set; }
        public RelayCommand RecentItemDoubleClickCommand { get; set; }
        public RelayCommand PinnedItemDoubleClickCommand { get; set; }
        public RelayCommand SearchItemCommand { get; set; }
        //public RelayCommand RecentItemSortCommand { get; set; }
        public RelayCommand PinCommand { get; set; }
        public RelayCommand UnpinCommand { get; set; }
        public RelayCommand StarCommand { get; set; }
        public RelayCommand UnstarCommand { get; set; }
        public RelayCommand OpenItemCommand { get; set; }
        public RelayCommand CopyItemPathCommand { get; set; }
        public RelayCommand RenameItemCommand { get; set; }
        public RelayCommand DeleteItemCommand { get; set; }
        public RelayCommand DoRenameCommand { get; set; }
        public RelayCommand CancelRenameCommand { get; set; }
        public RelayCommand RenameTextChangedCommand { get; set; }
        public RelayCommand ClearSearchCommand { get; set; }
        public RelayCommand ClearSearchErrorCommand { get; set; }
        public RelayCommand ShowDrivesCommand { get; set; }
        public RelayCommand ShowTypesCommand { get; set; }
        public RelayCommand ShowLocationsCommand { get; set; }
        public RelayCommand DoFilterCommand { get; set; }
        public RelayCommand ResetFilterCommand { get; set; }
        public RelayCommand DoSearchCommand { get; set; }
        public RelayCommand CancelSearchCommand { get; set; }
        public RelayCommand PrevDrivesCommand { get; set; }
        public RelayCommand NextDrivesCommand { get; set; }

        #endregion

        #region :: Methods ::

        public void Init()
        {
            PinnedItemList = PinManager.PinnedItemList;
            RecentItemList = RecentManager.RecentItemList;
            DriveList = DriveManager.DriveList;
            Drives = DriveManager.Drives;
            Types = FilterConditionModel.Types;

            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CurSearchStatus.IsLoading = true;
                    CurSearchStatus.IsLoadingDrives = true;
                    CurSearchStatus.IsLoadingPinned = true;
                    CurSearchStatus.IsLoadingRecent = true;
                });
                PinManager.GetPinnedItems();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CurFilterStatus.PinnedCount = PinnedItemList.Count;
                    CurSearchStatus.IsLoadingPinned = false;
                });
                RecentManager.GetRecentItems();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CurFilterStatus.RecentCount = RecentItemList.Count;
                    CurSearchStatus.IsLoadingRecent = false;
                });
                DriveManager.GetDrivesItem();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CurFilterStatus.DrivesCount = DriveList.Count;
                    if (CurFilterStatus.DrivesCount > 0)
                    {
                        CurDrivesPagesIndex = 1;
                    }

                    CurSearchStatus.IsLoading = false;
                    CurSearchStatus.IsLoadingDrives = false;
                });
            });
        }

        private void PrevDrives(object sender)
        {
            if (CurDrivesPagesIndex <= 1)
            {
                return;
            }
            CurDrivesPagesIndex--;
        }

        private void NextDrives(object sender)
        {
            if (CurDrivesPagesIndex >= DrivesPagesCount)
            {
                return;
            }
            CurDrivesPagesIndex++;
        }

        private void Pin(object obj)
        {
            FileFolderModel curItem = null;
            FileFolderModel unpinedItem = null;
            var driveItem = obj as DriveModel;
            if (null != driveItem)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(driveItem.DriveName);
                unpinedItem = new FileFolderModel
                {
                    Name = dirInfo.Name,
                    LastModifiedDateTime = dirInfo.LastWriteTime,
                    Path = dirInfo.FullName,
                    IconSource = IconHelper.GetIcon(dirInfo.FullName),
                    Type = "Dir",
                    IsPined = true
                };
            }
            else
            {
                curItem = obj as FileFolderModel;
                unpinedItem = new FileFolderModel
                {
                    Name = curItem.Name,
                    LastModifiedDateTime = curItem.LastModifiedDateTime,
                    Path = curItem.Path,
                    IconSource = curItem.IconSource,
                    Type = curItem.Type,
                    IsPined = true,
                    OriginalPath = curItem.OriginalPath
                };
            }
            try
            {
                PinManager.Pin(unpinedItem);

                FileFolderModel t = null;
                if (null == curItem || string.IsNullOrEmpty(curItem.OriginalPath))
                {
                    t = RecentItemList.FirstOrDefault(e => e.Path.Equals(unpinedItem.Path));
                    if (null != t)
                    {
                        t.IsPined = !t.IsPined;
                    }
                }
                else if (null == curItem || !string.IsNullOrEmpty(curItem.OriginalPath))
                {
                    t = FoldersItemList.FirstOrDefault(e => e.Path.Equals(unpinedItem.Path));
                    if (null != t)
                    {
                        t.IsPined = !t.IsPined;
                    }
                }
                if (null != driveItem)
                {
                    driveItem.IsPined = !driveItem.IsPined;
                }
                else
                {
                    curItem.IsPined = !curItem.IsPined;
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, $"Could not pin {unpinedItem.Name}");
            }
        }

        private void Unpin(object obj)
        {
            FileFolderModel pinedItem = null;
            FileFolderModel curItem = null;
            var driveItem = obj as DriveModel;
            if (null != driveItem)
            {
                pinedItem = PinnedItemList.FirstOrDefault(e => e.Path.Equals(driveItem.DriveName));
            }
            else
            {
                curItem = obj as FileFolderModel;
                pinedItem = PinnedItemList.FirstOrDefault(e => e.Path.Equals(curItem.Path));
            }
            if (null == pinedItem)
            {
                return;
            }
            try
            {
                PinManager.Unpin(pinedItem);

                FileFolderModel t = null;
                if (null == curItem || string.IsNullOrEmpty(curItem.OriginalPath))
                {
                    t = RecentItemList.FirstOrDefault(e => e.Path.Equals(pinedItem.Path));
                    if (null != t)
                    {
                        t.IsPined = !t.IsPined;
                    }
                }
                else if (null == curItem || !string.IsNullOrEmpty(curItem.OriginalPath))
                {
                    t = FoldersItemList.FirstOrDefault(e => e.Path.Equals(pinedItem.Path));
                    if (null != t)
                    {
                        t.IsPined = !t.IsPined;
                    }
                }
                if (null != driveItem)
                {
                    driveItem.IsPined = !driveItem.IsPined;
                }
                else
                {
                    curItem.IsPined = !curItem.IsPined;
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, $"Could not unpin {pinedItem.Name}");
            }
        }

        private void Star(object obj)
        {
            var item = obj as FileFolderModel;
            try
            {
                StarManager.SetDefault(item.Path);

                var t = RecentItemList.FirstOrDefault(e => !e.Equals(item) && e.Path.Equals(item.Path));
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }
                t = PinnedItemList.FirstOrDefault(e => !e.Equals(item) && e.Path.Equals(item.Path));
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }
                t = FoldersItemList.FirstOrDefault(e => !e.Equals(item) && e.Path.Equals(item.Path));
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }

                t = RecentItemList.FirstOrDefault(e => !e.Equals(item) && e.IsDefault);
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }
                t = PinnedItemList.FirstOrDefault(e => !e.Equals(item) && e.IsDefault);
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }
                t = FoldersItemList.FirstOrDefault(e => !e.Equals(item) && e.IsDefault);
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }

                item.IsDefault = !item.IsDefault;
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, $"Could not star {item.Name}");
            }
        }

        private void Unstar(object obj)
        {
            var item = obj as FileFolderModel;
            try
            {
                StarManager.SetDefault(item.Path);

                var t = RecentItemList.FirstOrDefault(e => !e.Equals(item) && e.Path.Equals(item.Path));
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }
                t = PinnedItemList.FirstOrDefault(e => !e.Equals(item) && e.Path.Equals(item.Path));
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }
                t = FoldersItemList.FirstOrDefault(e => !e.Equals(item) && e.Path.Equals(item.Path));
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }

                item.IsDefault = !item.IsDefault;
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, $"Could not unstar {item.Name}");
            }
        }

        private void OpenItem(object obj)
        {
            var item = obj as FileFolderModel;
            if (null == item)
            {
                return;
            }
            try
            {
                Process.Start(item.Path);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, $"Could not open file or folder {item.Name}");
                //throw ex;
            }
        }

        private void CopyItemPath(object obj)
        {
            var item = obj as FileFolderModel;
            if (null == item)
            {
                return;
            }
            Clipboard.SetText(item.Path);
        }

        private void RenameItem(object obj)
        {
            var item = obj as FileFolderModel;
            if (null == item)
            {
                return;
            }
            CurRenameItem = item;
            CurRenameStatus.IsDoing = true;
            curRenameStatus.Name = item.Name;
            CurSearchStatus.CanSearch = false;
        }

        private void DeleteItem(object obj)
        {
            var item = obj as FileFolderModel;
            if (null == item)
            {
                return;
            }
            var mbr = MessageBox.Show($"Are you sure to delete file or folder {item.Name}?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (mbr != MessageBoxResult.Yes)
            {
                return;
            }
            if (File.Exists(item.Path))
            {
                File.Delete(item.Path);
            }
            else
            {
                Directory.Delete(item.Path);
            }

            var t = RecentItemList.FirstOrDefault(e => e.Path.Equals(item.Path));
            if (null != t)
            {
                RecentManager.Remove(t);
            }
            t = PinnedItemList.FirstOrDefault(e => e.Path.Equals(item.Path));
            if (null != t)
            {
                PinManager.Unpin(t);
            }
            t = FoldersItemList.FirstOrDefault(e => e.Path.Equals(item.Path));
            if (null != t)
            {
                FoldersItemList.Remove(item);
                CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
            }
        }

        private void DoRename(object obj)
        {
            if (CurRenameStatus.IsError)
            {
                return;
            }
            if (CurRenameStatus.Name.Equals(CurRenameItem.Name))
            {
                CancelRename(obj);
                return;
            }
            var dir = Path.GetDirectoryName(CurRenameItem.Path);
            var newPath = Path.Combine(dir, CurRenameStatus.Name);
            if (CurRenameItem.Type == "Dir")
            {
                Directory.Move(CurRenameItem.Path, newPath);
            }
            else
            {
                File.Move(CurRenameItem.Path, newPath);
            }

            var t = RecentItemList.FirstOrDefault(e => e.Path.Equals(CurRenameItem.Path));
            if (null != t)
            {
                RecentManager.Remove(t);
                t.Name = curRenameStatus.Name;
                t.Path = newPath;
                RecentManager.Add(t);
            }
            t = PinnedItemList.FirstOrDefault(e => e.Path.Equals(CurRenameItem.Path));
            if (null != t)
            {
                PinManager.Unpin(t);
                t.Name = curRenameStatus.Name;
                t.Path = newPath;
                PinManager.Pin(t);
            }
            t = FoldersItemList.FirstOrDefault(e => e.Path.Equals(CurRenameItem.Path));
            if (null != t)
            {
                t.Name = curRenameStatus.Name;
                t.Path = newPath;
            }

            CancelRename(obj);
        }

        private void CancelRename(object obj)
        {
            CurRenameItem = null;
            CurRenameStatus.IsDoing = false;
            CurRenameStatus.Name = "";
            CurRenameStatus.IsError = false;
            CurRenameStatus.Desc = "";
            CurSearchStatus.CanSearch = true;
        }

        private void ClearSearch(object obj)
        {
            if (!CurSearchStatus.CanSearch)
            {
                return;
            }
            TxtSearchBoxItem = "";
        }

        private void ClearSearchError(object obj)
        {
            CurSearchStatus.IsError = false;
            CurSearchStatus.Desc = "";
        }

        private void ShowDrives(object obj)
        {
            var str = obj as string;
            switch (str)
            {
                case "MouseEnter":
                    EntereingDrives = true;
                    break;
                case "MouseLeave":
                    EntereingDrives = false;
                    break;
                case "MouseDown":
                    if (!EntereingDrives)
                    {
                        curFilterStatus.ShowDrives = false;
                    }
                    break;
                default:
                    curFilterStatus.ShowDrives = !curFilterStatus.ShowDrives;
                    if (curFilterStatus.ShowDrives)
                    {
                        curFilterStatus.ShowTypes = !curFilterStatus.ShowDrives;
                        curFilterStatus.ShowLocations = !curFilterStatus.ShowDrives;
                    }
                    break;
            }
        }

        private void ShowTypes(object obj)
        {
            curFilterStatus.ShowTypes = !curFilterStatus.ShowTypes;
            if (curFilterStatus.ShowTypes)
            {
                curFilterStatus.ShowDrives = !curFilterStatus.ShowTypes;
                curFilterStatus.ShowLocations = !curFilterStatus.ShowTypes;
            }
        }

        private void ShowLocations(object obj)
        {
            curFilterStatus.ShowLocations = !curFilterStatus.ShowLocations;
            if (curFilterStatus.ShowLocations)
            {
                curFilterStatus.ShowDrives = !curFilterStatus.ShowLocations;
                curFilterStatus.ShowTypes = !curFilterStatus.ShowLocations;
            }
        }

        private void HideAllPopups()
        {
            if (CurFilterStatus.ShowTypes) CurFilterStatus.ShowTypes = !CurFilterStatus.ShowTypes;
            if (CurFilterStatus.ShowDrives) CurFilterStatus.ShowDrives = !CurFilterStatus.ShowDrives;
            if (CurFilterStatus.ShowLocations) CurFilterStatus.ShowLocations = !CurFilterStatus.ShowLocations;
        }

        private void BackNavigationClick(object obj)
        {
            HideAllPopups();
            if (CurSearchStatus.IsDoing)
            {
                return;
            }
            if (CurSearchStatus.IsLoading)
            {
                return;
            }
            CurSearchStatus.Title = "";
            CurSearchStatus.Searched = false;
            curNavigatingFolderPath = "";
            ResetFilter(obj);
            FoldersItemList.Clear();
            CurFolder = nullFolder;
            if (directoryHistory.Count == 0)
            {
                //cannot go back
                CurrentDrivePath = "";
                return;
            }

            //Check if current is root directory or drive
            if (directoryHistory.Count == 1 || IsRootFolder(directoryHistory.Peek()))
            {
                CurrentDrivePath = "";
                IsDriveWindowVisible = Visibility.Visible;
                IsDriveFoldersVisible = Visibility.Collapsed;
                TxtSearchBoxItem = string.Empty;
                IsSearchBoxPlaceholderVisible = Visibility.Visible;
                return;
            }

            //Navigate to previous directory
            directoryHistory.Pop();
            Task.Run(() =>
            {
                NavigateToFolder(directoryHistory.Peek());
            });

            TxtSearchBoxItem = string.Empty;
            IsSearchBoxPlaceholderVisible = Visibility.Visible;
            CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
        }

        private void DoubleDriveClick(object sender)
        {
            try
            {
                CurFolder = nullFolder;
                FoldersItemList.Clear();

                DriveModel item = sender as DriveModel;

                if (item?.DriveName != null && item.DriveName != string.Empty)
                {
                    var defaultFolderPath = StarManager.GetDefault(item.DriveName, out string driveStarFile);
                    if (!string.IsNullOrEmpty(defaultFolderPath))
                    {
                        Task.Run(() =>
                        {
                            NavigateToFolder(defaultFolderPath);
                        });
                        directoryHistory.Push(item.DriveName);
                        directoryHistory.Push(defaultFolderPath);
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            NavigateToFolder(item.DriveName);
                        });
                        directoryHistory.Push(item.DriveName);
                    }

                    CurrentDrivePath = item.DriveName;
                    IsDriveFoldersVisible = Visibility.Visible;
                    IsDriveWindowVisible = Visibility.Collapsed;
                }

            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex);
                //throw ex;
            }

            //TxtSearchBoxItem = string.Empty;
            //IsSearchBoxPlaceholderVisible = Visibility.Visible;
            //CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
        }

        private void DeepSearch(string folderPath, string keyword, CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
            {
                return;
            }
            // Check if the folder exists
            if (!Directory.Exists(folderPath))
            {
                return;
            }

            try
            {
                if (ct.IsCancellationRequested)
                {
                    return;
                }
                //Get subfolders
                string[] subDirectories = Directory.GetDirectories(folderPath);
                foreach (string subFolder in subDirectories)
                {
                    if (ct.IsCancellationRequested)
                    {
                        return;
                    }
                    DirectoryInfo subFolderInfo = new DirectoryInfo(subFolder);
                    if (!subFolderInfo.Attributes.ToString().Contains(FileAttributes.Hidden.ToString()) &&
               !subFolderInfo.Attributes.ToString().Contains(FileAttributes.NotContentIndexed.ToString()) &&
               !subFolderInfo.Attributes.ToString().Contains(FileAttributes.ReparsePoint.ToString()))
                    {
                        if (DirectoryHelper.GetDirectoryName(subFolder).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                FoldersItemList.Add(new FileFolderModel
                                {
                                    Path = subFolderInfo.FullName,
                                    Name = subFolderInfo.Name,
                                    LastModifiedDateTime = subFolderInfo.LastWriteTime,
                                    IconSource = IconHelper.GetIcon(subFolderInfo.FullName),
                                    Type = "Dir",
                                    IsDefault = StarManager.IsDefault(subFolder),
                                    IsPined = PinManager.IsPined(subFolder)
                                });
                            });
                        }
                        DeepSearch(subFolder, keyword, ct);
                    }
                }
            }
            catch (Exception ex) { }

            try
            {
                if (ct.IsCancellationRequested)
                {
                    return;
                }
                //Get files
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    if (ct.IsCancellationRequested)
                    {
                        return;
                    }
                    if (Path.GetFileName(file).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        continue;
                    }

                    FileInfo fileInfo = new FileInfo(file);
                    if (!fileInfo.Attributes.ToString().Contains(FileAttributes.Hidden.ToString()) &&
                !fileInfo.Attributes.ToString().Contains(FileAttributes.NotContentIndexed.ToString()) &&
                !fileInfo.Attributes.ToString().Contains(FileAttributes.ReparsePoint.ToString()))
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            FoldersItemList.Add(new FileFolderModel()
                            {
                                Path = fileInfo.FullName,
                                Name = fileInfo.Name,
                                LastModifiedDateTime = fileInfo.LastWriteTime,
                                IconSource = IconHelper.GetIcon(fileInfo.FullName),
                                Type = "File",
                                IsDefault = StarManager.IsDefault(file),
                                IsPined = PinManager.IsPined(file)
                            });
                        });
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void DoSearch(object sender)
        {
            if (!CurSearchStatus.CanSearch)
            {
                return;
            }

            CurSearchStatus.IsDoing = true;
            CurSearchStatus.IsError = false;
            CurSearchStatus.Searched = false;
            CurSearchStatus.Desc = "";
            if (!string.IsNullOrEmpty(TxtSearchBoxItem))
            {
                IsSearchBoxPlaceholderVisible = Visibility.Collapsed;
                if (TxtSearchBoxItem.Length > 256)
                {
                    CurSearchStatus.IsError = true;
                    CurSearchStatus.Desc = "Search items have a maximum limit of 256 characters.";
                    return;
                }
                //if (FoldersItemList.Count > 0)
                if (!string.IsNullOrEmpty(curNavigatingFolderPath))
                {
                    CurFolder = nullFolder;
                    FoldersItemList.Clear();
                    CurSearchStatus.IsLoadingItems = true;

                    ctsSearch?.Cancel();
                    ctsSearch = null;
                    ctsSearch = new CancellationTokenSource();

                    Task.Run(() =>
                    {
                        List<string> dests = new List<string>();
                        if (SelectedLocation.Code == "CurDir")
                        {
                            dests.Add(curNavigatingFolderPath);
                        }
                        else if (SelectedLocation.Code == "CurDrive")
                        {
                            dests.Add(CurrentDrivePath);
                        }
                        else
                        {
                            foreach (var drive in DriveList)
                            {
                                dests.Add(drive.DriveName);
                            }
                        }
                        foreach (var dest in dests)
                        {
                            DeepSearch(dest, TxtSearchBoxItem, ctsSearch.Token);
                        }
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
                            CurFilterStatus.ItemsCount = FoldersItemList.Count;
                            CurSearchStatus.IsLoadingItems = false;
                            CurSearchStatus.IsDoing = false;
                            CurSearchStatus.Searched = true;
                        });
                    });
                }
                else
                {
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(RecentItemList);
                    view.Filter = searchItemFilter;
                    CurFilterStatus.RecentCount = view.Count;
                    view = (CollectionView)CollectionViewSource.GetDefaultView(PinnedItemList);
                    view.Filter = searchItemFilter;
                    CurFilterStatus.PinnedCount = view.Count;
                    CurSearchStatus.IsDoing = false;
                }
            }
            else
            {
                IsSearchBoxPlaceholderVisible = Visibility.Visible;
                //if (FoldersItemList.Count > 0)
                //{
                //    CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
                //}
                if (!string.IsNullOrEmpty(curNavigatingFolderPath))
                {
                    Task.Run(() =>
                    {
                        NavigateToFolder(curNavigatingFolderPath);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ResetFilter(sender);
                            CurSearchStatus.IsDoing = false;
                        });
                    });
                }
                else
                {
                    CollectionViewSource.GetDefaultView(RecentItemList).Refresh();
                    CurFilterStatus.RecentCount = RecentItemList.Count;
                    CollectionViewSource.GetDefaultView(PinnedItemList).Refresh();
                    CurFilterStatus.PinnedCount = PinnedItemList.Count;
                    ResetFilter(sender);
                    CurSearchStatus.IsDoing = false;
                }
            }
        }

        public void CancelSearch(object sender)
        {
            ctsSearch?.Cancel();
        }

        private void searchBoxTextChanged(object sender)
        {
            CurSearchStatus.IsError = false;
            CurSearchStatus.Desc = "";
            if (!string.IsNullOrEmpty(TxtSearchBoxItem) && TxtSearchBoxItem.Length > 256)
            {
                CurSearchStatus.IsError = true;
                CurSearchStatus.Desc = "Search items have a maximum limit of 256 characters.";
                return;
            }
            /*
            CurSearchStatus.IsDoing = false;
            if (!string.IsNullOrEmpty(TxtSearchBoxItem))
            {
                IsSearchBoxPlaceholderVisible = Visibility.Collapsed;
                if (FoldersItemList.Count > 0)
                {
                    CurFolder = nullFolder;
                    FoldersItemList.Clear();
                    DeepSearch(curNavigatingFolderPath, TxtSearchBoxItem);
                    CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
                    CurFilterStatus.ItemsCount = FoldersItemList.Count;
                }
                else
                {
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(RecentItemList);
                    view.Filter = searchItemFilter; 
                    view = (CollectionView)CollectionViewSource.GetDefaultView(PinnedItemList);
                    view.Filter = searchItemFilter;
                }
                CurSearchStatus.IsDoing = true;
            }
            else
            {
                IsSearchBoxPlaceholderVisible = Visibility.Visible;
                if (FoldersItemList.Count > 0)
                {
                    CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
                }
                else
                {
                    CollectionViewSource.GetDefaultView(RecentItemList).Refresh();
                    CollectionViewSource.GetDefaultView(PinnedItemList).Refresh();
                }
                ResetFilter(sender);
            }
            */
        }

        private void RenameTextChanged(object sender)
        {
            CurRenameStatus.IsError = false;
            CurRenameStatus.Desc = "";
            if (string.IsNullOrEmpty(CurRenameStatus.Name))
            {
                CurRenameStatus.IsError = true;
                CurRenameStatus.Desc = "Please type the name.";
                return;
            }
            foreach (var ch in forbiddenChars)
            {
                if (CurRenameStatus.Name.Contains(ch))
                {
                    CurRenameStatus.IsError = true;
                    CurRenameStatus.Desc = $"Name couldn't contain any of the following chars {forbiddenChars}";
                    return;
                }
            }
            if (CurRenameStatus.Name.Length > 256)
            {
                CurRenameStatus.IsError = true;
                CurRenameStatus.Desc = "File name or folder name has a maximum limit of 256 characters.";
                return;
            }
        }

        public void FolderItemDoubleClick(object sender)
        {
            if (SelectedFileFolderItem != null && !string.IsNullOrEmpty(SelectedFileFolderItem.Path))
            {
                string path = SelectedFileFolderItem.Path;

                // Check if the path exist and path is folder
                if (Directory.Exists(path))
                {
                    directoryHistory.Push(path);
                    Task.Run(() =>
                    {
                        NavigateToFolder(path);
                    });

                    //TxtSearchBoxItem = string.Empty;
                    //IsSearchBoxPlaceholderVisible = Visibility.Visible;
                    //CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
                }
                // Check if the path exist and path is file
                else if (File.Exists(path))
                {
                    try
                    {
                        Process.Start(path);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHelper.ShowErrorMessage(ex, $"Could not open file {SelectedFileFolderItem.Name}");
                        //throw ex;
                    }
                }
            }
        }

        public void RecentItemDoubleClick(object sender)
        {
            if (SelectedRecentItem != null && !string.IsNullOrEmpty(SelectedRecentItem.Path))
            {
                try
                {
                    Process.Start(SelectedRecentItem.Path);
                }
                catch (Exception ex)
                {
                    ExceptionHelper.ShowErrorMessage(ex, $"Could not open file or folder {SelectedRecentItem.Name}");
                    //throw ex;
                }
            }
        }

        public void PinnedItemDoubleClick(object sender)
        {
            var pinedItem = sender as FileFolderModel;
            if (pinedItem != null && !string.IsNullOrEmpty(pinedItem.Path))
            {
                try
                {
                    Process.Start(pinedItem.Path);
                }
                catch (Exception ex)
                {
                    ExceptionHelper.ShowErrorMessage(ex, $"Could not open file or folder {pinedItem.Name}");
                    //throw ex;
                }
            }
        }

        public void NavigateToFolder(string folderPath)
        {
            var isRoot = IsRootFolder(folderPath);
            Application.Current.Dispatcher.Invoke(() =>
            {
                HideAllPopups();
                CurSearchStatus.IsLoading = true;
                CurSearchStatus.IsLoadingItems = true;
                CurSearchStatus.Title = folderPath;
                CurSearchStatus.Searched = false;
                curNavigatingFolderPath = folderPath;
                ResetFilter(null);
                FoldersItemList.Clear();
                TxtSearchBoxItem = "";
                Locations = isRoot ? FilterConditionModel.DirveLocations : FilterConditionModel.DirectoryLocations;
                SelectedLocation = Locations.Last();
            });

            if (!string.IsNullOrEmpty(folderPath))
            {
                // Check if the folder exists
                if (Directory.Exists(folderPath))
                {
                    if (!isRoot)
                    {
                        var curDir = new DirectoryInfo(folderPath);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            CurFolder = new FileFolderModel
                            {
                                Path = curDir.FullName,
                                Name = curDir.Name,
                                LastModifiedDateTime = curDir.LastWriteTime,
                                IconSource = IconHelper.GetIcon(curDir.FullName),
                                Type = "Dir",
                                IsPined = PinManager.IsPined(folderPath),
                                IsDefault = StarManager.IsDefault(folderPath)
                            };
                        });
                    }

                    try
                    {
                        //Get subfolders
                        string[] subDirectories = Directory.GetDirectories(folderPath);
                        foreach (string subFolder in subDirectories)
                        {
                            DirectoryInfo subFolderInfo = new DirectoryInfo(subFolder);
                            if (!subFolderInfo.Attributes.ToString().Contains(FileAttributes.Hidden.ToString()) &&
                       !subFolderInfo.Attributes.ToString().Contains(FileAttributes.NotContentIndexed.ToString()) &&
                       !subFolderInfo.Attributes.ToString().Contains(FileAttributes.ReparsePoint.ToString()))
                            {

                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    FoldersItemList.Add(new FileFolderModel
                                    {
                                        Path = subFolderInfo.FullName,
                                        Name = subFolderInfo.Name,
                                        LastModifiedDateTime = subFolderInfo.LastWriteTime,
                                        IconSource = IconHelper.GetIcon(subFolderInfo.FullName),
                                        Type = "Dir",
                                        IsDefault = StarManager.IsDefault(subFolder),
                                        IsPined = PinManager.IsPined(subFolder)
                                    });
                                });
                            }
                        }
                    }
                    catch (Exception ex) { }

                    try
                    {
                        //Get files
                        string[] files = Directory.GetFiles(folderPath);
                        foreach (string file in files)
                        {
                            FileInfo fileInfo = new FileInfo(file);

                            if (!fileInfo.Attributes.ToString().Contains(FileAttributes.Hidden.ToString()) &&
                        !fileInfo.Attributes.ToString().Contains(FileAttributes.NotContentIndexed.ToString()) &&
                        !fileInfo.Attributes.ToString().Contains(FileAttributes.ReparsePoint.ToString()))
                            {
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    FoldersItemList?.Add(new FileFolderModel()
                                    {
                                        Path = fileInfo.FullName,
                                        Name = fileInfo.Name,
                                        LastModifiedDateTime = fileInfo.LastWriteTime,
                                        IconSource = IconHelper.GetIcon(fileInfo.FullName),
                                        Type = "File",
                                        IsDefault = StarManager.IsDefault(file),
                                        IsPined = PinManager.IsPined(file)
                                    });
                                });
                            }
                        }
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    // ExceptionHelper.ShowErrorMessage("The folder does not exist.");
                }
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                CurSearchStatus.IsLoading = false;
                CurSearchStatus.IsLoadingItems = false;
                TxtSearchBoxItem = string.Empty;
                IsSearchBoxPlaceholderVisible = Visibility.Visible;
                CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
                CurFilterStatus.ItemsCount = FoldersItemList.Count;
            });
        }

        private bool IsRootFolder(string path)
        {
            if (path.Length == 3 || path == CurrentDrivePath)
            {
                return true;
            }
            return false;
        }

        //search functionlaity
        private void searchBoxLostFocus(object obj)
        {
            if (!string.IsNullOrEmpty(TxtSearchBoxItem))
            {
                return;
            }
            else
            {
                IsSearchBoxPlaceholderVisible = Visibility.Visible;

                CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
            }
        }

        private void searchBoxGotFocus(object obj)
        {
            IsSearchBoxPlaceholderVisible = Visibility.Collapsed;
        }

        private bool searchItemFilter(object item)
        {
            if (string.IsNullOrEmpty(TxtSearchBoxItem) || TxtSearchBoxItem == "Search")
                return true;
            else
                return (item as FileFolderModel).Name.IndexOf(TxtSearchBoxItem, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void DoFilter(object obj)
        {
            var SelectedDrives = Drives.Where(item => item.IsSelected).ToList();
            if ((null != SelectedDrives && SelectedDrives.Count > 0) || (null != SelectedType && !string.IsNullOrEmpty(SelectedType.Code)))
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FoldersItemList);
                view.Filter = filter;
                CurFilterStatus.ItemsCount = view.Count;
            }
            else
            {
                CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
                CurFilterStatus.ItemsCount = FoldersItemList.Count;
            }
        }

        private bool filter(object item)
        {
            var SelectedDrives = Drives.Where(ele => ele.IsSelected).ToList();
            if ((null == SelectedDrives || SelectedDrives.Count == 0) && (null == SelectedType || string.IsNullOrEmpty(SelectedType.Code)))
            {
                return true;
            }
            else
            {
                var m = item as FileFolderModel;
                var result = true;
                if (null != SelectedDrives && SelectedDrives.Count > 0)
                {
                    var drives = SelectedDrives.Where(element => !string.IsNullOrEmpty(element.Code)).Select(element => element.Code).ToList();
                    result = result && drives.Contains(m.Path.Substring(0, 3));
                }
                if (null != SelectedType && !string.IsNullOrEmpty(SelectedType.Code))
                {
                    result = result && m.Type.Equals(SelectedType.Code);
                }
                return result;
            }
        }

        private void ResetFilter(object obj)
        {
            curFilterStatus.ShowDrives = false;
            curFilterStatus.ShowTypes = false;
            foreach (var drive in Drives)
            {
                drive.IsSelected = false;
            }
            SelectedType = null;
            DoFilter(obj);
        }

        // File icon set
        //private string SetIcon(string extension)
        //{
        //    string folderFileSource = ""; // Initialize the variable to hold the icon path

        //    switch (extension.ToLower())
        //    {
        //        case ".pdf":
        //            folderFileSource = "Resources/icnpdf.png";
        //            break;
        //        case ".jpg":
        //        case ".jpeg":
        //            folderFileSource = "Resources/icnjpg.png";
        //            break;
        //        case ".png":
        //            folderFileSource = "Resources/icnpng.png";
        //            break;
        //        case ".mkv":
        //            folderFileSource = "Resources/icnmkv.png";
        //            break;
        //        case ".xaml":
        //            folderFileSource = "Resources/icnxml.png";
        //            break;
        //        case ".svg":
        //            folderFileSource = "Resources/icnsvg.png";
        //            break;
        //        case ".txt":
        //            folderFileSource = "Resources/icntxt.png";
        //            break;
        //        case ".sql":
        //            folderFileSource = "Resources/icnsql.png";
        //            break;
        //        case ".zip":
        //            folderFileSource = "Resources/icnzip.png";
        //            break;
        //        case ".sln":
        //            folderFileSource = "Resources/icnsln.png";
        //            break;
        //        case ".exe":
        //            folderFileSource = "Resources/icnexe.png";
        //            break;
        //        default:
        //            folderFileSource = "Resources/icnfolder.png";
        //            break;
        //    }
        //    return folderFileSource;
        //}

        // search list filter
        //private bool folderListFilter(object item)
        //{
        //    if (String.IsNullOrEmpty(TxtSearchBoxItem))
        //        return true;
        //    else
        //        return (item as FolderModel).FolderName.StartsWith(TxtSearchBoxItem, StringComparison.OrdinalIgnoreCase);
        //}

        //private void RecentItemSort(object obj)
        //{
        //    IsAscending = !IsAscending;
        //}

        #endregion

        #region :: Events ::

        private void CDMViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(SelectedType)))
            {
                //if (CurFilterStatus.ShowTypes) CurFilterStatus.ShowTypes = !CurFilterStatus.ShowTypes;
                CurFilterStatus.CurType = string.IsNullOrEmpty(SelectedType?.Code) ? "" : SelectedType?.Name;
                DoFilter(sender);
                return;
            }
            if (e.PropertyName.Equals(nameof(SelectedLocation)))
            {
                //if (CurFilterStatus.ShowLocations) CurFilterStatus.ShowLocations = !CurFilterStatus.ShowLocations;
                return;
            }
            if (e.PropertyName.Equals(nameof(CurDrivesPagesIndex)))
            {
                IsFirstDrivesPage = CurDrivesPagesIndex == 1;
                IsLastDrivesPage = CurDrivesPagesIndex == DrivesPagesCount;
                var list = DriveList.Skip((CurDrivesPagesIndex - 1) * DrivesPageSize).Take(DrivesPageSize);
                PagedDrivesList.Clear();
                foreach (var item in list)
                {
                    PagedDrivesList.Add(item);
                }
                return;
            }
        }

        private void DriveManager_DriveIsSelectedChanged(object sender, EventArgs e)
        {
            var SelectedDrives = Drives.Where(item => item.IsSelected && !string.IsNullOrEmpty(item.Code)).ToList();
            CurFilterStatus.CurDrives = SelectedDrives.Count <= 1 ? SelectedDrives.FirstOrDefault()?.Name : $"{SelectedDrives.First().Name} +{(SelectedDrives.Count - 1)}";
            DoFilter(sender);
        }

        private void FilterStatus_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(CurFilterStatus.DrivesCount)))
            {
                DrivesPagesCount = CurFilterStatus.DrivesCount % DrivesPageSize == 0 ? (CurFilterStatus.DrivesCount / DrivesPageSize) : (CurFilterStatus.DrivesCount / DrivesPageSize + 1);
                return;
            }
        }

        private void SearchStatus_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(CurSearchStatus.IsDoing)))
            {
                CurSearchStatus.CanSearch = !CurSearchStatus.IsDoing;
            }
        }


        #endregion
    }
}