//-----------------------------------------------------------------------
// <copyright file="DomainUserConverter.cs" company="Lifeprojects.de">
//     Class: DomainUserConverter
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>20.02.2019</date>
//
// <summary>
// Converter Class gibt aus einem Bool Wert einen Text zurück
// </summary>
//-----------------------------------------------------------------------

namespace ModernUIDemo.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Windows.Media;

    using ModernBaseLibrary.Extension;

    using ModernIU.Base;

    [ValueConversion(typeof(int), typeof(DrawingImage))]
    public class DrawingImageConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DrawingImage result = null;

            if (value != null && value is int)
            {
                if (((int)value).IsEven() == true)
                {
                    result = ResourceReader.Instance.ReadAs<DrawingImage>("big-data-compute-filled-small", "Resources/Style/Icons.xaml");
                }
                else if (((int)value).IsOdd() == true)
                {
                    result = ResourceReader.Instance.ReadAs<DrawingImage>("big-data-people-filled-small", "Resources/Style/Icons.xaml");
                }
            }

            return result as DrawingImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}