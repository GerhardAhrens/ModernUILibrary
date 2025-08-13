//-----------------------------------------------------------------------
// <copyright file="IntToVisibilityConverter.cs" company="Lifeprojects.de">
//     Class: IntToVisibilityConverter
//     Copyright © Lifeprojects.de GmbH 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.06.2017</date>
//
// <summary>
// IntToVisibilityConverter Converter Class
// </summary>
//
// <example>
// IntToVisibilityConverter x:Key="intToVisibilityConverter" 
// Value="{Binding Path=IntValue, Converter={StaticResource intToVisibilityConverter}}"
// </example>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public sealed class IntToVisibilityConverter : ConverterBase<int, Visibility>
    {
        protected override Visibility Convert(int value, CultureInfo culture)
        {
            Visibility result = Visibility.Visible;

            int intTempValue;
            if (int.TryParse(value.ToString(), out intTempValue) == true)
            {
                if (intTempValue == 0)
                {
                    result = Visibility.Hidden;
                }
                else
                {
                    result = Visibility.Visible;
                }
            }

            return result;
        }
    }
}