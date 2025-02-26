namespace ModernIU.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    public class XamlIconHost : Control
    {
        static XamlIconHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(XamlIconHost), new FrameworkPropertyMetadata(typeof(XamlIconHost)));
        }

        public static readonly DependencyProperty XamlIconProperty =
            DependencyProperty.Register(nameof(XamlIcon), typeof(FrameworkElement), typeof(XamlIconHost), new PropertyMetadata(null));

        public FrameworkElement XamlIcon
        {
            get => (FrameworkElement) GetValue(XamlIconProperty);
            set => SetValue(XamlIconProperty, value);
        }

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(IconSize), typeof(XamlIconHost), new PropertyMetadata(IconSize.Medium));

        public IconSize IconSize
        {
            get => (IconSize) GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(XamlIconHost), new PropertyMetadata(null));

        public string Caption
        {
            get => (string) GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }

        public static readonly DependencyProperty CaptionPositionProperty =
            DependencyProperty.Register("CaptionPosition", typeof(CaptionPosition), typeof(XamlIconHost), new PropertyMetadata(CaptionPosition.ToRightOfIcon));

        public CaptionPosition CaptionPosition
        {
            get => (CaptionPosition) GetValue(CaptionPositionProperty);
            set => SetValue(CaptionPositionProperty, value);
        }

        public static readonly DependencyProperty StandardForegroundProperty =
            DependencyProperty.Register("StandardForeground", typeof(Brush), typeof(XamlIconHost), new PropertyMetadata(Brushes.Black));

        public Brush StandardForeground
        {
            get => (Brush) GetValue(StandardForegroundProperty);
            set => SetValue(StandardForegroundProperty, value);
        }

        public static readonly DependencyProperty StandardHighlightProperty =
            DependencyProperty.Register("StandardHighlight", typeof(Brush), typeof(XamlIconHost), new PropertyMetadata(Brushes.White));

        public Brush StandardHighlight
        {
            get => (Brush) GetValue(StandardHighlightProperty);
            set => SetValue(StandardHighlightProperty, value);
        }

        public static readonly DependencyProperty DisabledForegroundProperty =
            DependencyProperty.Register("DisabledForeground", typeof(Brush), typeof(XamlIconHost), new PropertyMetadata(Brushes.Silver));

        public Brush DisabledForeground
        {
            get => (Brush) GetValue(DisabledForegroundProperty);
            set => SetValue(DisabledForegroundProperty, value);
        }

        public static readonly DependencyProperty DisabledHighlightProperty =
            DependencyProperty.Register("DisabledHighlight", typeof(Brush), typeof(XamlIconHost), new PropertyMetadata(Brushes.Gray));

        public Brush DisabledHighlight
        {
            get => (Brush) GetValue(DisabledHighlightProperty);
            set => SetValue(DisabledHighlightProperty, value);
        }
    }

    public enum CaptionPosition
    {
        None,
        ToLeftOfIcon,
        AboveIcon,
        ToRightOfIcon,
        BelowIcon
    }

    public enum IconSize
    {
        ExtraSmall,
        Small,
        Medium,
        Large,
        ExtraLarge,
        ExtraExtraLarge
    }

    public class XamlIconSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            const int defaultSize = 40;

            if (!(value is IconSize))
            {
                return defaultSize;
            }

            var iconSizeValue = (IconSize) value;

            switch (iconSizeValue)
            {
                case IconSize.ExtraSmall:
                    return defaultSize / 2;
                case IconSize.Small:
                    return defaultSize * 3 / 4;
                case IconSize.Large:
                    return defaultSize * 3 / 2;
                case IconSize.ExtraLarge:
                    return defaultSize * 2;
                case IconSize.ExtraExtraLarge:
                    return defaultSize * 5 / 2;
                default:
                    return defaultSize;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}