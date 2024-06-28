﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using CDM.Common;
using CDM.Helper;
using CDM.Models;
using CDM.ViewModels;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;

namespace CDM.UserControls
{
    /// <summary>
    /// Interaction logic for CDMUserControl.xaml
    /// </summary>
    public partial class CDMUserControl : UserControl
    {
        #region :: Variables ::
        public delegate void dlgtTest();
        public event dlgtTest EventTest;
        private CDMViewModel vm = null;
        private CancellationTokenSource cts = null;
        Dispatcher _sysDispatcher;
        private Point _startPoint;
        private bool _isDragging = false;
        double PWidth = 0;
        double PHeight = 0;

        #endregion
        #region :: Constructor ::
        public CDMUserControl(Dispatcher sysDispatcher, double width = 0, double height = 0)
        {
            _sysDispatcher = sysDispatcher;
            PWidth = width;
            PHeight = height;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            InitializeComponent();
            SetInitialTheme();
        }

        public void CDMUserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (vm != null)
            {
                vm.ParentHeight = e.NewSize.Height;
                vm.ParentWidth = e.NewSize.Width;
                UpdateDrivePagination();
            }
        }
        #endregion
        #region :: Events ::
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception);
            e.Handled = true;
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject as Exception;
            HandleException(exception);
        }
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            HandleException(e.Exception);
            e.SetObserved();
        }
        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General)
            {
                // System theme might have changed, recheck and update UI
                bool isDarkTheme = IsSystemInDarkMode();
                //UpdateUIForTheme(isDarkTheme);
            }
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new CDMViewModel(_sysDispatcher);
            vm.ParentHeight = PHeight;
            vm.ParentWidth = PWidth;
            vm.EventHighlightSearchedText += Vm_EventHighlightSearchedText;
            this.DataContext = vm;
            await Task.Delay(100);
        }

        public void LoadUI()
        {
            try
            {
                // Subscribe to system theme changes
                Microsoft.Win32.SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;

                var drivesPageSize = ((int)Math.Floor((this.ActualWidth - 35D) / 464D) * 2);
                vm.DrivesPageSize = drivesPageSize == 0 ? 1 : drivesPageSize;

                vm.Init();
                DriveManager.DrivesStateChanged += DriveManager_DrivesStateChanged;
                cts = new CancellationTokenSource();
                _ = DriveManager.Check(cts.Token);
                vm.SortByName(true);
            }
            catch
            {
            }
        }

        private void UpdateDrivePagination()
        {
            try
            {
                if (vm == null)
                {
                    return;
                }
                var drivesPageSize = ((int)Math.Floor((this.ActualWidth - 35D) / 464D) * 2);
                vm.DrivesPageSize = drivesPageSize == 0 ? 1 : drivesPageSize;
                vm.UpdateDrivePageCount();
                vm.CurDrivesPagesIndex = 0;
            }
            catch
            {

            }
        }

        private void DriveManager_DrivesStateChanged(object sender, bool e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (vm.Offline == e)
                {
                    vm.Offline = !e;
                    if (vm.Offline)
                    {
                        vm.CancelSearch(this);
                    }
                }
            });
        }
        private void UserControl_DragEnter(object sender, DragEventArgs e)
        {
            var allowDragAndDrop = ConfigurationManager.AppSettings["AllowDragAndDrop"] == "true";
            if (!allowDragAndDrop) return;

            e.Handled = true;
            if (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent("Shell IDList Array"))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (_isDragging)
                {
                    return;
                }
                string destinationPath = vm.CurFolder.Path == null ? vm.CurrentDrivePath : vm.CurFolder.Path;
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (!string.IsNullOrEmpty(destinationPath))
                    {
                        bool isDirectory = false;
                        string fileName = "";
                        foreach (string file in files)
                        {
                            isDirectory = !System.IO.Path.HasExtension(file);
                            fileName = System.IO.Path.GetFileName(file);
                            if (isDirectory)
                            {
                                FileSystem.CopyDirectory(file, destinationPath + "\\" + fileName, UIOption.AllDialogs, UICancelOption.DoNothing);
                            }
                            else
                            {
                                FileSystem.CopyFile(file, destinationPath + "\\" + fileName, UIOption.AllDialogs, UICancelOption.DoNothing);
                            }
                        }
                    }
                }
                else if (e.Data.GetDataPresent("Shell IDList Array"))
                {
                    var shellObjects = (System.Runtime.InteropServices.ComTypes.IDataObject)e.Data.GetData("Shell IDList Array");
                    ProcessShellObjects(shellObjects);
                }

                _sysDispatcher.Invoke(() =>
                {
                    vm.NavigateToFolder(destinationPath);
                });
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }
        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!vm.EntereingDrives && vm.CurFilterStatus.ShowDrives) vm.CurFilterStatus.ShowDrives = false;
            if (vm.CurFilterStatus.ShowTypes) vm.CurFilterStatus.ShowTypes = false;
            if (vm.CurFilterStatus.ShowLocations) vm.CurFilterStatus.ShowLocations = false;
        }
        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (vm.CurFilterStatus.ShowDrives) vm.CurFilterStatus.ShowDrives = false;
            if (vm.CurFilterStatus.ShowTypes) vm.CurFilterStatus.ShowTypes = false;
            if (vm.CurFilterStatus.ShowLocations) vm.CurFilterStatus.ShowLocations = false;
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            cts?.Cancel();
        }
        #endregion
        #region :: Methods ::
        //private void RefreshList(List<currentLocation>)
        //{
        //    CollectionViewSource.GetDefaultView(RecentItemList).Refresh();
        //}
        private void ProcessShellObjects(System.Runtime.InteropServices.ComTypes.IDataObject shellObjects)
        {
            // Implementation for processing shell objects
            // This example only demonstrates getting the shell objects and their display names
            if (shellObjects == null)
                return;

            var dataFormat = DataFormats.GetDataFormat("Shell IDList Array");
            //if (shellObjects.GetDataPresent(dataFormat.Name))
            //{
            //    var obj = shellObjects.GetData(dataFormat.Name);
            //    // Handle the shell objects (e.g., display their names in the TextBox)
            //    // This part will require additional processing to extract useful information
            //}
        }
        private void HandleException(Exception ex)
        {
            ExceptionHelper.ShowErrorMessage(ex);
        }
        private void SetInitialTheme()
        {
            // Check the system theme initially
            bool isDarkTheme = IsSystemInDarkMode();
            //UpdateUIForTheme(isDarkTheme);
        }
        private bool IsSystemInDarkMode()
        {
            try
            {
                const string RegistryKeyPath = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
                const string RegistryValueName = "AppsUseLightTheme";

                //int res = (int)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", -1);

                int lightThemeValue = (int)Registry.GetValue(RegistryKeyPath, RegistryValueName, defaultValue: 1);
                // 0 - dark
                // 1 - Light
                if (lightThemeValue == 0)
                {
                    var darkStyle = new Uri("pack://application:,,,/CDM;component/Themes/DarkStyle.xaml", UriKind.RelativeOrAbsolute);
                    bool isUpdated = ResourceDictionaryManager.UpdateDictionary("Themes", darkStyle, this);
                    //ResourceDictionaryManager.UpdateResourceColor("NavibarBackgroundColor", (SolidColorBrush)(App.Current.Resources["NavibarBackgroundDarkColor"]));
                }
                else
                {
                    var lightStyle = new Uri("pack://application:,,,/CDM;component/Themes/LightStyle.xaml", UriKind.RelativeOrAbsolute);
                    bool isUpdated = ResourceDictionaryManager.UpdateDictionary("Themes", lightStyle, this);
                    // ResourceDictionaryManager.UpdateResourceColor("NavibarBackgroundColor", (SolidColorBrush)(App.Current.Resources["NavibarBackgroundLightColor"]));
                }

                return lightThemeValue == 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        private void ListOfDriveItems_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true; // Prevent the default sort from occurring

            if (e.Column.SortMemberPath == "Name")
            {
                vm.SortByName();
            }
            else if (e.Column.SortMemberPath == "DateModified")
            {
                vm.SortByDateModified();
            }
        }

        //private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        //{

        //    if (!string.IsNullOrEmpty(vm.TxtSearchBoxItem))
        //    {
        //        vm.IsSearchBoxPlaceholderVisible = Visibility.Collapsed;
        //    }

        //    if (!string.IsNullOrEmpty(vm.TxtSearchBoxItem) && vm.TxtSearchBoxItem.Length > 256)
        //    {
        //        vm.TxtSearchBoxItem = vm.TxtSearchBoxItem.Substring(0, 256);
        //        vm.CurSearchStatus.IsError = true;
        //        vm.CurSearchStatus.Desc = "Search items have a maximum limit of 256 characters.";
        //        return;
        //    }
        //    else if (string.IsNullOrEmpty(vm.TxtSearchBoxItem) || (vm.TxtSearchBoxItem.Length < 256))
        //    {
        //        vm.CurSearchStatus.IsError = false;
        //        vm.CurSearchStatus.Desc = "";
        //    }

        //    var tmp = vm.CurrentDrivePath;
        //    DataGrid dataGrid;
        //    if (tmp == null && TabView1.SelectedIndex == 0)
        //    {
        //        dataGrid = ListOfRecentItems;
        //    }
        //    else if (tmp == null && TabView1.SelectedIndex == 1)
        //    {
        //        dataGrid = ListOfPinnedItems;
        //    }
        //    else
        //    {
        //        dataGrid = ListOfDriveItems;
        //    }
        //    this.Dispatcher.Invoke(() =>
        //{
        //    Regex regex = null;

        //    if (!string.IsNullOrEmpty(vm.TxtSearchBoxItem))
        //    {
        //        regex = new Regex($"({Regex.Escape(vm.TxtSearchBoxItem)})", RegexOptions.IgnoreCase);
        //    }
        //    FindDataGridItem(dataGrid, regex);
        //});

        //}

        public void FindDataGridItem(DependencyObject obj, Regex regex)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                if (child is DataGridRow dataGridRow)
                {
                    HighlightText(dataGridRow, regex);
                }
                else
                {
                    FindDataGridItem(child, regex);
                }
            }
        }

        private void HighlightText(DependencyObject obj, Regex regex)
        {
            if (obj is TextBlock textBlock)
            {
                if (!(obj as TextBlock).Text.Contains(":"))
                {
                    string originalText = textBlock.Text;
                    textBlock.Inlines.Clear();

                    if (regex == null)
                    {
                        textBlock.Inlines.Add(new Run(originalText));
                        return;
                    }

                    string[] substrings = regex.Split(originalText);
                    foreach (var substring in substrings)
                    {
                        if (regex.IsMatch(substring))
                        {
                            Run highlightRun = new Run(substring)
                            {
                                FontWeight = FontWeights.Bold
                            };
                            textBlock.Inlines.Add(highlightRun);
                        }
                        else
                        {
                            textBlock.Inlines.Add(new Run(substring));
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    HighlightText(VisualTreeHelper.GetChild(obj, i), regex);
                }
            }
        }

        private void txtSearchBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            HighlightSearchedText();
        }

        private void HighlightSearchedText()
        {
            ListOfDriveItems.UpdateLayout();
            //Highlight searched text
            var tmp = vm.CurrentDrivePath;
            DataGrid dataGrid;
            if (string.IsNullOrEmpty(tmp) && TabView1.SelectedIndex == 0)
            {
                ListOfRecentItems.UpdateLayout();
                dataGrid = ListOfRecentItems;
            }
            else if (string.IsNullOrEmpty(tmp) && TabView1.SelectedIndex == 1)
            {
                ListOfPinnedItems.UpdateLayout();
                dataGrid = ListOfPinnedItems;
            }
            else
            {
                ListOfDriveItems.UpdateLayout();
                dataGrid = ListOfDriveItems;
            }
            this.Dispatcher.Invoke(() =>
            {
                Regex regex = null;

                if (!string.IsNullOrEmpty(vm.TxtSearchBoxItem))
                {
                    regex = new Regex($"({Regex.Escape(vm.TxtSearchBoxItem)})", RegexOptions.IgnoreCase);
                }

                if (!dataGrid.IsLoaded)
                {
                    return;
                }

                FindDataGridItem(dataGrid, regex);

            });
        }


        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {

            if (!string.IsNullOrEmpty(vm.TxtSearchBoxItem))
            {
                vm.IsSearchBoxPlaceholderVisible = Visibility.Collapsed;
            }

            if (!string.IsNullOrEmpty(vm.TxtSearchBoxItem) && vm.TxtSearchBoxItem.Length > 256)
            {
                vm.TxtSearchBoxItem = vm.TxtSearchBoxItem.Substring(0, 256);
                txtSearchBox.CaretIndex = vm.TxtSearchBoxItem.Length;   //set cursor to end
                vm.CurSearchStatus.IsError = true;
                vm.CurSearchStatus.Desc = "Search items have a maximum limit of 256 characters.";
                return;
            }
            else if (string.IsNullOrEmpty(vm.TxtSearchBoxItem) || (vm.TxtSearchBoxItem.Length < 256))
            {
                vm.CurSearchStatus.IsError = false;
                vm.CurSearchStatus.Desc = "";
                //isHighlightAllow = true;
            }

            if (string.IsNullOrEmpty(vm.CurrentDrivePath) && TabView1.SelectedIndex == 0)
            {
                vm.searchRecentItemList();
            }
            else if (string.IsNullOrEmpty(vm.CurrentDrivePath) && TabView1.SelectedIndex == 1)
            {
                vm.searchPinnedItemList();
            }
            else
            {
                vm.searchFileFolderItemList();
            }

        }

        private void DataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !_isDragging) // Check if not already dragging
            {
                Point mousePos = e.GetPosition(null);
                Vector diff = _startPoint - mousePos;

                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    // Get the data row under the mouse
                    DataGridRow dataGridRow = FindAncestor<DataGridRow>((DependencyObject)e.OriginalSource);

                    if (dataGridRow != null)
                    {
                        // Get the data from the DataGrid row
                        FileFolderModel data = dataGridRow.Item as FileFolderModel;
                        string filePath = data?.Path;

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            // Check if the path exists
                            if (System.IO.File.Exists(filePath) || System.IO.Directory.Exists(filePath))
                            {
                                try
                                {
                                    _isDragging = true; // Set the flag to indicate a drag operation is in progress

                                    // Create a DataObject containing the file or folder path
                                    DataObject dataObject = new DataObject(DataFormats.FileDrop, new string[] { filePath });
                                    DragDrop.DoDragDrop(dataGridRow, dataObject, DragDropEffects.Copy);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An error occurred during the drag-and-drop operation: " + ex.Message);
                                }
                                finally
                                {
                                    _isDragging = false; // Reset the flag after the drag operation
                                }
                            }
                            else
                            {
                                MessageBox.Show("The path does not exist: " + filePath);
                            }
                        }
                    }
                }
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null && !(current is T))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return current as T;
        }

        private void DataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Store the mouse position when the left button is pressed
            _startPoint = e.GetPosition(null);
        }

        private void Vm_EventHighlightSearchedText()
        {
            if (string.IsNullOrEmpty(vm.TxtSearchBoxItem))
            {
                return;
            }
            HighlightSearchedText();
        }

        private void ListOfPinnedItems_Sorting(object sender, DataGridSortingEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (string.IsNullOrEmpty(vm.TxtSearchBoxItem))
                {
                    return;
                }
                HighlightSearchedText();
            }));
        }

        private void ListOfRecentItems_Sorting(object sender, DataGridSortingEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (string.IsNullOrEmpty(vm.TxtSearchBoxItem))
                {
                    return;
                }
                HighlightSearchedText();
            }));
        }

        private void DataGridRow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {

                var row = sender as DataGridRow;
                if (row != null && row.IsSelected)
                {
                    vm.FolderItemSingleClick(null);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var row = sender as DataGridRow;
                if (row != null && row.IsSelected)
                {
                    vm.FolderItemDoubleClick(null);
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void TabView1_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void TabView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (vm == null)
                {
                    return;
                }
                if (PinnedTabItem != null && PinnedTabItem.IsSelected && vm.PinnedItemList != null)
                {
                    CollectionViewSource.GetDefaultView(vm.PinnedItemList).Refresh();
                    vm.CurFilterStatus.PinnedCountWithoutDrive = vm.PinnedItemList.Count;

                }
                else if (RecentTabItem != null && RecentTabItem.IsSelected && vm.RecentItemList != null)
                {
                    CollectionViewSource.GetDefaultView(vm.RecentItemList).Refresh();
                    vm.CurFilterStatus.RecentCount = vm.RecentItemList.Count;

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}