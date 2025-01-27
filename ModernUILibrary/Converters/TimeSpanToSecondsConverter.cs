//-----------------------------------------------------------------------
// <copyright file="TimeSpanToSecondsConverter.cs" company="Lifeprojects.de">
//     Class: TimeSpanToSecondsConverter
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>15.01.2021</date>
//
// <summary>
// Converter Class konvertiert den Typ TimeSpan zu Sekunden
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Converts timespan to seconds
    /// </summary>
    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public class TimeSpanToSecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var ts = (TimeSpan) value;
            return ts.TotalSeconds;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var ticks = (double) value;
            return TimeSpan.FromSeconds(ticks);
        }
    }
}