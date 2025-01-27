namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class BooleanToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToBoolean(value))
                return Visibility.Visible;
            else
                if (parameter != null && parameter.ToString() == "H")
            {
                return Visibility.Hidden;
            }
            else
            {
                return Visibility.Collapsed;
            }
                
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
