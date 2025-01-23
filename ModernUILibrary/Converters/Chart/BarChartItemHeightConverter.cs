namespace ModernIU.Converters
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Data;

    using ModernIU.Controls.Chart;

    public class BarChartItemHeightConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double result = 0;

            if (values != null &&
                values.Length == 3)
            {
                result = System.Convert.ToInt32(values[0]);

                ItemCollection rows = (ItemCollection)values[2];
                double height = System.Convert.ToDouble(values[1]) - 40;

                double max = GetMax(rows);
                result = (height / max) * result;
            }

            return (result);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private double GetMax(ItemCollection rows)
        {
            double result = 0;

            foreach (ChartRow row in rows)
            {
                if (row.Value > result) { result = row.Value; }
            }
            return (result);
        }

    }
}