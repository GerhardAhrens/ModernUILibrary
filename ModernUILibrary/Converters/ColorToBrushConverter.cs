//-----------------------------------------------------------------------
// <copyright file="ColorToBrushConverter.cs" company="Lifeprojects.de">
//     Class: ColorToBrushConverter
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.06.2017</date>
//
// <summary>
// WPF Converter um eine Farbe von Typ Color in einen SolidColorBrush umzuwandeln
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System.Globalization;
    using System.Windows.Media;

    public class ColorToBrushConverter : ConverterBase<Color, Brush>
    {
        protected override Brush Convert(Color value, CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }
    }
}