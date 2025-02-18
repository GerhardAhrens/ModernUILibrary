//-----------------------------------------------------------------------
// <copyright file="IsEqualOrGreaterThanConverter.cs" company="Lifeprojects.de">
//     Class: IsEqualOrGreaterThanConverter
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.06.2017</date>
//
// <summary>
// IsEqualOrGreaterThanConverter Converter Class
// </summary>
//
// <example>
// DataTrigger Binding = "{Binding Path=Text.Length, ElementName=textBox1, Converter={LibConverter:IsEqualOrGreaterThanConverter}, ConverterParameter=1}" Value="True">
//            Setter Property = "Button.IsEnabled" Value="True" />
//            Setter Property = "Button.Foreground" Value="Red" />
// /DataTrigger>
// </example>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public sealed class IsEqualOrGreaterThanConverter : ConverterBase<int, object, string>
    {
        protected override object Convert(int value, string parameter, CultureInfo culture)
        {
            int intValue = -1;
            int compareToValue = -1;

            intValue = value;
            compareToValue = parameter == null ? 0 : System.Convert.ToInt32(parameter);

            return intValue >= compareToValue;
        }
    }
}