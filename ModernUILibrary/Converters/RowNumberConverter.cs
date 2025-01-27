//-----------------------------------------------------------------------
// <copyright file="RowNumberConverter.cs" company="Lifeprojects.de">
//     Class: RowNumberConverter
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.02.2019</date>
//
// <summary>Converter Class for RowNumber in Listview</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    [ValueConversion(typeof(int), typeof(string))]
    public sealed class RowNumberConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
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
            throw new NotImplementedException();
        }
    }
}