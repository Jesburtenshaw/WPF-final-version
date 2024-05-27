using System;
using System.Windows;

namespace CDMWrapper
{
    public class CDMWrapper
    {
        public void showCDM(long param)
        {
            IntPtr hwnd = (IntPtr)param;
            System.Windows.Interop.HwndSourceParameters sourceParams = new System.Windows.Interop.HwndSourceParameters("CDMWrapper");
            sourceParams.ParentWindow = hwnd;
            sourceParams.WindowStyle = 0x10000000 | 0x40000000; // WS_VISIBLE | WS_CHILD; // style
            System.Windows.Interop.HwndSource source = new System.Windows.Interop.HwndSource(sourceParams);
            UIElement page = new CDM.UserControls.CDMUserControl();
            source.RootVisual = page;
        }
    }
}
