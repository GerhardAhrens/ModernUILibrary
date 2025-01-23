namespace ModernIU.Converters
{
    using System;
    using System.Windows.Data;

    public class PieChartRadiusConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double result = 1;

            if (values != null && values.Length == 2)
            {
                double height = System.Convert.ToDouble(values[0]) - 30;
                double width = System.Convert.ToDouble(values[1]);

                if (height > width)
                {
                    result = height / 2;
                }
                else
                {
                    result = width / 2;
                }
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}