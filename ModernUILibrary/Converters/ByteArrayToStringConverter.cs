//-----------------------------------------------------------------------
// <copyright file="ArrayToStringConverter.cs" company="Lifeprojects.de">
//     Class: ArrayToStringConverter
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>15.01.2021</date>
//
// <summary>
// Converter Class erstellt aus einem ByteArray einen String und gibt diesen zurück
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Windows.Data;

    /// <summary>
    /// Converts byte array to string
    /// </summary>
    [ValueConversion(typeof(byte[]), typeof(string))]
    public class ByteArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var bytes = (byte[]) value;
            var encoding = parameter as Encoding ?? Encoding.UTF8;
            return encoding.GetString(bytes);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var str = (string) value;
            var encoding = parameter as Encoding ?? Encoding.UTF8;
            return encoding.GetBytes(str);
        }
    }
}