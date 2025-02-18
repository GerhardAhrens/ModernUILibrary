//-----------------------------------------------------------------------
// <copyright file="RegexReplaceConverter.cs" company="Lifeprojects.de">
//     Class: RegexReplaceConverter
//     Copyright © Lifeprojects.de GmbH 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>27.10.2022</date>
//
// <summary>
// This Converter Replace the text fund with the Pattern Regex by Replacement
// Only usable from source to target. (OneWay)
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.Versioning;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    [SupportedOSPlatform("windows")]
    public class RegexReplaceConverter : ConverterSimpleBase, IValueConverter
    {
        public RegexReplaceConverter()
        { }

        public RegexReplaceConverter(string pattern)
        {
            Pattern = pattern;
        }

        public RegexReplaceConverter(string pattern, string replacement)
        {
            Pattern = pattern;
            Replacement = replacement;
        }

        public RegexReplaceConverter(string pattern, string replacement, RegexOptions options)
        {
            Pattern = pattern;
            Replacement = replacement;
            Options = options;
        }

        public string InDesigner { get; set; }

        /// <summary>
        /// The Regex pattern to find
        /// </summary>
        [ConstructorArgument("pattern")]
        public string Pattern { get; set; } = string.Empty;

        /// <summary>
        /// The text to put in place of the text found
        /// By default : an empty string
        /// </summary>
        [ConstructorArgument("replacement")]
        public string Replacement { get; set; } = "";

        /// <summary>
        /// Options of the regex
        /// By Default : RegexOptions.None
        /// </summary>
        [ConstructorArgument("options")]
        public RegexOptions Options { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()) && InDesigner != null)
            {
                return InDesigner;
            }
            else
            {
                return Regex.Replace(value.ToString(), Pattern.EscapeForXaml(), Replacement.EscapeForXaml(), Options);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
