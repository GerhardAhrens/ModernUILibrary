//-----------------------------------------------------------------------
// <copyright file="WindowExtensions.cs" company="Lifeprojects.de">
//     Class: WindowExtensions
//     Copyright � Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>.09.2018</date>
//
// <summary>Extension Class for WindowExtensions</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interop;
    using System.Windows.Media;

    using ModernBaseLibrary.Core;

    public static class WindowExtensions
    {
        private static bool firstTime = true;

        public static void CenterInScreen(this Window @this)
        {
            double width = @this.ActualWidth;
            double height = @this.ActualHeight;

            if (firstTime == true)
            {
                if (double.IsNaN(@this.Width) == false)
                {
                    width = @this.Width;
                }

                if (double.IsNaN(@this.Height) == false)
                {
                    height = @this.Height;
                }

                firstTime = false;
            }

            @this.Left = (SystemParameters.WorkArea.Width - width) / 2 + SystemParameters.WorkArea.Left;
            @this.Top = (SystemParameters.WorkArea.Height - height) / 2 + SystemParameters.WorkArea.Top;
        }

        public static bool IsModal(this Window window)
        {
            return (bool)typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(window);
        }

        public static void BringToFront(this FrameworkElement @this)
        {
            if (@this == null)
            {
                return;
            }

            Panel parent = @this.Parent as Panel;
            if (parent == null)
            {
                return;
            }

            var maxZ = parent.Children.OfType<UIElement>()
              .Where(x => x != @this)
              .Select(x => Panel.GetZIndex(x))
              .Max();

            Panel.SetZIndex(@this, maxZ + 1);
        }

        [Obsolete("Die Methode 'GetActiveWindow()' ist veraltet. Verwenden Sie 'LastActiveWindow()' mit gleicher Syntax.")]
        public static Window GetActiveWindow(this WindowCollection windows)
        {
            Window activeWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive == true);
            if (activeWindow != null)
            {
                return activeWindow;
            }

            return Application.Current.MainWindow;
        }

        public static Window LastActiveWindow(this WindowCollection @this)
        {
            Window activeWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive == true);
            if (activeWindow != null)
            {
                return activeWindow;
            }

            return Application.Current.MainWindow;
        }

        public static Window LastActiveWindow(this WindowCollection @this, bool isDefault)
        {
            Window activeWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive == true);
            if (activeWindow != null)
            {
                return activeWindow;
            }
            else
            {
                if (isDefault == true)
                {
                    return Application.Current.MainWindow;
                }
            }

            return null;
        }

        public static Window LastWindow(this WindowCollection @this)
        {
            Window current = null;

            current = @this.OfType<Window>().LastOrDefault();

            return current;
        }

        public static Window LastWindow(this Window @this)
        {
            Window current = @this;

            current = Application.Current.Windows.OfType<Window>().LastOrDefault();

            return current;
        }

        public static Window FirstWindow(this WindowCollection @this)
        {
            Window current = null;

            current = @this.OfType<Window>().FirstOrDefault();

            return current;
        }

        public static Window FirstWindow(this Window @this)
        {
            Window current = @this;

            current = Application.Current.MainWindow;

            return current;
        }

        public static System.Windows.Forms.DialogResult ShowDialog(this System.Windows.Forms.CommonDialog @this, Window parent)
        {
            return @this.ShowDialog(new Wpf32Window(parent));
        }

        public static Size GetNativePrimaryScreenSize(this Window @this)
        {
            PresentationSource mainWindowPresentationSource = PresentationSource.FromVisual(@this);
            Matrix m = mainWindowPresentationSource.CompositionTarget.TransformToDevice;
            var dpiWidthFactor = m.M11;
            var dpiHeightFactor = m.M22;
            double screenHeight = SystemParameters.PrimaryScreenHeight * dpiHeightFactor;
            double screenWidth = SystemParameters.PrimaryScreenWidth * dpiWidthFactor;

            return new Size(screenWidth, screenHeight);
        }

        // from winuser.h
        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x10000;
        private const int WS_MINIMIZEBOX = 0x20000;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll")]
        extern private static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        extern private static int SetWindowLong(IntPtr hwnd, int index, int value);

        /// <summary>
        /// Hides the minimize and maximize buttons.
        /// </summary>
        /// <param name="window">The window for which to hide the buttons.</param>
        public static void HideMinimizeAndMaximizeButtons(this Window window)
        {
            IntPtr hwnd = new WindowInteropHelper(window).Handle;
            var currentStyle = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, (currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX));
        }

        /// <summary>
        /// Hides the close button for the window. Also removes the icon and system menu from the top left corner.
        /// </summary>
        /// <param name="window">The window for which to hide the button.</param>
        public static void HideCloseButton(this Window window)
        {
            IntPtr hwnd = new WindowInteropHelper(window).Handle;
            var currentStyle = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, (currentStyle & ~WS_SYSMENU));
        }
    }
}
