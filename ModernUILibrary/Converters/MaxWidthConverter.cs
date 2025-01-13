//-----------------------------------------------------------------------
// <copyright file="MaxWidthConverter.cs" company="Lifeprojects.de">
//     Class: MaxWidthConverter
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>27.11.2018</date>
//
// <summary>
// Converter Class legt die Max. Breite (- Parameter) für eine Control fest
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(double), typeof(double))]
    public class MaxWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double currentWidth = 0;
            if (double.TryParse(parameter.ToString(), out currentWidth) == true)
            {
                if ((double)value == 0)
                {
                    return currentWidth;
                }
                else
                {
                    if ((double)value > currentWidth)
                    {
                        return double.Parse(value.ToString()) - currentWidth;
                    }
                }
            }

            return currentWidth;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
