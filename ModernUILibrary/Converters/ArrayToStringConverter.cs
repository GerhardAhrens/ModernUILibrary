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
// Converter Class erstellt aus einem IEnumerable einen String und gibt diesen zurück
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    /// <summary>
    /// Converts <see cref="IEnumerable"/> to <see cref="string"/> by using <see cref="string.Join(string,object[])"/>
    /// </summary>
    [ValueConversion(typeof(IEnumerable), typeof(string))]
    public class ArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var enumerable = (IEnumerable) value;
            var separator = parameter as string ?? ", ";
            return string.Join(separator, enumerable.Cast<object>());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var joined = (string) value;
            var separator = parameter as string ?? ", ";
            return joined.Split(new [] {separator}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}