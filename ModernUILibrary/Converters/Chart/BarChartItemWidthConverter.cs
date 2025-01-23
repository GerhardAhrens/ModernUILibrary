namespace ModernIU.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;


    public class BarChartItemWidthConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            double result = 1;

            if(values != null &&
                values.Length == 5 &&
                values[0] is double &&
                values[1] is Int32 &&
                values[2] is Thickness &&
                values[3] is ScrollBarVisibility &&
                values[4] is double) {

                double height = System.Convert.ToDouble(values[0]);
                double count = System.Convert.ToDouble(values[1]);
                Thickness margin = (Thickness)values[2];
                ScrollBarVisibility scrollbarVisibility = (ScrollBarVisibility)values[3];
                result = (double)values[4];

                if(scrollbarVisibility == ScrollBarVisibility.Hidden ||
                    scrollbarVisibility == ScrollBarVisibility.Disabled) {
                    double offset = margin.Left + margin.Right;
                    result = (height / count) - offset;
                }
            }

            return(result);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

    }
}