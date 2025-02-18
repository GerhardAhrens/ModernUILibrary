//-----------------------------------------------------------------------
// <copyright file="HexCodeToColorConverter.cs" company="Lifeprojects.de">
//     Class: HexCodeToColorConverter
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>02.08.2022</date>
//
// <summary>
// Der Converter wandelt einen Hex-Code nach SolidColorBrush um und gibt diesen zurück
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    [ValueConversion(typeof(string), typeof(SolidColorBrush))]
    public class HexCodeToColorConverter : IValueConverter
    {
        private static BrushConverter brushConverter = new BrushConverter();
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush color = (SolidColorBrush)brushConverter.ConvertFrom(value.ToString());
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
