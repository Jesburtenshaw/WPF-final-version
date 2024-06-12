using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace MyCSharpLibrary
{
    [ComVisible(true)]
    [Guid("D644E9BA-DA19-49D1-A047-2B095E3A01A3")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMyInterface
    {
        void SayHello(int x, int y);
        void showCDM(long param, long paramParent, int x, int y);
    }

    [ComVisible(true)]
    [Guid("76D407CD-6827-48A3-BE79-034C1F9C3F9B")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("MyCSharpLibrary.MyClass")]
    public class MyClass : IMyInterface
    {
        public void SayHello(int x, int y)
        {
            try
            {
                // Display a message box with the provided dimensions
                MessageBox.Show("Width: " + x + " | Height: " + y);

            }
            catch (Exception ex)
            {
                // Log or handle the error as necessary
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        public void showCDM(long param, long paramParent, int x, int y)
        {
            IntPtr hwnd = (IntPtr)param;
            IntPtr hwndParent = (IntPtr)paramParent;
            MessageBox.Show("Width: " + x + " | Height: " + y + " | hwnd: " + hwnd);

            RECT clientRect;
            if (GetClientRect(hwndParent, out clientRect))
            {
                int width = clientRect.Right - clientRect.Left;
                int height = clientRect.Bottom - clientRect.Top;

                MessageBox.Show("Calculated Width: " + width + " | Height: " + height );
            }
            else
            {
                Console.WriteLine("Failed to retrieve client rectangle.");
            }
            //System.Windows.Interop.HwndSourceParameters sourceParams = new System.Windows.Interop.HwndSourceParameters("CDMWrapper");
            //sourceParams.ParentWindow = hwnd;
            //sourceParams.WindowStyle = 0x10000000 | 0x40000000; // WS_VISIBLE | WS_CHILD; // style
            //System.Windows.Interop.HwndSource source = new System.Windows.Interop.HwndSource(sourceParams);
            //UIElement page = new CDM.UserControls.CDMUserControl();
            //source.RootVisual = page;
        }


    }
}
