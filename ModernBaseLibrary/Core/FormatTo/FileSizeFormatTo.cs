//-----------------------------------------------------------------------
// <copyright file="FileSizeFormatTo.cs" company="Lifeprojects.de">
//     Class: FileSizeFormatTo
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
// string sizeAsText = string.Format(new FileSizeFormatTo(), "Dateigröße: {0:fs}", 123000);
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
    public class FileSizeFormatTo : IFormatProvider, ICustomFormatter
    {
        private const string FileSizeFormat = "fs";
        private const decimal OneKiloByte = 1024M;
        private const decimal OneMegaByte = OneKiloByte * 1024M;
        private const decimal OneGigaByte = OneMegaByte * 1024M;

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
            if (format.IsUpper() == true)
            {
                if (format == null || format.StartsWith(FileSizeFormat.ToUpper()) == false)
                {
                    return DefaultFormat(format, arg, formatProvider);
                }
            }
            else
            {
                if (format == null || format.Contains(FileSizeFormat.ToUpper(),StringComparison.InvariantCultureIgnoreCase) == false)
                {
                    return DefaultFormat(format, arg, formatProvider);
                }
            }

            if (arg is string)
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            decimal size;

            try
            {
                size = Convert.ToDecimal(arg);
            }
            catch (InvalidCastException)
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            string suffix = string.Empty;

            if (size >= 0)
            {
                if (size > OneGigaByte)
                {
                    size /= OneGigaByte;
                    suffix = format.IsUpper() == true ? " GB" : " gb";
                }
                else if (size > OneMegaByte)
                {
                    size /= OneMegaByte;
                    suffix = format.IsUpper() == true ? " MB" : " mb";
                }
                else if (size > OneKiloByte)
                {
                    size /= OneKiloByte;
                    suffix = format.IsUpper() == true ? " KB" : " kb";
                }
                else
                {
                    suffix = suffix = format.IsUpper() == true ? " B" : " b";
                }
            }
            else
            {
                if (size < (OneGigaByte * -1))
                {
                    size /= OneGigaByte;
                    suffix = format.IsUpper() == true ? " GB" : " gb";
                }
                else if (size < OneMegaByte * -1)
                {
                    size /= OneMegaByte;
                    suffix = format.IsUpper() == true ? " MB" : " mb";
                }
                else if (size < OneKiloByte *-1)
                {
                    size /= OneKiloByte;
                    suffix = format.IsUpper() == true ? " KB" : " kb";
                }
                else
                {
                    suffix = suffix = format.IsUpper() == true ? " B" : " b";
                }
            }

            string precision = format.Substring(2);
            if (string.IsNullOrEmpty(precision))
            {
                precision = "2";
            }

            return string.Format("{0:N" + precision + "}{1}", size, suffix);
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
    }
}