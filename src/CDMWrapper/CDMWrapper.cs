using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace CDMWrapper
{
    public class CDMWrapper
    {
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        public void showCDM(long param, long param2, long param3)
        {
            IntPtr hwnd = (IntPtr)param;
            IntPtr hwndParent = (IntPtr)param2;
            IntPtr hwndLeft = (IntPtr)param3;

            RECT lpRect;
            GetClientRect(hwndParent, out lpRect);

            RECT lpRectLeft;
            GetClientRect(hwndLeft, out lpRectLeft);
            double width = (lpRect.Right - lpRect.Left) - (lpRectLeft.Right - lpRectLeft.Left);
            double height = (lpRect.Bottom - lpRect.Top);
            //MessageBox.Show("w: "+ width + "  h: "+ height);

            System.Windows.Interop.HwndSourceParameters sourceParams = new System.Windows.Interop.HwndSourceParameters("CDMWrapper");
            sourceParams.ParentWindow = hwnd;
            sourceParams.WindowStyle = 0x10000000 | 0x40000000; // WS_VISIBLE | WS_CHILD; // style
            System.Windows.Interop.HwndSource source = new System.Windows.Interop.HwndSource(sourceParams);
            UIElement page = new CDM.UserControls.CDMUserControl(source.Dispatcher,width, height);
            source.RootVisual = page;
        }
    }
}
