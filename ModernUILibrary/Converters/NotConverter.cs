namespace ModernIU.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    public class NotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(Visibility))
            {
                bool givenBool = (bool)value;
                if (givenBool) { return Visibility.Collapsed; } else { return Visibility.Visible; }
            }
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }

}
