
namespace ModernIU.Converters
{
    using System;
    using System.Text.RegularExpressions;
    using System.Windows.Data;

    [ValueConversion(typeof(bool), typeof(RegexOptions))]
    public class RegOptionsSetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var result = (bool)value ? RegexOptions.None : RegexOptions.IgnoreCase;
                return result;
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
