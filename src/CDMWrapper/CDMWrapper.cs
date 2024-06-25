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
        IntPtr hwnd;
        IntPtr hwndParent;
        IntPtr hwndLeft;
        MyWindow myWindow;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        public void showCDM(long param, long param2, long param3)
        {
            hwnd = (IntPtr)param;
            hwndParent = (IntPtr)param2;
            hwndLeft = (IntPtr)param3;

            RECT lpRect;
            GetClientRect(hwndParent, out lpRect);

            RECT lpRectLeft;
            GetClientRect(hwndLeft, out lpRectLeft);
            double width = (lpRect.Right - lpRect.Left) - (lpRectLeft.Right - lpRectLeft.Left);
            double height = (lpRect.Bottom - lpRect.Top);
            //MessageBox.Show("w: "+ width + "  h: "+ height);

            
            myWindow = new MyWindow(hwnd);


            System.Windows.Interop.HwndSourceParameters sourceParams = new System.Windows.Interop.HwndSourceParameters("CDMWrapper");
            sourceParams.ParentWindow = hwnd;
            sourceParams.WindowStyle = 0x10000000 | 0x40000000; // WS_VISIBLE | WS_CHILD; // style
            System.Windows.Interop.HwndSource source = new System.Windows.Interop.HwndSource(sourceParams);
            UIElement page = new CDM.UserControls.CDMUserControl(source.Dispatcher,width, height);
            source.RootVisual = page;
        }
    }
    class MyWindow
        {
            private IntPtr hwnd;
            private WndProc newProc;
            private IntPtr oldProc;

            delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll")]
            static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            [DllImport("user32.dll")]
            static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

            const int GWLP_WNDPROC = -4;

            public MyWindow(IntPtr hwnd)
            {
                this.hwnd = hwnd;
                this.newProc = new WndProc(WindowProc);
                this.oldProc = SetWindowLongPtr(hwnd, GWLP_WNDPROC, Marshal.GetFunctionPointerForDelegate(newProc));
            }

            private IntPtr WindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
            {
                // Handle messages here
                switch (msg)
                {
                    case 0x0005: // WM_SIZE
                    MessageBox.Show("WM_SIZE");
                    break;
                        // Add more cases as needed for different messages
                }

                return CallWindowProc(oldProc, hWnd, msg, wParam, lParam);
            }

            ~MyWindow()
            {
                // Restore original window procedure to clean up
                SetWindowLongPtr(hwnd, GWLP_WNDPROC, oldProc);
            }
    }
}
