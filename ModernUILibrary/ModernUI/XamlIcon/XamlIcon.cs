//-----------------------------------------------------------------------
// <copyright file="XamlIcon.cs" company="Lifeprojects.de">
//     Class: XamlIcon
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <Framework>7.0</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>07.05.2023 12:04:24</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public abstract class XamlIcon : Control
    {
        public enum EnuCaptionPosition { None, ToLeftOfIcon, AboveIcon, ToRightOfIcon, BelowIcon }

        public enum EnuIconSize { XSmall, Small, Medium, Large, ExtraLarge, ExtraExtraLarge }

        //static XamlIconButton()
        //{
        //    DefaultStyleKeyProperty.OverrideMetadata(typeof(XamlIconButton), new FrameworkPropertyMetadata(typeof(XamlIconButton)));
        //}


        public EnuIconSize IconSize
        {
            get => (EnuIconSize)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(EnuIconSize), typeof(XamlIcon), new PropertyMetadata(EnuIconSize.Medium));

        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(XamlIcon), new PropertyMetadata(null));

        public EnuCaptionPosition CaptionPosition
        {
            get => (EnuCaptionPosition)GetValue(CaptionPositionProperty);
            set => SetValue(CaptionPositionProperty, value);
        }

        public static readonly DependencyProperty CaptionPositionProperty =
            DependencyProperty.Register("CaptionPosition", typeof(EnuCaptionPosition), typeof(XamlIcon), new PropertyMetadata(EnuCaptionPosition.ToRightOfIcon));

        public Brush StandardForeground
        {
            get => (Brush)GetValue(StandardForegroundProperty);
            set => SetValue(StandardForegroundProperty, value);
        }

        public static readonly DependencyProperty StandardForegroundProperty =
            DependencyProperty.Register("StandardForeground", typeof(Brush), typeof(XamlIcon), new PropertyMetadata(Brushes.Black));

        public Brush IconForeground
        {
            get => (Brush)GetValue(IconForegroundProperty);
            set => SetValue(IconForegroundProperty, value);
        }

        public static readonly DependencyProperty IconForegroundProperty =
            DependencyProperty.Register("IconForeground", typeof(Brush), typeof(XamlIcon), new PropertyMetadata(Brushes.Black, IconForegroundChanged));

        private static void IconForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((e.NewValue != null) && ((XamlPathIcon)d).XamlPath != null)
            {
                ((XamlPathIcon)d).XamlPath.Fill = (Brush)e.NewValue;

            }
        }

        public Brush StandardHighlight
        {
            get => (Brush)GetValue(StandardHighlightProperty);
            set => SetValue(StandardHighlightProperty, value);
        }

        public static readonly DependencyProperty StandardHighlightProperty =
            DependencyProperty.Register("StandardHighlight", typeof(Brush), typeof(XamlIcon), new PropertyMetadata(Brushes.White));

        public Brush DisabledForeground
        {
            get => (Brush)GetValue(DisabledForegroundProperty);
            set => SetValue(DisabledForegroundProperty, value);
        }

        public static readonly DependencyProperty DisabledForegroundProperty =
            DependencyProperty.Register("DisabledForeground", typeof(Brush), typeof(XamlIcon), new PropertyMetadata(Brushes.Silver));

        public Brush DisabledHighlight
        {
            get => (Brush)GetValue(DisabledHighlightProperty);
            set => SetValue(DisabledHighlightProperty, value);
        }

        public static readonly DependencyProperty DisabledHighlightProperty =
            DependencyProperty.Register("DisabledHighlight", typeof(Brush), typeof(XamlIcon), new PropertyMetadata(Brushes.Gray));

        public Thickness CaptionMargin
        {
            get => (Thickness)GetValue(CaptionMarginProperty);
            set => SetValue(CaptionMarginProperty, value);
        }

        public static readonly DependencyProperty CaptionMarginProperty =
            DependencyProperty.Register("CaptionMargin", typeof(Thickness), typeof(XamlIcon), new PropertyMetadata(new Thickness(0, 0, 0, 0)));
    }
}
