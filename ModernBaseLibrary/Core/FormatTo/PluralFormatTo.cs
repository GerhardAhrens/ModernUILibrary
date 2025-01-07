//-----------------------------------------------------------------------
// <copyright file="PluralFormatTo.cs" company="Lifeprojects.de">
//     Class: PluralFormatTo
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.02.2023 14:30:21</date>
//
// <summary>
// Die Format Klasse gibt einen Text als Prural oder Singular zurück
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PluralFormatTo : ICustomFormatter, IFormatProvider
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg != null)
            {
                var parts = format.Split(':'); // ["P", "Plural", "Singular"]

                if (parts[0] == "P")
                {
                    int partIndex = (arg.ToString() == "1") ? 2 : 1;
                    return $"{arg} {(parts.Length > partIndex ? parts[partIndex] : "")}";
                }
            }

            return string.Format(format, arg);
        }

        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }
    }
}
