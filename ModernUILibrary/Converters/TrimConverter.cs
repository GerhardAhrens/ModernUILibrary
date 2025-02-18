//-----------------------------------------------------------------------
// <copyright file="TrimConverter.cs" company="Lifeprojects.de">
//     Class: TrimConverter
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>20.12.2017</date>
//
// <summary>
// WPF Converter entfernt alle Leerzeichen aus einem String
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Returns white-space characters around a given string.
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    public sealed class TrimConverter : ConverterExtension
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = value as string;
            return val == null ? value : val.Trim();
        }
    }
}