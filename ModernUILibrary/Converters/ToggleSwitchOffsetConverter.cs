//-----------------------------------------------------------------------
// <copyright file="ToggleSwitchOffsetConverter.cs" company="Lifeprojects.de">
//     Class: ToggleSwitchOffsetConverter
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>20.12.2017</date>
//
// <summary>
// WPF Converter setzt die breite eines ToggleSwitch Control
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public sealed class ToggleSwitchOffsetConverter : IValueConverter
    {
        public bool IsReversed { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var width = (double)value;
            return width > 20D ? IsReversed ? -((width / 2) - 10) : (width / 2) - 10 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}