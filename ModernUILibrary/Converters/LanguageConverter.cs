//-----------------------------------------------------------------------
// <copyright file="LanguageConverter.cs" company="Lifeprojects.de">
//     Class: LanguageConverter
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>23.02.2023</date>
//
// <summary>
// Konverter Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(string), typeof(string))]
    public class LanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            if (culture.Name.ToLower() == "de-de")
            {
                if (parameter is string param)
                {
                    if (param.Contains("|") == true)
                    {
                        result = param.Split("|")[0];
                    }
                    else
                    {
                        result = param;
                    }
                }
            }
            else
            {
                if (parameter is string param)
                {
                    if (param.Contains("|") == true)
                    {
                        result = param.Split("|")[1];
                    }
                    else
                    {
                        result = param;
                    }
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
