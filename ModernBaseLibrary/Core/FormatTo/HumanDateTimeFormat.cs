//-----------------------------------------------------------------------
// <copyright file="HumanDateTimeFormat.cs" company="Lifeprojects.de">
//     Class: HumanDateTimeFormat
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.02.2023 14:30:21</date>
//
// <summary>
// Die Format Klasse gibt eine Zeit als Sprachbezogenen Text zurück
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class HumanDateTimeFormat : ICustomFormatter, IFormatProvider
    {
        private static Dictionary<string, string> timeformats = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="HumanDateTimeFormat"/> class.
        /// </summary>
        public HumanDateTimeFormat()
        {
            if (timeformats == null)
            {
                timeformats = new Dictionary<string, string>();

                if (CultureInfo.CurrentCulture.Name == "de-DE")
                {
                    timeformats.Add("S", "{0:P:Sekunden:Sekunde}");
                    timeformats.Add("M", "{0:P:Minuten:Minute}");
                    timeformats.Add("H", "{0:P:Stunden:Stunde}");
                    timeformats.Add("D", "{0:P:Tage:Tag}");
                }
                else
                {
                    timeformats.Add("S", "{0:P:Seconds:Second}");
                    timeformats.Add("M", "{0:P:Minutes:Minute}");
                    timeformats.Add("H", "{0:P:Hours:Hour}");
                    timeformats.Add("D", "{0:P:Days:Day}");
                }
            }
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return String.Format(new PluralFormatTo(), timeformats[format], arg);
        }

        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }
    }
}
