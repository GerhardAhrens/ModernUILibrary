//-----------------------------------------------------------------------
// <copyright file="DummyConverter.cs" company="Lifeprojects.de">
//     Class: DummyConverter
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>05.03.2019</date>
//
// <summary>
// Die Converter Class dient als DummyConverter und gibt immer den übergebenen Wert zurück.
// Der Converter dient in der Hauptsachen zum Debuggen von einem WPF Binding
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(object), typeof(object))]
    public class DummyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
