using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using static CDMWrapper.CDMWrapper;

namespace CDMWrapper
{
    public class MyWindow
    {
        private IntPtr hwnd;
        private IntPtr hwndParent;
        private IntPtr hwndLeft;
        private WndProc newProc;
        private IntPtr oldProc;
        private CDM.UserControls.CDMUserControl cdmControl;

        delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        const int GWLP_WNDPROC = -4;

        public MyWindow(IntPtr hwnd, IntPtr hwndParent, IntPtr hwndLeft, CDM.UserControls.CDMUserControl userControl)
        {
            this.cdmControl = userControl;
            this.hwnd = hwnd;
            this.hwndParent = hwndParent;
            this.hwndLeft = hwndLeft;

            this.newProc = new WndProc(WindowProc);
            this.oldProc = SetWindowLongPtr(hwnd, GWLP_WNDPROC, Marshal.GetFunctionPointerForDelegate(newProc));
        }
        private IntPtr WindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                // Handle messages here
                switch (msg)
                {
                    case 0x0005: // WM_SIZE
                        {
                            RECT lpRect;
                            GetClientRect(hwndParent, out lpRect);

                            RECT lpRectLeft;
                            GetClientRect(hwndLeft, out lpRectLeft);
                            double width = (lpRect.Right - lpRect.Left) - (lpRectLeft.Right - lpRectLeft.Left);
                            double height = (lpRect.Bottom - lpRect.Top);
                            //MessageBox.Show("w: " + width + "  h: " + height);
                            if (height > 0 && width > 0)
                            {

                                cdmControl.Height = height;
                                cdmControl.Width = width;

                                cdmControl.MasterSizeChanged(height, width);
                                cdmControl.Dispatcher.Invoke(() =>
                                {
                                    cdmControl.UpdateLayout();
                                });
                            }
                            else
                            {
                                //Negative height or width received
                                // MessageBox.Show($"height: {height}, width:{width}");
                            }

                        }
                        break;

                        // Add more cases as needed for different messages
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
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
