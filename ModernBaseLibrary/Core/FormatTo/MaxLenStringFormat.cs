//-----------------------------------------------------------------------
// <copyright file="MaxLenStringFormat.cs" company="Lifeprojects.de">
//     Class: MaxLenStringFormat
//     Copyright ® Gerhard Ahrens, 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>24.08.2020</date>
//
// <summary>
// Class of MaxLenStringFormat Implemation
// </summary>

// <example>
// string text = string.Format(new MaxLenStringFormat(), "{0:max(7)}", "Das ist ein langer Text","Das ist");
// </example>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Diagnostics;
    using System.Text;

    [DebuggerStepThrough]
    [Serializable]
    public class MaxLenStringFormat : IFormatProvider, ICustomFormatter
    {
        private readonly IFormatProvider defaultFormatProvider;

        /// <summary>
        /// Erweiterter Konstruktur mit einem Standard Format Provider.
        /// </summary>
        /// <param name="defaultFormatProvider">
        /// Der Standard Format Provider wird verwendet, wenn der die Implementierung nicht zutrifft.
        /// </param>
        public MaxLenStringFormat(IFormatProvider defaultFormatProvider)
        {
            this.defaultFormatProvider = defaultFormatProvider;
        }

        /// <summary>
        /// Standard Konstruktur.
        /// </summary>
        public MaxLenStringFormat()
        {
        }

        /// <summary>
        /// Konvertiert den Wert eines angegebenen Objekts unter Verwendung des angegebenen Formats sowie der kulturspezifischen Formatierungsinformationen in die entsprechende Zeichenfolgendarstellung.
        /// </summary>
        /// <param name="format">Eine Formatzeichenfolge mit Formatierungsangaben.</param>
        /// <param name="arg">Ein zu formatierendes Objekt.</param>
        /// <param name="formatProvider">Ein Objekt, das Formatierungsinformationen zur aktuellen Instanz bereitstellt.</param>
        /// <returns>
        /// Die Zeichenfolgendarstellung des Werts von <paramref name="arg" />, die gemäß der Angabe von <paramref name="format" /> und <paramref name="formatProvider" /> formatiert wird.
        /// </returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (format != null && arg is string)
            {
                var formatPattern = format.Trim().ToLowerInvariant();

                if (formatPattern.StartsWith("max(") && formatPattern.EndsWith(")"))
                {
                    return ShortenString((string)arg, ExtractMaxLength(formatPattern));
                }

                if ((formatPattern.StartsWith("sl(") || formatPattern.StartsWith("shortleft(")) && formatPattern.EndsWith(")"))
                {
                    return ShortLeftString((string)arg, ExtractMaxLength(formatPattern));
                }

                if ((formatPattern.StartsWith("sr(") || formatPattern.StartsWith("shortright(")) && formatPattern.EndsWith(")"))
                {
                    return ShortRightString((string)arg, ExtractMaxLength(formatPattern));
                }

                if (formatPattern.StartsWith("sm(") || formatPattern.StartsWith("shortmiddle(") && formatPattern.EndsWith(")"))
                {
                    return ShortMiddleString((string)arg, ExtractMaxLength(formatPattern));
                }
            }

            var formattable = arg as IFormattable;

            return formattable != null
            ? formattable.ToString(format, this.defaultFormatProvider ?? formatProvider)
            : arg != null ? arg.ToString() : null;
        }

        /// <summary>
        /// Gibt ein Objekt, das Formatierungsdienste für den angegebenen Typ bereitstellt.
        /// </summary>
        /// <param name="formatType">Ein Objekt, das den Typ des zurückzugebenden Formatierungsobjekts angibt.</param>
        /// <returns>
        /// Eine Instanz des angegebenen Objekts <paramref name="formatType" />, wenn die <see cref="T:System.IFormatProvider" /> Implementierung Objekttyp bereitstellen kann, andernfalls null.
        /// </returns>
        public object GetFormat(Type formatType)
        {
            return typeof(ICustomFormatter) == formatType ? this : null;
        }

        /// <summary>
        /// Shortens the string.
        /// </summary>
        /// <param name="origString">The original string.</param>
        /// <param name="maxStringLength">Maximum length of the string.</param>
        /// <returns></returns>
        private static string ShortenString(string origString, int maxStringLength)
        {
            var retString = origString;
            if (retString.Length > maxStringLength)
            {
                retString = retString.Substring(0, maxStringLength);
            }

            return retString;
        }

        /// <summary>
        /// Shorts the left string.
        /// </summary>
        /// <param name="origString">The original string.</param>
        /// <param name="maxStringLength">Maximum length of the string.</param>
        /// <returns></returns>
        private static string ShortLeftString(string origString, int maxStringLength)
        {
            var retString = origString;
            if (retString.Length > maxStringLength)
            {
                retString = $"{retString.Substring(0, maxStringLength-3)}...";
            }

            return retString;
        }

        /// <summary>
        /// Shorts the right string.
        /// </summary>
        /// <param name="origString">The original string.</param>
        /// <param name="maxStringLength">Maximum length of the string.</param>
        /// <returns></returns>
        private static string ShortRightString(string origString, int maxStringLength)
        {
            var retString = origString;
            if (retString.Length > maxStringLength)
            {
                int startPos = retString.Length - maxStringLength;
                string full = retString.Substring(startPos, retString.Length- startPos);
                retString = $"...{full.Substring(3,full.Length-3)}";
            }

            return retString;
        }

        /// <summary>
        /// Shorts the middle string.
        /// </summary>
        /// <param name="origString">The original string.</param>
        /// <param name="maxStringLength">Maximum length of the string.</param>
        /// <returns></returns>
        private static string ShortMiddleString(string origString, int maxStringLength)
        {
            string retString = origString;
            if (retString.Length > maxStringLength)
            {
                string left = retString.Substring(0, System.Math.Abs(retString.Length / 2) - System.Math.Abs(maxStringLength/2));
                string right = retString.Substring(System.Math.Abs(retString.Length / 2) - System.Math.Abs(maxStringLength / 2));

                retString = retString.Substring(0, maxStringLength);
            }

            return retString;
        }

        /// <summary>
        /// Extrahier aus dem Format "max(#)" die maximale Länge.
        /// </summary>
        /// <param name="pattern">Das Format als string.</param>
        /// <returns>Die maximale Länge.</returns>
        private static int ExtractMaxLength(string pattern)
        {
            var maxIndex = pattern.LastIndexOf(")", StringComparison.Ordinal);
            var numIndex = pattern.IndexOf("(", StringComparison.Ordinal) + 1;

            StringBuilder stringBuilder = null;
            while (numIndex < maxIndex)
            {
                var num = pattern[numIndex];
                if (num <= 57 && num >= 47)
                {
                    if (stringBuilder == null)
                    {
                        stringBuilder = new StringBuilder();
                    }

                    stringBuilder.Append(num);
                }
                else
                {
                    throw new FormatException("Der Format String für max(#) ist ungültig.");
                }

                numIndex++;
            }

            if (stringBuilder == null)
            {
                throw new FormatException("Der Format String für max(#) ist ungültig.");
            }

            var maxStringLength = Convert.ToInt32(stringBuilder.ToString());
            stringBuilder.Clear();

            return maxStringLength;
        }
    }
}
