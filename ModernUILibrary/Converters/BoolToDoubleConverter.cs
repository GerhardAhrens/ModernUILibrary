namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Konvertiert einen booleschen Wert in einen Wert mit doppelter Genauigkeit.
    /// </summary>
    /// <remarks>
    /// Der Parameter sollte eine Zeichenkette sein, die eine Zahl darstellt, oder 2 Zahlen, die durch ein Semikolon (;) getrennt sind.
    /// Wenn der Parameter 2 Zahlen enthält, wird die erste Zahl als falscher Wert und die zweite als wahrer Wert verwendet.
    /// Wenn der Parameter 1 Zahl enthält, wird er als wahrer Wert verwendet, und eine 0 wird als falscher Wert verwendet.
    /// Wird der Parameter nicht angegeben, so wird 1 als wahrer Wert und 0 als falscher Wert verwendet.
    /// </remarks>
    [ValueConversion(typeof(bool), typeof(double), ParameterType = typeof(string))]
    public class BoolToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                throw new ArgumentException($"Cannot convert from type '{value.GetType().Name}' value");
            }

            double falseValue, trueValue;
            ParseParameter(parameter, out falseValue, out trueValue);
            return (bool)value ? trueValue : falseValue;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double))
            {
                throw new ArgumentException($"Cannot convert back from type '{value.GetType().Name}' value");
            }

            var doubleValue = (double)value;
            double falseValue, trueValue;
            ParseParameter(parameter, out falseValue, out trueValue);
            return doubleValue == trueValue;
        }

        private void ParseParameter(object parameter, out double falseValue, out double trueValue)
        {
            falseValue = 0.0;
            trueValue = 1.0;
            var stringValue = parameter as string;
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return;
            }

            var stringValues = stringValue.Split(';');
            if (stringValues.Length == 0)
            {
                return;
            }

            if (stringValues.Length == 1)
            {
                trueValue = double.Parse(stringValues[0]);
            }
            else
            {
                falseValue = double.Parse(stringValues[0]);
                trueValue = double.Parse(stringValues[1]);
            }
        }
    }
}
