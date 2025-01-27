//-----------------------------------------------------------------------
// <copyright file="TimeSpanToDaysConverter.cs" company="Lifeprojects.de">
//     Class: TimeSpanToDaysConverter
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>15.01.2021</date>
//
// <summary>
// Converter Class konvertiert den Typ TimeSpan zu einem Tag
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Converts timespan to days
    /// </summary>
    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public class TimeSpanToDaysConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var ts = (TimeSpan) value;
            return ts.TotalDays;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var ticks = (double) value;
            return TimeSpan.FromDays(ticks);
        }
    }
}