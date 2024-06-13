using System;
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
        #endregion
        #region :: Constructor ::
        public CDMUserControl()
        {
            //AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            //Application.Current.DispatcherUnhandledException += Application_DispatcherUnhandledException;
            InitializeComponent();
            vm = new CDMViewModel(this);
            this.DataContext = vm;
            SetInitialTheme();

            // Subscribe to system theme changes
            Microsoft.Win32.SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
            //IsSystemInDarkMode();
        }
        public CDMUserControl(int width, int height)
        {
            //AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            //Application.Current.DispatcherUnhandledException += Application_DispatcherUnhandledException;
            this.Width = width;
            this.Height = height;
            InitializeComponent();
            vm = new CDMViewModel(this);
            this.DataContext = vm;
            SetInitialTheme();

            // Subscribe to system theme changes
            Microsoft.Win32.SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
            //IsSystemInDarkMode();
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
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var drivesPageSize = (Convert.ToInt32(this.ActualWidth - 20D) / 472) * (Convert.ToInt32((this.ActualHeight - 130D) * 0.4D) / 104);
            vm.DrivesPageSize = drivesPageSize == 0 ? 1 : drivesPageSize;
            vm.Init();

            DriveManager.DrivesStateChanged += DriveManager_DrivesStateChanged;
            cts = new CancellationTokenSource();
            _ = DriveManager.Check(cts.Token);
            vm.SortByName(true);
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
            //var allowDragAndDrop = ConfigurationManager.AppSettings["AllowDragAndDrop"] == "true";
            //if (!allowDragAndDrop) return;

            e.Handled = true;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string destinationPath = vm.CurFolder.Path == null ? vm.CurrentDrivePath : vm.CurFolder.Path;
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
                            FileSystem.CopyFile(file, destinationPath + fileName, UIOption.AllDialogs, UICancelOption.DoNothing);
                        }
                    }
                    vm.NavigateToFolder(destinationPath);
                }
            }
            else if (e.Data.GetDataPresent("Shell IDList Array"))
            {
                var shellObjects = (System.Runtime.InteropServices.ComTypes.IDataObject)e.Data.GetData("Shell IDList Array");
                // Process shell objects (e.g., display their names in the TextBox)
                ProcessShellObjects(shellObjects);
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
            else
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    HighlightText(VisualTreeHelper.GetChild(obj, i), regex);
                }
            }
        }

        private void txtSearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtSearchBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {

            //Highlight searched text
            var tmp = vm.CurrentDrivePath;
            DataGrid dataGrid;
            if (string.IsNullOrEmpty(tmp) && TabView1.SelectedIndex == 0)
            {
                dataGrid = ListOfRecentItems;
            }
            else if (string.IsNullOrEmpty(tmp) && TabView1.SelectedIndex == 1)
            {
                dataGrid = ListOfPinnedItems;
            }
            else
            {
                dataGrid = ListOfDriveItems;
            }
            this.Dispatcher.Invoke(() =>
            {
                Regex regex = null;

                if (!string.IsNullOrEmpty(vm.TxtSearchBoxItem))
                {
                    regex = new Regex($"({Regex.Escape(vm.TxtSearchBoxItem)})", RegexOptions.IgnoreCase);
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

        }

    }
}