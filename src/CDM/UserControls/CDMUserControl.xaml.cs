using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Management;
using System.Text;
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
using CDM.ViewModels;
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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            Application.Current.DispatcherUnhandledException += Application_DispatcherUnhandledException;

            InitializeComponent();
            vm = new CDMViewModel();
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
        }
        private void DriveManager_DrivesStateChanged(object sender, bool e)
        {
            Application.Current.Dispatcher.Invoke(() =>
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
            var allowDragAndDrop = ConfigurationManager.AppSettings["AllowDragAndDrop"] == "true";
            if (!allowDragAndDrop) return;

            e.Handled = true;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    // Do something with the file (e.g., display the file path in the TextBox)
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
    }
}