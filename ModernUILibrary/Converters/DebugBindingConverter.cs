//-----------------------------------------------------------------------
// <copyright file="DebugBindingConverter.cs" company="Lifeprojects.de">
//     Class: DebugBindingConverter
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.12.2019</date>
//
// <summary>
// DebugBinding Converter Class
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Converter to debug the binding values
    /// </summary>
    public class DebugConverter : IValueConverter
    {
        /// <summary>
        /// ask the debugger to break
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Diagnostics.Trace.WriteLine($"Value: {value}; Typ: {targetType}; Param: {parameter}; Culture: {culture}");
            Debugger.Break();
            return Binding.DoNothing;
        }

        /// <summary>
        /// ask the debugger to break
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Diagnostics.Trace.WriteLine($"Value: {value}; Typ: {targetType}; Param: {parameter}; Culture: {culture}");
            Debugger.Break();
            return Binding.DoNothing;
        }
    }

    /// <summary>
    /// Markup extension to debug databinding
    /// </summary>
    public class DebugBindingConverter : MarkupExtension
    {
        /// <summary>
        /// Creates a new instance of the Convertor for debugging
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns>Return a convertor that can be debugged to see the values for the binding</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new DebugConverter();
        }
    }
}
