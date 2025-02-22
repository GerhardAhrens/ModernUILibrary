﻿//-----------------------------------------------------------------------
// <copyright file="MouseOverToScrollBarVisibility.cs" company="Lifeprojects.de">
//     Class: MouseOverToScrollBarVisibility
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>25.07.2017</date>
//
// <summary>WPF Converter for ScrollBarVisibility</summary>
//  <auto-generated />
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Data;

    [ValueConversion(typeof(bool), typeof(ScrollBarVisibility))]
    public sealed class MouseOverToScrollBarVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool)value) ? ScrollBarVisibility.Auto : ScrollBarVisibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException($"The converter '{this.GetType().Name}' does not support ConvertBack method");
        }
    }
}
