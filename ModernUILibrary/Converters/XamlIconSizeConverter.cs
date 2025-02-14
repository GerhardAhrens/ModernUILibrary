namespace ModernIU.Converters
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Data;

    using static ModernIU.Controls.XamlIcon;

    [ValueConversion(typeof(string), typeof(string))]
    public class XamlIconSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            const int defaultSize = 40;

            if (!(value is EnuIconSize))
            {
                return defaultSize;
            }

            var iconSizeValue = (EnuIconSize)value;

            switch (iconSizeValue)
            {
                case EnuIconSize.XSmall:
                    return defaultSize / 2;
                case EnuIconSize.Small:
                    return defaultSize * 3 / 4;
                case EnuIconSize.Large:
                    return defaultSize * 3 / 2;
                case EnuIconSize.ExtraLarge:
                    return defaultSize * 2;
                case EnuIconSize.ExtraExtraLarge:
                    return defaultSize * 5 / 2;
                case EnuIconSize.Medium:
                    return defaultSize;
                default:
                    return defaultSize;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value).ToUpper();
        }
    }
}
