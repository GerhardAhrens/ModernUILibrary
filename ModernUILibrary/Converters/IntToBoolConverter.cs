//-----------------------------------------------------------------------
// <copyright file="IntToBoolConverter.cs" company="Lifeprojects.de">
//     Class: IntToBoolConverter
//     Copyright © Lifeprojects.de GmbH 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.06.2017</date>
//
// <summary>
// IntToBoolConverter Converter Class
// </summary>
//
// <example>
// IntToBoolConverter x:Key="intToBoolConverter" 
// Value="{Binding Path=IntValue, Converter={StaticResource intToBoolConverter}}"
// </example>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System.Globalization;
    using System.Windows.Data;

    public sealed class IntToBoolConverter : ConverterBase<int, bool>
    {
        protected override bool Convert(int value, CultureInfo culture)
        {
            bool boolValue = false;

            int intTempValue;
            if (int.TryParse(value.ToString(), out intTempValue) == true)
            {
                if (intTempValue == 0)
                {
                    boolValue = false;
                }
                else
                {
                    boolValue = true;
                }
            }

            return boolValue;
        }
    }
}