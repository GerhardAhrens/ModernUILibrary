//-----------------------------------------------------------------------
// <copyright file="ScrollViewerHelper.cs" company="Lifeprojects.de">
//     Class: ScrollViewerHelper
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>14.05.2025</date>
//
// <summary>
// Die Klasse stellt Funktionen zur Verfügung um per ViewModel den
// Horizontalen und Vertikalen ScrollViewer zu positionieren.
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Base
{
    using System.Windows;
    using System.Windows.Controls;

    public class ScrollViewerHelper
    {
        #region HorizontalOffset

        /// <summary>
        /// HorizontalOffset Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ScrollViewerHelper),
                new FrameworkPropertyMetadata((double)0.0, new PropertyChangedCallback(OnHorizontalOffsetChanged)));

        /// <summary>
        /// Gets the HorizontalOffset property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static double GetHorizontalOffset(DependencyObject d)
        {
            return (double)d.GetValue(HorizontalOffsetProperty);
        }

        /// <summary>
        /// Sets the HorizontalOffset property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetHorizontalOffset(DependencyObject d, double value)
        {
            d.SetValue(HorizontalOffsetProperty, value);
        }

        /// <summary>
        /// Handles changes to the HorizontalOffset property.
        /// </summary>
        private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = (ScrollViewer)d;
            if (viewer != null)
            {
                viewer.ScrollToHorizontalOffset((double)e.NewValue);
            }
        }

        #endregion

        #region VerticalOffset

        /// <summary>
        /// VerticalOffset Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ScrollViewerHelper),
                new FrameworkPropertyMetadata((double)0.0, new PropertyChangedCallback(OnVerticalOffsetChanged)));

        /// <summary>
        /// Gets the VerticalOffset property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static double GetVerticalOffset(DependencyObject d)
        {
            return (double)d.GetValue(VerticalOffsetProperty);
        }

        /// <summary>
        /// Sets the VerticalOffset property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetVerticalOffset(DependencyObject d, double value)
        {
            d.SetValue(VerticalOffsetProperty, value);
        }

        /// <summary>
        /// Handles changes to the VerticalOffset property.
        /// </summary>
        private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = (ScrollViewer)d;
            if (viewer != null)
            {
                viewer.ScrollToVerticalOffset((double)e.NewValue);
            }
        }

        #endregion

    }
}
