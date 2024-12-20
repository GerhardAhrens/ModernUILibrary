namespace ModernIU.Base
{
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;

    public class DialogHelper
    {
        static Window GetWindowFromHwnd(IntPtr? hwnd)
        {
            return (Window)HwndSource.FromHwnd((IntPtr)hwnd).RootVisual;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        static Window GetTopWindow()
        {
            nint? hwnd = GetForegroundWindow();
            if (hwnd == null)
            {
                return null;
            }

            return GetWindowFromHwnd(hwnd);
        }

        public static void ShowDialog(Window win)
        {
            win.Owner = GetTopWindow();
            win.ShowInTaskbar = false;
            win.ShowDialog();
        }

        public static void ShowDialog(Window win, DependencyObject dependencyObject)
        {
            win.Owner = Window.GetWindow(dependencyObject);
            win.ShowInTaskbar = false;
            win.ShowActivated = true;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.ShowDialog();
        }
    }
}
