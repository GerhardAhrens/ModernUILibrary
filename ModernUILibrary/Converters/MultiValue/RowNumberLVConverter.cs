//-----------------------------------------------------------------------
// <copyright file="RowNumberLVConverter.cs" company="Lifeprojects.de">
//     Class: RowNumberLVConverter
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.02.2019</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Data;

    [ValueConversion(typeof(int), typeof(string))]
    public class RowNumberLVConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null)
            {
                return 0;
            }

            object item = values[0];
            ListView grid = values[1] as ListView;

            int index = -1;
            if (item != null)
            {
                index = grid.Items.IndexOf(item);
            }

            return $"{index + 1}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException($"The converter '{this.GetType().Name}' does not support ConvertBack method");
        }
    }
}