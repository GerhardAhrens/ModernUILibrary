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
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Markup;

    using ModernBaseLibrary.Extension;

    using ModernIU.Base;

    [ValueConversion(typeof(int), typeof(Viewbox))]
    public class ViewBoxConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Viewbox result = null;

            if (value != null && value is int)
            {
                if (((int)value).IsEven() == true)
                {
                    result = ResourceReader.Instance.ReadAs<Viewbox>("ViewboxB", "Resources/Style/Icons.xaml");
                }
                else
                {
                    result = ResourceReader.Instance.ReadAs<Viewbox>("ViewboxC", "Resources/Style/Icons.xaml");
                }
            }

            return result as Viewbox;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}