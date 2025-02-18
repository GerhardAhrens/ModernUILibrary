//-----------------------------------------------------------------------
// <copyright file="DoubleAbsoluteValueConverter.cs" company="Lifeprojects.de">
//     Class: DoubleAbsoluteValueConverter
//     Copyright © Lifeprojects.de GmbH 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.11.2022</date>
//
// <summary>
// WPF Converter um einen Double-Wert als Absolute Zahl zurückzugeben.
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;

    /// <summary>
    /// This Converter return the absolute of the binding value (must be a double).
    /// Equivalent to [Math.Abs(binding)]
    /// </summary>
    public class DoubleAbsoluteValueConverter : ConverterBase<double, object>
    {
        /// <summary>
        /// The default value to return if something goes wrong during the calculation.
        /// By default 0d;
        /// </summary>
        public double DefaultValue { get; set; }

        protected override object Convert(double value, CultureInfo culture)
        {
            try
            {
                return Math.Abs(System.Convert.ToDouble(value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        protected override double ConvertBack(object value, CultureInfo culture) => throw new NotImplementedException();
    }
}
