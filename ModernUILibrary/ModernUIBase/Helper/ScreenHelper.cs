namespace ModernIU.Base
{
    using System;
    using System.Runtime.InteropServices;

    public class ScreenHelper
    {
        #region Get DPI
        [DllImport("gdi32.dll", EntryPoint = "CreateDC", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr CreateDC(string lpszDriver, string lpszDeviceName, string lpszOutput, IntPtr devMode);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern Int32 GetDeviceCaps(IntPtr hdc, Int32 capindex);
        #endregion Get DPI

        private const int LOGPIXELSX = 88;
        private static int _dpi = -1;

        public static int DPI
        {
            get
            {
                if (_dpi != -1)
                    return _dpi;

                _dpi = 96;
                try
                {
                    IntPtr hdc = CreateDC("DISPLAY", null, null, IntPtr.Zero);
                    if (hdc != IntPtr.Zero)
                    {
                        _dpi = GetDeviceCaps(hdc, LOGPIXELSX);
                        if (_dpi == 0)
                        {
                            _dpi = 96;
                        }
                        DeleteDC(hdc);
                    }
                }
                catch (Exception)
                {
                    _dpi = -1;
                }

                return _dpi;
            }
        }
    }
}
