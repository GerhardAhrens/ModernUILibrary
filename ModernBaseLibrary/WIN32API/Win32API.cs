//-----------------------------------------------------------------------
// <copyright file="Win32API.cs" company="Lifeprojects.de">
//     Class: Win32API
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.01.2023</date>
//
// <summary>
// Klasse für eine Sammlung von Win32 APIs
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Win32API
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows;
    using System.Windows.Interop;
    using ModernIU.Base;

    public class Win32API : SingletonCoreBase<Win32API>
    {
        #region Windows Dialog Frame
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public void HideCloseButton(Window w)
        {
            IntPtr hWnd = new WindowInteropHelper(w).Handle;
            SetWindowLong(hWnd, GWL_STYLE, GetWindowLong(hWnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        public void ShowCloseButton(Window w)
        {
            IntPtr hWnd = new WindowInteropHelper(w).Handle;
            SetWindowLong(hWnd, GWL_STYLE, GetWindowLong(hWnd, GWL_STYLE) | WS_SYSMENU);
        }
        #endregion Windows Dialog Frame

        #region Process Info from Kernel32
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

        public IntPtr CreateToolHelp32Snapshot()
        {
            uint TH32CS_SNAPPROCESS = 2;

            IntPtr hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
            if (hSnapshot == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }

            return hSnapshot;
        }
        #endregion Process Info from Kernel32

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        internal delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        internal static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            return IntPtr.Size == 8 ? GetWindowLongPtr64(hWnd, nIndex) : GetWindowLongPtr32(hWnd, nIndex);
        }

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

        internal delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        internal delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        [DllImport("user32.dll")]
        internal static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        [DllImport("user32.dll")]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateDC(string strDriver, string strDevice, string strOutput, IntPtr pData);

        [DllImport("gdi32.dll")]
        internal static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        internal static extern int GetPixel(IntPtr hdc, int x, int y);

        [DllImport("user32.dll")]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        internal static extern IntPtr GetWindow(IntPtr hWnd, GetWindowFlags uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        internal static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, IntPtr dwExtraInfo);

        internal static void MouseEvent(int buttons)
        {
            mouse_event(buttons, 0, 0, 0, (IntPtr)0);
        }

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        internal static void KeyboardEvent(byte key, int flag)
        {
            keybd_event(key, 0, flag, 0);
        }

        //https://msdn.microsoft.com/de-de/library/windows/desktop/ms646304(v=vs.85).aspx
        internal static class KeyboardEventFlags
        {
            internal static int KEYEVENTF_EXTENDEDKEY = 0x0001;
            internal static int KEYEVENTF_KEYUP = 0x0002;
        }

        [Flags]
        internal enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010,
            WHEEL = 0x00000800,
            XDOWN = 0x00000080,
            XUP = 0x00000100
        }

        //Use the values of this enum for the 'dwData' parameter
        //to specify an X button when using MouseEventFlags.XDOWN or
        //MouseEventFlags.XUP for the dwFlags parameter.
        internal enum MouseEventDataXButtons
        {
            XBUTTON1 = 0x00000001,
            XBUTTON2 = 0x00000002
        }

        internal const int ID_Close = 0x10;

        internal const uint WS_DISABLED = 0x8000000;

        internal enum ShowWindowCommands
        {
            Hide = 0,
            Normal = 1,
            ShowMinimized = 2,
            Maximize = 3,
            ShowMaximized = 3,
            ShowNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActive = 7,
            ShowNA = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimize = 11
        }

        internal enum GetWindowFlags : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public WindowState showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        internal enum WindowLongFlags : int
        {
            GWL_EXSTYLE = -20,
            GWLP_HINSTANCE = -6,
            GWLP_HWNDPARENT = -8,
            GWL_ID = -12,
            GWL_STYLE = -16,
            GWL_USERDATA = -21,
            GWL_WNDPROC = -4,
            DWLP_USER = 0x8,
            DWLP_MSGRESULT = 0x0,
            DWLP_DLGPROC = 0x4
        }

        internal static class HwndInsertAfter
        {
            public static IntPtr HWND_NOTOPMOST = new IntPtr(-2);
            public static IntPtr HWND_TOPMOST = new IntPtr(-1);
            public static IntPtr HWND_TOP = new IntPtr(0);
            public static IntPtr HWND_BOTTOM = new IntPtr(1);
        }

        internal static class SetWindowPositionFlags
        {
            public static readonly uint SWP_NOSIZE = 0x0001;
            public static readonly uint SWP_NOMOVE = 0x0002;
            public static readonly uint SWP_NOZORDER = 0x0004;
            public static readonly uint SWP_NOREDRAW = 0x0008;
            public static readonly uint SWP_NOACTIVATE = 0x0010;
            public static readonly uint SWP_DRAWFRAME = 0x0020;
            public static readonly uint SWP_FRAMECHANGED = 0x0020;
            public static readonly uint SWP_SHOWWINDOW = 0x0040;
            public static readonly uint SWP_HIDEWINDOW = 0x0080;
            public static readonly uint SWP_NOCOPYBITS = 0x0100;
            public static readonly uint SWP_NOOWNERZORDER = 0x0200;
            public static readonly uint SWP_NOREPOSITION = 0x0200;
            public static readonly uint SWP_NOSENDCHANGING = 0x0400;
            public static readonly uint SWP_DEFERERASE = 0x2000;
            public static readonly uint SWP_ASYNCWINDOWPOS = 0x4000;
        }
    }

}

