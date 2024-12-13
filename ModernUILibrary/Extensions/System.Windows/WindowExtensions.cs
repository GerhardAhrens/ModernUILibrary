//-----------------------------------------------------------------------
// <copyright file="WindowExtensions.cs" company="Lifeprojects.de">
//     Class: WindowExtensions
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>.09.2018</date>
//
// <summary>Extension Class for WindowExtensions</summary>
//-----------------------------------------------------------------------

namespace System.Windows
{
    using System.Linq;
    using System.Reflection;
    using System.Windows.Controls;
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

        public static Forms.DialogResult ShowDialog(this Forms.CommonDialog @this, Window parent)
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
    }
}
