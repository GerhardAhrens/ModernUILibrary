//-----------------------------------------------------------------------
// <copyright file="NullToVisibilityConverter.cs" company="Lifeprojects.de">
//     Class: NullToVisibilityConverter
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>20.12.2017</date>
//
// <summary>
// WPF Converter zum ein- bzw. ausblenden , wenn ein Object Null ist
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts an object into a <see cref="Visibility"/>
    /// flag, optionally inverted by setting the parameter
    /// to an arbitrary value. Without inversion, a null parameter
    /// results in <see cref="Visibility.Collapsed"/> while and
    /// set value returns <see cref="Visibility.Visible"/>.
    /// </summary>
    [ValueConversion(typeof (object), typeof (Visibility))]
  public class NullToVisibilityConverter : ConverterExtension
  {
    /// <summary>
    /// Collapses an item if the submitted value is not true.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (parameter != null)
      {
        return value == null ? Visibility.Visible : Visibility.Collapsed;
      }
      else
      {
        return value == null ? Visibility.Collapsed : Visibility.Visible;
      }
    }


    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      Visibility visiblity = (Visibility) value;
      return visiblity == Visibility.Visible;
    }
  }
}