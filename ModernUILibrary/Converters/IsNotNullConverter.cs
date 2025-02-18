//-----------------------------------------------------------------------
// <copyright file="IsNotNullConverter.cs" company="Lifeprojects.de">
//     Class: IsNotNullConverter
//     Copyright © Lifeprojects.de GmbH 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>06.07.2020</date>
//
// <summary>
// WPF Converter liefert true zurück, wenn ein Object != null ist.
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Returns true if the input value is not null.
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsNotNullConverter : ConverterExtension
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }
    }
}