using CDM.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace CDM
{
    [ComVisible(true)] // Make the class visible to COM
    public class MyClass : IMyClass // Implement the interface
    {
        public int GetOne()
        {
            return 1;
        }

        public void CreateGUI()
        {
            CDMUserControl userControl = new CDMUserControl();
        }

        public void InitializeGUI(IntPtr hwnd, IntPtr instance)
        {
            //// Create an HwndSource object
            //HwndSourceParameters parameters = new HwndSourceParameters("CDMControl")
            //{
            //    ParentWindow = hwnd,
            //};

            //HwndSource hwnd1 = new HwndSource(parameters);
            //hwnd1.RootVisual = new ButtonControl();


            //// Use hwnd and instance to host WPF in native window
            //// Set the parent window for your WPF application
            //WindowInteropHelper helper = new WindowInteropHelper(this);
            //helper.Owner = hwnd;

            //// Load your UserControl into the WPF application
            //ButtonControl userControl = new ButtonControl();
            //this.Content = userControl;
        }
    }

    [ComVisible(true)] // Make the interface visible to COM
    public interface IMyClass
    {
        int GetOne();
        void CreateGUI();
        void InitializeGUI(IntPtr hwnd, IntPtr instance);
    }
}
