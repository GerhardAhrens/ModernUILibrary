namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class WordAngleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string selected = "00";
            int hour = 0;

            if(int.TryParse(System.Convert.ToString(value), out hour))
            {
                if(hour < 10)
                {
                    selected = "0" + hour;
                }
                else
                {
                    selected = System.Convert.ToString(hour);
                }
            }
            
            return selected;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
