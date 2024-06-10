
namespace ModernIU.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Data;

    public class StringToWordsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string stringToSplit = value as string;
            if (null == value)
                return string.Empty;

            List<string> strings = new List<string>();
            foreach (string word in stringToSplit.Split(" ,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                strings.Add(word);
            }
            return strings;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
