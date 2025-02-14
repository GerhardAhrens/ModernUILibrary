//-----------------------------------------------------------------------
// <copyright file="XamlPathIcon.cs" company="Lifeprojects.de">
//     Class: XamlPathIcon
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <Framework>7.0</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>07.05.2023 12:07:26</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Shapes;

    public class XamlPathIcon : XamlIcon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XamlPathIcon"/> class.
        /// </summary>
        static XamlPathIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(XamlPathIcon), new FrameworkPropertyMetadata(typeof(XamlPathIcon)));
        }

        public Path XamlPath
        {
            get => (Path)GetValue(XamlPathProperty);
            set => SetValue(XamlPathProperty, value);
        }

        public static readonly DependencyProperty XamlPathProperty =
            DependencyProperty.Register("XamlPath", typeof(FrameworkElement), typeof(XamlPathIcon), new PropertyMetadata(XamlPathPropertyChanged));

        private static void XamlPathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                if (((XamlPathIcon)d).IsEnabled)
                {
                    ((Path)e.NewValue).Fill = ((XamlPathIcon)d).IconForeground;
                }
                else
                {
                    ((Path)e.NewValue).Fill = ((XamlPathIcon)d).DisabledForeground;
                }

            }
        }
    }
}
