namespace ModernIU.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;


    public class PieChartCenterConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Point result = new Point(0, 0);

            if (values != null && values.Length == 2)
            {
                double height = System.Convert.ToDouble(values[0]) - 30;
                double width = System.Convert.ToDouble(values[1]);

                result = new Point(width / 2, height / 2);
            }

            return (result);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}