namespace ModernIU.Converters
{
    using System;
    using System.Windows.Data;

    public class PieChartHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double result = 0;

            if (value is double)
            {
                result = (System.Convert.ToDouble(value) - 60) / 2;
            }

            return (result);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}