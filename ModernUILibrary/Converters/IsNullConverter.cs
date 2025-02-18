//-----------------------------------------------------------------------
// <copyright file="IsNullConverter.cs" company="Lifeprojects.de">
//     Class: IsNullConverter
//     Copyright © Lifeprojects.de GmbH 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>06.07.2020</date>
//
// <summary>
// WPF Converter liefert true zurück, wenn ein Object == null ist.
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Returns true if the input value is a mull reference.
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsNullConverter : ConverterExtension
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }
    }
}