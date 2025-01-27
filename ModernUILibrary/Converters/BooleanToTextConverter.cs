//-----------------------------------------------------------------------
// <copyright file="BooleanToTextConverter.cs" company="Lifeprojects.de">
//     Class: BooleanToTextConverter
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

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    [ValueConversion(typeof(bool), typeof(string))]
    public class BooleanToTextConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool)
            {
                if ((bool)value == true)
                {
                    if (culture == null)
                    {
                        return "Ja";
                    }
                    else
                    {
                        return culture.Name == "de-DE" ? "Ja" : "Yes";
                    }
                }
                else
                {
                    if (culture == null)
                    {
                        return "Nein";
                    }
                    else
                    {
                        return culture.Name == "de-DE" ? "Nein" : "No";
                    }
                }
            }

            return "Nein";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}