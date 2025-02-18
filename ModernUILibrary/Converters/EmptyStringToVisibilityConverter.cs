//-----------------------------------------------------------------------
// <copyright file="EmptyStringToVisibilityConverter.cs" company="Lifeprojects.de">
//     Class: EmptyStringToVisibilityConverter
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>06.07.2020</date>
//
// <summary>
// Der Converter liefert Visibility zurück, wenn der String leer oder null ist
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(string), typeof(Visibility))]
    public class EmptyStringToVisibilityConverter : ConverterExtension
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string s = value as string;
                return s == string.Empty ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

    }
}