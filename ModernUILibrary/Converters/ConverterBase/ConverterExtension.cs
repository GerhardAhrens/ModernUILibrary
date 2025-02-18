//-----------------------------------------------------------------------
// <copyright file="ConverterExtension.cs" company="Lifeprojects.de">
//     Class: ConverterExtension
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.06.2017</date>
//
// <summary>
// Basisklasse zur Erstellung von WPF Convertern
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// A base converter class which derives from <see cref="MarkupExtension"/>
    /// in order to simplify declaration in XAML.
    /// </summary>
    public abstract class ConverterExtension : MarkupExtension, IValueConverter
  {
    /// <summary>
    /// Gets a converter instance.
    /// </summary>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return this;
    }


    /// <summary>
    /// Converts a value. 
    /// </summary>
    /// <returns>
    /// A converted value. If the method returns null, the valid null value is used.
    /// </returns>
    /// <param name="value">The value produced by the binding source.
    /// </param>
    /// <param name="targetType">The type of the binding target property.
    /// </param>
    /// <param name="parameter">The converter parameter to use.
    /// </param>
    /// <param name="culture">The culture to use in the converter.
    /// </param>
    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);


    /// <summary>
    /// Converts a value. The default implementation throws a <see cref="NotSupportedException"/>,
    /// indicating that only one-way conversions are supported.
    /// </summary>
    /// <returns>
    /// A converted value. If the method returns null, the valid null value is used.
    /// </returns>
    /// <param name="value">The value that is produced by the binding target.
    /// </param>
    /// <param name="targetType">The type to convert to.
    /// </param>
    /// <param name="parameter">The converter parameter to use.
    /// </param>
    /// <param name="culture">The culture to use in the converter.
    /// </param>
    public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException("This converter supports only one-way conversion.");
    }
  }
}