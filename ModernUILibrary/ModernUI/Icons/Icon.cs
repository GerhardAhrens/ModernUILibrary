namespace ModernIU.Controls
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    using ModernIU.Base;

    public class Icon : Control
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(PathFigureCollection), typeof(Icon));

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(EnumIconType), typeof(Icon));

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register("IconSize", typeof(IconSize), typeof(Icon), new PropertyMetadata(IconSize.Default));

        #region Constructors
        static Icon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Icon), new FrameworkPropertyMetadata(typeof(Icon)));
        }
        #endregion

        public EnumIconType Type
        {
            get { return (EnumIconType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public PathFigureCollection Data
        {
            get { return (PathFigureCollection)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public IconSize IconSize
        {
            get => (IconSize)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
    }

    public class IconSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            const int defaultSize = 20;

            if (!(value is IconSize))
            {
                return defaultSize;
            }

            var iconSizeValue = (IconSize)value;

            switch (iconSizeValue)
            {
                case IconSize.Default:
                    return defaultSize;
                case IconSize.ExtraSmall:
                    return defaultSize / 2;
                case IconSize.Small:
                    return defaultSize * 3 / 4;
                case IconSize.Large:
                    return defaultSize * 3 / 2;
                case IconSize.ExtraLarge:
                    return defaultSize * 2;
                case IconSize.BigLarge:
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
