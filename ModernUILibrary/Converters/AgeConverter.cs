//-----------------------------------------------------------------------
// <copyright file="AgeConverter.cs" company="Lifeprojects.de">
//     Class: AgeConverter
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>29.04.2019</date>
//
// <summary>
//      Converter Class gibt aus einem Datum das Alter zurück
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Runtime.Versioning;
    using System.Windows.Data;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    [ValueConversion(typeof(DateTime), typeof(int))]
    public class AgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int result = 0;
            string[] formats = { "dd.MM.yyyy", "dd.M.yyyy", "d.M.yyyy", "d.MM.yyyy", "dd.MM.yy", "dd.M.yy", "d.M.yy", "d.MM.yy"};

            if (value != null)
            {
                DateTime outDate;
                bool isDateOK = DateTime.TryParseExact(value.ToString().Split(' ')[0], formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out outDate);
                if (isDateOK == true)
                {
                    result = outDate.GetAge();
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
