namespace ModernIU.Controls
{
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Interop;
    using System.Windows.Threading;

    public class PopupEx : Popup
    {
        public static DependencyProperty TopmostProperty = Window.TopmostProperty.AddOwner(typeof(PopupEx), new FrameworkPropertyMetadata(false, OnTopmostChanged));
        public static readonly DependencyProperty IsUpdatePositionProperty = DependencyProperty.Register(nameof(IsUpdatePosition), typeof(bool), typeof(PopupEx), new PropertyMetadata(true));


        public bool Topmost
        {
            get { return (bool)GetValue(TopmostProperty); }
            set { SetValue(TopmostProperty, value); }
        }

        private static void OnTopmostChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as PopupEx).UpdateWindow();
        }


        public bool IsUpdatePosition
        {
            get { return (bool)GetValue(IsUpdatePositionProperty); }
            set { SetValue(IsUpdatePositionProperty, value); }
        }
        

        protected override void OnOpened(EventArgs e)
        {
            UpdateWindow();
        }

        private void UpdateWindow()
        {
            var hwnd = ((HwndSource)PresentationSource.FromVisual(this)).Handle;
            RECT rect;
            if (GetWindowRect(hwnd, out rect))
            {
                FrameworkElement element = this.PlacementTarget as FrameworkElement;
                if(element != null)
                {
                    SetWindowPos(hwnd, Topmost ? -1 : -2, rect.Left, rect.Top, (int)element.ActualWidth, (int)element.ActualHeight, 1);
                }
                else
                {
                    SetWindowPos(hwnd, Topmost ? -1 : -2, rect.Left, rect.Top, (int)this.Width, (int)this.Height, 1);
                }
            }
        }

        #region imports definitions
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32", EntryPoint = "SetWindowPos")]
        private static extern int SetWindowPos(IntPtr hWnd, int hwndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        #endregion
    }
}
