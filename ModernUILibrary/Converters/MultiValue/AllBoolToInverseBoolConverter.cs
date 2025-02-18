//-----------------------------------------------------------------------
// <copyright file="AllBoolToInverseBoolConverter.cs" company="Lifeprojects.de">
//     Class: AllBoolToInverseBoolConverter
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>09.03.2021</date>
//
// <summary>
// Converter Class gibt aus einer Liste von bool true zurück, wenn mind. ein Elemente false sind.
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <inheritdoc />
    /// <summary>
    /// <para>Expects a sequence of <see cref="bool" />.</para>
    /// <para>Returns <see langword="true"/> if any value is <see langword="false"/>.</para>
    /// <para>Returns <see langword="false"/> otherwise.</para>
    /// </summary>
    [ValueConversion(typeof(bool[]), typeof(bool))]
    public class AllBoolToInverseBoolConverter : MarkupExtension, IMultiValueConverter
    {
        private static IMultiValueConverter _instance;

        public static IMultiValueConverter Instance => _instance ?? (_instance = new AllBoolToInverseBoolConverter());

        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                return DependencyProperty.UnsetValue;
            }

            return !values.All(v => v is bool b && b);
        }

        public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }
    }
}