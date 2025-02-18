//-----------------------------------------------------------------------
// <copyright file="BoolReverseConverter.cs" company="Lifeprojects.de">
//     Class: BoolReverseConverter
//     Copyright © Lifeprojects.de GmbH 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.11.2022</date>
//
// <summary>
// WPF Converter um einen bool-Wert reverse zurückzugeben.
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
    public sealed class BoolReverseConverter : ConverterBase<bool,bool>
    {
        public bool? InDesigner { get; set; }

        protected override bool Convert(bool value, CultureInfo culture)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()) && InDesigner != null)
            {
                return InDesigner.Value;
            }

            return !(bool)value;
        }

        protected override bool ConvertBack(bool value, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
