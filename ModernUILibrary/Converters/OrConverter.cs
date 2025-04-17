namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Ein MultiValue Konverter, der boolesche Werte mit oder Logik kombiniert.
    /// </summary>
    public class OrConverter : OneWayMultiValueConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.OfType<bool>().Any(b => b);
        }
    }
}
