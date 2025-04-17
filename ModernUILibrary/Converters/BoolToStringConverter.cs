namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Konvertiert einen booleschen Wert in einen String.
    /// </summary>
    /// <remarks>
    /// Der Parameter sollte eine Zeichenkette oder 2 durch ein Semikolon (;) getrennte Zeichenketten enthalten.
    /// Wenn der Parameter 2 Zeichenfolgen enthält, wird die erste Zeichenfolge als falscher Wert und die zweite als wahrer Wert verwendet.
    /// Enthält der Parameter eine Zeichenkette, wird diese als wahrer Wert verwendet, eine leere Zeichenkette wird als falscher Wert verwendet.
    /// </remarks>
    [ValueConversion(typeof(bool), typeof(string), ParameterType=typeof(string))]
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                throw new ArgumentException($"Cannot convert from type '{value.GetType().Name}' value");
            }

            string falseValue, trueValue;
            ParseParameter(parameter, out falseValue, out trueValue);
            return (bool)value ? trueValue : falseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double))
            {
                throw new ArgumentException($"Cannot convert back from type '{value.GetType().Name}' value");
            }

            var stringValue = (string)value;
            string falseValue, trueValue;
            ParseParameter(parameter, out falseValue, out trueValue);
            return stringValue.Equals(trueValue, StringComparison.Ordinal);
        }

        private void ParseParameter(object parameter, out string falseValue, out string trueValue)
        {
            falseValue = string.Empty;
            trueValue = string.Empty;
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
                trueValue = stringValues[0];
            }
            else
            {
                falseValue = stringValues[0];
                trueValue = stringValues[1];
            }
        }
    }
}
