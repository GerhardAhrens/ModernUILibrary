//-----------------------------------------------------------------------
// <copyright file="DateQuarterFormatTo.cs" company="Lifeprojects.de">
//     Class: DateQuarterFormatTo
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>1.1.2016</date>
//
// <summary>
// Class of FileSizeFormatTo Implemation
// </summary>
// <example>
// string sizeAsText = string.Format(new DateQuarterFormatTo(), "Dateigröße: {0:q}", DateTime.Now);
// </example>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    [DebuggerStepThrough]
    [Serializable]
    public class DateQuarterFormatTo : IFormatProvider, ICustomFormatter
    {
        private const string QuarterFormat = "q";
        private const string QuarterFormatYear = "qy";
        private const string QuarterFormatRoman = "r";
        private const string QuarterFormatRomanYear = "ry";

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }

            return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is string)
            {
                return DefaultFormat(format, arg, formatProvider);
            }


            string quaterText = string.Empty;
            DateTime dateTime;

            try
            {
                dateTime = Convert.ToDateTime(arg);
            }
            catch (InvalidCastException)
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            int quater = (dateTime.Month + 2) / 3;
            if (quater > 0)
            {
                if (format == "q" || format == "Q")
                {
                    quaterText = format.IsUpper() == true ? $"{quater}. Quartal" : $"{quater}. Q";
                }
                else if (format == "qy" || format == "QY")
                {
                    quaterText = format.IsUpper() == true ? $"{quater}. Quartal {dateTime.Year}" : $"{quater}. Q {dateTime.Year}";
                }
                else if (format == "r" || format == "R")
                {
                    string romanText = ToRoman(quater);
                    quaterText = format.IsUpper() == true ? $"{romanText}. Quartal" : $"{romanText}. Q";
                }
                else if (format == "ry" || format == "RY")
                {
                    string romanText = ToRoman(quater);
                    quaterText = format.IsUpper() == true ? $"{romanText}. Quartal {dateTime.Year}" : $"{romanText}. Q {dateTime.Year}";
                }
                else
                {
                    quaterText = $"{format} is unknown Format";
                }
            }

            return quaterText;
        }

        private static string DefaultFormat(string format, object arg, IFormatProvider formatProvider)
        {
            IFormattable formattableArg = arg as IFormattable;
            if (formattableArg != null)
            {
                return formattableArg.ToString(format, formatProvider);
            }

            return arg.ToString();
        }

        private string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999))
            {
                throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            }

            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);

            return string.Empty;
        }
    }
}