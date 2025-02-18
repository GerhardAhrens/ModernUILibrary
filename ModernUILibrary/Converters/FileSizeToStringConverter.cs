//-----------------------------------------------------------------------
// <copyright file="FileSizeToStringConverter.cs" company="Lifeprojects.de">
//     Class: FileSizeToStringConverter
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.07.2017</date>
//
// <summary>
// WPF Converter FileSize to String Text
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;

    public sealed class FileSizeToStringConverter : ConverterBase<long, string>
    {
        protected override string Convert(long value, CultureInfo culture)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

            if (value == 0)
            {
                return $"0{suf[0]}";
            }

            long bytes = Math.Abs(value);
            int place = System.Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);

            return $"{(Math.Sign(value) * num).ToString()}{suf[place]}";
        }
    }
}