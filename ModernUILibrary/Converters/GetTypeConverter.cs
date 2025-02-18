//-----------------------------------------------------------------------
// <copyright file="GetTypeConverter.cs" company="Lifeprojects.de">
//     Class: GetTypeConverter
//     Copyright © Lifeprojects.de GmbH 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>27.10.2017</date>
//
// <summary>
// Der WPF Converter gibt den gebunden Datentyp zurück
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Return the type of the binding
    /// </summary>
    public class GetTypeConverter : ConverterSimpleBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.GetType();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
