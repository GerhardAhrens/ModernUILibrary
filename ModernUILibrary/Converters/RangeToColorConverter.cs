//-----------------------------------------------------------------------
// <copyright file="RangeToColorConverter.cs" company="Lifeprojects.de">
//     Class: RangeToColorConverter
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>13.07.2022</date>
//
// <summary>
// WPF Converter Class for 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    [ValueConversion(typeof(double), typeof(Brushes))]
    public class RangeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush result = Brushes.Transparent;
            if (value != null && value is double)
            {
                if (System.Convert.ToDouble(value) == 1.00)
                {
                    result = Brushes.Blue;
                }
                else if (System.Convert.ToDouble(value) > 1.00)
                {
                    result = Brushes.Red;
                }
                else if (System.Convert.ToDouble(value) < 1.00)
                {
                    result = Brushes.Green;
                }
            }
            else
            {
                result = Brushes.Black;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}