using System;
using System.Diagnostics;
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
                Debug.WriteLine($"msg: {msg}    , wParam {wParam.ToInt32()}    ,lParam {lParam}");

                /*
                   got below values on backspace key press
                   msg: 144 , wParam 0    ,lParam 0
                   msg: 24  , wParam 0    ,lParam 0
                   msg: 2   , wParam 0    ,lParam 0
                   msg: 130 , wParam 0    ,lParam 0
                */

                //Tried below code, but sometimes not hitting and crash sometime
                //if (msg == 144)
                //{
                //    Debug.WriteLine($"144  - BackSpace");
                //    MessageBox.Show("144  - BackSpace");
                //}

                // Handle messages here
                switch (msg)
                {
                    case 0x0100: // WM_KEYDOWN
                        {
                            int virtualKeyCode = wParam.ToInt32();

                            if (virtualKeyCode == 32) // Check for backspace key
                            {
                                //    // Handle backspace key press here
                                //    // Example: cdmControl.HandleBackspaceKeyPress();
                                //    // Replace with your actual logic to handle the backspace press
                                Debug.WriteLine($"WM_KEYDOWN  - BackSpace");
                                MessageBox.Show("WM_KEYDOWN  - BackSpace");
                            }
                        }
                        break;


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
