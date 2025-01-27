namespace ModernIU.Converters
{
    using System;
    using System.Windows.Data;


    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public class TimeSpanToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TimeSpan givenValue = (TimeSpan)value;
            return givenValue.Ticks;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new TimeSpan(((long)value));
        }
    }
}
