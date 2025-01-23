namespace ModernIU.Converters
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Data;

    using ModernIU.Controls.Chart;

    public class StackedChartHeightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double result = 0;

            if (values != null &&
                values.Length == 3)
            {
                // Initialisieren mit dem übergebenen Wert
                result = System.Convert.ToInt32(values[0]);

                ItemCollection rows = (ItemCollection)values[2];
                double height = System.Convert.ToDouble(values[1]) - 30;

                double max = GetMax(rows);
                result = (height / max) * result;
            }

            return (result);
        }


        /// <summary>
        /// Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// An array of values that have been converted from the target value back to the source values.
        /// </returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the max value.
        /// </summary>
        /// <param name="rows">The rows.</param>
        /// <returns></returns>
        private double GetMax(ItemCollection rows)
        {
            double result = 0;

            foreach (StackedChartRow multiRow in rows)
            {
                double rowSum = 0;
                foreach (ChartRow row in multiRow.Values)
                {
                    rowSum += row.Value;
                }

                if (rowSum > result) { result = rowSum; }
            }
            return result;
        }
    }
}