//-----------------------------------------------------------------------
// <copyright file="MyClass.cs" company="Lifeprojects.de">
//     Class: MyClass
//     Copyright © Lifeprojects.de yyyy
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Der WPF Converter stellt Zahlenbereiche mit Farben dar
// </summary>
//-----------------------------------------------------------------------


namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Runtime.Versioning;
    using System.Windows.Data;
    using System.Windows.Media;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    [ValueConversion(typeof(decimal), typeof(Brushes))]
    public class CurrencyToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush result = Brushes.Transparent;
            if (value != null && value is string)
            {
                if (value.ToDecimal() >= 0 && value.ToDecimal() < 100)
                {
                    result = Brushes.Transparent;
                }
                else if (value.ToDecimal() > 100 && value.ToDecimal() < 1_000)
                {
                    result = Brushes.LightGreen;
                }
                else if (value.ToDecimal() > 1_000 && value.ToDecimal() < 1_000_000)
                {
                    result = Brushes.YellowGreen;
                }
                else if (value.ToDecimal() > 1_000_000 && value.ToDecimal() < 1_000_000_000)
                {
                    result = Brushes.Green;
                }
                else if (value.ToDecimal() > 1_000_000_000 && value.ToDecimal() < 1_000_000_000_000)
                {
                    result = Brushes.Tomato;
                }
                else if (value.ToDecimal() > 1_000_000_000_000_000)
                {
                    result = Brushes.Red;
                }
                else
                {
                    result = Brushes.Red;
                }
            }
            else
            {
                result = Brushes.Transparent;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}