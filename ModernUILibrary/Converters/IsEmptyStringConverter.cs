//-----------------------------------------------------------------------
// <copyright file="IsEmptyStringConverter.cs" company="Lifeprojects.de">
//     Class: IsEmptyStringConverter
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>6.07.2020</date>
//
// <summary>
// Der Converter liefert false zurück, wenn der String leer oder null ist
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// This converter is used to check if a value is of type string.Empty..
    /// </summary>
    [ValueConversion(typeof(string), typeof(bool))]
  public class IsEmptyStringConverter : ConverterExtension
  {
        /// <summary>
        /// Checks whether the value is <see cref="String.Empty"/>.
        /// </summary>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string s = value as string;
                return s == string.Empty;
            }
            else
            {
                return false;
            }
        }

    }
}