//-----------------------------------------------------------------------
// <copyright file="DatePickerConverter.cs" company="Lifeprojects.de">
//     Class: DatePickerConverter
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>08.03.2019</date>
//
// <summary>
// Converter Class prüft die manuelle Eingabe eines Datums in den DatePicker
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
    public class DatePickerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = System.Convert.ToString(value);
            DateTime resultDateTime;

            if (value == null)
            {
                return null;
            }

            if (DateTime.TryParse(strValue, out resultDateTime))
            {
                if (resultDateTime.IsDateEmpty() == true)
                {
                    return null;
                }

                if (parameter == null)
                {
                    return resultDateTime.ToShortDateString();
                }
                else
                {
                    return resultDateTime.ToString(parameter.ToString());
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string strValue = value.ToString();

                if (strValue != string.Empty)
                {
                    DateTime resultDateTime;
                    return DateTime.TryParse(strValue, out resultDateTime) ? resultDateTime : new DateTime(1900,1,1);
                }
            }

            return new DateTime(1900, 1, 1);
        }
    }
}
