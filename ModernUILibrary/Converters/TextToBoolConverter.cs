//-----------------------------------------------------------------------
// <copyright file="TextToBoolConverter.cs" company="Lifeprojects.de">
//     Class: TextToBoolConverter
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.11.2022</date>
//
// <summary>
// WPF Converter um aus einem Text true oder false zu interpretieren
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// Converter that just revert the given bool value in two way
    /// </summary>
    public sealed class TextToBoolConverter : ConverterBase<string,bool>
    {
        public bool? InDesigner { get; set; }

        protected override bool Convert(string value, CultureInfo culture)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()) && InDesigner != null)
            {
                return InDesigner.Value;
            }

            if (value is string s)
            {
                if (value.ToString().ToLower() == "ja")
                {
                    return true;
                }
                else if (value.ToString().ToLower() == "yes")
                {
                    return true;
                }
                else if (value.ToString().ToLower() == "1")
                {
                    return true;
                }
                else if (value.ToString().ToLower() == "true")
                {
                    return true;
                }
            }

            return false;
        }

        protected override string ConvertBack(bool value, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
