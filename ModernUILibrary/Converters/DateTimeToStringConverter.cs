//-----------------------------------------------------------------------
// <copyright file="DateTimeToStringConverter.cs" company="Lifeprojects.de">
//     Class: DateTimeToStringConverter
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>16.08.2017</date>
//
// <summary>
// WPF Converter von DateTime nach String
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    public class DateTimeToStringConverter : MarkupExtension, IValueConverter
    {
        public DateTimeToStringConverter()
        {
            this.Format = "d";
        }

        public string Format { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                DateTime dateTime = DateTime.Now;
                if (value is DateTime)
                {
                    dateTime = (DateTime)value;
                    if (dateTime == new DateTime(1900, 1, 1))
                    {
                        return null;
                    }
                    else if (dateTime == new DateTime(1, 1, 1))
                    {
                        return null;
                    }
                }
                else if (value is DateTime?)
                {
                    var date = value as DateTime?;
                    if (date != null && date.HasValue)
                    {
                        dateTime = date.Value;
                    }
                }
                else
                {
                    return null;
                }


                if (dateTime.ToString().Contains("00:00:00") == true)
                {
                    parameter = "DateOnly";
                }

                if (this.Format != null)
                {
                    if (this.Format == "TimeOnly")
                    {
                        return dateTime.ToString("T", culture);
                    }
                    else if (this.Format == "DateOnly")
                    {
                        return dateTime.ToString("d", culture);
                    }
                    else if (this.Format == "d")
                    {
                        return dateTime.ToString(this.Format);
                    }
                    else if (this.Format == "g")
                    {
                        return dateTime.ToString(this.Format);
                    }
                    else
                    {
                        return dateTime.ToString(this.Format, culture);
                    }
                }

                return dateTime.ToString(culture);
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value is DateTime)
                {
                    return value;
                }

                if (value is DateTime?)
                {
                    return value;
                }

                DateTime dateValue;
                if (DateTime.TryParse(value.ToString(), CultureInfo.CurrentCulture, DateTimeStyles.None, out dateValue))
                {
                    if (dateValue >= new DateTime(1900, 1, 1))
                    {
                        return dateValue;
                    }
                    else if (dateValue >= new DateTime(1, 1, 1))
                    {
                        return dateValue;
                    }

                    return null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
