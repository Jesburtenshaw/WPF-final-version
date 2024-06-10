using System;
using System.Windows;

namespace CDMWrapper
{
    public class CDMWrapper
    {
        public void showCDM(long param, long paramParent, int width, int height)
        {
            IntPtr hwnd = (IntPtr)param;

            System.Windows.Interop.HwndSourceParameters sourceParams = new System.Windows.Interop.HwndSourceParameters("CDMWrapper");
            sourceParams.ParentWindow = hwnd;
            sourceParams.WindowStyle = 0x10000000 | 0x40000000; // WS_VISIBLE | WS_CHILD; // style

            MessageBox.Show("4");
            System.Windows.Interop.HwndSource source = new System.Windows.Interop.HwndSource(sourceParams);
            MessageBox.Show("5");
            UIElement page = new CDM.UserControls.CDMUserControl(width, height);
            MessageBox.Show("6");
            source.RootVisual = page;
            MessageBox.Show("7");
        }
    }
}
