namespace ModernIU.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    public class BarChartItemVisibilityRowDefinitionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int result = 0;
            if (value is Visibility)
            {
                switch ((Visibility)value)
                {
                    case System.Windows.Visibility.Visible:
                        result = 25;
                        break;
                    case System.Windows.Visibility.Hidden:
                        result = 0;
                        break;
                    case System.Windows.Visibility.Collapsed:
                        result = 0;
                        break;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}